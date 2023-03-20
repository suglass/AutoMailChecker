namespace MailChecker
{
    partial class frmProxyType
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rdoHttp = new System.Windows.Forms.RadioButton();
            this.rdoSocks5 = new System.Windows.Forms.RadioButton();
            this.rdoSocks4 = new System.Windows.Forms.RadioButton();
            this.btnProxyTypeSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdoHttp
            // 
            this.rdoHttp.AutoSize = true;
            this.rdoHttp.Checked = true;
            this.rdoHttp.Location = new System.Drawing.Point(31, 26);
            this.rdoHttp.Name = "rdoHttp";
            this.rdoHttp.Size = new System.Drawing.Size(61, 17);
            this.rdoHttp.TabIndex = 0;
            this.rdoHttp.TabStop = true;
            this.rdoHttp.Text = "HTTPS";
            this.rdoHttp.UseVisualStyleBackColor = true;
            // 
            // rdoSocks5
            // 
            this.rdoSocks5.AutoSize = true;
            this.rdoSocks5.Location = new System.Drawing.Point(31, 92);
            this.rdoSocks5.Name = "rdoSocks5";
            this.rdoSocks5.Size = new System.Drawing.Size(70, 17);
            this.rdoSocks5.TabIndex = 0;
            this.rdoSocks5.TabStop = true;
            this.rdoSocks5.Text = "SOCKS 5";
            this.rdoSocks5.UseVisualStyleBackColor = true;
            // 
            // rdoSocks4
            // 
            this.rdoSocks4.AutoSize = true;
            this.rdoSocks4.Location = new System.Drawing.Point(31, 58);
            this.rdoSocks4.Name = "rdoSocks4";
            this.rdoSocks4.Size = new System.Drawing.Size(70, 17);
            this.rdoSocks4.TabIndex = 0;
            this.rdoSocks4.TabStop = true;
            this.rdoSocks4.Text = "SOCKS 4";
            this.rdoSocks4.UseVisualStyleBackColor = true;
            // 
            // btnProxyTypeSet
            // 
            this.btnProxyTypeSet.Location = new System.Drawing.Point(128, 34);
            this.btnProxyTypeSet.Name = "btnProxyTypeSet";
            this.btnProxyTypeSet.Size = new System.Drawing.Size(75, 64);
            this.btnProxyTypeSet.TabIndex = 1;
            this.btnProxyTypeSet.Text = "SET";
            this.btnProxyTypeSet.UseVisualStyleBackColor = true;
            this.btnProxyTypeSet.Click += new System.EventHandler(this.btnProxyTypeSet_Click);
            // 
            // frmProxyType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 131);
            this.Controls.Add(this.btnProxyTypeSet);
            this.Controls.Add(this.rdoSocks4);
            this.Controls.Add(this.rdoSocks5);
            this.Controls.Add(this.rdoHttp);
            this.Name = "frmProxyType";
            this.Text = "frmProxyType";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoHttp;
        private System.Windows.Forms.RadioButton rdoSocks5;
        private System.Windows.Forms.RadioButton rdoSocks4;
        private System.Windows.Forms.Button btnProxyTypeSet;
    }
}