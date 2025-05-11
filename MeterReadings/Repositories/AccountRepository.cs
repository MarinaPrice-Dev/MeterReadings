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
    
    public async Task<List<Account>> GetValidAccountsAsync(List<int> accountIds)
    {
        return await _context.Accounts
            .Where(a => accountIds.Contains(a.AccountId))
            .ToListAsync();
    }
}
