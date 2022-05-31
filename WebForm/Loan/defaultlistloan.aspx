<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="defaultlistloan.aspx.cs" Inherits="RDLCReportServer.WebForm.Loan.defaultlistloan" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="RV_ScriptManager" runat="server">
            </asp:ScriptManager>
        </div>
        <rsweb:ReportViewer ID="RV_DefaultList" runat="server" Height="605px" Width="990px">
        </rsweb:ReportViewer>
        <div id="NoDataFound" runat="server" visible="false">
            <h1 style="color: #FF0000">No Data Found</h1>           
        </div>
    </form>
</body>
</html>
