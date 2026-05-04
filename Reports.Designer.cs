namespace Car_Rental_System
{
    partial class Reports
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
            refreshButton = new Button();
            helperLabel = new Label();
            titleLabel = new Label();
            contentPanel = new Panel();
            reportPanel = new Panel();
            reportGrid = new DataGridView();
            reportHeaderPanel = new Panel();
            searchTextBox = new TextBox();
            historyLabel = new Label();
            statsTable = new TableLayoutPanel();
            totalCarsCard = new Panel();
            totalCarsValueLabel = new Label();
            totalCarsTextLabel = new Label();
            availableCarsCard = new Panel();
            availableCarsValueLabel = new Label();
            availableCarsTextLabel = new Label();
            activeRentalsCard = new Panel();
            activeRentalsValueLabel = new Label();
            activeRentalsTextLabel = new Label();
            incomeCard = new Panel();
            incomeValueLabel = new Label();
            incomeTextLabel = new Label();
            headerPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            reportPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)reportGrid).BeginInit();
            reportHeaderPanel.SuspendLayout();
            statsTable.SuspendLayout();
            totalCarsCard.SuspendLayout();
            availableCarsCard.SuspendLayout();
            activeRentalsCard.SuspendLayout();
            incomeCard.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.White;
            headerPanel.Controls.Add(refreshButton);
            headerPanel.Controls.Add(helperLabel);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Margin = new Padding(3, 4, 3, 4);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(1234, 139);
            headerPanel.TabIndex = 0;
            // 
            // refreshButton
            // 
            refreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            refreshButton.BackColor = Color.FromArgb(0, 123, 255);
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 123, 255);
            refreshButton.FlatStyle = FlatStyle.Flat;
            refreshButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            refreshButton.ForeColor = Color.White;
            refreshButton.Location = new Point(1051, 47);
            refreshButton.Margin = new Padding(3, 4, 3, 4);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(137, 45);
            refreshButton.TabIndex = 2;
            refreshButton.Text = "Refresh";
            refreshButton.UseVisualStyleBackColor = false;
            refreshButton.Click += RefreshButton_Click;
            // 
            // helperLabel
            // 
            helperLabel.AutoSize = true;
            helperLabel.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            helperLabel.ForeColor = Color.FromArgb(52, 58, 64);
            helperLabel.Location = new Point(35, 85);
            helperLabel.Name = "helperLabel";
            helperLabel.Size = new Size(378, 24);
            helperLabel.TabIndex = 1;
            helperLabel.Text = "Review inventory, active rentals, and income.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(52, 58, 64);
            titleLabel.Location = new Point(32, 32);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(165, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Reports";
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.White;
            contentPanel.Controls.Add(reportPanel);
            contentPanel.Controls.Add(statsTable);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(0, 139);
            contentPanel.Margin = new Padding(3, 4, 3, 4);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(32, 37, 32, 37);
            contentPanel.Size = new Size(1234, 768);
            contentPanel.TabIndex = 1;
            // 
            // reportPanel
            // 
            reportPanel.BackColor = Color.White;
            reportPanel.Controls.Add(reportGrid);
            reportPanel.Controls.Add(reportHeaderPanel);
            reportPanel.Dock = DockStyle.Fill;
            reportPanel.Location = new Point(32, 205);
            reportPanel.Margin = new Padding(3, 4, 3, 4);
            reportPanel.Name = "reportPanel";
            reportPanel.Size = new Size(1170, 526);
            reportPanel.TabIndex = 1;
            // 
            // reportGrid
            // 
            reportGrid.AllowUserToAddRows = false;
            reportGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            reportGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            reportGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            reportGrid.BackgroundColor = Color.White;
            reportGrid.BorderStyle = BorderStyle.Fixed3D;
            reportGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            reportGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            reportGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            reportGrid.ColumnHeadersHeight = 36;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            reportGrid.DefaultCellStyle = dataGridViewCellStyle3;
            reportGrid.Dock = DockStyle.Fill;
            reportGrid.EnableHeadersVisualStyles = false;
            reportGrid.GridColor = Color.FromArgb(52, 58, 64);
            reportGrid.Location = new Point(0, 85);
            reportGrid.Margin = new Padding(3, 4, 3, 4);
            reportGrid.Name = "reportGrid";
            reportGrid.ReadOnly = true;
            reportGrid.RowHeadersVisible = false;
            reportGrid.RowHeadersWidth = 51;
            reportGrid.RowTemplate.Height = 38;
            reportGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            reportGrid.Size = new Size(1170, 441);
            reportGrid.TabIndex = 1;
            // 
            // reportHeaderPanel
            // 
            reportHeaderPanel.BorderStyle = BorderStyle.Fixed3D;
            reportHeaderPanel.Controls.Add(searchTextBox);
            reportHeaderPanel.Controls.Add(historyLabel);
            reportHeaderPanel.Dock = DockStyle.Top;
            reportHeaderPanel.Location = new Point(0, 0);
            reportHeaderPanel.Margin = new Padding(3, 4, 3, 4);
            reportHeaderPanel.Name = "reportHeaderPanel";
            reportHeaderPanel.Size = new Size(1170, 85);
            reportHeaderPanel.TabIndex = 0;
            // 
            // searchTextBox
            // 
            searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            searchTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchTextBox.Location = new Point(801, 27);
            searchTextBox.Margin = new Padding(3, 4, 3, 4);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Search rental history";
            searchTextBox.Size = new Size(331, 28);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // historyLabel
            // 
            historyLabel.AutoSize = true;
            historyLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            historyLabel.ForeColor = Color.FromArgb(52, 58, 64);
            historyLabel.Location = new Point(23, 27);
            historyLabel.Name = "historyLabel";
            historyLabel.Size = new Size(171, 32);
            historyLabel.TabIndex = 0;
            historyLabel.Text = "Rental History";
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
            statsTable.Controls.Add(activeRentalsCard, 2, 0);
            statsTable.Controls.Add(incomeCard, 3, 0);
            statsTable.Dock = DockStyle.Top;
            statsTable.Location = new Point(32, 37);
            statsTable.Margin = new Padding(3, 4, 3, 4);
            statsTable.Name = "statsTable";
            statsTable.RowCount = 1;
            statsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            statsTable.Size = new Size(1170, 168);
            statsTable.TabIndex = 0;
            // 
            // totalCarsCard
            // 
            totalCarsCard.BackColor = Color.White;
            totalCarsCard.BorderStyle = BorderStyle.Fixed3D;
            totalCarsCard.Controls.Add(totalCarsValueLabel);
            totalCarsCard.Controls.Add(totalCarsTextLabel);
            totalCarsCard.Dock = DockStyle.Fill;
            totalCarsCard.Location = new Point(0, 0);
            totalCarsCard.Margin = new Padding(0, 0, 14, 27);
            totalCarsCard.Name = "totalCarsCard";
            totalCarsCard.Size = new Size(278, 141);
            totalCarsCard.TabIndex = 0;
            // 
            // totalCarsValueLabel
            // 
            totalCarsValueLabel.AutoSize = true;
            totalCarsValueLabel.Font = new Font("Inter SemiBold", 22.2F, FontStyle.Bold);
            totalCarsValueLabel.ForeColor = Color.FromArgb(0, 123, 255);
            totalCarsValueLabel.Location = new Point(21, 60);
            totalCarsValueLabel.Name = "totalCarsValueLabel";
            totalCarsValueLabel.Size = new Size(47, 53);
            totalCarsValueLabel.TabIndex = 1;
            totalCarsValueLabel.Text = "0";
            // 
            // totalCarsTextLabel
            // 
            totalCarsTextLabel.AutoSize = true;
            totalCarsTextLabel.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalCarsTextLabel.ForeColor = Color.FromArgb(52, 58, 64);
            totalCarsTextLabel.Location = new Point(23, 27);
            totalCarsTextLabel.Name = "totalCarsTextLabel";
            totalCarsTextLabel.Size = new Size(92, 24);
            totalCarsTextLabel.TabIndex = 0;
            totalCarsTextLabel.Text = "Total Cars";
            // 
            // availableCarsCard
            // 
            availableCarsCard.BackColor = Color.White;
            availableCarsCard.BorderStyle = BorderStyle.Fixed3D;
            availableCarsCard.Controls.Add(availableCarsValueLabel);
            availableCarsCard.Controls.Add(availableCarsTextLabel);
            availableCarsCard.Dock = DockStyle.Fill;
            availableCarsCard.Location = new Point(292, 0);
            availableCarsCard.Margin = new Padding(0, 0, 14, 27);
            availableCarsCard.Name = "availableCarsCard";
            availableCarsCard.Size = new Size(278, 141);
            availableCarsCard.TabIndex = 1;
            // 
            // availableCarsValueLabel
            // 
            availableCarsValueLabel.AutoSize = true;
            availableCarsValueLabel.Font = new Font("Inter SemiBold", 22.2F, FontStyle.Bold);
            availableCarsValueLabel.ForeColor = Color.FromArgb(40, 167, 69);
            availableCarsValueLabel.Location = new Point(21, 60);
            availableCarsValueLabel.Name = "availableCarsValueLabel";
            availableCarsValueLabel.Size = new Size(47, 53);
            availableCarsValueLabel.TabIndex = 1;
            availableCarsValueLabel.Text = "0";
            // 
            // availableCarsTextLabel
            // 
            availableCarsTextLabel.AutoSize = true;
            availableCarsTextLabel.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            availableCarsTextLabel.ForeColor = Color.FromArgb(52, 58, 64);
            availableCarsTextLabel.Location = new Point(23, 27);
            availableCarsTextLabel.Name = "availableCarsTextLabel";
            availableCarsTextLabel.Size = new Size(125, 24);
            availableCarsTextLabel.TabIndex = 0;
            availableCarsTextLabel.Text = "Available Cars";
            // 
            // activeRentalsCard
            // 
            activeRentalsCard.BackColor = Color.White;
            activeRentalsCard.BorderStyle = BorderStyle.Fixed3D;
            activeRentalsCard.Controls.Add(activeRentalsValueLabel);
            activeRentalsCard.Controls.Add(activeRentalsTextLabel);
            activeRentalsCard.Dock = DockStyle.Fill;
            activeRentalsCard.Location = new Point(584, 0);
            activeRentalsCard.Margin = new Padding(0, 0, 14, 27);
            activeRentalsCard.Name = "activeRentalsCard";
            activeRentalsCard.Size = new Size(278, 141);
            activeRentalsCard.TabIndex = 2;
            // 
            // activeRentalsValueLabel
            // 
            activeRentalsValueLabel.AutoSize = true;
            activeRentalsValueLabel.Font = new Font("Inter SemiBold", 22.2F, FontStyle.Bold);
            activeRentalsValueLabel.ForeColor = Color.FromArgb(255, 204, 0);
            activeRentalsValueLabel.Location = new Point(21, 60);
            activeRentalsValueLabel.Name = "activeRentalsValueLabel";
            activeRentalsValueLabel.Size = new Size(47, 53);
            activeRentalsValueLabel.TabIndex = 1;
            activeRentalsValueLabel.Text = "0";
            // 
            // activeRentalsTextLabel
            // 
            activeRentalsTextLabel.AutoSize = true;
            activeRentalsTextLabel.Font = new Font("Inter", 10.2F);
            activeRentalsTextLabel.ForeColor = Color.FromArgb(52, 58, 64);
            activeRentalsTextLabel.Location = new Point(23, 27);
            activeRentalsTextLabel.Name = "activeRentalsTextLabel";
            activeRentalsTextLabel.Size = new Size(127, 24);
            activeRentalsTextLabel.TabIndex = 0;
            activeRentalsTextLabel.Text = "Active Rentals";
            // 
            // incomeCard
            // 
            incomeCard.BackColor = Color.White;
            incomeCard.BorderStyle = BorderStyle.Fixed3D;
            incomeCard.Controls.Add(incomeValueLabel);
            incomeCard.Controls.Add(incomeTextLabel);
            incomeCard.Dock = DockStyle.Fill;
            incomeCard.Location = new Point(876, 0);
            incomeCard.Margin = new Padding(0, 0, 0, 27);
            incomeCard.Name = "incomeCard";
            incomeCard.Size = new Size(294, 141);
            incomeCard.TabIndex = 3;
            // 
            // incomeValueLabel
            // 
            incomeValueLabel.AutoSize = true;
            incomeValueLabel.Font = new Font("Inter SemiBold", 22.2F, FontStyle.Bold);
            incomeValueLabel.ForeColor = Color.FromArgb(52, 58, 64);
            incomeValueLabel.Location = new Point(21, 64);
            incomeValueLabel.Name = "incomeValueLabel";
            incomeValueLabel.Size = new Size(138, 53);
            incomeValueLabel.TabIndex = 1;
            incomeValueLabel.Text = "P 0.00";
            // 
            // incomeTextLabel
            // 
            incomeTextLabel.AutoSize = true;
            incomeTextLabel.Font = new Font("Inter", 10.2F);
            incomeTextLabel.ForeColor = Color.FromArgb(52, 58, 64);
            incomeTextLabel.Location = new Point(23, 27);
            incomeTextLabel.Name = "incomeTextLabel";
            incomeTextLabel.Size = new Size(148, 24);
            incomeTextLabel.TabIndex = 0;
            incomeTextLabel.Text = "Returned Income";
            // 
            // Reports
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1234, 907);
            Controls.Add(contentPanel);
            Controls.Add(headerPanel);
            Font = new Font("Segoe UI", 9F);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1117, 811);
            Name = "Reports";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Avenue - Reports";
            Load += Reports_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            contentPanel.ResumeLayout(false);
            reportPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)reportGrid).EndInit();
            reportHeaderPanel.ResumeLayout(false);
            reportHeaderPanel.PerformLayout();
            statsTable.ResumeLayout(false);
            totalCarsCard.ResumeLayout(false);
            totalCarsCard.PerformLayout();
            availableCarsCard.ResumeLayout(false);
            availableCarsCard.PerformLayout();
            activeRentalsCard.ResumeLayout(false);
            activeRentalsCard.PerformLayout();
            incomeCard.ResumeLayout(false);
            incomeCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel headerPanel;
        private Button refreshButton;
        private Label helperLabel;
        private Label titleLabel;
        private Panel contentPanel;
        private Panel reportPanel;
        private DataGridView reportGrid;
        private Panel reportHeaderPanel;
        private TextBox searchTextBox;
        private Label historyLabel;
        private TableLayoutPanel statsTable;
        private Panel totalCarsCard;
        private Label totalCarsValueLabel;
        private Label totalCarsTextLabel;
        private Panel availableCarsCard;
        private Label availableCarsValueLabel;
        private Label availableCarsTextLabel;
        private Panel activeRentalsCard;
        private Label activeRentalsValueLabel;
        private Label activeRentalsTextLabel;
        private Panel incomeCard;
        private Label incomeValueLabel;
        private Label incomeTextLabel;
    }
}
