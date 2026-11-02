using bSpine.Data;
using bSpine.Models;
using bSpine.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace bSpine.Services;

public class CaseService : ICaseService
{
    private readonly BSpineDbContext _db;

    public CaseService(BSpineDbContext db)
    {
        _db = db;
    }

    public async Task<List<LegalCase>> SearchAsync(string? search, CaseStatus? status)
    {
        IQueryable<LegalCase> query = _db.Cases.AsNoTracking()
            .Include(c => c.Client)
            .Include(c => c.Treatments);

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(c =>
                c.CaseNumber.ToLower().Contains(term) ||
                c.AssignedAttorney.ToLower().Contains(term) ||
                c.Paralegal.ToLower().Contains(term) ||
                (c.Client != null &&
                 (c.Client.FirstName.ToLower().Contains(term) || c.Client.LastName.ToLower().Contains(term))));
        }

        return await query.OrderBy(c => c.StatuteOfLimitationsDate).ToListAsync();
    }

    public Task<LegalCase?> GetByIdAsync(int id)
    {
        return _db.Cases.AsNoTracking()
            .Include(c => c.Client)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CaseDetailViewModel?> GetDetailAsync(int id)
    {
        var legalCase = await GetByIdAsync(id);
        if (legalCase == null)
        {
            return null;
        }

        var treatments = await _db.Treatments.AsNoTracking()
            .Include(t => t.Provider)
            .Where(t => t.LegalCaseId == id)
            .OrderByDescending(t => t.VisitDate)
            .ToListAsync();

        var requests = await _db.RecordsRequests.AsNoTracking()
            .Include(r => r.Provider)
            .Where(r => r.LegalCaseId == id)
            .OrderByDescending(r => r.RequestedDate)
            .ToListAsync();

        var notes = await _db.CaseNotes.AsNoTracking()
            .Where(n => n.LegalCaseId == id)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var providers = await _db.Providers.AsNoTracking().OrderBy(p => p.Name).ToListAsync();
        var options = providers
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} ({p.Type.GetDisplayName()})"
            })
            .ToList();

        var liens = treatments.Where(t => t.HasLien).ToList();

        return new CaseDetailViewModel
        {
            Case = legalCase,
            Treatments = treatments,
            Requests = requests,
            Notes = notes,
            TotalBilled = treatments.Sum(t => t.BilledAmount),
            TotalPaid = treatments.Sum(t => t.PaidAmount),
            LienBilled = liens.Sum(t => t.BilledAmount),
            LienCount = liens.Count,
            ProviderOptions = options
        };
    }

    public Task<bool> CaseNumberExistsAsync(string caseNumber, int excludeId = 0)
    {
        var number = caseNumber.Trim().ToLower();
        return _db.Cases.AnyAsync(c => c.CaseNumber.ToLower() == number && c.Id != excludeId);
    }

    public async Task<LegalCase> CreateAsync(LegalCase legalCase)
    {
        legalCase.CaseNumber = legalCase.CaseNumber.Trim();
        _db.Cases.Add(legalCase);
        await _db.SaveChangesAsync();
        return legalCase;
    }

    public async Task<bool> UpdateAsync(LegalCase legalCase)
    {
        if (!await _db.Cases.AnyAsync(c => c.Id == legalCase.Id))
        {
            return false;
        }

        legalCase.CaseNumber = legalCase.CaseNumber.Trim();
        _db.Cases.Update(legalCase);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var legalCase = await _db.Cases
            .Include(c => c.Treatments)
            .Include(c => c.RecordsRequests)
            .Include(c => c.Notes)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (legalCase == null)
        {
            return false;
        }

        _db.Treatments.RemoveRange(legalCase.Treatments);
        _db.RecordsRequests.RemoveRange(legalCase.RecordsRequests);
        _db.CaseNotes.RemoveRange(legalCase.Notes);
        _db.Cases.Remove(legalCase);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddTreatmentAsync(int caseId, TreatmentInput input)
    {
        if (!await _db.Cases.AnyAsync(c => c.Id == caseId) || !input.ProviderId.HasValue)
        {
            return false;
        }

        _db.Treatments.Add(new Treatment
        {
            LegalCaseId = caseId,
            ProviderId = input.ProviderId.Value,
            VisitDate = input.VisitDate,
            Procedure = input.Procedure.Trim(),
            BilledAmount = input.BilledAmount,
            PaidAmount = input.PaidAmount,
            HasLien = input.HasLien
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddRequestAsync(int caseId, RecordsRequestInput input)
    {
        if (!await _db.Cases.AnyAsync(c => c.Id == caseId) || !input.ProviderId.HasValue)
        {
            return false;
        }

        _db.RecordsRequests.Add(new RecordsRequest
        {
            LegalCaseId = caseId,
            ProviderId = input.ProviderId.Value,
            RequestedDate = input.RequestedDate,
            Status = input.Status,
            FollowUpDate = input.FollowUpDate,
            ReceivedDate = input.Status == RequestStatus.Received ? DateTime.Today : null,
            Notes = input.Notes
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddNoteAsync(int caseId, NoteInput input)
    {
        if (!await _db.Cases.AnyAsync(c => c.Id == caseId))
        {
            return false;
        }

        _db.CaseNotes.Add(new CaseNote
        {
            LegalCaseId = caseId,
            Author = input.Author.Trim(),
            Text = input.Text.Trim(),
            CreatedAt = DateTime.Now
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkRequestReceivedAsync(int requestId)
    {
        var request = await _db.RecordsRequests.FindAsync(requestId);
        if (request == null)
        {
            return false;
        }

        request.Status = RequestStatus.Received;
        request.ReceivedDate = DateTime.Today;
        await _db.SaveChangesAsync();
        return true;
    }
}
