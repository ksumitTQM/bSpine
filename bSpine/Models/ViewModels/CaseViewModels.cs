using Microsoft.AspNetCore.Mvc.Rendering;

namespace bSpine.Models.ViewModels;

public class CaseListViewModel
{
    public IReadOnlyList<LegalCase> Cases { get; set; } = new List<LegalCase>();
    public string? Search { get; set; }
    public CaseStatus? Status { get; set; }
}

public class CaseDetailViewModel
{
    public LegalCase Case { get; set; } = null!;
    public IReadOnlyList<Treatment> Treatments { get; set; } = new List<Treatment>();
    public IReadOnlyList<RecordsRequest> Requests { get; set; } = new List<RecordsRequest>();
    public IReadOnlyList<CaseNote> Notes { get; set; } = new List<CaseNote>();

    // Medical specials summary
    public decimal TotalBilled { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalOutstanding => TotalBilled - TotalPaid;
    public decimal LienBilled { get; set; }
    public int LienCount { get; set; }

    // Inline "add" forms
    public TreatmentInput NewTreatment { get; set; } = new();
    public RecordsRequestInput NewRequest { get; set; } = new();
    public NoteInput NewNote { get; set; } = new();

    public IReadOnlyList<SelectListItem> ProviderOptions { get; set; } = new List<SelectListItem>();
}
