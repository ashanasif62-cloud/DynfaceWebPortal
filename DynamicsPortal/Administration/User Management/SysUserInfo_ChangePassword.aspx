<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="SysUserInfo_ChangePassword.aspx.cs" Inherits="DynamicsPortal.SysUserInfo_ChangePassword" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
        <script type="text/javascript">
        function validateFields() {
            var isValid = true;
            var txtUserId = document.getElementById("<%=txtUserId.ClientID%>");
            var txtCurPassword = document.getElementById("<%=txtOldPassword.ClientID%>");
            var txtNewPassword = document.getElementById("<%=txtNewPassword.ClientID%>");
            var txtConNewPassword = document.getElementById("<%=txtConfirmNewPassword.ClientID%>");
            //box-shadow: 0px 0px 4px #2bff2b;
            if (!(txtUserId.value.trim() === "")) {
                if (!(txtCurPassword.value.trim() === "")) {
                    var decimal = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$/;
                    if (txtNewPassword.value.match(decimal)) {
                        if (txtNewPassword.value !== txtConNewPassword.value) {
                            isValid = false;
                            txtConNewPassword.style.boxShadow = "0px 0px 4px #E9160B";
                            txtConNewPassword.focus();
                            txtNewPassword.style.boxShadow = "0px 0px 4px #bababa";
                        }
                        else
                            txtConNewPassword.style.boxShadow = "0px 0px 4px #bababa";
                    }
                    else {
                        txtNewPassword.style.boxShadow = "0px 0px 4px #E9160B";
                        txtNewPassword.focus();
                        txtCurPassword.style.boxShadow = "0px 0px 4px #bababa";
                        isValid = false;
                    }
                }
                else {
                    txtCurPassword.style.boxShadow = "0px 0px 4px #E9160B";
                    txtCurPassword.focus();
                    txtUserId.style.boxShadow = "0px 0px 4px #bababa";
                    isValid = false;
                }
            }
            else {
                txtUserId.style.boxShadow = "0px 0px 4px #E9160B";
                txtUserId.focus();
                isValid = false;
            }
            return isValid;
        }
    </script>
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
                    <span>Old Password</span>
                </td>
                <td>
                    <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>New Password</span>
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" ToolTip="least 8characters(1lowercase, 1uppercase, 1numeric digit & 1special)"/>
                </td>
            </tr>
            <tr>
                <td>
                    <span>Confirm New Password</span>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password" ToolTip="confirm new password"/>
                </td>
            </tr>


        </table>

        <div class="action-footer">
            <asp:LinkButton ID="btnSave" runat="server" OnClientClick="return validateFields();" OnClick="btnSave_Click">Update</asp:LinkButton>
            <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Close</asp:LinkButton>
        </div>
    </div>

</asp:Content>
