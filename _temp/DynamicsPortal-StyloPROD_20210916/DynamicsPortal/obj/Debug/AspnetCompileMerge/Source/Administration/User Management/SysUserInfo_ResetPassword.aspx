<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="SysUserInfo_ResetPassword.aspx.cs" Inherits="DynamicsPortal.SysUserInfo_ResetPassword" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <table class="form-table">
            <tr>
                <td>
                    <span>User Id</span>
                </td>
                <td>
                    <asp:TextBox ID="txtUserId" runat="server" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>New Password</span>
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>Confirm New Password</span>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password" />
                </td>
            </tr>


        </table>

        <div class="action-footer">
            <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Reset Password</asp:LinkButton>
            <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
        </div>
    </div>

</asp:Content>
