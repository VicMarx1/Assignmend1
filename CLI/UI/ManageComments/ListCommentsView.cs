using Entities;
using CLI.UI.ManagePosts;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository commentRepository;
    private Task<Post> post;
    private readonly PostView postView;
    private bool running;

    private CommentView commentView;
    private CreateCommentView createCommentView;

    public ListCommentsView(ICommentRepository commentRepository,Task<Post> post, PostView postView)
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
            {
                Console.WriteLine($"User: {comment.UserId} {comment.Id} \n Comment:\n {comment.Body}");
            }
            Console.WriteLine("===Comment Option===\n" + "Select an option:\n" + "[1] Add Comment\n" +
                              "[2] Edit Comment\n" + "[3] Delete Comment\n" + "[4] Back to Post Menu\n" + "===================");
                              int? selcection = int.Parse(Console.ReadLine());

                              switch (selcection)
                              {
                                  case 1:
                                      if (createCommentView is null)
                                      {
                                          createCommentView = new CreateCommentView(commentRepository, this, post);
                                      }
                                      await createCommentView.CreateComment();
                                      break;
                              }

        }
    }

}