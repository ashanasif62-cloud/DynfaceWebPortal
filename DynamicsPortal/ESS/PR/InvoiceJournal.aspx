<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeFile="InvoiceJournal.aspx.cs" Inherits="DynamicsPortal.ESS.PR.InvoiceJournal" Title="Invoice Journal" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .grid-container {
            padding: 10px;
            background-color: #fff;
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
        <asp:UpdatePanel ID="upGrid" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvInvoiceJournal" runat="server" AutoGenerateColumns="false" 
                    CssClass="table table-condensed no-border table-hover sortable d365-table"
                    ShowHeaderWhenEmpty="true" EmptyDataText="No records found.">
                    <Columns>
                        <%-- Selection Column --%>
                        <asp:TemplateField ItemStyle-CssClass="checkbox-col" HeaderStyle-CssClass="checkbox-col">
                            <HeaderTemplate>
                                <input type="checkbox" onclick="selectAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Data Columns --%>
                        <asp:BoundField DataField="PurchId" HeaderText="Purchase Order" />
                        <asp:BoundField DataField="TransDate" HeaderText="Date" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="VATRegisterDate_W" HeaderText="Date of VAT register" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="InvoiceId" HeaderText="Invoice" />
                        <asp:BoundField DataField="LedgerVoucher" HeaderText="Voucher" />
                        <asp:BoundField DataField="CurrencyCode" HeaderText="Currency" />
                        <asp:BoundField DataField="SumTax" HeaderText="Sales Tax" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="InvoiceAmount" HeaderText="Invoice Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="DataAreaId" HeaderText="Company" />
                        <asp:BoundField DataField="SalesId" HeaderText="Sales Order" />
                        <asp:BoundField DataField="Voucher" HeaderText="Voucher (Secondary)" />
                        <asp:BoundField DataField="IntercompanyPosted" HeaderText="Posted via Intercompany" />
                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="InvoiceRegisterDate" HeaderText="Invoice Register Date" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="InvoiceRegisterVoucher" HeaderText="Invoice Register Voucher" />

                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        function selectAll(chk) {
            var grid = document.getElementById('<%= gvInvoiceJournal.ClientID %>');
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox" && inputs[i] != chk) {
                    inputs[i].checked = chk.checked;
                }
            }
        }
    </script>
</asp:Content>
