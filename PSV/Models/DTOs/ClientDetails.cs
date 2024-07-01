using System.ComponentModel.DataAnnotations;

namespace PSV.Models.DTOs;

public class ClientDetails
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Pole jest wymagane")]
    public string Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? NIP { get; set; }
}