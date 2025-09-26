using CreditStatistics;
using Microsoft.Playwright;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;


/*
 * easy way to fill a listbox or any type of widget with scattered data.
 * datasource = ManagedPCs.LocalSystems.Select(h => h.ComputerID).ToList();
*/
namespace CreditStatistics
{
    public partial class CreditStatistics : Form
    {
        internal cProjectStruct ProjectStats = new cProjectStruct();
        private cAllProjectStudyInfo ProjectStudyDB;
        private cManagedPCs ManagedPCs;
        private int TagOfProject = 0; // the index of the selected project and matches the study db

        public CreditStatistics(string[] args)
        {
            InitializeComponent();
            globals.FormDataPaths();
            string BoincTaskRemotes = Path.Combine(WhereBoincTaskFolder, "computers.xml");
            findOtherPCsToolStripMenuItem.Enabled = File.Exists(BoincTaskRemotes);
#if DEBUG
            getAllConfigFilesToolStripMenuItem.Enabled = true;
            btnRun.Visible = true;
            btnPaste.Visible = true;
#else
            getAllConfigFilesToolStripMenuItem.Enabled = false;
            btnRun.Visible = false;
            btnPaste.Visible =false;
#endif

            if (args.Length > 0)
            {
                string s = args[0].ToLower();
                if (s == "reset")// && false)
                {
                    Properties.Settings.Default.Reset();
                }
            }
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            ProjectStats.Init();
            FormProjectRB();
            ProjectStudyDB = ProjectStats.ProjectStudyDB;
            ManagedPCs = ProjectStats.ManagedPCs;
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
            tbInfo.Text += ManagedPCs.Init(ref ProjectStats);
            ProjectStats.SavedHostList = ProjectStats.ReadHostList();

            if (ProjectStats.SavedHostList == null)  // there is no list of IDs so we need to create it
            {
                ProjectStats.AddLocalHostsProjects();   // need to save them.
                ProjectStats.SavedHostList = ProjectStats.ManagedPCs.SaveManagedPCs();
            }
            ProjectStats.GetHostSet(); // this read the SavedHostList and fill in the managed PCs 
            ProjectStats.AddCpuGpu();

            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                ProjectStats.sshCredentials.GetCredentials(hi.ComputerID, out string uName, out string uPass);
                hi.UserName = uName;
                hi.Password = uPass;
            }
            ProjectStats.GetAllProjectIDs();

            cbPCavail.DataSource = null;
            cbPCavail.DataSource = ManagedPCs.LocalSystems.Select(h => h.ComputerID).ToList();
            UpdateRB();
            PandoraConfig pc = new PandoraConfig(ref ProjectStats);
            ProjectStats.PandoraDatabase = pc.GetConfigParams();
            pc.PandoraDatabase = ProjectStats.PandoraDatabase; // todo to do this is a backfit as the original assigment was a null on startup!!
            ManagedPCs.SelectCurrentFromPC("localhost");    // we will always have a localhost entry
            // update with the latest templet
            // set default study from assigned ones
            foreach (cPClimit pcl in pc.PandoraDatabase)
            {
                pcl.UTC_bunker_release = ProjectStats.TempletDB.UTC_bunker_release;
                foreach (cCalcLimitProj clp in pcl.ProjList)
                {
                    cCalcLimitProj Tclp = ProjectStats.TempletDB.GetProjStruct(clp.ShortName);
                    clp.BunkerEnd = Tclp.BunkerEnd;
                    clp.BunkerStart = Tclp.BunkerStart;
                    if (clp.AppType == "")
                        clp.AppType = ProjectStats.GetDefaultAppType(clp.ShortName);
                }
                cPClimit pclx = pcl;
                pc.WriteDBrecord(ref pclx);
            }
            PerformPortCheck();
        }

        private void FormProjectRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;

            for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
            {
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = ProjectStats.ShortName(i);
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.ForeColor = bHasID ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.CheckedChanged += new System.EventHandler(this.rbProject_CheckedChanged);
                gbSamURL.Controls.Add(rb);
                iRow++;
                if (iRow > 14)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }

        private void UpdateRB()
        {
            RadioButton rbSelected;
            foreach (Control c in gbSamURL.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    int i = (int)rb.Tag;
                    string shortName = ProjectStats.ShortName(i);
                    string ProjID = ManagedPCs.ProjectIDfromPCsShortname(shortName);
                    bool bHasID = ProjID != "";
                    //rb.ForeColor = bHasID ? System.Drawing.Color.Blue : System.Drawing.Color.Red;
                    rb.Enabled = bHasID;
                    if (rb.Checked)
                    {
                        btnGetData.Enabled = rb.Enabled;
                        cbStudyAvail.Enabled = rb.Enabled;
                    }
                }
            }
        }

        private void FormUrlFromChange()
        {
            cPSlist PSl = ProjectStats.ProjectList[TagOfProject];
            PSl.sPageV = tbPage.Text;
            string sProjname = tbProjID.Text;
            tbProjUrl.Text = PSl.FormURL(sProjname);
        }

        private void SetLastStudy(string shortname, string PCname)
        {
            if (PCname == "") return;
            string LastStudy = ManagedPCs.NameToSystem(PCname).GetLastStudy(shortname);
            if (LastStudy == "")
                cbStudyAvail.SelectedIndex = 0;
            else
            {
                int i = ProjectStudyDB.GetStudyIndex(LastStudy);
                if (i >= 0)
                    cbStudyAvail.SelectedIndex = i;
                else
                    cbStudyAvail.SelectedIndex = 0;
            }
        }


        private void rbProject_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (TagOfProject >= 0)
                ProjectStats.ProjectList[TagOfProject].IsSelected = false;
            if (!rb.Checked)
            {
                return;
            }
            btnGetData.Enabled = rb.Enabled;
            TagOfProject = (int)rb.Tag;
            ProjectStats.ProjectList[TagOfProject].IsSelected = true;

            cbStudyAvail.SelectedIndexChanged -= cbStudyAvail_SelectedIndexChanged;

            cbStudyAvail.DataSource = null;
            cbStudyAvail.Items.Clear();
            cbStudyAvail.Text = "";
            cbStudyAvail.SelectedIndex = -1;
            ProjectStudyDB.GetStudyUsingName(rb.Text);
            cbStudyAvail.DataSource = ProjectStudyDB.AvailableStudies;
            string shortname = rb.Text.ToString();
            tbProjID.Text = ManagedPCs.ProjectIDfromPCsShortname(shortname);
            string PCname = cbPCavail.Text;
            cbStudyAvail.SelectedIndexChanged += cbStudyAvail_SelectedIndexChanged;
            SetLastStudy(shortname, PCname);
            tbStudyID.Text = ProjectStudyDB.CurrentStudy;
            ProjectChanged();
        }

        private void ProjectChanged()
        {
            string Shortname = ProjectStats.ShortName(TagOfProject);
            tbProjID.Text = ManagedPCs.ProjectIDfromPCsShortname(Shortname);
            FormUrlFromChange();
        }

        private void getStudyInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetStudies getStudies = new GetStudies(ref ProjectStats);
            getStudies.ShowDialog();
            getStudies.Dispose();
        }

        private void cbPCavail_TextChanged(object sender, EventArgs e)
        {
            tbPCname.Text = cbPCavail.Text.Trim();
        }

        private void cbStudyAvail_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iLoc = cbStudyAvail.SelectedIndex;
            string[] sID = ProjectStudyDB.AvailableStudies[iLoc].Split(':');
            ProjectStudyDB.CurrentStudy = sID[0];
            ProjectStats.ProjectList[TagOfProject].sStudyV = sID[0];
            tbStudyID.Text = sID[0];
            string PCname = cbPCavail.Text;
            string shortname = ProjectStats.ShortName(TagOfProject);
            if (PCname != "")
                ManagedPCs.NameToSystem(PCname).UpdateLastStudy(shortname, tbStudyID.Text);
            FormUrlFromChange();
        }


        private void btnGetData_Click(object sender, EventArgs e)
        {
            ReadRequest rr = new ReadRequest();
            rr.UsePandoraDatabase = false;
            rr.bUseUrl = false;
            rr.GetHdr = true;
            rr.GetBody = true;
            rr.PCname = tbPCname.Text;
            cPSlist PSl = ProjectStats.ProjectList[TagOfProject];
            rr.shortname = ProjectStats.ShortName(TagOfProject);
            rr.sStudyV = PSl.sStudyV;
            rr.sValid = PSl.sValid;
            rr.sHostID = tbProjID.Text;
            rr.UrlWanted = PSl.FormURL(rr.sHostID);
            ShowData showData = new ShowData(ref ProjectStats, rr);
            showData.ShowDialog();
            showData.Dispose();
        }

        private async void xbtnGetData_Click(object sender, EventArgs e)
        {
            ReadRequest rr = new ReadRequest();
            rr.UsePandoraDatabase = false;
            rr.bUseUrl = false;
            rr.GetHdr = true;
            rr.GetBody = true;
            rr.PCname = tbPCname.Text;
            cPSlist PSl = ProjectStats.ProjectList[TagOfProject];
            rr.shortname = ProjectStats.ShortName(TagOfProject);
            rr.sStudyV = PSl.sStudyV;
            rr.StudyName = ProjectStudyDB.GetNameOfStudy(PSl.sStudyV);
            rr.sValid = PSl.sValid;
            rr.sHostID = tbProjID.Text;
            rr.UrlWanted = PSl.FormURL(rr.sHostID);
            ShowData showData = new ShowData(ref ProjectStats, rr);

            // Fix: RunOne should be awaited as a Task, not as void
            await Task.Run(() => showData.RunOne());
            string sTemp = showData.SingleRunSheet;
            showData.Dispose();
            //PutOnNotepad(sTemp);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string sUrl = tbProjUrl.Text;
            FormReadRequest(sUrl);
            newReadRequest.UseStudy = true;
            ShowNotepadData(ref ProjectStats, ref newReadRequest);
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
        private void PutOnNotepad(string strIn)
        {
            CSendNotepad SendNotepad = new CSendNotepad();
            SendNotepad.PasteToNotepad(strIn);
        }

        private void ShowNotepadData(ref cProjectStruct ProjectStats, ref ReadRequest rr)
        {
            ShowData showData = new ShowData(ref ProjectStats, rr);
            showData.Visible = false;
            showData.ShowDialog();
            string sTemp = showData.SingleRunSheet;
            PutOnNotepad(sTemp);
            showData.Dispose();
        }

        private void findOtherPCsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoteSystems rs = new RemoteSystems(ref ProjectStats);
            rs.ShowDialog();
            rs.Dispose();
        }

        private void cbPCavail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPCavail.SelectedItem == null) return;
            string PCname = cbPCavail.SelectedItem.ToString();
            ManagedPCs.SelectCurrentFromPC(PCname);
            UpdateRB();
            ProjectChanged();
        }

        ReadRequest newReadRequest = new ReadRequest();
        private void FormReadRequest(string sUrl)
        {
            string sErrMsg = ParseUrl(sUrl, out string sHostID, out string sPage, out string sAppid, out string sProjName, out string sValid, out string PCname);
            if (sErrMsg == "")
            {
                newReadRequest.UsePandoraDatabase = false;
                newReadRequest.UrlWanted = sUrl;
                newReadRequest.bUseUrl = true;
                newReadRequest.shortname = sProjName;
                newReadRequest.sStudyV = sAppid;
                newReadRequest.PCname = PCname;
                newReadRequest.spage = sPage;
                newReadRequest.sValid = sValid;
                newReadRequest.sHostID = sHostID;
                newReadRequest.GetHdr = true;
                newReadRequest.GetBody = true;
                btnRun.Enabled = true;
            }
            else
            {
                MessageBox.Show(sErrMsg);
                btnRun.Enabled = false;
            }
        }


        private void btnPaste_Click(object sender, EventArgs e)
        {
            string sUrl = Clipboard.GetText().Trim().ToLower();
            tbProjUrl.Text = sUrl;
            FormReadRequest(sUrl);
        }


        private string ParseUrl(string sUrl, out string sHostID, out string sPage, out string sAppid, out string sProjName, out string sValid, out string sPCname)
        {
            int i, j;
            string sErrMsg = "";
            sHostID = "unknown";
            sPage = "0";
            sAppid = "null";
            sProjName = "null";
            sValid = "null";
            sPCname = "";
            int ProjectIndex = 0;

            if (string.IsNullOrEmpty(sUrl))
                return "null url";
            i = sUrl.IndexOf("http");
            if (i < 0)
            {
                return "badly formed url: missing http";
            }
            ProjectIndex = ProjectStats.GetUrlIndex(sUrl);
            if (ProjectIndex < 0)
            {
                return "Project not found in url";
            }
            cPSlist p = ProjectStats.ProjectList[ProjectIndex];
            sProjName = ProjectStats.ShortName(ProjectIndex);
            sErrMsg = HasHostID(sUrl, ref p, sProjName, out sHostID, out sPCname);
            sErrMsg += HasStudy(sUrl, ref p, out sAppid, out sValid);
            sErrMsg += HasOfst(sUrl, out sPage);
            return sErrMsg;
        }

        string HasHostID(string sUrl, ref cPSlist p, string ShortName, out string sHostID, out string ComputerID)
        {
            ComputerID = "unknown";
            sHostID = "";

            //need to get the value assigned to tHid and to tPage
            int i = sUrl.IndexOf(p.sHid);
            if (i < 0)
                return p.sHid + " not found in url";
            int j = FirstNonInteger(sUrl, i + p.sHid.Length);
            if (j > 0)
                sHostID = sUrl.Substring(i + p.sHid.Length, j - i - p.sHid.Length);
            if (j > 0 && sHostID != "")
            {
                bool bExit = false;
                foreach (cHostInfo hi in ManagedPCs.LocalSystems)
                {
                    if (bExit) break;
                    foreach (cLHe ce in hi.LocalProjID)
                    {
                        if (ce.ProjectsID == sHostID && ce.ProjectName == ShortName)
                        {
                            ComputerID = hi.ComputerID;
                            bExit = true;
                            break;
                        }
                    }
                }
            }
            else return p.sHid + "found, but no host value present";
            return "";
        }

        string HasStudy(string s, ref cPSlist p, out string studyID, out string sCode)
        {
            int j;
            studyID = "";
            sCode = "";
            int i = s.IndexOf("&appid="); // appid is not used for the study id by all projects!
            if (i >= 0)
            {
                j = FirstNonInteger(s, i + 7);
                studyID = s.Substring(i + 7, j - (i + 7));
                i = s.IndexOf(p.sHid);
                if (i < 0)
                    return p.sHid + " not found in url";
            }

            string tValid = p.sValid;
            if (tValid != "null") ;
            tValid = p.sValid.Substring(0, p.sValid.Length - 1);
            //&state=   &opt=  &type= /ltasktasks/ leaving off the 4, 4, 2 or 'B'

            i = s.IndexOf(tValid);
            if (i > 0)
            {
                j = FirstNonInteger(s, i + p.sValid.Length);
                if (j > 0)
                {
                    sCode = s.Substring(i + tValid.Length, j - i - tValid.Length);
                }
            }

            if (s.Contains("einstein"))
            {
                i = s.IndexOf("tasks/");
                if (i > 0)
                {
                    i += 6;
                    sCode = s.Substring(i, 1); // can be 0 or 4 but really needs to be 4
                    if (sCode == "0" || sCode == "4")
                    {
                        i++;
                        if (s.Substring(i, 1) == "/")
                        {
                            i++;
                            j = FirstNonInteger(s, i);
                            if (j > 0)
                            {
                                studyID = s.Substring(i, j - i);
                                return "";
                            }
                        }
                    }
                }
            }
            return "";
        }

        private string HasOfst(string s, out string ofst)
        {
            ofst = "0";
            int i = s.IndexOf("&offset=");
            if (i >= 0)
            {
                int j = FirstNonInteger(s, i + 8);
                if (j > 0)
                {
                    ofst = s.Substring(i + 8, j - (i + 8));
                    return "";
                }
                else return "&offset found but numeric value of offset is Missing";
            }
            else
            {
                i = s.IndexOf("?page=");
                if (i >= 0)
                {
                    int j = FirstNonInteger(s, i + 6);
                    if (j > 0)
                    {
                        ofst = s.Substring(i + 6, j - (i + 6));
                        return "";
                    }
                    else return "?page found but numeric value for page is missing";
                }
                return "";
            }
        }

        private void btnViewPage_Click(object sender, EventArgs e)
        {
            globals.OpenUrl(tbProjUrl.Text);
        }

        private void seeRemoteConfigFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.sUse = "BOINC";
            reqCmd.UseRadioButtons = true;
            EditccConfigs bpp = new EditccConfigs(ref ProjectStats, ref reqCmd);
            bpp.ShowDialog();
            bpp.Dispose();
        }


        private void seeWhatsBunkeredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.sUse = "BOINC";
            ShowBunkeredWUs bpp = new ShowBunkeredWUs(ref ProjectStats, ref reqCmd);
            bpp.ShowDialog();
            bpp.Dispose();
        }

        private void communicateWithPCsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.sUse = "SSH";
            CtrlRemotePCs bpp = new CtrlRemotePCs(ref ProjectStats, ref reqCmd);
            bpp.ShowDialog();
            bpp.Dispose();
        }

        private void ContactProject_Click(object sender, EventArgs e)
        {
            ContactProjects cp = new ContactProjects(ref ProjectStats);
            cp.ShowDialog();
            cp.Dispose();
        }

        private void collectDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadRequest rr = new ReadRequest();
            rr.UsePandoraDatabase = true;
            rr.GetHdr = true;
            rr.GetBody = true;
            ShowData showData = new ShowData(ref ProjectStats, rr);
            showData.FormSprint();
            showData.ShowDialog();
            showData.Dispose();
        }

        private void assignPCsAndProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReqCmds reqcmd = new ReqCmds();
            AssignStudies apCPUGPU = new AssignStudies(ref ProjectStats, reqcmd);
            apCPUGPU.ShowDialog();
            apCPUGPU.Dispose();
        }

        private void sendPandoraViewAppsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPandora ap = new ShowPandora(ref ProjectStats);
            ap.ShowDialog();
            ap.Dispose();
        }

        private void showEditAppConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAppConfig ap = new ShowAppConfig(ref ProjectStats);
            ap.ShowDialog();
            ap.Dispose();
        }

        private void PerformPortCheck()
        {
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.sUse = "SSHBOINC";
            reqCmd.PerformScan = true;
            reqCmd.UseRadioButtons = true;
            OnlinePCReport opp = new OnlinePCReport(ref ProjectStats, ref reqCmd);
            opp.ShowDialog();
            opp.Dispose();
        }

        private void whatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PerformPortCheck();
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.sUse = "SSHBOINC";
            reqCmd.UseRadioButtons = true;
            OnlinePCReport opp = new OnlinePCReport(ref ProjectStats, ref reqCmd);
            opp.ShowDialog();
            opp.Dispose();
        }

        private void passwordInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordInfo passwordInfo = new PasswordInfo();
            passwordInfo.ShowDialog();
            passwordInfo.Dispose();
        }

        private void CreditStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProjectStats.ManagedPCs.SaveManagedPCs();
        }

        private void getAllConfigFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewAppVersions eac = new ViewAppVersions(ref ProjectStats);
            eac.ShowDialog();
            eac.Dispose();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.sUse = "BOINC";
            reqCmd.UseRadioButtons = true;
            reqCmd.ColorAppConfig = true;
            EditAllAppConfigs eac = new EditAllAppConfigs(ref ProjectStats, ref reqCmd);
            eac.ShowDialog();
            eac.Dispose();
        }

        private void tsmHelp_Click(object sender, EventArgs e)
        {

            if (sender is ToolStripMenuItem menuItem)
            {
                string ClickedName = menuItem.Name;
                string sFilePath = Path.Combine(WhereCredit, ClickedName + ".docx");
                if (!File.Exists(sFilePath))
                    CreateFileFromTemplate(sFilePath);
                else
                {

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = sFilePath,
                        UseShellExecute = true
                    });
                }
            }
        }

        private void CreateFileFromTemplate(string targetPath)
        {
            // Get embedded template resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "CreditStatistics.blank.docx"; // <Namespace>.<Filename>

            using (Stream resource = assembly.GetManifestResourceStream(resourceName))
            {
                if (resource == null)
                    throw new Exception("Template resource not found.");

                using (FileStream file = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }

        private void tsmAssignCpuGpu_Click(object sender, EventArgs e)
        {
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.UseRadioButtons = true;
            reqCmd.ComboxUsage = "cpugpu";
            AssignCpuGpu mRuns = new AssignCpuGpu(ref ProjectStats, reqCmd);
            mRuns.ShowDialog();
            mRuns.Dispose();
        }

        private void tsmMinWUsNeeded_Click(object sender, EventArgs e)
        {
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.UseRadioButtons = true;
            reqCmd.ComboxUsage = "WUsWanted";
            SetupSampleData ssd = new SetupSampleData(ref ProjectStats, reqCmd);
            ssd.ShowDialog();
            ssd.Dispose();
        }

        private void tsmAssignStudy_Click(object sender, EventArgs e)
        {
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.UseRadioButtons = true;
            reqCmd.ComboxUsage = "StudyWanted";
            AssignStudies asW = new AssignStudies(ref ProjectStats, reqCmd);
            asW.ShowDialog();
            asW.Dispose();
        }

        private void tsmSendSC_Click(object sender, EventArgs e)
        {
            ReqCmds reqCmd = new ReqCmds();
            reqCmd.sUse = "SSHBOINC";
            reqCmd.UseRadioButtons = true;
            SampleRequest sReq = new SampleRequest(ref ProjectStats, ref reqCmd);
            sReq.ShowDialog();
            sReq.Dispose();
        }
    }
}
