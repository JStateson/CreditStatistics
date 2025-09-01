namespace CreditStatistics
{
    partial class CtrlRemotePCs
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
            groupBox1 = new GroupBox();
            lbSelectedPCs = new Label();
            btnRemovePC = new Button();
            btnBoincRestart = new Button();
            btnPCreset = new Button();
            btnPCoff = new Button();
            gbSamURL = new GroupBox();
            bltnSelectSprint = new Button();
            btnUpdate = new Button();
            btnResume = new Button();
            btnSuspProj = new Button();
            btnANW = new Button();
            btnNNW = new Button();
            toolTip1 = new ToolTip(components);
            lbCurPC = new Label();
            pbProj = new ProgressBar();
            groupBox1.SuspendLayout();
            gbSamURL.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lbSelectedPCs);
            groupBox1.Controls.Add(btnRemovePC);
            groupBox1.Controls.Add(btnBoincRestart);
            groupBox1.Controls.Add(btnPCreset);
            groupBox1.Controls.Add(btnPCoff);
            groupBox1.Location = new Point(336, 22);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(250, 564);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "Remote System Operations";
            // 
            // lbSelectedPCs
            // 
            lbSelectedPCs.AutoSize = true;
            lbSelectedPCs.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbSelectedPCs.ForeColor = SystemColors.HotTrack;
            lbSelectedPCs.Location = new Point(45, 35);
            lbSelectedPCs.Name = "lbSelectedPCs";
            lbSelectedPCs.Size = new Size(143, 21);
            lbSelectedPCs.TabIndex = 45;
            lbSelectedPCs.Text = "Systems selected:";
            // 
            // btnRemovePC
            // 
            btnRemovePC.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRemovePC.ForeColor = Color.Blue;
            btnRemovePC.Location = new Point(32, 488);
            btnRemovePC.Margin = new Padding(4, 3, 4, 3);
            btnRemovePC.Name = "btnRemovePC";
            btnRemovePC.Size = new Size(167, 53);
            btnRemovePC.TabIndex = 43;
            btnRemovePC.Text = "Click here to remove\r\npandora_config file";
            btnRemovePC.UseVisualStyleBackColor = true;
            btnRemovePC.Click += btnRemovePC_Click;
            // 
            // btnBoincRestart
            // 
            btnBoincRestart.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBoincRestart.ForeColor = Color.Blue;
            btnBoincRestart.Location = new Point(32, 305);
            btnBoincRestart.Margin = new Padding(4, 3, 4, 3);
            btnBoincRestart.Name = "btnBoincRestart";
            btnBoincRestart.Size = new Size(144, 53);
            btnBoincRestart.TabIndex = 42;
            btnBoincRestart.Text = "Click here to\r\nrestart Boinc";
            btnBoincRestart.UseVisualStyleBackColor = true;
            btnBoincRestart.Click += btnBoincRestart_Click;
            // 
            // btnPCreset
            // 
            btnPCreset.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPCreset.ForeColor = Color.Blue;
            btnPCreset.Location = new Point(32, 221);
            btnPCreset.Margin = new Padding(4, 3, 4, 3);
            btnPCreset.Name = "btnPCreset";
            btnPCreset.Size = new Size(144, 53);
            btnPCreset.TabIndex = 41;
            btnPCreset.Text = "Click here to\r\nrestart OS";
            btnPCreset.UseVisualStyleBackColor = true;
            btnPCreset.Click += btnPCreset_Click;
            // 
            // btnPCoff
            // 
            btnPCoff.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPCoff.ForeColor = Color.Blue;
            btnPCoff.Location = new Point(32, 126);
            btnPCoff.Margin = new Padding(4, 3, 4, 3);
            btnPCoff.Name = "btnPCoff";
            btnPCoff.Size = new Size(144, 53);
            btnPCoff.TabIndex = 40;
            btnPCoff.Text = "Click here to\r\nturn PCs off";
            btnPCoff.UseVisualStyleBackColor = true;
            btnPCoff.Click += btnPCoff_Click;
            // 
            // gbSamURL
            // 
            gbSamURL.Controls.Add(bltnSelectSprint);
            gbSamURL.Controls.Add(btnUpdate);
            gbSamURL.Controls.Add(btnResume);
            gbSamURL.Controls.Add(btnSuspProj);
            gbSamURL.Controls.Add(btnANW);
            gbSamURL.Controls.Add(btnNNW);
            gbSamURL.Location = new Point(627, 65);
            gbSamURL.Name = "gbSamURL";
            gbSamURL.Size = new Size(504, 521);
            gbSamURL.TabIndex = 21;
            gbSamURL.TabStop = false;
            gbSamURL.Text = "Project Controls";
            // 
            // bltnSelectSprint
            // 
            bltnSelectSprint.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bltnSelectSprint.ForeColor = Color.Blue;
            bltnSelectSprint.Location = new Point(318, 420);
            bltnSelectSprint.Margin = new Padding(4, 3, 4, 3);
            bltnSelectSprint.Name = "bltnSelectSprint";
            bltnSelectSprint.Size = new Size(167, 69);
            bltnSelectSprint.TabIndex = 46;
            bltnSelectSprint.Text = "Select all the sprint\r\nprojects except the\r\none selected here";
            toolTip1.SetToolTip(bltnSelectSprint, "Select the sprint project\r\nchosen by boinggames\r\nand suspend all others");
            bltnSelectSprint.UseVisualStyleBackColor = true;
            bltnSelectSprint.Click += bltnSelectSprint_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpdate.ForeColor = Color.Blue;
            btnUpdate.Location = new Point(360, 321);
            btnUpdate.Margin = new Padding(4, 3, 4, 3);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(125, 47);
            btnUpdate.TabIndex = 45;
            btnUpdate.Text = "Update\r\nProject";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnResume
            // 
            btnResume.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResume.ForeColor = Color.Blue;
            btnResume.Location = new Point(361, 227);
            btnResume.Margin = new Padding(4, 3, 4, 3);
            btnResume.Name = "btnResume";
            btnResume.Size = new Size(125, 47);
            btnResume.TabIndex = 44;
            btnResume.Text = "Resume\r\nProject";
            btnResume.UseVisualStyleBackColor = true;
            btnResume.Click += btnResume_Click;
            // 
            // btnSuspProj
            // 
            btnSuspProj.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSuspProj.ForeColor = Color.Blue;
            btnSuspProj.Location = new Point(360, 174);
            btnSuspProj.Margin = new Padding(4, 3, 4, 3);
            btnSuspProj.Name = "btnSuspProj";
            btnSuspProj.Size = new Size(125, 47);
            btnSuspProj.TabIndex = 43;
            btnSuspProj.Text = "Suspend\r\nProject";
            btnSuspProj.UseVisualStyleBackColor = true;
            btnSuspProj.Click += btnSuspProj_Click;
            // 
            // btnANW
            // 
            btnANW.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnANW.ForeColor = Color.Blue;
            btnANW.Location = new Point(360, 85);
            btnANW.Margin = new Padding(4, 3, 4, 3);
            btnANW.Name = "btnANW";
            btnANW.Size = new Size(125, 47);
            btnANW.TabIndex = 42;
            btnANW.Text = "Allow\r\nnew work";
            btnANW.UseVisualStyleBackColor = true;
            btnANW.Click += btnANW_Click;
            // 
            // btnNNW
            // 
            btnNNW.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNNW.ForeColor = Color.Blue;
            btnNNW.Location = new Point(361, 32);
            btnNNW.Margin = new Padding(4, 3, 4, 3);
            btnNNW.Name = "btnNNW";
            btnNNW.Size = new Size(125, 47);
            btnNNW.TabIndex = 41;
            btnNNW.Text = "Issue no\r\nnew work";
            btnNNW.UseVisualStyleBackColor = true;
            btnNNW.Click += btnNNW_Click;
            // 
            // lbCurPC
            // 
            lbCurPC.AutoSize = true;
            lbCurPC.Location = new Point(627, 30);
            lbCurPC.Name = "lbCurPC";
            lbCurPC.Size = new Size(38, 15);
            lbCurPC.TabIndex = 22;
            lbCurPC.Text = "label2";
            // 
            // pbProj
            // 
            pbProj.Location = new Point(773, 22);
            pbProj.Name = "pbProj";
            pbProj.Size = new Size(358, 23);
            pbProj.TabIndex = 23;
            // 
            // CtrlRemotePCs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1155, 598);
            Controls.Add(pbProj);
            Controls.Add(lbCurPC);
            Controls.Add(gbSamURL);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "CtrlRemotePCs";
            Text = "CtrlRemotePCs";
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(gbSamURL, 0);
            Controls.SetChildIndex(lbCurPC, 0);
            Controls.SetChildIndex(pbProj, 0);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            gbSamURL.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnBoincRestart;
        private Button btnPCreset;
        private Button btnPCoff;
        private Button btnRemovePC;
        private Label lbSelectedPCs;
        private GroupBox gbSamURL;
        private Button btnNNW;
        private Button btnResume;
        private Button btnSuspProj;
        private Button btnANW;
        private Button btnUpdate;
        private Button bltnSelectSprint;
        private ToolTip toolTip1;
        private Label lbCurPC;
        private ProgressBar pbProj;
    }
}