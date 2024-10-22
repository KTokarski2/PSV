namespace PSV.Models.DTOs;

public class OrderRelease
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; }
    public List<ReleaserDetails>? Releasers { get; set; }
    public RedirectionModel Redirection { get; set; }
}