<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ex01_Simple.aspx.cs" Inherits="GoogleAnalytics.Examples.Ex01_Simple" %>

<%@ Register src="../Controls/report.ascx" tagname="report" tagprefix="ga" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <!-- Must have scripts in correct order -->
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/js/jquery-1.9.1.min.js" />
                <asp:ScriptReference Path="https://www.google.com/jsapi" />
                <asp:ScriptReference Path="~/js/chart-config.js" />
            </Scripts>
        </asp:ScriptManager>

        <!-- All reports should be added between the ScriptManager and the init-analytics script -->
        <ga:report ID="report_Simple" runat="server" />

        <!-- do not forget to add init-analytics script as last script at end of body -->
        <script type="text/javascript" src="/js/init-analytics.js"></script>
    </div>
    </form>
</body>
</html>
