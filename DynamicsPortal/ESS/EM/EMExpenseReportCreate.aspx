<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="EMExpenseReportCreate.aspx.cs" Inherits="DynamicsPortal.ESS.EM.EMExpenseReportCreate" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <tr>
            <td>
                <span>Purpose</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlPurpose" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Location</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlLocation" runat="server"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <table ID="expenseReportCreatetable" runat="server" class="form-table">
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="showOverlay();" OnClick="btnSave_Click">Create</asp:LinkButton>

        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
