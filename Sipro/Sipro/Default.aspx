<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Sipro.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Label ID="lblName" Runat="server" /><br>
      <asp:Label ID="lblAuthType" Runat="server" /><br />
      <asp:Button ID="btnSalir" runat="server" onclick="btnSalir_Click" ToolTip="Salir del sistema" Text="Salir" />
    </div>
    </form>
</body>
</html>
