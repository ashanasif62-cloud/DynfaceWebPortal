<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ESSEmployeeSurvay_ListPage.aspx.cs"
    Inherits="DynamicsPortal.ESS.HR.ESSEmployeeSurvay_ListPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        /* ===== Header style (same as Help Desk / Complain) ===== */
        .table.sortable th,
        .table.sortable thead th,
        .table.table-condensed.no-border th,
        .table.table-condensed.no-border thead th {
            color: #999999 !important;              /* light gray header text */
            font-weight: 500 !important;
            background-color: #ffffff !important;
            border-bottom: 1px solid #e5e5e5 !important;
            font-size: 13px !important;
            padding: 10px 12px !important;
        }

        /* ===== Data (body) text style ===== */
        .table.sortable td,
        .table.table-condensed.no-border td {
            color: #333333 !important;              /* dark text for data */
            font-size: 13px !important;
            padding: 10px 12px !important;
            vertical-align: middle !important;
            border-bottom: 1px solid #f0f0f0 !important;
        }

        /* Hover effect same as the image */
        .table.sortable tbody tr:hover td {
            background-color: #f8f9fa !important;
        }

        /* Remove any bold or extra weight */
        .table.sortable th,
        .table.sortable td {
            font-weight: normal !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    <%-- Keep empty or add buttons later if needed --%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
    <div>
 <asp:GridView ID="gvEmployeeSurvey"
    runat="server"
    CssClass="table table-condensed no-border table-hover sortable"
    ShowHeaderWhenEmpty="true"
    EmptyDataText="No employee surveys found."
    AutoGenerateColumns="false"
    GridLines="None">
    <Columns>
        <asp:TemplateField HeaderText="Survey ID">
            <ItemTemplate>
                <asp:Label ID="lblSurveyId" runat="server"
                    Text='<%# Eval("SurveyId") %>' />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Personnel number">
            <ItemTemplate>
                <asp:Label ID="lblPersonnelNumber" runat="server"
                    Text='<%# Eval("EmployeeId") %>' />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Employee Name">
            <ItemTemplate>
                <asp:Label ID="lblEmployeeName" runat="server"
                    Text='<%# Eval("EmployeeName") %>' />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Date">
            <ItemTemplate>
                <asp:Label ID="lblSurveyDate" runat="server"
                    Text='<%# Eval("SurveyDate", "{0:yyyy-MM-dd}") %>' />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Employee Survey Status">
            <ItemTemplate>
                <asp:Label ID="lblEmployeeSurveyStatus" runat="server"
                    Text='<%# Eval("EmployeeSurveyStatus") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
    </div>
</asp:Content>