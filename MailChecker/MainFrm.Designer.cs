namespace MailChecker
{
    partial class MainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_launch = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btn_close = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btn_open_proxy = new System.Windows.Forms.Button();
            this.btn_open_acc = new System.Windows.Forms.Button();
            this.txt_thread_num = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.txt_proxy_path = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.txt_acc_path = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.txt_last_log = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txt_log = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txt_last_log, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 63);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 189F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(728, 657);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.btn_open_proxy);
            this.panel1.Controls.Add(this.btn_open_acc);
            this.panel1.Controls.Add(this.txt_thread_num);
            this.panel1.Controls.Add(this.materialLabel4);
            this.panel1.Controls.Add(this.txt_proxy_path);
            this.panel1.Controls.Add(this.materialLabel2);
            this.panel1.Controls.Add(this.txt_acc_path);
            this.panel1.Controls.Add(this.materialLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(722, 183);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btn_launch, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_close, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 140);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(722, 43);
            this.tableLayoutPanel2.TabIndex = 17;
            // 
            // btn_launch
            // 
            this.btn_launch.AutoSize = true;
            this.btn_launch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_launch.Depth = 0;
            this.btn_launch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_launch.Icon = null;
            this.btn_launch.Location = new System.Drawing.Point(364, 3);
            this.btn_launch.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_launch.Name = "btn_launch";
            this.btn_launch.Primary = true;
            this.btn_launch.Size = new System.Drawing.Size(144, 37);
            this.btn_launch.TabIndex = 15;
            this.btn_launch.Text = "Check Start";
            this.btn_launch.UseVisualStyleBackColor = true;
            this.btn_launch.Click += new System.EventHandler(this.btn_launch_Click);
            // 
            // btn_close
            // 
            this.btn_close.AutoSize = true;
            this.btn_close.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_close.Depth = 0;
            this.btn_close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_close.Icon = null;
            this.btn_close.Location = new System.Drawing.Point(514, 3);
            this.btn_close.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_close.Name = "btn_close";
            this.btn_close.Primary = true;
            this.btn_close.Size = new System.Drawing.Size(144, 37);
            this.btn_close.TabIndex = 13;
            this.btn_close.Text = "close all";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_open_proxy
            // 
            this.btn_open_proxy.BackColor = System.Drawing.Color.Transparent;
            this.btn_open_proxy.BackgroundImage = global::MailChecker.Properties.Resources.Opened_Folder_26px;
            this.btn_open_proxy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_open_proxy.FlatAppearance.BorderSize = 0;
            this.btn_open_proxy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btn_open_proxy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btn_open_proxy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_open_proxy.Location = new System.Drawing.Point(691, 53);
            this.btn_open_proxy.Name = "btn_open_proxy";
            this.btn_open_proxy.Size = new System.Drawing.Size(25, 25);
            this.btn_open_proxy.TabIndex = 11;
            this.btn_open_proxy.UseVisualStyleBackColor = false;
            this.btn_open_proxy.Click += new System.EventHandler(this.btn_open_proxy_Click);
            // 
            // btn_open_acc
            // 
            this.btn_open_acc.BackColor = System.Drawing.Color.Transparent;
            this.btn_open_acc.BackgroundImage = global::MailChecker.Properties.Resources.Opened_Folder_26px;
            this.btn_open_acc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_open_acc.FlatAppearance.BorderSize = 0;
            this.btn_open_acc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btn_open_acc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btn_open_acc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_open_acc.Location = new System.Drawing.Point(691, 14);
            this.btn_open_acc.Name = "btn_open_acc";
            this.btn_open_acc.Size = new System.Drawing.Size(25, 25);
            this.btn_open_acc.TabIndex = 12;
            this.btn_open_acc.UseVisualStyleBackColor = false;
            this.btn_open_acc.Click += new System.EventHandler(this.btn_open_acc_Click);
            // 
            // txt_thread_num
            // 
            this.txt_thread_num.Depth = 0;
            this.txt_thread_num.Hint = "";
            this.txt_thread_num.Location = new System.Drawing.Point(146, 94);
            this.txt_thread_num.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_thread_num.MaxLength = 32767;
            this.txt_thread_num.MouseState = MaterialSkin.MouseState.HOVER;
            this.txt_thread_num.Name = "txt_thread_num";
            this.txt_thread_num.PasswordChar = '\0';
            this.txt_thread_num.SelectedText = "";
            this.txt_thread_num.SelectionLength = 0;
            this.txt_thread_num.SelectionStart = 0;
            this.txt_thread_num.Size = new System.Drawing.Size(42, 23);
            this.txt_thread_num.TabIndex = 8;
            this.txt_thread_num.TabStop = false;
            this.txt_thread_num.Text = "5";
            this.txt_thread_num.UseSystemPasswordChar = false;
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel4.Location = new System.Drawing.Point(11, 96);
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(112, 19);
            this.materialLabel4.TabIndex = 6;
            this.materialLabel4.Text = "Thread Number";
            // 
            // txt_proxy_path
            // 
            this.txt_proxy_path.Depth = 0;
            this.txt_proxy_path.Hint = "";
            this.txt_proxy_path.Location = new System.Drawing.Point(147, 52);
            this.txt_proxy_path.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_proxy_path.MaxLength = 32767;
            this.txt_proxy_path.MouseState = MaterialSkin.MouseState.HOVER;
            this.txt_proxy_path.Name = "txt_proxy_path";
            this.txt_proxy_path.PasswordChar = '\0';
            this.txt_proxy_path.SelectedText = "";
            this.txt_proxy_path.SelectionLength = 0;
            this.txt_proxy_path.SelectionStart = 0;
            this.txt_proxy_path.Size = new System.Drawing.Size(526, 23);
            this.txt_proxy_path.TabIndex = 9;
            this.txt_proxy_path.TabStop = false;
            this.txt_proxy_path.UseSystemPasswordChar = false;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(11, 55);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(102, 19);
            this.materialLabel2.TabIndex = 6;
            this.materialLabel2.Text = "Proxy list TXT";
            // 
            // txt_acc_path
            // 
            this.txt_acc_path.Depth = 0;
            this.txt_acc_path.Hint = "";
            this.txt_acc_path.Location = new System.Drawing.Point(147, 12);
            this.txt_acc_path.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_acc_path.MaxLength = 32767;
            this.txt_acc_path.MouseState = MaterialSkin.MouseState.HOVER;
            this.txt_acc_path.Name = "txt_acc_path";
            this.txt_acc_path.PasswordChar = '\0';
            this.txt_acc_path.SelectedText = "";
            this.txt_acc_path.SelectionLength = 0;
            this.txt_acc_path.SelectionStart = 0;
            this.txt_acc_path.Size = new System.Drawing.Size(526, 23);
            this.txt_acc_path.TabIndex = 10;
            this.txt_acc_path.TabStop = false;
            this.txt_acc_path.UseSystemPasswordChar = false;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(11, 14);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(131, 19);
            this.materialLabel1.TabIndex = 7;
            this.materialLabel1.Text = "User account CSV";
            // 
            // txt_last_log
            // 
            this.txt_last_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_last_log.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_last_log.ForeColor = System.Drawing.Color.Tomato;
            this.txt_last_log.Location = new System.Drawing.Point(3, 637);
            this.txt_last_log.Name = "txt_last_log";
            this.txt_last_log.Size = new System.Drawing.Size(722, 20);
            this.txt_last_log.TabIndex = 1;
            this.txt_last_log.Text = "label1";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txt_log);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 192);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(722, 442);
            this.panel2.TabIndex = 2;
            // 
            // txt_log
            // 
            this.txt_log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txt_log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_log.ForeColor = System.Drawing.SystemColors.Info;
            this.txt_log.Location = new System.Drawing.Point(0, 0);
            this.txt_log.Name = "txt_log";
            this.txt_log.Size = new System.Drawing.Size(720, 440);
            this.txt_log.TabIndex = 3;
            this.txt_log.Text = "";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 723);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainFrm";
            this.Padding = new System.Windows.Forms.Padding(3, 63, 3, 3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web Automation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_open_proxy;
        private System.Windows.Forms.Button btn_open_acc;
        private MaterialSkin.Controls.MaterialSingleLineTextField txt_proxy_path;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialSingleLineTextField txt_acc_path;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private System.Windows.Forms.Label txt_last_log;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox txt_log;
        private MaterialSkin.Controls.MaterialSingleLineTextField txt_thread_num;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialRaisedButton btn_launch;
        private MaterialSkin.Controls.MaterialRaisedButton btn_close;
    }
}

