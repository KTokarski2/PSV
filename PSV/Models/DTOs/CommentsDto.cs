namespace PSV.Models.DTOs;

public class CommentsDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public List<CommentDto> Comments { get; set; }
}