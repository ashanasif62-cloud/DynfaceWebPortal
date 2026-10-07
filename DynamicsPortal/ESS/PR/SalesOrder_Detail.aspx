<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesOrder_Detail.aspx.cs" Inherits="DynamicsPortal.ESS.PR.SalesOrder_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <a href="/ESS/PR/SalesOrder_ListPage.aspx" class="btn-link">
            <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
        </a>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="BtnSaveHeader" runat="server" OnClick="BtnSave_Header_Click">
            <i class="mdi mdi-content-save" style="margin-right: 4px;"></i>Save
        </asp:LinkButton>
    </div>

    <style>
        /* 200px Width for TextBoxes & Dropdowns */
        input.form-control,
        textarea.form-control,
        select.form-control {
            width: 200px !important;
        }

        .form-control[readonly] {
            background-color: #f2f2f2 !important;
            color: grey;
        }

        /* D365 Style Panel Header */
        .d365-toggle-header {
            background-color: #e6f0fa;
            color: #1a1a1a;
            padding: 10px 15px;
            border: 1px solid #c5d9f1;
            border-radius: 4px;
            font-weight: 600;
            text-decoration: none !important;
            margin-bottom: 10px;
        }

        .d365-toggle-header:hover {
            background-color: #d4e6f7;
            text-decoration: none;
        }

        .section-title {
            font-size: 15px;
        }

        .rotate-icon {
            transition: transform 0.3s ease;
        }

        /* Reduce spacing between form fields */
        .form-group {
            margin-bottom: 6px;
        }

        /* Column headings style */
        .column-heading {
            font-weight: 600;
            margin-bottom: 4px;
            font-size: 14px;
        }

        /* GridView styling for tighter columns */
        .salesorder-grid th,
        .salesorder-grid td {
            padding: 4px 6px !important;
            white-space: nowrap;
            vertical-align: middle;
        }

        .salesorder-grid {
            border-collapse: collapse !important;
            width: 100% !important;
        }

        .salesorder-grid th {
            font-weight: 600;
            background-color: #f5f5f5;
        }

        .salesorder-grid td {
            font-size: 0.85rem;
        }

        /* Action buttons spacing */
        .grid-btn {
            margin-right: 8px;
        }

        /* Scrollable grid container */
        .grid-scroll {
            overflow-x: auto;
            overflow-y: auto;
            max-height: 400px;
            border: 1px solid #e0e0e0;
            padding: 5px;
        }

        .five-col {
            flex: 0 0 20%;
            max-width: 20%;
        }

        /* Toggle Switch */
        .switch {
            position: relative;
            display: inline-block;
            width: 42px;
            height: 22px;
        }

        .switch input {
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
            transition: .3s;
            border-radius: 34px;
        }

        .slider:before {
            position: absolute;
            content: "";
            height: 16px;
            width: 16px;
            left: 3px;
            bottom: 3px;
            background-color: white;
            transition: .3s;
            border-radius: 50%;
        }

        input:checked + .slider {
            background-color: #0078D4;
        }

        input:checked + .slider:before {
            transform: translateX(20px);
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
    <ul class="nav nav-tabs" id="salesOrderTabs" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" data-toggle="tab" href="#line">Line</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#header">Header</a>
        </li>
    </ul>

    <div class="tab-content mt-3">
        <!-- LINE TAB -->
        <div class="tab-pane fade show active" id="line">
            <!-- ===================== SALES ORDER HEADER PANEL ===================== -->
            <a href="#collapseSalesOrderHeader"
               class="d365-toggle-header d-flex justify-content-between align-items-center"
               data-toggle="collapse"
               role="button"
               aria-expanded="true"
               aria-controls="collapseSalesOrderHeader">
                <span class="section-title">Sales Order Header</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>

            <div id="collapseSalesOrderHeader" class="collapse show">
                <div class="card card-body">
                    <div class="row">
                        <!-- COLUMN 1: DELIVERY ADDRESS -->
                        <div class="col-md-3">
                            <div class="column-heading">DELIVERY ADDRESS</div>
                            <div class="form-group">
                                <label>Name</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Delivery Address</label>
                                <asp:TextBox ID="txtDeliveryAddress" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Address</label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" ReadOnly="true" />
                            </div>
                        </div>

                        <!-- COLUMN 2: DELIVERY DATE -->
                        <div class="col-md-3">
                            <div class="column-heading">DELIVERY DATE</div>
                            <div class="form-group">
                                <label>Requested Ship Date</label>
                                <asp:TextBox ID="txtRequestedShipDate" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                            <div class="form-group">
                                <label>Requested Receipt Date</label>
                                <asp:TextBox ID="txtRequestedReceiptDate" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                            <div class="column-heading">Simulate Delivery Dates</div>
                            <div class="form-group">
                                <label>Confirmed Ship Date</label>
                                <asp:TextBox ID="txtConfirmedShipDate" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                            <div class="form-group">
                                <label>Confirmed Receipt Date</label>
                                <asp:TextBox ID="txtConfirmedReceiptDate" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                        </div>

                        <!-- COLUMN 3: REFERENCES / DISCOUNTS / WAREHOUSE -->
                        <div class="col-md-3">
                            <div class="column-heading">REFERENCES</div>
                            <div class="form-group">
                                <label>Customer Reference</label>
                                <asp:TextBox ID="txtCustomerReference" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Customer Requisition</label>
                                <asp:TextBox ID="txtCustomerRequisition" runat="server" CssClass="form-control" />
                            </div>
                            <div class="column-heading">Discounts</div>
                            <div class="form-group">
                                <label>Total Discount %</label>
                                <asp:TextBox ID="txtTotalDiscount" runat="server" CssClass="form-control" />
                            </div>
                            <div class="column-heading">Warehouse</div>
                            <div class="form-group">
                                <label>Release Status</label>
                                <asp:TextBox ID="txtReleaseStatus" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <!-- COLUMN 4: POLICIES & TRANSPORTATION -->
                        <div class="col-md-3">
                            <div class="form-group">
                                <label>Order Fulfillment Policy</label>
                                <asp:DropDownList ID="ddlOrderFulfillmentPolicy" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="form-group">
                                <label>Default Fulfillment Policy</label>
                                <asp:TextBox ID="txtDefaultFulfillmentPolicy" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="form-group">
                                <label>Outbound Shipment Processing Policy</label>
                                <asp:DropDownList ID="ddlOutboundShipmentPolicy" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="column-heading">Transportation</div>
                            <div class="form-group">
                                <label>Routes</label>
                                <asp:TextBox ID="txtRoutes" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Carrier Customer Account Number</label>
                                <asp:DropDownList ID="ddlCarrierCustomerAccount" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="column-heading">Distributed order management</div>
                            <div class="form-group">
                                <label>DOM Status</label>
                                <asp:TextBox ID="txtDomStatus" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- ===================== SALES ORDER LINES PANEL ===================== -->
            <a class="d365-toggle-header d-flex justify-content-between align-items-center mt-4">
                <span class="section-title">Sales Order Lines</span>
            </a>
            
            <div class="text-left d-flex align-items-center gap-3 mb-2">
                <div class="action-items grid-btn">
                    <asp:LinkButton ID="BtnAddLine" runat="server" OnClick="btnNew_Grid_Click" CssClass="text-primary" style="font-size: 0.9rem;">
                        <i class="mdi mdi-plus" style="font-size: 0.9rem;"></i> Add line
                    </asp:LinkButton>
                </div>
                <div class="action-items grid-btn">
                    <asp:LinkButton ID="btnDeleteLine" CssClass="text-primary" runat="server" OnClick="btnDelete_Click" style="font-size: 0.9rem;">
                        <i class="mdi mdi-delete" style="font-size: 0.9rem;"></i> Remove
                    </asp:LinkButton>
                </div>
                <div class="action-items grid-btn">
                    <asp:LinkButton ID="btnSaveLine" CssClass="text-primary" runat="server" OnClick="btnSave_Click" style="font-size: 0.9rem;">
                        <i class="mdi mdi-content-save" style="font-size: 0.9rem;"></i> Save
                    </asp:LinkButton>
                </div>
            </div>

            <div class="card shadow-sm mt-2">
                <div class="card-body pt-2 pb-2">
                    <!-- GridView Scrollable Container -->
                    <div class="grid-scroll">
                        <asp:GridView ID="gvSalesOrderLines" runat="server" 
                            CssClass="salesorder-grid table table-condensed no-border table-hover sortable"
                            ShowHeaderWhenEmpty="true" 
                            AutoGenerateColumns="False" 
                            CellPadding="4" 
                            CellSpacing="0" 
                            OnRowDataBound="gvSalesOrderLines_RowDataBound" 
                            OnRowEditing="gvSalesOrderLines_RowEditing"
                            OnRowCancelingEdit="gvSalesOrderLines_RowCancelingEdit">
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true" runat="server" CssClass="round-checkbox" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Item Number">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlItemNumber" runat="server" CssClass="form-control form-control-sm filterable-dropdown" Style="width:100px !important;"  AutoPostBack="true"
    OnSelectedIndexChanged="ddlItemNumber_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Product Name">
                                    <ItemTemplate>
                                      <asp:Label ID="lblGridProductName" runat="server" Text='<%# Eval("itemName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                         <asp:TextBox ID="txtGridQty" runat="server" Text='<%# Bind("SalesQty") %>' 
            CssClass="form-control form-control-sm" Style="width:80px !important; text-align:right;">
        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unit">
                                    <ItemTemplate>
 <asp:DropDownList ID="ddlUnit" runat="server" 
            CssClass="form-control form-control-sm filterable-dropdown" Style="width:80px !important;">
        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delivery Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliveryType" runat="server" Text='<%# Eval("DeliveryType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Site">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlgridSite" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Warehouse">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlgridWarehouse" runat="server" CssClass="form-control form-control-sm filterable-dropdown" Style="width:100px !important;"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                
                                <asp:TemplateField HeaderText="Unit price">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGridUnitPrice" runat="server" Text='<%# Bind("SalesPrice") %>' 
            CssClass="form-control form-control-sm" Style="width:80px !important; text-align:right;">
        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Discount">
      <ItemTemplate>
 <asp:TextBox ID="txtGridDiscount" runat="server" Text='<%# Bind("SalesLineDisc") %>' 
            CssClass="form-control form-control-sm" Style="width:80px !important; text-align:right;">
        </asp:TextBox>      </ItemTemplate>
  </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Discount percentage">
    <ItemTemplate>
       <asp:TextBox ID="txtGridDiscPct" runat="server" Text='<%# Bind("linepercent") %>' 
            CssClass="form-control form-control-sm" Style="width:80px !important; text-align:right;">
        </asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>
             <asp:TemplateField HeaderText="Net amount">
    <ItemTemplate>
       <asp:TextBox ID="txtGridNetAmount" runat="server" Text='<%# Bind("LineAmount") %>' 
            CssClass="form-control form-control-sm" Style="width:80px !important; text-align:right;">
        </asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" Enabled="false"/>
                                        <asp:LinkButton ID="btnCancel" CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" CommandName="Cancel" Enabled="false"  />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="RecId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("recId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- ===================== LINE DETAILS PANEL ===================== -->
            <a href="#collapseLineDetails"
               class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
               data-toggle="collapse"
               role="button"
               aria-expanded="false"
               aria-controls="collapseLineDetails">
                <span class="section-title">Line Details</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>

            <div id="collapseLineDetails" class="collapse">
                <div class="card card-body">
                    <!-- Nested Tabs for Line Details -->
                    <ul class="nav nav-tabs" id="lineDetailsTabs" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" id="general-tab" data-toggle="tab" href="#general" role="tab" aria-controls="general" aria-selected="true">General</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="setup-tab" data-toggle="tab" href="#setup" role="tab" aria-controls="setup" aria-selected="false">Setup</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="address-tab" data-toggle="tab" href="#address" role="tab" aria-controls="address" aria-selected="false">Address</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="product-tab" data-toggle="tab" href="#product" role="tab" aria-controls="product" aria-selected="false">Product</a>
                        </li>
                       <%-- <li class="nav-item">
                            <a class="nav-link" id="packing-tab" data-toggle="tab" href="#packing" role="tab" aria-controls="packing" aria-selected="false">Packing</a>
                        </li>--%>
                        <li class="nav-item">
                            <a class="nav-link" id="delivery-tab" data-toggle="tab" href="#delivery" role="tab" aria-controls="delivery" aria-selected="false">Delivery</a>
                        </li>
                       <%-- <li class="nav-item">
                            <a class="nav-link" id="sourcing-tab" data-toggle="tab" href="#sourcing" role="tab" aria-controls="sourcing" aria-selected="false">Sourcing</a>
                        </li>--%>
                        <li class="nav-item">
                            <a class="nav-link" id="price-tab" data-toggle="tab" href="#price" role="tab" aria-controls="price" aria-selected="false">Price and discount</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="financialdimensions-tab" data-toggle="tab" href="#financialdimensions" role="tab">Financial dimensions</a>
                        </li>
                    </ul>

                    <div class="tab-content mt-3">
                        <!-- General Tab -->
                        <div class="tab-pane fade show active" id="general" role="tabpanel" aria-labelledby="general-tab">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="column-heading">Order Line</div>
                                    <div class="form-group">
                                        <label>Sales Category</label>
                                        <asp:TextBox ID="txtSalesCategory" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Product Name</label>
                                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Text</label>
                                        <asp:TextBox ID="txtText" runat="server" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading">External References</div>
                                    <div class="form-group">
                                        <label>External</label>
                                        <asp:TextBox ID="txtExternal" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label>Line Number</label>
                                        <asp:TextBox ID="txtLineNumber" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="column-heading mt-3">Intercompany</div>
                                    <div class="form-group">
                                        <label>Origin (Intercompany Orders)</label>
                                        <asp:TextBox ID="txtIntercompanyOrigin" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading">Status</div>
                                    <div class="form-group">
                                        <label>Line Status</label>
                                        <asp:TextBox ID="txtLineStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Stopped</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkStopped" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label>Prevent Partial Delivery</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkPreventPartialDelivery" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label>Quality Order Status</label>
                                        <asp:TextBox ID="txtQualityOrderStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Fulfillment Status</label>
                                        <asp:TextBox ID="txtFulfillmentStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Store Number</label>
                                        <asp:TextBox ID="txtStoreNumber" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="column-heading mt-3">Distributed Order Management</div>
                                    <div class="form-group">
                                        <label>Exclude from DOM Processing</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkExcludeFromDOM" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label>DOM Status</label>
                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Setup Tab -->
                        <div class="tab-pane fade" id="setup" role="tabpanel" aria-labelledby="setup-tab">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="column-heading">Inventory</div>
                                    <div class="form-group">
                                        <label>Lot ID</label>
                                        <asp:TextBox ID="txtLotID" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Reservation</label>
                                         <asp:DropDownList ID="ddlreservation1" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                                      <%--  <asp:TextBox ID="txtReservation" runat="server" CssClass="form-control" ReadOnly="true" />--%>
                                    </div>
                                    <div class="form-group">
                                        <label>Auto Batch Reservation</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkAutoBatchReservation" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label>Same Batch Selection</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkSameBatchSelection" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading">Returned Order</div>
                                    <div class="form-group">
                                        <label>Return Lot ID</label>
                                        <asp:TextBox ID="txtReturnLotID" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Return Cost Price</label>
                                        <asp:TextBox ID="txtReturnCostPrice" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Scrap</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkScrap" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading">Posting</div>
                                    <div class="form-group">
                                        <label>Main Account</label>
                                        <asp:TextBox ID="txtMainAccount" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="column-heading mt-3">Sales Tax</div>
                                    <div class="form-group">
                                        <label>Item Sales Tax Group</label>
                                         <asp:DropDownList ID="ddlitemsalestaxgroup" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                                        <%--<asp:TextBox ID="txtItemSalesTaxGroup" runat="server" CssClass="form-control" />--%>
                                    </div>
                                    <div class="form-group">
                                        <label>Sales Tax Group</label>
                                        <asp:DropDownList ID="ddlsalestaxgroup1" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                                       <%-- <asp:TextBox ID="txtSalesTaxGroup" runat="server" CssClass="form-control" />--%>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading">Commission</div>
                                    <div class="form-group">
                                        <label>Sales Group</label>
                                         <asp:DropDownList ID="ddlsalesgroup1" runat="server" CssClass="form-control filterable-dropdown" />
                                        <%--<asp:TextBox ID="txtSalesGroup" runat="server" CssClass="form-control" />--%>
                                    </div>
                                    <div class="column-heading mt-3">Date and Time</div>
                                    <div class="form-group">
                                        <label>Date and Time</label>
                                        <asp:TextBox ID="txtDateTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Address Tab -->
                        <div class="tab-pane fade" id="address" role="tabpanel" aria-labelledby="address-tab">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="column-heading">Delivery Address</div>
                                    <div class="form-group">
                                        <label>Name</label>
                                        <asp:TextBox ID="txtDeliveryName" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label>Delivery Address</label>
                                        <asp:TextBox ID="DeliveryAddressLine" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Address</label>
                                        <asp:TextBox ID="txtAddressLine" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Product Tab -->
                        <div class="tab-pane fade" id="product" role="tabpanel" aria-labelledby="product-tab">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="column-heading">Product Dimensions</div>
                                    <div class="form-group">
                                        <label>Configuration</label>
                                        <asp:TextBox ID="txtConfiguration" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Size</label>
                                        <asp:DropDownList ID="ddlSize" runat="server" CssClass="form-control filterable-dropdown" />
                                    </div>
                                    <div class="form-group">
                                        <label>Color</label>
                                        <asp:TextBox ID="txtColor" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Style</label>
                                        <asp:TextBox ID="txtStyle" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="column-heading mt-3">Tracking Dimensions</div>
                                    <div class="form-group">
                                        <label>Batch Number</label>
                                        <asp:TextBox ID="txtBatchNumber" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Serial Number</label>
                                        <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Owner</label>
                                        <asp:TextBox ID="txtOwner" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="column-heading mt-3">Storage Dimensions</div>
                                    <div class="form-group">
                                        <label>Site</label>
                                        <%--<asp:TextBox ID="txtSite" runat="server" CssClass="form-control" />--%>
                                        <asp:DropDownList ID="ddlsiteline" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Warehouse</label>
                                      <%--  <asp:TextBox ID="txtWarehouse" runat="server" CssClass="form-control" />--%>
                                        <asp:DropDownList ID="ddlwarehouseline" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Location</label>
                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Inventory Status</label>
                                        <asp:TextBox ID="txtInventoryStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>License Plate</label>
                                        <asp:TextBox ID="txtLicensePlate" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading mt-0">Product Attributes</div>
                                    <div class="form-group">
                                        <label>Installment Eligible</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkInstallmentEligible" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label>Schedule ID</label>
                                        <asp:TextBox ID="txtScheduleID" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="column-heading mt-3">Item Reference</div>
                                    <div class="form-group">
                                        <label>Reference Type</label>
                                        <asp:TextBox ID="txtReferenceType" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Reference Number</label>
                                        <asp:TextBox ID="txtReferenceNumber" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Reference Lot</label>
                                        <asp:TextBox ID="txtReferenceLot" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="column-heading mt-3">BOM/Route</div>
                                    <div class="form-group">
                                        <label>Sub-BOM</label>
                                        <asp:TextBox ID="txtSubBOM" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label>Subroute</label>
                                        <asp:TextBox ID="txtSubroute" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Packing Tab -->
                       <%-- <div class="tab-pane fade" id="packing" role="tabpanel" aria-labelledby="packing-tab">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-3">
                                        <h6 class="section-title">BAR CODE</h6>
                                        <div class="form-group">
                                            <label>Bar code</label>
                                            <asp:TextBox ID="txtBarCode" runat="server" CssClass="form-control custom-input" ReadOnly="true" />
                                        </div>
                                        <div class="form-group">
                                            <label>Bar code setup</label>
                                            <asp:TextBox ID="txtBarCodeSetup" runat="server" CssClass="form-control custom-input" ReadOnly="true" />
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <h6 class="section-title">PACKING MATERIAL</h6>
                                        <div class="form-group">
                                            <label>Packing unit</label>
                                            <asp:DropDownList ID="ddlPackingUnit" runat="server" CssClass="form-control custom-input" />
                                        </div>
                                        <div class="form-group">
                                            <label>Packing unit quantity</label>
                                            <asp:TextBox ID="txtPackingQty" runat="server" CssClass="form-control custom-input" />
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <h6 class="section-title">GIFT CARD</h6>
                                        <div class="form-group">
                                            <label>Gift card type</label>
                                            <asp:DropDownList ID="ddlGiftCardType" runat="server" CssClass="form-control custom-input" />
                                        </div>
                                        <div class="form-group">
                                            <label>Buyer name</label>
                                            <asp:TextBox ID="txtBuyerName" runat="server" CssClass="form-control custom-input" />
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Buyer email</label>
                                            <asp:TextBox ID="txtBuyerEmail" runat="server" CssClass="form-control custom-input" />
                                        </div>
                                        <div class="form-group">
                                            <label>Recipient name</label>
                                            <asp:TextBox ID="txtRecipientName" runat="server" CssClass="form-control custom-input" />
                                        </div>
                                        <div class="form-group">
                                            <label>Recipient email</label>
                                            <asp:TextBox ID="txtRecipientEmail" runat="server" CssClass="form-control custom-input" />
                                        </div>
                                        <div class="form-group">
                                            <label>Gift message</label>
                                            <asp:TextBox ID="txtGiftMessage" runat="server" CssClass="form-control custom-input" />
                                        </div>
                                        <div class="form-group">
                                            <label>Gift card number</label>
                                            <asp:TextBox ID="txtGiftCardNumber" runat="server" CssClass="form-control custom-input readonly-field" ReadOnly="true" />
                                        </div>
                                        <div class="form-group">
                                            <label>Expiration</label>
                                            <asp:TextBox ID="txtExpiration" runat="server" CssClass="form-control custom-input readonly-field" ReadOnly="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>--%>

                        <!-- Delivery Tab -->
                        <div class="tab-pane fade" id="delivery" role="tabpanel" aria-labelledby="delivery-tab">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="column-heading">Delivery Date</div>
                                    <div class="form-group">
                                        <label>Requested Ship Date</label>
                                        <asp:TextBox ID="TxtRequestedShipDateline" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                    <div class="form-group">
                                        <label>Requested Receipt Date</label>
                                        <asp:TextBox ID="TxtRequestReceiptDateline" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                    <div class="form-group">
                                        <label>Confirmed Ship Date</label>
                                        <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                    <div class="form-group">
                                        <label>Confirmed Receipt Date</label>
                                        <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Delivery Date Control</label>
                                        <asp:TextBox ID="txtDeliveryDateControl" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label>Batch CTP Status</label>
                                        <asp:TextBox ID="txtBatchCTPStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Planning Priority</label>
                                        <asp:TextBox ID="txtPlanningPriority" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="column-heading mt-3">Misc. Delivery Info</div>
                                    <div class="form-group">
                                        <label>Delivery Terms</label>
                                       <%-- <asp:TextBox ID="txtDeliveryTerms" runat="server" CssClass="form-control" />--%>
                                         <asp:DropDownList ID="ddlterms" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Overdelivery</label>
                                        <asp:TextBox ID="txtOverdelivery" runat="server" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Underdelivery</label>
                                        <asp:TextBox ID="txtUnderdelivery" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label>Mode of Delivery</label>
                                       <%-- <asp:TextBox ID="txtModeOfDelivery" runat="server" CssClass="form-control" />--%>
                                         <asp:DropDownList ID="ddlmode" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Delivery Type</label>
                                        <asp:TextBox ID="txtDeliveryType" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="form-group mt-2">
                                        <label>Direct Delivery</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkDirectDelivery" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label>Direct Delivery Status</label>
                                        <asp:TextBox ID="txtDirectDeliveryStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Comments</label>
                                        <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading">Carrier Information</div>
                                    <div class="form-group">
                                        <label>Shipping Carrier</label>
                                        <asp:TextBox ID="txtShippingCarrier" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Carrier Service</label>
                                        <asp:TextBox ID="txtCarrierService" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="column-heading mt-3">Shipping Location Time Zone</div>
                                    <div class="form-group">
                                        <label>Time Zone</label>
                                        <asp:TextBox ID="txtTimeZone" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="column-heading mt-3">Pickup Time Slot</div>
                                    <div class="form-group">
                                        <label>Pickup Time Range</label>
                                        <asp:TextBox ID="txtPickupTimeRange" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Sourcing Tab -->
              <%--          <div class="tab-pane fade" id="sourcing" role="tabpanel" aria-labelledby="sourcing-tab">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label>Delivery type</label>
                                            <asp:DropDownList ID="ddlDeliveryType" runat="server" CssClass="form-control custom-input">
                                                <asp:ListItem Text="Stock" Value="Stock" />
                                                <asp:ListItem Text="Direct" Value="Direct" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label>Sourcing origin</label>
                                            <asp:TextBox ID="txtSourcingOrigin" runat="server" CssClass="form-control custom-input" Text="Inventory" ReadOnly="true" />
                                        </div>
                                    </div>

                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label>Sourcing vendor</label>
                                            <asp:DropDownList ID="ddlSourcingVendor" runat="server" CssClass="form-control custom-input"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <label>Sourcing company</label>
                                            <asp:TextBox ID="txtSourcingCompany" runat="server" CssClass="form-control custom-input" />
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Sourcing site</label>
                                                    <asp:TextBox ID="txtSourcingSite" runat="server" CssClass="form-control custom-input" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Sourcing warehouse</label>
                                                    <asp:TextBox ID="txtSourcingWarehouse" runat="server" CssClass="form-control custom-input" />
                                                </div>
                                            </div>
                                        </div>
                                        <h6 class="section-title mt-3">MASTER PLANNING</h6>
                                        <div class="form-group d-flex align-items-center">
                                            <label class="mr-3 mb-0">Exclude from master planning</label>
                                            <label class="switch mb-0">
                                                <asp:CheckBox ID="chkExcludeMasterPlanning" runat="server" />
                                                <span class="slider round"></span>
                                            </label>
                                            <span class="ml-2">No</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>--%>

                        <!-- Price Tab -->
                        <div class="tab-pane fade" id="price" role="tabpanel" aria-labelledby="price-tab">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="column-heading">Discount</div>
                                    <div class="form-group">
                                        <label>Discount</label>
                                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Discount Percent</label>
                                        <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading">Discount</div>
                                    <div class="form-group">
                                        <label>Multiline Discount</label>
                                        <asp:TextBox ID="txtMultilineDiscount" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label>Multiline Discount Percentage</label>
                                        <asp:TextBox ID="txtMultilineDiscountPercent" runat="server" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading">Prices</div>
                                    <div class="form-group">
                                        <label>Price Unit</label>
                                        <asp:TextBox ID="txtPriceUnit" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Sales Charges</label>
                                        <asp:TextBox ID="txtSalesCharges" runat="server" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="column-heading">Rebates</div>
                                    <div class="form-group">
                                        <label>Exclude from Rebate</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkExcludeRebate" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label>Exclude from Rebate Management</label><br />
                                        <label class="switch">
                                            <asp:CheckBox ID="chkExcludeRebateMgmt" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                    <div class="column-heading mt-3">Attribute-based Pricing Details</div>
                                    <div class="form-group">
                                        <label>Attribute-based Pricing ID</label>
                                        <asp:TextBox ID="txtAttributePricingID" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Adjusted Unit Price</label>
                                        <asp:TextBox ID="txtAdjustedUnitPrice" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="form-group">
                                        <label>Adjusted Net Amount</label>
                                        <asp:TextBox ID="txtAdjustedNetAmount" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Financial Dimensions Tab -->
                        <div class="tab-pane fade" id="financialdimensions" role="tabpanel" aria-labelledby="financialdimensions-tab">
                            <div class="d-flex justify-content-between flex-wrap">
                                <div class="flex-grow-1" style="min-width: 300px;">
                                    <div class="custom-section-title">FINANCIAL DIMENSIONS</div>
                                    <div runat="server" id="financialdimesnsionlines"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- HEADER TAB -->
        <div class="tab-pane fade" id="header">
            <!-- GENERAL PANEL -->
            <a href="#collapseGeneral"
                class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
                data-toggle="collapse"
                role="button"
                aria-expanded="false"
                aria-controls="collapseGeneral">
                <span class="section-title">General</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>
            <div id="collapseGeneral" class="collapse">
                <div class="section-content">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="column-heading">Sales Order</div>
                            <div class="form-group">
                                <label>Sales Order</label>
                                <asp:TextBox ID="txtSalesOrder" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Source</label>
                                <asp:TextBox ID="txtSource" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Retail Sale</label><br />
                                <label class="switch">
                                    <asp:CheckBox ID="chkRetailSale" runat="server" />
                                    <span class="slider"></span>
                                </label>
                            </div>
                            <div class="form-group">
                                <label>Customer Name</label>
                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Order Type</label>
                                <asp:TextBox ID="txtOrderType" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Continuity Order</label><br />
                                <label class="switch">
                                    <asp:CheckBox ID="chkContinuityOrder" runat="server" />
                                    <span class="slider"></span>
                                </label>
                            </div>
                            <div class="column-heading">Customer</div>
                            <div class="form-group">
                                <label>Customer Account</label>
                                <asp:DropDownList ID="ddlcustomerAccount" runat="server" CssClass="form-control filterable-dropdown" ReadOnly="true" />
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                <label>One-Time Customer</label><br />
                                <label class="switch">
                                    <asp:CheckBox ID="chkOneTimeCustomer" runat="server" />
                                    <span class="slider"></span>
                                </label>
                            </div>
                            <div class="form-group">
                                <label>Invoice Account</label>
                                <asp:DropDownList ID="ddlInvoiceAccount" runat="server" CssClass="form-control filterable-dropdown" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Contact</label>
                                <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" />
                            </div>
                            <div class="column-heading mt-3">Contact Information</div>
                            <div class="form-group">
                                <label>Internet Address</label>
                                <asp:TextBox ID="txtInternetAddress" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Telephone</label>
                                <asp:TextBox ID="txtTelephone" runat="server" CssClass="form-control" />
                            </div>
                            <div class="column-heading">Status</div>
                            <div class="form-group">
                                <label>Status</label>
                                <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                <label>Deadline</label>
                                <asp:TextBox ID="txtDeadline" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Document Status</label>
                                <asp:TextBox ID="txtDocumentStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Quality Order Status</label>
                                <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Do Not Process</label><br />
                                <label class="switch">
                                    <asp:CheckBox ID="chkDoNotProcess" runat="server" />
                                    <span class="slider"></span>
                                </label>
                            </div>
                            <div class="column-heading mt-3">Storage Dimension</div>
                            <div class="form-group">
                                <label>Site</label>
                                <asp:DropDownList ID="ddlSite" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Warehouse</label>
                                <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Campaign ID</label>
                                <asp:DropDownList ID="ddlCampaignID" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="column-heading">References</div>
                            <div class="form-group">
                                <label>Customer Requisition</label>
                                <asp:TextBox ID="TextBox8" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Customer Reference</label>
                                <asp:TextBox ID="TextBox9" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Project ID</label>
                                <asp:TextBox ID="txtProjectID" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>RMA Number</label>
                                <asp:TextBox ID="txtRMANumber" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Reason Code</label>
                                <asp:DropDownList ID="ddlReasonCode" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Call List ID</label>
                                <asp:DropDownList ID="ddlCallListID" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Reason Comment</label>
                                <asp:TextBox ID="txtReasonComment" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Exclude from DOM Processing</label><br />
                                <label class="switch">
                                    <asp:CheckBox ID="chkExcludeDOM" runat="server" />
                                    <span class="slider"></span>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- SETUP PANEL -->
            <a href="#collapseSetup"
                class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
                data-toggle="collapse"
                role="button"
                aria-expanded="false"
                aria-controls="collapseSetup">
                <span class="section-title">Setup</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>
            <div id="collapseSetup" class="collapse">
                <div class="section-content">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="column-heading">Sales Tax</div>
                            <div class="form-group">
                                <label>Sales Tax Group</label>
                                <asp:DropDownList ID="ddlSalesTaxGroup" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Tax Exempt Number</label>
                                <asp:TextBox ID="txtTaxExemptNumber" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Prices Include Sales Tax</label><br />
                                <label class="switch">
                                    <asp:CheckBox ID="chkPricesIncludeTax" runat="server" />
                                    <span class="slider"></span>
                                </label>
                            </div>
                            <div class="column-heading mt-3">Posting</div>
                            <div class="form-group">
                                <label>Number Sequence Group</label>
                                <asp:DropDownList ID="ddlNumberSequenceGroup" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                <label>Settlement Type</label>
                                <asp:DropDownList ID="ddlSettlementType" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Posting Profile</label>
                                <asp:DropDownList ID="ddlPostingProfile" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="column-heading mt-3">Commission</div>
                            <div class="form-group">
                                <label>Sales Group</label>
                                <asp:DropDownList ID="ddlSalesGroup" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Commission Group</label>
                                <asp:DropDownList ID="ddlCommissionGroup" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="column-heading">Inventory</div>
                            <div class="form-group">
                                <label>Reservation</label>
                                <asp:DropDownList ID="ddlReservation" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Auto Batch Reservation</label><br />
                                <label class="switch">
                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                    <span class="slider"></span>
                                </label>
                            </div>
                            <div class="form-group">
                                <label>Sales Order Priority for Fulfillment</label>
                                <asp:DropDownList ID="ddlSalesOrderPriority" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="column-heading mt-3">Administration</div>
                            <div class="form-group">
                                <label>Sales Taker</label>
                                <asp:DropDownList ID="ddlSalesTaker" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                <label>Pool</label>
                                <asp:DropDownList ID="ddlPool" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Language</label>
                                <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Sales Unit</label>
                                <asp:DropDownList ID="ddlSalesUnit" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Sales Responsible</label>
                                <asp:DropDownList ID="ddlSalesResponsible" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Sales Origin</label>
                                <asp:DropDownList ID="ddlSalesOrigin" runat="server" CssClass="form-control filterable-dropdown"></asp:DropDownList>
                            </div>
                            <div class="column-heading mt-3">Date and Time</div>
                            <div class="form-group">
                                <label>Created Date and Time</label>
                                <asp:TextBox ID="txtCreatedDateTime" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- ADDRESS PANEL -->
            <a href="#collapseAddress"
               class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
               data-toggle="collapse"
               role="button"
               aria-expanded="false"
               aria-controls="collapseAddress">
                <span class="section-title">Address</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>
            <div id="collapseAddress" class="collapse">
                <div class="section-content">
                    <div class="row">
                        <div class="col-md-6">
                            <h5 class="mt-3">Delivery Address</h5>
                            <div class="form-group">
                                <label>Name</label>
                                <asp:TextBox ID="Txtnameheader" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group mt-4">
                                <label>Delivery Address</label>
                                 <asp:TextBox ID="Txtdeliveryaddressheader" runat="server" CssClass="form-control" />
                              <%--  <asp:DropDownList ID="ddlDeliveryAddress" runat="server" CssClass="form-control">--%>
                                  <%--  <asp:ListItem Text="Select Delivery Address" Value="" />--%>
                                <%--</asp:DropDownList>--%>
                            </div>
                            <div class="form-group">
                                <label>Address</label>
                                <asp:TextBox ID="TxtAddressHeader" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

<!-- DELIVERY PANEL -->
<a href="#collapseDelivery"
   class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
   data-toggle="collapse"
   role="button"
   aria-expanded="false"
   aria-controls="collapseDelivery">
    <span class="section-title">Delivery</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</a>

<div id="collapseDelivery" class="collapse">
<div class="section-content">

<div class="row">

<!-- DELIVERY DATE -->
<div class="col-md-4">
<h5>DELIVERY DATE</h5>

<div class="form-group">
<label>Requested ship date</label>
<asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
</div>

<div class="form-group">
<label>Requested receipt date</label>
<asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
</div>

<div class="form-group">
<label>Confirmed ship date</label>
<asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
</div>

<div class="form-group">
<label>Confirmed receipt date</label>
<asp:TextBox ID="TextBox12" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
</div>

</div>


<!-- DETAILS -->
<div class="col-md-4">

<h5>DETAILS</h5>

<div class="form-group">
<label>Ship complete</label><br>

<label class="switch">
<asp:CheckBox ID="chkShipComplete" runat="server"/>
<span class="slider"></span>
</label>

</div>

<div class="form-group">
<label>Automatic notification</label><br>

<label class="switch">
<asp:CheckBox ID="chkAutoNotification" runat="server"/>
<span class="slider"></span>
</label>

</div>

<div class="form-group">
<label>Blind shipment</label><br>

<label class="switch">
<asp:CheckBox ID="chkBlindShipment" runat="server"/>
<span class="slider"></span>
</label>

</div>

<div class="form-group">
<label>Mode of delivery</label>
<%--<asp:TextBox ID="txtModeOfDelivery" runat="server" CssClass="form-control"></asp:TextBox>--%>

     <asp:DropDownList ID="ddlmodeofdeliveryheader" runat="server" CssClass="form-control custom-input"/>
</div>

</div>


<!-- TRANSPORTATION -->
<div class="col-md-4">

<h5>TRANSPORTATION</h5>

<div class="form-group">
<label>Shipping carrier</label>
<asp:TextBox ID="TextBox13" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="form-group">
<label>Carrier service</label>
<asp:TextBox ID="TextBox14" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="form-group">
<label>Delivery terms</label>
<%--<asp:TextBox ID="txtDeliveryTerms" runat="server" CssClass="form-control"></asp:TextBox>--%>
    <asp:DropDownList ID="ddldeliverytermheader" runat="server" CssClass="form-control custom-input"/>
</div>

</div>

</div>
</div>
</div>

            <!-- PRICE AND DISCOUNT PANEL -->
            <a href="#collapsePrice"
               class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
               data-toggle="collapse"
               role="button"
               aria-expanded="false"
               aria-controls="collapsePrice">
                <span class="section-title">Price and Discount</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>
            <div id="collapsePrice" class="collapse">
                <div class="section-content">
                    <div class="row d-flex flex-wrap">
                        <div class="five-col">
                            <h6 class="font-weight-bold">CURRENCY</h6>
                            <div class="form-group">
                                <label>Currency</label>
                                <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control filterable-dropdown" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Fixed exchange rate</label>
                                <asp:TextBox ID="txtFixedExchangeRate" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Reporting currency fixed exchange</label>
                                <asp:TextBox ID="txtReportingExchange" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <h6 class="font-weight-bold mt-3">PAYMENT</h6>
                            <div class="form-group">
                                <label>Payment</label>
                                <asp:DropDownList ID="ddlPayment" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                        </div>

                        <div class="five-col">
                            <div class="form-group">
                                <label>Due date</label>
                                <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Method of payment</label>
                                <asp:DropDownList ID="ddlMethodOfPayment" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="form-group">
                                <label>Payment specification</label>
                                <asp:DropDownList ID="ddlPaymentSpec" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="form-group">
                                <label>Payment schedule</label>
                                <asp:DropDownList ID="ddlPaymentSchedule" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="form-group">
                                <label>Credit card number</label>
                                <asp:TextBox ID="txtCreditCard" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="five-col">
                            <div class="form-group">
                                <label>Cash discount</label>
                                <asp:DropDownList ID="ddlCashDisc" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="form-group">
                                <label>Discount percentage</label>
                                <asp:TextBox ID="txtDiscountPercHeader" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Payment terms base date</label>
                                <asp:TextBox ID="Txtpaymentdate" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Direct debit mandate ID</label>
                                <asp:TextBox ID="TxtDirectdebit" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <h6 class="font-weight-bold mt-3">DISCOUNT OR CHARGES</h6>
                            <div class="form-group">
                                <label>Price group</label>
                                <asp:DropDownList ID="ddlPriceGroup" runat="server" CssClass="form-control filterable-dropdown" ReadOnly="true" />
                            </div>
                        </div>

                        <div class="five-col">
                            <div class="form-group">
                                <label>Line discount group</label>
                                <asp:DropDownList ID="ddlLineDiscountGroup" runat="server" CssClass="form-control filterable-dropdown" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Multiline disc. group</label>
                                <asp:DropDownList ID="ddlMultilineDiscGroup" runat="server" CssClass="form-control filterable-dropdown" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Total discount group</label>
                                <asp:DropDownList ID="ddlTotalDiscountGroup" runat="server" CssClass="form-control filterable-dropdown" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Charges group</label>
                                <asp:DropDownList ID="ddlChargesGroup" runat="server" CssClass="form-control filterable-dropdown" ReadOnly="true" />
                            </div>
                        </div>

                        <div class="five-col">
                            <div class="form-group">
                                <label>Customer rebate group</label>
                                <asp:DropDownList ID="ddlCustomerRebateGroup" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="form-group">
                                <label>Customer TMA group</label>
                                <asp:DropDownList ID="ddlCustomerTMAGroup" runat="server" CssClass="form-control filterable-dropdown" />
                            </div>
                            <div class="form-group">
                                <label>Rebate reference</label>
                                <asp:TextBox ID="txtRebateReference" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Total discount %</label>
                                <asp:TextBox ID="txtTotalDiscountPercent" runat="server" CssClass="form-control" Text="0.00" ReadOnly="true" />
                            </div>
                            <div class="form-group">
                                <label>Total discount override</label><br />
                                <label class="switch">
                                    <asp:CheckBox ID="chkTotalDiscountOverride" runat="server" />
                                    <span class="slider"></span>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- FINANCIAL DIMENSIONS PANEL -->
            <a href="#collapseFinancialNew"
               class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
               data-toggle="collapse"
               role="button"
               aria-expanded="false"
               aria-controls="collapseFinancialNew">
                <span class="section-title">Financial Dimensions</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>
            <div id="collapseFinancialNew" class="collapse">
                <div class="section-content">
                    <div runat="server" id="financialDimensionsContainer"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>