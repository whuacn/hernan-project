using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

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

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //Label1.Text = "Grid Refreshed at: " + DateTime.Now.ToLongTimeString();
            DataGrid1.DataSource = WebTrace.Tracer.TracerList;
            DataGrid1.DataBind();
        }

        protected void DataGrid1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            DetailsView1.DataSource = WebTrace.Tracer.TracerList;
            DetailsView1.DataBind();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#currentdetail').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);
            /*
            if (e.CommandName.Equals("detail"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string code = DataGrid1.DataKeys[index].Value.ToString();

                IEnumerable<DataRow> query = from i in dt.AsEnumerable()
                                             where i.Field<String>("Code").Equals(code)
                                             select i;
                DataTable detailTable = query.CopyToDataTable<DataRow>();
                DetailsView1.DataSource = detailTable;
                DetailsView1.DataBind();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#currentdetail').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                           "ModalScript", sb.ToString(), false);
            
            }*/
        }
    }
}