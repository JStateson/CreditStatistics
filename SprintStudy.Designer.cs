namespace CreditStatistics
{
    partial class SprintStudy
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
            btnCancel = new Button();
            gbList = new GroupBox();
            btnSave = new Button();
            gpEdit = new GroupBox();
            groupBox3 = new GroupBox();
            tbProjName = new TextBox();
            groupBox2 = new GroupBox();
            tbSelectDays = new TextBox();
            groupBox1 = new GroupBox();
            tbSelStudy = new TextBox();
            label2 = new Label();
            btnApply = new Button();
            dgvStudyInfo = new DataGridView();
            lbPCs = new ListBox();
            btnApplyAll = new Button();
            ID = new DataGridViewTextBoxColumn();
            DaysDuration = new DataGridViewTextBoxColumn();
            NameStudy = new DataGridViewTextBoxColumn();
            CPUs = new DataGridViewTextBoxColumn();
            GPUs = new DataGridViewTextBoxColumn();
            gpEdit.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStudyInfo).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.ForeColor = SystemColors.Highlight;
            btnCancel.Location = new Point(1066, 127);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(123, 42);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel Changes";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // gbList
            // 
            gbList.Location = new Point(713, 52);
            gbList.Name = "gbList";
            gbList.Size = new Size(297, 507);
            gbList.TabIndex = 9;
            gbList.TabStop = false;
            gbList.Text = "LIST";
            // 
            // btnSave
            // 
            btnSave.ForeColor = SystemColors.Highlight;
            btnSave.Location = new Point(1066, 52);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(123, 61);
            btnSave.TabIndex = 8;
            btnSave.Text = "Save your changes\r\nand exit this form.";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // gpEdit
            // 
            gpEdit.Controls.Add(groupBox3);
            gpEdit.Controls.Add(groupBox2);
            gpEdit.Controls.Add(groupBox1);
            gpEdit.Controls.Add(label2);
            gpEdit.Controls.Add(btnApply);
            gpEdit.Controls.Add(dgvStudyInfo);
            gpEdit.Location = new Point(12, 42);
            gpEdit.Name = "gpEdit";
            gpEdit.Size = new Size(668, 503);
            gpEdit.TabIndex = 11;
            gpEdit.TabStop = false;
            gpEdit.Text = "Update or enter missing values";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tbProjName);
            groupBox3.Location = new Point(469, 405);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(145, 79);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "Selected Project";
            // 
            // tbProjName
            // 
            tbProjName.Location = new Point(11, 32);
            tbProjName.Name = "tbProjName";
            tbProjName.ReadOnly = true;
            tbProjName.Size = new Size(128, 23);
            tbProjName.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tbSelectDays);
            groupBox2.Location = new Point(480, 231);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(145, 79);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "Days Allowed";
            // 
            // tbSelectDays
            // 
            tbSelectDays.Location = new Point(46, 31);
            tbSelectDays.Name = "tbSelectDays";
            tbSelectDays.ReadOnly = true;
            tbSelectDays.Size = new Size(65, 23);
            tbSelectDays.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tbSelStudy);
            groupBox1.Location = new Point(480, 86);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(145, 79);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Sprint Study";
            // 
            // tbSelStudy
            // 
            tbSelStudy.Location = new Point(46, 31);
            tbSelStudy.Name = "tbSelStudy";
            tbSelStudy.ReadOnly = true;
            tbSelStudy.Size = new Size(65, 23);
            tbSelStudy.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.Info;
            label2.Location = new Point(93, 27);
            label2.Name = "label2";
            label2.Size = new Size(239, 45);
            label2.TabIndex = 6;
            label2.Text = "Days is how long you are given to process\r\nthe data.  You cannot edit the ID nor are you\r\nallowed to delete a row";
            // 
            // btnApply
            // 
            btnApply.Location = new Point(6, 23);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(75, 23);
            btnApply.TabIndex = 5;
            btnApply.Text = "Save Edits";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // dgvStudyInfo
            // 
            dgvStudyInfo.AllowUserToDeleteRows = false;
            dgvStudyInfo.AllowUserToResizeColumns = false;
            dgvStudyInfo.AllowUserToResizeRows = false;
            dgvStudyInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudyInfo.Columns.AddRange(new DataGridViewColumn[] { ID, DaysDuration, NameStudy, CPUs, GPUs });
            dgvStudyInfo.Location = new Point(6, 86);
            dgvStudyInfo.MultiSelect = false;
            dgvStudyInfo.Name = "dgvStudyInfo";
            dgvStudyInfo.RowHeadersVisible = false;
            dgvStudyInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudyInfo.Size = new Size(434, 398);
            dgvStudyInfo.TabIndex = 4;
            dgvStudyInfo.CellDoubleClick += dgvStudyInfo_CellDoubleClick;
            dgvStudyInfo.RowEnter += dgvStudyInfo_RowEnter;
            // 
            // lbPCs
            // 
            lbPCs.FormattingEnabled = true;
            lbPCs.ItemHeight = 15;
            lbPCs.Location = new Point(1066, 279);
            lbPCs.Name = "lbPCs";
            lbPCs.Size = new Size(177, 289);
            lbPCs.TabIndex = 12;
            lbPCs.SelectedIndexChanged += lbPCs_SelectedIndexChanged;
            // 
            // btnApplyAll
            // 
            btnApplyAll.ForeColor = SystemColors.Highlight;
            btnApplyAll.Location = new Point(1120, 215);
            btnApplyAll.Name = "btnApplyAll";
            btnApplyAll.Size = new Size(123, 40);
            btnApplyAll.TabIndex = 15;
            btnApplyAll.Text = "Apply default to\r\nall PCs and exit";
            btnApplyAll.UseVisualStyleBackColor = true;
            btnApplyAll.Click += btnApplyAll_Click;
            // 
            // ID
            // 
            ID.FillWeight = 40F;
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.SortMode = DataGridViewColumnSortMode.NotSortable;
            ID.Width = 40;
            // 
            // DaysDuration
            // 
            DaysDuration.FillWeight = 40F;
            DaysDuration.HeaderText = "Days";
            DaysDuration.Name = "DaysDuration";
            DaysDuration.Width = 40;
            // 
            // NameStudy
            // 
            NameStudy.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            NameStudy.HeaderText = "Name";
            NameStudy.Name = "NameStudy";
            NameStudy.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CPUs
            // 
            CPUs.HeaderText = "CPUs";
            CPUs.Name = "CPUs";
            CPUs.Width = 72;
            // 
            // GPUs
            // 
            GPUs.HeaderText = "GPUs";
            GPUs.Name = "GPUs";
            GPUs.Width = 72;
            // 
            // SprintStudy
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1263, 596);
            Controls.Add(btnApplyAll);
            Controls.Add(lbPCs);
            Controls.Add(gpEdit);
            Controls.Add(btnCancel);
            Controls.Add(gbList);
            Controls.Add(btnSave);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "SprintStudy";
            Text = "SprintStudy";
            gpEdit.ResumeLayout(false);
            gpEdit.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStudyInfo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnCancel;
        private GroupBox gbList;
        private Button btnSave;
        private GroupBox gpEdit;
        private Label label2;
        private Button btnApply;
        private DataGridView dgvStudyInfo;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TextBox tbSelectDays;
        private TextBox tbSelStudy;
        private GroupBox groupBox3;
        private TextBox tbProjName;
        private ListBox lbPCs;
        private Button btnApplyAll;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn DaysDuration;
        private DataGridViewTextBoxColumn NameStudy;
        private DataGridViewTextBoxColumn CPUs;
        private DataGridViewTextBoxColumn GPUs;
    }
}