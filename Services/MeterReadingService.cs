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
                //csv.Context.RegisterClassMap<MeterReadingCsvDtoMap>();
                var csvRecords = csv.GetRecords<MeterReadingCsvDto>().ToList();
                //RemoveDuplicatesFromCsvData(csvRecords); //todo
                var validReadings = new List<MeterReading>();
                
                var distinctAccountIds = csvRecords.Select(r => r.AccountId).Distinct().ToList();
                var latestReadingsStored = await _repository.GetLatestReadingsForAccountsAsync(distinctAccountIds);
                var validAccounts = await _accountRepository.GetValidAccountsAsync(distinctAccountIds);
                
                csvRecords.ForEach(csvRecord =>
                {
                    if (_validator.ValidateReading(csvRecord, result, validAccounts, latestReadingsStored))
                    {
                        validReadings.Add(new MeterReading
                        {
                            AccountId = csvRecord.AccountId,
                            MeterReadingDateTime = DateTime.Parse(csvRecord.MeterReadingDateTime),
                            MeterReadValue = csvRecord.MeterReadValue
                        });

                        // too slow
                        //await _repository.AddAsync(validReading);
                        //await _repository.SaveChangesAsync();
                    }
                });

                // better
                // await _repository.AddRangeAsync(validReadings);
                // await _repository.SaveChangesAsync();

                CalculateFailedReadings(result, csvRecords);
            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }
            
            return result;
        }

        private void CalculateFailedReadings(MeterReadingUploadResultDto result, List<MeterReadingCsvDto> csvRecords)
        {
            result.FailedReadings = csvRecords.Count - result.SuccessfulReadings;
        }
    }
}