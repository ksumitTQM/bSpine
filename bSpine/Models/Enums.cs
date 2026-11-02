using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace bSpine.Models;

public enum CaseStatus
{
    Intake,
    Treatment,
    Demand,
    Litigation,
    Settled,
    Closed
}

public enum ProviderType
{
    [Display(Name = "Spine Surgeon")]
    SpineSurgeon,

    Chiropractor,

    [Display(Name = "Imaging Center")]
    ImagingCenter,

    [Display(Name = "Pain Management")]
    PainManagement,

    [Display(Name = "Physical Therapy")]
    PhysicalTherapy,

    [Display(Name = "Primary Care")]
    PrimaryCare
}

public enum RequestStatus
{
    Pending,
    Received,
    Overdue
}

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var name = value.ToString();
        var member = value.GetType().GetField(name);
        var attribute = member?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.GetName() ?? name;
    }

    public static string BadgeClass(this CaseStatus status) => status switch
    {
        CaseStatus.Intake => "text-bg-info",
        CaseStatus.Treatment => "text-bg-primary",
        CaseStatus.Demand => "text-bg-warning",
        CaseStatus.Litigation => "text-bg-danger",
        CaseStatus.Settled => "text-bg-success",
        _ => "text-bg-secondary"
    };

    public static string BadgeClass(this RequestStatus status) => status switch
    {
        RequestStatus.Received => "text-bg-success",
        RequestStatus.Overdue => "text-bg-danger",
        _ => "text-bg-warning"
    };
}
