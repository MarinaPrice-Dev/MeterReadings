using MeterReadings.Models;
using MeterReadings.Models.Entities;

namespace MeterReadings.Services;

public interface ICsvParserService
{
    Task<List<Account>> ParseAccountsCsvAsync(string filePath);
}
