using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Microsoft.Tools.TestClient
{
	internal class ConfigFileMappingManager
	{
		private static string savedConfigFolder;

		private static string mappingFilePath;

		private static ConfigFileMappingManager configFileManager;

		private IDictionary<string, string> addressToFileEntries = new Dictionary<string, string>();

		static ConfigFileMappingManager()
		{
			ConfigFileMappingManager.savedConfigFolder = Path.Combine(ToolingEnvironment.SavedDataBase, "CachedConfig");
			ConfigFileMappingManager.mappingFilePath = Path.Combine(ConfigFileMappingManager.savedConfigFolder, "AddressToConfigMapping.xml");
		}

		private ConfigFileMappingManager()
		{
			this.ReadFromMappingFile();
		}

		internal void AddConfigFileMapping(string address)
		{
			if (!this.addressToFileEntries.ContainsKey(address))
			{
				Guid guid = Guid.NewGuid();
				string str = string.Concat(guid.ToString(), ".config");
				string str1 = Path.Combine(ConfigFileMappingManager.savedConfigFolder, str);
				this.addressToFileEntries.Add(address, str1);
				try
				{
					this.WriteToMappingFile();
				}
				catch (IOException oException)
				{
				}
				catch (UnauthorizedAccessException unauthorizedAccessException)
				{
				}
			}
		}

		internal void Clear()
		{
			this.addressToFileEntries.Clear();
			try
			{
				Directory.Delete(ConfigFileMappingManager.savedConfigFolder, true);
			}
			catch (IOException oException)
			{
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
			}
		}

		internal void DeleteConfigFileMapping(string address)
		{
			if (this.addressToFileEntries.ContainsKey(address))
			{
				try
				{
					File.Delete(this.addressToFileEntries[address]);
					this.WriteToMappingFile();
				}
				catch (IOException oException)
				{
				}
				catch (UnauthorizedAccessException unauthorizedAccessException)
				{
				}
				this.addressToFileEntries.Remove(address);
			}
		}

		internal bool DoesConfigMappingExist(string address)
		{
			if (!this.addressToFileEntries.ContainsKey(address))
			{
				return false;
			}
			return File.Exists(this.addressToFileEntries[address]);
		}

		internal static ConfigFileMappingManager GetInstance()
		{
			if (ConfigFileMappingManager.configFileManager == null)
			{
				ConfigFileMappingManager.configFileManager = new ConfigFileMappingManager();
			}
			return ConfigFileMappingManager.configFileManager;
		}

		internal string GetSavedConfigPath(string address)
		{
			if (!this.addressToFileEntries.ContainsKey(address))
			{
				return null;
			}
			return this.addressToFileEntries[address];
		}

		private void ReadFromMappingFile()
		{
			this.addressToFileEntries.Clear();
			if (!File.Exists(ConfigFileMappingManager.mappingFilePath))
			{
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(ConfigFileMappingManager.mappingFilePath);
				goto Label0;
			}
			catch (XmlException xmlException)
			{
			}
			catch (IOException oException)
			{
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
			}
			return;
		Label0:
			IEnumerator enumerator = xmlDocument.DocumentElement.ChildNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					XmlNode current = (XmlNode)enumerator.Current;
					if (current.ChildNodes.Count < 2)
					{
						continue;
					}
					this.addressToFileEntries.Add(current.ChildNodes[0].InnerText, current.ChildNodes[1].InnerText);
				}
				return;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		private void WriteToMappingFile()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateElement("Mapping"));
			foreach (KeyValuePair<string, string> addressToFileEntry in this.addressToFileEntries)
			{
				XmlElement xmlElement = xmlDocument.CreateElement("Entry");
				XmlElement key = xmlDocument.CreateElement("Address");
				key.InnerText = addressToFileEntry.Key;
				xmlElement.AppendChild(key);
				XmlElement value = xmlDocument.CreateElement("ConfigPath");
				value.InnerText = addressToFileEntry.Value;
				xmlElement.AppendChild(value);
				xmlDocument.DocumentElement.AppendChild(xmlElement);
			}
			if (!Directory.Exists(ConfigFileMappingManager.savedConfigFolder))
			{
				Directory.CreateDirectory(ConfigFileMappingManager.savedConfigFolder);
			}
			xmlDocument.Save(ConfigFileMappingManager.mappingFilePath);
		}
	}
}