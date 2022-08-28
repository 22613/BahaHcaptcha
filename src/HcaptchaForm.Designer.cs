namespace BahaHcaptcha {
    partial class HcaptchaForm {
        private System.ComponentModel.IContainer components = null; protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.addressBar = new System.Windows.Forms.TextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).BeginInit();
            this.notifyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // webView
            // 
            this.webView.AllowExternalDrop = true;
            this.webView.CreationProperties = null;
            this.webView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView.Location = new System.Drawing.Point(0, 21);
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(688, 581);
            this.webView.TabIndex = 0;
            this.webView.ZoomFactor = 1D;
            // 
            // addressBar
            // 
            this.addressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.addressBar.Location = new System.Drawing.Point(0, 0);
            this.addressBar.Name = "addressBar";
            this.addressBar.ReadOnly = true;
            this.addressBar.Size = new System.Drawing.Size(688, 21);
            this.addressBar.TabIndex = 1;
            this.addressBar.Enter += new System.EventHandler(this.AddressBar_Enter);
            this.addressBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AddressBar_MouseUp);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyMenu;
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.ShowMenuItem_Click);
            // 
            // notifyMenu
            // 
            this.notifyMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMenuItem,
            this.configMenuItem,
            this.exitMenuItem});
            this.notifyMenu.Name = "notifyMenu";
            this.notifyMenu.Size = new System.Drawing.Size(117, 70);
            this.notifyMenu.DoubleClick += new System.EventHandler(this.ShowMenuItem_Click);
            // 
            // showMenuItem
            // 
            this.showMenuItem.Name = "showMenuItem";
            this.showMenuItem.Size = new System.Drawing.Size(116, 22);
            this.showMenuItem.Text = "显示(&S)";
            this.showMenuItem.Click += new System.EventHandler(this.ShowMenuItem_Click);
            // 
            // configMenuItem
            // 
            this.configMenuItem.Name = "configMenuItem";
            this.configMenuItem.Size = new System.Drawing.Size(116, 22);
            this.configMenuItem.Text = "设置(&C)";
            this.configMenuItem.Click += new System.EventHandler(this.ConfigMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitMenuItem.Text = "退出(&X)";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // HcaptchaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 602);
            this.Controls.Add(this.webView);
            this.Controls.Add(this.addressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HcaptchaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.SizeChanged += new System.EventHandler(this.HcaptchaForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.webView)).EndInit();
            this.notifyMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.TextBox addressBar;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyMenu;
        private System.Windows.Forms.ToolStripMenuItem showMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
    }
}
