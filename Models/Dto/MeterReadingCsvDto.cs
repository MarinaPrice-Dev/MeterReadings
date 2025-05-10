namespace MeterReadings.Models.Dto;

public class MeterReadingCsvDto
{
    public int AccountId { get; set; } 
    public string MeterReadingDateTime { get; set; } 
    public string MeterReadValue { get; set; }
}