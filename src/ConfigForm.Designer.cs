namespace BahaHcaptcha {
    partial class ConfigForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.proxyBar = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listenPort = new System.Windows.Forms.NumericUpDown();
            this.clearCookie = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bahaSearchReplaceBar = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bilibiliSearchReplaceBar = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.listenPort)).BeginInit();
            this.SuspendLayout();
            // 
            // proxyBar
            // 
            this.proxyBar.Location = new System.Drawing.Point(13, 80);
            this.proxyBar.Margin = new System.Windows.Forms.Padding(4);
            this.proxyBar.Name = "proxyBar";
            this.proxyBar.Size = new System.Drawing.Size(228, 23);
            this.proxyBar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "外部代理服务器(如果没有就留空):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "监听端口:";
            // 
            // listenPort
            // 
            this.listenPort.Location = new System.Drawing.Point(13, 30);
            this.listenPort.Margin = new System.Windows.Forms.Padding(4);
            this.listenPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.listenPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.listenPort.Name = "listenPort";
            this.listenPort.Size = new System.Drawing.Size(132, 23);
            this.listenPort.TabIndex = 4;
            this.listenPort.Value = new decimal(new int[] {
            5100,
            0,
            0,
            0});
            // 
            // clearCookie
            // 
            this.clearCookie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearCookie.AutoSize = true;
            this.clearCookie.Location = new System.Drawing.Point(13, 350);
            this.clearCookie.Name = "clearCookie";
            this.clearCookie.Size = new System.Drawing.Size(202, 21);
            this.clearCookie.TabIndex = 5;
            this.clearCookie.Text = "启动时清理超过12小时的Cookie";
            this.clearCookie.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 110);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "巴哈搜索关键词替换:";
            // 
            // bahaSearchReplaceBar
            // 
            this.bahaSearchReplaceBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bahaSearchReplaceBar.Location = new System.Drawing.Point(13, 130);
            this.bahaSearchReplaceBar.Multiline = true;
            this.bahaSearchReplaceBar.Name = "bahaSearchReplaceBar";
            this.bahaSearchReplaceBar.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.bahaSearchReplaceBar.Size = new System.Drawing.Size(390, 90);
            this.bahaSearchReplaceBar.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 230);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "哔哩哔哩搜索关键词替换:";
            // 
            // bilibiliSearchReplaceBar
            // 
            this.bilibiliSearchReplaceBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bilibiliSearchReplaceBar.Location = new System.Drawing.Point(13, 250);
            this.bilibiliSearchReplaceBar.Multiline = true;
            this.bilibiliSearchReplaceBar.Name = "bilibiliSearchReplaceBar";
            this.bilibiliSearchReplaceBar.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.bilibiliSearchReplaceBar.Size = new System.Drawing.Size(390, 90);
            this.bilibiliSearchReplaceBar.TabIndex = 6;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 383);
            this.Controls.Add(this.bilibiliSearchReplaceBar);
            this.Controls.Add(this.bahaSearchReplaceBar);
            this.Controls.Add(this.clearCookie);
            this.Controls.Add(this.listenPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.proxyBar);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.listenPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox proxyBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown listenPort;
        private System.Windows.Forms.CheckBox clearCookie;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox bahaSearchReplaceBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox bilibiliSearchReplaceBar;
    }
}