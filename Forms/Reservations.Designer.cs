namespace Car_Rental_System
{
    partial class Reservations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reservations));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            headerPanel = new Panel();
            helperLabel = new Label();
            pageTitleLabel = new Label();
            bodyTable = new TableLayoutPanel();
            detailsPanel = new Panel();
            clearButton = new Button();
            convertButton = new Button();
            cancelReservationButton = new Button();
            updateButton = new Button();
            addButton = new Button();
            paymentReferenceTextBox = new TextBox();
            paymentReferenceLabel = new Label();
            paymentMethodComboBox = new ComboBox();
            paymentMethodLabel = new Label();
            balanceTextBox = new TextBox();
            balanceLabel = new Label();
            depositTextBox = new TextBox();
            depositLabel = new Label();
            estimatedTotalTextBox = new TextBox();
            estimatedTotalLabel = new Label();
            rateTextBox = new TextBox();
            rateLabel = new Label();
            returnDatePicker = new DateTimePicker();
            returnDateLabel = new Label();
            pickupDatePicker = new DateTimePicker();
            pickupDateLabel = new Label();
            carComboBox = new ComboBox();
            carLabel = new Label();
            contactTextBox = new TextBox();
            contactLabel = new Label();
            customerNameTextBox = new TextBox();
            customerNameLabel = new Label();
            detailsTitleLabel = new Label();
            recordsPanel = new Panel();
            reservationsGrid = new DataGridView();
            recordsHeaderPanel = new Panel();
            searchTextBox = new TextBox();
            titleLabel = new Label();
            headerPanel.SuspendLayout();
            bodyTable.SuspendLayout();
            detailsPanel.SuspendLayout();
            recordsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)reservationsGrid).BeginInit();
            recordsHeaderPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.White;
            headerPanel.Controls.Add(helperLabel);
            headerPanel.Controls.Add(pageTitleLabel);
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
            helperLabel.Size = new Size(520, 24);
            helperLabel.TabIndex = 1;
            helperLabel.Text = "Manage future bookings, deposits, and pickup schedules.";
            // 
            // pageTitleLabel
            // 
            pageTitleLabel.AutoSize = true;
            pageTitleLabel.Font = new Font("Inter", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pageTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            pageTitleLabel.Location = new Point(32, 32);
            pageTitleLabel.Name = "pageTitleLabel";
            pageTitleLabel.Size = new Size(256, 53);
            pageTitleLabel.TabIndex = 0;
            pageTitleLabel.Text = "Reservations";
            // 
            // bodyTable
            // 
            bodyTable.BackColor = Color.FromArgb(243, 244, 246);
            bodyTable.ColumnCount = 2;
            bodyTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 411F));
            bodyTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bodyTable.Controls.Add(detailsPanel, 0, 0);
            bodyTable.Controls.Add(recordsPanel, 1, 0);
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
            detailsPanel.Controls.Add(clearButton);
            detailsPanel.Controls.Add(convertButton);
            detailsPanel.Controls.Add(cancelReservationButton);
            detailsPanel.Controls.Add(updateButton);
            detailsPanel.Controls.Add(addButton);
            detailsPanel.Controls.Add(paymentReferenceTextBox);
            detailsPanel.Controls.Add(paymentReferenceLabel);
            detailsPanel.Controls.Add(paymentMethodComboBox);
            detailsPanel.Controls.Add(paymentMethodLabel);
            detailsPanel.Controls.Add(balanceTextBox);
            detailsPanel.Controls.Add(balanceLabel);
            detailsPanel.Controls.Add(depositTextBox);
            detailsPanel.Controls.Add(depositLabel);
            detailsPanel.Controls.Add(estimatedTotalTextBox);
            detailsPanel.Controls.Add(estimatedTotalLabel);
            detailsPanel.Controls.Add(rateTextBox);
            detailsPanel.Controls.Add(rateLabel);
            detailsPanel.Controls.Add(returnDatePicker);
            detailsPanel.Controls.Add(returnDateLabel);
            detailsPanel.Controls.Add(pickupDatePicker);
            detailsPanel.Controls.Add(pickupDateLabel);
            detailsPanel.Controls.Add(carComboBox);
            detailsPanel.Controls.Add(carLabel);
            detailsPanel.Controls.Add(contactTextBox);
            detailsPanel.Controls.Add(contactLabel);
            detailsPanel.Controls.Add(customerNameTextBox);
            detailsPanel.Controls.Add(customerNameLabel);
            detailsPanel.Controls.Add(detailsTitleLabel);
            detailsPanel.Dock = DockStyle.Fill;
            detailsPanel.Location = new Point(32, 37);
            detailsPanel.Margin = new Padding(0, 0, 21, 0);
            detailsPanel.Name = "detailsPanel";
            detailsPanel.Size = new Size(390, 694);
            detailsPanel.TabIndex = 0;
            // 
            // clearButton
            // 
            clearButton.BackColor = Color.White;
            clearButton.FlatAppearance.BorderColor = Color.FromArgb(31, 41, 55);
            clearButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 244, 246);
            clearButton.FlatStyle = FlatStyle.Flat;
            clearButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            clearButton.ForeColor = Color.FromArgb(31, 41, 55);
            clearButton.Image = (Image)resources.GetObject("clearButton.Image");
            clearButton.Location = new Point(25, 643);
            clearButton.Margin = new Padding(3, 4, 3, 4);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(343, 45);
            clearButton.TabIndex = 21;
            clearButton.Text = "Clear";
            clearButton.TextAlign = ContentAlignment.MiddleRight;
            clearButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += ClearButton_Click;
            // 
            // convertButton
            // 
            convertButton.BackColor = Color.FromArgb(22, 163, 74);
            convertButton.FlatAppearance.BorderSize = 0;
            convertButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 163, 74);
            convertButton.FlatStyle = FlatStyle.Flat;
            convertButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            convertButton.ForeColor = Color.White;
            convertButton.Image = (Image)resources.GetObject("convertButton.Image");
            convertButton.Location = new Point(25, 588);
            convertButton.Margin = new Padding(3, 4, 3, 4);
            convertButton.Name = "convertButton";
            convertButton.Size = new Size(160, 45);
            convertButton.TabIndex = 20;
            convertButton.Text = "Convert";
            convertButton.TextAlign = ContentAlignment.MiddleRight;
            convertButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            convertButton.UseVisualStyleBackColor = false;
            convertButton.Click += ConvertButton_Click;
            // 
            // cancelReservationButton
            // 
            cancelReservationButton.BackColor = Color.FromArgb(220, 38, 38);
            cancelReservationButton.FlatAppearance.BorderSize = 0;
            cancelReservationButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            cancelReservationButton.FlatStyle = FlatStyle.Flat;
            cancelReservationButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cancelReservationButton.ForeColor = Color.White;
            cancelReservationButton.Image = (Image)resources.GetObject("cancelReservationButton.Image");
            cancelReservationButton.Location = new Point(208, 588);
            cancelReservationButton.Margin = new Padding(3, 4, 3, 4);
            cancelReservationButton.Name = "cancelReservationButton";
            cancelReservationButton.Size = new Size(160, 45);
            cancelReservationButton.TabIndex = 18;
            cancelReservationButton.Text = "Cancel";
            cancelReservationButton.TextAlign = ContentAlignment.MiddleRight;
            cancelReservationButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            cancelReservationButton.UseVisualStyleBackColor = false;
            cancelReservationButton.Click += CancelReservationButton_Click;
            // 
            // updateButton
            // 
            updateButton.BackColor = Color.FromArgb(31, 41, 55);
            updateButton.FlatAppearance.BorderSize = 0;
            updateButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 41, 55);
            updateButton.FlatStyle = FlatStyle.Flat;
            updateButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            updateButton.ForeColor = Color.White;
            updateButton.Image = (Image)resources.GetObject("updateButton.Image");
            updateButton.Location = new Point(208, 533);
            updateButton.Margin = new Padding(3, 4, 3, 4);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(160, 45);
            updateButton.TabIndex = 17;
            updateButton.Text = "Update";
            updateButton.TextAlign = ContentAlignment.MiddleRight;
            updateButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            updateButton.UseVisualStyleBackColor = false;
            updateButton.Click += UpdateButton_Click;
            // 
            // addButton
            // 
            addButton.BackColor = Color.FromArgb(37, 99, 235);
            addButton.FlatAppearance.BorderSize = 0;
            addButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addButton.ForeColor = Color.White;
            addButton.Image = (Image)resources.GetObject("addButton.Image");
            addButton.Location = new Point(25, 533);
            addButton.Margin = new Padding(3, 4, 3, 4);
            addButton.Name = "addButton";
            addButton.Size = new Size(160, 45);
            addButton.TabIndex = 16;
            addButton.Text = "Reserve";
            addButton.TextAlign = ContentAlignment.MiddleRight;
            addButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            addButton.UseVisualStyleBackColor = false;
            addButton.Click += AddButton_Click;
            // 
            // paymentReferenceTextBox
            // 
            paymentReferenceTextBox.BorderStyle = BorderStyle.FixedSingle;
            paymentReferenceTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            paymentReferenceTextBox.Location = new Point(208, 481);
            paymentReferenceTextBox.Margin = new Padding(3, 4, 3, 4);
            paymentReferenceTextBox.Name = "paymentReferenceTextBox";
            paymentReferenceTextBox.PlaceholderText = "Reference number";
            paymentReferenceTextBox.Size = new Size(160, 28);
            paymentReferenceTextBox.TabIndex = 11;
            // 
            // paymentReferenceLabel
            // 
            paymentReferenceLabel.AutoSize = true;
            paymentReferenceLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            paymentReferenceLabel.ForeColor = Color.FromArgb(31, 41, 55);
            paymentReferenceLabel.Location = new Point(208, 455);
            paymentReferenceLabel.Name = "paymentReferenceLabel";
            paymentReferenceLabel.Size = new Size(152, 22);
            paymentReferenceLabel.TabIndex = 10;
            paymentReferenceLabel.Text = "Payment Reference";
            // 
            // paymentMethodComboBox
            // 
            paymentMethodComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            paymentMethodComboBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            paymentMethodComboBox.FormattingEnabled = true;
            paymentMethodComboBox.Location = new Point(25, 481);
            paymentMethodComboBox.Margin = new Padding(3, 4, 3, 4);
            paymentMethodComboBox.Name = "paymentMethodComboBox";
            paymentMethodComboBox.Size = new Size(160, 32);
            paymentMethodComboBox.TabIndex = 9;
            // 
            // paymentMethodLabel
            // 
            paymentMethodLabel.AutoSize = true;
            paymentMethodLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            paymentMethodLabel.ForeColor = Color.FromArgb(31, 41, 55);
            paymentMethodLabel.Location = new Point(25, 455);
            paymentMethodLabel.Name = "paymentMethodLabel";
            paymentMethodLabel.Size = new Size(131, 22);
            paymentMethodLabel.TabIndex = 8;
            paymentMethodLabel.Text = "Payment Method";
            // 
            // balanceTextBox
            // 
            balanceTextBox.BorderStyle = BorderStyle.FixedSingle;
            balanceTextBox.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            balanceTextBox.Location = new Point(208, 406);
            balanceTextBox.Margin = new Padding(3, 4, 3, 4);
            balanceTextBox.Name = "balanceTextBox";
            balanceTextBox.ReadOnly = true;
            balanceTextBox.Size = new Size(160, 28);
            balanceTextBox.TabIndex = 7;
            // 
            // balanceLabel
            // 
            balanceLabel.AutoSize = true;
            balanceLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            balanceLabel.ForeColor = Color.FromArgb(31, 41, 55);
            balanceLabel.Location = new Point(208, 380);
            balanceLabel.Name = "balanceLabel";
            balanceLabel.Size = new Size(67, 22);
            balanceLabel.TabIndex = 6;
            balanceLabel.Text = "Balance";
            // 
            // depositTextBox
            // 
            depositTextBox.BorderStyle = BorderStyle.FixedSingle;
            depositTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            depositTextBox.Location = new Point(25, 406);
            depositTextBox.Margin = new Padding(3, 4, 3, 4);
            depositTextBox.Name = "depositTextBox";
            depositTextBox.PlaceholderText = "0";
            depositTextBox.Size = new Size(160, 28);
            depositTextBox.TabIndex = 5;
            depositTextBox.TextChanged += DepositTextBox_TextChanged;
            // 
            // depositLabel
            // 
            depositLabel.AutoSize = true;
            depositLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            depositLabel.ForeColor = Color.FromArgb(31, 41, 55);
            depositLabel.Location = new Point(25, 380);
            depositLabel.Name = "depositLabel";
            depositLabel.Size = new Size(65, 22);
            depositLabel.TabIndex = 4;
            depositLabel.Text = "Deposit";
            // 
            // estimatedTotalTextBox
            // 
            estimatedTotalTextBox.BorderStyle = BorderStyle.FixedSingle;
            estimatedTotalTextBox.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            estimatedTotalTextBox.Location = new Point(208, 333);
            estimatedTotalTextBox.Margin = new Padding(3, 4, 3, 4);
            estimatedTotalTextBox.Name = "estimatedTotalTextBox";
            estimatedTotalTextBox.ReadOnly = true;
            estimatedTotalTextBox.Size = new Size(160, 28);
            estimatedTotalTextBox.TabIndex = 3;
            // 
            // estimatedTotalLabel
            // 
            estimatedTotalLabel.AutoSize = true;
            estimatedTotalLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            estimatedTotalLabel.ForeColor = Color.FromArgb(31, 41, 55);
            estimatedTotalLabel.Location = new Point(208, 307);
            estimatedTotalLabel.Name = "estimatedTotalLabel";
            estimatedTotalLabel.Size = new Size(119, 22);
            estimatedTotalLabel.TabIndex = 2;
            estimatedTotalLabel.Text = "Estimated Total";
            // 
            // rateTextBox
            // 
            rateTextBox.BorderStyle = BorderStyle.FixedSingle;
            rateTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rateTextBox.Location = new Point(25, 333);
            rateTextBox.Margin = new Padding(3, 4, 3, 4);
            rateTextBox.Name = "rateTextBox";
            rateTextBox.ReadOnly = true;
            rateTextBox.Size = new Size(160, 28);
            rateTextBox.TabIndex = 1;
            rateTextBox.TextChanged += DateOrRate_Changed;
            // 
            // rateLabel
            // 
            rateLabel.AutoSize = true;
            rateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            rateLabel.Location = new Point(25, 307);
            rateLabel.Name = "rateLabel";
            rateLabel.Size = new Size(81, 22);
            rateLabel.TabIndex = 0;
            rateLabel.Text = "Daily Rate";
            // 
            // returnDatePicker
            // 
            returnDatePicker.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            returnDatePicker.Format = DateTimePickerFormat.Short;
            returnDatePicker.Location = new Point(208, 259);
            returnDatePicker.Margin = new Padding(3, 4, 3, 4);
            returnDatePicker.Name = "returnDatePicker";
            returnDatePicker.Size = new Size(160, 28);
            returnDatePicker.TabIndex = 23;
            returnDatePicker.ValueChanged += DateOrRate_Changed;
            // 
            // returnDateLabel
            // 
            returnDateLabel.AutoSize = true;
            returnDateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            returnDateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            returnDateLabel.Location = new Point(208, 232);
            returnDateLabel.Name = "returnDateLabel";
            returnDateLabel.Size = new Size(129, 22);
            returnDateLabel.TabIndex = 22;
            returnDateLabel.Text = "Expected Return";
            // 
            // pickupDatePicker
            // 
            pickupDatePicker.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pickupDatePicker.Format = DateTimePickerFormat.Short;
            pickupDatePicker.Location = new Point(25, 259);
            pickupDatePicker.Margin = new Padding(3, 4, 3, 4);
            pickupDatePicker.Name = "pickupDatePicker";
            pickupDatePicker.Size = new Size(160, 28);
            pickupDatePicker.TabIndex = 21;
            pickupDatePicker.ValueChanged += DateOrRate_Changed;
            // 
            // pickupDateLabel
            // 
            pickupDateLabel.AutoSize = true;
            pickupDateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pickupDateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            pickupDateLabel.Location = new Point(25, 232);
            pickupDateLabel.Name = "pickupDateLabel";
            pickupDateLabel.Size = new Size(96, 22);
            pickupDateLabel.TabIndex = 20;
            pickupDateLabel.Text = "Pickup Date";
            // 
            // carComboBox
            // 
            carComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            carComboBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            carComboBox.FormattingEnabled = true;
            carComboBox.Location = new Point(25, 184);
            carComboBox.Margin = new Padding(3, 4, 3, 4);
            carComboBox.Name = "carComboBox";
            carComboBox.Size = new Size(343, 32);
            carComboBox.TabIndex = 17;
            carComboBox.SelectedIndexChanged += CarComboBox_SelectedIndexChanged;
            // 
            // carLabel
            // 
            carLabel.AutoSize = true;
            carLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            carLabel.ForeColor = Color.FromArgb(31, 41, 55);
            carLabel.Location = new Point(25, 157);
            carLabel.Name = "carLabel";
            carLabel.Size = new Size(35, 22);
            carLabel.TabIndex = 16;
            carLabel.Text = "Car";
            // 
            // contactTextBox
            // 
            contactTextBox.BorderStyle = BorderStyle.FixedSingle;
            contactTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            contactTextBox.Location = new Point(208, 109);
            contactTextBox.Margin = new Padding(3, 4, 3, 4);
            contactTextBox.Name = "contactTextBox";
            contactTextBox.PlaceholderText = "Contact number";
            contactTextBox.Size = new Size(160, 28);
            contactTextBox.TabIndex = 15;
            // 
            // contactLabel
            // 
            contactLabel.AutoSize = true;
            contactLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            contactLabel.ForeColor = Color.FromArgb(31, 41, 55);
            contactLabel.Location = new Point(208, 83);
            contactLabel.Name = "contactLabel";
            contactLabel.Size = new Size(66, 22);
            contactLabel.TabIndex = 14;
            contactLabel.Text = "Contact";
            // 
            // customerNameTextBox
            // 
            customerNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            customerNameTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customerNameTextBox.Location = new Point(25, 109);
            customerNameTextBox.Margin = new Padding(3, 4, 3, 4);
            customerNameTextBox.Name = "customerNameTextBox";
            customerNameTextBox.PlaceholderText = "Customer name";
            customerNameTextBox.Size = new Size(160, 28);
            customerNameTextBox.TabIndex = 13;
            // 
            // customerNameLabel
            // 
            customerNameLabel.AutoSize = true;
            customerNameLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customerNameLabel.ForeColor = Color.FromArgb(31, 41, 55);
            customerNameLabel.Location = new Point(25, 83);
            customerNameLabel.Name = "customerNameLabel";
            customerNameLabel.Size = new Size(51, 22);
            customerNameLabel.TabIndex = 12;
            customerNameLabel.Text = "Name";
            // 
            // detailsTitleLabel
            // 
            detailsTitleLabel.AutoSize = true;
            detailsTitleLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            detailsTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            detailsTitleLabel.Location = new Point(23, 24);
            detailsTitleLabel.Name = "detailsTitleLabel";
            detailsTitleLabel.Size = new Size(236, 32);
            detailsTitleLabel.TabIndex = 11;
            detailsTitleLabel.Text = "New Reservation";
            // 
            // recordsPanel
            // 
            recordsPanel.BackColor = Color.White;
            recordsPanel.Controls.Add(reservationsGrid);
            recordsPanel.Controls.Add(recordsHeaderPanel);
            recordsPanel.Dock = DockStyle.Fill;
            recordsPanel.Location = new Point(443, 37);
            recordsPanel.Margin = new Padding(0);
            recordsPanel.Name = "recordsPanel";
            recordsPanel.Size = new Size(759, 694);
            recordsPanel.TabIndex = 1;
            // 
            // reservationsGrid
            // 
            reservationsGrid.AllowUserToAddRows = false;
            reservationsGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 251);
            reservationsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            reservationsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            reservationsGrid.BackgroundColor = Color.White;
            reservationsGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            reservationsGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            reservationsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            reservationsGrid.ColumnHeadersHeight = 56;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            reservationsGrid.DefaultCellStyle = dataGridViewCellStyle3;
            reservationsGrid.Dock = DockStyle.Fill;
            reservationsGrid.EnableHeadersVisualStyles = false;
            reservationsGrid.GridColor = Color.FromArgb(229, 231, 235);
            reservationsGrid.Location = new Point(0, 77);
            reservationsGrid.Margin = new Padding(3, 4, 3, 4);
            reservationsGrid.MultiSelect = false;
            reservationsGrid.Name = "reservationsGrid";
            reservationsGrid.ReadOnly = true;
            reservationsGrid.RowHeadersVisible = false;
            reservationsGrid.RowHeadersWidth = 51;
            reservationsGrid.RowTemplate.Height = 42;
            reservationsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            reservationsGrid.Size = new Size(759, 617);
            reservationsGrid.TabIndex = 1;
            reservationsGrid.SelectionChanged += ReservationsGrid_SelectionChanged;
            // 
            // recordsHeaderPanel
            // 
            recordsHeaderPanel.BorderStyle = BorderStyle.FixedSingle;
            recordsHeaderPanel.Controls.Add(searchTextBox);
            recordsHeaderPanel.Controls.Add(titleLabel);
            recordsHeaderPanel.Dock = DockStyle.Top;
            recordsHeaderPanel.Location = new Point(0, 0);
            recordsHeaderPanel.Margin = new Padding(3, 4, 3, 4);
            recordsHeaderPanel.Name = "recordsHeaderPanel";
            recordsHeaderPanel.Size = new Size(759, 77);
            recordsHeaderPanel.TabIndex = 0;
            // 
            // searchTextBox
            // 
            searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            searchTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchTextBox.Location = new Point(412, 24);
            searchTextBox.Margin = new Padding(3, 4, 3, 4);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Search reservations";
            searchTextBox.Size = new Size(320, 28);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            titleLabel.Location = new Point(0, 21);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(258, 32);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Reservation Records";
            // 
            // Reservations
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
            Name = "Reservations";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Avenue - Reservations";
            Load += Reservations_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            bodyTable.ResumeLayout(false);
            detailsPanel.ResumeLayout(false);
            detailsPanel.PerformLayout();
            recordsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)reservationsGrid).EndInit();
            recordsHeaderPanel.ResumeLayout(false);
            recordsHeaderPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel headerPanel;
        private Label helperLabel;
        private Label pageTitleLabel;
        private TableLayoutPanel bodyTable;
        private Panel detailsPanel;
        private Button clearButton;
        private Button convertButton;
        private Button cancelReservationButton;
        private Button updateButton;
        private Button addButton;
        private TextBox paymentReferenceTextBox;
        private Label paymentReferenceLabel;
        private ComboBox paymentMethodComboBox;
        private Label paymentMethodLabel;
        private TextBox balanceTextBox;
        private Label balanceLabel;
        private TextBox depositTextBox;
        private Label depositLabel;
        private TextBox estimatedTotalTextBox;
        private Label estimatedTotalLabel;
        private TextBox rateTextBox;
        private Label rateLabel;
        private DateTimePicker returnDatePicker;
        private Label returnDateLabel;
        private DateTimePicker pickupDatePicker;
        private Label pickupDateLabel;
        private ComboBox carComboBox;
        private Label carLabel;
        private TextBox contactTextBox;
        private Label contactLabel;
        private TextBox customerNameTextBox;
        private Label customerNameLabel;
        private Label detailsTitleLabel;
        private Panel recordsPanel;
        private DataGridView reservationsGrid;
        private Panel recordsHeaderPanel;
        private TextBox searchTextBox;
        private Label titleLabel;
    }
}
