using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFLive
{
    interface IHTMLConvert
    {
        #region Properties
        string content
        {
            get;
            set;
        }
        int isURL
        {
            get;
            set;
        }
        int pagenumber
        {
            get;
            set;
        }
        int fechaemision
        {
            get;
            set;
        }
        //Orientacion de la pagina
        int orientacion
        {
            get;
            set;
        }
        //Ancho personalizado de la pagina
        float sizex
        {
            get;
            set;
        }
        //Alto personalizado de la pagina
        float sizey
        {
            get;
            set;
        }

        #region Margins
        float LeftMargin
        {
            get;
            set;
        }
        float RightMargin
        {
            get;
            set;
        }
        float TopMargin
        {
            get;
            set;
        }
        float BottonMargin
        {
            get;
            set;
        }
        //Si el margen lo pongo antes o despues del header
        int TopMarginAfter
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Public Methods
        void setCredentials(string user, string psw);
        Byte[] Convert();
        #endregion

    }
}
