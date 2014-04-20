using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PDFLive
{
    public class PDF_prueba : Base , IPDF
    {

        #region DataMember
        string _password = null;
        Document doc = new Document();
        MemoryStream docStream;
        PdfWriter writer;

        MemoryStream BufferPdf;

        #endregion

        #region Properties
        /// <summary>
        /// Devuelve el productor del PDF
        /// </summary> 
        public string Producer
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Agrega o devuelve el autor del PDF
        /// </summary>  
        public string Author
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Agrega o devuelve el titulo del PDF
        /// </summary>  
        public string Title
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Agrega o devuelve el asunto del PDF
        /// </summary>  
        public string Subject
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Public Methods

        ~PDF_prueba()
        {
            Dispose(true);

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {

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
            BufferPdf = new MemoryStream(content);

            doc = new Document();
            docStream = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, docStream);
            doc.Open();
            //Merge(content);
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
            PdfReader reader = new PdfReader(BufferPdf.ToArray());
            int r = reader.NumberOfPages;
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
            throw new NotImplementedException();
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
            bytesToDoc(doc, writer, BufferPdf.ToArray());
            doc.Close();
            return docStream.ToArray();
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

            PdfReader reader = new PdfReader(BufferPdf.ToArray());

            MemoryStream Stream = new MemoryStream();

            PdfStamper pdfStamper = new PdfStamper(reader, Stream);
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

                pdfPageContents.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, pageSize.Width / 2, pageSize.Height / 2, textAngle);

                pdfPageContents.EndText();
            }
            pdfStamper.FormFlattening = true;
            pdfStamper.Close();

            BufferPdf = new MemoryStream( Stream.ToArray());
            Stream.Dispose();

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

            using (PdfReader reader = new PdfReader(BufferPdf.ToArray()))
            {
                using (MemoryStream Stream = new MemoryStream())
                {
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
                    BufferPdf = new MemoryStream(Stream.ToArray());
                }
            }
        }
         #endregion


        private void bytesToDoc(Document document, PdfWriter writer, byte[] source)
        {
            try
            {
                //writer.PageEvent = new PdfPageEvents();
                PdfContentByte content = writer.DirectContent;
                PdfReader reader = new PdfReader(source);
                int numberOfPages = reader.NumberOfPages;
                for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                {
                    // Determine page size for the current page
                    document.SetPageSize(reader.GetPageSizeWithRotation(currentPageIndex));

                    // Create page
                    document.NewPage();
                    PdfImportedPage importedPage = writer.GetImportedPage(reader, currentPageIndex);
                    // Determine page orientation
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
            }
            catch (Exception exception)
            {
                throw new Exception("There has an unexpected exception" +
                      " occured during the pdf merging process.", exception);
            }
        }
        
        
        #region Merge
        private void Merge(byte[] source)
        {
            Document document = new Document();
            MemoryStream document_Stream = new MemoryStream();

            using (PdfCopy pdf = new PdfCopy(document, document_Stream))
            {
                PdfReader reader = null;
                PdfImportedPage page = null;
                document.Open();                

                reader = new PdfReader(BufferPdf);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    document.SetPageSize(reader.GetPageSizeWithRotation(i));
                    page = pdf.GetImportedPage(reader, i);
                    pdf.AddPage(page);
                }

                pdf.FreeReader(reader);
                reader.Close();

                reader = new PdfReader(source);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    document.SetPageSize(reader.GetPageSizeWithRotation(i));
                    page = pdf.GetImportedPage(reader, i);
                    pdf.AddPage(page);
                }

                pdf.FreeReader(reader);
                reader.Close();

                document.Close();
                BufferPdf = new MemoryStream(document_Stream.ToArray());
                document_Stream.Dispose();
            }

        }
        #endregion
    }
}
