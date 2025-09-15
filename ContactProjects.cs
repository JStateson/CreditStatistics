using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class ContactProjects : Form
    {
        cProjectStruct ProjectStats;
        public ContactProjects(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            FormProjectList();
            FormAccessParams();
            this.KeyPreview = true;  // Ensure the form receives key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();  // Close the form
            }
        }

        private void FormProjectList()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            bool bHasID = false;
            int MaxShortSize = 0;
            bool bInSprint = false;
            for (int i = 0; i < ProjectStats.ProjectList.Count; i++)
            {
                Button rb = new Button();
                rb.Tag = i;
                rb.Text = ProjectStats.ShortName(i);
                ToolTip toolTip = new ToolTip();
                toolTip.AutoPopDelay = 5000;   // Show for 5 seconds
                toolTip.InitialDelay = 1000;   // Delay before it appears
                toolTip.ReshowDelay = 500;     // Delay between re-show
                toolTip.ShowAlways = true;     // Show even if the form is not active
                toolTip.SetToolTip(rb, ProjectStats.ProjectList[i].MasterUrl);
                bInSprint = ProjectStats.ProjectList[i].UsedInSprint;
                MaxShortSize = Math.Max(MaxShortSize, rb.Text.Length);
                rb.AutoSize = true;
                rb.ForeColor = bInSprint ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 32);
                rb.Click += new System.EventHandler(this.rbProject_Clicked);
                gbSamURL.Controls.Add(rb);
                iRow++;
                if (iRow > 12)
                {
                    iRow = 0;
                    iCol++;
                }
            }
        }

        private void ResetAccessParams()
        {
            dgv.Rows.Clear();
            dgv.Rows.Add("Default", "", "prefs.php?subset=project", "hosts_user.php");
            dgv.Rows.Add("einstein", "", "account/prefs/project", "account/computers");
            dgv.Rows.Add("gerasim", "", "users/showUserHosts.aspx?userid=", "users/viewProjectPrefs.aspx");
            dgv.Rows.Add("wcg", "", "ms/device/viewProfiles.do", "ms/device/viewDevices.do");
        }

        private void FormAccessParams()
        {
            if (!LoadDgvFromXml())
                ResetAccessParams();
        }

        private void rbProject_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int iLoc = (int)btn.Tag;
            string MasterUrl = ProjectStats.ProjectList[iLoc].MasterUrl;
            iLoc = DgvFindProject(btn.Text);
            if (rbPrefs.Checked)
                FormUrl(MasterUrl, dgv.Rows[iLoc].Cells[2].Value.ToString(), dgv.Rows[iLoc].Cells[1].Value.ToString());
            else
                FormUrl(MasterUrl, dgv.Rows[iLoc].Cells[3].Value.ToString(), dgv.Rows[iLoc].Cells[1].Value.ToString());
        }

        private int DgvFindProject(string sName)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells[0].Value.ToString() == sName) return i;
                i++;
            }
            return 0;
        }

        private void FormUrl(string sMaster, string sCmd, string sID)
        {
            if (sCmd.Contains("http"))
            {
                LaunchDefaultBrowser(sCmd);
                return;
            }
            if (sCmd.EndsWith("="))
                sCmd += sID;
            string sOut = Path.Combine(sMaster + sCmd);
            LaunchDefaultBrowser(sOut);
        }

        private void LaunchDefaultBrowser(string sUrl)
        {
            globals.OpenUrl(sUrl);
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            ResetAccessParams();
            SaveDgvToXml();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveDgvToXml();
        }

        private void btnAllPCs_Click(object sender, EventArgs e)
        {
            int iLoc = 0;
            foreach (cPSlist c in ProjectStats.ProjectList)
            {
                if (!c.UsedInSprint) continue;
                FormUrl(c.MasterUrl, dgv.Rows[iLoc].Cells[3].Value.ToString(), dgv.Rows[iLoc].Cells[1].Value.ToString());
            }
            iLoc++;
        }

        private void btnAllPref_Click(object sender, EventArgs e)
        {
            int iLoc = 0;
            foreach (cPSlist c in ProjectStats.ProjectList)
            {
                if(!c.UsedInSprint) continue;               
                FormUrl(c.MasterUrl, dgv.Rows[iLoc].Cells[2].Value.ToString(), dgv.Rows[iLoc].Cells[1].Value.ToString());
            }
            iLoc++;
        }

        private void SaveDgvToXml()
        {
            string filePath = globals.WhereProjectAccessParams;

            DataTable dt = new DataTable("ProjAccessParams");

            // Create columns in DataTable based on DataGridView columns
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            // Copy rows from DataGridView to DataTable
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow) // Skip the empty new row
                {
                    object[] values = new object[dgv.Columns.Count];
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        values[i] = row.Cells[i].Value?.ToString() ?? "";
                    }
                    dt.Rows.Add(values);
                }
            }

            dt.WriteXml(filePath, XmlWriteMode.WriteSchema);
        }

        private bool LoadDgvFromXml()
        {
            string filePath = globals.WhereProjectAccessParams;
            if (!File.Exists(filePath))
            {
                return false;
            }

            DataTable dt = new DataTable();
            dt.ReadXml(filePath);

            dgv.Rows.Clear();

            // Fill DataGridView rows from DataTable
            foreach (DataRow dr in dt.Rows)
            {
                object[] values = new object[dgv.Columns.Count];
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    // Match by column name to avoid order issues
                    string colName = dgv.Columns[i].Name;
                    if (dt.Columns.Contains(colName))
                        values[i] = dr[colName]?.ToString() ?? "";
                    else
                        values[i] = "";
                }
                dgv.Rows.Add(values);
            }
            return true;
        }

        private void dgv_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int rowIndex = e.RowIndex; rowIndex < e.RowIndex + e.RowCount; rowIndex++)
            {
                DataGridViewRow row = dgv.Rows[rowIndex];

                // Only set values for cells that are currently null
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null)
                        cell.Value = "";
                }
            }
        }
    }
}
