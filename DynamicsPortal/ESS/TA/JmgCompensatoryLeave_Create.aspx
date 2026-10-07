<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="JmgCompensatoryLeave_Create.aspx.cs" Inherits="DynamicsPortal.JmgCompensatoryLeave_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">

        <tr>
            <td>
                <span>Worker</span>
            </td>

            <td>
                <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails" />
            </td>
        </tr>
        <tr>
            <td>
                <span>Attendance Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtProfileDate" runat="server" autocomplete="off"  masktype="date"></asp:TextBox>
            </td>
        </tr>
        <%--<tr>
            <td>
                <span>Profile</span>
            </td>

            <td>
                <asp:TextBox ID="txtProfile" runat="server" ></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Request Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestDate" runat="server" autocomplete="off"  masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Description</span>
            </td>

            <td>
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>

            </td>
        </tr>
        <%--<tr>
            <td>
                <span>WorkFlow Status</span>
            </td>

            <td>
                <asp:TextBox ID="txtWorkFlowStatus" runat="server"></asp:TextBox>

            </td>
        </tr>--%>
        

    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
