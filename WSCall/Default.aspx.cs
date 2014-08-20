using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WSCall;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
     
        ltime.LocalTime lo = new ltime.LocalTime();
        Response.Write(lo.LocalTimeByZipCode("33101"));

        WebService ws = new WebService("http://www.ripedev.com/webservices/localtime.asmx", "LocalTimeByZipCode", "http://www.ripedev.com/");
        ws.Params.Add("ZipCode", "33101");
        ws.Invoke();
        Response.Write(ws.ResultString);

        ws = new WebService("http://wsf.cdyne.com/WeatherWS/Weather.asmx", "GetCityForecastByZIP", "http://ws.cdyne.com/WeatherWS/");
        ws.Params.Add("ZIP", "33101");
        ws.AddCredentials("abc", "123");
        ws.Invoke();
        Response.Write(ws.ResultString);
    }
}