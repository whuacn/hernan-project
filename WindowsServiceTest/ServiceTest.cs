using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsServiceTest
{
    public partial class ServiceTest : ServiceBase
    {
        private System.Timers.Timer timer;
        public ServiceTest()
        {
            InitializeComponent();
            timer = new Timer(30000D);  //30000 milliseconds = 30 seconds
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(timer_elasped); //function that will be called after 30 
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                timer.Start();
            }
            catch (Exception ex)
            {
                //log anywhere
            }
        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        private void timer_elasped(object sender, ElapsedEventArgs e)
        {
            //it will continue to be called after 30 second intervals
            //do some operation that you want to perform through this service here
        }
    }
}
