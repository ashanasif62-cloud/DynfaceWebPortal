<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="HcmEmployeeTransferRequests_Create.aspx.cs" Inherits="DynamicsPortal.HcmEmployeeTransferRequests_Create" %>

<%@ Register Src="~/DropDownList_AllActiveFinancialDims.ascx" TagPrefix="uc1" TagName="DropDownList_AllActiveFinancialDims" %>



<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <tr>
            <td>
                <span>Employee Id</span>
            </td>
            <td>
                <span>Employee Name</span>
            </td>
            <td>
                <span>Employment Type</span>
            </td>
            <td>
                <span>Current Basic Salary</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtEmployeeId" runat="server" Enabled="false" />
            </td>
            <td>
                <asp:TextBox ID="txtEmployeeName" runat="server" Enabled="false" />
            </td>
            <td>
                <asp:TextBox ID="txtEmploymentType" runat="server" Enabled="false" />
            </td>
            <td>
                <asp:TextBox ID="txtBasicSalary" runat="server" Enabled="false" masktype="currency" />
            </td>
        </tr>

        <tr>
            <td>
                <span>Date of Joining</span>
            </td>
            <td>
                <span>Probation Date</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtDateOfJoining" runat="server" Enabled="false" masktype="date" />
            </td>
            <td>
                <asp:TextBox ID="txtProbationDate" runat="server" Enabled="false" masktype="date" />
            </td>
        </tr>

        <tr>
            <td colspan="4">
                <span>Current Assignment</span>
            </td>
        </tr>

        <tr>
            <td>
                <span>Job</span>
            </td>
            <td>
                <span>Position</span>
            </td>
            <td>
                <span>Worker</span>
            </td>
            <td>
                <span>Business Unit</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtJobId" runat="server" Enabled="false" />
            </td>
            <td>
                <asp:TextBox ID="txtPositionId" runat="server" Enabled="false" />
            </td>
            <td>
                <asp:TextBox ID="txtDimWorker" runat="server" Enabled="false" />
            </td>
            <td>
                <asp:TextBox ID="txtDimBusinessUnit" runat="server" Enabled="false" />
            </td>
        </tr>

        <tr>
            <td>
                <span>Cost Center</span>
            </td>
            <td>
                <span>Department</span>
            </td>
            <td>
                <span>Location</span>
            </td>
            <td>
                <span>Nature of Expense</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtDimCostCenter" runat="server" Enabled="false" />
            </td>
            <td>
                <asp:TextBox ID="txtDimDepartment" runat="server" Enabled="false" />
            </td>
            <td>
                <asp:TextBox ID="txtDimLocation" runat="server" Enabled="false" />
            </td>
            <td>
                <asp:TextBox ID="txtDimNatureofExpense" runat="server" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td>
                <span>Purpose</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtDimPurpose" runat="server" Enabled="false" />
            </td>
        </tr>


        <tr>
            <td colspan="4">
                <span>New Assignment</span>
            </td>
        </tr>

        <tr>
            <td>
                <span>New Job</span>
            </td>
            <td>
                <span>New Position</span>
            </td>
            <td>
                <span>Revised Basic Salary</span>
            </td>
            <td>
                <span>Effective Date</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlJobId" AutoPostBack="false" runat="server" />
            </td>
            <td>
                <asp:DropDownList ID="ddlPositionId" AutoPostBack="false" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtRevisedBasicSalary" runat="server" masktype="currency" />
            </td>
            <td>
                <asp:TextBox ID="txtEffectiveDate" runat="server" masktype="date" />
            </td>
        </tr>

        <tr>
            <td colspan="4">
                <uc1:DropDownList_AllActiveFinancialDims runat="server" id="cddl_AllActiveFinancialDims" />
            </td>
        </tr>

        <%--
        <tr>
            <td>
                <span>Business Unit</span>
            </td>
            <td>
                <span>Cost Center</span>
            </td>
            <td>
                <span>Department</span>
            </td>
            <td>
                <span>Location</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlDimBusinessUnit" AutoPostBack="false" runat="server" />
            </td>
            <td>
                <asp:DropDownList ID="ddlDimCostCenter" AutoPostBack="false" runat="server" />
            </td>
            <td>
                <asp:DropDownList ID="ddlDimDepartment" AutoPostBack="false" runat="server" />
            </td>
            <td>
                <asp:DropDownList ID="ddlDimLocation" AutoPostBack="false" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <span>Nature of Expense</span>
            </td>
            <td>
                <span>Purpose</span>
            </td>
            <td>
                <span>Worker</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlDimNatureOfExpense" AutoPostBack="false" runat="server" />
            </td>
            <td>
                <asp:DropDownList ID="ddlDimPurpose" AutoPostBack="false" runat="server" />
            </td>
            <td>
                <asp:DropDownList ID="ddlDimWorker" AutoPostBack="false" runat="server" />
            </td>
        </tr>
        --%>

    </table>

    <div class="action-footer">
        <asp:LinkButton ID="btnCreate" runat="server" OnClick="btnCreate_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>

