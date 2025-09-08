namespace Entities;

public class Comment
{
    public int id { set; get; }
    public string body { set; get; }
    public int userId { set; get; }
    public int postId { set; get; }
}