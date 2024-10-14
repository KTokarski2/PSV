using System.ComponentModel.DataAnnotations;
using PSV.Models.DTOs;

namespace PSV.Models.DTOs;

public class ReleaserDetails
{
    public int Id { get; set;}

    [Required(ErrorMessage = "Pole jest wymagane")]
    public string FirstName {get; set;}

    [Required(ErrorMessage = "Pole jest wymagane")]
    public string LastName {get; set;}
    public string Location {get; set;}
    public List<LocationInfo>? AllLocations {get; set;}
}