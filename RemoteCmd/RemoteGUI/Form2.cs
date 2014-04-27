using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace RemoteCmd
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            Thread objThread = new Thread(new ParameterizedThreadStart(ExecuteCommandAsync));
            objThread.IsBackground = true;
            objThread.Priority = ThreadPriority.AboveNormal;
            objThread.Start("notepad.exe");


        }

        public void ExecuteCommandAsync(object command)
        {
            ProcessWMI remote = new ProcessWMI();
            remote.ExecuteRemoteProcessWMI("HERNANDESKTOP", command.ToString());
        }
    }
}
