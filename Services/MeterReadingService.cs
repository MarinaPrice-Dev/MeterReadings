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
                
                csvRecords.ForEach(csvRecord =>
                {
                    if (_validator.ValidateReading(csvRecord, result).Result)
                    {
                        var validReading = new MeterReading
                        {
                            AccountId = csvRecord.AccountId,
                            MeterReadingDateTime = DateTime.Parse(csvRecord.MeterReadingDateTime),
                            MeterReadValue = csvRecord.MeterReadValue
                        };

                        //_repository.AddAsync(validReading);
                        //_repository.SaveChangesAsync();
                    }
                });

            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
        }

        // private void Validate()
        // {
        //     if (!DateTime.TryParseExact(dto.MeterReadingDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        //     {
        //         result.Errors.Add($"Invalid date format for account {dto.AccountId}: {dto.MeterReadingDateTime}");
        //         continue;
        //     }
        // }
    }
}