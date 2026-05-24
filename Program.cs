namespace Car_Rental_System
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
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
            catch (Exception ex)
            {
                Helpers.AppMessageBox.ShowUnexpectedError(ex);
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Helpers.AppMessageBox.ShowUnexpectedError(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                Helpers.AppLogger.LogException("Unhandled non-UI exception", exception);
                Helpers.AppMessageBox.ShowUnexpectedError(exception);
            }
            else
            {
                var fallbackException = new InvalidOperationException(
                    e.ExceptionObject?.ToString() ?? "Unknown non-UI exception");

                Helpers.AppLogger.LogMessage(
                    "ERROR",
                    "Unhandled non-UI exception",
                    fallbackException.Message);
                Helpers.AppMessageBox.ShowUnexpectedError(fallbackException);
            }
        }
    }
}
