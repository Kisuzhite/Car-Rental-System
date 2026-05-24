namespace Car_Rental_System.Helpers
{
    internal static class AppIcon
    {
        public static void ApplyTo(Form form)
        {
            try
            {
                using Icon? icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                if (icon != null)
                {
                    form.Icon = (Icon)icon.Clone();
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogException("Unable to apply application icon", ex);
            }
        }
    }
}
