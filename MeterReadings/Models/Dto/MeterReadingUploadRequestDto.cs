using Microsoft.AspNetCore.Http;

namespace MeterReadings.Models.Dto
{
    public class MeterReadingUploadRequestDto
    {
        public IFormFile File { get; set; }
    }
}