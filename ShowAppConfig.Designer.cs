namespace CreditStatistics
{
    partial class ShowAppConfig
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
            btnSendApp = new Button();
            tbInfo = new TextBox();
            gbPJs = new GroupBox();
            gbPCs = new GroupBox();
            btnFetch = new Button();
            groupBox1 = new GroupBox();
            tbSum = new TextBox();
            btnSaveVars = new Button();
            tb_maxapps = new TextBox();
            tb_cpu = new TextBox();
            tb_gpu = new TextBox();
            lb_cpu = new Label();
            lb_tasks = new Label();
            lb_gpu = new Label();
            tcAppConfig = new TabControl();
            tabAppConfig = new TabPage();
            tabResults = new TabPage();
            tbResInfo = new TextBox();
            groupBox1.SuspendLayout();
            tcAppConfig.SuspendLayout();
            tabAppConfig.SuspendLayout();
            tabResults.SuspendLayout();
            SuspendLayout();
            // 
            // btnSendApp
            // 
            btnSendApp.ForeColor = SystemColors.Highlight;
            btnSendApp.Location = new Point(509, 96);
            btnSendApp.Name = "btnSendApp";
            btnSendApp.Size = new Size(111, 42);
            btnSendApp.TabIndex = 15;
            btnSendApp.Text = "Send text To PC\r\nand save changes";
            btnSendApp.UseVisualStyleBackColor = true;
            btnSendApp.Click += btnSendApp_Click;
            // 
            // tbInfo
            // 
            tbInfo.Location = new Point(6, 6);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.ScrollBars = ScrollBars.Vertical;
            tbInfo.Size = new Size(610, 571);
            tbInfo.TabIndex = 14;
            // 
            // gbPJs
            // 
            gbPJs.Location = new Point(210, 21);
            gbPJs.Name = "gbPJs";
            gbPJs.Size = new Size(250, 350);
            gbPJs.TabIndex = 13;
            gbPJs.TabStop = false;
            gbPJs.Text = "Sprint Projects";
            // 
            // gbPCs
            // 
            gbPCs.Location = new Point(17, 21);
            gbPCs.Name = "gbPCs";
            gbPCs.Size = new Size(139, 350);
            gbPCs.TabIndex = 12;
            gbPCs.TabStop = false;
            gbPCs.Text = "Sprint PCs";
            // 
            // btnFetch
            // 
            btnFetch.ForeColor = SystemColors.Highlight;
            btnFetch.Location = new Point(509, 32);
            btnFetch.Name = "btnFetch";
            btnFetch.Size = new Size(111, 42);
            btnFetch.TabIndex = 16;
            btnFetch.Text = "Get app_config\r\nfrom PC and Proj";
            btnFetch.UseVisualStyleBackColor = true;
            btnFetch.Click += btnFetch_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tbSum);
            groupBox1.Controls.Add(btnSaveVars);
            groupBox1.Controls.Add(tb_maxapps);
            groupBox1.Controls.Add(tb_cpu);
            groupBox1.Controls.Add(tb_gpu);
            groupBox1.Controls.Add(lb_cpu);
            groupBox1.Controls.Add(lb_tasks);
            groupBox1.Controls.Add(lb_gpu);
            groupBox1.Location = new Point(17, 393);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(464, 229);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "CpuUsage";
            // 
            // tbSum
            // 
            tbSum.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbSum.Location = new Point(205, 22);
            tbSum.Multiline = true;
            tbSum.Name = "tbSum";
            tbSum.Size = new Size(238, 189);
            tbSum.TabIndex = 17;
            // 
            // btnSaveVars
            // 
            btnSaveVars.ForeColor = SystemColors.Highlight;
            btnSaveVars.Location = new Point(26, 169);
            btnSaveVars.Name = "btnSaveVars";
            btnSaveVars.Size = new Size(95, 42);
            btnSaveVars.TabIndex = 16;
            btnSaveVars.Text = "Save Changes";
            btnSaveVars.UseVisualStyleBackColor = true;
            btnSaveVars.Click += btnSaveVars_Click;
            // 
            // tb_maxapps
            // 
            tb_maxapps.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tb_maxapps.ForeColor = SystemColors.HotTrack;
            tb_maxapps.Location = new Point(119, 112);
            tb_maxapps.Name = "tb_maxapps";
            tb_maxapps.Size = new Size(51, 23);
            tb_maxapps.TabIndex = 5;
            // 
            // tb_cpu
            // 
            tb_cpu.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tb_cpu.ForeColor = SystemColors.HotTrack;
            tb_cpu.Location = new Point(119, 70);
            tb_cpu.Name = "tb_cpu";
            tb_cpu.Size = new Size(51, 23);
            tb_cpu.TabIndex = 4;
            // 
            // tb_gpu
            // 
            tb_gpu.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tb_gpu.ForeColor = SystemColors.HotTrack;
            tb_gpu.Location = new Point(119, 29);
            tb_gpu.Name = "tb_gpu";
            tb_gpu.Size = new Size(51, 23);
            tb_gpu.TabIndex = 3;
            // 
            // lb_cpu
            // 
            lb_cpu.AutoSize = true;
            lb_cpu.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lb_cpu.ForeColor = SystemColors.HotTrack;
            lb_cpu.Location = new Point(26, 71);
            lb_cpu.Name = "lb_cpu";
            lb_cpu.Size = new Size(68, 17);
            lb_cpu.TabIndex = 2;
            lb_cpu.Text = "CpuUsage";
            // 
            // lb_tasks
            // 
            lb_tasks.AutoSize = true;
            lb_tasks.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lb_tasks.ForeColor = SystemColors.HotTrack;
            lb_tasks.Location = new Point(26, 113);
            lb_tasks.Name = "lb_tasks";
            lb_tasks.Size = new Size(67, 17);
            lb_tasks.TabIndex = 1;
            lb_tasks.Text = "Max Apps";
            // 
            // lb_gpu
            // 
            lb_gpu.AutoSize = true;
            lb_gpu.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lb_gpu.ForeColor = SystemColors.HotTrack;
            lb_gpu.Location = new Point(26, 30);
            lb_gpu.Name = "lb_gpu";
            lb_gpu.Size = new Size(69, 17);
            lb_gpu.TabIndex = 0;
            lb_gpu.Text = "GpuUsage";
            // 
            // tcAppConfig
            // 
            tcAppConfig.Controls.Add(tabAppConfig);
            tcAppConfig.Controls.Add(tabResults);
            tcAppConfig.Location = new Point(649, 11);
            tcAppConfig.Name = "tcAppConfig";
            tcAppConfig.SelectedIndex = 0;
            tcAppConfig.Size = new Size(630, 611);
            tcAppConfig.TabIndex = 18;
            // 
            // tabAppConfig
            // 
            tabAppConfig.Controls.Add(tbInfo);
            tabAppConfig.Location = new Point(4, 24);
            tabAppConfig.Name = "tabAppConfig";
            tabAppConfig.Padding = new Padding(3);
            tabAppConfig.Size = new Size(622, 583);
            tabAppConfig.TabIndex = 0;
            tabAppConfig.Text = "App Config";
            tabAppConfig.UseVisualStyleBackColor = true;
            // 
            // tabResults
            // 
            tabResults.Controls.Add(tbResInfo);
            tabResults.Location = new Point(4, 24);
            tabResults.Name = "tabResults";
            tabResults.Padding = new Padding(3);
            tabResults.Size = new Size(622, 583);
            tabResults.TabIndex = 1;
            tabResults.Text = "Results and Info";
            tabResults.UseVisualStyleBackColor = true;
            // 
            // tbResInfo
            // 
            tbResInfo.Location = new Point(6, 212);
            tbResInfo.Multiline = true;
            tbResInfo.Name = "tbResInfo";
            tbResInfo.ScrollBars = ScrollBars.Vertical;
            tbResInfo.Size = new Size(610, 340);
            tbResInfo.TabIndex = 15;
            // 
            // ShowAppConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1291, 634);
            Controls.Add(tcAppConfig);
            Controls.Add(groupBox1);
            Controls.Add(btnFetch);
            Controls.Add(btnSendApp);
            Controls.Add(gbPJs);
            Controls.Add(gbPCs);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ShowAppConfig";
            Text = "ShowAppConfig";
            FormClosed += ShowAppConfig_FormClosed;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tcAppConfig.ResumeLayout(false);
            tabAppConfig.ResumeLayout(false);
            tabAppConfig.PerformLayout();
            tabResults.ResumeLayout(false);
            tabResults.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button btnSendApp;
        private TextBox tbInfo;
        private GroupBox gbPJs;
        private GroupBox gbPCs;
        private Button btnFetch;
        private GroupBox groupBox1;
        private Label lb_cpu;
        private Label lb_tasks;
        private Label lb_gpu;
        private TextBox tb_gpu;
        private Button btnSaveVars;
        private TextBox tb_maxapps;
        private TextBox tb_cpu;
        private TextBox tbSum;
        private TabControl tcAppConfig;
        private TabPage tabAppConfig;
        private TabPage tabResults;
        private TextBox tbResInfo;
    }
}