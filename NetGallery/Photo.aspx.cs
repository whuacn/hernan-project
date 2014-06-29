using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Photo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string path = Request.QueryString["pth"];
        string Extension = Path.GetExtension(path).Replace(".","").Trim();
        String contentType = "image/" + Extension;
        Response.ContentType = contentType;
        Response.BinaryWrite(NetGalleryController.GetPhoto(path));
        Response.Flush();
    }
}