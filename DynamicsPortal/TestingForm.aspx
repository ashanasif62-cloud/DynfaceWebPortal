<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestingForm.aspx.cs" Inherits="DynamicsPortal.TestingForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/distribution/css/custom.css" rel="stylesheet" />
    <script type="text/javascript">
        function openDatePicker(pickerId, targetId) {
            var picker = document.getElementById(pickerId);
            if (!picker) return;
            picker.style.display = 'inline';
            picker.focus();
            picker.onchange = function () {
                var target = document.getElementById(targetId);
                if (target) target.value = this.value;
                // hide the picker after selecting
                this.style.display = 'none';
            };
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" CssClass="btn btn-primary" />
    <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" CssClass="btn btn-secondary" />
    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CssClass="btn btn-danger" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
    <div class="form-search">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Width="300px" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-default" />
    </div>

    <asp:GridView ID="gvTesting" runat="server" AllowPaging="True" PageSize="10" AutoGenerateColumns="False" OnPageIndexChanging="gvTesting_PageIndexChanging" OnSelectedIndexChanged="gvTesting_SelectedIndexChanged" CssClass="table table-striped table-responsive">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" />
            <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" />
            <asp:BoundField DataField="Department" HeaderText="Department" />
            <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
            <asp:BoundField DataField="EndDate" HeaderText="End Date" />
        </Columns>
    </asp:GridView>

    <!-- Form uses grid-only layout per project convention; inputs are not included here. -->
</asp:Content>
