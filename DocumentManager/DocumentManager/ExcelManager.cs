using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop;
using Microsoft.Office.Core;
using System.Collections;
using System.Runtime.InteropServices;

namespace DocumentManager
{
    public class ExcelManager
    {

        public static byte[] ToPDF(byte[] buffer)
        {
            byte[] result = null;
            string fileOrigen = TempManager.CreateTmpFile();
            string fileDestino = TempManager.CreateTmpFile();


            Excel.Application objExcel = new Excel.Application();

            try
            {
                TempManager.UpdateTmpFile(fileOrigen, buffer);

                objExcel.Visible = false;
                objExcel.ScreenUpdating = false;
                objExcel.DisplayAlerts = false;
                objExcel.DisplayDocumentActionTaskPane = false;

                objExcel.Workbooks.Open(fileOrigen);

                if (objExcel.Workbooks.Count > 0)
                {                   
                    Excel.Workbook oXLS = objExcel.ActiveWorkbook;
                    oXLS.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, fileDestino);
                    oXLS.Close(Excel.XlSaveAction.xlDoNotSaveChanges);
                    objExcel.Workbooks.Close();
                }

                result = TempManager.ReadTmpFile(fileDestino + ".pdf");

                TempManager.DeleteTmpFile(fileOrigen);
                TempManager.DeleteTmpFile(fileDestino + ".pdf");
            }
            finally
            {
                objExcel.Quit();
                Marshal.ReleaseComObject(objExcel);
            }

            return result;
        }

        public static byte[] ToHTML(byte[] buffer)
        {
            byte[] result = null;
            string fileOrigen = TempManager.CreateTmpFile();
            string fileDestino = TempManager.CreateTmpFile();


            Excel.Application objExcel = new Excel.Application();

            try
            {
                TempManager.UpdateTmpFile(fileOrigen, buffer);

                objExcel.Visible = false;
                objExcel.ScreenUpdating = false;
                objExcel.DisplayAlerts = false;

                objExcel.Workbooks.Open(fileOrigen);

                if (objExcel.Workbooks.Count > 0)
                {
                    IEnumerator wsEnumerator = objExcel.ActiveWorkbook.Worksheets.GetEnumerator();
                    object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
                    int i = 1;
                    while (wsEnumerator.MoveNext())
                    {
                        Excel.Worksheet oXLS = (Microsoft.Office.Interop.Excel.Worksheet)wsEnumerator.Current;
                        oXLS.SaveAs(Filename: fileDestino, FileFormat: format);
                        ++i;
                        break;
                    }
                    objExcel.Workbooks.Close();
                }

                result = TempManager.ReadTmpFile(fileDestino);

                TempManager.DeleteTmpFile(fileOrigen);
                TempManager.DeleteTmpFile(fileDestino);
            }
            finally
            {
                objExcel.Quit();
            }

            return result;
        }

    }
}
