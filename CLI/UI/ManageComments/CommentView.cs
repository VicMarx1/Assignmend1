using Entities;

namespace CLI.UI.ManageComments;

public class CommentView
{
    private readonly Task<Comment> comment;
    private readonly ListCommentsView listCommentsView;

    public CommentView(Task<Comment> comment, ListCommentsView listCommentsView)
    {
        this.comment = comment;
        this.listCommentsView = listCommentsView;
    }

    public async Task ShowComment()
    {
        Console.WriteLine($"Comment By: {comment.Result.UserId}\n" +
                          $"{comment.Result.Body}\n");
        Console.WriteLine("===Comment Options===\n" +
                          "[1] Edit Comment \n" +
                          "[2] Delete Comment \n" +
                          "[3] Back To Comments Menu\n" +
                          "=====================");
        int? selection = int.Parse(Console.ReadLine());
        switch (selection)
        {
            case 1:
                throw new NotImplementedException(); break;
            case 2:
                throw new NotImplementedException(); break;
            default:
                await listCommentsView.ListComments(); break;
        }
    }
}