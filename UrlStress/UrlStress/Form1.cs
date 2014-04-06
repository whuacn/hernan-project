using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UrlStress
{
    public partial class Form1 : Form
    {

        static bool _shouldStop = false;
        static int numWorkItems = 0;
        static int requests = 0;
        static int requestsLastSec = 0;
        static int sec = 0;
        static int count200 = 0;
        static int count401 = 0;
        static int count404 = 0;
        static int count304 = 0;
        static int countFailures = 0;
        static int CantThreads = 0;
        static int CantRequest = 0;
        static bool isforever = false;
        
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ButtonRun_Click(object sender, EventArgs e)
        {
            ButtonRun.Enabled = false;
            ButtonStop.Enabled = true;
            buttonReset.Enabled = true;
            _shouldStop = false;
            sec = 0;
            requests = 0;
            count200 = 0;
            count401 = 0;
            count404 = 0;
            count304 = 0;
            countFailures = 0;
            CantThreads = Int32.Parse(intHilos.Value.ToString());
            CantRequest = Int32.Parse(numRequest.Value.ToString());
            isforever = chkForever.Checked;

            foreach (string url in listBoxUrls.Items)
            {
                for (int i = 1; i <= CantThreads; i++)
                {
                    Thread workerThread = new Thread(() => WorkerThreadProc(url));
                    workerThread.Start();
                }
            }
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            sec = sec + 1;
            labelCantRequest.Text = requests.ToString();
            labeCantlRequestSec.Text = Convert.ToDecimal(requests / sec).ToString();
            labelCant200.Text = count200.ToString();
            labelCant401.Text = count401.ToString();
            labelCant304.Text = count304.ToString();
            labelCant404.Text = count404.ToString();
            labelCantFallos.Text = countFailures.ToString();
            labelCantHilos.Text = numWorkItems.ToString();
            requestsLastSec = 0;

            if (numWorkItems == 0)
            {
                timer1.Stop();
                ButtonRun.Enabled = true;
                buttonReset.Enabled = false;
                ButtonStop.Enabled = false;

            }

        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            _shouldStop = true; 
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            requests = 0;
            sec = 0;
            count200 = 0;
            countFailures = 0;
        }

        public void WorkerThreadProc(string url)
        {
            Interlocked.Increment(ref numWorkItems);
            int tries = 0;
            while ((!_shouldStop) && ((tries < CantRequest || (isforever))))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = true;
                try
                {
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK) Interlocked.Increment(ref count200);
                    if (response.StatusCode == HttpStatusCode.Unauthorized) Interlocked.Increment(ref count401);
                    if (response.StatusCode == HttpStatusCode.NotFound) Interlocked.Increment(ref count404);
                    if (response.StatusCode == HttpStatusCode.NotModified) Interlocked.Increment(ref count304);
                    response.Close();
                }
                catch
                {
                    Interlocked.Increment(ref countFailures);
                }

                tries++;
                Interlocked.Increment(ref requests);
                Interlocked.Increment(ref requestsLastSec);
            }
            Interlocked.Decrement(ref numWorkItems);
        }

        private void chkForever_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUrl.Text.Trim() != "")
                listBoxUrls.Items.Add(txtUrl.Text);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxUrls.Items.Remove(listBoxUrls.SelectedItem);
            }
            catch (Exception)
            {
            }            
        }
    }
}
