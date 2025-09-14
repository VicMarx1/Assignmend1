using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;
namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private bool running;
    
    private ManagePostView managePostView;
    private ManageUserView manageUserView;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }
}

