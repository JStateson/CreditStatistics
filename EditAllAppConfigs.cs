using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.PandoraConfig;
using static CreditStatistics.PandoraRPC;
using static CreditStatistics.RadioBoxGroup;

namespace CreditStatistics
{
    internal partial class EditAllAppConfigs : BasePandoraPCs
    {

        private bool InitLoad = false;
        private string SelectedPCname = "";
        private string SelectedProjName;
        private string NL = Environment.NewLine;
        public EditAllAppConfigs()
        {
            InitializeComponent();
        }

        public void AllowedPCs_changed(object sender, PCsChangedEventArgs e)
        {
            if (InitLoad)
            {
                SelectedPCname = "";
                radioBoxGroup1.ShowProj(e.CheckedName);
            }
            else
            {
                InitLoad = true;
                SelectedPCname = e.CheckedName;
            }
        }

        public void AllowedPJs_changed(object sender, PJsChangedEventArgs e)
        {
            SelectedProjName = e.CheckedName;
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
            Color fC;
            int n = 0;
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                if (!hi.HasBOINC) continue;
                n += hi.LocalProjID.Count;
            }
            pbUSE.Maximum = n;
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                if (!hi.HasBOINC) continue;
                string PCname = hi.ComputerID;
                cPClimit PCl = pandoraConfig.NameToSprintPC(PCname);
                foreach (cLHe lh in hi.LocalProjID)
                {
                    string sn = lh.ProjectName;
                    string filePath = globals.GetAppConfigFilename(PCname, sn);
                    lh.HasAppConfig = File.Exists(filePath);
                    //if(!lh.HasAppConfig)
                    {
                        await pandoraRPC.FetchOne_app_config(PCname, sn);
                        if (PCl.ErrorStatus < globals.ERR_critical)
                        {
                            string[] ssAppConfig = PCl.strResult.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            string sOut = globals.WriteACrecord(filePath, ref ssAppConfig);
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
                        pbUSE.Value++;
                        await Task.Delay(500);
                        Application.DoEvents();
                    }
                }
            }
            pbUSE.Value = 0;
        }

        private async void btnSendAppConfig_Click(object sender, EventArgs e)
        {
            string sAppConfig = rtbLocalHostsBT.Text;
            pandoraRPC.TextToUnix(sAppConfig);
            cPClimit pcx = pandoraConfig.NameToSprintPC(SelectedPCname);
            await pandoraRPC.WriteOne_app_config(SelectedPCname, SelectedProjName);
            if(pcx.ErrorStatus == 0)
            {
                string sResult = ManagedPCs.ContactPCproject(SelectedPCname, " --read_cc_config", "");
            }
            else
            {
                globals.AppendColoredText(rtbLocalHostsBT, "Error: " + pcx.strResult + NL, Color.Red);
            }
            await Task.Delay(500);
        }
    }
}
