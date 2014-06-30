<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        NetGalleryController.StorePath = System.Configuration.ConfigurationManager.AppSettings.Get("StorePath").Replace("\\", "/");
        try
        {
            NetGalleryController.PhysicalPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get("StorePath"));
        }
        catch (Exception)
        {
            
            NetGalleryController.PhysicalPath = System.Configuration.ConfigurationManager.AppSettings.Get("StorePath").Replace("\\","/");
        }

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
