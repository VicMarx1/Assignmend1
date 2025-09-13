using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI Application...");
IUserRepository userRepository = new UserInMemoryRepository();
CommentInMemoryRepository userCommentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();

ICliApp cliApp = new CliApp(userRepository, userCommentRepository, postRepository);
await cliApp.StartAsync();
