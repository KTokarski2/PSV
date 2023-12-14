using System.ComponentModel.DataAnnotations;

namespace PSV.Models;

public class Cut
{
    [Key]
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsPresent { get; set; }
    public virtual Order Order { get; set; }
    public int IdOrder { get; set; }
}