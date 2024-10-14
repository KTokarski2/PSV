using System.Runtime.InteropServices.JavaScript;

namespace PSV.Models.DTOs;

public class OrderControl
{ 
    public int Id { get; set; }
    public string? OrderNumber { get; set; }
    public string? EdgeCode { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Comments { get; set; }
    public bool StartTimer { get; set; }
    public bool StopTimer { get; set; }
    public string? TotalTime { get; set; }
    public int OperatorId { get; set; }
    public List<OperatorInfo>? Operators { get; set; }
}