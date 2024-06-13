namespace PSV.Models.DTOs;

public class OrderDetails
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public bool Cut { get; set; }
    public string CutStart { get; set; }
    public string CutEnd { get; set; }
    public string CutTime { get; set; }
    public bool Milling { get; set; }
    public string MillingStart { get; set; }
    public string MillingEnd { get; set; }
    public string MillingTime { get; set; }
    public bool Wrapping { get; set; }
    public string WrappingStart { get; set; }
    public string WrappingEnd { get; set; }
    public string WrappingTime { get; set; }
    public string Comments { get; set; }
    public List<string> Photos { get; set; }
    public List<ClientInfo> AllClients { get; set; }
    
    public int ClientId { get; set; }
    public string EdgeCodeProvided{ get; set; }
    public string EdgeCodeUsed { get; set; }
}