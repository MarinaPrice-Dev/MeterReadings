using MeterReadings.Data;
using MeterReadings.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Repositories
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly ApplicationDbContext _context;

        public MeterReadingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int accountId, DateTime meterReadingDateTime)
        {
            return await _context.MeterReadings
                .AnyAsync(m => m.AccountId == accountId && m.MeterReadingDateTime == meterReadingDateTime);
        }

        public async Task AddRangeAsync(IEnumerable<MeterReading> readings)
        {
            await _context.MeterReadings.AddRangeAsync(readings);
        }
        
        public async Task AddAsync(MeterReading reading)
        {
            await _context.MeterReadings.AddAsync(reading);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<DateTime?> GetMostRecentReadingDateAsync(int accountId)
        {
            return await _context.MeterReadings
                .Where(m => m.AccountId == accountId)
                .MaxAsync(m => (DateTime?)m.MeterReadingDateTime);
        }
        
        public async Task<List<MeterReading>> GetLatestReadingsForAccountsAsync(List<int> accountIds)
        {
            return await _context.MeterReadings
                .Where(r => accountIds.Contains(r.AccountId))
                .GroupBy(r => r.AccountId)
                .Select(g => g
                    .OrderByDescending(r => r.MeterReadingDateTime)
                    .First())
                .ToListAsync();
        }

    }
}