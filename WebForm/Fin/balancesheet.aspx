<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="balancesheet.aspx.cs" Inherits="RDLCReportServer.WebForm.Fin.balancesheet" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="RV_BS" runat="server" Height="494px" Width="883px">
        </rsweb:ReportViewer>
        <div id="NoDataFound" runat="server" visible="false">
            <h1 style="color: #FF0000">No Data Found</h1>           
        </div>
    </form>
</body>
</html>
