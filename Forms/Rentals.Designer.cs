namespace Car_Rental_System
{
    partial class Rentals
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            headerPanel = new Panel();
            helperLabel = new Label();
            titleLabel = new Label();
            bodyTable = new TableLayoutPanel();
            detailsPanel = new Panel();
            clearButton = new Button();
            createButton = new Button();
            totalTextBox = new TextBox();
            totalLabel = new Label();
            rateTextBox = new TextBox();
            rateLabel = new Label();
            returnDatePicker = new DateTimePicker();
            returnDateLabel = new Label();
            rentalDatePicker = new DateTimePicker();
            rentalDateLabel = new Label();
            carComboBox = new ComboBox();
            carLabel = new Label();
            customerComboBox = new ComboBox();
            customerLabel = new Label();
            sectionLabel = new Label();
            listPanel = new Panel();
            rentalsGrid = new DataGridView();
            listHeaderPanel = new Panel();
            searchTextBox = new TextBox();
            activeLabel = new Label();
            headerPanel.SuspendLayout();
            bodyTable.SuspendLayout();
            detailsPanel.SuspendLayout();
            listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rentalsGrid).BeginInit();
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
            helperLabel.Size = new Size(526, 24);
            helperLabel.TabIndex = 1;
            helperLabel.Text = "Create rentals, compute charges, and track active transactions.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            titleLabel.Location = new Point(32, 32);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(381, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Rental Transactions";
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
            detailsPanel.Controls.Add(clearButton);
            detailsPanel.Controls.Add(createButton);
            detailsPanel.Controls.Add(totalTextBox);
            detailsPanel.Controls.Add(totalLabel);
            detailsPanel.Controls.Add(rateTextBox);
            detailsPanel.Controls.Add(rateLabel);
            detailsPanel.Controls.Add(returnDatePicker);
            detailsPanel.Controls.Add(returnDateLabel);
            detailsPanel.Controls.Add(rentalDatePicker);
            detailsPanel.Controls.Add(rentalDateLabel);
            detailsPanel.Controls.Add(carComboBox);
            detailsPanel.Controls.Add(carLabel);
            detailsPanel.Controls.Add(customerComboBox);
            detailsPanel.Controls.Add(customerLabel);
            detailsPanel.Controls.Add(sectionLabel);
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
            clearButton.Location = new Point(25, 457);
            clearButton.Margin = new Padding(3, 4, 3, 4);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(343, 45);
            clearButton.TabIndex = 17;
            clearButton.Text = "Clear";
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += ClearButton_Click;
            // 
            // createButton
            // 
            createButton.BackColor = Color.FromArgb(37, 99, 235);
            createButton.FlatAppearance.BorderSize = 0;
            createButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            createButton.FlatStyle = FlatStyle.Flat;
            createButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            createButton.ForeColor = Color.White;
            createButton.Location = new Point(23, 392);
            createButton.Margin = new Padding(3, 4, 3, 4);
            createButton.Name = "createButton";
            createButton.Size = new Size(342, 45);
            createButton.TabIndex = 15;
            createButton.Text = "Create Rental";
            createButton.UseVisualStyleBackColor = false;
            createButton.Click += CreateButton_Click;
            // 
            // totalTextBox
            // 
            totalTextBox.BorderStyle = BorderStyle.FixedSingle;
            totalTextBox.Font = new Font("Inter", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalTextBox.Location = new Point(208, 333);
            totalTextBox.Margin = new Padding(3, 4, 3, 4);
            totalTextBox.Name = "totalTextBox";
            totalTextBox.ReadOnly = true;
            totalTextBox.Size = new Size(160, 28);
            totalTextBox.TabIndex = 12;
            totalTextBox.Text = "P 2,500";
            // 
            // totalLabel
            // 
            totalLabel.AutoSize = true;
            totalLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalLabel.ForeColor = Color.FromArgb(31, 41, 55);
            totalLabel.Location = new Point(208, 307);
            totalLabel.Name = "totalLabel";
            totalLabel.Size = new Size(104, 22);
            totalLabel.TabIndex = 11;
            totalLabel.Text = "Total Amount";
            // 
            // rateTextBox
            // 
            rateTextBox.BorderStyle = BorderStyle.FixedSingle;
            rateTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rateTextBox.Location = new Point(25, 333);
            rateTextBox.Margin = new Padding(3, 4, 3, 4);
            rateTextBox.Name = "rateTextBox";
            rateTextBox.PlaceholderText = "2500";
            rateTextBox.Size = new Size(160, 28);
            rateTextBox.TabIndex = 10;
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
            rateLabel.TabIndex = 9;
            rateLabel.Text = "Daily Rate";
            // 
            // returnDatePicker
            // 
            returnDatePicker.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            returnDatePicker.Format = DateTimePickerFormat.Short;
            returnDatePicker.Location = new Point(208, 259);
            returnDatePicker.Margin = new Padding(3, 4, 3, 4);
            returnDatePicker.Name = "returnDatePicker";
            returnDatePicker.Size = new Size(159, 28);
            returnDatePicker.TabIndex = 8;
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
            returnDateLabel.TabIndex = 7;
            returnDateLabel.Text = "Expected Return";
            // 
            // rentalDatePicker
            // 
            rentalDatePicker.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rentalDatePicker.Format = DateTimePickerFormat.Short;
            rentalDatePicker.Location = new Point(25, 259);
            rentalDatePicker.Margin = new Padding(3, 4, 3, 4);
            rentalDatePicker.Name = "rentalDatePicker";
            rentalDatePicker.Size = new Size(159, 28);
            rentalDatePicker.TabIndex = 6;
            rentalDatePicker.ValueChanged += DateOrRate_Changed;
            // 
            // rentalDateLabel
            // 
            rentalDateLabel.AutoSize = true;
            rentalDateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rentalDateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            rentalDateLabel.Location = new Point(25, 232);
            rentalDateLabel.Name = "rentalDateLabel";
            rentalDateLabel.Size = new Size(92, 22);
            rentalDateLabel.TabIndex = 5;
            rentalDateLabel.Text = "Rental Date";
            // 
            // carComboBox
            // 
            carComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            carComboBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            carComboBox.FormattingEnabled = true;
            carComboBox.Location = new Point(25, 184);
            carComboBox.Margin = new Padding(3, 4, 3, 4);
            carComboBox.Name = "carComboBox";
            carComboBox.Size = new Size(342, 32);
            carComboBox.TabIndex = 4;
            carComboBox.SelectedIndexChanged += CarComboBox_SelectedIndexChanged;
            // 
            // carLabel
            // 
            carLabel.AutoSize = true;
            carLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            carLabel.ForeColor = Color.FromArgb(31, 41, 55);
            carLabel.Location = new Point(25, 157);
            carLabel.Name = "carLabel";
            carLabel.Size = new Size(102, 22);
            carLabel.TabIndex = 3;
            carLabel.Text = "Available Car";
            // 
            // customerComboBox
            // 
            customerComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            customerComboBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customerComboBox.FormattingEnabled = true;
            customerComboBox.Location = new Point(25, 109);
            customerComboBox.Margin = new Padding(3, 4, 3, 4);
            customerComboBox.Name = "customerComboBox";
            customerComboBox.Size = new Size(342, 32);
            customerComboBox.TabIndex = 2;
            // 
            // customerLabel
            // 
            customerLabel.AutoSize = true;
            customerLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customerLabel.ForeColor = Color.FromArgb(31, 41, 55);
            customerLabel.Location = new Point(25, 83);
            customerLabel.Name = "customerLabel";
            customerLabel.Size = new Size(80, 22);
            customerLabel.TabIndex = 1;
            customerLabel.Text = "Customer";
            // 
            // sectionLabel
            // 
            sectionLabel.AutoSize = true;
            sectionLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            sectionLabel.ForeColor = Color.FromArgb(31, 41, 55);
            sectionLabel.Location = new Point(23, 24);
            sectionLabel.Name = "sectionLabel";
            sectionLabel.Size = new Size(140, 32);
            sectionLabel.TabIndex = 0;
            sectionLabel.Text = "New Rental";
            // 
            // listPanel
            // 
            listPanel.BackColor = Color.White;
            listPanel.Controls.Add(rentalsGrid);
            listPanel.Controls.Add(listHeaderPanel);
            listPanel.Dock = DockStyle.Fill;
            listPanel.Location = new Point(443, 37);
            listPanel.Margin = new Padding(0);
            listPanel.Name = "listPanel";
            listPanel.Size = new Size(759, 694);
            listPanel.TabIndex = 1;
            // 
            // rentalsGrid
            // 
            rentalsGrid.AllowUserToAddRows = false;
            rentalsGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 251);
            rentalsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            rentalsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            rentalsGrid.BackgroundColor = Color.White;
            rentalsGrid.BorderStyle = BorderStyle.FixedSingle;
            rentalsGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            rentalsGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            rentalsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            rentalsGrid.ColumnHeadersHeight = 36;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            rentalsGrid.DefaultCellStyle = dataGridViewCellStyle3;
            rentalsGrid.Dock = DockStyle.Fill;
            rentalsGrid.EnableHeadersVisualStyles = false;
            rentalsGrid.GridColor = Color.FromArgb(229, 231, 235);
            rentalsGrid.Location = new Point(0, 77);
            rentalsGrid.Margin = new Padding(3, 4, 3, 4);
            rentalsGrid.Name = "rentalsGrid";
            rentalsGrid.ReadOnly = true;
            rentalsGrid.RowHeadersVisible = false;
            rentalsGrid.RowHeadersWidth = 51;
            rentalsGrid.RowTemplate.Height = 42;
            rentalsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            rentalsGrid.Size = new Size(759, 617);
            rentalsGrid.TabIndex = 1;
            rentalsGrid.SelectionChanged += RentalsGrid_SelectionChanged;
            // 
            // listHeaderPanel
            // 
            listHeaderPanel.BorderStyle = BorderStyle.FixedSingle;
            listHeaderPanel.Controls.Add(searchTextBox);
            listHeaderPanel.Controls.Add(activeLabel);
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
            searchTextBox.Location = new Point(412, 24);
            searchTextBox.Margin = new Padding(3, 4, 3, 4);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Search rental, customer, or car";
            searchTextBox.Size = new Size(320, 28);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // activeLabel
            // 
            activeLabel.AutoSize = true;
            activeLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activeLabel.ForeColor = Color.FromArgb(31, 41, 55);
            activeLabel.Location = new Point(0, 21);
            activeLabel.Name = "activeLabel";
            activeLabel.Size = new Size(174, 32);
            activeLabel.TabIndex = 0;
            activeLabel.Text = "Active Rentals";
            // 
            // Rentals
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
            Name = "Rentals";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Avenue - Rentals";
            Load += Rentals_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            bodyTable.ResumeLayout(false);
            detailsPanel.ResumeLayout(false);
            detailsPanel.PerformLayout();
            listPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)rentalsGrid).EndInit();
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
        private Button clearButton;
        private Button createButton;
        private TextBox totalTextBox;
        private Label totalLabel;
        private TextBox rateTextBox;
        private Label rateLabel;
        private DateTimePicker returnDatePicker;
        private Label returnDateLabel;
        private DateTimePicker rentalDatePicker;
        private Label rentalDateLabel;
        private ComboBox carComboBox;
        private Label carLabel;
        private ComboBox customerComboBox;
        private Label customerLabel;
        private Label sectionLabel;
        private Panel listPanel;
        private DataGridView rentalsGrid;
        private Panel listHeaderPanel;
        private TextBox searchTextBox;
        private Label activeLabel;
    }
}
