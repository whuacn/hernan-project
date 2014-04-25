using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PDFLive
{
    public class PDF2 : Base , IPDF
    {

        #region DataMember
        string _password = "0p7N4ATMtJWSjBg13XgeyoCUT";
        Document doc = new Document();
        MemoryStream docStream;
        PdfWriter writer;
        PdfDictionary Dictionary = new PdfDictionary();
        bool allowPrint = true;
        #endregion

        #region Properties
        /// <summary>
        /// Devuelve el productor del PDF
        /// </summary> 
        public string Producer
        {
            get { return this.getHeader("Producer"); }
        }
        /// <summary>
        /// Agrega o devuelve el autor del PDF
        /// </summary>  
        public string Author
        {
            get
            {
                return this.getHeader("Author");
            }
            set
            {
                this.addHeader("Author", value);
            }
        }
        /// <summary>
        /// Agrega o devuelve el titulo del PDF
        /// </summary>  
        public string Title
        {
            get
            {
                return this.getHeader("Title");
            }
            set
            {
                this.addHeader("Title", value);
            }
        }
        /// <summary>
        /// Agrega o devuelve el asunto del PDF
        /// </summary>  
        public string Subject
        {
            get
            {
                return this.getHeader("Subject");
            }
            set
            {
                this.addHeader("Subject", value);
            }
        }

        /// <summary>
        /// Setea si esta permitido imprimir el documento
        /// </summary>  
        public bool AllowPrint
        {
            get
            {
                return this.allowPrint;
            }
            set
            {
                this.allowPrint =  value;
            }
        }

        #endregion

        #region Public Methods

        ~PDF2()
        {
            Dispose(true);

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    docStream.Dispose();
                    doc.Dispose();
                }
                catch (Exception)
                {
                }
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Crea un PDF vacio
        /// </summary>  
        public void createFile()
        {
            doc = new Document();
            docStream = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, docStream);
            encryptPdf();
            doc.Open();
        }
        /// <summary>
        /// Abre el pdf desde una ruta fisica
        /// </summary>
        /// <param name="path">Path del PDF</param>
        public void openFile(string path)
        {
            FileStream Stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(Stream);
            open(br.ReadBytes((int)Stream.Length));
        }
        /// <summary>
        /// Abre el pdf desde un contenido binario
        /// </summary>
        /// <param name="content">Contenido binario</param>
        public void open(byte[] content)
        {
            doc = new Document();
            docStream = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, docStream);
            //writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
            encryptPdf();
            doc.Open();
            writer.Info.Merge(Dictionary);
            Merge(content);
        }
        /// <summary>
        /// Concatena el pdf abierto con otro desde un pdf fisico
        /// </summary>
        /// <param name="path">Path del PDF</param>
        public void addFile(string path)
        {
            FileStream Stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(Stream);
            Merge(br.ReadBytes((int)Stream.Length));
        }
        /// <summary>
        /// Concatena el pdf abierto con otro desde un contenido binario
        /// </summary>
        /// <param name="content">Contenido binario</param>
        public void add(byte[] content)
        {
            if (!doc.IsOpen())
            {
                open(content);
            }
            else
            {
                Merge(content);
            }
        }

        /// <summary>
        /// Devuelve la cantidad de paginas del PDF abierto
        /// </summary>
        public int cantPages()
        {
            return writer.PageNumber;
        }

        /// <summary>
        /// Devuelve la cantidad de paginas del PDF
        /// </summary>
        public int cantPages(byte[] content)
        {
            PdfReader reader = new PdfReader(content, System.Text.Encoding.UTF8.GetBytes(_password));
            int r = reader.NumberOfPages;
            reader.Close();
            reader.Dispose();
            return r;
        }

        /// <summary>
        /// Agrega un encabezado header
        /// </summary>
        /// <param name="key">Nombre del encabezado</param>
        /// <param name="value">Valor del encabezado</param>
        public void addHeader(string key, string value)
        {
            doc.AddHeader(key, value);
        }
        /// <summary>
        /// Devuelve la cantidad de paginas del PDF abierto
        /// </summary>
        public string getHeader(string key)
        {
            string r = "";
            try
            {
                PdfObject obj = writer.Info.Get(new PdfName(key));
                if (obj.IsString())
                    r = obj.ToString();
            }
            catch (Exception)
            {   }
            return r;
        }
		
        /// <summary>
        /// Protege el pdf con clave
        /// </summary>		
		/// <param name="password">Clave</param>
        public void Proteger(string password)
        {
            _password = password;

        }		
		
        /// <summary>
        /// Crea una nueva pagina en el PDF abierto
        /// </summary>     
        public void newPage()
        {
            doc.NewPage(); 
        }
        /// <summary>
        /// Inserta texto en el PDF abierto
        /// </summary>
        /// <param name="texto">Texto a insertar</param>
        /// <param name="fuente">Nombre de la fuente</param>
        /// <param name="size">Tamaño de la fuente</param>
        /// <param name="color">Color de la fuente en hexa (#000000)</param>
        /// <param name="leading">Interlineado</param>
        public void addText(string texto, string fuente, float size, string color, string leading)
        {
            BaseFont baseFont = BaseFont.CreateFont(fuente, BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseColor c = new BaseColor(System.Drawing.ColorTranslator.FromHtml(color).ToArgb());
            Font f = new Font(baseFont, size, 1, c);
            doc.Add(new Paragraph(((float)Convert.ToDecimal(leading) * 10), texto, f));

        }
        /// <summary>
        /// Devuelve el pdf abierto en bytes
        /// </summary>     
        public byte[] Generar()
        {

            doc.Close();
            return docStream.ToArray();
        }

        private void encryptPdf()
        {
            try
            {
                //Dictionary = writer.Info;
                int permissions = PdfWriter.ALLOW_COPY;
                if (this.allowPrint)
                    permissions = permissions | PdfWriter.ALLOW_PRINTING;
                byte[] pass = System.Text.Encoding.UTF8.GetBytes(_password);
                writer.SetEncryption(null, pass, permissions, PdfWriter.ENCRYPTION_AES_128);
            }
            catch (Exception exception)
            {
                throw new Exception("PDFLive - Metodo: encryptPdf - Ocurrio un error al realizar la proteccion del PDF. Detalle: " + exception.Message, exception);
            }

            /*doc.Close();
            using (PdfReader reader = new PdfReader(docStream.ToArray()))
            {
                docStream.Dispose();
                docStream = new MemoryStream();
                PdfStamper stamper = new PdfStamper(reader, docStream);
                stamper.SetEncryption(null, System.Text.Encoding.UTF8.GetBytes(_password), PdfWriter.ALLOW_COPY, PdfWriter.STRENGTH40BITS);
                stamper.Close();
                reader.Close();
            }*/
        }

        /// <summary>
        /// Agrega una marca de agua en el pdf abierto
        /// </summary>
        /// <param name="watermarkText">Texto a insertar como marca de agua</param>
        /// <param name="fuente">Nombre de la fuente</param>
        /// <param name="size">Tamaño de la fuente</param>
        /// <param name="color">Color de la fuente en hexa (#000000)</param>
        public void addWaterMark(string watermarkText, string fuente, float size, string color)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                Dictionary = writer.Info;
                doc.Close();
                using (PdfReader reader = new PdfReader(docStream.ToArray(), System.Text.Encoding.UTF8.GetBytes(_password)))
                {
                    docStream.Dispose();
                    using (PdfStamper pdfStamper = new PdfStamper(reader, Stream))
                    {
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                            PdfContentByte pdfPageContents = pdfStamper.GetOverContent(i);//.GetUnderContent(i);

                            float textAngle = 45;
                            PdfGState gstate = new PdfGState();
                            gstate.FillOpacity = 0.1f;
                            gstate.StrokeOpacity = 0.1f;

                            pdfPageContents.SetGState(gstate);
                            pdfPageContents.BeginText();
                            BaseFont baseFont = BaseFont.CreateFont(fuente, BaseFont.WINANSI, BaseFont.EMBEDDED);
                            pdfPageContents.SetFontAndSize(baseFont, size);
                            BaseColor c = new BaseColor(System.Drawing.ColorTranslator.FromHtml(color).ToArgb());
                            pdfPageContents.SetColorFill(c);

                           
                            var x = (pageSize.Width) / 2;
                            var y = (pageSize.Height) / 2;

                            //float fw = getTextheight(baseFont, size, watermarkText);
                           // x = x - (fw / 2);
                           /* Type1Font baseFont2 = baseFont as Type1Font;
                            if (baseFont2 != null)
                            { 
                                x = x + baseFont2.llx;
                                y = y + baseFont2.lly;
                            }*/

                            pdfPageContents.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, x, y, textAngle);

                            pdfPageContents.EndText();
                        }
                        pdfStamper.FormFlattening = true;
                        pdfStamper.Close();
                    }
                }
                open(Stream.ToArray());
            }
            addHeader("WaterMark", "1");
            addHeader("WaterMarkText", watermarkText);
        }


        /// <summary>
        /// Inserta texto en todas las paginas del PDF abierto
        /// </summary>
        /// <param name="texto">Texto a insertar</param>
        /// <param name="fuente">Nombre de la fuente</param>
        /// <param name="size">Tamaño de la fuente</param>
        /// <param name="color">Color de la fuente en hexa (#000000)</param>
        /// <param name="xPosition">Posicion horizontal del texto</param>
        /// <param name="yPosition">Posicion vertical del texto</param>
        /// <param name="angle">Angulo del texto</param>
        public void addTextAllPages(string texto, string fuente, float size, string color, float xPosition, float yPosition, float angle)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                Dictionary = writer.Info;
                doc.Close();
                using (PdfReader reader = new PdfReader(docStream.ToArray(), System.Text.Encoding.UTF8.GetBytes(_password)))
                {
                    docStream.Dispose();
                    using (PdfStamper pdfStamper = new PdfStamper(reader, Stream))
                    {
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                            PdfContentByte under = pdfStamper.GetOverContent(i);

                            BaseFont baseFont = BaseFont.CreateFont(fuente, BaseFont.WINANSI, BaseFont.EMBEDDED);

                            under.BeginText();
                            BaseColor c = new BaseColor(System.Drawing.ColorTranslator.FromHtml(color).ToArgb());
                            under.SetColorFill(c);
                            under.SetFontAndSize(baseFont, size);
                            under.ShowTextAligned(PdfContentByte.ALIGN_LEFT, texto, xPosition, pageSize.Height - yPosition, angle);
                            under.EndText();

                        }
                        pdfStamper.FormFlattening = true;
                        pdfStamper.Close();
                    }
                }
                open(Stream.ToArray());
            }
        }
         #endregion

        #region Merge
        private void Merge(byte[] source)
        {
            try
            {
                PdfContentByte content = writer.DirectContent;
                using (PdfReader reader = new PdfReader(source, System.Text.Encoding.UTF8.GetBytes(_password)))
                {
                    int numberOfPages = reader.NumberOfPages;
                    for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                    {
                        doc.SetPageSize(reader.GetPageSizeWithRotation(currentPageIndex));
                        doc.NewPage();
                        PdfImportedPage importedPage = writer.GetImportedPage(reader, currentPageIndex);
                        
                        int pageOrientation = reader.GetPageRotation(currentPageIndex);
                        if ((pageOrientation == 90) || (pageOrientation == 270))
                        {
                            content.AddTemplate(importedPage, 0, -1f, 1f, 0, 0,
                               reader.GetPageSizeWithRotation(currentPageIndex).Height);
                        }
                        else
                        {
                            content.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                        }
                    }
                    writer.FreeReader(reader);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("PDFLive - Metodo: Merge - Ocurrio un error al realizar el Merge del PDF. Detalle: " + exception.Message, exception);
            }
        }
        #endregion
    }
}
