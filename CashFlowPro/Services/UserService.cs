using CashFlowPro.Abstraction;
using CashFlowPro.Model;
using CashFlowPro.Services.Interface;

namespace CashFlowPro.Services
{
    public class UserService : UserBase, IUser
    {
        private List<User> _users;

        public const string SeedUsername = "me";
        public const string SeedPassword = "123";

        public UserService()
        {
            _users = LoadUsers();

            if (!_users.Any())
            {
                _users.Add(new User { UserName = SeedUsername, Password = SeedPassword });
                SaveUsers(_users);
            }

        }
        public bool Login(User user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return false; // Invalid input.
            }

            // Check if the username and password match any user in the list.
            return _users.Any(u => u.UserName == user.UserName && u.Password == user.Password);
        }
    }
}
