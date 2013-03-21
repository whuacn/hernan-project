using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Management;

namespace SiproBlock
{
    /*
     secpol.msc
    gpedit.msc
     */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Block(txtMachine.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnBlock(txtMachine.Text);
        }


        private void Block(string machine)
        {
            ExecuteRemoteCommand("netsh ipsec static add policy name=ProxyDesa assign=yes", machine);
            ExecuteRemoteCommand("netsh ipsec static add filterlist name=IPProxy", machine);
            ExecuteRemoteCommand("netsh ipsec static add filter filterlist=IPProxy srcaddr=Me dstaddr=any protocol=TCP srcport=0 dstport=8080", machine);
            ExecuteRemoteCommand("netsh ipsec static add filteraction name=blockIPProxy action=block", machine);
            ExecuteRemoteCommand("netsh ipsec static add rule name=myrule policy=ProxyDesa filterlist=IPProxy filteraction=blockIPProxy", machine);
        }

        private void UnBlock(string machine)
        {
            ExecuteRemoteCommand("netsh ipsec static set policy name=ProxyDesa assign=no", machine);
            ExecuteRemoteCommand("netsh ipsec static delete policy name=ProxyDesa", machine);
            ExecuteRemoteCommand("netsh ipsec static delete filterlist name=IPProxy", machine);
            ExecuteRemoteCommand("netsh ipsec static add filteraction name=blockIPProxy action=block", machine);
            ExecuteRemoteCommand("netsh ipsec static delete filteraction name=blockIPProxy", machine);
        }


        void ExecuteRemoteCommand(object command, string machine)
        {
            var processToRun = new[] { command };
            var connection = new ConnectionOptions();
            connection.Impersonation = ImpersonationLevel.Impersonate;
            connection.EnablePrivileges = true;
            //connection.Username = "username";
            //connection.Password = "password";
            var wmiScope = new ManagementScope(String.Format("\\\\{0}\\root\\cimv2", machine), connection);
            var wmiProcess = new ManagementClass(wmiScope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
            wmiProcess.InvokeMethod("Create", processToRun);
        }

        private void Block_local()
        {
            ExecuteCommandSync("netsh ipsec static add policy name=ProxyDesa assign=yes");
            ExecuteCommandSync("netsh ipsec static add filterlist name=IPProxy");
            ExecuteCommandSync("netsh ipsec static add filter filterlist=IPProxy srcaddr=93.189.36.83 dstaddr=Me protocol=any srcport=0 dstport=0");
            ExecuteCommandSync("netsh ipsec static add filteraction name=blockIPProxy action=block");
            ExecuteCommandSync("netsh ipsec static add rule name=myrule policy=ProxyDesa filterlist=IPProxy filteraction=blockIPProxy");
        }

        private void UnBlock_local()
        {
            ExecuteCommandSync("netsh ipsec static set policy name=ProxyDesa assign=no");
            ExecuteCommandSync("netsh ipsec static delete policy name=ProxyDesa");
            ExecuteCommandSync("netsh ipsec static delete filterlist name=IPProxy");
            ExecuteCommandSync("netsh ipsec static add filteraction name=blockIPProxy action=block");
            ExecuteCommandSync("netsh ipsec static delete filteraction name=blockIPProxy");
        }
        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command.</returns>
        public void ExecuteCommandSync(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                procStartInfo.Verb = "runas";
                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                // Log the exception
            }
        }

        /// <summary>
        /// Execute the command Asynchronously.
        /// </summary>
        /// <param name="command">string command.</param>
        public void ExecuteCommandAsync(string command)
        {
            try
            {
                //Asynchronously start the Thread to process the Execute command request.
                Thread objThread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync));
                //Make the thread as background thread.
                objThread.IsBackground = true;
                //Set the Priority of the thread.
                objThread.Priority = ThreadPriority.AboveNormal;
                //Start the thread.
                objThread.Start(command);
            }
            catch (ThreadStartException objException)
            {
                // Log the exception
            }
            catch (ThreadAbortException objException)
            {
                // Log the exception
            }
            catch (Exception objException)
            {
                // Log the exception
            }
        }




    }
}
