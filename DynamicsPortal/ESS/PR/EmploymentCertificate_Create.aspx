<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master"
    CodeBehind="EmploymentCertificate_Create.aspx.cs"
    Inherits="DynamicsPortal.ESS.PR.EmploymentCertificate_Create" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">

    <table class="form-table">
        
        <tr>
            <td><span>Employee</span></td>
            <td>
                <asp:TextBox ID="txtEmployee" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td><span>Job</span></td>
            <td>
                <asp:TextBox ID="txtJob" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>

      
        <tr>
            <td><span>Department</span></td>
            <td>
                <asp:TextBox ID="txtDepartment" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>

        
        <tr>
            <td><span>Requested Date</span></td>
            <td>
                <asp:TextBox ID="txtRequestedDate" runat="server" autocomplete="off" masktype="date" TextMode="Date"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td><span>Certificate Type</span></td>
            <td>
                <asp:DropDownList ID="ddlCertificateType" runat="server">
                    <asp:ListItem Text="-- Select Type --" Value=""></asp:ListItem>
                    <asp:ListItem Text="Experience Certificate" Value="Experience"></asp:ListItem>
                    <asp:ListItem Text="Employment Certificate" Value="Employment"></asp:ListItem>
                    <asp:ListItem Text="Salary Certificate" Value="Salary"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

  
        <tr>
            <td><span>Requested For</span></td>
            <td>
                <asp:DropDownList ID="ddlRequestedFor" runat="server">
                    <asp:ListItem Text="-- Select Option --" Value=""></asp:ListItem>
                    <asp:ListItem Text="Visa Purpose" Value="Visa"></asp:ListItem>
                    <asp:ListItem Text="Bank Loan" Value="BankLoan"></asp:ListItem>
                    <asp:ListItem Text="Personal Record" Value="PersonalRecord"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

        <!-- Remarks -->
        <tr>
            <td><span>Remarks</span></td>
            <td>
                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </td>
        </tr>
    </table>

    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">OK</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>

</asp:Content>
