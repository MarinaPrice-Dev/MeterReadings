using MeterReadings.Models.Dto;

namespace MeterReadings.Services.Validation
{
    public interface IMeterReadingValidator
    {
        Task<bool> ValidateReading(MeterReadingCsvDto dto, MeterReadingUploadResultDto result);
    }
}