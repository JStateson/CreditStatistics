using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class CtrlRemotePCs : BasePandoraPCs
    {
        string pcName = "";
        string ProjShortName = "";
        string MasterUrl = "";
        int TagOfProject = -1;

        public void AllowedPCs_changed(object sender, PCsChangedEventArgs e)
        {

        }


        public CtrlRemotePCs(ref cProjectStruct rProjectStats, ref ReqCmds RreqCmd)
: base(ref rProjectStats, ref RreqCmd) //Pass required parameters to base constructor
        {
            InitializeComponent();
            this.PCsChanged += AllowedPCs_changed;
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Shown += InitialLoad;
        }


        private void InitialLoad(object sender, EventArgs e)
        {
            FormProjectRB();
            //FormProjectCB();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();  // Close the form
            }
        }

        private void FormProjectRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;

            for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
            {
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = ProjectStats.ShortName(i);

                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.ForeColor = bHasID ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                rb.Checked = (i == 0);
                gbSamURL.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }


        private void FormProjectCB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;
            for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
            {
                CheckBox rb = new CheckBox();
                rb.Tag = i;
                rb.Text = ProjectStats.ShortName(i);
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.ForeColor = bHasID ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                gbSamURL.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }

        private void rbProject_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked)
            {
                if (TagOfProject >= 0)
                    ProjectStats.ProjectList[TagOfProject].IsSelected = false;
                return;
            }

            TagOfProject = (int)rb.Tag;
            ProjectStats.ProjectList[TagOfProject].IsSelected = true;
            ProjShortName = ProjectStats.ShortName(TagOfProject);
            MasterUrl = ProjectStats.ProjectList[TagOfProject].MasterUrl;
            ManagedPCs.ProjectIDfromPCsShortname(ProjShortName);
        }


        /* 
        private void xRestartBoinc(ref cPClimit PCl)
        {
            bool bIsLinux = PCl.OStype != "w";
            string sCommand = bIsLinux ? "sudo /etc/init.d/boinc-client restart" : "schtasks /run /tn \"CopyNewPandora\"";
            if (!PCl.IsLocalhost())
            {
                //xRunSshAndGetOutput(ref PCl, sCommand, bIsLinux, "c");
                ManagedPCs.RunSshAndGetOutput(PCl.PCname, sCommand, "c");
            }
            else
            {
                string sResult = globals.BoincCommand("boinccmd.exe", "--quit");
                System.Threading.Thread.Sleep(10000);
                sResult = globals.BoincCommand("boinc.exe", "--detach --allow_remote_gui_rpc");
            }
        }

        private void SSHconnect(ref cPClimit PCl)
        {
            bool bIsLinux = PCl.OStype != "w";
            //xRunSshAndGetOutput(ref PCl, "", bIsLinux, "k");
            ManagedPCs.RunSshAndGetOutput(PCl.PCname, "", "k");
        }

        */

        private void ShutdownOS(string PCname)
        {
            cHostInfo hi = ManagedPCs.NameToSystem(PCname); ;
            string sCommand = hi.OStype != "w" ? "sudo shutdown -h now" : "shutdown /s /f /t 0";
            ManagedPCs.RunSshAndGetOutput(PCname, sCommand, "c");
        }

        private void RestartOS(string PCname)
        {
            cHostInfo hi = ManagedPCs.NameToSystem(PCname); ;
            string sCommand = hi.OStype != "w" ? "sudo shutdown -r now" : "shutdown /r /f /t 0";
            ManagedPCs.RunSshAndGetOutput(PCname, sCommand, "c");
        }

        private bool CancelOp(string sMsg)
        {
            DialogResult dr = MessageBox.Show(sMsg, "Click Yes to confirm",MessageBoxButtons.YesNoCancel);
            return (dr != DialogResult.Yes);          
        }

        private void btnPCoff_Click(object sender, EventArgs e)
        {
            if (CancelOp("About to shutdown a lot of PCs, are you sure?"))return;
            BaseProgressBar.Value = 0;
            foreach (cPClimit PCl in PandoraDatabase)
            {
                BaseProgressBar.Value++;
                Application.DoEvents();
                if (PCl.IsSelected)
                    ShutdownOS(PCl.PCname);
            }
            BaseProgressBar.Value = 0;
        }

        private void btnPCreset_Click(object sender, EventArgs e)
        {
            if (CancelOp("About to restart a lot of PCs, are you sure?")) return;
            BaseProgressBar.Value = 0;
            foreach (cPClimit PCl in PandoraDatabase)
            {
                BaseProgressBar.Value++;
                Application.DoEvents();
                if (PCl.IsSelected)
                    RestartOS(PCl.PCname);
            }
            BaseProgressBar.Value = 0;
        }

        private void btnBoincRestart_Click(object sender, EventArgs e)
        {
            if (CancelOp("About to restart boinc on a lot of PCs, are you sure?")) return;
            BaseProgressBar.Value = 0;
            foreach (cPClimit PCl in PandoraDatabase)
            {
                BaseProgressBar.Value++;
                Application.DoEvents();
                if (PCl.IsSelected) 
                    ManagedPCs.RestartBoinc(PCl.PCname);
                //cPClimit PCx = PCl;
                //if (PCl.IsSelected) xRestartBoinc(ref PCx);
            }
            BaseProgressBar.Value = 0;
        }

        private void btnRemovePC_Click(object sender, EventArgs e)
        {
            BaseProgressBar.Value = 0;
            foreach (cPClimit PCl in PandoraDatabase)
            {
                BaseProgressBar.Value++;
                Application.DoEvents();
                //cPClimit PCx = PCl;
                //if (PCl.IsSelected) RemovePandora(ref PCx);
                RemovePandora(PCl.PCname);
            }
            BaseProgressBar.Value = 0;
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

        private async void btnSuspProj_Click(object sender, EventArgs e)
        {
            ContactPCs("suspend");
        }

        private async void btnResume_Click(object sender, EventArgs e)
        {
            ContactPCs("resume");
        }

        private async void btnANW_Click(object sender, EventArgs e)
        {
            ContactPCs("allowmorework");
        }

        private async void btnNNW_Click(object sender, EventArgs e)
        {
            ContactPCs("nomorework");
        }


        private async void ContactPCs(string sCmd)
        {
            foreach (cPClimit PC in PandoraDatabase)
            {
                if (!PC.IsSelected) continue;
                cHostInfo hi = ManagedPCs.NameToSystem(PC.PCname);
                await ManagedPCs.sendPCproject(PC.PCname, sCmd, MasterUrl);
                Application.DoEvents();
                await Task.Delay(1000);
                if (PC.ErrorStatus > ERR_warning) hi.HasBOINC = false;
                IsStillOnline(PC.PCname, true, PC.ErrorStatus == 0);
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ContactPCs("update");
        }

        private RadioButton GetRBfromName(string shortname)
        {
            int n = 0;
            foreach(Control c in gbSamURL.Controls)
            {
                if (c is RadioButton rb && (rb.Text == shortname))
                {
                    n = 1;
                    return rb;
                }
                
            }
            Debug.Assert(n==1, " project name not found ");
            return null;
        }

        private int CountPJs()
        {
            int n = 0;
            foreach (cPClimit PC in PandoraDatabase)
            {
                if (!PC.IsSelected) continue;
                foreach (cCalcLimitProj clp in PC.ProjList)
                {
                    if (clp.UsedInSprint) n++;
                }
            }
            return n;
        }


        private void bltnSelectSprint_Click(object sender, EventArgs e)
        {
            DialogResult sRes = MessageBox.Show("You will resume project " + ProjShortName + " and suspend all others", "Suspand non-sprint projects", MessageBoxButtons.OKCancel);
            int nLast = -1;
            RadioButton ThisRB;
            RadioButton LastRB = null;
            int nThisPJ;
            string sCmd;
            string sArgs;
            string sOut = "";
            Color rbColor;
            pbProj.Maximum = CountPJs();
            if (pbProj.Maximum == 0) return;
            pbProj.Value = 0;
            if (sRes == DialogResult.Cancel) return;
            foreach (cPClimit PC in PandoraDatabase)
            {
                if(!PC.IsSelected) continue; //todo to do may want to look at bUsedInSprint also
                string PCname = PC.PCname;
                lbCurPC.Text = PCname;
                foreach(cCalcLimitProj clp in PC.ProjList)
                {
                    if (!clp.UsedInSprint) continue;
                    ThisRB = GetRBfromName(clp.ShortName);  // index into the radio buttons
                    nThisPJ =  ProjectStats.ShortnameToIndex(clp.ShortName);
                    string GameUrl = ProjectStats.ProjectList[nThisPJ].MasterUrl;
                    if (clp.ShortName == ProjShortName)
                    {
                        sCmd = "resume";
                        rbColor = Color.Blue;
                    }
                    else
                    {
                        sCmd = "suspend";
                        rbColor = Color.Red;
                    }

                    string sResult = ManagedPCs.ContactPCproject(PC.PCname, sCmd, GameUrl);
                    /*
                    if (PC.IsLocalhost())
                        sArgs = "--project " + GameUrl + " " + sCmd;
                    else
                        sArgs = "--host " + PC.PCname + " --project " + GameUrl + " " + sCmd;
                    string sResult = globals.BoincCommand("boinccmd.exe", sArgs);
                    */
                    IsStillOnline(PC.PCname, true, PC.ErrorStatus == 0);
                    sOut += sResult;
                    ThisRB.ForeColor = rbColor;
                    if (LastRB != null) LastRB.ForeColor = Color.Black;
                    LastRB = ThisRB;
                    Application.DoEvents();
                    pbProj.Value++;
                }
            }
            if (LastRB != null)
                LastRB.ForeColor = Color.Black;
            pbProj.Value = 0;
        }
    }
}
