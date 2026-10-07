<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSPREmployeeLoan_Create.aspx.cs" Inherits="DynamicsPortal.ESSPREmployeeLoan_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>
<%@ Register Src="~/DropDownList_AllEmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_AllEmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function saveLoanAjax(submit) {
            showAJAXOverlay();
            
            var data = {
                employeeId: $("[id$='txtEmployeeId']").val(),
                currency: $("[id$='ddlCurrency']").val(),
                loanType: $("[id$='ddlLoanTypeCode']").val(),
                reqDate: $("[id$='txtRequestDate']").val(),
                amount: $("[id$='txtLoanAmount']").val(),
                payDate: $("[id$='txtPaymentDate']").val(),
                recoveryDate: $("[id$='txtRecoveryStartDate']").val(),
                desc: $("[id$='txtLoanDescription']").val(),
                installments: $("[id$='txtRequestedInstallments']").val(),
                installmentAmount: $("[id$='txtRequestedInstallmentAmount']").val(),
                submit: submit
            };

            $.ajax({
                type: "POST",
                url: "ESSPREmployeeLoan_Create.aspx/SaveLoanAjax",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    hideAJAXOverlay();
                    var result = response.d;
                    if (result.isSuccess) {
                        showNotificationMessage(result.Message, "Success", true);
                        setTimeout(function () { closeDialog(); refreshParent(); }, 1500);
                    } else {
                        showNotificationMessage(result.Message, "Error");
                    }
                },
                error: function (xhr, status, error) {
                    hideAJAXOverlay();
                    showNotificationMessage("An error occurred while processing your request.", "Error");
                    console.log(xhr.responseText);
                }
            });
            return false;
        }

        function refreshParent() {
            if (window.parent && window.parent.refreshPage) {
                window.parent.refreshPage();
            }
        }

        // Phase 2 Extension: AJAX Currency Update
        function onEmployeeSelected() {
            var empId = $("[id$='txtEmployeeId']").val();
            if (!empId) return;

            $.ajax({
                type: "POST",
                url: "ESSPREmployeeLoan_Create.aspx/GetEmployeeCurrency",
                data: JSON.stringify({ employeeId: empId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("[id$='ddlCurrency']").val(response.d);
                }
            });
        }

        $(document).ready(function() {
            $("[id$='txtEmployeeId']").on('change', function() {
                onEmployeeSelected();
            });
        });

    </script>
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
<%--        <tr>
            <td>
                <span>Guarantor 1</span>
            </td>

            <td>
                <uc1:DropDownList_AllEmployeeDetails runat="server" ID="cddlGuarantor1" />
            </td>
        </tr>
        <tr>
            <td>
                <span>Guarantor 2</span>
            </td>

            <td>
                <uc1:DropDownList_AllEmployeeDetails runat="server" ID="cddlGuarantor2" />
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Payment Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtPaymentDate" runat="server" masktype="date" Visible="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Loan Type Code</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlLoanTypeCode" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Loan Amount</span><span style="font-size:8px; padding-left:2px; color:crimson;">up to 80% of Salary</span>
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
                <asp:DropDownList ID="ddlCurrency" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Loan Description</span>
            </td>

            <td>
                <asp:TextBox ID="txtLoanDescription" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="action-footer">
        <button type="button" class="btn btn-primary" onclick="saveLoanAjax(false)">Create</button>
        <button type="button" class="btn btn-success" onclick="saveLoanAjax(true)">Create & Submit</button>
        <button type="button" class="btn btn-secondary" onclick="closeDialog()">Cancel</button>
    </div>
</asp:Content>
