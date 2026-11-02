using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bSpine.Models;

public class RecordsRequest
{
    public int Id { get; set; }

    public int LegalCaseId { get; set; }
    public LegalCase? LegalCase { get; set; }

    public int ProviderId { get; set; }
    public Provider? Provider { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Requested")]
    public DateTime RequestedDate { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    [DataType(DataType.Date)]
    [Display(Name = "Follow-up")]
    public DateTime? FollowUpDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Received")]
    public DateTime? ReceivedDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// True when the request is explicitly overdue, or still pending past its follow-up date.
    /// </summary>
    [NotMapped]
    public bool IsOverdue =>
        Status == RequestStatus.Overdue ||
        (Status == RequestStatus.Pending && FollowUpDate.HasValue && FollowUpDate.Value.Date < DateTime.Today);

    [NotMapped]
    public RequestStatus DisplayStatus => IsOverdue ? RequestStatus.Overdue : Status;
}
