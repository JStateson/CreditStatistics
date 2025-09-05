using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Linq;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class ViewAppVersions : Form
    {

        cProjectStruct ProjectStats;
        cManagedPCs ManagedPCs;
        PandoraConfig pandoraConfig;
        PandoraRPC pandoraRPC = new PandoraRPC();
        List<string> RestoreAC = new List<string>();
        string NL = Environment.NewLine;
        bool IsLoading = false;


        public ViewAppVersions(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            ManagedPCs = ProjectStats.ManagedPCs;
            pandoraConfig = new PandoraConfig(ref ProjectStats);
            pandoraRPC.Init(ref ProjectStats);
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
            FormDGV();
        }

        private void FormDGV()
        {
            IsLoading = true;
            foreach (cPSlist psi in ProjectStats.ProjectList)
            {
                string PJname = psi.shortname;
                bool HasApp = psi.AppConfigVersions != null;

                // Add a new row
                int rowIndex = dgvPJs.Rows.Add();
                DataGridViewRow row = dgvPJs.Rows[rowIndex];

                // Set text values
                row.Cells["ProjName"].Value = PJname;
                row.Cells["HasApp"].Value = HasApp;
                row.Cells["sCnt"].Value = HasApp ? psi.AppConfigVersions.Count.ToString() : "0";

                // If this PC has versions, bind them to the ComboBox cell
                if (HasApp && psi.AppConfigVersions.Count > 0)
                {

                    var comboCell = (DataGridViewComboBoxCell)row.Cells["AppVersion"];

                    comboCell.DataSource = psi.AppConfigVersions;
                    comboCell.Value = psi.AppConfigVersions.First();
                }
            }

            dgvPJs.Columns[0].ReadOnly = true;
            dgvPJs.Columns[1].ReadOnly = true;
            dgvPJs.Columns[2].ReadOnly = true;
            dgvPJs.Columns["AppVersion"].ReadOnly = false;  // unlock just this column

            IsLoading = false;
        }




        private async void SaveLocalAppConfigs()
        {
            string PCname = Dns.GetHostName().ToLower();
            cPClimit PCl = pandoraConfig.NameToSprintPC(PCname);
            ManagedPCs.SelectCurrentFromPC(PCname);
            Color fC = Color.Blue;
            AppendColoredText(rtbLocalHostsBT, "Backing up app_configs from localhost\r\n", fC);
            AppendColoredText(rtbLocalHostsBT, "This color is for custom app config\r\n", fC);
            AppendColoredText(rtbLocalHostsBT, "This color is for any default app config\r\n", Color.Black);
            AppendColoredText(rtbLocalHostsBT, "This color shows up for non-existant app configs\r\n", Color.Red);
            foreach (string ShortName in ManagedPCs.MatchingShortnames)
            {
                string ACpathname = GetAppConfigFilename(PCname, ShortName);
                await pandoraRPC.FetchOne_app_config(PCname, ShortName);
                if (PCl.ErrorStatus < ERR_critical)
                {
                    string[] ssAppConfig = PCl.strResult.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    string sOut = globals.WriteACrecord(ACpathname, ref ssAppConfig);
                    fC = Color.Blue;
                    if (PCl.ErrorStatus == ERR_warning)
                        fC = Color.Black;
                    AppendColoredText(rtbLocalHostsBT, sOut, fC);
                    RestoreAC.Add(ACpathname);
                }
                else
                {
                    fC = Color.Red;
                    AppendColoredText(rtbLocalHostsBT, "PC " + PCname + " Proj: " + ShortName + " has  no app_config\r\n", fC);
                }
                await Task.Delay(500);
                Application.DoEvents();
            }
            fC = Color.Blue;
            AppendColoredText(rtbLocalHostsBT, "Finished all backup of localhost\r\n", fC);
            AppendColoredText(rtbLocalHostsBT, "You can proceed with obtaining versions\r\n", fC);
        }

        private int LoadRestoreFrom(string PCname)
        {
            string filePath = globals.WhereDOC;
            string searchPattern = "app_config_" + PCname + "_" + "*.xml";
            string[] files = Directory.GetFiles(filePath, searchPattern);
            RestoreAC.Clear();
            foreach (string file in files)
            {
                RestoreAC.Add(file);
            }
            return RestoreAC.Count;
        }

        private async void RestoreLocalAppConfigs()
        {
            string PCname = Dns.GetHostName().ToLower();
            cPClimit PCl = pandoraConfig.NameToSprintPC(PCname);
            string sPCfind = "app_config_" + PCname + "_";
            rtbLocalHostsBT.Clear();
            AppendColoredText(rtbLocalHostsBT, "Restoring all backups of localhost\r\n", Color.Blue);
            if (RestoreAC.Count == 0)
            {
                LoadRestoreFrom(Dns.GetHostName().ToLower());
            }
            foreach (string s in RestoreAC)
            {
                int i = s.IndexOf(sPCfind);
                Debug.Assert(i >= 0, "internal error, cannot find project name");
                string t = s.Substring(i + sPCfind.Length);
                i = t.IndexOf(".xml");
                string ShortName = t.Substring(0, i);
                string sOut = File.ReadAllText(s);
                pandoraRPC.TextToUnix(sOut);
                await pandoraRPC.WriteOne_app_config(PCname, ShortName);
                AppendColoredText(rtbLocalHostsBT, s + NL, Color.Blue);
                await Task.Delay(500);
                Application.DoEvents();
            }
            ManagedPCs.RestartBoinc(PCname);
        }


        private string DummyAppConfig = @"
<app_config>
    <app_version>
        <app_name>AppVersion</app_name>
        <plan_class>whatever</plan_class>
        <cmdline>whatever</cmdline>
        <avg_ncpus>1.0</avg_ncpus>
        <ngpus>1.0</ngpus>
    </app_version>
    <project_max_concurrent>2</project_max_concurrent>
    <report_results_immediately>0</report_results_immediately>
</app_config>
";

        private async void btnFindAll_Click(object sender, EventArgs e)
        {
            string PCname = Dns.GetHostName().ToLower();
            string sResult = ManagedPCs.ContactPCproject(PCname, " --get_messages", "");
            if (ManagedPCs.ErrorStatus != 0) return;
            ParseForVersions(ref sResult);
        }


        /*
AsusX299

65	SRBase	9/3/2025 1:56:22 PM	Your app_config.xml file refers to an unknown application 'whatever'. 
        Known applications: 'TF', 'srbase8', 'srbase2', 'srbase10', 'srbase5', 'srbase6', 'srbase9', 'srbase11'	
         */
        private string GetShortName(string s)
        {
            int i = s.IndexOf("[");
            Debug.Assert(i >= 0);
            int j = s.IndexOf("]");
            Debug.Assert(j >= 0);
            string name = (s.Substring(i + 1, j - i - 1)).ToLower();
            name = name.Replace("@home", "");
            string ShortName = ProjectStats.NameToShortname(name);
            Debug.Assert(ShortName != "");
            return ShortName;
        }

        private void ParseForVersions(ref string sResult)
        {
            string[] lines = sResult.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string sAppVer = "";
            string[] sSAppVer = null;
            string sOut = "";
            foreach (string line in lines)
            {
                int i = line.IndexOf("Known applications:");
                if (i >= 0)
                {
                    string ShortName = GetShortName(line);
                    AppendColoredText(rtbLocalHostsBT, ShortName + " has versions: ", Color.Black);
                    cPSlist psi = ProjectStats.GetProjectByShortName(ShortName);
                    psi.AppConfigVersions.Clear();
                    string sTgt = line.Substring(i + 19).Trim();
                    if (sTgt.Contains('\''))
                    {
                        AppendColoredText(rtbLocalHostsBT, sTgt + NL, Color.Blue);
                        sSAppVer = sTgt.Split(',');
                        foreach (string s in sSAppVer)
                        {
                            psi.AppConfigVersions.Add(s.Replace("'", ""));
                        }
                    }
                    else
                    {
                        sAppVer = sTgt.ToLower();
                        AppendColoredText(rtbLocalHostsBT, sAppVer + NL, Color.Red);
                        Debug.Assert(sAppVer == "none");
                    }
                    sOut += ShortName + ":" + sTgt + NL;
                }
            }
            File.WriteAllText(globals.WhereAppVersions, sOut);
        }

        private async void btnBackup_Click(object sender, EventArgs e)
        {
            SaveLocalAppConfigs();
        }

        private async void btnMakeDummy_Click(object sender, EventArgs e)
        {
            string PCname = Dns.GetHostName().ToLower();
            ManagedPCs.SelectCurrentFromPC(PCname);
            rtbLocalHostsBT.Clear();
            AppendColoredText(rtbLocalHostsBT, "Creating dummy AppConfigs\r\n", Color.Blue);

            foreach (string ProjName in ManagedPCs.MatchingShortnames)
            {
                string app_name = "app_config_" + PCname + "_" + ProjName + ".xml";
                string pathName = Path.Join(globals.WhereDOC, app_name);
                if (File.Exists(pathName))
                {
                    AppendColoredText(rtbLocalHostsBT, "skipping: " + pathName + NL, Color.Black);
                    continue;
                }
                AppendColoredText(rtbLocalHostsBT, pathName + NL, Color.Black);
                pandoraRPC.TextToUnix(DummyAppConfig);
                await pandoraRPC.WriteOne_app_config(PCname, ProjName);
                await Task.Delay(500);
                Application.DoEvents();
            }
            AppendColoredText(rtbLocalHostsBT, "Restarting BOINC\r\n", Color.Black);
            ManagedPCs.RestartBoinc(PCname);
            for (int i = 20; i >= 0; i--)
            {
                await Task.Delay(1000);
                Application.DoEvents();
                AppendColoredText(rtbLocalHostsBT, i.ToString() + NL, Color.Red);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            RestoreLocalAppConfigs();
        }

        private void dgvPJs_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvPJs.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                dgvPJs.BeginEdit(true);
                ComboBox combo = dgvPJs.EditingControl as ComboBox;
                if (combo != null)
                {
                    combo.DroppedDown = true; // open dropdown immediately
                }
            }
        }

        private void dgvPJs_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (IsLoading) return;
            if (e.RowIndex >= 0 && dgvPJs.Columns[e.ColumnIndex].Name == "AppVersion")
            {
                var selectedValue = dgvPJs.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                string sVer = selectedValue.ToString();
                string sTemp = DummyAppConfig;
                sTemp = sTemp.Replace("AppVersion", sVer);
                rtbLocalHostsBT.Text = sTemp;
                //                AppendColoredText(rtbLocalHostsBT, sTemp, Color.Black);
            }
        }

        private void dgvPJs_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPJs.IsCurrentCellDirty)
            {
                dgvPJs.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvPJs_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dgvPJs_SelectionChanged(object sender, EventArgs e)
        {
            if (IsLoading) return;
            if (dgvPJs.CurrentRow != null)
            {
                int rowIndex = dgvPJs.CurrentRow.Index;
                int colIndex = dgvPJs.Columns["AppVersion"].Index;
                string sVer = dgvPJs.Rows[rowIndex].Cells[colIndex].Value.ToString();
                string sTemp = DummyAppConfig;
                sTemp = sTemp.Replace("AppVersion", sVer);
                rtbLocalHostsBT.Text = sTemp;
            }
        }
    }
}
