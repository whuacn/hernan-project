#region Using

using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;

#endregion

namespace SiteMonitor
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
      InitializeWorker();
      lastUpdated.Text = string.Empty;
      this.Icon = notifyIcon1.Icon;

      Monitor.BeginProcessSite += new EventHandler<EventArgs>(Monitor_BeginProcessSite);
      Monitor.Error += new EventHandler<EventArgs>(Monitor_Error);

      notifyMenu.Items[0].Click += delegate { ShowWindow(); };
      notifyMenu.Items[1].Click += delegate { _Close = true; this.Close(); };      
      notifyIcon1.MouseDoubleClick += delegate { ShowWindow(); };

      grid.CellMouseClick += new DataGridViewCellMouseEventHandler(grid_CellMouseClick);
      updateInverval.SelectedIndexChanged += new EventHandler(updateInverval_SelectedIndexChanged);
      
      BindGrid();
      SetSelectedInterval();
    }    

    #region Private fields

    BackgroundWorker _Worker;
    private bool _Close = false;
    private StringCollection _ErrorMessages = new StringCollection();
    private static Timer _Timer = new Timer();

    #endregion

    #region Methods

    /// <summary>
    /// Minimizes the window when the close button is clicked.
    /// </summary>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      if (!_Close && e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        this.ShowInTaskbar = false;
        Opacity = 0;
      }
    }

    /// <summary>
    /// Shows the main form and sets focus on it.
    /// </summary>
    private void ShowWindow()
    {
      BindGrid();
      Opacity = 1;
      Activate();
      ShowInTaskbar = true;
    }

    /// <summary>
    /// Reads the status file and binds it to the DataGridView.
    /// </summary>
    private void BindGrid()
    {
      string status = StorageHelper.Load("status.txt");
      if (status != null)
      {
        grid.Rows.Clear();
        using (System.IO.StringReader reader = new System.IO.StringReader(status))
        {
          string line;
          while ((line = reader.ReadLine()) != null)
          {
            string[] args = line.Split(';');
            grid.Rows.Add(args[0], args[1], args[2], "Visit");
          }
        }
      }

      PaintStatus();
      progress.Visible = false;
    }

    /// <summary>
    /// Paints the status column green or read based on the Http status code.
    /// </summary>
    private void PaintStatus()
    {
      foreach (DataGridViewRow row in grid.Rows)
      {
        if (row.Cells[2].Value != null)
        {
          if (row.Cells[2].Value.ToString().Contains("200 "))
            row.Cells[2].Style.ForeColor = System.Drawing.Color.Green;
          else
            row.Cells[2].Style.ForeColor = System.Drawing.Color.Red;
        }
      }
    }

    /// <summary>
    /// Resets the progress bar and makes it ready for use.
    /// </summary>
    private void PrepareProgressBar()
    {
      progress.Step = 1;
      progress.Maximum = grid.Rows.Count;
      progress.Value = 0;
      progress.Visible = true;
    }

    /// <summary>
    /// Set the selected value in the drop down list.
    /// </summary>
    private void SetSelectedInterval()
    {
      foreach (string s in updateInverval.Items)
      {
        if (s.Replace(" minutes", string.Empty).Replace(" minute", string.Empty) == Monitor.UpdateInterval.ToString())
          updateInverval.Text = s;
      }
    }

    /// <summary>
    /// Saves the websites in the grid to Isolated Storage.
    /// </summary>
    private void SaveWebsites()
    {
      grid.EndEdit();
      StringBuilder sb = new StringBuilder();
      foreach (DataGridViewRow row in grid.Rows)
      {
        if (row.Cells[0].Value != null)
        {
          if (!row.Cells[0].Value.ToString().Contains("://"))
            row.Cells[0].Value = "http://" + row.Cells[0].Value;

          sb.Append(row.Cells[0].Value.ToString() + ";");

        }
      }

      StorageHelper.Save("websites.txt", sb.ToString());
    }

    #endregion

    #region Background Worker and Timer

    /// <summary>
    /// Starts the monitoring of the websites.
    /// </summary>
    public void StartMonitoring()
    {
      _Timer.Interval = Monitor.UpdateInterval * 1000 * 60;
      _Timer.Tick += delegate { UpdateGrid(); };
      _Timer.Enabled = true;
    }

    private void InitializeWorker()
    {
      _Worker = new BackgroundWorker();
      _Worker.WorkerReportsProgress = true;
      _Worker.DoWork += delegate { Monitor.Process(); };
      _Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_Worker_RunWorkerCompleted);
      _Worker.ProgressChanged += delegate { progress.PerformStep(); };
    }

    /// <summary>
    /// Update the status for the selected websites.
    /// </summary>
    private void UpdateGrid()
    {
      if (!_Worker.IsBusy)
      {
        _Timer.Stop();
        toolStripButton1.Enabled = false;
        PrepareProgressBar();
        _Worker.RunWorkerAsync();
      }
    }

    private void _Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {      
      toolStripButton1.Enabled = true;
      lastUpdated.Text = string.Format("Last updated {0}", Monitor.LastProcessed.ToString("HH:mm:ss"));

      if (Opacity == 0 && _ErrorMessages.Count > 0)
      {
        string error = string.Empty;
        foreach (string s in _ErrorMessages)
          error += s + Environment.NewLine;

        notifyIcon1.ShowBalloonTip(3000, "Site Monitor", error, ToolTipIcon.Warning);
        notifyIcon1.BalloonTipClicked += delegate { ShowWindow(); };
        Activate();
      }

      if (Opacity == 1)
        BindGrid();

      _ErrorMessages.Clear();
      _Timer.Start();
    }

    /// <summary>
    /// Report progress to show in the progress bar.
    /// </summary>
    private void Monitor_BeginProcessSite(object sender, EventArgs e)
    {
      if (_Worker.IsBusy)
      {
        toolStripButton1.Enabled = false;
        _Worker.ReportProgress(1);
      }
    }

    #endregion

    #region Event handlers

    private void updateInverval_SelectedIndexChanged(object sender, EventArgs e)
    {
      Monitor.UpdateInterval = int.Parse(updateInverval.Text.Replace(" minutes", string.Empty).Replace(" minute", string.Empty));
      _Timer.Stop();
      _Timer.Interval = Monitor.UpdateInterval * 1000 * 60;
      _Timer.Start();
    }

    /// <summary>
    /// Opens the website in the default browser
    /// </summary>
    private void grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex == 3)
      {
        System.Diagnostics.Process.Start(grid.Rows[e.RowIndex].Cells[0].Value.ToString());
      }
    }

    private void Monitor_Error(object sender, EventArgs e)
    {
      if (!_ErrorMessages.Contains(sender.ToString()))
        _ErrorMessages.Add(sender.ToString());
    }   

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
      SaveWebsites();
      UpdateGrid();
    }

    #endregion

  }
}