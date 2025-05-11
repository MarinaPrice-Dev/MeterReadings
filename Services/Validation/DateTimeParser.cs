using System.Globalization;

namespace MeterReadings.Services.Validation;

public class DateTimeParser : IDateTimeParser
{
    public bool TryParseBritishDateTime(string dateTime, out DateTime parsedDate) =>
        DateTime.TryParseExact(
            dateTime,
            "dd/MM/yyyy HH:mm",
            CultureInfo.GetCultureInfo("en-GB"),
            DateTimeStyles.AssumeLocal,
            out parsedDate);
}