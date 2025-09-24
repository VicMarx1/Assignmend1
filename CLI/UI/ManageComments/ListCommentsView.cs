using CLI.UI.ManagePosts;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository commentRepository;
    private readonly Task<Post> post;
    private readonly PostView postView;

    private CommentView commentView;
    private CreateCommentView createCommentView;
    private bool running;

    public ListCommentsView(ICommentRepository commentRepository, Task<Post> post, PostView postView)
    {
        this.commentRepository = commentRepository;
        this.post = post;
        this.postView = postView;
    }

    public async Task ListComments()
    {
        running = true;
        while (running)
        {
            Console.WriteLine($"Listing Comments Form Post {post.Result.Title} [{post.Result.Id}]");
            foreach (var comment in commentRepository.GetAll().Where(c => c.PostId == post.Result.Id))
                Console.WriteLine($"User: {comment.UserId} {comment.Id} \n " +
                                  $"Comment:\n {comment.Body}");
            Console.WriteLine("===Comment Option===\n" + 
                              "Select an option:\n" + 
                              "[1] Add Comment\n" +
                              "[2] Edit Comment\n" + 
                              "[3] Delete Comment\n" +
                              "[4] Back to Post Menu\n" +
                              "===================");
            int? selcection = int.Parse(Console.ReadLine());

            switch (selcection)
            {
                case 1:
                    if (createCommentView is null)
                        createCommentView = new CreateCommentView(commentRepository, this, post);
                    await createCommentView.CreateComment();
                    break;
                case 2:
                    Console.WriteLine("Enter Comment [Id] to Edit:");
                    var commentSelection = Console.ReadLine();
                    var selectedComment = commentRepository.GetSingleAsync(int.Parse(commentSelection));
                    if (selectedComment is null)
                    {
                        Console.WriteLine($"Error: Comment {selectedComment.Id} not found");
                        break;
                    }

                    if (commentView is null) commentView = new CommentView(selectedComment, this);

                    await commentView.ShowComment();
                    break;
                default:
                    running = false;
                    break;
            }
        }

        postView.ShowPost();
    }
}