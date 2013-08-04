namespace GMailNotifier
{
    partial class frmNotifier
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNotifier));
            this.PanelPrincipal = new System.Windows.Forms.Panel();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbFrom = new System.Windows.Forms.Label();
            this.lbCount = new System.Windows.Forms.Label();
            this.lbSubject = new System.Windows.Forms.Label();
            this.ScrollInbox = new System.Windows.Forms.HScrollBar();
            this.lbSumary = new System.Windows.Forms.Label();
            this.lbIndex = new System.Windows.Forms.Label();
            this.PanelPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelPrincipal
            // 
            this.PanelPrincipal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelPrincipal.Controls.Add(this.lbSumary);
            this.PanelPrincipal.Controls.Add(this.PictureBox1);
            this.PanelPrincipal.Controls.Add(this.lbIndex);
            this.PanelPrincipal.Controls.Add(this.lbFrom);
            this.PanelPrincipal.Controls.Add(this.lbCount);
            this.PanelPrincipal.Controls.Add(this.ScrollInbox);
            this.PanelPrincipal.Controls.Add(this.lbSubject);
            resources.ApplyResources(this.PanelPrincipal, "PanelPrincipal");
            this.PanelPrincipal.Name = "PanelPrincipal";
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = global::GMailNotifier.Properties.Resources.gmail;
            resources.ApplyResources(this.PictureBox1, "PictureBox1");
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.TabStop = false;
            // 
            // lbFrom
            // 
            resources.ApplyResources(this.lbFrom, "lbFrom");
            this.lbFrom.Name = "lbFrom";
            // 
            // lbCount
            // 
            resources.ApplyResources(this.lbCount, "lbCount");
            this.lbCount.Name = "lbCount";
            // 
            // lbSubject
            // 
            resources.ApplyResources(this.lbSubject, "lbSubject");
            this.lbSubject.Name = "lbSubject";
            // 
            // ScrollInbox
            // 
            resources.ApplyResources(this.ScrollInbox, "ScrollInbox");
            this.ScrollInbox.Maximum = 9999;
            this.ScrollInbox.Name = "ScrollInbox";
            this.ScrollInbox.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollInbox_Scroll);
            // 
            // lbSumary
            // 
            resources.ApplyResources(this.lbSumary, "lbSumary");
            this.lbSumary.Name = "lbSumary";
            // 
            // lbIndex
            // 
            resources.ApplyResources(this.lbIndex, "lbIndex");
            this.lbIndex.Name = "lbIndex";
            // 
            // frmNotifier
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ControlBox = false;
            this.Controls.Add(this.PanelPrincipal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmNotifier";
            this.Opacity = 0.95D;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmNotifier_Load);
            this.PanelPrincipal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelPrincipal;
        internal System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.Label lbIndex;
        private System.Windows.Forms.Label lbFrom;
        private System.Windows.Forms.Label lbSumary;
        private System.Windows.Forms.Label lbCount;
        private System.Windows.Forms.HScrollBar ScrollInbox;
        private System.Windows.Forms.Label lbSubject;

    }
}