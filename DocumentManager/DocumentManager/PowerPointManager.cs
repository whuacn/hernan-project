using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Interop;
using Microsoft.Office.Core;

namespace DocumentManager
{
    public class PowerPointManager
    {

        public static byte[] ToPDF(byte[] buffer)
        {
            byte[] result = null;
            string fileOrigen = TempManager.CreateTmpFile();
            string fileDestino = TempManager.CreateTmpFile();
            PowerPoint.Application objPPT = new PowerPoint.Application();

            objPPT.DisplayAlerts = PowerPoint.PpAlertLevel.ppAlertsNone;
            try
            {
                TempManager.UpdateTmpFile(fileOrigen, buffer);

                PowerPoint.Presentation oPPT =  objPPT.Presentations.Open(fileOrigen, WithWindow: MsoTriState.msoFalse);

                if (oPPT != null)
                {
                    oPPT.ExportAsFixedFormat(fileDestino, PowerPoint.PpFixedFormatType.ppFixedFormatTypePDF);
                    oPPT.Close();
                }

                result = TempManager.ReadTmpFile(fileDestino);

                TempManager.DeleteTmpFile(fileOrigen);
                TempManager.DeleteTmpFile(fileDestino);
            }
            finally
            {
                objPPT.Quit();
            }

            return result;
        }

    }
}
