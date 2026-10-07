<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomErrorPage.aspx.cs" Inherits="DynamicsPortal.ErrorPages.CustomErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Div1" style="display: block;" class="pr-alert pr-error" runat="server">
            <h2>An error occurred, Please try again.</h2>
        </div>
    </form>
</body>
</html>
