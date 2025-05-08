using MeterReadings.Data;
using MeterReadings.Models;
using MeterReadings.Models.Data;

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
}
