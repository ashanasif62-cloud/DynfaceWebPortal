<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderLines_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_ListPage" %>
<%@ Register Src="~/DropDownList_ProductCombination.ascx" TagPrefix="uc" TagName="ProductLookup" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* ═══════════════════════════════════════════
           VARIABLES & BASE
           ═══════════════════════════════════════════ */
        :root {
            --field-disabled-bg:   #f3f2f1;
            --field-disabled-color:#6e6e6e;
            --field-border:        #d0d0d0;
            --field-border-focus:  #0078d4;
            --section-title-color: #323130;
            --label-color:         #605e5c;
            --card-shadow:         0 1px 3px rgba(0,0,0,.10);
            --tab-active-border:   #0078d4;
            --grid-btn-gap:        8px;
        }

        /* ═══════════════════════════════════════════
           FORM CONTROLS  –  consistent sizing
           ═══════════════════════════════════════════ */
        .form-control,
        .aspnet-textbox,
        select.form-control {
            font-size: 0.8125rem;   /* 13 px */
            height: 30px;
            padding: 3px 8px;
            border: 1px solid var(--field-border);
            border-radius: 2px;
            background-color: #fff;
            color: #323130;
            line-height: 1.4;
            width: 100%;
            box-sizing: border-box;
        }

        textarea.form-control {
            height: auto;           /* let rows attribute govern */
            resize: vertical;
        }

        .form-control:focus,
        select.form-control:focus {
            border-color: var(--field-border-focus);
            outline: none;
            box-shadow: 0 0 0 2px rgba(0,120,212,.15);
        }

        /* ── Disabled / read-only fields ── */
        .form-control[readonly],
        .form-control:disabled,
        select.form-control:disabled,
        .custom-textbox[readonly],
        input[readonly].form-control {
            background-color: var(--field-disabled-bg) !important;
            color:            var(--field-disabled-color) !important;
            border-color:     #e0e0e0 !important;
            cursor:           default;
            opacity:          1;            /* override browser fading */
        }

        /* Dropdowns that are disabled get same treatment */
        select:disabled {
            background-color: var(--field-disabled-bg) !important;
            color:            var(--field-disabled-color) !important;
            border-color:     #e0e0e0 !important;
        }

        /* ═══════════════════════════════════════════
           SECTION TITLES
           ═══════════════════════════════════════════ */
        .custom-section-title {
            font-size:      0.6875rem;      /* 11 px */
            font-weight:    700;
            text-transform: uppercase;
            letter-spacing: 0.06em;
            color:          #106ebe;
            margin-bottom:  6px;
            padding-bottom: 2px;
            border-bottom:  1px solid #e8e8e8;
        }

        /* ═══════════════════════════════════════════
           INFO BLOCKS  (label + control stacked)
           ═══════════════════════════════════════════ */
        .info-block {
            margin-bottom: 10px;
        }
        .info-block strong,
        .info-block > label {
            display:     block;
            font-size:   0.75rem;
            font-weight: 600;
            color:       var(--label-color);
            margin-bottom: 3px;
            line-height: 1.2;
        }

        /* ═══════════════════════════════════════════
           TOGGLE SWITCH
           ═══════════════════════════════════════════ */
        .toggle-switch {
            position: relative;
            display:  inline-block;
            width:    44px;
            height:   22px;
        }
        .toggle-switch input[type="checkbox"] { display: none; }

        .slider {
            position:         absolute;
            cursor:           default;
            inset:            0;
            background-color: #c8c8c8;
            transition:       0.3s;
            border-radius:    22px;
        }
        .slider:before {
            position:         absolute;
            content:          "";
            height:           16px;
            width:            16px;
            left:             3px;
            bottom:           3px;
            background-color: #fff;
            transition:       0.3s;
            border-radius:    50%;
        }
        .toggle-switch input[type="checkbox"]:checked + .slider {
            background-color: #107c10;
        }
        .toggle-switch input[type="checkbox"]:checked + .slider:before {
            transform: translateX(22px);
        }

        .small-icon { font-size: 0.75rem; vertical-align: middle; }

        /* ═══════════════════════════════════════════
           GRID TOOLBAR BUTTONS
           ═══════════════════════════════════════════ */
        .grid-toolbar {
            display:     flex;
            align-items: center;
            flex-wrap:   wrap;
            gap:         var(--grid-btn-gap);
            padding:     6px 0 4px;
        }
        .grid-btn a,
        .grid-btn asp\:LinkButton,
        .action-items.grid-btn a {
            font-size: 0.8125rem !important;
        }

        /* ═══════════════════════════════════════════
           CUSTOM DROPDOWN  (JS-driven)
           ═══════════════════════════════════════════ */
        .custom-dropdown {
            display:       none;
            position:      absolute;
            background:    #fff;
            border:        1px solid #d0d0d0;
            border-radius: 3px;
            min-width:     160px;
            z-index:       1050;
            box-shadow:    0 4px 12px rgba(0,0,0,.12);
        }
        .custom-dropdown .dropdown-item {
            display:     block;
            padding:     7px 14px;
            font-size:   0.8125rem;
            color:       #323130;
            text-decoration: none;
            white-space: nowrap;
        }
        .custom-dropdown .dropdown-item:hover {
            background-color: #f3f2f1;
        }

        /* ═══════════════════════════════════════════
           PAGE HEADER  (PO id / vendor bar)
           ═══════════════════════════════════════════ */
        .po-header-bar {
            display:         flex;
            justify-content: space-between;
            align-items:     center;
            padding:         6px 0 10px;
            border-bottom:   1px solid #edebe9;
            margin-bottom:   10px;
        }
        .po-header-bar .po-title {
            font-size:   1rem;
            font-weight: 700;
            color:       #323130;
        }
        .po-header-bar .po-status {
            font-size:   0.9375rem;
            font-weight: 600;
            color:       #0078d4;
        }

        /* ═══════════════════════════════════════════
           TABS
           ═══════════════════════════════════════════ */
        .nav-tabs .nav-link {
            font-size:  0.8125rem;
            padding:    6px 14px;
            color:      #605e5c;
            border:     none;
            border-bottom: 2px solid transparent;
        }
        .nav-tabs .nav-link.active {
            color:        #0078d4;
            border-bottom-color: var(--tab-active-border);
            font-weight:  600;
            background:   transparent;
        }
        .nav-tabs .nav-link:hover:not(.active) {
            color: #323130;
            border-bottom-color: #c8c8c8;
        }
        .nav-tabs { border-bottom: 1px solid #edebe9; }

        /* ═══════════════════════════════════════════
           ACCORDION PANELS (Header tab)
           ═══════════════════════════════════════════ */
        .card { border: 1px solid #edebe9; border-radius: 3px; margin-bottom: 6px; }
        .card-header {
            background:  #faf9f8;
            padding:     7px 12px;
            border-bottom: 1px solid #edebe9;
        }
        .card-header a { font-size: 0.8125rem; color: #323130 !important; }
        .card-body { padding: 14px 16px; }

        /* ═══════════════════════════════════════════
           COLLAPSE SECTION HEADERS  (d365-toggle-header)
           ═══════════════════════════════════════════ */
        .d365-toggle-header {
            font-size:       0.8125rem;
            font-weight:     600;
            color:           #323130;
            padding:         7px 0;
            border-bottom:   1px solid #edebe9;
            text-decoration: none;
            display:         flex;
            justify-content: space-between;
            align-items:     center;
        }
        .d365-toggle-header:hover { color: #0078d4; }
        .section-title { font-size: 0.8125rem; }

        /* ═══════════════════════════════════════════
           GRID TABLE
           ═══════════════════════════════════════════ */
        .table-condensed th,
        .table-condensed td {
            font-size:  0.78rem;
            padding:    4px 8px;
            white-space: nowrap;
            vertical-align: middle;
        }
        .table-condensed th {
            background:  #f3f2f1;
            color:       #323130;
            font-weight: 600;
            border-bottom: 2px solid #d0d0d0;
        }
        .table-hover tbody tr:hover { background-color: #f0f6ff; }

        /* ═══════════════════════════════════════════
           FINANCIAL DIMENSIONS CONTAINER
           ═══════════════════════════════════════════ */
        .info-row {
            display:   flex;
            flex-wrap: wrap;
            gap:       16px;
            margin-bottom: 8px;
        }
        .info-row .info-block { flex: 1 1 200px; min-width: 160px; }

        /* ═══════════════════════════════════════════
           MISC FIXES
           ═══════════════════════════════════════════ */

        /* Remove double border on nested card inside accordion */
        .accordion > .card { box-shadow: var(--card-shadow); }

        /* Prevent col overflow in tab panes */
        .tab-pane .row { margin-left: 0; margin-right: 0; }
        .tab-pane [class^="col-"] { padding-left: 10px; padding-right: 10px; }

        /* Gap utility for grid toolbar (Bootstrap 4 polyfill) */
        .gap-2 { gap: 8px !important; }

        /* Uniform label above dropdown or textbox */
        .form-label {
            font-size:     0.75rem;
            font-weight:   600;
            color:         var(--label-color);
            margin-bottom: 3px;
            display:       block;
        }
    </style>
</asp:Content>

<%-- ═══════════════════════════════════════════════════════════════
     ACTION PANEL
     ═══════════════════════════════════════════════════════════════ --%>
<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">

    <%-- Back --%>
    <div class="action-items">
        <a href="/ESS/PR/AllPurchaseOrder_ListPage.aspx" class="btn-link">
            <i class="mdi mdi-arrow-left" style="margin-right:4px;"></i>Back
        </a>
    </div>

    <%-- Save --%>
    <div class="action-items">
        <asp:LinkButton ID="BtnSaveHeader" runat="server" OnClick="BtnSave_Header_Click">
            <i class="mdi mdi-content-save" style="margin-right:4px;"></i>Save
        </asp:LinkButton>
    </div>

    <%-- Delete --%>
    <div class="action-items">
        <asp:LinkButton ID="BtnDeleteHeader" runat="server" OnClick="BtnDeleteHeader_Click">
            <i class="mdi mdi-delete"></i> Delete
        </asp:LinkButton>
    </div>

    <%-- Workflow --%>
    <div class="action-items dropdown">
        <asp:LinkButton ID="btnWorkflow" runat="server"
            CssClass="dropdown-toggle"
            OnClientClick="toggleWorkflowDropdown(); return false;">
            <i class="mdi mdi-sitemap"></i> Workflow
        </asp:LinkButton>
        <div id="workflowMenu" class="custom-dropdown">
            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="dropdown-item" OnClick="btnSubmit_Click">
                <i class="mdi mdi-shape-plus"></i> Submit
            </asp:LinkButton>
        </div>
    </div>

</asp:Content>

<%-- ═══════════════════════════════════════════════════════════════
     PAGE CONTENT
     ═══════════════════════════════════════════════════════════════ --%>
<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upddetailPanel" ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <%-- ── PO Header bar ── --%>
            <div class="po-header-bar">
                <div class="po-title">
                    <asp:Label ID="lblPurchaseOrderID" runat="server" /><span style="font-weight:400; margin:0 6px;">:</span><asp:Label ID="lblVendorAccount" runat="server" /><span style="font-weight:400; margin:0 6px;">-</span><asp:Label ID="lblVendorName" runat="server" />
                </div>
                <div class="po-status">
                    <asp:Label ID="lblPurchStatus" runat="server" />
                </div>
            </div>

            <%-- ── Main tabs: Lines / Header ── --%>
            <ul class="nav nav-tabs" id="purchaseOrderTabs" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="lines-tab" data-toggle="tab" href="#linesContent" role="tab">Lines</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="header-tab" data-toggle="tab" href="#headerContent" role="tab">Header</a>
                </li>
            </ul>

            <div class="tab-content">

                <%-- ══════════════════════════════════════════
                     LINES TAB
                     ══════════════════════════════════════════ --%>
                <div class="tab-pane fade show active" id="linesContent" role="tabpanel">

                    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

                    <%-- Purchase order header collapse --%>
                    <a href="#purchaseOrderHeaderPanel"
                       class="d365-toggle-header mt-3"
                       data-toggle="collapse" role="button"
                       aria-expanded="true" aria-controls="purchaseOrderHeaderPanel">
                        <span class="section-title">Purchase order header</span>
                        <i class="mdi mdi-chevron-down rotate-icon small-icon"></i>
                    </a>

                    <div class="collapse show mt-2" id="purchaseOrderHeaderPanel">
                        <div class="card">
                            <div class="card-body">
                                <div class="row">

                                    <%-- DELIVERY --%>
                                    <div class="col-md-2">
                                        <div class="custom-section-title">Delivery</div>
                                        <div class="info-block">
                                            <strong>Requested receipt date</strong>
                                            <asp:TextBox ID="txtUniqueDeliveryHeaderRequestDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="info-block">
                                            <strong>Earliest confirmed receipt date</strong>
                                            <asp:TextBox ID="txtEarliestConfirmedReceiptDate" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <%-- DISCOUNTS --%>
                                    <div class="col-md-2">
                                        <div class="custom-section-title">Discounts</div>
                                        <div class="info-block">
                                            <strong>Total discount %</strong>
                                            <asp:TextBox ID="txtTotalDiscount" ReadOnly="true" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <%-- VENDOR --%>
                                    <div class="col-md-2">
                                        <div class="custom-section-title">Vendor</div>
                                        <div class="info-block">
                                            <strong>Contact</strong>
                                            <asp:TextBox ID="txtVendorContact" ReadOnly="true" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <%-- REPLENISHMENT --%>
                                    <div class="col-md-2">
                                        <div class="custom-section-title">Replenishment</div>
                                        <div class="info-block">
                                            <strong>Service category</strong>
                                            <asp:TextBox ID="txtServiceCategory" ReadOnly="true" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="info-block">
                                            <strong>Location</strong>
                                            <asp:TextBox ID="txtLocation" ReadOnly="true" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <%-- CROSS DOCKING DATES --%>
                                    <div class="col-md-2">
                                        <div class="custom-section-title">Cross Docking Dates</div>
                                        <div class="info-block">
                                            <strong>Requested receipt date</strong>
                                            <asp:TextBox ID="txtCrossDockRequestedDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="info-block">
                                            <strong>Cross docking date</strong>
                                            <asp:TextBox ID="txtCrossDockDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <%-- PRODUCT / ORDER CREATION --%>
                                    <div class="col-md-2">
                                        <div class="custom-section-title">Product / Order Creation</div>
                                        <div class="info-block">
                                            <strong>Auto created</strong>
                                            <div class="mt-1">
                                                <label class="toggle-switch">
                                                    <asp:CheckBox ID="chkAutoCreated" runat="server" ReadOnly="true" />
                                                    <span class="slider"></span>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="info-block">
                                            <strong>Origin</strong>
                                            <asp:TextBox ID="txtOrigin" runat="server" CssClass="form-control" Text="Purchase" ReadOnly="true" />
                                        </div>
                                    </div>

                                </div><%-- /row --%>
                            </div>
                        </div>
                    </div><%-- /collapse purchaseOrderHeaderPanel --%>

                    <%-- Purchase order details collapse --%>
                    <a class="d365-toggle-header mt-3">
                        <span class="section-title">Purchase order details</span>
                        <i class="mdi mdi-chevron-down rotate-icon small-icon"></i>
                    </a>

                    <div class="collapse show mt-2" id="gridPanel">
                        <div class="card">
                            <div class="card-body pb-1">

                                <%-- Grid toolbar --%>
                                <div class="grid-toolbar">
                                    <div class="action-items grid-btn">
                                        <asp:LinkButton ID="BtnAddLine" runat="server" OnClick="btnNew_Grid_Click" CssClass="text-primary">
                                            <i class="mdi mdi-plus"></i> Add line
                                        </asp:LinkButton>
                                    </div>
                                    <div class="action-items grid-btn">
                                        <asp:LinkButton ID="btnDelete" CssClass="text-primary" runat="server" OnClick="btnDelete_Click">
                                            <i class="mdi mdi-delete"></i> Remove
                                        </asp:LinkButton>
                                    </div>

                                    <%-- Financial dropdown --%>
                                    <div class="action-items grid-btn dropdown">
                                        <asp:LinkButton ID="btnFinancial" runat="server"
                                            CssClass="text-primary dropdown-toggle"
                                            OnClientClick="toggleDropdown('financialMenu'); return false;">
                                            Financial
                                        </asp:LinkButton>
                                        <div id="financialMenu" class="custom-dropdown">
                                            <asp:LinkButton ID="btnMaintainCharges" runat="server" CssClass="text-primary dropdown-item" OnClick="btnMaintainCharges_Click">
                                                Maintain Charges
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnSalesTax" runat="server" CssClass="text-primary dropdown-item" OnClick="btnSalesTax_Click">
                                                Sales Tax
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <%-- Inventory dropdown --%>
                                    <div class="action-items grid-btn dropdown">
                                        <asp:LinkButton ID="btnInventory" runat="server"
                                            CssClass="text-primary dropdown-toggle"
                                            OnClientClick="toggleDropdown('inventoryMenu'); return false;">
                                            Inventory
                                        </asp:LinkButton>
                                        <div id="inventoryMenu" class="custom-dropdown">
                                            <asp:LinkButton ID="btnOnHand" runat="server" CssClass="text-primary dropdown-item" OnClick="btnOn_HandClick">
                                                On Hand
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <%-- Update Lines dropdown --%>
                                    <div class="action-items grid-btn dropdown">
                                        <asp:LinkButton ID="btnUpdateLine" runat="server"
                                            CssClass="text-primary dropdown-toggle"
                                            OnClientClick="toggleDropdown('updateline'); return false;"
                                            Visible="false">
                                            Update Lines
                                        </asp:LinkButton>
                                        <div id="updateline" class="custom-dropdown">
                                            <asp:LinkButton ID="btnUpdate_Line" runat="server" CssClass="text-primary dropdown-item" OnClick="btnUpdateLine_Click" Visible="false">
                                                Deliver Remainder
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div><%-- /grid-toolbar --%>

                                <%-- Grid --%>
                                <div class="table-responsive" style="overflow-x:auto; white-space:nowrap;">
                                    <asp:GridView ID="gridView" runat="server" data="searchable"
                                        CssClass="table table-condensed no-border table-hover sortable"
                                        ShowHeaderWhenEmpty="true"
                                        OnRowEditing="gridView_RowEditing"
                                        OnRowDataBound="gridView_RowDataBound"
                                        EmptyDataText="No Record Found."
                                        DataKeyNames="RecId"
                                        AutoGenerateColumns="false">
                                        <Columns>
                                            <%-- Checkbox --%>
                                            <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                                <HeaderTemplate>
                                                    <input type="checkbox" id="chk_SelectAll" class="round-checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_SelectSingle" AutoPostBack="true" OnCheckedChanged="chk_SelectSingle_CheckedChanged" runat="server" CssClass="round-checkbox" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate><asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtType" Enabled="false" Visible="false" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Budget check results">
                                                <ItemTemplate><asp:Label ID="lblBudgetCheckResults" runat="server" Text='<%# Bind("BudgetCheckResult") %>' Visible="false"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtBudgetCheckResults" Enabled="false" runat="server" Visible="false"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Line number">
                                                <ItemTemplate><asp:Label ID="lblLineNumber" runat="server" Text='<%# Bind("LineNumber") %>' Style="text-align:right; display:block;"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtLineNumber" Visible="false" Enabled="false" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Item number">
                                                <ItemTemplate><asp:Label ID="lblItemId" runat="server" Text='<%# Bind("ItemId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlItemId" OnSelectedIndexChanged="ddlTemId_fillDimension" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                    <asp:TextBox ID="txtItemId" runat="server" Text='<%# Bind("ItemId") %>' Visible="true"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Product name">
                                                <ItemTemplate><asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtProductName" ClientIDMode="Static" Enabled="false" runat="server" CssClass="form-control autocomplete-input"></asp:TextBox>
                                                    <asp:TextBox ID="txtProductNameEdit" runat="server" Text='<%# Bind("ItemName") %>' Visible="true"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Item Description" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("ItemTextDescription") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Procurement category">
                                                <ItemTemplate><asp:Label ID="lblProcurementCategory" Enabled="false" runat="server" Text='<%# Bind("ProcurementCategory") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtProcurementCategory" Visible="false" Enabled="false" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Variant number">
                                                <ItemTemplate><asp:Label ID="lblVariantNumber" runat="server" Text='<%# Bind("VariantId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtVariantNumber" Visible="false" Enabled="true" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Site Name" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblsitename" runat="server" Text='<%# Bind("SiteName") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtSitename" Enabled="false" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Combinations">
                                                <EditItemTemplate>
                                                    <div style="min-width:400px; overflow:visible;">
                                                        <uc:ProductLookup ID="ProductLookupControl" runat="server" onproductselected="ProductLookupControl_ProductSelected" />
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Configuration">
                                                <ItemTemplate><asp:Label ID="lblConfigId" runat="server" Text='<%# Bind("ConfigId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlConfigId" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Color">
                                                <ItemTemplate><asp:Label ID="lblInventColorId" runat="server" Text='<%# Bind("InventColorId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlInventColorId" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Size">
                                                <ItemTemplate><asp:Label ID="lblInventSizeId" runat="server" Text='<%# Bind("InventSizeId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlInventSizeId" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Style">
                                                <ItemTemplate><asp:Label ID="lblInventStyleId" runat="server" Text='<%# Bind("InventStyleId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlInventStyleId" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Batch Number">
                                                <ItemTemplate><asp:Label ID="lblInventBatchId" runat="server" Text='<%# Bind("InventBatchId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlInventBatchId" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Serial Number">
                                                <ItemTemplate><asp:Label ID="lblInventSerialId" runat="server" Text='<%# Bind("InventSerialId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlInventSerialId" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="WMS Location">
                                                <ItemTemplate><asp:Label ID="lblWMSLocationId" runat="server" Text='<%# Bind("WMSLocationId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlWMSLocationId" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Site">
                                                <ItemTemplate><asp:Label ID="lblSite" runat="server" Text='<%# Bind("InventSiteId") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlSiteId" OnSelectedIndexChanged="ddlsiteId_selection" AutoPostBack="true" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Warehouse">
                                                <ItemTemplate><asp:Label ID="lblWarehouse" runat="server" Text='<%# Bind("InventLocationID") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlWarehouse" AutoPostBack="true" OnSelectedIndexChanged="ddlwarehouse_selection" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="CW quantity">
                                                <ItemTemplate><asp:Label ID="lblCWQuantity" runat="server" Text='<%# Bind("PdsCWInventoryQty") %>' Visible="false"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtCWQuantity" Enabled="false" runat="server" Visible="false"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="CW unit">
                                                <ItemTemplate><asp:Label ID="lblCWUnit" runat="server" Text='<%# Bind("CWUnitId") %>' Visible="false"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtCWUnit" Enabled="false" runat="server" Visible="false"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate><asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("PurchQty", "{0:N2}") %>' Style="text-align:right; display:block;"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtQuantity" runat="server" OnTextChanged="OnPurchQty_Changed" AutoPostBack="true"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate><asp:Label ID="lblUnit" runat="server" Text='<%# Bind("PurchUnitofMeasureCode") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtUnit" Enabled="false" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Unit price">
                                                <ItemTemplate><asp:Label ID="lblUnitPrice" runat="server" Text='<%# Bind("PurchPrice") %>' Style="text-align:right; display:block;"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtUnitPrice" OnTextChanged="OnUnitPrice_Changed" AutoPostBack="true" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Adjusted unit price">
                                                <ItemTemplate><asp:Label ID="lblAdjustedUnitPrice" runat="server" Text='<%# Bind("CalculatedUnitPrice", "{0:N2}") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtAdjustedUnitPrice" Visible="false" Enabled="false" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Discount">
                                                <ItemTemplate><asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("LineDiscount", "{0:N2}") %>' Style="text-align:right; display:block;"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtDiscount" OnTextChanged="OnDiscountValue_Changed" AutoPostBack="true" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Discount percent">
                                                <ItemTemplate><asp:Label ID="lblDiscountPercent" runat="server" Text='<%# Bind("LineDiscountPercent", "{0:N2}") %>' Style="text-align:right; display:block;"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtDiscountPercent" OnTextChanged="OnDiscountPercent_Changed" AutoPostBack="true" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Net amount">
                                                <ItemTemplate><asp:Label ID="lblNetAmount" runat="server" Text='<%# Bind("LineAmount", "{0:N2}") %>' Style="text-align:right; display:block;"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtNetAmount" OnTextChanged="OnLineAmount_Changed" AutoPostBack="true" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Adjusted net amount">
                                                <ItemTemplate><asp:Label ID="lblAdjustedNetAmount" runat="server" Text='<%# Bind("CalculatedLineAmount", "{0:N2}") %>' Visible="false"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtAdjustedNetAmount" Enabled="false" runat="server" Visible="false"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Receive now">
                                                <ItemTemplate><asp:Label ID="lblReceiveNow" runat="server" Text='<%# Bind("ReceiveNow") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtReceiveNow" Visible="false" Enabled="false" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="CW receive now">
                                                <ItemTemplate><asp:Label ID="lblCWReceiveNow" runat="server" Text='<%# Bind("CWReceiveNow") %>' Visible="false"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtCWReceiveNow" Enabled="false" runat="server" Visible="false"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Quality order status">
                                                <ItemTemplate><asp:Label ID="lblQualityOrderStatus" runat="server" Text='<%# Bind("QualityOrderStatusDisplay") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtQualityOrderStatus" Visible="false" Enabled="false" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Load ID">
                                                <ItemTemplate><asp:Label ID="lblLoadID" runat="server" Text='<%# Bind("LoadID") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtLoadID" Visible="false" Enabled="false" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Inventory Quantity" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblInventoryQuantity" runat="server" Text='<%# Bind("QtyOrdered", "{0:N2}") %>' Style="text-align:right; display:block;"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtInventoryQuantity" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Deliver Remainder" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblDeliverRemainder" runat="server" Text='<%# Bind("RemainPurchPhysical", "{0:N2}") %>' Style="text-align:right; display:block;"></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtDeliverRemainder" runat="server"></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <%-- Hidden reference fields --%>
                                            <asp:TemplateField HeaderText="RFQ Number" Visible="false"><ItemTemplate><asp:Label ID="lblRfqNumber" runat="server" Text='<%# Bind("RfqNumber") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="RFQ Reply Number" Visible="false"><ItemTemplate><asp:Label ID="lblRfqReplyNumber" runat="server" Text='<%# Bind("RfqReplyNumber") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="RFQ Line Number" Visible="false"><ItemTemplate><asp:Label ID="lblRfqLineNumber" runat="server" Text='<%# Bind("RfqLineNumber") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Procurement Category" Visible="false"><ItemTemplate><asp:Label ID="lblOrderProcurementCategory" runat="server" Text='<%# Bind("ProcurementCategory") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Name" Visible="false"><ItemTemplate><asp:Label ID="lblOrderProductName" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Purchase Requisition" Visible="false"><ItemTemplate><asp:Label ID="lblPrRequisition" runat="server" Text='<%# Bind("PurchReqId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Requisition Product Name" Visible="false"><ItemTemplate><asp:Label ID="lblPrProductName" runat="server" Text='<%# Bind("RequisitionProductName") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Supplier Part Auxiliary ID" Visible="false"><ItemTemplate><asp:Label ID="lblPrSupplierAuxId" runat="server" Text='<%# Bind("PurchSupplierAuxId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Intercompany Origin" Visible="false"><ItemTemplate><asp:Label ID="lblIntercompanyOrigin" runat="server" Text='<%# Bind("IntercompanyOrigin") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reference External" Visible="false"><ItemTemplate><asp:Label ID="lblReferenceExternal" runat="server" Text='<%# Bind("ExternalItemId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Origin" Visible="false"><ItemTemplate><asp:Label ID="lblReferenceOrigin" runat="server" Text='<%# Bind("PurchaseOrderLineCreationMethod") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Requisition" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryCustomerRequisition" runat="server" Text='<%# Bind("CustPurchaseOrderFormNum") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Reference" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryCustomerReference" runat="server" Text='<%# Bind("CustomerRef") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="General Budget Reservation" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryBudgetReservation" runat="server" Text='<%# Bind("BudgetReservationLine_PSN") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Line Status" Visible="false"><ItemTemplate><asp:Label ID="lblStatusLineStatus" runat="server" Text='<%# Bind("PurchStatus") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stopped" Visible="false"><ItemTemplate><asp:CheckBox ID="chkStatusStopped" runat="server" Checked='<%# Eval("Blocked").ToString() == "Yes" %>' Enabled="false" /></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Prevent Partial Delivery" Visible="false"><ItemTemplate><asp:CheckBox ID="chkStatusPreventPartial" runat="server" Checked='<%# Eval("Completed").ToString() == "Yes" %>' Enabled="false" /></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="State" Visible="false"><ItemTemplate><asp:Label ID="lblStatusState" runat="server" Text='<%# Bind("WorkflowState") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quality Order Status" Visible="false"><ItemTemplate><asp:Label ID="lblStatusQualityOrder" runat="server" Text='<%# Bind("qualityOrderStatusDisplay") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Finalized" Visible="false"><ItemTemplate><asp:CheckBox ID="chkStatusFinalized" runat="server" Checked='<%# Eval("IsFinalized").ToString() == "Yes" %>' Enabled="false" /></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Added by POS Receiving" Visible="false"><ItemTemplate><asp:Label ID="lblStatusAddedByPOS" runat="server" Text='<%# Bind("IsAddedByChannel") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lot Id" Visible="false"><ItemTemplate><asp:Label ID="lblSetupLotId" runat="server" Text='<%# Bind("NewLotID") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Matching Policy" Visible="false"><ItemTemplate><asp:Label ID="lblSetupMatchingPolicy" runat="server" Text='<%# Bind("MatchingPolicy") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Return Action" Visible="false"><ItemTemplate><asp:Label ID="lblSetupReturnAction" runat="server" Text='<%# Bind("ReturnActionId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Scrap" Visible="false"><ItemTemplate><asp:CheckBox ID="chkSetupScrap" runat="server" Checked='<%# Eval("Scrap").ToString() == "Yes" %>' /></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Sales Tax Group" Visible="false"><ItemTemplate><asp:Label ID="lblSetupItemSalesTaxGroup" runat="server" Text='<%# Bind("TaxItemGroup") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sales Tax Group" Visible="false"><ItemTemplate><asp:Label ID="lblSetupSalesTaxGroup" runat="server" Text='<%# Bind("TaxGroup") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="1099 Amount" Visible="false"><ItemTemplate><asp:Label ID="lblSetup1099Amount" runat="server" Text='<%# Bind("Tax1099Amount") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="1099 State Amount" Visible="false"><ItemTemplate><asp:Label ID="lblSetup1099StateAmount" runat="server" Text='<%# Bind("Tax1099StateAmount") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ledger Account" Visible="false"><ItemTemplate><asp:TextBox ID="txtSetupLedgerAccount" runat="server" Text='<%# Bind("LedgerDimension") %>' CssClass="form-control form-control-sm"></asp:TextBox></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inventory Quantity" Visible="false"><ItemTemplate><asp:Label ID="lblSetupInventoryQuantity" runat="server" Text='<%# Bind("QtyOrdered") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice Remainder" Visible="false"><ItemTemplate><asp:Label ID="lblSetupInvoiceRemainder" runat="server" Text='<%# Bind("RemainInventFinancial") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Confirmed Receipt Date" Visible="false"><ItemTemplate><asp:Label ID="lblSetupConfirmedReceiptDate" runat="server" Text='<%# Bind("ConfirmedDlv") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Type" Visible="false"><ItemTemplate><asp:Label ID="lblSetupDeliveryType" runat="server" Text='<%# Bind("DeliveryType") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryAddressName" runat="server" Text='<%# Bind("DeliveryName") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Address" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryAddress" runat="server" Text='<%# Bind("Description") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address Full" Visible="false"><ItemTemplate><asp:Label ID="lblAddressFull" runat="server" Text='<%# Bind("Address") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Customer Requisition" Visible="false"><ItemTemplate><asp:Label ID="lblServiceCustomerRequisition" runat="server" Text='<%# Bind("CustPurchaseOrderFormNum") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Customer Reference" Visible="false"><ItemTemplate><asp:Label ID="lblServiceCustomerReference" runat="server" Text='<%# Bind("CustomerRef") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Attention Information" Visible="false"><ItemTemplate><asp:Label ID="lblAttentionInformation" runat="server" Text='<%# Bind("ReqAttention") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Planned Order Number" Visible="false"><ItemTemplate><asp:Label ID="lblPlannedOrderNumber" runat="server" Text='<%# Bind("ReqPOId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Master Plan" Visible="false"><ItemTemplate><asp:Label ID="lblPlannedOrderMasterPlan" runat="server" Text='<%# Bind("ReqPlanIdSched") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reference Type" Visible="false"><ItemTemplate><asp:Label ID="lblItemReferenceType" runat="server" Text='<%# Bind("ItemRefType") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reference Number" Visible="false"><ItemTemplate><asp:Label ID="lblItemReferenceNumber" runat="server" Text='<%# Bind("InventRefId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reference Lot" Visible="false"><ItemTemplate><asp:Label ID="lblItemReferenceLot" runat="server" Text='<%# Bind("InventRefTransId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Requested Receipt Date" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryRequestedDate" runat="server" Text='<%# Bind("DeliveryDate") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Confirmed Receipt Date" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryConfirmedDate" runat="server" Text='<%# Bind("ConfirmedDlv") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Planning Priority" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryPlanningPriority" runat="server" Text='<%# Bind("PlanningPriority") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Overdelivery" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryOverDelivery" runat="server" Text='<%# Bind("OverDeliveryPct") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mode of Delivery" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryMode" runat="server" Text='<%# Bind("DlvMode") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Underdelivery" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryUnderDelivery" runat="server" Text='<%# Bind("UnderDeliveryPct") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Terms" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryTerms" runat="server" Text='<%# Bind("DlvTerm") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Type" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryType" runat="server" Text='<%# Bind("DeliveryType") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Direct Delivery Status" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryDirectStatus" runat="server" Text='<%# Bind("MCRDropShipStatus") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comments" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryComments" runat="server" Text='<%# Bind("MCRDropShipComment") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Direct Delivery" Visible="false"><ItemTemplate><asp:Label ID="lblDeliveryDirect" runat="server" Text='<%# Bind("MCRDropShipment") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bar Code" Visible="false"><ItemTemplate><asp:Label ID="lblBarCode" runat="server" Text='<%# Bind("BarCode") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bar Code Setup" Visible="false"><ItemTemplate><asp:Label ID="lblBarCodeSetup" runat="server" Text='<%# Bind("BarCodeType") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cross Docking" Visible="false"><ItemTemplate><asp:Label ID="lblCrossDocking" runat="server" Text='<%# Bind("CrossDock") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price Unit" Visible="false"><ItemTemplate><asp:Label ID="lblPriceUnit" runat="server" Text='<%# Bind("PriceUnit") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Charges on Purchases" Visible="false"><ItemTemplate><asp:Label ID="lblChargesOnPurchases" runat="server" Text='<%# Bind("PurchMarkup") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Attribute-Based Pricing ID" Visible="false"><ItemTemplate><asp:Label ID="lblAttributeBasedPricingID" runat="server" Text='<%# Bind("PDSCalculationId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="New Fixed Asset" Visible="false"><ItemTemplate><asp:Label ID="lblNewFixedAsset" runat="server" Text='<%# Bind("CreateFixedAsset") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fixed Asset Group" Visible="false"><ItemTemplate><asp:Label ID="lblFixedAssetGroup" runat="server" Text='<%# Bind("AssetGroup") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fixed Asset Number" Visible="false"><ItemTemplate><asp:Label ID="lblFixedAssetNumber" runat="server" Text='<%# Bind("AssetId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Book" Visible="false"><ItemTemplate><asp:Label ID="lblBook" runat="server" Text='<%# Bind("AssetBookId") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transaction Type" Visible="false"><ItemTemplate><asp:Label ID="lblTransactionType" runat="server" Text='<%# Bind("AssetTransTypePurch") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Default Dimension" Visible="false"><ItemTemplate><asp:Label ID="lblDefaultDimension" runat="server" Text='<%# Bind("DefaultDimension") %>'></asp:Label></ItemTemplate></asp:TemplateField>

                                            <asp:TemplateField HeaderText="Business Unit" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblBusinessUnit" runat="server" Text='<%# Bind("BusinessUnit") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cost Center" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblCostCenter" runat="server" Text='<%# Bind("CostCenter") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlCostCenter" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("Department") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Group" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblItemGroup" runat="server" Text='<%# Bind("ItemGroup") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlItemGroup" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Main Account" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblMainAccount" runat="server" Text='<%# Bind("MainAccount") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlMainAccount" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Project" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblProject" runat="server" Text='<%# Bind("Project") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate><asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="InventDimId" Visible="false"><ItemTemplate><asp:Label ID="lblInventDimId" runat="server" Text='<%# Bind("InventDimId") %>' /></ItemTemplate></asp:TemplateField>
                                            <asp:TemplateField HeaderText="isStockedProduct" Visible="false"><ItemTemplate><asp:Label ID="lblisStockedProduct" runat="server" Text='<%# Bind("isStockedProduct") %>' /></ItemTemplate></asp:TemplateField>

                                            <asp:BoundField DataField="RecVersion"      HeaderText="RecVersion"      Visible="false" />
                                            <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" Visible="false" />
                                            <asp:BoundField DataField="ModifiedBy"       HeaderText="ModifiedBy"       Visible="false" />
                                            <asp:BoundField DataField="CreatedDateTime"  HeaderText="CreatedDateTime"  Visible="false" />
                                            <asp:BoundField DataField="CreatedBy"        HeaderText="CreatedBy"        Visible="false" />
                                            <asp:BoundField DataField="DataAreaId"       HeaderText="DataAreaId"       Visible="false" />
                                            <asp:BoundField DataField="Partition"        HeaderText="Partition"        Visible="false" />

                                            <%-- Update / Attachment / Edit buttons --%>
                                            <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                                                    <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="RecId" Visible="false">
                                                <ItemTemplate><asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' /></ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:HiddenField ID="EditMode" runat="server" />
                                </div><%-- /table-responsive --%>

                            </div>
                        </div>
                    </div><%-- /collapse gridPanel --%>

                    <%-- Lines detail collapse --%>
                    <a href="#detailPanel"
                       class="d365-toggle-header mt-3"
                       data-toggle="collapse" role="button"
                       aria-expanded="true" aria-controls="detailPanel">
                        <span class="section-title">Lines detail</span>
                        <i class="mdi mdi-chevron-down rotate-icon small-icon"></i>
                    </a>

                    <div class="collapse show mt-2" id="detailPanel">
                        <div class="card">
                            <div class="card-body p-0">

                                <%-- Detail sub-tabs --%>
                                <div class="PR-card-header bg-light px-3 pt-2">
                                    <ul class="nav nav-tabs border-0" id="detailTabs" role="tablist">
                                        <li class="nav-item"><a class="nav-link active" id="general-tab"          data-toggle="tab" href="#general"           role="tab">General</a></li>
                                        <li class="nav-item"><a class="nav-link"        id="setup-tab"            data-toggle="tab" href="#setup"             role="tab">Setup</a></li>
                                        <li class="nav-item"><a class="nav-link"        id="address-tab"          data-toggle="tab" href="#address"           role="tab">Address</a></li>
                                        <li class="nav-item"><a class="nav-link"        id="product-tab"          data-toggle="tab" href="#product"           role="tab">Product</a></li>
                                        <li class="nav-item"><a class="nav-link"        id="delivery-tab"         data-toggle="tab" href="#delivery"          role="tab">Delivery</a></li>
                                        <li class="nav-item"><a class="nav-link"        id="picking-tab"          data-toggle="tab" href="#picking"           role="tab">Picking</a></li>
                                        <li class="nav-item"><a class="nav-link"        id="fixedassets-tab"      data-toggle="tab" href="#fixed-assets"      role="tab">Fixed assets</a></li>
                                        <li class="nav-item"><a class="nav-link"        id="financialdimensions-tab" data-toggle="tab" href="#financialdimensions" role="tab">Financial dimensions</a></li>
                                    </ul>
                                </div>

                                <div class="tab-content p-3">

                                    <%-- ── GENERAL TAB ── --%>
                                    <div class="tab-pane fade show active" id="general" role="tabpanel">
                                        <div class="row">
                                            <%-- REQUEST FOR QUOTATION + ORDER LINE --%>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Request for Quotation</div>
                                                <div class="info-block"><strong>RFQ number</strong><asp:TextBox ID="txtRFQNumber" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>RFQ reply number</strong><asp:TextBox ID="txtRFQReplyNumber" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>RFQ line number
                                                                        </strong><asp:TextBox ID="txtRFQLineNumber" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="custom-section-title mt-3">Order Line</div>
                                                <div class="info-block"><strong>Procurement category</strong><asp:TextBox ID="txtProcurementCategory" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>

                                            <%-- ORDER LINE + PURCHASE REQUISITION --%>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Order Line</div>
                                                <div class="info-block"><strong>Product name</strong><asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Text</strong><asp:TextBox ID="txtitemdescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" ReadOnly="true" /></div>
                                                <div class="custom-section-title mt-3">Purchase Requisition</div>
                                                <div class="info-block"><strong>Purchase requisition</strong><asp:TextBox ID="txtPurchaseRequisition" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>

                                            <%-- PURCHASE REQUISITION + INTERCOMPANY --%>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Purchase Requisition</div>
                                                <div class="info-block"><strong>Requisition product name</strong><asp:TextBox ID="txtRequisitionProductName" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Supplier part auxiliary ID</strong><asp:TextBox ID="txtSupplierAuxId" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="custom-section-title mt-3">Intercompany</div>
                                                <div class="info-block"><strong>Origin (intercompany orders)</strong><asp:TextBox ID="txtIntercompanyOrigin" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>

                                            <%-- REFERENCE + DELIVERY REFERENCE --%>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Reference</div>
                                                <div class="info-block"><strong>External</strong><asp:TextBox ID="txtReferenceExternal" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Origin</strong><asp:TextBox ID="txtReferenceOrigin" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="custom-section-title mt-3">Delivery Reference</div>
                                                <div class="info-block"><strong>Customer requisition</strong><asp:TextBox ID="txtDeliveryCustomerRequisition" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Customer reference</strong><asp:TextBox ID="txtDeliveryCustomerReference" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>

                                            <%-- STATUS --%>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Status</div>
                                                <div class="info-block"><strong>Line status</strong><asp:TextBox ID="txtLineStatus" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block">
                                                    <strong>Stopped</strong>
                                                    <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkStopped" ReadOnly="true" runat="server" /><span class="slider"></span></label></div>
                                                </div>
                                                <div class="info-block">
                                                    <strong>Prevent partial delivery</strong>
                                                    <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkPreventPartial" runat="server" ReadOnly="true" /><span class="slider"></span></label></div>
                                                </div>
                                                <div class="info-block"><strong>State</strong><asp:TextBox ID="txtState" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>

                                            <%-- QUALITY / FINALIZED --%>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Quality order status</strong><asp:TextBox ID="txtQualityOrderStatus" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block">
                                                    <strong>Finalized</strong>
                                                    <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkFinalized" runat="server" ReadOnly="true" /><span class="slider"></span></label></div>
                                                </div>
                                                <div class="info-block">
                                                    <strong>Added by POS receiving</strong>
                                                    <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkAddedByPOS" runat="server" ReadOnly="true" /><span class="slider"></span></label></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- ── SETUP TAB ── --%>
                                    <div class="tab-pane fade" id="setup" role="tabpanel">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Inventory</div>
                                                <div class="info-block"><strong>Lot ID</strong><asp:TextBox ID="setupLotId" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="custom-section-title mt-3">Invoice Matching</div>
                                                <div class="info-block"><strong>Matching policy</strong><asp:Label ID="lblMatchingPolicy" runat="server" CssClass="form-control" style="display:block;" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Returned Order</div>
                                                <div class="info-block"><strong>Return action</strong><asp:Label ID="lblReturnAction" runat="server" CssClass="form-control" style="display:block;" /></div>
                                                <div class="info-block">
                                                    <strong>Scrap</strong>
                                                    <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkScrap" runat="server" ReadOnly="true" /><span class="slider"></span></label></div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Sales Tax</div>
                                                <div class="info-block"><strong>Item sales group</strong><asp:DropDownList ID="ddlItemSalestaxGrouplineDetail" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>Sales tax group</strong><asp:DropDownList ID="ddlSalesTaxGrouplineDetail" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>1099 amount</strong><asp:TextBox ID="txt1099Amount" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>State / Province</strong><asp:Label ID="lblStateProvince" runat="server" CssClass="form-control" style="display:block;" /></div>
                                                <div class="info-block"><strong>1099 State Amount</strong><asp:TextBox ID="txt1099StateAmount" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>1099 Box</strong><asp:Label ID="lbl1099Box" runat="server" CssClass="form-control" style="display:block;" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Posting</div>
                                                <div class="info-block"><strong>Ledger account</strong><asp:TextBox ID="txtLedgerAccount" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="custom-section-title mt-3">Date and Time</div>
                                                <div class="info-block"><strong>Created date and time</strong><asp:Label ID="lblCreatedDateTime" runat="server" CssClass="form-control" style="display:block;" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Inventory</div>
                                                <div class="info-block"><strong>Inventory quantity</strong><asp:TextBox ID="txtInventoryQuantity" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Inventory remainder</strong><asp:TextBox ID="txtInventoryRemainder" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="custom-section-title mt-3">Delivery</div>
                                                <div class="info-block"><strong>Confirmed receipt date</strong><asp:TextBox ID="txtConfirmedReceiptDate" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Delivery type</strong><asp:TextBox ID="txtDeliveryType" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- ── ADDRESS TAB ── --%>
                                    <div class="tab-pane fade" id="address" role="tabpanel">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Delivery Address</div>
                                                <div class="info-block"><strong>Name</strong><asp:TextBox ID="txtDeliveryName" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Delivery address</strong><asp:TextBox ID="txtDeliveryAddress" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Address</div>
                                                <div class="info-block"><asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Service Address</div>
                                                <div class="info-block"><strong>Service address</strong><asp:TextBox ID="txtServiceAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Service Reference</div>
                                                <div class="info-block"><strong>Customer requisition</strong><asp:TextBox ID="txtCustomerRequisition" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Customer reference</strong><asp:TextBox ID="txtCustomerReference" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Attention Information</div>
                                                <div class="info-block"><strong>Attention information</strong><asp:TextBox ID="txtAttentionInformation" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Requester</strong><asp:TextBox ID="txtRequester" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- ── PRODUCT TAB ── --%>
                                    <div class="tab-pane fade" id="product" role="tabpanel">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Product Dimensions</div>
                                                <div class="info-block"><strong>Configuration</strong><asp:DropDownList ID="ddlConfigurationLineDetail" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>Size</strong><asp:DropDownList ID="ddlSizeLineDetail" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>Color</strong><asp:DropDownList ID="ddlColorLineDetail" runat="server" CssClass="form-control" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Style</strong><asp:DropDownList ID="ddlStyleLineDetail" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>Version</strong><asp:TextBox ID="txtVersion" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="custom-section-title mt-3">Tracking Dimensions</div>
                                                <div class="info-block"><strong>Batch number</strong><asp:DropDownList ID="ddlBatchNumberLineDetail" runat="server" CssClass="form-control" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Serial number</strong><asp:DropDownList ID="ddlSerialNumberLineDetail" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>Owner</strong><asp:TextBox ID="txtOwner" runat="server" CssClass="form-control" /></div>
                                                <div class="custom-section-title mt-3">Storage Dimensions</div>
                                                <div class="info-block"><strong>Site</strong><asp:DropDownList ID="ddlSiteLineDetail" OnTextChanged="ddlSiteLineDetailProductTab_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Warehouse</strong><asp:DropDownList ID="ddlWarehouseLineDetail" OnTextChanged="ddlWarehouseLineDetail_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>Location</strong><asp:DropDownList ID="ddlWmsLocationLineDetail" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>License plate</strong><asp:TextBox ID="txtLicensePlate" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>Inventory status</strong><asp:TextBox ID="txtInventoryStatus" runat="server" CssClass="form-control" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Planned-Order Reference</div>
                                                <div class="info-block"><strong>Number</strong><asp:TextBox ID="txtPlannedOrderNumber" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>Master plan</strong><asp:TextBox ID="txtMasterPlan" runat="server" CssClass="form-control" /></div>
                                                <div class="custom-section-title mt-3">Item Reference</div>
                                                <div class="info-block"><strong>Reference type</strong><asp:TextBox ID="txtReferenceType" runat="server" CssClass="form-control" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Reference number</strong><asp:TextBox ID="txtReferenceNumber" runat="server" CssClass="form-control" /></div>
                                                <div class="info-block"><strong>Reference lot</strong><asp:TextBox ID="txtReferenceLot" runat="server" CssClass="form-control" /></div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- ── DELIVERY TAB ── --%>
                                    <div class="tab-pane fade" id="delivery" role="tabpanel">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Delivery Date</div>
                                                <div class="info-block"><strong>Requested receipt date</strong><asp:TextBox ID="txtdeliveryrequestedreceiptdate" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Confirmed receipt date</strong><asp:TextBox ID="txtdeliveryconfirmedreceiptdate" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Planning priority</strong><asp:TextBox ID="txtPlanningPriority" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Delivery</div>
                                                <div class="info-block"><strong>Overdelivery</strong><asp:TextBox ID="txtOverdelivery" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Modes of delivery</strong><asp:TextBox ID="txtModesOfDelivery" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Delivery terms</strong><asp:TextBox ID="txtDeliveryTerms" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Underdelivery</strong><asp:TextBox ID="txtUnderdelivery" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Delivery type</strong><asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Direct Delivery</div>
                                                <div class="info-block"><strong>Direct delivery status</strong><asp:TextBox ID="txtDirectDeliveryStatus" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Comments</strong><asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" ReadOnly="true" /></div>
                                                <div class="info-block">
                                                    <strong>Direct delivery</strong>
                                                    <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkDirectDelivery" runat="server" ReadOnly="true" /><span class="slider"></span></label></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- ── PICKING TAB ── --%>
                                    <div class="tab-pane fade" id="picking" role="tabpanel">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Bar Code</div>
                                                <div class="info-block"><strong>Bar code</strong><asp:TextBox ID="txtBarCode" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Bar code setup</strong><asp:TextBox ID="txtBarCodeSetup" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Warehouse</div>
                                                <div class="info-block">
                                                    <strong>Cross docking</strong>
                                                    <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkCrossDocking" runat="server" ReadOnly="true" /><span class="slider"></span></label></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- ── FIXED ASSETS TAB ── --%>
                                    <div class="tab-pane fade" id="fixed-assets" role="tabpanel">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="custom-section-title">Fixed Assets</div>
                                                <div class="info-block">
                                                    <strong>New fixed asset?</strong>
                                                    <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkNewFixedAsset" runat="server" ReadOnly="true" /><span class="slider"></span></label></div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Fixed asset group</strong><asp:TextBox ID="txtFixedAssetGroup" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Fixed asset number</strong><asp:TextBox ID="txtFixedAssetNumber" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="custom-section-title">&nbsp;</div>
                                                <div class="info-block"><strong>Book</strong><asp:TextBox ID="txtBook" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                                <div class="info-block"><strong>Transaction type</strong><asp:TextBox ID="txtTransactionType" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- ── FINANCIAL DIMENSIONS TAB ── --%>
                                    <div class="tab-pane fade" id="financialdimensions" role="tabpanel">
                                        <div class="custom-section-title mb-2">Financial Dimensions</div>
                                        <div runat="server" id="financialDimensionsContainer"></div>
                                    </div>

                                </div><%-- /tab-content --%>
                            </div>
                        </div>
                    </div><%-- /collapse detailPanel --%>

                </div><%-- /linesContent tab-pane --%>

                <%-- ══════════════════════════════════════════
                     HEADER TAB
                     ══════════════════════════════════════════ --%>
                <div class="tab-pane fade" id="headerContent" role="tabpanel">
                    <div class="accordion mt-3" id="headerAccordion">

                        <%-- ── General panel ── --%>
                        <div class="card">
                            <div class="card-header" id="headingGeneral">
                                <a class="text-dark d-flex justify-content-between w-100" data-toggle="collapse" href="#collapseGeneral" role="button" aria-expanded="true">
                                    <strong>General</strong>
                                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                                </a>
                            </div>
                            <div id="collapseGeneral" class="collapse show">
                                <div class="card-body">
                                    <div class="row">
                                        <%-- Column 1 --%>
                                        <div class="col-md-3">
                                            <div class="custom-section-title">Purchase Order</div>
                                            <div class="info-block"><strong>Purchase Order</strong><asp:TextBox ID="txtPurchaseOrder" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div class="info-block"><strong>Vendor Name</strong><asp:TextBox ID="txtPurchName" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div class="info-block"><strong>Purchase Type</strong><asp:TextBox ID="txtPurchaseType" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div class="custom-section-title mt-3">Vendor</div>
                                            <div class="info-block">
                                                <strong>One-time Supplier</strong>
                                                <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkOneTimeSupplier" runat="server" /><span class="slider"></span></label></div>
                                            </div>
                                            <div class="info-block"><strong>Vendor Account</strong><asp:DropDownList ID="ddlVenderAccount" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlVenderAccount_SelectedIndexChanged" CssClass="form-control" /></div>
                                            <div class="info-block"><strong>Invoice Account</strong><asp:DropDownList ID="ddlInvoiceAccount" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlVenderAccount_SelectedIndexChanged" CssClass="form-control" /></div>
                                            <div style="display:none;"><asp:DropDownList ID="ddlContactID" runat="server" CssClass="form-control" Visible="false" /></div>
                                        </div>

                                        <%-- Column 2 --%>
                                        <div class="col-md-3">
                                            <div class="custom-section-title">Contact Information</div>
                                            <div class="info-block"><strong>Internet Address</strong><asp:TextBox ID="txtInternetAddress" runat="server" CssClass="form-control" /></div>
                                            <div class="info-block"><strong>Email</strong><asp:DropDownList ID="ddlEmail" runat="server" CssClass="form-control" /></div>
                                            <div class="custom-section-title mt-3">Status</div>
                                            <div class="info-block"><strong>Purchase Order Status</strong><asp:TextBox ID="txtPurchaseOrderStatus" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div class="info-block"><strong>Document Status</strong><asp:TextBox ID="txtDocumentStatus" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div class="info-block"><strong>Approval Status</strong><asp:TextBox ID="txtApprovalStatus" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div class="info-block"><strong>Header Budget Check Results</strong><asp:TextBox ID="txtHeaderBudgetCheck" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                        </div>

                                        <%-- Column 3 --%>
                                        <div class="col-md-3">
                                            <div class="info-block"><strong>Quality Order Status</strong><asp:TextBox ID="txtQualityOrderStatuses" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div class="info-block">
                                                <strong>Direct Delivery</strong>
                                                <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="CheckBox1" runat="server" Visible="false" /><span class="slider"></span></label></div>
                                            </div>
                                            <div class="custom-section-title mt-3">Storage Dimensions</div>
                                            <div class="info-block"><strong>Site</strong><asp:DropDownList ID="ddlSite" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSiteLineDetail_SelectedIndexChanged" /></div>
                                            <div class="info-block"><strong>Warehouse</strong><asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlWarehouseLineDetailHeaderTab_SelectedIndexChanged" /></div>
                                            <div class="custom-section-title mt-3">Reason</div>
                                            <div class="info-block"><strong>Reason</strong><asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control" /></div>
                                            <div class="info-block"><strong>Reason Comment</strong><asp:TextBox ID="txtReasonComment" runat="server" CssClass="form-control" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtCustomerReferences" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" /></div>
                                        </div>

                                        <%-- Column 4 (hidden fields) --%>
                                        <div class="col-md-3">
                                            <div style="display:none;"><asp:TextBox ID="txtRMANumber" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div style="display:none;"><asp:TextBox ID="TxtOrigins" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtCustomerRequisitions" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtCustomerAccount" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" /></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- ── Setup panel ── --%>
                        <div class="card">
                            <div class="card-header" id="headingSetup">
                                <a class="text-dark d-flex justify-content-between w-100" data-toggle="collapse" href="#collapseSetup" role="button" aria-expanded="false">
                                    <strong>Setup</strong>
                                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                                </a>
                            </div>
                            <div id="collapseSetup" class="collapse">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="custom-section-title">Sales Tax</div>
                                            <div class="info-block"><strong>Sales Tax Group</strong><asp:DropDownList ID="ddlSaleTaxGroup" runat="server" CssClass="form-control" /></div>
                                            <div class="info-block"><strong>Tax Exempt Number</strong><asp:TextBox ID="txtTaxExemptNumber" runat="server" CssClass="form-control" /></div>
                                            <div class="info-block">
                                                <strong>Prices Include Sales Tax</strong>
                                                <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkPricesIncludeSalesTax" runat="server" /><span class="slider"></span></label></div>
                                            </div>
                                            <div class="custom-section-title mt-3">Posting</div>
                                            <div class="info-block"><strong>Posting Profile</strong><asp:DropDownList ID="ddlPostingProfile" runat="server" CssClass="form-control" /></div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="info-block"><strong>Number Sequence Group</strong><asp:DropDownList ID="ddlNumberSequenceGroup" runat="server" CssClass="form-control" /></div>
                                            <div class="custom-section-title mt-3">Administration</div>
                                            <div class="info-block"><strong>Buyer Group</strong><asp:DropDownList ID="ddlBuyerGroup" runat="server" CssClass="form-control" /></div>
                                            <div class="custom-section-title mt-3">Other Setup</div>
                                            <div class="info-block"><asp:DropDownList ID="ddlheaderRequester" runat="server" CssClass="form-control" Visible="false" /></div>
                                            <div class="info-block"><strong>Pool</strong><asp:DropDownList ID="ddlPool" runat="server" CssClass="form-control" /></div>
                                            <div class="info-block"><strong>Language</strong><asp:DropDownList ID="ddlLanguageId" runat="server" CssClass="form-control" /></div>
                                            <div style="display:none;"><asp:DropDownList ID="ddlOrderer" runat="server" CssClass="form-control" Visible="false" /></div>
                                            <div class="info-block mt-2">
                                                <strong>Activate Change Management</strong>
                                                <div class="mt-1"><label class="toggle-switch"><asp:CheckBox ID="chkActivateChangeManagement" runat="server" /><span class="slider"></span></label></div>
                                            </div>
                                            <div style="display:none;">
                                                <label class="toggle-switch"><asp:CheckBox ID="chkSendPurchaseOrderViaCXML" runat="server" Visible="false" /><span class="slider"></span></label>
                                            </div>
                                        </div>
                                        <%-- hidden reference columns --%>
                                        <div class="col-md-3" style="display:none;">
                                            <asp:TextBox ID="txtProjectID" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                            <asp:TextBox ID="txtOriginIntercompany" runat="server" CssClass="form-control" Visible="false" />
                                            <asp:TextBox ID="txtCreatedDateAndTime" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                            <asp:TextBox ID="txtConfirmingPO" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                            <asp:TextBox ID="txtSettlementType" runat="server" CssClass="form-control" Visible="false" />
                                            <asp:TextBox ID="txtAccountingDate" runat="server" CssClass="form-control" Visible="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- ── Address panel ── --%>
                        <div class="card">
                            <div class="card-header" id="headingAddress">
                                <a class="text-dark d-flex justify-content-between w-100" data-toggle="collapse" href="#collapseAddress" role="button" aria-expanded="false">
                                    <strong>Address</strong>
                                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                                </a>
                            </div>
                            <div id="collapseAddress" class="collapse">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="info-block"><strong>Delivery Name</strong><asp:TextBox ID="txtDeliveryNames" runat="server" CssClass="form-control" /></div>
                                            <div class="custom-section-title mt-3">Attention</div>
                                            <div class="info-block"><strong>Attention Information</strong><asp:TextBox ID="txtAttentionInformations" runat="server" CssClass="form-control" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtDeliveryAddresss" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" /></div>
                                        </div>
                                        <div class="col-md-4" style="display:none;">
                                            <asp:TextBox ID="txtAddresss" runat="server" CssClass="form-control" Visible="false" />
                                        </div>
                                        <div class="col-md-4" style="display:none;">
                                            <asp:TextBox ID="txtServiceAddresss" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- ── Delivery panel ── --%>
                        <div class="card">
                            <div class="card-header" id="headingDelivery">
                                <a class="text-dark d-flex justify-content-between w-100" data-toggle="collapse" href="#collapseDelivery" role="button" aria-expanded="false">
                                    <strong>Delivery</strong>
                                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                                </a>
                            </div>
                            <div id="collapseDelivery" class="collapse">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="custom-section-title">Delivery</div>
                                            <div class="info-block"><strong>Requested Receipt Date</strong><asp:TextBox ID="txtRequestedReceiptDates" runat="server" CssClass="form-control" /></div>
                                            <div class="info-block"><strong>Mode of Delivery</strong><asp:DropDownList ID="ddlModeOfDelivery" runat="server" CssClass="form-control" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtConfirmedReceiptDates" runat="server" CssClass="form-control" Visible="false" /></div>
                                            <div style="display:none;"><asp:TexTBox ID="txtEarliestConfirmedReceiptDates" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" /></div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="custom-section-title">&nbsp;</div>
                                            <div class="info-block"><strong>Delivery Terms</strong><asp:DropDownList ID="ddlDeliveryTerm" runat="server" CssClass="form-control" /></div>
                                            <div class="info-block"><strong>Mode</strong><asp:TextBox ID="txtMode" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtUPSZone" runat="server" CssClass="form-control" Visible="false" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtCallTagType" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" /></div>
                                        </div>
                                        <div class="col-md-3" style="display:none;">
                                            <asp:DropDownList ID="ddlShippingCarrier" runat="server" CssClass="form-control" Visible="false" />
                                            <asp:TextBox ID="txtCarrierService" runat="server" CssClass="form-control" Visible="false" />
                                            <asp:TextBox ID="txtCarrierGroup" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                        </div>
                                        <div class="col-md-3" style="display:none;">
                                            <asp:TextBox ID="txtTransportationTemplateID" runat="server" CssClass="form-control" Visible="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- ── Price and Discount panel ── --%>
                        <div class="card">
                            <div class="card-header" id="headingPriceandDiscount">
                                <a class="text-dark d-flex justify-content-between w-100" data-toggle="collapse" href="#collapsePriceandDiscount" role="button" aria-expanded="false">
                                    <strong>Price and Discount</strong>
                                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                                </a>
                            </div>
                            <div id="collapsePriceandDiscount" class="collapse">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="custom-section-title">Currency</div>
                                            <div class="info-block"><strong>Currency</strong><asp:TextBox ID="txtCurrency" runat="server" CssClass="form-control" ReadOnly="true" /></div>
                                            <div class="custom-section-title mt-3">Payment</div>
                                            <div class="info-block"><strong>Terms of Payment</strong><asp:DropDownList ID="ddlTermsOfPayment" runat="server" CssClass="form-control" /></div>
                                            <div style="display:none;"><asp:TextBox ID="DueDate" runat="server" CssClass="form-control" Visible="false" /></div>
                                            <div class="info-block"><strong>Method of Payment</strong><asp:DropDownList ID="ddlMethodOfPayment" runat="server" CssClass="form-control" /></div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="info-block"><strong>Payment Schedule</strong><asp:DropDownList ID="ddlPaymentSchedule" runat="server" CssClass="form-control" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtPaymentSpecification" runat="server" CssClass="form-control" Visible="false" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtCashDiscount" runat="server" CssClass="form-control" Visible="false" /></div>
                                            <div style="display:none;"><asp:TextBox ID="txtDiscountPercentage" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" /></div>
                                        </div>
                                        <%-- remaining hidden price columns --%>
                                        <div class="col-md-3" style="display:none;">
                                            <asp:TextBox ID="txtPriceGroup" runat="server" CssClass="form-control" Visible="false" />
                                            <asp:TextBox ID="txtLineDiscountGroup" runat="server" CssClass="form-control" Visible="false" />
                                            <asp:TextBox ID="txtMultilineDiscGroup" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                            <asp:TextBox ID="txtTotalDiscountGroup" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                        </div>
                                        <div class="col-md-3" style="display:none;">
                                            <asp:TextBox ID="txtTotalDiscountPercent" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                            <asp:TextBox ID="txtChargesGroup" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                            <asp:TextBox ID="txtVendorRebateGroup" runat="server" CssClass="form-control" Visible="false" />
                                            <asp:TextBox ID="txtRebateReference" runat="server" CssClass="form-control" ReadOnly="true" Visible="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- ── Financial Dimensions panel ── --%>
                        <div class="card">
                            <div class="card-header" id="headingFinancialDimension">
                                <a class="text-dark d-flex justify-content-between w-100" data-toggle="collapse" href="#collapseFinancialDimension" role="button" aria-expanded="false">
                                    <strong>Financial Dimensions</strong>
                                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                                </a>
                            </div>
                            <div id="collapseFinancialDimension" class="collapse">
                                <div class="card-body">
                                    <div class="custom-section-title mb-2">Vendor Financial Dimensions</div>
                                    <div runat="server" id="VendorFinancialDimensionContainer"></div>
                                </div>
                            </div>
                        </div>

                    </div><%-- /accordion --%>
                </div><%-- /headerContent tab-pane --%>

            </div><%-- /tab-content (main) --%>

        </ContentTemplate>
    </asp:UpdatePanel>

    <%-- Scripts --%>
    <script>
        /* ── Number formatting ── */
        function formatLabelNumbers() {
            document.querySelectorAll('.format-number').forEach(function (label) {
                var raw = label.textContent.replace(/,/g, '').trim();
                if (!isNaN(raw) && raw !== '') {
                    label.textContent = parseFloat(raw).toLocaleString(undefined, {
                        minimumFractionDigits: 2, maximumFractionDigits: 2
                    });
                }
            });
        }
        window.addEventListener('load', formatLabelNumbers);
        if (typeof Sys !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(formatLabelNumbers);
        }

        /* ── Custom dropdown toggle ── */
        function toggleDropdown(menuId) {
            document.querySelectorAll('.custom-dropdown').forEach(function (d) {
                if (d.id !== menuId) d.style.display = 'none';
            });
            var menu = document.getElementById(menuId);
            if (!menu) return;
            var first = menu.firstElementChild;
            if (first && first.classList.contains('aspNetDisabled')) return;
            menu.style.display = (menu.style.display === 'block') ? 'none' : 'block';
        }

        /* Close dropdowns on outside click */
        document.addEventListener('click', function (e) {
            if (!e.target.closest('.dropdown')) {
                document.querySelectorAll('.custom-dropdown').forEach(function (d) {
                    d.style.display = 'none';
                });
            }
        });

        /* ── Workflow dropdown ── */
        function toggleWorkflowDropdown() {
            var menu = document.getElementById('workflowMenu');
            menu.style.display = (menu.style.display === 'block') ? 'none' : 'block';
        }

        /* ── Page methods ── */
        function fetchVendorName(dropdown) {
            var vendAccount = dropdown.value;
            var row = dropdown.closest("tr");
            var txtBox = row ? row.querySelector("input[id*='txtVendorName']") : null;
            if (!vendAccount || !txtBox) return;
            PageMethods.GetVendorName(vendAccount, function (result) { txtBox.value = result; });
        }

        function fetchProductName(dropdown) {
            var itemId = dropdown.value;
            var txtProductName = document.getElementById("txtProductName");
            if (!itemId || !txtProductName) return;
            PageMethods.GetProductName(itemId, function (result) { txtProductName.value = result; });
        }
    </script>
</asp:Content>
