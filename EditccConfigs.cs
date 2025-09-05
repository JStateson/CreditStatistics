using CreditStatistics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class EditccConfigs : BasePandoraPCs
    {
        cPClimit CurrentPCDB;
        string OneShot = "";
        public void AllowedPCs_changed(object sender, PCsChangedEventArgs e)
        {
            SelectConfig(e.CheckedName);
        }

        public EditccConfigs(ref cProjectStruct rProjectStats, ref ReqCmds RreqCmd)
: base(ref rProjectStats, ref RreqCmd) //Pass required parameters to base constructor
        {
            InitializeComponent();
            pandoraConfig.SaveSelected();
            this.PCsChanged += AllowedPCs_changed;
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

        private async void InitialLoad(object sender, EventArgs e)
        {

        }

        private void SetSelected(string PCname)
        {
            foreach (cPClimit PCl in PandoraDatabase)
            {
                PCl.IsSelected = (PCname == PCl.PCname);
            }
        }


        private async void SelectConfig(string PCname)
        {
            SetSelected(PCname);
            CurrentPCDB = pandoraConfig.NameToSprintPC(PCname);
            await pandoraRPC.FetchSelected_cc_config();

            if (CurrentPCDB.cc_config != null)
            {
                tbCC_config.Text = string.Join(Environment.NewLine, CurrentPCDB.cc_config);
            }
            else
            {
                tbCC_config.Text = "";
            }
            if (CurrentPCDB.pc_config != null)
            {
                tbPC_config.Text = string.Join(Environment.NewLine, CurrentPCDB.pc_config);
            }
            else
            {
                tbPC_config.Text = "";
            }
        }

        private void btnCancelPC_Click(object sender, EventArgs e)
        {
            //tbPC_config.Text = bu_pc;
        }

        private void btnSetDefPC_Click(object sender, EventArgs e)
        {
            string s = pandoraConfig.ReadPandoraConfig(ref CurrentPCDB, out string filePath);
            if (s == "")
                tbPC_config.Text = ProjectStats.current_default_pandora_config;
            else tbPC_config.Text = s;
        }

        private async void btnSavePC_Click(object sender, EventArgs e)
        {
            CurrentPCDB.pc_config = tbPC_config.Text.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            await pandoraRPC.WriteSelected_cc_pc_config();
        }
        private void btnCancelCC_Click(object sender, EventArgs e)
        {
            //tbCC_config.Text = bu_cc;
        }

        private void btnSetDefCC_Click(object sender, EventArgs e)
        {
            tbCC_config.Text = ProjectStats.current_default_cc_config;
        }

        private async void btnSaveCC_Click(object sender, EventArgs e)
        {
            CurrentPCDB.cc_config = tbCC_config.Text.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            await pandoraRPC.WriteSelected_cc_config();
            //if (CurrentPCDB.ErrorStatus == 0)
            //    globals.WriteCCrecord(bu_name, ref CurrentPCDB.cc_config);
        }

        private void EditAllConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            pandoraConfig.RestoreSelected();
        }
    }
}
