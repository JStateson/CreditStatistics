using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class AssignCpuGpu : BaseMultiples
    {

        public AssignCpuGpu()
        {
            InitializeComponent();
        }

        private ReqCmds reqCmd;

        public AssignCpuGpu(ref cProjectStruct rProjectStats, ReqCmds reqcmd) : base(ref rProjectStats, reqcmd)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            reqCmd = reqcmd;
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

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void btnAssignProj_Click_1(object sender, EventArgs e)
        {

        }
    }
}
