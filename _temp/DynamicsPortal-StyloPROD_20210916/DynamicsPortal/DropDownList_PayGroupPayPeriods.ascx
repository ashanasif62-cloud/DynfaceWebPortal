<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDownList_PayGroupPayPeriods.ascx.cs" Inherits="DynamicsPortal.DropDownList_PayGroupPayPeriods" %>

<style>
    tr {
        min-width: 100px;
    }
</style>
<!-- Bootstrap CSS-->
<link rel="stylesheet" href="/distribution/vendor/bootstrap/css/bootstrap.min.css">
<link rel="stylesheet" href="/distribution/css/custom.css">

<div style="display: inline-block; overflow: visible; position: relative;">
    <div style="display: inline-block;">
        <a onclick="showMenu(this);">
            <asp:TextBox ID="txtPayPeriodCode" CssClass="textbox" ReadOnly="true" runat="server"></asp:TextBox></a>
    </div>
    <div id="mddPayGroupPayPeriods" class="dropdown-panel" style="left: 0px; min-width: 400px;">
        <asp:GridView ID="gvPayGroupPayPeriods" CssClass="table no-border table-hover" runat="server" AutoGenerateColumns="false"
            OnSelectedIndexChanged="gvPayGroupPayPeriods_SelectedIndexChanged" OnRowDataBound="gvPayGroupPayPeriods_RowDataBound">
            <Columns>
                <asp:BoundField DataField="PayGroupCode" HeaderText="Pay Group Code" />
                <asp:BoundField DataField="PayPeriodCode" HeaderText="Pay Period Code" />
                <asp:BoundField DataField="PayPeriodYear" HeaderText="Pay Period Year" />
                <asp:BoundField DataField="StatementDate" HeaderText="Statement Date" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" />
            </Columns>
        </asp:GridView>
    </div>
</div>



<!-- JavaScript files-->
<script src="/distribution/js/jquery-3.3.1.min.js"></script>
<script src="/distribution/vendor/jquery-ui-1.12.1.custom/jquery-ui.min.js"></script>


<script>
    function showMenu(args) {
        $(args).parent().parent().find(".dropdown-panel").toggleClass("show");
    }
</script>
