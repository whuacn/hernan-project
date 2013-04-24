using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiteTrace
{
    public partial class TracerView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataGrid1.DataSource = WebTrace.Tracer.TracerList;
            DataGrid1.DataBind();
        }

        protected void Clear(object sender, EventArgs e)
        {
            WebTrace.Tracer.TracerClear();
        }
        protected void Start(object sender, EventArgs e)
        {
            HttpContext.Current.Session["TracerActive"] = "On";
        }
        protected void Stop(object sender, EventArgs e)
        {
            HttpContext.Current.Session["TracerActive"] = "Off";
        }
    }
}