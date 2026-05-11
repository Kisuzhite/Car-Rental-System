using System.Drawing.Printing;
using Car_Rental_System.Data;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Helpers
{
    internal static class RentalDocumentPrinter
    {
        private enum RentalDocumentKind
        {
            RentalAgreement,
            FinalReceipt
        }

        public static void PromptToPrintRentalAgreement(Form owner, int? rentalId)
        {
            PromptToPrint(owner, rentalId, RentalDocumentKind.RentalAgreement);
        }

        public static void PromptToPrintFinalReceipt(Form owner, int? rentalId)
        {
            PromptToPrint(owner, rentalId, RentalDocumentKind.FinalReceipt);
        }

        private static void PromptToPrint(Form owner, int? rentalId, RentalDocumentKind documentKind)
        {
            if (!rentalId.HasValue)
            {
                return;
            }

            string documentName = documentKind == RentalDocumentKind.RentalAgreement
                ? "rental agreement"
                : "final receipt";

            DialogResult result = MessageBox.Show(
                $"Preview and print the {documentName} now?",
                "Print Document",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using var db = new AppDbContext();
                Rental? rental = db.Rentals
                    .Include(r => r.Customer)
                    .Include(r => r.Car).ThenInclude(c => c.CarType)
                    .FirstOrDefault(r => r.RentalId == rentalId.Value);

                if (rental == null)
                {
                    MessageBox.Show("The rental record could not be found.", "Print Document", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PreviewDocument(owner, rental, documentKind);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to prepare the printable document.\n\nDetails: {ex.Message}",
                    "Print Document",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void PreviewDocument(Form owner, Rental rental, RentalDocumentKind documentKind)
        {
            using var document = new PrintDocument();
            document.DocumentName = documentKind == RentalDocumentKind.RentalAgreement
                ? $"Rental Agreement {rental.RentalId:D6}"
                : $"Final Receipt {rental.RentalId:D6}";
            document.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
            document.PrintPage += (_, e) =>
            {
                if (documentKind == RentalDocumentKind.RentalAgreement)
                {
                    DrawRentalAgreement(e, rental);
                }
                else
                {
                    DrawFinalReceipt(e, rental);
                }

                e.HasMorePages = false;
            };

            using var preview = new PrintPreviewDialog
            {
                Document = document,
                Width = 1000,
                Height = 800,
                StartPosition = FormStartPosition.CenterParent
            };

            preview.ShowDialog(owner);
        }

        private static void DrawRentalAgreement(PrintPageEventArgs e, Rental rental)
        {
            if (e.Graphics == null)
            {
                return;
            }

            DocumentPainter painter = new(e.Graphics, e.MarginBounds, AppConfiguration.GetBusinessProfile());
            painter.DrawHeader("RENTAL AGREEMENT", $"Agreement No. RA-{rental.RentalId:D6}");

            painter.DrawSectionTitle("Customer Details");
            painter.DrawLabelValue("Customer", rental.Customer.FullName);
            painter.DrawLabelValue("Contact Number", rental.Customer.ContactNumber);
            painter.DrawLabelValue("License Number", rental.Customer.LicenseNumber);
            painter.DrawLabelValue("Address", rental.Customer.Address);

            painter.DrawSectionTitle("Vehicle Details");
            painter.DrawLabelValue("Plate Number", rental.Car.PlateNumber);
            painter.DrawLabelValue("Vehicle", $"{rental.Car.Year} {rental.Car.Brand} {rental.Car.Model}");
            painter.DrawLabelValue("Car Type", rental.Car.CarType?.TypeName ?? AppConstants.NotProvided);
            painter.DrawLabelValue("Color", rental.Car.Color);

            painter.DrawSectionTitle("Rental Terms");
            painter.DrawLabelValue("Rental Date", FormatDate(rental.RentalDate));
            painter.DrawLabelValue("Expected Return", FormatDate(rental.ExpectedReturnDate));
            painter.DrawLabelValue("Daily Rate", RentalCalculator.FormatCurrency(rental.DailyRate));
            painter.DrawLabelValue("Estimated Total", RentalCalculator.FormatCurrency(rental.TotalAmount));
            painter.DrawLabelValue("Amount Paid", RentalCalculator.FormatCurrency(rental.AmountPaid));
            painter.DrawLabelValue("Balance", RentalCalculator.FormatCurrency(rental.Balance));
            painter.DrawLabelValue("Payment Method", PaymentHelper.FormatPaymentMethod(rental.PaymentMethod));
            painter.DrawLabelValue("Payment Status", rental.PaymentStatus.ToString());

            painter.DrawSectionTitle("Agreement Notes");
            painter.DrawParagraph("1. The customer agrees to return the vehicle on or before the expected return date.");
            painter.DrawParagraph("2. Late returns may add one daily rate for each late day.");
            painter.DrawParagraph("3. Fuel, traffic violations, damage, and missing items are the customer's responsibility.");
            painter.DrawParagraph("4. Any remaining balance must be settled according to the owner's payment terms.");

            painter.DrawSignatures("Customer Signature", "Owner / Authorized Signature");
        }

        private static void DrawFinalReceipt(PrintPageEventArgs e, Rental rental)
        {
            if (e.Graphics == null)
            {
                return;
            }

            DocumentPainter painter = new(e.Graphics, e.MarginBounds, AppConfiguration.GetBusinessProfile());
            painter.DrawHeader("FINAL RECEIPT", $"Receipt No. FR-{rental.RentalId:D6}");

            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);
            decimal baseAmount = RentalCalculator.CalculateBaseAmount(
                rental.RentalDate.ToDateTime(TimeOnly.MinValue),
                rental.ExpectedReturnDate.ToDateTime(TimeOnly.MinValue),
                rental.DailyRate);
            decimal latePenalty = Math.Max(0, rental.TotalAmount - baseAmount);
            int lateDays = rental.ActualReturnDate.HasValue
                ? Math.Max(0, rental.ActualReturnDate.Value.DayNumber - rental.ExpectedReturnDate.DayNumber)
                : 0;

            painter.DrawSectionTitle("Receipt Details");
            painter.DrawLabelValue("Date Issued", FormatDate(today));
            painter.DrawLabelValue("Customer", rental.Customer.FullName);
            painter.DrawLabelValue("Contact Number", rental.Customer.ContactNumber);
            painter.DrawLabelValue("License Number", rental.Customer.LicenseNumber);
            painter.DrawLabelValue("Plate Number", rental.Car.PlateNumber);
            painter.DrawLabelValue("Vehicle", $"{rental.Car.Brand} {rental.Car.Model}");
            painter.DrawLabelValue("Car Type", rental.Car.CarType?.TypeName ?? AppConstants.NotProvided);
            painter.DrawLabelValue("Rental Date", FormatDate(rental.RentalDate));
            painter.DrawLabelValue("Expected Return", FormatDate(rental.ExpectedReturnDate));
            painter.DrawLabelValue("Actual Return", rental.ActualReturnDate.HasValue ? FormatDate(rental.ActualReturnDate.Value) : AppConstants.NotProvided);

            painter.DrawSectionTitle("Charges");
            painter.DrawLabelValue("Daily Rate", RentalCalculator.FormatCurrency(rental.DailyRate));
            painter.DrawLabelValue("Base Amount", RentalCalculator.FormatCurrency(baseAmount));
            painter.DrawLabelValue("Late Days", lateDays.ToString());
            painter.DrawLabelValue("Late Penalty", RentalCalculator.FormatCurrency(latePenalty));
            painter.DrawDivider();
            painter.DrawLabelValue("Final Amount", RentalCalculator.FormatCurrency(rental.TotalAmount), emphasize: true);

            painter.DrawSectionTitle("Payment");
            painter.DrawLabelValue("Amount Paid", RentalCalculator.FormatCurrency(rental.AmountPaid));
            painter.DrawLabelValue("Balance", RentalCalculator.FormatCurrency(rental.Balance), emphasize: rental.Balance > 0);
            painter.DrawLabelValue("Payment Method", PaymentHelper.FormatPaymentMethod(rental.PaymentMethod));
            painter.DrawLabelValue("Payment Status", rental.PaymentStatus.ToString());

            painter.DrawParagraph("Thank you for choosing Auto Avenue Car Rental.");
            painter.DrawSignatures("Received By", "Customer Signature");
        }

        private static string FormatDate(DateOnly date)
        {
            return date.ToString("MMMM d, yyyy");
        }

        private sealed class DocumentPainter
        {
            private readonly Graphics graphics;
            private readonly Rectangle bounds;
            private readonly BusinessProfile businessProfile;
            private readonly Font brandFont = new("Inter", 14, FontStyle.Bold);
            private readonly Font titleFont = new("Inter", 18, FontStyle.Bold);
            private readonly Font sectionFont = new("Inter", 10, FontStyle.Bold);
            private readonly Font bodyFont = new("Inter", 9);
            private readonly Font bodyBoldFont = new("Inter", 9, FontStyle.Bold);
            private readonly Brush darkBrush = new SolidBrush(Color.FromArgb(17, 24, 39));
            private readonly Brush mutedBrush = new SolidBrush(Color.FromArgb(75, 85, 99));
            private readonly Pen dividerPen = new(Color.FromArgb(209, 213, 219));
            private float y;

            public DocumentPainter(Graphics graphics, Rectangle bounds, BusinessProfile businessProfile)
            {
                this.graphics = graphics;
                this.bounds = bounds;
                this.businessProfile = businessProfile;
                y = bounds.Top;
            }

            public void DrawHeader(string title, string documentNumber)
            {
                RectangleF headerBounds = new(bounds.Left, y, bounds.Width - 190, 25);
                graphics.DrawString(businessProfile.BusinessName, brandFont, darkBrush, headerBounds);
                graphics.DrawString(DateTime.Today.ToString("MMMM d, yyyy"), bodyFont, mutedBrush, bounds.Right - 150, y + 4);
                y += 26;

                DrawBusinessProfileLine("Owner/Operator", businessProfile.OwnerName);
                DrawBusinessProfileLine("Location", businessProfile.Location);
                DrawBusinessProfileLine("Contact", businessProfile.ContactNumber);

                if (!string.IsNullOrWhiteSpace(businessProfile.FacebookPage))
                {
                    DrawBusinessProfileLine("Facebook", businessProfile.FacebookPage);
                }

                y += 8;

                graphics.DrawString(title, titleFont, darkBrush, bounds.Left, y);
                graphics.DrawString(documentNumber, bodyBoldFont, darkBrush, bounds.Right - 170, y + 8);
                y += 42;
                DrawDivider();
                y += 8;
            }

            private void DrawBusinessProfileLine(string label, string value)
            {
                RectangleF lineBounds = new(bounds.Left, y, bounds.Width - 190, 18);
                graphics.DrawString($"{label}: {value}", bodyFont, mutedBrush, lineBounds);
                y += 18;
            }

            public void DrawSectionTitle(string title)
            {
                y += 10;
                graphics.DrawString(title, sectionFont, darkBrush, bounds.Left, y);
                y += 24;
            }

            public void DrawLabelValue(string label, string value, bool emphasize = false)
            {
                graphics.DrawString(label, bodyBoldFont, mutedBrush, bounds.Left, y);
                graphics.DrawString(value, emphasize ? bodyBoldFont : bodyFont, darkBrush, bounds.Left + 150, y);
                y += 22;
            }

            public void DrawParagraph(string text)
            {
                RectangleF textBounds = new(bounds.Left, y, bounds.Width, 38);
                graphics.DrawString(text, bodyFont, darkBrush, textBounds);
                y += 30;
            }

            public void DrawDivider()
            {
                graphics.DrawLine(dividerPen, bounds.Left, y, bounds.Right, y);
                y += 8;
            }

            public void DrawSignatures(string leftLabel, string rightLabel)
            {
                y = Math.Max(y + 35, bounds.Bottom - 95);
                int signatureWidth = (bounds.Width - 80) / 2;
                int rightX = bounds.Left + signatureWidth + 80;

                graphics.DrawLine(dividerPen, bounds.Left, y, bounds.Left + signatureWidth, y);
                graphics.DrawLine(dividerPen, rightX, y, rightX + signatureWidth, y);
                y += 8;
                graphics.DrawString(leftLabel, bodyFont, mutedBrush, bounds.Left, y);
                graphics.DrawString(rightLabel, bodyFont, mutedBrush, rightX, y);
            }
        }
    }
}
