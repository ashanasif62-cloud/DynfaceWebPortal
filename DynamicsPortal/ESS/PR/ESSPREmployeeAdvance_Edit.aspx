<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="ESSPREmployeeAdvance_Edit.aspx.cs" Inherits="DynamicsPortal.ESS.PR.ESSPREmployeeAdvance_Edit" %>
<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
     <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
 <ContentTemplate>
    <table class="form-table">
        <tr>
            <td>
                <span>Employee Id</span>
            </td>

            <td>
                <uc1:DropDownList_EmployeeDetails ID="cddlEmployeeDetails" runat="server" OnEmployeeSelected="cddlEmployeeDetails_OnEmployeeSelected" />
            </td>
        </tr>
        <%--        <tr>
            <td>
                <span>Employee Name</span>
            </td>

            <td>
                <asp:TextBox ID="txtEmployeeName" Enabled="false" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Request Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestDate" runat="server" autocomplete="off" masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr style=" display:none !important;">
            <td>
                <span>Payment Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtPaymentDate" OnTextChanged="onRequestedDateModified" AutoPostBack="true" runat="server" autocomplete="off" TextMode="Date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Advance Type Code</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlAdvanceTypeCode" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Advance Description</span>
            </td>

            <td>
                <asp:TextBox ID="txtAdvanceDescription" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Advance Amount</span><%--<span style="font-size:8px; padding-left:2px; color:crimson;">up to 50% of Salary</span>--%>
            </td>

            <td>
                <asp:TextBox ID="txtAdvanceAmount" runat="server" masktype="number"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Currency</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlCurrency" runat="server" Enabled="false"></asp:DropDownList>
            </td>
        </tr>
      

    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Update</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnUpdate_Submit_Click">Update & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
         </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
