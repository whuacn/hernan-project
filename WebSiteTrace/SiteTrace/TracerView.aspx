<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TracerView.aspx.cs" Inherits="SiteTrace.TracerView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:DataGrid ID="DataGrid1" runat="server" CellPadding="4" ForeColor="#333333" 
            GridLines="None">
            <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
            <EditItemStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        </asp:DataGrid>
        <asp:Button ID="btnDelete" runat="server" Text="Borrar" OnClick="Clear" />
        <asp:Button ID="btnStart" runat="server" Text="Iniciar" OnClick="Start" />
        <asp:Button ID="btnStop" runat="server" Text="Detener" OnClick="Stop" />
    </div>
    </form>
</body>
</html>
