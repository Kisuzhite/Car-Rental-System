using Guna.UI2.WinForms;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            brandPanel = new Guna2Panel();
            brandSubtitleLabel = new Label();
            brandLogoPictureBox = new Guna2PictureBox();
            mainPanel = new Guna2Panel();
            exitButton = new Guna2Button();
            loginButton = new Guna2Button();
            statusLabel = new Label();
            passwordTextBox = new Guna2TextBox();
            passwordLabel = new Label();
            userNameTextBox = new Guna2TextBox();
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
            brandPanel.CustomizableEdges = customizableEdges3;
            brandPanel.Dock = DockStyle.Left;
            brandPanel.FillColor = Color.FromArgb(31, 41, 55);
            brandPanel.Location = new Point(0, 0);
            brandPanel.Name = "brandPanel";
            brandPanel.ShadowDecoration.CustomizableEdges = customizableEdges4;
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
            brandLogoPictureBox.CustomizableEdges = customizableEdges1;
            brandLogoPictureBox.Image = (Image)resources.GetObject("brandLogoPictureBox.Image");
            brandLogoPictureBox.ImageRotate = 0F;
            brandLogoPictureBox.Location = new Point(30, 38);
            brandLogoPictureBox.Name = "brandLogoPictureBox";
            brandLogoPictureBox.ShadowDecoration.CustomizableEdges = customizableEdges2;
            brandLogoPictureBox.Size = new Size(200, 125);
            brandLogoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            brandLogoPictureBox.TabIndex = 4;
            brandLogoPictureBox.TabStop = false;
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.White;
            mainPanel.BorderColor = Color.FromArgb(229, 231, 235);
            mainPanel.BorderRadius = 18;
            mainPanel.BorderThickness = 1;
            mainPanel.Controls.Add(exitButton);
            mainPanel.Controls.Add(loginButton);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(passwordTextBox);
            mainPanel.Controls.Add(passwordLabel);
            mainPanel.Controls.Add(userNameTextBox);
            mainPanel.Controls.Add(userNameLabel);
            mainPanel.Controls.Add(subtitleLabel);
            mainPanel.Controls.Add(titleLabel);
            mainPanel.CustomizableEdges = customizableEdges13;
            mainPanel.FillColor = Color.White;
            mainPanel.Location = new Point(300, 42);
            mainPanel.Name = "mainPanel";
            mainPanel.ShadowDecoration.CustomizableEdges = customizableEdges14;
            mainPanel.Size = new Size(380, 402);
            mainPanel.TabIndex = 0;
            // 
            // exitButton
            // 
            exitButton.BackColor = Color.White;
            exitButton.BorderColor = Color.FromArgb(31, 41, 55);
            exitButton.BorderRadius = 9;
            exitButton.BorderThickness = 1;
            exitButton.CustomizableEdges = customizableEdges5;
            exitButton.FillColor = Color.White;
            exitButton.Font = new Font("Inter SemiBold", 10F, FontStyle.Bold);
            exitButton.ForeColor = Color.FromArgb(31, 41, 55);
            exitButton.HoverState.FillColor = Color.FromArgb(243, 244, 246);
            exitButton.Image = (Image)resources.GetObject("exitButton.Image");
            exitButton.ImageAlign = HorizontalAlignment.Left;
            exitButton.ImageOffset = new Point(20, 0);
            exitButton.ImageSize = new Size(22, 22);
            exitButton.Location = new Point(201, 322);
            exitButton.Name = "exitButton";
            exitButton.ShadowDecoration.CustomizableEdges = customizableEdges6;
            exitButton.Size = new Size(136, 45);
            exitButton.TabIndex = 8;
            exitButton.Text = "Exit";
            exitButton.TextOffset = new Point(1, 0);
            exitButton.Click += ExitButton_Click;
            // 
            // loginButton
            // 
            loginButton.BackColor = Color.White;
            loginButton.BorderRadius = 9;
            loginButton.CustomizableEdges = customizableEdges7;
            loginButton.FillColor = Color.FromArgb(37, 99, 235);
            loginButton.Font = new Font("Inter SemiBold", 10F, FontStyle.Bold);
            loginButton.ForeColor = Color.White;
            loginButton.HoverState.FillColor = Color.FromArgb(29, 78, 216);
            loginButton.Image = (Image)resources.GetObject("loginButton.Image");
            loginButton.ImageAlign = HorizontalAlignment.Left;
            loginButton.ImageOffset = new Point(20, 0);
            loginButton.ImageSize = new Size(22, 22);
            loginButton.Location = new Point(33, 322);
            loginButton.Name = "loginButton";
            loginButton.ShadowDecoration.CustomizableEdges = customizableEdges8;
            loginButton.Size = new Size(136, 45);
            loginButton.TabIndex = 7;
            loginButton.Text = "Login";
            loginButton.TextOffset = new Point(10, 0);
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
            passwordTextBox.BorderColor = Color.FromArgb(209, 213, 219);
            passwordTextBox.BorderRadius = 8;
            passwordTextBox.Cursor = Cursors.IBeam;
            passwordTextBox.CustomizableEdges = customizableEdges9;
            passwordTextBox.DefaultText = "";
            passwordTextBox.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            passwordTextBox.Font = new Font("Inter", 10.2F);
            passwordTextBox.ForeColor = Color.FromArgb(17, 24, 39);
            passwordTextBox.HoverState.BorderColor = Color.FromArgb(37, 99, 235);
            passwordTextBox.Location = new Point(33, 218);
            passwordTextBox.Margin = new Padding(3, 4, 3, 4);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PlaceholderForeColor = Color.FromArgb(107, 114, 128);
            passwordTextBox.PlaceholderText = "Owner password";
            passwordTextBox.SelectedText = "";
            passwordTextBox.ShadowDecoration.CustomizableEdges = customizableEdges10;
            passwordTextBox.Size = new Size(304, 42);
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
            userNameTextBox.BorderColor = Color.FromArgb(209, 213, 219);
            userNameTextBox.BorderRadius = 8;
            userNameTextBox.Cursor = Cursors.IBeam;
            userNameTextBox.CustomizableEdges = customizableEdges11;
            userNameTextBox.DefaultText = "";
            userNameTextBox.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            userNameTextBox.Font = new Font("Inter", 10.2F);
            userNameTextBox.ForeColor = Color.FromArgb(17, 24, 39);
            userNameTextBox.HoverState.BorderColor = Color.FromArgb(37, 99, 235);
            userNameTextBox.Location = new Point(33, 151);
            userNameTextBox.Margin = new Padding(3, 4, 3, 4);
            userNameTextBox.Name = "userNameTextBox";
            userNameTextBox.PlaceholderForeColor = Color.FromArgb(107, 114, 128);
            userNameTextBox.PlaceholderText = "Owner username";
            userNameTextBox.SelectedText = "";
            userNameTextBox.ShadowDecoration.CustomizableEdges = customizableEdges12;
            userNameTextBox.Size = new Size(304, 42);
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

        private Guna2Panel brandPanel;
        private Guna2PictureBox brandLogoPictureBox;
        private Label brandSubtitleLabel;
        private Guna2Panel mainPanel;
        private Guna2Button exitButton;
        private Guna2Button loginButton;
        private Label statusLabel;
        private Guna2TextBox passwordTextBox;
        private Label passwordLabel;
        private Guna2TextBox userNameTextBox;
        private Label userNameLabel;
        private Label subtitleLabel;
        private Label titleLabel;
    }
}



