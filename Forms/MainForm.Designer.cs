namespace Car_Rental_System
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            sidebarPanel = new Panel();
            logoutButton = new Button();
            reportsButton = new Button();
            returnsButton = new Button();
            reservationsButton = new Button();
            rentalsButton = new Button();
            customersButton = new Button();
            ratesButton = new Button();
            carsButton = new Button();
            dashboardButton = new Button();
            sidebarLogoPictureBox = new PictureBox();
            appTitleLabel = new Label();
            topPanel = new Panel();
            staffBadgeLabel = new Label();
            dateLabel = new Label();
            pageTitleLabel = new Label();
            contentPanel = new Panel();
            recentPanel = new Panel();
            recentRentalsGrid = new DataGridView();
            recentTitleLabel = new Label();
            mainTable = new TableLayoutPanel();
            quickRentalPanel = new Panel();
            clearButton = new Button();
            createRentalButton = new Button();
            paymentMethodComboBox = new ComboBox();
            paymentMethodLabel = new Label();
            paymentAmountTextBox = new TextBox();
            paymentAmountLabel = new Label();
            totalAmountTextBox = new TextBox();
            totalAmountLabel = new Label();
            dailyRateTextBox = new TextBox();
            dailyRateLabel = new Label();
            returnDatePicker = new DateTimePicker();
            returnDateLabel = new Label();
            rentalDatePicker = new DateTimePicker();
            rentalDateLabel = new Label();
            selectedCarTextBox = new TextBox();
            selectedCarLabel = new Label();
            customerNameTextBox = new TextBox();
            customerNameLabel = new Label();
            quickRentalTitleLabel = new Label();
            availabilityPanel = new Panel();
            carsGrid = new DataGridView();
            availabilityHeaderPanel = new Panel();
            searchTextBox = new TextBox();
            availableCarsTitleLabel = new Label();
            statsTable = new TableLayoutPanel();
            totalCarsCard = new Panel();
            totalCarsValueLabel = new Label();
            totalCarsTextLabel = new Label();
            availableCarsCard = new Panel();
            availableCarsValueLabel = new Label();
            availableCarsTextLabel = new Label();
            rentedCarsCard = new Panel();
            rentedCarsValueLabel = new Label();
            rentedCarsTextLabel = new Label();
            incomeCard = new Panel();
            incomeValueLabel = new Label();
            incomeTextLabel = new Label();
            sidebarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sidebarLogoPictureBox).BeginInit();
            topPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            recentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)recentRentalsGrid).BeginInit();
            mainTable.SuspendLayout();
            quickRentalPanel.SuspendLayout();
            availabilityPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)carsGrid).BeginInit();
            availabilityHeaderPanel.SuspendLayout();
            statsTable.SuspendLayout();
            totalCarsCard.SuspendLayout();
            availableCarsCard.SuspendLayout();
            rentedCarsCard.SuspendLayout();
            incomeCard.SuspendLayout();
            SuspendLayout();
            // 
            // sidebarPanel
            // 
            sidebarPanel.BackColor = Color.FromArgb(31, 41, 55);
            sidebarPanel.Controls.Add(logoutButton);
            sidebarPanel.Controls.Add(reportsButton);
            sidebarPanel.Controls.Add(returnsButton);
            sidebarPanel.Controls.Add(reservationsButton);
            sidebarPanel.Controls.Add(rentalsButton);
            sidebarPanel.Controls.Add(customersButton);
            sidebarPanel.Controls.Add(ratesButton);
            sidebarPanel.Controls.Add(carsButton);
            sidebarPanel.Controls.Add(dashboardButton);
            sidebarPanel.Controls.Add(sidebarLogoPictureBox);
            sidebarPanel.Controls.Add(appTitleLabel);
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Location = new Point(0, 0);
            sidebarPanel.Margin = new Padding(3, 4, 3, 4);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Size = new Size(274, 960);
            sidebarPanel.TabIndex = 0;
            // 
            // logoutButton
            // 
            logoutButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            logoutButton.BackColor = Color.FromArgb(245, 158, 11);
            logoutButton.FlatAppearance.BorderColor = Color.FromArgb(245, 158, 11);
            logoutButton.FlatAppearance.BorderSize = 0;
            logoutButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 158, 11);
            logoutButton.FlatStyle = FlatStyle.Flat;
            logoutButton.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            logoutButton.ForeColor = Color.FromArgb(31, 41, 55);
            logoutButton.Image = (Image)resources.GetObject("logoutButton.Image");
            logoutButton.ImageAlign = ContentAlignment.MiddleLeft;
            logoutButton.Location = new Point(23, 872);
            logoutButton.Margin = new Padding(3, 4, 3, 4);
            logoutButton.Name = "logoutButton";
            logoutButton.Padding = new Padding(16, 0, 0, 0);
            logoutButton.Size = new Size(229, 56);
            logoutButton.TabIndex = 8;
            logoutButton.Text = "Logout";
            logoutButton.TextAlign = ContentAlignment.MiddleLeft;
            logoutButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            logoutButton.UseVisualStyleBackColor = false;
            logoutButton.Click += LogoutButton_Click;
            // 
            // reportsButton
            // 
            reportsButton.BackColor = Color.FromArgb(31, 41, 55);
            reportsButton.FlatAppearance.BorderSize = 0;
            reportsButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            reportsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            reportsButton.FlatStyle = FlatStyle.Flat;
            reportsButton.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            reportsButton.ForeColor = Color.White;
            reportsButton.Image = (Image)resources.GetObject("reportsButton.Image");
            reportsButton.ImageAlign = ContentAlignment.MiddleLeft;
            reportsButton.Location = new Point(23, 625);
            reportsButton.Margin = new Padding(3, 4, 3, 4);
            reportsButton.Name = "reportsButton";
            reportsButton.Padding = new Padding(16, 0, 0, 0);
            reportsButton.Size = new Size(229, 56);
            reportsButton.TabIndex = 9;
            reportsButton.Text = "Reports";
            reportsButton.TextAlign = ContentAlignment.MiddleLeft;
            reportsButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            reportsButton.UseVisualStyleBackColor = false;
            reportsButton.Click += NavigationButton_Click;
            // 
            // returnsButton
            // 
            returnsButton.BackColor = Color.FromArgb(31, 41, 55);
            returnsButton.FlatAppearance.BorderSize = 0;
            returnsButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            returnsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            returnsButton.FlatStyle = FlatStyle.Flat;
            returnsButton.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            returnsButton.ForeColor = Color.White;
            returnsButton.Image = (Image)resources.GetObject("returnsButton.Image");
            returnsButton.ImageAlign = ContentAlignment.MiddleLeft;
            returnsButton.Location = new Point(23, 559);
            returnsButton.Margin = new Padding(3, 4, 3, 4);
            returnsButton.Name = "returnsButton";
            returnsButton.Padding = new Padding(16, 0, 0, 0);
            returnsButton.Size = new Size(229, 56);
            returnsButton.TabIndex = 8;
            returnsButton.Text = "Return Car";
            returnsButton.TextAlign = ContentAlignment.MiddleLeft;
            returnsButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            returnsButton.UseVisualStyleBackColor = false;
            returnsButton.Click += NavigationButton_Click;
            // 
            // reservationsButton
            // 
            reservationsButton.BackColor = Color.FromArgb(31, 41, 55);
            reservationsButton.FlatAppearance.BorderSize = 0;
            reservationsButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            reservationsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            reservationsButton.FlatStyle = FlatStyle.Flat;
            reservationsButton.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            reservationsButton.ForeColor = Color.White;
            reservationsButton.Image = (Image)resources.GetObject("reservationsButton.Image");
            reservationsButton.ImageAlign = ContentAlignment.MiddleLeft;
            reservationsButton.Location = new Point(23, 493);
            reservationsButton.Margin = new Padding(3, 4, 3, 4);
            reservationsButton.Name = "reservationsButton";
            reservationsButton.Padding = new Padding(16, 0, 0, 0);
            reservationsButton.Size = new Size(229, 56);
            reservationsButton.TabIndex = 7;
            reservationsButton.Text = "Reservations";
            reservationsButton.TextAlign = ContentAlignment.MiddleLeft;
            reservationsButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            reservationsButton.UseVisualStyleBackColor = false;
            reservationsButton.Click += NavigationButton_Click;
            // 
            // rentalsButton
            // 
            rentalsButton.BackColor = Color.FromArgb(31, 41, 55);
            rentalsButton.FlatAppearance.BorderSize = 0;
            rentalsButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            rentalsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            rentalsButton.FlatStyle = FlatStyle.Flat;
            rentalsButton.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rentalsButton.ForeColor = Color.White;
            rentalsButton.Image = (Image)resources.GetObject("rentalsButton.Image");
            rentalsButton.ImageAlign = ContentAlignment.MiddleLeft;
            rentalsButton.Location = new Point(23, 427);
            rentalsButton.Margin = new Padding(3, 4, 3, 4);
            rentalsButton.Name = "rentalsButton";
            rentalsButton.Padding = new Padding(16, 0, 0, 0);
            rentalsButton.Size = new Size(229, 56);
            rentalsButton.TabIndex = 6;
            rentalsButton.Text = "Rentals";
            rentalsButton.TextAlign = ContentAlignment.MiddleLeft;
            rentalsButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            rentalsButton.UseVisualStyleBackColor = false;
            rentalsButton.Click += NavigationButton_Click;
            // 
            // customersButton
            // 
            customersButton.BackColor = Color.FromArgb(31, 41, 55);
            customersButton.FlatAppearance.BorderSize = 0;
            customersButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            customersButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            customersButton.FlatStyle = FlatStyle.Flat;
            customersButton.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            customersButton.ForeColor = Color.White;
            customersButton.Image = (Image)resources.GetObject("customersButton.Image");
            customersButton.ImageAlign = ContentAlignment.MiddleLeft;
            customersButton.Location = new Point(23, 360);
            customersButton.Margin = new Padding(3, 4, 3, 4);
            customersButton.Name = "customersButton";
            customersButton.Padding = new Padding(16, 0, 0, 0);
            customersButton.Size = new Size(229, 56);
            customersButton.TabIndex = 5;
            customersButton.Text = "Customers";
            customersButton.TextAlign = ContentAlignment.MiddleLeft;
            customersButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            customersButton.UseVisualStyleBackColor = false;
            customersButton.Click += NavigationButton_Click;
            // 
            // ratesButton
            // 
            ratesButton.BackColor = Color.FromArgb(31, 41, 55);
            ratesButton.FlatAppearance.BorderSize = 0;
            ratesButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            ratesButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            ratesButton.FlatStyle = FlatStyle.Flat;
            ratesButton.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ratesButton.ForeColor = Color.White;
            ratesButton.Image = (Image)resources.GetObject("ratesButton.Image");
            ratesButton.ImageAlign = ContentAlignment.MiddleLeft;
            ratesButton.Location = new Point(23, 293);
            ratesButton.Margin = new Padding(3, 4, 3, 4);
            ratesButton.Name = "ratesButton";
            ratesButton.Padding = new Padding(16, 0, 0, 0);
            ratesButton.Size = new Size(229, 56);
            ratesButton.TabIndex = 4;
            ratesButton.Text = "Car Rates";
            ratesButton.TextAlign = ContentAlignment.MiddleLeft;
            ratesButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            ratesButton.UseVisualStyleBackColor = false;
            ratesButton.Click += NavigationButton_Click;
            // 
            // carsButton
            // 
            carsButton.BackColor = Color.FromArgb(31, 41, 55);
            carsButton.FlatAppearance.BorderSize = 0;
            carsButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            carsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            carsButton.FlatStyle = FlatStyle.Flat;
            carsButton.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            carsButton.ForeColor = Color.White;
            carsButton.Image = (Image)resources.GetObject("carsButton.Image");
            carsButton.ImageAlign = ContentAlignment.MiddleLeft;
            carsButton.Location = new Point(23, 227);
            carsButton.Margin = new Padding(3, 4, 3, 4);
            carsButton.Name = "carsButton";
            carsButton.Padding = new Padding(16, 0, 0, 0);
            carsButton.Size = new Size(229, 56);
            carsButton.TabIndex = 3;
            carsButton.Text = "Cars";
            carsButton.TextAlign = ContentAlignment.MiddleLeft;
            carsButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            carsButton.UseVisualStyleBackColor = false;
            carsButton.Click += NavigationButton_Click;
            // 
            // dashboardButton
            // 
            dashboardButton.BackColor = Color.FromArgb(37, 99, 235);
            dashboardButton.FlatAppearance.BorderColor = Color.White;
            dashboardButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            dashboardButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            dashboardButton.FlatStyle = FlatStyle.Flat;
            dashboardButton.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dashboardButton.ForeColor = Color.White;
            dashboardButton.Image = (Image)resources.GetObject("dashboardButton.Image");
            dashboardButton.ImageAlign = ContentAlignment.MiddleLeft;
            dashboardButton.Location = new Point(23, 160);
            dashboardButton.Margin = new Padding(3, 4, 3, 4);
            dashboardButton.Name = "dashboardButton";
            dashboardButton.Padding = new Padding(16, 0, 0, 0);
            dashboardButton.Size = new Size(229, 56);
            dashboardButton.TabIndex = 2;
            dashboardButton.Text = "Dashboard";
            dashboardButton.TextAlign = ContentAlignment.MiddleLeft;
            dashboardButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            dashboardButton.UseVisualStyleBackColor = false;
            dashboardButton.Click += NavigationButton_Click;
            // 
            // sidebarLogoPictureBox
            // 
            sidebarLogoPictureBox.Image = (Image)resources.GetObject("sidebarLogoPictureBox.Image");
            sidebarLogoPictureBox.Location = new Point(23, 20);
            sidebarLogoPictureBox.Name = "sidebarLogoPictureBox";
            sidebarLogoPictureBox.Size = new Size(229, 96);
            sidebarLogoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            sidebarLogoPictureBox.TabIndex = 9;
            sidebarLogoPictureBox.TabStop = false;
            // 
            // appTitleLabel
            // 
            appTitleLabel.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            appTitleLabel.ForeColor = Color.FromArgb(245, 158, 11);
            appTitleLabel.Location = new Point(23, 122);
            appTitleLabel.Name = "appTitleLabel";
            appTitleLabel.Size = new Size(229, 28);
            appTitleLabel.TabIndex = 0;
            appTitleLabel.Text = "Owner Portal";
            appTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.White;
            topPanel.Controls.Add(staffBadgeLabel);
            topPanel.Controls.Add(dateLabel);
            topPanel.Controls.Add(pageTitleLabel);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(274, 0);
            topPanel.Margin = new Padding(3, 4, 3, 4);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1097, 112);
            topPanel.TabIndex = 1;
            // 
            // staffBadgeLabel
            // 
            staffBadgeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            staffBadgeLabel.BackColor = Color.FromArgb(245, 158, 11);
            staffBadgeLabel.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            staffBadgeLabel.ForeColor = Color.FromArgb(31, 41, 55);
            staffBadgeLabel.Location = new Point(903, 24);
            staffBadgeLabel.Name = "staffBadgeLabel";
            staffBadgeLabel.Size = new Size(160, 40);
            staffBadgeLabel.TabIndex = 2;
            staffBadgeLabel.Text = "Owner Account";
            staffBadgeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dateLabel
            // 
            dateLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dateLabel.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            dateLabel.Location = new Point(677, 27);
            dateLabel.Name = "dateLabel";
            dateLabel.Size = new Size(206, 32);
            dateLabel.TabIndex = 1;
            dateLabel.Text = "May 2, 2026";
            dateLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pageTitleLabel
            // 
            pageTitleLabel.AutoSize = true;
            pageTitleLabel.Font = new Font("Inter", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pageTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            pageTitleLabel.Location = new Point(32, 29);
            pageTitleLabel.Name = "pageTitleLabel";
            pageTitleLabel.Size = new Size(203, 49);
            pageTitleLabel.TabIndex = 0;
            pageTitleLabel.Text = "Dashboard";
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.FromArgb(243, 244, 246);
            contentPanel.Controls.Add(recentPanel);
            contentPanel.Controls.Add(mainTable);
            contentPanel.Controls.Add(statsTable);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(274, 112);
            contentPanel.Margin = new Padding(3, 4, 3, 4);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(32, 37, 32, 37);
            contentPanel.Size = new Size(1097, 848);
            contentPanel.TabIndex = 2;
            // 
            // recentPanel
            // 
            recentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            recentPanel.BackColor = Color.White;
            recentPanel.BorderStyle = BorderStyle.FixedSingle;
            recentPanel.Controls.Add(recentRentalsGrid);
            recentPanel.Controls.Add(recentTitleLabel);
            recentPanel.Location = new Point(32, 615);
            recentPanel.Margin = new Padding(3, 4, 3, 4);
            recentPanel.Name = "recentPanel";
            recentPanel.Padding = new Padding(21, 19, 21, 24);
            recentPanel.Size = new Size(1033, 195);
            recentPanel.TabIndex = 2;
            // 
            // recentRentalsGrid
            // 
            recentRentalsGrid.AllowUserToAddRows = false;
            recentRentalsGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 251);
            recentRentalsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            recentRentalsGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            recentRentalsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            recentRentalsGrid.BackgroundColor = Color.White;
            recentRentalsGrid.BorderStyle = BorderStyle.None;
            recentRentalsGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            recentRentalsGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            recentRentalsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            recentRentalsGrid.ColumnHeadersHeight = 56;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            recentRentalsGrid.DefaultCellStyle = dataGridViewCellStyle3;
            recentRentalsGrid.EnableHeadersVisualStyles = false;
            recentRentalsGrid.GridColor = Color.FromArgb(229, 231, 235);
            recentRentalsGrid.Location = new Point(21, 64);
            recentRentalsGrid.Margin = new Padding(3, 4, 3, 4);
            recentRentalsGrid.Name = "recentRentalsGrid";
            recentRentalsGrid.ReadOnly = true;
            recentRentalsGrid.RowHeadersVisible = false;
            recentRentalsGrid.RowHeadersWidth = 51;
            recentRentalsGrid.RowTemplate.Height = 42;
            recentRentalsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            recentRentalsGrid.Size = new Size(988, 103);
            recentRentalsGrid.TabIndex = 1;
            // 
            // recentTitleLabel
            // 
            recentTitleLabel.AutoSize = true;
            recentTitleLabel.Font = new Font("Inter SemiBold", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recentTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            recentTitleLabel.Location = new Point(21, 19);
            recentTitleLabel.Name = "recentTitleLabel";
            recentTitleLabel.Size = new Size(181, 32);
            recentTitleLabel.TabIndex = 0;
            recentTitleLabel.Text = "Recent Rentals";
            // 
            // mainTable
            // 
            mainTable.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            mainTable.ColumnCount = 2;
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 366F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTable.Controls.Add(quickRentalPanel, 0, 0);
            mainTable.Controls.Add(availabilityPanel, 1, 0);
            mainTable.Location = new Point(32, 152);
            mainTable.Margin = new Padding(3, 4, 3, 4);
            mainTable.Name = "mainTable";
            mainTable.RowCount = 1;
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTable.Size = new Size(1033, 445);
            mainTable.TabIndex = 1;
            // 
            // quickRentalPanel
            // 
            quickRentalPanel.BackColor = Color.White;
            quickRentalPanel.BorderStyle = BorderStyle.FixedSingle;
            quickRentalPanel.Controls.Add(clearButton);
            quickRentalPanel.Controls.Add(createRentalButton);
            quickRentalPanel.Controls.Add(paymentMethodComboBox);
            quickRentalPanel.Controls.Add(paymentMethodLabel);
            quickRentalPanel.Controls.Add(paymentAmountTextBox);
            quickRentalPanel.Controls.Add(paymentAmountLabel);
            quickRentalPanel.Controls.Add(totalAmountTextBox);
            quickRentalPanel.Controls.Add(totalAmountLabel);
            quickRentalPanel.Controls.Add(dailyRateTextBox);
            quickRentalPanel.Controls.Add(dailyRateLabel);
            quickRentalPanel.Controls.Add(returnDatePicker);
            quickRentalPanel.Controls.Add(returnDateLabel);
            quickRentalPanel.Controls.Add(rentalDatePicker);
            quickRentalPanel.Controls.Add(rentalDateLabel);
            quickRentalPanel.Controls.Add(selectedCarTextBox);
            quickRentalPanel.Controls.Add(selectedCarLabel);
            quickRentalPanel.Controls.Add(customerNameTextBox);
            quickRentalPanel.Controls.Add(customerNameLabel);
            quickRentalPanel.Controls.Add(quickRentalTitleLabel);
            quickRentalPanel.Dock = DockStyle.Fill;
            quickRentalPanel.Location = new Point(0, 0);
            quickRentalPanel.Margin = new Padding(0, 0, 21, 0);
            quickRentalPanel.Name = "quickRentalPanel";
            quickRentalPanel.Size = new Size(345, 445);
            quickRentalPanel.TabIndex = 0;
            // 
            // clearButton
            // 
            clearButton.BackColor = Color.White;
            clearButton.FlatAppearance.BorderColor = Color.FromArgb(31, 41, 55);
            clearButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 244, 246);
            clearButton.FlatStyle = FlatStyle.Flat;
            clearButton.Font = new Font("Inter Medium", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            clearButton.ForeColor = Color.FromArgb(31, 41, 55);
            clearButton.Image = (Image)resources.GetObject("clearButton.Image");
            clearButton.Location = new Point(183, 393);
            clearButton.Margin = new Padding(3, 4, 3, 4);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(137, 37);
            clearButton.TabIndex = 14;
            clearButton.Text = "Clear";
            clearButton.TextAlign = ContentAlignment.MiddleRight;
            clearButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += ClearButton_Click;
            // 
            // createRentalButton
            // 
            createRentalButton.BackColor = Color.FromArgb(37, 99, 235);
            createRentalButton.FlatAppearance.BorderSize = 0;
            createRentalButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            createRentalButton.FlatStyle = FlatStyle.Flat;
            createRentalButton.Font = new Font("Inter Medium", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            createRentalButton.ForeColor = Color.White;
            createRentalButton.Image = (Image)resources.GetObject("createRentalButton.Image");
            createRentalButton.Location = new Point(23, 393);
            createRentalButton.Margin = new Padding(3, 4, 3, 4);
            createRentalButton.Name = "createRentalButton";
            createRentalButton.Size = new Size(152, 37);
            createRentalButton.TabIndex = 13;
            createRentalButton.Text = "Create Rental";
            createRentalButton.TextAlign = ContentAlignment.MiddleRight;
            createRentalButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            createRentalButton.UseVisualStyleBackColor = false;
            createRentalButton.Click += CreateRentalButton_Click;
            // 
            // paymentMethodComboBox
            // 
            paymentMethodComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            paymentMethodComboBox.Font = new Font("Segoe UI", 9.5F);
            paymentMethodComboBox.FormattingEnabled = true;
            paymentMethodComboBox.Location = new Point(183, 354);
            paymentMethodComboBox.Margin = new Padding(3, 4, 3, 4);
            paymentMethodComboBox.Name = "paymentMethodComboBox";
            paymentMethodComboBox.Size = new Size(137, 29);
            paymentMethodComboBox.TabIndex = 16;
            // 
            // paymentMethodLabel
            // 
            paymentMethodLabel.AutoSize = true;
            paymentMethodLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            paymentMethodLabel.ForeColor = Color.FromArgb(31, 41, 55);
            paymentMethodLabel.Location = new Point(183, 330);
            paymentMethodLabel.Name = "paymentMethodLabel";
            paymentMethodLabel.Size = new Size(65, 22);
            paymentMethodLabel.TabIndex = 15;
            paymentMethodLabel.Text = "Method";
            // 
            // paymentAmountTextBox
            // 
            paymentAmountTextBox.BorderStyle = BorderStyle.FixedSingle;
            paymentAmountTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            paymentAmountTextBox.Location = new Point(23, 354);
            paymentAmountTextBox.Margin = new Padding(3, 4, 3, 4);
            paymentAmountTextBox.Name = "paymentAmountTextBox";
            paymentAmountTextBox.PlaceholderText = "0";
            paymentAmountTextBox.Size = new Size(137, 28);
            paymentAmountTextBox.TabIndex = 14;
            paymentAmountTextBox.TextChanged += PaymentAmountTextBox_TextChanged;
            // 
            // paymentAmountLabel
            // 
            paymentAmountLabel.AutoSize = true;
            paymentAmountLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            paymentAmountLabel.ForeColor = Color.FromArgb(31, 41, 55);
            paymentAmountLabel.Location = new Point(23, 330);
            paymentAmountLabel.Name = "paymentAmountLabel";
            paymentAmountLabel.Size = new Size(100, 22);
            paymentAmountLabel.TabIndex = 13;
            paymentAmountLabel.Text = "Amount Paid";
            // 
            // totalAmountTextBox
            // 
            totalAmountTextBox.BorderStyle = BorderStyle.FixedSingle;
            totalAmountTextBox.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalAmountTextBox.Location = new Point(183, 296);
            totalAmountTextBox.Margin = new Padding(3, 4, 3, 4);
            totalAmountTextBox.Name = "totalAmountTextBox";
            totalAmountTextBox.ReadOnly = true;
            totalAmountTextBox.Size = new Size(137, 28);
            totalAmountTextBox.TabIndex = 12;
            totalAmountTextBox.Text = "P 2,500";
            // 
            // totalAmountLabel
            // 
            totalAmountLabel.AutoSize = true;
            totalAmountLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalAmountLabel.ForeColor = Color.FromArgb(31, 41, 55);
            totalAmountLabel.Location = new Point(183, 272);
            totalAmountLabel.Name = "totalAmountLabel";
            totalAmountLabel.Size = new Size(104, 22);
            totalAmountLabel.TabIndex = 11;
            totalAmountLabel.Text = "Total Amount";
            // 
            // dailyRateTextBox
            // 
            dailyRateTextBox.BorderStyle = BorderStyle.FixedSingle;
            dailyRateTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dailyRateTextBox.Location = new Point(23, 296);
            dailyRateTextBox.Margin = new Padding(3, 4, 3, 4);
            dailyRateTextBox.Name = "dailyRateTextBox";
            dailyRateTextBox.ReadOnly = true;
            dailyRateTextBox.Size = new Size(137, 28);
            dailyRateTextBox.TabIndex = 10;
            dailyRateTextBox.Text = "2500";
            dailyRateTextBox.TextChanged += DateOrRate_Changed;
            // 
            // dailyRateLabel
            // 
            dailyRateLabel.AutoSize = true;
            dailyRateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dailyRateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            dailyRateLabel.Location = new Point(23, 272);
            dailyRateLabel.Name = "dailyRateLabel";
            dailyRateLabel.Size = new Size(81, 22);
            dailyRateLabel.TabIndex = 9;
            dailyRateLabel.Text = "Daily Rate";
            // 
            // returnDatePicker
            // 
            returnDatePicker.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            returnDatePicker.Format = DateTimePickerFormat.Short;
            returnDatePicker.Location = new Point(183, 231);
            returnDatePicker.Margin = new Padding(3, 4, 3, 4);
            returnDatePicker.Name = "returnDatePicker";
            returnDatePicker.Size = new Size(137, 26);
            returnDatePicker.TabIndex = 8;
            returnDatePicker.ValueChanged += DateOrRate_Changed;
            // 
            // returnDateLabel
            // 
            returnDateLabel.AutoSize = true;
            returnDateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            returnDateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            returnDateLabel.Location = new Point(183, 207);
            returnDateLabel.Name = "returnDateLabel";
            returnDateLabel.Size = new Size(95, 22);
            returnDateLabel.TabIndex = 7;
            returnDateLabel.Text = "Return Date";
            // 
            // rentalDatePicker
            // 
            rentalDatePicker.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rentalDatePicker.Format = DateTimePickerFormat.Short;
            rentalDatePicker.Location = new Point(23, 231);
            rentalDatePicker.Margin = new Padding(3, 4, 3, 4);
            rentalDatePicker.Name = "rentalDatePicker";
            rentalDatePicker.Size = new Size(137, 26);
            rentalDatePicker.TabIndex = 6;
            rentalDatePicker.ValueChanged += DateOrRate_Changed;
            // 
            // rentalDateLabel
            // 
            rentalDateLabel.AutoSize = true;
            rentalDateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rentalDateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            rentalDateLabel.Location = new Point(23, 207);
            rentalDateLabel.Name = "rentalDateLabel";
            rentalDateLabel.Size = new Size(92, 22);
            rentalDateLabel.TabIndex = 5;
            rentalDateLabel.Text = "Rental Date";
            // 
            // selectedCarTextBox
            // 
            selectedCarTextBox.BorderStyle = BorderStyle.FixedSingle;
            selectedCarTextBox.Font = new Font("Segoe UI", 9.5F);
            selectedCarTextBox.Location = new Point(23, 163);
            selectedCarTextBox.Margin = new Padding(3, 4, 3, 4);
            selectedCarTextBox.Name = "selectedCarTextBox";
            selectedCarTextBox.PlaceholderText = "Click an available car from the grid";
            selectedCarTextBox.ReadOnly = true;
            selectedCarTextBox.Size = new Size(297, 29);
            selectedCarTextBox.TabIndex = 4;
            // 
            // selectedCarLabel
            // 
            selectedCarLabel.AutoSize = true;
            selectedCarLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            selectedCarLabel.ForeColor = Color.FromArgb(31, 41, 55);
            selectedCarLabel.Location = new Point(23, 139);
            selectedCarLabel.Name = "selectedCarLabel";
            selectedCarLabel.Size = new Size(103, 22);
            selectedCarLabel.TabIndex = 3;
            selectedCarLabel.Text = "Selected Car";
            // 
            // customerNameTextBox
            // 
            customerNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            customerNameTextBox.Font = new Font("Segoe UI", 9.5F);
            customerNameTextBox.Location = new Point(23, 96);
            customerNameTextBox.Margin = new Padding(3, 4, 3, 4);
            customerNameTextBox.Name = "customerNameTextBox";
            customerNameTextBox.PlaceholderText = "Customer full name";
            customerNameTextBox.Size = new Size(297, 29);
            customerNameTextBox.TabIndex = 2;
            // 
            // customerNameLabel
            // 
            customerNameLabel.AutoSize = true;
            customerNameLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customerNameLabel.ForeColor = Color.FromArgb(31, 41, 55);
            customerNameLabel.Location = new Point(23, 72);
            customerNameLabel.Name = "customerNameLabel";
            customerNameLabel.Size = new Size(51, 22);
            customerNameLabel.TabIndex = 1;
            customerNameLabel.Text = "Name";
            // 
            // quickRentalTitleLabel
            // 
            quickRentalTitleLabel.AutoSize = true;
            quickRentalTitleLabel.Font = new Font("Inter SemiBold", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickRentalTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            quickRentalTitleLabel.Location = new Point(21, 21);
            quickRentalTitleLabel.Name = "quickRentalTitleLabel";
            quickRentalTitleLabel.Size = new Size(154, 32);
            quickRentalTitleLabel.TabIndex = 0;
            quickRentalTitleLabel.Text = "Quick Rental";
            // 
            // availabilityPanel
            // 
            availabilityPanel.BackColor = Color.White;
            availabilityPanel.Controls.Add(carsGrid);
            availabilityPanel.Controls.Add(availabilityHeaderPanel);
            availabilityPanel.Dock = DockStyle.Fill;
            availabilityPanel.Location = new Point(366, 0);
            availabilityPanel.Margin = new Padding(0);
            availabilityPanel.Name = "availabilityPanel";
            availabilityPanel.Size = new Size(667, 445);
            availabilityPanel.TabIndex = 1;
            // 
            // carsGrid
            // 
            carsGrid.AllowUserToAddRows = false;
            carsGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(249, 250, 251);
            carsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            carsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            carsGrid.BackgroundColor = Color.White;
            carsGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            carsGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle5.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            carsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            carsGrid.ColumnHeadersHeight = 56;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            carsGrid.DefaultCellStyle = dataGridViewCellStyle6;
            carsGrid.Dock = DockStyle.Fill;
            carsGrid.EnableHeadersVisualStyles = false;
            carsGrid.GridColor = Color.FromArgb(229, 231, 235);
            carsGrid.Location = new Point(0, 80);
            carsGrid.Margin = new Padding(3, 4, 3, 4);
            carsGrid.Name = "carsGrid";
            carsGrid.ReadOnly = true;
            carsGrid.RowHeadersVisible = false;
            carsGrid.RowHeadersWidth = 51;
            carsGrid.RowTemplate.Height = 42;
            carsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            carsGrid.Size = new Size(667, 365);
            carsGrid.TabIndex = 1;
            carsGrid.CellClick += CarsGrid_CellClick;
            // 
            // availabilityHeaderPanel
            // 
            availabilityHeaderPanel.BorderStyle = BorderStyle.FixedSingle;
            availabilityHeaderPanel.Controls.Add(searchTextBox);
            availabilityHeaderPanel.Controls.Add(availableCarsTitleLabel);
            availabilityHeaderPanel.Dock = DockStyle.Top;
            availabilityHeaderPanel.Location = new Point(0, 0);
            availabilityHeaderPanel.Margin = new Padding(3, 4, 3, 4);
            availabilityHeaderPanel.Name = "availabilityHeaderPanel";
            availabilityHeaderPanel.Padding = new Padding(21, 19, 21, 11);
            availabilityHeaderPanel.Size = new Size(667, 80);
            availabilityHeaderPanel.TabIndex = 0;
            // 
            // searchTextBox
            // 
            searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            searchTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchTextBox.Location = new Point(385, 24);
            searchTextBox.Margin = new Padding(3, 4, 3, 4);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Search plate or model";
            searchTextBox.Size = new Size(258, 28);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // availableCarsTitleLabel
            // 
            availableCarsTitleLabel.AutoSize = true;
            availableCarsTitleLabel.Font = new Font("Inter SemiBold", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            availableCarsTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            availableCarsTitleLabel.Location = new Point(21, 21);
            availableCarsTitleLabel.Name = "availableCarsTitleLabel";
            availableCarsTitleLabel.Size = new Size(172, 32);
            availableCarsTitleLabel.TabIndex = 0;
            availableCarsTitleLabel.Text = "Available Cars";
            // 
            // statsTable
            // 
            statsTable.ColumnCount = 4;
            statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsTable.Controls.Add(totalCarsCard, 0, 0);
            statsTable.Controls.Add(availableCarsCard, 1, 0);
            statsTable.Controls.Add(rentedCarsCard, 2, 0);
            statsTable.Controls.Add(incomeCard, 3, 0);
            statsTable.Dock = DockStyle.Top;
            statsTable.Location = new Point(32, 37);
            statsTable.Margin = new Padding(3, 4, 3, 4);
            statsTable.Name = "statsTable";
            statsTable.RowCount = 1;
            statsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            statsTable.Size = new Size(1033, 96);
            statsTable.TabIndex = 0;
            // 
            // totalCarsCard
            // 
            totalCarsCard.BackColor = Color.FromArgb(249, 250, 251);
            totalCarsCard.BorderStyle = BorderStyle.FixedSingle;
            totalCarsCard.Controls.Add(totalCarsValueLabel);
            totalCarsCard.Controls.Add(totalCarsTextLabel);
            totalCarsCard.Dock = DockStyle.Fill;
            totalCarsCard.Location = new Point(0, 0);
            totalCarsCard.Margin = new Padding(0, 0, 14, 0);
            totalCarsCard.Name = "totalCarsCard";
            totalCarsCard.Size = new Size(244, 96);
            totalCarsCard.TabIndex = 0;
            // 
            // totalCarsValueLabel
            // 
            totalCarsValueLabel.AutoSize = true;
            totalCarsValueLabel.Font = new Font("Inter SemiBold", 18F, FontStyle.Bold);
            totalCarsValueLabel.ForeColor = Color.FromArgb(37, 99, 235);
            totalCarsValueLabel.Location = new Point(18, 41);
            totalCarsValueLabel.Name = "totalCarsValueLabel";
            totalCarsValueLabel.Size = new Size(39, 43);
            totalCarsValueLabel.TabIndex = 1;
            totalCarsValueLabel.Text = "0";
            // 
            // totalCarsTextLabel
            // 
            totalCarsTextLabel.AutoSize = true;
            totalCarsTextLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalCarsTextLabel.ForeColor = Color.FromArgb(31, 41, 55);
            totalCarsTextLabel.Location = new Point(18, 14);
            totalCarsTextLabel.Name = "totalCarsTextLabel";
            totalCarsTextLabel.Size = new Size(82, 22);
            totalCarsTextLabel.TabIndex = 0;
            totalCarsTextLabel.Text = "Total Cars";
            // 
            // availableCarsCard
            // 
            availableCarsCard.BackColor = Color.FromArgb(249, 250, 251);
            availableCarsCard.BorderStyle = BorderStyle.FixedSingle;
            availableCarsCard.Controls.Add(availableCarsValueLabel);
            availableCarsCard.Controls.Add(availableCarsTextLabel);
            availableCarsCard.Dock = DockStyle.Fill;
            availableCarsCard.Location = new Point(258, 0);
            availableCarsCard.Margin = new Padding(0, 0, 14, 0);
            availableCarsCard.Name = "availableCarsCard";
            availableCarsCard.Size = new Size(244, 96);
            availableCarsCard.TabIndex = 1;
            // 
            // availableCarsValueLabel
            // 
            availableCarsValueLabel.AutoSize = true;
            availableCarsValueLabel.Font = new Font("Inter SemiBold", 18F, FontStyle.Bold);
            availableCarsValueLabel.ForeColor = Color.FromArgb(22, 163, 74);
            availableCarsValueLabel.Location = new Point(18, 41);
            availableCarsValueLabel.Name = "availableCarsValueLabel";
            availableCarsValueLabel.Size = new Size(39, 43);
            availableCarsValueLabel.TabIndex = 1;
            availableCarsValueLabel.Text = "0";
            // 
            // availableCarsTextLabel
            // 
            availableCarsTextLabel.AutoSize = true;
            availableCarsTextLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            availableCarsTextLabel.ForeColor = Color.FromArgb(31, 41, 55);
            availableCarsTextLabel.Location = new Point(18, 14);
            availableCarsTextLabel.Name = "availableCarsTextLabel";
            availableCarsTextLabel.Size = new Size(110, 22);
            availableCarsTextLabel.TabIndex = 0;
            availableCarsTextLabel.Text = "Available Cars";
            // 
            // rentedCarsCard
            // 
            rentedCarsCard.BackColor = Color.FromArgb(249, 250, 251);
            rentedCarsCard.BorderStyle = BorderStyle.FixedSingle;
            rentedCarsCard.Controls.Add(rentedCarsValueLabel);
            rentedCarsCard.Controls.Add(rentedCarsTextLabel);
            rentedCarsCard.Dock = DockStyle.Fill;
            rentedCarsCard.Location = new Point(516, 0);
            rentedCarsCard.Margin = new Padding(0, 0, 14, 0);
            rentedCarsCard.Name = "rentedCarsCard";
            rentedCarsCard.Size = new Size(244, 96);
            rentedCarsCard.TabIndex = 2;
            // 
            // rentedCarsValueLabel
            // 
            rentedCarsValueLabel.AutoSize = true;
            rentedCarsValueLabel.Font = new Font("Inter SemiBold", 18F, FontStyle.Bold);
            rentedCarsValueLabel.ForeColor = Color.FromArgb(220, 38, 38);
            rentedCarsValueLabel.Location = new Point(18, 41);
            rentedCarsValueLabel.Name = "rentedCarsValueLabel";
            rentedCarsValueLabel.Size = new Size(39, 43);
            rentedCarsValueLabel.TabIndex = 1;
            rentedCarsValueLabel.Text = "0";
            // 
            // rentedCarsTextLabel
            // 
            rentedCarsTextLabel.AutoSize = true;
            rentedCarsTextLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rentedCarsTextLabel.ForeColor = Color.FromArgb(31, 41, 55);
            rentedCarsTextLabel.Location = new Point(18, 14);
            rentedCarsTextLabel.Name = "rentedCarsTextLabel";
            rentedCarsTextLabel.Size = new Size(128, 22);
            rentedCarsTextLabel.TabIndex = 0;
            rentedCarsTextLabel.Text = "Overdue Rentals";
            // 
            // incomeCard
            // 
            incomeCard.BackColor = Color.FromArgb(249, 250, 251);
            incomeCard.BorderStyle = BorderStyle.FixedSingle;
            incomeCard.Controls.Add(incomeValueLabel);
            incomeCard.Controls.Add(incomeTextLabel);
            incomeCard.Dock = DockStyle.Fill;
            incomeCard.Location = new Point(774, 0);
            incomeCard.Margin = new Padding(0);
            incomeCard.Name = "incomeCard";
            incomeCard.Size = new Size(259, 96);
            incomeCard.TabIndex = 3;
            // 
            // incomeValueLabel
            // 
            incomeValueLabel.AutoSize = true;
            incomeValueLabel.Font = new Font("Inter SemiBold", 18F, FontStyle.Bold);
            incomeValueLabel.ForeColor = Color.FromArgb(31, 41, 55);
            incomeValueLabel.Location = new Point(18, 43);
            incomeValueLabel.Name = "incomeValueLabel";
            incomeValueLabel.Size = new Size(114, 43);
            incomeValueLabel.TabIndex = 1;
            incomeValueLabel.Text = "P 0.00";
            // 
            // incomeTextLabel
            // 
            incomeTextLabel.AutoSize = true;
            incomeTextLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            incomeTextLabel.ForeColor = Color.FromArgb(31, 41, 55);
            incomeTextLabel.Location = new Point(18, 14);
            incomeTextLabel.Name = "incomeTextLabel";
            incomeTextLabel.Size = new Size(141, 22);
            incomeTextLabel.TabIndex = 0;
            incomeTextLabel.Text = "Monthly Collected";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1371, 960);
            Controls.Add(contentPanel);
            Controls.Add(topPanel);
            Controls.Add(sidebarPanel);
            Font = new Font("Segoe UI", 9F);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1255, 851);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Avenue Car Rental";
            Load += MainForm_Load;
            sidebarPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)sidebarLogoPictureBox).EndInit();
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            contentPanel.ResumeLayout(false);
            recentPanel.ResumeLayout(false);
            recentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)recentRentalsGrid).EndInit();
            mainTable.ResumeLayout(false);
            quickRentalPanel.ResumeLayout(false);
            quickRentalPanel.PerformLayout();
            availabilityPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)carsGrid).EndInit();
            availabilityHeaderPanel.ResumeLayout(false);
            availabilityHeaderPanel.PerformLayout();
            statsTable.ResumeLayout(false);
            totalCarsCard.ResumeLayout(false);
            totalCarsCard.PerformLayout();
            availableCarsCard.ResumeLayout(false);
            availableCarsCard.PerformLayout();
            rentedCarsCard.ResumeLayout(false);
            rentedCarsCard.PerformLayout();
            incomeCard.ResumeLayout(false);
            incomeCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel sidebarPanel;
        private Button logoutButton;
        private Button reportsButton;
        private Button returnsButton;
        private Button reservationsButton;
        private Button rentalsButton;
        private Button customersButton;
        private Button ratesButton;
        private Button carsButton;
        private Button dashboardButton;
        private PictureBox sidebarLogoPictureBox;
        private Label appTitleLabel;
        private Panel topPanel;
        private Label staffBadgeLabel;
        private Label dateLabel;
        private Label pageTitleLabel;
        private Panel contentPanel;
        private Panel recentPanel;
        private DataGridView recentRentalsGrid;
        private Label recentTitleLabel;
        private TableLayoutPanel mainTable;
        private Panel quickRentalPanel;
        private Button clearButton;
        private Button createRentalButton;
        private ComboBox paymentMethodComboBox;
        private Label paymentMethodLabel;
        private TextBox paymentAmountTextBox;
        private Label paymentAmountLabel;
        private TextBox totalAmountTextBox;
        private Label totalAmountLabel;
        private TextBox dailyRateTextBox;
        private Label dailyRateLabel;
        private DateTimePicker returnDatePicker;
        private Label returnDateLabel;
        private DateTimePicker rentalDatePicker;
        private Label rentalDateLabel;
        private TextBox selectedCarTextBox;
        private Label selectedCarLabel;
        private TextBox customerNameTextBox;
        private Label customerNameLabel;
        private Label quickRentalTitleLabel;
        private Panel availabilityPanel;
        private DataGridView carsGrid;
        private Panel availabilityHeaderPanel;
        private TextBox searchTextBox;
        private Label availableCarsTitleLabel;
        private TableLayoutPanel statsTable;
        private Panel totalCarsCard;
        private Label totalCarsValueLabel;
        private Label totalCarsTextLabel;
        private Panel availableCarsCard;
        private Label availableCarsValueLabel;
        private Label availableCarsTextLabel;
        private Panel rentedCarsCard;
        private Label rentedCarsValueLabel;
        private Label rentedCarsTextLabel;
        private Panel incomeCard;
        private Label incomeValueLabel;
        private Label incomeTextLabel;
    }
}
