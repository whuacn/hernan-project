using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using PDFLive.ConvertHTML;

namespace PDFLive
{
    public class HTMLConvert : IHTMLConvert
    {
        #region Data Member
        private int _isURL = 1;
        private string _content;
        private float _marginLeft = 20;
        private float _marginRight = 20;
        private float _marginTop = 20;
        private float _marginBottom = 20;
        private int _pagenumber = 1;
        private int _fechaemision = 0;
        private string _baseurl = null;
        private int _margin_after_header = 0;
        private int _orientacion_pagina = 0; //vertical

        private float _pagina_size_x = 0; //ancho de pagina
        private float _pagina_size_y = 0; //alto de pagina

        /*credenciales*/
        private bool _auth = false;
        private string _user = null;
        private string _psw = null;
        #endregion

        #region Properties
        public string content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }
        public int isURL
        {
            get
            {
                return this._isURL;
            }
            set
            {
                this._isURL = value;
            }
        }
        public int pagenumber
        {
            get
            {
                return this._pagenumber;
            }
            set
            {
                this._pagenumber = value;
            }
        }
        public int fechaemision
        {
            get
            {
                return this._fechaemision;
            }
            set
            {
                this._fechaemision = value;
            }
        }
        public int orientacion
        {
            get
            {
                return this._orientacion_pagina;
            }
            set
            {
                this._orientacion_pagina = value;
            }
        }

        public float sizex
        {
            get
            {
                return this._pagina_size_x;
            }
            set
            {
                this._pagina_size_x = value;
            }
        }
        public float sizey
        {
            get
            {
                return this._pagina_size_y;
            }
            set
            {
                this._pagina_size_y = value;
            }
        }

        #region Margins
        public float LeftMargin
        {
            get
            {
                return _marginLeft;
            }
            set
            {
                _marginLeft = value;
            }
        }

        public float RightMargin
        {
            get
            {
                return _marginRight;
            }
            set
            {
                _marginRight = value;
            }
        }
        public float TopMargin
        {
            get
            {
                return _marginTop;
            }
            set
            {
                _marginTop = value;
            }
        }
        public float BottonMargin
        {
            get
            {
                return _marginBottom;
            }
            set
            {
                _marginBottom = value;
            }
        }

        //Si el margen lo pongo antes o despues del header
        public int TopMarginAfter
        {
            get
            {
                return this._margin_after_header;
            }
            set
            {
                this._margin_after_header = value;
            }
        }
        #endregion

        #endregion

        #region Public Methods
        public void setCredentials(string user, string psw)
        {
            this._user = user;
            this._psw = psw;
            this._auth = true;
        }

        public byte[] Convert()
        {
            //throw new NotImplementedException();

            this.RegisterFonts();
            String CodeSource = this.GetContent();
            Rectangle PaginaSize = this.GetPageSize();

            Document PDFdocument = new Document(PaginaSize);

            //header footer html---------------
            //Si se agrega el tag header/footer, luego se busca y borra del codigo fuente de la pagina
            PDFLive.ConvertHTML.Structures.TagHeader objHeader = (PDFLive.ConvertHTML.Structures.TagHeader)PDFLive.ConvertHTML.HTMLFunctions.getTagHeader(CodeSource.Trim(), _baseurl);
            PDFLive.ConvertHTML.Structures.TagFooter objFooter = (PDFLive.ConvertHTML.Structures.TagFooter)PDFLive.ConvertHTML.HTMLFunctions.getTagFooter(CodeSource.Trim(), _baseurl);

            objHeader.Height = 0;
            //end header footer html---------------  

            /*Pongo los margenes*/

            _marginLeft = PDFLive.Functions.MilimetroToPoint(_marginLeft);
            _marginRight = PDFLive.Functions.MilimetroToPoint(_marginRight);
            _marginBottom = PDFLive.Functions.MilimetroToPoint(_marginBottom);

            if (objHeader.texto != "" && _margin_after_header == 0)
            {
                PDFdocument.SetMargins(_marginLeft, _marginRight, PDFLive.Functions.MilimetroToPoint(5), _marginBottom);
                objHeader.Height = _marginTop;
            }
            else
            {
                _marginTop = PDFLive.Functions.MilimetroToPoint(_marginTop);
                PDFdocument.SetMargins(_marginLeft, _marginRight, _marginTop, _marginBottom);
            }

            /*-----Tomo los estilos---------*/
            String attachCss = HTMLFunctions.getCss(CodeSource, _baseurl, _user, _psw);
            StyleSheet st = new StyleSheet();

            CodeSource = CodeSource + attachCss;
            st = HTMLFunctions.extractStyles(ref CodeSource);

            objHeader.css = st;
            objFooter.css = st;
            /*------------------------------*/

            /*Limpio el codigo fuente*/
            HTMLFunctions.ClearCode(ref CodeSource);
            /*-----------------------*/

            MemoryStream PDFfile = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(PDFdocument, PDFfile);
            StringReader sReader = new StringReader(CodeSource);

            HTMLWorker worker;

            if (_auth)
                worker = new HTMLWorker(PDFdocument, _user, _psw);
            else
                worker = new HTMLWorker(PDFdocument);


            //el evento se ejecuta cuando se crea una nueva pagina
            EventPage e = new EventPage();
            writer.PageEvent = e;
            e.objHeader = objHeader;
            e.objFooter = objFooter;
            e._pagenumber = _pagenumber;
            e._fechaemision = _fechaemision;
            e.setCredentials(_user, _psw);
            e.SetMargins(PDFdocument.LeftMargin, PDFdocument.RightMargin, PDFdocument.TopMargin, PDFdocument.BottomMargin);


            PDFdocument.Open();

            Dictionary<string, object> Props = new Dictionary<string, object>();

            if (isURL == 1)
                Props["img_baseurl"] = _baseurl;

            List<IElement> p = HTMLWorker.ParseToList(sReader, st, Props, _user, _psw);

            for (int k = 0; k < p.Count; k++)
            {
                p[k] = PDFLive.ConvertHTML.HTMLFunctions.ExtendElement((IElement)p[k]);
                PDFdocument.Add((IElement)p[k]);
            }

            PDFdocument.Close();

            Byte[] Buffer = PDFfile.ToArray();
            PDFfile.Close();
            return Buffer;

        }


        #endregion       

        void RegisterFonts()
        {
            /*-----Registro las Fonts agregadas-----*/
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Tahoma.ttf", "Tahoma");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Tahoma-Bold.ttf", "Tahoma-Bold");

            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Georgia.ttf", "Georgia");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Georgia-Bold.ttf", "Georgia-Bold");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Georgia-Oblique.ttf", "Georgia-Oblique");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Georgia-BoldOblique.ttf", "Georgia-BoldOblique");

            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.LucidaSans.ttf", "LucidaSans");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.LucidaSans-Bold.ttf", "LucidaSans-Bold");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.LucidaSans-Oblique.ttf", "LucidaSans-Oblique");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.LucidaSans-BoldOblique.ttf", "LucidaSans-BoldOblique");

            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Verdana.ttf", "Verdana");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Verdana-Bold.ttf", "Verdana-Bold");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Verdana-Oblique.ttf", "Verdana-Oblique");
            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.Verdana-BoldOblique.ttf", "Verdana-BoldOblique");

            FontFactory.Register("PDFLive.iTextSharp.iTextSharp.text.pdf.fonts.c39hrp24dhtt.ttf", "CodeBar");
            /*-----Registro las Fonts agregadas-----*/

        }
        string GetContent()
        {
            String CodeSource;

            if (isURL == 1)
            {

                byte[] cont = null;

                if (_auth == true)
                {
                    cont = PDFLive.Functions.SourceDataBin(_content, _user, _psw);
                }
                else
                {
                    cont = PDFLive.Functions.SourceDataBin(_content);
                }

                //Verifico que codificacion tiene
                if (PDFLive.Functions.isUTF8(cont)) //Si es UTF-8
                {
                    CodeSource = Encoding.UTF8.GetString(cont);
                }
                else //Sino es Ansi
                {
                    CodeSource = Encoding.Default.GetString(cont);
                }

                cont = null;

                _baseurl = _content.Substring(0, _content.LastIndexOf("/") + 1);
            }
            else
            {
                CodeSource = _content;
            }

            return CodeSource;
        }
        Rectangle GetPageSize()
        {
            Rectangle Size = new Rectangle(PageSize.A4);

            if (_pagina_size_x > 0 && _pagina_size_y > 0)
            {
                Size = new Rectangle(_pagina_size_x, _pagina_size_y);
            }

            /*Orientacion de la Pagina Horizontal*/
            if (_orientacion_pagina == 1)
                Size = new Rectangle(Size.Rotate());

            return Size;

        }

    }
}
