namespace PSV.Models.DTOs;

public class OrderDetails
{
    public string OrderNumber { get; set; }
    public bool Cut { get; set; }
    public bool Milling { get; set; }
    public bool Wrapping { get; set; }
    public string FormatCode { get; set; }
    public string Client { get; set; }
    public string Comments { get; set; }
    public List<IFormFile> Photos { get; set; }
}