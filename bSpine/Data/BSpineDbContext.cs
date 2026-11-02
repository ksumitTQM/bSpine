using bSpine.Models;
using Microsoft.EntityFrameworkCore;

namespace bSpine.Data;

public class BSpineDbContext : DbContext
{
    public BSpineDbContext(DbContextOptions<BSpineDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<LegalCase> Cases => Set<LegalCase>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<Treatment> Treatments => Set<Treatment>();
    public DbSet<RecordsRequest> RecordsRequests => Set<RecordsRequest>();
    public DbSet<CaseNote> CaseNotes => Set<CaseNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LegalCase>(e =>
        {
            e.HasOne(c => c.Client)
                .WithMany(c => c.Cases)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Treatment>(e =>
        {
            e.Property(t => t.BilledAmount).HasPrecision(18, 2);
            e.Property(t => t.PaidAmount).HasPrecision(18, 2);

            e.HasOne(t => t.LegalCase)
                .WithMany(c => c.Treatments)
                .HasForeignKey(t => t.LegalCaseId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(t => t.Provider)
                .WithMany(p => p.Treatments)
                .HasForeignKey(t => t.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RecordsRequest>(e =>
        {
            e.HasOne(r => r.LegalCase)
                .WithMany(c => c.RecordsRequests)
                .HasForeignKey(r => r.LegalCaseId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(r => r.Provider)
                .WithMany(p => p.RecordsRequests)
                .HasForeignKey(r => r.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CaseNote>(e =>
        {
            e.HasOne(n => n.LegalCase)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.LegalCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
