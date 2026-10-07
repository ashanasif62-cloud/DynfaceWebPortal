<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="PendingInvoice.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PendingInvoice" Title="Pending Invoice" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .grid-container {
            padding: 10px;
            background-color: #fff;
            margin-bottom: 20px;
        }
        .d365-table {
            width: 100%;
            border-collapse: collapse;
            font-size: 0.85rem;
        }
        .d365-table th {
            background-color: #f3f2f1;
            color: #323130;
            font-weight: 600;
            padding: 8px;
            text-align: left;
            border-bottom: 1px solid #edebe9;
            position: sticky;
            top: 0;
            z-index: 10;
        }
        .d365-table td {
            padding: 8px;
            border-bottom: 1px solid #f3f2f1;
            color: #323130;
        }
        .d365-table tr:hover {
            background-color: #f3f2f1;
        }
        .checkbox-col {
            width: 40px;
            text-align: center;
        }
        .grid-title {
            font-size: 1.1rem;
            font-weight: 600;
            margin-bottom: 10px;
            color: #323130;
            border-bottom: 1px solid #edebe9;
            padding-bottom: 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="action-items">
        <asp:LinkButton ID="btnExport" runat="server"><i class="mdi mdi-export"></i>Export</asp:LinkButton>
    </div>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div class="grid-container">
        <%-- Header Grid --%>
        <asp:UpdatePanel ID="upHeaderGrid" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvPendingInvoiceHeader" runat="server" AutoGenerateColumns="false" 
                    CssClass="table table-condensed no-border table-hover sortable d365-table"
                    ShowHeaderWhenEmpty="true" EmptyDataText="No records found.">
                    <Columns>
                        <%-- Selection Column --%>
                        <asp:TemplateField ItemStyle-CssClass="checkbox-col" HeaderStyle-CssClass="checkbox-col">
                            <HeaderTemplate>
                                <input type="checkbox" onclick="selectAll(this, 'gvPendingInvoiceHeader')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Data Columns --%>
                        <asp:BoundField DataField="PurchId" HeaderText="Purchase order" />
                        <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice date" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="InvoiceId" HeaderText="Invoice" />
                        <asp:BoundField DataField="OnHold" HeaderText="On hold" />
                        <asp:BoundField DataField="CurrencyCode" HeaderText="Currency" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="grid-container">
        <div class="grid-title">Lines</div>
        <%-- Lines Grid --%>
        <asp:UpdatePanel ID="upLinesGrid" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvPendingInvoiceLines" runat="server" AutoGenerateColumns="false" 
                    CssClass="table table-condensed no-border table-hover sortable d365-table"
                    ShowHeaderWhenEmpty="true" EmptyDataText="No records found.">
                    <Columns>
                         <%-- Selection Column --%>
                        <asp:TemplateField ItemStyle-CssClass="checkbox-col" HeaderStyle-CssClass="checkbox-col">
                            <HeaderTemplate>
                                <input type="checkbox" onclick="selectAll(this, 'gvPendingInvoiceLines')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Data Columns --%>
                        <asp:BoundField DataField="PurchId" HeaderText="Purchase order" />
                        <asp:BoundField DataField="LineNum" HeaderText="Line number" />
                        <asp:BoundField DataField="ItemId" HeaderText="Item" />
                        <asp:BoundField DataField="ProcurementCategory" HeaderText="Procurement category" />
                        <asp:BoundField DataField="Name" HeaderText="Text" />
                        <asp:BoundField DataField="InventSiteId" HeaderText="Site" />
                        <asp:BoundField DataField="InventLocationId" HeaderText="Warehouse" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        function selectAll(chk, gridId) {
            var grid = document.querySelector('[id*="' + gridId + '"]');
            if (!grid) return;
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox" && inputs[i] != chk) {
                    inputs[i].checked = chk.checked;
                }
            }
        }
    </script>
</asp:Content>
