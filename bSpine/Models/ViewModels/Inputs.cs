using System.ComponentModel.DataAnnotations;

namespace bSpine.Models.ViewModels;

public class TreatmentInput : IValidatableObject
{
    [Required(ErrorMessage = "Select a provider.")]
    [Display(Name = "Provider")]
    public int? ProviderId { get; set; }

    [Required, DataType(DataType.Date)]
    [Display(Name = "Visit date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime VisitDate { get; set; } = DateTime.Today;

    [Required, StringLength(150)]
    public string Procedure { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "10000000")]
    [Display(Name = "Billed amount")]
    public decimal BilledAmount { get; set; }

    [Range(typeof(decimal), "0", "10000000")]
    [Display(Name = "Paid amount")]
    public decimal PaidAmount { get; set; }

    [Display(Name = "Lien")]
    public bool HasLien { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PaidAmount > BilledAmount)
        {
            yield return new ValidationResult(
                "Paid amount cannot exceed the billed amount.",
                new[] { nameof(PaidAmount) });
        }
    }
}

public class RecordsRequestInput : IValidatableObject
{
    [Required(ErrorMessage = "Select a provider.")]
    [Display(Name = "Provider")]
    public int? ProviderId { get; set; }

    [Required, DataType(DataType.Date)]
    [Display(Name = "Requested date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime RequestedDate { get; set; } = DateTime.Today;

    [Required]
    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    [DataType(DataType.Date)]
    [Display(Name = "Follow-up date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? FollowUpDate { get; set; } = DateTime.Today.AddDays(14);

    [StringLength(500)]
    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FollowUpDate.HasValue && FollowUpDate.Value.Date < RequestedDate.Date)
        {
            yield return new ValidationResult(
                "Follow-up date cannot be before the requested date.",
                new[] { nameof(FollowUpDate) });
        }
    }
}

public class NoteInput
{
    [Required, StringLength(80)]
    public string Author { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    [Display(Name = "Note")]
    public string Text { get; set; } = string.Empty;
}
