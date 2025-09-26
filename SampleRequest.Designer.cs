namespace CreditStatistics
{
    partial class SampleRequest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleRequest));
            tabControl1 = new TabControl();
            tabPC = new TabPage();
            tbPCfile = new TextBox();
            tabError = new TabPage();
            tbInfo = new TextBox();
            btnRemoveAll = new Button();
            label3 = new Label();
            label5 = new Label();
            btnPCtoAll = new Button();
            label6 = new Label();
            btnSave = new Button();
            btnSendApp = new Button();
            btnAllowWork = new Button();
            btnNoWork = new Button();
            btnResAll = new Button();
            btnSuspAll = new Button();
            tabControl1.SuspendLayout();
            tabPC.SuspendLayout();
            tabError.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPC);
            tabControl1.Controls.Add(tabError);
            tabControl1.Location = new Point(627, 24);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(676, 564);
            tabControl1.TabIndex = 42;
            // 
            // tabPC
            // 
            tabPC.Controls.Add(tbPCfile);
            tabPC.Location = new Point(4, 24);
            tabPC.Name = "tabPC";
            tabPC.Padding = new Padding(3);
            tabPC.Size = new Size(668, 536);
            tabPC.TabIndex = 0;
            tabPC.Text = "pandor_config";
            tabPC.UseVisualStyleBackColor = true;
            // 
            // tbPCfile
            // 
            tbPCfile.Dock = DockStyle.Fill;
            tbPCfile.Location = new Point(3, 3);
            tbPCfile.Multiline = true;
            tbPCfile.Name = "tbPCfile";
            tbPCfile.ScrollBars = ScrollBars.Both;
            tbPCfile.Size = new Size(662, 530);
            tbPCfile.TabIndex = 21;
            // 
            // tabError
            // 
            tabError.Controls.Add(tbInfo);
            tabError.Location = new Point(4, 24);
            tabError.Name = "tabError";
            tabError.Padding = new Padding(3);
            tabError.Size = new Size(668, 536);
            tabError.TabIndex = 1;
            tabError.Text = "Error/Status";
            tabError.UseVisualStyleBackColor = true;
            // 
            // tbInfo
            // 
            tbInfo.Dock = DockStyle.Fill;
            tbInfo.Location = new Point(3, 3);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.Size = new Size(662, 530);
            tbInfo.TabIndex = 0;
            // 
            // btnRemoveAll
            // 
            btnRemoveAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRemoveAll.ForeColor = SystemColors.Highlight;
            btnRemoveAll.Location = new Point(427, 270);
            btnRemoveAll.Name = "btnRemoveAll";
            btnRemoveAll.Size = new Size(146, 42);
            btnRemoveAll.TabIndex = 41;
            btnRemoveAll.Text = "Remove pandora from\r\nall PCs and restart boinc";
            btnRemoveAll.UseVisualStyleBackColor = true;
            btnRemoveAll.Click += btnRemoveAll_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.Info;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(291, 208);
            label3.Name = "label3";
            label3.Size = new Size(282, 42);
            label3.TabIndex = 40;
            label3.Text = "The buttons below act on ENABLED\r\nPCs, to the left, not just the selected";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = SystemColors.Info;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(291, 448);
            label5.Name = "label5";
            label5.Size = new Size(294, 63);
            label5.TabIndex = 39;
            label5.Text = "The buttons below act only on the PC\r\nselected.  You should examine each\r\npandora config file to verify values";
            // 
            // btnPCtoAll
            // 
            btnPCtoAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPCtoAll.ForeColor = SystemColors.Highlight;
            btnPCtoAll.Location = new Point(291, 270);
            btnPCtoAll.Name = "btnPCtoAll";
            btnPCtoAll.Size = new Size(116, 42);
            btnPCtoAll.TabIndex = 38;
            btnPCtoAll.Text = "Send pandora\r\nto All PCs";
            btnPCtoAll.UseVisualStyleBackColor = true;
            btnPCtoAll.Click += btnPCtoAll_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = SystemColors.Info;
            label6.Location = new Point(291, 24);
            label6.Name = "label6";
            label6.Size = new Size(262, 165);
            label6.TabIndex = 37;
            label6.Text = resources.GetString("label6.Text");
            // 
            // btnSave
            // 
            btnSave.ForeColor = SystemColors.Highlight;
            btnSave.Location = new Point(508, 535);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 42);
            btnSave.TabIndex = 35;
            btnSave.Text = "Save Edits";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnSendApp
            // 
            btnSendApp.ForeColor = SystemColors.Highlight;
            btnSendApp.Location = new Point(291, 535);
            btnSendApp.Name = "btnSendApp";
            btnSendApp.Size = new Size(116, 42);
            btnSendApp.TabIndex = 36;
            btnSendApp.Text = "Send pandora\r\nto selected PC";
            btnSendApp.UseVisualStyleBackColor = true;
            btnSendApp.Click += btnSendApp_Click;
            // 
            // btnAllowWork
            // 
            btnAllowWork.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAllowWork.ForeColor = SystemColors.Highlight;
            btnAllowWork.Location = new Point(457, 379);
            btnAllowWork.Name = "btnAllowWork";
            btnAllowWork.Size = new Size(116, 42);
            btnAllowWork.TabIndex = 46;
            btnAllowWork.Text = "Allow new work\r\non sprint projects";
            btnAllowWork.UseVisualStyleBackColor = true;
            // 
            // btnNoWork
            // 
            btnNoWork.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNoWork.ForeColor = SystemColors.Highlight;
            btnNoWork.Location = new Point(291, 379);
            btnNoWork.Name = "btnNoWork";
            btnNoWork.Size = new Size(116, 42);
            btnNoWork.TabIndex = 45;
            btnNoWork.Text = "no new work\r\nto all sprints";
            btnNoWork.UseVisualStyleBackColor = true;
            // 
            // btnResAll
            // 
            btnResAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResAll.ForeColor = SystemColors.Highlight;
            btnResAll.Location = new Point(457, 322);
            btnResAll.Name = "btnResAll";
            btnResAll.Size = new Size(116, 42);
            btnResAll.TabIndex = 44;
            btnResAll.Text = "Resume all\r\nsprint projects";
            btnResAll.UseVisualStyleBackColor = true;
            // 
            // btnSuspAll
            // 
            btnSuspAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSuspAll.ForeColor = SystemColors.Highlight;
            btnSuspAll.Location = new Point(291, 322);
            btnSuspAll.Name = "btnSuspAll";
            btnSuspAll.Size = new Size(116, 42);
            btnSuspAll.TabIndex = 43;
            btnSuspAll.Text = "Suspend all\r\nsprint projects";
            btnSuspAll.UseVisualStyleBackColor = true;
            // 
            // SampleRequest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1359, 611);
            Controls.Add(btnAllowWork);
            Controls.Add(btnNoWork);
            Controls.Add(btnResAll);
            Controls.Add(btnSuspAll);
            Controls.Add(tabControl1);
            Controls.Add(btnRemoveAll);
            Controls.Add(label3);
            Controls.Add(label5);
            Controls.Add(btnPCtoAll);
            Controls.Add(label6);
            Controls.Add(btnSave);
            Controls.Add(btnSendApp);
            Name = "SampleRequest";
            Text = "SampleRequest";
            Controls.SetChildIndex(gbPCs, 0);
            Controls.SetChildIndex(pbUSE, 0);
            Controls.SetChildIndex(btnSendApp, 0);
            Controls.SetChildIndex(btnSave, 0);
            Controls.SetChildIndex(label6, 0);
            Controls.SetChildIndex(btnPCtoAll, 0);
            Controls.SetChildIndex(label5, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(btnRemoveAll, 0);
            Controls.SetChildIndex(tabControl1, 0);
            Controls.SetChildIndex(btnSuspAll, 0);
            Controls.SetChildIndex(btnResAll, 0);
            Controls.SetChildIndex(btnNoWork, 0);
            Controls.SetChildIndex(btnAllowWork, 0);
            tabControl1.ResumeLayout(false);
            tabPC.ResumeLayout(false);
            tabPC.PerformLayout();
            tabError.ResumeLayout(false);
            tabError.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPC;
        private TextBox tbPCfile;
        private TabPage tabError;
        private TextBox tbInfo;
        private Button btnRemoveAll;
        private Label label3;
        private Label label5;
        private Button btnPCtoAll;
        private Label label6;
        private Button btnSave;
        private Button btnSendApp;
        private Button btnAllowWork;
        private Button btnNoWork;
        private Button btnResAll;
        private Button btnSuspAll;
    }
}