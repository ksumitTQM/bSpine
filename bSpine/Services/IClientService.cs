using bSpine.Models;

namespace bSpine.Services;

public interface IClientService
{
    Task<List<Client>> GetAllAsync(string? search = null);
    Task<Client?> GetByIdAsync(int id);
    Task<Client> CreateAsync(Client client);
    Task<bool> UpdateAsync(Client client);

    /// <summary>Returns null on success, otherwise an error message.</summary>
    Task<string?> DeleteAsync(int id);
}
