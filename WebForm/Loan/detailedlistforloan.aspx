<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="detailedlistforloan.aspx.cs" Inherits="RDLCReportServer.WebForm.Loan.detailedlistforloan" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </div>
        <rsweb:ReportViewer ID="RVDetailedListLoan" runat="server" Width="1306px" Height="531px">
        </rsweb:ReportViewer>
        <div id="NoDataFound" runat="server" visible="false">
            <h1 style="color: #FF0000">No Data Found</h1>           
        </div>
    </form>
</body>
</html>
