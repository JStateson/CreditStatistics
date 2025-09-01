using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreditStatistics
{
    internal partial class OnlinePCReport : BasePandoraPCs
    {

        private string selectedPC = "";

        public OnlinePCReport(ref cProjectStruct rProjectStats, ref ReqCmds RreqCmd)
: base(ref rProjectStats, ref RreqCmd) //Pass required parameters to base constructor
        {
            InitializeComponent();
            this.PCsChanged += AllowedPCs_changed;
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Shown += InitialLoad;
        }

        public void AllowedPCs_changed(object sender, PCsChangedEventArgs e)
        {
            ChangeOccured(e.NumChecked);
            selectedPC = e.CheckedName;
        }

        private void ChangeOccured(int n)
        {
        }

        private void InitialLoad(object sender, EventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();  // Close the form
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSSHconnect_Click(object sender, EventArgs e)
        {
            if (selectedPC == "") return;
            ManagedPCs.SSHconsole(selectedPC);
        }
    }
}
