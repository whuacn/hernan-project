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
        Timer timerCheck;
        Timer timerHide = new Timer();
        public frmNotifier()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);

            int borderWidth = 1;
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.Red, borderWidth, ButtonBorderStyle.Outset,
                Color.Red, borderWidth, ButtonBorderStyle.Outset,
                Color.Red, borderWidth, ButtonBorderStyle.Inset,
                Color.Red, borderWidth, ButtonBorderStyle.Inset);
        }


        private void frmNotifier_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.Left = (SystemInformation.WorkingArea.Size.Width - Size.Width);
            this.Top = (SystemInformation.WorkingArea.Size.Height - Size.Height);
            this.TopLevel = true;
            this.TopMost = true;
            Init();
        }

        void Init()
        {
            SysTrayApp();
            MouseHoverControls(this);

            svrNotifier.OnUpdate += new EventHandler(svrNotifier_OnUpdate);
            svrNotifier.CheckUpdate();

            timerCheck = new Timer();
            timerCheck.Interval = 15000;
            timerCheck.Tick += new EventHandler(timer_Tick);
            timerCheck.Start();
        }

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
            if (!this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
            {
                ShowForm();
            }
            
        }
        void Application_MouseHover(object sender, EventArgs e)
        {

            if (this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
            {
                try
                {
                    timerHide.Stop();
                }
                catch (Exception)
                {
                }
            }
        }


        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        public void SysTrayApp()
        {
            
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Gmail Notifier";
            trayIcon.Icon = new Icon(GMailNotifier.Properties.Resources.gmail_ico, 40, 40);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible     = true;
            trayIcon.Click += new EventHandler(trayIcon_Click);
        }

        void trayIcon_Click(object sender, EventArgs e)
        {
            ShowForm();
        }
 
        protected override void OnLoad(EventArgs e)
        {
            Visible       = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
 
            base.OnLoad(e);
        }
 
        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
 
        void timer_Tick(object sender, EventArgs e)
        {
            svrNotifier.CheckUpdate();
        }

        void svrNotifier_OnUpdate(object sender, EventArgs e)
        {
            lbCount.Text = Program.inbox.fullcount.ToString();
            ScrollInbox.Maximum = Program.inbox.fullcount;
            ScrollInbox.Minimum = 1;

            for (int i = 0; i < Program.inbox.entries.Count; i++)
            {
                if (Program.inbox.entries[i].Notify)
                {
                    CompleteText(i);
                    ScrollInbox.Value = i+1;
                    ShowForm();
                    break;
                }
            }

        }

        private void ScrollInbox_Scroll(object sender, ScrollEventArgs e)
        {
            CompleteText(e.NewValue - 1);
        }

        void CompleteText(int index)
        {
            Entry entry = Program.inbox.entries[index];
            lbFrom.Text = entry.author.name;
            lbSubject.Text = entry.title;
            lbSumary.Text = entry.summary;
            lbIndex.Text = (index + 1).ToString() + " /";
        }

        void timerhide_Tick(object sender, EventArgs e)
        {
            this.Hide();
            timerHide.Stop();
        }
        void ShowForm()
        {
            try
            {
                timerHide.Stop();
            }
            catch (Exception)
            {
            }
            
            this.Show();
            timerHide.Interval = 5000;
            timerHide.Tick += new EventHandler(timerhide_Tick);
            timerHide.Start();
        }

    }
}
