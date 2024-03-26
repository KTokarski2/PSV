using System.Runtime.InteropServices.JavaScript;

namespace PSV.Models.DTOs;

public class OrderControl
{ 
    public int Id { get; set; }
    
    public string OrderNumber { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Comments { get; set; }
}