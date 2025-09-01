using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.globals;
using static CreditStatistics.globals.cAllProjectStudyInfo;

/*
 *All (xxx) if null else
 ;appid=17">Beta of DENIS-fiber</a> (0)
State: All (1018)
*/

namespace CreditStatistics
{
    internal partial class GetStudies : Form
    {
        private List<cProjectStudiesInfo> psi;
        private cProjectStruct ProjectStats;
        private string sUrl = "";
        private string sRawPage = "";
        private int iLoc = 0;
        private string shortName = "";
        private string projID = "";
        private int inxProj = 0;
        private string sResult = "";
        private bool bDone = false;
        public CancellationTokenSource cts;
        private int TimeOut = 20000; // 20 seconds timeout for the task
        private cProjectStudiesInfo SelectedEdit_psi = null;
        private ReadSitePage readSitePage;
        private RunList sigRun = new RunList();
        private cSequencer ts;
        private bool bInSequencer = false;
        private static int TimeMax = 20;
        private int Timeout = TimeMax;

        public GetStudies(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            psi = rProjectStats.ProjectStudyDB.psi;
            ProjectStats = rProjectStats;
            readSitePage = new ReadSitePage();
            cts = readSitePage.cts;
            ts = new cSequencer();
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
            lbSelProj.DataSource = ProjectStats.ManagedPCs.MatchingShortnames; 
            if(lbSelProj.Items.Count <= 0)
            {
                MessageBox.Show("You must obtain projects and remote PCs first!", "Critical Error: form will close",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                this.Close();
                return;
            }
            lbSelProj.SelectedIndex = 0;

#if DEBUG
            btnStart.Visible = true;
#else
            btnStart.Visible = false;
#endif

        }

        private void RunUrl()
        {
            ts.sUrl = sUrl;
            sigRun.PCname = "unknown";
            sigRun.PCname = "unknown";
            sigRun.shortname = shortName;

            ts.ShortName = shortName;
            sigRun.NextOperation = "exit";
            sigRun.bDone = false;
            sigRun.bBusy = true;
            bInSequencer = false;
            timerDoSelected.Enabled = true;
            //tbHdrInfo.Text += "PC:" + sigRun.PCname + " Project:" + sigRun.shortname + " Study:" + readRequest.sStudyV + NL;
            readSitePage.ReadProjectThisSite(ts.ShortName, ts.sUrl, ref sigRun);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            tbInfo.Text = "";
            RunUrl();
            //TaskStart();
        }

        private void EnableControls(bool bEnable)
        {
            lbSelProj.Enabled = bEnable;
            btnStart.Enabled = bEnable;
            btnSave.Enabled = bEnable;
            btnView.Enabled = bEnable;
        }



        private string ExtractAppIds(int inxProj, ref string input)
        {
            string[] sExclude = { "All", " progress", " pending", " inconclusive", "Invalid", "Error", "Show ", "Next ", "Valid" };
            string shortName = psi[inxProj].ShortName;
            string sOut = "";
            string sVal;
            //Process.Start(psi[inxProj].sHostUrl);
            List<string> matches = new List<string>();
            string pattern = @"appid=.*?</a>";
            cProjectStudiesInfo PCIforSN = psi[inxProj];

            // Perform the match
            MatchCollection matchCollection = Regex.Matches(input, pattern);
            // Add each match to the list
            foreach (Match match in matchCollection)
            {
                string sM = match.Value.ToString(); //.ToLower();
                bool bFound = false;
                foreach (string sE in sExclude)
                {
                    if (sM.Contains(sE))
                    {
                        bFound = true;
                        break;
                    }
                }
                if (bFound) continue; // skip this match

                int i = sM.IndexOf("appid=");
                int j = globals.FirstNonInteger(sM, i + 6);
                string tOut = sM.Substring(i + 6, j - (i + 6)) + ":";
                sVal = "0";
                int iCnt = input.IndexOf(sM + " (");
                if (iCnt > 0)
                {
                    int jCnt = globals.FirstNonInteger(input, iCnt + 2 + sM.Length);
                    if (jCnt > 0)
                    {
                        sVal = input.Substring(iCnt + 2 + sM.Length, jCnt - (iCnt + 2 + sM.Length)).Trim();
                    }
                }

                i = sM.LastIndexOf("</a>");
                j = sM.LastIndexOf("\">", i);
                tOut += sM.Substring(j + 2, i - j - 2).Trim() + ":" + sVal;
                sOut += tOut + Environment.NewLine;
                if (!matches.Contains(tOut))
                    matches.Add(tOut);
            }
            foreach (string s in matches)
            {
                string[] sTrio = s.Split(':');
                PCIforSN.AddStudy(sTrio[0], sTrio[1], Convert.ToInt32(sTrio[2]));
            }
            if (matches.Count == 0)
            {
                sVal = "0";
                int iCnt = input.IndexOf("State: All (");
                if (iCnt > 0)
                {
                    int jCnt = globals.FirstNonInteger(input, iCnt + 12);
                    if (jCnt > 0)
                    {
                        sVal = input.Substring(iCnt + 12, jCnt - (iCnt + 12)).Trim();
                        PCIforSN.AddStudy("0", "unknown", Convert.ToInt32(sVal));
                        sOut = "0:unknown:" + sVal + Environment.NewLine;
                    }
                }
            }
            return sOut;
        }

        private void lbSelProj_SelectedIndexChanged(object sender, EventArgs e)
        {
            iLoc = lbSelProj.SelectedIndex;
            shortName = ProjectStats.ManagedPCs.MatchingShortnames[iLoc];
            projID = ProjectStats.ManagedPCs.AvailableProjectIDs[iLoc];
            inxProj = ProjectStats.ShortnameToIndex(shortName);
            //sUrl = ProjectStats.ProjectList[inxProj].FormURL(projID); // this includes the study
            sUrl = ProjectStats.ProjectList[inxProj].sURL;
            sUrl += ProjectStats.ProjectList[inxProj].sHid;
            sUrl += projID;
            tbInfo.Text = ProjectStats.ProjectStudyDB.GetStudyInfoList(shortName);
            FillTable(shortName);
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            tbInfo.Text = ProjectStats.ProjectStudyDB.SaveStudyFile();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            tcStudy.SelectedTab = tabView;
            tbInfo.Text = ProjectStats.ProjectStudyDB.GetStudyInfoList("");
        }

        private void lbSelProj_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbSelProj.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                lbSelProj.SelectedIndex = index;
                globals.OpenUrl(sUrl);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            tcStudy.SelectedTab = tabEdit;
        }


        private void FillTable(string sName)
        {
            dgvStudyInfo.Rows.Clear();
            SelectedEdit_psi = ProjectStats.ProjectStudyDB.GetStudyElements(shortName);
            if (SelectedEdit_psi == null) return;
            foreach (cStudyIDsDue SID in SelectedEdit_psi.StudyIDsDue)
            {
                dgvStudyInfo.Rows.Add(SID.sStudy, SID.nDueDuration, SID.sStudyName,
                    SID.CPUsUsed.ToString("F2"), SID.GPUsUsed.ToString("F2"));
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            string OrigID = "";
            int nDays = 3;
            double nGPUs = 1.0;
            double nCPUs = 1.0;
            SelectedEdit_psi = ProjectStats.ProjectStudyDB.GetStudyElements(shortName);
            if (SelectedEdit_psi == null) return;
            foreach (DataGridViewRow row in dgvStudyInfo.Rows)
            {
                if (row.IsNewRow) continue;
                string sID = row.Cells["ID"].Value.ToString();
                string sName = row.Cells["NameStudy"].Value.ToString();
                string sDays = row.Cells["DaysDuration"].Value.ToString();
                string sCPUs = row.Cells["CPUs"].Value.ToString();  
                string sGPUs = row.Cells["GPUs"].Value.ToString();
                
                if (row.Cells["OrigID"].Value == null)
                    OrigID = sID;
                else OrigID = row.Cells["OrigID"].Value.ToString();


                if (int.TryParse(sDays, out nDays))
                {
                }
                else
                {
                    nDays = 3;
                    row.Cells["Days"].Value = "3";
                }

                if (double.TryParse(sCPUs, out nCPUs))
                {
                }
                else
                {
                    nCPUs = 1.0;
                    row.Cells["CPUs"].Value = "1.0";
                }

                if (double.TryParse(sGPUs, out nGPUs))
                {
                }
                else
                {
                    nGPUs = 1.0;
                    row.Cells["GPUs"].Value = "1.0";
                }


                SelectedEdit_psi.UpdateStudy(sID, sName, OrigID, nDays, nCPUs, nGPUs);
            }
        }

        private void SequenceConcluded()
        {
            timerDoSelected.Enabled = false;
            EnableControls(true);
            tbInfo.Text = sResult;
            if (sResult.Length > 0)
                FillTable(shortName);
        }

        private void timerDoSelected_Tick(object sender, EventArgs e)
        {
            Timeout--;
            if (sigRun.bDone)
            {
                string sRawPage = readSitePage.sRawPage;
                string sMsgOut = readSitePage.sMsgOut;
                readSitePage.SignalTaskDone();
                if (sMsgOut.Length == 0)
                {
                    timerDoSelected.Enabled = false; ;
                    sResult = ExtractAppIds(inxProj, ref sRawPage);
                    switch (sigRun.NextOperation)
                    {
                        case "exit":
                            SequenceConcluded();
                            return;
                    }
                }
                else tbInfo.Text = sMsgOut;
            }
            if (Timeout < 0)
            {
                timerDoSelected.Enabled = false;
                readSitePage.Cancel();
            }
        }

        private void dgvStudyInfo_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int rowIndex = e.RowIndex;
        }
    }
}
