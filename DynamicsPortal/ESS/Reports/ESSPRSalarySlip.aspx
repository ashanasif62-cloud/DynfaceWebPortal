<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSPRSalarySlip.aspx.cs" Inherits="DynamicsPortal.ESSPRSalarySlip" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #mCSB_1_container {
            overflow: visible;
        }
    </style>
</asp:Content>

<%--<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
       <asp:LinkButton ID="btnView" runat="server" OnClick="btnViewReport_Click"><i class="mdi mdi-eye"></i> View Salary Slip</asp:LinkButton>
        <asp:LinkButton ID="btnEmail" runat="server" OnClick="btnEmailReport_Click"><i class="mdi mdi-eye"></i> Email Salary Slip</asp:LinkButton>
    </div>
</asp:Content>--%>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <div style="display: inline-block;">
            <asp:LinkButton ID="btnView" runat="server" OnClick="btnViewReport_Click" CssClass="action-button">
                <i class="mdi mdi-eye"></i> View Salary Slip
            </asp:LinkButton>
        </div>
        <div style="display: inline-block; margin-left: 0.02in;">
            <asp:LinkButton ID="btnEmail" runat="server" OnClick="btnEmailReport_Click" CssClass="action-button">
                <i class="mdi mdi-email"></i> Email Salary Slip
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
    <div style="display: inline-block; overflow: visible;">
        <span style="font-size: 12px; margin-right: 5px;">Pay Period Code</span>
        <asp:DropDownList ID ="ddlPayPeriodCode" runat="server" AutoPostBack="false" />
    </div>
    <div style="margin-top: 5px;">
        <embed id="embed01" runat="server" type="application/pdf" height="700" width="850" />
    </div>
</asp:Content>

