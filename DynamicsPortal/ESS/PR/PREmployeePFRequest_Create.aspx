<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="PREmployeePFRequest_Create.aspx.cs" Inherits="DynamicsPortal.PREmployeePFRequest_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
      <link href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css" rel="stylesheet" />
 <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
      <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
  <ContentTemplate>
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
        <tr>
    <td>
        <span>PF Description</span>
    </td>

    <td>
        <asp:TextBox ID="txtPFDescription" runat="server" ></asp:TextBox>
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
                <asp:TextBox ID="txtRequestDate" runat="server" autocomplete="off"  masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Payment Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedPaymentDate" runat="server"  TextMode="SingleLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Advance Type Code</span>
            </td>

            <td>
<%--                <asp:DropDownList ID="ddlAdvanceTypeCode" runat="server" OnSelectedIndexChanged="ddlAdvanceTypeCode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                <asp:DropDownList ID="ddlAdvanceTypeCode" runat="server" Enabled="false"></asp:DropDownList>
               

            </td>
        </tr>
        <tr>
            <td>
                <span>PF Balance</span>
            </td>

            <td>
                <asp:TextBox ID="txtPFBalance" runat="server"  CssClass="amount-input" Enabled="false"></asp:TextBox>
            </td>
        </tr>
       <tr style="display: none;"> <!-- Hidden row -->
            <td>
                <span>Employer PF Balance</span>
            </td>

            <td>
                <asp:TextBox ID="txtEmployerPFBalance" runat="server"  Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Request Amount</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestAmount" runat="server"  CssClass="amount-input1"></asp:TextBox>
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
                <asp:TextBox ID="txtRecoveryStartDate" runat="server" Enabled="false" TextMode="SingleLine"></asp:TextBox>
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
          </ContentTemplate>
</asp:UpdatePanel>
        <script type="text/javascript">
document.addEventListener("DOMContentLoaded", function () {
    const amountInput = document.querySelector(".amount-input");

    if (amountInput) {
        amountInput.addEventListener("input", function (e) {
            let value = e.target.value.replace(/,/g, "").replace(/[^\d]/g, "");
            if (!isNaN(value) && value !== "") {
                e.target.value = Number(value).toLocaleString("en-US");
            } else {
                e.target.value = "";
            }
        });

        amountInput.addEventListener("blur", function (e) {
            let value = e.target.value.replace(/,/g, "").trim();
            if (value !== "" && !isNaN(value)) {
                e.target.value = parseFloat(value).toLocaleString("en-US");
            }
        });
    }
});
        </script>
            <script type="text/javascript">
                document.addEventListener("DOMContentLoaded", function () {
                    const amountInput = document.querySelector(".amount-input1");

                    if (amountInput) {
                        amountInput.addEventListener("input", function (e) {
                            let value = e.target.value.replace(/,/g, "").replace(/[^\d]/g, "");
                            if (!isNaN(value) && value !== "") {
                                e.target.value = Number(value).toLocaleString("en-US");
                            } else {
                                e.target.value = "";
                            }
                        });

                        amountInput.addEventListener("blur", function (e) {
                            let value = e.target.value.replace(/,/g, "").trim();
                            if (value !== "" && !isNaN(value)) {
                                e.target.value = parseFloat(value).toLocaleString("en-US");
                            }
                        });
                    }
                });

           
            </script>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            const txtPaymentDateId = '<%= txtRequestedPaymentDate.ClientID %>';
        const txtRecoveryStartDateId = '<%= txtRecoveryStartDate.ClientID %>';

        flatpickr("#" + txtPaymentDateId, {
            dateFormat: "d/m/Y",
            onChange: function (selectedDates) {
                if (selectedDates.length > 0) {
                    const inputDate = selectedDates[0];

                    // Set Recovery Date to 25th of next month
                    const nextMonth = new Date(inputDate);
                    nextMonth.setMonth(nextMonth.getMonth() + 1);
                    nextMonth.setDate(25);

                    const dd = String(nextMonth.getDate()).padStart(2, '0');
                    const mm = String(nextMonth.getMonth() + 1).padStart(2, '0');
                    const yyyy = nextMonth.getFullYear();

                    document.getElementById(txtRecoveryStartDateId).value = `${dd}/${mm}/${yyyy}`;
                }
            }
        });

        flatpickr("#" + txtRecoveryStartDateId, {
            dateFormat: "d/m/Y"
        });
    });
    </script>
</asp:Content>
