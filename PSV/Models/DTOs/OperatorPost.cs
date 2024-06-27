namespace PSV.Models.DTOs;

public class OperatorPost
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Location { get; set; }
    public List<LocationInfo> AllLocations { get; set; }
}