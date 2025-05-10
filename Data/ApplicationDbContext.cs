using MeterReadings.Models;
using MeterReadings.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<MeterReading> MeterReadings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Account
        modelBuilder.Entity<Account>()
            .HasIndex(a => a.AccountId)
            .IsUnique();
        
        //MeterReading
        modelBuilder.Entity<MeterReading>()
            .HasOne(m => m.Account)
            .WithMany()
            .HasForeignKey(m => m.AccountId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<MeterReading>()
            .Property(m => m.MeterReadValue)
            .IsRequired()
            .HasMaxLength(5);
    }

}

