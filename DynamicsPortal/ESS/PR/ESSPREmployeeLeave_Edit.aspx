<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSPREmployeeLeave_Edit.aspx.cs" Inherits="DynamicsPortal.ESSPREmployeeLeave_Edit" %>

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
                <asp:DropDownList ID="ddlLeaveCategory" runat="server"></asp:DropDownList>
            </td>
        </tr>
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
                <span>Leave Start Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtLeaveStartDate"  runat="server" autocomplete="off" TextMode="Date"></asp:TextBox>
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
        <tr style="display:none !important">
    <td>
        <span>Quota</span>
    </td>
    <td>
        <asp:TextBox 
            ID="txtQuota"
            runat="server"
            Enabled="false"
            TextMode="Number">
        </asp:TextBox>
    </td>
</tr>

<tr style="display:none !important">
    <td>
        <span>Eligibility</span>
    </td>
    <td>
        <asp:TextBox 
            ID="txtEligibility"
            runat="server"
            Enabled="false"
            TextMode="Number">
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
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">OK</asp:LinkButton>
<%--        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>--%>
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
</asp:Content>
