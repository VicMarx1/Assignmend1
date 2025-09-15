using Entities;
using CLI.UI.ManageComments;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class PostView
{
    private readonly ICommentRepository commentRepository;
    private readonly ListPostsView listPostsView;
    private readonly Task<Post> post;

   private ListCommentsView listCommentsView;

    public PostView(ICommentRepository commentRepository, Task<Post> post, ListPostsView listPostsView)
    {
        this.commentRepository = commentRepository;
        this.post = post;
        this.listPostsView = listPostsView;
    }

    public async Task ShowPost()
    {
        Console.WriteLine($"Post {post.Result.Title}[{post.Result.Id}]\n" + $"By User: {post.Result.UserId}");
        Console.WriteLine("===Post Options===\n" +
                          "[1] View Comments \n" +
                          "[2] Edit Post \n" +
                          "[3] Delete Post\n" +
                          "[4] Back To Main Menu\n" +
                          "===================");
        int? selection = int.Parse(Console.ReadLine());
        switch (selection)
        {
            case 1:
                if (listCommentsView is null)
                {
                    listCommentsView = new ListCommentsView(commentRepository, post, this);
                }

                await listCommentsView.ListComments();
                break;

            default:
                await listPostsView.ListPosts();
                break;
        }
    }
}