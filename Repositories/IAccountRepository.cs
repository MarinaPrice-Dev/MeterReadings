using MeterReadings.Models;
using MeterReadings.Models.Data;

namespace MeterReadings.Repositories;

public interface IAccountRepository
{
    Task SeedAccountsAsync(List<Account> accounts);
}
