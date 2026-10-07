<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="SysUserInfo_Create.aspx.cs" Inherits="DynamicsPortal.SysUserInfo_Create" %>

<%--<%@ Register Src="~/DropDownList_AllEmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_AllEmployeeDetails" %>--%>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
<ContentTemplate>
    <div>
        <table class="form-table">
            <tr>
                <td colspan="2" style="font-size: 16px;">
                    <asp:Label ID="lblShowPassword" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>Employee Id</span>
                </td>
                <td>
                    <asp:TextBox ID="txtEmployeeId" runat="server" MaxLength="9" />
                    <%--<uc1:DropDownList_AllEmployeeDetails ID="cddlEmployeeDetails" runat="server" />--%>
                </td>
            </tr>
            <tr>
                <td>
                    <span>User Id</span>
                </td>
                <td>
                    <asp:TextBox ID="txtUserId" runat="server" MaxLength="10" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>Enabled</span>
                </td>
                <td>
                    <asp:CheckBox ID="cbUserEnabled" runat="server" Checked="true" />
                </td>
            </tr>
        </table>

        <div class="action-footer">
            <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create User</asp:LinkButton>
            <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Close</asp:LinkButton>
        </div>
    </div>
        </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
