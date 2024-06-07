namespace PSV.Models;

public class Operator
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public virtual List<Cut> Cuts { get; set; } = new List<Cut>();
    public virtual List<Milling> Millings { get; set; } = new List<Milling>();
    public virtual List<Wrapping> Wrappings { get; set; } = new List<Wrapping>();
}