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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OnlinePCReport));
            btnSSHconnect = new Button();
            btnExit = new Button();
            label3 = new Label();
            SuspendLayout();
            // 
            // btnSSHconnect
            // 
            btnSSHconnect.ForeColor = SystemColors.Highlight;
            btnSSHconnect.Location = new Point(333, 268);
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
            btnExit.Location = new Point(343, 416);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(75, 31);
            btnExit.TabIndex = 16;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.Info;
            label3.Location = new Point(289, 59);
            label3.Name = "label3";
            label3.Size = new Size(208, 165);
            label3.TabIndex = 17;
            label3.Text = resources.GetString("label3.Text");
            // 
            // OnlinePCReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(524, 598);
            Controls.Add(label3);
            Controls.Add(btnExit);
            Controls.Add(btnSSHconnect);
            Name = "OnlinePCReport";
            Text = "OnlinePCReport";
            Controls.SetChildIndex(btnSSHconnect, 0);
            Controls.SetChildIndex(btnExit, 0);
            Controls.SetChildIndex(label3, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSSHconnect;
        private Button btnExit;
        private Label label3;
    }
}