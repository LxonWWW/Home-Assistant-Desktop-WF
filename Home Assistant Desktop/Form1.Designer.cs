namespace Home_Assistant_Desktop
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            mainWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            itemOpenInBrowser = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            itemAlignViewTopLeft = new ToolStripMenuItem();
            itemAlignViewTopRight = new ToolStripMenuItem();
            itemAlignViewBottomLeft = new ToolStripMenuItem();
            itemAlignViewBottomRight = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            itemSetStartURL = new ToolStripMenuItem();
            itemStayOnTop = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            itemRestartApplication = new ToolStripMenuItem();
            itemResetApplication = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            itemSaveCurrentSettings = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            itemQuit = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)mainWebView).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // mainWebView
            // 
            mainWebView.AllowExternalDrop = true;
            mainWebView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainWebView.CreationProperties = null;
            mainWebView.DefaultBackgroundColor = Color.White;
            mainWebView.Location = new Point(-1, 0);
            mainWebView.Name = "mainWebView";
            mainWebView.Size = new Size(389, 812);
            mainWebView.Source = new Uri("http://homeassistant.local:8123", UriKind.Absolute);
            mainWebView.TabIndex = 0;
            mainWebView.ZoomFactor = 1D;
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipText = "Click here to Open";
            notifyIcon1.BalloonTipTitle = "Home Assistant Desktop";
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Home Assistant Desktop";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseClick += notifyIcon1_MouseClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.BackColor = Color.FromArgb(17, 17, 17);
            contextMenuStrip1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { itemOpenInBrowser, toolStripSeparator1, itemAlignViewTopLeft, itemAlignViewTopRight, itemAlignViewBottomLeft, itemAlignViewBottomRight, toolStripSeparator4, itemSetStartURL, itemStayOnTop, toolStripSeparator2, itemRestartApplication, itemResetApplication, toolStripSeparator3, itemSaveCurrentSettings, toolStripSeparator5, itemQuit });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.RenderMode = ToolStripRenderMode.System;
            contextMenuStrip1.Size = new Size(205, 298);
            // 
            // itemOpenInBrowser
            // 
            itemOpenInBrowser.ForeColor = Color.WhiteSmoke;
            itemOpenInBrowser.Name = "itemOpenInBrowser";
            itemOpenInBrowser.Size = new Size(204, 22);
            itemOpenInBrowser.Text = "Open in Browser";
            itemOpenInBrowser.Click += itemOpenInBrowser_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(201, 6);
            // 
            // itemAlignViewTopLeft
            // 
            itemAlignViewTopLeft.ForeColor = Color.WhiteSmoke;
            itemAlignViewTopLeft.Name = "itemAlignViewTopLeft";
            itemAlignViewTopLeft.Size = new Size(204, 22);
            itemAlignViewTopLeft.Text = "Align View Top Left";
            itemAlignViewTopLeft.Click += itemAlignViewTopLeft_Click;
            // 
            // itemAlignViewTopRight
            // 
            itemAlignViewTopRight.ForeColor = Color.WhiteSmoke;
            itemAlignViewTopRight.Name = "itemAlignViewTopRight";
            itemAlignViewTopRight.Size = new Size(204, 22);
            itemAlignViewTopRight.Text = "Align View Top Right";
            itemAlignViewTopRight.Click += itemAlignViewTopRight_Click;
            // 
            // itemAlignViewBottomLeft
            // 
            itemAlignViewBottomLeft.ForeColor = Color.WhiteSmoke;
            itemAlignViewBottomLeft.Name = "itemAlignViewBottomLeft";
            itemAlignViewBottomLeft.Size = new Size(204, 22);
            itemAlignViewBottomLeft.Text = "Align View Bottom Left";
            itemAlignViewBottomLeft.Click += itemAlignViewBottomLeft_Click;
            // 
            // itemAlignViewBottomRight
            // 
            itemAlignViewBottomRight.ForeColor = Color.WhiteSmoke;
            itemAlignViewBottomRight.Name = "itemAlignViewBottomRight";
            itemAlignViewBottomRight.Size = new Size(204, 22);
            itemAlignViewBottomRight.Text = "Align View Bottom Right";
            itemAlignViewBottomRight.Click += itemAlignViewBottomRight_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(201, 6);
            // 
            // itemSetStartURL
            // 
            itemSetStartURL.ForeColor = Color.WhiteSmoke;
            itemSetStartURL.Name = "itemSetStartURL";
            itemSetStartURL.Size = new Size(204, 22);
            itemSetStartURL.Text = "Set Start URL";
            itemSetStartURL.Click += itemSetStartURL_Click;
            // 
            // itemStayOnTop
            // 
            itemStayOnTop.ForeColor = Color.WhiteSmoke;
            itemStayOnTop.Name = "itemStayOnTop";
            itemStayOnTop.Size = new Size(204, 22);
            itemStayOnTop.Text = "Stay on Top";
            itemStayOnTop.Click += itemStayOnTop_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(201, 6);
            // 
            // itemRestartApplication
            // 
            itemRestartApplication.ForeColor = Color.WhiteSmoke;
            itemRestartApplication.Name = "itemRestartApplication";
            itemRestartApplication.Size = new Size(204, 22);
            itemRestartApplication.Text = "Restart Application";
            itemRestartApplication.Click += itemRestartApplication_Click;
            // 
            // itemResetApplication
            // 
            itemResetApplication.ForeColor = Color.WhiteSmoke;
            itemResetApplication.Name = "itemResetApplication";
            itemResetApplication.Size = new Size(204, 22);
            itemResetApplication.Text = "⚠ Reset Application";
            itemResetApplication.Click += itemResetApplication_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(201, 6);
            // 
            // itemSaveCurrentSettings
            // 
            itemSaveCurrentSettings.ForeColor = Color.WhiteSmoke;
            itemSaveCurrentSettings.Name = "itemSaveCurrentSettings";
            itemSaveCurrentSettings.Size = new Size(204, 22);
            itemSaveCurrentSettings.Text = "Save Current Settings";
            itemSaveCurrentSettings.Click += itemSaveCurrentSettings_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(201, 6);
            // 
            // itemQuit
            // 
            itemQuit.ForeColor = Color.WhiteSmoke;
            itemQuit.Name = "itemQuit";
            itemQuit.Size = new Size(204, 22);
            itemQuit.Text = "Quit";
            itemQuit.Click += itemQuit_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(17, 17, 17);
            ClientSize = new Size(441, 811);
            Controls.Add(mainWebView);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Form1";
            Deactivate += Form1_Deactivate;
            Load += Form1_Load;
            SizeChanged += Form1_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)mainWebView).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Microsoft.Web.WebView2.WinForms.WebView2 mainWebView;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem itemOpenInBrowser;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem itemStayOnTop;
        private ToolStripMenuItem itemStartAtLogin;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem itemRestartApplication;
        private ToolStripMenuItem itemResetApplication;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem itemQuit;
        private ToolStripMenuItem itemAlignViewTopLeft;
        private ToolStripMenuItem itemAlignViewTopRight;
        private ToolStripMenuItem itemAlignViewBottomLeft;
        private ToolStripMenuItem itemAlignViewBottomRight;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem itemSaveCurrentSettings;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem itemSetStartURL;
        private Button button1;
    }
}