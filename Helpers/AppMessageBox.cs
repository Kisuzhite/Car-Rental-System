using System.Diagnostics;

namespace Car_Rental_System.Helpers
{
    internal static class AppMessageBox
    {
        public static void ShowLocalDbWarning(Exception exception)
        {
            Debug.WriteLine(exception);

            MessageBox.Show(
                "The app could not connect to SQL Server LocalDB yet.\n\nInstall SQL Server Express LocalDB from Visual Studio Installer, then run the project again.\n\nDetails: " + exception.Message,
                "LocalDB Not Ready",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static void ShowDatabaseError(string action, Exception exception)
        {
            Debug.WriteLine(exception);
            Exception rootCause = exception.GetBaseException();

            MessageBox.Show(
                $"{action}\n\nThe database operation could not be completed. Please check your data and try again.\n\nDetails: {rootCause.Message}",
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
