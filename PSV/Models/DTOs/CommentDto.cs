namespace PSV.Models.DTOs;

public class CommentDto
{
    public int Id { get; set;}  
    public string Source { get; set; }
    public string Content { get; set; }
    public string Time { get; set; }
    public string Operator { get; set; }
}