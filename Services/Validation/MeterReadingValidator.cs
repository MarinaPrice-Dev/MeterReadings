using System.Text.RegularExpressions;
using MeterReadings.Models.Dto;
using MeterReadings.Repositories;

namespace MeterReadings.Services.Validation
{
    public class MeterReadingValidator : IMeterReadingValidator
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMeterReadingRepository _meterReadingRepository;

        public MeterReadingValidator(
            IAccountRepository accountRepository,
            IMeterReadingRepository meterReadingRepository)
        {
            _accountRepository = accountRepository;
            _meterReadingRepository = meterReadingRepository;
        }

        public async Task<bool> ValidateReading(MeterReadingCsvDto dto, MeterReadingUploadResultDto result)
        {
            if (!await _accountRepository.ExistsAsync(dto.AccountId))
            {
                result.Errors.Add($"Account ID {dto.AccountId} does not exist.");
                return false;
            }
            
            if (!Regex.IsMatch(dto.MeterReadValue, @"^\d{5}$"))
            {
                result.Errors.Add($"Invalid meter read format '{dto.MeterReadValue}' for account {dto.AccountId}.");
                return false;
            }
            
            if (!DateTime.TryParse(dto.MeterReadingDateTime, out var parsedDate))
            {
                result.Errors.Add($"Invalid date format '{dto.MeterReadingDateTime}' for account {dto.AccountId}.");
                return false;
            }
            
            if (await _meterReadingRepository.ExistsAsync(dto.AccountId, parsedDate))
            {
                result.Errors.Add($"Duplicate reading for account {dto.AccountId} at {parsedDate}.");
                return false;
            }

            // if (await _meterReadingRepository.ExistsAsync(dto.AccountId, dto.MeterReadingDateTime))
            // {
            //     result.Errors.Add($"Duplicate reading for account {dto.AccountId} at {dto.MeterReadingDateTime}.");
            //     return false;
            // }
            
            var mostRecent = await _meterReadingRepository.GetMostRecentReadingDateAsync(dto.AccountId);
            if (mostRecent.HasValue && parsedDate <= mostRecent.Value)
            {
                result.Errors.Add($"Reading for account {dto.AccountId} is not newer than the latest reading at {mostRecent.Value}.");
                return false;
            }

            result.SuccessfulReadings++;
            return true;
        }
    }
}