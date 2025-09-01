namespace CreditStatistics
{
    partial class ShowBunkeredWUs
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
            btnBacklog = new Button();
            btnRawTab = new Button();
            btnReadyToReport = new Button();
            btnPoints = new Button();
            btnFailedJobs = new Button();
            btnSuccess = new Button();
            label2 = new Label();
            dgv = new DataGridView();
            label1 = new Label();
            label3 = new Label();
            tbInfo = new TextBox();
            toolTip1 = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // btnBacklog
            // 
            btnBacklog.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBacklog.Location = new Point(879, 517);
            btnBacklog.Margin = new Padding(4, 3, 4, 3);
            btnBacklog.Name = "btnBacklog";
            btnBacklog.Size = new Size(154, 33);
            btnBacklog.TabIndex = 24;
            btnBacklog.Text = "Backlog of WUs";
            btnBacklog.UseVisualStyleBackColor = true;
            btnBacklog.Click += btnBacklog_Click;
            // 
            // btnRawTab
            // 
            btnRawTab.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRawTab.Location = new Point(1065, 466);
            btnRawTab.Margin = new Padding(4, 3, 4, 3);
            btnRawTab.Name = "btnRawTab";
            btnRawTab.Size = new Size(114, 33);
            btnRawTab.TabIndex = 23;
            btnRawTab.Text = "Raw Table";
            btnRawTab.UseVisualStyleBackColor = true;
            btnRawTab.Click += btnRawTab_Click;
            // 
            // btnReadyToReport
            // 
            btnReadyToReport.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReadyToReport.Location = new Point(879, 466);
            btnReadyToReport.Margin = new Padding(4, 3, 4, 3);
            btnReadyToReport.Name = "btnReadyToReport";
            btnReadyToReport.Size = new Size(154, 33);
            btnReadyToReport.TabIndex = 22;
            btnReadyToReport.Text = "Ready to report";
            btnReadyToReport.UseVisualStyleBackColor = true;
            btnReadyToReport.Click += btnReadyToReport_Click;
            // 
            // btnPoints
            // 
            btnPoints.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPoints.Location = new Point(1065, 517);
            btnPoints.Margin = new Padding(4, 3, 4, 3);
            btnPoints.Name = "btnPoints";
            btnPoints.Size = new Size(114, 33);
            btnPoints.TabIndex = 21;
            btnPoints.Text = "Points";
            btnPoints.UseVisualStyleBackColor = true;
            btnPoints.Click += btnPoints_Click;
            // 
            // btnFailedJobs
            // 
            btnFailedJobs.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFailedJobs.Location = new Point(735, 517);
            btnFailedJobs.Margin = new Padding(4, 3, 4, 3);
            btnFailedJobs.Name = "btnFailedJobs";
            btnFailedJobs.Size = new Size(114, 33);
            btnFailedJobs.TabIndex = 20;
            btnFailedJobs.Text = "Failed Jobs";
            btnFailedJobs.UseVisualStyleBackColor = true;
            btnFailedJobs.Click += btnFailedJobs_Click;
            // 
            // btnSuccess
            // 
            btnSuccess.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSuccess.Location = new Point(735, 466);
            btnSuccess.Margin = new Padding(4, 3, 4, 3);
            btnSuccess.Name = "btnSuccess";
            btnSuccess.Size = new Size(114, 33);
            btnSuccess.TabIndex = 19;
            btnSuccess.Text = "Good Jobs";
            btnSuccess.UseVisualStyleBackColor = true;
            btnSuccess.Click += btnSuccess_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.Info;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(346, 22);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(326, 20);
            label2.TabIndex = 18;
            label2.Text = "TotalJobs  ReadyToReport  JobsSucceeded ";
            // 
            // dgv
            // 
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Location = new Point(306, 51);
            dgv.Margin = new Padding(4, 3, 4, 3);
            dgv.Name = "dgv";
            dgv.ReadOnly = true;
            dgv.Size = new Size(898, 381);
            dgv.TabIndex = 17;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.White;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(848, 21);
            label1.Name = "label1";
            label1.Size = new Size(157, 21);
            label1.TabIndex = 25;
            label1.Text = "Suspended or Offline";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.White;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Blue;
            label3.Location = new Point(1081, 21);
            label3.Name = "label3";
            label3.Size = new Size(123, 21);
            label3.TabIndex = 26;
            label3.Text = "NO NEW WORK";
            // 
            // tbInfo
            // 
            tbInfo.Location = new Point(1271, 51);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.Size = new Size(171, 287);
            tbInfo.TabIndex = 27;
            // 
            // ShowBunkeredWUs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1475, 617);
            Controls.Add(tbInfo);
            Controls.Add(label3);
            Controls.Add(btnBacklog);
            Controls.Add(btnRawTab);
            Controls.Add(btnReadyToReport);
            Controls.Add(btnPoints);
            Controls.Add(btnFailedJobs);
            Controls.Add(btnSuccess);
            Controls.Add(label2);
            Controls.Add(dgv);
            Name = "ShowBunkeredWUs";
            Text = "ShowBunkeredWUs";
            Controls.SetChildIndex(dgv, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(btnSuccess, 0);
            Controls.SetChildIndex(btnFailedJobs, 0);
            Controls.SetChildIndex(btnPoints, 0);
            Controls.SetChildIndex(btnReadyToReport, 0);
            Controls.SetChildIndex(btnRawTab, 0);
            Controls.SetChildIndex(btnBacklog, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(tbInfo, 0);
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBacklog;
        private Button btnRawTab;
        private Button btnReadyToReport;
        private Button btnPoints;
        private Button btnSuccess;
        private Label label2;
        private DataGridView dgv;
        private Button btnFailedJobs;
        private Label label1;
        private Label label3;
        private TextBox tbInfo;
        private ToolTip toolTip1;
    }
}