using System.ComponentModel.DataAnnotations;

namespace PSV.Models.DTOs;

public class OrderDetails
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Pole jest wymagane")]
    public string OrderNumber { get; set; }
    
    [Required(ErrorMessage = "Pole jest wymagane")]
    public string OrderName { get; set; }
    public bool Cut { get; set; }
    public string? CutStart { get; set; }
    public string? CutEnd { get; set; }
    public string? CutTime { get; set; }
    public string? CutOperator { get; set; }
    public bool Milling { get; set; }
    public string? CreatedAt { get; set; }
    public string? MillingStart { get; set; }
    public string? MillingEnd { get; set; }
    public string? MillingTime { get; set; }
    public string? MillingOperator { get; set; }
    public bool Wrapping { get; set; }
    public string? WrappingStart { get; set; }
    public string? WrappingEnd { get; set; }
    public string? WrappingTime { get; set; }
    public string? WrappingOperator { get; set; }
    public string? Comments { get; set; }
    public List<string>? Photos { get; set; }
    public List<ClientInfo>? AllClients { get; set; }
    public string Location { get; set; }
    public List<LocationInfo>? AllLocations { get; set; }
    public int ClientId { get; set; }
    public string? EdgeCodeProvided{ get; set; }
    public string? EdgeCodeUsed { get; set; }
    public string? Status { get; set; }
    public string? ReleasedAt { get; set; }
    public string? ReleasedBy { get; set; }
}