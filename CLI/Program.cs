using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;
using FileRepositories;

Console.WriteLine("Starting CLI...");
IUserRepository userRepository = new UserFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();
IPostRepository postRepository = new PostFileRepository();

var cliApp = new CliApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();