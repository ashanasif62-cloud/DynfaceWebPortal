<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SalesOrder_ListPage.aspx.cs" Inherits="DynamicsPortal.SalesOrder_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />

    <script>
        $(document).ready(function () {
            var $row = $(".highlight-row");
            if ($row.length) {
                setTimeout(function () {
                    $row.addClass("fade-out");
                }, 1000);
            }
        });
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

        /* ── Pagination Styling ── */
        .pagination-container {
            display: flex;
            align-items: center;
            justify-content: flex-end;
            padding: 1rem;
            gap: 1rem;
            background: #fff;
            border-top: 1px solid #eee;
        }

        .page-info {
            font-size: 0.875rem;
            color: #666;
            font-weight: 500;
        }
    </style>
</asp:Content>

<%-- ═══════════════════════════════════════════════════════════════ ACTION PANEL
     ═══════════════════════════════════════════════════════════════ --%>
<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>

            <%-- ── New ── --%>
            <div class="action-items">
                <asp:LinkButton ID="btnNew" runat="server"
                    OnClientClick="javascript: return openPopupPanel('/ESS/PR/SalesOrder_Create.aspx', 700)">
                    <i class="mdi mdi-plus"></i> New
                </asp:LinkButton>
            </div>

            <%-- ── Delete ── --%>
            <div class="action-items">
                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click">
                    <i class="mdi mdi-delete"></i> Delete
                </asp:LinkButton>
            </div>

            <%-- ════════════════════════════════════ MENU: Workflow ════════════════════════════════════ --%>
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

            <%-- ════════════════════════════════════ MENU: Sales Order ════════════════════════════════════ --%>
            <div class="action-items dropdown">
                <asp:LinkButton ID="btnSalesOrderMenu" runat="server"
                    CssClass="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="mdi mdi-file-document-outline"></i> Sales Order
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

                    <li><hr class="dropdown-divider" /></li>

                    <%-- Section: View --%>
                    <li><span class="dropdown-section-header">View</span></li>
                    <li>
                        <asp:LinkButton ID="btnTotal" runat="server"
                            CssClass="dropdown-item" OnClick="btnTotal_Click">
                            <i class="mdi mdi-sigma"></i> Totals
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>

            <%-- ════════════════════════════════════ MENU: Sell ════════════════════════════════════ --%>
            <div class="action-items dropdown">
                <asp:LinkButton ID="btnSell" runat="server" CssClass="dropdown-toggle"
                    data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="mdi mdi-cart-outline"></i> Sell
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

                    <li><hr class="dropdown-divider" /></li>

                    <%-- Section: Charges --%>
                    <li><span class="dropdown-section-header">Charges</span></li>
                    <li>
                        <asp:LinkButton ID="btnMaintainCharges" runat="server"
                            CssClass="dropdown-item" OnClick="btnMaintainCharges_Click">
                            <i class="mdi mdi-currency-usd"></i> Maintain Charges
                        </asp:LinkButton>
                    </li>

                    <li><hr class="dropdown-divider" /></li>

                    <%-- Section: Tax --%>
                    <li><span class="dropdown-section-header">Tax</span></li>
                    <li>
                        <asp:LinkButton ID="btnSalesTax" runat="server"
                            CssClass="dropdown-item" OnClick="btnSalesTax_Click">
                            <i class="mdi mdi-percent"></i> Sales Tax
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>

            <%-- ════════════════════════════════════ MENU: Pick and Pack ════════════════════════════════════ --%>
            <div class="action-items dropdown">
                <asp:LinkButton ID="btnPickPack" runat="server"
                    CssClass="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="mdi mdi-truck-delivery-outline"></i> Pick and Pack
                </asp:LinkButton>

                <ul class="dropdown-menu">
                    <%-- Section: Generate --%>
                    <li><span class="dropdown-section-header">Generate</span></li>
                    <li>
                        <asp:LinkButton ID="btnPickingList" runat="server"
                            CssClass="dropdown-item" OnClick="btnPickingList_Click">
                            <i class="mdi mdi-clipboard-list-outline"></i> Picking List
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnPackingSlip" runat="server"
                            CssClass="dropdown-item" OnClick="btnPackingSlip_Click">
                            <i class="mdi mdi-package-variant-closed"></i> Packing Slip
                        </asp:LinkButton>
                    </li>

                    <li><hr class="dropdown-divider" /></li>

                    <%-- Section: Journal --%>
                    <li><span class="dropdown-section-header">Journal</span></li>
                    <li>
                        <asp:LinkButton ID="btnPickingListJournal" runat="server"
                            CssClass="dropdown-item" OnClick="btnPickingListJournal_Click">
                            <i class="mdi mdi-book-open-outline"></i> Picking List
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnPackingSlipJournal" runat="server"
                            CssClass="dropdown-item" OnClick="btnPackingSlipJournal_Click">
                            <i class="mdi mdi-book-multiple-outline"></i> Packing Slip
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>

            <%-- ════════════════════════════════════ MENU: Invoice ════════════════════════════════════ --%>
            <div class="action-items dropdown">
                <asp:LinkButton ID="btnInvoice" runat="server"
                    CssClass="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="mdi mdi-file-invoice-dollar"></i> Invoice
                </asp:LinkButton>

                <ul class="dropdown-menu">
                    <%-- Section: Generate --%>
                    <li><span class="dropdown-section-header">Generate</span></li>
                    <li>
                        <asp:LinkButton ID="btnInvoiceGenerate" runat="server"
                            CssClass="dropdown-item" OnClick="btnInvoice_Click">
                            <i class="mdi mdi-file-plus-outline"></i> Invoice
                        </asp:LinkButton>
                    </li>

                    <li><hr class="dropdown-divider" /></li>

                    <%-- Section: Journal --%>
                    <li><span class="dropdown-section-header">Journal</span></li>
                    <li>
                        <asp:LinkButton ID="btnInvoiceJournal" runat="server"
                            CssClass="dropdown-item" OnClick="btnInvoiceJournal_Click">
                            <i class="mdi mdi-file-document-outline"></i> Invoice Journal
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%-- ═══════════════════════════════════════════════════════════════ PAGE CONTENT
     ═══════════════════════════════════════════════════════════════ --%>
<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>

            <div>
                <asp:GridView ID="gvSalesOrders" runat="server"
                    CssClass="table table-condensed no-border table-hover sortable table-responsive"
                    ShowHeaderWhenEmpty="true"
                    EmptyDataText="No Record Found."
                    DataKeyNames="RecId"
                    OnRowDataBound="gvSalesOrders_RowDataBound"
                    AutoGenerateColumns="false">
                    <Columns>

                        <%-- Checkbox column --%>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle"
                                    OnCheckedChanged="chk_SelectSingle_CheckedChanged"
                                    AutoPostBack="true"
                                    runat="server"
                                    CssClass="round-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Sales Order ID as clickable link --%>
                        <asp:TemplateField HeaderText="Sales order">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSalesOrderId" runat="server"
                                    Text='<%# Eval("SalesId") %>'
                                    CommandArgument='<%# Eval("SalesId") %>'
                                    OnClick="lnkSalesOrderId_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Hidden: RecId --%>
                        <asp:TemplateField HeaderText="RecId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Customer account">
                            <ItemTemplate>
                                <asp:Label ID="lblCustAccount" runat="server"
                                    Text='<%# Bind("custAccount") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCustAccount" runat="server"
                                    Text='<%# Bind("custAccount") %>' CssClass="aspnet-textbox" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Customer name">
                            <ItemTemplate>
                                <asp:Label ID="lblSalesName" runat="server"
                                    Text='<%# Bind("salesName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSalesName" runat="server"
                                    Text='<%# Bind("salesName") %>' CssClass="aspnet-textbox" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Order type">
                            <ItemTemplate>
                                <asp:Label ID="lblSalesType" runat="server"
                                    Text='<%# Bind("SalesType") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSalesType" runat="server"
                                    Text='<%# Bind("SalesType") %>' CssClass="aspnet-textbox" />
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

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblSalesStatus" runat="server"
                                    Text='<%# Bind("SalesStatus") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSalesStatus" runat="server"
                                    Text='<%# Bind("SalesStatus") %>' CssClass="aspnet-textbox" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Release status">
                            <ItemTemplate>
                                <asp:Label ID="lblReleaseStatus" runat="server"
                                    Text='<%# Bind("ReleaseStatus") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtReleaseStatus" runat="server"
                                    Text='<%# Bind("ReleaseStatus") %>' CssClass="aspnet-textbox" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sales taker">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkerSalesTakerName" runat="server"
                                    Text='<%# Bind("WorkerSalesTakerName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtWorkerSalesTakerName" runat="server"
                                    Text='<%# Bind("WorkerSalesTakerName") %>' CssClass="aspnet-textbox" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Hidden: Createddatetime --%>
                      <%--  <asp:TemplateField HeaderText="Created datetime" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCreateddatetime" runat="server"
                                    Text='<%# Bind("Createddatetime") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <%-- Attachment button --%>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>
                                <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment"
                                    Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>

            <%-- Pagination --%>
            <div class="pagination-container">
                <asp:LinkButton ID="btnPrev" runat="server" OnClick="btnPrev_Click"
                    CssClass="btn btn-sm btn-outline-secondary" Visible="false">
                    <i class="mdi mdi-chevron-left"></i> Previous
                </asp:LinkButton>

                <asp:Label ID="lblPageInfo" runat="server" CssClass="page-info" Text="Page 1"></asp:Label>

                <asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click"
                    CssClass="btn btn-sm btn-primary">
                    Next <i class="mdi mdi-chevron-right"></i>
                </asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function refreshParentGrid() {
            __doPostBack('RefreshGrid', '');
        }
    </script>
</asp:Content>
