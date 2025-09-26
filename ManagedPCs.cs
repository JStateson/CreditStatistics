using Microsoft.Win32;
using System.Diagnostics;
using System.Management;
using System.Net;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;


namespace CreditStatistics
{
    internal class cManagedPCs
    {
        internal List<cHostInfo> LocalSystems = new List<cHostInfo>();
        public string CurrentComputerID;
        public string CurrentProjectsID;
        public string CurrentUserName;
        public string CurrentPassword;
        public string CurrentOStype;
        public string strResult;    // for error or status info
        public int ErrorStatus;     //0: no error, 1: info, 2: warning, 3: critical error, 4: fatal error
        public int CurrentProcessorCount;
        public int CurrentGPUcount;
        public string CurrentIPaddress;
        public bool CurrentHasSSH;     //port 22
        public bool CurrentHasBOINC;   //31415
        public bool CurrentSSHvalid;
        public bool CurrentBOINCvalid;
        public string sVersion;
        public List<string> LastStudyIDselected = new List<string>();
        public List<string> AvailableProjectIDs = new List<string>();
        public List<string> MatchingShortnames = new List<string>();
        public List<bool>HasAppConfig = new List<bool>();
        public PandoraRPC rpc = new PandoraRPC();

        public string Init(ref cProjectStruct ProjectStats)
        {
            string sRtn = "";
            List<string> MasterUrls = new List<string>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Space Sciences Laboratory, U.C. Berkeley\BOINC Setup");
            if (key == null)
            {
                return "Boinc not installed";
            }
            string sLocData = key.GetValue("DATADIR")?.ToString();
            string sLocBins = key.GetValue("INSTALLDIR")?.ToString();
            key.Close();
            PathToBoincData = sLocData;
            PathTo_boinccmd_exe = Path.Combine(sLocBins, "boinccmd.exe");
            PathTo_boinc = sLocBins;

            string sMaster = "";
            string uMaster = "";
            string sCL = Path.Combine(sLocData, "all_projects_list.xml");
            string[] Lines = File.ReadAllLines(sCL);

            foreach (string uline in Lines)
            {
                string line = uline.ToLower();
                int i = line.IndexOf("<web_url>");
                if (i < 0) continue;
                i += 9;
                int j = line.IndexOf("</web_url>", i);
                Debug.Assert(j >= 0);
                sMaster = line.Substring(i, j - i);
                uMaster = uline.Substring(i, j - i);
                int iLoc = ProjectStats.GetUrlIndex(sMaster);
                if (iLoc < 0)
                {
                    sRtn += "Project " + sMaster + " is unknown to this app" + Environment.NewLine;
                }
                else ProjectStats.ProjectList[iLoc].MasterUrl = uMaster;
                MasterUrls.Add(sMaster);
            }

            sCL = Path.Combine(sLocData, "client_state.xml");
            Lines = File.ReadAllLines(sCL);


            foreach (string uline in Lines)
            {
                string line = uline.ToLower();
                int i = line.IndexOf("<master_url>");
                if (i < 0) continue;
                i += 12;
                int j = line.IndexOf("</master_url>", i);
                Debug.Assert(j >= 0);
                sMaster = line.Substring(i, j - i);
                uMaster = uline.Substring(i, j - i);
                int iLoc = ProjectStats.GetUrlIndex(sMaster);
                if (iLoc < 0)
                {
                    sRtn += "Project " + uMaster + " is unknown and probably discontinued" + Environment.NewLine;
                }
                else if (ProjectStats.ProjectList[iLoc].MasterUrl == "")
                    ProjectStats.ProjectList[iLoc].MasterUrl = uMaster;
                if (!MasterUrls.Contains(sMaster))
                    MasterUrls.Add(sMaster);
            }



            foreach (string sUrl in MasterUrls)
            {
                int n = ProjectStats.GetUrlIndex(sUrl);
                if (n < 0)
                {
                    sRtn += "Ignoring project " + sUrl + Environment.NewLine;
                    continue;
                }
            }

            for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
            {
                if (ProjectStats.ProjectList[i].MasterUrl == "")
                {
                    sRtn += "!!Missing master url for project " + ProjectStats.ProjectList[i].name + Environment.NewLine;
                }
            }

            sRtn += TryFormMasterPandora(ref MasterUrls, ref ProjectStats);

            int ProcessorCount = Environment.ProcessorCount;
            int GPUcount = 0;
            int nIntelGPU = 0;
            int nNvidia = 0;
            int nAmdGPU = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                GPUcount++; // could be lower if boinc excludes some GPUs
                string sGPU = obj["Name"].ToString().ToLower();
                if (sGPU.Contains("nvidia")) nNvidia++;
                else if (sGPU.Contains("amd") || sGPU.Contains("radeon") || sGPU.Contains("ati")) nAmdGPU++;
                else if (sGPU.Contains("intel")) nIntelGPU++;
            }
            cHostInfo hi = AddPC(Dns.GetHostName().ToLower(), ProcessorCount, GPUcount, "w", "127.0.0.1");
            hi.UserName = Environment.UserName;
            hi.Password = "";
            SelectCurrentFromIndex(0);
            return sRtn;
        }

        public void ChangeStudy(string PCname, string shortname, string sStudy)
        {
            cHostInfo hi = NameToSystem(PCname);
            
        }

        public string RunScpAndGetOutput(string PCname, string sLocalFile, string sRemoteFile, string CK)
        {
            string Argument = "";
            cHostInfo hi = NameToSystem(PCname);
            string remoteUser = hi.UserName;
            string remotePassword = hi.Password;
            string remoteHost = hi.IPaddress;
            string ck = "/" + CK;
            ErrorStatus = 0;
            strResult = "";

            if (remoteHost == "127.0.0.1")  //local pc is windows and there is no SSH server
            {
                try
                {
                    string stemp = File.ReadAllText(sLocalFile);
                    File.WriteAllText(sRemoteFile, stemp);
                }
                catch (Exception e)
                {
                    return "Error transfering file " + sLocalFile + Environment.NewLine;
                }

                return sLocalFile + " copied to " + sRemoteFile + Environment.NewLine;
            }


            if (hi.OStype != "w")
            {
                Argument = ck + $" scp {sLocalFile}  {remoteUser}@{remoteHost}:{sRemoteFile}";
            }
            else
            {
                Argument = ck + $" scp {sLocalFile}  {remoteHost}:{sRemoteFile}";
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

            try
            {
                using (var process = Process.Start(psi))
                {
                    if (bRedirect)
                    {
                        // Read asynchronously to avoid deadlock
                        string output = "";
                        string error = "";

                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (e.Data != null)
                                output += e.Data + Environment.NewLine;
                        };
                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data != null)
                                error += e.Data + Environment.NewLine;
                        };

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        // Timeout safety: wait up to 10s (adjust as needed)
                        if (!process.WaitForExit(5000))
                        {
                            process.Kill();
                            throw new TimeoutException("Process hung: " + psi.Arguments);
                        }

                        // Check for SSH failure only (network issues, bad command, etc.)
                        if (process.ExitCode == 255 || (string.IsNullOrWhiteSpace(output) && !string.IsNullOrWhiteSpace(error)))
                        {
                            strResult = "Error launching cmd: " + Argument + " - " + error.Trim();
                            ErrorStatus = ERR_critical;
                            hi.HasSSH = false;
                            MessageBox.Show(strResult);
                        }
                        return output.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                strResult = "Exception running process: " + ex.Message;
                ErrorStatus = ERR_critical;
                MessageBox.Show(strResult);
                hi.HasSSH = false;
                return strResult;
            }
            return strResult;
        }


        public string RunSshAndGetOutput(string PCname, string remoteCommand, string CK)
        {
            string Argument = "";
            cHostInfo hi = NameToSystem(PCname);
            string remoteUser = hi.UserName;
            string remotePassword = hi.Password;
            string remoteHost = hi.IPaddress;
            string ck = "/" + CK;
            ErrorStatus = 0;
            strResult = "";

            if (hi.OStype != "w")
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

            try
            {
                using (var process = Process.Start(psi))
                {
                    if (bRedirect)
                    {
                        // Read asynchronously to avoid deadlock
                        string output = "";
                        string error = "";

                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (e.Data != null)
                                output += e.Data + Environment.NewLine;
                        };
                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data != null)
                                error += e.Data + Environment.NewLine;
                        };

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        // Timeout safety: wait up to 30s (adjust as needed)
                        if (!process.WaitForExit(10000))
                        {
                            process.Kill();
                            throw new TimeoutException("Process hung: " + psi.Arguments);
                        }

                        // Check for SSH failure only (network issues, bad command, etc.)
                        if (process.ExitCode == 255 || (string.IsNullOrWhiteSpace(output) && !string.IsNullOrWhiteSpace(error)))
                        {
                            strResult = "Error launching cmd: " + Argument + " - " + error.Trim();
                            ErrorStatus = ERR_critical;
                            hi.HasSSH = false;
                            MessageBox.Show(strResult);
                        }
                        return output.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                strResult = "Exception running process: " + ex.Message;
                ErrorStatus = ERR_critical;
                MessageBox.Show(strResult);
                hi.HasSSH = false;
                return strResult;
            }
            return "";
        }


        public void SSHconsole(string PCname)
        {
            string Argument = "";
            cHostInfo hi = NameToSystem(PCname);
            string remoteUser = hi.UserName;
            string remotePassword = hi.Password;
            string remoteHost = hi.IPaddress;
            string remoteCommand = "";
            string ck = "/c";
            if (hi.OStype != "w" )
            {
                Argument = remoteUser == "" ? ck + $" ssh {remoteHost} \"{remoteCommand}\"" :
                            ck + $" ssh {remoteUser}@{remoteHost} \"{remoteCommand}\"";
            }
            else
            {
                Argument = remoteUser == "" ? ck + $" ssh {remoteHost} {remoteCommand}" :
                    ck + $" ssh {remoteUser}@{remoteHost} {remoteCommand}";
            }
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = Argument,
                UseShellExecute = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = false
            };

            Process.Start(psi);
        }

        public void RestartBoinc(string PCname)
        {
            cHostInfo hi = NameToSystem(PCname);
            string sCommand = hi.OStype != "w" ? "sudo systemctl restart boinc-client" : "schtasks /run /tn \"CopyNewPandora\"";
            if (hi.IPaddress != "127.0.0.1")
            {
                RunSshAndGetOutput(PCname, sCommand, "c");  //(PCl.PCname, sCommand, "c");                    
            }
            else
            {
                string sResult = ContactPCproject(PCname, " --quit", "");
                if (ErrorStatus != 0) return;
                System.Threading.Thread.Sleep(10000);
                sResult = globals.BoincCommand("boinc.exe", " --detach --allow_remote_gui_rpc");
            }
        }

        public async Task sendPCproject(string PCname, string sCmd, string MasterUrl)
        {
            await rpc.SendCmd(PCname, sCmd, MasterUrl);
            cHostInfo hi = NameToSystem(PCname);
            hi.HasBOINC = rpc.cmdRequest.CMDerr == 0;          
        }

        public string ContactPCproject(string PCname, string sCmd, string MasterUrl)
        {
            cHostInfo hi = NameToSystem(PCname);
            if (!hi.HasBOINC) return "offline";
            string sArgs = "";

            if(MasterUrl != "")
            {

                if (hi.IPaddress == "127.0.0.1")
                    sArgs = " --project " + MasterUrl + " " + sCmd;
                else
                    sArgs = " --host " + hi.IPaddress + " --project " + MasterUrl + " " + sCmd;
            }

            else
            {

                if (hi.IPaddress == "127.0.0.1")
                    sArgs =  sCmd;
                else
                    sArgs = " --host " + hi.IPaddress + " " +  sCmd;
            }

            try
            {

                var psi = new ProcessStartInfo
                {
                    FileName = Path.Combine(PathTo_boinc, "boinccmd.exe"),
                    Arguments = sArgs,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(psi))
                {
                    // Read asynchronously to avoid deadlock
                    string output = "";
                    string error = "";

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                            output += e.Data + Environment.NewLine;
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                            error += e.Data + Environment.NewLine;
                    };

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Timeout safety: wait up to 5s (adjust as needed)
                    if (!process.WaitForExit(5000))
                    {
                        process.Kill();
                        throw new TimeoutException("Process hung: " + psi.Arguments);
                    }

                    // Check for SSH failure only (network issues, bad command, etc.)
                    if (process.ExitCode == 255 || (string.IsNullOrWhiteSpace(output) && !string.IsNullOrWhiteSpace(error)))
                    {
                        strResult = "Error launching cmd: " + sArgs + " - " + error.Trim();
                        ErrorStatus = ERR_critical;
                        hi.HasBOINC = false;
                        MessageBox.Show(strResult);
                    }
                    return output.Trim();
                }
            }
            catch (Exception ex)
            {
                strResult = "Exception running process: " + ex.Message;
                ErrorStatus = ERR_critical;
                MessageBox.Show(strResult);
                hi.HasBOINC = false;
                return strResult;
            }
            return "";
        }

        public Color GetColor(string PCname)
        {
            cHostInfo hi = NameToSystem(PCname);
            Color color = Color.Black;   //assume worst case
            if (hi.HasSSH || hi.IPaddress == "127.0.0.1")
            {
                if (hi.HasBOINC) color = Color.Blue;
                else color = Color.Red;
            }
            return color;
        }


        // some sprint PCs no longer exist and could be removed from boinctasks
        public bool IsSystemManaged(string PCname)
        {
            foreach(cHostInfo ls in LocalSystems)
            {
                if(ls.ComputerID == PCname)
                {
                    return true;
                }
            }
            return false;
        }

        public cHostInfo NameToSystem(string PCname)
        {
            int i = 0;
            foreach (cHostInfo ls in LocalSystems)
            {
                if (ls.ComputerID == PCname)
                {
                    return ls;
                }
            }
            Debug.Assert(false, "PCname not in database");
            return null;
        }

        public int NameToIndex(string PCname)
        {
            int i = 0;
            foreach (cHostInfo ls in LocalSystems)
            {
                if (ls.ComputerID == PCname)
                {
                    return i;
                }
                i++;
            }
            Debug.Assert(false, "internal error, cannot find PC " + PCname);
            return -1;
        }

        public string[] SaveManagedPCs()
        {
            string sOut = "";
            foreach(cHostInfo hi in LocalSystems)
            {
                sOut += hi.ComputerID + ":";
                foreach (cLHe lh in hi.LocalProjID)
                {
                    sOut += lh.ProjectName + " " + lh.ProjectsID + " " + lh.LastStudyID + " " + (lh.HasAppConfig ? "1" : "0") +  ",";
                }
                sOut = sOut.Substring(0, sOut.Length - 1) + Environment.NewLine;
            }
            string[] sTemp = sOut.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            File.WriteAllLines(globals.WhereDefaultHostList, sTemp);
            return sTemp;
        }
        
        public  cHostInfo AddPC(string pcName, int nCPUs, int nGPUs, string OStype, string IPaddress)
        {
            foreach (cHostInfo hI in LocalSystems)
            {
                if (hI.ComputerID == pcName)
                {
                    hI.ProcessorCount = nCPUs;
                    hI.GPUcount = nGPUs;
                    hI.OStype = OStype;
                    hI.IPaddress = IPaddress;
                    return hI;
                }
            }
            cHostInfo hInfo = new cHostInfo();
            hInfo.ComputerID = pcName;
            hInfo.ProcessorCount = nCPUs;
            hInfo.GPUcount = nGPUs;
            hInfo.OStype = OStype;
            hInfo.IPaddress = IPaddress;
            LocalSystems.Add(hInfo);
            return hInfo;
        }

        public cHostInfo AddProj(string pcName, string shortName, string projectID, int iLoc, string LastStudyID, bool HasAppConfig)
        {
            cLHe clex;
            foreach (cHostInfo hInfo in LocalSystems)
            {
                if (hInfo.ComputerID == pcName)
                {
                    foreach(cLHe ce in hInfo.LocalProjID)
                    {
                        if(ce.ProjectName == shortName) // check for dups
                        {
                            return hInfo;
                        }
                    }
                    clex = new cLHe();
                    clex.ProjectName = shortName;
                    clex.ProjectsID = projectID;
                    clex.LastStudyID = LastStudyID;
                    clex.IndexToProjectList = iLoc;
                    clex.HasAppConfig = HasAppConfig;
                    hInfo.LocalProjID.Add(clex); // todo to do must not add same one
                    return hInfo;
                }
            }
            cHostInfo hi = new cHostInfo();
            hi.ComputerID = pcName;
            hi.ProcessorCount = 0;
            hi.GPUcount = 0;
            clex = new cLHe();
            clex.ProjectName = shortName;
            clex.ProjectsID = projectID;
            clex.LastStudyID = LastStudyID;
            clex.IndexToProjectList = iLoc;
            clex.HasAppConfig = HasAppConfig;
            hi.LocalProjID.Add(clex); // todo to do must not add same one
            LocalSystems.Add(hi);
            return hi;
        }

        //same as getidfromname
        //SelectCurrentFromRadioButtonProject
        public string ProjectIDfromPCsShortname(string shortname)
        {
            int i = -1;
            foreach (string sn in MatchingShortnames)
            {
                i++;
                if (sn == shortname)
                {
                    CurrentProjectsID = AvailableProjectIDs[i];
                    return CurrentProjectsID;
                }
            }
            CurrentProjectsID = "";
            return "";
        }

        public void SelectCurrentFromIndex(int n)
        {
            CurrentComputerID = LocalSystems[n].ComputerID;
            AvailableProjectIDs.Clear();
            MatchingShortnames.Clear();
            HasAppConfig.Clear();
            LastStudyIDselected.Clear();
            cHostInfo hInfo = LocalSystems[n];
            CurrentProcessorCount = hInfo.ProcessorCount;
            CurrentGPUcount = hInfo.GPUcount;
            CurrentOStype = hInfo.OStype;
            CurrentIPaddress = hInfo.IPaddress;
            CurrentUserName = hInfo.UserName;
            CurrentPassword = hInfo.Password;
            CurrentBOINCvalid = hInfo.BOINCvalid;
            CurrentHasSSH = hInfo.HasSSH;
            CurrentHasBOINC = hInfo.HasBOINC;
            CurrentSSHvalid = hInfo.SSHvalid;
            sVersion = hInfo.sVersion;
            foreach (cLHe cle in hInfo.LocalProjID)
            {
                AvailableProjectIDs.Add(cle.ProjectsID);
                MatchingShortnames.Add(cle.ProjectName);
                HasAppConfig.Add(cle.HasAppConfig);
                LastStudyIDselected.Add(cle.LastStudyID);
            }
        }

        public void SelectCurrentFromPC(string pcName)
        {
            if(pcName== "localhost")
                pcName = Environment.MachineName.ToLower();
            CurrentComputerID = pcName;
            AvailableProjectIDs.Clear();
            MatchingShortnames.Clear();
            LastStudyIDselected.Clear();
            HasAppConfig.Clear();

            foreach (cHostInfo hInfo in LocalSystems)
            {
                hInfo.IsSelected = false;
                if (hInfo.ComputerID == pcName)
                { 
                    CurrentProcessorCount = hInfo.ProcessorCount;
                    CurrentGPUcount = hInfo.GPUcount;
                    CurrentOStype = hInfo.OStype;
                    CurrentIPaddress= hInfo.IPaddress;
                    CurrentUserName = hInfo.UserName;
                    CurrentPassword = hInfo.Password;
                    CurrentBOINCvalid = hInfo.BOINCvalid;
                    CurrentHasSSH = hInfo.HasSSH;
                    CurrentHasBOINC = hInfo.HasBOINC;
                    CurrentSSHvalid = hInfo.SSHvalid;
                    hInfo.IsSelected = true;
                    sVersion = hInfo.sVersion;
                    foreach (cLHe cle in hInfo.LocalProjID)
                    {
                        AvailableProjectIDs.Add(cle.ProjectsID);
                        MatchingShortnames.Add(cle.ProjectName);
                        HasAppConfig.Add(cle.HasAppConfig);
                        LastStudyIDselected.Add(cle.LastStudyID);
                    }
                    return;
                }
            }
            Debug.Assert(false, "PCname missing from database");
        }

        public async Task GetRemotePCinfo()
        {
            PCresources pcResources = new PCresources();
            int n = pcResources.LoadPCresources();   
            if (n > 0)
            {
                LocalSystems[0].IsSelected = true; // only one so far, so it is selected
                foreach (PCresources.cCpuGpu cg in pcResources.CpuGpu)
                {
                    bool bHasBoinc = await PortChecker.IsPortOpenAsync(cg.IPaddress, 31416);
                    cHostInfo hi = AddPC(cg.sPC, cg.nCPU, cg.nGPU,cg.OSname,cg.IPaddress);
                    hi.HasBOINC = bHasBoinc;
                    hi.BOINCvalid = true;
                }            
            }
        }

        private string TryFormMasterPandora(ref List<string> MasterUrls, ref cProjectStruct ProjectStats)
        {
            string sErr = "";
            string NL = Environment.NewLine;
            int nMasters = Properties.Settings.Default.nMasterUrls;
            string sOut = "";
            if(MasterUrls.Count > nMasters || !File.Exists(WhereMasterPandora))
            {
                sOut += "bunker_strategy: 3" + NL;
                sOut += "message_filters:has reached a limit,your preferences are set,No tasks sent,No work sent,see scheduler log,#" + NL;
                sOut += "earliest_deadline_first" + NL; 
                sOut += "debug" + NL;
                nMasters = MasterUrls.Count;
                foreach (string s in MasterUrls)
                {
                    int ProjectIndex = ProjectStats.GetUrlIndex(s);
                    if(ProjectIndex < 0)
                    {
                        sErr += "Ignoring unknown project " + s + NL;
                        continue;
                    }
                    string ProjName = ProjectStats.ShortName(ProjectIndex);
                    cPSlist psl = ProjectStats.ProjectList[ProjectIndex];
                    sOut += NL + "project: " + s + NL;
                    sOut += "block_reports: 1000" + NL;   
                    string sWanted = psl.MinWUsNeeded.ToString();
                    sOut += "limit: " + sWanted + NL;   // this is the required minimum number of work units needed for an average                    
                    sOut += "#app_type: " + ProjectStats.GetDefaultAppType(ProjName);
                }
                sOut += "#" + NL;
                File.WriteAllText(WhereMasterPandora, sOut);
                Properties.Settings.Default.nMasterUrls = nMasters;
                Properties.Settings.Default.Save();
            }
            return sErr;
        }
    }
}
