namespace Car_Rental_System.Helpers
{
    internal sealed class UiDebouncer : IDisposable
    {
        private readonly System.Windows.Forms.Timer timer;
        private Func<Task>? pendingAction;
        private bool disposed;

        public UiDebouncer(int delayMilliseconds = 350)
        {
            timer = new System.Windows.Forms.Timer
            {
                Interval = delayMilliseconds
            };
            timer.Tick += Timer_Tick;
        }

        public void Run(Func<Task> action)
        {
            if (disposed)
            {
                return;
            }

            pendingAction = action;
            timer.Stop();
            timer.Start();
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            timer.Stop();

            Func<Task>? action = pendingAction;
            pendingAction = null;

            if (action == null)
            {
                return;
            }

            try
            {
                await action();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowUnexpectedError(ex);
            }
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            timer.Stop();
            timer.Dispose();
        }
    }
}
