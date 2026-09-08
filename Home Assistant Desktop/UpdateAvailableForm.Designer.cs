namespace Home_Assistant_Desktop
{
    partial class UpdateAvailableForm
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
            pictureLogo = new PictureBox();
            labelTitle = new Label();
            labelMessage = new Label();
            buttonDownload = new Button();
            buttonSkip = new Button();
            buttonDontNotify = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureLogo).BeginInit();
            SuspendLayout();
            //
            // pictureLogo
            //
            pictureLogo.Location = new Point(12, 20);
            pictureLogo.Name = "pictureLogo";
            pictureLogo.Size = new Size(32, 32);
            pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureLogo.TabIndex = 5;
            pictureLogo.TabStop = false;
            pictureLogo.Click += AnyControl_Click;
            //
            // labelTitle
            //
            labelTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            labelTitle.ForeColor = Color.WhiteSmoke;
            labelTitle.Location = new Point(54, 20);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(300, 32);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Update Available";
            labelTitle.TextAlign = ContentAlignment.MiddleLeft;
            labelTitle.Click += AnyControl_Click;
            //
            // labelMessage
            //
            labelMessage.Anchor = AnchorStyles.Top;
            labelMessage.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelMessage.ForeColor = Color.Gainsboro;
            labelMessage.Location = new Point(12, 58);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(396, 40);
            labelMessage.TabIndex = 1;
            labelMessage.Text = "Version 0.0.0 is available.";
            labelMessage.TextAlign = ContentAlignment.MiddleCenter;
            labelMessage.Click += AnyControl_Click;
            //
            // buttonDownload
            //
            buttonDownload.Anchor = AnchorStyles.Top;
            buttonDownload.BackColor = Color.FromArgb(3, 169, 244);
            buttonDownload.FlatAppearance.BorderSize = 0;
            buttonDownload.FlatStyle = FlatStyle.Flat;
            buttonDownload.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            buttonDownload.ForeColor = Color.Black;
            buttonDownload.Location = new Point(12, 112);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(396, 32);
            buttonDownload.TabIndex = 2;
            buttonDownload.Text = "Download";
            buttonDownload.UseVisualStyleBackColor = false;
            buttonDownload.Click += buttonDownload_Click;
            //
            // buttonSkip
            //
            buttonSkip.Anchor = AnchorStyles.Top;
            buttonSkip.BackColor = Color.FromArgb(40, 40, 40);
            buttonSkip.FlatAppearance.BorderSize = 0;
            buttonSkip.FlatStyle = FlatStyle.Flat;
            buttonSkip.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSkip.ForeColor = Color.WhiteSmoke;
            buttonSkip.Location = new Point(12, 154);
            buttonSkip.Name = "buttonSkip";
            buttonSkip.Size = new Size(396, 28);
            buttonSkip.TabIndex = 3;
            buttonSkip.Text = "Skip This Release";
            buttonSkip.UseVisualStyleBackColor = false;
            buttonSkip.Click += buttonSkip_Click;
            //
            // buttonDontNotify
            //
            buttonDontNotify.Anchor = AnchorStyles.Top;
            buttonDontNotify.BackColor = Color.FromArgb(40, 40, 40);
            buttonDontNotify.FlatAppearance.BorderSize = 0;
            buttonDontNotify.FlatStyle = FlatStyle.Flat;
            buttonDontNotify.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            buttonDontNotify.ForeColor = Color.WhiteSmoke;
            buttonDontNotify.Location = new Point(12, 190);
            buttonDontNotify.Name = "buttonDontNotify";
            buttonDontNotify.Size = new Size(396, 28);
            buttonDontNotify.TabIndex = 4;
            buttonDontNotify.Text = "Don't notify me for Updates";
            buttonDontNotify.UseVisualStyleBackColor = false;
            buttonDontNotify.Click += buttonDontNotify_Click;
            //
            // UpdateAvailableForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 17, 17);
            ClientSize = new Size(420, 234);
            Controls.Add(pictureLogo);
            Controls.Add(labelTitle);
            Controls.Add(labelMessage);
            Controls.Add(buttonDownload);
            Controls.Add(buttonSkip);
            Controls.Add(buttonDontNotify);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UpdateAvailableForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Update Available";
            TopMost = true;
            Load += UpdateAvailableForm_Load;
            Deactivate += UpdateAvailableForm_Deactivate;
            KeyDown += UpdateAvailableForm_KeyDown;
            Click += AnyControl_Click;
            ((System.ComponentModel.ISupportInitialize)pictureLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureLogo;
        private Label labelTitle;
        private Label labelMessage;
        private Button buttonDownload;
        private Button buttonSkip;
        private Button buttonDontNotify;
    }
}
