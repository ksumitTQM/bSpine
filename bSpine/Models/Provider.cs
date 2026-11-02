using System.ComponentModel.DataAnnotations;

namespace bSpine.Models;

public class Provider
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    [Display(Name = "Provider / practice name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Provider type")]
    public ProviderType Type { get; set; }

    [StringLength(100)]
    public string? Specialty { get; set; }

    [Phone, StringLength(30)]
    public string? Phone { get; set; }

    [EmailAddress, StringLength(120)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(80)]
    public string? City { get; set; }

    public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
    public ICollection<RecordsRequest> RecordsRequests { get; set; } = new List<RecordsRequest>();
}
