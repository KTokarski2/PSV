using PSV.Models;

public class Releaser
{
    public int Id { get; set; }
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public int IdLocation {get; set;}
    public virtual Location Location { get; set; }
    public virtual List<Order> Orders { get; set; }
}