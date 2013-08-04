using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMailNotifier.Engine;

namespace GMailNotifier
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
                svrNotifier.LoadData(UsernameTextBox.Text, PasswordTextBox.Text);
                Storage.setSetting("UID", Security.Protect(UsernameTextBox.Text));
                Storage.setSetting("CLV", Security.Protect(PasswordTextBox.Text));
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gmail Notifier", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
                      
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            UsernameTextBox.Text = Security.Unprotect(Storage.getSetting("UID"));
        }
    }
}
