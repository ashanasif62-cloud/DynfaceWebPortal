<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHRHelpDeskRequest_Create.aspx.cs" Inherits="DynamicsPortal.ESSHRHelpDeskRequest_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <%--        <tr>
            <td>
                <span>Help Desk Request Id</span>
            </td>

            <td>
                <asp:TextBox ID="txtESSHRHelpDeskRequestId" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Employee</span>
            </td>

            <td>
                <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails" />
                <%--<asp:DropDownList ID="ddlEmployee" runat="server"></asp:DropDownList>--%>
            </td>
        </tr>
        <%--        <tr>
            <td>
                <span>Division</span>
            </td>

            <td>
                <asp:TextBox ID="txtESSDivision" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Type of Request</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlESSTypeOfProblem" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Request Started On</span>
            </td>

            <td>
                <asp:TextBox ID="txtESSDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Transaction Status</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlTransactionStatus" runat="server" Enabled="false"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Detail</span>
            </td>

            <td>
                <asp:TextBox ID="txtDetail" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
