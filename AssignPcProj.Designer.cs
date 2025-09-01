namespace CreditStatistics
{
    partial class AssignPcProj
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
            gbPJs = new GroupBox();
            btnSetDefault = new Button();
            btnSavePcProj = new Button();
            btnInvertPC = new Button();
            btnClearPC = new Button();
            btnCheckPC = new Button();
            gbPCs = new GroupBox();
            gbPJs.SuspendLayout();
            SuspendLayout();
            // 
            // gbPJs
            // 
            gbPJs.Controls.Add(btnSetDefault);
            gbPJs.Controls.Add(btnSavePcProj);
            gbPJs.Controls.Add(btnInvertPC);
            gbPJs.Controls.Add(btnClearPC);
            gbPJs.Controls.Add(btnCheckPC);
            gbPJs.Location = new Point(304, 34);
            gbPJs.Name = "gbPJs";
            gbPJs.Size = new Size(274, 409);
            gbPJs.TabIndex = 3;
            gbPJs.TabStop = false;
            gbPJs.Text = "Sprint Projects";
            // 
            // btnSetDefault
            // 
            btnSetDefault.Location = new Point(139, 281);
            btnSetDefault.Name = "btnSetDefault";
            btnSetDefault.Size = new Size(101, 45);
            btnSetDefault.TabIndex = 9;
            btnSetDefault.Text = "Use default\r\nsprint settings";
            btnSetDefault.UseVisualStyleBackColor = true;
            btnSetDefault.Click += btnSetDefault_Click;
            // 
            // btnSavePcProj
            // 
            btnSavePcProj.Location = new Point(165, 357);
            btnSavePcProj.Name = "btnSavePcProj";
            btnSavePcProj.Size = new Size(75, 23);
            btnSavePcProj.TabIndex = 6;
            btnSavePcProj.Text = "Save";
            btnSavePcProj.UseVisualStyleBackColor = true;
            btnSavePcProj.Click += btnSavePcProj_Click;
            // 
            // btnInvertPC
            // 
            btnInvertPC.Location = new Point(165, 132);
            btnInvertPC.Name = "btnInvertPC";
            btnInvertPC.Size = new Size(75, 23);
            btnInvertPC.TabIndex = 8;
            btnInvertPC.Text = "Invert";
            btnInvertPC.UseVisualStyleBackColor = true;
            btnInvertPC.Click += ChangeCKbox;
            // 
            // btnClearPC
            // 
            btnClearPC.Location = new Point(165, 78);
            btnClearPC.Name = "btnClearPC";
            btnClearPC.Size = new Size(75, 23);
            btnClearPC.TabIndex = 7;
            btnClearPC.Text = "Clear all";
            btnClearPC.UseVisualStyleBackColor = true;
            btnClearPC.Click += ChangeCKbox;
            // 
            // btnCheckPC
            // 
            btnCheckPC.Location = new Point(165, 31);
            btnCheckPC.Name = "btnCheckPC";
            btnCheckPC.Size = new Size(75, 23);
            btnCheckPC.TabIndex = 6;
            btnCheckPC.Text = "Check all";
            btnCheckPC.UseVisualStyleBackColor = true;
            btnCheckPC.Click += ChangeCKbox;
            // 
            // gbPCs
            // 
            gbPCs.Location = new Point(49, 34);
            gbPCs.Name = "gbPCs";
            gbPCs.Size = new Size(205, 409);
            gbPCs.TabIndex = 2;
            gbPCs.TabStop = false;
            gbPCs.Text = "Sprint PCs";
            // 
            // AssignPcProj
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(959, 499);
            Controls.Add(gbPJs);
            Controls.Add(gbPCs);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "AssignPcProj";
            Text = "AssignPcProj";
            gbPJs.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbPJs;
        private Button btnInvertPC;
        private Button btnClearPC;
        private Button btnCheckPC;
        private GroupBox gbPCs;
        private Button btnSavePcProj;
        private Button btnSetDefault;
    }
}