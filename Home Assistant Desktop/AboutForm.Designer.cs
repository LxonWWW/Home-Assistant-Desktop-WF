namespace Home_Assistant_Desktop
{
    partial class AboutForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureLogo = new PictureBox();
            labelAppName = new Label();
            labelVersion = new Label();
            linkGitHub = new LinkLabel();
            labelAuthor = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureLogo).BeginInit();
            SuspendLayout();
            //
            // pictureLogo
            //
            pictureLogo.Anchor = AnchorStyles.Top;
            pictureLogo.Location = new Point(162, 32);
            pictureLogo.Name = "pictureLogo";
            pictureLogo.Size = new Size(96, 96);
            pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureLogo.TabIndex = 0;
            pictureLogo.TabStop = false;
            pictureLogo.Click += AnyControl_Click;
            //
            // labelAppName
            //
            labelAppName.Anchor = AnchorStyles.Top;
            labelAppName.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            labelAppName.ForeColor = Color.WhiteSmoke;
            labelAppName.Location = new Point(12, 140);
            labelAppName.Name = "labelAppName";
            labelAppName.Size = new Size(396, 40);
            labelAppName.TabIndex = 1;
            labelAppName.Text = "Home Assistant Desktop";
            labelAppName.TextAlign = ContentAlignment.MiddleCenter;
            labelAppName.Click += AnyControl_Click;
            //
            // labelVersion
            //
            labelVersion.Anchor = AnchorStyles.Top;
            labelVersion.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelVersion.ForeColor = Color.Gainsboro;
            labelVersion.Location = new Point(12, 182);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(396, 20);
            labelVersion.TabIndex = 2;
            labelVersion.Text = "Version 0.0.0";
            labelVersion.TextAlign = ContentAlignment.MiddleCenter;
            labelVersion.Click += AnyControl_Click;
            //
            // linkGitHub
            //
            linkGitHub.Anchor = AnchorStyles.Top;
            linkGitHub.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            linkGitHub.LinkColor = Color.FromArgb(3, 169, 244);
            linkGitHub.ActiveLinkColor = Color.FromArgb(3, 169, 244);
            linkGitHub.VisitedLinkColor = Color.FromArgb(3, 169, 244);
            linkGitHub.Location = new Point(12, 214);
            linkGitHub.Name = "linkGitHub";
            linkGitHub.Size = new Size(396, 20);
            linkGitHub.TabIndex = 3;
            linkGitHub.TabStop = true;
            linkGitHub.Text = "GitHub";
            linkGitHub.TextAlign = ContentAlignment.MiddleCenter;
            linkGitHub.LinkClicked += linkGitHub_LinkClicked;
            //
            // labelAuthor
            //
            labelAuthor.Anchor = AnchorStyles.Top;
            labelAuthor.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelAuthor.ForeColor = Color.Gainsboro;
            labelAuthor.Location = new Point(12, 244);
            labelAuthor.Name = "labelAuthor";
            labelAuthor.Size = new Size(396, 20);
            labelAuthor.TabIndex = 4;
            labelAuthor.Text = "Made with ❤ by Leon Dierkes";
            labelAuthor.TextAlign = ContentAlignment.MiddleCenter;
            labelAuthor.Click += AnyControl_Click;
            //
            // AboutForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 17, 17);
            ClientSize = new Size(420, 284);
            Controls.Add(labelAuthor);
            Controls.Add(linkGitHub);
            Controls.Add(labelVersion);
            Controls.Add(labelAppName);
            Controls.Add(pictureLogo);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "About";
            TopMost = true;
            Load += AboutForm_Load;
            Deactivate += AboutForm_Deactivate;
            KeyDown += AboutForm_KeyDown;
            Click += AnyControl_Click;
            ((System.ComponentModel.ISupportInitialize)pictureLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureLogo;
        private Label labelAppName;
        private Label labelVersion;
        private LinkLabel linkGitHub;
        private Label labelAuthor;
    }
}
