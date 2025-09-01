using CreditStatistics;
using Microsoft.Playwright;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static CreditStatistics.globals.cAllProjectStudyInfo;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class ShowAppConfig : Form
    {

        private cPClimit TempletDB;
        private cProjectStruct ProjectStats;
        private List<cPClimit> PClimit;
        private PandoraConfig pc;
        private cPClimit SelectedPC = null;
        private PandoraRPC pandoraRPC = new PandoraRPC();
        private cCalcLimitProj ThisClp;

        private string current_PCname;
        private string current_Projectname;

        private bool OnStartup = true;

        public ShowAppConfig(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            TempletDB = ProjectStats.TempletDB;
            pc = new PandoraConfig(ref rProjectStats);
            pc.SaveSelected();
            FormProjectRB();
            //FormSystemsRB();
            //gbPCs.Visible = true;
            //gbPJs.Visible = true;
            pandoraRPC.Init(ref ProjectStats);
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Shown += InitialLoad;
        }

        private void InitialLoad(object sender, EventArgs e)
        {
            this.Enabled = false;
            FormSystemsRB();
            gbPCs.Visible = true;
            gbPJs.Visible = true;
            SetPC(current_PCname);
            GetResource();
            this.Enabled = true;
        }

        private void SetPC(string PCname)
        {
            current_PCname = PCname;
            SelectedPC = pc.GetPCbyName(PCname);
            foreach (cCalcLimitProj clp in SelectedPC.ProjList)
            {
                int iTag = EnablePJ(clp.ShortName, clp.UsedInSprint);
                SetLabel(iTag, clp.ShowUsedCpuGpu, clp.UsedInSprint);
            }
        }

        private void FormProjectRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;

            for (int i = 0; i < TempletDB.ProjList.Count; i++)
            {
                RadioButton rb = new RadioButton();
                Label lb = new Label();
                lb.Tag = i;
                rb.Tag = i;
                rb.Text = TempletDB.ProjList[i].ShortName;
                if (i == 0) current_Projectname = rb.Text;
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                lb.Location = new System.Drawing.Point(oCol + 120 + iCol * 120, oRow + iRow * 20);
                lb.Text = "Label: " + (i + 1).ToString();
                rb.Checked = (i == 0);
                rb.Enabled = true;
                if (i == 0)
                    current_Projectname = rb.Text;
                rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                gbPJs.Controls.Add(rb);
                gbPJs.Controls.Add(lb);
                iRow++;
                if (iRow > 10)
                {
                    iRow = 0;
                    iCol++;
                }
                if (i == 0) rb.Checked = true;
            }
        }
        private void rbProject_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked) return;
            current_Projectname = rb.Text;
            GetResource();
        }

        private void FormSystemsRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            int i = 0;
            int rbRow = 0;
            int rbCol = 0;
            Task aTask;
            bool bFoundOnline = false;
            foreach (cPClimit pcl in pc.PandoraDatabase)
            {
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = pcl.PCname;
                if (i == 0)
                    current_PCname = pcl.PCname;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.Enabled = false;
                ProjectStats.ManagedPCs.SelectCurrentFromPC(pcl.PCname);
                rb.Enabled = ProjectStats.ManagedPCs.CurrentHasBOINC;
                /*
                string IPaddress = ProjectStats.ManagedPCs.CurrentIPaddress;
                aTask = Task.Run(async () =>
                {
                    rb.Enabled = await globals.PortChecker.IsPortOpenAsync(IPaddress, 31416);
                });
                aTask.Wait();
                */
                rb.ForeColor = ProjectStats.ManagedPCs.GetColor(pcl.PCname);

                if (!bFoundOnline && rb.Enabled)
                {
                    bFoundOnline = true;
                    current_PCname = pcl.PCname;
                    rb.Checked = true;
                }
                rb.CheckedChanged += new System.EventHandler(this.rbPC_CheckedChanged);
                gbPCs.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
                i++;
            }
        }
        private void rbPC_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked) return;
            SetPC(rb.Text);
            GetResource();
        }

        private void GetResource()
        {
            SelectedPC = pc.GetPCbyName(current_PCname);
            ThisClp = SelectedPC.GetProjStruct(current_Projectname);
            tb_cpu.Text = ThisClp.cpu_usage.ToString("F2");
            tb_gpu.Text = ThisClp.gpu_usage.ToString("F2");
            tb_maxapps.Text = ThisClp.MaxApps.ToString();
            if(OnStartup)
            {
                OnStartup = false;
                //return;
            }
            tbSum.Text = SelectedPC.SumSprintCPUs();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();  // Close the form
            }
        }

        private async void btnFetch_Click(object sender, EventArgs e)
        {
            tbInfo.Text = "";
            string PCname = PCchecked();
            string ShortName = PJchecked();
            Debug.Assert(PCname != "" && ShortName != "", "missing project or pc name");
            SetSelected(PCname);
            await pandoraRPC.FetchSelected_app_config(ShortName);
            tbInfo.Text = string.Join(Environment.NewLine, SelectedPC.app_config);
            pc.CalcLimit(ref SelectedPC, 3.0);
            foreach (cCalcLimitProj clp in SelectedPC.ProjList)
            {
                int iTag = EnablePJ(clp.ShortName, clp.UsedInSprint);
                SetLabel(iTag, clp.ShowUsedCpuGpu, clp.UsedInSprint);
            }
            GetResource();
        }

        private void SetSelected(string PCname)
        {
            foreach (cPClimit PCl in pc.PandoraDatabase)
            {
                PCl.IsSelected = (PCname == PCl.PCname);
                if (PCname == PCl.PCname)
                {
                    SelectedPC = PCl;
                    PCl.IsSelected = true;
                }
                else PCl.IsSelected = false;
            }
        }

        private string PJchecked()
        {
            foreach (Control c in gbPJs.Controls)
            {
                if (c is RadioButton rb)
                {
                    if (rb.Checked)
                        return rb.Text;
                }
            }
            return "";
        }

        private void SetLabel(int iTag, string s, bool bVisible)
        {
            foreach (Control c in gbPJs.Controls)
            {
                if (c is Label lb)
                {
                    if ((int)lb.Tag == iTag)
                    {
                        lb.Text = s;
                        lb.Visible = bVisible;
                    }
                }
            }
        }

        private int EnablePJ(string shortname, bool b)
        {
            bool bMustSet = false;
            int iTag = -1;
            foreach (Control c in gbPJs.Controls)
            {
                if (c is RadioButton rb && rb.Text == shortname)
                {
                    rb.Enabled = b;
                    iTag = (int)rb.Tag;
                    if (!b)
                    {
                        if (rb.Checked)
                        {
                            rb.Checked = false;
                            bMustSet = true;
                        }
                    }
                }
            }
            if (bMustSet)
            {
                foreach (Control c in gbPJs.Controls)
                {
                    if (c is RadioButton rb)
                    {
                        if (rb.Enabled)
                        {
                            rb.Checked = true;
                            return iTag;
                        }
                    }
                }
            }
            return iTag;
        }

        private string PCchecked()
        {
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton rb)
                {
                    if (rb.Checked)
                        return rb.Text;
                }
            }
            return "";
        }

        private void ShowAppConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            pc.RestoreSelected();
        }

        private void btnSaveVars_Click(object sender, EventArgs e)
        {
            tbResInfo.Text = pc.WriteDBrecord(ref SelectedPC);

        }

        private async void btnSendApp_Click(object sender, EventArgs e)
        {
            string PCname = PCchecked();
            string ShortName = PJchecked();
            Debug.Assert(PCname != "" && ShortName != "", "missing project or pc name");
            SetSelected(PCname);
            pandoraRPC.TextToUnix(tbInfo.Text);
            await pandoraRPC.WriteSelected_app_config(ShortName);
            tbResInfo.Text = SelectedPC.strResult;
            if(!SelectedPC.strResult.ToLower().Contains("success"))
            {
                tcAppConfig.SelectTab("tabResults");
                tbResInfo.Text += Environment.NewLine + "file was not updated on the remote pc" + Environment.NewLine;
            }
            else
            {
                SelectedPC.app_config = tbInfo.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);                
                tbResInfo.Text = pc.WriteDBrecord(ref SelectedPC);
                tbResInfo.Text += globals.WriteACrecord(PCname, ShortName, ref SelectedPC.app_config);                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
