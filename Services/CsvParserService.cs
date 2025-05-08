using MeterReadings.Models;
using CsvHelper;
using System.Globalization;
using MeterReadings.Models.Data;
using MeterReadings.Models.DTO;

namespace MeterReadings.Services;

public class CsvParserService : ICsvParserService
{
    public async Task<List<Account>> ParseAccountsCsvAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var csvRecords = csv.GetRecords<AccountCsvDto>().ToList();

        var records = csvRecords.Select(dto => new Account
        {
            AccountId = dto.AccountId,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        }).ToList();

        return await Task.FromResult(records);
    }
}
