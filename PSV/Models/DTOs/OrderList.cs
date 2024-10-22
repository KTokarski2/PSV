namespace PSV.Models.DTOs;

public class OrderList
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public string OrderName { get; set; }
    public string CreatedAt { get; set; }
    public string Client { get; set; }
    public bool Cut { get; set; }
    public bool Milling { get; set; }
    public bool Wrapping { get; set; }
    public bool CutDone { get; set; }
    public bool MillingDone { get; set; }
    public bool WrappingDone { get; set; }
    public string EdgeCodeProvided { get; set; }
    public string Location { get; set; }
    public string Status { get; set; }

}