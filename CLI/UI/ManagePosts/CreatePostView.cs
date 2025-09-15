using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly ManagePostView managePostsView;
    private readonly IPostRepository postRepository;

    public CreatePostView(IPostRepository postRepository, ManagePostView managePostsView)
    {
        this.postRepository = postRepository;
        this.managePostsView = managePostsView;
    }

    public async Task createPost()
    {
        Console.WriteLine("Createing a new post...");
        Console.Write("Enter the title of the post: ");
        var title = Console.ReadLine();
        Console.Write("Enter the body of the post: ");
        var body = Console.ReadLine();
        Console.Write("Enter the user ID of the author: ");
        var userId = int.Parse(Console.ReadLine());

        await postRepository.AddAsync(new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        });

        Console.WriteLine($"New post '{title}' created successfully. User ID: {userId}");
        await managePostsView.ShowOptions();
    }
}