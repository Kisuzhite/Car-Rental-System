namespace Car_Rental_System
{
    partial class Customers
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
            deleteButton = new Button();
            updateButton = new Button();
            addButton = new Button();
            addressTextBox = new TextBox();
            addressLabel = new Label();
            licenseTextBox = new TextBox();
            licenseLabel = new Label();
            contactTextBox = new TextBox();
            contactLabel = new Label();
            nameTextBox = new TextBox();
            nameLabel = new Label();
            sectionLabel = new Label();
            listPanel = new Panel();
            customersGrid = new DataGridView();
            listHeaderPanel = new Panel();
            searchTextBox = new TextBox();
            recordsLabel = new Label();
            headerPanel.SuspendLayout();
            bodyTable.SuspendLayout();
            detailsPanel.SuspendLayout();
            listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)customersGrid).BeginInit();
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
            helperLabel.ForeColor = Color.FromArgb(52, 58, 64);
            helperLabel.Location = new Point(35, 85);
            helperLabel.Name = "helperLabel";
            helperLabel.Size = new Size(450, 24);
            helperLabel.TabIndex = 1;
            helperLabel.Text = "Keep customer contact and license records organized.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(52, 58, 64);
            titleLabel.Location = new Point(32, 32);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(448, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Customer Management";
            // 
            // bodyTable
            // 
            bodyTable.BackColor = Color.White;
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
            detailsPanel.BorderStyle = BorderStyle.Fixed3D;
            detailsPanel.Controls.Add(clearButton);
            detailsPanel.Controls.Add(deleteButton);
            detailsPanel.Controls.Add(updateButton);
            detailsPanel.Controls.Add(addButton);
            detailsPanel.Controls.Add(addressTextBox);
            detailsPanel.Controls.Add(addressLabel);
            detailsPanel.Controls.Add(licenseTextBox);
            detailsPanel.Controls.Add(licenseLabel);
            detailsPanel.Controls.Add(contactTextBox);
            detailsPanel.Controls.Add(contactLabel);
            detailsPanel.Controls.Add(nameTextBox);
            detailsPanel.Controls.Add(nameLabel);
            detailsPanel.Controls.Add(sectionLabel);
            detailsPanel.Dock = DockStyle.Fill;
            detailsPanel.Location = new Point(32, 37);
            detailsPanel.Margin = new Padding(0, 0, 21, 0);
            detailsPanel.Name = "detailsPanel";
            detailsPanel.Size = new Size(368, 694);
            detailsPanel.TabIndex = 0;
            // 
            // clearButton
            // 
            clearButton.BackColor = Color.White;
            clearButton.FlatAppearance.BorderColor = Color.FromArgb(52, 58, 64);
            clearButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 204, 0);
            clearButton.FlatStyle = FlatStyle.Flat;
            clearButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            clearButton.ForeColor = Color.FromArgb(52, 58, 64);
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
            deleteButton.BackColor = Color.FromArgb(255, 204, 0);
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 204, 0);
            deleteButton.FlatStyle = FlatStyle.Flat;
            deleteButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            deleteButton.ForeColor = Color.FromArgb(52, 58, 64);
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
            updateButton.BackColor = Color.FromArgb(52, 58, 64);
            updateButton.FlatAppearance.BorderSize = 0;
            updateButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(52, 58, 64);
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
            addButton.BackColor = Color.FromArgb(0, 123, 255);
            addButton.FlatAppearance.BorderSize = 0;
            addButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 123, 255);
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addButton.ForeColor = Color.White;
            addButton.Location = new Point(25, 483);
            addButton.Margin = new Padding(3, 4, 3, 4);
            addButton.Name = "addButton";
            addButton.Size = new Size(151, 45);
            addButton.TabIndex = 9;
            addButton.Text = "Add Customer";
            addButton.UseVisualStyleBackColor = false;
            addButton.Click += AddButton_Click;
            // 
            // addressTextBox
            // 
            addressTextBox.BorderStyle = BorderStyle.FixedSingle;
            addressTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            addressTextBox.Location = new Point(25, 333);
            addressTextBox.Margin = new Padding(3, 4, 3, 4);
            addressTextBox.Multiline = true;
            addressTextBox.Name = "addressTextBox";
            addressTextBox.PlaceholderText = "Street, City, Province";
            addressTextBox.Size = new Size(320, 109);
            addressTextBox.TabIndex = 8;
            // 
            // addressLabel
            // 
            addressLabel.AutoSize = true;
            addressLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            addressLabel.ForeColor = Color.FromArgb(52, 58, 64);
            addressLabel.Location = new Point(25, 307);
            addressLabel.Name = "addressLabel";
            addressLabel.Size = new Size(69, 22);
            addressLabel.TabIndex = 7;
            addressLabel.Text = "Address";
            // 
            // licenseTextBox
            // 
            licenseTextBox.BorderStyle = BorderStyle.FixedSingle;
            licenseTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            licenseTextBox.Location = new Point(25, 259);
            licenseTextBox.Margin = new Padding(3, 4, 3, 4);
            licenseTextBox.Name = "licenseTextBox";
            licenseTextBox.PlaceholderText = "N01-23-456789";
            licenseTextBox.Size = new Size(320, 28);
            licenseTextBox.TabIndex = 6;
            // 
            // licenseLabel
            // 
            licenseLabel.AutoSize = true;
            licenseLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            licenseLabel.ForeColor = Color.FromArgb(52, 58, 64);
            licenseLabel.Location = new Point(25, 232);
            licenseLabel.Name = "licenseLabel";
            licenseLabel.Size = new Size(127, 22);
            licenseLabel.TabIndex = 5;
            licenseLabel.Text = "License Number";
            // 
            // contactTextBox
            // 
            contactTextBox.BorderStyle = BorderStyle.FixedSingle;
            contactTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            contactTextBox.Location = new Point(25, 184);
            contactTextBox.Margin = new Padding(3, 4, 3, 4);
            contactTextBox.Name = "contactTextBox";
            contactTextBox.PlaceholderText = "09XX XXX XXXX";
            contactTextBox.Size = new Size(320, 28);
            contactTextBox.TabIndex = 4;
            // 
            // contactLabel
            // 
            contactLabel.AutoSize = true;
            contactLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            contactLabel.ForeColor = Color.FromArgb(52, 58, 64);
            contactLabel.Location = new Point(25, 157);
            contactLabel.Name = "contactLabel";
            contactLabel.Size = new Size(127, 22);
            contactLabel.TabIndex = 3;
            contactLabel.Text = "Contact Number";
            // 
            // nameTextBox
            // 
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            nameTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nameTextBox.Location = new Point(25, 109);
            nameTextBox.Margin = new Padding(3, 4, 3, 4);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.PlaceholderText = "Customer full name";
            nameTextBox.Size = new Size(320, 28);
            nameTextBox.TabIndex = 2;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nameLabel.ForeColor = Color.FromArgb(52, 58, 64);
            nameLabel.Location = new Point(25, 83);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(80, 22);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Full Name";
            // 
            // sectionLabel
            // 
            sectionLabel.AutoSize = true;
            sectionLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            sectionLabel.ForeColor = Color.FromArgb(52, 58, 64);
            sectionLabel.Location = new Point(23, 24);
            sectionLabel.Name = "sectionLabel";
            sectionLabel.Size = new Size(207, 32);
            sectionLabel.TabIndex = 0;
            sectionLabel.Text = "Customer Details";
            // 
            // listPanel
            // 
            listPanel.BackColor = Color.White;
            listPanel.Controls.Add(customersGrid);
            listPanel.Controls.Add(listHeaderPanel);
            listPanel.Dock = DockStyle.Fill;
            listPanel.Location = new Point(421, 37);
            listPanel.Margin = new Padding(0);
            listPanel.Name = "listPanel";
            listPanel.Size = new Size(781, 694);
            listPanel.TabIndex = 1;
            // 
            // customersGrid
            // 
            customersGrid.AllowUserToAddRows = false;
            customersGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            customersGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            customersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            customersGrid.BackgroundColor = Color.White;
            customersGrid.BorderStyle = BorderStyle.Fixed3D;
            customersGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            customersGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            customersGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            customersGrid.ColumnHeadersHeight = 36;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            customersGrid.DefaultCellStyle = dataGridViewCellStyle3;
            customersGrid.Dock = DockStyle.Fill;
            customersGrid.EnableHeadersVisualStyles = false;
            customersGrid.GridColor = Color.FromArgb(52, 58, 64);
            customersGrid.Location = new Point(0, 77);
            customersGrid.Margin = new Padding(3, 4, 3, 4);
            customersGrid.Name = "customersGrid";
            customersGrid.ReadOnly = true;
            customersGrid.RowHeadersVisible = false;
            customersGrid.RowHeadersWidth = 51;
            customersGrid.RowTemplate.Height = 38;
            customersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customersGrid.Size = new Size(781, 617);
            customersGrid.TabIndex = 1;
            customersGrid.SelectionChanged += CustomersGrid_SelectionChanged;
            // 
            // listHeaderPanel
            // 
            listHeaderPanel.BorderStyle = BorderStyle.Fixed3D;
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
            searchTextBox.PlaceholderText = "Search name or license";
            searchTextBox.Size = new Size(274, 28);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // recordsLabel
            // 
            recordsLabel.AutoSize = true;
            recordsLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recordsLabel.ForeColor = Color.FromArgb(52, 58, 64);
            recordsLabel.Location = new Point(0, 21);
            recordsLabel.Name = "recordsLabel";
            recordsLabel.Size = new Size(222, 32);
            recordsLabel.TabIndex = 0;
            recordsLabel.Text = "Customer Records";
            // 
            // Customers
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
            Name = "Customers";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Avenue - Customers";
            Load += Customers_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            bodyTable.ResumeLayout(false);
            detailsPanel.ResumeLayout(false);
            detailsPanel.PerformLayout();
            listPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)customersGrid).EndInit();
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
        private Button deleteButton;
        private Button updateButton;
        private Button addButton;
        private TextBox addressTextBox;
        private Label addressLabel;
        private TextBox licenseTextBox;
        private Label licenseLabel;
        private TextBox contactTextBox;
        private Label contactLabel;
        private TextBox nameTextBox;
        private Label nameLabel;
        private Label sectionLabel;
        private Panel listPanel;
        private DataGridView customersGrid;
        private Panel listHeaderPanel;
        private TextBox searchTextBox;
        private Label recordsLabel;
    }
}
