<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSPREntitlementBalance.aspx.cs" Inherits="DynamicsPortal.ESSPREntitlementBalance" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true"
            EmptyDataText="No Record Found." AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Pay Group Code">
                    <ItemTemplate>
                        <asp:Label ID="lblPayGroupCode" runat="server" Text='<%# Bind("PayGroupCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Id">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("EmployeeId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Entitlement Code">
                    <ItemTemplate>
                        <asp:Label ID="lblEntitlementCode" runat="server" Text='<%# Bind("EntitlementCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Unit">
                    <ItemTemplate>
                        <asp:Label ID="lblEntitlementUnit" runat="server" Text='<%# Bind("EntitlementUnit") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Accrued">
                    <ItemTemplate>
                        <asp:Label ID="lblAccrued" runat="server" Text='<%# Bind("Accrued") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Taken">
                    <ItemTemplate>
                        <asp:Label ID="lblTaken" runat="server" Text='<%# Bind("Taken") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Balance">
                    <ItemTemplate>
                        <asp:Label ID="lblBalance" runat="server" Text='<%# Bind("Balance") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
