using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView(ICommentRepository commentRepository, ListCommentsView listCommentsView, Task<Post> post)
{
    private readonly ICommentRepository commentRepository = commentRepository;
    private readonly ListCommentsView listCommentsView = listCommentsView;
    private readonly Task<Post> post = post;

    public async Task CreateComment()
    {
        Console.WriteLine($"Creating new Comment [{post.Result.Id}]");
        Console.WriteLine("Enter User Id: ");
        var userId = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter Comment Text: ");
        var body = Console.ReadLine();

        await commentRepository.AddAsync(new Comment
        {
            PostId = post.Result.Id,
            UserId = userId,
            Body = body
        });
        Console.WriteLine($"Comment created. [ID] {commentRepository.GetAll().Last().Id}\n");
        await listCommentsView.ListComments();
    }
}