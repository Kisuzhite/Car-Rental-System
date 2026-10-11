using System.Diagnostics;
using Car_Rental_System.Presenters;

namespace Car_Rental_System.Helpers
{
    internal static class AppMessageBox
    {
        public static void ShowLocalDbWarning(Exception exception)
        {
            Debug.WriteLine(exception);
            AppLogger.LogException("LocalDB connection warning", exception);

            MessageBox.Show(
                "The app could not connect to SQL Server LocalDB yet.\n\nInstall SQL Server Express LocalDB from Visual Studio Installer, then run the project again.\n\nDetails: " + exception.Message,
                "LocalDB Not Ready",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static void ShowDatabaseError(string action, Exception exception)
        {
            Debug.WriteLine(exception);
            AppLogger.LogException(action, exception);
            Exception rootCause = exception.GetBaseException();

            MessageBox.Show(
                $"{action}\n\nThe database operation could not be completed. Please check your data and try again.\n\nDetails: {rootCause.Message}",
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ShowDatabaseStartupError(Exception exception)
        {
            Debug.WriteLine(exception);
            AppLogger.LogException("Database startup preparation failed", exception);
            Exception rootCause = exception.GetBaseException();

            MessageBox.Show(
                $"The database could not be prepared for startup.\n\nPlease make sure SQL Server LocalDB is installed and the connection string is correct. Details have been saved to the log.\n\nDetails: {rootCause.Message}",
                "Database Startup Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ShowPresenterResult(PresenterResult result)
        {
            MessageBox.Show(
                result.Message,
                result.Title,
                MessageBoxButtons.OK,
                GetMessageBoxIcon(result.Kind));
        }

        public static MessageBoxIcon GetMessageBoxIcon(PresenterMessageKind kind)
        {
            return kind switch
            {
                PresenterMessageKind.Warning => MessageBoxIcon.Warning,
                PresenterMessageKind.Error => MessageBoxIcon.Error,
                _ => MessageBoxIcon.Information
            };
        }

        public static void ShowUnexpectedError(Exception exception)
        {
            Debug.WriteLine(exception);
            AppLogger.LogException("Unexpected application error", exception);

            MessageBox.Show(
                $"An unexpected error occurred. Details have been saved to the log.\n\nLog file:\n{AppLogger.CurrentLogFilePath}",
                "Unexpected Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
