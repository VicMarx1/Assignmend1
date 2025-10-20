using CLI.UI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI...");
ICommentRepository commentRepository = new CommentFileRepository();
IPostRepository postRepository = new PostFileRepository();
IUserRepository userRepository = new UserFileRepository();

var cliApp = new CliApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();