<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="PREmployeePFRequest_Create.aspx.cs" Inherits="DynamicsPortal.PREmployeePFRequest_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
       <%-- <tr>
            <td>
                <span>Request Id</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestId" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Employee</span>
            </td>

            <td>
                <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails" OnEmployeeSelected="cddlEmployeeDetails_OnEmployeeSelected"/>
            </td>
        </tr>
        <%--<tr>
            <td>
                <span>Employee Name</span>
            </td>

            <td>
                <asp:TextBox ID="txtEmployeeName" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        
        
        <tr>
            <td>
                <span>Request Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Payment Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedPaymentDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Advance Type Code</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlAdvanceTypeCode" runat="server" OnSelectedIndexChanged="ddlAdvanceTypeCode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

            </td>
        </tr>
        <tr>
            <td>
                <span>Employee PF/CF Balance</span>
            </td>

            <td>
                <asp:TextBox ID="txtPFBalance" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Employer PF/CF Balance</span>
            </td>

            <td>
                <asp:TextBox ID="txtEmployerPFBalance" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Request Amount</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestAmount" runat="server"></asp:TextBox>
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
                <span>Recoveries</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedInstallments" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>PF/CF Description</span>
            </td>

            <td>
                <asp:TextBox ID="txtPFDescription" runat="server" ></asp:TextBox>
            </td>
        </tr>
        <%--<tr>
            <td>
                <span>Pay Period Code</span>
            </td>

            <td>
                <asp:TextBox ID="txtPayPeriodCode" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Pay Period Year</span>
            </td>

            <td>
                <asp:TextBox ID="txtPayPeriodYear" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>WF Status</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlWFStatus" runat="server" Enabled="false"></asp:DropDownList>
            </td>
        </tr>
        
        
        
        <tr>
            <td>
                <span>Recovery Amount</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedInstallmentAmount" runat="server"></asp:TextBox>
            </td>
        </tr>
        
        
        <%--<tr>
            <td>
                <span>Advance Id Ref</span>
            </td>

            <td>
                <asp:TextBox ID="txtAdvanceIdRef" runat="server"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td>
                <span>Outstanding Amount</span>
            </td>

            <td>
                <asp:TextBox ID="txtOutStandingAmount" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Pay Group Code</span>
            </td>

            <td>
                <asp:TextBox ID="txtPayGroupCode" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
