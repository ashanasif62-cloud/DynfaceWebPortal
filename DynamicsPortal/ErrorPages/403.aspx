<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="403.aspx.cs" Inherits="DynamicsPortal.ErrorPages._403" %>
<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .page-contents>.card.card-header{
            display: none !important;
        }
        .action-panel{
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
       <h2 style="padding: 20px;"> Access denied! Authentication is required to access this resource.</h2>
</asp:Content>