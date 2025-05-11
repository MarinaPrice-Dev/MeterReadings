using MeterReadings.Models.Dto;

namespace MeterReadings.Services
{
    public interface IMeterReadingService
    {
        Task<MeterReadingUploadResultDto> UploadMeterReadingsAsync(IFormFile file);
    }
}
