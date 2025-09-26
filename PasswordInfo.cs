using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreditStatistics
{
    public partial class PasswordInfo : Form
    {
        public PasswordInfo()
        {
            InitializeComponent();
            tbUsername.Text = Properties.Settings.Default.BoincWebUsername;
        }

        private void btnSavePasswd_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.BoincWebPassword = tbPasswd.Text;
            Properties.Settings.Default.BoincWebUsername = tbUname.Text;
            Properties.Settings.Default.Save();
        }

        private void btnClearPassWD_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.BoincWebPassword = "";
        }
    }
}
