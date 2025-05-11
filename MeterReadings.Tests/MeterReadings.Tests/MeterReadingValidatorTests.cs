using MeterReadings.Models.Dto;
using MeterReadings.Models.Entities;
using MeterReadings.Services.Validation;
using Moq;

namespace MeterReadings.Tests;

[TestClass]
public class MeterReadingValidatorTests
{
    private MeterReadingValidator _validator = null!;

    [TestInitialize]
    public void Setup()
    {
        var mockDateTimeParser = new Mock<IDateTimeParser>();
        mockDateTimeParser
            .Setup(p => p.TryParseBritishDateTime(It.IsAny<string>(), out It.Ref<DateTime>.IsAny))
            .Returns((string s, out DateTime result) =>
            {
                result = new(2019, 4, 22, 12, 25, 0);;
                return true;
            });

        _validator = new MeterReadingValidator(mockDateTimeParser.Object);
    }

    [TestMethod]
    public void ValidateReading_ShouldReturnTrue_WhenReadingIsValid()
    {
        var dto = new MeterReadingCsvDto
        {
            AccountId = 123,
            MeterReadingDateTime = "22/04/2019 12:25",
            MeterReadValue = "12345"
        };
        var result = new MeterReadingUploadResultDto();
        var accounts = new List<Account> { new() { AccountId = 123 } };
        var readings = new List<MeterReading>();

        var isValid = _validator.ValidateReading(dto, result, accounts, readings);

        Assert.IsTrue(isValid);
        Assert.AreEqual(1, result.SuccessfulReadings);
        Assert.AreEqual(0, result.Errors.Count);
    }

    [TestMethod]
    public void ValidateReading_ShouldReturnFalse_WhenAccountIsInvalid()
    {
        var dto = new MeterReadingCsvDto
        {
            AccountId = 999,
            MeterReadingDateTime = "22/04/2019 12:25",
            MeterReadValue = "12345"
        };
        var result = new MeterReadingUploadResultDto();
        var accounts = new List<Account> { new() { AccountId = 123 } };
        var readings = new List<MeterReading>();

        var isValid = _validator.ValidateReading(dto, result, accounts, readings);

        Assert.IsFalse(isValid);
        Assert.AreEqual(0, result.SuccessfulReadings);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].Contains("Account ID 999 does not exist."));
    }

    [TestMethod]
    public void ValidateReading_ShouldReturnFalse_WhenMeterReadValueIsInvalidFormat()
    {
        var dto = new MeterReadingCsvDto
        {
            AccountId = 123,
            MeterReadingDateTime = "22/04/2019 12:25",
            MeterReadValue = "12A45"
        };
        var result = new MeterReadingUploadResultDto();
        var accounts = new List<Account> { new() { AccountId = 123 } };
        var readings = new List<MeterReading>();

        var isValid = _validator.ValidateReading(dto, result, accounts, readings);

        Assert.IsFalse(isValid);
        Assert.AreEqual(0, result.SuccessfulReadings);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].Contains("Invalid meter read format '12A45' for account 123."));
    }
}
