using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PSV.Models.DTOs
{
    public class OrderPost
    {
       [Required(ErrorMessage = "Pole jest wymagane")] 
       [UniqueOrderNumberValidator(ErrorMessage = "Numer zamówienia musi być unikalny")]
       public string OrderNumber { get; set; }
       
       [Required(ErrorMessage = "Pole jest wymagane")]
       public string OrderName { get; set; }
       
       public bool Cut { get; set; }
       
       public bool Milling { get; set; }
       
       public bool Wrapping { get; set; }
       
       public List<ClientInfo>? AllClients { get; set; }
       
       public List<LocationInfo>? AllLocations { get; set; }
       
       [Required(ErrorMessage = "Należy wybrać klienta")]
       public string? Client { get; set; }
       
       [Required(ErrorMessage = "Należy wybrać lokalizację")]
       public string? Location { get; set; }
       
       public string? Comments { get; set; }
       
       public IFormFile? OrderFile { get; set; }
       
       public List<IFormFile>? Photos { get; set; }
       
       public string? EdgeCodeProvided { get; set; }

    }
}