using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.globals.cAllProjectStudyInfo;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class AssignStudies : BaseMultiples
    {


        private cPClimit TempletDB;
        private cPClimit DefaultTempletDB = new cPClimit();
        private cProjectStudiesInfo SelectedEdit_psi;
        private List<cPClimit> localPandoraDatabase = new List<cPClimit>();
        private string templetName = "";
        private ReqCmds reqCmd;
        private bool OnInitLoad = true;
        private string InitialProject = "";
        private int InitialStudyIndex = -1;

        public AssignStudies()
        {
            InitializeComponent();
        }

        public void project_changed(object sender, MultiChangedEventArgs e)
        {
            if(OnInitLoad)
            {
                InitialProject = e.SelectedPJname;
                InitialStudyIndex = e.SelectedComboIndex;
                OnInitLoad = false;
                return;
            }
            ProjectChanged(e.SelectedPCname, e.SelectedPJname, e.SelectedComboIndex);
        }

        private string LastProject = "";
        private void ProjectChanged(string PCname, string ShortName, int SelectedStudyIndex)
        {
            tbProjName.Text = ShortName;
            if (LastProject != ShortName)
                FillTable(ShortName);
            LastProject = ShortName;
            dgvStudyInfo.Rows[SelectedStudyIndex].Selected = true;
            RowSelected(SelectedStudyIndex);
            cPClimit PCl = pc.GetPCbyName(PCname);
            bool InSprint = PCl.IsInSprint(ShortName);
            gpEdit.Enabled = InSprint;
        }


        public AssignStudies(ref cProjectStruct rProjectStats, ReqCmds reqcmd) : base(ref rProjectStats, reqcmd)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            DefaultTempletDB.DeepCopy(ref ProjectStats.TempletDB);
            TempletDB = DefaultTempletDB;
            foreach (cPClimit pcL in pc.PandoraDatabase)
            {
                cPClimit newpc = new cPClimit();
                cPClimit npcL = pcL;
                newpc.DeepCopy(ref npcL);
                localPandoraDatabase.Add(newpc);
            }
            this.PCsChanged += project_changed;
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Shown += InitialLoad;
            reqCmd = reqcmd;
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
            FillTable(InitialProject);
            RowSelected(InitialStudyIndex);
        }


        private void FillTable(string sName)
        {
            dgvStudyInfo.Rows.Clear();
            SelectedEdit_psi = ProjectStats.ProjectStudyDB.GetStudyElements(sName);
            if (SelectedEdit_psi == null) return;
            foreach (cStudyIDsDue SID in SelectedEdit_psi.StudyIDsDue)
            {
                dgvStudyInfo.Rows.Add(SID.sStudy, SID.nDueDuration, SID.sStudyName, SID.CPUsUsed, SID.GPUsUsed);
            }
        }

        private void RowSelected(int irow)
        {
            DataGridViewRow sRow = dgvStudyInfo.SelectedRows[0];
            if (sRow != null)
            {
                string sStudy = sRow.Cells[0].Value.ToString();
                tbSelStudy.Text = sStudy;
                tbSelStudy.Tag = sRow.Cells[2].Value.ToString();
                string sDays = sRow.Cells[1].Value.ToString();
                tbSelectDays.Text = sDays;
                //string shortName = tbProjName.Text;
                //int iLoc = TempletDB.NameToIndex(shortName);
                //int nLoc = ProjectStats.GetNameIndex(shortName);
                //string sProjID = ManagedPCs.ProjectIDfromPCsShortname(shortName);
                //TempletDB.ProjList[iLoc].AcqUrl = ProjectStats.ProjectList[nLoc].FormAcqUrl(sProjID, sStudy);
            }
        }

        private void dgvStudyInfo_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            DataGridViewRow sRow = dgv.Rows[e.RowIndex];
            if (sRow != null)
            {
                string sStudy = sRow.Cells[0].Value.ToString();
                tbSelStudy.Text = sStudy;
                tbSelStudy.Tag = sRow.Cells[2].Value.ToString();
                string sDays = sRow.Cells[1].Value.ToString();
                tbSelectDays.Text = sDays;
                string shortName = tbProjName.Text;
                int iLoc = TempletDB.NameToIndex(shortName);
                int nLoc = ProjectStats.GetNameIndex(shortName);
                string sProjID = ManagedPCs.ProjectIDfromPCsShortname(shortName);
                TempletDB.ProjList[iLoc].AcqUrl = ProjectStats.ProjectList[nLoc].FormAcqUrl(sProjID, sStudy);
            }
        }

        private bool TryApplyChanges(bool bAll)
        {
            string sTemplet = DefaultTempletDB.CreateTemplet();
            BigMessageBox bmb = new BigMessageBox(sTemplet, ref localPandoraDatabase);
            bmb.ShowDialog();
            bool bChanged = bmb.PC_Changed;
            bool bApproved = bmb.PC_Approved;
            if (bChanged)
            {
                string sEdited = bmb.EditedPandoraConfig;
                PandoraConfig pc = new PandoraConfig(ref ProjectStats);
                DefaultTempletDB = pc.ParsePandoraConfig(sEdited, templetName);
                string sTemp = TempletDB.CreateTemplet();
                if (bApproved)
                    sTemplet = sTemp;
            }
            bmb.Dispose();
            if (bApproved)
            {
                ProjectStats.TempletDB.DeepCopy(ref DefaultTempletDB);
                foreach (cCalcLimitProj clP in DefaultTempletDB.ProjList)
                {
                    cProjectStudiesInfo se_psi = ProjectStats.ProjectStudyDB.GetStudyElements(clP.ShortName);
                    int bunkerStart = Convert.ToInt32(clP.BunkerStart);
                    se_psi.UpdateStudy(clP.sStudy, clP.sStudy, clP.fullStudyName, bunkerStart, -1.0, -1.0);
                }

                ProjectStats.ProjectStudyDB.SaveStudyFile(); // updates study database
                int index = 0;
                foreach (cPClimit db in pc.PandoraDatabase)
                {
                    cPClimit updatedDB = localPandoraDatabase[index];
                    pc.WriteDBrecord(ref updatedDB);
                    db.DeepCopy(ref updatedDB);
                    index++;
                }
                return true;
            }
            else return false;
        }

        private void dgvStudyInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvStudyInfo.Rows[e.RowIndex];
                int iLoc = TempletDB.NameToIndex(tbProjName.Text);
                string sUrl = TempletDB.ProjList[iLoc].AcqUrl;
                globals.OpenUrl(sUrl);
            }
        }


    }
}
