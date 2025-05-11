using System.Text.RegularExpressions;
using MeterReadings.Models.Dto;
using MeterReadings.Models.Entities;
using MeterReadings.Repositories;

namespace MeterReadings.Services.Validation
{
    public class MeterReadingValidator : IMeterReadingValidator
    {
        public bool ValidateReading(
            MeterReadingCsvDto meterReadingCsvRow, 
            MeterReadingUploadResultDto uploadResults, 
            List<Account> validAccounts, 
            List<MeterReading> latestReadingsStored)
        {
            var hasErrors = false;
            
            if (!validAccounts.Exists(x => x.AccountId == meterReadingCsvRow.AccountId))
            {
                uploadResults.Errors.Add($"Account ID {meterReadingCsvRow.AccountId} does not exist.");
                hasErrors = true;
            }
            
            if (!Regex.IsMatch(meterReadingCsvRow.MeterReadValue, @"^\d{5}$"))
            {
                uploadResults.Errors.Add($"Invalid meter read format '{meterReadingCsvRow.MeterReadValue}' for account {meterReadingCsvRow.AccountId}.");
                hasErrors = true;
            }
            
            if (!DateTime.TryParse(meterReadingCsvRow.MeterReadingDateTime, out var parsedDate))
            {
                uploadResults.Errors.Add($"Invalid date format '{meterReadingCsvRow.MeterReadingDateTime}' for account {meterReadingCsvRow.AccountId}.");
                hasErrors = true;
            }
            
            var mostRecent = latestReadingsStored.Find(x => x.AccountId == meterReadingCsvRow.AccountId)?.MeterReadingDateTime;
            if (mostRecent.HasValue && parsedDate <= mostRecent.Value)
            {
                uploadResults.Errors.Add($"Reading for account {meterReadingCsvRow.AccountId} is not newer than the latest reading at {mostRecent.Value}.");
                hasErrors = true;
            }
            
            if (latestReadingsStored.Exists(x=> x.AccountId == meterReadingCsvRow.AccountId && 
                                                x.MeterReadingDateTime == parsedDate &&
                                                x.MeterReadValue == meterReadingCsvRow.MeterReadValue))
            {
                uploadResults.Errors.Add($"Duplicate reading for account {meterReadingCsvRow.AccountId} with date {parsedDate} and value {meterReadingCsvRow.MeterReadValue}.");
                hasErrors = true;
            }

            if (hasErrors)
            {
                return false;
            }

            uploadResults.SuccessfulReadings++;
            UpdateLatestReadings(latestReadingsStored, meterReadingCsvRow, parsedDate);
            return true;
        }

        // We want to keep out latest readings list updated without needing to query the database
        // as we already have the data
        private void UpdateLatestReadings(List<MeterReading> latestReadingsStored,
            MeterReadingCsvDto validMeterReadingCsvRow, DateTime parsedDate)
        {
            var latest = latestReadingsStored.Find(x => x.AccountId == validMeterReadingCsvRow.AccountId);
            if (latest != null)
            {
                //update latest reading 
                latest.MeterReadingDateTime = parsedDate;
                latest.MeterReadValue = validMeterReadingCsvRow.MeterReadValue;
            }
            else
            {
                //account exists but did not have a meter reading previously, so add new
                latestReadingsStored.Add(new MeterReading
                {
                    AccountId = validMeterReadingCsvRow.AccountId,
                    MeterReadingDateTime = parsedDate,
                    MeterReadValue = validMeterReadingCsvRow.MeterReadValue
                });
            }
        }
    }
}