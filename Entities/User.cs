namespace Entities;

public class User
{
    public int Id { set; get; }
    
    public string Email { set; get; }
    public string Username { set; get; }
    public string Password { set; get; }
    
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get;set; } = new List<Comment>();
    
}