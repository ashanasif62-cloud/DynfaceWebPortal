<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder_Invoices.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_Invoices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .card-header { cursor: pointer; }
        .rotate-icon { transition: transform 0.3s; }
        .rotate-icon.rotate { transform: rotate(180deg); }
        .tab-content { margin-top: 20px; }
        .form-control { margin-bottom: 5px; }
        .switch {
  position: relative;
  display: inline-block;
  width: 45px;
  height: 22px;
}

.switch input {
  display: none;
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
  border-radius: 22px;
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
  background-color: #28a745;
}

input:checked + .slider:before {
  transform: translateX(22px);
}
.small-textbox {
        width: 200px !important;   /* Adjust size as needed */
       
    }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    
    <ul class="nav nav-tabs" id="purchaseOrderTabs" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" id="lines-tab" data-toggle="tab" href="#linesTab" role="tab" aria-controls="linesTab" aria-selected="true">Lines</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="header-tab" data-toggle="tab" href="#headerTab" role="tab" aria-controls="headerTab" aria-selected="false">Header</a>
        </li>
    </ul>

    <div class="tab-content" id="purchaseOrderTabContent">

        <!-- LINES TAB CONTENT -->
        <div class="tab-pane fade show active" id="linesTab" role="tabpanel" aria-labelledby="lines-tab">
            <div class="container-fluid">
                
              <div class="card mb-3">
    <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center">
        <div data-toggle="collapse" data-target="#collapseInvoiceHeader" aria-expanded="true" style="flex-grow: 1; cursor: pointer;">
            <i class="fa fa-chevron-down rotate-icon"></i>
            <strong>Vendor Invoice Header</strong>
        </div>
        <div class="header-actions">
            <asp:LinkButton ID="btnPostInvoice" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnPostInvoice_Click">
                <i class="fa fa-paper-plane"></i> Post
            </asp:LinkButton>
        </div>
    </div>

    <div id="collapseInvoiceHeader" class="collapse show">
        <div class="card-body">
            <div class="row">

                <!-- COL 1: Vendor + Invoice Identification -->
                <div class="col-md-2">
                  
                    <label style="color:black;"><b>VENDOR</b></label>
   
                    <div class="form-group">
                        <label class="small">Company</label>
                        <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control form-control-sm" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="small">Invoice account</label>
                        <asp:TextBox ID="txtInvoiceAccount" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <asp:TextBox ID="txtVendorName" runat="server" CssClass="form-control form-control-sm" ReadOnly="true"></asp:TextBox>

                  
                      <label style="color:black;"><b>INVOICE IDENTIFICATION</b></label>
                    <div class="form-group">

                        <label class="small">Number</label>
                        <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>

                <!-- COL 2: Invoice Description + Related Documents -->
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="small">Invoice description</label>
                        <asp:TextBox ID="txtInvoiceDescription" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

               
                                 
                      <label style="color:black;"><b>RELATED DOCUMENTS</b></label>
                    <div class="form-group">
                        <label class="small">Purchase order</label>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="txtPurchaseOrder" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            <div class="input-group-append">
                                <button class="btn btn-outline-secondary btn-sm" type="button">+</button>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="small">Product receipt</label>
                        <asp:TextBox ID="txtProductReceipt" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="small">Purchase agreement</label>
                        <div class="row no-gutters">
                            <div class="col-6 pr-1">
                                <asp:TextBox ID="txtPurchaseAgreement1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <asp:TextBox ID="txtPurchaseAgreement2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- COL 3: Invoice Dates -->
                <div class="col-md-2">
                    
                                                       
                    <label style="color:black;"><b>INVOICE DATES</b></label>
                    <div class="form-group">
                        <label class="small">Invoice received date</label>
                        <asp:TextBox ID="txtInvoiceReceivedDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="small">Invoice date</label>
                        <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="small">Date of vendor VAT register</label>
                        <asp:TextBox ID="txtVATRegisterDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                    </div>
                </div>

                <!-- COL 4: Posting/Due Dates + Invoice Status Details -->
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="small">Posting date</label>
                        <asp:TextBox ID="txtPostingDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="small">Due date</label>
                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                    </div>

                   
                                                                           
                     <label style="color:black;"><b>INVOICE STATUS DETAILS</b></label>
                    <div class="form-group d-flex align-items-center">
                        <label class="small mr-2 mb-0">On hold</label>
                        <label class="switch mb-0">
                            <input type="checkbox" id="chkOnHold" runat="server">
                            <span class="slider round"></span>
                        </label>
                        <span class="small ml-2">No</span>
                    </div>
                    <div class="form-group">
                        <label class="small">Match status</label>
                        <div class="border-bottom pb-1 small">Not performed</div>
                    </div>
                </div>

                <!-- COL 5: Budget Check + Allow Toggle + Sales Tax -->
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="small">Header budget check results</label>
                        <asp:TextBox ID="txtBudgetCheckResults" runat="server" CssClass="form-control form-control-sm" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="small">Allow matched invoice line quant...</label>
                        <div class="d-flex align-items-center">
                            <label class="switch mb-0">
                                <input type="checkbox" id="chkAllowMatchedQty" runat="server">
                                <span class="slider round"></span>
                            </label>
                            <span class="small ml-2">No</span>
                        </div>
                    </div>

                   
                      <label style="color:black;"><b>SALES TAX</b></label>
                    <div class="form-group">
                        <label class="small">Accrue sales tax type</label>
                        <asp:DropDownList ID="ddlAccrueSalesTaxType" runat="server" CssClass="form-control form-control-sm">
                            <asp:ListItem Text="Default" Value="Default"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="small">Vendor charged sales tax</label>
                        <asp:TextBox ID="txtVendorChargedSalesTax" runat="server" CssClass="form-control form-control-sm" Text="0.00"></asp:TextBox>
                    </div>
                </div>

            </div>
        </div>
    </div>
</div>
                

                <!-- Vendor Invoice Line -->
             <!-- Vendor Invoice Line -->
<div class="card mb-3">
    <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center" 
         data-toggle="collapse" data-target="#collapseVendorInvoiceLine" aria-expanded="true">
        <strong>Vendor Invoice Line</strong>
        <span><i class="fa fa-chevron-down rotate-icon"></i></span>
    </div>

    <div id="collapseVendorInvoiceLine" class="collapse show">
        <div class="card-body pt-2 pb-2">

            <%-- Toolbar --%>
            <div class="text-left d-flex align-items-center mb-2" style="gap:16px;">
                <asp:LinkButton ID="btnAddInvoiceLine" runat="server"
                    CssClass="text-primary"
                    OnClick="btnAddInvoiceLine_Click"
                    Style="font-size: 0.9rem;">
                    <i class="mdi mdi-plus" style="font-size: 0.9rem;"></i> Add line
                </asp:LinkButton>

                <asp:LinkButton ID="btnDeleteInvoiceLine" runat="server"
                    CssClass="text-primary"
                    OnClick="btnDeleteInvoiceLine_Click"
                    Style="font-size: 0.9rem;">
                    <i class="mdi mdi-delete" style="font-size: 0.9rem;"></i> Remove
                </asp:LinkButton>
            </div>

            <%-- Grid --%>
            <div class="table-responsive" style="overflow-x:auto; white-space:nowrap;">
                <asp:GridView ID="gvVendorInvoiceLine" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-condensed no-border table-hover sortable"
                    ShowHeaderWhenEmpty="true"
                    DataKeyNames="RecId"
                    EmptyDataText="No Record Found."
                    OnRowEditing="gvVendorInvoiceLine_RowEditing"
                    OnRowDataBound="gvVendorInvoiceLine_RowDataBound">

                    <Columns>

                        <%-- Checkbox --%>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkInvoiceSelectAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle" runat="server"
                                    CssClass="round-checkbox"
                                    AutoPostBack="true"
                                    OnCheckedChanged="chkInvoiceLine_SelectSingle_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Hidden RecId --%>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Budget Check Results --%>
                        <asp:TemplateField HeaderText="Budget Check Results" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetCheckResult" runat="server"
                                    Text='<%# Bind("BudgetCheckResult") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Item Number --%>
                        <asp:TemplateField HeaderText="Item Number">
                            <ItemTemplate>
                                <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("ItemId") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtItemId" runat="server"
                                    Text='<%# Bind("ItemId") %>'
                                    CssClass="form-control form-control-sm" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Item Name --%>
                        <asp:TemplateField HeaderText="Item Name">
                            <ItemTemplate>
                                <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtItemName" runat="server"
                                    Text='<%# Bind("ItemName") %>'
                                    CssClass="form-control form-control-sm" Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Procurement Category --%>
                        <asp:TemplateField HeaderText="Procurement Category">
                            <ItemTemplate>
                                <asp:Label ID="lblProcurementCategory" runat="server"
                                    Text='<%# Bind("ProcurementCategory") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtProcurementCategory" runat="server"
                                    Text='<%# Bind("ProcurementCategory") %>'
                                    CssClass="form-control form-control-sm" Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Quantity --%>
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="lblReceiveNow" runat="server"
                                    Text='<%# Bind("ReceiveNow", "{0:N2}") %>'
                                    Style="text-align:right; display:block;" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtReceiveNow" runat="server"
                                    Text='<%# Bind("ReceiveNow") %>'
                                    CssClass="form-control form-control-sm" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Unit --%>
                        <asp:TemplateField HeaderText="Unit">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchUnit" runat="server" Text='<%# Bind("purchUnit") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPurchUnit" runat="server"
                                    Text='<%# Bind("purchUnit") %>'
                                    CssClass="form-control form-control-sm" Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Unit Price --%>
                        <asp:TemplateField HeaderText="Unit Price">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchPrice" runat="server"
                                    Text='<%# Bind("PurchPrice", "{0:N2}") %>'
                                    Style="text-align:right; display:block;" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPurchPrice" runat="server"
                                    Text='<%# Bind("PurchPrice") %>'
                                    CssClass="form-control form-control-sm" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Line Net Amount --%>
                        <asp:TemplateField HeaderText="Line Net Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblLineAmount" runat="server"
                                    Text='<%# Bind("LineAmount", "{0:N2}") %>'
                                    Style="text-align:right; display:block;" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLineAmount" runat="server"
                                    Text='<%# Bind("LineAmount") %>'
                                    CssClass="form-control form-control-sm" Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Site --%>
                        <asp:TemplateField HeaderText="Site">
                            <ItemTemplate>
                                <asp:Label ID="lblInventSiteId" runat="server"
                                    Text='<%# Bind("InventSiteId") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtInventSiteId" runat="server"
                                    Text='<%# Bind("InventSiteId") %>'
                                    CssClass="form-control form-control-sm" Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Warehouse --%>
                        <asp:TemplateField HeaderText="Warehouse">
                            <ItemTemplate>
                                <asp:Label ID="lblInventLocationId" runat="server"
                                    Text='<%# Bind("InventLocationId") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtInventLocationId" runat="server"
                                    Text='<%# Bind("InventLocationId") %>'
                                    CssClass="form-control form-control-sm" Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Product Receipt --%>
                        <asp:TemplateField HeaderText="Product Receipt">
                            <ItemTemplate>
                                <asp:Label ID="lblPackingSlipId" runat="server"
                                    Text='<%# Bind("PackingSlipId") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPackingSlipId" runat="server"
                                    Text='<%# Bind("PackingSlipId") %>'
                                    CssClass="form-control form-control-sm" Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Check If Quantity --%>
                        <asp:TemplateField HeaderText="Check Quantity" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCheckIfQuantity" runat="server"
                                    Text='<%# Bind("checkIfQuantity") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Price Variance Status --%>
                        <asp:TemplateField HeaderText="Price Variance Status" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPriceVarianceStatus" runat="server"
                                    Text='<%# Bind("priceVarianceStatus") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Purchase Order --%>
                        <asp:TemplateField HeaderText="Purchase Order">
                            <ItemTemplate>
                                <asp:Label ID="lblOrigPurchId" runat="server"
                                    Text='<%# Bind("OrigPurchId") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOrigPurchId" runat="server"
                                    Text='<%# Bind("OrigPurchId") %>'
                                    CssClass="form-control form-control-sm" Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Update / Cancel buttons in edit mode --%>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>
                                <asp:LinkButton
                                    CssClass="grid-img-btn btn-attachment"
                                    ToolTip="Attachment" Text="Attachment"
                                    runat="server"
                                    OnClick="btnInvoiceLineAttachment_Click" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton
                                    CssClass="grid-img-btn btn-update"
                                    ToolTip="Update" Text="Update"
                                    runat="server"
                                    OnClick="gvVendorInvoiceLine_Update_Click" />
                                <asp:LinkButton
                                    CssClass="grid-img-btn btn-cancel"
                                    ToolTip="Cancel" Text="Cancel"
                                    runat="server"
                                    OnClick="gvVendorInvoiceLine_Cancel_Click" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Edit button --%>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CssClass="grid-img-btn btn-edit"
                                    ToolTip="Edit" Text="Edit"
                                    CommandName="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
            <%-- Save bar — hidden until Add line is clicked --%>
<%--<asp:Panel ID="pnlSaveBar" runat="server" Visible="false"
    CssClass="d-flex justify-content-end align-items-center mt-2"
    Style="gap: 8px; border-top: 1px solid #dee2e6; padding-top: 8px;">
    <asp:LinkButton ID="btnCancelAll" runat="server"
        CssClass="btn btn-sm btn-outline-secondary"
        OnClick="btnCancelAll_Click">
        <i class="mdi mdi-close"></i> Cancel
    </asp:LinkButton>
    <asp:LinkButton ID="btnSaveAll" runat="server"
        CssClass="btn btn-sm btn-primary"
        OnClick="btnSaveAll_Click">
        <i class="mdi mdi-content-save"></i> Save
    </asp:LinkButton>
</asp:Panel>--%>

        </div>
    </div>
</div>

                <!-- Line Detail -->
                <div class="card">
                    <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center" data-toggle="collapse" data-target="#collapseLineDetail" aria-expanded="false">
                        <strong>Line Detail</strong>
                        <span><i class="fa fa-chevron-down rotate-icon"></i></span>
                    </div>
                    <div id="collapseLineDetail" class="collapse">
                        <div class="card-body">
                            <ul class="nav nav-tabs" id="lineDetailTabs" role="tablist">
                                <li class="nav-item">
                                    <a class="nav-link active" id="lineDetail-tab" data-toggle="tab" href="#lineDetailContent" role="tab" aria-controls="lineDetailContent" aria-selected="true">Line Detail</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="setup-tab" data-toggle="tab" href="#setupContent" role="tab" aria-controls="setupContent" aria-selected="false">Setup</a>
                                </li>
                              <%--  <li class="nav-item" >
                                    <a class="nav-link" id="product-tab" data-toggle="tab" href="#productContent" role="tab" aria-controls="productContent" aria-selected="false" >Product</a>
                                </li>--%>
                                <li class="nav-item">
                                    <a class="nav-link" id="price-tab" data-toggle="tab" href="#priceContent" role="tab" aria-controls="priceContent" aria-selected="false">Price and Discount</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="fixedAssets-tab" data-toggle="tab" href="#fixedAssetsContent" role="tab" aria-controls="fixedAssetsContent" aria-selected="false">Fixed Assets</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="financialDimensions-tab" data-toggle="tab" href="#financialDimensionsContent" role="tab" aria-controls="financialDimensionsContent" aria-selected="false">Financial Dimensions</a>
                                </li>
                            </ul>

                            <div class="tab-content mt-3" id="lineDetailTabContent">
                                <div class="tab-pane fade show active" id="lineDetailContent" role="tabpanel" aria-labelledby="lineDetail-tab">
                                    <div class="container-fluid">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="d-flex flex-column">
                                                       <h5 class="mb-2"><strong>General</strong></h5>
                                                    <div class="mb-3 info-block">
                                                       
                                                        <label>Item number</label>
                                                        <asp:TextBox ID="txtItemNumberID" runat="server" CssClass="form-control custom-textbox" style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                    <div class="mb-3 info-block">
                                                        <label>Item name</label>
                                                        <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control custom-textbox"  style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="d-flex flex-column">
                                                    <div class="mb-3 info-block">
                                                        <label>Procurement category</label>
                                                        <asp:TextBox ID="txtProcurementCategoryLD" runat="server" CssClass="form-control custom-textbox"  style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                    <div class="mb-3 info-block">
                                                        <label>Text</label>
                                                        <asp:TextBox ID="txtTextLD" runat="server" CssClass="form-control custom-textbox"  style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="d-flex flex-column">
                                                    <div class="mb-3 info-block">
                                                        <label>Quantity</label>
                                                        <asp:TextBox ID="txtQuantityLD" runat="server" CssClass="form-control custom-textbox"  style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                    <div class="mb-3 info-block">
                                                        <label>Unit</label>
                                                        <asp:TextBox ID="txtUnitLD" runat="server" CssClass="form-control custom-textbox"  style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                    <div class="mb-3 info-block">
                                                        <label>Unit price</label>
                                                        <asp:TextBox ID="txtUnitPriceLD" runat="server" CssClass="form-control custom-textbox"  style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="d-flex flex-column">
                                                    <div class="mb-3 info-block">
                                                        <label>Adjusted unit price</label>
                                                        <asp:TextBox ID="txtAdjustedUnitPriceLD" runat="server" CssClass="form-control custom-textbox"  style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                    <div class="mb-3 info-block">
                                                        <label>Price unit</label>
                                                        <asp:TextBox ID="txtPriceUnitLD" runat="server" CssClass="form-control custom-textbox"  style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                    <div class="mb-3 info-block">
                                                        <label>Line net amount</label>
                                                        <asp:TextBox ID="txtLineNetAmountLD" runat="server" CssClass="form-control custom-textbox"  style="width: 200px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                  <div class="tab-pane fade" id="setupContent" role="tabpanel" aria-labelledby="setup-tab">
    <div class="container-fluid">
        <div class="row">

            <!-- COLUMN 1 : SALES TAX -->
            <div class="col-md-4">
                <h5 class="mt-3 mb-3"><strong>Sales Tax</strong></h5>

                <div class="form-group mb-2">
                    <label for="txtItemSalesTax">Item Sales Tax</label>
                    <asp:TextBox ID="txtItemSalesTax" runat="server" 
                        CssClass="form-control custom-textbox" style="width: 200px;"></asp:TextBox>
                </div>

                <div class="form-group mb-2">
                    <label for="txtSalesTaxGroup">Sales Tax Group</label>
                    <asp:TextBox ID="txtSalesTaxGroup" runat="server" 
                        CssClass="form-control custom-textbox" style="width: 200px;"></asp:TextBox>
                </div>
            </div>

            <!-- COLUMN 2 : PURCHASE ORDER + REASONS -->
            <div class="col-md-4">
                <h5 class="mt-3 mb-3"><strong>Purchase Order</strong></h5>

                <div class="form-group mb-2">
                    <label for="txtLineNumber">Line Number</label>
                    <asp:TextBox ID="txtLineNumber" runat="server" 
                        CssClass="form-control custom-textbox" style="width: 200px;"></asp:TextBox>
                </div>

                <h5 class="mt-4 mb-2"><strong>Reasons</strong></h5>

                <div class="form-group mb-2">
                    <label for="txtReason">Reason</label>
                    <asp:TextBox ID="txtReason" runat="server" 
                        CssClass="form-control custom-textbox" style="width: 200px;"></asp:TextBox>
                </div>

                <div class="form-group mb-2">
                    <label for="txtReasonComment">Reason Comment</label>
                    <asp:TextBox ID="txtReasonComment" runat="server" 
                        CssClass="form-control custom-textbox" style="width: 200px;"></asp:TextBox>
                </div>
            </div>

            <!-- COLUMN 3 : DELIVERY ADDRESS -->
        <!-- COLUMN 3 : DELIVERY ADDRESS -->
<div class="col-md-4">
    <h5 class="mt-3 mb-3"><strong>DELIVERY ADDRESS</strong></h5>

    <!-- Row 1: Name + Address + Delivery Date side by side -->
    <div class="row mb-2">
        <div class="col-12 col-md-4 mb-2">
            <label for="txtDeliveryName">Name</label>
            <asp:TextBox ID="txtDeliveryName" runat="server" TextMode="MultiLine" Rows="2"
                CssClass="form-control custom-textbox" style="width: 100%;"></asp:TextBox>
        </div>
         <div class="col-12 col-md-4 mb-2">
     <label for="txtDeliveryDate">Delivery Date</label>
     <asp:TextBox ID="TxtDeliveryDate" runat="server" TextMode="Date" 
         CssClass="form-control custom-textbox" style="width: 100%;"></asp:TextBox>
 </div>

     <%--   <div class="col-12 col-md-4 mb-2">
            <label for="txtAddress">Address</label>
            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2"
                CssClass="form-control custom-textbox" style="width: 100%;"></asp:TextBox>
        </div>--%>

      <%--  <div class="col-12 col-md-4 mb-2">
            <label for="txtDeliveryDate">Delivery Date</label>
            <asp:TextBox ID="txtDeliveryDate" runat="server" TextMode="Date" 
                CssClass="form-control custom-textbox"></asp:TextBox>
        </div>--%>
    </div>

    <!-- Row 2: Postal Address -->
  <%--  <div class="row mb-2">
        <div class="col-12">
            <label for="txtPostalAddress">Postal Address</label>
            <asp:TextBox ID="txtPostalAddress" runat="server" 
                CssClass="form-control custom-textbox"  style="width: 200px;" Visible="false"></asp:TextBox>
        </div>
           <div class="col-12">
       <label for="txtAddress" >Address</label>
       <asp:TextBox ID="txtAddress" runat="server"
           CssClass="form-control custom-textbox" Visible="false"></asp:TextBox>
   </div>
    </div>--%>
</div>


        </div>
    </div>
</div>



                            <%--    <div class="tab-pane fade" id="productContent" role="tabpanel" aria-labelledby="product-tab" ><p>Product info goes here...</p></div>--%>
<div class="tab-pane fade" id="priceContent" role="tabpanel" aria-labelledby="price-tab">
    <div class="container-fluid">

        <!-- DISCOUNT + PRICES ROW -->
        <div class="row mb-3">

            <!-- Discount -->
            <div class="col-12 col-md-3 mb-2">
                  <h5 class="mt-3 mb-3"><strong>DISCOUNT</strong></h5>
                <label for="txtDiscount">Discount</label>
                <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control custom-textbox"></asp:TextBox>
            </div>

            <!-- Discount Percentage -->
            <div class="col-12 col-md-3 mb-2">
                <label for="txtDiscountPercentage">Discount Percentage</label>
                <asp:TextBox ID="txtDiscountPercentage" runat="server" CssClass="form-control custom-textbox"></asp:TextBox>
            </div>

            <!-- Multiline Discount + Multiline Discount Percentage BELOW -->
            <div class="col-12 col-md-3 mb-2">
                <label for="txtMultilineDiscount">Multiline Discount</label>
                <asp:TextBox ID="txtMultilineDiscount" runat="server" TextMode="MultiLine" Rows="2" 
                    CssClass="form-control custom-textbox"></asp:TextBox>

                <!-- Multiline Discount Percentage BELOW -->
                <div class="mt-2">
                    <label for="txtMultilineDiscountPercentage">Multiline Discount Percentage</label>
                    <asp:TextBox ID="txtMultilineDiscountPercentage" runat="server" 
                        CssClass="form-control custom-textbox"></asp:TextBox>
                </div>
            </div>

            <!-- PRICES: Charges on Purchases -->
            <div class="col-12 col-md-3 mb-2">
                  <h5 class="mt-3 mb-3"><strong>PRICES</strong></h5>
                <label for="txtChargesOnPurchases">Charges on Purchases</label>
                <asp:TextBox ID="txtChargesOnPurchases" runat="server" CssClass="form-control custom-textbox"></asp:TextBox>
            </div>

        </div>

    </div>
</div>

                                <div class="tab-pane fade" id="fixedAssetsContent" role="tabpanel" aria-labelledby="fixedAssets-tab">
    <div class="container-fluid">
        <div class="row mb-3 align-items-end">

            <!-- Create a new fixed asset (Toggle) -->
            <div class="col-12 col-md-2 mb-2">
                <label class="d-block">Create a new fixed asset</label>
                <div class="d-flex align-items-center mt-1">
                    <label class="switch mb-0">
                        <input type="checkbox" id="chkCreateFixedAsset" runat="server">
                        <span class="slider round"></span>
                    </label>
                    <span class="ml-2">No</span>
                </div>
            </div>

            <!-- Fixed asset group -->
            <div class="col-12 col-md-2 mb-2">
                <label for="ddlFixedAssetGroup">Fixed asset group</label>
                <asp:DropDownList ID="ddlFixedAssetGroup" runat="server" CssClass="form-control custom-textbox">
                </asp:DropDownList>
            </div>

            <!-- Fixed asset number -->
            <div class="col-12 col-md-2 mb-2">
                <label for="ddlFixedAssetNumber">Fixed asset number</label>
                <asp:DropDownList ID="ddlFixedAssetNumber" runat="server" CssClass="form-control custom-textbox">
                </asp:DropDownList>
            </div>

            <!-- Book -->
            <div class="col-12 col-md-2 mb-2">
                <label for="ddlBook">Book</label>
                <asp:DropDownList ID="ddlBook" runat="server" CssClass="form-control custom-textbox">
                </asp:DropDownList>
            </div>

            <!-- Transaction type -->
            <div class="col-12 col-md-2 mb-2">
                <label for="ddlTransactionType">Transaction type</label>
                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control custom-textbox">
                    <asp:ListItem Text="Acquisition" Value="Acquisition" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Acquisition adjustment" Value="AcquisitionAdjustment"></asp:ListItem>
                    <asp:ListItem Text="Depreciation" Value="Depreciation"></asp:ListItem>
                    <asp:ListItem Text="Disposal" Value="Disposal"></asp:ListItem>
                </asp:DropDownList>
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
        <!-- HEADER TAB CONTENT -->
  <div class="tab-pane fade" id="headerTab" role="tabpanel" aria-labelledby="header-tab">
    <div class="container-fluid mt-3">

        <!-- General Collapsible Panel -->
        <div class="card mb-3">
            <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center" 
                 data-toggle="collapse" 
                 data-target="#collapseGeneral" 
                 aria-expanded="true" 
                 aria-controls="collapseGeneral"
                 style="cursor:pointer;">
                <strong>General</strong>
                <span><i class="fa fa-chevron-down rotate-icon"></i></span>
            </div>

              <div id="collapseGeneral" class="collapse show">
    <div class="card-body">
        <div class="row">

            <!-- COLUMN 1: Vendor -->
            <div class="col-md-3">
                <h5 class="mb-2"><strong>Vendor</strong></h5>
                <div class="form-group">
                    <label>Invoice account</label>
                    <asp:TextBox ID="TxtInvoiceAccountHeader" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                </div>

                <h5 class="mt-4 mb-2"><strong>Invoice identification</strong></h5>
                <div class="form-group">
                    <label>Number</label>
                    <asp:TextBox ID="TxtInvoiceIdentificationHeader" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                </div>

                <h5 class="mt-4 mb-2"><strong>Related documents</strong></h5>
                <div class="form-group">
                    <label>Purchase order</label>
                    <asp:TextBox ID="TxtPurchaseOrderHeader" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Product receipt</label>
                    <asp:TextBox ID="TxtFormReceiptHeader" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                </div>
            </div>

            <!-- COLUMN 2: Invoice Description -->
            <div class="col-md-3">
                <h5 class="mb-2"><strong>Invoice Description</strong></h5>
                <div class="form-group">
                    <label>Description</label>
                    <asp:TextBox ID="TxtDescriptionHeader" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Document Number</label>
                    <asp:TextBox ID="TxtDocumentNumberHeader" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                </div>
            </div>

            <!-- COLUMN 3: Invoice Dates -->
            <div class="col-md-3">
                <h5 class="mb-2"><strong>Invoice Dates</strong></h5>
                <div class="form-group">
                    <label>Posting date</label>
                    <asp:TextBox ID="TxtPostingDateHeader" runat="server" CssClass="form-control small-textbox" TextMode="Date"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Invoice date</label>
                    <asp:TextBox ID="TxtInvoiceDateHeader" runat="server" CssClass="form-control small-textbox" TextMode="Date"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Due date</label>
                    <asp:TextBox ID="TxtDueDateHeader" runat="server" CssClass="form-control small-textbox" TextMode="Date"></asp:TextBox>
                </div>
            </div>

            <!-- COLUMN 4: Invoice Status Details -->
            <div class="col-md-3">
                <h5 class="mb-2"><strong>Invoice Status Details</strong></h5>
                <div class="form-group d-flex align-items-center mb-2">
                    <label class="mr-2">Prepayment</label>
                    <label class="switch">
                        <input type="checkbox" ID="chkPrepayment" runat="server">
                        <span class="slider round"></span>
                    </label>
                </div>
                <div class="form-group d-flex align-items-center mb-2">
                    <label class="mr-2">On hold</label>
                    <label class="switch">
                        <input type="checkbox" ID="Checkbox1" runat="server">
                        <span class="slider round"></span>
                    </label>
                </div>
                <div class="form-group">
                    <label>Header budget check results</label>
                    <asp:TextBox ID="TxtBudgetCheckResultsHeader" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                </div>
            </div>

        </div>
    </div>
</div>

        </div>

    <div class="card mb-3">
    <!-- Panel Header -->
    <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center" 
         data-toggle="collapse" 
         data-target="#collapseSetup" 
         aria-expanded="true" 
         aria-controls="collapseSetup"
         style="cursor:pointer;">
        <strong>Setup</strong>
        <span><i class="fa fa-chevron-down rotate-icon"></i></span>
    </div>

    <!-- Collapsible Panel Body -->
  <div id="collapseSetup" class="collapse show">
    <div class="card-body">
        <div class="row">

            <!-- COLUMN 1 -->
            <div class="col-md-2">
                <h4 class="section-title">Cash Discount</h4>

                <div class="form-group info-block">
                    <label>Cash discount</label>
                    <asp:TextBox ID="txtCashDiscount" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>

                <div class="form-group info-block mt-4">
                    <label>Discount percentage</label>
                    <asp:TextBox ID="txtDiscountPercentageSetup" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>
            </div>

            <!-- COLUMN 2 -->
            <div class="col-md-2">
                <h4 class="section-title">Posting</h4>

                <div class="form-group info-block">
                    <label>Posting profile</label>
                    <asp:TextBox ID="txtPostingProfile" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>

                <div class="form-group info-block mt-4">
                    <label>Settlement type</label>
                    <asp:TextBox ID="txtSettlementType" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>
            </div>

            <!-- COLUMN 3 -->
            <div class="col-md-2">
                <h4 class="section-title">Sales Tax</h4>

                <div class="form-group info-block">
                    <label>Sales tax group</label>
                    <asp:TextBox ID="TxtSalesTaxGroupHeader" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>

                <div class="form-group info-block mt-4">
                    <label>Tax exempt number</label>
                    <asp:TextBox ID="txtTaxExemptNumber" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>

                <div class="form-group mt-4">
                    <label class="d-block mb-2">Prices include sales tax</label>

                    <label class="switch">
                        <input type="checkbox" id="chkPricesIncludeSalesTax" runat="server">
                        <span class="slider round"></span>
                    </label>
                </div>
            </div>

            <!-- COLUMN 4 -->
            <div class="col-md-2">
                <h4 class="section-title">Currency</h4>

                <div class="form-group info-block">
                    <label>Currency code</label>
                    <asp:TextBox ID="txtCurrencyCode" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>

                <div class="form-group mt-4">
                    <label class="d-block mb-2">Fixed rate</label>

                    <label class="switch">
                        <input type="checkbox" id="chkFixedRate" runat="server">
                        <span class="slider round"></span>
                    </label>
                </div>
            </div>

            <!-- COLUMN 5 -->
            <div class="col-md-3">
                <div class="form-group info-block">
                    <label>Exchange rate</label>
                    <asp:TextBox ID="txtExchangeRate" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>

                <div class="form-group info-block mt-4">
                    <label>Secondary exchange rate</label>
                    <asp:TextBox ID="txtSecondaryExchangeRate" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>

                <div class="form-group info-block mt-4">
                    <label>Reporting currency fixed exchange rate</label>
                    <asp:TextBox ID="txtReportingFixedRate" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>

                <div class="form-group info-block mt-4">
                    <label>Status</label>
                    <asp:TextBox ID="txtStatus" runat="server"
                        CssClass="form-control borderless-textbox"></asp:TextBox>
                </div>
            </div>

        </div>
    </div>
</div>
        <div class="card mb-3">
    <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center"
         data-toggle="collapse"
         data-target="#collapseApproval"
         aria-expanded="true"
         aria-controls="collapseApproval"
         style="cursor:pointer;">
        <strong>Approval</strong>
        <span><i class="fa fa-chevron-down rotate-icon"></i></span>
    </div>

    <div id="collapseApproval" class="collapse show">
        <div class="card-body">
            <div class="row">

                <!-- COLUMN 1: Approval -->
                <div class="col-md-3">
                   
                    <div class="form-group d-flex align-items-center mb-2">
                        <label class="mr-2 mb-0">Approved</label>
                        <label class="switch mb-0">
                            <input type="checkbox" ID="chkApproved" runat="server" checked="checked">
                            <span class="slider round"></span>
                        </label>
                       
                    </div>
                </div>

                <!-- COLUMN 2: Requested Approver -->
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Requested approver</label>
                      
                             <asp:TextBox ID="TxtRequestApprover" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                           <%-- <asp:ListItem Text="Aaron Con" Value="AaronCon"></asp:ListItem>--%>
                      
                    </div>
                </div>

                <!-- COLUMN 3: Requested Approver Email -->
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Requested approver email address</label>
                        <asp:TextBox ID="TxtApproverEmail" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                    </div>
                </div>

                <!-- COLUMN 4: Release Date + Comment -->
             <%--   <div class="col-md-3">
                    <div class="form-group">
                        <label>Invoice payment release date</label>
                        <asp:TextBox ID="TxtInvoicePaymentReleaseDate" runat="server" CssClass="form-control small-textbox" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Release date comment</label>
                        <asp:TextBox ID="TxtReleaseDateComment" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                    </div>
                </div>--%>

            </div>
        </div>
    </div>

            <div class="card mb-3">
    <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center"
         data-toggle="collapse"
         data-target="#collapseAddress"
         aria-expanded="true"
         aria-controls="collapseAddress"
         style="cursor:pointer;">
        <strong>Address</strong>
        <span><i class="fa fa-chevron-down rotate-icon"></i></span>
    </div>

    <div id="collapseAddress" class="collapse show">
        <div class="card-body">
            <div class="row">

                <!-- COLUMN 1: Delivery Name + Postal Address -->
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Delivery name</label>
                        <asp:TextBox ID="TextBoxDeliveryName" runat="server" CssClass="form-control small-textbox"
                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Postal address</label>
                        <div class="d-flex align-items-center">
                            <asp:TextBox ID="TxtPostalAddress" runat="server" CssClass="form-control small-textbox mr-1"></asp:TextBox>
                            <button type="button" class="btn btn-sm btn-outline-secondary mr-1" title="Edit address">
                                <i class="fa fa-map-marker"></i>
                            </button>
                            <button type="button" class="btn btn-sm btn-outline-secondary" title="Add address">
                                <i class="fa fa-plus"></i>
                            </button>
                        </div>
                    </div>
                </div>

                <!-- COLUMN 2: Address (multiline readonly) -->
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Address</label>
                        <asp:TextBox ID="TxtAddress" runat="server" CssClass="form-control small-textbox"
                            TextMode="MultiLine" Rows="5" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>

                <!-- COLUMN 3: Remittance -->
                <div class="col-md-3">
                    <h5 class="mb-2"><strong>Remittance</strong></h5>
                    <div class="form-group">
                        <label>Remittance location</label>
                        <asp:TextBox ID="TextBoxRemittance" runat="server" CssClass="form-control small-textbox"
     ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Address</label>
                        <asp:TextBox ID="TxtAddress1" runat="server" CssClass="form-control small-textbox"
                            TextMode="MultiLine" Rows="5" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>

                <!-- COLUMN 4: Inventory -->
                <div class="col-md-3">
                    <h5 class="mb-2"><strong>Inventory</strong></h5>
                    <div class="form-group">
                        <label>Site</label>
                        <asp:TextBox ID="TxtSite" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Warehouse</label>
                        <asp:TextBox ID="TxtWarehouse" runat="server" CssClass="form-control small-textbox"></asp:TextBox>
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

          </div> <!-- Close tab-content -->


    <script>
        document.querySelectorAll('.card-header').forEach(header => {
            header.addEventListener('click', function () {
                const icon = this.querySelector('.rotate-icon');
                icon.classList.toggle('rotate');
            });
        });
    </script>

</asp:Content>