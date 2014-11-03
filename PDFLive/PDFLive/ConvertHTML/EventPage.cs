using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

namespace PDFLive
{
    public class EventPage : iTextSharp.text.pdf.PdfPageEventHelper
    {

        public PDFLive.ConvertHTML.Structures.TagHeader objHeader;
        public PDFLive.ConvertHTML.Structures.TagFooter objFooter;
        public int _pagenumber;
        public int _fechaemision;

        private bool _auth = false;
        private string _user = null;
        private string _psw = null;

        private float _marginLeft = 0;
        private float _marginRight = 0;
        private float _marginTop = 0;
        private float _marginBottom = 0;

        private PdfContentByte cb;

        public void SetMargins(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            this._marginLeft = marginLeft;
            this._marginRight = marginRight;
            this._marginTop = marginTop;
            this._marginBottom = marginBottom;

        }

        public void setCredentials(string user, string psw)
        {
            this._user = user;
            this._psw = psw;
            this._auth = true;
        }


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            

        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            AddHeader(writer, document);
            AddFooter(writer, document);
        }

        void AddHeader(PdfWriter writer, Document document)
        {
            PdfPTable tableH = new PdfPTable(1);
            tableH.WidthPercentage = 100f;
            tableH.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

            if (objHeader.texto != "" && objHeader.texto != null)
            {

                PdfPCell cell = new PdfPCell();
                cell.Border = Rectangle.NO_BORDER;
                cell.Padding = 0;

                String texto = objHeader.texto + "<table Height='" + System.Convert.ToString(objHeader.Height) + "'><tr><td></td></tr></table>";

                StringReader sReader = new StringReader(texto);

                iTextSharp.text.html.simpleparser.HTMLWorker worker;

                if (_auth)
                    worker = new iTextSharp.text.html.simpleparser.HTMLWorker(document, _user, _psw);
                else
                    worker = new iTextSharp.text.html.simpleparser.HTMLWorker(document);


                Dictionary<string, object> Props = new Dictionary<string, object>();

                if (objHeader.baseurl != "")
                    Props["img_baseurl"] = objHeader.baseurl;

                List<IElement> p = HTMLWorker.ParseToList(sReader, objHeader.css, Props, _user, _psw);

                for (int k = 0; k < p.Count; k++)
                {
                    p[k] = PDFLive.ConvertHTML.HTMLFunctions.ExtendElement((IElement)p[k]);
                    cell.AddElement((IElement)p[k]);
                }
                tableH.AddCell(cell);
                
                if (objHeader.Height > 0)
                {
                    tableH.WriteSelectedRows(0, 1, document.LeftMargin, document.PageSize.Height - document.marginTop, writer.DirectContent);
                    document.marginTop = tableH.TotalHeight + objHeader.Height;
                }
                else
                {
                    tableH.WriteSelectedRows(0, 1, document.LeftMargin, document.PageSize.Height - _marginTop, writer.DirectContent);
                    document.marginTop = tableH.TotalHeight + _marginTop;
                }

                
            }
        }
        void AddFooter(PdfWriter writer, Document document)
        {
            int rows = 0;

            PdfPTable tableF = new PdfPTable(1);
            PdfPCell cell;
            tableF.WidthPercentage = 100f;
            tableF.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

            if (objFooter.texto != "" && objFooter.texto != null)
            {
                rows = -1;
                cell = new PdfPCell();
                cell.Border = Rectangle.NO_BORDER;
                cell.Padding = 0;


                String texto = objFooter.texto;

                TextReader sReader = new StringReader(texto);

                iTextSharp.text.html.simpleparser.HTMLWorker worker;

                if (_auth)
                    worker = new iTextSharp.text.html.simpleparser.HTMLWorker(document, _user, _psw);
                else
                    worker = new iTextSharp.text.html.simpleparser.HTMLWorker(document);


                Dictionary<string, object> Props = new Dictionary<string, object>();

                if (objFooter.baseurl != "")
                    Props["img_baseurl"] = objFooter.baseurl;

                List<IElement> p = HTMLWorker.ParseToList(sReader, objFooter.css, Props, _user, _psw);

                for (int k = 0; k < p.Count; k++)
                {
                    p[k] = PDFLive.ConvertHTML.HTMLFunctions.ExtendElement((IElement)p[k]);
                    cell.AddElement((IElement)p[k]);
                }

                tableF.AddCell(cell);
            }

            //-----------------------------------
            if (_pagenumber == 1 || _fechaemision == 1)
            {
                cell = new PdfPCell();
                cell.Border = Rectangle.NO_BORDER;
                tableF.AddCell(cell);
            }


            PdfPTable ptabla;
            ptabla = new PdfPTable(1);

            if (_pagenumber == 1 && _fechaemision == 1)
            {
                ptabla = new PdfPTable(2);
            }
            ptabla.HorizontalAlignment = Element.ALIGN_RIGHT;
            ptabla.WidthPercentage = 100f;
            

            if (_fechaemision == 1)
            {
                rows = 3;

                /*Agrego la fecha de Emision del reporte*/
                PdfPCell pcell = new PdfPCell();
                pcell.Border = Rectangle.NO_BORDER;
                pcell.HorizontalAlignment = Element.ALIGN_LEFT;

                Phrase paginado = new Phrase("Emisión: " + Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy HH:mm")), FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL));
                pcell.Column.AddText(paginado);
                ptabla.AddCell(pcell);
            }

            if (_pagenumber == 1)
            {
                if (_fechaemision == 1)
                    rows = 4;
                else
                    rows = 3;

                /*Agrego los numeros de paginas*/
                PdfPCell pcell = new PdfPCell();
                pcell.Border = Rectangle.NO_BORDER;
                pcell.HorizontalAlignment = Element.ALIGN_CENTER;

                Phrase paginado = new Phrase("Página " + document.PageNumber.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL));
                pcell.Column.AddText(paginado);
                ptabla.AddCell(pcell);

            }

            if (_pagenumber == 1 || _fechaemision == 1)
            {
                cell = new PdfPCell();
                cell.Border = Rectangle.TOP_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.AddElement((IElement)ptabla);
                tableF.AddCell(cell);
            }

            if (rows != 0)
            {
                float maxHeight;
                maxHeight = tableF.TotalHeight;

                if (document.marginBottom != _marginBottom + maxHeight)
                    document.marginBottom = _marginBottom + maxHeight + 5;

                tableF.WriteSelectedRows(0, rows, document.LeftMargin, _marginBottom + maxHeight, writer.DirectContent);
            }        
        }

        
    }
}
