using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            throw new NotImplementedException();
        }
        #endregion
    }
}
