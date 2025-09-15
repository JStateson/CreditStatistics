using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;


namespace CreditStatistics
{
    internal partial class SprintEdit : Form
    {
        cProjectStruct ProjectStats;
        cManagedPCs ManagedPCs;
        PandoraConfig pc;
        private List<cPClimit> PandoraDatabase;
        private List<cPClimit> CopyDatabase = new List<cPClimit>();
        string sBackup = "";
        cPClimit TempletDB;// = new cPClimit();
        string sOperation;
        int CBstatus = 0;
        int CBchange = 0;
        string lbPCselectedItem = "";
        int lbPCselectedIndex = 0;

        public SprintEdit(ref cProjectStruct rProjectStats, string rOperation)
        {
            InitializeComponent();
            pc = new PandoraConfig(ref rProjectStats);
            ProjectStats = rProjectStats;
            TempletDB = ProjectStats.TempletDB;
            PandoraDatabase = ProjectStats.PandoraDatabase;
            ManagedPCs = ProjectStats.ManagedPCs;
            sOperation = rOperation;
            InitSprintConfig();
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

        private void InitSprintConfig()
        {
            CopyDatabase.Clear();
            lbPCs.Items.Clear();
            for (int i = 0; i < PandoraDatabase.Count; i++)
            {
                cPClimit pcT = PandoraDatabase[i];
                cPClimit pcL = new cPClimit();
                pcL.DeepCopy(ref pcT);
                CopyDatabase.Add(pcL);
            }
            sBackup = ProjectStats.current_default_database_config;
            TempletDB = pc.ParsePandoraConfig(sBackup, "templet");
            tbEdit.Text = TempletDB.CreateTemplet();
            lbPCs.Items.Add("Default");
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                lbPCs.Items.Add(hi.ComputerID);
            }
            lbPCs.SelectedIndex = 0;
            SetDefaultProjectCB();
        }

        private void InitialLoad(object sender, EventArgs e)
        {
            gbList.Text = sOperation;
            switch (sOperation)
            {
                case "AssignProjects":
                    FormProjectCB();
                    break;
                case "AssignSystems":
                    FormSystemsCB();
                    break;
            }
            tbEdit.SelectionStart = tbEdit.TextLength;
            tbEdit.SelectionLength = 0;
        }

        private void FormSystemsCB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            int MaxShortSize = 0;

            for (int i = 0; i < ManagedPCs.LocalSystems.Count(); i++)
            {
                CheckBox rb = new CheckBox();
                rb.Tag = i;
                rb.Text = ManagedPCs.LocalSystems[i].ComputerID;
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                gbList.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }


        private bool IsProjectChecked(string projectName)
        {
            foreach (Control c in gbList.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    if (cb.Text == projectName && cb.Checked)
                        return true;
                }
            }
            return false;
        }

        private void SetDefaultProjectCB()
        {
            bool bIsSprint = false;
            int i = 0;

            foreach (Control c in gbList.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    string ProjName = cb.Text;
                    int nLoc = TempletDB.NameToIndex(ProjName);
                    bIsSprint = TempletDB.IsInSprint(ProjName);
                    bool bIsInGames = TempletDB.IsInGames(ProjName);
                    cb.ForeColor = bIsInGames ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                    cb.Checked = bIsSprint;
                    i++;
                }
            }
        }

        private void FormProjectCB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bIsSprint = false;
            int MaxShortSize = 0;

            for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
            {
                CheckBox rb = new CheckBox();
                rb.Tag = i;
                rb.Text = ProjectStats.ShortName(i);
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                bIsSprint = ProjectStats.ProjectList[i].UsedInSprint;
                rb.ForeColor = bIsSprint ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                rb.Checked = bIsSprint;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                rb.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rbProject_mouseclick);
                gbList.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }

        private int CalcChange()
        {
            CBchange = 0;
            int i = 1;
            foreach (Control c in gbList.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    if (cb.Checked)
                        CBchange |= i;
                    i = i << 1;
                }
            }
            return CBchange;
        }

        private void rbProject_mouseclick(object sender, MouseEventArgs e)
        {
            CalcChange();
            btnApplyAbove.Enabled = (CBchange != CBstatus);
        }

        private void UncheckAll()
        {
            foreach (Control c in gbList.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    cb.Checked = false;
                }
            }
        }

        private void CheckProj(string shortName)
        {
            foreach (Control c in gbList.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    if (cb.Text.ToString() == shortName)
                    {
                        cb.Checked = true;
                        return;
                    }
                }
            }
        }

        private void rbProject_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            bool bUseInSprint = cb.Checked;
            int iLoc = (int)cb.Tag;
            string shortName = cb.Text;
            int wLoc = TempletDB.GetProjNameIndex(shortName);
            if (bUseInSprint)
            {
                if (wLoc < 0) // project is missing
                {
                    cCalcLimitProj clp = new cCalcLimitProj();
                    clp.ShortName = shortName;
                    clp.sPCname = TempletDB.PCname;
                    clp.ProjectDisabled = false;
                    ProjectStats.AddProjInfoToSprintTemplet(ref clp);
                    TempletDB.ProjList.Add(clp);
                }
                else
                {
                    TempletDB.ProjList[wLoc].ProjectDisabled = false; // possible it was disabled
                }
            }
            else
            {
                if (wLoc >= 0)
                {
                    TempletDB.ProjList[wLoc].ProjectDisabled = true;
                }
                //else Debug.Assert(false, "Project missing from templet");
            }
            string sTemp = TempletDB.CreatePDstream(out string sFile);
            TempletDB = pc.ParsePandoraConfig(sTemp, "templet");
            tbEdit.Text = TempletDB.CreateTemplet();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            InitSprintConfig();
        }

        private void UpdateDefaultTemplet()
        {
            ProjectStats.TempletDB.DeepCopy(ref TempletDB);
            ProjectStats.current_default_database_config = tbEdit.Text;
            for (int i = 0; i < ProjectStats.ProjectList.Count(); i++)
            {
                string shortname = ProjectStats.ShortName(i);
                ProjectStats.ProjectList[i].UsedInSprint = IsProjectChecked(shortname);
            }
            foreach (cCalcLimitProj clp in TempletDB.ProjList)
            {
                int iLoc = ProjectStats.GetNameIndex(clp.ShortName);
                clp.UsedInSprint = IsProjectChecked(clp.ShortName);
            }
            string sTemplet = TempletDB.CreateTemplet();
            ProjectStats.current_default_database_config = sTemplet.Trim();
            globals.WriteDefaultDatabaseConfig(sTemplet);
            for (int i = 0; i < CopyDatabase.Count; i++)
            {
                cPClimit pcDB = PandoraDatabase[i];
                cPClimit pcC = CopyDatabase[i];
                pcDB.DeepCopy(ref pcC);
                foreach(cCalcLimitProj clp in pcDB.ProjList)
                {
                    int iLoc = ProjectStats.GetNameIndex(clp.ShortName);
                    clp.UsedInSprint = ProjectStats.ProjectList[iLoc].UsedInSprint;
                }
                pc.WriteDBrecord(ref pcDB);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateDefaultTemplet();
            this.Close();
        }

        private void btnApplyAll_Click(object sender, EventArgs e)
        {
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                string PCname = hi.ComputerID;
                int nLoc = pc.FindPCnameIndex(PCname);
                if(nLoc == -1)
                {
                    pc.UpdateSystemInDB(hi, ref TempletDB);
                }
                else CopyDatabase[nLoc].UpdateFromTemplet(ref TempletDB.ProjList, PCname);
            }
        }

        private void EnableSubscribed(string PCname)
        {
            foreach (Control c in gbList.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    if(PCname == "Default")
                    {
                        cb.Enabled = true;
                    }
                    else
                    {
                        cHostInfo hi = ManagedPCs.NameToSystem(PCname);
                        cLHe lh = hi.GetLocalProjectInfo(cb.Text);
                        cb.Enabled = lh != null;
                    }
                }
            }
        }

        private void lbPCs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = lbPCs.SelectedItem;
            lbPCselectedIndex = lbPCs.SelectedIndex;
            if (selectedItem != null && lbPCselectedIndex >= 0)
            {
                btnApplyAbove.Enabled = lbPCselectedIndex > 0;
                btnApplyAll.Enabled = !btnApplyAbove.Enabled;
                lbPCselectedItem = selectedItem.ToString();
                EnableSubscribed(lbPCselectedItem);
                if (lbPCselectedIndex > 0)
                {
                    int i = 1;
                    CBstatus = 0;
                    UncheckAll();
                    string PCname = selectedItem.ToString();
                    int nLoc = pc.FindPCnameIndex(PCname);
                    foreach (cCalcLimitProj clP in CopyDatabase[nLoc].ProjList)
                    {
                        if (!clP.ProjectDisabled)
                        {
                            CBstatus |= i;
                            CheckProj(clP.ShortName);
                        }
                        i = i << 1;
                    }
                }
                else
                {
                    TempletDB = pc.ParsePandoraConfig(sBackup, "templet");
                    tbEdit.Text = TempletDB.CreateTemplet();
                    SetDefaultProjectCB();
                    CBstatus = CalcChange();
                }
                btnApplyAbove.Enabled = false;
            }
        }

        private void btnApplyAbove_Click(object sender, EventArgs e)
        {
            if (lbPCselectedIndex == 0)
            {
                sBackup = TempletDB.CreateTemplet();
            }
            else
            {
                int nLoc = pc.FindPCnameIndex(lbPCselectedItem);
                foreach(cCalcLimitProj clP in  CopyDatabase[nLoc].ProjList)
                    clP.ProjectDisabled = true;

                for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
                {
                    string sn = ProjectStats.ProjectList[i].shortname;
                    bool bIsChecked = IsProjectChecked(sn);
                    int iLoc = CopyDatabase[nLoc].NameToIndex(sn);
                    if (iLoc < 0 && bIsChecked)
                    {
                        cCalcLimitProj newclP = new cCalcLimitProj();
                        cPSlist PSl = ProjectStats.GetProjectByShortName(sn);
                        newclP.init(PSl.sURL, false);
                        newclP.sPCname = lbPCselectedItem;
                        CopyDatabase[nLoc].ProjList.Add(newclP);
                    }
                    if(iLoc >= 0 && !bIsChecked)
                        CopyDatabase[nLoc].ProjList[iLoc].ProjectDisabled = true;
                }
            }
        }
    }
}
