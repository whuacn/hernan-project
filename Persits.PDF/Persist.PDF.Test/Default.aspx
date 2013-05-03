<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Persist.PDF.Test._Default" %>
<%@ Import Namespace="Persits.PDF" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script runat="server" language="c#">
    
void Page_Load(Object Source, EventArgs E)
{

    PdfManager objPdf = new PdfManager();
    PdfDocument objDoc = objPdf.CreateDocument();
    objDoc.ImportFromUrl("http://es.wikipedia.org");

    // Save to HTTP stream for instant viewing by user        
   objDoc.SaveHttp( "filename=test.pdf");
}
                                                                                
</script>

    <h2>
        ASP.NET
    </h2>
    <p>
        Para obtener más información acerca de ASP.NET, visite <a href="http://www.asp.net" title="Sitio web de ASP.NET">www.asp.net</a>.
    </p>
    <p>
        También puede encontrar <a href="http://go.microsoft.com/fwlink/?LinkID=152368"
            title="Documentación de ASP.NET en MSDN">documentación sobre ASP.NET en MSDN</a>.
    </p>
</asp:Content>
