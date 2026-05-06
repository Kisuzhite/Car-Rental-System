using Car_Rental_System.Models;
using Car_Rental_System.Services;

namespace Car_Rental_System
{
    public partial class LoginForm : Form
    {
        private readonly AuthService authService = new();

        internal AppUser? AuthenticatedUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            AcceptButton = loginButton;
            CancelButton = exitButton;
        }

        private void LoginForm_Load(object? sender, EventArgs e)
        {
            userNameTextBox.Focus();
        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            statusLabel.Text = "";

            if (string.IsNullOrWhiteSpace(userNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                statusLabel.Text = "Enter the owner username and password.";
                return;
            }

            try
            {
                if (authService.TrySignIn(userNameTextBox.Text, passwordTextBox.Text, out AppUser? user))
                {
                    AuthenticatedUser = user;
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                statusLabel.Text = "Invalid owner username or password.";
                passwordTextBox.SelectAll();
                passwordTextBox.Focus();
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
