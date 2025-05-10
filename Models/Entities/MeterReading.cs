
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeterReadings.Models.Entities
{
    public class MeterReading
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        public int AccountId { get; set; } 
        public Account Account { get; set; }

        public DateTime MeterReadingDateTime { get; set; } 

        [MaxLength(5)]
        public string MeterReadValue { get; set; }
    }
}
