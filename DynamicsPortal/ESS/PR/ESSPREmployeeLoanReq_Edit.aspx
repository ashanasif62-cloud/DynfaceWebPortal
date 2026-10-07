<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="ESSPREmployeeLoanReq_Edit.aspx.cs" Inherits="DynamicsPortal.ESS.PR.ESSPREmployeeLoanReq_Edit" %>
<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>



<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Heavily limit the height of the guarantor dropdowns to show fewer entries at a time */
        .ui-selectmenu-menu .ui-menu {
            max-height: 150px !important; /* Shows about 5-6 entries only */
            overflow-y: auto;
            font-size: 11px;
            border: 1px solid #ddd;
        }
        .ui-selectmenu-button.ui-button {
            width: 100% !important;
            font-size: 12px;
            background: #fff;
            border: 1px solid #ccc;
            height: 30px;
        }
        /* Style for the items to make them compact */
        .ui-menu-item {
            font-size: 11px;
            padding: 3px 1em 3px .4em;
        }
    </style>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
        <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
<ContentTemplate>
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
                <span>Request Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestDate" runat="server" masktype="date"></asp:TextBox>
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
        <tr>
            <td>
                <span>Payment Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtPaymentDate" runat="server" Textmode="Date" AutoPostBack="true" OnTextChanged="onPaymentDateModified"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Loan Type Code</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlAdvanceTypeCode" OnSelectedIndexChanged="onAdvanceTypeChange" AutoPostBack="true" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Loan Amount</span>
            </td>

            <td>
                <asp:TextBox ID="txtLoanAmount" runat="server" masktype="number"></asp:TextBox>
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
<%--        <tr>
            <td>
                <span>Guarantor 1 ID</span>
            </td>

            <td>
               <asp:DropDownList ID="ddlGuarantor1" runat="server"></asp:DropDownList>

<asp:DropDownList 
    ID="DropDownList1" 
    runat="server"
    onchange="validateGuarantors();">
</asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Guarantor 2 ID</span>
            </td>

            <td>
                <asp:DropDownList 
    ID="ddlGuarantor2" 
    runat="server"
    onchange="validateGuarantors();">
</asp:DropDownList>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Recovery Start Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRecoveryStartDate" runat="server" TextMode="Date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Monthy Installments</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedInstallmentAmount" runat="server" Enabled="false" masktype="number"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Total Recoveries</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedInstallments" runat="server" Enabled="false" masktype="number"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Update</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Update & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
             </ContentTemplate>
</asp:UpdatePanel>
<%--    <script type="text/javascript">
        function initSelectMenu() {
            try {
                $("#<%= ddlGuarantor1.ClientID %>, #<%= ddlGuarantor2.ClientID %>").selectmenu({
                    position: { my: "left top", at: "left bottom", collision: "flipfit" }
                });
            } catch (e) { console.log(e); }
        }

        $(document).ready(function () {
            initSelectMenu();
        });

        // Re-initialize after UpdatePanel postback
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm) {
            prm.add_endRequest(function () {
                initSelectMenu();
            });
        }
    </script>--%>

<%--    <script type="text/javascript">

        function validateGuarantors() {

            var guarantor1 = document.getElementById('<%= ddlGuarantor1.ClientID %>');
        var guarantor2 = document.getElementById('<%= ddlGuarantor2.ClientID %>');

            if (guarantor1.value !== "" &&
                guarantor2.value !== "" &&
                guarantor1.value === guarantor2.value) {

                alert("Guarantor 1 and Guarantor 2 cannot be the same. Please select another guarantor.");

                guarantor2.selectedIndex = 0;

                return false;
            }

            return true;
        }

    </script>--%>
</asp:Content>
