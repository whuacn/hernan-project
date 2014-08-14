using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;
using System.IO;


namespace DocumentManager
{
    //http://www.microsoft.com/en-us/download/details.aspx?id=3508
    public class WordManager
    {
        public static byte[] ToPDF(byte[] buffer)
        {
            byte[] result = null;
            string fileOrigen = TempManager.CreateTmpFile();
            string fileDestino = TempManager.CreateTmpFile();
           
            
            Word.Application objWord = new Word.Application();

            try
            {
                TempManager.UpdateTmpFile(fileOrigen, buffer);

                objWord.Visible = false;
                objWord.ScreenUpdating = false;
                objWord.Documents.Open(FileName: fileOrigen);

                if (objWord.Documents.Count > 0)
                {
                    Microsoft.Office.Interop.Word.Document oDoc = objWord.ActiveDocument;
                    oDoc.SaveAs(FileName: fileDestino, FileFormat: Word.WdSaveFormat.wdFormatPDF);
                    oDoc.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                }

                result = TempManager.ReadTmpFile(fileDestino);

                TempManager.DeleteTmpFile(fileOrigen);
                TempManager.DeleteTmpFile(fileDestino);
            }
            finally
            {
                objWord.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                Marshal.ReleaseComObject(objWord);
            }

            return result;
        }

        public static string ToHTML(byte[] buffer)
        {
            return ToHTML(buffer, null);
        }
        public static string ToHTML(byte[] buffer, string tempPath)
        {
            string result = null;
            string fileOrigen = null;
            string fileDestino = null;

            if (tempPath == null)
            {
                fileOrigen = TempManager.CreateTmpFile();
                fileDestino = TempManager.CreateTmpFile();
                tempPath = Path.GetTempPath();
            }
            else
            {
                fileOrigen = Path.Combine(tempPath, DateTime.Now.Ticks.ToString() + ".TMP");
                fileDestino = Path.Combine(tempPath, DateTime.Now.Ticks.ToString() + "_dest.TMP");
                File.Create(fileOrigen).Close();
                File.Create(fileDestino).Close();
            }

            Word.Application objWord = new Word.Application();

            try
            {
                TempManager.UpdateTmpFile(fileOrigen, buffer);

                objWord.Visible = false;
                objWord.ScreenUpdating = false;
                objWord.Documents.Open(FileName: fileOrigen);

                if (objWord.Documents.Count > 0)
                {
                    Microsoft.Office.Interop.Word.Document oDoc = objWord.ActiveDocument;
                    oDoc.SaveAs(FileName: fileDestino, FileFormat: Word.WdSaveFormat.wdFormatFilteredHTML);
                    oDoc.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                }

                result = TempManager.ReadTmpFileString(fileDestino);
                result = Utils.HtmlEmbedImages(result, tempPath);

                TempManager.DeleteTmpFile(fileOrigen);
                TempManager.DeleteTmpFile(fileDestino);
            }
            finally
            {
                objWord.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                Marshal.ReleaseComObject(objWord);
            }

            return result;
        }
    }
}
