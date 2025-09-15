namespace CreditStatistics
{
    partial class AssignCpuGpu
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
            tbInfo = new TextBox();
            label1 = new Label();
            btnSetBGsprint = new Button();
            btnAssignStudy = new Button();
            btnSaveChanges = new Button();
            SuspendLayout();
            // 
            // tbInfo
            // 
            tbInfo.Location = new Point(800, 547);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.Size = new Size(263, 71);
            tbInfo.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Location = new Point(899, 459);
            label1.Name = "label1";
            label1.Size = new Size(185, 60);
            label1.TabIndex = 11;
            label1.Text = "If all PCs use the same projects,\r\nthen select (check) those projects\r\nand also your PCs and then click\r\nthe above 'Assign default' button.";
            // 
            // btnSetBGsprint
            // 
            btnSetBGsprint.ForeColor = SystemColors.Highlight;
            btnSetBGsprint.Location = new Point(773, 377);
            btnSetBGsprint.Name = "btnSetBGsprint";
            btnSetBGsprint.Size = new Size(123, 61);
            btnSetBGsprint.TabIndex = 8;
            btnSetBGsprint.Text = "Assign default\r\nSprint settings";
            btnSetBGsprint.UseVisualStyleBackColor = true;
            btnSetBGsprint.Click += btnSetBGsprint_Click;
            // 
            // btnAssignStudy
            // 
            btnAssignStudy.ForeColor = SystemColors.Highlight;
            btnAssignStudy.Location = new Point(940, 348);
            btnAssignStudy.Name = "btnAssignStudy";
            btnAssignStudy.Size = new Size(123, 61);
            btnAssignStudy.TabIndex = 10;
            btnAssignStudy.Text = "Assign Studies for\r\nBoincGames sprint";
            btnAssignStudy.UseVisualStyleBackColor = true;
            btnAssignStudy.Click += btnAssignStudy_Click;
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.ForeColor = SystemColors.Highlight;
            btnSaveChanges.Location = new Point(856, 76);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(123, 61);
            btnSaveChanges.TabIndex = 14;
            btnSaveChanges.Text = "Save Changes";
            btnSaveChanges.UseVisualStyleBackColor = true;
            btnSaveChanges.Click += btnSaveChanges_Click;
            // 
            // MultipleRuns
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1096, 650);
            Controls.Add(btnSaveChanges);
            Controls.Add(tbInfo);
            Controls.Add(label1);
            Controls.Add(btnSetBGsprint);
            Controls.Add(btnAssignStudy);
            Name = "MultipleRuns";
            Controls.SetChildIndex(gbProj, 0);
            Controls.SetChildIndex(gbPCs, 0);
            Controls.SetChildIndex(btnAssignStudy, 0);
            Controls.SetChildIndex(btnSetBGsprint, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(tbInfo, 0);
            Controls.SetChildIndex(btnSaveChanges, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbInfo;
        private Label label1;
        private Button btnSetBGsprint;
        private Button btnAssignStudy;
        private Button btnSaveChanges;
    }
}