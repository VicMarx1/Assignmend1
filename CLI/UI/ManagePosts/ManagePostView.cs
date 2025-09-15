using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostView
{
    private readonly CliApp cliApp;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private CreatePostView createPostView;

    private ListPostView listPostView;
    private bool running;

    public ManagePostView(IPostRepository postRepository,
        ICommentRepository commentRepository, CliApp cliApp)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.cliApp = cliApp;
    }

    public async Task ShowOptions()
    {
        running = true;
        while (running)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("====Manages Posts====" +
                              "[1] List Posts \n" +
                              "[2] Create Post \n" +
                              "[3]Back to Main Menu\n" +
                              "====================");
            int? selection = int.Parse(Console.ReadLine());
            switch (selection)
            {
                case 1:
                    if (listPostView is null) listPostView = new ListPostView(postRepository, commentRepository, this);

                    await listPostView.ListPosts();
                    break;
                case 2:
                    if (createPostView is null) createPostView = new CreatePostView(postRepository, this);

                    await createPostView.createPost();
                    break;
            }
        }

        await cliApp.StartAsync();
    }
}