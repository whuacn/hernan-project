<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        NetGalleryController.StorePath = System.Configuration.ConfigurationManager.AppSettings.Get("StorePath");
        NetGalleryController.PhysicalPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get("StorePath"));
    }
    
    void Application_End(object sender, EventArgs e) 
    {

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 

    }

    void Session_Start(object sender, EventArgs e) 
    {

    }

    void Session_End(object sender, EventArgs e) 
    {

    }
       
</script>
