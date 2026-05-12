namespace Car_Rental_System
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            brandPanel = new Panel();
            brandSubtitleLabel = new Label();
            brandLogoPictureBox = new PictureBox();
            mainPanel = new Panel();
            exitButton = new Button();
            loginButton = new Button();
            statusLabel = new Label();
            passwordTextBox = new TextBox();
            passwordLabel = new Label();
            userNameTextBox = new TextBox();
            userNameLabel = new Label();
            subtitleLabel = new Label();
            titleLabel = new Label();
            brandPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)brandLogoPictureBox).BeginInit();
            mainPanel.SuspendLayout();
            SuspendLayout();
            // 
            // brandPanel
            // 
            brandPanel.BackColor = Color.FromArgb(31, 41, 55);
            brandPanel.Controls.Add(brandSubtitleLabel);
            brandPanel.Controls.Add(brandLogoPictureBox);
            brandPanel.Dock = DockStyle.Left;
            brandPanel.Location = new Point(0, 0);
            brandPanel.Name = "brandPanel";
            brandPanel.Size = new Size(260, 488);
            brandPanel.TabIndex = 1;
            // 
            // brandSubtitleLabel
            // 
            brandSubtitleLabel.Font = new Font("Inter", 10F);
            brandSubtitleLabel.ForeColor = Color.FromArgb(209, 213, 219);
            brandSubtitleLabel.Location = new Point(31, 236);
            brandSubtitleLabel.Name = "brandSubtitleLabel";
            brandSubtitleLabel.Size = new Size(199, 93);
            brandSubtitleLabel.TabIndex = 2;
            brandSubtitleLabel.Text = "Manage rentals, returns, rates, customers, and reports in one place.";
            // 
            // brandLogoPictureBox
            // 
            brandLogoPictureBox.Image = (Image)resources.GetObject("brandLogoPictureBox.Image");
            brandLogoPictureBox.Location = new Point(30, 38);
            brandLogoPictureBox.Name = "brandLogoPictureBox";
            brandLogoPictureBox.Size = new Size(200, 125);
            brandLogoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            brandLogoPictureBox.TabIndex = 4;
            brandLogoPictureBox.TabStop = false;
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.White;
            mainPanel.Controls.Add(exitButton);
            mainPanel.Controls.Add(loginButton);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(passwordTextBox);
            mainPanel.Controls.Add(passwordLabel);
            mainPanel.Controls.Add(userNameTextBox);
            mainPanel.Controls.Add(userNameLabel);
            mainPanel.Controls.Add(subtitleLabel);
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Location = new Point(300, 42);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(380, 402);
            mainPanel.TabIndex = 0;
            // 
            // exitButton
            // 
            exitButton.BackColor = Color.White;
            exitButton.FlatAppearance.BorderColor = Color.FromArgb(31, 41, 55);
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.Font = new Font("Inter SemiBold", 10F, FontStyle.Bold);
            exitButton.ForeColor = Color.FromArgb(31, 41, 55);
            exitButton.Image = (Image)resources.GetObject("exitButton.Image");
            exitButton.Location = new Point(209, 322);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(128, 45);
            exitButton.TabIndex = 8;
            exitButton.Text = "Exit";
            exitButton.TextAlign = ContentAlignment.MiddleRight;
            exitButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            exitButton.UseVisualStyleBackColor = false;
            exitButton.Click += ExitButton_Click;
            // 
            // loginButton
            // 
            loginButton.BackColor = Color.FromArgb(37, 99, 235);
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(29, 78, 216);
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.Font = new Font("Inter SemiBold", 10F, FontStyle.Bold);
            loginButton.ForeColor = Color.White;
            loginButton.Image = (Image)resources.GetObject("loginButton.Image");
            loginButton.Location = new Point(33, 322);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(156, 45);
            loginButton.TabIndex = 7;
            loginButton.Text = "Login";
            loginButton.TextAlign = ContentAlignment.MiddleRight;
            loginButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            loginButton.UseVisualStyleBackColor = false;
            loginButton.Click += LoginButton_Click;
            // 
            // statusLabel
            // 
            statusLabel.Font = new Font("Inter", 9F);
            statusLabel.ForeColor = Color.FromArgb(220, 38, 38);
            statusLabel.Location = new Point(33, 263);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(304, 45);
            statusLabel.TabIndex = 6;
            // 
            // passwordTextBox
            // 
            passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextBox.Font = new Font("Inter", 10.2F);
            passwordTextBox.Location = new Point(33, 221);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PlaceholderText = "Owner password";
            passwordTextBox.Size = new Size(304, 28);
            passwordTextBox.TabIndex = 5;
            passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Font = new Font("Inter", 9F);
            passwordLabel.ForeColor = Color.FromArgb(31, 41, 55);
            passwordLabel.Location = new Point(33, 196);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(80, 22);
            passwordLabel.TabIndex = 4;
            passwordLabel.Text = "Password";
            // 
            // userNameTextBox
            // 
            userNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            userNameTextBox.Font = new Font("Inter", 10.2F);
            userNameTextBox.Location = new Point(33, 154);
            userNameTextBox.Name = "userNameTextBox";
            userNameTextBox.PlaceholderText = "Owner username";
            userNameTextBox.Size = new Size(304, 28);
            userNameTextBox.TabIndex = 3;
            // 
            // userNameLabel
            // 
            userNameLabel.AutoSize = true;
            userNameLabel.Font = new Font("Inter", 9F);
            userNameLabel.ForeColor = Color.FromArgb(31, 41, 55);
            userNameLabel.Location = new Point(33, 129);
            userNameLabel.Name = "userNameLabel";
            userNameLabel.Size = new Size(83, 22);
            userNameLabel.TabIndex = 2;
            userNameLabel.Text = "Username";
            // 
            // subtitleLabel
            // 
            subtitleLabel.Font = new Font("Inter", 10F);
            subtitleLabel.ForeColor = Color.FromArgb(75, 85, 99);
            subtitleLabel.Location = new Point(33, 79);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(304, 34);
            subtitleLabel.TabIndex = 1;
            subtitleLabel.Text = "Sign in to Auto Avenue.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter", 20F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            titleLabel.Location = new Point(28, 29);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(252, 49);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Owner Sign In";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(720, 488);
            Controls.Add(mainPanel);
            Controls.Add(brandPanel);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Avenue - Owner Login";
            Load += LoginForm_Load;
            brandPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)brandLogoPictureBox).EndInit();
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel brandPanel;
        private PictureBox brandLogoPictureBox;
        private Label brandSubtitleLabel;
        private Panel mainPanel;
        private Button exitButton;
        private Button loginButton;
        private Label statusLabel;
        private TextBox passwordTextBox;
        private Label passwordLabel;
        private TextBox userNameTextBox;
        private Label userNameLabel;
        private Label subtitleLabel;
        private Label titleLabel;
    }
}
