using MeterReadings.Models.Entities;

namespace MeterReadings.Repositories
{
    public interface IMeterReadingRepository
    {
        Task<bool> ExistsAsync(int accountId, DateTime meterReadingDateTime);
        Task AddRangeAsync(IEnumerable<MeterReading> readings);
        Task AddAsync(MeterReading reading);
        Task SaveChangesAsync();
        Task<DateTime?> GetMostRecentReadingDateAsync(int accountId);
        Task<List<MeterReading>> GetLatestReadingsForAccountsAsync(List<int> accountIds);
    }
}