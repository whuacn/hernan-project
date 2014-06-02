using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager
{
    class Program
    {
        static void Main(string[] args)
        {

            FileStream Stream = new FileStream("test7.docx", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(Stream);
            byte[] buffer = br.ReadBytes((int)Stream.Length);
            br.Close();
            Stream.Close();

            byte[] result  = WordManager.ToPDF(buffer);

            Stream = new FileStream("test7.pdf", FileMode.Create, FileAccess.Write);
            BinaryWriter Write = new BinaryWriter(Stream);
            Write.Write(result);
            Write.Close();
            Stream.Close();

            System.Diagnostics.Process.Start("test7.pdf");
            
            /*
            FileStream Stream = new FileStream("test7.docx", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(Stream);
            byte[] buffer = br.ReadBytes((int)Stream.Length);
            br.Close();
            Stream.Close();
           
            string result  = WordManager.ToHTML(buffer);

            Stream = new FileStream("test7.html", FileMode.Create, FileAccess.Write);
            StreamWriter Write = new StreamWriter(Stream, Encoding.GetEncoding(1252));
            Write.Write(result);
            Write.Close();
            Stream.Close();

            System.Diagnostics.Process.Start("test7.html");             
             */


            /*
            FileStream Stream = new FileStream("test2.xlsx", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(Stream);
            byte[] buffer = br.ReadBytes((int)Stream.Length);
            br.Close();
            Stream.Close();

            byte[] result = ExcelManager.ToPDF(buffer);

            Stream = new FileStream("testxls2.pdf", FileMode.Create, FileAccess.Write);
            BinaryWriter Write = new BinaryWriter(Stream);
            Write.Write(result);
            Write.Close();
            Stream.Close();

            System.Diagnostics.Process.Start("testxls2.pdf");
            */
            /*
            FileStream Stream = new FileStream("test1.xlsx", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(Stream);
            byte[] buffer = br.ReadBytes((int)Stream.Length);
            br.Close();
            Stream.Close();

            byte[] result = ExcelManager.ToHTML(buffer);

            Stream = new FileStream("test1.html", FileMode.Create, FileAccess.Write);
            BinaryWriter Write = new BinaryWriter(Stream);
            Write.Write(result);
            Write.Close();
            Stream.Close();

            System.Diagnostics.Process.Start("test1.html");
             */
            /*
            FileStream Stream = new FileStream("test3.ppt", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(Stream);
            byte[] buffer = br.ReadBytes((int)Stream.Length);
            br.Close();
            Stream.Close();

            byte[] result = PowerPointManager.ToPDF(buffer);

            Stream = new FileStream("testppt3.pdf", FileMode.Create, FileAccess.Write);
            BinaryWriter Write = new BinaryWriter(Stream);
            Write.Write(result);
            Write.Close();
            Stream.Close();

            System.Diagnostics.Process.Start("testppt3.pdf");
            */
        }
    }
}
