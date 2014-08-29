<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="/Content/themes/start/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" />
    <script src="/Scripts/jquery-2.0.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="/Scripts/modernizr-2.6.2.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts-4.0.1/js/highcharts-all.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts-4.0.1/js/modules/funnel.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts-4.0.1/js/modules/solid-gauge.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts-4.0.1/js/modules/drilldown.js" type="text/javascript"></script>


</head>
<body>
    <form id="form1" runat="server">

            <div id="DivContent" runat="server">

            </div>

        
    </form>
</body>
</html>
<script type="text/javascript">

    $(document).ready(function () {
        $(".accordion").accordion({
            heightStyle: "content",
            collapsible: true
        });
    });
</script>