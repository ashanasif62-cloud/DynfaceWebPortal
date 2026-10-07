<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSPREmployeeEOS_Create.aspx.cs" Inherits="DynamicsPortal.ESSPREmployeeEOS_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
     <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
 <ContentTemplate>
    <table class="form-table">
        <%--        <tr>
            <td>
                <span>EOS Request Id</span>
            </td>

            <td>
                <asp:TextBox ID="txtEOSRequestId" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
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
                <span>EOS Type</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlEOSType" runat="server" Enabled="false"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Notice Period Code</span>
            </td>

            <td>
                  <asp:DropDownList ID="ddlNoticePeriodCode" OnSelectedIndexChanged="onNoticePeriodModified" AutoPostBack="true" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Reason Code</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlReasonCode" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Hiring Date</span>
            </td>

            <td>
                    <asp:TextBox ID="txtHiringDate" runat="server" Enabled="false" autocomplete="off"  TextMode="Date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Notification Date</span>
            </td>

            <td>
               <%-- <asp:TextBox ID="txtNotificationDate" runat="server" autocomplete="off"  masktype="date"></asp:TextBox>--%>
                <asp:TextBox ID="txtNotificationDate" runat="server" TextMode="Date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Last Working Date Planned</span>
            </td>

            <td>
                 <asp:TextBox ID="txtLastWorkingDatePlanned" Enabled="false" runat="server" TextMode="Date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Last Working Date Requested</span>
            </td>

            <td>
                <asp:TextBox ID="txtLastWorkingDateRequested" OnTextChanged="txtLastWorkingDateRequested_TextChanged" AutoPostBack="true" runat="server" TextMode="Date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Service Duration</span>
            </td>

            <td>
                <asp:TextBox ID="txtServiceDuration" runat="server" Enabled="false"></asp:TextBox>
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
        <tr style="display: none">
            <td>
                <span>Active Pay Period</span>
            </td>

            <td>
                <asp:TextBox ID="txtActivePayPeriod" runat="server" Enabled="false"></asp:TextBox>
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
</asp:Content>


