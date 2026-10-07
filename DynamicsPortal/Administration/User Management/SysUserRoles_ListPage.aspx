<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SysUserRoles_ListPage.aspx.cs" Inherits="DynamicsPortal.SysUserRoles_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
<link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
</asp:Content>
 


<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
    </div>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                   <%--<asp:TemplateField HeaderText="User Id">
<ItemTemplate>
<asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlUserId" runat="server" />
</EditItemTemplate>
</asp:TemplateField>--%>
<asp:TemplateField HeaderText="User Id">
<ItemTemplate>
<asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>' />
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlUserId" runat="server" CssClass="filterable-dropdown" />
</EditItemTemplate>
</asp:TemplateField>

                 <asp:TemplateField HeaderText="Role Id">
                        <ItemTemplate>
                            <asp:Label ID="lblRoleId" runat="server" Text='<%# Bind("RoleId") %>'></asp:Label>
                        </ItemTemplate>
                       <EditItemTemplate>
<asp:DropDownList ID="ddlRoleId" runat="server" CssClass="filterable-dropdown" />
</EditItemTemplate>
</asp:TemplateField>
                  <%--  <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>--%>

                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
  <%--                  <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                    </ItemTemplate>--%>
                    <EditItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="LicenseType" HeaderText="LicenseType" Visible="false" />
                <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
                <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false" />
                <asp:TemplateField HeaderText="RecId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
      <script>
        function applyAutocomplete() {
            $('.filterable-dropdown').each(function () {
                var $dropdown = $(this);
 
                // Avoid adding input twice
                if ($dropdown.next('.autocomplete-input').length === 0) {
                    var options = [];
 
                    $dropdown.find('option').each(function () {
                        if ($(this).val()) { // Skip empty option in autocomplete list
                            options.push({
                                label: $(this).text(),
                                value: $(this).val()
                            });
                        }
                    });
 
                    // Determine placeholder based on dropdown ID
                    var placeholderText = '-- Select --';
                    var dropdownId = $dropdown.attr('id')?.toLowerCase();
 
                    switch (dropdownId) {
                        case "ddlUserId":
                            placeholderText = '-- Select User --';
                            break;
                        case "ddlRoleId":
                            placeholderText = '-- Select Role --';
                            break;
                    }
 
                    var $input = $('<input type="text" class="autocomplete-input form-control" />')
                        .attr('placeholder', placeholderText)
                        .val($dropdown.find("option:selected").text())
                        .insertAfter($dropdown)
                        .autocomplete({
                            source: options,
                            minLength: 0,
                            select: function (event, ui) {
                                $dropdown.val(ui.item.value);
                            }
                        })
                        .on("focus", function () {
                            $(this).autocomplete("search", ""); // Show all options on focus
                        });
 
                    $dropdown.hide();
                }
            });
        }
 
        $(document).ready(function () {
            applyAutocomplete();
 
            if (Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(applyAutocomplete);
            }
        });
</script>
 
    <style>
        .autocomplete-input {
            width: 50%;
            /*padding: 2px 4px;*/
            font-size: 12px;
            font-family: Arial, sans-serif;
            box-sizing: border-box;
        }
 
        .ui-menu-item:hover {
            background-color: #f0f0f0;
        }
 
        /*.ui-autocomplete {
            z-index: 99999 !important;
            max-height: 200px;
            overflow-y: auto;
            background-color: white;
            border: 1px solid #ccc;
        }*/
 
        /* Dropdown suggestion box */
        .ui-autocomplete {
            z-index: 99999 !important;
            max-height: 200px;
            overflow-y: auto;
            background-color: white;
            border: 1px solid #ccc;
            font-family: Arial, sans-serif;
            font-size: 10px;
        }
</style>
</asp:Content>

