#region Using

using System.IO;
using System.IO.IsolatedStorage;

#endregion

namespace SiteMonitor
{
  public static class StorageHelper
  {

    /// <summary>
    /// Saves the content to the file.
    /// </summary>s
    public static void Save(string fileName, string content)
    {
      using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
      {
        using (StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.Create, isoStore)))
        {
          writer.Write(content);
        }
      }
    }

    /// <summary>
    /// Loads the content of the file.
    /// </summary>
    public static string Load(string fileName)
    {
      using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
      {
        if (isoStore.GetFileNames(fileName).Length > 0)
        {
          using (StreamReader reader = new StreamReader(new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, isoStore)))
          {
            return reader.ReadToEnd();
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Deletes the file to clear the settings.
    /// </summary>
    public static void Delete(string fileName)
    {
      using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
      {
        if (isoStore.GetFileNames(fileName).Length > 0)
          isoStore.DeleteFile(fileName);
      }
    }

  }
}

