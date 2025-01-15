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
            var existingUser = _users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

            if (existingUser != null)
            {
                // Update the user's currency if it doesn't match or is invalid (this assumes you want to store the currency value)
                existingUser.Currency = user.Currency;

                // Save updated users to the file
                SaveUsers(_users);

                return true;
            }

            return false;
        }
    }
}
