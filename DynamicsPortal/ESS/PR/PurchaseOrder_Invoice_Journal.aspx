<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder_Invoice_Journal.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_Invoice_Journal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <style>
        .tab-content {
            margin-top: 20px;
        }
        .card-header {
            cursor: pointer;
        }
        .rotate-icon {
            transition: transform 0.3s;
        }
        .rotate-icon.rotate {
            transform: rotate(180deg);
        }
        .form-control {
            margin-bottom: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
     <ul class="nav nav-tabs" id="invoiceJournalTabs" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overviewTab" role="tab" aria-controls="overviewTab" aria-selected="true">Overview</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="lines-tab" data-toggle="tab" href="#linesTab" role="tab" aria-controls="linesTab" aria-selected="false">Lines</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="remittance-tab" data-toggle="tab" href="#remittanceTab" role="tab" aria-controls="remittanceTab" aria-selected="false">Remittance</a>
        </li>
    </ul>

    <!-- Tabs Content -->
    <div class="tab-content" id="invoiceJournalTabContent">

        <!-- Overview Tab -->
<div class="tab-pane fade show active" id="overviewTab" role="tabpanel" aria-labelledby="overview-tab">
    <div class="container-fluid p-3">
        <h5>Overview</h5>

        <!-- Grid for Purchase Order Overview -->
        <div class="table-responsive mt-3">
            <asp:GridView ID="gvOverview" runat="server" CssClass="table table-striped table-bordered"
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                
                <Columns>
                    <asp:BoundField DataField="PurchaseOrder" HeaderText="Purchase Order" />
                    <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="DateOfVATRegister" HeaderText="Date of VAT Register" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" />
                    <asp:BoundField DataField="Voucher" HeaderText="Voucher" />
                    <asp:BoundField DataField="Currency" HeaderText="Currency" />
                    <asp:BoundField DataField="SalesTax" HeaderText="Sales Tax" />
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Invoice Amount" DataFormatString="{0:C2}" />
                    <asp:BoundField DataField="Company" HeaderText="Company" />
                    <asp:BoundField DataField="SalesOrder" HeaderText="Sales Order" />
                    <asp:BoundField DataField="Voucher2" HeaderText="Voucher" />
                    <asp:BoundField DataField="PostedViaIntercompany" HeaderText="Posted via Intercompany" />
                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="InvoiceRegisterDate" HeaderText="Invoice Register Date" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>


    <!-- Lines Tab -->
<div class="tab-pane fade" id="linesTab" role="tabpanel" aria-labelledby="lines-tab">
    <div class="container-fluid p-3">
        <h5>Lines</h5>

        <!-- Grid for Purchase Order Lines -->
        <div class="table-responsive mt-3">
            <asp:GridView ID="gvLines" runat="server" CssClass="table table-striped table-bordered"
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">

                <Columns>
                    <asp:BoundField DataField="PurchaseOrder" HeaderText="Purchase Order" />
                    <asp:BoundField DataField="LineNumber" HeaderText="Line Number" />
                    <asp:BoundField DataField="Item" HeaderText="Item" />
                    <asp:BoundField DataField="ProcurementCategory" HeaderText="Procurement Category" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Site" HeaderText="Site" />
                    <asp:BoundField DataField="Warehouse" HeaderText="Warehouse" />
                    <asp:BoundField DataField="InventoryStatus" HeaderText="Inventory Status" />
                    <asp:BoundField DataField="CWQuantity" HeaderText="CW Quantity" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C2}" />
                    <asp:BoundField DataField="Discount" HeaderText="Discount" DataFormatString="{0:C2}" />
                    <asp:BoundField DataField="DiscountPercent" HeaderText="Discount Percent" DataFormatString="{0:P2}" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C2}" />
                    <asp:BoundField DataField="SalesTaxIncludedInAmount" HeaderText="Sales Tax Included in Amount" />
                    <asp:BoundField DataField="Box1099" HeaderText="1099 Box" />
                    <asp:BoundField DataField="Amount1099" HeaderText="1099 Amount" DataFormatString="{0:C2}" />
                    <asp:BoundField DataField="StateProvince" HeaderText="State/Province" />
                    <asp:BoundField DataField="Amount1099State" HeaderText="1099 State Amount" DataFormatString="{0:C2}" />
                    <asp:BoundField DataField="ReasonCode" HeaderText="Reason Code" />
                    <asp:BoundField DataField="ReasonComment" HeaderText="Reason Comment" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>


        <!-- Remittance Tab -->
        <div class="tab-pane fade" id="remittanceTab" role="tabpanel" aria-labelledby="remittance-tab">
            <div class="container-fluid p-3">
                <h5>Remittance</h5>
                <p>Add your remittance information here...</p>
            </div>
        </div>

    </div>

    <script>
        // Optional: Toggle icons for collapsible sections inside tabs
        document.querySelectorAll('.card-header').forEach(header => {
            header.addEventListener('click', function () {
                const icon = this.querySelector('.rotate-icon');
                icon.classList.toggle('rotate');
            });
        });
    </script>
</asp:Content>
