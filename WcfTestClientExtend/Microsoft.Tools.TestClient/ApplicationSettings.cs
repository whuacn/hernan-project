using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient
{
	internal class ApplicationSettings
	{
		private const string DisableRefreshPrompt = "RefreshPrompt : Disabled";

		private const string DisableRegenerateConfig = "RegenerateConfig : Disabled";

		private const string DisableSecurityPrompt = "SecurityPrompt : Disabled";

		private const string EnableRefreshPrompt = "RefreshPrompt : Enabled";

		private const string EnableRegenerateConfig = "RegenerateConfig : Enabled";

		private const string EnableSecurityPrompt = "SecurityPrompt : Enabled";

		private const int maxRecentUrlsCount = 10;

		private const string projectBaseTitle = "ProjectBase:";

		private static ApplicationSettings instance;

		private string projectBase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), string.Concat("Temp\\", ToolingEnvironment.TestClientProjectFolderName));

		private LinkedList<string> recentUrls = new LinkedList<string>();

		private bool refreshPromptEnabled = true;

		private Regex refreshRegex = new Regex("^\\s*RefreshPrompt\\s*:\\s*(En|Dis)abled\\s*$", RegexOptions.IgnoreCase);

		private bool regenerateConfigEnabled = true;

		private Regex regenerateRegex = new Regex("^\\s*RegenerateConfig\\s*:\\s*(En|Dis)abled\\s*$", RegexOptions.IgnoreCase);

		private bool securityPromptEnabled = true;

		private Regex securityRegex = new Regex("^\\s*SecurityPrompt\\s*:\\s*(En|Dis)abled\\s*$", RegexOptions.IgnoreCase);

		private string settingPath;

		internal string ProjectBase
		{
			get
			{
				if (!Directory.Exists(this.projectBase))
				{
					try
					{
						Directory.CreateDirectory(this.projectBase);
					}
					catch (IOException oException)
					{
						ApplicationSettings.ReportCreateFolderError(this.projectBase);
					}
					catch (UnauthorizedAccessException unauthorizedAccessException)
					{
						ApplicationSettings.ReportCreateFolderError(this.projectBase);
					}
				}
				return this.projectBase;
			}
			set
			{
				this.projectBase = value;
				this.WriteSettingFile(true);
			}
		}

		internal ICollection<string> RecentUrls
		{
			get
			{
				return this.recentUrls;
			}
		}

		internal bool RefreshPromptEnabled
		{
			get
			{
				return this.refreshPromptEnabled;
			}
			set
			{
				this.refreshPromptEnabled = value;
				this.WriteSettingFile(true);
			}
		}

		internal bool RegenerateConfigEnabled
		{
			get
			{
				return this.regenerateConfigEnabled;
			}
			set
			{
				this.regenerateConfigEnabled = value;
				this.WriteSettingFile(true);
			}
		}

		internal bool SecurityPromptEnabled
		{
			get
			{
				return this.securityPromptEnabled;
			}
			set
			{
				this.securityPromptEnabled = value;
				this.WriteSettingFile(true);
			}
		}

		private ApplicationSettings()
		{
			string location = Assembly.GetExecutingAssembly().Location;
			this.settingPath = string.Concat(Path.GetFileName(location), ".option");
			this.settingPath = Path.Combine(ToolingEnvironment.SavedDataBase, this.settingPath);
			if (File.Exists(this.settingPath))
			{
				this.ReadSettingFile();
				return;
			}
			this.CreateSettingFile();
		}

		private void CreateSettingFile()
		{
			try
			{
				File.CreateText(this.settingPath).Close();
			}
			catch (IOException oException)
			{
				ApplicationSettings.ReportAccessError();
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
				ApplicationSettings.ReportAccessError();
			}
		}

		internal static ApplicationSettings GetInstance()
		{
			if (ApplicationSettings.instance == null)
			{
				ApplicationSettings.instance = new ApplicationSettings();
			}
			return ApplicationSettings.instance;
		}

		private static string ReadLine(StreamReader reader)
		{
			string str = null;
			while (string.IsNullOrEmpty(str) && !reader.EndOfStream)
			{
				str = reader.ReadLine().Trim();
			}
			return str;
		}

		private void ReadProjectBaseOrUrl(StreamReader reader)
		{
			string str = ApplicationSettings.ReadLine(reader);
			if (string.IsNullOrEmpty(str))
			{
				return;
			}
			if (str.StartsWith("ProjectBase:", StringComparison.OrdinalIgnoreCase))
			{
				str = str.Substring("ProjectBase:".Length).Trim();
				if (!string.IsNullOrEmpty(str) && Directory.Exists(str))
				{
					this.projectBase = str;
					return;
				}
			}
			else if (Uri.IsWellFormedUriString(str, UriKind.Absolute))
			{
				this.recentUrls.AddLast(str);
			}
		}

		private void ReadSettingFile()
		{
			try
			{
				using (StreamReader streamReader = File.OpenText(this.settingPath))
				{
					if (!streamReader.EndOfStream)
					{
						this.ReadValueFromFile(streamReader, this.securityRegex, out this.securityPromptEnabled);
						this.ReadValueFromFile(streamReader, this.refreshRegex, out this.refreshPromptEnabled);
						this.ReadValueFromFile(streamReader, this.regenerateRegex, out this.regenerateConfigEnabled);
					}
					this.ReadProjectBaseOrUrl(streamReader);
					while (!streamReader.EndOfStream)
					{
						string str = streamReader.ReadLine();
						if (!Uri.IsWellFormedUriString(str, UriKind.Absolute))
						{
							continue;
						}
						this.recentUrls.AddLast(str);
					}
				}
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
			}
		}

		private void ReadValueFromFile(StreamReader reader, Regex regex, out bool enabled)
		{
			enabled = true;
			string str = ApplicationSettings.ReadLine(reader);
			if (string.IsNullOrEmpty(str))
			{
				return;
			}
			Match match = regex.Match(str);
			if (match.Success)
			{
				enabled = match.Groups[1].ToString().StartsWith("E", StringComparison.OrdinalIgnoreCase);
			}
		}

		internal void RecordUrl(string pathToRecord)
		{
			foreach (string recentUrl in this.recentUrls)
			{
				if (string.Compare(recentUrl, pathToRecord, StringComparison.OrdinalIgnoreCase) != 0)
				{
					continue;
				}
				this.recentUrls.Remove(pathToRecord);
				break;
			}
			this.recentUrls.AddFirst(pathToRecord);
			if (this.recentUrls.Count > 10)
			{
				this.recentUrls.RemoveLast();
			}
			this.WriteSettingFile(false);
		}

		private static void ReportAccessError()
		{
			RtlAwareMessageBox.Show(StringResources.ErrorSettingsNotAccessible, StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, 0);
		}

		internal static void ReportCreateFolderError(string folderName)
		{
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			string cannotCreateFolder = StringResources.CannotCreateFolder;
			object[] objArray = new object[] { folderName };
			string str = string.Format(currentUICulture, cannotCreateFolder, objArray);
			RtlAwareMessageBox.Show(str, StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, 0);
		}

		private void WriteSettingFile(bool reportError)
		{
			try
			{
				using (StreamWriter streamWriter = File.CreateText(this.settingPath))
				{
					streamWriter.WriteLine((this.securityPromptEnabled ? "SecurityPrompt : Enabled" : "SecurityPrompt : Disabled"));
					streamWriter.WriteLine((this.refreshPromptEnabled ? "RefreshPrompt : Enabled" : "RefreshPrompt : Disabled"));
					streamWriter.WriteLine((this.regenerateConfigEnabled ? "RegenerateConfig : Enabled" : "RegenerateConfig : Disabled"));
					streamWriter.WriteLine(string.Concat("ProjectBase:", this.projectBase));
					foreach (string recentUrl in this.recentUrls)
					{
						streamWriter.WriteLine(recentUrl);
					}
				}
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
				if (reportError)
				{
					ApplicationSettings.ReportAccessError();
				}
			}
		}
	}
}