namespace PSV.Models.DTOs;

public class OrderList
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Client { get; set; }
    public bool Cut { get; set; }
    public bool Milling { get; set; }
    public bool Wrapping { get; set; }
}