using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;

namespace Sipro
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblName.Text = "Hello " + Context.User.Identity.Name + ". SV: " + Request.ServerVariables["AUTH_USER"];
            lblAuthType.Text = "You were authenticated using " + Context.User.Identity.AuthenticationType + ".";


            Sipro.Providers.AuthenticationProvider auth = ConfigurationManager.GetSection("Auth") as Sipro.Providers.AuthenticationProvider;

            Response.Write(auth.Name);
            Response.Write("<br>");
            Response.Write(auth.Test);

        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}