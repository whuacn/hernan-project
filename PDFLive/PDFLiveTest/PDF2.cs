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
            PDF2.AllowPrint = true;
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
