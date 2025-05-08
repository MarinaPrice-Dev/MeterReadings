using MeterReadings.Services;

namespace MeterReadings.Repositories.SeedData;

public class TestAccountSeeder : IDataSeeder
{
    private readonly ICsvParserService _csvParser;
    private readonly IAccountRepository _accountRepository;
    private readonly IWebHostEnvironment _env;

    public TestAccountSeeder(ICsvParserService csvParser, IAccountRepository accountRepo, IWebHostEnvironment env)
    {
        _csvParser = csvParser;
        _accountRepository = accountRepo;
        _env = env;
    }

    public async Task SeedAsync()
    {
        if (_env.IsDevelopment())
        {
            var path = Path.Combine("Data/SeedData", "Test_Accounts.csv");
            var accounts = await _csvParser.ParseAccountsCsvAsync(path);
            await _accountRepository.SeedAccountsAsync(accounts);
        }
    }
}
