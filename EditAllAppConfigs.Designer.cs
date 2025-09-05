namespace CreditStatistics
{
    partial class EditAllAppConfigs
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
            radioBoxGroup1 = new RadioBoxGroup();
            rtbLocalHostsBT = new RichTextBox();
            btnCountAC = new Button();
            btnSendAppConfig = new Button();
            SuspendLayout();
            // 
            // radioBoxGroup1
            // 
            radioBoxGroup1.Location = new Point(278, 115);
            radioBoxGroup1.Name = "radioBoxGroup1";
            radioBoxGroup1.ProjectStats = null;
            radioBoxGroup1.Size = new Size(264, 473);
            radioBoxGroup1.TabIndex = 5;
            // 
            // rtbLocalHostsBT
            // 
            rtbLocalHostsBT.Location = new Point(578, 123);
            rtbLocalHostsBT.Name = "rtbLocalHostsBT";
            rtbLocalHostsBT.Size = new Size(400, 465);
            rtbLocalHostsBT.TabIndex = 8;
            rtbLocalHostsBT.Text = "";
            rtbLocalHostsBT.WordWrap = false;
            // 
            // btnCountAC
            // 
            btnCountAC.ForeColor = SystemColors.Highlight;
            btnCountAC.Location = new Point(330, 34);
            btnCountAC.Name = "btnCountAC";
            btnCountAC.Size = new Size(110, 64);
            btnCountAC.TabIndex = 7;
            btnCountAC.Text = "Fetch all project\r\napp_config.xml\r\nfrom all remotes";
            btnCountAC.UseVisualStyleBackColor = true;
            btnCountAC.Click += btnCountAC_Click;
            // 
            // btnSendAppConfig
            // 
            btnSendAppConfig.ForeColor = SystemColors.Highlight;
            btnSendAppConfig.Location = new Point(716, 34);
            btnSendAppConfig.Name = "btnSendAppConfig";
            btnSendAppConfig.Size = new Size(129, 64);
            btnSendAppConfig.TabIndex = 9;
            btnSendAppConfig.Text = "Save Changes, set\r\nsystem to read the\r\ncc and app configs";
            btnSendAppConfig.UseVisualStyleBackColor = true;
            btnSendAppConfig.Click += btnSendAppConfig_Click;
            // 
            // EditAllAppConfigs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1027, 616);
            Controls.Add(btnSendAppConfig);
            Controls.Add(rtbLocalHostsBT);
            Controls.Add(btnCountAC);
            Controls.Add(radioBoxGroup1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "EditAllAppConfigs";
            Text = "EditAllAppConfigs";
            Controls.SetChildIndex(radioBoxGroup1, 0);
            Controls.SetChildIndex(btnCountAC, 0);
            Controls.SetChildIndex(rtbLocalHostsBT, 0);
            Controls.SetChildIndex(pbUSE, 0);
            Controls.SetChildIndex(btnSendAppConfig, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioBoxGroup radioBoxGroup1;
        private RichTextBox rtbLocalHostsBT;
        private Button btnCountAC;
        private Button btnSendAppConfig;
    }
}