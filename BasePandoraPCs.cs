using CreditStatistics;
using CreditStatistics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;


namespace CreditStatistics
{
    internal partial class BasePandoraPCs : Form
    {
        public class PCsChangedEventArgs : EventArgs
        {
            public bool MustRescan { get; }
            public string CheckedName { get; }
            public int NumChecked { get; }
            public bool pbUseCancel { get; }
            public PCsChangedEventArgs(bool value, string checkedName, int numChecked, bool Bcancel)
            {
                MustRescan = value;
                CheckedName = checkedName;
                NumChecked = numChecked;
                pbUseCancel = Bcancel;
            }
        }

        protected ProgressBar BaseProgressBar => pbUSE;

        public cProjectStruct ProjectStats;
        public cManagedPCs ManagedPCs;
        public PandoraConfig pandoraConfig;
        public List<cPClimit> PandoraDatabase;
        public cPClimit TempletDB;
        public PandoraRPC pandoraRPC;

        protected int NumOffLine = 0;
        public event EventHandler<PCsChangedEventArgs> PCsChanged;
        private ReqCmds reqCmd;
        private bool isCB = true;
        private string LastChecked = "";
        private int LastCount = -1;
        private bool InitLoad = false;
        private bool CancelLocal = false;

        private bool mS;    // must check ssh status
        private bool mB;    // must check boinc port status

        public BasePandoraPCs()
        {
            InitializeComponent();
        }

        bool FirstOnLine = true;
        string FirstOnlinePC = "";

        public BasePandoraPCs(ref cProjectStruct rProjectStats, ref ReqCmds RreqCmd)
        {
            InitializeComponent();
            // InitLoad = true;
            ProjectStats = rProjectStats;
            ManagedPCs = ProjectStats.ManagedPCs;

            pbUSE.Maximum = ManagedPCs.LocalSystems.Count + 1;
            pandoraConfig = new PandoraConfig(ref rProjectStats);
            PandoraDatabase = ProjectStats.PandoraDatabase;
            pandoraRPC = ManagedPCs.rpc;
            pandoraRPC.Init(ref ProjectStats);
            TempletDB = ProjectStats.TempletDB;
            reqCmd = RreqCmd;
            if (RreqCmd.UseRadioButtons)
            {
                isCB = false;
                btnCheckPC.Visible = false;
                btnClearPC.Visible = false;
                btnInvertPC.Visible = false;
                FormSystemsRB();
            }
            else
                FormSystemsCB();
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Shown += InitialLoad;
        }

        private void InitialLoad(object sender, EventArgs e)
        {
            if (reqCmd.sUse == "SSHBOINC")
            {
                reqCmd.sUse = "SSH";
                PerformRescan();
            }
            else
            {
                CancelLocal = false;
                if (isCB)
                    IsOnline();
                else
                    IsOnlineRB();
                lbLastScanned.Text = ProjectStats.DateLastPortScan;
            }
            InitLoad = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();  // Close the form
            }
        }

        protected async void IsOnlineRB()
        {
            int i = -1;
            int j = 1;
            NumOffLine = 0;
            bool JustScanned = false;
            Task aTask;
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                if (CancelLocal) break;
                i++;
                string PCname = hi.ComputerID;
                mS = !hi.SSHvalid;
                mB = !hi.BOINCvalid;
                JustScanned |= mS;
                JustScanned |= mB;
                pbUSE.Value++;
                RadioButton rb = ThisRB(i);
                Application.DoEvents();
                rb.CheckedChanged -= rb_CheckedChanged;
                switch (reqCmd.sUse)
                {
                    case "BOINC":
                        if (mB)
                        {
                            aTask = Task.Run(async () =>
                            {
                                hi.HasBOINC = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[0]);
                            });
                            aTask.Wait();
                            hi.BOINCvalid = true;
                        }
                        rb.Enabled = hi.HasBOINC;
                        //rb.Checked = hi.HasBOINC && isCB;
                        if (hi.HasBOINC && FirstOnLine)
                        {
                            rb.Checked = true;
                            FirstOnLine = false;
                        }
                        rb.ForeColor = ManagedPCs.GetColor(PCname);
                        if (!rb.Enabled)
                            NumOffLine++;
                        else
                            if (LastChecked == "")
                        {
                            LastChecked = rb.Text;
                            //rb.Checked = true;
                        }
                        break;

                    case "SSH":
                        if (mB && mS)
                        {
                            aTask = Task.Run(async () =>
                            {
                                if (hi.IPaddress != "127.0.0.1")
                                    hi.HasSSH = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[1]);
                                else hi.HasSSH = true;
                                hi.HasBOINC = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[0]);
                            });
                            aTask.Wait();
                            hi.BOINCvalid = true;
                            hi.SSHvalid = true;
                            if (hi.HasSSH && FirstOnLine)
                            {
                                FirstOnLine = false;
                                rb.Checked = true;
                            }
                        }
                        else if (mB)
                        {
                            aTask = Task.Run(async () =>
                            {
                                hi.HasBOINC = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[0]);
                            });
                            aTask.Wait();
                            hi.BOINCvalid = true;
                        }
                        else if (mS)
                        {
                            aTask = Task.Run(async () =>
                            {
                                if (hi.IPaddress != "127.0.0.1")
                                    hi.HasSSH = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[1]);
                                else hi.HasSSH = true;
                            });
                            aTask.Wait();
                            hi.SSHvalid = true;
                        }

                        rb.Enabled = hi.HasSSH;
                        rb.Checked = hi.HasSSH && isCB;
                        rb.ForeColor = ManagedPCs.GetColor(PCname);
                        if (!rb.Enabled)
                            NumOffLine++;
                        else
                        {
                            if (LastChecked == "")
                                LastChecked = rb.Text;

                            if (rb.ForeColor == Color.Red)
                                NumOffLine++;
                        }
                        break;
                    default:
                        rb.Enabled = true;
                        rb.Checked = FirstOnLine;
                        FirstOnLine = false;
                        break;
                }
                rb.CheckedChanged += rb_CheckedChanged;
            }
            pbUSE.Value = 0;
            Application.DoEvents();
            //btnReScan.Enabled = NumOffLine > 0;
            NotifyForms(true, LastChecked, 1, false);
            pbUSE.Value = 0;
            if (JustScanned)
                SetScanDate();
        }

        //the checkbox version
        //protected async void IsOnline()
        private void IsOnline()
        {
            int i = -1;
            int j = 1;
            NumOffLine = 0;
            int CheckedCount = 0;
            Task aTask;
            bool JustScanned = false;
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                if (CancelLocal) break;
                i++;
                mS = !hi.SSHvalid;
                mB = !hi.BOINCvalid;
                JustScanned |= mS;
                JustScanned |= mB;
                pbUSE.Value++;
                string PCname = hi.ComputerID;
                CheckBox cb = ThisCB(i);
                Application.DoEvents();
                cb.CheckedChanged -= cb_CheckedChanged;
                switch (reqCmd.sUse)
                {
                    case "BOINC":
                        if (mB)
                        {
                            aTask = Task.Run(async () =>
                            {
                                hi.HasBOINC = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[0]);
                            });
                            aTask.Wait();
                            hi.BOINCvalid = true;
                        }

                        cb.Enabled = hi.HasBOINC;
                        cb.Checked = hi.HasBOINC && isCB;
                        cb.ForeColor = ManagedPCs.GetColor(PCname);
                        if (!cb.Enabled)
                            NumOffLine++;
                        pandoraConfig.NameToSprintPC(cb.Text.ToString()).IsSelected = cb.Checked;
                        break;

                    case "SSH":
                        if (mB && mS)
                        {
                            aTask = Task.Run(async () =>
                            {
                                if (hi.IPaddress != "127.0.0.1")
                                    hi.HasSSH = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[1]);
                                else hi.HasSSH = true; // assume true but will not be used as ssh
                                hi.HasBOINC = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[0]);
                            });
                            aTask.Wait();
                            hi.BOINCvalid = true;
                            hi.SSHvalid = true;
                        }
                        else if (mB)
                        {
                            aTask = Task.Run(async () =>
                            {
                                hi.HasBOINC = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[0]);
                            });
                            aTask.Wait();
                            hi.BOINCvalid = true;
                        }
                        else if (mS)
                        {
                            aTask = Task.Run(async () =>
                            {
                                if (hi.IPaddress != "127.0.0.1")
                                    hi.HasSSH = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, reqCmd.nPorts[1]);
                                else hi.HasSSH = true;
                            });
                            aTask.Wait();
                            hi.SSHvalid = true;
                        }

                        cb.Enabled = hi.HasSSH;
                        cb.Checked = hi.HasSSH && isCB;
                        cb.ForeColor = ManagedPCs.GetColor(PCname);
                        if (!cb.Enabled)
                            NumOffLine++;
                        else
                        {
                            if (LastChecked == "")
                                LastChecked = cb.Text;
                            if (cb.ForeColor == Color.Red)
                                NumOffLine++;
                        }
                        pandoraConfig.NameToSprintPC(cb.Text.ToString()).IsSelected = cb.Checked;
                        break;

                    default:
                        cb.Enabled = true;
                        cb.Checked = true && isCB;
                        break;
                }
                if (cb.Checked) CheckedCount++;
                cb.CheckedChanged += cb_CheckedChanged;
            }
            pbUSE.Value = 0;
            Application.DoEvents();
            //btnReScan.Enabled = NumOffLine > 0;
            NotifyForms(true, LastChecked, CheckedCount, false);
            pbUSE.Value = 0;
            if (JustScanned)
                SetScanDate();
        }


        private void NotifyForms(bool bRescan, string sname, int iLoc, bool ForceCancel)
        {
            if (InitLoad) return;
            LastCount = iLoc;
            PCsChanged?.Invoke(this, new PCsChangedEventArgs(bRescan, sname, iLoc, ForceCancel));
        }


        private void FormSystemsRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;

            for (int i = 0; i < ManagedPCs.LocalSystems.Count(); i++)
            {
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = ManagedPCs.LocalSystems[i].ComputerID;
                rb.Enabled = false;
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
                gbPCs.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }



        private void FormSystemsCB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;

            for (int i = 0; i < ManagedPCs.LocalSystems.Count(); i++)
            {
                CheckBox cb = new CheckBox();
                cb.Tag = i;
                cb.Text = ManagedPCs.LocalSystems[i].ComputerID;
                cb.Enabled = false;
                MaxShortSize = Math.Max(MaxShortSize, cb.Text.Length);
                cb.AutoSize = true;
                cb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                cb.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
                gbPCs.Controls.Add(cb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            pandoraConfig.NameToSprintPC(cb.Text.ToString()).IsSelected = cb.Checked;
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked) return;
            LastChecked = rb.Text;
            NotifyForms(false, LastChecked, 1, false);
        }

        public void StartPBuse( int imax)
        {
            btnCancel.Visible = true;
            pbUSE.Value = 1;
            pbUSE.Maximum = imax + 1;
        }

        public void IncPBuse()
        {
            pbUSE.Value++;
        }

        public void StopPBuse()
        {
            btnCancel.Visible = false;
            pbUSE.Value = 0;
        }

        // status may have changed, need to udpate checkboxes or radioboxes
        public bool IsStillOnline(string PCname, bool IsCheckbox, bool SetCheck)
        {
            bool Rtn = true;
            cHostInfo hi = ManagedPCs.NameToSystem(PCname);
            int iTag = ManagedPCs.NameToIndex(PCname);
            bool bHasSSH = true, bHasBOINC = true;
            CheckBox cb;
            RadioButton rb;
            Color ThisColor, NewColor;
            if (IsCheckbox)
            {
                cb = ThisCB(iTag);
                ThisColor = cb.ForeColor;
                NewColor = ManagedPCs.GetColor(PCname);
                if (NewColor == ThisColor) return true;
                if (!hi.HasBOINC && !hi.HasSSH)
                {
                    cb.Enabled = false;
                    cb.ForeColor = Color.Black;
                    cb.Checked = SetCheck;
                    return false;
                }
                cb.ForeColor = NewColor;
                return false;
            }
            else
            {
                rb = ThisRB(iTag);
                ThisColor = rb.ForeColor;
                NewColor = ManagedPCs.GetColor(PCname);
                if (NewColor == ThisColor) return true;
                if (hi.HasBOINC && !hi.HasSSH)
                {
                    rb.Enabled = false;
                    rb.ForeColor = Color.Black;
                    return false;
                }
                rb.ForeColor = NewColor;
                return false;
            }
            return Rtn;
        }

        public CheckBox ThisCB(int iTag)
        {
            foreach (Control c in gbPCs.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    if ((int)c.Tag == iTag) return cb;
                }
            }
            Debug.Assert(false, "internal error: checkbox missing");
            return null;
        }

        public RadioButton ThisRB(int iTag)
        {
            foreach (Control c in gbPCs.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    if ((int)c.Tag == iTag) return rb;
                }
            }
            Debug.Assert(false, "internal error: radiobutton missing");
            return null;
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
                            if (!cb.Enabled) continue;
                            switch (sOP)
                            {
                                case "Check all":
                                    cb.Checked = true;
                                    break;
                                case "Clear all":
                                    cb.Checked = false;
                                    break;
                                case "Invert":
                                    cb.Checked = !cb.Checked;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void PerformRescan()
        {
            pbUSE.Maximum = ManagedPCs.LocalSystems.Count + 1;
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                hi.BOINCvalid = false;
                hi.SSHvalid = false;
            }
            CancelLocal = false;
            if (isCB)
                IsOnline();
            else
                IsOnlineRB();
            SetScanDate();
        }

        private void SetScanDate()
        {
            lbLastScanned.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            ProjectStats.DateLastPortScan = lbLastScanned.Text;
        }

        private void btnReScan_Click(object sender, EventArgs e)
        {
            PerformRescan();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelLocal = true;
            NotifyForms(false, LastChecked, LastCount, true);
            StopPBuse();
        }
    }
}
