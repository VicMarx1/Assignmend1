namespace Entities;

public class Post
{
    public int Id { set; get; }
    public string Title { set; get; }
    public string Body { set; get; }
    public int UserId { set; get; }
    
    public User User { set; get; } = null!;
    public ICollection<Comment> Comments { get;set; } = new List<Comment>();
}