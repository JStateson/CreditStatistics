using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static CreditStatistics.PandoraConfig;
using static CreditStatistics.PandoraRPC;
using static CreditStatistics.RadioBoxGroup;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace CreditStatistics
{
    internal partial class EditAllAppConfigs : BasePandoraPCs
    {

        private bool InitLoad = false;
        private string SelectedPCname = "";
        private string SelectedProjName;
        private string NL = Environment.NewLine;
        private bool CancelOperation = false;


        public EditAllAppConfigs()
        {
            InitializeComponent();
        }

        private string UpdateTSM(string sn, string sIn)
        {
            int i = sIn.IndexOf(':');
            if (i == -1) return sIn;
            return sn + sIn.Substring(i);
        }

        private string UpdateTSM(string sn, string sIn, string PJname)
        {
            string s = UpdateTSM(sn, sIn);
            int i = s.IndexOf('(');
            int j = s.IndexOf(')');
            Debug.Assert(i >= 0 && j >= 0);
            return s.Substring(0,i+1) + PJname + s.Substring(j);
        }


        public void AllowedPCs_changed(object sender, PCsChangedEventArgs e)
        {
            if (InitLoad)
            {
                rtbLocalHostsBT.Clear();
                radioBoxGroup1.ShowProj(e.CheckedName);
                SelectedPCname = e.CheckedName;
                tsmAllac.Text = UpdateTSM(SelectedPCname, tsmAllac.Text);
                tsmFetch1cc.Text = UpdateTSM(SelectedPCname, tsmFetch1cc.Text);
                tsmForceRead.Text = UpdateTSM(SelectedPCname, tsmForceRead.Text);
                ReadSelectedCCconfig();
                CancelOperation = e.pbUseCancel;
            }
            else
            {
                InitLoad = true;
                SelectedPCname = e.CheckedName;
                ReadSelectedCCconfig();
            }
        }

        private void ReadSelectedCCconfig()
        {
            tbCCconfig.Text = string.Join(Environment.NewLine, globals.ReadCCrecord(SelectedPCname));
        }

        public void AllowedPJs_changed(object sender, PJsChangedEventArgs e)
        {
            SelectedProjName = e.CheckedName;
            tsm1ac.Text = UpdateTSM(SelectedPCname, tsm1ac.Text, SelectedProjName);
            string sAppConfig = globals.ReadACstring(SelectedPCname, SelectedProjName, out bool IsDefault);
            rtbLocalHostsBT.Clear();
            if (sAppConfig != "")
                rtbLocalHostsBT.Text = sAppConfig;
            else
                globals.AppendColoredText(rtbLocalHostsBT, "System " + SelectedPCname + " does not have an app_config for project " + SelectedProjName + NL, Color.Red);
        }

        public EditAllAppConfigs(ref cProjectStruct rProjectStats, ref ReqCmds RreqCmd)
: base(ref rProjectStats, ref RreqCmd) //Pass required parameters to base constructor
        {
            InitializeComponent();
            radioBoxGroup1.ProjectStats = rProjectStats;
            radioBoxGroup1.Init(true, "HasAppConfig");
            this.PCsChanged += AllowedPCs_changed;
            this.radioBoxGroup1.PJsChanged += AllowedPJs_changed;
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Shown += InitialLoad;
        }
        private void InitialLoad(object sender, EventArgs e)
        {
            if (SelectedPCname != "")
                radioBoxGroup1.ShowProj(SelectedPCname);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();  // Close the form
            }
        }


        private async void btnCountAC_Click(object sender, EventArgs e)
        {

            int n = 0;
            Color fC;
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                if (!hi.HasBOINC) continue;
                n += hi.LocalProjID.Count;
            }
            if (n < 0) return;
            StartPBuse(n);
            CancelOperation = false;
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                if (!hi.HasBOINC) continue;
                string PCname = hi.ComputerID;
                cPClimit PCl = pandoraConfig.NameToSprintPC(PCname);
                foreach (cLHe lh in hi.LocalProjID)
                {
                    string sn = lh.ProjectName;
                    string filePath = globals.FormAppConfigFilename(PCname, sn);
                    lh.HasAppConfig = File.Exists(filePath);
                    if (CancelOperation) break;
                    await pandoraRPC.FetchOne_app_config(PCname, sn);
                    if (PCl.ErrorStatus < globals.ERR_critical)
                    {
                        PCl.app_config = PCl.strResult.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        string sOut = globals.WriteACrecord(filePath, ref PCl.app_config);
                        fC = Color.Blue;
                        if (PCl.ErrorStatus == globals.ERR_warning)
                            fC = Color.Black;
                        lh.HasAppConfig = true;
                        globals.AppendColoredText(rtbLocalHostsBT, sOut, fC);
                    }
                    else
                    {
                        fC = Color.Red;
                        globals.AppendColoredText(rtbLocalHostsBT, "PC " + PCname + " Proj: " + sn + " has  no app_config\r\n", fC);
                        lh.HasAppConfig = false;
                    }
                    IncPBuse();
                    await Task.Delay(500);
                    Application.DoEvents();

                }
                if (CancelOperation) break;
            }
            StopPBuse();
        }

        private async void btnSendAppConfig_Click(object sender, EventArgs e)
        {
            string sAppConfig = rtbLocalHostsBT.Text;
            pandoraRPC.TextToUnix(sAppConfig);
            cPClimit pcx = pandoraConfig.NameToSprintPC(SelectedPCname);
            await pandoraRPC.WriteOne_app_config(SelectedPCname, SelectedProjName);
            if (pcx.ErrorStatus == 0)
            {
                //string sResult = ManagedPCs.ContactPCproject(SelectedPCname, " --read_cc_config", "");
            }
            else
            {
                globals.AppendColoredText(rtbLocalHostsBT, "Error: " + pcx.strResult + NL, Color.Red);
            }
            await Task.Delay(500);
        }

        private async void btnFetchCCconfig_Click(object sender, EventArgs e)
        {
            Color fC;
            int n = ManagedPCs.LocalSystems.Count();
            if (n < 0) return;
            StartPBuse(n);
            CancelOperation = false;
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                if (!hi.HasBOINC) continue;
                if (CancelOperation) break;
                IncPBuse();
                cPClimit PCl = pandoraConfig.GetPCbyName(hi.ComputerID);
                await pandoraRPC.FetchOne_cc_config(hi.ComputerID);
                tbCCconfig.Text = string.Join(Environment.NewLine, PCl.cc_config);
            }
            StopPBuse();
        }

        private async void btnSendCcConfig_Click(object sender, EventArgs e)
        {
            string sAppConfig = tbCCconfig.Text;
            pandoraRPC.TextToUnix(sAppConfig);
            cPClimit pcx = pandoraConfig.NameToSprintPC(SelectedPCname);
            await pandoraRPC.WriteOne_cc_config(SelectedPCname);
            if (pcx.ErrorStatus == 0)
            {
                //string sResult = ManagedPCs.ContactPCproject(SelectedPCname, " --read_cc_config", "");
            }
            else
            {
                globals.AppendColoredText(rtbLocalHostsBT, "Error: " + pcx.strResult + NL, Color.Red);
            }
            await Task.Delay(500);
        }

        private void rtbLocalHostsBT_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmsConfigOptions.Show(Cursor.Position);
            }
            rtbLocalHostsBT.ScrollToCaret();
        }

        private void tbCCconfig_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmsConfigOptions.Show(Cursor.Position);
            }
            tbCCconfig.ScrollToCaret();
        }

        private async void cmsConfigOptions_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            ToolStripMenuItem menuItem = e.ClickedItem as ToolStripMenuItem;
            cPClimit PCl;
            cHostInfo hi;
            cLHe lh;
            Color fC = Color.Black;
            int n;
            string filePath;

            if (cms?.SourceControl is TextBoxBase tb)
            {
                switch (menuItem.Text)
                {
                    case "Copy":
                        tb.Copy(); break;
                    case "Cut":
                        tb.Cut(); break;
                    case "Paste":
                        tb.Paste(); break;
                    case "Delete":
                        tb.SelectedText = ""; break;
                }
            }
            else if (cms?.SourceControl is RichTextBox rtb)
            {
                switch (menuItem.Text)
                {
                    case "Copy":
                        rtb.Copy(); break;
                    case "Cut":
                        rtb.Cut(); break;
                    case "Paste":
                        rtb.Paste(); break;
                    case "Delete":
                        rtb.SelectedText = ""; break;
                }
            }
            switch((string)menuItem.Tag)
            {
                case "fetch1cc":
                    PCl = pandoraConfig.GetPCbyName(SelectedPCname);
                    await pandoraRPC.FetchOne_cc_config(SelectedPCname);
                    tbCCconfig.Text = string.Join(Environment.NewLine, PCl.cc_config);
                    break;
                case "fetch1ac":
                    PCl = pandoraConfig.NameToSprintPC(SelectedPCname);
                    hi = ManagedPCs.NameToSystem(SelectedPCname);
                    lh = hi.GetLocalProjectInfo(SelectedProjName);
                    filePath = globals.FormAppConfigFilename(SelectedPCname, SelectedProjName);
                    await pandoraRPC.FetchOne_app_config(SelectedPCname, SelectedProjName);
                    if (PCl.ErrorStatus < globals.ERR_critical)
                    {
                        PCl.app_config = PCl.strResult.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        string sOut = globals.WriteACrecord(filePath, ref PCl.app_config);
                        fC = Color.Blue;
                        if (PCl.ErrorStatus == globals.ERR_warning)
                            fC = Color.Black;
                        lh.HasAppConfig = true;
                        globals.AppendColoredText(rtbLocalHostsBT, sOut, fC);
                    }
                    else
                    {
                        fC = Color.Red;
                        globals.AppendColoredText(rtbLocalHostsBT, "PC " + SelectedPCname + " Proj: " + SelectedProjName + " has  no app_config\r\n", fC);
                        lh.HasAppConfig = false;
                    }
                    break;
                case "fetchAllac":
                    n = 0;
                    PCl = pandoraConfig.NameToSprintPC(SelectedPCname);
                    hi = ManagedPCs.NameToSystem(SelectedPCname);
                    if (!hi.HasBOINC) return;
                    n = hi.LocalProjID.Count;
                    if (n < 0) return;
                    StartPBuse(n);
                    CancelOperation = false;
                    
                    foreach (cLHe nlh in hi.LocalProjID)
                    {
                        string sn = nlh.ProjectName;
                        filePath = globals.FormAppConfigFilename(SelectedPCname, sn);
                        nlh.HasAppConfig = File.Exists(filePath);
                        if (CancelOperation) break;
                        await pandoraRPC.FetchOne_app_config(SelectedPCname, sn);
                        if (PCl.ErrorStatus < globals.ERR_critical)
                        {
                            PCl.app_config = PCl.strResult.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            string sOut = globals.WriteACrecord(filePath, ref PCl.app_config);
                            fC = Color.Blue;
                            if (PCl.ErrorStatus == globals.ERR_warning)
                                fC = Color.Black;
                            nlh.HasAppConfig = true;
                            globals.AppendColoredText(rtbLocalHostsBT, sOut, fC);
                        }
                        else
                        {
                            fC = Color.Red;
                            globals.AppendColoredText(rtbLocalHostsBT, "PC " + SelectedPCname + " Proj: " + sn + " has  no app_config\r\n", fC);
                            nlh.HasAppConfig = false;
                        }
                        IncPBuse();
                        Application.DoEvents();
                        await Task.Delay(500);
                    }
                    StopPBuse();
                    break;
                case "read1c":
                    ManagedPCs.ContactPCproject(SelectedPCname, " --read_cc_config", "");
                    break;
                case "readAllac":
                    n = ManagedPCs.LocalSystems.Count();
                    if (n < 0) return;
                    StartPBuse(n);
                    CancelOperation = false;
                    foreach (cHostInfo nhi in ManagedPCs.LocalSystems)
                    {
                        if (!nhi.HasBOINC) continue;
                        if (CancelOperation) break;
                        IncPBuse();
                        Application.DoEvents();
                        ManagedPCs.ContactPCproject(SelectedPCname, " --read_cc_config", "");
                    }
                    StopPBuse();
                    break;
            }
        }
    }
}
