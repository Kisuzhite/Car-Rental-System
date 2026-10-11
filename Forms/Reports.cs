using Car_Rental_System.Helpers;
using Car_Rental_System.Presenters;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;
using Guna.Charts.WinForms;
using System.Globalization;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace Car_Rental_System
{
    public partial class Reports : Form, IReportsView
    {
        private static readonly XNamespace SpreadsheetNamespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        private static readonly XNamespace PackageRelationshipNamespace = "http://schemas.openxmlformats.org/package/2006/relationships";
        private static readonly XNamespace OfficeRelationshipNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

        private readonly ReportsPresenter presenter;
        private List<ReportRentalHistoryRow> allReportRows = new();
        private List<ReportRentalHistoryRow> currentReportRows = new();

        public Reports()
        {
            InitializeComponent();
            presenter = new ReportsPresenter(this);
            ConfigureReportFilters();
        }

        public string SearchText => searchTextBox.Text;
        public DateOnly DateFrom => RentalCalculator.ToDateOnly(fromDatePicker.Value);
        public DateOnly DateTo => RentalCalculator.ToDateOnly(toDatePicker.Value);

        void IReportsView.BindReports(ReportsData reports)
        {
            totalCarsValueLabel.Text = reports.TotalBookings.ToString();
            availableCarsValueLabel.Text = reports.CompletedRentals.ToString();
            activeRentalsValueLabel.Text = reports.OverdueRentals.ToString();
            incomeValueLabel.Text = RentalCalculator.FormatCurrency(reports.TotalCollected);
            totalCarsTrendLabel.Text = reports.TotalBookingsTrend;
            availableCarsTrendLabel.Text = reports.CompletedRentalsTrend;
            activeRentalsTrendLabel.Text = reports.OverdueRentalsTrend;
            incomeTrendLabel.Text = reports.TotalCollectedTrend;
            historyLabel.Text = "Rental Transaction Report";
            revenueRangeLabel.Text = $"{DateFrom:MMM d, yyyy} - {DateTo:MMM d, yyyy}";

            BindRevenueChart(reports.MonthlyRevenue);

            allReportRows = reports.RentalHistory;
            BindReportRows();
        }

        private void ConfigureReportFilters()
        {
            fromDatePicker.Value = DateTime.Today.AddMonths(-5);
            toDatePicker.Value = DateTime.Today;
        }

        private void BindRevenueChart(List<DashboardRevenuePoint> monthlyRevenue)
        {
            revenueChart.Datasets.Clear();

            var revenueDataset = new GunaBarDataset
            {
                Label = "Collected Revenue",
                BorderWidth = 0,
                YFormat = "C0"
            };

            Color[] chartColors =
            {
                Color.FromArgb(37, 99, 235),
                Color.FromArgb(14, 165, 233),
                Color.FromArgb(16, 185, 129),
                Color.FromArgb(245, 158, 11),
                Color.FromArgb(99, 102, 241),
                Color.FromArgb(239, 68, 68)
            };

            for (int i = 0; i < monthlyRevenue.Count; i++)
            {
                DashboardRevenuePoint point = monthlyRevenue[i];
                revenueDataset.FillColors.Add(chartColors[i % chartColors.Length]);
                revenueDataset.DataPoints.Add(new LPoint
                {
                    Label = point.Month,
                    Y = (double)point.Amount
                });
            }

            revenueChart.Datasets.Add(revenueDataset);
            revenueChart.Update();
        }

        private void BindReportRows()
        {
            currentReportRows = FilterReportRows(allReportRows, SearchText);
            int totalRecords = currentReportRows.Count;
            reportGrid.DataSource = currentReportRows;

            DataGridViewHelper.ApplyFriendlyHeaders(reportGrid);
            DataGridViewHelper.HideColumn(reportGrid, "RentalId");
            DataGridViewHelper.ApplyStatusFormatting(reportGrid);
            DataGridViewHelper.ApplyFullTextColumn(reportGrid, "Car", 340);
            DataGridViewHelper.ApplyEmptyState(reportGrid, "No rental transactions found for this date range.");
            DataGridViewHelper.ClearSelectionWhenReady(reportGrid);

            recordsSummaryLabel.Text = $"Showing {totalRecords} record{(totalRecords == 1 ? "" : "s")}";
        }

        private static List<ReportRentalHistoryRow> FilterReportRows(
            List<ReportRentalHistoryRow> rows,
            string search)
        {
            string trimmed = search.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return rows;
            }

            return rows
                .Where(row => ReportRowMatchesSearch(row, trimmed))
                .ToList();
        }

        private static bool ReportRowMatchesSearch(ReportRentalHistoryRow row, string search)
        {
            return Contains(row.Customer, search) ||
                Contains(row.Car, search) ||
                Contains(row.CarType, search) ||
                Contains(row.PaymentMethod, search) ||
                Contains(row.PaymentStatus.ToString(), search) ||
                Contains(row.Status, search) ||
                Contains(row.RentalDate.ToString("d", CultureInfo.CurrentCulture), search) ||
                Contains(row.ExpectedReturn.ToString("d", CultureInfo.CurrentCulture), search) ||
                (row.ActualReturn.HasValue &&
                    Contains(row.ActualReturn.Value.ToString("d", CultureInfo.CurrentCulture), search));
        }

        private static bool Contains(string value, string search)
        {
            return value.Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        private void ExportExcelReport()
        {
            if (currentReportRows.Count == 0)
            {
                MessageBox.Show("There are no report records to export.", "Export Excel File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Title = "Export Excel File",
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = $"Rental-Transaction-Report-{DateTime.Now:yyyyMMdd-HHmm}.xlsx"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                CreateExcelWorkbook(dialog.FileName);
                MessageBox.Show("The Excel report was exported successfully.", "Export Excel File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to export the Excel report.\n\nDetails: {ex.Message}",
                    "Export Excel File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CreateExcelWorkbook(string fileName)
        {
            using var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);

            AddXmlPart(archive, "[Content_Types].xml", BuildContentTypes());
            AddXmlPart(archive, "_rels/.rels", BuildPackageRelationships());
            AddXmlPart(archive, "docProps/app.xml", BuildAppProperties());
            AddXmlPart(archive, "docProps/core.xml", BuildCoreProperties());
            AddXmlPart(archive, "xl/workbook.xml", BuildWorkbook());
            AddXmlPart(archive, "xl/_rels/workbook.xml.rels", BuildWorkbookRelationships());
            AddXmlPart(archive, "xl/styles.xml", BuildStyles());
            AddXmlPart(archive, "xl/worksheets/sheet1.xml", BuildWorksheet());
        }

        private XDocument BuildWorksheet()
        {
            string[] headers =
            [
                "Customer",
                "Car",
                "Car Type",
                "Rental Date",
                "Expected Return",
                "Actual Return",
                "Daily Rate",
                "Late Days",
                "Penalty",
                "Total Amount",
                "Amount Paid",
                "Balance",
                "Payment Status",
                "Payment Method",
                "Status"
            ];

            var sheetData = new XElement(SpreadsheetNamespace + "sheetData");
            sheetData.Add(BuildRow(1, [BuildTextCell(1, 1, "Rental Transaction Report", 3)]));
            sheetData.Add(BuildRow(2, [BuildTextCell(2, 1, $"{DateFrom:MMM d, yyyy} - {DateTo:MMM d, yyyy}", 4)]));
            sheetData.Add(new XElement(SpreadsheetNamespace + "row", new XAttribute("r", 3)));
            sheetData.Add(BuildRow(4, headers.Select((header, index) => BuildTextCell(4, index + 1, header, 1))));

            int rowIndex = 5;
            foreach (ReportRentalHistoryRow row in currentReportRows)
            {
                sheetData.Add(BuildRow(rowIndex,
                [
                    BuildTextCell(rowIndex, 1, row.Customer),
                    BuildTextCell(rowIndex, 2, row.Car),
                    BuildTextCell(rowIndex, 3, row.CarType),
                    BuildTextCell(rowIndex, 4, row.RentalDate.ToString("yyyy-MM-dd")),
                    BuildTextCell(rowIndex, 5, row.ExpectedReturn.ToString("yyyy-MM-dd")),
                    BuildTextCell(rowIndex, 6, row.ActualReturn?.ToString("yyyy-MM-dd") ?? ""),
                    BuildNumberCell(rowIndex, 7, row.DailyRate, 2),
                    BuildNumberCell(rowIndex, 8, row.LateDays),
                    BuildNumberCell(rowIndex, 9, row.Penalty, 2),
                    BuildNumberCell(rowIndex, 10, row.TotalAmount, 2),
                    BuildNumberCell(rowIndex, 11, row.AmountPaid, 2),
                    BuildNumberCell(rowIndex, 12, row.Balance, 2),
                    BuildTextCell(rowIndex, 13, row.PaymentStatus.ToString()),
                    BuildTextCell(rowIndex, 14, row.PaymentMethod),
                    BuildTextCell(rowIndex, 15, row.Status)
                ]));
                rowIndex++;
            }

            int lastRow = Math.Max(4, rowIndex - 1);
            return new XDocument(
                new XElement(SpreadsheetNamespace + "worksheet",
                    new XAttribute(XNamespace.Xmlns + "r", OfficeRelationshipNamespace.NamespaceName),
                    BuildSheetViews(),
                    BuildColumns(),
                    sheetData,
                    new XElement(SpreadsheetNamespace + "autoFilter", new XAttribute("ref", $"A4:O{lastRow}")),
                    new XElement(SpreadsheetNamespace + "mergeCells",
                        new XAttribute("count", 2),
                        new XElement(SpreadsheetNamespace + "mergeCell", new XAttribute("ref", "A1:O1")),
                        new XElement(SpreadsheetNamespace + "mergeCell", new XAttribute("ref", "A2:O2"))),
                    new XElement(SpreadsheetNamespace + "pageMargins",
                        new XAttribute("left", 0.7),
                        new XAttribute("right", 0.7),
                        new XAttribute("top", 0.75),
                        new XAttribute("bottom", 0.75),
                        new XAttribute("header", 0.3),
                        new XAttribute("footer", 0.3))));
        }

        private static XElement BuildSheetViews()
        {
            return new XElement(SpreadsheetNamespace + "sheetViews",
                new XElement(SpreadsheetNamespace + "sheetView",
                    new XAttribute("workbookViewId", 0),
                    new XElement(SpreadsheetNamespace + "pane",
                        new XAttribute("ySplit", 4),
                        new XAttribute("topLeftCell", "A5"),
                        new XAttribute("activePane", "bottomLeft"),
                        new XAttribute("state", "frozen"))));
        }

        private static XElement BuildColumns()
        {
            double[] widths = [24, 30, 16, 15, 17, 16, 14, 12, 14, 15, 15, 14, 17, 18, 14];
            return new XElement(SpreadsheetNamespace + "cols",
                widths.Select((width, index) =>
                    new XElement(SpreadsheetNamespace + "col",
                        new XAttribute("min", index + 1),
                        new XAttribute("max", index + 1),
                        new XAttribute("width", width.ToString(CultureInfo.InvariantCulture)),
                        new XAttribute("customWidth", 1))));
        }

        private static XElement BuildRow(int rowIndex, IEnumerable<XElement> cells)
        {
            return new XElement(SpreadsheetNamespace + "row",
                new XAttribute("r", rowIndex),
                cells);
        }

        private static XElement BuildTextCell(int rowIndex, int columnIndex, string value, int styleId = 0)
        {
            XElement cell = BuildCell(rowIndex, columnIndex, styleId);
            cell.Add(new XAttribute("t", "inlineStr"));
            cell.Add(new XElement(SpreadsheetNamespace + "is",
                new XElement(SpreadsheetNamespace + "t",
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    value)));
            return cell;
        }

        private static XElement BuildNumberCell(int rowIndex, int columnIndex, decimal value, int styleId = 0)
        {
            XElement cell = BuildCell(rowIndex, columnIndex, styleId);
            cell.Add(new XElement(SpreadsheetNamespace + "v", value.ToString(CultureInfo.InvariantCulture)));
            return cell;
        }

        private static XElement BuildNumberCell(int rowIndex, int columnIndex, int value)
        {
            XElement cell = BuildCell(rowIndex, columnIndex, 0);
            cell.Add(new XElement(SpreadsheetNamespace + "v", value.ToString(CultureInfo.InvariantCulture)));
            return cell;
        }

        private static XElement BuildCell(int rowIndex, int columnIndex, int styleId)
        {
            var cell = new XElement(SpreadsheetNamespace + "c",
                new XAttribute("r", GetCellReference(rowIndex, columnIndex)));

            if (styleId > 0)
            {
                cell.Add(new XAttribute("s", styleId));
            }

            return cell;
        }

        private static string GetCellReference(int rowIndex, int columnIndex)
        {
            return GetColumnName(columnIndex) + rowIndex.ToString(CultureInfo.InvariantCulture);
        }

        private static string GetColumnName(int columnIndex)
        {
            string columnName = "";
            while (columnIndex > 0)
            {
                int remainder = (columnIndex - 1) % 26;
                columnName = (char)('A' + remainder) + columnName;
                columnIndex = (columnIndex - 1) / 26;
            }

            return columnName;
        }

        private static XDocument BuildContentTypes()
        {
            XNamespace ns = "http://schemas.openxmlformats.org/package/2006/content-types";
            return new XDocument(
                new XElement(ns + "Types",
                    new XElement(ns + "Default",
                        new XAttribute("Extension", "rels"),
                        new XAttribute("ContentType", "application/vnd.openxmlformats-package.relationships+xml")),
                    new XElement(ns + "Default",
                        new XAttribute("Extension", "xml"),
                        new XAttribute("ContentType", "application/xml")),
                    new XElement(ns + "Override",
                        new XAttribute("PartName", "/xl/workbook.xml"),
                        new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml")),
                    new XElement(ns + "Override",
                        new XAttribute("PartName", "/xl/worksheets/sheet1.xml"),
                        new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml")),
                    new XElement(ns + "Override",
                        new XAttribute("PartName", "/xl/styles.xml"),
                        new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml")),
                    new XElement(ns + "Override",
                        new XAttribute("PartName", "/docProps/core.xml"),
                        new XAttribute("ContentType", "application/vnd.openxmlformats-package.core-properties+xml")),
                    new XElement(ns + "Override",
                        new XAttribute("PartName", "/docProps/app.xml"),
                        new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.extended-properties+xml"))));
        }

        private static XDocument BuildPackageRelationships()
        {
            return new XDocument(
                new XElement(PackageRelationshipNamespace + "Relationships",
                    new XElement(PackageRelationshipNamespace + "Relationship",
                        new XAttribute("Id", "rId1"),
                        new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"),
                        new XAttribute("Target", "xl/workbook.xml")),
                    new XElement(PackageRelationshipNamespace + "Relationship",
                        new XAttribute("Id", "rId2"),
                        new XAttribute("Type", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties"),
                        new XAttribute("Target", "docProps/core.xml")),
                    new XElement(PackageRelationshipNamespace + "Relationship",
                        new XAttribute("Id", "rId3"),
                        new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties"),
                        new XAttribute("Target", "docProps/app.xml"))));
        }

        private static XDocument BuildWorkbookRelationships()
        {
            return new XDocument(
                new XElement(PackageRelationshipNamespace + "Relationships",
                    new XElement(PackageRelationshipNamespace + "Relationship",
                        new XAttribute("Id", "rId1"),
                        new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"),
                        new XAttribute("Target", "worksheets/sheet1.xml")),
                    new XElement(PackageRelationshipNamespace + "Relationship",
                        new XAttribute("Id", "rId2"),
                        new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles"),
                        new XAttribute("Target", "styles.xml"))));
        }

        private static XDocument BuildWorkbook()
        {
            return new XDocument(
                new XElement(SpreadsheetNamespace + "workbook",
                    new XAttribute(XNamespace.Xmlns + "r", OfficeRelationshipNamespace.NamespaceName),
                    new XElement(SpreadsheetNamespace + "sheets",
                        new XElement(SpreadsheetNamespace + "sheet",
                            new XAttribute("name", "Rental Report"),
                            new XAttribute("sheetId", 1),
                            new XAttribute(OfficeRelationshipNamespace + "id", "rId1")))));
        }

        private static XDocument BuildStyles()
        {
            return new XDocument(
                new XElement(SpreadsheetNamespace + "styleSheet",
                    new XElement(SpreadsheetNamespace + "numFmts",
                        new XAttribute("count", 1),
                        new XElement(SpreadsheetNamespace + "numFmt",
                            new XAttribute("numFmtId", 164),
                            new XAttribute("formatCode", "\"\u20B1\"#,##0.00"))),
                    new XElement(SpreadsheetNamespace + "fonts",
                        new XAttribute("count", 4),
                        BuildFont("Inter", 11, "FF111827"),
                        BuildFont("Inter", 11, "FFFFFFFF", bold: true),
                        BuildFont("Inter", 16, "FF111827", bold: true),
                        BuildFont("Inter", 10, "FF4B5563")),
                    new XElement(SpreadsheetNamespace + "fills",
                        new XAttribute("count", 3),
                        new XElement(SpreadsheetNamespace + "fill",
                            new XElement(SpreadsheetNamespace + "patternFill", new XAttribute("patternType", "none"))),
                        new XElement(SpreadsheetNamespace + "fill",
                            new XElement(SpreadsheetNamespace + "patternFill", new XAttribute("patternType", "gray125"))),
                        new XElement(SpreadsheetNamespace + "fill",
                            new XElement(SpreadsheetNamespace + "patternFill",
                                new XAttribute("patternType", "solid"),
                                new XElement(SpreadsheetNamespace + "fgColor", new XAttribute("rgb", "FF1F2937")),
                                new XElement(SpreadsheetNamespace + "bgColor", new XAttribute("indexed", 64))))),
                    new XElement(SpreadsheetNamespace + "borders",
                        new XAttribute("count", 2),
                        new XElement(SpreadsheetNamespace + "border",
                            new XElement(SpreadsheetNamespace + "left"),
                            new XElement(SpreadsheetNamespace + "right"),
                            new XElement(SpreadsheetNamespace + "top"),
                            new XElement(SpreadsheetNamespace + "bottom"),
                            new XElement(SpreadsheetNamespace + "diagonal")),
                        new XElement(SpreadsheetNamespace + "border",
                            new XElement(SpreadsheetNamespace + "left", new XAttribute("style", "thin"), new XElement(SpreadsheetNamespace + "color", new XAttribute("rgb", "FFE5E7EB"))),
                            new XElement(SpreadsheetNamespace + "right", new XAttribute("style", "thin"), new XElement(SpreadsheetNamespace + "color", new XAttribute("rgb", "FFE5E7EB"))),
                            new XElement(SpreadsheetNamespace + "top", new XAttribute("style", "thin"), new XElement(SpreadsheetNamespace + "color", new XAttribute("rgb", "FFE5E7EB"))),
                            new XElement(SpreadsheetNamespace + "bottom", new XAttribute("style", "thin"), new XElement(SpreadsheetNamespace + "color", new XAttribute("rgb", "FFE5E7EB"))),
                            new XElement(SpreadsheetNamespace + "diagonal"))),
                    new XElement(SpreadsheetNamespace + "cellStyleXfs",
                        new XAttribute("count", 1),
                        new XElement(SpreadsheetNamespace + "xf",
                            new XAttribute("numFmtId", 0),
                            new XAttribute("fontId", 0),
                            new XAttribute("fillId", 0),
                            new XAttribute("borderId", 0))),
                    new XElement(SpreadsheetNamespace + "cellXfs",
                        new XAttribute("count", 5),
                        BuildCellFormat(),
                        BuildCellFormat(fontId: 1, fillId: 2, borderId: 1, applyFont: true, applyFill: true, applyBorder: true),
                        BuildCellFormat(numFmtId: 164, applyNumberFormat: true),
                        BuildCellFormat(fontId: 2, applyFont: true),
                        BuildCellFormat(fontId: 3, applyFont: true)),
                    new XElement(SpreadsheetNamespace + "cellStyles",
                        new XAttribute("count", 1),
                        new XElement(SpreadsheetNamespace + "cellStyle",
                            new XAttribute("name", "Normal"),
                            new XAttribute("xfId", 0),
                            new XAttribute("builtinId", 0)))));
        }

        private static XElement BuildFont(string fontName, int size, string color, bool bold = false)
        {
            var font = new XElement(SpreadsheetNamespace + "font",
                new XElement(SpreadsheetNamespace + "sz", new XAttribute("val", size)),
                new XElement(SpreadsheetNamespace + "color", new XAttribute("rgb", color)),
                new XElement(SpreadsheetNamespace + "name", new XAttribute("val", fontName)));

            if (bold)
            {
                font.AddFirst(new XElement(SpreadsheetNamespace + "b"));
            }

            return font;
        }

        private static XElement BuildCellFormat(
            int numFmtId = 0,
            int fontId = 0,
            int fillId = 0,
            int borderId = 0,
            bool applyNumberFormat = false,
            bool applyFont = false,
            bool applyFill = false,
            bool applyBorder = false)
        {
            var xf = new XElement(SpreadsheetNamespace + "xf",
                new XAttribute("numFmtId", numFmtId),
                new XAttribute("fontId", fontId),
                new XAttribute("fillId", fillId),
                new XAttribute("borderId", borderId),
                new XAttribute("xfId", 0));

            if (applyNumberFormat) xf.Add(new XAttribute("applyNumberFormat", 1));
            if (applyFont) xf.Add(new XAttribute("applyFont", 1));
            if (applyFill) xf.Add(new XAttribute("applyFill", 1));
            if (applyBorder) xf.Add(new XAttribute("applyBorder", 1));

            return xf;
        }

        private static XDocument BuildAppProperties()
        {
            XNamespace appNs = "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties";
            XNamespace vtNs = "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes";
            return new XDocument(
                new XElement(appNs + "Properties",
                    new XAttribute(XNamespace.Xmlns + "vt", vtNs.NamespaceName),
                    new XElement(appNs + "Application", "Auto Avenue Car Rental"),
                    new XElement(appNs + "DocSecurity", 0),
                    new XElement(appNs + "ScaleCrop", false),
                    new XElement(appNs + "HeadingPairs",
                        new XElement(vtNs + "vector",
                            new XAttribute("size", 2),
                            new XAttribute("baseType", "variant"),
                            new XElement(vtNs + "variant", new XElement(vtNs + "lpstr", "Worksheets")),
                            new XElement(vtNs + "variant", new XElement(vtNs + "i4", 1)))),
                    new XElement(appNs + "TitlesOfParts",
                        new XElement(vtNs + "vector",
                            new XAttribute("size", 1),
                            new XAttribute("baseType", "lpstr"),
                            new XElement(vtNs + "lpstr", "Rental Report")))));
        }

        private static XDocument BuildCoreProperties()
        {
            XNamespace cpNs = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";
            XNamespace dcNs = "http://purl.org/dc/elements/1.1/";
            XNamespace dctermsNs = "http://purl.org/dc/terms/";
            XNamespace xsiNs = "http://www.w3.org/2001/XMLSchema-instance";
            string created = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);

            return new XDocument(
                new XElement(cpNs + "coreProperties",
                    new XAttribute(XNamespace.Xmlns + "dc", dcNs.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "dcterms", dctermsNs.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "xsi", xsiNs.NamespaceName),
                    new XElement(dcNs + "creator", "Auto Avenue Car Rental"),
                    new XElement(cpNs + "lastModifiedBy", "Auto Avenue Car Rental"),
                    new XElement(dctermsNs + "created",
                        new XAttribute(xsiNs + "type", "dcterms:W3CDTF"),
                        created),
                    new XElement(dctermsNs + "modified",
                        new XAttribute(xsiNs + "type", "dcterms:W3CDTF"),
                        created)));
        }

        private static void AddXmlPart(ZipArchive archive, string path, XDocument document)
        {
            ZipArchiveEntry entry = archive.CreateEntry(path);
            using Stream stream = entry.Open();
            using var writer = XmlWriter.Create(stream, new XmlWriterSettings
            {
                Encoding = new System.Text.UTF8Encoding(false),
                Indent = false
            });
            document.Save(writer);
        }

        private async void Reports_Load(object? sender, EventArgs e)
        {
            await LoadReportsAsync();
        }

        private async void RefreshButton_Click(object? sender, EventArgs e)
        {
            await LoadReportsAsync();
        }

        private void ExcelButton_Click(object? sender, EventArgs e)
        {
            using var busy = UiBusyScope.Disable(excelButton, generateButton, refreshButton);
            ExportExcelReport();
        }

        private async void GenerateButton_Click(object? sender, EventArgs e)
        {
            await LoadReportsAsync();
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            BindReportRows();
        }

        private async Task LoadReportsAsync()
        {
            using var busy = UiBusyScope.Disable(generateButton, refreshButton, excelButton);

            try
            {
                await presenter.LoadReportsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }
    }
}

