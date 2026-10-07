<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDownList_ItemDetails.ascx.cs" Inherits="DynamicsPortal.DropDownList_ItemDetails" %>

<!-- Bootstrap CSS-->
<link rel="stylesheet" href="/distribution/vendor/bootstrap/css/bootstrap.min.css">
<link rel="stylesheet" href="/distribution/css/custom.css">
<!-- Custom Scrollbar-->
<link rel="stylesheet" href="/distribution/vendor/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.css">
<syle></syle>
<div style="display: inline-block; overflow: visible;">
    <div style="display: inline-block;">
        <a onclick="showMenu(this);">
            <asp:TextBox ID="txtItemId" CssClass="textbox" ReadOnly="true" runat="server"></asp:TextBox></a>
    </div>
    <div id="mddItemDetails" class="dropdown-panel">
        <asp:GridView ID="gvItemDetails" CssClass="table no-border table-hover no-sort" runat="server" AutoGenerateColumns="false"
            OnSelectedIndexChanged="gvItemDetails_SelectedIndexChanged" OnRowDataBound="gvgvItemDetails_RowDataBound">
            <Columns>
                <asp:BoundField DataField="ItemId" HeaderText="Item number" />
                <asp:BoundField DataField="ItemName" HeaderText="Product name" />
            </Columns>
        </asp:GridView>
    </div>
</div>