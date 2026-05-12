using Car_Rental_System.Models;
using Car_Rental_System.Presenters;
using Car_Rental_System.Views;

namespace Car_Rental_System
{
    public partial class LoginForm : Form, ILoginView
    {
        private readonly LoginPresenter presenter;

        internal AppUser? AuthenticatedUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            presenter = new LoginPresenter(this);
            AcceptButton = loginButton;
            CancelButton = exitButton;
        }

        public string UserName => userNameTextBox.Text;
        public string Password => passwordTextBox.Text;

        public void ShowLoginStatus(string message)
        {
            statusLabel.Text = message;

            if (!string.IsNullOrWhiteSpace(message) &&
                message.StartsWith("Invalid", StringComparison.OrdinalIgnoreCase))
            {
                passwordTextBox.SelectAll();
                passwordTextBox.Focus();
            }
        }

        void ILoginView.CompleteLogin(AppUser user)
        {
            AuthenticatedUser = user;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void LoginForm_Load(object? sender, EventArgs e)
        {
            userNameTextBox.Focus();
        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            try
            {
                presenter.SignIn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Login Configuration Missing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void ExitButton_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
