namespace bSpine.Models.ViewModels;

public class CaseSpecialsRow
{
    public int CaseId { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public CaseStatus Status { get; set; }
    public decimal Billed { get; set; }
    public decimal Paid { get; set; }
    public decimal Outstanding => Billed - Paid;
}

public class DashboardViewModel
{
    public int TotalCases { get; set; }
    public int OpenCases { get; set; }
    public int SolWarningCount { get; set; }
    public decimal TotalBilled { get; set; }
    public decimal TotalPaid { get; set; }

    public Dictionary<CaseStatus, int> StatusCounts { get; set; } = new();
    public List<LegalCase> UpcomingSol { get; set; } = new();
    public List<RecordsRequest> OverdueRequests { get; set; } = new();
    public List<CaseSpecialsRow> Specials { get; set; } = new();
}
