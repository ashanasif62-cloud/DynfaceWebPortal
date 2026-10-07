<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHRRequestForLetter_Create.aspx.cs" Inherits="DynamicsPortal.ESSHRRequestForLetter_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .required-field {
            border: 1px solid #ccc;
            transition: 0.2s;
        }

        .required-field.invalid {
            border: 1px solid red;
            outline: none;
        }
    </style>
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
      <asp:UpdatePanel ID="updHelpDeskForm" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
    <table class="form-table">
        <%--        <tr>
            <td>
                <span>Certificate Id</span>
            </td>

            <td>
                <asp:TextBox ID="txtCertificateId" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Employee</span>
            </td>

            <td>
                <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails" OnEmployeeSelected="cddlEmployeeDetails_OnEmployeeSelected" />
                <%--<asp:DropDownList ID="ddlEmployee" runat="server"></asp:DropDownList>--%>
            </td>
        </tr>
       <%-- <tr>
            <td>
                <span>Job</span>
            </td>

            <td>
                <asp:TextBox ID="txtJobId" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Department</span>
            </td>

            <td>
                <asp:TextBox ID="txtDepartment" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>--%>
            <td>
                <span>Requested Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Certificate Type</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlCertificateType" runat="server"></asp:DropDownList>
            </td>
        </tr>
     <%--   <tr>
            <td>
                <span>Authentication</span>
            </td>

            <td>
                <asp:TextBox ID="txtAuthentication" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Requested For</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlRequestedFor" runat="server"></asp:DropDownList>
            </td>
        </tr>
       <%-- <tr>
            <td>
                <span>Reason</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlReason" runat="server"></asp:DropDownList>
            </td>
        </tr>--%>
        <%--<tr>
            <td>
                <span>Remarks</span>
            </td>

            <td>
                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
                    <td>
                        <label for="txtRemarks">Remarks <span style="color:red">*</span></label>
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                style="width:100%; min-height:120px; font-size:12px; box-sizing:border-box;"></asp:TextBox>
                    </td>
                </tr>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" OnClientClick="return validateRemarks();" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" OnClientClick="return validateRemarks();" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
          </ContentTemplate>
</asp:UpdatePanel>
       <script>
           function validateRemarks() {
               var txt = document.getElementById('<%= txtRemarks.ClientID %>');
            var errMsg = document.getElementById('remarksError');

            if (!txt.value.trim()) {
                txt.classList.add("invalid");
                errMsg.style.display = 'inline';
                return false;
            } else {
                txt.classList.remove("invalid");
                errMsg.style.display = 'none';
                return true;
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            var txt = document.getElementById('<%= txtRemarks.ClientID %>');

            txt.addEventListener("input", function () {
                if (txt.value.trim()) {
                    txt.classList.remove("invalid");
                    document.getElementById('remarksError').style.display = 'none';
                }
            });
        });
    </script>
</asp:Content>
