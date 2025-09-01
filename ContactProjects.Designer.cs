namespace CreditStatistics
{
    partial class ContactProjects
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
            gbSamURL = new GroupBox();
            groupBox1 = new GroupBox();
            rbPCs = new RadioButton();
            rbPrefs = new RadioButton();
            groupBox2 = new GroupBox();
            btnAllPCs = new Button();
            btnAllPref = new Button();
            toolTip1 = new ToolTip(components);
            dgv = new DataGridView();
            project = new DataGridViewTextBoxColumn();
            user_id = new DataGridViewTextBoxColumn();
            preference = new DataGridViewTextBoxColumn();
            your_pcs = new DataGridViewTextBoxColumn();
            btnSave = new Button();
            label1 = new Label();
            btnRestore = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // gbSamURL
            // 
            gbSamURL.Location = new Point(12, 12);
            gbSamURL.Name = "gbSamURL";
            gbSamURL.Size = new Size(379, 494);
            gbSamURL.TabIndex = 0;
            gbSamURL.TabStop = false;
            gbSamURL.Text = "Click to contact";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rbPCs);
            groupBox1.Controls.Add(rbPrefs);
            groupBox1.Location = new Point(415, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(174, 90);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "What to view";
            // 
            // rbPCs
            // 
            rbPCs.AutoSize = true;
            rbPCs.Location = new Point(45, 61);
            rbPCs.Name = "rbPCs";
            rbPCs.Size = new Size(72, 19);
            rbPCs.TabIndex = 2;
            rbPCs.Text = "Your PCs";
            rbPCs.UseVisualStyleBackColor = true;
            // 
            // rbPrefs
            // 
            rbPrefs.AutoSize = true;
            rbPrefs.Checked = true;
            rbPrefs.Location = new Point(45, 22);
            rbPrefs.Name = "rbPrefs";
            rbPrefs.Size = new Size(86, 19);
            rbPrefs.TabIndex = 0;
            rbPrefs.TabStop = true;
            rbPrefs.Text = "Preferences";
            rbPrefs.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnAllPCs);
            groupBox2.Controls.Add(btnAllPref);
            groupBox2.Location = new Point(694, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(182, 94);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Contact ALL Sprint sites";
            // 
            // btnAllPCs
            // 
            btnAllPCs.Location = new Point(33, 61);
            btnAllPCs.Name = "btnAllPCs";
            btnAllPCs.Size = new Size(84, 23);
            btnAllPCs.TabIndex = 1;
            btnAllPCs.Text = "Your PCs";
            btnAllPCs.UseVisualStyleBackColor = true;
            btnAllPCs.Click += btnAllPCs_Click;
            // 
            // btnAllPref
            // 
            btnAllPref.Location = new Point(33, 22);
            btnAllPref.Name = "btnAllPref";
            btnAllPref.Size = new Size(84, 23);
            btnAllPref.TabIndex = 0;
            btnAllPref.Text = "Preferences";
            btnAllPref.UseVisualStyleBackColor = true;
            btnAllPref.Click += btnAllPref_Click;
            // 
            // dgv
            // 
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new DataGridViewColumn[] { project, user_id, preference, your_pcs });
            dgv.Location = new Point(415, 214);
            dgv.Name = "dgv";
            dgv.RowHeadersVisible = false;
            dgv.Size = new Size(619, 198);
            dgv.TabIndex = 3;
            dgv.RowsAdded += dgv_RowsAdded;
            // 
            // project
            // 
            project.HeaderText = "Project";
            project.Name = "project";
            project.Width = 90;
            // 
            // user_id
            // 
            user_id.HeaderText = "UserD";
            user_id.Name = "user_id";
            user_id.Width = 72;
            // 
            // preference
            // 
            preference.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            preference.HeaderText = "Preferences";
            preference.Name = "preference";
            // 
            // your_pcs
            // 
            your_pcs.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            your_pcs.HeaderText = "Your PCs";
            your_pcs.Name = "your_pcs";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(415, 442);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 42);
            btnSave.TabIndex = 4;
            btnSave.Text = "Click here\r\nto save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(515, 166);
            label1.Name = "label1";
            label1.Size = new Size(154, 21);
            label1.TabIndex = 5;
            label1.Text = "Project access table";
            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(561, 442);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(94, 42);
            btnRestore.TabIndex = 6;
            btnRestore.Text = "Restore\r\nDefaults";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // ContactProjects
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1046, 518);
            Controls.Add(btnRestore);
            Controls.Add(label1);
            Controls.Add(btnSave);
            Controls.Add(dgv);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(gbSamURL);
            MaximizeBox = false;
            Name = "ContactProjects";
            Text = "ContactProjects";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox gbSamURL;
        private GroupBox groupBox1;
        private RadioButton rbPCs;
        private RadioButton rbPrefs;
        private GroupBox groupBox2;
        private Button btnAllPCs;
        private Button btnAllPref;
        private ToolTip toolTip1;
        private DataGridView dgv;
        private Button btnSave;
        private Label label1;
        private Button btnRestore;
        private DataGridViewTextBoxColumn project;
        private DataGridViewTextBoxColumn user_id;
        private DataGridViewTextBoxColumn preference;
        private DataGridViewTextBoxColumn your_pcs;
    }
}