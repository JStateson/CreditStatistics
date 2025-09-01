using Accessibility;
using CreditStatistics;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Management;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class ShowPandora : Form
    {
        private cPClimit TempletDB;
        private cProjectStruct ProjectStats;
        private List<cPClimit> PClimit;
        private PandoraConfig pc;
        private cPClimit SelectedPC = null;
        private string selectedPCfilePath = "";
        private cManagedPCs ManagedPCs;

        public ShowPandora(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            ManagedPCs = rProjectStats.ManagedPCs;  
            TempletDB = ProjectStats.TempletDB;
            pc = new PandoraConfig(ref rProjectStats); ;
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Shown += InitialLoad;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();  // Close the form
            }
        }

        private void InitialLoad(object sender, EventArgs e)
        {
            FormSystemsRB();
        }

        private void FormSystemsRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            int i = 0;
            int rbRow = 0;
            int rbCol = 0;
            //Task aTask;
            bool bFoundOnline = false;
            foreach (cPClimit pcl in pc.PandoraDatabase)
            {
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = pcl.PCname;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.Enabled = false; // pcl.IsSelected;

                ProjectStats.ManagedPCs.SelectCurrentFromPC(pcl.PCname);
                string IPaddress = ProjectStats.ManagedPCs.CurrentIPaddress;

                /*
                aTask = Task.Run(async () =>
                {
                    rb.Enabled = await globals.PortChecker.IsPortOpenAsync(IPaddress, 31416);
                });
                aTask.Wait();
                Application.DoEvents();
                */
                rb.Enabled = ProjectStats.ManagedPCs.CurrentHasBOINC;
                rb.ForeColor = ProjectStats.ManagedPCs.GetColor(pcl.PCname);
                rb.CheckedChanged += new System.EventHandler(this.rbPC_CheckedChanged);

                if (!bFoundOnline && rb.Enabled)
                {
                    bFoundOnline = true;
                    rb.Checked = true;
                }

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
            int iPC = (int)rb.Tag;
            SelectedPC = pc.PandoraDatabase[iPC];
            string s = pc.ReadPandoraConfig(ref SelectedPC, out string filePath);
            selectedPCfilePath = filePath;
            tbInfo.Text = s;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (File.Exists(selectedPCfilePath))
            {
                File.WriteAllText(selectedPCfilePath, tbInfo.Text);
            }
        }

        private void SendPandoraFile()
        {
            string cmd, cmd1;
            if (File.Exists(selectedPCfilePath))
            {
                File.WriteAllText(selectedPCfilePath, tbInfo.Text);
            }
            if (SelectedPC.OStype == "w")
            {
                cmd = "scp " + selectedPCfilePath + " " + SelectedPC.PCname + ":c:\\programdata\\boinc\\pandora_config";
                ManagedPCs.RunScpAndGetOutput(SelectedPC.PCname, selectedPCfilePath, "C:\\programdata\\boinc\\pandora_config", "c");
            }
            else
            {
                string sUnix = globals.NewLineToLinux(File.ReadAllText(selectedPCfilePath));
                File.WriteAllText(selectedPCfilePath + ".linux", sUnix); ;
                cmd = "scp " + selectedPCfilePath + ".linux " + SelectedPC.UserName + "@" + SelectedPC.PCname + ":/home/" + SelectedPC.UserName + "/pandora_config";
                ManagedPCs.RunScpAndGetOutput(SelectedPC.PCname, selectedPCfilePath, "/home/" + SelectedPC.UserName + "/pandora_config", "c");
                cmd1 = "sudo mv -f /home/" + SelectedPC.UserName + "/pandora_config /var/lib/boinc/pandora_config";
                ManagedPCs.RunSshAndGetOutput(SelectedPC.PCname, cmd1, "c");
            }

        }

        private void btnSendApp_Click(object sender, EventArgs e)
        {
            SendPandoraFile(); 
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemovePandora(SelectedPC.PCname);
        }

        private void RemovePandora(string PCname)
        {
            cHostInfo hi = ManagedPCs.NameToSystem(PCname);
            string sCommand = "";

            if (hi.OStype != "w")
            {
                sCommand = "sudo rm -f /var/lib/boinc/pandora_config";
            }
            else
            {
                sCommand = "\"del \\ProgramData\\boinc\\pandora_config\"";
            }

            if (hi.IPaddress != "127.0.0.1")
            {
                ManagedPCs.RunSshAndGetOutput(PCname, sCommand, "c");
                //xRunSshAndGetOutput(ref PCl, sCommand, bIsLinux, "c");
            }
            else
            {
                File.Delete("C:\\ProgramData\\boinc\\pandora_config");
            }
            ManagedPCs.RestartBoinc(PCname);
        }



        private string nEnabled()
        {
            int n = 0;
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton rb && rb.Enabled)
                {
                    n++;
                }
            }
            return n.ToString();
        }

        private void btnPCtoAll_Click(object sender, EventArgs e)
        {
            DialogResult mres = MessageBox.Show("This send the pandora file to all PCs.  Press OK to confirm", "sending " + nEnabled(), MessageBoxButtons.OKCancel);
            if (mres == DialogResult.Cancel) return;
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton rb && rb.Enabled)
                {
                    int iPC = (int)rb.Tag;
                    SelectedPC = pc.PandoraDatabase[iPC];
                    string s = pc.ReadPandoraConfig(ref SelectedPC, out string filePath);
                    selectedPCfilePath = filePath;
                    tbInfo.Text = s;
                    SendPandoraFile();
                    Application.DoEvents();
                }
            }
        }

        private async void btnSuspAll_Click(object sender, EventArgs e)
        {
            DialogResult mres = MessageBox.Show("This will suspend all sprint projects on all PCs.  Press OK to confirm", "sending " + nEnabled(), MessageBoxButtons.OKCancel);
            if (mres == DialogResult.Cancel) return;
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton rb && rb.Enabled)
                {
                    int iPC = (int)rb.Tag;
                    SelectedPC = pc.PandoraDatabase[iPC];
                    string PCname = SelectedPC.PCname;

                    //ContactPCs("nomorework");

                    foreach (cCalcLimitProj clp in SelectedPC.ProjList)
                    {
                        if (!clp.UsedInSprint) continue;
                        int iLoc = ProjectStats.ShortnameToIndex(clp.ShortName);
                        string GameUrl = ProjectStats.ProjectList[iLoc].MasterUrl;
                        cHostInfo hi = ManagedPCs.NameToSystem(PCname);
                        await ManagedPCs.sendPCproject(PCname, "suspend", GameUrl);
                        if (SelectedPC.ErrorStatus != 0) hi.HasBOINC = false;
                        rb.Enabled = hi.HasBOINC;
                        Application.DoEvents();
                    }
                }
            }
        }

        private async void btnResAll_Click(object sender, EventArgs e)
        {
            DialogResult mres = MessageBox.Show("This resume all sprint projects on all PCs.  Press OK to confirm", "sending " + nEnabled(), MessageBoxButtons.OKCancel);
            if (mres == DialogResult.Cancel) return;
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton rb && rb.Enabled)
                {
                    int iPC = (int)rb.Tag;
                    SelectedPC = pc.PandoraDatabase[iPC];
                    string PCname = SelectedPC.PCname;

                    //ContactPCs("nomorework");

                    foreach (cCalcLimitProj clp in SelectedPC.ProjList)
                    {
                        if (!clp.UsedInSprint) continue;
                        int iLoc = ProjectStats.ShortnameToIndex(clp.ShortName);
                        string GameUrl = ProjectStats.ProjectList[iLoc].MasterUrl;
                        cHostInfo hi = ManagedPCs.NameToSystem(PCname);
                        await ManagedPCs.sendPCproject(PCname, "resume", GameUrl);
                        if (SelectedPC.ErrorStatus != 0) hi.HasBOINC = false;
                        rb.Enabled = hi.HasBOINC;
                        Application.DoEvents();
                    }
                }
            }
        }

        private async void btnAllowWork_Click(object sender, EventArgs e)
        {
            DialogResult mres = MessageBox.Show("This allows new work on all sprint projects for all PCs.  Press OK to confirm", "sending " + nEnabled(), MessageBoxButtons.OKCancel);
            if (mres == DialogResult.Cancel) return;
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton rb && rb.Enabled)
                {
                    int iPC = (int)rb.Tag;
                    SelectedPC = pc.PandoraDatabase[iPC];
                    string PCname = SelectedPC.PCname;

                    //ContactPCs("nomorework");

                    foreach (cCalcLimitProj clp in SelectedPC.ProjList)
                    {
                        if (!clp.UsedInSprint) continue;
                        int iLoc = ProjectStats.ShortnameToIndex(clp.ShortName);
                        string GameUrl = ProjectStats.ProjectList[iLoc].MasterUrl;
                        cHostInfo hi = ManagedPCs.NameToSystem(PCname);
                        await ManagedPCs.sendPCproject(PCname, "allowmorework", GameUrl);
                        if (SelectedPC.ErrorStatus != 0) hi.HasBOINC = false;
                        rb.Enabled = hi.HasBOINC;
                        Application.DoEvents();
                    }
                }
            }
        }

        private async void btnNoWork_Click(object sender, EventArgs e)
        {
            DialogResult mres = MessageBox.Show("This prevents new work on all sprint projects for all PCs.  Press OK to confirm", "sending " + nEnabled(), MessageBoxButtons.OKCancel);
            if (mres == DialogResult.Cancel) return;
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton rb && rb.Enabled)
                {
                    int iPC = (int)rb.Tag;
                    SelectedPC = pc.PandoraDatabase[iPC];
                    string PCname = SelectedPC.PCname;

                    //ContactPCs("nomorework");

                    foreach (cCalcLimitProj clp in SelectedPC.ProjList)
                    {
                        if (!clp.UsedInSprint) continue;
                        int iLoc = ProjectStats.ShortnameToIndex(clp.ShortName);
                        string GameUrl = ProjectStats.ProjectList[iLoc].MasterUrl;
                        cHostInfo hi = ManagedPCs.NameToSystem(PCname);
                        await ManagedPCs.sendPCproject(PCname, "nomorework", GameUrl);
                        if (SelectedPC.ErrorStatus != 0) hi.HasBOINC = false;
                        rb.Enabled = hi.HasBOINC;
                        Application.DoEvents();
                    }
                }
            }
        }


        private void ContactPCs(string sCmd)
        {
            string sArgs = "";
            foreach (cCalcLimitProj clp in SelectedPC.ProjList)
            {
                if (!clp.UsedInSprint) continue;
                int iLoc = ProjectStats.ShortnameToIndex(clp.ShortName);
                string GameUrl = ProjectStats.ProjectList[iLoc].MasterUrl;

                // suspend, resume, sllowmorework, nomorework, update
                string sResult = ManagedPCs.ContactPCproject(SelectedPC.PCname, sCmd, GameUrl);  
                Application.DoEvents();
            }
        }

    }
}
