<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="EssWorkerBankAccount_Create.aspx.cs" Inherits="DynamicsPortal.EssWorkerBankAccount_Create" %>
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
                <span>Account Identification</span>
            </td>

            <td>
                <asp:TextBox ID="txtAccountIdentification" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Name</span>
            </td>

            <td>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Bank Account Number</span>
            </td>

            <td>
                <asp:TextBox ID="txtBankAccountNumber" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>IBAN</span>
            </td>

            <td>
                <asp:TextBox ID="txtIBAN" runat="server" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Branch Number</span>
            </td>

            <td>
                <asp:TextBox ID="txtBranchNumber" runat="server" ></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td>
                <span>Branch Name</span>
            </td>

            <td>
                <asp:TextBox ID="txtBranchName" runat="server"></asp:TextBox>

            </td>
        </tr>
        
        <tr>
            <td>
                <span>Account Title</span>
            </td>

            <td>
                <asp:TextBox ID="txtAccountTitle" runat="server"></asp:TextBox>
            </td>
        </tr>
        
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>


