namespace CreditStatistics
{
    partial class BasePandoraPCs
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
            gbPCs = new GroupBox();
            lbLastScanned = new Label();
            label2 = new Label();
            btnReScan = new Button();
            btnInvertPC = new Button();
            btnCheckPC = new Button();
            btnClearPC = new Button();
            pbUSE = new ProgressBar();
            label1 = new Label();
            btnCancel = new Button();
            gbPCs.SuspendLayout();
            SuspendLayout();
            // 
            // gbPCs
            // 
            gbPCs.Controls.Add(lbLastScanned);
            gbPCs.Controls.Add(label2);
            gbPCs.Controls.Add(btnReScan);
            gbPCs.Controls.Add(btnInvertPC);
            gbPCs.Controls.Add(btnCheckPC);
            gbPCs.Controls.Add(btnClearPC);
            gbPCs.Location = new Point(12, 115);
            gbPCs.Name = "gbPCs";
            gbPCs.Size = new Size(245, 473);
            gbPCs.TabIndex = 2;
            gbPCs.TabStop = false;
            gbPCs.Text = "Systems to use";
            // 
            // lbLastScanned
            // 
            lbLastScanned.AutoSize = true;
            lbLastScanned.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbLastScanned.ForeColor = SystemColors.Highlight;
            lbLastScanned.Location = new Point(10, 438);
            lbLastScanned.Name = "lbLastScanned";
            lbLastScanned.Size = new Size(75, 21);
            lbLastScanned.TabIndex = 19;
            lbLastScanned.Text = "Last Scan";
            lbLastScanned.TextAlign = ContentAlignment.TopCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.Highlight;
            label2.Location = new Point(10, 403);
            label2.Name = "label2";
            label2.Size = new Size(62, 17);
            label2.TabIndex = 18;
            label2.Text = "Last Scan";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnReScan
            // 
            btnReScan.Location = new Point(126, 389);
            btnReScan.Name = "btnReScan";
            btnReScan.Size = new Size(98, 46);
            btnReScan.TabIndex = 6;
            btnReScan.Text = "Check if PCs \r\nare back online";
            btnReScan.UseVisualStyleBackColor = true;
            btnReScan.Click += btnReScan_Click;
            // 
            // btnInvertPC
            // 
            btnInvertPC.Location = new Point(139, 132);
            btnInvertPC.Name = "btnInvertPC";
            btnInvertPC.Size = new Size(85, 23);
            btnInvertPC.TabIndex = 5;
            btnInvertPC.Text = "Invert";
            btnInvertPC.UseVisualStyleBackColor = true;
            btnInvertPC.Click += ChangeCKbox;
            // 
            // btnCheckPC
            // 
            btnCheckPC.Location = new Point(139, 51);
            btnCheckPC.Name = "btnCheckPC";
            btnCheckPC.Size = new Size(85, 23);
            btnCheckPC.TabIndex = 3;
            btnCheckPC.Text = "Check all";
            btnCheckPC.UseVisualStyleBackColor = true;
            btnCheckPC.Click += ChangeCKbox;
            // 
            // btnClearPC
            // 
            btnClearPC.Location = new Point(139, 93);
            btnClearPC.Name = "btnClearPC";
            btnClearPC.Size = new Size(85, 23);
            btnClearPC.TabIndex = 4;
            btnClearPC.Text = "Clear all";
            btnClearPC.UseVisualStyleBackColor = true;
            btnClearPC.Click += ChangeCKbox;
            // 
            // pbUSE
            // 
            pbUSE.Location = new Point(12, 22);
            pbUSE.Name = "pbUSE";
            pbUSE.Size = new Size(245, 23);
            pbUSE.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 60);
            label1.Name = "label1";
            label1.Size = new Size(136, 39);
            label1.TabIndex = 4;
            label1.Text = "Color Codes:  Black or\r\nBlue online.  Red for SSH\r\nworking but not Boinc";
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.Red;
            btnCancel.Location = new Point(183, 60);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(74, 39);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel\r\noperation\r\n";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Visible = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // BasePandoraPCs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1385, 600);
            Controls.Add(btnCancel);
            Controls.Add(label1);
            Controls.Add(pbUSE);
            Controls.Add(gbPCs);
            MaximizeBox = false;
            Name = "BasePandoraPCs";
            Text = "ma";
            gbPCs.ResumeLayout(false);
            gbPCs.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnInvertPC;
        private Button btnClearPC;
        private Button btnCheckPC;
        private Button btnReScan;
        public ProgressBar pbUSE;
        private Label label1;
        private Label lbLastScanned;
        private Label label2;
        private Button btnCancel;
        protected GroupBox gbPCs;
    }
}