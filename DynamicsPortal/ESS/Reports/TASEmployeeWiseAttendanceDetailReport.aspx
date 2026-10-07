<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TASEmployeeWiseAttendanceDetailReport.aspx.cs" Inherits="DynamicsPortal.ESS.Reports.TASEmployeeWiseAttendanceDetailReport" %>
<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <div style="display: inline-block;">
            <asp:LinkButton ID="btnView" runat="server" OnClick="btnViewReport_Click" CssClass="action-button">
                <i class="mdi mdi-eye"></i> View Employee Wise Attendance Detail 
            </asp:LinkButton>
        </div>
        <div style="display: inline-block; margin-left: 0.02in;">
            <asp:LinkButton ID="btnEmail" runat="server" OnClick="btnEmailReport_Click" CssClass="action-button">
                <i class="mdi mdi-email"></i> Email Employee Wise Attendance Detail 

            </asp:LinkButton>
        </div>
    </div>

    <style>
        .action-button {
            text-decoration: none;
            cursor: pointer;
            padding: 2px 8px;
            background-color: transparent;
            border: none;
         color: #ffffff;   
            font-size: 14px;
        }

        .action-button:hover {
            text-decoration: underline;
        }
    </style>
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <div style="display: inline-block; font-size: 12px;">
        <span style="margin-right: 5px;">From Date</span>
        <asp:TextBox ID="txtFromDate" runat="server" autocomplete="off"  masktype="date"></asp:TextBox>

        <span style="margin-right: 5px;">To Date</span>
        <asp:TextBox ID="txtToDate" runat="server" autocomplete="off"  masktype="date"></asp:TextBox>
    </div>
     <div style="display: inline-block; overflow: visible;">
     <span style="font-size: 12px; margin-right: 5px;">Employee ID</span>
     <asp:DropDownList ID ="ddlEmployeeId" runat="server" AutoPostBack="false" />
   

 </div>

   
    
    <div style="margin-top: 5px;">
        <embed id="embed01" runat="server" type="application/pdf" height="700" width="850" />
    </div>

</asp:Content>

