using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMailNotifier.Engine;
using System.Runtime.InteropServices;

namespace GMailNotifier
{
    public partial class frmNotifier : Form
    {

        public frmNotifier()
        {
            InitializeComponent();
        }

        private void frmNotifier_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.Left = (SystemInformation.WorkingArea.Size.Width - Size.Width);
            this.Top = (SystemInformation.WorkingArea.Size.Height - Size.Height);
            this.TopLevel = true;
            this.TopMost = true;

            string user = Storage.getSetting("UID");
            if (user == null)
            {
                frmLogin login = new frmLogin();
                while (true)
                {
                    DialogResult result = login.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        Init();
                        break;
                    }

                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        if (MessageBox.Show("¿Desea salir de Gmail Notifier?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            this.BeginInvoke(new MethodInvoker(this.Close));
                            break;
                         
                        }
                    }

                }
            }
            else
            {
                Init();
            }
            
            
        }

        void Init()
        {
            SysTrayApp();
            MouseHoverControls(this);

            svrNotifier.OnUpdate += new EventHandler(svrNotifier_OnUpdate);

            timerCheck = new Timer();
            timerCheck.Interval = 15000;
            timerCheck.Tick += new EventHandler(timerCheck_Tick);
            timerCheck.Enabled = true;

            timerHide = new Timer();
            timerHide.Interval = 5000;
            timerHide.Tick += new EventHandler(timerhide_Tick);
            timerHide.Enabled = true;

            timerCheck_Tick(null, null);
        }

        #region Mouse
        void MouseHoverControls(Control ctrl)
        {
            try
            {
                ctrl.MouseHover += new EventHandler(Application_MouseHover);
                ctrl.MouseLeave += new EventHandler(Application_MouseLeave);
            }
            catch (Exception)
            {
            }
            foreach (Control c in ctrl.Controls)
            {
                MouseHoverControls(c);
            }
        }
        void Application_MouseLeave(object sender, EventArgs e)
        {
            if (!MouseHoverApplication())
            {
                ShowForm();
            }

        }
        void Application_MouseHover(object sender, EventArgs e)
        {

            if (MouseHoverApplication())
            {
                try
                {
                    this.Opacity = 100;
                }
                catch (Exception)
                {
                }
            }
        }
        bool MouseHoverApplication()
        {
            if (this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
                return true;
            else
                return false;
        }
        #endregion

        #region Tray
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        public void SysTrayApp()
        {

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
            trayMenu.MenuItems.Add("Login", OnLogin);
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Gmail Notifier";
            trayIcon.Icon = new Icon(GMailNotifier.Properties.Resources.gmail_ico, 40, 40);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.Click += new EventHandler(trayIcon_Click);
        }
        void trayIcon_Click(object sender, EventArgs e)
        {
            ShowForm();
        }
        private void OnLogin(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.ShowDialog();
        }
        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Dispose();
            Application.Exit();
        }
        #endregion

        #region Timers
        Timer timerCheck;
        Timer timerHide;
        void timerCheck_Tick(object sender, EventArgs e)
        {
            try
            {
                svrNotifier.CheckUpdate();
                timerCheck.Interval = 15000;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gmail Notifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                timerCheck.Interval = 60000;
            }
        }

        void timerhide_Tick(object sender, EventArgs e)
        {
            if (this.Visible && !MouseHoverApplication())
            {
                this.Hide();
            }
            else
            {
                ResetTimerHide();
            }
        }
        void ResetTimerHide()
        {
            try
            {
                timerHide.Enabled = false;
                timerHide.Enabled = true;
            }
            catch (Exception)
            {

            }

        }
        #endregion

        #region events
        void svrNotifier_OnUpdate(object sender, EventArgs e)
        {
            if (Program.inbox == null)
                return;

            trayIcon.Text = String.Format("Gmail Notifier ({0})", Program.inbox.fullcount.ToString());
            lbCount.Text = Program.inbox.fullcount.ToString();
            ScrollInbox.Maximum = Program.inbox.fullcount;
            ScrollInbox.Minimum = 1;

            for (int i = 0; i < Program.inbox.entries.Count; i++)
            {
                if (Program.inbox.entries[i].Notify)
                {
                    CompleteText(i);
                    ScrollInbox.Value = i + 1;
                    ResetTimerHide();
                    ShowForm();
                    break;
                }
            }

        }

        private void lbSubject_LinkClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(((Label)sender).Tag as string);
        }

        private void ScrollInbox_Scroll(object sender, ScrollEventArgs e)
        {
            CompleteText(e.NewValue - 1);
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        #endregion

        void CompleteText(int index)
        {
            Entry entry = Program.inbox.entries[index];
            lbFrom.Text = entry.author.name;
            lbSubject.Text = entry.title;
            lbSubject.Tag = entry.link;
            lbSumary.Text = entry.summary;
            lbIndex.Text = (index + 1).ToString() + " /";
        }

        void ShowForm()
        {
            if (Program.inbox == null)
                return;
            if (Program.inbox.entries.Count == 0)
                return;

            this.Opacity = 100;

            this.Show();
        }

    }
}
