#region Using

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Timers;
using System.IO;
using System.Configuration;

#endregion

namespace SiteMonitor
{
  public static class Monitor
  {

    #region Private fields

    private static Regex _Regex = new Regex("<title>(.*)</title>");

    #endregion

    #region Properties

    private static int _UpdateInterval;
    /// <summary>
    /// Gets or sets the update interval in minutes.
    /// </summary>
    public static int UpdateInterval
    {
      get
      {
        if (_UpdateInterval == 0)
        {
          string interval = StorageHelper.Load("interval.txt");
          if (!string.IsNullOrEmpty(interval))
            _UpdateInterval = int.Parse(interval);
          else
            _UpdateInterval = 10;
        }

        return _UpdateInterval;
      }
      set
      {
        StorageHelper.Save("interval.txt", value.ToString());
        _UpdateInterval = value;
      }
    }

    private static DateTime _LastProcessed;
    /// <summary>
    /// Gets the last time the websites were processed.
    /// </summary>
    public static DateTime LastProcessed
    {
      get { return _LastProcessed; }
    }

    #endregion

    #region methods

    /// <summary>
    /// Processes the websites.
    /// </summary>
    public static void Process()
    {
      StringBuilder sb = new StringBuilder();
      foreach (string url in GetWebsites())
      {
        if (!string.IsNullOrEmpty(url))
        {
          OnBeginProcess(url);
          CheckUrl(url, sb);
        }
      }

      StorageHelper.Save("status.txt", sb.ToString());
      _LastProcessed = DateTime.Now;
    }

    /// <summary>
    /// Retrieves the selected websites from Isolated Storage.
    /// </summary>
    /// <returns></returns>
    private static string[] GetWebsites()
    {
      string websites = StorageHelper.Load("websites.txt");
      if (!string.IsNullOrEmpty(websites))
        return websites.Split(';');

      return new string[0];
    }

    /// <summary>
    /// Checks the status of the specified URL
    /// </summary>
    private static void CheckUrl(string url, StringBuilder sb)
    {
      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

      try
      {
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
          using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
          {
            string html = reader.ReadToEnd().Replace("\n", string.Empty);
            MatchCollection matches = _Regex.Matches(html);
            string title = "[no title found]";
            if (matches.Count > 0)
              title = matches[0].Groups[1].Value.Trim();

            sb.AppendLine(url + ";" + title + ";" + (int)response.StatusCode + " - " + response.StatusCode);
          }
        }
      }
      catch (WebException ex)
      {
        using (HttpWebResponse response = (HttpWebResponse)ex.Response)
        {
          string status = (int)HttpStatusCode.NotFound + " - " + HttpStatusCode.NotFound;
          if (response != null)
            status = response.StatusCode + " - " + response.StatusDescription;

          sb.AppendLine(url + ";n/a;" + status);
          OnError(url + ": " + status);
        }
      }
    }

    #endregion

    #region Events

    public static event EventHandler<EventArgs> BeginProcessSite;
    /// <summary>
    /// Occurs when the class is BeginProcess
    /// </summary>
    private static void OnBeginProcess(string url)
    {
      if (BeginProcessSite != null)
      {
        BeginProcessSite(url, new EventArgs());
      }
    }

    public static event EventHandler<EventArgs> Error;
    /// <summary>
    /// Occurs when the class is Error
    /// </summary>
    private static void OnError(string url)
    {
      if (Error != null)
      {
        Error(url, new EventArgs());
      }
    }

    #endregion

  }
}
