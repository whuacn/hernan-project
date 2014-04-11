using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            Settings.CountThreads = Int32.Parse(numThreads.Value.ToString());
            Settings.CountRequest = Int32.Parse(numRequest.Value.ToString());
            Settings.isforever = chkForever.Checked;

            Settings.autenticate = chkAutenticacion.Checked;
            Settings.authUser = txtAuthUser.Text;
            Settings.authPass = txtAuthPass.Text;

            Settings.proxy = chkProxy.Checked;
            Settings.proxyPort = Int32.Parse(numPort.Value.ToString());
            Settings.proxyHost = txtProxyHost.Text;
            Settings.proxyUser = txtProxyUser.Text;
            Settings.proxyPass = txtProxyPass.Text;

            foreach (string url in listBoxUrls.Items)
            {
                for (int i = 1; i <= Settings.CountThreads; i++)
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
            while ((!_shouldStop) && ((tries < Settings.CountRequest || (Settings.isforever))))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = true;
                try
                {
                    request.Method = "GET";

                    if (Settings.autenticate)
                    {
                        var cache = new CredentialCache();
                        cache.Add(new Uri(url), "Basic", new NetworkCredential(Settings.authUser, Settings.authPass));
                        request.Credentials = cache;

                    }

                    if (Settings.proxy)
                    {
                        WebProxy myproxy = new WebProxy(Settings.proxyHost, Settings.proxyPort);
                        if (Settings.proxyUser != "")
                            myproxy.Credentials = new NetworkCredential(Settings.proxyUser, Settings.proxyPass);
                        request.Proxy = myproxy;
                    }

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
            try
            {
                string url = new UriBuilder(txtUrl.Text).Uri.ToString();
                if (txtUrl.Text.Trim() != "")
                    listBoxUrls.Items.Add(url);
            }
            catch (Exception)
            {
                MessageBox.Show("Url invalid: " + txtUrl.Text);
            }

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

        private void chkAutenticacion_CheckedChanged(object sender, EventArgs e)
        {
                txtAuthUser.Enabled = chkAutenticacion.Checked;
                txtAuthPass.Enabled = chkAutenticacion.Checked;
        }

        private void chkProxy_CheckedChanged(object sender, EventArgs e)
        {
            numPort.Enabled = chkProxy.Checked;
            txtProxyHost.Enabled = chkProxy.Checked;
            txtProxyUser.Enabled = chkProxy.Checked;
            txtProxyPass.Enabled = chkProxy.Checked;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();            
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] filelines = File.ReadAllLines(openFileDialog1.FileName);
                    foreach (string line in filelines)
                    {
                        string url = new UriBuilder(line).Uri.ToString();
                        listBoxUrls.Items.Add(url);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Can not read the selected file. Details: " + ex.Message);
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile()))
                    {
                        for (int i = 0; i < listBoxUrls.Items.Count; i++)
                        {
                            writer.WriteLine(listBoxUrls.Items[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Unable to write the selected file. Details: " + ex.Message);
                }
                
            }
        }
    }
}
