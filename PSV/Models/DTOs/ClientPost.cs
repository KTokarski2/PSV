using System.ComponentModel.DataAnnotations;

namespace PSV.Models.DTOs;

public class ClientPost
{
    [Required(ErrorMessage = "Pole jest wymagane")]
    public string Name { get; set; }
    public string? Address { get; set; }

    [Required(ErrorMessage = "Pole jest wymagane")]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Nieprawidłowy numer telefonu.")]
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? NIP { get; set; }
}