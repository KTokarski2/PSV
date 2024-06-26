using System.ComponentModel.DataAnnotations;

namespace PSV.Models;

public class Client
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NIP { get; set; }
    public string? Email { get; set; }
    public virtual List<Order> Orders { get; set; } = new List<Order>();
}