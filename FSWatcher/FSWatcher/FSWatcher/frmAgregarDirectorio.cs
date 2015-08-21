using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FSWatcherLib;

namespace FSWatcher
{
    public partial class frmAgregarDirectorio : Form
    {
        FileSystemWatcherExt _watcher;
        public frmAgregarDirectorio()
        {
            InitializeComponent();
            _watcher = new FileSystemWatcherExt();
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = false;
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

        }

        public frmAgregarDirectorio(FileSystemWatcherExt watcher)
        {
            InitializeComponent();
            _watcher = watcher;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txtPathOrigen.Text = _watcher.Path;
            txtPathDestino.Text = _watcher.PathDestino;
            chSubFolders.Checked = _watcher.IncludeSubdirectories;
            txtFiltro.Text = _watcher.Filter;
        }


        private void btnExaminar_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string path = folderBrowserDialog1.SelectedPath;
            if (Directory.Exists(path))
            {
                txtPathOrigen.Text = path;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string path = folderBrowserDialog1.SelectedPath;
            if (Directory.Exists(path))
            {
                txtPathDestino.Text = path;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidatePath())
            {
                _watcher.Path = txtPathOrigen.Text;
                _watcher.PathDestino = txtPathDestino.Text;                
                _watcher.IncludeSubdirectories = chSubFolders.Checked;
                _watcher.Filter = txtFiltro.Text;

                //watcher.EnableRaisingEvents = true;
                if (!Program.Watchers.Contains(_watcher))
                    Program.Watchers.Add(_watcher);

                this.Close();
            }
        }

        bool ValidatePath()
        {
            if (!Directory.Exists(txtPathOrigen.Text))
            {
                MessageBox.Show("El path origen no existe");
                return false;
            }
            if (!Directory.Exists(txtPathDestino.Text))
            {
                MessageBox.Show("El path destino no existe");
                return false;
            }
            return true;
        }
    }
}
