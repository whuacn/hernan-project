namespace CodeGen
{
    partial class Tablas
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvTablas = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtConnection = new System.Windows.Forms.TextBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.TableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GestorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameSpaceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTablas)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTablas
            // 
            this.dgvTablas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTablas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTablas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TableName,
            this.ClassName,
            this.GestorName,
            this.NameSpaceName,
            this.Selected});
            this.dgvTablas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTablas.Location = new System.Drawing.Point(0, 32);
            this.dgvTablas.Name = "dgvTablas";
            this.dgvTablas.RowHeadersVisible = false;
            this.dgvTablas.Size = new System.Drawing.Size(603, 260);
            this.dgvTablas.TabIndex = 0;
            this.dgvTablas.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTablas_CellContentDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGenerar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 292);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(603, 39);
            this.panel1.TabIndex = 2;
            // 
            // btnGenerar
            // 
            this.btnGenerar.Enabled = false;
            this.btnGenerar.Location = new System.Drawing.Point(516, 6);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(75, 23);
            this.btnGenerar.TabIndex = 0;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnConectar);
            this.panel2.Controls.Add(this.txtConnection);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(603, 32);
            this.panel2.TabIndex = 3;
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(12, 6);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.Size = new System.Drawing.Size(498, 20);
            this.txtConnection.TabIndex = 0;
            this.txtConnection.Text = "Provider=SQLOLEDB;Server=190.183.59.254;Database=SUMAR_Administracion;User Id=glr" +
    ";Password=glr1234;";
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(516, 4);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(75, 23);
            this.btnConectar.TabIndex = 1;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // TableName
            // 
            this.TableName.DataPropertyName = "TableName";
            this.TableName.HeaderText = "Tabla";
            this.TableName.Name = "TableName";
            this.TableName.ReadOnly = true;
            // 
            // ClassName
            // 
            this.ClassName.DataPropertyName = "ClassName";
            this.ClassName.HeaderText = "Clase";
            this.ClassName.Name = "ClassName";
            // 
            // GestorName
            // 
            this.GestorName.DataPropertyName = "GestorName";
            this.GestorName.HeaderText = "Gestor";
            this.GestorName.Name = "GestorName";
            // 
            // NameSpaceName
            // 
            this.NameSpaceName.DataPropertyName = "NameSpaceName";
            this.NameSpaceName.HeaderText = "NameSpaceName";
            this.NameSpaceName.Name = "NameSpaceName";
            // 
            // Selected
            // 
            this.Selected.DataPropertyName = "Selected";
            this.Selected.HeaderText = "Seleccionar";
            this.Selected.Name = "Selected";
            // 
            // Tablas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 331);
            this.Controls.Add(this.dgvTablas);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Tablas";
            this.Text = "Tablas";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTablas)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTablas;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.TextBox txtConnection;
        private System.Windows.Forms.DataGridViewTextBoxColumn TableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn GestorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameSpaceName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
    }
}

