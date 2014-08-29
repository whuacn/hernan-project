using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BarImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Bitmap bmp = new Bitmap(600, 250);
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.LavenderBlush);

        Rectangle rect = new Rectangle(0, 0, 200, 575);

        LinearGradientBrush lgBrush = new LinearGradientBrush(rect, Color.Red, Color.Pink, LinearGradientMode.Horizontal);
        Pen gradientPen = new Pen(lgBrush, 22);

        g.DrawLine(gradientPen, 25, 100, 125, 100);

        gradientPen.Brush = new LinearGradientBrush(rect, Color.LawnGreen, Color.DarkGreen, LinearGradientMode.Horizontal);
        g.DrawLine(gradientPen, 25, 150, 150, 150);

        gradientPen.Brush = new LinearGradientBrush(rect, Color.YellowGreen, Color.Yellow, LinearGradientMode.Horizontal);
        g.DrawLine(gradientPen, 25, 200, 175, 200);

        /*
        g.DrawLine(gradientPen, 200, 50, 200, 200);

        gradientPen.Brush = new LinearGradientBrush(rect, Color.LawnGreen, Color.DarkGreen, LinearGradientMode.Vertical);
        g.DrawLine(gradientPen, 250, 50, 250, 200);

        gradientPen.Brush = new LinearGradientBrush(rect, Color.DarkBlue, Color.LightBlue, LinearGradientMode.Vertical);
        g.DrawLine(gradientPen, 300, 50, 300, 200);
        */
        bmp.Save(Response.OutputStream, ImageFormat.Jpeg);

        Response.End();
        // Cleanup
        g.Dispose();
        bmp.Dispose();

        /*String path = Server.MapPath("~/Image/GradientColorVerticalLine.jpg");
        bmp.Save(path, ImageFormat.Jpeg);

        Image1.ImageUrl = "~/Image/GradientColorVerticalLine.jpg";
        g.Dispose();
        bmp.Dispose();*/
    }

    void Eclipse()
    {

        Bitmap oCanvas = new Bitmap(200, 150);
        Graphics g = Graphics.FromImage(oCanvas);
        g.Clear(Color.White);
        g.DrawEllipse(Pens.Red, 10, 10, 150, 100);

        // Now, we only need to send it // to the client
        Response.ContentType = "image/jpeg";
        oCanvas.Save(Response.OutputStream, ImageFormat.Jpeg);
        Response.End();

        // Cleanup
        g.Dispose();
        oCanvas.Dispose();
    }
}