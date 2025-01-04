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
                Nav.NavigateTo("/Dashboard");
            }

            else
            {
                ErrorMessage = "userName or password is invalid";
            }
        }

    }
}