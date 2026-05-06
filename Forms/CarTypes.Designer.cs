namespace Car_Rental_System
{
    partial class CarTypes
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
            isActiveCheckBox = new CheckBox();
            clearButton = new Button();
            deleteButton = new Button();
            updateButton = new Button();
            addButton = new Button();
            descriptionTextBox = new TextBox();
            descriptionLabel = new Label();
            dailyRateTextBox = new TextBox();
            dailyRateLabel = new Label();
            typeNameTextBox = new TextBox();
            typeNameLabel = new Label();
            sectionLabel = new Label();
            listPanel = new Panel();
            carTypesGrid = new DataGridView();
            listHeaderPanel = new Panel();
            searchTextBox = new TextBox();
            recordsLabel = new Label();
            headerPanel.SuspendLayout();
            bodyTable.SuspendLayout();
            detailsPanel.SuspendLayout();
            listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)carTypesGrid).BeginInit();
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
            helperLabel.Size = new Size(471, 24);
            helperLabel.TabIndex = 1;
            helperLabel.Text = "Manage vehicle categories and adjustable daily rates.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            titleLabel.Location = new Point(32, 32);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(326, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Car Types & Rates";
            // 
            // bodyTable
            // 
            bodyTable.BackColor = Color.FromArgb(243, 244, 246);
            bodyTable.ColumnCount = 2;
            bodyTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 389F));
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
            detailsPanel.Controls.Add(isActiveCheckBox);
            detailsPanel.Controls.Add(clearButton);
            detailsPanel.Controls.Add(deleteButton);
            detailsPanel.Controls.Add(updateButton);
            detailsPanel.Controls.Add(addButton);
            detailsPanel.Controls.Add(descriptionTextBox);
            detailsPanel.Controls.Add(descriptionLabel);
            detailsPanel.Controls.Add(dailyRateTextBox);
            detailsPanel.Controls.Add(dailyRateLabel);
            detailsPanel.Controls.Add(typeNameTextBox);
            detailsPanel.Controls.Add(typeNameLabel);
            detailsPanel.Controls.Add(sectionLabel);
            detailsPanel.Dock = DockStyle.Fill;
            detailsPanel.Location = new Point(32, 37);
            detailsPanel.Margin = new Padding(0, 0, 21, 0);
            detailsPanel.Name = "detailsPanel";
            detailsPanel.Size = new Size(368, 694);
            detailsPanel.TabIndex = 0;
            // 
            // isActiveCheckBox
            // 
            isActiveCheckBox.AutoSize = true;
            isActiveCheckBox.Checked = true;
            isActiveCheckBox.CheckState = CheckState.Checked;
            isActiveCheckBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            isActiveCheckBox.ForeColor = Color.FromArgb(31, 41, 55);
            isActiveCheckBox.Location = new Point(25, 389);
            isActiveCheckBox.Margin = new Padding(3, 4, 3, 4);
            isActiveCheckBox.Name = "isActiveCheckBox";
            isActiveCheckBox.Size = new Size(145, 28);
            isActiveCheckBox.TabIndex = 8;
            isActiveCheckBox.Text = "Active for rentals";
            isActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            clearButton.BackColor = Color.White;
            clearButton.FlatAppearance.BorderColor = Color.FromArgb(31, 41, 55);
            clearButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 244, 246);
            clearButton.FlatStyle = FlatStyle.Flat;
            clearButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            clearButton.ForeColor = Color.FromArgb(31, 41, 55);
            clearButton.Location = new Point(194, 544);
            clearButton.Margin = new Padding(3, 4, 3, 4);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(151, 45);
            clearButton.TabIndex = 12;
            clearButton.Text = "Clear";
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += ClearButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.BackColor = Color.FromArgb(220, 38, 38);
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(185, 28, 28);
            deleteButton.FlatStyle = FlatStyle.Flat;
            deleteButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            deleteButton.ForeColor = Color.White;
            deleteButton.Location = new Point(25, 544);
            deleteButton.Margin = new Padding(3, 4, 3, 4);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(151, 45);
            deleteButton.TabIndex = 11;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = false;
            deleteButton.Click += DeleteButton_Click;
            // 
            // updateButton
            // 
            updateButton.BackColor = Color.FromArgb(31, 41, 55);
            updateButton.FlatAppearance.BorderSize = 0;
            updateButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 41, 55);
            updateButton.FlatStyle = FlatStyle.Flat;
            updateButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            updateButton.ForeColor = Color.White;
            updateButton.Location = new Point(194, 483);
            updateButton.Margin = new Padding(3, 4, 3, 4);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(151, 45);
            updateButton.TabIndex = 10;
            updateButton.Text = "Update";
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
            addButton.Location = new Point(25, 483);
            addButton.Margin = new Padding(3, 4, 3, 4);
            addButton.Name = "addButton";
            addButton.Size = new Size(151, 45);
            addButton.TabIndex = 9;
            addButton.Text = "Add Type";
            addButton.UseVisualStyleBackColor = false;
            addButton.Click += AddButton_Click;
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.BorderStyle = BorderStyle.FixedSingle;
            descriptionTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            descriptionTextBox.Location = new Point(25, 259);
            descriptionTextBox.Margin = new Padding(3, 4, 3, 4);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.PlaceholderText = "Optional notes about this car type";
            descriptionTextBox.Size = new Size(320, 109);
            descriptionTextBox.TabIndex = 6;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            descriptionLabel.ForeColor = Color.FromArgb(31, 41, 55);
            descriptionLabel.Location = new Point(25, 232);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(93, 22);
            descriptionLabel.TabIndex = 5;
            descriptionLabel.Text = "Description";
            // 
            // dailyRateTextBox
            // 
            dailyRateTextBox.BorderStyle = BorderStyle.FixedSingle;
            dailyRateTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dailyRateTextBox.Location = new Point(25, 184);
            dailyRateTextBox.Margin = new Padding(3, 4, 3, 4);
            dailyRateTextBox.Name = "dailyRateTextBox";
            dailyRateTextBox.PlaceholderText = "2500";
            dailyRateTextBox.Size = new Size(320, 28);
            dailyRateTextBox.TabIndex = 4;
            // 
            // dailyRateLabel
            // 
            dailyRateLabel.AutoSize = true;
            dailyRateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dailyRateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            dailyRateLabel.Location = new Point(25, 157);
            dailyRateLabel.Name = "dailyRateLabel";
            dailyRateLabel.Size = new Size(81, 22);
            dailyRateLabel.TabIndex = 3;
            dailyRateLabel.Text = "Daily Rate";
            // 
            // typeNameTextBox
            // 
            typeNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            typeNameTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            typeNameTextBox.Location = new Point(25, 109);
            typeNameTextBox.Margin = new Padding(3, 4, 3, 4);
            typeNameTextBox.Name = "typeNameTextBox";
            typeNameTextBox.PlaceholderText = "Economy";
            typeNameTextBox.Size = new Size(320, 28);
            typeNameTextBox.TabIndex = 2;
            // 
            // typeNameLabel
            // 
            typeNameLabel.AutoSize = true;
            typeNameLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            typeNameLabel.ForeColor = Color.FromArgb(31, 41, 55);
            typeNameLabel.Location = new Point(25, 83);
            typeNameLabel.Name = "typeNameLabel";
            typeNameLabel.Size = new Size(89, 22);
            typeNameLabel.TabIndex = 1;
            typeNameLabel.Text = "Type Name";
            // 
            // sectionLabel
            // 
            sectionLabel.AutoSize = true;
            sectionLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            sectionLabel.ForeColor = Color.FromArgb(31, 41, 55);
            sectionLabel.Location = new Point(23, 24);
            sectionLabel.Name = "sectionLabel";
            sectionLabel.Size = new Size(143, 32);
            sectionLabel.TabIndex = 0;
            sectionLabel.Text = "Rate Details";
            // 
            // listPanel
            // 
            listPanel.BackColor = Color.White;
            listPanel.Controls.Add(carTypesGrid);
            listPanel.Controls.Add(listHeaderPanel);
            listPanel.Dock = DockStyle.Fill;
            listPanel.Location = new Point(421, 37);
            listPanel.Margin = new Padding(0);
            listPanel.Name = "listPanel";
            listPanel.Size = new Size(781, 694);
            listPanel.TabIndex = 1;
            // 
            // carTypesGrid
            // 
            carTypesGrid.AllowUserToAddRows = false;
            carTypesGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 251);
            carTypesGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            carTypesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            carTypesGrid.BackgroundColor = Color.White;
            carTypesGrid.BorderStyle = BorderStyle.FixedSingle;
            carTypesGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            carTypesGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            carTypesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            carTypesGrid.ColumnHeadersHeight = 36;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            carTypesGrid.DefaultCellStyle = dataGridViewCellStyle3;
            carTypesGrid.Dock = DockStyle.Fill;
            carTypesGrid.EnableHeadersVisualStyles = false;
            carTypesGrid.GridColor = Color.FromArgb(229, 231, 235);
            carTypesGrid.Location = new Point(0, 77);
            carTypesGrid.Margin = new Padding(3, 4, 3, 4);
            carTypesGrid.Name = "carTypesGrid";
            carTypesGrid.ReadOnly = true;
            carTypesGrid.RowHeadersVisible = false;
            carTypesGrid.RowHeadersWidth = 51;
            carTypesGrid.RowTemplate.Height = 42;
            carTypesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            carTypesGrid.Size = new Size(781, 617);
            carTypesGrid.TabIndex = 1;
            carTypesGrid.SelectionChanged += CarTypesGrid_SelectionChanged;
            // 
            // listHeaderPanel
            // 
            listHeaderPanel.BorderStyle = BorderStyle.FixedSingle;
            listHeaderPanel.Controls.Add(searchTextBox);
            listHeaderPanel.Controls.Add(recordsLabel);
            listHeaderPanel.Dock = DockStyle.Top;
            listHeaderPanel.Location = new Point(0, 0);
            listHeaderPanel.Margin = new Padding(3, 4, 3, 4);
            listHeaderPanel.Name = "listHeaderPanel";
            listHeaderPanel.Size = new Size(781, 77);
            listHeaderPanel.TabIndex = 0;
            // 
            // searchTextBox
            // 
            searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            searchTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchTextBox.Location = new Point(480, 24);
            searchTextBox.Margin = new Padding(3, 4, 3, 4);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Search type or description";
            searchTextBox.Size = new Size(274, 28);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // recordsLabel
            // 
            recordsLabel.AutoSize = true;
            recordsLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recordsLabel.ForeColor = Color.FromArgb(31, 41, 55);
            recordsLabel.Location = new Point(0, 21);
            recordsLabel.Name = "recordsLabel";
            recordsLabel.Size = new Size(169, 32);
            recordsLabel.TabIndex = 0;
            recordsLabel.Text = "Rate Records";
            // 
            // CarTypes
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
            Name = "CarTypes";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Avenue - Car Types";
            Load += CarTypes_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            bodyTable.ResumeLayout(false);
            detailsPanel.ResumeLayout(false);
            detailsPanel.PerformLayout();
            listPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)carTypesGrid).EndInit();
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
        private CheckBox isActiveCheckBox;
        private Button clearButton;
        private Button deleteButton;
        private Button updateButton;
        private Button addButton;
        private TextBox descriptionTextBox;
        private Label descriptionLabel;
        private TextBox dailyRateTextBox;
        private Label dailyRateLabel;
        private TextBox typeNameTextBox;
        private Label typeNameLabel;
        private Label sectionLabel;
        private Panel listPanel;
        private DataGridView carTypesGrid;
        private Panel listHeaderPanel;
        private TextBox searchTextBox;
        private Label recordsLabel;
    }
}
