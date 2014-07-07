<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="HandheldFriendly" content="True" />
<meta name="MobileOptimized" content="320" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1" />
    <title></title>
    <link rel="stylesheet" href="/shadowbox/shadowbox.css" />
    <link rel="stylesheet" href="/Styles/app.css" />
    <link rel="stylesheet" href="/Styles/jPages.css" />
    <script src="/Scripts/jquery-1.7.1.min.js"></script>
    <script src="/Scripts/jquery.lazyload.js"></script>    
    <script src="/Scripts/jPages.js"></script>
    <script src="/shadowbox/shadowbox.js"></script>
    <script>
        Shadowbox.init();
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="navbar">
            <h1>Superintendencia de Riesgos del Trabajo</h1>
            <br /><p id="nav" runat="server"></p>    
        </div>
        <br /><br />
        <div id="content">
            <!-- navigation panel -->
            <div class="holder"> </div>
            <ul class="gallery" runat="server" id="thegallery"></ul>
        </div>
    </div>
    </form>

        <script>
            $(function () {

                 $("img.lazy").lazyload({
                     event: "turnPage",
                     effect: "fadeIn"
                 });
                 
                //$('.tl').glisse({ changeSpeed: 550, fullscreen: false });

                $("div.holder").jPages({
                    containerID: "thegallery",
                    animation: "fadeInUp",
                    callback: function (pages, items) {
                        /* lazy load current images */
                        items.showing.find("img").trigger("turnPage");
                        /* lazy load next page images */
                        items.oncoming.find("img").trigger("turnPage")
                    }
                });
            });
           /* $('#changefx').change(function () {
                var val = $(this).val();
                $('.tl').each(function () {
                    $(this).data('glisse').changeEffect(val);
                });
            });*/
         
        </script>
</body>
</html>
