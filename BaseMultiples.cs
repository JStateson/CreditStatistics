using Microsoft.Playwright;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.BasePandoraPCs;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class BaseMultiples : Form
    {
        public int[] ComboChoices = { 0, 5, 10, 20, 40 };
        public class CheckedProjInfo
        {
            public string PCname;
            public bool[] InSprint;
            public int[] SelectedIndi;
            public void Init(string rc, string sPCname, int n)
            {
                PCname = sPCname;
                InSprint = new bool[n];
                switch(rc)
                {
                    case "cpugpu":
                        SelectedIndi = new int[n];
                        break;
                    case "WUsWanted":
                        SelectedIndi = Enumerable.Repeat(2, n).ToArray();
                        break;
                    case "StudyWanted":
                        SelectedIndi = new int[n];
                        break;
                }
            }
        }
        public List<CheckedProjInfo> cpi = new List<CheckedProjInfo>();

        public void SetPcProjectIndex(int SelectedIndex)
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is ComboBox cbx)
                {
                    cbx.SelectedIndex = SelectedIndex;
                }
            }
        }

        private void SetProjectCheckbox(string PCname, int iPJ, bool SprintChecked)
        {
            foreach (CheckedProjInfo pi in cpi)
            {
                if (pi.PCname == PCname)
                {
                    pi.InSprint[iPJ] = SprintChecked;
                    return;
                }
            }
        }

        public void GetCheckedPI(string PCname, string PJname, out bool InSprint, out int ItemSelected)
        {
            InSprint = false;
            ItemSelected = 0;
            foreach (CheckedProjInfo pi in cpi)
            {
                if (pi.PCname == PCname)
                {
                    int iPJ = ProjectStats.GetNameIndex(PJname);
                    InSprint = pi.InSprint[iPJ];
                    ItemSelected = pi.SelectedIndi[iPJ];
                    return;
                }
            }
            Debug.Assert(false, "cannot find project or pc");
        }

        // return the index, not the wanted amount
        private int GetWantedWUcnt(string ProjName)
        {
            int iLoc = SelectedPC.GetProjNameIndex(ProjName);
            if (iLoc < 0)
            {
                return 0; // project not registered
            }
            iLoc = ProjectStats.GetNameIndex(ProjName);
            int MinSamples = ProjectStats.ProjectList[iLoc].MinWUsNeeded;
            return WantedToIndex(MinSamples);
        }

        private int GetWantedStudy(string ProjName)
        {
            int iLoc = ProjectStats.GetNameIndex(ProjName);
            string sStudyV = ProjectStats.ProjectList[iLoc].sStudyV;
            ProjectStats.GetIndexToStudy(ProjName, out int SelectedIndex, out int NumStudies);
            return SelectedIndex;
        }

        private void AddSelectedProject(string PCname)
        {
            CheckedProjInfo newCPI = new CheckedProjInfo();
            newCPI.Init(reqCmd.ComboxUsage, PCname, ProjectStats.ProjectList.Count);
            foreach (cCalcLimitProj clp in SelectedPC.ProjList)
            {
                int iPJ = ProjectStats.GetNameIndex(clp.ShortName);
                newCPI.InSprint[iPJ] = clp.UsedInSprint;
                int WantedIndex = -1;
                if (reqCmd.ComboxUsage == "cpugpu")
                    WantedIndex = (clp.AppType == "cpu" ? 1 : 0);
                if (reqCmd.ComboxUsage == "WUsWanted")
                    WantedIndex = GetWantedWUcnt(clp.ShortName);
                if (reqCmd.ComboxUsage == "StudyWanted")
                    WantedIndex = GetWantedStudy(clp.ShortName);
                Debug.Assert(WantedIndex >= 0, "internal error adding project");
                newCPI.SelectedIndi[iPJ] = WantedIndex;
            }
            cpi.Add(newCPI);
        }

        private int ComboWantedIndex = 1;  // should be 10 work units
        public void SetSelectComboIndex(int n)
        {
            ComboWantedIndex = n;
        }

        public class MultiChangedEventArgs : EventArgs
        {
            public string SelectedPCname { get; }
            public string SelectedPJname { get; }
            public int SelectedComboIndex { get; }

            public MultiChangedEventArgs(string selectedPCname, string selectedPJname, int selectedComboIndex)
            {
                SelectedPCname = selectedPCname;
                SelectedPJname = selectedPJname;
                SelectedComboIndex = selectedComboIndex;
            }
        }


        public event EventHandler<MultiChangedEventArgs> PCsChanged;
        private bool InitLoad = false;
        public cProjectStruct ProjectStats;
        public cManagedPCs ManagedPCs;
        public PandoraConfig pc;
        private bool IgnorePJchange = false;
        private ReqCmds reqCmd;
        public cPClimit SelectedPC = null;
        private string selectedPCname = "";
        private int selectedPCnameTag = -1;
        private string selectedProjname = "";
        private cPClimit SprintTemplete;
        public BaseMultiples()
        {
            InitializeComponent();
        }


        public BaseMultiples(ref cProjectStruct rProjectStats, ReqCmds reqcmd)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            reqCmd = reqcmd;
            ManagedPCs = ProjectStats.ManagedPCs;
            pc = new PandoraConfig(ref rProjectStats);
            if (reqcmd.ComboxUsage == "StudyWanted")
                FormProjectRB();
            else
                FormProjectCB();
            if(reqcmd.UseRadioButtons)            
                FormSystemsRB();        
            else
                FormSystemsCB();
            
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Shown += InitialLoad;
        }

        private void NotifyFormsRB(string ProjName)
        {
            if (InitLoad) return;
            if (reqCmd.ComboxUsage == "StudyWanted") return; // does not apply
            GetCheckedPI(selectedPCname, ProjName, out bool b, out int SelectedIndex);
            PCsChanged?.Invoke(this, new MultiChangedEventArgs(selectedPCname, ProjName, SelectedIndex));
        }


        private void NotifyFormsCB(string newPCname)
        {
            if (InitLoad) return;
            selectedPCname = newPCname;
            if (reqCmd.ComboxUsage == "StudyWanted")
            {
                GetCheckedPI(selectedPCname, selectedProjname, out bool b, out int SelectedIndex);
                PCsChanged?.Invoke(this, new MultiChangedEventArgs(selectedPCname, selectedProjname, SelectedIndex));
            }
            else PCsChanged?.Invoke(this, new MultiChangedEventArgs(selectedPCname, selectedProjname, -1));
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
            InitLoad = true;
            ChecksLoadPCs(); // fill in that table with data we read in earlier
            // pandora database only has about 8 projects but this form handles all projects
            ShowSprintColors();
            //GetSystemRB(0).Checked = true;
            InitLoad = false;
            GetSystemRB(0).Checked = true;
        }

        private void ChecksLoadPCs()
        {
            foreach(cPClimit PCl in pc.PandoraDatabase)
            {
                SelectedPC = PCl;
                AddSelectedProject(PCl.PCname);
            }
            SelectedPC = pc.PandoraDatabase[0];
            selectedPCname = SelectedPC.PCname;
        }


        private void ShowSprintColors()
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is CheckBox cb)
                {
                    bool bUsedInGames = ProjectStats.IsProjInGames(cb.Text);    // this looks at templetDB, not ProjectList
                    cb.ForeColor = bUsedInGames ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                    cb.Checked = bUsedInGames;
                }
            }
        }


        private void FormProjectRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bIsSprint = false;
            int MaxShortSize = 0;
            int cbxofst = (ProjectStats.LengthLongestShortname) * 8;
            for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
            {
                RadioButton rb = new RadioButton();
                ComboBox cbx = new ComboBox();
                string ProjName = ProjectStats.ProjectList[i].shortname;
                switch (reqCmd.ComboxUsage)
                {
                    case "StudyWanted":
                        cbx.Items.AddRange(ProjectStats.ProjectList[i].sStudyL.Split(' '));
                        break;
                }
                cbx.Width = 64;
                cbx.Height = 12;
                cbx.Tag = i;
                rb.Tag = i;
                rb.Text = ProjectStats.ShortName(i);

                rb.ForeColor = (ProjectStats.IsProjInGames(rb.Text) ? Color.Blue : Color.Black);
                cbx.ForeColor = rb.ForeColor;
                rb.Visible = ProjectStats.IsProjInSprint(rb.Text);
                cbx.Visible = rb.Visible;
                if(rb.Visible)
                {
                    if (selectedProjname == "")
                    {
                        selectedProjname = rb.Text;
                        rb.Checked = true;
                    }
                }
                else
                {

                }
                rb.AutoSize = true;
                rb.CheckedChanged += project_changed_rb;
                cbx.SelectedIndexChanged += project_changed_index;
                rb.Location = new System.Drawing.Point(oCol + iCol * (120 + cbxofst), oRow + iRow * 26);
                cbx.Location = new System.Drawing.Point(oCol + cbxofst + iCol * (120 + cbxofst), oRow + iRow * 26);
                gbProj.Controls.Add(rb);
                gbProj.Controls.Add(cbx);
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
            bool bIsSprint = false;
            int MaxShortSize = 0;
            int cbxofst = (ProjectStats.LengthLongestShortname)*8;
            for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
            {
                CheckBox cb = new CheckBox();
                ComboBox cbx = new ComboBox();
                cbx.Width = 64;
                cbx.Height = 12;
                cbx.Tag = i;
                cb.Tag = i;
                cb.Text = ProjectStats.ShortName(i);

                switch (reqCmd.ComboxUsage)
                {
                    case "cpugpu":
                            cbx.Items.AddRange(new string[] { "gpu", "cpu" });
                            break;
                    case "WUsWanted":
                            cbx.Items.AddRange(ComboChoices.Select(c => c.ToString()).ToArray());
                            cb.Visible = ProjectStats.IsProjInGames(cb.Text);
                            cbx.Visible = cb.Visible;
                        break;
                }


                cb.ForeColor = (ProjectStats.IsProjInGames(cb.Text) ? Color.Blue : Color.Black);
                cbx.ForeColor = cb.ForeColor;
                cb.AutoSize = true;                
                cb.Checked = ProjectStats.IsProjInSprint(cb.Text);
                cb.CheckedChanged += project_changed_cb;
                cbx.SelectedIndexChanged += project_changed_index;
                cb.Location = new System.Drawing.Point(oCol + iCol * (120 + cbxofst), oRow + iRow * 26);
                cbx.Location = new System.Drawing.Point(oCol + cbxofst + iCol * (120 + cbxofst), oRow + iRow * 26);
                gbProj.Controls.Add(cb);
                gbProj.Controls.Add(cbx);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }

        

        //combobox changed
        private void project_changed_index(object sender, EventArgs e)
        {
            if (IgnorePJchange || InitLoad) return;
            ComboBox cbx = (ComboBox)sender;
            int iPJ = (int)cbx.Tag;
            string ProjName = ProjectStats.ShortName(iPJ);
            SetProjectCombobox(selectedPCname, iPJ, cbx.SelectedIndex);
            NotifyFormsRB(ProjName);
        }


        private void project_changed_rb(object sender, EventArgs e)
        {
            if (IgnorePJchange || InitLoad) return;
            RadioButton rb = (RadioButton)sender;
            int iPJ = (int)rb.Tag;
            if (!rb.Checked) return;
            ComboBox cbx = GetProjCBX(iPJ);
            string ProjName = ProjectStats.ShortName(iPJ);
            SetProjectCombobox(selectedPCname, iPJ, cbx.SelectedIndex);
            selectedProjname = ProjName;
            NotifyFormsRB(ProjName);
        }

        private void project_changed_cb(object sender, EventArgs e)
        {
            if (IgnorePJchange || InitLoad) return;
            CheckBox cb = (CheckBox)sender;
            int iPJ = (int)cb.Tag;
            SetProjectCheckbox(selectedPCname, iPJ, cb.Checked);

        }


        private void SetProjectCombobox(string PCname, int iPJ, int SelectedIndex)
        {
            foreach (CheckedProjInfo pi in cpi)
            {
                if (pi.PCname == PCname)
                {
                    if (SelectedIndex >= 0)
                        pi.SelectedIndi[iPJ] = SelectedIndex;
                    return;
                }
            }
            Debug.Assert(false, "internal error cannot find pc name with project # " + iPJ);
        }


        // this is not used yet
        private void FormSystemsCB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;

            for (int i = 0; i < ProjectStats.PandoraDatabase.Count; i++)
            {
                cPClimit p = ProjectStats.PandoraDatabase[i];
                CheckBox cb = new CheckBox();
                cb.Tag = i;
                cb.Checked = p.IsSelected;
                cb.Text = p.PCname;
                MaxShortSize = Math.Max(MaxShortSize, cb.Text.Length);
                cb.AutoSize = true;
                cb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                cb.CheckedChanged += system_changed_cb ;
                gbPCs.Controls.Add(cb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }


        //pc system changed radio button
        private void FormSystemsRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;
            int iOffset = 0;

            for (int i = 0; i < ProjectStats.PandoraDatabase.Count; i++)
            {
                cPClimit p = ProjectStats.PandoraDatabase[i];
                RadioButton rb = new RadioButton();
                rb.Tag = i + iOffset;
                rb.Text = p.PCname;
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.CheckedChanged += system_changed_rb ;
                gbPCs.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }
        private void system_changed_cb(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cPClimit p = ProjectStats.PandoraDatabase[(int)cb.Tag];
            p.IsSelected = cb.Checked;
            //pc.WriteDBrecord(ref p);
            //ShowSelectedSprintProjects();
        }


        //pc system changed radio button
        private void system_changed_rb(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked) return;
            string PCname = rb.Text;
            int iTag = (int)rb.Tag;
            SelectedPC = pc.GetPCbyName(PCname);
            NotifyFormsCB(rb.Text);
            selectedPCnameTag = iTag;
            EnableSubscribed(PCname);
            for (int iPJ = 0; iPJ < ProjectStats.ProjectList.Count; iPJ++)
            {
                string ProjName = ProjectStats.ShortName(iPJ);
                GetCheckedPI(SelectedPC.PCname, ProjName, out bool InSprint, out int SelectedIndex);
                if (reqCmd.ComboxUsage == "StudyWanted")
                {
                    GetProjRB(iPJ).Visible = InSprint || ProjectStats.IsProjInGames(ProjName);
                    GetProjCBX(iPJ).Visible = InSprint || ProjectStats.IsProjInGames(ProjName);
                    GetProjCBX(iPJ).Enabled = InSprint;
                    GetProjRB(iPJ).Enabled = InSprint;
                }
                else if(reqCmd.ComboxUsage == "WUsWanted")
                {
                    GetProjCB(iPJ).Visible= InSprint || ProjectStats.IsProjInGames(ProjName);
                    GetProjCBX(iPJ).Visible = InSprint || ProjectStats.IsProjInGames(ProjName);
                    GetProjCBX(iPJ).Visible = InSprint || ProjectStats.IsProjInGames(ProjName);
                    GetProjCBX(iPJ).Enabled = InSprint;
                    GetProjCB(iPJ).Checked = InSprint;
                }
                else
                {
                    GetProjCB(iPJ).Checked = InSprint;
                }
                GetProjCBX(iPJ).SelectedIndex = SelectedIndex;
            }
        }


        private int WantedToIndex(int nWUs)
        {
            for (int i = 0; i < ComboChoices.Length; i++)
            {
                if (ComboChoices[i] == nWUs)
                {
                    return i;
                }
            }
            return 2;
        }

        private int CpuGpuToIndex(string app_type)
        {
            if (app_type == "gpu") return 0;
            return 1;
        }

        private bool EnableCheckboxes(string ProjName)
        {
            bool b = false;
            cHostInfo hi = ManagedPCs.NameToSystem(SelectedPC.PCname);
            cLHe lh = hi.GetLocalProjectInfo(ProjName);
            switch (reqCmd.ComboxUsage)
            {
                case "cpugpu":
                bool OnlyCpu = ProjectStats.IsCPUonly(ProjName);
                return (lh != null && !OnlyCpu);
                break;
                case "WUsWanted":
                b = lh != null; // not subscribed
                break;
                case "StudyWanted":
                    return true;
            }
            return b;
        }


        private void EnableSubscribed(string PCname)
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is CheckBox cb)
                {
                    //cb.Enabled = EnableCheckboxes(cb.Text);
                }
                if (c is ComboBox cbx)
                {
                    string ProjName = ProjectStats.ShortName((int)cbx.Tag);
                    cbx.Enabled = EnableCheckboxes(ProjName);
                }
            }
        }


        public RadioButton GetSystemRB(int iTag)
        {
            foreach(Control c in gbPCs.Controls)
            {
                if(c is  RadioButton rb && ((int)rb.Tag == iTag))
                {
                    return rb;
                }
            }
            return null;
        }

        public CheckBox GetProjCB(int iTag)
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is CheckBox cb && ((int)cb.Tag == iTag))
                {
                    return cb;
                }
            }
            return null;
        }

        public RadioButton GetProjRB(int iTag)
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is RadioButton rb && ((int)rb.Tag == iTag))
                {
                    return rb;
                }
            }
            return null;
        }



        public ComboBox GetProjCBX(int iTag)
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is ComboBox cbx && ((int)cbx.Tag == iTag))
                {
                    return cbx;
                }
            }
            return null;
        }


        private void ChangeCKbox(object sender, EventArgs e)
        {
            string sOP = "";
            Button btn = sender as Button;
            if (btn != null)
            {
                sOP = btn.Text;
                Control parent = btn.Parent;
                while (parent != null && !(parent is GroupBox))
                {
                    parent = parent.Parent;
                }

                if (parent is GroupBox groupBox)
                {
                    foreach (Control c in groupBox.Controls)
                    {
                        if (c is CheckBox)
                        {
                            CheckBox cb = (CheckBox)c;
                            globals.cPSlist p = ProjectStats.ProjectList[(int)cb.Tag];
                            switch (sOP)
                            {
                                case "Check all":
                                    cb.Checked = true;
                                    p.IsSelected = true;
                                    break;
                                case "Clear all":
                                    cb.Checked = false;
                                    p.IsSelected = false;
                                    break;
                                case "Invert":
                                    cb.Checked = !cb.Checked;
                                    p.IsSelected = cb.Checked;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void AssignKnownSprints()
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is CheckBox cb)
                { 
                    cb.Checked = ProjectStats.IsProjInSprint(cb.Text);
                }
            }
        }

        private void btnSelSprint_Click(object sender, EventArgs e)
        {
            AssignKnownSprints();
        }

        public void SaveChanges()
        {
            foreach (cPClimit PCl in pc.PandoraDatabase)
            {
                SelectedPC = PCl;
                foreach (cCalcLimitProj clP in PCl.ProjList) clP.ProjectDisabled = true;

                for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
                {
                    cPSlist PSl = ProjectStats.ProjectList[i];

                    string ProjName = PSl.shortname;
                    cCalcLimitProj newclP = PCl.GetProjStruct(ProjName);
                    GetCheckedPI(PCl.PCname, ProjName, out bool IsSprint, out int ItemSelected);
                    string AppWanted = (ItemSelected == 1 ? "cpu" : "gpu");

                    if (IsSprint)
                    {
                        if (newclP != null)
                        {
                            newclP.AppType = AppWanted;
                            newclP.ProjectDisabled = false;
                            newclP.UsedInSprint = true;
                        }
                        else
                        {
                            newclP = new cCalcLimitProj();
                            newclP.init(PSl.sURL, false);
                            newclP.BunkerStart = "-5";
                            newclP.UsedInSprint = true;
                            newclP.AppType = AppWanted;
                            PCl.ProjList.Add(newclP);
                        }
                    }
                    else
                    {
                        if (newclP != null)
                        {
                            newclP.UsedInSprint = false;
                            newclP.UsedInGames = false;
                        }
                    }
                }
                pc.WriteDBrecord(ref SelectedPC);
            }
        }

    }
}
