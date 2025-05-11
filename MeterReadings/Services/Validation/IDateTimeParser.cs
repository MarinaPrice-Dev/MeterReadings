namespace MeterReadings.Services.Validation;

public interface IDateTimeParser
{
    bool TryParseBritishDateTime(string dateTime, out DateTime parsedDate);
}