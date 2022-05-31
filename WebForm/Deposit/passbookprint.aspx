<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="passbookprint.aspx.cs" Inherits="RDLCReportServer.WebForm.Deposit.passbookprint" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #form1 {
            width: 1023px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="RV_Passbook" runat="server" Height="468px" Width="891px">
        </rsweb:ReportViewer>
        <div id="NoDataFound" runat="server" visible="false">
            <h1 style="color: #FF0000">No Data Found</h1>           
        </div>
    </form>
</body>
</html>
