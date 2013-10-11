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
    public partial class Tablas : Form
    {
        string connection = "";
        List<Table> tables = new List<Table>();
        public Tablas()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            this.connection = txtConnection.Text;
            dgvTablas.DataSource = null;
            dgvTablas.Refresh();
            //dgvTablas.Rows.Clear();
            Tables();
            btnGenerar.Enabled = true;
        }


        void Tables()
        {
            using (OleDbConnection oleDbConnection = new OleDbConnection(connection))
            {
                try
                {
                    oleDbConnection.Open();

                    DataTable schema = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "table" });
                    oleDbConnection.Close();

                    foreach (DataRow rs in schema.Rows)
                    {
                        Table t = new Table(rs["TABLE_NAME"].ToString());
                        t.Columns = Columns(t);                        
                        tables.Add(t);
                    }
                    dgvTablas.DataSource = tables;

                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al conectar: " + e.Message);
                }
            }
        }

        List<Column> Columns(Table t)
        {

            List<Column> Columns = new List<Column>();

            string sql =
                string.Format(
                    CultureInfo.InvariantCulture,
                    @"select * from [{0}]",
                    t.TableName);

            using (OleDbConnection oleDbConnection = new OleDbConnection(connection))
            {
                try
                {
                    OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDbConnection);
                    oleDbConnection.Open();

                    OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader(CommandBehavior.KeyInfo);
                    DataTable dataTable = oleDbDataReader.GetSchemaTable();

                    foreach (DataRow rs in dataTable.Rows)
                    {
                        Column c = new Column();
                        c.ColumnName = rs["ColumnName"].ToString();
                        c.Property = c.ColumnName;
                        c.DataType = rs["DataType"].ToString().Replace("System.", string.Empty);
                        c.Selected = true;
                        Columns.Add(c);
                    }

                    oleDbDataReader.Close();

                    return Columns;
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al conectar: " + e.Message);
                    return null;
                }
            }
        }

        private void dgvTablas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Table t = (Table)dgvTablas.Rows[e.RowIndex].DataBoundItem;
            Columnas colform = new Columnas(t);
            colform.ShowDialog();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            List<Table> ts = tables.Where(s => s.Selected == true).ToList();
            foreach (Table t in ts)
            {
                Procesar p = new Procesar(t);
                p.Iniciar();
            }
        }


    }
}
