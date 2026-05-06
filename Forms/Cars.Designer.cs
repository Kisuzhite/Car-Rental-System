namespace Car_Rental_System
{
    partial class Cars
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
            browseImageButton = new Button();
            imagePathTextBox = new TextBox();
            imagePathLabel = new Label();
            carImagePictureBox = new PictureBox();
            statusComboBox = new ComboBox();
            statusLabel = new Label();
            dailyRateTextBox = new TextBox();
            dailyRateLabel = new Label();
            colorTextBox = new TextBox();
            colorLabel = new Label();
            yearTextBox = new TextBox();
            yearLabel = new Label();
            modelTextBox = new TextBox();
            modelLabel = new Label();
            brandTextBox = new TextBox();
            brandLabel = new Label();
            plateTextBox = new TextBox();
            plateLabel = new Label();
            sectionLabel = new Label();
            listPanel = new Panel();
            carsGrid = new DataGridView();
            listHeaderPanel = new Panel();
            searchTextBox = new TextBox();
            recordsLabel = new Label();
            headerPanel.SuspendLayout();
            bodyTable.SuspendLayout();
            detailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)carImagePictureBox).BeginInit();
            listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)carsGrid).BeginInit();
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
            headerPanel.Padding = new Padding(32, 24, 32, 0);
            headerPanel.Size = new Size(1353, 115);
            headerPanel.TabIndex = 0;
            // 
            // helperLabel
            // 
            helperLabel.AutoSize = true;
            helperLabel.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            helperLabel.ForeColor = Color.FromArgb(31, 41, 55);
            helperLabel.Location = new Point(35, 75);
            helperLabel.Name = "helperLabel";
            helperLabel.Size = new Size(423, 24);
            helperLabel.TabIndex = 1;
            helperLabel.Text = "Manage car records, availability, rates, and photos.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            titleLabel.Location = new Point(32, 21);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(289, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Cars Inventory";
            // 
            // bodyTable
            // 
            bodyTable.BackColor = Color.FromArgb(243, 244, 246);
            bodyTable.ColumnCount = 2;
            bodyTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 434F));
            bodyTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bodyTable.Controls.Add(detailsPanel, 0, 0);
            bodyTable.Controls.Add(listPanel, 1, 0);
            bodyTable.Dock = DockStyle.Fill;
            bodyTable.Location = new Point(0, 115);
            bodyTable.Margin = new Padding(3, 4, 3, 4);
            bodyTable.Name = "bodyTable";
            bodyTable.Padding = new Padding(32, 37, 32, 37);
            bodyTable.RowCount = 1;
            bodyTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bodyTable.Size = new Size(1353, 820);
            bodyTable.TabIndex = 1;
            // 
            // detailsPanel
            // 
            detailsPanel.BackColor = Color.White;
            detailsPanel.BorderStyle = BorderStyle.FixedSingle;
            detailsPanel.Controls.Add(clearButton);
            detailsPanel.Controls.Add(deleteButton);
            detailsPanel.Controls.Add(updateButton);
            detailsPanel.Controls.Add(addButton);
            detailsPanel.Controls.Add(browseImageButton);
            detailsPanel.Controls.Add(imagePathTextBox);
            detailsPanel.Controls.Add(imagePathLabel);
            detailsPanel.Controls.Add(carImagePictureBox);
            detailsPanel.Controls.Add(statusComboBox);
            detailsPanel.Controls.Add(statusLabel);
            detailsPanel.Controls.Add(dailyRateTextBox);
            detailsPanel.Controls.Add(dailyRateLabel);
            detailsPanel.Controls.Add(colorTextBox);
            detailsPanel.Controls.Add(colorLabel);
            detailsPanel.Controls.Add(yearTextBox);
            detailsPanel.Controls.Add(yearLabel);
            detailsPanel.Controls.Add(modelTextBox);
            detailsPanel.Controls.Add(modelLabel);
            detailsPanel.Controls.Add(brandTextBox);
            detailsPanel.Controls.Add(brandLabel);
            detailsPanel.Controls.Add(plateTextBox);
            detailsPanel.Controls.Add(plateLabel);
            detailsPanel.Controls.Add(sectionLabel);
            detailsPanel.Dock = DockStyle.Fill;
            detailsPanel.Location = new Point(32, 37);
            detailsPanel.Margin = new Padding(0, 0, 23, 0);
            detailsPanel.Name = "detailsPanel";
            detailsPanel.Size = new Size(411, 746);
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
            clearButton.Location = new Point(213, 672);
            clearButton.Margin = new Padding(3, 4, 3, 4);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(158, 48);
            clearButton.TabIndex = 22;
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
            deleteButton.Location = new Point(27, 672);
            deleteButton.Margin = new Padding(3, 4, 3, 4);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(158, 48);
            deleteButton.TabIndex = 21;
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
            updateButton.Location = new Point(213, 608);
            updateButton.Margin = new Padding(3, 4, 3, 4);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(158, 48);
            updateButton.TabIndex = 20;
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
            addButton.Location = new Point(27, 608);
            addButton.Margin = new Padding(3, 4, 3, 4);
            addButton.Name = "addButton";
            addButton.Size = new Size(158, 48);
            addButton.TabIndex = 19;
            addButton.Text = "Add Car";
            addButton.UseVisualStyleBackColor = false;
            addButton.Click += AddButton_Click;
            // 
            // browseImageButton
            // 
            browseImageButton.BackColor = Color.FromArgb(37, 99, 235);
            browseImageButton.FlatAppearance.BorderSize = 0;
            browseImageButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            browseImageButton.FlatStyle = FlatStyle.Flat;
            browseImageButton.Font = new Font("Inter Medium", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            browseImageButton.ForeColor = Color.White;
            browseImageButton.Location = new Point(27, 539);
            browseImageButton.Margin = new Padding(3, 4, 3, 4);
            browseImageButton.Name = "browseImageButton";
            browseImageButton.Size = new Size(343, 45);
            browseImageButton.TabIndex = 18;
            browseImageButton.Text = "Browse Photo";
            browseImageButton.UseVisualStyleBackColor = false;
            browseImageButton.Click += BrowseImageButton_Click;
            // 
            // imagePathTextBox
            // 
            imagePathTextBox.BorderStyle = BorderStyle.FixedSingle;
            imagePathTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            imagePathTextBox.Location = new Point(27, 499);
            imagePathTextBox.Margin = new Padding(3, 4, 3, 4);
            imagePathTextBox.Name = "imagePathTextBox";
            imagePathTextBox.ReadOnly = true;
            imagePathTextBox.Size = new Size(343, 28);
            imagePathTextBox.TabIndex = 17;
            // 
            // imagePathLabel
            // 
            imagePathLabel.AutoSize = true;
            imagePathLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            imagePathLabel.ForeColor = Color.FromArgb(31, 41, 55);
            imagePathLabel.Location = new Point(27, 472);
            imagePathLabel.Name = "imagePathLabel";
            imagePathLabel.Size = new Size(88, 22);
            imagePathLabel.TabIndex = 16;
            imagePathLabel.Text = "Image path";
            // 
            // carImagePictureBox
            // 
            carImagePictureBox.BackColor = Color.White;
            carImagePictureBox.BorderStyle = BorderStyle.FixedSingle;
            carImagePictureBox.Location = new Point(27, 312);
            carImagePictureBox.Margin = new Padding(3, 4, 3, 4);
            carImagePictureBox.Name = "carImagePictureBox";
            carImagePictureBox.Size = new Size(343, 143);
            carImagePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            carImagePictureBox.TabIndex = 15;
            carImagePictureBox.TabStop = false;
            // 
            // statusComboBox
            // 
            statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statusComboBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            statusComboBox.FormattingEnabled = true;
            statusComboBox.Items.AddRange(new object[] { "Available", "Rented", "Maintenance" });
            statusComboBox.Location = new Point(213, 259);
            statusComboBox.Margin = new Padding(3, 4, 3, 4);
            statusComboBox.Name = "statusComboBox";
            statusComboBox.Size = new Size(157, 32);
            statusComboBox.TabIndex = 14;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            statusLabel.ForeColor = Color.FromArgb(31, 41, 55);
            statusLabel.Location = new Point(213, 232);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(55, 22);
            statusLabel.TabIndex = 13;
            statusLabel.Text = "Status";
            // 
            // dailyRateTextBox
            // 
            dailyRateTextBox.BorderStyle = BorderStyle.FixedSingle;
            dailyRateTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dailyRateTextBox.Location = new Point(27, 259);
            dailyRateTextBox.Margin = new Padding(3, 4, 3, 4);
            dailyRateTextBox.Name = "dailyRateTextBox";
            dailyRateTextBox.PlaceholderText = "2500";
            dailyRateTextBox.Size = new Size(157, 28);
            dailyRateTextBox.TabIndex = 12;
            // 
            // dailyRateLabel
            // 
            dailyRateLabel.AutoSize = true;
            dailyRateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dailyRateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            dailyRateLabel.Location = new Point(27, 232);
            dailyRateLabel.Name = "dailyRateLabel";
            dailyRateLabel.Size = new Size(77, 22);
            dailyRateLabel.TabIndex = 11;
            dailyRateLabel.Text = "Daily rate";
            // 
            // colorTextBox
            // 
            colorTextBox.BorderStyle = BorderStyle.FixedSingle;
            colorTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            colorTextBox.Location = new Point(213, 184);
            colorTextBox.Margin = new Padding(3, 4, 3, 4);
            colorTextBox.Name = "colorTextBox";
            colorTextBox.PlaceholderText = "White";
            colorTextBox.Size = new Size(157, 28);
            colorTextBox.TabIndex = 10;
            // 
            // colorLabel
            // 
            colorLabel.AutoSize = true;
            colorLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            colorLabel.ForeColor = Color.FromArgb(31, 41, 55);
            colorLabel.Location = new Point(213, 157);
            colorLabel.Name = "colorLabel";
            colorLabel.Size = new Size(49, 22);
            colorLabel.TabIndex = 9;
            colorLabel.Text = "Color";
            // 
            // yearTextBox
            // 
            yearTextBox.BorderStyle = BorderStyle.FixedSingle;
            yearTextBox.Font = new Font("Segoe UI", 9.5F);
            yearTextBox.Location = new Point(27, 184);
            yearTextBox.Margin = new Padding(3, 4, 3, 4);
            yearTextBox.Name = "yearTextBox";
            yearTextBox.PlaceholderText = "2024";
            yearTextBox.Size = new Size(157, 29);
            yearTextBox.TabIndex = 8;
            // 
            // yearLabel
            // 
            yearLabel.AutoSize = true;
            yearLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            yearLabel.ForeColor = Color.FromArgb(31, 41, 55);
            yearLabel.Location = new Point(27, 157);
            yearLabel.Name = "yearLabel";
            yearLabel.Size = new Size(42, 22);
            yearLabel.TabIndex = 7;
            yearLabel.Text = "Year";
            // 
            // modelTextBox
            // 
            modelTextBox.BorderStyle = BorderStyle.FixedSingle;
            modelTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modelTextBox.Location = new Point(213, 109);
            modelTextBox.Margin = new Padding(3, 4, 3, 4);
            modelTextBox.Name = "modelTextBox";
            modelTextBox.PlaceholderText = "Vios";
            modelTextBox.Size = new Size(157, 28);
            modelTextBox.TabIndex = 6;
            // 
            // modelLabel
            // 
            modelLabel.AutoSize = true;
            modelLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modelLabel.ForeColor = Color.FromArgb(31, 41, 55);
            modelLabel.Location = new Point(213, 83);
            modelLabel.Name = "modelLabel";
            modelLabel.Size = new Size(55, 22);
            modelLabel.TabIndex = 5;
            modelLabel.Text = "Model";
            // 
            // brandTextBox
            // 
            brandTextBox.BorderStyle = BorderStyle.FixedSingle;
            brandTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            brandTextBox.Location = new Point(27, 109);
            brandTextBox.Margin = new Padding(3, 4, 3, 4);
            brandTextBox.Name = "brandTextBox";
            brandTextBox.PlaceholderText = "Toyota";
            brandTextBox.Size = new Size(157, 28);
            brandTextBox.TabIndex = 4;
            // 
            // brandLabel
            // 
            brandLabel.AutoSize = true;
            brandLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            brandLabel.ForeColor = Color.FromArgb(31, 41, 55);
            brandLabel.Location = new Point(27, 83);
            brandLabel.Name = "brandLabel";
            brandLabel.Size = new Size(52, 22);
            brandLabel.TabIndex = 3;
            brandLabel.Text = "Brand";
            // 
            // plateTextBox
            // 
            plateTextBox.BorderStyle = BorderStyle.FixedSingle;
            plateTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            plateTextBox.Location = new Point(213, 35);
            plateTextBox.Margin = new Padding(3, 4, 3, 4);
            plateTextBox.Name = "plateTextBox";
            plateTextBox.PlaceholderText = "Plate number";
            plateTextBox.Size = new Size(157, 28);
            plateTextBox.TabIndex = 2;
            // 
            // plateLabel
            // 
            plateLabel.AutoSize = true;
            plateLabel.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            plateLabel.ForeColor = Color.FromArgb(31, 41, 55);
            plateLabel.Location = new Point(213, 8);
            plateLabel.Name = "plateLabel";
            plateLabel.Size = new Size(105, 22);
            plateLabel.TabIndex = 1;
            plateLabel.Text = "Plate number";
            // 
            // sectionLabel
            // 
            sectionLabel.AutoSize = true;
            sectionLabel.Font = new Font("Inter SemiBold", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            sectionLabel.ForeColor = Color.FromArgb(31, 41, 55);
            sectionLabel.Location = new Point(27, 29);
            sectionLabel.Name = "sectionLabel";
            sectionLabel.Size = new Size(136, 32);
            sectionLabel.TabIndex = 0;
            sectionLabel.Text = "Car Details";
            // 
            // listPanel
            // 
            listPanel.BackColor = Color.White;
            listPanel.Controls.Add(carsGrid);
            listPanel.Controls.Add(listHeaderPanel);
            listPanel.Dock = DockStyle.Fill;
            listPanel.Location = new Point(466, 37);
            listPanel.Margin = new Padding(0);
            listPanel.Name = "listPanel";
            listPanel.Size = new Size(855, 746);
            listPanel.TabIndex = 1;
            // 
            // carsGrid
            // 
            carsGrid.AllowUserToAddRows = false;
            carsGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 251);
            carsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            carsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            carsGrid.BackgroundColor = Color.White;
            carsGrid.BorderStyle = BorderStyle.FixedSingle;
            carsGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            carsGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.Font = new Font("Inter SemiBold", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            carsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            carsGrid.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            carsGrid.DefaultCellStyle = dataGridViewCellStyle3;
            carsGrid.Dock = DockStyle.Fill;
            carsGrid.EnableHeadersVisualStyles = false;
            carsGrid.GridColor = Color.FromArgb(229, 231, 235);
            carsGrid.Location = new Point(0, 85);
            carsGrid.Margin = new Padding(3, 4, 3, 4);
            carsGrid.Name = "carsGrid";
            carsGrid.ReadOnly = true;
            carsGrid.RowHeadersVisible = false;
            carsGrid.RowHeadersWidth = 51;
            carsGrid.RowTemplate.Height = 42;
            carsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            carsGrid.Size = new Size(855, 661);
            carsGrid.TabIndex = 1;
            carsGrid.SelectionChanged += CarsGrid_SelectionChanged;
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
            listHeaderPanel.Padding = new Padding(23, 21, 23, 13);
            listHeaderPanel.Size = new Size(855, 85);
            listHeaderPanel.TabIndex = 0;
            // 
            // searchTextBox
            // 
            searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            searchTextBox.Font = new Font("Inter", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchTextBox.Location = new Point(519, 27);
            searchTextBox.Margin = new Padding(3, 4, 3, 4);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Search plate, brand, or model";
            searchTextBox.Size = new Size(308, 28);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // recordsLabel
            // 
            recordsLabel.AutoSize = true;
            recordsLabel.Font = new Font("Inter SemiBold", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recordsLabel.ForeColor = Color.FromArgb(31, 41, 55);
            recordsLabel.Location = new Point(23, 24);
            recordsLabel.Name = "recordsLabel";
            recordsLabel.Size = new Size(151, 32);
            recordsLabel.TabIndex = 0;
            recordsLabel.Text = "Car Records";
            // 
            // Cars
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1353, 935);
            Controls.Add(bodyTable);
            Controls.Add(headerPanel);
            Font = new Font("Segoe UI", 9F);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1255, 918);
            Name = "Cars";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Avenue - Cars";
            Load += Cars_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            bodyTable.ResumeLayout(false);
            detailsPanel.ResumeLayout(false);
            detailsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)carImagePictureBox).EndInit();
            listPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)carsGrid).EndInit();
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
        private Button browseImageButton;
        private TextBox imagePathTextBox;
        private Label imagePathLabel;
        private PictureBox carImagePictureBox;
        private ComboBox statusComboBox;
        private Label statusLabel;
        private TextBox dailyRateTextBox;
        private Label dailyRateLabel;
        private TextBox colorTextBox;
        private Label colorLabel;
        private TextBox yearTextBox;
        private Label yearLabel;
        private TextBox modelTextBox;
        private Label modelLabel;
        private TextBox brandTextBox;
        private Label brandLabel;
        private TextBox plateTextBox;
        private Label plateLabel;
        private Label sectionLabel;
        private Panel listPanel;
        private DataGridView carsGrid;
        private Panel listHeaderPanel;
        private TextBox searchTextBox;
        private Label recordsLabel;
    }
}
