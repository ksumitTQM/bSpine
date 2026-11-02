using bSpine.Data;
using bSpine.Models;
using Microsoft.EntityFrameworkCore;

namespace bSpine.Services;

public class ProviderService : IProviderService
{
    private readonly BSpineDbContext _db;

    public ProviderService(BSpineDbContext db)
    {
        _db = db;
    }

    public async Task<List<Provider>> GetAllAsync(ProviderType? type = null, string? search = null)
    {
        IQueryable<Provider> query = _db.Providers.AsNoTracking();

        if (type.HasValue)
        {
            query = query.Where(p => p.Type == type.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term) ||
                                     (p.City != null && p.City.ToLower().Contains(term)));
        }

        return await query.OrderBy(p => p.Name).ToListAsync();
    }

    public Task<Provider?> GetByIdAsync(int id)
    {
        return _db.Providers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Provider> CreateAsync(Provider provider)
    {
        _db.Providers.Add(provider);
        await _db.SaveChangesAsync();
        return provider;
    }

    public async Task<bool> UpdateAsync(Provider provider)
    {
        if (!await _db.Providers.AnyAsync(p => p.Id == provider.Id))
        {
            return false;
        }

        _db.Providers.Update(provider);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<string?> DeleteAsync(int id)
    {
        var provider = await _db.Providers.FindAsync(id);
        if (provider == null)
        {
            return "Provider not found.";
        }

        var inUse = await _db.Treatments.AnyAsync(t => t.ProviderId == id) ||
                    await _db.RecordsRequests.AnyAsync(r => r.ProviderId == id);
        if (inUse)
        {
            return "This provider is referenced by treatments or records requests and cannot be deleted.";
        }

        _db.Providers.Remove(provider);
        await _db.SaveChangesAsync();
        return null;
    }
}
