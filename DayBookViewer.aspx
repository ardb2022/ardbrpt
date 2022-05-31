<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DayBookViewer.aspx.cs" Inherits="RDLCReportServer.DayBookViewer" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h5>Day Book</h5>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="928px">
        </rsweb:ReportViewer>
    </form>
</body>
</html>
