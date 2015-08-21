using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FSWatcherLib
{
    [Serializable]
    public class FileSystemWatcherExt : FileSystemWatcher
    {

        public FileSystemWatcherExt()
        {
            this.Changed += new FileSystemEventHandler(OnChanged);
            this.Created += new FileSystemEventHandler(OnCreated);
            this.Deleted += new FileSystemEventHandler(OnDeleted);
            this.Renamed += new RenamedEventHandler(OnRenamed);
        }


        private string _pathDestino = "";
        public string PathDestino { 
            get
            {
                return _pathDestino;
            }
            set
            {
                _pathDestino = value;
            }        
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

            if (File.Exists(e.FullPath))
            {
                string path = System.IO.Path.GetDirectoryName(e.FullPath);
                path = path.Replace(this.Path, "");
                if ((path != ""))
                {
                    FileManagement.MakeDirectory(_pathDestino + path);
                }
                try
                {
                    File.Copy(e.FullPath, (_pathDestino + (path + ("\\" + System.IO.Path.GetFileName(e.FullPath)))), true);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            string path = "";
            if (Directory.Exists(e.FullPath))
            {
                path = e.FullPath.Replace(this.Path, "");
                FileManagement.MakeDirectory(_pathDestino + path);
            }
            else
            {
                if (File.Exists(e.FullPath))
                {
                    path = System.IO.Path.GetDirectoryName(e.FullPath);
                    path = path.Replace(this.Path, "");

                    if ((path != ""))
                    {
                        FileManagement.MakeDirectory(_pathDestino + path);
                    }
                    try
                    {
                        File.Copy(e.FullPath, _pathDestino + path + "\\" + System.IO.Path.GetFileName(e.FullPath), true);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

            string path = "";
            if (!Directory.Exists(e.FullPath) && Directory.Exists(_pathDestino + e.FullPath.Replace(this.Path, "")))
            {
                path = e.FullPath.Replace(this.Path, "");
                FileManagement.DeleteDirectory(_pathDestino + path);
            }
            else
            {
                path = System.IO.Path.GetDirectoryName(e.FullPath);
                path = path.Replace(this.Path, "");
                try
                {
                    File.Delete(_pathDestino + path + "\\" + System.IO.Path.GetFileName(e.FullPath));
                }
                catch (Exception ex)
                {
                }
            }

        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            //Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);

            string path = "";
            string pathOld = "";
            if (Directory.Exists(e.FullPath))
            {
                path = e.FullPath.Replace(this.Path, "");
                pathOld = e.OldFullPath.Replace(this.Path, "");
                FileManagement.RenameDirectory(_pathDestino + path, _pathDestino + pathOld);
            }
            else
            {
                if (File.Exists(e.FullPath))
                {
                    path = System.IO.Path.GetDirectoryName(e.FullPath);
                    path = path.Replace(this.Path, "");

                    pathOld = System.IO.Path.GetDirectoryName(e.OldFullPath);
                    pathOld = pathOld.Replace(this.Path, "");
                    try
                    {
                        File.Move(_pathDestino + pathOld + "\\" + System.IO.Path.GetFileName(e.OldFullPath), _pathDestino + path + "\\" + System.IO.Path.GetFileName(e.FullPath));
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

        }

    }
}
