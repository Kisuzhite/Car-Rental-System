namespace Car_Rental_System.Helpers
{
    internal sealed class UiBusyScope : IDisposable
    {
        private readonly List<(Control Control, bool WasEnabled)> controls = new();
        private bool disposed;

        private UiBusyScope(IEnumerable<Control?> controlsToDisable)
        {
            foreach (Control? control in controlsToDisable)
            {
                if (control == null || control.IsDisposed)
                {
                    continue;
                }

                controls.Add((control, control.Enabled));
                control.Enabled = false;
            }
        }

        public static UiBusyScope Disable(params Control?[] controls)
        {
            return new UiBusyScope(controls);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            foreach ((Control control, bool wasEnabled) in controls)
            {
                if (!control.IsDisposed)
                {
                    control.Enabled = wasEnabled;
                }
            }
        }
    }
}
