namespace UrlStress
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
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
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
            this.components = new System.ComponentModel.Container();
            this.ButtonRun = new System.Windows.Forms.Button();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.intHilos = new System.Windows.Forms.NumericUpDown();
            this.numRequest = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.chkForever = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label200 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelCant200 = new System.Windows.Forms.Label();
            this.labelCant304 = new System.Windows.Forms.Label();
            this.label304 = new System.Windows.Forms.Label();
            this.labelCant401 = new System.Windows.Forms.Label();
            this.label401 = new System.Windows.Forms.Label();
            this.labelCant404 = new System.Windows.Forms.Label();
            this.label404 = new System.Windows.Forms.Label();
            this.labelCantFallos = new System.Windows.Forms.Label();
            this.labelFallos = new System.Windows.Forms.Label();
            this.labelCantRequest = new System.Windows.Forms.Label();
            this.labelRequest = new System.Windows.Forms.Label();
            this.labeCantlRequestSec = new System.Windows.Forms.Label();
            this.labelRequestSec = new System.Windows.Forms.Label();
            this.labelCantHilos = new System.Windows.Forms.Label();
            this.labelHilos = new System.Windows.Forms.Label();
            this.listBoxUrls = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.intHilos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRequest)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonRun
            // 
            this.ButtonRun.Location = new System.Drawing.Point(9, 355);
            this.ButtonRun.Name = "ButtonRun";
            this.ButtonRun.Size = new System.Drawing.Size(75, 23);
            this.ButtonRun.TabIndex = 0;
            this.ButtonRun.Text = "Iniciar";
            this.ButtonRun.UseVisualStyleBackColor = true;
            this.ButtonRun.Click += new System.EventHandler(this.ButtonRun_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.Location = new System.Drawing.Point(90, 355);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(75, 23);
            this.ButtonStop.TabIndex = 1;
            this.ButtonStop.Text = "Detener";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(171, 355);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "Limpiar";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cantidad de Hilos";
            // 
            // intHilos
            // 
            this.intHilos.Location = new System.Drawing.Point(133, 7);
            this.intHilos.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.intHilos.Name = "intHilos";
            this.intHilos.Size = new System.Drawing.Size(63, 20);
            this.intHilos.TabIndex = 5;
            this.intHilos.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numRequest
            // 
            this.numRequest.Location = new System.Drawing.Point(133, 33);
            this.numRequest.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numRequest.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRequest.Name = "numRequest";
            this.numRequest.Size = new System.Drawing.Size(63, 20);
            this.numRequest.TabIndex = 7;
            this.numRequest.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Cantidad de Solicitudes";
            // 
            // chkForever
            // 
            this.chkForever.AutoSize = true;
            this.chkForever.Location = new System.Drawing.Point(203, 35);
            this.chkForever.Name = "chkForever";
            this.chkForever.Size = new System.Drawing.Size(86, 17);
            this.chkForever.TabIndex = 8;
            this.chkForever.Text = "para siempre";
            this.chkForever.UseVisualStyleBackColor = true;
            this.chkForever.CheckedChanged += new System.EventHandler(this.chkForever_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Lista de direcciones";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(12, 82);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(285, 20);
            this.txtUrl.TabIndex = 10;
            // 
            // label200
            // 
            this.label200.AutoSize = true;
            this.label200.Location = new System.Drawing.Point(25, 26);
            this.label200.Name = "label200";
            this.label200.Size = new System.Drawing.Size(25, 13);
            this.label200.TabIndex = 11;
            this.label200.Text = "200";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelCantFallos);
            this.groupBox1.Controls.Add(this.labelFallos);
            this.groupBox1.Controls.Add(this.labelCant404);
            this.groupBox1.Controls.Add(this.label404);
            this.groupBox1.Controls.Add(this.labelCant401);
            this.groupBox1.Controls.Add(this.label401);
            this.groupBox1.Controls.Add(this.labelCant304);
            this.groupBox1.Controls.Add(this.label304);
            this.groupBox1.Controls.Add(this.labelCant200);
            this.groupBox1.Controls.Add(this.label200);
            this.groupBox1.Location = new System.Drawing.Point(427, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 106);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // labelCant200
            // 
            this.labelCant200.AutoSize = true;
            this.labelCant200.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCant200.ForeColor = System.Drawing.Color.Green;
            this.labelCant200.Location = new System.Drawing.Point(106, 26);
            this.labelCant200.Name = "labelCant200";
            this.labelCant200.Size = new System.Drawing.Size(14, 13);
            this.labelCant200.TabIndex = 12;
            this.labelCant200.Text = "0";
            // 
            // labelCant304
            // 
            this.labelCant304.AutoSize = true;
            this.labelCant304.Location = new System.Drawing.Point(106, 39);
            this.labelCant304.Name = "labelCant304";
            this.labelCant304.Size = new System.Drawing.Size(13, 13);
            this.labelCant304.TabIndex = 14;
            this.labelCant304.Text = "0";
            // 
            // label304
            // 
            this.label304.AutoSize = true;
            this.label304.Location = new System.Drawing.Point(25, 39);
            this.label304.Name = "label304";
            this.label304.Size = new System.Drawing.Size(25, 13);
            this.label304.TabIndex = 13;
            this.label304.Text = "304";
            // 
            // labelCant401
            // 
            this.labelCant401.AutoSize = true;
            this.labelCant401.Location = new System.Drawing.Point(106, 52);
            this.labelCant401.Name = "labelCant401";
            this.labelCant401.Size = new System.Drawing.Size(13, 13);
            this.labelCant401.TabIndex = 16;
            this.labelCant401.Text = "0";
            // 
            // label401
            // 
            this.label401.AutoSize = true;
            this.label401.Location = new System.Drawing.Point(25, 52);
            this.label401.Name = "label401";
            this.label401.Size = new System.Drawing.Size(25, 13);
            this.label401.TabIndex = 15;
            this.label401.Text = "401";
            // 
            // labelCant404
            // 
            this.labelCant404.AutoSize = true;
            this.labelCant404.Location = new System.Drawing.Point(106, 65);
            this.labelCant404.Name = "labelCant404";
            this.labelCant404.Size = new System.Drawing.Size(13, 13);
            this.labelCant404.TabIndex = 18;
            this.labelCant404.Text = "0";
            // 
            // label404
            // 
            this.label404.AutoSize = true;
            this.label404.Location = new System.Drawing.Point(25, 65);
            this.label404.Name = "label404";
            this.label404.Size = new System.Drawing.Size(25, 13);
            this.label404.TabIndex = 17;
            this.label404.Text = "404";
            // 
            // labelCantFallos
            // 
            this.labelCantFallos.AutoSize = true;
            this.labelCantFallos.ForeColor = System.Drawing.Color.Red;
            this.labelCantFallos.Location = new System.Drawing.Point(106, 78);
            this.labelCantFallos.Name = "labelCantFallos";
            this.labelCantFallos.Size = new System.Drawing.Size(13, 13);
            this.labelCantFallos.TabIndex = 20;
            this.labelCantFallos.Text = "0";
            // 
            // labelFallos
            // 
            this.labelFallos.AutoSize = true;
            this.labelFallos.Location = new System.Drawing.Point(25, 78);
            this.labelFallos.Name = "labelFallos";
            this.labelFallos.Size = new System.Drawing.Size(34, 13);
            this.labelFallos.TabIndex = 19;
            this.labelFallos.Text = "Fallos";
            // 
            // labelCantRequest
            // 
            this.labelCantRequest.AutoSize = true;
            this.labelCantRequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCantRequest.Location = new System.Drawing.Point(533, 205);
            this.labelCantRequest.Name = "labelCantRequest";
            this.labelCantRequest.Size = new System.Drawing.Size(14, 13);
            this.labelCantRequest.TabIndex = 14;
            this.labelCantRequest.Text = "0";
            // 
            // labelRequest
            // 
            this.labelRequest.AutoSize = true;
            this.labelRequest.Location = new System.Drawing.Point(452, 205);
            this.labelRequest.Name = "labelRequest";
            this.labelRequest.Size = new System.Drawing.Size(47, 13);
            this.labelRequest.TabIndex = 13;
            this.labelRequest.Text = "Request";
            // 
            // labeCantlRequestSec
            // 
            this.labeCantlRequestSec.AutoSize = true;
            this.labeCantlRequestSec.Location = new System.Drawing.Point(533, 218);
            this.labeCantlRequestSec.Name = "labeCantlRequestSec";
            this.labeCantlRequestSec.Size = new System.Drawing.Size(13, 13);
            this.labeCantlRequestSec.TabIndex = 16;
            this.labeCantlRequestSec.Text = "0";
            // 
            // labelRequestSec
            // 
            this.labelRequestSec.AutoSize = true;
            this.labelRequestSec.Location = new System.Drawing.Point(452, 218);
            this.labelRequestSec.Name = "labelRequestSec";
            this.labelRequestSec.Size = new System.Drawing.Size(71, 13);
            this.labelRequestSec.TabIndex = 15;
            this.labelRequestSec.Text = "Request/Sec";
            // 
            // labelCantHilos
            // 
            this.labelCantHilos.AutoSize = true;
            this.labelCantHilos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCantHilos.ForeColor = System.Drawing.Color.Blue;
            this.labelCantHilos.Location = new System.Drawing.Point(533, 248);
            this.labelCantHilos.Name = "labelCantHilos";
            this.labelCantHilos.Size = new System.Drawing.Size(14, 13);
            this.labelCantHilos.TabIndex = 18;
            this.labelCantHilos.Text = "0";
            // 
            // labelHilos
            // 
            this.labelHilos.AutoSize = true;
            this.labelHilos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHilos.ForeColor = System.Drawing.Color.Blue;
            this.labelHilos.Location = new System.Drawing.Point(452, 248);
            this.labelHilos.Name = "labelHilos";
            this.labelHilos.Size = new System.Drawing.Size(35, 13);
            this.labelHilos.TabIndex = 17;
            this.labelHilos.Text = "Hilos";
            // 
            // listBoxUrls
            // 
            this.listBoxUrls.FormattingEnabled = true;
            this.listBoxUrls.Location = new System.Drawing.Point(12, 109);
            this.listBoxUrls.Name = "listBoxUrls";
            this.listBoxUrls.Size = new System.Drawing.Size(366, 186);
            this.listBoxUrls.TabIndex = 19;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(303, 80);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 20;
            this.btnAdd.Text = "Agregar";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(303, 301);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 21;
            this.btnRemove.Text = "Quitar";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 390);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.listBoxUrls);
            this.Controls.Add(this.labelCantHilos);
            this.Controls.Add(this.labelHilos);
            this.Controls.Add(this.labeCantlRequestSec);
            this.Controls.Add(this.labelRequestSec);
            this.Controls.Add(this.labelCantRequest);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.labelRequest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkForever);
            this.Controls.Add(this.numRequest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.intHilos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.ButtonStop);
            this.Controls.Add(this.ButtonRun);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Url Stress";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.intHilos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRequest)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonRun;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown intHilos;
        private System.Windows.Forms.NumericUpDown numRequest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkForever;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label200;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelCantFallos;
        private System.Windows.Forms.Label labelFallos;
        private System.Windows.Forms.Label labelCant404;
        private System.Windows.Forms.Label label404;
        private System.Windows.Forms.Label labelCant401;
        private System.Windows.Forms.Label label401;
        private System.Windows.Forms.Label labelCant304;
        private System.Windows.Forms.Label label304;
        private System.Windows.Forms.Label labelCant200;
        private System.Windows.Forms.Label labelCantRequest;
        private System.Windows.Forms.Label labelRequest;
        private System.Windows.Forms.Label labeCantlRequestSec;
        private System.Windows.Forms.Label labelRequestSec;
        private System.Windows.Forms.Label labelCantHilos;
        private System.Windows.Forms.Label labelHilos;
        private System.Windows.Forms.ListBox listBoxUrls;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
    }
}

