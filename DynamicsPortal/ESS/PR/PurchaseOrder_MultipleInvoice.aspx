<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder_MultipleInvoice.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_MultipleInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <!-- Use container-fluid to remove left & right padding -->
    <div class="container-fluid mt-3">

        <h3 class="mb-3">Purchase Orders - Multiple Invoice</h3>

        <!-- Grid aligned fully to the left -->
        <div class="table-responsive">
            <asp:GridView ID="gvMultipleInvoice" runat="server"
                CssClass="table table-striped table-bordered table-hover"
                AutoGenerateColumns="False"
                ShowHeaderWhenEmpty="true"
                EmptyDataText="No records found"
                Width="100%">

                <Columns>
                    <asp:BoundField DataField="InvoiceAccount" HeaderText="Invoice Account" />
                    <asp:BoundField DataField="AccountName" HeaderText="Account Name" />
                    <asp:BoundField DataField="PurchaseOrder" HeaderText="Purchase Order" />
                    <asp:BoundField DataField="PurchaseAgreement" HeaderText="Purchase Agreement" />
                    <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ProductReceipt" HeaderText="Product Receipt" />
                    <asp:CheckBoxField DataField="OnHold" HeaderText="On Hold" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="MatchStatus" HeaderText="Match Status" />
                </Columns>

            </asp:GridView>
        </div>

    </div>

</asp:Content>
