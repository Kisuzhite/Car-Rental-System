namespace Car_Rental_System
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            using var loginForm = new LoginForm();

            if (loginForm.ShowDialog() == DialogResult.OK &&
                loginForm.AuthenticatedUser != null)
            {
                using var mainForm = new MainForm(loginForm.AuthenticatedUser)
                {
                    WindowState = FormWindowState.Maximized
                };

                Application.Run(mainForm);
            }
        }
    }
}
