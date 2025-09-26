using Microsoft.Playwright;
using Microsoft.Playwright;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
        //private string shortName = "";
        private string sResult = "";
        private string NL = Environment.NewLine;
        private bool bDone = false;
        public CancellationTokenSource cts;
        private ReadSitePage readSitePage;
        private RunList sigRun = new RunList();
        private cSequencer ts;
        private bool bInSequencer = false;
        private static int TimeMax = 60;
        private int Timeout = 0;

        private cProjectStudiesInfo SelectedEdit_psi = null;

        public GetStudies(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            psi = rProjectStats.ProjectStudyDB.psi;
            ProjectStats = rProjectStats;
            readSitePage = new ReadSitePage();
            readSitePage.Init(ref sigRun);
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
            string PCname = Dns.GetHostName().ToLower();
            ProjectStats.ManagedPCs.SelectCurrentFromPC(PCname);
            lbSelProj.DataSource = ProjectStats.ManagedPCs.MatchingShortnames;
            if (lbSelProj.Items.Count <= 0)
            {
                MessageBox.Show("You must obtain projects and remote PCs first!", "Critical Error: form will close", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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

        private void RunUrl(string shortname)
        {
            ts.sUrl = sUrl;
            sigRun.PCname = Dns.GetHostName().ToLower();
            sigRun.shortname = shortname;
            ts.ShortName = shortname;
            sigRun.NextOperation = "exit";
            sigRun.bDone = false;
            sigRun.bBusy = true;
            bInSequencer = false;
            timerDoSelected.Start();
            //tbHdrInfo.Text += "PC:" + sigRun.PCname + " Project:" + sigRun.shortname + " Study:" + readRequest.sStudyV + NL;
            readSitePage.ReadProjectThisSite(ts.ShortName, ts.sUrl);
        }

        private void TaskStart()
        {
            rtbInfo.Clear();
            tcStudy.SelectedTab = tabView;
            ts.NumSeqPJ = -1;
            StartNext();
        }

        private void StartNext()
        {
            ts.NumSeqPJ++;
            if (ts.NumSeqPJ >= ProjectStats.ManagedPCs.MatchingShortnames.Count)
            {
                SequenceConcluded();
                return;
            }
            sigRun.PCname = Dns.GetHostName().ToLower();
            sigRun.shortname = ProjectStats.ManagedPCs.MatchingShortnames[ts.NumSeqPJ];

            iLoc = ts.NumSeqPJ;
            ts.ShortName = ProjectStats.ManagedPCs.MatchingShortnames[iLoc];
            ts.ProjID = ProjectStats.ManagedPCs.AvailableProjectIDs[ts.NumSeqPJ];
            sUrl = ProjectStats.ProjectList[ts.NumSeqPJ].sURL;
            sUrl += ProjectStats.ProjectList[ts.NumSeqPJ].sHid;
            sUrl += ts.ProjID;
            ts.sUrl = sUrl;
            sigRun.PCname = Dns.GetHostName().ToLower();
            ts.ShortName = sigRun.shortname;
            sigRun.NextOperation = "next";
            sigRun.bDone = false;
            sigRun.bBusy = true;
            bInSequencer = true;

            timerDoSelected.Start();
            globals.AppendColoredText(rtbInfo, "Obtaining study information project " + ts.ShortName + Environment.NewLine, Color.Blue);
            Application.DoEvents();
            readSitePage.ReadProjectThisSite(ts.ShortName, ts.sUrl);
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            rtbInfo.Clear();
            //RunUrl(shortName);
            TaskStart();
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
            string[] sExclude = { "All", " progress", " pending", " inconclusive", "Invalid", "Error", "Show ", "Next ", "Valid", "Pending" };
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
                string t1 = sM.Substring(j + 2, i - j - 2).Trim().Replace(":", "-");
                tOut += t1 + ":" + sVal;
                sOut += tOut + Environment.NewLine;
                if (!matches.Contains(tOut))
                    matches.Add(tOut);
            }
            foreach (string s in matches)
            {
                string[] sTrio = s.Split(':');
                if(int.TryParse(sTrio[2], out int result))
                {
                    PCIforSN.AddStudy(sTrio[0], sTrio[1],result);
                }
                else
                {
                    Debug.Assert(false,"problem parsing trio");
                }

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
                        PCIforSN.AddStudy("0", "All", Convert.ToInt32(sVal));
                        sOut = "0:All:" + sVal + Environment.NewLine;
                    }
                }
            }
            return sOut;
        }

        private void lbSelProj_SelectedIndexChanged(object sender, EventArgs e)
        {
            iLoc = lbSelProj.SelectedIndex;
            ts.ShortName = ProjectStats.ManagedPCs.MatchingShortnames[iLoc];
            ts.ProjID = ProjectStats.ManagedPCs.AvailableProjectIDs[iLoc];
            ts.NumSeqPJ = ProjectStats.ShortnameToIndex(ts.ShortName);
            //sUrl = ProjectStats.ProjectList[inxProj].FormURL(projID); // this includes the study
            sUrl = ProjectStats.ProjectList[ts.NumSeqPJ].sURL;
            sUrl += ProjectStats.ProjectList[ts.NumSeqPJ].sHid;
            sUrl += ts.ProjID;
            rtbInfo.Text = ProjectStats.ProjectStudyDB.GetStudyInfoList(ts.ShortName);
            FillTable(ts.ShortName);
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            rtbInfo.Text = ProjectStats.ProjectStudyDB.SaveStudyFile();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            tcStudy.SelectedTab = tabView;
            rtbInfo.Text = ProjectStats.ProjectStudyDB.GetStudyInfoList("");
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
            SelectedEdit_psi = ProjectStats.ProjectStudyDB.GetStudyElements(ts.ShortName);
            if (SelectedEdit_psi == null) return;
            foreach (cStudyIDsDue SID in SelectedEdit_psi.StudyIDsDue)
            {
                dgvStudyInfo.Rows.Add(SID.sStudy, SID.nDueDuration, SID.sStudyName,
                    SID.CPUsUsed.ToString("F2"), SID.GPUsUsed.ToString("F2"), SID.nItems.ToString());
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            string OrigID = "";
            int nDays = 3;
            double nGPUs = 1.0;
            double nCPUs = 1.0;
            int nItms = 1;
            SelectedEdit_psi = ProjectStats.ProjectStudyDB.GetStudyElements(ts.ShortName);
            if (SelectedEdit_psi == null) return;
            SelectedEdit_psi.StudyIDsDue.Clear();
            foreach (DataGridViewRow row in dgvStudyInfo.Rows)
            {
                if (row.IsNewRow) continue;
                string sID = row.Cells["ID"].Value.ToString();
                string sName = row.Cells["NameStudy"].Value.ToString();
                string sDays = row.Cells["DaysDuration"].Value.ToString();
                string sCPUs = row.Cells["CPUs"].Value.ToString();
                string sGPUs = row.Cells["GPUs"].Value.ToString();
                string sItms = row.Cells["MaxApps"].Value.ToString();
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


                if (int.TryParse(sItms, out nItms))
                {
                    if(nItms < 0) nItms = 0;
                    row.Cells["MaxApps"].Value = nItms.ToString();
                }
                else
                {
                    nItms = 0;
                    row.Cells["MaxApps"].Value = "0";

                }
                SelectedEdit_psi.AddFullStudy(sID, sName, nDays, nCPUs, nGPUs, nItms);
                //SelectedEdit_psi.UpdateStudy(sID, OrigID, sName, nDays, nCPUs, nGPUs);
            }
        }

        private void SequenceConcluded()
        {
            timerDoSelected.Enabled = false;
            bInSequencer = false;
            EnableControls(true);
        }

        private void Sequence1Concluded()
        {
            timerDoSelected.Enabled = false;
            EnableControls(true);
            rtbInfo.Text = sResult;
            if (sResult.Length > 0)
                FillTable(ts.ShortName);
        }


        private async void timerDoSelected_Tick(object sender, EventArgs e)
        {
            Timeout++;
            if (Timeout >= TimeMax)
            {
                Timeout = 0;
                timerDoSelected.Enabled = false;
                readSitePage.Cancel();
                sigRun.bDone = true;
                readSitePage.sRawPage = "";
                globals.AppendColoredText(rtbInfo, "Timeout exceeded " + TimeMax.ToString() + " seconds" + Environment.NewLine,Color.Red);
                await Task.Delay(500);
                Application.DoEvents();
                switch (sigRun.NextOperation)
                {
                    case "exit":
                        Sequence1Concluded();
                        return;
                    case "next":
                        StartNext();
                        return;
                }
            }
            if (sigRun.bDone)
            {
                Timeout = 0;
                timerDoSelected.Enabled = false;
                string sRawPage = readSitePage.sRawPage;
                string sMsgOut = readSitePage.sMsgOut;
                if (sRawPage.Length > 0)
                {
                    sResult = ExtractAppIds(ts.NumSeqPJ, ref sRawPage);
                    if(sResult.Length == 0)
                        globals.AppendColoredText(rtbInfo, "No data for " + ts.ShortName + NL, Color.Red);
                    readSitePage.sRawPage = "";
                }
                if (sMsgOut.Length > 0)
                {
                    globals.AppendColoredText(rtbInfo, "Error " + sMsgOut + NL, Color.Red);
                    sResult = "";
                }
                await Task.Delay(500);
                switch (sigRun.NextOperation)
                {
                    case "exit":
                        Sequence1Concluded();
                        return;
                    case "next":
                        globals.AppendColoredText(rtbInfo,sResult + NL, Color.Black);
                        Application.DoEvents(); 
                        StartNext();
                        return;
                }
            }

        }

        private void dgvStudyInfo_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int rowIndex = e.RowIndex;
        }

        private void tcStudy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnGetUrl_Click(object sender, EventArgs e)
        {
            RunUrl(ts.ShortName);
        }
    }
}
