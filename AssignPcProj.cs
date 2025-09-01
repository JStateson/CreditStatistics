using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CreditStatistics.PandoraConfig;

namespace CreditStatistics
{
    internal partial class AssignPcProj : Form
    {
        private cProjectStruct ProjectStats;
        private List<cPClimit> PClimit;
        private PandoraConfig pc;
        private cPClimit SelectedPC = null;

        public AssignPcProj(ref cProjectStruct rProjectStats)
        {
            InitializeComponent();
            ProjectStats = rProjectStats;
            pc = new PandoraConfig(ref rProjectStats);
            FormSystemsRB();
            FormProjectsCB();
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

        private void FormSystemsRB()
        {
            int iRow = 0, iCol = 0;
            int oRow = 32, oCol = 10;
            int i = 0;
            int rbRow = 0;
            int rbCol = 0;
            foreach (cPClimit pcl in pc.PandoraDatabase)
            {
                RadioButton rb = new RadioButton();
                rb.Tag = i;
                rb.Text = pcl.PCname;
                rb.Location = new System.Drawing.Point(oCol + iCol * 120, oRow + iRow * 20);
                rb.Enabled = true; // pcl.IsSelected;
                rb.CheckedChanged += new System.EventHandler(this.rbPC_CheckedChanged);
                rb.ForeColor = ProjectStats.ManagedPCs.GetColor(pcl.PCname);
                gbPCs.Controls.Add(rb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
                i++;
            }
        }

        private void rbPC_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int iPC = (int)rb.Tag;
            SelectedPC = pc.PandoraDatabase[iPC];
            FormProjChecks();
        }

        private void FormProjChecks()
        {
            foreach (cCalcLimitProj clp in SelectedPC.ProjList)
            {
                SetPROJcheck(clp.UsedInSprint, clp.ShortName);
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
                //cb.Checked = clp.UsedInGames;
                gbPJs.Controls.Add(cb);
                iRow++;
                if (iRow > 20)
                {
                    iRow = 0;
                    iCol++;
                }
                i++;
            }
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

        private void SetPROJcheck(bool b, string shortname)
        {
            foreach (Control c in gbPJs.Controls)
            {
                if (c is CheckBox cb && cb.Text == shortname)
                {
                    cb.Checked = b;
                }
            }
        }

        private void btnSavePcProj_Click(object sender, EventArgs e)
        {
            foreach (Control c in gbPJs.Controls)
            {
                if (c is CheckBox cb)
                {
                    cCalcLimitProj clp = SelectedPC.GetProjStruct(cb.Text);
                    clp.UsedInSprint = cb.Checked;
                }
            }
            pc.WriteDBrecord(ref SelectedPC);
        }

        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            foreach(cCalcLimitProj clp in ProjectStats.TempletDB.ProjList)
            {
                SetPROJcheck(clp.UsedInSprint, clp.ShortName);
            }
        }
    }
}
