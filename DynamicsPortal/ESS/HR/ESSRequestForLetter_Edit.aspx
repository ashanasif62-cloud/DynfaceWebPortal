<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="ESSRequestForLetter_Edit.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSRequestForLetter_Edit" %>
<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <tr>
            <td>
                <span>Employee</span>
            </td>

            <td>
                <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails" OnEmployeeSelected="cddlEmployeeDetails_OnEmployeeSelected" />
               
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
  
        <tr>
            <td>
                <span>Requested For</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlRequestedFor" runat="server"></asp:DropDownList>
            </td>
        </tr>
    
        <tr>
            <td>
                <span>Remarks</span>
            </td>

            <td>
                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">OK</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
