using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static CreditStatistics.globals;
using static CreditStatistics.globals.cAllProjectStudyInfo;
using static CreditStatistics.PandoraConfig;
using static CreditStatistics.ShowData;
using static System.Windows.Forms.DataFormats;

namespace CreditStatistics
{

    internal class cLHe
    {
        public string ProjectName;  // cpdn, lhc, etc
        public string ProjectsID;   // the ID given by the project to the PC
        public int IndexToProjectList; // the index in the projectlist of the project associated with the ProjectName
    }

    internal class cHostInfo
    {
        public string ComputerID;   // name of the PC such as "shire2"
        public string UserName = "";
        public string Password = "";
        public string IPaddress;
        public string OStype;
        public string sVersion;
        public int ProcessorCount;
        public int GPUcount;
        public bool IsSelected;
        public bool UsedInSprint;
        public bool HasSSH;     //port 22
        public bool HasBOINC;   //31415
        public bool SSHvalid;
        public bool BOINCvalid;
        public List<cLHe> LocalProjID = new List<cLHe>();
        public string GetProjectID(string shortname)
        {
            foreach(cLHe lh in LocalProjID)
            {
                if (lh.ProjectName == shortname) return lh.ProjectsID;
            }
            Debug.Assert(false, "cannot find " + shortname + " in LocalProjID");
            return "";
        }
    }

    internal class ReqCmds
    {
        public string sUse; // SSH, BOINC, WEB, SSHBOINC
        public int[] nPorts  = new int[3] {31416, 22,-1};
        public bool UseRadioButtons = false;
        public bool PerformScan = false;
    }

    internal class CSendNotepad
    {

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        public void PasteToNotepad(string strText)
        {
            if (strText == "") return;
            // Let's start Notepad
            Process process = new Process();
            process.StartInfo.FileName = "C:\\Windows\\Notepad.exe";
            process.Start();
            Thread.Sleep(2000);
            Clipboard.SetText(strText);
            IntPtr hWnd = process.Handle;
            BringWindowToTop(hWnd);
            SendKeys.SendWait("^V");
        }
        public void PasteToNotepad(string strText, string strFile)
        {
            if (strText == "") return;
            // Let's start Notepad
            Process process = new Process();
            process.StartInfo.FileName = "C:\\Windows\\Notepad.exe";
            process.StartInfo.Arguments = strFile;
            process.Start();
            Thread.Sleep(2000);
            Clipboard.SetText(strText);
            IntPtr hWnd = process.Handle;
            BringWindowToTop(hWnd);
            SendKeys.SendWait("^{end}");
            SendKeys.SendWait("{ENTER}");
            SendKeys.SendWait("^V");
        }
    }

    internal class globals
    {
        public static string WhereEXE;
        public static string WhereDOC;
        public static string name_ID_Study_config = "PROJ_STUDY_ID.cfg";
        public static string WhereDefaultHostList = "PROJ_PC_ID.cfg";
        public static string WhereMasterPandora = "ALL_PANDORA_CONFIG.txt";
        public static string WhereCurrentDefaultDatabaseConfig = "DEFAULT_DATABASE_CONFIG.txt";
        public static string WhereCurrentDefaultPandoraConfig = "DEFAULT_PANDORA_CONFIG.txt";
        public static string WhereCurrentDefaultCC_config = "DEFAULT_CC_CONFIG.txt";
        public static string WhereProjectAccessParams = "Proj_Access_Params.xml";
        public static int ERR_none = 0;
        public static int ERR_info = 1;
        public static int ERR_warning = 2;
        public static int ERR_critical = 3;
        public static int ERR_fatal = 4;
        public static int PC_REPORT_STATUS = 1;     // report status of completed, failed, backloged jobs periodically
        public static int PC_NNW_WU_10 = 2;           // issue no new work when 10 WUs have been downloaded.
        public static int PC_NNW_PROJECT_LIMIT = 4; // issue no new work when the project has acquired enough work units
        public static string PathTo_boinccmd_exe;
        public static string PathTo_boinc;
        public static string PathToBoincData = "";
        public static string BoincTaskFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\eFMer\\BoincTasks";
        // pad right side with spaces to fill
        public static string Rp(string strIn, int cnt)
        {
            int i = cnt - strIn.Length;
            if (i < 0) return strIn.Substring(0, cnt);
            return strIn + "                              ".Substring(0, i);
        }


/*

        public static string xRunScpAndGetOutput(ref cPClimit PCl, string sLocalFile, string sRemoteFile, bool IsLinux, string CK)
        {
            //if (PCl.IsLocalhost()) return "";
            string Argument = "";
            string remoteUser = PCl.UserName;
            string remoteHost = PCl.PCname;
            string ck = "/" + CK;

            if (PCl.IsLocalhost())  //local pc is windows and there is no SSH server
            {
                try
                {
                    string stemp = File.ReadAllText(sLocalFile);
                    File.WriteAllText(sRemoteFile, stemp);                    
                }
                catch(Exception e)
                {
                    return "Error transfering file " + sLocalFile + Environment.NewLine;
                }

                return sLocalFile + " copied to " + sRemoteFile + Environment.NewLine;
            }


            if (IsLinux)
            {
                Argument = ck + $" scp {sLocalFile}  {remoteUser}@{remoteHost}:{sRemoteFile}";
            }
            else
            {
                Argument =  ck + $" scp {sLocalFile}  {remoteHost}:{sRemoteFile}";
            }

            bool bRedirect = CK == "c"; // if c then redirect output, else just run command

            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = Argument,
                RedirectStandardOutput = bRedirect,
                RedirectStandardError = bRedirect,
                RedirectStandardInput = bRedirect,
                UseShellExecute = !bRedirect,
                CreateNoWindow = bRedirect
            };

            using (var process = Process.Start(psi))
            {
                if (bRedirect)
                {
                    string output = process.StandardOutput.ReadToEnd().Trim();
                    string error = process.StandardError.ReadToEnd().Trim();
                    process.WaitForExit();

                    // Check for SSH failure only (e.g., network issues, bad command, does not exist)
                    if (process.ExitCode == 255 || output == "" && error != "")
                    {
                        MessageBox.Show("Error launching cmd:" + Argument + "-" + error);
                    }
                    return output.Trim(); // Strip newline characters
                }
            }
            return "";

        }

        public static void xRemovePandora(ref cPClimit PCl)
        {
            bool bIsLinux = PCl.OStype != "w";
            string sCommand = "";

            if (bIsLinux)
            {
                sCommand = "sudo rm -f /var/lib/boinc/pandora_config";
            }
            else
            {
                sCommand = "\"del \\ProgramData\\boinc\\pandora_config\"";
            }

            if (!PCl.IsLocalhost())
            {
                xRunSshAndGetOutput(ref PCl, sCommand, bIsLinux, "c");
            }
            else
            {
                File.Delete("C:\\ProgramData\\boinc\\pandora_config");
            }
        }



        public static string xRunSshAndGetOutput(ref cPClimit PCl, string remoteCommand, bool IsLinux, string CK)
        {
            if (PCl.IsLocalhost()) return "";
            string Argument = "";
            string remoteUser = PCl.UserName;
            string remoteHost = PCl.PCname;
            string ck = "/" + CK;
            if (IsLinux)
            {
                Argument = remoteUser == "" ? ck + $" ssh {remoteHost} \"{remoteCommand}\"" :
                            ck + $" ssh {remoteUser}@{remoteHost} \"{remoteCommand}\"";
            }
            else
            {
                Argument = remoteUser == "" ? ck + $" ssh {remoteHost} {remoteCommand}" :
                    ck + $" ssh {remoteUser}@{remoteHost} {remoteCommand}";
            }

            bool bRedirect = CK == "c"; // if c then redirect output, else just run command

            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = Argument,
                RedirectStandardOutput = bRedirect,
                RedirectStandardError = bRedirect,
                RedirectStandardInput = bRedirect,
                UseShellExecute = !bRedirect,
                CreateNoWindow = bRedirect
            };

            using (var process = Process.Start(psi))
            {
                if (bRedirect)
                {
                    string output = process.StandardOutput.ReadToEnd().Trim();
                    string error = process.StandardError.ReadToEnd().Trim();
                    process.WaitForExit();

                    // Check for SSH failure only (e.g., network issues, bad command, does not exist)
                    if (process.ExitCode == 255 || output == "" && error != "")
                    {
                        MessageBox.Show("Error launching cmd:" + Argument + "-" + error);
                    }
                    return output.Trim(); // Strip newline characters
                }
            }
            return "";
        }

    */
        public static string NewLineToLinux(string input)
        {
            return input.Replace("\r\n", "\n");
        }

        public static void SetTextSafe(TabControl tabControl, string tabPageName, TextBox textBox, string text)
        {
            // Find the tab page with the matching Name
            TabPage tabPage = null;
            foreach (TabPage tp in tabControl.TabPages)
            {
                if (tp.Name == tabPageName)
                {
                    tabPage = tp;
                    break;
                }
            }

            if (tabPage == null)
                throw new ArgumentException($"No tab with name '{tabPageName}' found.", nameof(tabPageName));

            // Ensure the tab is selected *before* setting text
            if (tabControl.SelectedTab != tabPage)
                tabControl.SelectedTab = tabPage;

            // Update the text
            textBox.Text = text;

            // Clear any unwanted selection
            textBox.SelectionStart = textBox.TextLength; // caret at end
            textBox.SelectionLength = 0;
        }


        public static void SetTextHidden(TextBox textBox, string text)
        {
            // Update text even if the tab/page is not selected
            textBox.Text = text;

            // Prevent auto-highlight when the tab becomes visible later
            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;
        }

        //input is number of seconds (since 1970 the unix time) output is local time and also the string equivalent of local time
        public static string DateToString(double d, out DateTime localTime)
        {
            if (d == 0.0)
            {
                localTime = DateTime.Now;
                return "0";
            }
            long secondsSince1970 = Convert.ToInt64(d);

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(secondsSince1970);
            localTime = dateTimeOffset.LocalDateTime;
            return localTime.ToString("MM/dd/yyyyTHH:mm:ss");
        }

        //06/29/2025T10:40:21
        public static double StringToTime(string sIn, double secRelease)
        {
            sIn = sIn.Trim();
            int n = sIn.Length;
            if (n == 0) return 0.0;
            if (n < 14)
            {
                double d = Convert.ToDouble(sIn);
                return 86400.0 * d + secRelease;
            }
            DateTime utcTime = DateTime.ParseExact(
                sIn,
                "MM/dd/yyyyTHH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
            );

            // Calculate seconds since Unix epoch
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long secondsSince1970 = (long)(utcTime - epoch).TotalSeconds;
            return secondsSince1970;
        }

        // input is a local time string, not UTC
        public static double RemainingTime(DateTime StartDate)
        {
            //double ProjStart = (double)new DateTimeOffset(StartDate).ToUnixTimeSeconds();
            //double CurrentTime = (double)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            double StartDiff = (double)(StartDate - DateTime.Now).TotalSeconds;
            if (StartDiff >= 0) return 0;
            return Math.Round(StartDiff);
        }

        // change release date and time from UTC to local
        public static DateTime GetReleaseLocal(string sRelease_UTC)
        {
            //MM/dd/yyyyTHH:mm:ss"
            //need to remove the T
            int i = sRelease_UTC.IndexOf("T");
            if (i < 0)
            {
                MessageBox.Show("Critical error: release date is not in format MM/dd/yyyyTHH:mm:ss " + sRelease_UTC);
                return DateTime.Now;
            }
            sRelease_UTC = sRelease_UTC.Substring(0, i) + " " + sRelease_UTC.Substring(i + 1);
            DateTime utcDateTime = DateTime.SpecifyKind(
                DateTime.Parse(sRelease_UTC), DateTimeKind.Utc);
            DateTime localDateTime = utcDateTime.ToLocalTime();
            return localDateTime;
        }

        // better time handleing
        public static DateTimeOffset UTCtoLOCAL_OFFSET(string utcString)
        {
            DateTimeOffset dto = DateTimeOffset.ParseExact(
                utcString,
                "MM/dd/yyyyTHH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal
            );
            return dto;
        }

        public static string GetUTCstring(DateTimeOffset dto)
        {
            return dto.ToString("MM/dd/yyyyTHH:mm:ss", CultureInfo.InvariantCulture);
        }



        public static string fNonN(string sBunker_days) // from
        {
            if (sBunker_days == "") return "0";
            if(int.TryParse(sBunker_days, out int nDays))
            {
                return Math.Abs(nDays).ToString();
            }
            else
            {
                return "0";
            }
        }

        public static string tNonN(string sBunker_days) // to
        {
            if (sBunker_days == "" || sBunker_days == "0") return "0";
            if (int.TryParse(sBunker_days, out int nDays))
            {
                return (-Math.Abs(nDays)).ToString();
            }
            else
            {
                return "0";
            }
        }

        public static void WriteDefaultDatabaseConfig(string s)
        {
            File.WriteAllText(WhereCurrentDefaultDatabaseConfig, s.Trim());
        }

        public static void WriteCCrecord(string PCname, ref string[] cc_config)
        {
            string Pathname = WhereDOC + "\\cc_config_" + PCname + ".xml";
            if(cc_config != null)
                File.WriteAllLines(Pathname, cc_config);
        }

        public static string WriteACrecord(string PCname, string ProjName, ref string[] app_config)
        {
            string Pathname = WhereDOC + "\\app_config_" + ProjName + "_" + PCname + ".xml";
            try
            {
                if (app_config != null)
                    File.WriteAllLines(Pathname, app_config);
                else return "ERROR: expected data, but found NULL" + Environment.NewLine;
            }
            catch (Exception e)
            {
                return "ERROR: " + e.Message + Environment.NewLine;
            }
            return Pathname + " was written " + Environment.NewLine;
        }

        public static string[] ReadCCrecord(string PCname)
        {
            string Pathname = WhereDOC + "\\cc_config_" + PCname + ".xml";
            if (!File.Exists(Pathname)) return null;
            return File.ReadAllLines(Pathname);
        }

        public static void WritePCrecordS(string PCname, ref string[] pc_config)
        {
            string Pathname = WhereDOC + "\\pandora_config_" + PCname ;
            if (pc_config != null)
                File.WriteAllLines(Pathname, pc_config);
        }

        public static string[] ReadPCrecordS(string PCname)
        {
            string Pathname = WhereDOC + "\\pandora_config_" + PCname ;
            if (!File.Exists(Pathname)) return null;
            return File.ReadAllLines(Pathname);
        }

        public static string BoincCommand(string sApp, string sCmd)
        {
            var psi = new ProcessStartInfo
            {
                FileName = Path.Combine(PathTo_boinc,sApp),
                Arguments = sCmd,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd().Trim();
                string error = process.StandardError.ReadToEnd().Trim();

                process.WaitForExit();
                // Check for command failure only (e.g., network issues, bad command)
                if (process.ExitCode == 255 || output == "" && error != "")
                {
                    MessageBox.Show("Error launching cmd:" + sApp + " " + sCmd);
                    return "";
                }
                return output.Trim(); // Strip newline characters
            }
        }

        public static class PortChecker
        {
            public static async Task<bool> IsPortOpenAsync(string IPaddress, int port, int timeoutMs = 1000)
            {
                try
                {
                    using var client = new TcpClient();
                    var connectTask = client.ConnectAsync(IPaddress, port);
                    var timeoutTask = Task.Delay(timeoutMs);
                    var completed = await Task.WhenAny(connectTask, timeoutTask);
                    if (completed == connectTask && client.Connected)
                        return true;
                }
                catch
                {
                    // Ignore and return false
                }
                return false;
            }
        }

        // pad left side with spaces to fill
        public static string Lp(string strIn, int cnt)
        {
            int i = cnt - strIn.Length;
            if (i < 0) return strIn.Substring(0, cnt);
            return "                                               ".Substring(0, i) + strIn;
        }

        public class SSHCredentials
        {
            public class Cred
            {
                public string spcName;
                public string sPassword;
                public string sUsername;
            }
            public List<Cred> creds = new List<Cred>();
            public void Add(string PCname, string Username, string Password)
            {
                int index = creds.FindIndex(c => c.spcName == PCname);
                if(index == -1)
                {
                    Cred c1 = new Cred();
                    c1.sUsername = Username;
                    c1.sPassword = Password;
                    c1.spcName = PCname;
                    creds.Add(c1);
                }
                else
                {
                    Cred c2 = creds[index];
                    c2.sUsername = Username;
                    c2.sPassword = Password;
                }
            }
            public bool GetCredentials(string PCname, out string Username, out string Password)
            {
                Username = "";
                Password = "";
                int index = creds.FindIndex(c => c.spcName == PCname);
                if(index == -1) return false;
                Username = creds[index].sUsername;
                Password = creds[index].sPassword;
                return true;
            }
            public void SaveCredentials()
            {
                Properties.Settings.Default.RC_systems = creds.Select (c => c.spcName).ToArray();
                Properties.Settings.Default.RC_passwords = creds.Select(c => c.sPassword).ToArray();
                Properties.Settings.Default.RC_usernames = creds.Select (creds => creds.sUsername).ToArray();
                Properties.Settings.Default.Save();
            }
            public int Init()
            {
                string[] sPC = Properties.Settings.Default.RC_systems;
                if (sPC == null || sPC.Length == 0)
                {
                    Add(Dns.GetHostName().ToLower(), Environment.UserName, "");
                    Properties.Settings.Default.Save();
                    return 1;
                }
                string[] sPW = Properties.Settings.Default.RC_passwords;
                string[] sPU = Properties.Settings.Default.RC_usernames;
                for(int i = 0; i < sPC.Length; i++)
                {
                    Add(sPC[i], sPU[i], sPW[i]);
                }
                return sPC.Length;
            }
        }


        public class cSequencer
        {
            public List<string> SeqResults = new List<string>();
            public List<string> BGResults = new List<string>();
            public List<string> SeqTotalsText = new List<string>();
            public List<string> SeqOverallText = new List<string>();
            //public List<double> SumCreSecs = new List<double>();
            //public List<cEachPc> EachPCsHDR = new List<cEachPc>();
            public List<cPClimit> PClimit;
            public cPSlist selPlist;
            public cPClimit selPClimit;
            public cCalcLimitProj selProjList;
            public string rbPLcheckedShortName;
            public cHostInfo hi;
            public cPClimit DB; // the template database for the current project
            public PandoraConfig pc;
            public List<cPSlist> ProjectList;
            public int NumPagesToRead;
            public int CurrentPage;
            public int SeqInxPC;
            public int SeqInxPJ;
            public int NumSeqPC;
            public int NumSeqPJ;
            public string ProjID; // the project ID of the current project
            public string ShortName;
            public string sStudy;
            public string sHostName;
            public string sUrl;
            public double std;
            public int nLongestName;
            public cCreditInfo SeqTotals = new cCreditInfo();
            public bool OnStartup = false;
            public bool StopSequencing = false;
            
            private void CalcEfficiency()
            {
                double d = 0;
                foreach(cPClimit pcl in PClimit)
                {
                    if(pcl.IsSelected)
                    {
                        d = Math.Max(d, pcl.SumCreSecs);
                    }
                }

                foreach (cPClimit pcl in PClimit)
                {
                    if (d > 0)
                    {
                        pcl.EachPCsHDR.PctEff = 100 * pcl.SumCreSecs / d;
                    }
                    else pcl.EachPCsHDR.PctEff = 0;
                }
            }

            private cPClimit FindPC(string PCname)
            {
                foreach (cPClimit pc in PClimit)
                {
                    if (pc.PCname == PCname) return pc;
                }
                cPClimit NewPC = new cPClimit();
                NewPC.PCname = PCname;
                PClimit.Add(NewPC);
                return PClimit.Last();
            }

            public void AddProject(string PCname, string shortname, string tUrl, string study)
            {
                cPClimit pc = FindPC(PCname);
                cCalcLimitProj cLP = new cCalcLimitProj();
                cLP.ShortName = shortname;
                cLP.sPCname = PCname;
                //cLP.ProjUrl = masterurl todo to do
                cLP.AcqUrl = tUrl;
                cLP.sStudy = study;
                pc.ProjList.Add(cLP);
            }

            /*
            public bool OutOfData()
            {
                return EachPCsHDR[SeqInxPC].OutOfData;
            }
            public void SetHDR(int NumValid)
            {
                EachPCsHDR[SeqInxPC].nValidWUs = NumValid;
                EachPCsHDR[SeqInxPC].nRemainWUs = NumValid;
            }
            public void SetBODY(int NumFetched)
            {
                EachPCsHDR[SeqInxPC].nRemainWUs -= NumFetched;
                EachPCsHDR[SeqInxPC].OutOfData = (EachPCsHDR[SeqInxPC].nRemainWUs <= 0);
            }
            */
        }

        public class cCreditInfo
        {
            public DateTime tCompleted;
            public int nCnt;
            public bool bValid;
            public double ElapsedSecs;
            public double CPUtimeSecs;
            public double Credits;
            public double mELA;
            public double mCPU;
            public double dHours;
            public double stdCI;    // confidence interval
            public double std;  // std of samples
            public string PCname;
            public void Init(string sPCame)
            {
                Credits = 0;
                ElapsedSecs = 0;
                CPUtimeSecs = 0;
                nCnt = 0;
                dHours = 0;
                mELA = 0;
                mCPU = 0;
                PCname = sPCame;
            }
        }

        public class RunList
        {
            public string shortname;
            public string PCname;
            public string url;
            public string NextOperation;
            public bool bDone;
            public bool bBusy;
            public bool bCancel;
            public int NumValidWUs;
            public int NumValidOnPage;  // some projects need to count the number of valid WUs.  We need at  least 4 and possibly 20 for the maximum,
            public string sMsgErr;
            public string sOutInfo = "";
            public string BGinfo = "";
            public string[]? RawLines;
        }
        public class ReadRequest
        {
            public bool GetHdr;
            public bool GetBody;
            public bool GetNext;
            public bool UsePandoraDatabase; // use selected PCs and sequence through them
            public bool bUseUrl;        // use the url as the PC is unknown
            public string UrlWanted;  // a specific url was wanted
            public string shortname;  // the project named in the url
            public string sStudyV;    // the study or appic used in the url
            public string sHostID;  // ID assigned by the project to the PC
            public string spage;    // page number (offset)
            public string sValid;   // whether valid, all or error is wanted.  
            public string PCname;  // is usually unknown if user pastes a url
            public string sValidV;  // usually 4 gets valid results
        }

        public class cOrganizedRaw
        {
            public string ProjNameFull;
            public List<string> PCnameHostID = new List<string>();
        }
        public class cBoincRaw
        {
            public string PCname;
            public List<string> Proj = new List<string>();
            public List<string> HostID = new List<string>();
        }
        public class cOldraw
        {
            public List<string> RawOut = new List<string>();
            public List<cOrganizedRaw> OrgRaw = new List<cOrganizedRaw>();
            public int AddProjName(string s)
            {
                if (OrgRaw.Count == 0)
                {
                    cOrganizedRaw OR = new cOrganizedRaw();
                    OR.ProjNameFull = s;
                    OrgRaw.Add(OR);
                    return OrgRaw.Count - 1;
                }
                int i = 0;
                foreach (cOrganizedRaw OR1 in OrgRaw)
                {
                    if (s == OR1.ProjNameFull) return i;
                    i++;
                }
                cOrganizedRaw OR2 = new cOrganizedRaw();
                OR2.ProjNameFull = s;
                OrgRaw.Add(OR2);
                return OrgRaw.Count - 1;
            }
            public void AddTriple(string sPCname, string ProjName, string sPCid)
            {
                int i = AddProjName(ProjName);
                OrgRaw[i].PCnameHostID.Add(sPCname + " " + sPCid);
            }
        }

        public static string OpenUrl(string surl)
        {
            string edgePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";

/*
            Process.Start(new ProcessStartInfo
            {
                FileName = edgePath,
                Arguments = $"--profile-directory=Default \"{surl}\"",
                UseShellExecute = true
            });

            Thread.Sleep(1000);
*/

            Process.Start(new ProcessStartInfo
            {
                FileName = edgePath,
                Arguments = $"--profile-directory=Default \"{surl}\"",
                UseShellExecute = true
            });
            return "";
        }

        public static bool IsInteger(string input)
        {
            return int.TryParse(input, out _);
        }
        public static int FirstNonInteger(string s, int iOffset)
        {
            int i = iOffset;
            while (i < s.Length)
            {
                if (s[i] < '0' || s[i] > '9') return i;
                i++;
            }
            return s.Length;
        }


        public class cPSlist
        {
            public bool UseDefault;   // use tasks/4/0 and omit &appid=nn      
            public bool IsSelected;
            public bool UsedInSprint;
            public string MasterUrl; // this is NOT lower case!
            public string shortname;
            public string name;
            public string sURL;
            public string sHid;
            public string sValid;
            public string sStudy;
            public string sStudyV;
            public string sStudyL; // list of studies separated by space
            public string sPage;
            public string sPageV; // value of offset (page or count)
            public string sCountValids;
            public List<string> HostProjIDs = new List<string>(); // the ID of the pc
            public List<string> HostPCNames = new List<string>();
            public List<string> AppID = new List<string>(); // some projects list the applications in the xml files
            public void AddHosts(string sHostIDs)
            {
                if (sHostIDs == "") return;
                string[] s = sHostIDs.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s1 in s)
                {
                    if (s1 == "") continue;
                    HostProjIDs.Add(s1);
                }
            }
            public int AddAppID(string sAppID)
            {
                if (AppID.Count == 0) AppID.Add(sAppID);
                else if (AppID.Contains(sAppID)) return 0;
                else AppID.Add(sAppID);
                return 1;
            }
            public string FormURL(string sProjID)
            {
                string s = sURL + sHid + sProjID + ((sValid == "null") ? "" : sValid);
                s += sStudy == "null" ? "" : sStudy + sStudyV; //todo to do einstein
                if(sPage != "null")
                    s += sPage + sPageV;
                
                return s;
            }
            public string FormAcqUrl(string sProjID, string tStudyV)
            {
                string s = sURL + sHid + sProjID + ((sValid == "null") ? "" : sValid);
                s += sStudy == "null" ? "" : sStudy + tStudyV;
                s += sPage + sPageV;
                return s;
            }
        }



        public class cAllProjectStudyInfo
        {
            public string CurrentStudy;
            public List<string> AvailableStudies = new List<string>();
            public List<cProjectStudiesInfo> psi = new List<cProjectStudiesInfo>();

            public void GetStudyUsingName(string shortname)
            {
                foreach(cProjectStudiesInfo PSI in psi)
                {
                    if (PSI.ShortName == shortname)
                    {
                        AvailableStudies.Clear();
                        foreach(cStudyIDsDue sid in PSI.StudyIDsDue)
                        {
                            AvailableStudies.Add(sid.sStudy + ":" + sid.sStudyName);
                        }
                        string[] cs = (AvailableStudies.Count > 0 ?  AvailableStudies[0] : "").Split(":");
                        CurrentStudy = cs[0];
                        return;
                    }
                }
                return;
            }

            public class cProjectStudiesInfo
            {
                public string ShortName;
                public string sUrl;
                public string sHostUrl;
                public List<cStudyIDsDue> StudyIDsDue = new List<cStudyIDsDue>();

                public cStudyIDsDue GetSIDs(string sStudy)
                {
                    if (sStudy == "")
                        sStudy = "0";
                    if (StudyIDsDue == null) return null;;
                    foreach(cStudyIDsDue sid in StudyIDsDue)
                    {
                        if(sid.sStudy == sStudy)return sid;
                    }
                    return StudyIDsDue[0];
                }

                public void UpdateStudy(string sStudy, string newStudyID, string sStudyName, int nDays, double nCPUs, double nGPUs)
                {
                    int i = -1;
                    foreach(cStudyIDsDue sid  in StudyIDsDue)
                    {
                        i++;
                        if(sStudy == sid.sStudy)
                        {
                            if(sStudyName != "")    
                                sid.sStudyName = sStudyName;
                            if(nDays != 0)
                                sid.nDueDuration = nDays;
                            if (newStudyID == "-1")
                            {
                                StudyIDsDue.RemoveAt(i);
                                return;
                            }
                            if (nCPUs > 0)
                                StudyIDsDue[i].CPUsUsed = nCPUs;
                            if (nGPUs > 0)  
                                StudyIDsDue[i].GPUsUsed = nGPUs;
                            sid.sStudy = newStudyID;
                            return;
                        }
                    }
                    cStudyIDsDue Newsid = new cStudyIDsDue();
                    Newsid.sStudy = sStudy;
                    if (sStudyName != "")
                        Newsid.sStudyName = sStudyName;
                    if (nDays != 0)
                        Newsid.nDueDuration = nDays;
                    if (nCPUs > 0)
                        Newsid.CPUsUsed = nCPUs;
                    if (nGPUs > 0)
                        Newsid.GPUsUsed = nGPUs;
                    Newsid.nItems = 0;
                    Newsid.bNew = true;
                    StudyIDsDue.Add(Newsid);
                }

                public void AddStudy(string sStudy, string sStudyName, int nItems)  // if true the rewrite config file
                {
                    int nDays = 0;
                    foreach (cStudyIDsDue SID in StudyIDsDue)
                    {
                        if (SID.sStudy == sStudy)
                        {
                            if (SID.sStudyName == sStudyName)
                            {
                                if(nItems >=0)SID.nItems = nItems;
                                //SID.nDueDuration = nDays;
                                return;
                            }
                            if (sStudyName == "")
                                sStudyName = "unknown";
                            if(SID.sStudyName == "" || SID.sStudyName == "unknown")
                                SID.sStudyName =  sStudyName;
                            if (nItems >= 0) SID.nItems = nItems;
                            return;
                        }
                    }
                    cStudyIDsDue sid = new cStudyIDsDue();
                    sid.sStudy = sStudy;
                    sid.sStudyName = sStudyName;
                    sid.nDueDuration = 0;
                    sid.CPUsUsed = 1.0;
                    sid.GPUsUsed = 1.0; // default is 1 cpu and 1 gpu
                    sid.bNew = true;
                    StudyIDsDue.Add(sid);
                    return;
                }
            }

            public class cStudyIDsDue
            {
                public string sStudy;
                public string sStudyName;
                public int nItems;  // usually 0 since projects remove old results
                public int nDueDuration;  // days due
                public double CPUsUsed; // 0.25 = 1/4 of a cpu is all that is needed.  4 means 4 cpus were used for the study (milkyway)_
                // default is 1 cpu except for milkyway, set default to 4 always and prevent from being used unless PC has > 4 cpus
                public double GPUsUsed; // if 0.5 then two tasks per GPU were used
                public bool bNew;   // a new one we just added
            }


            public cProjectStudiesInfo ThisPSI(string shortname)
            {
                foreach (cProjectStudiesInfo PSI in psi)
                {
                    if (shortname == PSI.ShortName) return PSI;
                }
                //Debug.Assert(false);
                return null;
            }
            private string WhereStudyFile = "";

            public void init(ref cProjectStruct ProjectStats, string sPath)
            {
                string[] StudyConfigFile = null;
                psi.Clear();
                WhereStudyFile = sPath;
                int iPS = -1;
                int nSCF = 0;

                if (File.Exists(WhereStudyFile))
                {
                    StudyConfigFile = File.ReadAllLines(WhereStudyFile);
                    nSCF = StudyConfigFile.Length;
                }
                else StudyConfigFile = null;
                
                

                foreach (cPSlist ps in ProjectStats.ProjectList)
                {
                    iPS++;
                    cProjectStudiesInfo PSI = new cProjectStudiesInfo();
                    PSI.sUrl = ps.sURL + ps.sHid; // just need to add the host id!
                    if(nSCF>0)
                        ps.sStudyL = "";
                    PSI.ShortName = ProjectStats.ShortName(iPS);
                    PSI.StudyIDsDue = new List<cStudyIDsDue>();
                    psi.Add(PSI);
                }
                

                cProjectStudiesInfo PCIforSN = null;

                if (StudyConfigFile != null)
                {
                    foreach (string s in StudyConfigFile) // may not be in the same order as the projectlist
                    {
                        string[] quad = s.Split(','); // 5 items now
                        int n = quad.Length;
                        string ShortName = quad[0];
                        PCIforSN = ThisPSI(ShortName);
                        cStudyIDsDue sid = new cStudyIDsDue();
                        sid.sStudy = quad[1];
                        int iLoc = ProjectStats.ShortnameToIndex(ShortName);
                        ProjectStats.ProjectList[iLoc].sStudyL += sid.sStudy + " ";
                        sid.sStudyName = quad[2];
                        sid.nItems = Convert.ToInt32(quad[3]);
                        sid.nDueDuration = Math.Abs(Convert.ToInt32(quad[4]));
                        sid.CPUsUsed = Convert.ToDouble(quad[5]);
                        sid.GPUsUsed = Convert.ToDouble(quad[6]);
                        if(false)
                        {
                            if(ShortName == "milkyway")
                            {
                                sid.CPUsUsed = 4.0; // milkyway uses 4 cpus by default
                                sid.GPUsUsed = 0;
                            }
                            else if(ShortName == "nfs")
                            {
                                sid.CPUsUsed = 1; 
                                sid.GPUsUsed = 0;
                            }
                            else
                            {
                                sid.CPUsUsed = 1.0;
                                sid.GPUsUsed = 1.0;  // unknown but use 1.0
                            }
                        }
                        PCIforSN.StudyIDsDue.Add(sid);
                    }
                }

                bool bMissing = false;
                for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
                {
                    string shortName = ProjectStats.ShortName(i);

                    PCIforSN = ThisPSI(shortName);
                    if(PCIforSN.StudyIDsDue.Count == 0) // database of studies is missing an entry or has been deleted
                    {
                        int iLoc = ProjectStats.ShortnameToIndex(shortName);
                        cPSlist ps = ProjectStats.ProjectList[i];
                        string[] ss = ps.sStudyL.Split(' ');
                        cProjectStudiesInfo PCI = new cProjectStudiesInfo();
                        PCI.ShortName = shortName;
                        PCI.sUrl = ps.sURL + ps.sHid;
                        foreach (string s in ss)
                            PCI.AddStudy(s, "unknown", -1);
                        psi.Add(PCI);
                        bMissing = true;
                    }
                }                


                bool bMustUpdate = bMissing;
                foreach(PandoraConfig.cCalcLimitProj clp in ProjectStats.TempletDB.ProjList)
                {
                    string shortName = clp.ShortName;
                    foreach(cProjectStudiesInfo p in psi)
                    {
                        if(p.ShortName == shortName)
                        {
                            foreach (cStudyIDsDue sid in p.StudyIDsDue)
                            {
                                if(clp.sStudy == sid.sStudy)
                                {
                                    if(sid.nDueDuration == 0)
                                    {
                                        int j = Convert.ToInt32(clp.BunkerStart);
                                        for (int i = 0; i < p.StudyIDsDue.Count; i++)
                                        {
                                            p.StudyIDsDue[i].nDueDuration = Math.Abs(j);
                                            bMustUpdate = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if(bMustUpdate)
                {
                    SaveStudyFile();
                }
            }

            public string SaveStudyFile()
            {
                int NumNew = 0;
                string sOut = "";
                List<string> quad = new List<string>();
                
                foreach (cProjectStudiesInfo PSI in psi)
                {                    
                    foreach (cStudyIDsDue sid in PSI.StudyIDsDue)
                    {

                        quad.Add(PSI.ShortName + "," + sid.sStudy + "," + sid.sStudyName + "," + sid.nItems + "," +
                            sid.nDueDuration.ToString() +"," + sid.CPUsUsed.ToString("F2") + "," + sid.GPUsUsed.ToString("F2"));
                        if (sid.bNew)
                        {
                            NumNew++;
                            if (sid.sStudyName == "")
                                sid.sStudyName = "unknown";
                            sOut += "New " + PSI.ShortName + ":" + sid.sStudy + "," + sid.sStudyName + "," + sid.nItems + "," +
                                sid.nDueDuration.ToString() + sid.CPUsUsed.ToString("F2") + "," + sid.GPUsUsed.ToString("F2")
                                + Environment.NewLine;
                        }
                    }
                }
                string[] StudyConfigFile = quad.ToArray();
                File.WriteAllLines(WhereStudyFile, StudyConfigFile);
                return sOut;
            }

            public cProjectStudiesInfo GetStudyElements(string shortName)
            {
                foreach (cProjectStudiesInfo PSI in psi)
                {
                    if (PSI.ShortName == shortName) return PSI;
                }
                Debug.Assert(false, "name missing from table " + shortName);
                return null;
            }
            public string GetStudyInfoList(string ProjName)
            {
                string sOut = "";
                List<string> quad = new List<string>();
                foreach (cProjectStudiesInfo PSI in psi)
                {
                    foreach (cStudyIDsDue sid in PSI.StudyIDsDue)
                    {
                        if(ProjName == "")
                        {
                            if (sid.sStudyName == "")
                                sid.sStudyName = "unknown";
                            sOut += PSI.ShortName + ":" + sid.sStudy + "," + sid.sStudyName + "," + sid.nItems + "," + sid.nDueDuration.ToString() + "," +
                                 sid.CPUsUsed.ToString("F2") + "," + sid.GPUsUsed.ToString("F2") +Environment.NewLine;
                        }
                        else if (PSI.ShortName == ProjName)
                        {
                            if (sid.sStudyName == "")
                                sid.sStudyName = "unknown";
                            sOut += PSI.ShortName + ":" + sid.sStudy + "," + sid.sStudyName + "," + sid.nItems + "," + sid.nDueDuration.ToString() + "," +
                                 sid.CPUsUsed.ToString("F2") + "," + sid.GPUsUsed.ToString("F2") + Environment.NewLine;
                        }

                    }
                }
                return sOut;
            }
        }

        public class cNoHeaderProj
        {
            private string nl = Environment.NewLine;
            public int nAll;
            public int nValid;
            public int nInvalid;
            public int nError;
            public int nInProgress;
            public string sProjectName;
            public List<cNoHeaderBody> NoHdrList = new List<cNoHeaderBody>();

            public string GetHDR()
            {
                return "All (" + nAll.ToString() + ") " + nl +
                    "Valid (" + nValid.ToString() + ") " + nl +
                    "Invalid (" + nInvalid.ToString() + ") " + nl +
                    "Error (" + nError.ToString() + ")" + nl +
                    "In progress (" + nInProgress.ToString() + ")" + nl;
            }
            public void Init(string sPJname)
            {
                nAll = 0; nValid = 0; nInvalid = 0; nError = 0; nInProgress = 0;
                NoHdrList.Clear();
                sProjectName = sPJname;
            }
            public void AddBody(ref cNoHeaderBody Body)
            {
                NoHdrList.Add(Body);
                nValid++;
                nAll++;
            }
            public int ProcessRawBodyYOYO(ref string sRaw)
            {
                int n = 0;
                int iStart = sRaw.IndexOf("<h2>Results for computer</h2>");
                if (iStart < 0) return 0;
                iStart = sRaw.IndexOf("</tr>", iStart);
                iStart += 5;
                int iEnd = sRaw.IndexOf("</table>", iStart);
                string[] spara = sRaw.Substring(iStart, iEnd - iStart).Split(new string[] { "<tr>" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in spara)
                {
                    string[] sS = s.Split(new string[] { "<td>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (sS.Length == 10)
                    {
                        if (sS[5].Contains("Success"))
                        {
                            cNoHeaderBody Body = new cNoHeaderBody();
                            Body.AddGranted(sS[9]);
                            Body.AddCpuEla(sS[7]);
                            Body.AddDate(sS[3]);
                            //3,7,9 are date, seconds, granted
                            //NoHdrList.Add(Body);
                            AddBody(ref Body);
                            n++;
                        }
                        else
                        {
                            nAll++;
                            n++;
                            if (sS[5].Contains("Validate"))
                                nInvalid++;
                            else if (sS[5].Contains("Error"))
                                nError++;
                            else if (sS[5].Contains("Unknown"))
                                nInProgress++;
                        }
                    }
                }
                return n;
            }
        }
        public class cNoHeaderBody
        {
            public DateTime tCompleted;
            public bool bValid;
            public bool bInvalid;
            public bool bError;
            public double ElapsedSecs;
            public double CPUtimeSecs;
            public double Credits;
            private string sNoTD(string s)
            {
                int i = s.IndexOf("</td>");
                Debug.Assert(i > 0);
                return s.Substring(0, i).Trim();
            }
            public void AddDate(string sTD)
            {
                string s = sNoTD(sTD);
                int i = s.IndexOf("UTC");
                if (i > 0)
                {
                    s = s.Substring(0, i).Trim();
                }
                tCompleted = DateTime.Parse(s).ToUniversalTime();
            }
            public void AddCpuEla(string sTD)
            {
                string s = sNoTD(sTD);
                CPUtimeSecs = double.Parse(s, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                ElapsedSecs = CPUtimeSecs;
            }
            public void AddGranted(string sTD)
            {
                string s = sNoTD(sTD);
                Credits = double.Parse(s, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            }
        }

    }
}
