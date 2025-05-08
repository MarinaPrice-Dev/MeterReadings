using MeterReadings.Models;
using MeterReadings.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.AccountId)
            .IsUnique();
    }

}

