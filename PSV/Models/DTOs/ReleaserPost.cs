using System.ComponentModel.DataAnnotations;

namespace PSV.Models.DTOs;

public class ReleaserPost
{
    [Required(ErrorMessage = "Imię jest wymagane")]
    public string FirstName {get; set;}

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public string LastName {get; set;}
    public string Location {get; set;}
    public List<LocationInfo>? AllLocations {get; set;}
}