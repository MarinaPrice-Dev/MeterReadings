using MeterReadings.Models;
using MeterReadings.Models.Data;

namespace MeterReadings.Services;

public interface ICsvParserService
{
    Task<List<Account>> ParseAccountsCsvAsync(string filePath);
}
