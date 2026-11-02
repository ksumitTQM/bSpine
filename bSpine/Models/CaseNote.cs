using System.ComponentModel.DataAnnotations;

namespace bSpine.Models;

public class CaseNote
{
    public int Id { get; set; }

    public int LegalCaseId { get; set; }
    public LegalCase? LegalCase { get; set; }

    [Required, StringLength(80)]
    public string Author { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
