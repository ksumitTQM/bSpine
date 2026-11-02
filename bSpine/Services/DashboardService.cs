using bSpine.Data;
using bSpine.Models;
using bSpine.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace bSpine.Services;

public class DashboardService : IDashboardService
{
    private const int UpcomingSolWindowDays = 180;

    private readonly BSpineDbContext _db;

    public DashboardService(BSpineDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var cases = await _db.Cases.AsNoTracking()
            .Include(c => c.Client)
            .Include(c => c.Treatments)
            .ToListAsync();

        var openCases = cases.Where(c => c.IsOpen).ToList();

        var statusCounts = Enum.GetValues<CaseStatus>()
            .ToDictionary(s => s, s => cases.Count(c => c.Status == s));

        var upcomingSol = openCases
            .Where(c => c.DaysToSol <= UpcomingSolWindowDays)
            .OrderBy(c => c.StatuteOfLimitationsDate)
            .Take(8)
            .ToList();

        var pendingRequests = await _db.RecordsRequests.AsNoTracking()
            .Include(r => r.Provider)
            .Include(r => r.LegalCase)
            .ThenInclude(c => c!.Client)
            .Where(r => r.Status != RequestStatus.Received)
            .ToListAsync();

        var overdue = pendingRequests
            .Where(r => r.IsOverdue)
            .OrderBy(r => r.FollowUpDate ?? r.RequestedDate)
            .ToList();

        var specials = cases
            .Select(c => new CaseSpecialsRow
            {
                CaseId = c.Id,
                CaseNumber = c.CaseNumber,
                ClientName = c.Client?.FullName ?? "Unknown",
                Status = c.Status,
                Billed = c.Treatments.Sum(t => t.BilledAmount),
                Paid = c.Treatments.Sum(t => t.PaidAmount)
            })
            .OrderByDescending(r => r.Billed)
            .ToList();

        return new DashboardViewModel
        {
            TotalCases = cases.Count,
            OpenCases = openCases.Count,
            SolWarningCount = openCases.Count(c => c.IsSolWarning || c.IsSolExpired),
            TotalBilled = specials.Sum(s => s.Billed),
            TotalPaid = specials.Sum(s => s.Paid),
            StatusCounts = statusCounts,
            UpcomingSol = upcomingSol,
            OverdueRequests = overdue,
            Specials = specials
        };
    }
}
