using Guna.UI2.WinForms;

namespace Car_Rental_System
{
    partial class AvailabilityCalendar
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            headerPanel = new Guna2Panel();
            nextButton = new Guna2Button();
            previousButton = new Guna2Button();
            rangeLabel = new Label();
            helperLabel = new Label();
            titleLabel = new Label();
            contentPanel = new Guna2Panel();
            timelineScrollPanel = new Panel();
            legendPanel = new Guna2Panel();
            rentedLegendLabel = new Label();
            rentedLegendDot = new Panel();
            reservedLegendLabel = new Label();
            reservedLegendDot = new Panel();
            headerPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            legendPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.Transparent;
            headerPanel.BorderColor = Color.FromArgb(243, 244, 246);
            headerPanel.Controls.Add(nextButton);
            headerPanel.Controls.Add(previousButton);
            headerPanel.Controls.Add(rangeLabel);
            headerPanel.Controls.Add(helperLabel);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.CustomizableEdges = customizableEdges5;
            headerPanel.Dock = DockStyle.Top;
            headerPanel.FillColor = Color.FromArgb(243, 244, 246);
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.ShadowDecoration.CustomizableEdges = customizableEdges6;
            headerPanel.Size = new Size(1514, 112);
            headerPanel.TabIndex = 0;
            // 
            // nextButton
            // 
            nextButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            nextButton.BorderColor = Color.FromArgb(203, 213, 225);
            nextButton.BorderRadius = 8;
            nextButton.BorderThickness = 1;
            nextButton.CustomizableEdges = customizableEdges1;
            nextButton.FillColor = Color.White;
            nextButton.Font = new Font("Inter Medium", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            nextButton.ForeColor = Color.FromArgb(17, 24, 39);
            nextButton.HoverState.FillColor = Color.FromArgb(248, 250, 252);
            nextButton.Location = new Point(1434, 9);
            nextButton.Name = "nextButton";
            nextButton.ShadowDecoration.CustomizableEdges = customizableEdges2;
            nextButton.Size = new Size(44, 42);
            nextButton.TabIndex = 5;
            nextButton.Text = ">";
            nextButton.Click += NextButton_Click;
            // 
            // previousButton
            // 
            previousButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            previousButton.BorderColor = Color.FromArgb(203, 213, 225);
            previousButton.BorderRadius = 8;
            previousButton.BorderThickness = 1;
            previousButton.CustomizableEdges = customizableEdges3;
            previousButton.FillColor = Color.White;
            previousButton.Font = new Font("Inter Medium", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            previousButton.ForeColor = Color.FromArgb(17, 24, 39);
            previousButton.HoverState.FillColor = Color.FromArgb(248, 250, 252);
            previousButton.Location = new Point(1226, 9);
            previousButton.Name = "previousButton";
            previousButton.ShadowDecoration.CustomizableEdges = customizableEdges4;
            previousButton.Size = new Size(44, 42);
            previousButton.TabIndex = 4;
            previousButton.Text = "<";
            previousButton.Click += PreviousButton_Click;
            // 
            // rangeLabel
            // 
            rangeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rangeLabel.Font = new Font("Inter SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rangeLabel.ForeColor = Color.FromArgb(17, 24, 39);
            rangeLabel.Location = new Point(1276, 14);
            rangeLabel.Name = "rangeLabel";
            rangeLabel.Size = new Size(152, 32);
            rangeLabel.TabIndex = 2;
            rangeLabel.Text = "May 2026";
            rangeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // helperLabel
            // 
            helperLabel.AutoSize = true;
            helperLabel.Font = new Font("Inter SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            helperLabel.ForeColor = Color.FromArgb(75, 85, 99);
            helperLabel.Location = new Point(24, 48);
            helperLabel.Name = "helperLabel";
            helperLabel.Size = new Size(322, 24);
            helperLabel.TabIndex = 1;
            helperLabel.Text = "Requested booking: May 18 - 19, 2026";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Inter", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(17, 24, 39);
            titleLabel.Location = new Point(22, 1);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(321, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Fleet Availability";
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.FromArgb(243, 244, 246);
            contentPanel.Controls.Add(timelineScrollPanel);
            contentPanel.Controls.Add(legendPanel);
            contentPanel.CustomizableEdges = customizableEdges9;
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.FillColor = Color.FromArgb(243, 244, 246);
            contentPanel.Location = new Point(0, 112);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(24, 0, 24, 18);
            contentPanel.ShadowDecoration.CustomizableEdges = customizableEdges10;
            contentPanel.Size = new Size(1514, 678);
            contentPanel.TabIndex = 1;
            // 
            // timelineScrollPanel
            // 
            timelineScrollPanel.AutoScroll = true;
            timelineScrollPanel.BackColor = Color.White;
            timelineScrollPanel.Dock = DockStyle.Fill;
            timelineScrollPanel.Location = new Point(24, 50);
            timelineScrollPanel.Name = "timelineScrollPanel";
            timelineScrollPanel.Size = new Size(1466, 610);
            timelineScrollPanel.TabIndex = 1;
            timelineScrollPanel.Resize += TimelineScrollPanel_Resize;
            // 
            // legendPanel
            // 
            legendPanel.BackColor = Color.Transparent;
            legendPanel.Controls.Add(rentedLegendLabel);
            legendPanel.Controls.Add(rentedLegendDot);
            legendPanel.Controls.Add(reservedLegendLabel);
            legendPanel.Controls.Add(reservedLegendDot);
            legendPanel.CustomizableEdges = customizableEdges7;
            legendPanel.Dock = DockStyle.Top;
            legendPanel.FillColor = Color.FromArgb(243, 244, 246);
            legendPanel.Location = new Point(24, 0);
            legendPanel.Name = "legendPanel";
            legendPanel.ShadowDecoration.CustomizableEdges = customizableEdges8;
            legendPanel.Size = new Size(1466, 50);
            legendPanel.TabIndex = 0;
            // 
            // rentedLegendLabel
            // 
            rentedLegendLabel.AutoSize = true;
            rentedLegendLabel.Font = new Font("Inter SemiBold", 9F, FontStyle.Bold);
            rentedLegendLabel.ForeColor = Color.FromArgb(31, 41, 55);
            rentedLegendLabel.Location = new Point(115, 13);
            rentedLegendLabel.Name = "rentedLegendLabel";
            rentedLegendLabel.Size = new Size(61, 22);
            rentedLegendLabel.TabIndex = 3;
            rentedLegendLabel.Text = "Rented";
            // 
            // rentedLegendDot
            // 
            rentedLegendDot.BackColor = Color.FromArgb(236, 119, 110);
            rentedLegendDot.Location = new Point(103, 17);
            rentedLegendDot.Name = "rentedLegendDot";
            rentedLegendDot.Size = new Size(12, 12);
            rentedLegendDot.TabIndex = 2;
            // 
            // reservedLegendLabel
            // 
            reservedLegendLabel.AutoSize = true;
            reservedLegendLabel.Font = new Font("Inter SemiBold", 9F, FontStyle.Bold);
            reservedLegendLabel.ForeColor = Color.FromArgb(31, 41, 55);
            reservedLegendLabel.Location = new Point(18, 13);
            reservedLegendLabel.Name = "reservedLegendLabel";
            reservedLegendLabel.Size = new Size(79, 22);
            reservedLegendLabel.TabIndex = 1;
            reservedLegendLabel.Text = "Reserved";
            // 
            // reservedLegendDot
            // 
            reservedLegendDot.BackColor = Color.FromArgb(224, 170, 73);
            reservedLegendDot.Location = new Point(3, 17);
            reservedLegendDot.Name = "reservedLegendDot";
            reservedLegendDot.Size = new Size(12, 12);
            reservedLegendDot.TabIndex = 0;
            // 
            // AvailabilityCalendar
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(243, 244, 246);
            ClientSize = new Size(1514, 790);
            Controls.Add(contentPanel);
            Controls.Add(headerPanel);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1180, 700);
            Name = "AvailabilityCalendar";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Auto Avenue - Availability Calendar";
            Load += AvailabilityCalendar_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            contentPanel.ResumeLayout(false);
            legendPanel.ResumeLayout(false);
            legendPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna2Panel headerPanel;
        private Guna2Button nextButton;
        private Label rangeLabel;
        private Label helperLabel;
        private Label titleLabel;
        private Guna2Panel contentPanel;
        private Guna2Panel legendPanel;
        private Label rentedLegendLabel;
        private Panel rentedLegendDot;
        private Label reservedLegendLabel;
        private Panel reservedLegendDot;
        private Panel timelineScrollPanel;
        private Guna2Button previousButton;
    }
}
