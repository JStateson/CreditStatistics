namespace CreditStatistics
{
    partial class RemoteSystems
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteSystems));
            lbTestPC = new Label();
            label2 = new Label();
            btnReadBoinc = new Button();
            groupBox1 = new GroupBox();
            tbWhereBoinc = new TextBox();
            label1 = new Label();
            label15 = new Label();
            BoincTaskFolder = new TextBox();
            label14 = new Label();
            WorkingFolderLoc = new TextBox();
            rtbLocalHostsBT = new RichTextBox();
            pbTask = new ProgressBar();
            SchTimer = new System.Windows.Forms.Timer(components);
            btnListPCs = new Button();
            tabRemote = new TabControl();
            tabPage1 = new TabPage();
            btnSave = new Button();
            label3 = new Label();
            dgv = new DataGridView();
            pcname = new DataGridViewTextBoxColumn();
            username = new DataGridViewTextBoxColumn();
            password = new DataGridViewTextBoxColumn();
            online = new DataGridViewCheckBoxColumn();
            nCPUs = new DataGridViewTextBoxColumn();
            nGPUs = new DataGridViewTextBoxColumn();
            version = new DataGridViewTextBoxColumn();
            tabPage2 = new TabPage();
            btnIDNotepad = new Button();
            btnFetchAC = new Button();
            groupBox1.SuspendLayout();
            tabRemote.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // lbTestPC
            // 
            lbTestPC.AutoSize = true;
            lbTestPC.ForeColor = Color.Red;
            lbTestPC.Location = new Point(907, 72);
            lbTestPC.Margin = new Padding(4, 0, 4, 0);
            lbTestPC.Name = "lbTestPC";
            lbTestPC.Size = new Size(59, 15);
            lbTestPC.TabIndex = 37;
            lbTestPC.Text = "testing pc";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.Info;
            label2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(22, 85);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(479, 96);
            label2.TabIndex = 36;
            label2.Text = resources.GetString("label2.Text");
            // 
            // btnReadBoinc
            // 
            btnReadBoinc.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnReadBoinc.ForeColor = Color.Blue;
            btnReadBoinc.Location = new Point(22, 12);
            btnReadBoinc.Margin = new Padding(4, 3, 4, 3);
            btnReadBoinc.Name = "btnReadBoinc";
            btnReadBoinc.Size = new Size(116, 65);
            btnReadBoinc.TabIndex = 33;
            btnReadBoinc.Text = "Click here to\r\nfetch remote\r\nPC names";
            btnReadBoinc.UseVisualStyleBackColor = true;
            btnReadBoinc.Click += btnReadBoinc_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tbWhereBoinc);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label15);
            groupBox1.Controls.Add(BoincTaskFolder);
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(WorkingFolderLoc);
            groupBox1.Location = new Point(21, 35);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(534, 377);
            groupBox1.TabIndex = 35;
            groupBox1.TabStop = false;
            groupBox1.Text = "Folder Locations";
            // 
            // tbWhereBoinc
            // 
            tbWhereBoinc.Location = new Point(28, 258);
            tbWhereBoinc.Margin = new Padding(4, 3, 4, 3);
            tbWhereBoinc.Name = "tbWhereBoinc";
            tbWhereBoinc.ReadOnly = true;
            tbWhereBoinc.Size = new Size(450, 23);
            tbWhereBoinc.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(35, 231);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(126, 14);
            label1.TabIndex = 16;
            label1.Text = "Boinc data folder";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.BackColor = SystemColors.Info;
            label15.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label15.Location = new Point(35, 126);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(357, 14);
            label15.TabIndex = 14;
            label15.Text = "Boinctask folder: AppData\\Roaming\\eFMer\\BoincTasks\r\n";
            // 
            // BoincTaskFolder
            // 
            BoincTaskFolder.Location = new Point(28, 153);
            BoincTaskFolder.Margin = new Padding(4, 3, 4, 3);
            BoincTaskFolder.Name = "BoincTaskFolder";
            BoincTaskFolder.ReadOnly = true;
            BoincTaskFolder.Size = new Size(450, 23);
            BoincTaskFolder.TabIndex = 13;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.BackColor = SystemColors.Info;
            label14.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label14.Location = new Point(35, 36);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(105, 14);
            label14.TabIndex = 12;
            label14.Text = "Working folder";
            // 
            // WorkingFolderLoc
            // 
            WorkingFolderLoc.Location = new Point(28, 63);
            WorkingFolderLoc.Margin = new Padding(4, 3, 4, 3);
            WorkingFolderLoc.Name = "WorkingFolderLoc";
            WorkingFolderLoc.ReadOnly = true;
            WorkingFolderLoc.Size = new Size(450, 23);
            WorkingFolderLoc.TabIndex = 0;
            // 
            // rtbLocalHostsBT
            // 
            rtbLocalHostsBT.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbLocalHostsBT.Location = new Point(22, 199);
            rtbLocalHostsBT.Margin = new Padding(4, 3, 4, 3);
            rtbLocalHostsBT.Name = "rtbLocalHostsBT";
            rtbLocalHostsBT.Size = new Size(511, 377);
            rtbLocalHostsBT.TabIndex = 34;
            rtbLocalHostsBT.Text = "";
            rtbLocalHostsBT.WordWrap = false;
            // 
            // pbTask
            // 
            pbTask.Location = new Point(636, 27);
            pbTask.Name = "pbTask";
            pbTask.Size = new Size(395, 23);
            pbTask.TabIndex = 38;
            // 
            // btnListPCs
            // 
            btnListPCs.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnListPCs.ForeColor = Color.Blue;
            btnListPCs.Location = new Point(389, 12);
            btnListPCs.Margin = new Padding(4, 3, 4, 3);
            btnListPCs.Name = "btnListPCs";
            btnListPCs.Size = new Size(112, 65);
            btnListPCs.TabIndex = 39;
            btnListPCs.Text = "Click here to\r\nview list of\r\nProject IDs";
            btnListPCs.UseVisualStyleBackColor = true;
            btnListPCs.Click += btnListPCs_Click;
            // 
            // tabRemote
            // 
            tabRemote.Controls.Add(tabPage1);
            tabRemote.Controls.Add(tabPage2);
            tabRemote.Location = new Point(593, 84);
            tabRemote.Name = "tabRemote";
            tabRemote.SelectedIndex = 0;
            tabRemote.Size = new Size(593, 492);
            tabRemote.TabIndex = 40;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnSave);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(dgv);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(585, 464);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "UsernamePassword";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.Blue;
            btnSave.Location = new Point(372, 8);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(71, 39);
            btnSave.TabIndex = 40;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.Info;
            label3.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(21, 8);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(280, 32);
            label3.TabIndex = 37;
            label3.Text = "Username and password are for SSH to\r\nyour systems, not any Boinc credentials";
            // 
            // dgv
            // 
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new DataGridViewColumn[] { pcname, username, password, online, nCPUs, nGPUs, version });
            dgv.Location = new Point(21, 53);
            dgv.Name = "dgv";
            dgv.RowHeadersVisible = false;
            dgv.Size = new Size(546, 392);
            dgv.TabIndex = 0;
            // 
            // pcname
            // 
            pcname.HeaderText = "System";
            pcname.Name = "pcname";
            pcname.ReadOnly = true;
            // 
            // username
            // 
            username.HeaderText = "Username";
            username.Name = "username";
            // 
            // password
            // 
            password.HeaderText = "Password";
            password.Name = "password";
            password.Width = 64;
            // 
            // online
            // 
            online.HeaderText = "OnLine";
            online.Name = "online";
            online.ReadOnly = true;
            online.Resizable = DataGridViewTriState.True;
            online.SortMode = DataGridViewColumnSortMode.Automatic;
            online.Width = 48;
            // 
            // nCPUs
            // 
            nCPUs.HeaderText = "nCPUs";
            nCPUs.Name = "nCPUs";
            nCPUs.Width = 48;
            // 
            // nGPUs
            // 
            nGPUs.HeaderText = "nGPUs";
            nGPUs.Name = "nGPUs";
            nGPUs.Width = 48;
            // 
            // version
            // 
            version.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            version.HeaderText = "version";
            version.Name = "version";
            version.ReadOnly = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(585, 464);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Folder Locations";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnIDNotepad
            // 
            btnIDNotepad.Location = new Point(509, 116);
            btnIDNotepad.Name = "btnIDNotepad";
            btnIDNotepad.Size = new Size(70, 44);
            btnIDNotepad.TabIndex = 41;
            btnIDNotepad.Text = "Copy To\r\nnotepad";
            btnIDNotepad.UseVisualStyleBackColor = true;
            btnIDNotepad.Click += btnIDNotepad_Click;
            // 
            // btnFetchAC
            // 
            btnFetchAC.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnFetchAC.ForeColor = Color.Blue;
            btnFetchAC.Location = new Point(205, 12);
            btnFetchAC.Margin = new Padding(4, 3, 4, 3);
            btnFetchAC.Name = "btnFetchAC";
            btnFetchAC.Size = new Size(120, 65);
            btnFetchAC.TabIndex = 42;
            btnFetchAC.Text = "Click here for\r\napp configs\r\nused in sprint";
            btnFetchAC.UseVisualStyleBackColor = true;
            btnFetchAC.Click += btnFetchAC_Click;
            // 
            // RemoteSystems
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1209, 594);
            Controls.Add(btnFetchAC);
            Controls.Add(btnIDNotepad);
            Controls.Add(btnReadBoinc);
            Controls.Add(tabRemote);
            Controls.Add(btnListPCs);
            Controls.Add(pbTask);
            Controls.Add(lbTestPC);
            Controls.Add(label2);
            Controls.Add(rtbLocalHostsBT);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            Name = "RemoteSystems";
            Text = "RemoteSystems";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabRemote.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTestPC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReadBoinc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbWhereBoinc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox BoincTaskFolder;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox WorkingFolderLoc;
        private System.Windows.Forms.RichTextBox rtbLocalHostsBT;
        private ProgressBar pbTask;
        private System.Windows.Forms.Timer SchTimer;
        private Button btnListPCs;
        private TabControl tabRemote;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView dgv;
        private Button btnSave;
        private Label label3;
        private DataGridViewTextBoxColumn pcname;
        private DataGridViewTextBoxColumn username;
        private DataGridViewTextBoxColumn password;
        private DataGridViewCheckBoxColumn online;
        private DataGridViewTextBoxColumn nCPUs;
        private DataGridViewTextBoxColumn nGPUs;
        private DataGridViewTextBoxColumn version;
        private Button btnIDNotepad;
        private Button btnFetchAC;
    }
}