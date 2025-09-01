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

namespace CreditStatistics
{
    internal partial class BigMessageBox : Form
    {
        public string EditedPandoraConfig { get; set; }
        public bool PC_Approved { get; set; }
        public bool PC_Changed { get; set; }

        private List<cPClimit> localPandoraDatabase;
        public BigMessageBox(string PandoraConfig, ref List<cPClimit> rlocalPandoraDatabase)
        {
            InitializeComponent();
            EditedPandoraConfig = PandoraConfig;
            localPandoraDatabase = rlocalPandoraDatabase;
            PC_Approved = false;
            PC_Changed = false;
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
            lbPCs.Items.Add("Default");
            foreach (cPClimit pcL in localPandoraDatabase)
            {
                lbPCs.Items.Add(pcL.PCname);
            }
            lbPCs.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PC_Approved = true;
            TryClose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            PC_Approved = false;
            TryClose();
        }

        private void cbReadOnly_CheckStateChanged(object sender, EventArgs e)
        {
            tbBig.Enabled = !cbReadOnly.Checked;
        }

        private void TryClose()
        {
            PC_Changed = tbBig.Text.ToString() != EditedPandoraConfig;
            if (!PC_Approved) PC_Changed = false;
            else
            {
                EditedPandoraConfig = tbBig.Text.Trim();
                if (!EditedPandoraConfig.EndsWith('#'))
                    EditedPandoraConfig += Environment.NewLine + "#";
            }
            this.Close();
        }

        private void lbPCs_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iLoc = lbPCs.SelectedIndex;
            string pcName = lbPCs.Text.ToString();
            if(pcName == "Default")
            {
                tbBig.Text = EditedPandoraConfig;
                cbReadOnly.Enabled = true;
            }
            else
            {
                cPClimit SelectedDB = localPandoraDatabase[iLoc - 1];
                tbBig.Text = SelectedDB.CreateTemplet();
                tbBig.ReadOnly = true;
                cbReadOnly.Enabled = false;
            }
        }
    }
}
