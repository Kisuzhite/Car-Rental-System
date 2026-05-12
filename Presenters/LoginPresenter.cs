using Car_Rental_System.Models;
using Car_Rental_System.Services;
using Car_Rental_System.Views;

namespace Car_Rental_System.Presenters
{
    internal sealed class LoginPresenter
    {
        private readonly ILoginView view;
        private readonly AuthService authService = new();

        public LoginPresenter(ILoginView view)
        {
            this.view = view;
        }

        public void SignIn()
        {
            view.ShowLoginStatus("");

            if (string.IsNullOrWhiteSpace(view.UserName) ||
                string.IsNullOrWhiteSpace(view.Password))
            {
                view.ShowLoginStatus("Enter the owner username and password.");
                return;
            }

            if (authService.TrySignIn(view.UserName, view.Password, out AppUser? user) &&
                user != null)
            {
                view.CompleteLogin(user);
                return;
            }

            view.ShowLoginStatus("Invalid owner username or password.");
        }
    }
}
