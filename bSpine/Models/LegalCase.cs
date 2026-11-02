using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bSpine.Models;

public class LegalCase
{
    public const int SolWarningDays = 90;

    public int Id { get; set; }

    [Required, StringLength(30)]
    [Display(Name = "Case number")]
    public string CaseNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Client")]
    public int ClientId { get; set; }

    public Client? Client { get; set; }

    [Required, StringLength(80)]
    [Display(Name = "Assigned attorney")]
    public string AssignedAttorney { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Paralegal { get; set; } = string.Empty;

    [Required]
    public CaseStatus Status { get; set; } = CaseStatus.Intake;

    [Required, DataType(DataType.Date)]
    [Display(Name = "Opened")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime OpenedDate { get; set; }

    [Required, DataType(DataType.Date)]
    [Display(Name = "Statute of limitations")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime StatuteOfLimitationsDate { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
    public ICollection<RecordsRequest> RecordsRequests { get; set; } = new List<RecordsRequest>();
    public ICollection<CaseNote> Notes { get; set; } = new List<CaseNote>();

    [NotMapped]
    public bool IsOpen => Status != CaseStatus.Settled && Status != CaseStatus.Closed;

    [NotMapped]
    public int DaysToSol => (StatuteOfLimitationsDate.Date - DateTime.Today).Days;

    [NotMapped]
    public bool IsSolExpired => IsOpen && DaysToSol < 0;

    [NotMapped]
    public bool IsSolWarning => IsOpen && DaysToSol >= 0 && DaysToSol <= SolWarningDays;

    [NotMapped]
    public decimal TotalBilled => Treatments.Sum(t => t.BilledAmount);

    [NotMapped]
    public decimal TotalPaid => Treatments.Sum(t => t.PaidAmount);
}
