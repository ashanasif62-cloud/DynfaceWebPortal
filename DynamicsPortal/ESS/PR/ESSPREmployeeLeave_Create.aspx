<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSPREmployeeLeave_Create.aspx.cs" Inherits="DynamicsPortal.ESSPREmployeeLeave_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        input[readonly="readonly"] {
            background-color: #e9ecef !important;
            cursor: not-allowed !important;
            opacity: 1 !important;
            color: #495057 !important;
        }
    </style>
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
                        <%--<asp:DropDownList ID="ddlEmployee" runat="server"></asp:DropDownList>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Leave Code</span>
                    </td>

                    <td>
                        <asp:DropDownList ID="ddlLeaveCode" runat="server" OnSelectedIndexChanged="ddlLeaveCode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Leave Category</span>
                    </td>
                    <td>
                        <asp:DropDownList
                            ID="ddlLeaveCategory"
                            runat="server"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlLeaveCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Request Date</span>
                    </td>

                    <td>
                        <asp:TextBox ID="txtRequestDate" runat="server" autocomplete="off" masktype="date"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Leave Start Date</span>
                    </td>

                    <td>
                        <asp:TextBox ID="txtLeaveStartDate" AutoPostBack="true" OnTextChanged="txtLeaveStartDate_modified" runat="server" autocomplete="off" TextMode="Date"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Leave End Date</span>
                    </td>

                    <td>
                        <asp:TextBox ID="txtLeaveEndDate" AutoPostBack="true" runat="server" OnTextChanged="txtleaveEndDate_modified" Enabled="true" autocomplete="off" TextMode="Date"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trLeaveStartTime" runat="server">
                    <td>
                        <span>Leave Start Time</span>
                    </td>
                    <td>
                        <asp:TextBox
                            ID="txtLeaveStartTime"
                            runat="server"
                            TextMode="Time"
                            autocomplete="off">
                        </asp:TextBox>
                    </td>
                </tr>

                <tr id="trLeaveEndTime" runat="server">
                    <td>
                        <span>Leave End Time</span>
                    </td>
                    <td>
                        <asp:TextBox
                            ID="txtLeaveEndTime"
                            runat="server"
                            TextMode="Time"
                            autocomplete="off">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Leave Days</span>
                    </td>
                    <td>
                        <asp:TextBox
                            ID="txtNewLeaveDays"
                            runat="server"
                            Enabled="false"
                            TextMode="Number">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Balance</span>
                    </td>

                    <td>
                        <asp:TextBox ID="txtBalance" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr style="display: none !important">
                    <td>
                        <span>Quota</span>
                    </td>
                    <td>
                        <asp:TextBox
                            ID="txtQuota"
                            runat="server"
                            Enabled="false"
                            TextMode="Number"
                            Visible="false">
                        </asp:TextBox>
                    </td>
                </tr>

                <tr style="display: none !important">
                    <td>
                        <span>Eligibility</span>
                    </td>
                    <td>
                        <asp:TextBox
                            ID="txtEligibility"
                            runat="server"
                            Enabled="false"
                            TextMode="Number"
                            Visible="false">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Reason</span>
                    </td>

                    <td>
                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div class="action-footer">
                <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
                <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            $("input[id*='txtLeaveDays']").keyup(function () {
                var leaveDays = parseInt(this.value);
                if (leaveDays > 0) {
                    var d = $("input[id*='txtLeaveStartDate']").datepicker('getDate');
                    //d = new Date(d.getFullYear(), (d.getMonth() + 1), (d.getDate() + leaveDays - 1))
                    d.setDate(d.getDate() + (leaveDays - 1));

                    //$("input[id*='txtLeaveEndDate']").datepicker('setDate', d.format("dd/mm/yy"));
                    $("input[id*='txtLeaveEndDate']").val(d.format("dd/MM/yyyy"));
                }
            });
        });
    </script>

    <script type="text/javascript">
        function toggleShortLeaveTimes() {
            var ddl = document.getElementById('<%= ddlLeaveCategory.ClientID %>');
         if (!ddl) return;

         var selectedValue = ddl.value.toLowerCase();
         var selectedText = ddl.options[ddl.selectedIndex].text.toLowerCase();

         // Check if it's Short Leave
         var isShortLeave = selectedValue === 'shortleave' ||
             selectedText.indexOf('shortleave') !== -1 ||
             selectedText.indexOf('short leave') !== -1;

         // Check if it's Half Leave
         var isHalfLeave = selectedValue.indexOf('halfleave') !== -1 ||
             selectedText.indexOf('halfleave') !== -1 ||
             selectedText.indexOf('half leave') !== -1 ||
             selectedText.indexOf('half day') !== -1 ||
             selectedText.indexOf('halfday') !== -1;

         // Check if it's Full Day Leave (not short or half)
         var isFullDayLeave = !isShortLeave && !isHalfLeave;

         var startRow = document.getElementById('<%= trLeaveStartTime.ClientID %>');
         var endRow = document.getElementById('<%= trLeaveEndTime.ClientID %>');
         var endDateTextBox = document.getElementById('<%= txtLeaveEndDate.ClientID %>');
         var startDateTextBox = document.getElementById('<%= txtLeaveStartDate.ClientID %>');

         if (startRow) {
             startRow.style.display = isShortLeave ? "table-row" : "none";
         }
         if (endRow) {
             endRow.style.display = isShortLeave ? "table-row" : "none";
         }

         // Handle End Date readonly for Short Leave and Half Leave
         // Editable only for Full Day Leave
         if (endDateTextBox) {
             if (isFullDayLeave) {
                 // Full Day Leave - End Date is editable
                 endDateTextBox.readOnly = false;
                 endDateTextBox.removeAttribute('readonly');
                 endDateTextBox.style.backgroundColor = '';
                 endDateTextBox.style.cursor = '';
                 endDateTextBox.style.opacity = '';
                 endDateTextBox.style.color = '';
             } else {
                 // Short Leave or Half Leave - End Date is readonly
                 endDateTextBox.readOnly = true;
                 endDateTextBox.setAttribute('readonly', 'readonly');
                 endDateTextBox.style.backgroundColor = '#e9ecef';
                 endDateTextBox.style.cursor = 'not-allowed';
                 endDateTextBox.style.opacity = '1';
                 endDateTextBox.style.color = '#495057';

                 // Set end date equal to start date
                 if (startDateTextBox && startDateTextBox.value) {
                     endDateTextBox.value = startDateTextBox.value;
                 }
             }
         }

         // Clear the time values when hiding
         if (!isShortLeave) {
             var txtStart = document.getElementById('<%= txtLeaveStartTime.ClientID %>');
             var txtEnd = document.getElementById('<%= txtLeaveEndTime.ClientID %>');
                if (txtStart) txtStart.value = "";
                if (txtEnd) txtEnd.value = "";
            }
        }

        // Run on page load
        $(document).ready(function () {
            toggleShortLeaveTimes();
        });

        // Re-apply after every UpdatePanel async postback
        if (typeof Sys !== "undefined") {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                toggleShortLeaveTimes();
            });
        }

        // Also on dropdown change
        $(document).on("change", "#<%= ddlLeaveCategory.ClientID %>", function () {
            setTimeout(toggleShortLeaveTimes, 50);
        });
    </script>
</asp:Content>
