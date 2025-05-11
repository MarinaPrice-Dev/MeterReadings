using CsvHelper;
using MeterReadings.Models.Dto;
using System.Globalization;
using MeterReadings.Models.Entities;
using MeterReadings.Repositories;
using MeterReadings.Services.Validation;

namespace MeterReadings.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingValidator _validator;
        private readonly IMeterReadingRepository _repository;
        private readonly IAccountRepository _accountRepository;
        
        public MeterReadingService(IMeterReadingValidator validator, IMeterReadingRepository repository, IAccountRepository accountRepository)
        {
            _validator = validator;
            _repository = repository;
            _accountRepository = accountRepository;
        }
        
        public async Task<MeterReadingUploadResultDto> UploadMeterReadingsAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var result = new MeterReadingUploadResultDto();

            try
            {
                var csvRecords = csv.GetRecords<MeterReadingCsvDto>().ToList();
                var validReadings = new List<MeterReading>();
                
                var distinctAccountIds = csvRecords.Select(r => r.AccountId).Distinct().ToList();
                var validAccounts = await _accountRepository.GetValidAccountsAsync(distinctAccountIds);
                var latestReadingsStored = await _repository.GetLatestReadingsForAccountsAsync(distinctAccountIds);
                
                foreach (var csvRecord in csvRecords)
                {
                    if (_validator.ValidateReading(csvRecord, result, validAccounts, latestReadingsStored))
                    {
                        validReadings.Add(new MeterReading
                        {
                            AccountId = csvRecord.AccountId,
                            MeterReadingDateTime = DateTime.Parse(csvRecord.MeterReadingDateTime),
                            MeterReadValue = csvRecord.MeterReadValue
                        });
                    }
                }

                result.FailedReadings = csvRecords.Count - validReadings.Count;

                await _repository.AddRangeAsync(validReadings);
                await _repository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Errors.Add($"{e.Message}, {e.InnerException}");
            }
            
            return result;
        }
    }
}