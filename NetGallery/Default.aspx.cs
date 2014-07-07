using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string path = Request.QueryString["p"] ?? "";       
        List<NetFile> files = NetGalleryController.GetGalleryFiles(path);

        foreach (NetFile file in files)
        {
            if (file.Type == TypeFile.Folder)
                thegallery.InnerHtml += "<li><center><img src='/Images/galeria.png' style='width:100px' onclick='location.href=\"/?p=" + file.Path + "\"'  /><div class='folder-legend'>" + file.Name + "</div><center></li>";


            if (file.Type == TypeFile.Photo)
                thegallery.InnerHtml += "<li><center><a href='Photo.aspx?pth=" + file.Path + "' rel='shadowbox[photo-gallery]' title='" + file.Description + "'><img src='/Images/loader.gif' data-original='ThumbnailPhoto.aspx?p=" + file.Path + "' rel='group1' class='lazy' title='" + file.Description + "' /></a></center></li>";
        }

        List<NavBar> navs = NetGalleryController.GetNavBar(path);
        string navigation = "";
        foreach (NavBar n in navs)
        {
            if (navigation != "")
                navigation += " >";
            navigation += "<a href='" + n.Link + "' >" + n.Name + "</a>";

        }

        nav.InnerHtml = navigation;
    }
}