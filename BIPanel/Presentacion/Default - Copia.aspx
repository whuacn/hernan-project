<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default - Copia.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="/Content/themes/start/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" />
    <script src="/Scripts/jquery-2.0.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/modernizr-2.6.2.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts-4.0.1/js/highcharts-all.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts-4.0.1/js/modules/funnel.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts-4.0.1/js/modules/solid-gauge.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts-4.0.1/js/modules/drilldown.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div style="display:inline-block">
        <asp:Literal ID="ltrChart4" runat="server"></asp:Literal>    
    </div>
    <div style="display:inline-block">
        <asp:Literal ID="ltrChart5" runat="server"></asp:Literal>    
    </div>
    <div style="display:inline-block">
        <asp:Literal ID="ltrChart6" runat="server"></asp:Literal>    
    </div>
    <br />
    <div style="display:inline-block">
        <asp:Literal ID="ltrChart" runat="server"></asp:Literal>    
    </div>
    <br />
    <div style="display:inline-block">
        <asp:Literal ID="ltrChart2" runat="server"></asp:Literal>    
    </div>
        
    <div style="display:inline-block">
        <asp:Literal ID="ltrChart3" runat="server"></asp:Literal>    
    </div>
    <br />
    <div style="display:inline-block">
        <asp:Image ImageUrl="~/BarImage.aspx" runat="server" />
    </div>
    </div>
    </form>
</body>
</html>
