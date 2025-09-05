using Microsoft.Playwright;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;
using static System.Windows.Forms.DataFormats;

namespace CreditStatistics
{
    internal partial class ShowData : Form
    {
        ReadSitePage readSitePage;
        private string[] formats = { "d MMM yyyy | H:mm:ss UTC", "dd MMM yyyy | H:mm:ss UTC", "d MMM yyyy, H:mm:ss UTC", "dd MMM yyyy, H:mm:ss UTC", "dd MMM yyyy, h:mm:ss tt UTC" };
        private string[] format2 = { "d MMM yyyy, H:mm:ss", "dd MMM yyyy, H:mm:ss" };
        ReadRequest readRequest;
        cProjectStruct ProjectStats;
        private cNoHeaderProj NoHeaderProj = new cNoHeaderProj();
        private bool bInSequencer = false;
        private RunList sigRun = new RunList();
        private string NL = Environment.NewLine;

        private List<cCreditInfo> LCreditInfo = new List<cCreditInfo>();
        private List<cCreditInfo> UnsortedLCI = new List<cCreditInfo>();
        private List<DateTime> UnsortedDT = new List<DateTime>();
        private List<int> indicesCreditInfo;
        public List<double> mCPU = new List<double>();
        public List<double> mELA = new List<double>();

        private int[] HDRresults;

        public class cOutFilter
        {
            public int n;
            public long nWidth;
            public double avg;
            public double std;
            public List<double> data;
            public List<int> outlierIndexes;
        }

        cOutFilter cNAS = new cOutFilter();

        private cSequencer ts;
        public class cEachPc
        {
            public int nValidWUs;
            public int nRemainWUs;
            public bool OutOfData;
            public double PctEff;   // 100 is best efficiency of all the sytstems
        }

        public CancellationTokenSource cts;
        public ShowData(ref cProjectStruct rProjectStats, ReadRequest rr)
        {
            InitializeComponent();
            readSitePage = new ReadSitePage();
            cts = readSitePage.cts;
            readRequest = rr;
            ProjectStats = rProjectStats;
            ts = new cSequencer();
            ts.pc = new PandoraConfig(ref ProjectStats);
            ts.PClimit = ts.pc.PandoraDatabase;
            ts.DB = ProjectStats.TempletDB;
            ts.sUrl = readRequest.UrlWanted;
            ts.ProjectList = ProjectStats.ProjectList;

#if DEBUG
            btnRead.Visible = true;
#else
            btnRead.Visible = false;
#endif
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
        private void SelectTask()
        {
            ClearInfoDisplay();
            if (readRequest.bUseUrl)
            {
                tcShowData.SelectTab("tabresults");
                ClearResultInfo();
                HDRresults = new int[] { 0, 0, 0, 0 };
                RunUrl();
            }
            else
            {
                RequestFromSelector();
            }

        }

        private void InitialLoad(object sender, EventArgs e)
        {
            SelectTask();
            FormSystemsCB();
            FormProjectsCB();
            pbSeq.Value = 0;
            tbUTCs.Text = ProjectStats.UTC_Start;
            tbUTCe.Text = ProjectStats.UTC_End;
            ShowSprintDays();
        }

        private void ClearResultInfo()
        {
            UnsortedLCI.Clear();
            UnsortedDT.Clear();
            LCreditInfo.Clear();
            mELA.Clear();
            mCPU.Clear();
        }

        private void SelectSprintSystems()
        {
            string NL = Environment.NewLine;
            ClearInfoDisplay();
            string sOut = "";
            ts.NumSeqPC = 0;
            ts.NumSeqPJ = 0;
            ts.pc.GetLongest(out int nPC, out int nPJ);
            foreach (cPClimit pcl in ts.pc.PandoraDatabase)
            {
                if (pcl.IsSelected)
                {
                    ts.NumSeqPC++;
                    sOut += (pcl.PCname + ": ").PadRight(nPC + 2);
                    foreach (cCalcLimitProj clp in pcl.ProjList)
                    {
                        if (clp.IsSelected)
                        {
                            ts.NumSeqPJ++;
                            sOut += (clp.ShortName + "(" + clp.sStudy + "),").PadRight(6 + clp.ShortName.Length);
                            //ts.AddProject(pcl.PCname, clp.ShortName, sUrl, clp.sStudy);
                        }
                    }
                    sOut = sOut.TrimEnd(',', ' ') + NL;
                }
            }
            SetPBseq();
            sOut += NL + "The above PCs and their projects will be processed in sequence." + NL;
            sOut += "Please ensure the project studies shown in parentheses are correct." + NL;
            sOut += "If not, please select the correct projects and studies in run options" + NL;
            sOut += "the multiple project studies.  Then re-run this form." + NL;
            sOut += "Also, please examine the previous statistics and see if there is enough" + NL;
            sOut += "data to run the Sprint. Ensure 10 or more data results for each project." + NL;
            sOut += "If more data needed then you must acquire the data first" + NL;
            SetTextHidden(tbInfo, sOut);            
        }

        private void RequestFromSelector()
        {
            if (readRequest.UsePandoraDatabase)
            {
                tcShowData.SelectTab("tabSettings");
                SelectSprintSystems();
            }
            else
            {
                tcShowData.SelectTab("tabresults");
                ClearResultInfo();
                HDRresults = new int[] { 0, 0, 0, 0 };
                sigRun.PCname = readRequest.PCname;
                sigRun.shortname = readRequest.shortname;
                ts.ShortName = readRequest.shortname;
                ts.sUrl = readRequest.UrlWanted;
                sigRun.NextOperation = "exit";
                sigRun.bDone = false;
                sigRun.bBusy = true;
                bInSequencer = false;
                timerDoSelected.Enabled = true;
                tbHdrInfo.Text += "PC:" + sigRun.PCname + " Project:" + sigRun.shortname + " Study:" + readRequest.sStudyV + NL;
                readSitePage.ReadProjectThisSite(ts.ShortName, ts.sUrl, ref sigRun);
                tbHdrInfo.Text = "20 SEC: " + ts.ShortName + NL;
            }
        }

        private void RunUrl()
        {
            sigRun.PCname = "unknown";
            sigRun.shortname = readRequest.shortname;
            ts.ShortName = readRequest.shortname;
            sigRun.NextOperation = "exit";
            sigRun.bDone = false;
            sigRun.bBusy = true;
            bInSequencer = false;
            timerDoSelected.Enabled = true;
            tbHdrInfo.Text += "PC:" + sigRun.PCname + " Project:" + sigRun.shortname + " Study:" + readRequest.sStudyV + NL;
            readSitePage.ReadProjectThisSite(ts.ShortName, ts.sUrl, ref sigRun);
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            ClearInfoDisplay();
            SelectTask();
        }

        private void SequenceConcluded()
        {
            timerDoSelected.Enabled = false;
            pbSeq.Value = 0;
            bInSequencer = false;
            gbAnal.Enabled = true;
        }

        private void timerDoSelected_Tick(object sender, EventArgs e)
        {
            if (sigRun.bDone)
            {
                string sRawPage = readSitePage.sRawPage;
                string sMsgOut = readSitePage.sMsgOut;
                readSitePage.SignalTaskDone();
                if (sMsgOut.Length == 0)
                {
                    timerDoSelected.Enabled = false;
                    ParseWebPage(ref sRawPage, sigRun);
                    switch (sigRun.NextOperation)
                    {
                        case "next":
                            StartNext();
                            break;
                        case "exit":
                            SequenceConcluded();
                            return;
                    }
                }
                else tbInfo.Text = sMsgOut;
            }
        }

        private void ParseWebPage(ref string sRawPage, RunList runList)
        {
            string sTemp = "";
            int RecordsPerPage = 0;
            if (readRequest.GetHdr)
            {
                switch (runList.shortname)
                {
                    case "yoyo":
                        
                            RecordsPerPage = NoHeaderProj.ProcessRawBodyYOYO(ref sRawPage);  
                            if(RecordsPerPage > 0)
                            {
                                GetTableFromNoHeader(ref NoHeaderProj);
                                //CreditInfo = ProjectStats.LCreditInfo;
                                GetResults(ref runList);
                                tbHdrInfo.Text = NoHeaderProj.GetHDR();
                            }

                            //AllowGS(!ProjectStats.TaskError); 
                        
                        break;
                    case "wcg":
                        break;
                    default:
                        runList.NumValidWUs = ProcessHDR(ref sRawPage, ref sTemp, ref HDRresults);
                        tbInfo.Text += "PC:" + ts.sHostName + " Project:" + ts.ShortName + " Study:" + (ts.sStudy == "" ? "0" : ts.sStudy) + " " + sTemp + NL;
                        if (runList.NumValidWUs == 0)
                        {
                            tbInfo.Text += "No valid records found for " + runList.PCname + " and project " + runList.shortname + Environment.NewLine;
                        }
                        else if (readRequest.GetBody)
                        {
                            RecordsPerPage = ProcessBody(ref sRawPage, ref runList); // same as older GetTableFromRaw
                            GetResults(ref runList);
                            if (bInSequencer)
                                ts.selProjList.sResultInfo = runList.sOutInfo;
                            tbInfo.Text += runList.BGinfo;
                            tbEachResult.Text += runList.sOutInfo;
                        }
                        break;
                }
            }
        }
        public int ProcessHDR(ref string RawPage, ref string sOut, ref int[] HDRresults)
        {
            string[] FindHdr = { "All (", "Valid (", "Invalid (", "Error (" };
            string[] FindHdrA = { "All</a> (", "Valid</a> (", "Invalid</a> (", "Error</a> (" };
            string[] FindHdrD = { "All</a> (", "Valid</b> (", "Invalid</a> (", "Error</a> (" };
            string[] FindHdrB = { "All</a> ", "Valid</b> ", "Invalid</font> ", "Error</a> " };
            string[] FindBTrm = { "|", "|", "|", "<" };
            string[] FindHdrC = { ">All ", ">Valid ", ">Invalid ", ">To late " };
            string[] FindCTrm = { "</a>", "</a>", "</a>", "</a>" };
            List<string> HdrOut = new List<string>();
            int nHdrOut = 0;
            if (RawPage == null) return 0;
            int NumValid = 0;
            for (int k = 0; k < FindHdr.Length; k++)
            {
                string sDFT = ")";  // default terminator

                string sB = FindHdrB[k];
                string tB = FindBTrm[k];
                string sC = FindHdrC[k];
                string tC = FindCTrm[k];

                string sD = FindHdrD[k];
                string sA = FindHdrA[k];
                string s = FindHdr[k];
                int i = RawPage.IndexOf(s);
                int n = s.Length;
                if (i < 0)
                {
                    i = RawPage.IndexOf(sA);
                    n = sA.Length;
                }
                if (i < 0)
                {
                    i = RawPage.IndexOf(sD);
                    n = sA.Length;
                }
                if (i < 0)
                {
                    i = RawPage.IndexOf(sB);
                    n = sB.Length;
                    sDFT = tB;
                }
                if (i < 0)
                {
                    i = RawPage.IndexOf(sC);
                    n = sC.Length;
                    sDFT = tC;
                }
                if (i < 0) continue;

                int j = RawPage.IndexOf(sDFT, i + n);
                if (j < 0) continue;

                string t = RawPage.Substring(i + n, j - i - n).Trim();
                if (t == "") t = "0";
                if (k == 1) NumValid = Convert.ToInt32(t);
                HDRresults[k] = Convert.ToInt32(t);
                sOut += FindHdr[k].Replace(" ", "") + t + ") ";
            }
            sOut += Environment.NewLine;
            return NumValid;
        }

        private int ProcessBody(ref string RawPage, ref RunList runList)
        {
            int iStart, iEnd, i, j, k;
            string t;
            int NumWUs = 0;
            runList.sMsgErr = "";
            string RawTable = "";
            switch (runList.shortname)
            {
                case "yoyo":
                case "wcg":
                    Debug.Assert(false);
                    break;
                case "einstein":
                    iStart = RawPage.IndexOf("<tbody>");
                    iEnd = RawPage.IndexOf("</tbody>");
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd))
                    {
                        runList.sMsgErr += "Bad einstein url:  cannot find TABLE nor TBODY or maybe no results\n";
                        return -3;
                    }
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    BuildEinsteinStatsTable(ref RawTable, ref runList);
                    break;
                case "odlk1": // latinsquares":
                case "lhc":
                case "amicable":
                case "rosetta":
                case "milkyway":
                case "numberfields":
                case "cpdn": // climateprediction":
                case "moowrap":
                case "nfs\": //  escatter":
                case "nfs": // why backslash above??? 
                case "srbase":
                case "yafu":
                case "loda":
                case "rakesearch":
                case "sidock":
                case "rnma":
                case "radioactive":
                case "odlk\": //  progger":
                case "gpugrid":
                case "denis":
                case "gfn":
                case "mine":
                    string strH = "workunit.php?";
                    iStart = RawPage.IndexOf(strH);
                    if (iStart < 0)
                    {
                        runList.sMsgErr += "error: missing 'a href=' or maybe no results\n";  // had to remove <
                        return -4;
                    }
                    iEnd = RawPage.Substring(iStart).IndexOf("</table>");
                    iEnd += iStart;
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildStatsTable(ref RawTable, ref runList);
                case "asteroids":
                    iStart = RawPage.IndexOf("<table class");
                    iEnd = RawPage.IndexOf("</table>");
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildStatsTable(ref RawTable, ref runList);
                case "primegrid":
                    iStart = RawPage.IndexOf("<tr class=\"row0\">");
                    if (iStart < 0)
                    {
                        runList.sMsgErr += "error: no data or missing 'tr class=\"row0\"'\n"; // had to remove < and >
                        return -4;
                    }
                    iEnd = RawPage.Substring(iStart).IndexOf("</table>");   // need skip over any earlier tables in the header
                    if (iStart < 0 || iEnd < 0)
                    {
                        int ERR = 0;
                    }
                    iEnd += iStart;
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildPrimeTable(ref RawTable, ref runList);
                case "gerasim":
                    iStart = RawPage.IndexOf("<table class=\"gridTable\"");
                    iEnd = RawPage.IndexOf("</table>", iStart);
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    string[] OuterTable = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    bool bNext = false;
                    i = 0;
                    while (i < OuterTable.Length)
                    {
                        string s = OuterTable[i++];
                        j = s.IndexOf("<span id=");
                        if (j < 0) continue;
                        k = s.IndexOf("</span>", j);
                        if (j < 0) continue;
                        j = s.LastIndexOf(">", k);
                        t = s.Substring(j + 1, k - j - 1);
                        cCreditInfo ci = new cCreditInfo();
                        if (DateTime.TryParseExact(t, format2, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime1))
                        {
                            ci.tCompleted = dateTime1;
                        }
                        else
                        {
                            return 0;
                        }
                        s = OuterTable[i++];
                        iStart = s.IndexOf("</td><td align=\"center\">cpu</td><td align=\"right\">");
                        if (iStart < 0) return 0;
                        iEnd = s.LastIndexOf(">", iStart);
                        t = s.Substring(iEnd + 1, iStart - iEnd - 1);
                        ci.ElapsedSecs = Convert.ToDouble(t);
                        ci.CPUtimeSecs = ci.ElapsedSecs;
                        j = s.LastIndexOf("</td>", s.Length - 1);
                        if (j < 0) return 0;
                        k = s.LastIndexOf(">", j);
                        if (k < 0) return 0;
                        t = s.Substring(k + 1, j - k - 1);
                        ci.Credits = Convert.ToDouble(t);
                        ci.mELA = ci.Credits / ci.ElapsedSecs;
                        mELA.Add(ci.mELA);
                        if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                        ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                        mCPU.Add(ci.mCPU);
                        ci.bValid = true;
                        LCreditInfo.Add(ci);
                        continue;
                    }
                    break;
                default:
                    iStart = RawPage.IndexOf("<tr class=row0>");
                    if (iStart < 0)
                    {
                        runList.sMsgErr += "error: no data or missing 'tr class=row0'\n"; // had to remove < and >
                        return -4;
                    }
                    iEnd = RawPage.Substring(iStart).IndexOf("</table>");   // need skip over any earlier tables in the header
                    if (iStart < 0 || iEnd < 0)
                    {
                        int ERR = 0;
                    }
                    iEnd += iStart;
                    if (iStart < 0 || iEnd < 0 || (iStart >= iEnd)) return -4;
                    RawTable = RawPage.Substring(iStart, iEnd - iStart);
                    return BuildStatsTable(ref RawTable, ref runList);
            }
            return NumWUs;
        }

        private DateTime GetValueT(ref RunList runList, int iLoc, int iOffset)
        {
            string lSide;
            if ((iLoc + iOffset) >= runList.RawLines.Length)
                return DateTime.MinValue;
            string sTemp = runList.RawLines[iLoc + iOffset];
            int iRight = sTemp.IndexOf("<td>");
            if (iRight < 0)
            {
                runList.sMsgErr += "Error expected <td> not found in line " + sTemp;
                return DateTime.MinValue;
            }
            lSide = sTemp.Substring(iRight + 4);
            iRight = lSide.IndexOf("</td>");
            if (iRight < 0)
            {
                runList.sMsgErr += "Error expected /td not found in line " + sTemp;
                return DateTime.MinValue;
            }
            string s = lSide.Substring(0, iRight);
            if (s == "")
            {
                return DateTime.MinValue;
            }

            if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime1))
            {
                return dateTime1;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        private double GetValueD(ref RunList runList, int iLoc, int iOffset)
        {
            string sTemp = runList.RawLines[iLoc + iOffset];
            string lSide;
            string sRight = "\"right\">";
            int iRight = sTemp.IndexOf(sRight);
            if (iRight < 0)
            {
                runList.sMsgErr += "Error expected right greaterthansign  not found in line " + sTemp;
                return -1;
            }
            lSide = sTemp.Substring(iRight + sRight.Length);
            iRight = lSide.IndexOf("</td>");
            if (iRight < 0)
            {
                runList.sMsgErr += "Error expected /td not found in line " + sTemp;
                return -1;
            }
            string s = lSide.Substring(0, iRight);
            double aValue = Convert.ToDouble(s);
            return aValue;
        }

        private int BuildPrimeTable(ref string RawTable, ref RunList runList)
        {
            string s;
            int WUs = 0;
            string[] OuterTable = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string sLine in OuterTable)
            {
                cCreditInfo ci = new cCreditInfo();
                string[] RawLines = sLine.Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                if(RawLines.Length < 9)
                {
                    break;
                }
                s = RawLines[4];
                if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime1))
                {
                    ci.tCompleted = dateTime1;
                    if (ci.tCompleted == DateTime.MinValue)
                    {
                        break;
                    }
                    UnsortedDT.Add(ci.tCompleted);
                }
                else
                {
                    break;
                }
                int i = RawLines[6].LastIndexOf(">");
                s = RawLines[6].Substring(i + 1);
                ci.ElapsedSecs = Convert.ToDouble(s);
                s = RawLines[7].Substring(i + 1);
                ci.CPUtimeSecs = Convert.ToDouble(s);
                s = RawLines[8].Substring(i + 1);
                ci.Credits = Convert.ToDouble(s);
                ci.mELA = ci.Credits / ci.ElapsedSecs;
                mELA.Add(ci.mELA);
                if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                mCPU.Add(ci.mCPU);
                ci.bValid = true;   // do not really know if it is valid yet
                UnsortedLCI.Add(ci);
                WUs++;
            }
            RunDTsort();
            return WUs;
        }

        public void GetTableFromNoHeader(ref cNoHeaderProj noHeaderProj)
        {
            UnsortedLCI.Clear();
            UnsortedDT.Clear();
            mELA.Clear();
            mCPU.Clear();
            foreach (cNoHeaderBody nhb in noHeaderProj.NoHdrList)
            {
                if (nhb != null)
                {
                    cCreditInfo ci = new cCreditInfo();
                    ci.tCompleted = nhb.tCompleted;
                    ci.ElapsedSecs = nhb.ElapsedSecs;
                    ci.CPUtimeSecs = nhb.CPUtimeSecs;
                    ci.Credits = nhb.Credits;
                    if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                    ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                    ci.mELA = nhb.CPUtimeSecs;
                    mELA.Add(ci.mELA);
                    mCPU.Add(ci.mCPU);
                    ci.bValid = true;
                    ci.nCnt = 0;    // 2025 seems to be used when averaging ???
                    UnsortedLCI.Add(ci);
                    UnsortedDT.Add(ci.tCompleted);
                }
            }
            if (UnsortedDT.Count > 0)
                RunDTsort();
        }

        private int BuildEinsteinStatsTable(ref string RawTable, ref RunList runList)
        {
            int i, j, k;
            string sDataKey = "</td><td>Completed and validated</td><td>";
            double t;
            string[] RawLineValues;
            string[] eDTformats = { "d MMM yyyy H:mm:ss UTC", "dd MMM yyyy H:mm:ss UTC", };
            runList.RawLines = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int NumberToCollect = runList.RawLines.Length - 1;
            int NumberRecordsRead = 0;
            if (NumberToCollect <= 0) return -1;

            for (i = 1; i < (1 + NumberToCollect); i++) // first data line starts as row 1, not 0
            {
                string s = runList.RawLines[i];
                j = s.IndexOf(sDataKey); // to the left is a date and the right is data
                if (j > 0)
                {
                    cCreditInfo ci = new cCreditInfo();
                    k = s.LastIndexOf("</td><td>", j);
                    k += 9;

                    RawLineValues = s.Substring(k).Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);

                    string sDT = RawLineValues[0];
                    if (DateTime.TryParseExact(sDT, eDTformats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTime1))
                    {
                        ci.tCompleted = dateTime1;
                        UnsortedDT.Add(dateTime1);
                    }
                    else
                    {
                        runList.sMsgErr += "Error - bad date time string";
                        RunDTsort();
                        return -1;
                    }

                    ci.ElapsedSecs = Convert.ToDouble(RawLineValues[2]);
                    ci.CPUtimeSecs = Convert.ToDouble(RawLineValues[3]);
                    ci.Credits = Convert.ToDouble(RawLineValues[4]);

                    ci.mELA = ci.Credits / ci.ElapsedSecs;
                    mELA.Add(ci.mELA);
                    if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                    ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                    mCPU.Add(ci.mCPU);
                    ci.bValid = true;   // do not really know if it is valid yet
                    UnsortedLCI.Add(ci);
                    NumberRecordsRead++;
                }
                else
                {
                    runList.sMsgErr += "Error - could not find first value on the page";
                    RunDTsort();
                    return -1;
                }
            }
            RunDTsort();
            return 0;
        }


        private int BuildStatsTable(ref string RawTable, ref RunList runList)
        {
            int i;
            int nWUs = 0;
            runList.RawLines = RawTable.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (i = 0; i < 21; i++)    // first line of data must be within first 21 or so lines
            {
                if (runList.RawLines[i].Contains("\"right\">"))
                {
                    int iFirst = i;
                    while (true)
                    {
                        cCreditInfo ci = new cCreditInfo();
                        ci.tCompleted = GetValueT(ref runList, iFirst, -2);
                        if (ci.tCompleted == DateTime.MinValue)
                        {
                            break; ;
                        }
                        UnsortedDT.Add(ci.tCompleted);
                        ci.ElapsedSecs = GetValueD(ref runList, iFirst, 0);
                        ci.CPUtimeSecs = GetValueD(ref runList, iFirst, 1);
                        ci.Credits = GetValueD(ref runList, iFirst, 2);
                        ci.mELA = ci.Credits / ci.ElapsedSecs;
                        mELA.Add(ci.mELA);
                        if (ci.CPUtimeSecs == 0.0) ci.CPUtimeSecs = 0.01;
                        ci.mCPU = ci.Credits / ci.CPUtimeSecs;
                        mCPU.Add(ci.mCPU);
                        ci.bValid = true;   // do not really know if it is valid yet
                                            //LCreditInfo.Add(ci);
                        UnsortedLCI.Add(ci);
                        iFirst += 10;
                        nWUs++;
                    }
                    RunDTsort();
                    return nWUs;
                }
            }
            runList.sMsgErr += "Error - could not find first value on the page\n";
            return -2;
        }

        private void RunDTsort()
        {
            indicesCreditInfo = Enumerable.Range(0, UnsortedDT.Count).ToList();
            indicesCreditInfo.Sort((i1, i2) => UnsortedDT[i2].CompareTo(UnsortedDT[i1]));
            LCreditInfo = indicesCreditInfo.Select(i => UnsortedLCI[i]).ToList();
        }

        public static (List<double>, List<int>) RemoveOutliersWithIndexes(ref List<double> data, out double std, double threshold = 2)
        {
            double mean = data.Average();
            double stdDev = Math.Sqrt(data.Average(v => Math.Pow(v - mean, 2)));
            std = stdDev;
            // Track indexes of removed outliers
            List<int> outlierIndexes = new List<int>();

            // Filter data while keeping track of original indexes
            List<double> filteredData = data
                .Select((value, index) => new { value, index }) // Store original index
                .Where(item =>
                {
                    bool isOutlier = Math.Abs(item.value - mean) > threshold * stdDev;
                    if (!isOutlier) outlierIndexes.Add(item.index);
                    return !isOutlier;
                })
                .Select(item => item.value)
                .ToList();

            return (filteredData, outlierIndexes);
        }



        private double ApplyFilter(out double stdDev)
        {
            stdDev = 0.0;
            double sem;   // standard error of the mean
            if (mCPU.Count == 0)
            {
                return 1.0;
            }
            foreach (cCreditInfo ci in LCreditInfo)
            {
                ci.bValid = true;
            }
            if (cbfilterSTD.Checked)
            {
                cNAS.data = null;
                cNAS.outlierIndexes = null;
                (cNAS.data, cNAS.outlierIndexes) = RemoveOutliersWithIndexes(ref mELA, out stdDev, 2.0);
                for (int k = 0; k < mELA.Count; k++)
                {
                    LCreditInfo[k].bValid = false;
                }
                foreach (int k in cNAS.outlierIndexes)
                {
                    int ik = indicesCreditInfo.IndexOf(k);
                    LCreditInfo[ik].bValid = true;
                }
            }
            sem = stdDev / Math.Sqrt(mELA.Count);
            return sem;
        }


        private void GetResults(ref RunList runList)
        {
            runList.RawLines = null;
            runList.sOutInfo = "";
            runList.BGinfo = "";
            int n = 0;
            cCreditInfo SumC = new cCreditInfo();
            double dH = 0;  // hours
            long Sd = 0;
            long Ed = 0;
            SumC.Init(runList.PCname);
            SumC.stdCI = 1.0 + 1.96 * ApplyFilter(out SumC.std); // 1.96 is %95 confidence
            string sTotals = "";
            string sOut = "\n";
            string sOutHdrs = "Num" + Rp("     Date Completed", 22) + "    Credit     RunTime     RunTime     CPUtime     CPUtime  Valid" + Environment.NewLine;
            sOutHdrs += "   " + Rp(" ", 22) + "    Points        Secs     Credits        Secs     Credits";
            //lbHdr.Text += sOutHdrs;
            for (int i = 0; i < LCreditInfo.Count; i++)
            {
                cCreditInfo ci = LCreditInfo[i];
                string dtS = ci.tCompleted.ToString("yyyy-MM-dd HH:mm:ss");
                if (i == 0) Ed = (long)ci.tCompleted.Ticks;
                if (i == (LCreditInfo.Count - 1))
                {
                    Sd = (long)ci.tCompleted.Ticks;
                    dH = (double)((double)(Ed - Sd) / TimeSpan.TicksPerHour);
                }

                sOut += Lp((i + 1).ToString(), 3) + " "
                    + Rp(dtS, 22)
                    + Lp(ci.Credits.ToString("F2"), 10)
                    + Lp(ci.ElapsedSecs.ToString("F2"), 12)
                    + Lp(ci.mELA.ToString("F4"), 12)
                    + Lp(ci.CPUtimeSecs.ToString("F2"), 12)
                    + Lp(ci.mCPU.ToString("F4"), 12)
                    + (ci.bValid ? "" : "   X")
                    + "\r\n";

                if (ci.bValid)
                {
                    SumC.dHours += dH;
                    SumC.Credits += ci.Credits;
                    SumC.ElapsedSecs += ci.ElapsedSecs;
                    SumC.CPUtimeSecs += ci.CPUtimeSecs;
                    SumC.mELA += ci.mELA;
                    SumC.mCPU += ci.mCPU;
                    n++;
                }
                else
                {

                }
            }
            SumC.nCnt = n;
            if (n > 0)
            {
                SumC.mELA /= n;
                SumC.mCPU /= n;
                SumC.ElapsedSecs /= n;
                SumC.CPUtimeSecs /= n;
            }
            sOut += Environment.NewLine;
            string s1 = Lp(SumC.nCnt.ToString(), 3)
                + Rp("   Hours:" + dH.ToString("F2").PadLeft(7), 23)
                + Lp(SumC.Credits.ToString("F2"), 10)
                + Lp(SumC.ElapsedSecs.ToString("F2"), 12)
                + Lp(SumC.mELA.ToString("F4"), 12)
                + Lp(SumC.CPUtimeSecs.ToString("F2"), 12)
                + Lp(SumC.mCPU.ToString("F4"), 12)
                + "\r\n";

            sTotals = s1 + Lp(runList.PCname, 25) + Lp("Total", 10) // put hostname here
                + Lp("Avg", 12)
                + Lp("Avg", 12)
                + Lp("Avg", 12)
                + Lp("Avg", 12);


            if (bInSequencer)
            {
                ts.selProjList.newCnt = SumC.nCnt;
                ts.selProjList.newPoints = Convert.ToInt32(SumC.Credits / SumC.nCnt);
                ts.selProjList.newAvg = Convert.ToInt32(SumC.stdCI * SumC.ElapsedSecs);
                ts.selProjList.newStd = SumC.std;
                if (SumC.ElapsedSecs == 0)
                {
                    runList.BGinfo += "ZERO NEW WORK for " + ts.sHostName + " " + ts.sStudy + " study:" + ts.sStudy + Environment.NewLine;
                }
                else
                {
                    string sF = (SumC.stdCI * SumC.ElapsedSecs).ToString("F0");
                    runList.BGinfo += runList.PCname + ":" + ts.ShortName + " " + (SumC.stdCI * SumC.ElapsedSecs).ToString("F0") + " CI:" + SumC.stdCI.ToString("F4") + Environment.NewLine;
                    string sG = "0";
                    if (SumC.nCnt > 0)
                        sG = Convert.ToString(ts.selProjList.Points);
                    runList.BGinfo += "zz Count: " + SumC.nCnt.ToString().PadLeft(4) + SumC.PCname.PadLeft(16) + ts.ShortName.PadLeft(16) + sF.PadLeft(8) + sG.PadLeft(8) + Environment.NewLine;
                }

                runList.sOutInfo += sOutHdrs + Environment.NewLine + sOut + sTotals;

                /*

                ts.SeqOverallText.Add(s1.TrimEnd() + " " + SumC.PCname);
                double dN = SumC.nCnt;
                ts.SeqTotals.nCnt += SumC.nCnt;
                ts.SeqTotals.Credits += SumC.Credits;
                ts.SeqTotals.ElapsedSecs += dN * SumC.ElapsedSecs;
                ts.SeqTotals.CPUtimeSecs += dN * SumC.CPUtimeSecs;
                ts.SeqTotals.mELA += dN * SumC.mELA;
                ts.SeqTotals.mCPU += dN * SumC.mCPU;
                ts.SeqTotals.dHours += SumC.dHours;
                ts.selPClimit.SumCreSecs = SumC.mELA;

                */

            }
            else
            {
                tbInfo.Text += sOutHdrs + Environment.NewLine + sOut + sTotals + Environment.NewLine;
            }
        }

        private void ClearInfoDisplay()
        {
            tbInfo.Clear();
            tbHdrInfo.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (cts == null) return;
            readSitePage.Cancel();
            try
            {
                var token = cts.Token;  // This will throw if disposed
                if (cts.Token.CanBeCanceled)
                    cts.Cancel();
            }
            catch (ObjectDisposedException)
            {

            }
            ts.StopSequencing = true;
        }

        private void SetPBseq()
        {
            if (ts.NumSeqPC > 0)
            {
                pbSeq.Maximum = 1 + ts.NumSeqPC * ts.NumSeqPJ;
                pbSeq.Value = ts.SeqInxPC * ts.NumSeqPJ + ts.SeqInxPJ;
            }
            else
            {
                pbSeq.Maximum = 1;
                pbSeq.Value = 1;
            }
        }

        private void SelectPCsfromCheckboxes()
        {
            foreach (cPClimit pcl in ts.PClimit)
            {
                pcl.IsSelected = IsPCchecked(pcl.PCname);
                foreach (cCalcLimitProj clp in pcl.ProjList)
                    clp.IsSelected = IsPJchecked(clp.ShortName);  //8/25/2025 was used in sprint
            }
            foreach (cPSlist psl in ProjectStats.ProjectList)
                psl.IsSelected = IsPJchecked(psl.shortname);

        }

        private void btnRunSprint_Click(object sender, EventArgs e)
        {
            if (ts.PClimit.Count > 0)
            {
                ClearInfoDisplay();
                ClearRB();
                pbSeq.Value = 0;
                SelectPCsfromCheckboxes();
                ts.SeqInxPC = ts.pc.GetFirstSelected();
                ts.SeqInxPJ = ts.pc.GetFirstInSprint();
                ts.NumSeqPC = ts.pc.GetNumSelected();
                ts.NumSeqPJ = ts.pc.GetNumInSprint();
                bInSequencer = ts.NumSeqPC > 1 || ts.NumSeqPJ > 1;
                ts.OnStartup = true;
                ts.StopSequencing = false;
                ts.rbPLcheckedShortName = "";
                StartNext();
            }
        }

        private void StopSequencing()
        {
            ts.selPClimit = ts.PClimit[ts.pc.GetFirstSelected()];   // this is the PClimit for the PC we are working on
            ts.selPlist = ts.ProjectList[ts.pc.GetFirstInSprint()]; // this is the ProjectList for the project we are working on
            ts.ShortName = ts.selPlist.shortname;                   // name of project from the ProjectList
            RBsetProjectChecked(ts.ShortName);
            SequenceConcluded();
        }

        private void StartNext()
        {
            if (!ts.OnStartup)
            {
                ts.SeqInxPJ = ts.pc.GetNextInSprint(ts.SeqInxPJ);
                if (ts.SeqInxPJ < 0)
                {
                    ts.SeqInxPC = ts.pc.GetNextSelected(ts.SeqInxPC);
                    ts.SeqInxPJ = ts.pc.GetFirstInSprint();
                    if (ts.SeqInxPC < 0)
                    {
                        StopSequencing();
                        return;
                    }
                }
            }

            if(ts.StopSequencing)
            {
                StopSequencing();
                return;
            }

            ClearResultInfo();

            ts.selPClimit = ts.PClimit[ts.SeqInxPC];    // this is the PClimit for the PC we are working on
            ts.selPlist = ts.ProjectList[ts.SeqInxPJ];  // this is the ProjectList for the project we are working on
            ts.ShortName = ts.selPlist.shortname;       // name of project from the ProjectList
            RBenableProject(ts.ShortName);

            ts.sHostName = ts.selPClimit.PCname;          // name of the PC from the PClimit list

            RBenableSystem(ts.sHostName);
            if (ts.OnStartup) RBsetSystemChecked(ts.sHostName); // set the radio button for the PC to checked

            ts.selProjList = ts.selPClimit.GetProjStruct(ts.ShortName); // this is the ProjList for the project ShortName on the PC sHostName
            HDRresults = ts.selProjList.HDRresults;
            ts.hi = ProjectStats.ManagedPCs.NameToSystem(ts.sHostName);     // from the managed PC, we need the list of project IDs
            ts.ProjID = ts.hi.GetProjectID(ts.ShortName);                   // project ID for the project ShortName on the PC
            int iLoc = ts.selPClimit.GetProjNameIndex(ts.ShortName);
            // iLoc is the index into the ProjList of the sprint for the PC not the index into the main ProjectList
            //ts.sUrl = ts.selPClimit.ProjList[iLoc].FormAcqUrl(ts.ProjID, ref ts.selPlist);
            ts.sUrl = ts.selPlist.FormURL(ts.ProjID);// ts.selPClimit.ProjList[iLoc].FormAcqUrl(ts.ProjID, ref ts.selPlist);
            ts.selProjList.AcqUrl = ts.sUrl; // Save it so we can view later if we want
            // this is the acquisition URL for the project ShortName on the PC sHostName, not the pandora URL
            //globals.OpenUrl(ts.sUrl);

            ts.sStudy = ts.selPClimit.ProjList[iLoc].sStudy;  // possibly ts.sStudy is not the same one
            if (ts.sStudy == "")
            {
                ts.sStudy = "0";   // because I could not store "" in projectlist's sStudyV
                ts.selPClimit.ProjList[iLoc].sStudy = "0";  //todo to do 8/23/2025 should have been 0 to start with!!!
            }
            Debug.Assert(ts.sStudy == ts.selPlist.sStudyV, "sStudy in PClimit and selPlist do not match");

            ts.OnStartup = false;

            sigRun.PCname = ts.sHostName;
            sigRun.NextOperation = "next";
            sigRun.shortname = ts.ShortName;
            sigRun.bDone = false;
            sigRun.bBusy = true;
            if (pbSeq.Value < pbSeq.Maximum - 1)
                pbSeq.Value++;
            tbHdrInfo.Text += "PC:" + sigRun.PCname + " Project:" + sigRun.shortname + " Study:" + ts.sStudy + NL;
            tbInfo.Text += ts.sUrl + NL;
            Application.DoEvents();

            timerDoSelected.Enabled = true;
            readSitePage.ReadProjectThisSite(ts.ShortName, ts.sUrl, ref sigRun);
        }

        private string FormGC(ref cPClimit cpl, string shortname)
        {
            //cpu:  2(4) means there are 2 concurrent tasks and each tasks uses 4 cpus
            //gpu:  1(2) means each gpu can run 2 tasks concurrently
            if (shortname == "")
                return cpl.ConcurrentCpuTasks.ToString() + "\\" + cpl.ConcurrentGpuTasks.ToString();
            string cg = "";
            cCalcLimitProj clp = cpl.GetProjStruct(shortname);
            string sType = clp.AppType;
            switch (sType)
            {
                case "cpu":
                    return "C:" + clp.ShowUsedCpuGpu;
                    break;
                case "gpu":
                    return "G:" + clp.ShowUsedCpuGpu;
                    break;
            }
            return cpl.ConcurrentCpuTasks.ToString() + "\\" + cpl.ConcurrentGpuTasks.ToString();
        }

        private void btnViewCol_Click(object sender, EventArgs e)
        {
            string NL = Environment.NewLine;
            tbHdrInfo.Clear();
            tbCurCol.Clear();
            SelectPCsfromCheckboxes();
            tcShowData.SelectTab("tabResults");
            string sOut = "";
            ts.NumSeqPC = 0;
            ts.NumSeqPJ = 0;
            int nC = 0; // first column 
            int nColWidth = 0;
            int SummedCPUs = 0;
            ts.pc.GetLongest(out int nPC, out int nPJ);
            int nPad = Math.Max(nPC, nPJ);
            nColWidth = nPad + 2 + 5 + 12 + 5;
            nColWidth += 12;    // width of one paragraph plus the space to the next
            List<string> tString = new List<string>();
            List<int> PCptr = new List<int>();
            //string tHdr = "PC".PadRight(nPad + 2) + "Cnt".PadRight(5) + "Avg".PadRight(12) + "Limit" + NL;
            string tOut = "";
            int iPtr = 0;
            List<string> ProblemProj = new List<string>();
            List<string> ProblemInfo = new List<string>();
            foreach (cPClimit pcl in ts.pc.PandoraDatabase)
            {
                cPClimit pcx = pcl;                
                if (pcl.IsSelected)
                {
                    PCptr.Add(iPtr++);
                    tOut = pcl.PCname.PadRight(nPad + 2) + "Cnt".PadRight(5) + "Avg".PadRight(12) + "Limit " + FormGC(ref pcx, "");
                    tString.Add(tOut);
                    tOut += NL;
                    foreach (cCalcLimitProj clp in pcl.ProjList)
                    {
                        if (clp.IsSelected)
                        {
                            iPtr++;
                            sOut = "";
                            sOut += clp.ShortName.PadRight(nPad) + clp.Cnt.ToString().PadLeft(5) + "  ";
                            CheckProblems(clp.ShortName, clp.Cnt, ref ProblemProj, ref ProblemInfo);
                            sOut += (clp.Avg.ToString("F2")).PadRight(8) + " ";
                            sOut += (clp.LimitValue.ToString()).PadLeft(8);
                            sOut += " " + FormGC(ref pcx, clp.ShortName);
                            tString.Add(sOut);
                            tOut += sOut + NL;
                        }
                    }
                }
            }
            PCptr.Add(iPtr); // add the end pointer
            List<List<string>> paragraphs = new List<List<string>>();
            for (int i = 0; i < PCptr.Count - 1; i++)
            {
                int start = PCptr[i];
                int end = PCptr[i + 1];
                paragraphs.Add(tString.GetRange(start, end - start));
            }
            tOut = "";
            for (int i = 0; i < paragraphs.Count; i += 2)
            {
                List<string> left = paragraphs[i];
                List<string> right = (i + 1 < paragraphs.Count) ? paragraphs[i + 1] : new List<string>();

                int maxLines = Math.Max(left.Count, right.Count);

                for (int line = 0; line < maxLines; line++)
                {
                    string l = line < left.Count ? left[line] : "";
                    string r = line < right.Count ? right[line] : "";
                    tOut += $"{l.PadRight(nColWidth)}{r}" + NL;
                }

                tOut += NL;
            }

            tcShowData.SelectTab("tabCurCol");
            tbCurCol.Text = tOut;
            SetTextSafe(tcShowData, "tabCurCol", tbCurCol, tOut);

            sOut = "";
            if (ProblemProj.Count > 0)
            {
                sOut = "Possible problems with the following projects:" + NL;
                nPC = 0;
                nPJ = 0; ;
                for (int i = 0; i < ProblemProj.Count; i++)
                {
                    nPC = Math.Max(nPC, ProblemProj[i].Length);
                    nPJ = Math.Max(nPJ, ProblemInfo[i].Length);
                }
                for (int i = 0; i < ProblemProj.Count; i++)
                {
                    sOut += (ProblemProj[i] + ":").PadRight(nPC) + " " + ProblemInfo[i].PadRight(nPJ) + NL;
                }
                tbHdrInfo.Text = sOut.Trim() + NL;
            }

        }

        private void CheckProblems(string shortName, int cnt, ref List<string> ProblemProj, ref List<string> ProblemInfo)
        {
            int i = ProblemProj.IndexOf(shortName);
            if (cnt == 0)
            {
                if (i < 0)
                {
                    ProblemProj.Add(shortName);
                    ProblemInfo.Add("No data");
                }
                else
                {
                    ProblemInfo[i] = "No data, cannot calculate a limit";
                }
            }
            else if (cnt < 9) // using a limit of 10 there is usually at least 1 failure
            {
                if (i < 0)
                {
                    ProblemProj.Add(shortName);
                    ProblemInfo.Add("Insufficient data, poor limit");
                }
            }
        }

        private void FormProjectsCB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            int i = 0;
            int rbRow = 0;
            int rbCol = 0;
            foreach (cCalcLimitProj clp in ProjectStats.TempletDB.ProjList)
            {
                CheckBox cb = new CheckBox();
                cb.Tag = i;
                cb.Text = clp.ShortName;
                cb.AutoSize = true;
                cb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                cb.Checked = clp.IsSelected;
                cb.ForeColor = clp.UsedInSprint ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                gbPJs.Controls.Add(cb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = clp.ShortName;
                rb.Location = new System.Drawing.Point(oCol + rbCol * 120, oRow / 2 + rbRow * 24);
                rb.Enabled = false;
                rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                gbProj.Controls.Add(rb);
                rbCol++;
                if (rbCol == 6)
                {
                    rbCol = 0;
                    rbRow++;
                }
                i++;
            }
        }

        private void rbProject_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked) return;
            ts.rbPLcheckedShortName = rb.Text;
            ts.selProjList = ts.selPClimit.GetProjStruct(ts.rbPLcheckedShortName);
            if (ts.selProjList == null) return;
            ts.sUrl = ts.selProjList.AcqUrl;
            tbEachResult.Text = ts.selProjList.sResultInfo;
        }

        private void FormSystemsCB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            int i = 0;
            int rbRow = 0;
            int rbCol = 0;
            foreach (cPClimit pcl in ts.pc.PandoraDatabase)
            {
                CheckBox cb = new CheckBox();
                cb.Tag = i;
                cb.Text = pcl.PCname;
                cb.AutoSize = true;
                cb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                cb.Checked = pcl.IsSelected;
                gbPCs.Controls.Add(cb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = pcl.PCname;
                rb.Location = new System.Drawing.Point(oCol + rbCol * 120, oRow / 2 + rbRow * 24);
                rb.Enabled = false;
                rb.CheckedChanged += new System.EventHandler(this.rbSystem_CheckedChanged);
                gbSys.Controls.Add(rb);
                rbCol++;
                if (rbCol == 6)
                {
                    rbCol = 0;
                    rbRow++;
                }
                i++;
            }
        }

        private void rbSystem_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked) return;
            ts.selPClimit = ts.pc.GetPCbyName(rb.Text);
            if (ts.rbPLcheckedShortName == "") return; // no project selected yet
            ts.selProjList = ts.selPClimit.GetProjStruct(ts.rbPLcheckedShortName);
            if (ts.selProjList == null) return;
            ts.sUrl = ts.selProjList.AcqUrl;
            tbEachResult.Text = ts.selProjList.sResultInfo;
        }

        private void RBenableSystem(string pcName)
        {
            foreach (Control c in gbSys.Controls)
            {
                if (c is RadioButton rb)
                {
                    if (rb.Text == pcName)
                    {
                        rb.Enabled = true;
                        rb.ForeColor = System.Drawing.Color.Blue;
                        return;
                    }
                }
            }
        }

        private void RBenableProject(string shortname)
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is RadioButton rb)
                {
                    if (rb.Text == shortname)
                    {
                        rb.Enabled = true;
                        rb.ForeColor = System.Drawing.Color.Blue;
                        return;
                    }
                }
            }
        }

        private void RBsetProjectChecked(string shortname)
        {
            foreach (Control c in gbProj.Controls)
            {
                if (c is RadioButton rb && rb.Text == shortname)
                {
                    rb.Checked = true;
                }
            }
        }

        private void RBsetSystemChecked(string pcName)
        {
            foreach (Control c in gbSys.Controls)
            {
                if (c is RadioButton rb && rb.Text == pcName)
                {
                    rb.Checked = true;
                }
            }
        }

        private void ClearRB()
        {
            foreach (Control c in gbSys.Controls)
            {
                if (c is RadioButton rb)
                {
                    rb.Enabled = false;
                    rb.Checked = false;
                    rb.ForeColor = System.Drawing.Color.Black;
                }
            }
            foreach (Control c in gbProj.Controls)
            {
                if (c is RadioButton rb)
                {
                    rb.Enabled = false;
                    rb.Checked = false;
                    rb.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private bool IsPJchecked(string shortname)
        {
            foreach (Control c in gbPJs.Controls)
            {
                if (c is CheckBox cb && cb.Text == shortname)
                {
                    return cb.Checked;
                }
            }
            return false;
        }

        private bool IsPCchecked(string pcname)
        {
            foreach (Control c in gbPCs.Controls)
            {
                if (c is CheckBox cb && cb.Text == pcname)
                {
                    return cb.Checked;
                }
            }
            return false;
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
                            switch (sOP)
                            {
                                case "Check all":
                                    cb.Checked = true;
                                    break;
                                case "Clear all":
                                    cb.Checked = false;
                                    break;
                                case "Invert":
                                    cb.Checked = !@cb.Checked;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void btnVisitPage_Click(object sender, EventArgs e)
        {
            globals.OpenUrl(ts.sUrl);
        }

        private void btnShowAnal_Click(object sender, EventArgs e)
        {
            if (ts.selProjList == null) return; // no data taken
            string sOut = tbEachResult.Text = ts.selProjList.sResultInfo + NL + NL;
            sOut += "PC:" + ts.sHostName + " Project:" + ts.ShortName + " Study:" + (ts.sStudy == "" ? "0" : ts.sStudy) + NL;
            sOut += "New Statistics".PadRight(46) + "Old Statistics" + NL;
            sOut += "Cnt".PadLeft(10) + "Avg".PadLeft(12) + "Points".PadLeft(12) + "Std".PadLeft(12) + "Cnt".PadLeft(10) + "Avg".PadLeft(12) + "Points".PadLeft(12) + "Std".PadLeft(12) + NL;
            sOut += ts.selProjList.newCnt.ToString().PadLeft(10)
                + ts.selProjList.newAvg.ToString("F2").PadLeft(12)
                + ts.selProjList.newPoints.ToString().PadLeft(12)
                + ts.selProjList.newStd.ToString("F4").PadLeft(12)
                + ts.selProjList.Cnt.ToString().PadLeft(10)
                + ts.selProjList.Avg.ToString("F2").PadLeft(12)
                + ts.selProjList.Points.ToString().PadLeft(12)
                + ts.selProjList.Std.ToString("F4").PadLeft(12) + NL;
            tbEachResult.Text = sOut;
        }

        private void btnLCalcGPU_Click(object sender, EventArgs e)
        {
            ts.pc.IncludeSuccessfullJobs = cbUseSecc.Checked;
            string sOut = ts.pc.CalcLimits(RemainingLimitDays());            
            SetTextHidden(tbInfo, sOut);
        }

        private void btnApplyLimit_Click(object sender, EventArgs e)
        {
            foreach (cPClimit pcl in ts.PClimit)
            {
                cPClimit pcx = pcl;
                ts.pc.WriteDBrecord(ref pcx);
                ts.pc.WritePandoraConfig(ref pcx);
            }
        }

        private void btnSaveSystems_Click(object sender, EventArgs e)
        {
            foreach (cPClimit pcl in ts.PClimit)
            {
                string pcName = pcl.PCname;
                bool b = IsPCchecked(pcl.PCname);
                pcl.IsSelected = b;
                cPClimit pcx = pcl;
                ts.pc.WriteDBrecord(ref pcx);
            }
        }

        private double GetDaysTillStart()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            TimeSpan diff = ProjectStats.UTCstart - now;
            if (diff.TotalDays < 0.0) return 0.0;
            return diff.TotalDays;
        }

        private void ShowSprintDays()
        {
            TimeSpan SprintLength = ProjectStats.UTCend - ProjectStats.UTCstart;
            double SprintDays = SprintLength.TotalDays;
            double DaysToStart = GetDaysTillStart();
            double LimitDays = 0;

            LimitDays = DaysToStart;
            if(DaysToStart > SprintDays)
                LimitDays = SprintDays;
            if(DaysToStart < 0)
                LimitDays = 0;
            if (DaysToStart > 14)
                lbRemDays.Text = "over 2 weeks";
            else lbRemDays.Text = "Start in " + DaysToStart.ToString("F2") + " Days";
            lbBunkerDays.Text = LimitDays.ToString("F2") + " valid limit days";
        }

        private double RemainingLimitDays()
        {
            TimeSpan SprintLength = ProjectStats.UTCend - ProjectStats.UTCstart;
            double SprintDays = SprintLength.TotalDays;
            double DaysToStart = GetDaysTillStart();
            double LimitDays = 0;

            LimitDays = DaysToStart;
            if (DaysToStart > SprintDays)
                LimitDays = SprintDays;
            if (DaysToStart < 0)
                LimitDays = 0;
            return LimitDays;
        }
    }
}
