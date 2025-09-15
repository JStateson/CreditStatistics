namespace CreditStatistics
{
    partial class SprintEdit
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
            tbEdit = new TextBox();
            btnSave = new Button();
            gbList = new GroupBox();
            btnApplyAbove = new Button();
            btnCancel = new Button();
            lbPCs = new ListBox();
            btnApplyAll = new Button();
            gbList.SuspendLayout();
            SuspendLayout();
            // 
            // tbEdit
            // 
            tbEdit.Location = new Point(12, 30);
            tbEdit.Multiline = true;
            tbEdit.Name = "tbEdit";
            tbEdit.ReadOnly = true;
            tbEdit.ScrollBars = ScrollBars.Both;
            tbEdit.Size = new Size(434, 541);
            tbEdit.TabIndex = 0;
            tbEdit.Text = "rat";
            tbEdit.WordWrap = false;
            // 
            // btnSave
            // 
            btnSave.ForeColor = SystemColors.Highlight;
            btnSave.Location = new Point(815, 30);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(123, 61);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save your changes\r\nand exit this form.";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // gbList
            // 
            gbList.Controls.Add(btnApplyAbove);
            gbList.Location = new Point(500, 30);
            gbList.Name = "gbList";
            gbList.Size = new Size(279, 541);
            gbList.TabIndex = 6;
            gbList.TabStop = false;
            gbList.Text = "LIST";
            // 
            // btnApplyAbove
            // 
            btnApplyAbove.ForeColor = SystemColors.Highlight;
            btnApplyAbove.Location = new Point(140, 478);
            btnApplyAbove.Name = "btnApplyAbove";
            btnApplyAbove.Size = new Size(123, 40);
            btnApplyAbove.TabIndex = 15;
            btnApplyAbove.Text = "Apply above\r\nto selected PC";
            btnApplyAbove.UseVisualStyleBackColor = true;
            btnApplyAbove.Click += btnApplyAbove_Click;
            // 
            // btnCancel
            // 
            btnCancel.ForeColor = SystemColors.Highlight;
            btnCancel.Location = new Point(815, 109);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(123, 40);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel Changes";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lbPCs
            // 
            lbPCs.FormattingEnabled = true;
            lbPCs.ItemHeight = 15;
            lbPCs.Location = new Point(815, 282);
            lbPCs.Name = "lbPCs";
            lbPCs.Size = new Size(177, 289);
            lbPCs.TabIndex = 13;
            lbPCs.SelectedIndexChanged += lbPCs_SelectedIndexChanged;
            // 
            // btnApplyAll
            // 
            btnApplyAll.ForeColor = SystemColors.Highlight;
            btnApplyAll.Location = new Point(869, 202);
            btnApplyAll.Name = "btnApplyAll";
            btnApplyAll.Size = new Size(123, 40);
            btnApplyAll.TabIndex = 14;
            btnApplyAll.Text = "Apply default\r\nto all PCs";
            btnApplyAll.UseVisualStyleBackColor = true;
            btnApplyAll.Click += btnApplyAll_Click;
            // 
            // SprintEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1013, 583);
            Controls.Add(btnApplyAll);
            Controls.Add(lbPCs);
            Controls.Add(btnCancel);
            Controls.Add(gbList);
            Controls.Add(tbEdit);
            Controls.Add(btnSave);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "SprintEdit";
            Text = "SprintEdit";
            gbList.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbEdit;
        private Button btnSave;
        private GroupBox gbList;
        private Button btnCancel;
        private ListBox lbPCs;
        private Button btnApplyAll;
        private Button btnApplyAbove;
    }
}