namespace CreditStatistics
{
    partial class ViewAppVersions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewAppVersions));
            dgvPJs = new DataGridView();
            ProjName = new DataGridViewTextBoxColumn();
            HasApp = new DataGridViewCheckBoxColumn();
            sCnt = new DataGridViewTextBoxColumn();
            AppVersion = new DataGridViewComboBoxColumn();
            label1 = new Label();
            btnFindAll = new Button();
            rtbLocalHostsBT = new RichTextBox();
            btnMakeDummy = new Button();
            btnBackup = new Button();
            btnRestore = new Button();
            groupBox1 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)dgvPJs).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvPJs
            // 
            dgvPJs.AllowUserToAddRows = false;
            dgvPJs.AllowUserToDeleteRows = false;
            dgvPJs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPJs.Columns.AddRange(new DataGridViewColumn[] { ProjName, HasApp, sCnt, AppVersion });
            dgvPJs.Location = new Point(20, 113);
            dgvPJs.Name = "dgvPJs";
            dgvPJs.RowHeadersVisible = false;
            dgvPJs.ScrollBars = ScrollBars.Vertical;
            dgvPJs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPJs.Size = new Size(465, 406);
            dgvPJs.TabIndex = 0;
            dgvPJs.CellEnter += dgvPJs_CellEnter;
            dgvPJs.CellValueChanged += dgvPJs_CellValueChanged;
            dgvPJs.CurrentCellDirtyStateChanged += dgvPJs_CurrentCellDirtyStateChanged;
            dgvPJs.SelectionChanged += dgvPJs_SelectionChanged;
            // 
            // ProjName
            // 
            ProjName.HeaderText = "Project Name";
            ProjName.Name = "ProjName";
            ProjName.Width = 120;
            // 
            // HasApp
            // 
            HasApp.HeaderText = "Has app";
            HasApp.Name = "HasApp";
            HasApp.Resizable = DataGridViewTriState.True;
            HasApp.SortMode = DataGridViewColumnSortMode.Automatic;
            HasApp.Width = 64;
            // 
            // sCnt
            // 
            sCnt.HeaderText = "Count";
            sCnt.Name = "sCnt";
            sCnt.Width = 64;
            // 
            // AppVersion
            // 
            AppVersion.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AppVersion.HeaderText = "AppVersion";
            AppVersion.Name = "AppVersion";
            AppVersion.Resizable = DataGridViewTriState.True;
            AppVersion.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Location = new Point(20, 51);
            label1.Name = "label1";
            label1.Size = new Size(375, 45);
            label1.TabIndex = 1;
            label1.Text = resources.GetString("label1.Text");
            // 
            // btnFindAll
            // 
            btnFindAll.Location = new Point(502, 293);
            btnFindAll.Name = "btnFindAll";
            btnFindAll.Size = new Size(127, 44);
            btnFindAll.TabIndex = 2;
            btnFindAll.Text = "3 - Find all\r\napp versions";
            btnFindAll.UseVisualStyleBackColor = true;
            btnFindAll.Click += btnFindAll_Click;
            // 
            // rtbLocalHostsBT
            // 
            rtbLocalHostsBT.Location = new Point(678, 24);
            rtbLocalHostsBT.Name = "rtbLocalHostsBT";
            rtbLocalHostsBT.Size = new Size(400, 528);
            rtbLocalHostsBT.TabIndex = 0;
            rtbLocalHostsBT.Text = "";
            // 
            // btnMakeDummy
            // 
            btnMakeDummy.Location = new Point(502, 192);
            btnMakeDummy.Name = "btnMakeDummy";
            btnMakeDummy.Size = new Size(127, 72);
            btnMakeDummy.TabIndex = 4;
            btnMakeDummy.Text = "2 - Create fake\r\napp_config files\r\nBoinc must restart";
            btnMakeDummy.UseVisualStyleBackColor = true;
            btnMakeDummy.Click += btnMakeDummy_Click;
            // 
            // btnBackup
            // 
            btnBackup.Location = new Point(502, 113);
            btnBackup.Name = "btnBackup";
            btnBackup.Size = new Size(127, 44);
            btnBackup.TabIndex = 5;
            btnBackup.Text = "1 - Backup \r\napp_configs";
            btnBackup.UseVisualStyleBackColor = true;
            btnBackup.Click += btnBackup_Click;
            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(502, 383);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(127, 44);
            btnRestore.TabIndex = 6;
            btnRestore.Text = "4 - Restore\r\nfrom backup";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnRestore);
            groupBox1.Controls.Add(dgvPJs);
            groupBox1.Controls.Add(btnBackup);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(btnMakeDummy);
            groupBox1.Controls.Add(btnFindAll);
            groupBox1.Location = new Point(12, 24);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(645, 537);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tools here only find app config versions";
            // 
            // ViewAppVersions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1102, 573);
            Controls.Add(rtbLocalHostsBT);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            Name = "ViewAppVersions";
            Text = "EditAppConfigs";
            ((System.ComponentModel.ISupportInitialize)dgvPJs).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvPJs;
        private Label label1;
        private Button btnFindAll;
        private RichTextBox rtbLocalHostsBT;
        private Button btnMakeDummy;
        private Button btnBackup;
        private Button btnRestore;
        private DataGridViewTextBoxColumn ProjName;
        private DataGridViewCheckBoxColumn HasApp;
        private DataGridViewTextBoxColumn sCnt;
        private DataGridViewComboBoxColumn AppVersion;
        private GroupBox groupBox1;
    }
}