namespace RemoteCmd
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtMachine = new System.Windows.Forms.TextBox();
            this.lbPC = new System.Windows.Forms.Label();
            this.btnTestport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(61, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Block";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(142, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "UnBlock";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtMachine
            // 
            this.txtMachine.Location = new System.Drawing.Point(87, 12);
            this.txtMachine.Name = "txtMachine";
            this.txtMachine.Size = new System.Drawing.Size(100, 20);
            this.txtMachine.TabIndex = 2;
            // 
            // lbPC
            // 
            this.lbPC.AutoSize = true;
            this.lbPC.Location = new System.Drawing.Point(58, 15);
            this.lbPC.Name = "lbPC";
            this.lbPC.Size = new System.Drawing.Size(20, 13);
            this.lbPC.TabIndex = 3;
            this.lbPC.Text = "Pc";
            // 
            // btnTestport
            // 
            this.btnTestport.Location = new System.Drawing.Point(61, 74);
            this.btnTestport.Name = "btnTestport";
            this.btnTestport.Size = new System.Drawing.Size(75, 23);
            this.btnTestport.TabIndex = 4;
            this.btnTestport.Text = "Test PC";
            this.btnTestport.UseVisualStyleBackColor = true;
            this.btnTestport.Click += new System.EventHandler(this.btnTestport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 119);
            this.Controls.Add(this.btnTestport);
            this.Controls.Add(this.lbPC);
            this.Controls.Add(this.txtMachine);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtMachine;
        private System.Windows.Forms.Label lbPC;
        private System.Windows.Forms.Button btnTestport;
    }
}

