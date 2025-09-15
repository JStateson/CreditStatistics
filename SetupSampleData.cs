using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.BasePandoraPCs;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class SetupSampleData : BaseMultiples
    {
        cProjectStruct ProjectStats;

        public SetupSampleData()
        {
            InitializeComponent();
        }

        private ReqCmds reqCmd;
        private string SelectedPCname = "";

        public void project_changed(object sender, MultiChangedEventArgs e)
        {
            PcChanged(e.SelectedPCname);
        }

        private void PcChanged(string sname)
        {
            SelectedPCname = sname;
        }

        public SetupSampleData(ref cProjectStruct rProjectStats, ReqCmds reqcmd)
: base(ref rProjectStats, reqcmd)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            cbNumWanted.Items.AddRange(ComboChoices.Select(c => c.ToString()).ToArray());
            cbNumWanted.SelectedIndex = 2; // 10 work units
            reqCmd = reqcmd;
            this.PCsChanged += project_changed;
        }


        //1: show job status periodically bunkerstrategy 
        //2: issue no new work when AcqLimit reached regardless of project. for testing purposes to get a quick elapsed time estimate
        //4: issue no new work when the project limit value is reached
        public string GetSamplePandora()
        {
            string NL = Environment.NewLine;
            string sOut = @"
earliest_deadline_first
debug
bunker_strategy: 3
message_filters:has reached a limit,your preferences are set,No tasks sent,No work sent,see scheduler log,#";
            return sOut;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SetPcProjectIndex(cbNumWanted.SelectedIndex);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }
    }
}
