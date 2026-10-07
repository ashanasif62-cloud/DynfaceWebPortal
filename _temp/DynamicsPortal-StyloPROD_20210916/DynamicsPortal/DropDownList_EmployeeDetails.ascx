<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDownList_EmployeeDetails.ascx.cs" Inherits="DynamicsPortal.DropDownList_EmployeeDetails" %>

<!-- Bootstrap CSS-->
<link rel="stylesheet" href="/distribution/vendor/bootstrap/css/bootstrap.min.css">
<link rel="stylesheet" href="/distribution/css/custom.css">
<!-- Custom Scrollbar-->
<link rel="stylesheet" href="/distribution/vendor/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.css">

<div style="display: inline-block; overflow: visible;">
    <div style="display: inline-block;">
        <a onclick="showMenu(this);">
            <asp:TextBox ID="txtEmployeeId" CssClass="textbox" ReadOnly="true" runat="server"></asp:TextBox></a>
    </div>
    <div id="mddEmployeeDetails" class="dropdown-panel">
        <asp:GridView ID="gvEmployeeDetails" CssClass="table no-border table-hover" runat="server" AutoGenerateColumns="false"
            OnSelectedIndexChanged="gvEmployeeDetails_SelectedIndexChanged" OnRowDataBound="gvEmployeeDetails_RowDataBound">
            <Columns>
                <asp:BoundField DataField="EmployeeId" HeaderText="Emp Id" />
                <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" />
                <asp:BoundField DataField="Currency" HeaderText="Currency" />
            </Columns>
        </asp:GridView>
    </div>
</div>


<!-- JavaScript files-->
<script src="/distribution/js/jquery-3.3.1.min.js"></script>
<script src="/distribution/vendor/jquery-ui-1.12.1.custom/jquery-ui.min.js"></script>
<script src="/distribution/vendor/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js"></script>

<script>
    function showMenu(args) {
        //debugger;
        $(args).parent().parent().find(".dropdown-panel").toggleClass("show");
    }
    //$("table[id*='gvEmployeeDetails'] tr").click(function () {
    //    var index = $(this).index();
    //    if (index > 0) {
    //        var employeeId = $((this).cells[0]).text();
    //        $("input[id*='txtEmployeeId']").val(employeeId);
    //    }
    //    else {
    //        $("input[id*='txtEmployeeId']").val("");
    //    }

    //    //$(this).toggleClass("highlight");
    //});
</script>
