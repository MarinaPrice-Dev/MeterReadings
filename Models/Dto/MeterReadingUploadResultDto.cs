namespace MeterReadings.Models.Dto;

public class MeterReadingUploadResultDto
{
    public int SuccessfulReadings { get; set; }

    public List<string> Errors { get; set; } = new();

    public int FailedReadings => Errors.Count;
}