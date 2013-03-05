namespace Sipro.SourceControlProvider.SiproSCP
{
    partial class ViewHistoryFile
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
            this.lbFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbFile
            // 
            this.lbFile.AutoSize = true;
            this.lbFile.Location = new System.Drawing.Point(13, 13);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(0, 13);
            this.lbFile.TabIndex = 0;
            // 
            // ViewHistoryFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lbFile);
            this.Name = "ViewHistoryFile";
            this.Text = "ViewHistoryFile";
            this.Load += new System.EventHandler(this.ViewHistoryFile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbFile;
    }
}