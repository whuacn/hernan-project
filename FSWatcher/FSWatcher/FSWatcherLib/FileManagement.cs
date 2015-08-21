using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FSWatcherLib
{
    public static class FileManagement
    {

        public static void MakeDirectory(string strPathName)
        {
            int Length;
            int DirLength;
            Length = 4;
            if ((strPathName.Substring((strPathName.Length - 1)) != "\\"))
            {
                strPathName = (strPathName + "\\");
            }
            // create nested directory ---------
            while (!Directory.Exists(strPathName))
            {
                DirLength = (strPathName.IndexOf("\\", (Length - 1)) + 1);
                if (!Directory.Exists(strPathName.Substring(0, DirLength)))
                {
                    Directory.CreateDirectory(strPathName.Substring(0, (DirLength - 1)));
                }
                Length = (DirLength + 1);
            }
        }

        public static void RenameDirectory(string strPathName, string strPathNameOld)
        {
            // create nested directory ---------
            if (Directory.Exists(strPathNameOld))
            {
                Directory.Move(strPathNameOld, strPathName);
            }
        }


        internal static void DeleteDirectory(string strPathName)
        {
            if (Directory.Exists(strPathName))
            {
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(strPathName);
                directory.Delete(true); 
            }
        }
    }
}
