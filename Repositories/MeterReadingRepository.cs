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

    }
}