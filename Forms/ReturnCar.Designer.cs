namespace Car_Rental_System
{
    partial class ReturnCar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReturnCar));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            headerPanel = new Panel();
            helperLabel = new Label();
            titleLabel = new Label();
            bodyTable = new TableLayoutPanel();
            detailsPanel = new Panel();
            processReturnButton = new Button();
            paymentMethodComboBox = new ComboBox();
            paymentMethodLabel = new Label();
            additionalPaymentTextBox = new TextBox();
            additionalPaymentLabel = new Label();
            balanceTextBox = new TextBox();
            balanceLabel = new Label();
            amountPaidTextBox = new TextBox();
            amountPaidLabel = new Label();
            finalAmountTextBox = new TextBox();
            finalAmountLabel = new Label();
            penaltyTextBox = new TextBox();
            penaltyLabel = new Label();
            lateDaysTextBox = new TextBox();
            lateDaysLabel = new Label();
            actualReturnDatePicker = new DateTimePicker();
            actualReturnLabel = new Label();
            baseAmountTextBox = new TextBox();
            baseAmountLabel = new Label();
            dailyRateTextBox = new TextBox();
            dailyRateLabel = new Label();
            expectedReturnTextBox = new TextBox();
            expectedReturnLabel = new Label();
            carTextBox = new TextBox();
            carLabel = new Label();
            customerTextBox = new TextBox();
            customerLabel = new Label();
            sectionLabel = new Label();
            listPanel = new Panel();
            activeRentalsGrid = new DataGridView();
            listHeaderPanel = new Panel();
            searchTextBox = new TextBox();
            activeRentalsLabel = new Label();
            headerPanel.SuspendLayout();
            bodyTable.SuspendLayout();
            detailsPanel.SuspendLayout();
            listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)activeRentalsGrid).BeginInit();
            listHeaderPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.White;
            headerPanel.Controls.Add(helperLabel);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Margin = new Padding(3, 4, 3, 4);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(1234, 139);
            headerPanel.TabIndex = 0;
            // 
            // helperLabel
            // 
            helperLabel.AutoSize = true;
            helperLabel.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            helperLabel.ForeColor = Color.FromArgb(31, 41, 55);
            helperLabel.Location = new Point(35, 85);
            helperLabel.Name = "helperLabel";
            helperLabel.Size = new Size(439, 24);
            helperLabel.TabIndex = 1;
            helperLabel.Text = "Process active rentals and release returned vehicles.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            titleLabel.Location = new Point(32, 32);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(216, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Return Car";
            // 
            // bodyTable
            // 
            bodyTable.BackColor = Color.FromArgb(243, 244, 246);
            bodyTable.ColumnCount = 2;
            bodyTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 411F));
            bodyTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bodyTable.Controls.Add(detailsPanel, 0, 0);
            bodyTable.Controls.Add(listPanel, 1, 0);
            bodyTable.Dock = DockStyle.Fill;
            bodyTable.Location = new Point(0, 139);
            bodyTable.Margin = new Padding(3, 4, 3, 4);
            bodyTable.Name = "bodyTable";
            bodyTable.Padding = new Padding(32, 37, 32, 37);
            bodyTable.RowCount = 1;
            bodyTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bodyTable.Size = new Size(1234, 768);
            bodyTable.TabIndex = 1;
            // 
            // detailsPanel
            // 
            detailsPanel.BackColor = Color.White;
            detailsPanel.BorderStyle = BorderStyle.FixedSingle;
            detailsPanel.Controls.Add(processReturnButton);
            detailsPanel.Controls.Add(paymentMethodComboBox);
            detailsPanel.Controls.Add(paymentMethodLabel);
            detailsPanel.Controls.Add(additionalPaymentTextBox);
            detailsPanel.Controls.Add(additionalPaymentLabel);
            detailsPanel.Controls.Add(balanceTextBox);
            detailsPanel.Controls.Add(balanceLabel);
            detailsPanel.Controls.Add(amountPaidTextBox);
            detailsPanel.Controls.Add(amountPaidLabel);
            detailsPanel.Controls.Add(finalAmountTextBox);
            detailsPanel.Controls.Add(finalAmountLabel);
            detailsPanel.Controls.Add(penaltyTextBox);
            detailsPanel.Controls.Add(penaltyLabel);
            detailsPanel.Controls.Add(lateDaysTextBox);
            detailsPanel.Controls.Add(lateDaysLabel);
            detailsPanel.Controls.Add(actualReturnDatePicker);
            detailsPanel.Controls.Add(actualReturnLabel);
            detailsPanel.Controls.Add(baseAmountTextBox);
            detailsPanel.Controls.Add(baseAmountLabel);
            detailsPanel.Controls.Add(dailyRateTextBox);
            detailsPanel.Controls.Add(dailyRateLabel);
            detailsPanel.Controls.Add(expectedReturnTextBox);
            detailsPanel.Controls.Add(expectedReturnLabel);
            detailsPanel.Controls.Add(carTextBox);
            detailsPanel.Controls.Add(carLabel);
            detailsPanel.Controls.Add(customerTextBox);
            detailsPanel.Controls.Add(customerLabel);
            detailsPanel.Controls.Add(sectionLabel);
            detailsPanel.Dock = DockStyle.Fill;
            detailsPanel.Font = new Font("Inter", 9F);
            detailsPanel.Location = new Point(32, 37);
            detailsPanel.Margin = new Padding(0, 0, 21, 0);
            detailsPanel.Name = "detailsPanel";
            detailsPanel.Size = new Size(390, 694);
            detailsPanel.TabIndex = 0;
            // 
            // processReturnButton
            // 
            processReturnButton.BackColor = Color.FromArgb(37, 99, 235);
            processReturnButton.FlatAppearance.BorderSize = 0;
            processReturnButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            processReturnButton.FlatStyle = FlatStyle.Flat;
            processReturnButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            processReturnButton.ForeColor = Color.White;
            processReturnButton.Image = (Image)resources.GetObject("processReturnButton.Image");
            processReturnButton.Location = new Point(23, 566);
            processReturnButton.Margin = new Padding(3, 4, 3, 4);
            processReturnButton.Name = "processReturnButton";
            processReturnButton.Size = new Size(343, 45);
            processReturnButton.TabIndex = 27;
            processReturnButton.Text = "Process Return";
            processReturnButton.TextAlign = ContentAlignment.MiddleRight;
            processReturnButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            processReturnButton.UseVisualStyleBackColor = false;
            processReturnButton.Click += ProcessReturnButton_Click;
            // 
            // paymentMethodComboBox
            // 
            paymentMethodComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            paymentMethodComboBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            paymentMethodComboBox.FormattingEnabled = true;
            paymentMethodComboBox.Location = new Point(208, 516);
            paymentMethodComboBox.Margin = new Padding(3, 4, 3, 4);
            paymentMethodComboBox.Name = "paymentMethodComboBox";
            paymentMethodComboBox.Size = new Size(160, 32);
            paymentMethodComboBox.TabIndex = 26;
            // 
            // paymentMethodLabel
            // 
            paymentMethodLabel.AutoSize = true;
            paymentMethodLabel.Font = new Font("Inter", 9F);
            paymentMethodLabel.ForeColor = Color.FromArgb(31, 41, 55);
            paymentMethodLabel.Location = new Point(208, 493);
            paymentMethodLabel.Name = "paymentMethodLabel";
            paymentMethodLabel.Size = new Size(65, 22);
            paymentMethodLabel.TabIndex = 25;
            paymentMethodLabel.Text = "Method";
            // 
            // additionalPaymentTextBox
            // 
            additionalPaymentTextBox.BorderStyle = BorderStyle.FixedSingle;
            additionalPaymentTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            additionalPaymentTextBox.Location = new Point(25, 520);
            additionalPaymentTextBox.Margin = new Padding(3, 4, 3, 4);
            additionalPaymentTextBox.Name = "additionalPaymentTextBox";
            additionalPaymentTextBox.PlaceholderText = "0";
            additionalPaymentTextBox.Size = new Size(160, 28);
            additionalPaymentTextBox.TabIndex = 24;
            additionalPaymentTextBox.TextChanged += AdditionalPaymentTextBox_TextChanged;
            // 
            // additionalPaymentLabel
            // 
            additionalPaymentLabel.AutoSize = true;
            additionalPaymentLabel.Font = new Font("Inter", 9F);
            additionalPaymentLabel.ForeColor = Color.FromArgb(31, 41, 55);
            additionalPaymentLabel.Location = new Point(25, 493);
            additionalPaymentLabel.Name = "additionalPaymentLabel";
            additionalPaymentLabel.Size = new Size(108, 22);
            additionalPaymentLabel.TabIndex = 23;
            additionalPaymentLabel.Text = "Payment Now";
            // 
            // balanceTextBox
            // 
            balanceTextBox.BorderStyle = BorderStyle.FixedSingle;
            balanceTextBox.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            balanceTextBox.Location = new Point(208, 460);
            balanceTextBox.Margin = new Padding(3, 4, 3, 4);
            balanceTextBox.Name = "balanceTextBox";
            balanceTextBox.ReadOnly = true;
            balanceTextBox.Size = new Size(160, 28);
            balanceTextBox.TabIndex = 22;
            // 
            // balanceLabel
            // 
            balanceLabel.AutoSize = true;
            balanceLabel.Font = new Font("Inter", 9F);
            balanceLabel.ForeColor = Color.FromArgb(31, 41, 55);
            balanceLabel.Location = new Point(208, 433);
            balanceLabel.Name = "balanceLabel";
            balanceLabel.Size = new Size(67, 22);
            balanceLabel.TabIndex = 21;
            balanceLabel.Text = "Balance";
            // 
            // amountPaidTextBox
            // 
            amountPaidTextBox.BorderStyle = BorderStyle.FixedSingle;
            amountPaidTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            amountPaidTextBox.Location = new Point(25, 460);
            amountPaidTextBox.Margin = new Padding(3, 4, 3, 4);
            amountPaidTextBox.Name = "amountPaidTextBox";
            amountPaidTextBox.ReadOnly = true;
            amountPaidTextBox.Size = new Size(160, 28);
            amountPaidTextBox.TabIndex = 20;
            // 
            // amountPaidLabel
            // 
            amountPaidLabel.AutoSize = true;
            amountPaidLabel.Font = new Font("Inter", 9F);
            amountPaidLabel.ForeColor = Color.FromArgb(31, 41, 55);
            amountPaidLabel.Location = new Point(25, 433);
            amountPaidLabel.Name = "amountPaidLabel";
            amountPaidLabel.Size = new Size(100, 22);
            amountPaidLabel.TabIndex = 19;
            amountPaidLabel.Text = "Amount Paid";
            // 
            // finalAmountTextBox
            // 
            finalAmountTextBox.BorderStyle = BorderStyle.FixedSingle;
            finalAmountTextBox.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            finalAmountTextBox.Location = new Point(208, 400);
            finalAmountTextBox.Margin = new Padding(3, 4, 3, 4);
            finalAmountTextBox.Name = "finalAmountTextBox";
            finalAmountTextBox.ReadOnly = true;
            finalAmountTextBox.Size = new Size(160, 28);
            finalAmountTextBox.TabIndex = 18;
            // 
            // finalAmountLabel
            // 
            finalAmountLabel.AutoSize = true;
            finalAmountLabel.Font = new Font("Inter", 9F);
            finalAmountLabel.ForeColor = Color.FromArgb(31, 41, 55);
            finalAmountLabel.Location = new Point(208, 373);
            finalAmountLabel.Name = "finalAmountLabel";
            finalAmountLabel.Size = new Size(103, 22);
            finalAmountLabel.TabIndex = 17;
            finalAmountLabel.Text = "Final Amount";
            // 
            // penaltyTextBox
            // 
            penaltyTextBox.BorderStyle = BorderStyle.FixedSingle;
            penaltyTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            penaltyTextBox.Location = new Point(25, 400);
            penaltyTextBox.Margin = new Padding(3, 4, 3, 4);
            penaltyTextBox.Name = "penaltyTextBox";
            penaltyTextBox.ReadOnly = true;
            penaltyTextBox.Size = new Size(160, 28);
            penaltyTextBox.TabIndex = 16;
            // 
            // penaltyLabel
            // 
            penaltyLabel.AutoSize = true;
            penaltyLabel.Font = new Font("Inter", 9F);
            penaltyLabel.ForeColor = Color.FromArgb(31, 41, 55);
            penaltyLabel.Location = new Point(25, 373);
            penaltyLabel.Name = "penaltyLabel";
            penaltyLabel.Size = new Size(97, 22);
            penaltyLabel.TabIndex = 15;
            penaltyLabel.Text = "Late Penalty";
            // 
            // lateDaysTextBox
            // 
            lateDaysTextBox.BorderStyle = BorderStyle.FixedSingle;
            lateDaysTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lateDaysTextBox.Location = new Point(208, 340);
            lateDaysTextBox.Margin = new Padding(3, 4, 3, 4);
            lateDaysTextBox.Name = "lateDaysTextBox";
            lateDaysTextBox.ReadOnly = true;
            lateDaysTextBox.Size = new Size(160, 28);
            lateDaysTextBox.TabIndex = 14;
            // 
            // lateDaysLabel
            // 
            lateDaysLabel.AutoSize = true;
            lateDaysLabel.Font = new Font("Inter", 9F);
            lateDaysLabel.ForeColor = Color.FromArgb(31, 41, 55);
            lateDaysLabel.Location = new Point(208, 313);
            lateDaysLabel.Name = "lateDaysLabel";
            lateDaysLabel.Size = new Size(79, 22);
            lateDaysLabel.TabIndex = 13;
            lateDaysLabel.Text = "Late Days";
            // 
            // actualReturnDatePicker
            // 
            actualReturnDatePicker.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            actualReturnDatePicker.Format = DateTimePickerFormat.Short;
            actualReturnDatePicker.Location = new Point(25, 340);
            actualReturnDatePicker.Margin = new Padding(3, 4, 3, 4);
            actualReturnDatePicker.Name = "actualReturnDatePicker";
            actualReturnDatePicker.Size = new Size(159, 28);
            actualReturnDatePicker.TabIndex = 12;
            actualReturnDatePicker.ValueChanged += ActualReturnDatePicker_ValueChanged;
            // 
            // actualReturnLabel
            // 
            actualReturnLabel.AutoSize = true;
            actualReturnLabel.Font = new Font("Inter", 9F);
            actualReturnLabel.ForeColor = Color.FromArgb(31, 41, 55);
            actualReturnLabel.Location = new Point(25, 313);
            actualReturnLabel.Name = "actualReturnLabel";
            actualReturnLabel.Size = new Size(107, 22);
            actualReturnLabel.TabIndex = 11;
            actualReturnLabel.Text = "Actual Return";
            // 
            // baseAmountTextBox
            // 
            baseAmountTextBox.BorderStyle = BorderStyle.FixedSingle;
            baseAmountTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            baseAmountTextBox.Location = new Point(208, 280);
            baseAmountTextBox.Margin = new Padding(3, 4, 3, 4);
            baseAmountTextBox.Name = "baseAmountTextBox";
            baseAmountTextBox.ReadOnly = true;
            baseAmountTextBox.Size = new Size(160, 28);
            baseAmountTextBox.TabIndex = 10;
            // 
            // baseAmountLabel
            // 
            baseAmountLabel.AutoSize = true;
            baseAmountLabel.Font = new Font("Inter", 9F);
            baseAmountLabel.ForeColor = Color.FromArgb(31, 41, 55);
            baseAmountLabel.Location = new Point(208, 253);
            baseAmountLabel.Name = "baseAmountLabel";
            baseAmountLabel.Size = new Size(104, 22);
            baseAmountLabel.TabIndex = 9;
            baseAmountLabel.Text = "Base Amount";
            // 
            // dailyRateTextBox
            // 
            dailyRateTextBox.BorderStyle = BorderStyle.FixedSingle;
            dailyRateTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dailyRateTextBox.Location = new Point(25, 280);
            dailyRateTextBox.Margin = new Padding(3, 4, 3, 4);
            dailyRateTextBox.Name = "dailyRateTextBox";
            dailyRateTextBox.ReadOnly = true;
            dailyRateTextBox.Size = new Size(160, 28);
            dailyRateTextBox.TabIndex = 8;
            // 
            // dailyRateLabel
            // 
            dailyRateLabel.AutoSize = true;
            dailyRateLabel.Font = new Font("Inter", 9F);
            dailyRateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            dailyRateLabel.Location = new Point(25, 253);
            dailyRateLabel.Name = "dailyRateLabel";
            dailyRateLabel.Size = new Size(81, 22);
            dailyRateLabel.TabIndex = 7;
            dailyRateLabel.Text = "Daily Rate";
            // 
            // expectedReturnTextBox
            // 
            expectedReturnTextBox.BorderStyle = BorderStyle.FixedSingle;
            expectedReturnTextBox.Font = new Font("Segoe UI", 10F);
            expectedReturnTextBox.Location = new Point(25, 220);
            expectedReturnTextBox.Margin = new Padding(3, 4, 3, 4);
            expectedReturnTextBox.Name = "expectedReturnTextBox";
            expectedReturnTextBox.ReadOnly = true;
            expectedReturnTextBox.Size = new Size(343, 30);
            expectedReturnTextBox.TabIndex = 6;
            // 
            // expectedReturnLabel
            // 
            expectedReturnLabel.AutoSize = true;
            expectedReturnLabel.Font = new Font("Inter", 9F);
            expectedReturnLabel.ForeColor = Color.FromArgb(31, 41, 55);
            expectedReturnLabel.Location = new Point(25, 193);
            expectedReturnLabel.Name = "expectedReturnLabel";
            expectedReturnLabel.Size = new Size(129, 22);
            expectedReturnLabel.TabIndex = 5;
            expectedReturnLabel.Text = "Expected Return";
            // 
            // carTextBox
            // 
            carTextBox.BorderStyle = BorderStyle.FixedSingle;
            carTextBox.Font = new Font("Segoe UI", 10F);
            carTextBox.Location = new Point(25, 160);
            carTextBox.Margin = new Padding(3, 4, 3, 4);
            carTextBox.Name = "carTextBox";
            carTextBox.ReadOnly = true;
            carTextBox.Size = new Size(343, 30);
            carTextBox.TabIndex = 4;
            // 
            // carLabel
            // 
            carLabel.AutoSize = true;
            carLabel.Font = new Font("Inter", 9F);
            carLabel.ForeColor = Color.FromArgb(31, 41, 55);
            carLabel.Location = new Point(25, 133);
            carLabel.Name = "carLabel";
            carLabel.Size = new Size(35, 22);
            carLabel.TabIndex = 3;
            carLabel.Text = "Car";
            // 
            // customerTextBox
            // 
            customerTextBox.BorderStyle = BorderStyle.FixedSingle;
            customerTextBox.Font = new Font("Segoe UI", 10F);
            customerTextBox.Location = new Point(25, 100);
            customerTextBox.Margin = new Padding(3, 4, 3, 4);
            customerTextBox.Name = "customerTextBox";
            customerTextBox.ReadOnly = true;
            customerTextBox.Size = new Size(343, 30);
            customerTextBox.TabIndex = 2;
            // 
            // customerLabel
            // 
            customerLabel.AutoSize = true;
            customerLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customerLabel.ForeColor = Color.FromArgb(31, 41, 55);
            customerLabel.Location = new Point(25, 73);
            customerLabel.Name = "customerLabel";
            customerLabel.Size = new Size(80, 22);
            customerLabel.TabIndex = 1;
            customerLabel.Text = "Customer";
            // 
            // sectionLabel
            // 
            sectionLabel.AutoSize = true;
            sectionLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold);
            sectionLabel.ForeColor = Color.FromArgb(31, 41, 55);
            sectionLabel.Location = new Point(23, 24);
            sectionLabel.Name = "sectionLabel";
            sectionLabel.Size = new Size(171, 32);
            sectionLabel.TabIndex = 0;
            sectionLabel.Text = "Return Details";
            // 
            // listPanel
            // 
            listPanel.BackColor = Color.White;
            listPanel.Controls.Add(activeRentalsGrid);
            listPanel.Controls.Add(listHeaderPanel);
            listPanel.Dock = DockStyle.Fill;
            listPanel.Location = new Point(443, 37);
            listPanel.Margin = new Padding(0);
            listPanel.Name = "listPanel";
            listPanel.Size = new Size(759, 694);
            listPanel.TabIndex = 1;
            // 
            // activeRentalsGrid
            // 
            activeRentalsGrid.AllowUserToAddRows = false;
            activeRentalsGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 251);
            activeRentalsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            activeRentalsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            activeRentalsGrid.BackgroundColor = Color.White;
            activeRentalsGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            activeRentalsGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.Font = new Font("Inter SemiBold", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            activeRentalsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            activeRentalsGrid.ColumnHeadersHeight = 56;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            activeRentalsGrid.DefaultCellStyle = dataGridViewCellStyle3;
            activeRentalsGrid.Dock = DockStyle.Fill;
            activeRentalsGrid.EnableHeadersVisualStyles = false;
            activeRentalsGrid.GridColor = Color.FromArgb(229, 231, 235);
            activeRentalsGrid.Location = new Point(0, 77);
            activeRentalsGrid.Margin = new Padding(3, 4, 3, 4);
            activeRentalsGrid.Name = "activeRentalsGrid";
            activeRentalsGrid.ReadOnly = true;
            activeRentalsGrid.RowHeadersVisible = false;
            activeRentalsGrid.RowHeadersWidth = 51;
            activeRentalsGrid.RowTemplate.Height = 42;
            activeRentalsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            activeRentalsGrid.Size = new Size(759, 617);
            activeRentalsGrid.TabIndex = 1;
            activeRentalsGrid.SelectionChanged += ActiveRentalsGrid_SelectionChanged;
            // 
            // listHeaderPanel
            // 
            listHeaderPanel.BorderStyle = BorderStyle.FixedSingle;
            listHeaderPanel.Controls.Add(searchTextBox);
            listHeaderPanel.Controls.Add(activeRentalsLabel);
            listHeaderPanel.Dock = DockStyle.Top;
            listHeaderPanel.Location = new Point(0, 0);
            listHeaderPanel.Margin = new Padding(3, 4, 3, 4);
            listHeaderPanel.Name = "listHeaderPanel";
            listHeaderPanel.Size = new Size(759, 77);
            listHeaderPanel.TabIndex = 0;
            // 
            // searchTextBox
            // 
            searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            searchTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchTextBox.Location = new Point(435, 24);
            searchTextBox.Margin = new Padding(3, 4, 3, 4);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Search active rental";
            searchTextBox.Size = new Size(297, 28);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // activeRentalsLabel
            // 
            activeRentalsLabel.AutoSize = true;
            activeRentalsLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold);
            activeRentalsLabel.ForeColor = Color.FromArgb(31, 41, 55);
            activeRentalsLabel.Location = new Point(0, 21);
            activeRentalsLabel.Name = "activeRentalsLabel";
            activeRentalsLabel.Size = new Size(174, 32);
            activeRentalsLabel.TabIndex = 0;
            activeRentalsLabel.Text = "Active Rentals";
            // 
            // ReturnCar
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1234, 907);
            Controls.Add(bodyTable);
            Controls.Add(headerPanel);
            Font = new Font("Segoe UI", 9F);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1117, 811);
            Name = "ReturnCar";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Avenue - Return Car";
            Load += ReturnCar_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            bodyTable.ResumeLayout(false);
            detailsPanel.ResumeLayout(false);
            detailsPanel.PerformLayout();
            listPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)activeRentalsGrid).EndInit();
            listHeaderPanel.ResumeLayout(false);
            listHeaderPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel headerPanel;
        private Label helperLabel;
        private Label titleLabel;
        private TableLayoutPanel bodyTable;
        private Panel detailsPanel;
        private Button processReturnButton;
        private ComboBox paymentMethodComboBox;
        private Label paymentMethodLabel;
        private TextBox additionalPaymentTextBox;
        private Label additionalPaymentLabel;
        private TextBox balanceTextBox;
        private Label balanceLabel;
        private TextBox amountPaidTextBox;
        private Label amountPaidLabel;
        private TextBox finalAmountTextBox;
        private Label finalAmountLabel;
        private TextBox penaltyTextBox;
        private Label penaltyLabel;
        private TextBox lateDaysTextBox;
        private Label lateDaysLabel;
        private DateTimePicker actualReturnDatePicker;
        private Label actualReturnLabel;
        private TextBox baseAmountTextBox;
        private Label baseAmountLabel;
        private TextBox dailyRateTextBox;
        private Label dailyRateLabel;
        private TextBox expectedReturnTextBox;
        private Label expectedReturnLabel;
        private TextBox carTextBox;
        private Label carLabel;
        private TextBox customerTextBox;
        private Label customerLabel;
        private Label sectionLabel;
        private Panel listPanel;
        private DataGridView activeRentalsGrid;
        private Panel listHeaderPanel;
        private TextBox searchTextBox;
        private Label activeRentalsLabel;
    }
}
