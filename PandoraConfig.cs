using Microsoft.Playwright;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml;
using System.Xml.Linq;
using static CreditStatistics.globals;
using static CreditStatistics.globals.cAllProjectStudyInfo;
using static CreditStatistics.PandoraConfig;
using static CreditStatistics.ShowData;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CreditStatistics
{
    internal class PandoraConfig
    {
        private cProjectStruct ProjectStats;
        private cManagedPCs ManagedPCs;
        private List<cPSlist> ProjectList;
        public List<cPClimit> PandoraDatabase;

        private List<bool> SelectedItems = new List<bool>();
        public bool IncludeSuccessfullJobs = false;
        public class cCalcLimitProj
        {
            public string ProjUrl;  // this is the master url
            public string AcqUrl;   // data acquisition url with valid study
            public string ShortName;
            public string sPCname;
            public string sStudy;
            public string fullStudyName;
            public string AppType; // GPU or CPU:  only one type can be run
            public string BunkerStart;
            public string BunkerEnd;
            public string ProjID; // the project ID is used by the website to identify the PC.
            public string strResult;    // for error or status info
            public string ShowUsedCpuGpu;   //cpu:  2(4) means there are 2 concurrent tasks and each tasks uses 4 cpus
                                            //gpu:  1(2) means each gpu can run 2 tasks concurrently
            public int ErrorStatus;     //0: no error, 1: info, 2: warning, 3: critical error, 4: fatal error
            public double gpu_usage;    // 0.5 means two apps can run on the GPU
            public double cpu_usage;    // .25 means on 1/4 of a cpu is needed for a single GPU app
            public double avg_ncpus;    // 4 means it 4 CPUs are used by the cpu bound app
            public double ngpus;        // think this is the same as the gpu_usage
            public int MaxApps;         // if 0 then no limit//, if 2 then maximum of 2 concurrent apps
            public int Avg;
            public int Cnt;
            public int Points;
            public double Std;
            public int LimitValue;
            public double newAvg;
            public int newCnt;
            public int newPoints;
            public double newStd;
            public int newLimitValue;
            public int ReadyToReport; // Number bunkered
            public int JobsFailed;   // number of bad work units
            public int JobsSucceeded;
            public int TotalWUcnt; // includes ones being worked on and downloading
            public int BlockCnt;
            public string PrevCnt, PrevAvg, PrevStudy, PrevPoints, PrevLimit, PrevStd;
            public string PrevStart, PrevEnd, PrevAppType;
            public int[] HDRresults = new int[] { 0, 0, 0, 0 };
            public string sResultInfo = "";
            public bool UsedInSprint;
            public bool UsedInGames; // the default templete is always used in games but not possibly not in sprint
            public bool IsSelected; // used when collecting data but not in sprint
            public bool ProjectDisabled;
            public bool ProjectSuspended;
            public bool ProjectNoNewWork;
            public void init(string sProjUrl, bool bDisabled)
            {
                ProjUrl = sProjUrl;
                strResult = "";
                ErrorStatus = 0; // no error    
                ProjID = "";
                sStudy = "";
                fullStudyName = "";
                AppType = "";
                Avg = 0;
                Cnt = 0;
                ngpus = 0;
                avg_ncpus = 0;
                gpu_usage = 0;
                cpu_usage = 0;
                MaxApps = 0;
                BunkerStart = "0";
                BunkerEnd = "0";
                Points = 0;
                ReadyToReport = 0;
                JobsFailed = 0;
                JobsSucceeded = 0;
                TotalWUcnt = 0;
                LimitValue = 0;
                BlockCnt = 0;
                PrevAvg = "";
                PrevCnt = "";
                PrevPoints = "";
                PrevStd = "";
                PrevStudy = "";
                PrevStart = "";
                PrevEnd = "";
                PrevAppType = "";
                UsedInSprint = false;
                ProjectDisabled = bDisabled;
                ProjectSuspended = false;
                ProjectNoNewWork = false;
                UsedInGames = false;
            }

            public string FormAcqUrl(string ProjID, ref cPSlist psi)
            {
                string s = ProjUrl + psi.sHid + ProjID + ((psi.sValid == "null") ? "" : psi.sValid);
                s += psi.sStudy == "null" ? "" : psi.sStudy + sStudy;
                if (psi.sPage != "null")
                    s += psi.sPage + "0";
                AcqUrl = s;
                return s;
            }

            public void CopyTemplet(cCalcLimitProj ep)
            {
                init(ep.ProjUrl, ep.ProjectDisabled);
                sStudy = ep.sStudy;
                AppType = ep.AppType;
                BunkerStart = ep.BunkerStart;
                BunkerEnd = ep.BunkerEnd;
            }


            public int FindStudyIndex(string sS)
            {
                if (PrevStudy == "") return -1;
                string[] Studies = PrevStudy.Split(',');
                int n = Array.IndexOf(Studies, sS);
                return n;
            }

            public int UpdateStudy(ref cCalcLimitProj p)
            {
                int nAdded = 0;
                if (p.sStudy != sStudy)
                    ChangeStudy(p);
                return nAdded;
            }

            public string CompareStudies(cCalcLimitProj p)
            {
                string sOut = "";
                if (p.sStudy != sStudy)
                {
                    sOut += "Changing study from " + sStudy + " to " + p.sStudy + Environment.NewLine;
                    ChangeStudy(p);
                }
                else
                {
                    sOut += "Keeping study: " + sStudy + Environment.NewLine;
                }
                sOut += "Points: " + Points.ToString() + " => " + p.Points.ToString() + Environment.NewLine;
                sOut += "Avg: " + Avg.ToString("F2") + " => " + p.Avg.ToString("F2") + Environment.NewLine;
                sOut += "Cnt: " + Cnt.ToString() + " => " + p.Cnt.ToString() + Environment.NewLine;
                return sOut;
            }

            public void ChangeStudy(cCalcLimitProj p)
            {
                string NewStudyName = p.sStudy;

                // is this the initial study?  Does the previous study have 0 data values?
                // if so, then nothing to save, just clear and set the new study
                if (Cnt == 0)
                {
                    sStudy = NewStudyName; // the new one
                    Avg = p.Avg;
                    Cnt = p.Cnt;
                    Points = p.Points;
                    LimitValue = 0;
                    return;
                }

                // if no previous studies then create one to save it

                int nPS = PrevStudy.Length;
                if (nPS == 0) // no previous study
                {
                    PrevStudy = sStudy;
                    PrevPoints = Points.ToString();
                    PrevStd = Std.ToString();
                    PrevAvg = Avg.ToString();
                    PrevCnt = Cnt.ToString();
                    PrevLimit = LimitValue.ToString();
                    PrevStart = BunkerStart;
                    PrevEnd = BunkerEnd;
                    PrevAppType = AppType;
                    sStudy = NewStudyName; // the new one
                    Points = 0;
                    Avg = 0;
                    Cnt = 0;
                    LimitValue = 0;
                    BunkerStart = p.BunkerStart;
                    BunkerEnd = p.BunkerEnd;
                    AppType = p.AppType;
                    return;
                }

                // there were previous studies so either update it or add it


                string[] Studies = PrevStudy.Split(',');
                string[] sPoints = PrevPoints.Split(',');
                string[] sStd = PrevStd.Split(',');
                string[] sAvgs = PrevAvg.Split(',');
                string[] sCnts = PrevCnt.Split(',');
                string[] sLimit = PrevLimit.Split(',');
                string[] sStarts = PrevStart.Split(',');
                string[] sEnds = PrevEnd.Split(',');
                string[] sAppType = PrevAppType.Split(',');

                int LocOldStudy = Array.IndexOf(Studies, sStudy); // old study

                if (LocOldStudy >= 0) // found old study so update it
                {
                    sPoints[LocOldStudy] = Points.ToString();
                    sAvgs[LocOldStudy] = Avg.ToString();
                    sCnts[LocOldStudy] = Cnt.ToString();
                    sLimit[LocOldStudy] = LimitValue.ToString();
                    sStarts[LocOldStudy] = BunkerStart;
                    sEnds[LocOldStudy] = BunkerEnd;
                    sAppType[LocOldStudy] = AppType;
                }
                else
                {
                    PrevStudy += "," + sStudy;
                    PrevPoints += "," + Points.ToString();
                    PrevStd += "," + Std.ToString();
                    PrevCnt += "," + Cnt.ToString();
                    PrevAvg += "," + Avg.ToString();
                    PrevLimit += "," + LimitValue.ToString();
                    PrevStart += "," + BunkerStart;
                    PrevEnd += "," + BunkerEnd;
                    PrevAppType += "," + AppType;
                }

                PrevStudy = string.Join(",", Studies);
                PrevPoints = string.Join(",", sPoints);
                PrevStd = string.Join(",", sStd);
                PrevCnt = string.Join(",", sCnts);
                PrevAvg = string.Join(",", sAvgs);
                PrevLimit = string.Join(",", sLimit);
                PrevStart = string.Join(",", sStarts);
                PrevEnd = string.Join(",", sAppType);
                PrevAppType = string.Join(",", sEnds);

                int LocNewStudy = Array.IndexOf(Studies, NewStudyName); // new study

                if (LocNewStudy < 0)
                {
                    sStudy = NewStudyName;
                    Points = 0;
                    Std = 0;
                    Avg = 0;
                    Cnt = 0;
                    LimitValue = 0;
                    BunkerStart = p.BunkerStart;
                    BunkerEnd = p.BunkerEnd;
                    AppType = p.AppType;
                    return;
                }

                sStudy = NewStudyName;
                Points = Convert.ToInt32(sPoints[LocNewStudy]);
                Avg = Convert.ToInt32(PrevAvg[LocNewStudy]);
                Std = Convert.ToInt32(sStd[LocNewStudy]);
                Cnt = Convert.ToInt32(PrevCnt[LocNewStudy]);
                LimitValue = Convert.ToInt32(PrevLimit[LocNewStudy]);
                BunkerStart = sStarts[LocNewStudy];
                BunkerEnd = sEnds[LocNewStudy];
                AppType = sAppType[LocNewStudy];
            }
        }

        public class cPClimit
        {
            public int ConcurrentGpuTasks = 0;
            public int ConcurrentCpuTasks = 0;
            public int TotalCPUsUsed = 0;
            public string PCname = "";
            public string UserName = "root";
            public string Password = "root";
            public string IPaddress = "";
            public string UTC_bunker_release = "";
            public string bunker_end = "";
            public string MessageFilters = "";
            public string OStype = ""; // w or l
            public string[]? cc_config = null;
            public string[]? pc_config = null;
            public string[]? app_config = null;
            public string strResult = ""; // for error or status info
            public int ErrorStatus = 0;   //0: no error, 1: info, 2: warning, 3: critical error, 4: fatal error
            public bool IsSelected = false;
            public List<cCalcLimitProj> ProjList = new List<cCalcLimitProj>();
            public double SumCreSecs;
            public cEachPc EachPCsHDR = new cEachPc();
            private List<double> SecondsAvailable; // how many seconds are left before we can stop bunkering
            private List<double> nDaysAvail = new List<double>();  //this is the time duration the project allows before the results is ignored
            private List<double> nDaysPossi = new List<double>();   //possible days are the minimum of the days available
            private List<double> DurationsBunkeredThisRun;
            public double NumberOfLimitDays;

            public string SumSprintCPUs()
            {
                int n = ConcurrentCpuTasks;
                int m = ConcurrentGpuTasks;
                string NL = Environment.NewLine;
                string sOut = "#CPUs(" + n.ToString() + ") -#GPUs(" + m.ToString() + ") -OS(1) =" + (n - m - 1).ToString() + NL;
                n = n - 1;
                foreach (cCalcLimitProj clp in ProjList)
                {
                    if (!clp.UsedInSprint) continue;
                    bool b = clp.AppType == "cpu";
                    string[] sN = clp.ShowUsedCpuGpu.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                    int sn1 = int.Parse(sN[0]);
                    int sn2 = int.Parse(sN[1]);
                    int sn3;
                    string sn4 = "";
                    if (b)
                    {
                        sn3 = sn1 * sn2;
                        sn4 = clp.ShowUsedCpuGpu.Trim() + "=> " + sn3.ToString().PadLeft(2);
                        sOut += sn4.PadRight(12) + clp.ShortName + NL;
                        n -= sn3;
                    }
                    else
                    {
                        sn4 = clp.ShowUsedCpuGpu.Trim() + "=>  0";
                        sOut += sn4.PadRight(12) + clp.ShortName + NL;
                    }
                }
                if (n < 0)
                {
                    sOut += "Error: Reduce CPUs by: " + (-n).ToString();
                }
                else if (n > 0)
                {
                    sOut += "Available CPUs: " + n.ToString() + NL;
                }
                else sOut += "No leftover CPUs" + NL;
                return sOut;
            }
            public void CalcSecondsLeft()
            {
                SecondsAvailable = new List<double>();
                DurationsBunkeredThisRun = new List<double>();  
                DateTime localTime;
                double dbunker_release = StringToTime(UTC_bunker_release, 0.0); //seconds since 1970
                DateTime bunker_release = GetReleaseLocal(UTC_bunker_release);
                string sLocalDate = DateToString(dbunker_release, out localTime);
                double StartDiff = (double)(bunker_release - DateTime.Now).TotalSeconds;

                foreach (cCalcLimitProj lp in ProjList)
                {
                    double d = Convert.ToDouble(lp.BunkerStart) * 86400;
                    d = Math.Abs(d);
                    if (StartDiff > d) d = 0;
                    else d = StartDiff;
                    SecondsAvailable.Add(Math.Abs(d));
                    DurationsBunkeredThisRun.Add(d / 86400);
                }
            }

            private void CalculateDurations()
            {
                nDaysAvail.Clear();
                nDaysPossi.Clear();
                double[] Durations = DurationsBunkeredThisRun.ToArray();
                double iMin = 100; ;
                for (int i = 0; i < Durations.Length; i++)
                {
                    double iDur = Durations[i];
                    iMin = Math.Min(iDur, iMin);
                    nDaysPossi.Add(iDur);
                }
                for (int i = 0; i < Durations.Length; i++) nDaysAvail.Add(iMin);
                NumberOfLimitDays = iMin;
            }

            public bool IsLocalhost() // only use 127.0.0.1 for local host and never try to ssh to it.
            {
                return (Dns.GetHostName().ToLower() == PCname);
            }

            public bool IsInSprint(string shortname)
            {
                foreach (cCalcLimitProj c in ProjList)
                {
                    if (shortname == c.ShortName)
                    {
                        return c.UsedInSprint;
                    }
                }
                return false;
            }

            public bool IsInGames(string shortname)
            {
                foreach (cCalcLimitProj c in ProjList)
                {
                    if (shortname == c.ShortName)
                    {
                        return c.UsedInGames;
                    }
                }
                return false;
            }

            public int GetNumInSprint()
            {
                int n = 0;
                foreach (cCalcLimitProj c in ProjList)
                {
                    if (c.UsedInSprint) n++;
                }
                return n;
            }

            public bool Parse_AC(ref string sTemp, out XDocument doc)
            {
                doc = null;
                int i = sTemp.IndexOf("<app_config>");
                if (i == -1)
                {
                    cc_config = null;
                    return false;
                }
                Debug.Assert(i >= 0);
                int j = sTemp.IndexOf("</app_config>", i + 12);
                Debug.Assert(j >= 0);
                sTemp = sTemp.Substring(i, j + 13 - i).Trim();
                app_config = sTemp.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                doc = XDocument.Parse(sTemp);
                return true;
            }

            public bool Parse_CC_PC(ref string sTemp)
            {
                int i = sTemp.IndexOf("<cc_config>");
                if (i == -1)
                {
                    cc_config = null;
                    return false;
                }
                Debug.Assert(i >= 0);
                int j = sTemp.IndexOf("</cc_config>", i + 11);
                Debug.Assert(j >= 0);
                cc_config = sTemp.Substring(i, j + 12 - i).Trim().Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                i = sTemp.IndexOf("<!--pandora");
                if (i < 0) pc_config = null;
                else
                {
                    j = sTemp.IndexOf("-->", 11);
                    Debug.Assert(j > 0);
                    sTemp = sTemp.Substring(i + 11, j - i - 11);
                    pc_config = sTemp.Trim().Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
                return true;
            }

            public cCalcLimitProj AddProject(string PCname, string ShortName, string masterUrl, string studyV)
            {
                cCalcLimitProj clp = new cCalcLimitProj();
                clp.init(masterUrl, false);
                clp.sPCname = PCname;
                clp.sStudy = studyV;
                clp.ShortName = ShortName;
                ProjList.Add(clp);
                return clp;
            }

            public cCalcLimitProj GetProjStruct(string shortname)
            {
                foreach (cCalcLimitProj c in ProjList)
                {
                    if (c.ShortName == shortname) return c;
                }
                return null;
            }


            // this for the pc when running pandora config
            public string CreateTempletPC(int PC_code)
            {
                string NL = Environment.NewLine;
                string sOut = "message_filters:has reached a limit,your preferences are set,No tasks sent,No work sent,see scheduler log,#" + NL;
                sOut += "earliest_deadline_first\r\ndebug\r\nbunker_strategy: " + PC_code + NL;
                foreach (cCalcLimitProj ep in ProjList)
                {
                    if (!ep.ProjectDisabled)
                    {
                        sOut += NL + NL + "project: " + ep.ProjUrl + NL;
                        sOut += "block_reports: 1000" + NL;
                        int lv = ep.LimitValue;
                        if (lv == 0 || ((PC_code & PC_NNW_WU_10) > 0)) lv = 10;
                        sOut += "limit: " + lv.ToString() + NL;
                    }
                }
                pc_config = sOut.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                return sOut + "#";
            }

            //database templet but pandora needs additional parameters
            public string CreateTemplet()
            {
                string NL = Environment.NewLine;
                string sOut = "message_filters:has reached a limit,your preferences are set,No tasks sent,No work sent,see scheduler log,#" + NL;
                sOut += "earliest_deadline_first\r\ndebug\r\nbunker_strategy: 5\r\nbunker_release: " +
                     UTC_bunker_release + NL + "bunker_end: " + globals.tNonN(bunker_end) + NL;
                sOut += "#concurrent_gpu_tasks: " + ConcurrentGpuTasks.ToString() + NL;
                double d = ConcurrentCpuTasks;
                sOut += "#concurrent_cpu_tasks: " + Convert.ToInt32(d) + NL;
                foreach (cCalcLimitProj ep in ProjList)
                {
                    //if(!ep.ProjectDisabled)
                    {
                        sOut += NL + NL + "project: " + ep.ProjUrl + NL;
                        sOut += "block_reports: 1000" + NL;
                        sOut += "#study: " + ep.sStudy + NL;
                        sOut += "#used_in_sprint: " + (ep.UsedInSprint ? "1" : "0") + NL;
                        sOut += "#app_type: " + ep.AppType + NL;
                        sOut += "#bunker_start: " + globals.tNonN(ep.BunkerStart) + NL;
                        sOut += "#bunker_end: " + globals.tNonN(ep.BunkerEnd) + NL;
                    }
                }
                return sOut + "#";
            }

            public void AddMissingParams(ref cPClimit pPCtemplet)
            {
                if (MessageFilters == "")
                    MessageFilters = pPCtemplet.MessageFilters;
                if (ConcurrentCpuTasks == 0)
                    ConcurrentCpuTasks = pPCtemplet.ConcurrentCpuTasks;
                if (ConcurrentGpuTasks == 0)
                    ConcurrentGpuTasks = pPCtemplet.ConcurrentGpuTasks;
                foreach (cCalcLimitProj c in ProjList)
                {
                    int n = pPCtemplet.NameToIndex(c.ShortName);
                    if (n < 0) continue;    // project was not in the BG 
                    cCalcLimitProj p = pPCtemplet.ProjList[n];
                    if(c.AppType == "")
                        c.AppType = p.AppType;
                    if (c.sStudy != "")
                    {
                        if (c.sStudy != p.sStudy)
                        {
                            if (c.sStudy == "-1")
                            {
                                c.sStudy = p.sStudy;
                            }
                            else
                            {
                                c.ChangeStudy(p);
                            }
                        }
                    }
                    switch(c.AppType)
                    {
                        case "gpu":
                            if(c.gpu_usage == 0)
                                c.gpu_usage = 1;
                            if(c.cpu_usage == 1)
                                c.cpu_usage = 1;
                            if(c.MaxApps == 0)
                                c.MaxApps = ConcurrentGpuTasks; //it can run on as many GPUS as available.
                            break;
                        case "cpu":
                            c.gpu_usage = 0;
                            if( c.cpu_usage == 0)
                                c.cpu_usage = 1;
                            if(c.MaxApps == 0)
                                c.MaxApps = 2;
                            break;
                    }

                }
            }

            public string CreatePCstream(out string sPathname)
            {
                string NL = Environment.NewLine;
                string sOut = "message_filters:has reached a limit,your preferences are set,No tasks sent,No work sent,see scheduler log,#" + NL;
                sPathname = globals.WhereDOC + "\\pandora_config_" + PCname;
                sOut += "earliest_deadline_first\r\nbunker_strategy:5\r\ndebug\r\nbunker_release: " +
                    UTC_bunker_release + NL + "bunker_end: " + bunker_end + NL;
                if (!IsSelected)
                {
                    File.Delete(sPathname);
                    sPathname = "";
                    return "";
                }
                foreach (cCalcLimitProj ep in ProjList)
                {
                    if (!ep.UsedInSprint) continue;
                    sOut += NL + "project: " + ep.ProjUrl + NL;
                    sOut += "block_reports: 1000" + NL;
                    int lv = 1;
                    if (ep.LimitValue > 0) lv = ep.LimitValue;
                    sOut += "limit: " +lv +  NL;
                    //Debug.Assert(ep.LimitValue > 0, "Limit cannot be 0 or negative here");                    
                }
                return sOut + "#" + NL;
            }

            public string CreatePDstream(out string sPathname)
            {
                string NL = Environment.NewLine;
                string sOut = "message_filters:has reached a limit,your preferences are set,No tasks sent,No work sent,see scheduler log,#" + NL;
                sPathname = globals.WhereDOC + "\\pandora_config_" + PCname + ".txt";
                sOut += "earliest_deadline_first\r\nbunker_strategy:1\r\ndebug\r\nbunker_release: " +
                    UTC_bunker_release + NL + "bunker_end: " + bunker_end + NL; // always positive
                sOut += "#concurrent_gpu_tasks: " + ConcurrentGpuTasks.ToString() + NL;
                double d = ConcurrentCpuTasks;
                sOut += "#concurrent_cpu_tasks: " + Convert.ToInt32(d) + NL;
                sOut += "#os_type : " + OStype + NL;
                sOut += "#is_selected: " + (IsSelected ? "1" : "0") + NL;
                foreach (cCalcLimitProj ep in ProjList)   // each project
                {
                    if (ep.ProjectDisabled)
                        sOut += NL + NL + "#project: " + ep.ProjUrl + NL;
                    else
                        sOut += NL + NL + "project: " + ep.ProjUrl + NL;
                    bool SetBlock = ep.BlockCnt > 0;
                    SetBlock = true;
                    ep.BlockCnt = 1000;
                    string sBn = ep.BlockCnt.ToString();
                    if (SetBlock && !ep.ProjectDisabled)
                        sOut += "block_reports: " + sBn + NL;
                    else
                        sOut += "#block_reports: 1000" + NL;
                    if (ep.LimitValue >= 0)
                        sOut += "limit: " + ep.LimitValue.ToString() + NL;
                    else
                        sOut += "limit: 0" + NL;
                    sOut += "#used_in_sprint: " + (ep.UsedInSprint ? "1" : "0") + NL;
                    sOut += "#study: " + ep.sStudy + NL;
                    sOut += "#ncpus: " + ep.cpu_usage.ToString("F2") + NL;
                    sOut += "#ngpus: " + ep.gpu_usage.ToString("F2") + NL;
                    sOut += "#max_apps: " + ep.MaxApps.ToString() + NL;
                    sOut += "#save_resources: " + ep.ShowUsedCpuGpu + NL;
                    sOut += "#work_value: " + ep.Points.ToString() + NL;
                    sOut += "#number_samples: " + ep.Cnt.ToString() + NL;
                    sOut += "#ready_to_report: " + ep.ReadyToReport.ToString() + NL;
                    sOut += "#jobs_succeeded: " + ep.JobsSucceeded.ToString() + NL;
                    sOut += "#jobs_failed: " + ep.JobsFailed.ToString() + NL;
                    sOut += "#total_wu_count: " + ep.TotalWUcnt.ToString() + NL;
                    sOut += "#average_elapsed: " + ep.Avg.ToString() + NL;
                    sOut += "#std: " + ep.Std.ToString("F4") + NL;
                    sOut += "#app_type: " + ep.AppType + NL;
                    sOut += "#bunker_start: " + globals.tNonN(ep.BunkerStart) + NL;
                    sOut += "#bunker_end: " + globals.tNonN(ep.BunkerEnd) + NL;
                    sOut += "#PrevCnt: " + ep.PrevCnt + NL;
                    sOut += "#PrevAvg: " + ep.PrevAvg + NL;
                    sOut += "#PrevStudy: " + ep.PrevStudy + NL;
                    sOut += "#PrevPoints: " + ep.PrevPoints + NL;
                    sOut += "#PrevStd: " + ep.PrevStd + NL;
                    sOut += "#PrevLimit: " + ep.PrevLimit + NL;
                    sOut += "#PrevStart: " + ep.PrevStart + NL;
                    sOut += "#PrevEnd: " + ep.PrevEnd + NL;
                    sOut += "#PrevAppType: " + ep.PrevAppType + NL;
                    sOut += "#";
                }

                return sOut;
            }

            public int GetProjNameIndex(string shortName)
            {
                int i = 0;
                foreach (cCalcLimitProj proj in ProjList)
                {
                    if (proj.ShortName == shortName) return i;
                    i++;
                }
                return -1;
            }

            public bool SetProjectInSprint(string shortName, bool b)
            {
                foreach (cCalcLimitProj proj in ProjList)
                {
                    if (proj.ShortName == shortName)
                    {
                        proj.UsedInSprint = b;
                        return true;
                    }
                }
                return false;
            }

            public void UpdateFromTemplet(ref List<cCalcLimitProj> tList, string PCname)
            {
                foreach (cCalcLimitProj p in ProjList)
                    p.ProjectDisabled = true;

                foreach (cCalcLimitProj t in tList)
                {
                    int iLoc = GetProjNameIndex(t.ShortName);
                    if (iLoc < 0)
                    {
                        cCalcLimitProj clP = new cCalcLimitProj();
                        clP.CopyTemplet(t);
                        ProjList.Add(clP);
                    }
                    else
                    {
                        ProjList[iLoc].ProjectDisabled = t.ProjectDisabled;
                        ProjList[iLoc].BunkerStart = t.BunkerStart;
                        ProjList[iLoc].BunkerStart = t.BunkerEnd;
                        ProjList[iLoc].AppType = t.AppType;
                        ProjList[iLoc].sStudy = t.sStudy;
                    }
                }

            }

            private cCalcLimitProj CloneProj(cCalcLimitProj proj)
            {
                cCalcLimitProj newProj = new cCalcLimitProj();
                newProj.init(proj.ProjUrl, proj.ProjectDisabled);
                newProj.ShortName = proj.ShortName;
                newProj.sPCname = proj.sPCname;
                newProj.sStudy = proj.sStudy;
                newProj.AppType = proj.AppType;
                newProj.Avg = proj.Avg;
                newProj.Cnt = proj.Cnt;
                newProj.MaxApps = proj.MaxApps;
                newProj.gpu_usage = proj.cpu_usage;
                newProj.gpu_usage = proj.gpu_usage;
                newProj.BunkerStart = proj.BunkerStart;
                newProj.BunkerEnd = proj.BunkerEnd;
                newProj.Points = proj.Points;
                newProj.LimitValue = proj.LimitValue;
                newProj.ReadyToReport = proj.ReadyToReport;
                newProj.JobsFailed = proj.JobsFailed;
                newProj.JobsSucceeded = proj.JobsSucceeded;
                newProj.TotalWUcnt = proj.TotalWUcnt;
                newProj.ProjectDisabled = proj.ProjectDisabled;
                newProj.UsedInSprint = proj.UsedInSprint;
                newProj.UsedInGames = proj.UsedInGames;
                return newProj;
            }

            public void DeepCopy(ref cPClimit PCl)
            {
                ConcurrentGpuTasks = PCl.ConcurrentGpuTasks;
                ConcurrentCpuTasks = PCl.ConcurrentCpuTasks;
                OStype = PCl.OStype;
                PCname = PCl.PCname;
                UTC_bunker_release = PCl.UTC_bunker_release;
                bunker_end = PCl.bunker_end;
                IsSelected = PCl.IsSelected;
                ProjList.Clear();
                cCalcLimitProj proj = null;
                List<string> MissingInPCl = new List<string>();
                foreach (string sID in PCl.ProjList.Select(p => p.ShortName))
                {
                    int n = PCl.NameToIndex(sID);
                    proj = PCl.ProjList[n];  //  PCl.ProjList.FirstOrDefault(p => p.ShortName == sID);
                    if (proj != null)
                    {
                        ProjList.Add(CloneProj(proj));
                    }
                }

                foreach (string sID in PCl.ProjList.Select(p => p.ShortName))
                {
                    if (PCl.NameToIndex(sID) < 0 && NameToIndex(sID) < 0)
                    {
                        proj = PCl.ProjList.FirstOrDefault(p => p.ShortName == sID);
                        if (proj != null)
                        {
                            ProjList.Add(CloneProj(proj));
                        }
                    }
                }
            }
            public int NameToIndex(string shortName)
            {
                for (int i = 0; i < ProjList.Count; i++)
                {
                    if (shortName == ProjList[i].ShortName) return i;
                }
                return -1;
            }
            public void ClearTotals()
            {
                foreach (cCalcLimitProj proj in ProjList)
                {
                    proj.JobsFailed = 0;
                    proj.JobsSucceeded = 0;
                    proj.ReadyToReport = 0;
                    proj.TotalWUcnt = 0;
                }
            }
        }


        public cPClimit GetPCbyName(string PCname)
        {
            foreach (cPClimit PCl in PandoraDatabase)
            {
                if (PCl.PCname == PCname) return PCl;
            }
            return null;
        }

        public void GetLongest(out int LongestPCname, out int LongestProjectName)
        {
            LongestProjectName = 0;
            LongestPCname = 0;
            foreach (cPClimit PCl in PandoraDatabase)
            {
                if (!PCl.IsSelected) continue;
                if (PCl.PCname.Length > LongestPCname) LongestPCname = PCl.PCname.Length;
                foreach (cCalcLimitProj c in PCl.ProjList)
                {
                    if (!c.UsedInSprint) continue;
                    if (c.ShortName.Length > LongestProjectName) LongestProjectName = c.ShortName.Length;
                }
            }
        }
        public void SaveSelected()
        {
            SelectedItems.Clear();
            foreach (cPClimit PCl in PandoraDatabase)
                SelectedItems.Add(PCl.IsSelected);
        }

        public void RestoreSelected()
        {
            int i = 0;
            foreach (cPClimit PCl in PandoraDatabase)
                PCl.IsSelected = SelectedItems[i++];
        }

        public int GetFirstSelected()
        {
            for (int i = 0; i < PandoraDatabase.Count; i++)
            {
                if (PandoraDatabase[i].IsSelected) return i;
            }
            return -1; // no selected items
        }

        public int GetNumSelected()
        {
            int n = 0;
            foreach (cPClimit PCl in PandoraDatabase)
            {
                if (PCl.IsSelected) n++;
            }
            return n;
        }

        public int GetNextSelected(int prevIndex)
        {
            for (int i = prevIndex + 1; i < PandoraDatabase.Count; i++)
            {
                if (PandoraDatabase[i].IsSelected) return i;
            }
            return -1; // no more selected items
        }


        public int GetFirstInSprint()
        {
            for (int i = 0; i < ProjectList.Count; i++)
            {
                if (ProjectList[i].UsedInSprint) return i;
            }
            return -1; // no project in sprint
        }

        public int GetNextInSprint(int index)
        {
            for (int i = index + 1; i < ProjectList.Count; i++)
            {
                if (ProjectList[i].UsedInSprint) return i;
            }
            return -1; // no more project in sprint
        }

        public int GetNumInSprint()
        {
            int n = 0;
            for (int i = 0; i < ProjectList.Count; i++)
            {
                if (ProjectList[i].UsedInSprint) n++;
            }
            return n;
        }


        // will either update or add a new system
        public void UpdateSystemInDB(cHostInfo hostInfo, ref cPClimit templetDB)
        {
            string PCname = hostInfo.ComputerID;
            cPClimit cpL = NameToSprintPC(PCname);
            if (cpL == null)
            {
                cpL = new cPClimit();
                cpL.PCname = PCname;
                cpL.ConcurrentCpuTasks = hostInfo.ProcessorCount;
                cpL.ConcurrentGpuTasks = hostInfo.GPUcount;
                cpL.ProjList = new List<cCalcLimitProj>(templetDB.ProjList);
                PandoraDatabase.Add(cpL);
                cpL = PandoraDatabase.Last();
            }
            templetDB.ProjList = new List<cCalcLimitProj>(templetDB.ProjList);
        }

        public PandoraConfig(ref cProjectStruct projectStats)
        {
            ProjectStats = projectStats;
            ManagedPCs = projectStats.ManagedPCs;
            ProjectList = projectStats.ProjectList;
            PandoraDatabase = projectStats.PandoraDatabase; //!!! this does not exist on startup !!!
            //todo to do pandorconfig is used to read the above database but it is null on startup
            //so the above assignment is a null the first time up
        }

        public cPClimit NameToSprintPC(string pcName)
        {
            int iLoc = FindPCnameIndex(pcName);
            if (iLoc >= 0) return PandoraDatabase[iLoc];
            return null;
        }

        public void UpdateDBfromPC(string PCname, ref cPClimit PCl)
        {
            int iLoc = FindPCnameIndex(PCname);
            PandoraDatabase[iLoc].DeepCopy(ref PCl);
        }

        public int FindPCnameIndex(string pcName)
        {
            if (PandoraDatabase.Count == 0) return -1;
            for (int i = 0; i < PandoraDatabase.Count; i++)
                if (PandoraDatabase[i].PCname == pcName) return i;
            return -1;
        }

        public string WriteDBrecord(ref cPClimit PCx)
        {
            string sOut = PCx.CreatePDstream(out string sPathname);
            if (sOut == "") return "ERROR: empty PD stream" + Environment.NewLine;
            try
            {
                File.WriteAllText(sPathname, sOut);
            }
            catch (Exception e)
            {
                return "ERROR: " + e.Message + Environment.NewLine;
            }
            return sPathname + " was written " + Environment.NewLine;
        }

        public string WritePandoraConfig(ref cPClimit PCx)
        {
            string sOut = PCx.CreatePCstream(out string sPathname);
            File.WriteAllText(sPathname, sOut);
            return sOut;
        }

        public string ReadPandoraConfig(ref cPClimit PCx, out string filePath)
        {
            filePath = globals.WhereDOC + "\\pandora_config_" + PCx.PCname;
            if(File.Exists(filePath))
                return File.ReadAllText(filePath);
            return "";
        }

        public List<cPClimit> GetConfigParams()
        {
            cPClimit pClimit;
            cPClimit FormattedPCl; // in the proper order with any new projects added
            List<cPClimit> ThisPC = new List<cPClimit>();
            string[] files = Directory.GetFiles(globals.WhereEXE, "pandora_config*.txt");
            foreach (string filePath in files)
            {
                string filename = Path.GetFileName(filePath);
                if (filename.StartsWith("pandora_config_") && filename.EndsWith(".txt"))
                {
                    string PCname = filename.Substring("pandora_config_".Length, filename.Length - "pandora_config_".Length - ".txt".Length);
                    if (!ManagedPCs.IsSystemManaged(PCname)) continue;
                    string sPandoraText = "";
                    if (File.Exists(filePath))
                        sPandoraText = File.ReadAllText(filePath);
                    else
                    {
                        sPandoraText = ProjectStats.current_default_pandora_config;
                        File.WriteAllText(filePath, sPandoraText);
                    }

                    pClimit = ParsePandoraConfig(sPandoraText, PCname);

                    ManagedPCs.SelectCurrentFromPC(PCname);
                    pClimit.ConcurrentCpuTasks = ManagedPCs.CurrentProcessorCount;
                    pClimit.ConcurrentGpuTasks = ManagedPCs.CurrentGPUcount;
                    pClimit.OStype = ManagedPCs.CurrentOStype;

                    pClimit.AddMissingParams(ref ProjectStats.TempletDB);

                    pClimit.IPaddress = ManagedPCs.CurrentIPaddress;
                    WriteDBrecord(ref pClimit);
                    ProjectStats.sshCredentials.GetCredentials(pClimit.PCname, out pClimit.UserName, out pClimit.Password);
                    ThisPC.Add(pClimit);
                }
            }
            return ThisPC;
        }
        /*
        private  void AddMissingParams1(ref cPClimit PCl, ref cPClimit pPCtemplet)
        {
            if (PCl.MessageFilters == "")
                PCl.MessageFilters = pPCtemplet.MessageFilters;
            if (PCl.ConcurrentCpuTasks == 0)
                PCl.ConcurrentCpuTasks = pPCtemplet.ConcurrentCpuTasks;
            if (PCl.ConcurrentGpuTasks == 0)
                PCl.ConcurrentGpuTasks = pPCtemplet.ConcurrentGpuTasks;
            foreach (cCalcLimitProj c in PCl.ProjList)
            {
                int n = pPCtemplet.NameToIndex(c.ShortName);
                if (n < 0) continue;    // project was not in the BG 
                cCalcLimitProj p = pPCtemplet.ProjList[n];
                if (c.sStudy != "")
                {
                    if (c.sStudy == p.sStudy) continue;
                    c.ChangeStudy(p);
                }

            }
        }
        */

        public cPClimit AddPC(string PCname, string nCPU, string nGPU, string sOStype, string IPaddress)
        {
            int iCPU = Convert.ToInt32(nCPU);
            int iGPU = Convert.ToInt32(nGPU);
            ManagedPCs.AddPC(PCname, iCPU, iGPU, sOStype, IPaddress);
            int nLoc = FindPCnameIndex(PCname);
            cPClimit PCl;
            if (nLoc < 0)
            {
                PCl = ParsePandoraConfig(ProjectStats.current_default_database_config, PCname);
                PCl.ConcurrentCpuTasks = iCPU;
                PCl.ConcurrentGpuTasks = iGPU;
                PCl.OStype = sOStype;
                PCl.IPaddress = IPaddress;
                PandoraDatabase.Add(PCl);
            }
            else
            {
                PCl = PandoraDatabase[nLoc];
                PCl.OStype = sOStype;
            }
            WriteDBrecord(ref PCl);
            return PCl;
        }

        public cPClimit ParsePandoraConfig(string sInput, string PCname)
        {

            cPClimit ThisPC = new cPClimit();
            if (PCname == "localhost")
                PCname = Environment.MachineName.ToLower();
            ThisPC.PCname = PCname;
            int nLineCnt = 0;
            string[] lines = sInput.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            cCalcLimitProj EachProj;
            bool bInProject = false;
            string line;
            int ProjIndex = -1;
            cCalcLimitProj ThisProj = null;
            ProjectStats.sErrMsg = "";
            foreach (string aline in lines)
            {
                nLineCnt++;
                int i = aline.IndexOf("project:");
                if (i == 0 || i == 1)
                {
                    bInProject = true;
                    ProjIndex++;
                    EachProj = new cCalcLimitProj();
                    EachProj.sPCname = PCname;
                    EachProj.init(aline.Substring(i + 8).Trim(), i == 1);
                    EachProj.UsedInGames = PCname == "templet";
                    ThisPC.ProjList.Add(EachProj);
                    int j = ProjectStats.GetNameIndex(EachProj.ProjUrl);
                    if (j < 0)
                    {
                        ProjectStats.sErrMsg += "badly formed or missing project info at line " + nLineCnt + Environment.NewLine;
                        continue;
                    }
                    EachProj.ShortName = ProjectStats.ShortName(j);
                    ThisProj = ThisPC.ProjList[ProjIndex];
                }


                if (bInProject)
                {
                    line = aline.ToLower();

                    i = line.IndexOf("block:");
                    if (i >= 0 && i < 2)
                    {
                        ThisProj.Avg = Convert.ToInt32(line.Substring(i + 6).Trim());
                    }

                    i = line.IndexOf("limit:");
                    if (i >= 0 && i < 2)
                    {
                        int nLimit = Convert.ToInt32(line.Substring(i + 6).Trim());
                        if (ThisProj.LimitValue <= 0)
                            ThisProj.LimitValue = nLimit;
                    }

                    i = line.IndexOf("#average_elapsed:");
                    if (i == 0)
                    {
                        ThisProj.Avg = Convert.ToInt32(line.Substring(i + 17).Trim());
                    }

                    i = line.IndexOf("#bunker_start:");
                    if (i >= 0)
                    {
                        ThisProj.BunkerStart = globals.fNonN(line.Substring(i + 14).Trim());
                    }

                    i = line.IndexOf("#bunker_end:");
                    if (i >= 0)
                    {
                        ThisProj.BunkerEnd = globals.fNonN(line.Substring(i + 12).Trim());
                    }

                    i = line.IndexOf("#work_value:");
                    if (i == 0)
                    {
                        ThisProj.Points = Convert.ToInt32(line.Substring(i + 12).Trim());
                    }

                    i = line.IndexOf("#number_samples:");
                    if (i == 0)
                    {
                        ThisProj.Cnt = Convert.ToInt32(line.Substring(i + 16).Trim());
                    }

                    i = line.IndexOf("#app_type:");
                    if (i == 0)
                    {
                        ThisProj.AppType = line.Substring(i + 10).Trim();
                    }


                    i = line.IndexOf("#used_in_sprint:");
                    if (i == 0)
                    {
                        ThisProj.UsedInSprint = (line.Substring(i + 16).Trim() == "1");
                    }


                    i = line.IndexOf("#study:");
                    if (i == 0)
                    {
                        ThisProj.sStudy = line.Substring(i + 7).Trim();
                    }

                    i = line.IndexOf("#ncpus:");
                    if (i == 0)
                    {
                        ThisProj.cpu_usage = Convert.ToDouble(line.Substring(i + 7));
                        ThisProj.avg_ncpus = ThisProj.cpu_usage;
                    }

                    i = line.IndexOf("#ngpus:");
                    if (i == 0)
                    {
                        ThisProj.gpu_usage = Convert.ToDouble(line.Substring(i + 7));
                        ThisProj.ngpus = ThisProj.gpu_usage;
                    }

                    i = line.IndexOf("#max_apps:");
                    if (i == 0)
                    {
                        ThisProj.MaxApps = Convert.ToInt32(line.Substring(i + 10));
                    }

                    i = line.IndexOf("#save_resources:");
                    if (i == 0)
                    {
                        ThisProj.ShowUsedCpuGpu = line.Substring(i + 16).Trim();
                    }


                    /* do not want to preserve this as it is only good at the time it was taken
                    i = line.IndexOf("#jobs_succeeded:");
                    if (i == 0)
                    {
                        ThisProj.JobsSucceeded = Convert.ToInt32(line.Substring(i + 16));
                    }
                    */

                    i = line.IndexOf("#jobs_failed:");
                    if (i == 0)
                    {
                        ThisProj.JobsFailed = Convert.ToInt32(line.Substring(i + 13));
                    }


                    i = line.IndexOf("#ready_to_report:");
                    if (i == 0)
                    {
                        ThisProj.ReadyToReport = Convert.ToInt32(line.Substring(i + 17));
                    }


                    i = line.IndexOf("#total_wu_count:");
                    if (i == 0)
                    {
                        ThisProj.TotalWUcnt = Convert.ToInt32(line.Substring(i + 16));
                    }

                    i = line.IndexOf("#PrevStd:");
                    if (i == 0)
                    {
                        ThisProj.PrevStd = line.Substring(i + 9);
                    }

                    i = line.IndexOf("#std:");
                    if (i == 0)
                    {
                        ThisProj.Std = Convert.ToDouble(line.Substring(i + 5));
                    }


                    i = line.IndexOf("#PrevCnt:");
                    if (i == 0)
                    {
                        ThisProj.PrevCnt = line.Substring(i + 9);
                    }

                    i = line.IndexOf("#PrevAvg:");
                    if (i == 0)
                    {
                        ThisProj.PrevAvg = line.Substring(i + 9);
                    }

                    i = line.IndexOf("#PrevStudy:");
                    if (i == 0)
                    {
                        ThisProj.PrevStudy = line.Substring(i + 11);
                    }

                    i = line.IndexOf("#PrevPoints:");
                    if (i == 0)
                    {
                        ThisProj.PrevPoints = line.Substring(i + 12);
                    }

                    i = line.IndexOf("#PrevLimit:");
                    if (i == 0)
                    {
                        ThisProj.PrevLimit = line.Substring(i + 11);
                    }

                    i = line.IndexOf("#PrevStart:");
                    if (i == 0)
                    {
                        ThisProj.PrevStart = line.Substring(i + 11);
                    }

                    i = line.IndexOf("#PrevEnd:");
                    if (i == 0)
                    {
                        ThisProj.PrevEnd = line.Substring(i + 9);
                    }
                    i = line.IndexOf("#PrevAppType:");
                    if (i == 0)
                    {
                        ThisProj.PrevAppType = line.Substring(i + 13);
                    }
                }
                else
                {
                    i = aline.IndexOf("bunker_release:");
                    if (i >= 0)
                    {
                        ThisPC.UTC_bunker_release = aline.Substring(i + 15).Trim();
                    }

                    i = aline.IndexOf("bunker_end:");
                    if (i >= 0)
                    {
                        ThisPC.bunker_end = globals.fNonN(aline.Substring(i + 11).Trim());
                    }

                    i = aline.IndexOf("#concurrent_gpu_tasks:");
                    if (i == 0)
                    {
                        ThisPC.ConcurrentGpuTasks = Convert.ToInt32(aline.Substring(i + 22).Trim());
                    }

                    i = aline.IndexOf("#concurrent_cpu_tasks:");
                    if (i == 0)
                    {
                        ThisPC.ConcurrentCpuTasks = Convert.ToInt32(aline.Substring(i + 22).Trim());
                    }

                    i = aline.IndexOf("message_filters:");
                    if (i >= 0)
                    {
                        ThisPC.MessageFilters = aline.Substring(i + 16).Trim();
                    }

                    i = aline.IndexOf("#os_type:");
                    if (i >= 0)
                    {
                        ThisPC.OStype = aline.Substring(i + 9).Trim();
                    }

                    i = aline.IndexOf("#is_selected:");
                    if (i >= 0)
                    {
                        ThisPC.IsSelected = (aline.Substring(i + 13).Trim() == "1");
                    }

                }

            }
            return ThisPC;
        }



        public string CalcLimit(ref cPClimit PCl, double NumberOfLimitDays)
        {
            string sOut = "";
            int nCPUsPerTask = 0;
            int nGPUsPerTask = 0;
            int nTasks = 0;
            string CGinfo = "";
            
            PCl.TotalCPUsUsed = 0;
            PCl.CalcSecondsLeft();
            string sRtn = GetLimits(ref PCl, NumberOfLimitDays, "cpu") + Environment.NewLine;
            if (sRtn.Contains("Error:"))
            {
                sOut += "Error: " + PCl.PCname + " has no data for CPUs" + Environment.NewLine;
            }
            else
            {
                sOut += sRtn;
                foreach (cCalcLimitProj clp in PCl.ProjList)
                {
                    if (clp.AppType != "cpu") continue;
                    nCPUsPerTask = (int)clp.cpu_usage;
                    if (nCPUsPerTask == 0)
                        nCPUsPerTask = 1;
                    nTasks = clp.MaxApps;
                    if (nTasks == 0)
                        nTasks = 1;
                    clp.ShowUsedCpuGpu = nTasks.ToString() + "(" + nCPUsPerTask.ToString() + ")";
                }
                PCl.TotalCPUsUsed += nTasks * nCPUsPerTask;
            }

            sOut = sOut.Trim() + Environment.NewLine; ;

            sRtn = GetLimits(ref PCl, NumberOfLimitDays, "gpu") + Environment.NewLine;
            if (sRtn.Contains("Error:"))
            {
                sOut += "Error: " + PCl.PCname + " has no data for GPUs" + Environment.NewLine;
            }
            else
            {
                sOut += sRtn;
                foreach (cCalcLimitProj clp in PCl.ProjList)
                {
                    if (clp.AppType != "gpu") continue;
                    if (clp.gpu_usage == 0)
                        clp.gpu_usage = 1.0;
                    nTasks = (int)(1.0 / clp.gpu_usage);
                    clp.ShowUsedCpuGpu = PCl.ConcurrentGpuTasks.ToString() + "(" + nTasks.ToString() + ")";
                }
                PCl.TotalCPUsUsed += PCl.ConcurrentGpuTasks;
            }
            PCl.TotalCPUsUsed++;    // add one for OS

            CGinfo = "CPUs available: " + PCl.ConcurrentCpuTasks + " CPUs used: " + PCl.TotalCPUsUsed.ToString() + Environment.NewLine;
            if (PCl.TotalCPUsUsed > PCl.ConcurrentCpuTasks)
            {
                CGinfo += "Error: number of CPUs used exceeds actual count" + Environment.NewLine;
            }
            // tOut += CGinfo + sOut;
            return CGinfo + sOut;

        }
        public string CalcLimits(double days)
        {
            string tOut = "";
            for (int i = 0; i < PandoraDatabase.Count; i++)
            {
                cPClimit PCl = PandoraDatabase[i];
                tOut += CalcLimit(ref PCl, days);
            }
            return tOut;
        }

        private string GetLimits(ref cPClimit OnePC, double DaysDuration, string sType)
        {
            int Q;
            string sOut = "";
            double sumInitial = 0;
            List<string> AllowedProjects = new List<string>();
            double E = 86400 * DaysDuration; // total time in seconds
            
            foreach (cCalcLimitProj LP in OnePC.ProjList)
            {
                if (!LP.UsedInSprint) continue;
                if (sType == "gpu")
                {
                    if (LP.AppType != "gpu") continue;
                }
                else
                {
                    if (LP.AppType != "cpu") continue;
                }

                if(LP.Cnt >0)
                    AllowedProjects.Add(LP.ShortName);
            }
            Q = AllowedProjects.Count;
            if (Q < 2) return "Error:No limit is possible" + Environment.NewLine;
            int Qunallowed = OnePC.ProjList.Count;
            int[] qPtr = new int[Q];
            int iQ = 0;
            for (int i = 0; i < Qunallowed; i++)
            {
                if (AllowedProjects.Contains(OnePC.ProjList[i].ShortName))
                {
                    qPtr[iQ] = i;
                    iQ++;
                }
            }

            double[] t = new double[Q];
            double[] v = new double[Q];
            double[] p = new double[Q];


            int nCoprocGPU = OnePC.ConcurrentGpuTasks;
            if (nCoprocGPU == 0)
            {
                nCoprocGPU = 1;
            }

            int nCoproc = nCoprocGPU;
            if(sType == "cpu")
            {
                nCoproc = OnePC.ConcurrentCpuTasks - nCoprocGPU - 1;
                nCoproc /= AllowedProjects.Count;
                if (nCoproc == 0)
                {
                    nCoproc = 1;
                }
            }

            for (int i = 0; i < Q; i++)
            {
                int ix = qPtr[i];
                v[i] = OnePC.ProjList[ix].Points;
                t[i] = OnePC.ProjList[ix].Avg / nCoproc;
                p[i] = IncludeSuccessfullJobs ? OnePC.ProjList[ix].JobsSucceeded : 0;
            }

            double sum = 0;
            for (int i = 0; i < Q; i++)
            {
                sum += t[i] / v[i];
                sumInitial += p[i] * v[i] * (t[i] / v[i]); // simplifies to p[i] * t[i]
            }

            double V = E / sum;
            int nV = (int)Math.Round(V);
            double[] n = new double[Q];
            for (int i = 0; i < Q; i++)
            {
                int ix = qPtr[i];
                string sProj = OnePC.ProjList[ix].ShortName;

                double neededValue = V - p[i] * v[i];
                if (neededValue < 0)
                    n[i] = 0;
                else
                    n[i] = neededValue / v[i];

                int nR = (int)Math.Round(n[i]);
                OnePC.ProjList[ix].LimitValue = nR; // only want this many work units
                                                    //OnePC.ProjList[ix].Duration = -DaysDuration;
                sOut += OnePC.PCname + $" {sProj}: produce {nR} units to get value {nV}" + Environment.NewLine;
            }

            return sOut;
        }
    }
}
