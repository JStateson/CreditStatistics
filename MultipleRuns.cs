using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class MultipleRuns : Form
    {
        cProjectStruct ProjectStats;
        cManagedPCs ManagedPCs;
        PandoraConfig pc;

        public MultipleRuns(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            ManagedPCs = ProjectStats.ManagedPCs;
            pc = new PandoraConfig(ref rProjectStats);
            FormProjectCB();
            FormSystemsCB();
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
            ShowSelectedSprintProjects();
        }

        private void ShowSelectedSprintProjects()
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    {
                        cPClimit p = pc.ParsePandoraConfig(ProjectStats.current_default_database_config, "templet");
                        bool bIsSprint = p.IsInSprint(cb.Text);
                        bool bUsedInGames = ProjectStats.IsProjInGames(cb.Text);
                        ProjectStats.ProjectList[(int)cb.Tag].IsSelected = bIsSprint;
                        cb.ForeColor = bUsedInGames ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                    }
                }
            }
        }

        private void ShowDefaultSprint()
        {
            int i = 0;
            foreach (Control c in gbProj.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    {
                        bool bIsSprint = ProjectStats.ProjectList[i].UsedInSprint;
                        ProjectStats.ProjectList[i].IsSelected = bIsSprint;
                        cb.ForeColor = bIsSprint ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                    }
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
                bIsSprint = ProjectStats.IsProjInSprint(rb.Text);
                bool bUsedInGames = ProjectStats.IsProjInGames(rb.Text);
                ProjectStats.ProjectList[i].UsedInSprint = bIsSprint;
                rb.Checked = bIsSprint;
                rb.ForeColor = bUsedInGames ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                gbProj.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }
        private void FormSystemsCB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;

            for (int i = 0; i < ProjectStats.PandoraDatabase.Count; i++)
            {
                cPClimit p = ProjectStats.PandoraDatabase[i];
                CheckBox rb = new CheckBox();
                rb.Tag = i;
                rb.Checked = p.IsSelected;
                rb.Text = p.PCname;
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.CheckedChanged += ChangedPC;
                gbPCs.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }

        private void ChangedPC(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cPClimit p = ProjectStats.PandoraDatabase[(int)cb.Tag];
            p.IsSelected = cb.Checked;
            //pc.WriteDBrecord(ref p);
            //ShowSelectedSprintProjects();
        }

        private void ChangeCKbox(object sender, EventArgs e)
        {
            string sOP = "";
            Button btn = sender as Button;
            if (btn != null)
            {
                sOP = btn.Text;
                cPClimit p;
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
                            p = ProjectStats.PandoraDatabase[(int)cb.Tag];
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
                                    cb.Checked = !@cb.Checked;
                                    p.IsSelected = cb.Checked;
                                    break;
                            }
                        }
                    }
                }
            }
        }



        private void btnAssignProj_Click(object sender, EventArgs e)
        {
            SprintEdit se = new SprintEdit(ref ProjectStats, "AssignProjects");
            se.ShowDialog();
            se.Dispose();

        }

        private void btnAssignStudy_Click(object sender, EventArgs e)
        {
            SprintStudy ss = new SprintStudy(ref ProjectStats);
            ss.ShowDialog();
            ss.Dispose();
        }

        private void btnSetBGsprint_Click(object sender, EventArgs e)
        {
            bool AddedProj = false;
            bool RemovedProject = false;
            int iPC = -1;
            foreach (cPClimit cp in pc.PandoraDatabase)
                cp.IsSelected = false;

            foreach (Control c in gbPCs.Controls)
            {
                if (c is CheckBox)
                {
                    iPC++;
                    CheckBox cb = (CheckBox)c;
                    cPClimit p = pc.PandoraDatabase[iPC];
                    p.IsSelected = cb.Checked;
                    if (cb.Checked)
                    {
                        foreach (cCalcLimitProj clp in p.ProjList)
                            clp.UsedInSprint = false;
                        foreach (cCalcLimitProj clp in ProjectStats.TempletDB.ProjList)
                            clp.UsedInSprint = false;

                        foreach (Control cc in gbProj.Controls)
                        {
                            if (cc is CheckBox)
                            {
                                CheckBox ccb = (CheckBox)cc;
                                if (ccb.Checked)
                                {
                                    cCalcLimitProj clp = p.GetProjStruct(ccb.Text);
                                    if (clp == null)
                                    {
                                        int iLoc = ProjectStats.ShortnameToIndex(ccb.Text);
                                        string masterUrl = ProjectStats.ProjectList[iLoc].MasterUrl;
                                        string sStudyV = ProjectStats.ProjectList[iLoc].sStudyV;
                                        p.AddProject(p.PCname, ccb.Text, masterUrl, sStudyV);
                                        clp = p.ProjList.Last();
                                        tbInfo.Text = "project " + ccb.Text + " was added. Verify the study id " + sStudyV + " is correct" + Environment.NewLine;
                                    }
                                    clp.UsedInSprint = true;

                                    clp = ProjectStats.TempletDB.GetProjStruct(ccb.Text);
                                    if (clp == null)
                                    {
                                        int iLoc = ProjectStats.ShortnameToIndex(ccb.Text);
                                        string masterUrl = ProjectStats.ProjectList[iLoc].MasterUrl;
                                        string sStudyV = ProjectStats.ProjectList[iLoc].sStudyV;
                                        ProjectStats.TempletDB.AddProject(p.PCname, ccb.Text, masterUrl, sStudyV);
                                        clp = ProjectStats.TempletDB.ProjList.Last();
                                        AddedProj = true;
                                    }
                                    clp.UsedInSprint = true;
                                }
                            }
                        }
                        foreach (cCalcLimitProj clp in ProjectStats.TempletDB.ProjList)
                        {
                            if (!clp.UsedInSprint)
                            {
                                tbInfo.Text += clp.ShortName + " not used in " + p.PCname + Environment.NewLine;
                                RemovedProject = true;
                            }
                        }
                        if (AddedProj || RemovedProject)
                        {
                            string sTemplet = p.CreateTemplet();
                            ProjectStats.current_default_database_config = sTemplet.Trim();
                            globals.WriteDefaultDatabaseConfig(sTemplet);
                        }
                    }
                    pc.WriteDBrecord(ref p);
                }
            }
        }

        private void btnSelSprint_Click(object sender, EventArgs e)
        {
            foreach(Control c in gbProj.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    bool bIsInGames = ProjectStats.IsProjInGames(cb.Text);
                    cb.Checked = bIsInGames;
                    ProjectStats.TempletDB.SetProjectInSprint(cb.Text, bIsInGames);
                    ProjectStats.TempletDB.SetProjectInSprint(cb.Text, bIsInGames);
                }
            }
        }
    }
}
