using bSpine.Models;

namespace bSpine.Services;

public interface IProviderService
{
    Task<List<Provider>> GetAllAsync(ProviderType? type = null, string? search = null);
    Task<Provider?> GetByIdAsync(int id);
    Task<Provider> CreateAsync(Provider provider);
    Task<bool> UpdateAsync(Provider provider);

    /// <summary>Returns null on success, otherwise an error message.</summary>
    Task<string?> DeleteAsync(int id);
}
