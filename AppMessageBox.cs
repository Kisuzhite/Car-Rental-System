namespace Car_Rental_System
{
    internal static class AppMessageBox
    {
        public static void ShowLocalDbWarning(Exception exception)
        {
            MessageBox.Show(
                "The app could not connect to SQL Server LocalDB yet.\n\nInstall SQL Server Express LocalDB from Visual Studio Installer, then run the project again.\n\nDetails: " + exception.Message,
                "LocalDB Not Ready",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static void ShowDatabaseError(string action, Exception exception)
        {
            MessageBox.Show(
                $"{action}\n\n{exception.Message}",
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
