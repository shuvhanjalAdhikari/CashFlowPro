using System.Text.Json;
using CashFlowPro.Model;

namespace CashFlowPro.Abstraction
{
    public abstract class UserBase
    {
        protected static readonly string FilePath = Path.Combine(@"C:\Users\ashuv\AppDevCW1\CashFlowPro\CashFlowPro\CashFlowPro\wwwroot", "user.json");
        protected List<User> LoadUsers()
        {
            if (!File.Exists(FilePath)) return new List<User>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        protected void SaveUsers(List<User> users)
        {

            var json = JsonSerializer.Serialize(users);

            File.WriteAllText(FilePath, json);
        }
    }
}
