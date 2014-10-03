using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenXmlPowerTools;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        WordConverter w = new WordConverter();
        //Response.Write(w.xHtmlConvert(@"D:\Desarrollo\hernan-project\DocViewer\Ejemplos\TP.docx"));

        //Response.Write(w.xHtmlConvert(@"D:\Desarrollo\hernan-project\DocViewer\Ejemplos\Manual.docx"));

        Response.Write(w.xHtmlConvert(@"D:\Desarrollo\hernan-project\DocViewer\Ejemplos\norma.docx"));

        //Response.Write(w.xHtmlConvert(@"D:\Desarrollo\hernan-project\DocViewer\Ejemplos\Especificacion.docx"));

        //Response.Write(w.xHtmlConvert(@"D:\Desarrollo\hernan-project\DocViewer\Ejemplos\Especificacion2.docx"));
    }
}