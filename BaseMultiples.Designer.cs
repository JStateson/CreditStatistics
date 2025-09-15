namespace CreditStatistics
{
    partial class BaseMultiples
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
            gbProj = new GroupBox();
            btnSelSprint = new Button();
            btnInvPJ = new Button();
            btnClearPJ = new Button();
            btnCheckPG = new Button();
            gbPCs = new GroupBox();
            gbProj.SuspendLayout();
            SuspendLayout();
            // 
            // gbProj
            // 
            gbProj.Controls.Add(btnSelSprint);
            gbProj.Controls.Add(btnInvPJ);
            gbProj.Controls.Add(btnClearPJ);
            gbProj.Controls.Add(btnCheckPG);
            gbProj.Location = new Point(12, 12);
            gbProj.Name = "gbProj";
            gbProj.Size = new Size(476, 606);
            gbProj.TabIndex = 0;
            gbProj.TabStop = false;
            gbProj.Text = "Known sprints in blue, checked projects will be used in the sprint";
            // 
            // btnSelSprint
            // 
            btnSelSprint.Location = new Point(351, 545);
            btnSelSprint.Name = "btnSelSprint";
            btnSelSprint.Size = new Size(91, 44);
            btnSelSprint.TabIndex = 3;
            btnSelSprint.Text = "Check all the\r\nknown Sprints";
            btnSelSprint.UseVisualStyleBackColor = true;
            btnSelSprint.Click += btnSelSprint_Click;
            // 
            // btnInvPJ
            // 
            btnInvPJ.Location = new Point(263, 564);
            btnInvPJ.Name = "btnInvPJ";
            btnInvPJ.Size = new Size(75, 23);
            btnInvPJ.TabIndex = 2;
            btnInvPJ.Text = "Invert";
            btnInvPJ.UseVisualStyleBackColor = true;
            btnInvPJ.Visible = false;
            btnInvPJ.Click += ChangeCKbox;
            // 
            // btnClearPJ
            // 
            btnClearPJ.Location = new Point(263, 535);
            btnClearPJ.Name = "btnClearPJ";
            btnClearPJ.Size = new Size(75, 23);
            btnClearPJ.TabIndex = 1;
            btnClearPJ.Text = "Clear all";
            btnClearPJ.UseVisualStyleBackColor = true;
            btnClearPJ.Visible = false;
            btnClearPJ.Click += ChangeCKbox;
            // 
            // btnCheckPG
            // 
            btnCheckPG.Location = new Point(263, 506);
            btnCheckPG.Name = "btnCheckPG";
            btnCheckPG.Size = new Size(75, 23);
            btnCheckPG.TabIndex = 0;
            btnCheckPG.Text = "Check all";
            btnCheckPG.UseVisualStyleBackColor = true;
            btnCheckPG.Visible = false;
            btnCheckPG.Click += ChangeCKbox;
            // 
            // gbPCs
            // 
            gbPCs.Location = new Point(528, 12);
            gbPCs.Name = "gbPCs";
            gbPCs.Size = new Size(220, 606);
            gbPCs.TabIndex = 1;
            gbPCs.TabStop = false;
            gbPCs.Text = "Systems to use";
            // 
            // BaseMultiples
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1092, 630);
            Controls.Add(gbPCs);
            Controls.Add(gbProj);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "BaseMultiples";
            Text = "MultipleRuns";
            gbProj.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button btnCheckPG;
        private Button btnInvPJ;
        private Button btnClearPJ;
        private Button btnSelSprint;
        protected GroupBox gbProj;
        protected GroupBox gbPCs;
    }
}