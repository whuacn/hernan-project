<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="Sipro.Logon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:Label ID="Label1" Runat="server" >Domain:</asp:Label>
      <asp:TextBox ID="txtDomain" Runat="server" ></asp:TextBox><br>    
      <asp:Label ID="Label2" Runat="server" >Username:</asp:Label>
      <asp:TextBox ID=txtUsername Runat="server" ></asp:TextBox><br>
      <asp:Label ID="Label3" Runat="server" >Password:</asp:Label>
      <asp:TextBox ID="txtPassword" Runat="server" TextMode=Password></asp:TextBox><br>
      <asp:Button ID="btnLogin" Runat="server" Text="Login" OnClick="Login_Click"></asp:Button><br>
      <asp:Label ID="errorLabel" Runat="server" ForeColor=#ff3300></asp:Label><br>
      <asp:CheckBox ID=chkPersist Runat="server" Text="Persist Cookie" />    
    </div>
    </form>
</body>
</html>
