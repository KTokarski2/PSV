using System.ComponentModel.DataAnnotations;

namespace PSV.Models.DTOs;

public class OperatorDetails
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Pole jest wymagane")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Pole jest wymagane")]
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string Location { get; set; }
    public List<LocationInfo>? AllLocations { get; set; }
}