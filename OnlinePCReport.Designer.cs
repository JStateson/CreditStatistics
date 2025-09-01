namespace CreditStatistics
{
    partial class OnlinePCReport
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
            btnSSHconnect = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // btnSSHconnect
            // 
            btnSSHconnect.ForeColor = SystemColors.Highlight;
            btnSSHconnect.Location = new Point(295, 208);
            btnSSHconnect.Name = "btnSSHconnect";
            btnSSHconnect.Size = new Size(103, 66);
            btnSSHconnect.TabIndex = 15;
            btnSSHconnect.Text = "Click here to\r\nconnect to\r\nPC using SSH";
            btnSSHconnect.UseVisualStyleBackColor = true;
            btnSSHconnect.Click += btnSSHconnect_Click;
            // 
            // btnExit
            // 
            btnExit.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.ForeColor = SystemColors.Highlight;
            btnExit.Location = new Point(305, 410);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(75, 31);
            btnExit.TabIndex = 16;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // OnlinePCReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(461, 598);
            Controls.Add(btnExit);
            Controls.Add(btnSSHconnect);
            Name = "OnlinePCReport";
            Text = "OnlinePCReport";
            Controls.SetChildIndex(btnSSHconnect, 0);
            Controls.SetChildIndex(btnExit, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSSHconnect;
        private Button btnExit;
    }
}