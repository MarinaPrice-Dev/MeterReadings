namespace MeterReadings.Models.Dto;

public class MeterReadingUploadResultDto
{
    public int SuccessfulReadings { get; set; }
    public int FailedReadings { get; set; }
    public List<string> Errors { get; set; } = new();
}