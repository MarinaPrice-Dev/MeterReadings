using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeterReadings.Models.Data;

public class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Internal primary key
    
    public int AccountId { get; set; } // Business identifier from CSV
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}
