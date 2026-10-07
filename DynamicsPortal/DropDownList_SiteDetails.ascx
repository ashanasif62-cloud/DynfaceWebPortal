<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDownList_SiteDetails.ascx.cs" Inherits="DynamicsPortal.DropDownList_SiteDetails" %>

<div class="usercontrol-dropdown" style="display: inline-block; overflow: visible; position: relative;">
    <div style="display: inline-block;">
        <a class="toggle-dropdown">
            <asp:TextBox ID="txtSiteId" CssClass="form-control autocomplete-input" ReadOnly="true" runat="server"></asp:TextBox></a>
    </div>
    <div id="mddSiteDetails" class="dropdown-panel">
        <!-- Search Box -->
        <div class="dropdown-search-container" style="padding: 10px; border-bottom: 1px solid #ddd;">
            <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="Search..." runat="server" 
                onkeyup="filterGridView(this)"></asp:TextBox>
        </div>
        <!-- GridView -->
        <asp:GridView ID="gvSiteDetails" CssClass="table no-border table-hover" runat="server" AutoGenerateColumns="false"
            OnSelectedIndexChanged="gvSiteDetails_SelectedIndexChanged" OnRowDataBound="gvSiteDetails_RowDataBound">
            <Columns>
                <asp:BoundField DataField="SiteID" HeaderText="Site" />
                <asp:BoundField DataField="SiteName" HeaderText="Name" />
            </Columns>
        </asp:GridView>
    </div>
</div>

<script type="text/javascript">
    function filterGridView(input) {
        var searchText = $(input).val().toLowerCase();
        var $panel = $(input).closest('.dropdown-panel');
        var $grid = $panel.find('table');

        $grid.find('tr:not(:first)').each(function () {
            var $row = $(this);
            var rowText = $row.text().toLowerCase();
            if (rowText.indexOf(searchText) > -1) {
                $row.show();
            } else {
                $row.hide();
            }
        });
    }
</script>

<style>
    .dropdown-search-container {
    background-color: #f8f9fa;
    padding: 8px;
    box-sizing: border-box;
}

.dropdown-search-container .form-control {
    width: 100% !important; 
    max-width: 100%;
    border-radius: 4px;
    box-sizing: border-box;
    margin: 0;
}

.dropdown-panel {
    max-height: 400px;
    overflow-y: auto;
    box-sizing: border-box;
    left: 0 !important; /* Force left alignment */
    right: auto !important; /* Disable right alignment */
    width: 450px; /* Standard width */
}

</style>