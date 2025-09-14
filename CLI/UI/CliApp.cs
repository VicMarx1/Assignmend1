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
/*
    private ManagePostsView managePostsView;
    private ManageUsersView manageUsersView;
*/
    public CliApp(IUserRepository userRepository,
        ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        running = true;
        while (running)
        {
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("Select an option:");
            Console.WriteLine("[1] Manage Posts \n" + "[2] Manage Users \n" + "[3] Exit");
            Console.WriteLine("=================");
            int? selection = int.Parse(Console.ReadLine());
          /*  switch (selection)
            {
                case 1:
                    if (managePostsView is null)
                    {
                        managePostsView = new ManagePostsView(postRepository,
                            commentRepository, this);
                    }

                    await managePostsView.ShowOptions();
                    break;
                case 2:
                    if (manageUsersView is null)
                    {
                        manageUsersView =
                            new ManageUsersView(userRepository, this);
                    }

                    await manageUsersView.ShowOptions();
                    break;
                default:
                    running = false;
                    break;
            }*/
        }
    }
}