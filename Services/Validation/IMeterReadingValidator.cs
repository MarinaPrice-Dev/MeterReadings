using MeterReadings.Models.Dto;
using MeterReadings.Models.Entities;

namespace MeterReadings.Services.Validation
{
    public interface IMeterReadingValidator
    {
        bool ValidateReading(MeterReadingCsvDto meterReadingCsvRow, 
            MeterReadingUploadResultDto uploadResults, 
            List<Account> validAccounts, 
            List<MeterReading> latestReadingsStored);
    }
}