using MeterReadings.Data;
using MeterReadings.Models;
using MeterReadings.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAccountsAsync(List<Account> accounts)
    {
        if (!_context.Accounts.Any())
        {
            await _context.Accounts.AddRangeAsync(accounts);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> ExistsAsync(int accountId)
    {
        return await _context.Accounts
            .AnyAsync(a => a.AccountId == accountId);
    }
}
