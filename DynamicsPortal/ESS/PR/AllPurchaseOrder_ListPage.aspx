<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllPurchaseOrder_ListPage.aspx.cs" Inherits="DynamicsPortal.AllPurchaseOrder_ListPage" %>

    <asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
        <!-- These Libraries controls the behaviour of header dropdowns.  -->
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
        <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />

        <script type="text/javascript">
            // Infinite scroll state variables
            var isLoading = false;
            var lastLoadTime = 0;
            var savedScrollTop = 0;

            // Highlight row fade-out effect using jQuery (Restored)
            $(document).ready(function () {
                var $row = $(".highlight-row");
                if ($row.length) {
                    setTimeout(function () {
                        $row.addClass("fade-out");
                    }, 1000);
                }
            });

            // Bind infinite scroll & AJAX events when DOM is ready
            document.addEventListener('DOMContentLoaded', function () {
                console.log("D365 Infinite Scroll: Vanilla JS loaded & ready.");

                // Suppress global loader overlay on dropdown toggle clicks (event delegation)
                document.addEventListener('click', function (event) {
                    var toggle = event.target.closest('.dropdown-toggle');
                    if (toggle) {
                        console.log("D365 Infinite Scroll: Suppressing overlay for dropdown toggle click.");
                        window.suppressOverlay = true;
                    }
                });

                // Initial scroll attachment
                bindScrollEvent();
                checkViewportFilled();

                // Register ASP.NET AJAX request listeners
                if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    var prm = Sys.WebForms.PageRequestManager.getInstance();

                    prm.add_beginRequest(function (sender, args) {
                        var postbackElement = args.get_postBackElement();
                        var isLoadMore = postbackElement && postbackElement.id && postbackElement.id.indexOf('btnLoadMore') > -1;
                        console.log("D365 Infinite Scroll: beginRequest. isLoadMore = " + isLoadMore);

                        if (isLoadMore) {
                            window.suppressOverlay = true;
                            isLoading = true;

                            var shimmer = document.getElementById('shimmer-loader');
                            if (shimmer) shimmer.style.display = 'block';
                        }

                        // Capture current scroll position
                        var container = document.querySelector('.page-placeholder');
                        if (container) {
                            savedScrollTop = container.scrollTop;
                        } else {
                            savedScrollTop = window.pageYOffset || document.documentElement.scrollTop;
                        }
                        console.log("D365 Infinite Scroll: Saved scroll position = " + savedScrollTop);
                    });

                    prm.add_endRequest(function (sender, args) {
                        console.log("D365 Infinite Scroll: endRequest fired.");

                        // Immediately restore scroll position to avoid screen jumping
                        var container = document.querySelector('.page-placeholder');
                        if (container) {
                            container.scrollTop = savedScrollTop;
                        } else {
                            window.scrollTo(0, savedScrollTop);
                        }
                        console.log("D365 Infinite Scroll: Restored scroll position = " + savedScrollTop);

                        // Allow DOM rendering and browser repaint to settle
                        setTimeout(function () {
                            isLoading = false;

                            var shimmer = document.getElementById('shimmer-loader');
                            if (shimmer) shimmer.style.display = 'none';

                            window.suppressOverlay = false;

                            var hdnHasMore = document.getElementById('<%= hdnHasMore.ClientID %>');
                            var hasMoreVal = hdnHasMore ? hdnHasMore.value : 'unknown';
                            console.log("D365 Infinite Scroll: State reset. hdnHasMore = " + hasMoreVal);

                            bindScrollEvent();
                            checkViewportFilled();
                        }, 150);
                    });
                }
            });

            function checkViewportFilled() {
                var container = document.querySelector('.page-placeholder');
                if (!container) return;

                // Stop automatic loading if client-side column filters are active
                if (document.querySelector('.lp-col-dot')) {
                    console.log("D365 Infinite Scroll: Filter is active (.lp-col-dot found). Skipping viewport check.");
                    return;
                }

                var hdnHasMore = document.getElementById('<%= hdnHasMore.ClientID %>');
                var hasMoreVal = hdnHasMore ? hdnHasMore.value : 'false';

                if (hasMoreVal === 'false' || isLoading) return;

                // Check if current content height is less than visible viewport height
                if (container.scrollHeight > 0 && container.scrollHeight <= container.clientHeight) {
                    console.log("D365 Infinite Scroll: Viewport not filled. Auto-loading next page...");
                    setTimeout(function () {
                        loadMoreRecords();
                    }, 300);
                }
            }

            function bindScrollEvent() {
                var container = document.querySelector('.page-placeholder') || window;

                // Remove existing scroll listener to avoid duplicates
                container.removeEventListener('scroll', handleScrollDebounced);
                container.addEventListener('scroll', handleScrollDebounced);
            }

            function handleScrollDebounced() {
                var container = document.querySelector('.page-placeholder');
                if (!container) return;

                // Stop scrolling actions if client-side column filters are active
                if (document.querySelector('.lp-col-dot')) {
                    console.log("D365 Infinite Scroll: Filter is active (.lp-col-dot found). Skipping scroll trigger.");
                    return;
                }

                var prm = (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) ? Sys.WebForms.PageRequestManager.getInstance() : null;
                var inAsync = prm && prm.get_isInAsyncPostBack && prm.get_isInAsyncPostBack();

                var hdnHasMore = document.getElementById('<%= hdnHasMore.ClientID %>');
                var hasMoreVal = hdnHasMore ? hdnHasMore.value : 'false';

                if (isLoading || inAsync || hasMoreVal === 'false') {
                    return;
                }

                // Check if scrolled near the bottom (threshold of 80px)
                // We must verify scrollThreshold > 0 to avoid false triggers on horizontal scrolling
                // when no vertical scrollbar is present or when vertical scrolling is not possible.
                var currentScroll = container.scrollTop;
                var scrollThreshold = container.scrollHeight - container.clientHeight - 80;

                if (scrollThreshold > 0 && currentScroll >= scrollThreshold) {
                    console.log("D365 Infinite Scroll: User scrolled near bottom. Loading more...");
                    loadMoreRecords();
                }
            }

            function loadMoreRecords() {
                var now = new Date().getTime();
                if (now - lastLoadTime < 1500) {
                    console.log("D365 Infinite Scroll: Throttle active (1.5s lock).");
                    return;
                }
                lastLoadTime = now;
                isLoading = true;

                window.suppressOverlay = true;

                var shimmer = document.getElementById('shimmer-loader');
                if (shimmer) shimmer.style.display = 'block';

                console.log("D365 Infinite Scroll: Executing async postback via __doPostBack...");

                // Native ASP.NET AJAX call to trigger UpdatePanel partial postback cleanly
                __doPostBack('<%= btnLoadMore.UniqueID %>', '');
            }
        </script>
        <style>
            /* ── Dropdown section headers ── */
            .dropdown-section-header {
                display: block;
                font-weight: 700;
                font-size: 0.7rem;
                text-transform: uppercase;
                letter-spacing: 0.08em;
                color: #6c757d;
                padding: 0.45rem 1rem 0.2rem;
                pointer-events: none;
                user-select: none;
            }

            .hidden-column {
                display: none;
            }

            .selected-row {
                background-color: #e8f4fd !important;
                /* Light blue highlight */
                border-left: 4px solid #007bff;
            }

            /* ── Tighten dropdown items ── */
            .dropdown-menu .dropdown-item {
                font-size: 0.875rem;
                padding: 0.35rem 1.25rem;
            }

            /* ── Divider spacing ── */
            .dropdown-menu .dropdown-divider {
                margin: 0.25rem 0;
            }

            /* Shimmer Loader Styling */
            .shimmer-loader {
                display: none;
                padding: 15px;
                background: #fff;
                border: 1px solid #e0e0e0;
                border-top: none;
                border-radius: 0 0 4px 4px;
            }

            .shimmer-row {
                display: flex;
                gap: 15px;
                padding: 10px 0;
                border-bottom: 1px solid #f8f9fa;
            }

            .shimmer-cell {
                height: 20px;
                background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
                background-size: 200% 100%;
                animation: loading-shimmer 1.5s infinite;
                border-radius: 4px;
            }

            .shimmer-cell.chk {
                width: 40px;
            }

            .shimmer-cell.po {
                width: 120px;
            }

            .shimmer-cell.vendor {
                width: 100px;
            }

            .shimmer-cell.name {
                flex: 1;
            }

            .shimmer-cell.status {
                width: 90px;
            }

            @keyframes loading-shimmer {
                0% {
                    background-position: 200% 0;
                }

                100% {
                    background-position: -200% 0;
                }
            }
        </style>
    </asp:Content>

    <%-- ═══════════════════════════════════════════════════════════════ ACTION PANEL – grouped menus
        ═══════════════════════════════════════════════════════════════ --%>
        <asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="updButtons" runat="server">
                <ContentTemplate>

                    <%-- ── New ── --%>
                        <div class="action-items">
                            <asp:LinkButton ID="btnNew" runat="server"
                                OnClientClick="javascript: return openPopupPanel('/ESS/PR/PurchaseOrderHeader_Create.aspx', 700)">
                                <i class="mdi mdi-plus"></i> New
                            </asp:LinkButton>
                        </div>

                        <%-- ── Delete ── --%>
                            <div class="action-items">
                                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click">
                                    <i class="mdi mdi-delete"></i> Delete
                                </asp:LinkButton>
                            </div>

                            <%-- ── Cancel ── --%>
                            <div class="action-items">
                                <asp:LinkButton ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Enabled="false">
                                    <i class="mdi mdi-close-circle-outline"></i> Cancel
                                </asp:LinkButton>
                            </div>

                            <%-- ════════════════════════════════════ MENU: Workflow
                                ════════════════════════════════════ --%>
                                <div class="action-items dropdown">
                                    <asp:LinkButton ID="btnWorkflow" runat="server" CssClass="dropdown-toggle"
                                        data-bs-toggle="dropdown" aria-expanded="false">
                                        <i class="mdi mdi-sitemap"></i> Workflow
                                    </asp:LinkButton>

                                    <ul class="dropdown-menu">
                                        <li>
                                            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="dropdown-item"
                                                OnClick="btnSubmit_Click">
                                                <i class="mdi mdi-shape-plus"></i> Submit
                                            </asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>

                                <%-- ════════════════════════════════════ MENU: Purchase Order Sections: Maintain | View
                                    ════════════════════════════════════ --%>
                                    <div class="action-items dropdown">
                                        <asp:LinkButton ID="btnPurchaseOrderMenu" runat="server"
                                            CssClass="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                                            <i class="mdi mdi-file-document-outline"></i> Purchase Order
                                        </asp:LinkButton>

                                        <ul class="dropdown-menu">
                                            <%-- Section: Maintain --%>
                                                <li><span class="dropdown-section-header">Maintain</span></li>
                                                <li>
                                                    <asp:LinkButton ID="btnRequestChange" runat="server"
                                                        CssClass="dropdown-item" OnClick="btnRequestChange_Click">
                                                        <i class="mdi mdi-clipboard-edit"></i> Request Change
                                                    </asp:LinkButton>
                                                </li>

                                                <li>
                                                    <hr class="dropdown-divider" />
                                                </li>

                                                <%-- Section: View --%>
                                                    <li><span class="dropdown-section-header">View</span></li>
                                                    <li>
                                                        <asp:LinkButton ID="LinkButton1" runat="server"
                                                            CssClass="dropdown-item" OnClick="btnTotal_Click">
                                                            <i class="mdi mdi-sigma"></i> Totals
                                                        </asp:LinkButton>
                                                    </li>
                                        </ul>
                                    </div>

                                    <%-- ════════════════════════════════════ MENU: Purchase Sections: Confirm | Charges
                                        | Tax ════════════════════════════════════ --%>
                                        <div class="action-items dropdown">
                                            <asp:LinkButton ID="btnPurchases" runat="server" CssClass="dropdown-toggle"
                                                data-bs-toggle="dropdown" aria-expanded="false">
                                                <i class="mdi mdi-cart-outline"></i> Purchase
                                            </asp:LinkButton>

                                            <ul class="dropdown-menu">
                                                <%-- Section: Confirm --%>
                                                    <li><span class="dropdown-section-header">Confirm</span></li>
                                                    <li>
                                                        <asp:LinkButton ID="btnConfirm" runat="server"
                                                            CssClass="dropdown-item" OnClick="btnConfirm_Click">
                                                            <i class="mdi mdi-check-circle-outline"></i> Confirm
                                                        </asp:LinkButton>
                                                    </li>

                                                    <li>
                                                        <hr class="dropdown-divider" />
                                                    </li>

                                                    <%-- Section: Charges --%>
                                                        <li><span class="dropdown-section-header">Charges</span></li>
                                                        <li>
                                                            <asp:LinkButton ID="btnMaintainCharges" runat="server"
                                                                CssClass="dropdown-item"
                                                                OnClick="btnMaintainCharges_Click">
                                                                <i class="mdi mdi-currency-usd"></i> Maintain Charges
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="btnAllocateCharges" runat="server"
                                                                CssClass="dropdown-item"
                                                                OnClick="btnAllocateCharges_Click">
                                                                <i class="mdi mdi-scale-balance"></i> Allocate Charges
                                                            </asp:LinkButton>
                                                        </li>

                                                        <li>
                                                            <hr class="dropdown-divider" />
                                                        </li>

                                                        <%-- Section: Create --%>
                                                            <li><span class="dropdown-section-header">Create</span></li>
                                                            <li>
                                                               <asp:LinkButton ID="btnCreateNote" runat="server"
                                                                CssClass="dropdown-item"
                                                             OnClick="btnCreateNote_Click">
                                                           <i class="mdi mdi-note-plus-outline"></i> Credit Note
                                                              </asp:LinkButton>
                                                            </li>

                                                            <li>
                                                                <hr class="dropdown-divider" />
                                                            </li>

                                                            <%-- Section: Prepay --%>
                                                                <li><span class="dropdown-section-header">Prepay</span>
                                                                </li>
                                                                <li>
                                                                        <asp:LinkButton ID="btnPrepayment" runat="server"
                                                                         CssClass="dropdown-item"
                                                                      OnClick="btnPrepayment_Click">
                                                                    <i class="mdi mdi-cash-fast"></i> Prepayment
                                                                       </asp:LinkButton>

                                                                <%--    <asp:LinkButton ID="btnPrepayment" runat="server"
                                                                        CssClass="dropdown-item"
                                                                        OnClientClick="javascript: return openPopupPanel('/ESS/PR/PurchaseOrder_Prepayment.aspx', 400);">
                                                                        <i class="mdi mdi-cash-fast"></i> Prepayment
                                                                    </asp:LinkButton>--%>
                                                                </li>
                                                            <%--    <li>
                                                                    <asp:LinkButton ID="btnRemovePrepayment" runat="server"
                                                                        CssClass="dropdown-item" OnClick="btnRemovePrepayment_Click">
                                                                        <i class="mdi mdi-cash-minus"></i> Remove Prepayment
                                                                    </asp:LinkButton>
                                                                </li>--%>

                                                               <li>
                                                        <asp:LinkButton ID="btnRemovePrepayment" runat="server"
                                                            CssClass="dropdown-item" OnClick="btnRemovePrepayment_Click"
                                                            OnClientClick="return confirm('Are you sure you want to remove this prepayment?');">
                                                            <i class="mdi mdi-cash-minus"></i> Remove Prepayment
                                                        </asp:LinkButton>
                                                    </li>

                                                                <li>
                                                                    <hr class="dropdown-divider" />
                                                                </li>

                                                                <%-- Section: Tax --%>
                                                                    <li><span class="dropdown-section-header">Tax</span>
                                                                    </li>
                                                                    <li>
                                                                        <asp:LinkButton ID="btnSalesTax" runat="server"
                                                                            CssClass="dropdown-item"
                                                                            OnClick="btnSalesTax_Click">
                                                                            <i class="mdi mdi-percent"></i> Sales Tax
                                                                        </asp:LinkButton>
                                                                    </li>
                                            </ul>
                                        </div>

                                        <%-- ════════════════════════════════════ MENU: Receive Sections: Generate |
                                            Journal ════════════════════════════════════ --%>
                                            <div class="action-items dropdown">
                                                <asp:LinkButton ID="btnReceive" runat="server"
                                                    CssClass="dropdown-toggle" data-bs-toggle="dropdown"
                                                    aria-expanded="false">
                                                    <i class="mdi mdi-truck-delivery-outline"></i> Receive
                                                </asp:LinkButton>

                                                <ul class="dropdown-menu">
                                                    <%-- Section: Generate --%>
                                                        <li><span class="dropdown-section-header">Generate</span></li>
                                                        <li>
                                                            <asp:LinkButton ID="btnReceiptsList" runat="server"
                                                                CssClass="dropdown-item"
                                                                OnClick="btnReceiptsList_Click">
                                                                <i class="mdi mdi-clipboard-list-outline"></i> Receipts
                                                                List
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="btnProductReceipt" runat="server"
                                                                CssClass="dropdown-item"
                                                                OnClick="btnProductReceipt_Click">
                                                                <i class="mdi mdi-package-variant-closed"></i> Product
                                                                Receipt
                                                            </asp:LinkButton>
                                                        </li>

                                                        <li>
                                                            <hr class="dropdown-divider" />
                                                        </li>

                                                        <%-- Section: Journal --%>
                                                            <li><span class="dropdown-section-header">Journal</span>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnReceiptList" runat="server"
                                                                    CssClass="dropdown-item"
                                                                    OnClick="btnReceiptList_Click">
                                                                    <i class="mdi mdi-book-open-outline"></i> Receipts
                                                                    List
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnProductReceipts" runat="server"
                                                                    CssClass="dropdown-item"
                                                                    OnClick="btnProductReceipst_Click">
                                                                    <i class="mdi mdi-book-multiple-outline"></i>
                                                                    Product Receipts
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnInvoiceJournal" runat="server"
                                                                    CssClass="dropdown-item"
                                                                    OnClick="btnInvoiceJournal_Click">
                                                                    <i class="mdi mdi-file-document-outline"></i>
                                                                    Invoice
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnPendingInvoice" runat="server"
                                                                    CssClass="dropdown-item"
                                                                    OnClick="btnPendingInvoice_Click">
                                                                    <i class="mdi mdi-file-document-outline"></i>
                                                                    Pending Invoice
                                                                </asp:LinkButton>
                                                            </li>
                                                </ul>
                                            </div>

                                            <%-- ════════════════════════════════════ MENU: Invoice Section: Generate
                                                ════════════════════════════════════ --%>
                                                <div class="action-items dropdown">
                                                    <asp:LinkButton ID="btnInvoice" runat="server"
                                                        CssClass="dropdown-toggle" data-bs-toggle="dropdown"
                                                        aria-expanded="false">
                                                        <i class="mdi mdi-file-invoice-dollar"></i> Invoice
                                                    </asp:LinkButton>

                                                    <ul class="dropdown-menu">
                                                        <%-- Section: Generate --%>
                                                            <li><span class="dropdown-section-header">Generate</span>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btninvoice_generate" runat="server"
                                                                    CssClass="dropdown-item" OnClick="btnInvoice_Click">
                                                                    <i class="mdi mdi-file-plus-outline"></i> Invoice
                                                                </asp:LinkButton>
                                                            </li>
                                                    </ul>
                                                </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Content>


        <asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
            <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>

                    <asp:HiddenField ID="hdnSearchQuery" runat="server" />
                    <asp:Button ID="btnServerSearch" runat="server" OnClick="btnServerSearch_Click" Style="display: none;" />
                    <asp:HiddenField ID="hdnHasMore" runat="server" Value="true" />
                    <asp:Button ID="btnLoadMore" runat="server" OnClick="btnLoadMore_Click" Style="display: none;" />

                    <div>
                        <asp:GridView ID="gridView" runat="server" Data="searchable"
                            CssClass="table table-condensed no-border table-hover table-responsive"
                            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
                            OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                    <%-- <HeaderTemplate>
                                        <input type="checkbox" id="chk_SelectAll" CssClass="round-checkbox" />
                                        </HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_SelectSingle"
                                                OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true"
                                                runat="server" CssClass="round-checkbox" />
                                        </ItemTemplate>
                                </asp:TemplateField>

                                <%-- <asp:TemplateField HeaderText="Purchase Order">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseOrderId" runat="server"
                                            Text='<%# Bind("PurchaseOrderId") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPurchaseOrderId" runat="server"
                                            Text='<%# Bind("PurchaseOrderId") %>' CssClass="aspnet-textbox" />
                                    </EditItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Purchase order">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPurchcaseOrderId" runat="server"
                                                Text='<%# Eval("PurchaseOrderId") %>'
                                                CommandArgument='<%# Eval("PurchaseOrderId") %>'
                                                OnClick="lnkPurchReqId_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Default Dimension" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDefaultDimension" runat="server"
                                                Text='<%# Bind("DefaultDimension") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Vendor account">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVendorAccount" runat="server"
                                                Text='<%# Bind("VendorAccount") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtVendorAccount" runat="server"
                                                Text='<%# Bind("VendorAccount") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Invoice account">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceAccount" runat="server"
                                                Text='<%# Bind("InvoiceAccount") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtInvoiceAccount" runat="server"
                                                Text='<%# Bind("InvoiceAccount") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Vendor name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVendorName" runat="server"
                                                Text='<%# Bind("VendorName") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtVendorName" runat="server"
                                                Text='<%# Bind("VendorName") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Purchase type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseType" runat="server"
                                                Text='<%# Bind("PurchaseType") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPurchaseType" runat="server"
                                                Text='<%# Bind("PurchaseType") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Approval status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovalStatus" runat="server"
                                                Text='<%# Bind("ApprovalStatus") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtApprovalStatus" runat="server"
                                                Text='<%# Bind("ApprovalStatus") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Purchase order status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseOrderStatus" runat="server"
                                                Text='<%# Bind("PurchaseOrderStatus") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPurchaseOrderStatus" runat="server"
                                                Text='<%# Bind("PurchaseOrderStatus") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Currency">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrency" runat="server"
                                                Text='<%# Bind("CurrencyCode") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCurrency" runat="server"
                                                Text='<%# Bind("CurrencyCode") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Requested receipt date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrequestedreceiptdate" runat="server"
                                                Text='<%# Eval("RequestedReceiptDate", "{0:M/d/yyyy}") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="txtSrequestedreceiptdate" runat="server"
                                                Text='<%# Bind("RequestedReceiptDate", "{0:M/d/yyyy}") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Delivery mode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeliveryMode" runat="server"
                                                Text='<%# Bind("DeliveryMode") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDeliveryMode" runat="server"
                                                Text='<%# Bind("DeliveryMode") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Delivery terms">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeliveryTerms" runat="server"
                                                Text='<%# Bind("DeliveryTerms") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDeliveryTerms" runat="server"
                                                Text='<%# Bind("DeliveryTerms") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Purchase agreement">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseAgreement" runat="server"
                                                Text='<%# Bind("PurchaseAgreement") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPurchaseAgreement" runat="server"
                                                Text='<%# Bind("PurchaseAgreement") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quality order status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQualityOrderStatus" runat="server"
                                                Text='<%# Bind("QualityOrderStatus") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtQualityOrderStatus" runat="server"
                                                Text='<%# Bind("QualityOrderStatus") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Direct delivery">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDirectDelivery" runat="server"
                                                Text='<%# Bind("DirectDelivery") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDirectDelivery" runat="server"
                                                Text='<%# Bind("DirectDelivery") %>' CssClass="aspnet-textbox" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteID" runat="server" Text='<%# Bind("SiteID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocationID" runat="server"
                                                Text='<%# Bind("LocationID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RecId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RecId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreateddatetime" runat="server"
                                                Text='<%# Bind("Createddatetime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                        <ItemTemplate>
                                            <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment"
                                                Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                                        </ItemTemplate>
                                        <EditItemTemplate>

                                        </EditItemTemplate>
                                    </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <%-- Shimmer Loading State --%>
                        <div id="shimmer-loader" class="shimmer-loader">
                            <div class="shimmer-row">
                                <div class="shimmer-cell chk"></div>
                                <div class="shimmer-cell po"></div>
                                <div class="shimmer-cell vendor"></div>
                                <div class="shimmer-cell name"></div>
                                <div class="shimmer-cell status"></div>
                            </div>
                            <div class="shimmer-row">
                                <div class="shimmer-cell chk"></div>
                                <div class="shimmer-cell po"></div>
                                <div class="shimmer-cell vendor"></div>
                                <div class="shimmer-cell name"></div>
                                <div class="shimmer-cell status"></div>
                            </div>
                            <div class="shimmer-row">
                                <div class="shimmer-cell chk"></div>
                                <div class="shimmer-cell po"></div>
                                <div class="shimmer-cell vendor"></div>
                                <div class="shimmer-cell name"></div>
                                <div class="shimmer-cell status"></div>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <script type="text/javascript">
                function refreshParentGrid() {
                    __doPostBack('RefreshGrid', '');
                }

                $(document).ready(function () {
                    var searchInput = document.querySelector('.search-input');
                    var typingTimer;
                    var doneTypingInterval = 800; // time in ms

                    if (searchInput) {
                        // Unbind the previous enter key listener just in case it was cached, 
                        // though overwriting the script does this.
                        // We also nullify the default oninput if we want to take full control, 
                        // but let's just add our debounce listener.
                        
                        searchInput.addEventListener('input', function () {
                            clearTimeout(typingTimer);
                            var query = searchInput.value.trim();
                            
                            // Client-side quick filter
                            var tbody = document.querySelector('#<%= gridView.ClientID %> tbody');
                            if (tbody) {
                                var rows = tbody.querySelectorAll('tr:not(.grid-header)');
                                rows.forEach(function(row) {
                                    if (query === '' || row.textContent.toLowerCase().indexOf(query.toLowerCase()) !== -1) {
                                        row.style.display = '';
                                    } else {
                                        row.style.display = 'none';
                                    }
                                });
                            }

                            typingTimer = setTimeout(function () {
                                var hdn = document.getElementById('<%= hdnSearchQuery.ClientID %>');
                                if (hdn) {
                                    // If query changed, do a server postback
                                    if (hdn.value !== query || query !== '') {
                                        hdn.value = query;
                                        window.suppressOverlay = true; // prevent generic overlay
                                        
                                        var shimmer = document.getElementById('shimmer-loader');
                                        if (shimmer) shimmer.style.display = 'none'; // Ensure shimmer is hidden during search
                                        
                                        __doPostBack('<%= btnServerSearch.UniqueID %>', '');
                                    }
                                }
                            }, doneTypingInterval);
                        });

                        // Prevent Enter key from doing full page postback
                        searchInput.addEventListener('keydown', function(e) {
                            if (e.key === 'Enter') {
                                e.preventDefault();
                            }
                        });
                    }
                });
            </script>
        </asp:Content>