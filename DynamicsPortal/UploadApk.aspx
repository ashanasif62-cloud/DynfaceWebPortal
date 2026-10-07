<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadApk.aspx.cs" Inherits="DynamicsPortal.UploadApk" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Upload APK</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f6f9;
            margin: 0;
            padding: 0;
        }

        .upload-container {
            max-width: 500px;
            margin: 80px auto;
            padding: 30px;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 6px 20px rgba(0,0,0,0.1);
            text-align: center;
        }

        h2 {
            margin-bottom: 20px;
            color: #333;
        }

        .file-input {
            margin: 20px 0;
        }

        input[type="file"] {
            display: block;
            margin: 0 auto 15px auto;
        }

        .btn {
            background-color: #0078d7;
            color: #fff;
            border: none;
            padding: 12px 25px;
            font-size: 16px;
            border-radius: 6px;
            cursor: pointer;
            transition: background 0.3s ease;
        }

        .btn:hover {
            background-color: #005a9e;
        }

        .status {
            margin-top: 15px;
            font-size: 14px;
        }

        .success {
            color: green;
        }

        .error {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <div class="upload-container">
            <h2>Upload Mobile APK</h2>
            
            <asp:FileUpload ID="fileUploadApk" CssClass="file-input" runat="server" />
            <asp:Button ID="btnUpload" CssClass="btn" runat="server" Text="Upload APK" OnClick="btnUpload_Click" />

            <asp:Label ID="lblStatus" CssClass="status" runat="server" />
        </div>
    </form>
</body>
</html>