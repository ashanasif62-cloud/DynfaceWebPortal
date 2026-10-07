<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="EditConfig.aspx.cs" Inherits="YourNamespace.EditConfig" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Edit Client Configuration</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="/distribution/vendor/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/distribution/vendor/font-awesome/css/font-awesome.min.css" />

    <style>
        body {
            background-color: #f5f7fa;
            font-family: 'Segoe UI', sans-serif;
        }
        .config-container {
            max-width: 700px;
            margin: 50px auto;
            background: #fff;
            border-radius: 10px;
            padding: 30px 40px;
            box-shadow: 0px 4px 12px rgba(0,0,0,0.1);
        }
        .config-container h2 {
            text-align: center;
            margin-bottom: 25px;
            font-weight: bold;
            color: #0047AB;
        }
        .form-label {
            font-weight: 600;
            margin-top: 10px;
        }
        .btn-save {
            width: 100%;
            padding: 12px;
            font-size: 16px;
            font-weight: bold;
            border-radius: 6px;
            background-color: #0047AB;
            border: none;
            color: white;
            transition: background-color 0.3s ease-in-out;
        }
        .btn-save:hover {
            background-color: #003580;
        }
        .message-label {
            display: block;
            margin-top: 15px;
            text-align: center;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="config-container">
            <h2><i class="fa fa-cogs"></i> Edit Client Configuration</h2>

            <div class="mb-3">
                <label class="form-label" for="txtTitle">Title</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
                <label class="form-label" for="txtActiveDirectoryResource">Resource URL</label>
                <asp:TextBox ID="txtActiveDirectoryResource" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
                <label class="form-label" for="txtActiveDirectoryTenant">Tenant ID</label>
                <asp:TextBox ID="txtActiveDirectoryTenant" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
                <label class="form-label" for="txtActiveDirectoryClientAppId">Client ID</label>
                <asp:TextBox ID="txtActiveDirectoryClientAppId" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
                <label class="form-label" for="txtActiveDirectoryClientAppSecret">Client Secret</label>
                <asp:TextBox ID="txtActiveDirectoryClientAppSecret" runat="server" CssClass="form-control" TextMode="Password"/>
            </div>

            <asp:Button ID="btnSave" runat="server" CssClass="btn-save" Text="💾 Save Configuration" OnClick="btnSave_Click" />

            <asp:Label ID="lblMessage" runat="server" CssClass="message-label" ForeColor="Green" />
        </div>
    </form>
</body>
</html>
