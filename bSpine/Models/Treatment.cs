using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bSpine.Models;

public class Treatment
{
    public int Id { get; set; }

    public int LegalCaseId { get; set; }
    public LegalCase? LegalCase { get; set; }

    public int ProviderId { get; set; }
    public Provider? Provider { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Visit date")]
    public DateTime VisitDate { get; set; }

    [Required, StringLength(150)]
    public string Procedure { get; set; } = string.Empty;

    [DataType(DataType.Currency)]
    [Display(Name = "Billed")]
    public decimal BilledAmount { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Paid")]
    public decimal PaidAmount { get; set; }

    [Display(Name = "Lien")]
    public bool HasLien { get; set; }

    [NotMapped]
    public decimal Outstanding => BilledAmount - PaidAmount;
}
