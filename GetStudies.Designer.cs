namespace CreditStatistics
{
    partial class GetStudies
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
            components = new System.ComponentModel.Container();
            btnStart = new Button();
            tbInfo = new TextBox();
            label1 = new Label();
            lbSelProj = new ListBox();
            btnSave = new Button();
            btnView = new Button();
            btnEdit = new Button();
            tcStudy = new TabControl();
            tabView = new TabPage();
            tabEdit = new TabPage();
            label2 = new Label();
            btnApply = new Button();
            dgvStudyInfo = new DataGridView();
            timerDoSelected = new System.Windows.Forms.Timer(components);
            ID = new DataGridViewTextBoxColumn();
            DaysDuration = new DataGridViewTextBoxColumn();
            NameStudy = new DataGridViewTextBoxColumn();
            CPUs = new DataGridViewTextBoxColumn();
            GPUs = new DataGridViewTextBoxColumn();
            MaxApps = new DataGridViewTextBoxColumn();
            OrigID = new DataGridViewTextBoxColumn();
            tcStudy.SuspendLayout();
            tabView.SuspendLayout();
            tabEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStudyInfo).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(27, 459);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(99, 42);
            btnStart.TabIndex = 0;
            btnStart.Text = "Obtain Study\r\ninformation";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // tbInfo
            // 
            tbInfo.Location = new Point(22, 10);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.ScrollBars = ScrollBars.Vertical;
            tbInfo.Size = new Size(438, 412);
            tbInfo.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Location = new Point(209, 63);
            label1.Name = "label1";
            label1.Size = new Size(122, 30);
            label1.TabIndex = 4;
            label1.Text = "Project Name: double\r\nclick to view page";
            // 
            // lbSelProj
            // 
            lbSelProj.FormattingEnabled = true;
            lbSelProj.ItemHeight = 15;
            lbSelProj.Location = new Point(209, 107);
            lbSelProj.Name = "lbSelProj";
            lbSelProj.Size = new Size(127, 394);
            lbSelProj.TabIndex = 5;
            lbSelProj.SelectedIndexChanged += lbSelProj_SelectedIndexChanged;
            lbSelProj.MouseDoubleClick += lbSelProj_MouseDoubleClick;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(27, 213);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(99, 23);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save Changes";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnView
            // 
            btnView.Location = new Point(27, 107);
            btnView.Name = "btnView";
            btnView.Size = new Size(99, 23);
            btnView.TabIndex = 7;
            btnView.Text = "View All";
            btnView.UseVisualStyleBackColor = true;
            btnView.Click += btnView_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(27, 171);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(99, 23);
            btnEdit.TabIndex = 8;
            btnEdit.Text = "Edit Selected";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // tcStudy
            // 
            tcStudy.Controls.Add(tabView);
            tcStudy.Controls.Add(tabEdit);
            tcStudy.Location = new Point(397, 35);
            tcStudy.Name = "tcStudy";
            tcStudy.SelectedIndex = 0;
            tcStudy.Size = new Size(655, 466);
            tcStudy.TabIndex = 9;
            // 
            // tabView
            // 
            tabView.Controls.Add(tbInfo);
            tabView.Location = new Point(4, 24);
            tabView.Name = "tabView";
            tabView.Padding = new Padding(3);
            tabView.Size = new Size(647, 438);
            tabView.TabIndex = 0;
            tabView.Text = "View";
            tabView.UseVisualStyleBackColor = true;
            // 
            // tabEdit
            // 
            tabEdit.Controls.Add(label2);
            tabEdit.Controls.Add(btnApply);
            tabEdit.Controls.Add(dgvStudyInfo);
            tabEdit.Location = new Point(4, 24);
            tabEdit.Name = "tabEdit";
            tabEdit.Padding = new Padding(3);
            tabEdit.Size = new Size(647, 438);
            tabEdit.TabIndex = 1;
            tabEdit.Text = "Editor";
            tabEdit.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.Info;
            label2.Location = new Point(23, 200);
            label2.Name = "label2";
            label2.Size = new Size(124, 135);
            label2.TabIndex = 3;
            label2.Text = "Days is how long you\r\nare given to process\r\nthe data.  To delete a\r\nrow, set the ID to -1\r\n\r\nYou may add a row.\r\nYou must click both\r\nsave Edits and the\r\nSave Changes buttons";
            // 
            // btnApply
            // 
            btnApply.Location = new Point(38, 108);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(75, 23);
            btnApply.TabIndex = 2;
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
            dgvStudyInfo.Columns.AddRange(new DataGridViewColumn[] { ID, DaysDuration, NameStudy, CPUs, GPUs, MaxApps, OrigID });
            dgvStudyInfo.Location = new Point(178, 86);
            dgvStudyInfo.Name = "dgvStudyInfo";
            dgvStudyInfo.RowHeadersVisible = false;
            dgvStudyInfo.Size = new Size(441, 328);
            dgvStudyInfo.TabIndex = 0;
            dgvStudyInfo.RowsAdded += dgvStudyInfo_RowsAdded;
            // 
            // timerDoSelected
            // 
            timerDoSelected.Interval = 1000;
            timerDoSelected.Tick += timerDoSelected_Tick;
            // 
            // ID
            // 
            ID.FillWeight = 40F;
            ID.HeaderText = "ID";
            ID.Name = "ID";
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
            // MaxApps
            // 
            MaxApps.HeaderText = "MaxApps";
            MaxApps.Name = "MaxApps";
            MaxApps.Width = 72;
            // 
            // OrigID
            // 
            OrigID.HeaderText = "OrigID";
            OrigID.Name = "OrigID";
            OrigID.Visible = false;
            // 
            // GetStudies
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1086, 550);
            Controls.Add(tcStudy);
            Controls.Add(btnEdit);
            Controls.Add(btnView);
            Controls.Add(btnSave);
            Controls.Add(lbSelProj);
            Controls.Add(label1);
            Controls.Add(btnStart);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "GetStudies";
            Text = "GetStudies";
            tcStudy.ResumeLayout(false);
            tabView.ResumeLayout(false);
            tabView.PerformLayout();
            tabEdit.ResumeLayout(false);
            tabEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStudyInfo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private TextBox tbInfo;
        private Label label1;
        private ListBox lbSelProj;
        private Button btnSave;
        private Button btnView;
        private Button btnEdit;
        private TabControl tcStudy;
        private TabPage tabView;
        private TabPage tabEdit;
        private DataGridView dgvStudyInfo;
        private Button btnApply;
        private Label label2;
        private System.Windows.Forms.Timer timerDoSelected;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn DaysDuration;
        private DataGridViewTextBoxColumn NameStudy;
        private DataGridViewTextBoxColumn CPUs;
        private DataGridViewTextBoxColumn GPUs;
        private DataGridViewTextBoxColumn MaxApps;
        private DataGridViewTextBoxColumn OrigID;
    }
}