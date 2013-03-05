using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sipro.SourceControlProvider.SiproSCP
{
    public partial class ViewHistoryFile : Form
    {
        public string file { get; set; }
        public ViewHistoryFile()
        {
            InitializeComponent();
            
        }

        private void ViewHistoryFile_Load(object sender, EventArgs e)
        {
            lbFile.Text = file;
        }
    }
}
