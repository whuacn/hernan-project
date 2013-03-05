
/***************************************************************************

Copyright (c) Hernán Javier Hegykozi. All rights reserved.

***************************************************************************/

using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using IServiceProvider = System.IServiceProvider;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Sipro.SourceControlProvider.SiproSCP
{
    /// <summary>
    /// Summary description for SiproSCPToolWindowControl.
    /// </summary>
    public class SiproSCPToolWindowControl : System.Windows.Forms.UserControl
    {
        private Label label1;

        public SiproSCPToolWindowControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary> 
        /// Let this control process the mnemonics.
        /// </summary>
        protected override bool ProcessDialogChar(char charCode)
        {
              // If we're the top-level form or control, we need to do the mnemonic handling
              if (charCode != ' ' && ProcessMnemonic(charCode))
              {
                    return true;
              }
              return base.ProcessDialogChar(charCode);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiproSCPToolWindowControl));
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // SiproSCPToolWindowControl
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label1);
            this.Name = "SiproSCPToolWindowControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
