using bSpine.Data;
using bSpine.Models;
using Microsoft.EntityFrameworkCore;

namespace bSpine.Services;

public class ClientService : IClientService
{
    private readonly BSpineDbContext _db;

    public ClientService(BSpineDbContext db)
    {
        _db = db;
    }

    public async Task<List<Client>> GetAllAsync(string? search = null)
    {
        IQueryable<Client> query = _db.Clients.AsNoTracking().Include(c => c.Cases);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(c =>
                c.FirstName.ToLower().Contains(term) ||
                c.LastName.ToLower().Contains(term) ||
                c.InjuryType.ToLower().Contains(term));
        }

        return await query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
    }

    public Task<Client?> GetByIdAsync(int id)
    {
        return _db.Clients.AsNoTracking()
            .Include(c => c.Cases)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Client> CreateAsync(Client client)
    {
        _db.Clients.Add(client);
        await _db.SaveChangesAsync();
        return client;
    }

    public async Task<bool> UpdateAsync(Client client)
    {
        if (!await _db.Clients.AnyAsync(c => c.Id == client.Id))
        {
            return false;
        }

        _db.Clients.Update(client);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<string?> DeleteAsync(int id)
    {
        var client = await _db.Clients.FindAsync(id);
        if (client == null)
        {
            return "Client not found.";
        }

        if (await _db.Cases.AnyAsync(c => c.ClientId == id))
        {
            return "This client has cases on file. Delete or reassign the cases first.";
        }

        _db.Clients.Remove(client);
        await _db.SaveChangesAsync();
        return null;
    }
}
