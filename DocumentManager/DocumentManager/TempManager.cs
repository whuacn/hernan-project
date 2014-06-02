using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager
{
    public class TempManager
    {
        public static string CreateTmpFile()
        {
            string fileName = string.Empty;
            try
            {
                fileName = Path.GetTempFileName();
                FileInfo fileInfo = new FileInfo(fileName);
                fileInfo.Attributes = FileAttributes.Temporary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileName;
        }

        public static void UpdateTmpFile(string tmpFile, byte[] buffer)
        {
            try
            {
                using (FileStream stream = new FileStream(tmpFile, FileMode.Open, FileAccess.Write))
                {
                    using (BinaryWriter binaryWriter = new BinaryWriter(stream))
                    {
                        binaryWriter.Write(buffer);
                        binaryWriter.Close();
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] ReadTmpFile(string tmpFile)
        {
            byte[] result = null;
            try
            {
                using (FileStream stream = new FileStream(tmpFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        result = br.ReadBytes((int)stream.Length);
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public static void DeleteTmpFile(string tmpFile)
        {
            try
            {
                if (File.Exists(tmpFile))
                {
                    File.Delete(tmpFile);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
