namespace CreditStatistics
{
    partial class SetupSampleData
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
            btnApply = new Button();
            label1 = new Label();
            cbNumWanted = new ComboBox();
            btnSave = new Button();
            SuspendLayout();
            // 
            // gbPCs
            // 
            gbPCs.Size = new Size(200, 606);
            // 
            // btnApply
            // 
            btnApply.ForeColor = SystemColors.HotTrack;
            btnApply.Location = new Point(774, 110);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(102, 64);
            btnApply.TabIndex = 11;
            btnApply.Text = "Apply to\r\nPC selected";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(774, 47);
            label1.Name = "label1";
            label1.Size = new Size(78, 15);
            label1.TabIndex = 10;
            label1.Text = "WUs wanted";
            // 
            // cbNumWanted
            // 
            cbNumWanted.FormattingEnabled = true;
            cbNumWanted.Location = new Point(878, 47);
            cbNumWanted.Name = "cbNumWanted";
            cbNumWanted.Size = new Size(78, 23);
            cbNumWanted.TabIndex = 9;
            // 
            // btnSave
            // 
            btnSave.ForeColor = SystemColors.HotTrack;
            btnSave.Location = new Point(774, 233);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(102, 61);
            btnSave.TabIndex = 12;
            btnSave.Text = "Save Changes";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // SetupSampleData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 646);
            Controls.Add(btnSave);
            Controls.Add(label1);
            Controls.Add(btnApply);
            Controls.Add(cbNumWanted);
            Name = "SetupSampleData";
            Text = "SetupSampleData";
            Controls.SetChildIndex(cbNumWanted, 0);
            Controls.SetChildIndex(gbProj, 0);
            Controls.SetChildIndex(gbPCs, 0);
            Controls.SetChildIndex(btnApply, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(btnSave, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private ComboBox cbNumWanted;
        private Button btnSendAll;
        private Button btnSend1;
        private Button btnApply;
        private Button btnSave;
    }
}