/************************************************ 2014 Pete_H *******************************************************
 * 
 * This software released under the Code Project Open License. Refer to: http://www.codeproject.com/info/cpol10.aspx
 * or refer to the copy of the Code Project Open License (CPOL.htm) included with this solution. 
 * 
 * This code and the compiled components including libraries and the demonstration application have been made 
 * available only for the purpose of learning, sharing and demonstrating ideas and NOT to imply, recommend or 
 * suggest usage of any part of the code or components.
 * 
 * No claim of suitability, guarantee, or any warranty whatsoever is provided. The software is provided "as-is"
 * Usage of any of this code or components is entirely at your own risk.
 * 
 ********************************************************************************************************************/
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace SqlTraceExpressUI
{

	public class ClientSettings
	{
		private static readonly string _settingsfileName = Application.StartupPath + @"\" +
					Application.ProductName + "_ClientSettings.xml";

		// no public constructor,  force using this static instance.  i.e. a singleton
		private static ClientSettings _theOneSettingsInstance = null;

		private ClientSettings()
		{
			TraceDefinitions = new List<UiTraceDefinition>();
			WindowPosition = new Point(0, 0);
			WindowSize = new Size(0, 0);
		}

		public string FontFamilyName { get; set; }

		public float FontSize { get; set; }

		public List<UiTraceDefinition> TraceDefinitions { get; set; }

		public Point WindowPosition { get; set; }

		public Size WindowSize { get; set; }

		public static ClientSettings GetSavedSettings()
		{
			if (_theOneSettingsInstance == null)
			{
				_theOneSettingsInstance = loadSettingsfromFile();
			}

			return _theOneSettingsInstance;
		}

		public bool Save()
		{
			try
			{
				using (FileStream fs = File.Open(_settingsfileName, FileMode.Create))
				{
					XmlSerializer xSerializer = new XmlSerializer(this.GetType());
					xSerializer.Serialize(fs, this);
				}
				return true;
			}
			catch { }
			return false;
		}

		private static ClientSettings loadSettingsfromFile()
		{
			ClientSettings settings = new ClientSettings();
			try
			{
				using (FileStream fs = File.Open(_settingsfileName, FileMode.Open))
				{
					XmlSerializer xSerializer = new XmlSerializer(settings.GetType());
					XmlReader xReader = XmlReader.Create(fs);
					settings = (ClientSettings)xSerializer.Deserialize(xReader);
				}
			}
			catch { } // in a nutshell: since we 'newed up' an instance first, failure to find a file means getting defaults. How it should be.
			return settings;
		}
	}
}