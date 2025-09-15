using RepositoryContracts;
using Entities;
namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly ICommentRepository commentRepository;
    private readonly ManagePostView managePostView;
    private readonly IPostRepository postRepostitory;

    private PostView postView;
    private bool running;

    public ListPostsView(IPostRepository postRepository, ICommentRepository commentRepository,
        ManagePostView managePostView)
    {
        postRepostitory = postRepository;
        this.commentRepository = commentRepository;
        this.managePostView = managePostView;
    }

    public async Task ListPosts()
    {
        running = true;
        while (running)
        {
            Console.WriteLine("Listing all posts...");
            foreach (var post in postRepostitory.GetAll()) Console.WriteLine($"(Title: {post.title} (ID: {post.id})");

            Console.WriteLine("===Post Selected===\n" +
                              "[1] View Post Details \n" +
                              "[2] Back to ...\n" +
                              "===================");
            int? selection = int.Parse(Console.ReadLine());
            switch (selection)
            {
                case 1:
                    Console.Write("Enter ID to select post ");
                    var postSelection = Console.ReadLine();
                    var selecteedPost = postRepostitory.GetSingleAsync(int.Parse(postSelection)):
                    if (selecteedPost is null)
                    {
                        Console.WriteLine("Error: Post not found.");
                        break;
                    }

                    if (PostView is null) postView = new PostView(commentRepository, selecteedPost, this);

                    await postView.ShowPost();
                    break;
                default: running = false; break;
            }
        }

        managePostView.ShowOptions();
    }
}