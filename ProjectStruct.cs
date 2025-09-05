using CreditStatistics;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;

//wcg was originally ?deviceid=

namespace CreditStatistics
{
    internal class cProjectStruct
    {
        const int NUM_ENTRIES = 10; // number of entries in the KnownProjects array
        public List<cPSlist> ProjectList = new List<cPSlist>();
        public cAllProjectStudyInfo ProjectStudyDB = new cAllProjectStudyInfo();
        public cManagedPCs ManagedPCs = new cManagedPCs();
        public cPClimit TempletDB; // boincgames sprint pandora_config file with extras
        public string sErrMsg;
        public List<cPClimit> PandoraDatabase; // the list of projects in the pandora_config file
        public PCresources pcResources = new PCresources();
        public string current_default_pandora_config;
        public string current_default_database_config; // original boincgames sprint pandora_config file
        public string current_default_cc_config;
        public SSHCredentials sshCredentials = new SSHCredentials();
        public string[] SavedHostList;

        public string UTC_Start = "";
        public string UTC_End = "";
        public DateTimeOffset UTCstart;   // local time is UTCstart.ToLocalTime()
        public DateTimeOffset UTCend;        
        public string DateLastPortScan = "not fully scanned";
        public string boinc_passwd = Properties.Settings.Default.BoincWebPassword;
        public string boinc_email = Properties.Settings.Default.BoincWebUsername;


        public static readonly string[] KnownProjects = @"
sidock
https://www.sidock.si/sidock/results.php?
hostid=
&state=4
&appid=
5
5
&offset=
0
 Valid () .

denis
https://denis.usj.es/denisathome/results.php?
hostid=
&state=4
&appid=
17
17
&offset=
0
 Valid () .

gpugrid
https://gpugrid.net/gpugrid/results.php?
hostid=
&state=4
&appid=
41
32 41
&offset=
0
 Valid () .

rosetta
https://boinc.bakerlab.org/rosetta/results.php?
hostid=
&state=4
&appid=
3
3
&offset=
0
 Valid () .

asteroids
https://asteroidsathome.net/boinc/results.php?
hostid=
&state=4
&appid=
0
0
&offset=
0
 Valid () .

milkyway
https://milkyway.cs.rpi.edu/milkyway/results.php?
hostid=
&state=4
&appid=
4
4
&offset=
0
 Valid () .

einstein
https://einsteinathome.org
/host/
/tasks/4
/
57
0 25 40 56 57 58 60
?page=
0
>Valid ()</span>.

lhc
https://lhcathome.cern.ch/lhcathome/results.php?
hostid=
&state=4
&appid=
13
1 11 13 14
&offset=
0
 Valid () .

amicable
https://sech.me/boinc/Amicable/results.php?
hostid=
&state=4
&appid=
0
0
&offset=
0
 Valid () .

numberfields
https://numberfields.asu.edu/NumberFields/results.php?
hostid=
&state=4
&appid=
0
0
&offset=
0
 Valid () .

odlk progger
https://boinc.progger.info/odlk/results.php?
hostid=
&state=4
&appid=
3
1 3
&offset=
0
 Valid () .

odlk1 latinsquares
https://boinc.multi-pool.info/latinsquares/results.php?
hostid=
&state=4
&appid=
1
1
&offset=
0
 Valid () .

moowrap moo
https://moowrap.net/results.php?
hostid=
&state=4
&appid=
0
0
&offset=
0
 Valid () .

nfs escatter
https://escatter11.fullerton.edu/nfs/results.php?
hostid=
&state=4
&appid=
11
11
&offset=
0
 Valid () .

PrimeGrid
https://www.primegrid.com/results.php?
hostid=
&state=4
&appid=
8
8
&offset=
0
prime

gerasim
https://gerasim.boinc.ru/users/viewHostResults.aspx?
hostid=
&opt=2
&appid=
0
0
null
0
>Valid 0 </a>.

srbase
https://srbase.my-firewall.org/sr5/results.php?
hostid=
&state=4
&appid=
20
20
&offset=
0
 Valid () .

rakesearch rake
https://rake.boincfast.ru/rakesearch/results.php?
hostid=
&state=4
&appid=
8
8
&offset=
0
 Valid () .

rnma ramanujanmachine
https://rnma.xyz/boinc/results.php?
hostid=
&state=4
&appid=
6
6
&offset=
0
 Valid () .

loda
https://boinc.loda-lang.org/loda/results.php?
hostid=
&state=4
&appid=
0
0
&offset=
0
 Valid () .

yafu
https://yafu.myfirewall.org/yafu/results.php?
hostid=
&state=4
&appid=
15
15
&offset=
0
 Valid () .

cpdn ClimatePrediction
https://main.cpdn.org/results.php?
hostid=
&state=4
&appid=
0
0
&offset=
0
 Valid () .

yoyo
https://www.rechenkraft.net/yoyo/results.php?
hostid=
null
null
5
5 61
&offset=
0
yoyo

WCG WORLDCOMMUNITYGRID community
https://www.worldcommunitygrid.org/contribution
device?id=
&type=B
&projectId=
124
74 92 94 124
null
0
wcg

radioactive
http://radioactiveathome.org/boinc/results.php?
hostid=
&state=3
null
null
null
&offset=
0
null

mine minecrafthome
https://minecraftathome.com/minecrafthome/results.php?
hostid=
&state=4
&appid=
9
9
&offset=
0
 Valid () .

gfn proxyma
http://boincvm.proxyma.ru:30080/test4vm/results.php?
hostid=
&state=4
&appid=
15
5 8 15
&offset=
0
<b>Valid</b> 0 .

gene tn-grid
https://gene.disi.unitn.it/test/results.php?
hostid=
&state=4
null
null
null
&offset=
0
null

wuprop
https://wuprop.boinc-af.org/results.php?
hostid=
&state=4
&appid=
9
9 43 30 4 14
&offset=
0
 Valid () .

central
https://boinc.berkeley.edu/central/results.php?
hostid=
&state=4
&appid=
2
1 2
&offset=
0
 Valid () .

rnaworld rna
https://www.rnaworld.de/rnaworld/results.php?
hostid=
&state=4
&appid=
2
2 3 7 8 15 16
&offset=
0
 Valid () .

"
.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        private static readonly string DefaultPandoraConfig = @"
earliest_deadline_first
debug
bunker_strategy: 5
message_filters:has reached a limit,your preferences are set,No tasks sent,No work sent,see scheduler log,#

project: https://minecraftathome.com/minecrafthome/
block_reports: 1000

project: https://numberfields.asu.edu/NumberFields/
block_reports: 1000

project: http://boincvm.proxyma.ru:30080/test4vm/
block_reports: 1000

project: https://srbase.my-firewall.org/sr5/
block_reports: 1000

project: https://sech.me/boinc/Amicable/
block_reports: 1000

project: https://einstein.phys.uwm.edu/
block_reports: 1000

project: https://milkyway.cs.rpi.edu/milkyway/
block_reports: 1000

project: https://escatter11.fullerton.edu/nfs/
block_reports: 1000
#
";

        private static readonly string DefaultDatabaseConfig = @"
earliest_deadline_first
debug
bunker_strategy: 5
#bunker_release: 08/27/2025T14:00:00
#bunker_end: +3
message_filters:has reached a limit,your preferences are set,No tasks sent,No work sent,see scheduler log,#
#concurrent_gpu_tasks: 1
#concurrent_cpu_tasks: 1

project: https://minecraftathome.com/minecrafthome/
#block_reports: 1000
#app_type: gpu
#study: 9
#used_in_sprint: 1
#bunker_start: -3

project: https://numberfields.asu.edu/NumberFields/
#block_reports: 1000
#app_type: gpu
#study: 0
#used_in_sprint: 1
#bunker_start: -7
#bunker_end: -3

project: http://boincvm.proxyma.ru:30080/test4vm/
#block_reports: 1000
#app_type: gpu
#study: 15
#used_in_sprint: 1
#bunker_start: -3

project: https://srbase.my-firewall.org/sr5/
#block_reports: 1000
#app_type: gpu
#study: 20
#used_in_sprint: 1
#bunker_start: -5
#bunker_end: -3

project: https://sech.me/boinc/Amicable/
#block_reports: 1000
#app_type: gpu
#study: 0
#used_in_sprint: 1
#bunker_start: -7
#bunker_end: -3

project: https://einstein.phys.uwm.edu/
#block_reports: 1000
#app_type: gpu
#study: 57
#used_in_sprint: 1
#bunker_start: -7
#bunker_end: -5

project: https://milkyway.cs.rpi.edu/milkyway/
#block_reports: 1000
#app_type: cpu
#bunker_start: -12
#study: 4
#used_in_sprint: 1

project: https://escatter11.fullerton.edu/nfs/
#block_reports: 1000
#app_type: cpu
#bunker_start: -15
#study: 9
#used_in_sprint: 1
#
";


        public static readonly string DefaultCCconfig = @"
<cc_config>
 <log_flags>
  <task>0</task>
  <work_fetch_debug>0</work_fetch_debug>
  <file_xfer>0</file_xfer>
  <sched_ops>0</sched_ops>
 </log_flags>
 <options>
  <use_all_gpus>1</use_all_gpus>
  <allow_remote_gui_rpc>1</allow_remote_gui_rpc>
 </options>
</cc_config>
";

        public static readonly string DefaultAPPconfig = @"
<app_config>
<!--This is a default app_config and is NOT on the remote pc
you can edit it and then send it if desired
    <app_version>
        <app_name>whatever</app_name>
        <plan_class>whatever</plan_class>
        <cmdline>whatever</cmdline>
        <avg_ncpus>1.0</avg_ncpus>
        <ngpus>1.0</ngpus>
    </app_version>
-->
<project_max_concurrent>2</project_max_concurrent>
    <report_results_immediately>0</report_results_immediately>
</app_config>
";

        public void Init()
        {
            int i, j, n = KnownProjects.Length;
            cProjectStruct localProjectStats = this;
            sshCredentials.Init();
            string where_ID_Study_config = globals.WhereDOC + "\\" + name_ID_Study_config;
            globals.WhereDefaultHostList = globals.WhereDOC + "\\" + WhereDefaultHostList;
            globals.WhereMasterPandora = globals.WhereDOC + "\\" + WhereMasterPandora;
            globals.WhereCurrentDefaultCC_config = globals.WhereDOC + "\\" + WhereCurrentDefaultCC_config;
            globals.WhereCurrentDefaultDatabaseConfig = globals.WhereDOC + "\\" + WhereCurrentDefaultDatabaseConfig;
            globals.WhereAppVersions = globals.WhereDOC + "\\" + WhereAppVersions;
            globals.WhereCurrentDefaultPandoraConfig = globals.WhereDOC + "\\" + WhereCurrentDefaultPandoraConfig;
            globals.WhereProjectAccessParams = globals.WhereDOC + "\\" + WhereProjectAccessParams;

            if (!File.Exists(WhereCurrentDefaultPandoraConfig))
                File.WriteAllText(WhereCurrentDefaultPandoraConfig, DefaultPandoraConfig.Trim());
            current_default_pandora_config = File.ReadAllText(WhereCurrentDefaultPandoraConfig).Trim();
            //Properties.Settings.Default.CurrentDefaultPandoraConfig = current_default_pandora_config;

            if (!File.Exists(WhereCurrentDefaultDatabaseConfig))
                globals.WriteDefaultDatabaseConfig(DefaultDatabaseConfig);
            current_default_database_config = File.ReadAllText(WhereCurrentDefaultDatabaseConfig.Trim());

            i = current_default_database_config.IndexOf("#bunker_release:");
            if (i < 0) UTC_Start = "08/27/2025T14:00:00";
            else
            {
                j = current_default_database_config.IndexOf(Environment.NewLine, i +16);
                Debug.Assert(j > 0, "bad bunker start time in templet");
                UTC_Start = current_default_database_config.Substring(i+16,j - i - 16).Trim();
            }
            UTCstart = UTCtoLOCAL_OFFSET(UTC_Start);

            i = current_default_database_config.IndexOf("#bunker_end:");
            if (i < 0) UTC_Start = "08/30/2025T14:00:00";
            else
            {
                j = current_default_database_config.IndexOf(Environment.NewLine, i + 12);
                Debug.Assert(j > 0, "bad bunker start time in templet");
                string sDays = current_default_database_config.Substring(i + 12, j - i - 12).Trim();
                double days;
                if (!double.TryParse(sDays, NumberStyles.Float, CultureInfo.InvariantCulture, out days))
                {
                    days = 3.0; // default value if parsing fails
                }
                UTCend = UTCstart.AddDays(days);
                UTC_End = GetUTCstring(UTCend);
            }


            if (!File.Exists(WhereCurrentDefaultCC_config))
                File.WriteAllText(WhereCurrentDefaultCC_config, DefaultCCconfig.Trim());
            current_default_cc_config = File.ReadAllText(WhereCurrentDefaultCC_config.Trim());
            //Properties.Settings.Default.CurrentDefaultCC_config = current_default_cc_config;

            Properties.Settings.Default.Save();

            pcResources.LoadPCresources();

            List<string> UnsortedNames = new List<string>();

            for (j = 0; j < n; j += NUM_ENTRIES)
            {
                UnsortedNames.Add(KnownProjects[j].ToLower());
            }
            n = UnsortedNames.Count;
            List<int> indices = Enumerable.Range(0, n).ToList();
            indices.Sort((i1, i2) => UnsortedNames[i1].CompareTo(UnsortedNames[i2]));
            for (j = 0; j < n; j++)
            {
                i = indices[j] * NUM_ENTRIES;
                ProjectList.Add(new cPSlist()
                {
                    IsSelected = false,
                    UsedInSprint = false,
                    MasterUrl = "",
                    shortname = "",
                    name = KnownProjects[i++].ToLower(),
                    sURL = KnownProjects[i++],
                    sHid = KnownProjects[i++],
                    sValid = KnownProjects[i++],
                    sStudy = KnownProjects[i++],
                    sStudyV = KnownProjects[i++],
                    sStudyL = KnownProjects[i++],
                    sPage = KnownProjects[i++],
                    sPageV = KnownProjects[i++],
                    sCountValids = KnownProjects[i]
                });
            }
            List<string>NoResponse= new List<string>();
            NoResponse.Add("No Response");
            for (i = 0; i < n; i++)
            {
                ProjectList[i].shortname = ShortName(i);
                ProjectList[i].AppConfigVersions = NoResponse;
            }

            if (File.Exists(globals.WhereAppVersions))
            {
                string[] sSAppVersions = File.ReadAllLines(globals.WhereAppVersions);
                foreach (string s in sSAppVersions)
                {
                    string[] lines = s.Split(':');
                    string shortname = lines[0];
                    cPSlist psi = GetProjectByShortName(shortname);
                    string[] sVers = lines[1].Split(',');
                    psi.AppConfigVersions = new List<string>();
                    foreach (string t in sVers)
                    {
                        psi.AppConfigVersions.Add(t.Replace("'", "").Trim());
                    }
                }
            }


            PandoraConfig pc = new PandoraConfig(ref localProjectStats);
            TempletDB = pc.ParsePandoraConfig(current_default_database_config, "templet");
            foreach (cCalcLimitProj lp in TempletDB.ProjList)
            {
                cPSlist PSl = GetProjectByShortName(lp.ShortName);
                PSl.sStudyV = lp.sStudy;
                SetSprintUsage(lp.ShortName, true);
            }
            pc = null;
            ProjectStudyDB.init(ref localProjectStats, where_ID_Study_config);
        }

        public void AddCpuGpu()
        {
            foreach (PCresources.cCpuGpu cg in pcResources.CpuGpu)
                ManagedPCs.AddPC(cg.sPC, cg.nCPU, cg.nGPU, cg.OSname, cg.IPaddress);
        }

        public void SetSprintUsage(string shortName, bool b)
        {
            int iLoc = ShortnameToIndex(shortName);
            ProjectList[iLoc].UsedInSprint = b;
        }

        public void AddProjInfoToSprintTemplet(ref cCalcLimitProj clp)
        {
            int iLoc = ShortnameToIndex(clp.ShortName);
            string MasterUrl = ProjectList[iLoc].MasterUrl;
            clp.init(MasterUrl, false);
            clp.sStudy = ProjectList[iLoc].sStudyV;
        }

        public string NameToShortname(string s)
        {
            for (int i = 0; i < ProjectList.Count; i++)
            {
                string[] names = ProjectList[i].name.Split(' ');
                foreach (string name in names)
                {
                    if (s.Contains(name))
                    {
                        return names[0];
                    }
                }
            }
            return "";
        }

        public int ShortnameToIndex(string sn)
        {
            for (int i = 0; i < ProjectList.Count; i++)
            {
                string[] names = ProjectList[i].name.Split(' ');
                if (names[0].Equals(sn, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            Debug.Assert(false, "Shortname not found in ProjectList: " + sn);
            return -1; // Not found
        }

        public cPSlist GetProjectByShortName(string shortName)
        {
            int iLoc = ShortnameToIndex(shortName);
            if (iLoc < 0)
            {
                Debug.Assert(false, "Shortname not found in ProjectList: " + shortName);
                return null; // Not found
            }
            return ProjectList[iLoc];
        }

        public string ShortName(int i)
        {
            string[] s = ProjectList[i].name.Split(' ');
            return s[0];
        }

        public int GetNameIndex(string tUrl)
        {
            int i = 0;
            string s = tUrl.ToLower();
            string[] sTemp;
            foreach (cPSlist c in ProjectList)
            {
                sTemp = c.name.Split(' ');
                foreach (string s1 in sTemp)
                {
                    if (s.Contains(s1))
                    {
                        return i;
                    }
                }
                i++;
            }
            return -1;
        }

        /*
        public bool GetHosts(string sBoincInfo)
        {
            string sPCname;
            string ProjHost;
            string sProj;
            string sHost;
            bool b;
            int j;
            List<cBoincRaw> LocalHostsRaw = new List<cBoincRaw>();
            cBoincRaw br;
            cOldraw cOl = new cOldraw();
            string UnknownProjects = "";
            sBoincInfo = sBoincInfo.Replace("@home", "");
            string[] content = sBoincInfo.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            while (true)
            {
                if (i >= content.Length)
                {
                    break;
                }

                do
                {
                    sPCname = content[i].Trim();
                    i++;
                    if (sPCname.Contains(",") || sPCname == "")
                    {
                        MessageBox.Show("expected hostname found: " + sPCname);
                        b = true;
                    }
                    else b = false;
                } while (b);

                br = new cBoincRaw();
                if (sPCname == "localhost")
                    sPCname = Environment.MachineName.ToLower(); // replace localhost with the actual machine name
                br.PCname = sPCname;
                LocalHostsRaw.Add(br);

                do
                {
                    if (i >= content.Length)
                    {
                        break;  // todo this is an error as there should be data for this PC
                    }
                    ProjHost = content[i].Trim();
                    i++;
                    j = ProjHost.IndexOf(",");
                    if (j < 0)
                    {
                        i--;
                        break;
                    }
                    else
                    {
                        if (j >= (ProjHost.Length - 1))
                        {
                            break;
                        }
                        sProj = ProjHost.Substring(0, j).Trim();
                        sHost = ProjHost.Substring(j + 1).Trim(); // project id for the host pc
                        LocalHostsRaw.Last().Proj.Add(sProj.ToLower());
                        LocalHostsRaw.Last().HostID.Add(sHost);
                    }
                    b = (i < content.Length);
                } while (b);
            }

            foreach (cBoincRaw abr in LocalHostsRaw)
            {
                int k = 0;
                for (i = 0; i < abr.Proj.Count; i++)
                {
                    if (ProjectExists(abr.Proj[i]))
                    {
                        cOl.AddTriple(abr.PCname, abr.Proj[i], abr.HostID[i]);
                        string sFullProjName = abr.Proj[i];
                        string shortname = NameToShortname(sFullProjName);
                        int l = ShortnameToIndex(shortname);
                        ManagedPCs.AddProj(abr.PCname, shortname, abr.HostID[i], l);
                    }
                    else
                    {
                        string u = abr.Proj[i];
                        if (!UnknownProjects.Contains(u))
                            UnknownProjects += u + " ";
                        k++;
                        if (k == 4)
                        {
                            k = 0;
                            UnknownProjects += Environment.NewLine;
                        }
                    }
                }
            }
            ManagedPCs.SaveManagedPCs();
            return true;
        }

        */

        public void WriteHostList(ref string[] sTemp)
        {
            File.WriteAllLines(globals.WhereDefaultHostList, sTemp);
        }

        public string[] ReadHostList()
        {
            if (File.Exists(globals.WhereDefaultHostList))
                return File.ReadAllLines(globals.WhereDefaultHostList);
            else return null;
        }

        public void AddLocalHostsProjects()
        {
            if (PathTo_boinccmd_exe == "")
            {
                MessageBox.Show("BOINC not found. Please check your BOINC installation.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            Process[] pname = Process.GetProcessesByName("boinc");
            bool bBoincExecuting = (pname.Length > 0);
            if (!bBoincExecuting)
            {
                MessageBox.Show("BOINC is not running. Please start BOINC before using this tool.", "BOINC Not Running", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }

            cProjectStruct localProjectStats = this;
            RemoteSystems rs = new RemoteSystems(ref localProjectStats);

            Task aTask = Task.Run(async () =>
            {
                await rs.CreateLocalHost(Dns.GetHostName().ToLower(), "127.0.0.1");
                await ManagedPCs.GetRemotePCinfo();
            });
            aTask.Wait();
            rs.Dispose();
        }

        //no longer using this format: project name, pc name and pc id "computer id"

        public bool GetHostSet()
        {
            int i;
            List<string> Proj_PC_ID = new List<string>();   // pc name: projectname and projects ID for the computer
            if (SavedHostList == null || SavedHostList.Length == 0)
            {
                return false;
            }
            Proj_PC_ID.AddRange(SavedHostList);
            if (Proj_PC_ID.Count == 0) return false;
            return FormHostList(ref Proj_PC_ID);
        }


        public void GetAllProjectIDs()
        {
            int iLoc = 0;
            foreach (cHostInfo chl in ManagedPCs.LocalSystems)
            {
                string sPCname = chl.ComputerID;
                foreach (cLHe ce in chl.LocalProjID)
                {
                    string sIDname = ce.ProjectsID;
                    string sPJname = ce.ProjectName;
                    iLoc = ShortnameToIndex(sPJname);
                    ProjectList[iLoc].HostPCNames.Add(sPCname);
                    ProjectList[iLoc].HostProjIDs.Add(sIDname);
                }
            }
        }

        public bool FormHostList(ref List<string> Proj_PC_ID)
        {
            if (Proj_PC_ID.Count == 0) return false;
            int i, n = 0;
            foreach (string s in Proj_PC_ID)
            {
                i = s.IndexOf(":");
                if (i < 0)
                {
                    //MessageBox.Show("Expected format of pcname: projname projID .. not found!");
                    // treat as project with no IDs
                    continue;
                }
                string PCname = s.Substring(0, i).Trim();
                if (i >= s.Length) continue;
                bool HasAC = false;
                string sRem = s.Substring(i + 1).Trim();
                string[] sPairHosts = sRem.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < sPairHosts.Length; j++)
                {
                    string[] sPair = sPairHosts[j].Split(' ');
                    string shortName = sPair[0].Trim();
                    int iLoc = GetNameIndex(shortName);
                    if (iLoc < 0)
                    {
                        MessageBox.Show("Unknown project found: ", shortName);
                        continue;
                    }
                    n++;
                    string ProjID = sPair[1].Trim();
                    string sStudy = ProjectList[iLoc].sStudyV;
                    if (sPair.Length >= 3)
                        sStudy = sPair[2].Trim();
                    if (sPair.Length == 4)
                        HasAC = (sPair[3] == "1");
                    ManagedPCs.AddProj(PCname, shortName, ProjID, iLoc, sStudy, HasAC);
                }
            }
            return n > 0;
        }

        public bool IsProjInSprint(string shortname)
        {
            cCalcLimitProj clp = TempletDB.GetProjStruct(shortname);
            if (clp == null) return false;
            return clp.UsedInSprint;
        }

        public bool IsProjInGames(string shortname)
        {
            cCalcLimitProj clp = TempletDB.GetProjStruct(shortname);
            if (clp == null) return false;
            return clp.UsedInGames;
        }
    }
}
