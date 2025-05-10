// using CsvHelper.Configuration;
// using MeterReadings.Models.Dto;
// using System.Globalization;
//
// namespace MeterReadings.Mappings
// {
//     public class MeterReadingCsvDtoMap : ClassMap<MeterReadingCsvDto>
//     {
//         public MeterReadingCsvDtoMap()
//         {
//             Map(m => m.AccountId).Name("AccountId");
//             Map(m => m.MeterReadingDateTime)
//                 .Name("MeterReadingDateTime")
//                 .TypeConverterOption
//                 .DateTimeStyles(DateTimeStyles.AssumeUniversal)
//                 .TypeConverterOption
//                 .Format("dd/MM/yyyy HH:mm", "dd/MM/yyyy HH:mm:ss"); 
//             Map(m => m.MeterReadValue).Name("MeterReadValue");
//         }
//     }
// }