<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSPRTaxCertificate.aspx.cs" Inherits="DynamicsPortal.ESSPRTaxCertificate" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<%--<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnView" runat="server" OnClick="btnViewReport_Click"><i class="mdi mdi-eye"></i> View Tax Certificate</asp:LinkButton>
        <asp:LinkButton ID="btnEmail" runat="server" OnClick="btnEmailReport_Click"><i class="mdi mdi-eye"></i> Email Tax Certificate</asp:LinkButton>
    </div>
</asp:Content>--%>
<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
      <%--  <div style="display: inline-block;">
            <asp:LinkButton ID="btnView" Visible="false" runat="server" OnClick="btnViewReport_Click" CssClass="action-button">
                <i class="mdi mdi-eye"></i> View Tax Certificate
            </asp:LinkButton>
        </div>--%>
        <div style="display: inline-block; margin-left: 0.02in;">
            <asp:LinkButton ID="btnEmail" runat="server" OnClick="btnEmailReport_Click" CssClass="action-button">
                <i class="mdi mdi-email"></i> Email Tax Certificate
            </asp:LinkButton>
        </div>
    </div>

    <style>
        .action-button {
            text-decoration: none;
            cursor: pointer;
            padding: 2px 8px;
            background-color: transparent;
            border: none;
         color: #ffffff;   
            font-size: 14px;
        }

        .action-button:hover {
            text-decoration: underline;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
  <div style="display: inline-block; font-size: 12px;">
    <span style="margin-right: 5px;">Pay Period Year</span>
    <asp:DropDownList 
        ID="ddlPayPeriodYear" 
        runat="server" 
        style="min-width: 120px;">
    </asp:DropDownList>
</div>

<div style="margin-top: 5px;">
    <embed id="embed01" runat="server" type="application/pdf" height="700" width="850" />
</div>
</asp:Content>

