using Car_Rental_System.Models;

namespace Car_Rental_System.Helpers
{
    internal static class PaymentHelper
    {
        public static PaymentMethod[] GetSelectablePaymentMethods()
        {
            return Enum.GetValues<PaymentMethod>()
                .Where(method => method is not PaymentMethod.Card and not PaymentMethod.Other)
                .ToArray();
        }

        public static decimal GetBalance(decimal totalAmount, decimal amountPaid)
        {
            return Math.Max(0, totalAmount - amountPaid);
        }

        public static decimal GetMinimumDepositAmount(decimal totalAmount)
        {
            if (totalAmount <= 0)
            {
                return 0;
            }

            return Math.Round(
                totalAmount * AppConstants.MinimumDepositRate,
                AppConstants.MoneyScale,
                MidpointRounding.AwayFromZero);
        }

        public static PaymentStatus GetPaymentStatus(decimal totalAmount, decimal amountPaid)
        {
            if (amountPaid <= 0)
            {
                return PaymentStatus.Unpaid;
            }

            return amountPaid >= totalAmount
                ? PaymentStatus.Paid
                : PaymentStatus.Partial;
        }

        public static bool IsValidPaymentAmount(decimal amountPaid, decimal totalAmount)
        {
            return amountPaid >= 0 && amountPaid <= totalAmount;
        }

        public static bool IsValidInitialPaymentAmount(decimal amountPaid, decimal totalAmount)
        {
            return amountPaid >= GetMinimumDepositAmount(totalAmount) &&
                amountPaid <= totalAmount;
        }

        public static string FormatPaymentMethod(PaymentMethod paymentMethod)
        {
            return paymentMethod == PaymentMethod.BankTransfer
                ? "Bank Transfer"
                : paymentMethod.ToString();
        }
    }
}
