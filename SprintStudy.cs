using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.globals.cAllProjectStudyInfo;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{

    internal partial class SprintStudy : Form
    {
        private cProjectStruct ProjectStats;
        private cManagedPCs ManagedPCs;
        private cPClimit TempletDB;
        private cPClimit DefaultTempletDB = new cPClimit();
        private cProjectStudiesInfo SelectedEdit_psi;
        private List<cPClimit> localPandoraDatabase = new List<cPClimit>();
        private PandoraConfig pc;
        private string templetName = "";

        public SprintStudy(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            ManagedPCs = ProjectStats.ManagedPCs;
            pc = new PandoraConfig(ref ProjectStats);
            DefaultTempletDB.DeepCopy(ref ProjectStats.TempletDB);
            TempletDB = DefaultTempletDB;
            foreach(cPClimit pcL in pc.PandoraDatabase)
            {
                cPClimit newpc = new cPClimit();
                cPClimit npcL = pcL;
                newpc.DeepCopy(ref npcL);
                localPandoraDatabase.Add(newpc);
            }            
            FormProjectRB();
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
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                lbPCs.Items.Add(hi.ComputerID);
            }
            lbPCs.SelectedIndex = 0;            
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

        private void FormProjectRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;

            for (int i = 0; i < TempletDB.ProjList.Count; i++)
            {
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = TempletDB.ProjList[i].ShortName;
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                gbList.Controls.Add(rb);
                iRow++;
                if (iRow > 10)
                {
                    iRow = 0;
                    iCol++;
                }
                if (i == 0) rb.Checked = true;
            }
        }
        private void rbProject_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked) return;
            tbProjName.Text = rb.Text;
            FillTable(rb.Text);
            GetSelectedStudy(rb.Text);
        }

        private int GetSelectedStudy(string shortName)
        {
            int n = 0;
            int iLoc = TempletDB.NameToIndex(shortName);
            string sStudy = TempletDB.ProjList[iLoc].sStudy;
            foreach (DataGridViewRow row in dgvStudyInfo.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells[0].Value.ToString() == sStudy)
                {
                    row.Selected = true;
                    return n;
                }
                n++;
            }
            return n;
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


        private void btnSave_Click(object sender, EventArgs e)
        {
            if(TryApplyChanges(false))
                this.Close();
            return;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            string sStudy = tbSelStudy.Text;
            string sDays = tbSelectDays.Text;
            string shortName = tbProjName.Text;
            int iLoc = TempletDB.NameToIndex(shortName);
            TempletDB.ProjList[iLoc].sStudy = sStudy;
            TempletDB.ProjList[iLoc].fullStudyName = (string) tbSelStudy.Tag;
            TempletDB.ProjList[iLoc].BunkerStart = sDays;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void lbPCs_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iLoc = lbPCs.SelectedIndex;
            templetName = lbPCs.Text.ToString();
            if (iLoc > 0)
            {
                ManagedPCs.SelectCurrentFromIndex(iLoc - 1);
                TempletDB = localPandoraDatabase[iLoc - 1];
            }
            else TempletDB = DefaultTempletDB;
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


        private void btnApplyAll_Click(object sender, EventArgs e)
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
                string sTemp = DefaultTempletDB.CreateTemplet();
                if (bApproved)
                    sTemplet = sTemp;
            }
            bmb.Dispose();
            if (bApproved)
            {
                ProjectStats.TempletDB.DeepCopy(ref DefaultTempletDB);
                string jystodo = sTemplet;
                foreach (cPClimit cpL in pc.PandoraDatabase)
                {
                    cpL.UpdateFromTemplet(ref DefaultTempletDB.ProjList, cpL.PCname);
                    cPClimit ncpL = cpL;
                    pc.WriteDBrecord(ref ncpL);
                }
                this.Close();
            }
        }
    }
}