﻿namespace UrlStress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ButtonRun = new System.Windows.Forms.Button();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.numThreads = new System.Windows.Forms.NumericUpDown();
            this.numRequest = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.chkForever = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label200 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelCountFailures = new System.Windows.Forms.Label();
            this.labelFallos = new System.Windows.Forms.Label();
            this.labelCount404 = new System.Windows.Forms.Label();
            this.label404 = new System.Windows.Forms.Label();
            this.labelCount401 = new System.Windows.Forms.Label();
            this.label401 = new System.Windows.Forms.Label();
            this.labelCount304 = new System.Windows.Forms.Label();
            this.label304 = new System.Windows.Forms.Label();
            this.labelCount200 = new System.Windows.Forms.Label();
            this.labelCountRequest = new System.Windows.Forms.Label();
            this.labelRequest = new System.Windows.Forms.Label();
            this.labeCountRequestSec = new System.Windows.Forms.Label();
            this.labelRequestSec = new System.Windows.Forms.Label();
            this.labelCountThreads = new System.Windows.Forms.Label();
            this.labelThreads = new System.Windows.Forms.Label();
            this.listBoxUrls = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.txtProxyHost = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProxyPass = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtProxyUser = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkProxy = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtAuthPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAuthUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkAutenticacion = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRequest)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonRun
            // 
            this.ButtonRun.Location = new System.Drawing.Point(6, 360);
            this.ButtonRun.Name = "ButtonRun";
            this.ButtonRun.Size = new System.Drawing.Size(75, 23);
            this.ButtonRun.TabIndex = 0;
            this.ButtonRun.Text = "Start";
            this.ButtonRun.UseVisualStyleBackColor = true;
            this.ButtonRun.Click += new System.EventHandler(this.ButtonRun_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.Location = new System.Drawing.Point(87, 360);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(75, 23);
            this.ButtonStop.TabIndex = 1;
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(168, 360);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "Reset";
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
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Number of threads";
            // 
            // numThreads
            // 
            this.numThreads.Location = new System.Drawing.Point(130, 12);
            this.numThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreads.Name = "numThreads";
            this.numThreads.Size = new System.Drawing.Size(63, 20);
            this.numThreads.TabIndex = 5;
            this.numThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numRequest
            // 
            this.numRequest.Location = new System.Drawing.Point(130, 38);
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
            this.label2.Location = new System.Drawing.Point(6, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Number of request";
            // 
            // chkForever
            // 
            this.chkForever.AutoSize = true;
            this.chkForever.Location = new System.Drawing.Point(200, 40);
            this.chkForever.Name = "chkForever";
            this.chkForever.Size = new System.Drawing.Size(59, 17);
            this.chkForever.TabIndex = 8;
            this.chkForever.Text = "forever";
            this.chkForever.UseVisualStyleBackColor = true;
            this.chkForever.CheckedChanged += new System.EventHandler(this.chkForever_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "URLs";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(9, 87);
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
            this.groupBox1.Controls.Add(this.labelCountFailures);
            this.groupBox1.Controls.Add(this.labelFallos);
            this.groupBox1.Controls.Add(this.labelCount404);
            this.groupBox1.Controls.Add(this.label404);
            this.groupBox1.Controls.Add(this.labelCount401);
            this.groupBox1.Controls.Add(this.label401);
            this.groupBox1.Controls.Add(this.labelCount304);
            this.groupBox1.Controls.Add(this.label304);
            this.groupBox1.Controls.Add(this.labelCount200);
            this.groupBox1.Controls.Add(this.label200);
            this.groupBox1.Location = new System.Drawing.Point(424, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 106);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // labelCountFailures
            // 
            this.labelCountFailures.AutoSize = true;
            this.labelCountFailures.ForeColor = System.Drawing.Color.Red;
            this.labelCountFailures.Location = new System.Drawing.Point(106, 78);
            this.labelCountFailures.Name = "labelCountFailures";
            this.labelCountFailures.Size = new System.Drawing.Size(13, 13);
            this.labelCountFailures.TabIndex = 20;
            this.labelCountFailures.Text = "0";
            // 
            // labelFallos
            // 
            this.labelFallos.AutoSize = true;
            this.labelFallos.Location = new System.Drawing.Point(25, 78);
            this.labelFallos.Name = "labelFallos";
            this.labelFallos.Size = new System.Drawing.Size(43, 13);
            this.labelFallos.TabIndex = 19;
            this.labelFallos.Text = "Failures";
            // 
            // labelCount404
            // 
            this.labelCount404.AutoSize = true;
            this.labelCount404.Location = new System.Drawing.Point(106, 65);
            this.labelCount404.Name = "labelCount404";
            this.labelCount404.Size = new System.Drawing.Size(13, 13);
            this.labelCount404.TabIndex = 18;
            this.labelCount404.Text = "0";
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
            // labelCount401
            // 
            this.labelCount401.AutoSize = true;
            this.labelCount401.Location = new System.Drawing.Point(106, 52);
            this.labelCount401.Name = "labelCount401";
            this.labelCount401.Size = new System.Drawing.Size(13, 13);
            this.labelCount401.TabIndex = 16;
            this.labelCount401.Text = "0";
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
            // labelCount304
            // 
            this.labelCount304.AutoSize = true;
            this.labelCount304.Location = new System.Drawing.Point(106, 39);
            this.labelCount304.Name = "labelCount304";
            this.labelCount304.Size = new System.Drawing.Size(13, 13);
            this.labelCount304.TabIndex = 14;
            this.labelCount304.Text = "0";
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
            // labelCount200
            // 
            this.labelCount200.AutoSize = true;
            this.labelCount200.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCount200.ForeColor = System.Drawing.Color.Green;
            this.labelCount200.Location = new System.Drawing.Point(106, 26);
            this.labelCount200.Name = "labelCount200";
            this.labelCount200.Size = new System.Drawing.Size(14, 13);
            this.labelCount200.TabIndex = 12;
            this.labelCount200.Text = "0";
            // 
            // labelCountRequest
            // 
            this.labelCountRequest.AutoSize = true;
            this.labelCountRequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCountRequest.Location = new System.Drawing.Point(530, 210);
            this.labelCountRequest.Name = "labelCountRequest";
            this.labelCountRequest.Size = new System.Drawing.Size(14, 13);
            this.labelCountRequest.TabIndex = 14;
            this.labelCountRequest.Text = "0";
            // 
            // labelRequest
            // 
            this.labelRequest.AutoSize = true;
            this.labelRequest.Location = new System.Drawing.Point(449, 210);
            this.labelRequest.Name = "labelRequest";
            this.labelRequest.Size = new System.Drawing.Size(47, 13);
            this.labelRequest.TabIndex = 13;
            this.labelRequest.Text = "Request";
            // 
            // labeCountRequestSec
            // 
            this.labeCountRequestSec.AutoSize = true;
            this.labeCountRequestSec.Location = new System.Drawing.Point(530, 223);
            this.labeCountRequestSec.Name = "labeCountRequestSec";
            this.labeCountRequestSec.Size = new System.Drawing.Size(13, 13);
            this.labeCountRequestSec.TabIndex = 16;
            this.labeCountRequestSec.Text = "0";
            // 
            // labelRequestSec
            // 
            this.labelRequestSec.AutoSize = true;
            this.labelRequestSec.Location = new System.Drawing.Point(449, 223);
            this.labelRequestSec.Name = "labelRequestSec";
            this.labelRequestSec.Size = new System.Drawing.Size(71, 13);
            this.labelRequestSec.TabIndex = 15;
            this.labelRequestSec.Text = "Request/Sec";
            // 
            // labelCountThreads
            // 
            this.labelCountThreads.AutoSize = true;
            this.labelCountThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCountThreads.ForeColor = System.Drawing.Color.Blue;
            this.labelCountThreads.Location = new System.Drawing.Point(530, 253);
            this.labelCountThreads.Name = "labelCountThreads";
            this.labelCountThreads.Size = new System.Drawing.Size(14, 13);
            this.labelCountThreads.TabIndex = 18;
            this.labelCountThreads.Text = "0";
            // 
            // labelThreads
            // 
            this.labelThreads.AutoSize = true;
            this.labelThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelThreads.ForeColor = System.Drawing.Color.Blue;
            this.labelThreads.Location = new System.Drawing.Point(449, 253);
            this.labelThreads.Name = "labelThreads";
            this.labelThreads.Size = new System.Drawing.Size(53, 13);
            this.labelThreads.TabIndex = 17;
            this.labelThreads.Text = "Threads";
            // 
            // listBoxUrls
            // 
            this.listBoxUrls.FormattingEnabled = true;
            this.listBoxUrls.Location = new System.Drawing.Point(9, 114);
            this.listBoxUrls.Name = "listBoxUrls";
            this.listBoxUrls.Size = new System.Drawing.Size(366, 186);
            this.listBoxUrls.TabIndex = 19;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(300, 85);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 20;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(300, 306);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 21;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(671, 431);
            this.tabControl1.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSave);
            this.tabPage1.Controls.Add(this.btnLoadFile);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnRemove);
            this.tabPage1.Controls.Add(this.ButtonRun);
            this.tabPage1.Controls.Add(this.btnAdd);
            this.tabPage1.Controls.Add(this.ButtonStop);
            this.tabPage1.Controls.Add(this.listBoxUrls);
            this.tabPage1.Controls.Add(this.buttonReset);
            this.tabPage1.Controls.Add(this.labelCountThreads);
            this.tabPage1.Controls.Add(this.numThreads);
            this.tabPage1.Controls.Add(this.labelThreads);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.labeCountRequestSec);
            this.tabPage1.Controls.Add(this.numRequest);
            this.tabPage1.Controls.Add(this.labelRequestSec);
            this.tabPage1.Controls.Add(this.chkForever);
            this.tabPage1.Controls.Add(this.labelCountRequest);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.labelRequest);
            this.tabPage1.Controls.Add(this.txtUrl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(663, 405);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Stress";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(113, 306);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 23);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(9, 306);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(98, 23);
            this.btnLoadFile.TabIndex = 22;
            this.btnLoadFile.Text = "Load";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(663, 405);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Setting";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.numPort);
            this.groupBox3.Controls.Add(this.txtProxyHost);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtProxyPass);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtProxyUser);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.chkProxy);
            this.groupBox3.Location = new System.Drawing.Point(25, 190);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(357, 150);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Proxy";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(233, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Port";
            // 
            // numPort
            // 
            this.numPort.Enabled = false;
            this.numPort.Location = new System.Drawing.Point(286, 42);
            this.numPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(55, 20);
            this.numPort.TabIndex = 11;
            this.numPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // txtProxyHost
            // 
            this.txtProxyHost.Enabled = false;
            this.txtProxyHost.Location = new System.Drawing.Point(89, 42);
            this.txtProxyHost.Name = "txtProxyHost";
            this.txtProxyHost.Size = new System.Drawing.Size(137, 20);
            this.txtProxyHost.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Host";
            // 
            // txtProxyPass
            // 
            this.txtProxyPass.Enabled = false;
            this.txtProxyPass.Location = new System.Drawing.Point(89, 105);
            this.txtProxyPass.Name = "txtProxyPass";
            this.txtProxyPass.PasswordChar = '*';
            this.txtProxyPass.Size = new System.Drawing.Size(137, 20);
            this.txtProxyPass.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Password";
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Enabled = false;
            this.txtProxyUser.Location = new System.Drawing.Point(89, 79);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(137, 20);
            this.txtProxyUser.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "User";
            // 
            // chkProxy
            // 
            this.chkProxy.AutoSize = true;
            this.chkProxy.Location = new System.Drawing.Point(18, 19);
            this.chkProxy.Name = "chkProxy";
            this.chkProxy.Size = new System.Drawing.Size(69, 17);
            this.chkProxy.TabIndex = 1;
            this.chkProxy.Text = "Required";
            this.chkProxy.UseVisualStyleBackColor = true;
            this.chkProxy.CheckedChanged += new System.EventHandler(this.chkProxy_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtAuthPass);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtAuthUser);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.chkAutenticacion);
            this.groupBox2.Location = new System.Drawing.Point(25, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(357, 150);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Authentication";
            // 
            // txtAuthPass
            // 
            this.txtAuthPass.Enabled = false;
            this.txtAuthPass.Location = new System.Drawing.Point(89, 78);
            this.txtAuthPass.Name = "txtAuthPass";
            this.txtAuthPass.PasswordChar = '*';
            this.txtAuthPass.Size = new System.Drawing.Size(137, 20);
            this.txtAuthPass.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Password";
            // 
            // txtAuthUser
            // 
            this.txtAuthUser.Enabled = false;
            this.txtAuthUser.Location = new System.Drawing.Point(89, 52);
            this.txtAuthUser.Name = "txtAuthUser";
            this.txtAuthUser.Size = new System.Drawing.Size(137, 20);
            this.txtAuthUser.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "User";
            // 
            // chkAutenticacion
            // 
            this.chkAutenticacion.AutoSize = true;
            this.chkAutenticacion.Location = new System.Drawing.Point(18, 20);
            this.chkAutenticacion.Name = "chkAutenticacion";
            this.chkAutenticacion.Size = new System.Drawing.Size(69, 17);
            this.chkAutenticacion.TabIndex = 0;
            this.chkAutenticacion.Text = "Required";
            this.chkAutenticacion.UseVisualStyleBackColor = true;
            this.chkAutenticacion.CheckedChanged += new System.EventHandler(this.chkAutenticacion_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 431);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Url Stress";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRequest)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonRun;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numThreads;
        private System.Windows.Forms.NumericUpDown numRequest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkForever;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label200;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelCountFailures;
        private System.Windows.Forms.Label labelFallos;
        private System.Windows.Forms.Label labelCount404;
        private System.Windows.Forms.Label label404;
        private System.Windows.Forms.Label labelCount401;
        private System.Windows.Forms.Label label401;
        private System.Windows.Forms.Label labelCount304;
        private System.Windows.Forms.Label label304;
        private System.Windows.Forms.Label labelCount200;
        private System.Windows.Forms.Label labelCountRequest;
        private System.Windows.Forms.Label labelRequest;
        private System.Windows.Forms.Label labeCountRequestSec;
        private System.Windows.Forms.Label labelRequestSec;
        private System.Windows.Forms.Label labelCountThreads;
        private System.Windows.Forms.Label labelThreads;
        private System.Windows.Forms.ListBox listBoxUrls;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkProxy;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkAutenticacion;
        private System.Windows.Forms.TextBox txtAuthPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAuthUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProxyPass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtProxyUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtProxyHost;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnSave;
    }
}
