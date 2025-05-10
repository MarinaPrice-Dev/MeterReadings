using MeterReadings.Models.Dto;
using MeterReadings.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeterReadings.Controllers;

[ApiController]
[Route("[controller]")]
public class MeterReadingUploadController : ControllerBase
{
    private readonly IMeterReadingService _meterReadingService;

    public MeterReadingUploadController(IMeterReadingService meterReadingService)
    {
        _meterReadingService = meterReadingService;
    }

    [HttpPost]
    [Route("")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCsv([FromForm] MeterReadingUploadRequestDto request)
    {
        var result = await _meterReadingService.UploadMeterReadingsAsync(request.File);
        return Ok(result);
    }
}
