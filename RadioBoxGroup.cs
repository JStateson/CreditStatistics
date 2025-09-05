using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.BasePandoraPCs;
using static CreditStatistics.globals;

namespace CreditStatistics
{
    internal partial class RadioBoxGroup : UserControl
    {
        public RadioBoxGroup()
        {
            InitializeComponent();
        }

        public class PJsChangedEventArgs : EventArgs
        {
  
            public string CheckedName { get; }
            public PJsChangedEventArgs(string checkedName)
            {
                CheckedName = checkedName;
            }
        }
        public event EventHandler<PJsChangedEventArgs> PJsChanged;

        private cProjectStruct _projectStats;
        private cManagedPCs ManagedPCs;
        private string PCname;
        private bool UseRB;
        private List<string> Urls = new List<string>();
        private bool NextLoad = false;

        // Property to set the reference
        public cProjectStruct ProjectStats
        {
            get => _projectStats;
            set => _projectStats = value;
        }

        bool bH = false;
        bool bS = false;

        public void Init(bool UseRadioButtons, string ColorID)
        {
            UseRB = UseRadioButtons;
            ManagedPCs = _projectStats.ManagedPCs;
            switch(ColorID)
            {
                case "HasAppConfig" :
                    bH = true;
                break;
                case "UseSprint":
                    bS = true;
                break;  
            }
        }


        public void ShowProj(string PcName)
        {
            PCname = PcName;
            if(NextLoad)
            {
                EraseOld();
            }
            NextLoad = true;
            ManagedPCs.SelectCurrentFromPC(PCname);
            foreach (string sn in ManagedPCs.MatchingShortnames)
            {
                cPSlist pci = _projectStats.GetProjectByShortName(sn);
                Urls.Add(pci.MasterUrl);
            }

            if (UseRB)
            {
                FormControlRB();
            }
            else
            {
                FormControlCB();
            }
        }

        private void FormControlRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bIsSprint = false;
            int MaxShortSize = 0;

            for (int i = 0; i < ManagedPCs.MatchingShortnames.Count; i++)
            {
                string PJname = ManagedPCs.MatchingShortnames[i];
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = PJname;
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                bool bUsedInGames = ProjectStats.IsProjInGames(rb.Text);
                ProjectStats.ProjectList[i].UsedInSprint = bIsSprint;
                if(bS)
                    rb.ForeColor = bUsedInGames ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                else if(bH)
                    rb.ForeColor = ManagedPCs.HasAppConfig[i] ? System.Drawing.Color.Blue : System.Drawing.Color.Red;
                else                
                    rb.ForeColor = Color.Black;
                rb.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                groupBox1.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked) return;
            NotifyForms(false, rb.Text, 1);
        }


        private void FormControlCB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bIsSprint = false;
            int MaxShortSize = 0;

            for (int i = 0; i < ManagedPCs.MatchingShortnames.Count; i++)
            {
                string PJname = ManagedPCs.MatchingShortnames[i];
                CheckBox rb = new CheckBox();
                rb.Tag = i;
                rb.Text = PJname;
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                bIsSprint = ProjectStats.IsProjInSprint(rb.Text);
                bool bUsedInGames = ProjectStats.IsProjInGames(rb.Text);
                ProjectStats.ProjectList[i].UsedInSprint = bIsSprint;
                rb.ForeColor = bUsedInGames ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                groupBox1.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }

        private void EraseOld()
        {
            foreach (Control ctrl in groupBox1.Controls)
            {
                if (ctrl is RadioButton)
                    ctrl.Dispose(); // remove from memory
            }
            groupBox1.Controls.Clear(); // remove all controls from the group box
        }


        private void NotifyForms(bool bRescan, string sname, int iLoc)
        {

            PJsChanged?.Invoke(this, new PJsChangedEventArgs(sname));
        }
    }
}
