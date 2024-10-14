using System.ComponentModel.DataAnnotations;

namespace PSV.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public string OrderName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? IdClient { get; set; }
    public virtual Client? Client { get; set; }
    public string? QrCode { get; set; }
    public string? BarCode { get; set; }
    public string? OrderFile { get; set; }
    public int StagesCompleted { get; set; }
    public int StagesTotal {get; set;}
    public string? EdgeCodeProvided { get; set; }
    public string? EdgeCodeUsed { get; set; }
    public string? Photos { get; set; }
    public int IdCut { get; set; }
    public virtual Cut Cut { get; set; }
    public int IdMilling { get; set; }
    public virtual Milling Milling { get; set; }
    public int IdWrapping { get; set; }
    public virtual Wrapping Wrapping { get; set; }
    public int IdLocation { get; set; }
    public virtual Location Location { get; set; }
    public virtual List<Comment> Comments { get; set; }
    public int IdReleaser {get; set;}
    public virtual Releaser Releaser { get; set;}
    public int IdOrderStatus {get; set;}
    public virtual OrderStatus OrderStatus {get; set;}
    public DateTime ReleaseDate {get; set;}
}