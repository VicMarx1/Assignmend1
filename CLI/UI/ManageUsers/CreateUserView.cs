using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;
    private readonly ManageUserView manageUserView;

    public CreateUserView(IUserRepository userRepository, ManageUserView manageUserView)
    {
        this.userRepository = userRepository;
        this.manageUserView = manageUserView;
    }
    
}