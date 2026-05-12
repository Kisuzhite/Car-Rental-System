namespace Car_Rental_System.Presenters
{
    internal enum PresenterMessageKind
    {
        Information,
        Warning,
        Error
    }

    internal sealed record PresenterResult(
        bool Success,
        string Title,
        string Message,
        PresenterMessageKind Kind = PresenterMessageKind.Information,
        int? RentalId = null)
    {
        public static PresenterResult Ok(string title, string message, int? rentalId = null)
        {
            return new PresenterResult(true, title, message, PresenterMessageKind.Information, rentalId);
        }

        public static PresenterResult Info(string title, string message)
        {
            return new PresenterResult(false, title, message, PresenterMessageKind.Information);
        }

        public static PresenterResult Warning(string title, string message)
        {
            return new PresenterResult(false, title, message, PresenterMessageKind.Warning);
        }

        public static PresenterResult Error(string title, string message)
        {
            return new PresenterResult(false, title, message, PresenterMessageKind.Error);
        }
    }

    internal sealed record CarTypeDeletePreview(
        bool CanContinue,
        bool HasCars,
        string Title,
        string Message);

    internal sealed record CustomerDeletePreview(
        bool CanContinue,
        bool HasRentalHistory,
        string Title,
        string Message);
}
