using Guna.UI2.WinForms;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reports));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            headerPanel = new Guna2Panel();
            refreshButton = new Guna2Button();
            helperLabel = new Label();
            titleLabel = new Label();
            contentPanel = new Guna2Panel();
            reportPanel = new Guna2Panel();
            reportGrid = new Guna2DataGridView();
            reportHeaderPanel = new Guna2Panel();
            searchTextBox = new Guna2TextBox();
            historyLabel = new Label();
            statsTable = new TableLayoutPanel();
            totalCarsCard = new Guna2Panel();
            totalCarsValueLabel = new Label();
            totalCarsTextLabel = new Label();
            availableCarsCard = new Guna2Panel();
            availableCarsValueLabel = new Label();
            availableCarsTextLabel = new Label();
            activeRentalsCard = new Guna2Panel();
            activeRentalsValueLabel = new Label();
            activeRentalsTextLabel = new Label();
            incomeCard = new Guna2Panel();
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
            headerPanel.CustomizableEdges = customizableEdges3;
            headerPanel.Dock = DockStyle.Top;
            headerPanel.FillColor = Color.White;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Margin = new Padding(3, 4, 3, 4);
            headerPanel.Name = "headerPanel";
            headerPanel.ShadowDecoration.CustomizableEdges = customizableEdges4;
            headerPanel.Size = new Size(1234, 139);
            headerPanel.TabIndex = 0;
            // 
            // refreshButton
            // 
            refreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            refreshButton.BackColor = Color.White;
            refreshButton.BorderRadius = 8;
            refreshButton.CustomizableEdges = customizableEdges1;
            refreshButton.FillColor = Color.FromArgb(37, 99, 235);
            refreshButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            refreshButton.ForeColor = Color.White;
            refreshButton.HoverState.FillColor = Color.FromArgb(29, 78, 216);
            refreshButton.Image = (Image)resources.GetObject("refreshButton.Image");
            refreshButton.ImageAlign = HorizontalAlignment.Left;
            refreshButton.ImageOffset = new Point(15, 0);
            refreshButton.ImageSize = new Size(22, 22);
            refreshButton.Location = new Point(1051, 47);
            refreshButton.Margin = new Padding(3, 4, 3, 4);
            refreshButton.Name = "refreshButton";
            refreshButton.ShadowDecoration.CustomizableEdges = customizableEdges2;
            refreshButton.Size = new Size(137, 45);
            refreshButton.TabIndex = 2;
            refreshButton.Text = "Refresh";
            refreshButton.TextAlign = HorizontalAlignment.Left;
            refreshButton.TextOffset = new Point(8, 0);
            refreshButton.Click += RefreshButton_Click;
            // 
            // helperLabel
            // 
            helperLabel.AutoSize = true;
            helperLabel.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            helperLabel.ForeColor = Color.FromArgb(31, 41, 55);
            helperLabel.Location = new Point(35, 85);
            helperLabel.Name = "helperLabel";
            helperLabel.Size = new Size(478, 24);
            helperLabel.TabIndex = 1;
            helperLabel.Text = "Review inventory, active rentals, and collected payments.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            titleLabel.Location = new Point(32, 32);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(165, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Reports";
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.FromArgb(243, 244, 246);
            contentPanel.Controls.Add(reportPanel);
            contentPanel.Controls.Add(statsTable);
            contentPanel.CustomizableEdges = customizableEdges19;
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.FillColor = Color.FromArgb(243, 244, 246);
            contentPanel.Location = new Point(0, 139);
            contentPanel.Margin = new Padding(3, 4, 3, 4);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(32, 37, 32, 37);
            contentPanel.ShadowDecoration.CustomizableEdges = customizableEdges20;
            contentPanel.Size = new Size(1234, 768);
            contentPanel.TabIndex = 1;
            // 
            // reportPanel
            // 
            reportPanel.BackColor = Color.White;
            reportPanel.BorderColor = Color.FromArgb(209, 213, 219);
            reportPanel.BorderRadius = 8;
            reportPanel.BorderThickness = 1;
            reportPanel.Controls.Add(reportGrid);
            reportPanel.Controls.Add(reportHeaderPanel);
            reportPanel.CustomizableEdges = customizableEdges9;
            reportPanel.Dock = DockStyle.Fill;
            reportPanel.FillColor = Color.White;
            reportPanel.Location = new Point(32, 205);
            reportPanel.Margin = new Padding(3, 4, 3, 4);
            reportPanel.Name = "reportPanel";
            reportPanel.ShadowDecoration.CustomizableEdges = customizableEdges10;
            reportPanel.Size = new Size(1170, 526);
            reportPanel.TabIndex = 1;
            // 
            // reportGrid
            // 
            reportGrid.AllowUserToAddRows = false;
            reportGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 251);
            reportGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.Font = new Font("Inter SemiBold", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            reportGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            reportGrid.ColumnHeadersHeight = 56;
            reportGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            reportGrid.AutoGenerateColumns = true;
            reportGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            reportGrid.ColumnHeadersVisible = true;
            reportGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            reportGrid.DefaultCellStyle = dataGridViewCellStyle3;
            reportGrid.Dock = DockStyle.Fill;
            reportGrid.GridColor = Color.White;
            reportGrid.Location = new Point(0, 85);
            reportGrid.Margin = new Padding(3, 4, 3, 4);
            reportGrid.Name = "reportGrid";
            reportGrid.ReadOnly = true;
            reportGrid.RowHeadersVisible = false;
            reportGrid.RowHeadersWidth = 51;
            reportGrid.RowTemplate.Height = 42;
            reportGrid.Size = new Size(1170, 441);
            reportGrid.TabIndex = 1;
            reportGrid.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            reportGrid.ThemeStyle.AlternatingRowsStyle.Font = null;
            reportGrid.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            reportGrid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            reportGrid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            reportGrid.ThemeStyle.BackColor = Color.White;
            reportGrid.ThemeStyle.GridColor = Color.White;
            reportGrid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            reportGrid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            reportGrid.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            reportGrid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            reportGrid.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            reportGrid.ThemeStyle.HeaderStyle.Height = 56;
            reportGrid.ThemeStyle.ReadOnly = true;
            reportGrid.ThemeStyle.RowsStyle.BackColor = Color.White;
            reportGrid.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.None;
            reportGrid.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            reportGrid.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            reportGrid.ThemeStyle.RowsStyle.Height = 42;
            reportGrid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            reportGrid.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // reportHeaderPanel
            // 
            reportHeaderPanel.BorderColor = Color.FromArgb(209, 213, 219);
            reportHeaderPanel.BorderRadius = 8;
            reportHeaderPanel.BorderThickness = 1;
            reportHeaderPanel.Controls.Add(searchTextBox);
            reportHeaderPanel.Controls.Add(historyLabel);
            reportHeaderPanel.CustomizableEdges = customizableEdges7;
            reportHeaderPanel.Dock = DockStyle.Top;
            reportHeaderPanel.Location = new Point(0, 0);
            reportHeaderPanel.Margin = new Padding(3, 4, 3, 4);
            reportHeaderPanel.Name = "reportHeaderPanel";
            reportHeaderPanel.ShadowDecoration.CustomizableEdges = customizableEdges8;
            reportHeaderPanel.Size = new Size(1170, 85);
            reportHeaderPanel.TabIndex = 0;
            // 
            // searchTextBox
            // 
            searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            searchTextBox.BorderColor = Color.FromArgb(209, 213, 219);
            searchTextBox.BorderRadius = 6;
            searchTextBox.Cursor = Cursors.IBeam;
            searchTextBox.CustomizableEdges = customizableEdges5;
            searchTextBox.DefaultText = "";
            searchTextBox.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            searchTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchTextBox.ForeColor = Color.FromArgb(17, 24, 39);
            searchTextBox.HoverState.BorderColor = Color.FromArgb(37, 99, 235);
            searchTextBox.Location = new Point(801, 27);
            searchTextBox.Margin = new Padding(3, 4, 3, 4);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderForeColor = Color.FromArgb(107, 114, 128);
            searchTextBox.PlaceholderText = "Search rental history";
            searchTextBox.SelectedText = "";
            searchTextBox.ShadowDecoration.CustomizableEdges = customizableEdges6;
            searchTextBox.Size = new Size(331, 28);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // historyLabel
            // 
            historyLabel.AutoSize = true;
            historyLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            historyLabel.ForeColor = Color.FromArgb(31, 41, 55);
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
            totalCarsCard.BorderColor = Color.FromArgb(209, 213, 219);
            totalCarsCard.BorderRadius = 8;
            totalCarsCard.BorderThickness = 1;
            totalCarsCard.Controls.Add(totalCarsValueLabel);
            totalCarsCard.Controls.Add(totalCarsTextLabel);
            totalCarsCard.CustomizableEdges = customizableEdges11;
            totalCarsCard.Dock = DockStyle.Fill;
            totalCarsCard.FillColor = Color.White;
            totalCarsCard.Location = new Point(0, 0);
            totalCarsCard.Margin = new Padding(0, 0, 14, 27);
            totalCarsCard.Name = "totalCarsCard";
            totalCarsCard.ShadowDecoration.CustomizableEdges = customizableEdges12;
            totalCarsCard.Size = new Size(278, 141);
            totalCarsCard.TabIndex = 0;
            // 
            // totalCarsValueLabel
            // 
            totalCarsValueLabel.AutoSize = true;
            totalCarsValueLabel.Font = new Font("Inter SemiBold", 22.2F, FontStyle.Bold);
            totalCarsValueLabel.ForeColor = Color.FromArgb(37, 99, 235);
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
            totalCarsTextLabel.ForeColor = Color.FromArgb(31, 41, 55);
            totalCarsTextLabel.Location = new Point(23, 27);
            totalCarsTextLabel.Name = "totalCarsTextLabel";
            totalCarsTextLabel.Size = new Size(92, 24);
            totalCarsTextLabel.TabIndex = 0;
            totalCarsTextLabel.Text = "Total Cars";
            // 
            // availableCarsCard
            // 
            availableCarsCard.BackColor = Color.White;
            availableCarsCard.BorderColor = Color.FromArgb(209, 213, 219);
            availableCarsCard.BorderRadius = 8;
            availableCarsCard.BorderThickness = 1;
            availableCarsCard.Controls.Add(availableCarsValueLabel);
            availableCarsCard.Controls.Add(availableCarsTextLabel);
            availableCarsCard.CustomizableEdges = customizableEdges13;
            availableCarsCard.Dock = DockStyle.Fill;
            availableCarsCard.FillColor = Color.White;
            availableCarsCard.Location = new Point(292, 0);
            availableCarsCard.Margin = new Padding(0, 0, 14, 27);
            availableCarsCard.Name = "availableCarsCard";
            availableCarsCard.ShadowDecoration.CustomizableEdges = customizableEdges14;
            availableCarsCard.Size = new Size(278, 141);
            availableCarsCard.TabIndex = 1;
            // 
            // availableCarsValueLabel
            // 
            availableCarsValueLabel.AutoSize = true;
            availableCarsValueLabel.Font = new Font("Inter SemiBold", 22.2F, FontStyle.Bold);
            availableCarsValueLabel.ForeColor = Color.FromArgb(22, 163, 74);
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
            availableCarsTextLabel.ForeColor = Color.FromArgb(31, 41, 55);
            availableCarsTextLabel.Location = new Point(23, 27);
            availableCarsTextLabel.Name = "availableCarsTextLabel";
            availableCarsTextLabel.Size = new Size(125, 24);
            availableCarsTextLabel.TabIndex = 0;
            availableCarsTextLabel.Text = "Available Cars";
            // 
            // activeRentalsCard
            // 
            activeRentalsCard.BackColor = Color.White;
            activeRentalsCard.BorderColor = Color.FromArgb(209, 213, 219);
            activeRentalsCard.BorderRadius = 8;
            activeRentalsCard.BorderThickness = 1;
            activeRentalsCard.Controls.Add(activeRentalsValueLabel);
            activeRentalsCard.Controls.Add(activeRentalsTextLabel);
            activeRentalsCard.CustomizableEdges = customizableEdges15;
            activeRentalsCard.Dock = DockStyle.Fill;
            activeRentalsCard.FillColor = Color.White;
            activeRentalsCard.Location = new Point(584, 0);
            activeRentalsCard.Margin = new Padding(0, 0, 14, 27);
            activeRentalsCard.Name = "activeRentalsCard";
            activeRentalsCard.ShadowDecoration.CustomizableEdges = customizableEdges16;
            activeRentalsCard.Size = new Size(278, 141);
            activeRentalsCard.TabIndex = 2;
            // 
            // activeRentalsValueLabel
            // 
            activeRentalsValueLabel.AutoSize = true;
            activeRentalsValueLabel.Font = new Font("Inter SemiBold", 22.2F, FontStyle.Bold);
            activeRentalsValueLabel.ForeColor = Color.FromArgb(245, 158, 11);
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
            activeRentalsTextLabel.ForeColor = Color.FromArgb(31, 41, 55);
            activeRentalsTextLabel.Location = new Point(23, 27);
            activeRentalsTextLabel.Name = "activeRentalsTextLabel";
            activeRentalsTextLabel.Size = new Size(147, 24);
            activeRentalsTextLabel.TabIndex = 0;
            activeRentalsTextLabel.Text = "Active / Overdue";
            // 
            // incomeCard
            // 
            incomeCard.BackColor = Color.White;
            incomeCard.BorderColor = Color.FromArgb(209, 213, 219);
            incomeCard.BorderRadius = 8;
            incomeCard.BorderThickness = 1;
            incomeCard.Controls.Add(incomeValueLabel);
            incomeCard.Controls.Add(incomeTextLabel);
            incomeCard.CustomizableEdges = customizableEdges17;
            incomeCard.Dock = DockStyle.Fill;
            incomeCard.FillColor = Color.White;
            incomeCard.Location = new Point(876, 0);
            incomeCard.Margin = new Padding(0, 0, 0, 27);
            incomeCard.Name = "incomeCard";
            incomeCard.ShadowDecoration.CustomizableEdges = customizableEdges18;
            incomeCard.Size = new Size(294, 141);
            incomeCard.TabIndex = 3;
            // 
            // incomeValueLabel
            // 
            incomeValueLabel.AutoSize = true;
            incomeValueLabel.Font = new Font("Inter SemiBold", 22.2F, FontStyle.Bold);
            incomeValueLabel.ForeColor = Color.FromArgb(31, 41, 55);
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
            incomeTextLabel.ForeColor = Color.FromArgb(31, 41, 55);
            incomeTextLabel.Location = new Point(23, 27);
            incomeTextLabel.Name = "incomeTextLabel";
            incomeTextLabel.Size = new Size(172, 24);
            incomeTextLabel.TabIndex = 0;
            incomeTextLabel.Text = "Collected Payments";
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

        private Guna2Panel headerPanel;
        private Guna2Button refreshButton;
        private Label helperLabel;
        private Label titleLabel;
        private Guna2Panel contentPanel;
        private Guna2Panel reportPanel;
        private Guna2DataGridView reportGrid;
        private Guna2Panel reportHeaderPanel;
        private Guna2TextBox searchTextBox;
        private Label historyLabel;
        private TableLayoutPanel statsTable;
        private Guna2Panel totalCarsCard;
        private Label totalCarsValueLabel;
        private Label totalCarsTextLabel;
        private Guna2Panel availableCarsCard;
        private Label availableCarsValueLabel;
        private Label availableCarsTextLabel;
        private Guna2Panel activeRentalsCard;
        private Label activeRentalsValueLabel;
        private Label activeRentalsTextLabel;
        private Guna2Panel incomeCard;
        private Label incomeValueLabel;
        private Label incomeTextLabel;
    }
}




