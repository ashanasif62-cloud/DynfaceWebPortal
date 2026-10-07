<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSPREmployeeLoanReq_Create.aspx.cs" Inherits="DynamicsPortal.ESSPREmployeeLoanReq_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>
<%--<%@ Register Src="~/DropDownList_AllEmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_AllEmployeeDetails" %>--%>


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
                <%--<asp:DropDownList ID="ddlEmployee" runat="server"></asp:DropDownList>--%>
            </td>
        </tr>
        <tr>
            <td>
                <span>Request Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>
<%----%>        <tr>
            <td>
                <span>Guarantor 1 ID</span>
            </td>

            <td>
                <asp:TextBox ID="txtGuarantor1" runat="server" masktype="number"></asp:TextBox>
                <%--<uc1:DropDownList_EmployeeDetails runat="server" ID="cddlGuarantor12" />--%>
                <%--<uc1:DropDownList_AllEmployeeDetails runat="server" ID="cddlGuarantor1" />--%>
            </td>
        </tr>
        <tr>
            <td>
                <span>Guarantor 2 ID</span>
            </td>

            <td>
                <asp:TextBox ID="txtGuarantor2" runat="server" masktype="number"></asp:TextBox>
                <%--<uc1:DropDownList_EmployeeDetails runat="server" ID="cddlGuarantor22" />--%>
                <%--<uc1:DropDownList_AllEmployeeDetails runat="server" ID="cddlGuarantor2" />--%>
            </td>
        </tr>
        <tr>
            <td>
                <span>Payment Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtPaymentDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Loan Type Code</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlAdvanceTypeCode" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Loan Amount</span><%--<span style="font-size:8px; padding-left:2px; color:crimson;">up to 80% of Salary</span>--%>
            </td>

            <td>
                <asp:TextBox ID="txtLoanAmount" runat="server" masktype="number"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Recovery Start Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRecoveryStartDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Monthy Installments</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedInstallmentAmount" runat="server" masktype="number"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Total Recoveries</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedInstallments" runat="server" masktype="number"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Currency</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlCurrency" runat="server" ></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Loan Description</span>
            </td>

            <td>
                <asp:TextBox ID="txtAdvanceDescription" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
