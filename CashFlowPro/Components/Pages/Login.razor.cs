using CashFlowPro.Base;
using CashFlowPro.Model;

namespace CashFlowPro.Components.Pages
{
    public partial class Login
    {

        private User Users { get; set; } = new();

        private string ErrorMessage { get; set; } = string.Empty;


        private void HandleLogin()
        {
            if (UserService.Login(Users))
            {
                // Check if the currency is valid
                if (Enum.IsDefined(typeof(Currency), Users.Currency))
                {
                    Nav.NavigateTo("/Dashboard");
                }
                else
                {
                    ErrorMessage = "Invalid currency selected.";
                }
            }
            else
            {
                ErrorMessage = "Username or password is invalid.";
            }
        }


    }
}