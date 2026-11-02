using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bSpine.Models;

public class Client
{
    public int Id { get; set; }

    [Required, StringLength(60)]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(60)]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required, DataType(DataType.Date)]
    [Display(Name = "Date of birth")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

    [Phone, StringLength(30)]
    public string? Phone { get; set; }

    [EmailAddress, StringLength(120)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [Required, DataType(DataType.Date)]
    [Display(Name = "Injury date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime InjuryDate { get; set; }

    [Required, StringLength(120)]
    [Display(Name = "Injury type")]
    public string InjuryType { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Injury summary")]
    public string? InjurySummary { get; set; }

    public ICollection<LegalCase> Cases { get; set; } = new List<LegalCase>();

    [NotMapped]
    [Display(Name = "Client")]
    public string FullName => $"{FirstName} {LastName}".Trim();

    [NotMapped]
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
