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
        
        public MeterReadingService(IMeterReadingValidator validator, IMeterReadingRepository repository)
        {
            _validator = validator;
            _repository = repository;
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
                var validReadings = new List<MeterReading>();
                
                csvRecords.ForEach(csvRecord =>
                {
                    if (_validator.ValidateReading(csvRecord, result).Result)
                    {
                        validReadings.Add(new MeterReading
                        {
                            AccountId = csvRecord.AccountId,
                            MeterReadingDateTime = DateTime.Parse(csvRecord.MeterReadingDateTime),
                            MeterReadValue = csvRecord.MeterReadValue
                        });

                        // works better with validation
                        //await _repository.AddAsync(validReading);
                        //await _repository.SaveChangesAsync();
                    }
                });

                // faster, but breaks how we do validation
                // await _repository.AddRangeAsync(validReadings);
                // await _repository.SaveChangesAsync();

            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
        }
    }
}