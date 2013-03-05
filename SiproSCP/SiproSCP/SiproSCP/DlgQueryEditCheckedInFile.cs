/***************************************************************************

Copyright (c) Hernán Javier Hegykozi. All rights reserved.

***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sipro.SourceControlProvider.SiproSCP
{
    public partial class DlgQueryEditCheckedInFile : Form
    {
        public const int qecifCheckout = 1;
        public const int qecifEditInMemory = 2;
        public const int qecifCancelEdit = 3;

        int _answer = qecifCancelEdit;
        
        public int Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }

        public DlgQueryEditCheckedInFile(string filename)
        {
            InitializeComponent();

            // Format the message text with the current file name
            msgText.Text = String.Format(CultureInfo.CurrentUICulture, msgText.Text, filename);
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            Answer = qecifCheckout;
            Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Answer = qecifEditInMemory;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Answer = qecifCancelEdit;
            Close();
        }
    }
}