<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSPRSalarySlip.aspx.cs" Inherits="DynamicsPortal.ESSPRSalarySlip" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #mCSB_1_container {
            overflow: visible;
        }
    </style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnView" runat="server" OnClick="btnViewReport_Click"><i class="mdi mdi-eye"></i> View Salary Slip</asp:LinkButton>
    </div>
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

