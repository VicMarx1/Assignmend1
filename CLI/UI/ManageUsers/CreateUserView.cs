using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly ManageUserView manageUserView;
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository, ManageUserView manageUserView)
    {
        this.userRepository = userRepository;
        this.manageUserView = manageUserView;
    }
}