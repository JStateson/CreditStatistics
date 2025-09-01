namespace CreditStatistics
{
    partial class BigMessageBox
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
            tbBig = new TextBox();
            cbReadOnly = new CheckBox();
            btnOK = new Button();
            btnCancel = new Button();
            lbPCs = new ListBox();
            SuspendLayout();
            // 
            // tbBig
            // 
            tbBig.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbBig.Location = new Point(17, 17);
            tbBig.Margin = new Padding(4);
            tbBig.Multiline = true;
            tbBig.Name = "tbBig";
            tbBig.ReadOnly = true;
            tbBig.ScrollBars = ScrollBars.Vertical;
            tbBig.Size = new Size(724, 577);
            tbBig.TabIndex = 0;
            tbBig.WordWrap = false;
            // 
            // cbReadOnly
            // 
            cbReadOnly.AutoSize = true;
            cbReadOnly.Checked = true;
            cbReadOnly.CheckState = CheckState.Checked;
            cbReadOnly.Location = new Point(33, 634);
            cbReadOnly.Margin = new Padding(4);
            cbReadOnly.Name = "cbReadOnly";
            cbReadOnly.Size = new Size(107, 25);
            cbReadOnly.TabIndex = 1;
            cbReadOnly.Text = "Read Only";
            cbReadOnly.UseVisualStyleBackColor = true;
            cbReadOnly.CheckStateChanged += cbReadOnly_CheckStateChanged;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(847, 90);
            btnOK.Margin = new Padding(4);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(107, 32);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(847, 165);
            btnCancel.Margin = new Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(107, 32);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "CANCEL";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lbPCs
            // 
            lbPCs.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbPCs.ForeColor = SystemColors.MenuHighlight;
            lbPCs.FormattingEnabled = true;
            lbPCs.ItemHeight = 21;
            lbPCs.Location = new Point(847, 255);
            lbPCs.Name = "lbPCs";
            lbPCs.Size = new Size(177, 340);
            lbPCs.TabIndex = 13;
            lbPCs.SelectedIndexChanged += lbPCs_SelectedIndexChanged;
            // 
            // BigMessageBox
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 692);
            Controls.Add(lbPCs);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(cbReadOnly);
            Controls.Add(tbBig);
            Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.MenuHighlight;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4);
            Name = "BigMessageBox";
            Text = "BigMessageBox";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbBig;
        private CheckBox cbReadOnly;
        private Button btnOK;
        private Button btnCancel;
        private ListBox lbPCs;
    }
}