<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSJmgAttendanceRegister_TimeSheet.aspx.cs" Inherits="DynamicsPortal.ESSJmgAttendanceRegister_TimeSheet" %>

<%@ Register Src="~/DropDownList_PayGroupPayPeriods.ascx" TagPrefix="uc1" TagName="DropDownList_PayGroupPayPeriods" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnView" runat="server" OnClick="btnViewReport_Click"><i class="mdi mdi-eye"></i> View Time Sheet</asp:LinkButton>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <div style="display: inline-block; font-size: 12px;">
        <span style="margin-right: 5px;">From Date</span>
        <asp:TextBox ID="txtFromDate" runat="server" masktype="date"></asp:TextBox>

        <span style="margin-right: 5px;">To Date</span>
        <asp:TextBox ID="txtToDate" runat="server" masktype="date"></asp:TextBox>
    </div>

    <div style="margin-top: 5px;">
        <embed id="embed01" runat="server" type="application/pdf" height="700" width="850" />
    </div>
</asp:Content>

