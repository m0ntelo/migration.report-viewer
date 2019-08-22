<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" 
    namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Relaóriorio</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>   
        <rsweb:ReportViewer ID="ReportViewer" runat="server" ProcessingMode="Remote"></rsweb:ReportViewer>
        <asp:Label ID="lblMsg" runat="server" Text="Dados Inexistentes" Visible="false"/>
    </form>
</body>
</html>