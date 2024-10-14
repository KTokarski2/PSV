namespace PSV.Models;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual List<Order> Orders { get; set; } = new List<Order>();
    public virtual List<Operator> Operators { get; set; } = new List<Operator>();
    public virtual List<Releaser> Releasers {get; set;} = new List<Releaser>();
}