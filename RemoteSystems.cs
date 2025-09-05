using CreditStatistics;
using Microsoft.Playwright;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;
using static CreditStatistics.globals;

namespace CreditStatistics
{
    internal partial class RemoteSystems : Form
    {
        cProjectStruct ProjectStats;
        cManagedPCs ManagedPCs;
        PandoraConfig pc;
        PCresources pcResources;
        string NL = Environment.NewLine;
        public bool HostsChanged { get; set; }

        public RemoteSystems(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            ManagedPCs = ProjectStats.ManagedPCs;
            pcResources = ProjectStats.pcResources;
            pc = new PandoraConfig(ref rProjectStats);
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Load += RemoteSystems_Load;
        }

        private async void RemoteSystems_Load(object sender, EventArgs e)
        {
            this.Enabled = false; // Disable clicks
            this.Shown += InitialLoad;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();  // Close the form
            }
        }

        private async void InitialLoad(object sender, EventArgs e)
        {
            BoincTaskFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\eFMer\\BoincTasks";
            WorkingFolderLoc.Text = globals.WhereDOC;
            tbWhereBoinc.Text = PathToBoincData;
            AppendColoredText(rtbLocalHostsBT, "Local PCs using 31416:" + NL + NL, Color.Blue);
            MakeDGV();
            this.Enabled = true; // Re-enable clicks
        }

        private void MakeDGV()
        {
            rtbLocalHostsBT.Clear();
            dgv.Rows.Clear();
            //Task aTask;
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                string uName = (hi.UserName == "" ? "null" : hi.UserName);
                string uPass = (hi.Password == "" ? "null" : hi.Password);

                /*
                aTask = Task.Run(async () =>
                {
                    hi.HasBOINC = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, 31416);
                });
                aTask.Wait();
                */


                if (hi.HasBOINC)
                    hi.sVersion = GetVersion(hi.IPaddress);

                dgv.Rows.Add(hi.ComputerID, hi.UserName, hi.Password, hi.HasBOINC,
                    hi.ProcessorCount.ToString(), hi.GPUcount.ToString(), hi.sVersion);

                uName = "UserName:" + uName;
                uPass = "Password:" + uPass;

                string s = hi.ComputerID.PadRight(16) + ("  CPUs:" + hi.ProcessorCount.ToString()).PadRight(12) +
                    ("GPUs:" + hi.GPUcount.ToString()).PadRight(10) + ("OS type: " + hi.OStype).PadRight(12) + NL;
                s += hi.IPaddress + NL;
                AppendColoredText(rtbLocalHostsBT, s + NL, Color.Blue);
            }
        }


        private void btnReadBoinc_Click(object sender, EventArgs e)
        {
            if (CreateAllHostList())
                CreateRemoteHostlist();
            MakeDGV();
        }

        private bool CreateAllHostList()
        {
            string FilePath = Path.Combine(BoincTaskFolder.Text, "computers.xml");
            if (!File.Exists(FilePath))
            {
                MessageBox.Show("Cannot find 'computers.xml'. This app uses boinctasks database");
                return false;
            }
            List<string> PCnames = new List<string>();
            string s;
            try
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        int i = line.IndexOf("<ip>");
                        if (i < 0) continue;
                        int j = line.IndexOf("</ip>", i);
                        Debug.Assert(j > 0);
                        s = line.Substring(i + 4, j - i - 4);
                        if (s == "localhost")
                            s = Dns.GetHostName();
                        PCnames.Add(s.ToLower());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading file: " + ex.Message);
                return false;
            }
            Properties.Settings.Default.AllHostList = PCnames.ToArray();
            Properties.Settings.Default.Save();
            return true;
        }

        public string GetVersion(string IPaddress)
        {
            string sArgs = "--host " + IPaddress + " --get_messages";
            string sResult = BoincCommand(sArgs).ToLower();
            return ParseVersion(sResult);
        }

        public async Task CreateLocalHost(string PCname, string IPaddress)
        {
            ManagedPCs.strResult = "";
            ManagedPCs.ErrorStatus = ERR_none;
            string sArgs = "--host " + IPaddress + " --get_messages";
            bool isOpen = await PortChecker.IsPortOpenAsync(IPaddress, 31416);
            if (isOpen)
            {
                string sResult = BoincCommand(sArgs).ToLower();
                if (sResult == "")
                {
                    ManagedPCs.strResult = "Using 31416, unable to contact boinc at " + PCname;
                    ManagedPCs.ErrorStatus = ERR_critical;
                    return;
                }
                sResult = ParseProjs(sResult, out List<int> SNlocs, out List<string> RawProjIDs);
                if (sResult == "FATAL")
                {
                    ManagedPCs.strResult = "unable to parse boinc message from " + PCname;
                    ManagedPCs.ErrorStatus = ERR_fatal;
                    return;
                }
                if (sResult != "")
                {
                    ManagedPCs.strResult = sResult;
                    ManagedPCs.ErrorStatus = ERR_warning;
                }

                int n = RawProjIDs.Count();
                for (int i = 0; i < n; i++)
                {
                    string sProj = RawProjIDs[i];
                    int sLoc = SNlocs[i];
                    string sName = ProjectStats.ShortName(sLoc);
                    ManagedPCs.strResult += "ProjName:" + sName + " ProjID:" + sProj + NL;
                    bool HasAppConfig = File.Exists(globals.GetAppConfigFilename(PCname, sName));
                    ManagedPCs.AddProj(PCname, sName, sProj, sLoc, ProjectStats.ProjectList[sLoc].sStudyV, HasAppConfig);
                }
                if (n == 0)
                {
                    ManagedPCs.strResult = PCname + "  has no projects";
                    ManagedPCs.ErrorStatus = ERR_critical;
                }
                else ManagedPCs.ErrorStatus = ERR_info;
                return;
            }
            ManagedPCs.strResult = "Using 31416, unable to contact boinc at " + PCname;
            ManagedPCs.ErrorStatus = ERR_critical;
        }


        static async Task<IPAddress?> GetIPv4AddressAsync(string host)
        {
            try
            {
                IPAddress[] addresses = await Dns.GetHostAddressesAsync(host);

                foreach (var addr in addresses)
                {
                    if (addr.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(addr))
                    {
                        return addr; // First valid IPv4
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"{host} → DNS lookup failed: {ex.Message}");
            }

            return null;
        }

        //called synchronous from a button see https://chatgpt.com/c/68977a1e-20e0-8331-8a7a-cd2c850f56db
        private async void CreateRemoteHostlist()
        {
            PandoraConfig pc = new PandoraConfig(ref ProjectStats);
            string[] PCnamesIN = Properties.Settings.Default.AllHostList;
            string[] PcIPs = new string[PCnamesIN.Length];
            int i = 0;
            pbTask.Maximum = PCnamesIN.Count();
            pbTask.Value = 0;
            Task aTask;
            bool bOnLine = true;
            foreach (string PCname in PCnamesIN)
            {
                pbTask.Value++;
                Application.DoEvents();
                IPAddress? ipv4 = await GetIPv4AddressAsync(PCname);
                if (ipv4 == null) PcIPs[i] = "";
                else
                {
                    string sIP = ipv4.ToString();

                    aTask = Task.Run(async () =>
                    {
                        bOnLine = await globals.PortChecker.IsPortOpenAsync(sIP, 31416);
                    });
                    aTask.Wait();

                    if (!bOnLine) sIP = "";

                    if (PCname == Dns.GetHostName().ToLower())
                        sIP = "127.0.0.1";
                    PcIPs[i] = sIP;
                }
                i++;
            }
            pbTask.Value = 0;
            Application.DoEvents();
            rtbLocalHostsBT.Clear();
            string r = Environment.NewLine;
            string s = "List of online PCs handled by BoincTasks" + r;
            AppendColoredText(rtbLocalHostsBT, s, Color.Blue);
            //HostRPC hostRPC;
            i = -1;
            foreach (string PC in PCnamesIN)
            {
                i++;
                lbTestPC.Text = PC;
                bool isOpen = PcIPs[i] != "";
                if (isOpen)
                {
                    string sOut = CountResources(PC, PcIPs[i], out string nCPU, out string nGPU, out string OStype);
                    if (sOut != "")
                    {
                        sOut += r + "IP address: " + PcIPs[i] + r + r;
                        AppendColoredText(rtbLocalHostsBT, sOut, Color.Blue);
                        pc.AddPC(PC, nCPU, nGPU, OStype, PcIPs[i]);
                        cHostInfo hi = ManagedPCs.NameToSystem(PC);
                        hi.OStype = OStype;
                    }
                    else
                    {
                        AppendColoredText(rtbLocalHostsBT, PC + " critical error: unable to count CPUs or GPUs" + r, Color.Red);
                    }
                }
                else
                {
                    AppendColoredText(rtbLocalHostsBT, PC + r, Color.Red);
                }
                pbTask.Value++;
                Application.DoEvents();
            }
            pcResources.SavePCresource();
            pbTask.Value = 0;
            ProjectStats.SavedHostList = ManagedPCs.SaveManagedPCs();
            ProjectStats.GetAllProjectIDs();
            string sTemp = "Remote host list now has " + pcResources.CpuGpu.Count.ToString() + " systems you can use.";
            AppendColoredText(rtbLocalHostsBT, sTemp, Color.Blue);
        }

        private string BoincCommand(string sCmd)
        {
            var psi = new ProcessStartInfo
            {
                FileName = PathTo_boinccmd_exe,
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
                    //MessageBox.Show("Error launching cmd:" + PathTo_boinccmd_exe + " " + sCmd);
                    return "";
                }
                return output.Trim(); // Strip newline characters
            }
        }

        private string CountResources(string shost, string IPaddress, out string sCPU, out string sGPU, out string OStype)
        {
            string host = shost;
            OStype = "w";
            if (Dns.GetHostName().ToLower() == shost)
                host = "localhost";
            sCPU = "0";
            sGPU = "0";
            string sArgs = "--host " + host + " --get_host_info"; ;
            string strResult = BoincCommand(sArgs).ToLower();
            if (strResult == "") return "";
            PCresources.cCpuGpu CpuGpu = pcResources.GpuCpuParse(shost, strResult);
            CpuGpu.IPaddress = IPaddress;
            OStype = CpuGpu.OSname;
            sCPU = CpuGpu.nCPU.ToString();
            sGPU = (CpuGpu.nIntel + CpuGpu.nVidia + CpuGpu.nAti).ToString();
            string sOut = "PCname:" + shost + NL + "CPUs:" + CpuGpu.nCPU.ToString() + NL;
            sOut += "NVidia GPUs:" + CpuGpu.nVidia + NL;

            sArgs = "--host " + host + " --get_messages";
            strResult = BoincCommand(sArgs).ToLower();
            if (strResult == "") return "";
            sOut += ParseProjs(strResult, out List<int> SNlocs, out List<string> RawProjIDs);
            for (int i = 0; i < RawProjIDs.Count(); i++)
            {
                string sProj = RawProjIDs[i];
                int sLoc = SNlocs[i];
                string sName = ProjectStats.ShortName(sLoc);
                sOut += "ProjName:" + sName + " ProjID:" + sProj + NL;
                bool hasAppConfig = File.Exists(globals.GetAppConfigFilename(shost, sName));
                ManagedPCs.AddProj(shost, sName, sProj, sLoc, ProjectStats.ProjectList[sLoc].sStudyV, hasAppConfig);
            }
            return sOut.Trim();
        }

        private string ParseVersion(string s)
        {
            int i, j;
            string[] lines = s.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                i = line.IndexOf("starting boinc client version");
                if ((i >= 0))
                {
                    i = line.IndexOf("build:");
                    j = line.LastIndexOf(" for ");
                    string sBuild = line.Substring(i + 6, j - (i + 6)).Trim();
                    return sBuild;
                }
            }
            return "";
        }

        //Starting BOINC client version 8.3.0 build:2025-08-08T00:10:43 for windows_x86_64
        private string ParseProjs(string s, out List<int> SNs, out List<string> IDs)
        {
            string sErr = "";
            string[] lines = s.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            IDs = new List<string>();
            SNs = new List<int>();
            int i, j;
            foreach (string line in lines)
            {
                if (line.Contains("setting up gui rpc socket"))
                {
                    return sErr;
                }
                i = line.IndexOf("computer id ");
                if (i > 0)
                {
                    if (IDs.Count == 21)
                    {

                    }
                    j = line.IndexOf(";", i + 12);
                    Debug.Assert(j > 0);
                    string id = line.Substring(i + 12, j - i - 12);
                    i = line.IndexOf("url http");
                    Debug.Assert(i > 0);
                    i += 4;
                    j = line.IndexOf(";");
                    Debug.Assert(j > 0);
                    string sUrl = line.Substring(i + 4, j - i - 4);
                    int k = ProjectStats.GetNameIndex(sUrl);
                    if (k < 0)
                    {
                        sErr += "Unknown project: " + sUrl + NL;
                        continue;
                    }
                    IDs.Add(id);
                    SNs.Add(k);
                }
            }
            Debug.Assert(false, "expected terminator 'setting up gui rpc' not found!");
            return "FATAL";
        }


        private void ShowClients()
        {

            int lSN = 0;    // length of shortname
            int lPN = 0;    // length of PC's i
            string NL = Environment.NewLine;
            string t = "";
            rtbLocalHostsBT.Clear();
            foreach (cPSlist ps in ProjectStats.ProjectList)
            {
                lPN = Math.Max(lPN, ps.shortname.Length);
                foreach (string sID in ps.HostPCNames)
                {
                    lSN = Math.Max(lSN, sID.Length);
                }
            }

            foreach (cPSlist ps in ProjectStats.ProjectList)
            {

                int n = ps.HostPCNames.Count;
                AppendColoredText(rtbLocalHostsBT, t + Rp(ps.shortname, lSN) + ": ", (n == 0 ? Color.Red : Color.Blue));
                t = "";

                foreach (string sID in ps.HostPCNames)
                {
                    AppendColoredText(rtbLocalHostsBT, Rp(sID, lPN) + "  ", Color.Blue);
                }
                t = NL;

                AppendColoredText(rtbLocalHostsBT, t + Rp("ID of PC:", lPN + 2), Color.Blue);
                t = "";
                foreach (string sID in ps.HostProjIDs)
                {
                    AppendColoredText(rtbLocalHostsBT, Rp(sID, lPN) + "  ", Color.Blue);
                }
                t = NL + NL;
            }
        }

        private void btnListPCs_Click(object sender, EventArgs e)
        {
            ShowClients();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                string pName = row.Cells[0].Value.ToString();
                string uName = row.Cells[1].Value.ToString();
                string uPass = row.Cells[2].Value.ToString();
                int nCPUs = Convert.ToInt32(row.Cells["nCPUs"].Value.ToString());
                int nGPUs = Convert.ToInt32(row.Cells["nGPUs"].Value.ToString());
                ProjectStats.sshCredentials.Add(pName, uName, uPass);
                cHostInfo hi = ManagedPCs.NameToSystem(pName);
                hi.UserName = uName;
                hi.Password = uPass;
                cPClimit PCl = pc.NameToSprintPC(hi.ComputerID);
                PCl.UserName = uName;
                PCl.Password = uPass;
                hi.GPUcount = nGPUs;
                PCl.ConcurrentGpuTasks = nGPUs;
                hi.ProcessorCount = nCPUs;
                PCl.ConcurrentCpuTasks = nCPUs;
                pc.WriteDBrecord(ref PCl);
            }
            ProjectStats.sshCredentials.SaveCredentials();
            MakeDGV();
        }

        private void btnIDNotepad_Click(object sender, EventArgs e)
        {
            CSendNotepad SendNotepad = new CSendNotepad();
            string npTitle = "PC - Project matrix";
            SendNotepad.PasteToNotepad(rtbLocalHostsBT.Text);
        }

        private async void btnFetchAC_Click(object sender, EventArgs e)
        {
            PandoraRPC pandoraRPC = new PandoraRPC();
            pandoraRPC.Init(ref ProjectStats);
            Color fC;
            rtbLocalHostsBT.Clear();
            foreach (cPClimit PCl in ProjectStats.PandoraDatabase)
            {
                string PCname = PCl.PCname;
                foreach(cCalcLimitProj CLp in PCl.ProjList)
                {
                    string ShortName = CLp.ShortName;
                    await pandoraRPC.FetchOne_app_config(PCname, ShortName);
                    if(PCl.ErrorStatus < ERR_critical)
                    {
                        string[] ssAppConfig = PCl.strResult.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        string sOut = globals.WriteACrecord(GetAppConfigFilename( PCname, ShortName), ref ssAppConfig);
                        fC = Color.Blue;
                        if (PCl.ErrorStatus == ERR_warning)
                            fC = Color.Black;
                        AppendColoredText(rtbLocalHostsBT, sOut, fC);
                    }
                    else
                    {
                        fC = Color.Red;
                        AppendColoredText(rtbLocalHostsBT, "PC " + PCname + " Proj: " + ShortName + " has  no app_config\r\n", fC);
                    }
                    await Task.Delay(500);
                }
            }
        }
    }

}
