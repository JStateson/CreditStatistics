using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.PandoraConfig;
using static CreditStatistics.RadioBoxGroup;

namespace CreditStatistics
{
    internal partial class SampleRequest : BasePandoraPCs
    {
        public SampleRequest()
        {
            InitializeComponent();
        }

        private string selectedSCfilePath;
        private cPClimit SelectedSC;
        private bool InitlLoad = true;
        private string InitialPCname = "";
        public SampleRequest(ref cProjectStruct rProjectStats, ref ReqCmds RreqCmd)
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
            CreateAllSamplePandoras(3); //bit 0:report info, bit 1: issue nnw when limit reached
            if(InitlLoad == true)
            {
                SelectConfig(InitialPCname);
            }
            InitlLoad = false;
        }

        private void CreateAllSamplePandoras(int pc_code)
        {
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton rb && rb.Enabled)
                {
                    string PCname = (string)rb.Text;
                    SelectedSC = pandoraConfig.GetPCbyName(PCname);
                    pandoraConfig.CreateSamplePandoraConfig(pc_code, ref SelectedSC);
                    globals.WriteSCrecordS(PCname, ref SelectedSC.pc_config);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();  // Close the form
            }
        }

        public void AllowedPCs_changed(object sender, PCsChangedEventArgs e)
        {
            InitialPCname = e.CheckedName;
            if (InitlLoad) return;
            SelectConfig(e.CheckedName);
        }


        private void btnPCtoAll_Click(object sender, EventArgs e)
        {
            DialogResult mres = MessageBox.Show("This sends the pandora files to all PCs.  Press OK to confirm", "sending " + nEnabled(), MessageBoxButtons.OKCancel);
            if (mres == DialogResult.Cancel) return;
            tbInfo.Clear();
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton rb && rb.Enabled)
                {
                    string PCname = (string)rb.Text;
                    SelectedSC = pandoraConfig.GetPCbyName(PCname);
                    SelectedSC.pc_config = globals.ReadSCrecordS(PCname);
                    SendPandoraFile();
                    Application.DoEvents();
                }
            }
        }

        private void SelectConfig(string PCname)
        {
            if (PCname == "") return;
            SelectedSC = pandoraConfig.GetPCbyName(PCname);
            selectedSCfilePath = globals.GetSCfilePath(PCname);
            if (File.Exists(selectedSCfilePath))
            {
               tbPCfile.Text = File.ReadAllText(selectedSCfilePath);
            }
            else
            {
                tbPCfile.Text = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (File.Exists(selectedSCfilePath))
            {
                File.WriteAllText(selectedSCfilePath, tbInfo.Text);
            }
        }

        private void SendPandoraFile()
        {
            string cmd, cmd1;
            string sOut = "";
            if (File.Exists(selectedSCfilePath))
            {
                File.WriteAllText(selectedSCfilePath, tbInfo.Text);
            }
            if (SelectedSC.OStype == "w")
            {
                cmd = "scp " + selectedSCfilePath + " " + SelectedSC.PCname + ":c:\\programdata\\boinc\\pandora_config";
                sOut += ManagedPCs.RunScpAndGetOutput(SelectedSC.PCname, selectedSCfilePath, "C:\\programdata\\boinc\\pandora_config", "c");
            }
            else
            {
                string sUnix = globals.NewLineToLinux(File.ReadAllText(selectedSCfilePath));
                File.WriteAllText(selectedSCfilePath + ".linux", sUnix); ;
                cmd = "scp " + selectedSCfilePath + ".linux " + SelectedSC.UserName + "@" + SelectedSC.PCname + ":/home/" + SelectedSC.UserName + "/pandora_config";
                sOut += ManagedPCs.RunScpAndGetOutput(SelectedSC.PCname, selectedSCfilePath, "/home/" + SelectedSC.UserName + "/pandora_config", "c");
                cmd1 = "sudo mv -f /home/" + SelectedSC.UserName + "/pandora_config /var/lib/boinc/pandora_config";
                sOut += ManagedPCs.RunSshAndGetOutput(SelectedSC.PCname, cmd1, "c");
            }
            tbInfo.Text += sOut;
        }

        private void btnSendApp_Click(object sender, EventArgs e)
        {
            tbInfo.Clear();
            SendPandoraFile();
        }



        private void btnRemoveAll_Click(object sender, EventArgs e)
        {

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
    }
}
