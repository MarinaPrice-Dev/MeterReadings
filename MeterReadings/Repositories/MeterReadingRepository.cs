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
        
        public async Task AddRangeAsync(IEnumerable<MeterReading> readings)
        {
            await _context.MeterReadings.AddRangeAsync(readings);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}