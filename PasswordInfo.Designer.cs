namespace CreditStatistics
{
    partial class PasswordInfo
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordInfo));
            label1 = new Label();
            groupBox1 = new GroupBox();
            btnSavePasswd = new Button();
            tbPasswd = new TextBox();
            groupBox2 = new GroupBox();
            tbUsername = new TextBox();
            label3 = new Label();
            label2 = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            textBox1 = new TextBox();
            tabPage2 = new TabPage();
            textBox2 = new TextBox();
            tabPage3 = new TabPage();
            textBox3 = new TextBox();
            imageList1 = new ImageList(components);
            tbUname = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.ForeColor = SystemColors.ControlText;
            label1.Location = new Point(17, 32);
            label1.Name = "label1";
            label1.Size = new Size(257, 105);
            label1.TabIndex = 0;
            label1.Text = resources.GetString("label1.Text");
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tbUname);
            groupBox1.Controls.Add(btnSavePasswd);
            groupBox1.Controls.Add(tbPasswd);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 28);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(321, 242);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Web credentials";
            // 
            // btnSavePasswd
            // 
            btnSavePasswd.Location = new Point(40, 175);
            btnSavePasswd.Name = "btnSavePasswd";
            btnSavePasswd.Size = new Size(59, 23);
            btnSavePasswd.TabIndex = 2;
            btnSavePasswd.Text = "Save";
            btnSavePasswd.UseVisualStyleBackColor = true;
            btnSavePasswd.Click += btnSavePasswd_Click;
            // 
            // tbPasswd
            // 
            tbPasswd.Location = new Point(160, 203);
            tbPasswd.Name = "tbPasswd";
            tbPasswd.Size = new Size(128, 23);
            tbPasswd.TabIndex = 1;
            tbPasswd.Text = "password";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tbUsername);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(12, 294);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(321, 362);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Boinc and SSH passwords";
            // 
            // tbUsername
            // 
            tbUsername.Location = new Point(90, 319);
            tbUsername.Name = "tbUsername";
            tbUsername.Size = new Size(128, 23);
            tbUsername.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.Info;
            label3.ForeColor = SystemColors.ControlText;
            label3.Location = new Point(17, 167);
            label3.Name = "label3";
            label3.Size = new Size(289, 120);
            label3.TabIndex = 1;
            label3.Text = resources.GetString("label3.Text");
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.Info;
            label2.ForeColor = SystemColors.ControlText;
            label2.Location = new Point(17, 32);
            label2.Name = "label2";
            label2.Size = new Size(270, 90);
            label2.TabIndex = 0;
            label2.Text = resources.GetString("label2.Text");
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(413, 37);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(750, 629);
            tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(textBox1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(742, 601);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Windows (OpenSSH)";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(0, 0);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(736, 595);
            textBox1.TabIndex = 0;
            textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(textBox2);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(742, 601);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Linux (Ubuntu)";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(3, 3);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(736, 595);
            textBox2.TabIndex = 1;
            textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(textBox3);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(742, 601);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Windows Boinc";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(3, 3);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.ScrollBars = ScrollBars.Vertical;
            textBox3.Size = new Size(736, 595);
            textBox3.TabIndex = 2;
            textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // tbUname
            // 
            tbUname.Location = new Point(160, 156);
            tbUname.Name = "tbUname";
            tbUname.Size = new Size(128, 23);
            tbUname.TabIndex = 3;
            tbUname.Text = "username";
            // 
            // PasswordInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1247, 678);
            Controls.Add(tabControl1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "PasswordInfo";
            Text = "PasswordInfo";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private GroupBox groupBox1;
        private Button btnSavePasswd;
        private TextBox tbPasswd;
        private GroupBox groupBox2;
        private Label label2;
        private Label label3;
        private TextBox tbUsername;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox textBox1;
        private TextBox textBox2;
        private TabPage tabPage3;
        private TextBox textBox3;
        private TextBox tbUname;
        private ImageList imageList1;
    }
}