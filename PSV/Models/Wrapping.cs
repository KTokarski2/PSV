using System.ComponentModel.DataAnnotations;

namespace PSV.Models;

public class Wrapping
{
    [Key]
    public int Id { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool IsPresent { get; set; }
    public bool ClientNotified {get; set;}
    public virtual Order Order { get; set; }
    public int? IdOperator { get; set; }
    public virtual Operator? Operator { get; set; }
}