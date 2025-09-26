using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;
using static System.Windows.Forms.LinkLabel;

namespace CreditStatistics
{
    internal partial class ShowBunkeredWUs : BasePandoraPCs
    {

        List<string> ShortNameList = new List<string>();

        public void AllowedPCs_changed(object sender, PCsChangedEventArgs e)
        {
            CreateDGV();
            if (e.MustRescan)
                PerformFetchTasks();
        }

        public ShowBunkeredWUs(ref cProjectStruct rProjectStats, ref ReqCmds RreqCmd)
    : base(ref rProjectStats, ref RreqCmd) //Pass required parameters to base constructor
        {
            InitializeComponent();
            this.PCsChanged += AllowedPCs_changed;
            foreach (cCalcLimitProj clp in TempletDB.ProjList)
                ShortNameList.Add(clp.ShortName);
            this.Shown += InitialLoad;
        }
        /*
        The shortname list and the shortname fields in the datagridview came from templetdb
        that order may be different than in the pandoradatabase projlist
        The PCs are checkboxs ordered by managed receiveds the the datarow number matches the checkbox in the base form
        */

        private void InitialLoad(object sender, EventArgs e)
        {

        }

        /*
         There is an extra column in the grid and two extra rows
         projects not in the sprint are ignored
         */


        int nDataColumns = 0;
        int nDataRows = 0;
        int nColumns = 0;
        int nRows = 0;
        private void CreateDGV()
        {
            nDataColumns = TempletDB.ProjList.Count;
            nColumns = nDataColumns + 1;
            nDataRows = PandoraDatabase.Count;
            nRows = nDataRows + 2;

            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.Columns.Add("PC Name", "PC Name");
            for (int i = 0; i < nDataColumns; i++)
            {
                string sN = ShortNameList[i];
                dgv.Columns.Add(sN, sN);
            }
            dgv.Columns.Add("Totals", "Totals");

            string[] arr = Enumerable.Repeat("", nRows).ToArray();
            for (int i = 0; i < nDataRows; i++)
            {
                CheckBox cb = ThisCB(i);
                arr[0] = cb.Text;
                dgv.Rows.Add(arr);
                dgv.Rows[i].Cells[0].Style.ForeColor = (cb.Checked) ? Color.Blue : Color.Black; //ThisCB(i).ForeColor;//  
            }
            arr[0] = "Totals";
            dgv.Rows.Add(arr);
            arr[0] = "Percent";
            dgv.Rows.Add(arr);
        }

        private int FNlookup(string s, ref int NumUnk)
        {
            int i = ProjectStats.GetUrlIndex(s);
            if (i < 0)
            {
                NumUnk++;
                return -1;
            }
            return i;
        }

        public List<List<string>> RawList = new List<List<string>>();

        private void ReDisplay()
        {
            int r, c;
            string sN;
            r = 0;
            foreach (List<string> list in RawList)
            {
                c = 0;
                foreach (string s in list)
                {
                    dgv.Rows[r].Cells[c + 1].Value = s;
                    c++;
                }
                r++;
            }
        }

        private void ParseProjectStatus(ref string sReport, ref cPClimit PCl)
        {
            string ShortName = "";
            bool bWanted = false;
            int nCnt = 0;
            string[] lines = sReport.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string sUnk = "";
            string NL = Environment.NewLine;
            cCalcLimitProj clp = null;
            foreach (string line in lines)
            {
                nCnt++;
                if (nCnt % 10 == 0)
                    Application.DoEvents();
                int i = line.IndexOf("master url:");
                if (i > 0)
                {
                    int iNamePS = ProjectStats.GetUrlIndex(line);
                    if (iNamePS == -1)
                    {
                        sUnk += "Cannot identify project " + line + NL;
                        continue;
                    }
                    ShortName = ProjectStats.ShortName(iNamePS);
                    if (!ShortNameList.Contains(ShortName))
                    {
                        bWanted = false;
                        continue;
                    }
                    bWanted = true;
                    clp = PCl.GetProjStruct(ShortName);
                    if (clp == null)
                    {
                        /*
                        cPSlist ps = ProjectStats.ProjectList[iNamePS];
                        SelectedProject = PCl.AddProject(PCname, ShortName, ps.MasterUrl, ps.sStudyV);
                        pandoraConfig.WriteDBrecord(ref PCl);
                        pandoraConfig.UpdateDBfromPC(PCname, ref PCl);
                        ShortNameList.Add(ShortName);
                        tbInfo.Text += "Added " + ShortName + " to " + PCname + " database" + Environment.NewLine;
                        */
                        sUnk += "Ignoring unknown project: " + ShortName + NL;
                    }
                    else
                    {
                        i = line.IndexOf("suspended via gui:");
                        if (i > 0 && bWanted)
                        {
                            string Ans = line.Substring(i + 18).Trim();
                            clp.ProjectSuspended = (Ans != "no");
                        }
                        else
                        {
                            i = line.IndexOf("more work:");
                            if (i > 0 && bWanted)
                            {
                                string Ans = line.Substring(i + 10).Trim();
                                clp.ProjectNoNewWork = (Ans == "yes");
                                bWanted = false;  // more work comes after the suspended
                            }
                        }
                    }
                }
            }
        }

        private void ParseTaskReport(ref string sReport, ref cPClimit PCl)
        {
            string Operator = "";
            string Operand = "";
            string sUnkOut = "";
            int inxProj;
            int UnkCnt = 0;
            cCalcLimitProj clp = null;
            string NL = Environment.NewLine;
            string[] lines = sReport.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                int i = line.IndexOf(":");
                if (i > 0)
                {
                    Operator = line.Substring(0, i).Trim();
                    Operand = line.Substring(i + 1).Trim();
                }
                else
                {
                    sUnkOut += "unk " + line + NL;
                    continue;
                }

                if (Operator == "project url")
                {
                    int nName = FNlookup(Operand, ref UnkCnt);
                    if (nName < 0)
                    {
                        sUnkOut += "url lookup failed: " + line + NL;
                        continue;
                    }
                    string Shortname = ProjectStats.ShortName(nName);
                    inxProj = PCl.NameToIndex(Shortname);
                    if (inxProj < 0)
                    {
                        sUnkOut += "project name lookup failed: " + line + " name: " + Shortname + NL;
                        continue;
                    }
                    else
                    {
                        clp = PCl.ProjList[inxProj];
                        clp.TotalWUcnt++;
                    }
                }
                else if (Operator == "ready to report")
                {
                    if (clp == null) continue;
                    if (Operand == "1")
                        clp.ReadyToReport++;
                }
                else if (Operator == "exit_status")
                {
                    if (clp == null) continue;
                    if (Operand == "0")
                        clp.JobsSucceeded++;
                    else clp.JobsFailed++;
                }
                else sUnkOut += "unk " + line;
            }
        }

        private void PerformFetchTasks()
        {
            int UnkCnt = 0;
            int inxProj = 0;
            cCalcLimitProj clp = null;
            int r = -1;
            int n = TempletDB.ProjList.Count;
            string NL = Environment.NewLine;
            RawList.Clear();
            int iDGV = -1;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                r++;
                if (r == nDataRows) break;
                iDGV++;
                BaseProgressBar.Value++;
                Application.DoEvents();
                List<string> OneRaw = Enumerable.Repeat(string.Empty, n + 1).ToList();
                cCalcLimitProj SelectedProject = null;
                string PCname = row.Cells["PC Name"].Value.ToString();
                if (PCname == "Totals") break;   // slame as r == ndatarows break ??
                CheckBox cb = ThisCB(iDGV);
                cPClimit PCl = pandoraConfig.NameToSprintPC(PCname);
                PCl.ClearTotals();
                string[] lines;
                if (cb.Checked)
                {
                    string strResult = ManagedPCs.ContactPCproject(PCname, "--get_task_reports", "").ToLower();
                    if (!IsStillOnline(PCname, true, ManagedPCs.ErrorStatus == 0))
                    {
                        row.Cells["PC name"].Style.ForeColor = cb.ForeColor;
                        RawList.Add(OneRaw);
                        continue;
                    }
                    if (strResult != "")
                    {
                        ParseTaskReport(ref strResult, ref PCl);
                        strResult = ManagedPCs.ContactPCproject(PCname, "--get_project_status", "").ToLower();
                        if (strResult != "")
                        {
                            ParseProjectStatus(ref strResult, ref PCl);
                        }
                    }

                    for (int j = 0; j < PCl.ProjList.Count; j++)
                    {
                        clp = PCl.ProjList[j];
                        Color FCO = Color.Black;
                        if (!ShortNameList.Contains(clp.ShortName)) continue;
                        string sOut = "[" + clp.TotalWUcnt + "] (" + clp.ReadyToReport + ":" + clp.JobsSucceeded + ")";
                        row.Cells[clp.ShortName].Value = sOut;
                        int iName = PCl.GetProjNameIndex(clp.ShortName);
                        SelectedProject = PCl.ProjList[iName];
                        if (SelectedProject.ProjectSuspended)
                            FCO = Color.Red;
                        else if (SelectedProject.ProjectNoNewWork)
                            FCO = Color.Blue;
                        row.Cells[clp.ShortName].Style.ForeColor = FCO;
                        int colIndex = dgv.Columns[clp.ShortName].Index; // this is fubar todo to do
                        OneRaw[colIndex - 1] = sOut;
                    }
                }
                RawList.Add(OneRaw);
                Application.DoEvents();
            }
            List<string> TwoRaw = Enumerable.Repeat(string.Empty, n + 1).ToList();
            for (int i = 0; i < 2; i++)// blank out the totals and percent extra rows
                RawList.Add(TwoRaw);
            BaseProgressBar.Value = 0;
        }

        private void ClearDataTable()
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex == 0) continue;
                    cell.Value = "";
                    cell.Style.ForeColor = Color.Black;
                }
            }
        }

        private void FilterResults(string sFilter)
        {
            int n = TempletDB.ProjList.Count;
            int r = -1;
            int nRows = dgv.Rows.Count;
            int[] ProjValues = new int[dgv.Rows.Count];
            int TotalPoints = 0;
            int NumPCs = ProjectStats.PandoraDatabase.Count;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                r++;
                if (r == NumPCs) break;
                string PCname = row.Cells[0].Value.ToString();
                cPClimit PCl = pandoraConfig.GetPCbyName(PCname);
                int PCValues = 0;
                int c = 0;
                foreach (cCalcLimitProj clp in PCl.ProjList)
                {
                    int Value = 0;
                    switch (sFilter)
                    {
                        case "GoodJobs":
                            Value = clp.JobsSucceeded;
                            break;
                        case "BadJobs":
                            Value = clp.JobsFailed;
                            break;
                        case "Points":
                            Value = clp.JobsSucceeded * clp.Points;
                            break;
                        case "Backlog":
                            Value = clp.TotalWUcnt - clp.ReadyToReport; // -clp.JobsSucceeded -clp.JobsFailed;
                            break;
                        case "Ready":
                            Value = clp.ReadyToReport;
                            break;
                    }
                    ProjValues[c] += Value;
                    PCValues += Value;
                    TotalPoints += Value;
                    if (!ShortNameList.Contains(clp.ShortName)) continue;

                    row.Cells[clp.ShortName].Value = (Value > 0) ? Value : "";
                    c++;
                }
                row.Cells["Totals"].Value = (PCValues > 0) ? PCValues : "";
            }

            int MaxPts = 0;
            for (int i = 0; i < n; i++)
                MaxPts = Math.Max(MaxPts, ProjValues[i]);

            double[] dPct = new double[n];

            for (int i = 0; i < n; i++)
            {
                if (MaxPts != 0)
                    dPct[i] = 100.0 * (double)ProjValues[i] / (double)MaxPts;
                else dPct[i] = 0;
            }

            int BottomRow = nRows - 1;

            for (int i = 0; i < n; i++)
            {
                string ShortName1 = dgv.Columns[i + 1].HeaderCell.Value.ToString();
                string ShortName2 = TempletDB.ProjList[i].ShortName;
                string ShortName = ShortName1;
                if (!ShortNameList.Contains(ShortName)) continue;
                dgv.Rows[BottomRow - 1].Cells[ShortName].Value = (ProjValues[i] > 0) ? ProjValues[i] : "";
                double d = dPct[i];
                string sd = (d != 0.0) ? d.ToString("F1") : "";
                dgv.Rows[BottomRow].Cells[ShortName].Value = sd;
            }
            dgv.Rows[BottomRow - 1].Cells[n + 1].Value = (TotalPoints > 0) ? TotalPoints : "";
            dgv.Rows[BottomRow - 1].Cells[0].Value = "Totals";
        }


        private void btnSuccess_Click(object sender, EventArgs e)
        {
            FilterResults("GoodJobs");
        }

        private void btnFailedJobs_Click(object sender, EventArgs e)
        {
            FilterResults("BadJobs");
        }

        private void btnPoints_Click(object sender, EventArgs e)
        {
            FilterResults("Points");
        }

        private void btnReadyToReport_Click(object sender, EventArgs e)
        {
            FilterResults("Ready");
        }

        private void btnRawTab_Click(object sender, EventArgs e)
        {
            ReDisplay();
        }

        private void btnBacklog_Click(object sender, EventArgs e)
        {
            FilterResults("Backlog");
        }

        private void btnScanData_Click(object sender, EventArgs e)
        {
            ClearDataTable();
            PerformFetchTasks();
        }
    }
}