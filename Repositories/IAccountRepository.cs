using MeterReadings.Models;
using MeterReadings.Models.Entities;

namespace MeterReadings.Repositories;

public interface IAccountRepository
{
    Task SeedAccountsAsync(List<Account> accounts);
    Task<bool> ExistsAsync(int accountId);
}
