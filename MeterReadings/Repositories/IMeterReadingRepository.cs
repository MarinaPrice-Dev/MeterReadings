﻿using MeterReadings.Models.Entities;

namespace MeterReadings.Repositories
{
    public interface IMeterReadingRepository
    {
        Task<List<MeterReading>> GetLatestReadingsForAccountsAsync(List<int> accountIds);
        Task AddRangeAsync(IEnumerable<MeterReading> readings);
        Task SaveChangesAsync();
    }
}