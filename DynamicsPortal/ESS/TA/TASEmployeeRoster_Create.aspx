<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="TASEmployeeRoster_Create.aspx.cs" Inherits="DynamicsPortal.TASEmployeeRoster_Create" %>
<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
<ContentTemplate>
    <table class="form-table">
        <tr>
            <td><span>Employee Id</span></td>
            <td>   <uc1:DropDownList_EmployeeDetails ID="DropDownList_EmployeeDetails1" runat="server" OnEmployeeSelected="cddlEmployeeDetails_OnEmployeeSelected" />

            </td>
        </tr>
      
           <tr>
        <td><span>Shift Id</span></td>
            <td>
                <asp:DropDownList ID="ddlShiftId" runat="server" AutoPostBack="false" />
            </td>

   </tr>
        <tr>
            <td><span>From Date</span></td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" />
            </td>
        </tr>
        <tr>
            <td><span>To Date</span></td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" />
            </td>
        </tr>
    </table>

    <div class="action-footer">
        <asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click">OK</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
        </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
