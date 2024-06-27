namespace PSV.Models.DTOs;

public class OperatorDetails
{
    public int Id { get; set; }
    public string FristName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Location { get; set; }
    public List<LocationInfo> AllLocations { get; set; }
}