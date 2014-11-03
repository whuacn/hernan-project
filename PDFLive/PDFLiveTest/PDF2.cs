using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFLiveTest
{
    public partial class PDF2 : Form
    {
        public PDF2()
        {
            InitializeComponent();
        }



        private void PDF2_Load(object sender, EventArgs e)
        {

            Html();
        }        
        
        void Html()
        {
            PDFLive.HTMLConvert PDF = new PDFLive.HTMLConvert();
            //PDF.content = "http://www.mercadolibre.com.ar";
            //PDF.content = "http://sistemas.srt.gov.ar/ConsultasV2/Exportacion/Siniestralidad_CUIL_Exporta.asp?CUIL=23115219154&PERIODO=";
            //PDF.content = "http://sistemas.srt.gob.ar/actas/carga/ACTA_PREV.asp?IDACTA=117103";
            //PDF.content = "http://sistemas.srt.gob.ar/Gestion/gestionOHV/Reportes/EXPDTESTERMINADOS.asp?FDESDE=20100102&FHASTA=20100625";
            //PDF.content = "http://sistemas.srt.gob.ar/Gestion/gestionOHV/Reportes/EXPDTESTERMINADOS.asp?FDESDE=20100102&FHASTA=20100410";
            //PDF.content = "http://sistemas.srt.gob.ar/Gestion/gestionOHV/Reportes/CANCELACIONES.asp?AUX=20060501&AUX2=20140510";
            PDF.content = "http://localhost/reporte.html";

            PDF.LeftMargin = 10;
            PDF.RightMargin = 10;
            PDF.TopMargin = 10;
            PDF.BottonMargin = 10;
            PDF.TopMarginAfter = 0; //Por default 0
            PDF.fechaemision = 1;
            PDF.orientacion = 1;
            //PDF.sizex = 1000;
            //PDF.sizey = 500;
            PDF.setCredentials("", "");


            Byte[] buffer;
            buffer = PDF.Convert();

            FileStream Stream = new FileStream("Html_pdf.pdf", FileMode.Create, FileAccess.Write);
            BinaryWriter Write = new BinaryWriter(Stream);
            Write.Write(buffer);
            Write.Close();
            Stream.Close();

            System.Diagnostics.Process.Start("Html_pdf.pdf");
            this.Close();


        }

        void pdf()
        {
            PDFLive.PDF2 PDF2;
            FileStream Stream;
            BinaryReader br;
            Byte[] buffer;

            PDF2 = new PDFLive.PDF2();

            /*Stream = new FileStream("pdf_horizontal.pdf", FileMode.Open, FileAccess.Read);
            br = new BinaryReader(Stream);
            buffer = br.ReadBytes((int)Stream.Length);
            PDF2.open(buffer);
            MessageBox.Show(PDF2.cantPages().ToString());
            MessageBox.Show(PDF2.getHeader("Producer"));*/


            PDF2 = new PDFLive.PDF2();
            PDF2.AllowPrint = false;
            Stream = new FileStream("pdf2.pdf", FileMode.Open, FileAccess.Read);
            br = new BinaryReader(Stream);
            buffer = br.ReadBytes((int)Stream.Length);
            PDF2.add(buffer);

            Stream = new FileStream("pdf_horizontal.pdf", FileMode.Open, FileAccess.Read);
            br = new BinaryReader(Stream);
            buffer = br.ReadBytes((int)Stream.Length);
            PDF2.add(buffer);



            /* Stream = new FileStream("pdf_clavesrt.pdf", FileMode.Open, FileAccess.Read);
             br = new BinaryReader(Stream);
             buffer = br.ReadBytes((int)Stream.Length);
             PDF2.add(buffer);*/

            PDF2.addWaterMark("VERSION DIGITAL", "Helvetica", 80, "#F9150D");
            PDF2.addTextAllPages("VERSION DIGITAL", "Helvetica", 18, "#F9150D", 20, 250, 90);


            /*Stream = new FileStream("pdf_horizontal.pdf", FileMode.Open, FileAccess.Read);
             br = new BinaryReader(Stream);
             buffer = br.ReadBytes((int)Stream.Length);
             PDF2.add(buffer);
             PDF2.addText("VERSION DIGITAL XXXXX", "Helvetica", 18, "#F9150D", "20");
             //PDF2.add(buffer);

             Stream = new FileStream("pdf2.pdf", FileMode.Open, FileAccess.Read);
             br = new BinaryReader(Stream);
             buffer = br.ReadBytes((int)Stream.Length);
             PDF2.add(buffer);

             PDF2.addWaterMark("VERSION DIGITAL", "Helvetica", 80, "#F9150D");
             PDF2.addTextAllPages("VERSION DIGITAL", "Helvetica", 18, "#F9150D", 20, 250, 90);

             PDF2.addHeader("YO", "HERNAN");
             //MessageBox.Show(PDF2.getHeader("WaterMarkText"));
             PDF2.Proteger("xxxa");*/
            buffer = PDF2.Generar();



            Stream = new FileStream("pdf_result.pdf", FileMode.Create, FileAccess.Write);
            BinaryWriter Write = new BinaryWriter(Stream);
            Write.Write(buffer);
            Write.Close();
            Stream.Close();

            System.Diagnostics.Process.Start("pdf_result.pdf");
            this.Close();
        }
    }
}
