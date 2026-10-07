<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" 
    CodeBehind="PurchaseOrder_ReceiptLists.aspx.cs" 
    Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_ReceiptLists" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .toggle-switch {
    position: relative;
    display: inline-block;
    width: 50px;
    height: 24px;
}

.toggle-switch input {
    opacity: 0;
    width: 0;
    height: 0;
}

.slider {
    position: absolute;
    cursor: pointer;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: #ccc;
    transition: .4s;
    border-radius: 24px;
}

.slider:before {
    position: absolute;
    content: "";
    height: 18px;
    width: 18px;
    left: 3px;
    bottom: 3px;
    background-color: white;
    transition: .4s;
    border-radius: 50%;
}

input:checked + .slider {
    background-color: #4CAF50;
}

input:checked + .slider:before {
    transform: translateX(26px);
}

        .card-header a {
            text-decoration: none;
            color: #000;
        }

        .rotate-icon {
            transition: transform 0.3s ease;
        }

        .collapsed .rotate-icon {
            transform: rotate(180deg);
        }

        /* ✅ Adjust spacing: small left indent, less top margin */
        .card-body {
            padding-left: 2px !important;   /* slight indent */
            padding-top: 5px !important;    /* reduced top spacing */
            margin-left: 2px !important;    /* align cleanly */
            margin-top: 0 !important;       /* remove extra top gap */
        }

        /* Ensure grid spans card width */
        #gvOverview {
            width: 100% !important;
            margin: 0 !important;
        }

        /* Optional: make header look tighter */
        .table > thead > tr > th {
            padding-top: 6px !important;
            padding-bottom: 6px !important;
        }

        .table > tbody > tr > td {
            padding-top: 4px !important;
            padding-bottom: 4px !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
        <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
<ContentTemplate>
 <div class="container-fluid mt-2">
    <div id="accordion">

        <div class="card mb-2 shadow-sm">
            <div class="card-header p-2 bg-light" id="headingDetails">
                <a class="collapsed d-flex justify-content-between align-items-center"
                   data-toggle="collapse"
                   href="#collapseDetails" aria-expanded="false" aria-controls="collapseDetails">
                    <strong>Settings</strong>
                    <i class="fa fa-chevron-down rotate-icon"></i>
                </a>
            </div>

            <div id="collapseDetails" class="collapse">
                <div class="card-body">

                    <div class="row">

                        <!-- ================= COLUMN 1 ================= -->
                        <div class="col-md-4">
                            <h6 class="font-weight-bold mb-2">Parameters</h6>

                            <div class="form-group" style="width: 250px;">
                                <label for="ddlQuantity">Quantity</label>
                                <asp:DropDownList 
                                    ID="ddlQuantity"
                                    runat="server"
                                    CssClass="form-select"
                                    Width="100%"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>

                            <div class="form-group mt-2">
                                <label class="d-block">Posting</label>
                                <div class="toggle-group mt-2">
                                    <label class="toggle-switch">
                                        <input type="checkbox" id="togglePosting" checked />
                                        <span class="slider"></span>
                                    </label>
                                </div>
                            </div>
                        </div>

                        <!-- ================= COLUMN 2 ================= -->
                        <div class="col-md-4">
                            <h6 class="font-weight-bold mb-2">Print Options</h6>

                            <div class="form-group" style="width: 250px;">
                                <label for="ddlPrint">Print Options</label>
                                <asp:DropDownList
                                    ID="ddlPrint"
                                    runat="server"
                                    CssClass="form-select"
                                    Width="100%"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <!-- ================= COLUMN 3 ================= -->
                        <div class="col-md-4">
                            <h6 class="font-weight-bold mb-2">Setup</h6>

                            <div class="form-group" style="width: 250px;">
                                <label for="ddlCheckCredit">Check Credit Limit</label>
                                <asp:DropDownList
                                    ID="ddlCheckCredit"
                                    runat="server"
                                    CssClass="form-select"
                                    Width="100%"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>

                            <h6 class="font-weight-bold mt-3 mb-2">Summary Purchase</h6>

                            <div class="form-group" style="width: 250px;">
                                <label for="ddlSummary">Summary Update For</label>
                                <asp:DropDownList
                                    ID="ddlSummary"
                                    runat="server"
                                    CssClass="form-select"
                                    Width="100%"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div> <!-- row end -->

                </div>
            </div>

        </div>

    </div>
</div>


            <div class="card mb-2 shadow-sm">
                <div class="card-header p-2 bg-light" id="headingOverview">
                    <a class="d-flex justify-content-between align-items-center" data-toggle="collapse" 
                       href="#collapseOverview" aria-expanded="true" aria-controls="collapseOverview">
                        <strong>Overview</strong>
                        <i class="fa fa-chevron-down rotate-icon"></i>
                    </a>
                </div>

                <div id="collapseOverview" class="collapse show">
                    <div class="card-body text-left">
                        <asp:GridView ID="gvOverview" runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-striped table-sm"
                            ShowHeaderWhenEmpty="true">
                            <Columns>
                            
                                <asp:TemplateField HeaderText="Update">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUpdate" runat="server"
                                            CssClass="form-control form-control-sm"
                                            Text='<%# Bind("displayOrdering") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Purchase Order">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPurchaseOrder" runat="server"
                                            CssClass="form-control form-control-sm"
                                            Text='<%# Bind("PurchId") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server"
                                            CssClass="form-control form-control-sm"
                                            Text='<%# Bind("PurchName") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Receipt List Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtReceiptListDate" runat="server"
                                            CssClass="form-control form-control-sm text-center"
                                            Text='<%# Bind("TransDate", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Document Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDocumentDate" runat="server"
                                            CssClass="form-control form-control-sm text-center"
                                            Text='<%# Bind("DocumentDate", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Terms of Payment">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTermsOfPayment" runat="server"
                                            CssClass="form-control form-control-sm"
                                            Text='<%# Bind("Payment") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

         
     <div class="card mb-2 shadow-sm">
    <div class="card-header p-2 bg-light" id="headingLines">
        <a class="collapsed d-flex justify-content-between align-items-center" data-toggle="collapse"
           href="#collapseLines" aria-expanded="false" aria-controls="collapseLines">
            <strong>Lines</strong>
            <i class="fa fa-chevron-down rotate-icon"></i>
        </a>
    </div>

    <div id="collapseLines" class="collapse">
        <div class="card-body">
            <!-- Scroll wrapper if many columns -->
            <div class="table-responsive">
                <asp:GridView ID="gvLines" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered table-striped table-sm"
                    ShowHeaderWhenEmpty="true">
                    <Columns>
                    <asp:TemplateField Visible="false">
    <ItemTemplate>
        <asp:Label ID="hfRecId" runat="server" Text='<%# Bind("RecId") %>' />
    </ItemTemplate>
</asp:TemplateField>
                        <asp:TemplateField HeaderText="Purchase Order">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("PurchaseOrderId") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Line Number">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLineNumber" runat="server"
                                    CssClass="form-control form-control-sm text-center"
                                    Text='<%# Bind("LineNumber") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                      <%--  <asp:TemplateField HeaderText="Product Number">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProductNumber" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("ProductNumber") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Item Number">
                            <ItemTemplate>
                                <asp:TextBox ID="txtItemNumber" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("ItemId") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Procurement Category">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProcurementCategory" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("ProcurementCategory") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Text">
                            <ItemTemplate>
                                <asp:TextBox ID="txtText" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("ItemName") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Site">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSite" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("InventSiteId") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Warehouse">
                            <ItemTemplate>
                                <asp:TextBox ID="txtWarehouse" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("InventLocationID") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

<%--                        <asp:TemplateField HeaderText="Inventory Status">
                            <ItemTemplate>
                                <asp:TextBox ID="txtInventoryStatus" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("InventoryStatus") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                      <%--  <asp:TemplateField HeaderText="CW Update">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCWUpdate" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("CWUpdate") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Quantity Ordered">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantityOrdered" runat="server"
                                    CssClass="form-control form-control-sm text-right"
                                    Text='<%# Bind("PurchQty", "{0:N2}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                      <%--  <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantity" runat="server"
                                    CssClass="form-control form-control-sm text-right"
                                    Text='<%# Bind("PurchQty", "{0:N2}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Deliver Remainder">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDeliverRemainder" runat="server"
                                    CssClass="form-control form-control-sm text-right"
                                    Text='<%# Bind("DeliverRemainder") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Unit Price">
                            <ItemTemplate>
                                <asp:TextBox ID="txtUnitPrice" runat="server"
                                    CssClass="form-control form-control-sm text-right"
                                    Text='<%# Bind("PurchPrice", "{0:N2}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                    <%--    <asp:TemplateField HeaderText="Vendor Batch Date">
                            <ItemTemplate>
                                <asp:TextBox ID="txtVendorBatchDate" runat="server"
                                    CssClass="form-control form-control-sm text-center"
                                    Text='<%# Bind("VendorBatchDate", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Line Net Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLineNetAmount" runat="server"
                                    CssClass="form-control form-control-sm text-right"
                                    Text='<%# Bind("LineAmount", "{0:N2}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                      <%--  <asp:TemplateField HeaderText="Vendor Expiry Date">
                            <ItemTemplate>
                                <asp:TextBox ID="txtVendorExpiryDate" runat="server"
                                    CssClass="form-control form-control-sm text-center"
                                    Text='<%# Bind("VendorExpiryDate", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Quality Order Status">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQualityOrderStatus" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("QualityOrderStatus") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>

           
      <div class="col text-right">

        <asp:Button 
            ID="btnOk" 
            runat="server" 
            Text="OK" 
            CssClass="btn btn-primary"
            OnClick="btnOk_Click" />

        <asp:Button 
            ID="btnCancel" 
            runat="server" 
            Text="Cancel" 
            CssClass="btn btn-secondary "
            OnClick="btnCancel_Click" />

    </div>
     


          
<%-- <div class="card mb-2 shadow-sm">
    <div class="card-header p-2 bg-light" id="headingDetails">
        <a class="collapsed d-flex justify-content-between align-items-center" data-toggle="collapse"
           href="#collapseDetails" aria-expanded="false" aria-controls="collapseDetails">
            <strong>Details</strong>
            <i class="fa fa-chevron-down rotate-icon"></i>
        </a>
    </div>

    <div id="collapseDetails" class="collapse">
        <div class="card-body">

           
            <ul class="nav nav-tabs" id="detailsTabs" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="general-tab" data-toggle="tab" href="#generalTab" role="tab">General</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="purchases-tab" data-toggle="tab" href="#purchasesTab" role="tab">Purchases</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="fixedAssets-tab" data-toggle="tab" href="#fixedAssetsTab" role="tab">Fixed Assets</a>
                </li>
            </ul>

            <div class="tab-content mt-3" id="detailsTabsContent">

             
                <div class="tab-pane fade show active" id="generalTab" role="tabpanel">

                   
                    <div class="row">
                     
                        <div class="col-md-2">
                            <h6 class="font-weight-bold">Price</h6>
                            <div class="form-group mb-2">
                                <label>Unit Price</label>
                                <input type="text" class="form-control" id="textUnitPrice" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Price Unit</label>
                                <input type="text" class="form-control" id="txtPriceUnit" />
                            </div>

                            <h6 class="font-weight-bold mt-3">Charges</h6>
                            <div class="form-group mb-2">
                                <label>Charges on Purchases</label>
                                <input type="text" class="form-control" id="txtChargesOnPurchases" />
                            </div>

                            <h6 class="font-weight-bold mt-3">Order Line</h6>
                            <div class="form-group mb-2">
                                <label>Procurement Category</label>
                                <input type="text" class="form-control" id="textProcurementCategory" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Text</label>
                                <input type="text" class="form-control" id="txtText" />
                            </div>
                        </div>

                      
                        <div class="col-md-2">
                            <h6 class="font-weight-bold">Discount</h6>
                            <div class="form-group mb-2">
                                <label>Discount</label>
                                <input type="text" class="form-control" id="txtDiscount" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Discount Percent</label>
                                <input type="text" class="form-control" id="txtDiscountPercent" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Multiline Discount</label>
                                <input type="text" class="form-control" id="txtMultilineDiscount" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Multiline Discount Percentage</label>
                                <input type="text" class="form-control" id="txtMultilineDiscountPercentage" />
                            </div>

                            <h6 class="font-weight-bold mt-3">Product Receipt</h6>
                            <div class="form-group mb-2">
                                <label>Deliver Remainder</label>
                                <input type="text" class="form-control" id="textDeliverRemainder" />
                            </div>
                        </div>

                        <div class="col-md-2">
                            <h6 class="font-weight-bold">Inventory</h6>
                            <div class="form-group mb-2">
                                <label>Update</label>
                                <input type="text" class="form-control" id="textUpdate" />
                            </div>
                            <div class="form-group mb-2">
                                <label>CW Update</label>
                                <input type="text" class="form-control" id="textCWUpdate" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Deliver Remainder</label>
                                <input type="text" class="form-control" id="txtInventoryDeliverRemainder" />
                            </div>
                            <div class="form-group mb-2">
                                <label>CW Deliver Remainder</label>
                                <input type="text" class="form-control" id="txtCWDeliverRemainder" />
                            </div>

                            <h6 class="font-weight-bold mt-3">Reasons</h6>
                            <div class="form-group mb-2">
                                <label>Reason</label>
                                <input type="text" class="form-control" id="txtReason" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Reason Comment</label>
                                <textarea class="form-control" id="txtReasonComment" rows="2"></textarea>
                            </div>
                        </div>

                      
                        <div class="col-md-3">
                            <h6 class="font-weight-bold">Purchase Line</h6>
                            <div class="form-group mb-2">
                                <label>Line Description</label>
                                <input type="text" class="form-control" id="txtLineDescription" />
                            </div>

                            <h6 class="font-weight-bold mt-3">Delivery</h6>
                            <div class="form-group mb-2">
                                <label>Postal Address</label>
                                <input type="text" class="form-control" id="txtPostalAddress" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Name</label>
                                <input type="text" class="form-control" id="txtName" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Address</label>
                                <textarea class="form-control" id="txtAddress" rows="2"></textarea>
                            </div>
                        </div>

                        <!-- Vendor Batch Section (Right of Purchase Line) -->
                        <div class="col-md-3">
                            <h6 class="font-weight-bold">Vendor Batch</h6>
                            <div class="form-group mb-2">
                                <label>Country/Region of Origin 1</label>
                                <input type="text" class="form-control" id="txtCountryOrigin1" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Country/Region of Origin 2</label>
                                <input type="text" class="form-control" id="txtCountryOrigin2" />
                            </div>
                            <div class="form-group mb-2">
                                <label>Vendor Batch Number</label>
                                <input type="text" class="form-control" id="txtVendorBatchNumber" />
                            </div>
                        </div>
                    </div>

                </div>

                  <div class="tab-pane fade" id="purchasesTab" role="tabpanel">
    <style>
        /* Reduce space inside GridView cells */
        #purchasesTab .table td, 
        #purchasesTab .table th {
            padding: 4px 6px !important; /* default is ~12px */
        }

        /* Reduce margin between textboxes */
        #purchasesTab .form-control-sm {
            padding: 2px 4px !important;
            margin: 0 !important;
            height: 28px !important;
        }

        /* Optional: make columns a bit tighter */
        #purchasesTab .table {
            border-collapse: collapse;
        }
    </style>

    <asp:GridView ID="gvPurchases" runat="server" AutoGenerateColumns="False"
        CssClass="table table-bordered table-striped table-sm" ShowHeaderWhenEmpty="true">
        <Columns>
            <asp:TemplateField HeaderText="Purchase Order">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server"
                        CssClass="form-control form-control-sm"
                        Text='<%# Bind("PurchaseOrder") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox3" runat="server"
                        CssClass="form-control form-control-sm"
                        Text='<%# Bind("Name") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
<div class="tab-pane fade" id="fixedAssetsTab" role="tabpanel">
    <div class="p-3">
        <h6 class="font-weight-bold mb-3">FIXED ASSETS</h6>

        <div class="form-row align-items-center mb-3">
          
            <div class="form-group col-md-2">
                <label class="d-block">New fixed asset?</label>
                <div class="custom-control custom-switch">
                    <asp:CheckBox ID="chkNewFixedAsset" runat="server" CssClass="custom-control-input" />
                    <label class="custom-control-label" for="chkNewFixedAsset">No</label>
                </div>
            </div>

           
            <div class="form-group col-md-3">
                <label>Fixed asset group</label>
                <asp:TextBox ID="txtFixedAssetGroup" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>

            <div class="form-group col-md-3">
                <label>Fixed asset number</label>
                <asp:DropDownList ID="ddlFixedAssetNumber" runat="server" CssClass="form-control form-control-sm">
                    <asp:ListItem Text="Select" Value="" />
                </asp:DropDownList>
            </div>

           
            <div class="form-group col-md-2">
                <label>Book</label>
                <asp:DropDownList ID="ddlBook" runat="server" CssClass="form-control form-control-sm">
                    <asp:ListItem Text="Select" Value="" />
                </asp:DropDownList>
            </div>

         
            <div class="form-group col-md-2">
                <label>Transaction type</label>
                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control form-control-sm">
                    <asp:ListItem Text="Acquisition" Value="Acquisition" />
                    <asp:ListItem Text="Disposal" Value="Disposal" />
                    <asp:ListItem Text="Depreciation" Value="Depreciation" />
                </asp:DropDownList>
            </div>
        </div>
    </div>
</div>

            </div>
        </div>
    </div>
</div>--%>
                </ContentTemplate>
</asp:UpdatePanel>
       
</asp:Content>
