using Entities;
using CLI.UI.ManagePosts;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository commentRepository;
    private Task<Post> post;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    private bool running;
    
    private CommentView CommentView;

    public ListCommentsView(ICommentRepository commentRepository, IUserRepository userRepository,
        IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }

    public async Task ListComments()
    {
        running = true;
        while (running)
        {
            Console.WriteLine($"Listing Comments Form Post {post.Result.Title} [{post.Result.Id}]");
            foreach (var comment in commentRepository.GetAll().Where(c => c.PostId == post.Result.Id))
            {
                Console.WriteLine($"User: {comment.UserId} {comment.Id} \n Comment:\n {comment.Body}");
            }
            Console.WriteLine();
            
        }
    }

}