namespace CreditStatistics
{
    partial class EditAllConfig
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
            tbCC_config = new TextBox();
            tbPC_config = new TextBox();
            btnSaveCC = new Button();
            btnSetDefCC = new Button();
            btnSetDefPC = new Button();
            btnSavePC = new Button();
            SuspendLayout();
            // 
            // tbCC_config
            // 
            tbCC_config.Location = new Point(301, 88);
            tbCC_config.Multiline = true;
            tbCC_config.Name = "tbCC_config";
            tbCC_config.Size = new Size(361, 493);
            tbCC_config.TabIndex = 4;
            // 
            // tbPC_config
            // 
            tbPC_config.Location = new Point(701, 88);
            tbPC_config.Multiline = true;
            tbPC_config.Name = "tbPC_config";
            tbPC_config.ScrollBars = ScrollBars.Vertical;
            tbPC_config.Size = new Size(377, 493);
            tbPC_config.TabIndex = 5;
            // 
            // btnSaveCC
            // 
            btnSaveCC.ForeColor = SystemColors.Highlight;
            btnSaveCC.Location = new Point(422, 16);
            btnSaveCC.Name = "btnSaveCC";
            btnSaveCC.Size = new Size(114, 60);
            btnSaveCC.TabIndex = 12;
            btnSaveCC.Text = "Save cc_config\r\nDelete pandora";
            btnSaveCC.UseVisualStyleBackColor = true;
            btnSaveCC.Click += btnSaveCC_Click;
            // 
            // btnSetDefCC
            // 
            btnSetDefCC.ForeColor = SystemColors.Highlight;
            btnSetDefCC.Location = new Point(301, 16);
            btnSetDefCC.Name = "btnSetDefCC";
            btnSetDefCC.Size = new Size(87, 23);
            btnSetDefCC.TabIndex = 14;
            btnSetDefCC.Text = "Set Default";
            btnSetDefCC.UseVisualStyleBackColor = true;
            btnSetDefCC.Click += btnSetDefCC_Click;
            // 
            // btnSetDefPC
            // 
            btnSetDefPC.ForeColor = SystemColors.Highlight;
            btnSetDefPC.Location = new Point(701, 16);
            btnSetDefPC.Name = "btnSetDefPC";
            btnSetDefPC.Size = new Size(146, 56);
            btnSetDefPC.TabIndex = 17;
            btnSetDefPC.Text = "Load New Pandora\r\nlimits just calculated";
            btnSetDefPC.UseVisualStyleBackColor = true;
            btnSetDefPC.Click += btnSetDefPC_Click;
            // 
            // btnSavePC
            // 
            btnSavePC.ForeColor = SystemColors.Highlight;
            btnSavePC.Location = new Point(928, 12);
            btnSavePC.Name = "btnSavePC";
            btnSavePC.Size = new Size(134, 60);
            btnSavePC.TabIndex = 15;
            btnSavePC.Text = "Save cc_config\r\nand pandora";
            btnSavePC.UseVisualStyleBackColor = true;
            btnSavePC.Click += btnSavePC_Click;
            // 
            // EditAllConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1090, 605);
            Controls.Add(btnSetDefPC);
            Controls.Add(btnSavePC);
            Controls.Add(btnSetDefCC);
            Controls.Add(btnSaveCC);
            Controls.Add(tbPC_config);
            Controls.Add(tbCC_config);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "EditAllConfig";
            Text = "AccessAllConfigs";
            FormClosed += EditAllConfig_FormClosed;
            Controls.SetChildIndex(tbCC_config, 0);
            Controls.SetChildIndex(tbPC_config, 0);
            Controls.SetChildIndex(btnSaveCC, 0);
            Controls.SetChildIndex(btnSetDefCC, 0);
            Controls.SetChildIndex(btnSavePC, 0);
            Controls.SetChildIndex(btnSetDefPC, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbCC_config;
        private TextBox tbPC_config;
        private Button btnSaveCC;
        private Button btnSetDefCC;
        private Button btnSetDefPC;
        private Button btnSavePC;
    }
}