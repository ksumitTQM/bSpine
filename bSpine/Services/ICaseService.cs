using bSpine.Models;
using bSpine.Models.ViewModels;

namespace bSpine.Services;

public interface ICaseService
{
    Task<List<LegalCase>> SearchAsync(string? search, CaseStatus? status);
    Task<LegalCase?> GetByIdAsync(int id);
    Task<CaseDetailViewModel?> GetDetailAsync(int id);
    Task<bool> CaseNumberExistsAsync(string caseNumber, int excludeId = 0);
    Task<LegalCase> CreateAsync(LegalCase legalCase);
    Task<bool> UpdateAsync(LegalCase legalCase);
    Task<bool> DeleteAsync(int id);

    Task<bool> AddTreatmentAsync(int caseId, TreatmentInput input);
    Task<bool> AddRequestAsync(int caseId, RecordsRequestInput input);
    Task<bool> AddNoteAsync(int caseId, NoteInput input);
    Task<bool> MarkRequestReceivedAsync(int requestId);
}
