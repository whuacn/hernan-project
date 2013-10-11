using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Globalization;
using CodeGen.Clases;

namespace CodeGen
{
    public partial class Columnas : Form
    {
        
        Table table = null;
        public Columnas(Table t)
        {
            table = t;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Loader();
        }

        void Loader()
        {
            dgvColumns.DataSource = table.Columns;
        }

        private void dgvColumns_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
