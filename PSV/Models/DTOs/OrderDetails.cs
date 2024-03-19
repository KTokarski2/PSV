namespace PSV.Models.DTOs;

public class OrderDetails
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public bool Cut { get; set; }
    public DateTime CutStart { get; set; }
    public DateTime CutEnd { get; set; }
    public string CutTime { get; set; }
    public bool Milling { get; set; }
    public DateTime MillingStart { get; set; }
    public DateTime MillingEnd { get; set; }
    public string MillingTime { get; set; }
    public bool Wrapping { get; set; }
    public DateTime WrappingStart { get; set; }
    public DateTime WrappingEnd { get; set; }
    public string WrappingTime { get; set; }
    public string FormatCode { get; set; }
    public string Client { get; set; }
    public string Comments { get; set; }
    public List<string> Photos { get; set; }
}