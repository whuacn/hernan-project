using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ThumbnailPhoto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string path = Request.QueryString["p"];
        String contentType = "image/jpg";
        Response.ContentType = contentType;
        Response.BinaryWrite(NetGalleryController.GetThumbnailPhoto(path));
        Response.Flush();
    }
}