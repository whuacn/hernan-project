<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TracerView.aspx.cs" Inherits="SiteTrace.TracerView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery.js"></script>
    <script src="Scripts/bootstrap.js" type="text/javascript"></script>
</head>
<body>
    <h3 id="H1">Tracer Viewer</h3>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div>
            <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="4000">
            </asp:Timer>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
            <ContentTemplate>
                <div style="overflow: auto; width: 100%; height: 400px">
                <asp:GridView  ID="DataGrid1" runat="server" CellPadding="3" AutoGenerateColumns="False"
                    Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                    OnRowCommand="DataGrid1_RowCommand" CssClass="table table-hover table-striped" >
                    <Columns>
                        <asp:ButtonField CommandName="detail" ControlStyle-CssClass="btn btn-info" ButtonType="Button"
                            Text="Detalle" HeaderText="Detalle" />
                        <asp:BoundField DataField="Date" HeaderText="Fecha"></asp:BoundField>
                        <asp:BoundField DataField="Page" HeaderText="Pagina"></asp:BoundField>
                        <asp:BoundField DataField="Message" HeaderText="Mensaje"></asp:BoundField>
                        <asp:BoundField DataField="Type" HeaderText="Tipo"></asp:BoundField>
                    </Columns>
                </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnDelete" runat="server" Text="Borrar" OnClick="Clear" CssClass="btn btn-danger" />
        <asp:Button ID="btnStart" runat="server" Text="Iniciar" OnClick="Start"  CssClass="btn btn-primary"/>
        <asp:Button ID="btnStop" runat="server" Text="Detener" OnClick="Stop"  CssClass="btn btn-primary"/>
        <div id="currentdetail" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="myModalLabel">
                    Detalle</h3>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <div style="overflow: auto; width: 100%; height: 350px">
                        <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered table-hover"
                            BackColor="White" ForeColor="Black" FieldHeaderStyle-Wrap="false" FieldHeaderStyle-Font-Bold="true"
                            FieldHeaderStyle-BackColor="LavenderBlush" FieldHeaderStyle-ForeColor="Black"
                            BorderStyle="Groove" AutoGenerateRows="False">
                            <Fields>
                                <asp:BoundField DataField="Page" HeaderText="Page" />
                            </Fields>
                        </asp:DetailsView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DataGrid1" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">
                        Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
