using System.ComponentModel.DataAnnotations;
using PSV.Models;

public class OrderStatus
{
    [Key]
    public int Id { get; set;}
    public string Name {get; set;}
    public virtual List<Order> Orders { get; set;}
}