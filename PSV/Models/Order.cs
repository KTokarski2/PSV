using System.ComponentModel.DataAnnotations;

namespace PSV.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public int IdClient { get; set; }
    public virtual Client Client { get; set; }
    public string? QrCode { get; set; }
    public string? BarCode { get; set; }
    public string Format { get; set; }
    public string? Comments { get; set; }
    public string? Photos { get; set; }
    public int IdCut { get; set; }
    public virtual Cut Cut { get; set; }
    public int IdMilling { get; set; }
    public virtual Milling Milling { get; set; }
    public int IdWrapping { get; set; }
    public virtual Wrapping Wrapping { get; set; }
}