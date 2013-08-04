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
        Timer timer;
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

            svrNotifier.OnUpdate += new EventHandler(svrNotifier_OnUpdate);
            svrNotifier.CheckUpdate();

            timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            svrNotifier.CheckUpdate();
        }

        void svrNotifier_OnUpdate(object sender, EventArgs e)
        {
            lbCount.Text = Program.inbox.fullcount.ToString();

            for (int i = 0; i < Program.inbox.entries.Count; i++)
            {
                if (Program.inbox.entries[i].Notify)
                {
                    CompleteText(i);
                    break;
                }
            }
            ScrollInbox.Maximum = Program.inbox.fullcount;
            ScrollInbox.Minimum = 1;
            this.Show();
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

    }
}
