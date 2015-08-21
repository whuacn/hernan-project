using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSWatcherLib;

namespace FSWatcher
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        void LoadGrid()
        {
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = null;
            dgvList.DataSource = Program.Watchers;
            dgvList.Refresh();

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregarDirectorio frm = new frmAgregarDirectorio();
            frm.ShowDialog();
            LoadGrid();
        }

        void dgvList_CellDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                frmAgregarDirectorio frm = new frmAgregarDirectorio((FileSystemWatcherExt)dgvList.Rows[e.RowIndex].DataBoundItem);
                frm.ShowDialog();
                LoadGrid();
            }

        }

 


    }
}
