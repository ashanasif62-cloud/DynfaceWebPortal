<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SysRoles_ListPage.aspx.cs" Inherits="DynamicsPortal.SysRoles_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">

    <div class="action-items">
        <asp:LinkButton ID="btnUserRoles" runat="server" OnClick="btnUserRoles_Click"><i class="mdi mdi-account-edit"></i>User Roles</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnRoleMenuItems" runat="server" OnClick="btnRoleMenuItems_Click"><i class="mdi mdi-menu"></i>Role MenuItems</asp:LinkButton>
    </div>

</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <div class="action-panel-grid">
            <div class="action-items-grid">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnNew_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
            </div>
            <div class="action-items-grid">
                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Remove</asp:LinkButton>
            </div>
        </div>

        <div>
            <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
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

                    <asp:TemplateField HeaderText="Role Id">
                        <ItemTemplate>
                            <asp:Label ID="lblRoleId" runat="server" Text='<%# Bind("RoleId") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRoleId" runat="server" Text='<%# Bind("RoleId") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <%--<ItemTemplate>
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
    </div>
</asp:Content>

