using bSpine.Models.ViewModels;

namespace bSpine.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardAsync();
}
