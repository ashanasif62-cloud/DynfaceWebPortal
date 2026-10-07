<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="PREmployeeLeaveEncashment_Create.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PREmployeeLeaveEncashment_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
       <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
 <ContentTemplate>
 <table class="form-table" style="width: 100% !important; table-layout: fixed !important;">
                      <tr>
                     <td>
                         <span>Employee ID</span>
                     </td>
                     <td>
                         <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails" />
                     </td>
                 </tr>
                    <tr>
                    <td>
                        <span>Request Date</span>
                    </td>

                    <td>
                        <asp:TextBox ID="txtRequestDate" runat="server" Enabled="false" TextMode="Date"></asp:TextBox>
                    </td>
                </tr>

                 <tr>
                    <td>
                        <span>Entitlement Code</span>
                    </td>

                    <td>
                        <asp:Dropdownlist ID="ddlEntitlementCode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnEntitlementCodeSelection"></asp:Dropdownlist>
                    </td>
                </tr>
                    <tr>
                    <td style="word-wrap: break-word; white-space: normal;">
                        <span>Leave Balance Before Application</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLeavesBalanceBefore" Text="0.00" runat="server" Enabled="false"  style="text-align: right;"></asp:TextBox>
                    </td>
                </tr>
                         <tr>
                    <td>
                        <span>Leaves to be Encashed</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLeavestoBeEncashed" AutoPostBack="true" OnTextChanged="OnLeavesToBeEncashedModified" Text="0.00" runat="server" Enabled="true"  style="text-align: right;"></asp:TextBox>
                    </td>
                </tr>

            <%--  <tr>
                <td>
                    <span>Earning Amount</span>
                </td>
                <td>
                    <asp:TextBox ID="txtEarningAmount" Text="0.00" runat="server" Enabled="false"  style="text-align: right;"></asp:TextBox>
                </td>
                </tr>--%>
                         <tr>
                    <td>
                        <span>Remaining Balance</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemainingBalance" Text="0.00" runat="server" Enabled="false"  style="text-align: right;"></asp:TextBox>
                    </td>
                </tr>
                    <%--     <tr>
                    <td>
                        <span>Last Encashment date</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastEncashmentDate" Text="0.00" runat="server" Enabled="false" TextMode="Date"></asp:TextBox>
                    </td>
                </tr>
                               <tr>
                <td>
                    <span>Last Encashment Leaves</span>
                </td>
                <td>
                    <asp:TextBox ID="txtLastEncashmentLeaves" Text="0.00" runat="server" Enabled="false"  style="text-align: right;"></asp:TextBox>
                </td>
                 </tr>--%>
     </table>
        
                   <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="showOverlay();" OnClick="btnSave_Click">Create</asp:LinkButton>
<%--        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>--%>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
      </ContentTemplate>
</asp:UpdatePanel>
     
</asp:Content>
