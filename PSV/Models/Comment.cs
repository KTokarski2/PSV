namespace PSV.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Time { get; set; }
    public string Source { get; set; }
    public int? IdOperator { get; set; }
    public virtual Operator? Operator { get; set; }
    public int IdOrder { get; set; }
    public virtual Order Order { get; set; }
}