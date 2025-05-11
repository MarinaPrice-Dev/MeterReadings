using MeterReadings.Models;
using MeterReadings.Models.Entities;

namespace MeterReadings.Repositories;

public interface IAccountRepository
{
    Task SeedAccountsAsync(List<Account> accounts);
    Task<List<Account>> GetValidAccountsAsync(List<int> accountIds);
}
