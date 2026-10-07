<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PostingProduct_Receipt.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PostingProduct_Receipt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <style>
        .grid-size {
            max-width: max-content;
            min-width: max-content;
        }
        .d365-toggle-header {
            background-color: #f3f3f3;
            padding: 0.75rem 1rem;
            border: 1px solid #dcdcdc;
            border-radius: 4px;
            font-weight: 600;
            color: #212529;
            cursor: pointer;
            text-decoration: none;
            transition: background-color 0.2s;
            margin-top: 1.25rem;
        }
        .d365-toggle-header:hover {
            background-color: #e5e5e5;
            color: #000;
        }
        .section-title {
            font-size: 0.75rem !important; /* Reduced from 1rem to 12px */
        }
        .rotate-icon {
            transition: transform 0.3s ease;
        }
        .d365-toggle-header[aria-expanded="true"] .rotate-icon {
            transform: rotate(180deg);
        }
        .card-body {
            padding: 1.25rem;
        }
        .gridView {
            width: 100%;
            border-collapse: collapse;
            font-size: 0.85rem;
        }
     /*   .gridView th, .gridView td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }
        .gridView th {
            background-color: #4682b4;
            color: white;
            font-weight: 600;
        }*/
      /*  .gridView tr:nth-child(even) {
            background-color: #f9f9f9;
        }*/
    /*    .gridView tr:hover {
            background-color: #f1f1f1;
        }*/
        .custom-section-title {
            text-transform: uppercase;
            font-size: 0.7rem !important; /* Reduced from 0.8rem to 11.2px */
            color: #666;
            font-weight: 600;
            margin-top: 1rem;
            margin-bottom: 0.5rem;
        }
        .nav-tabs .nav-link.active {
            border-bottom: 3px solid #007bff !important;
            font-weight: bold;
        }
        .general-container {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 15px; /* Reduced from 40px to 15px for less spacing between columns */
            padding: 10px 20px;
        }
        .field-group {
            background: none;          /* remove grey background */
            border: none;              /* remove borders */
            box-shadow: none;          /* remove shadow */
            padding: 2px 5px;         /* Reduced from 5px 10px to minimize internal spacing */
            margin-bottom: 0;          /* Remove bottom margin to control spacing with grid gap */
        }
        .field-group h4 {
            font-size: 12px !important; /* Reduced from 13px to 12px */
            font-weight: 600;
            margin-bottom: 8px;
            color: #444;
            text-transform: uppercase;
            border-bottom: 1px solid #ddd; /* keep subtle underline */
            padding-bottom: 3px;
        }
        label {
            display: block;
            margin-top: 6px;
            font-size: 12px;
            color: #333;
            font-weight: 500;
        }
        .input-text, .input-select, .input-area {
            width: 100%;
            padding: 4px 6px;       /* smaller padding */
            border: 1px solid #ccc;
            border-radius: 4px;
            font-size: 12px;
            margin-top: 2px;
        }
        .input-area {
            resize: vertical;
            min-height: 40px;
        }
        .form-check-input {
            margin-top: 8px;
        }
        .custom-section-title {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 10px;
            text-transform: uppercase;
        }

        .gridView {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 15px;
        }

        .gridView th {
            background-color: #d3d3d3; /* light gray */
            color: black;
            font-weight: 600;
            font-size: 14px;
            padding: 8px;
            border: 1px solid #ccc;
            text-align: left;
        }

        .gridView td {
            border: 1px solid #ccc;
            padding: 8px;
            font-size: 13px;
        }

        .gridView tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .gridView tr:hover {
            background-color: #f1f1f1;
        }

        .card {
            border-radius: 8px;
        }
          #gvPurchases td,
    #gvPurchases th {
        white-space: nowrap; /* Prevent wrapping */
        width: 1%;          /* Auto-fit to content */
    }

    </style>
    <script type="text/javascript">
        // Re-apply collapse icon rotation after partial postbacks
        $(document).ready(function () {
            if (typeof Sys !== 'undefined') {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    $('.d365-toggle-header').each(function () {
                        var $icon = $(this).find('.rotate-icon');
                        if ($(this).attr('aria-expanded') === 'true') {
                            $icon.css('transform', 'rotate(180deg)');
                        } else {
                            $icon.css('transform', 'rotate(0deg)');
                        }
                    });
                });
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="detailUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="alert alert-danger" Style="font-size: 12px; padding: 6px; display: block;" />
            <div class="card shadow-sm">
                <div class="card-body">
                    <!-- Settings Panel -->
                    <a href="#settingsPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-0"
                        data-toggle="collapse" role="button" aria-expanded="true" aria-controls="settingsPanel">
                        <span class="section-title">Settings</span>
                        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                    </a>
                    <div class="collapse show" id="settingsPanel">
                        <div class="container-fluid settings-card">
                            <div class="row">
                                <!-- PARAMETERS -->
                                <div class="col-md-3 form-section mt-2">
                                    <div class="section-title"><b>PARAMETERS</b></div>
                                    <div class="mt-3 field-group">
                                        <label class="form-label">Quantity</label>
                                        <asp:DropDownList ID="ddlQuantity" runat="server" CssClass="input-select" AutoPostBack="true">
                                            <asp:ListItem Text="Ordered quantity" Value="Ordered" />
                                            <asp:ListItem Text="Received quantity" Value="Received" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-check form-switch mb-3 field-group">
                                        <asp:CheckBox ID="chkPosting" runat="server" CssClass="form-check-input" Checked="true" AutoPostBack="true" />
                                        <label class="form-check-label" for="chkPosting">Posting</label>
                                    </div>
                                </div>
                                <!-- PRINT OPTIONS -->
                                <div class="col-md-3 form-section mt-2">
                                    <div class="section-title"><b>PRINT OPTIONS</b></div>
                                    <div class="mt-3 field-group">
                                        <label class="form-label">Print</label>
                                        <asp:DropDownList ID="ddlPrint" runat="server" CssClass="input-select" AutoPostBack="true">
                                            <asp:ListItem Text="Current" Value="Current" />
                                            <asp:ListItem Text="None" Value="None" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-check form-switch field-group">
                                        <asp:CheckBox ID="chkPrintProductReceipt" runat="server" CssClass="form-check-input" AutoPostBack="true" />
                                        <label class="form-check-label" for="chkPrintProductReceipt">Print product receipt</label>
                                    </div>
                                    <div class="form-check form-switch field-group">
                                        <asp:CheckBox ID="chkPrintSalesDocs" runat="server" CssClass="form-check-input" AutoPostBack="true" />
                                        <label class="form-check-label">Print sales documents</label>
                                    </div>
                                    <div class="form-check form-switch field-group">
                                        <asp:CheckBox ID="chkPrintShelfLabels" runat="server" CssClass="form-check-input" AutoPostBack="true" />
                                        <label class="form-check-label">Print shelf labels</label>
                                    </div>
                                    <div class="form-check form-switch field-group">
                                        <asp:CheckBox ID="chkPrintProductLabels" runat="server" CssClass="form-check-input" AutoPostBack="true" />
                                        <label class="form-check-label">Print product labels</label>
                                    </div>
                                    <div class="form-check form-switch field-group">
                                        <asp:CheckBox ID="chkUsePrintManagement" runat="server" CssClass="form-check-input" AutoPostBack="true" />
                                        <label class="form-check-label">Use print management destination</label>
                                    </div>
                                </div>
                                <!-- SETUP -->
                                <div class="col-md-3 form-section mt-2">
                                    <div class="section-title"><b>SETUP</b></div>
                                    <div class="mt-3 field-group">
                                        <label class="form-label">Check credit limit</label>
                                        <asp:DropDownList ID="ddlCreditLimit" runat="server" CssClass="input-select" AutoPostBack="true">
                                            <asp:ListItem Text="None" Value="None" />
                                            <asp:ListItem Text="Balance" Value="Balance" />
                                            <asp:ListItem Text="Extended" Value="Extended" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <!-- SUMMARY PURCHASE -->
                                <div class="col-md-3 form-section mt-2">
                                    <div class="section-title"><b>SUMMARY PURCHASE</b></div>
                                    <div class="mt-3 field-group">
                                        <label class="form-label">Summary update for</label>
                                        <asp:DropDownList ID="ddlSummaryUpdate" runat="server" CssClass="input-select" AutoPostBack="true">
                                            <asp:ListItem Text="None" Value="None" />
                                            <asp:ListItem Text="Invoice" Value="Invoice" />
                                            <asp:ListItem Text="Packing slip" Value="PackingSlip" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Overview Panel -->
                <a href="#overviewPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
    data-toggle="collapse" role="button" aria-expanded="false" aria-controls="overviewPanel">
    <span class="section-title">Overview</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</a>
<div class="collapse mt-2" id="overviewPanel">
    <div class="card shadow-sm">
        <div class="card-body">
            <asp:GridView ID="gvOverview" runat="server" CssClass="table table-striped table-bordered"
                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Overview Data Found.">
                <Columns>
                    <asp:BoundField DataField="Update" HeaderText="Update" />
                    <asp:BoundField DataField="PurchaseOrder" HeaderText="Purchase order" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="ProductReceipt" HeaderText="Product receipt" />
                    <asp:BoundField DataField="ShipmentNumber" HeaderText="Shipment number" />
                    <asp:BoundField DataField="ProductReceiptDate" HeaderText="Product receipt date" 
                        DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
                    <asp:BoundField DataField="DocumentDate" HeaderText="Document date" 
                        DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
                    <asp:BoundField DataField="TermsOfPayment" HeaderText="Terms of payment" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

                    <!-- Lines Panel -->
                  <a href="#linesPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
    data-toggle="collapse" role="button" aria-expanded="false" aria-controls="linesPanel">
    <span class="section-title">Lines</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</a>

 <div class="collapse mt-2" id="linesPanel">
        <div class="card shadow-sm">
            <div class="card-body">
              <%--  <div class="custom-section-title mb-3"><strong>LINES</strong></div>--%>
             <asp:GridView ID="gvLines" runat="server" 
    CssClass="table table-striped table-bordered table-hover grid-size"
    AutoGenerateColumns="false"
    ShowHeaderWhenEmpty="true"
    EmptyDataText="No Lines Found.">
    <Columns>
        <asp:BoundField DataField="PurchaseOrder" HeaderText="Purchase Order" />
        <asp:BoundField DataField="LineNumber" HeaderText="Line Number" />
        <asp:BoundField DataField="ProductNumber" HeaderText="Product Number" />
        <asp:BoundField DataField="ItemNumber" HeaderText="Item Number" />
        <asp:BoundField DataField="ProcurementCategory" HeaderText="Procurement Category" />
        <asp:BoundField DataField="Text" HeaderText="Text" />
        <asp:BoundField DataField="Site" HeaderText="Site" />
        <asp:BoundField DataField="Warehouse" HeaderText="Warehouse" />
        <asp:BoundField DataField="InventoryStatus" HeaderText="Inventory Status" />
        <asp:BoundField DataField="CWUpdate" HeaderText="CW Update" />
        <asp:BoundField DataField="QuantityOrder" HeaderText="Quantity Order" />
        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
        <asp:BoundField DataField="DeliveryRemaining" HeaderText="Delivery Remaining" />
        <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" />
        <asp:BoundField DataField="VendorBatchDate" HeaderText="Vendor Batch Date" 
                        DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
        <asp:BoundField DataField="LineNetAmount" HeaderText="Line Net Amount" />
        <asp:BoundField DataField="VendorExpiryDate" HeaderText="Vendor Expiry Date" 
                        DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
        <asp:TemplateField HeaderText="Close for Receipt">
            <ItemTemplate>
                <asp:CheckBox ID="chkCloseForReceipt" runat="server" 
                              Checked='<%# Convert.ToBoolean(Eval("CloseForReceipt")) %>' 
                              Enabled="false" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Backorder" HeaderText="Backorder" />
        <asp:BoundField DataField="QualityOrderStatus" HeaderText="Quality Order Status" />
    </Columns>
</asp:GridView>

            </div>
        </div>
    </div>

  <!-- Details Panel Header -->
                    <a href="#detailsPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                        data-toggle="collapse" role="button" aria-expanded="false" aria-controls="detailsPanel">
                        <span class="section-title">Details</span>
                        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                    </a>
                    <!-- Collapsible Details Panel -->
                    <div class="collapse mt-2" id="detailsPanel">
                        <div class="card card-body">
                            <!-- Nav Tabs -->
                            <ul class="nav nav-tabs" id="detailsTab" role="tablist">
                                <li class="nav-item">
                                    <a class="nav-link active" id="general-tab" data-toggle="tab" href="#general" role="tab">General</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="purchases-tab" data-toggle="tab" href="#purchases" role="tab">Purchases</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="fixedassets-tab" data-toggle="tab" href="#fixedassets" role="tab">Fixed Assets</a>
                                </li>
                            </ul>
                            <!-- Tab Contents -->
                            <div class="tab-content mt-3" id="detailsTabContent">
                                <!-- GENERAL TAB -->
                                <div class="tab-pane fade show active" id="general" role="tabpanel" aria-labelledby="general-tab">
                                    <div class="general-container">
                                        <!-- PRICE -->
                                        <div class="field-group">
                                            <h4>PRICE</h4>
                                            <label>Unit price</label>
                                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="input-text" Text="20.00" AutoPostBack="true"></asp:TextBox>
                                            <label>Price unit</label>
                                            <asp:TextBox ID="txtPriceUnit" runat="server" CssClass="input-text" Text="1.00" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <!-- DISCOUNT -->
                                        <div class="field-group">
                                            <h4>DISCOUNT</h4>
                                            <label>Discount</label>
                                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="input-text" Text="0.00" AutoPostBack="true"></asp:TextBox>
                                            <label>Discount percent</label>
                                            <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="input-text" Text="0.00" AutoPostBack="true"></asp:TextBox>
                                            <label>Multiline discount</label>
                                            <asp:TextBox ID="txtMultilineDiscount" runat="server" CssClass="input-text" Text="0.00" AutoPostBack="true"></asp:TextBox>
                                            <label>Multiline discount percentage</label>
                                            <asp:TextBox ID="txtMultilineDiscountPct" runat="server" CssClass="input-text" Text="0.00" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <!-- INVENTORY -->
                                        <div class="field-group">
                                            <h4>INVENTORY</h4>
                                            <label>Update</label>
                                            <asp:TextBox ID="txtUpdate" runat="server" CssClass="input-text" Text="1.00" AutoPostBack="true"></asp:TextBox>
                                            <label>CW update</label>
                                            <asp:TextBox ID="txtCWUpdate" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                                            <label>Deliver remainder</label>
                                            <asp:TextBox ID="txtDeliverRemainder" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                                            <label>CW deliver remainder</label>
                                            <asp:TextBox ID="txtCWDeliverRemainder" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <!-- CHARGES -->
                                        <div class="field-group">
                                            <h4>CHARGES</h4>
                                            <label>Charges on purchases</label>
                                            <asp:TextBox ID="txtCharges" runat="server" CssClass="input-text" Text="0.00" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <!-- ORDER LINE -->
                                        <div class="field-group">
                                            <h4>ORDER LINE</h4>
                                            <label>Procurement category</label>
                                            <asp:TextBox ID="txtProcurementCategory" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                                            <label>Text</label>
                                            <asp:TextBox ID="txtOrderLineText" runat="server" CssClass="input-area" TextMode="MultiLine" Rows="3" Text="IPHONE&#13;&#10;10 Black" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <!-- PRODUCT RECEIPT -->
                                        <div class="field-group">
                                            <h4>PRODUCT RECEIPT</h4>
                                            <label>Deliver remainder</label>
                                            <asp:TextBox ID="txtProdDeliverRemainder" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <!-- REASONS -->
                                        <div class="field-group">
                                            <h4>REASONS</h4>
                                            <label>Reason</label>
                                            <asp:DropDownList ID="ddlReason" runat="server" CssClass="input-select" AutoPostBack="true">
                                                <asp:ListItem Text="--Select--" Value="" />
                                                <asp:ListItem Text="Damaged" Value="Damaged" />
                                                <asp:ListItem Text="Expired" Value="Expired" />
                                                <asp:ListItem Text="Customer Return" Value="CustomerReturn" />
                                            </asp:DropDownList>
                                            <label>Reason comment</label>
                                            <asp:TextBox ID="txtReasonComment" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <!-- PURCHASE LINE -->
                                        <div class="field-group">
                                            <h4>PURCHASE LINE</h4>
                                            <label>Line description</label>
                                            <asp:TextBox ID="txtPurchaseLineDesc" runat="server" CssClass="input-area" TextMode="MultiLine" Rows="3" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <!-- VENDOR BATCH -->
                                        <div class="field-group">
                                            <h4>VENDOR BATCH</h4>
                                            <label>Country/region of Origin 1</label>
                                            <asp:TextBox ID="txtCountryOrigin1" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                                            <label>Country/region of Origin 2</label>
                                            <asp:TextBox ID="txtCountryOrigin2" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                                            <label>Vendor batch number</label>
                                            <asp:TextBox ID="txtVendorBatchNo" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <!-- DELIVERY -->
                                        <div class="field-group">
                                            <h4>DELIVERY</h4>
                                            <label>Postal address</label>
                                            <asp:DropDownList ID="ddlPostalAddress" runat="server" CssClass="input-select" AutoPostBack="true">
                                                <asp:ListItem Text="Site 1" Value="1" />
                                                <asp:ListItem Text="Site 2" Value="2" />
                                            </asp:DropDownList>
                                            <label>Postal date</label>
                                            <asp:TextBox ID="txtPostalDate" runat="server" CssClass="input-text" Text="1/2/2009" AutoPostBack="true"></asp:TextBox>
                                            <label>Name</label>
                                            <asp:TextBox ID="txtDeliveryName" runat="server" CssClass="input-text" Text="Site 1" AutoPostBack="true"></asp:TextBox>
                                            <label>Address</label>
                                            <asp:TextBox ID="txtDeliveryAddress" runat="server" CssClass="input-area" TextMode="MultiLine" Rows="3" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <!-- PURCHASES TAB -->
                        <div class="tab-pane fade" id="purchases" role="tabpanel" aria-labelledby="purchases-tab">
    <asp:GridView ID="gvPurchases" runat="server" 
        CssClass="table table-striped table-bordered table-hover"
        AutoGenerateColumns="false"
        ShowHeaderWhenEmpty="true" 
        EmptyDataText="No Purchases Data Found.">
        <Columns>
            <asp:BoundField DataField="PurchaseOrder" HeaderText="Purchase Order" />
            <asp:BoundField DataField="Name" HeaderText="Name">
                <ItemStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
</div>



                                <!-- FIXED ASSETS TAB -->
                                <!-- FIXED ASSETS TAB -->
<!-- FIXED ASSETS TAB -->
<div class="tab-pane fade" id="fixedassets" role="tabpanel" aria-labelledby="fixedassets-tab">
    <div class="container-fluid settings-card">
        <div class="row">
            <!-- FIXED ASSETS COLUMN -->
            <div class="col-md-3 form-section mt-2">
                <div class="section-title"><b>FIXED ASSETS</b></div>

                <!-- NEW FIXED ASSET TOGGLE -->
                <%--<div class="form-check form-switch field-group mt-2">
                    <asp:CheckBox ID="chkNewFixedAsset" runat="server" CssClass="form-check-input" AutoPostBack="true" />
                    <label class="form-check-label" for="chkNewFixedAsset">New fixed asset</label>
                </div>--%>
                 <div class="form-check form-switch mb-3 field-group">
     <asp:CheckBox ID="chckNewFixedAsset" runat="server" CssClass="form-check-input" Checked="true" AutoPostBack="false" />
     <label class="form-check-label" for="chkNewFixedAsset"></label>
 </div>

                <!-- FIXED ASSET GROUP -->
                <div class="mt-2 field-group">
                    <label class="form-label">Fixed asset group</label>
                    <asp:TextBox ID="txtFixedAssetGroup" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                </div>

                <!-- FIXED ASSET NUMBER -->
                <div class="mt-2 field-group">
                    <label class="form-label">Fixed asset number</label>
                    <asp:TextBox ID="txtFixedAssetNumber" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                </div>
            </div>

            <!-- BOOK COLUMN -->
            <div class="col-md-3 form-section mt-2">
                <%--<div class="section-title"><b>BOOK</b></div>--%>

                <div class="mt-2 field-group">
                    <label class="form-label">Book</label>
                   <%-- <asp:DropDownList ID="ddlBook" runat="server" CssClass="input-select" AutoPostBack="true">
                        <asp:ListItem Text="--Select--" Value="" />
                        <asp:ListItem Text="Book 1" Value="Book1" />
                        <asp:ListItem Text="Book 2" Value="Book2" />
                    </asp:DropDownList>--%>
                     <asp:TextBox ID="txtBook" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                </div>
            </div>

            <!-- TRANSACTION TYPE COLUMN -->
            <div class="col-md-3 form-section mt-2">
              <%--  <div class="section-title"><b>TRANSACTION</b></div>--%>

                <div class="mt-2 field-group">
                    <label class="form-label">Transaction type</label>
                 <%--   <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="input-select">
                        <asp:ListItem Text="Acquisition" Value="Acquisition" />
                        <asp:ListItem Text="Depreciation" Value="Depreciation" />
                        <asp:ListItem Text="Disposal" Value="Disposal" />
                    </asp:DropDownList>--%>
                     <asp:TextBox ID="txttransactionType" runat="server" CssClass="input-text" AutoPostBack="true"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
</div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <div class="action-footer">
     <asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click">OK</asp:LinkButton>
  <asp:LinkButton ID="btnCancel" runat="server" 
    OnClientClick="window.location.href='/ESS/PR/AllPurchaseOrder_ListPage.aspx'; return false;">
    Cancel
</asp:LinkButton>

 </div>
</asp:Content>