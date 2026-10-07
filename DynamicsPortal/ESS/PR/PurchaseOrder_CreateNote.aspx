<%@ Page Title="Credit Note" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true"
    CodeFile="PurchaseOrder_CreateNote.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_CreateNote" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
        <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@mdi/font/css/materialdesignicons.min.css" />

        <style>
            .modal-footer {
                position: sticky;
                bottom: 0;
                background-color: white;
                z-index: 1050;
                box-shadow: 0 -4px 6px -1px rgba(0, 0, 0, 0.1);
                padding: 15px 20px;
                border-top: 1px solid #dee2e6;
            }
            .modal-body {
                overflow-y: auto;
                max-height: calc(100vh - 120px);
            }
            .d365-table {
                font-size: 0.85rem;
                vertical-align: middle;
            }
            .d365-table th {
                font-weight: 600;
                color: #333;
                background-color: #f8f9fa;
                border-bottom: 2px solid #dee2e6;
            }
            .selected-row {
                background-color: #e8f4fd !important;
            }
            .create-note-container {
                padding: 20px;
                font-family: "Segoe UI", Arial, sans-serif;
                background-color: #f8f9fa;
            }
            .d365-accordion-button {
                font-weight: 600;
                font-size: 1rem;
                color: #000;
                background-color: transparent !important;
                padding: 0.75rem 1.25rem;
                box-shadow: none !important;
                border-bottom: 1px solid #e9ecef;
            }
            .d365-accordion-button::after {
                background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' fill='%23000'%3e%3cpath fill-rule='evenodd' d='M1.646 4.646a.5.5 0 0 1 .708 0L8 10.293l5.646-5.647a.5.5 0 0 1 .708.708l-6 6a.5.5 0 0 1-.708 0l-6-6a.5.5 0 0 1 0-.708z'/%3e%3c/svg%3e") !important;
                transform: rotate(-180deg);
            }
            .d365-accordion-button.collapsed::after {
                transform: rotate(0deg);
            }
            .d365-section-title {
                font-size: 0.75rem;
                text-transform: uppercase;
                font-weight: 700;
                letter-spacing: 0.05em;
                margin-bottom: 0.5rem;
                color: #000;
            }
            .d365-label {
                font-size: 0.85rem;
                color: #666;
                margin-bottom: 0.25rem;
            }
            .d365-table tbody tr:hover {
                background-color: #f1f5f9;
                cursor: pointer;
            }
            .d365-switch-container {
                display: flex;
                align-items: center;
            }
            .d365-switch {
                position: relative;
                display: inline-block;
                width: 44px;
                height: 22px;
                margin-right: 10px;
                margin-bottom: 0;
            }
            .d365-switch input {
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
                transition: .4s;
                border-radius: 50%;
            }
            input:checked+.slider {
                background-color: #000;
            }
            input:focus+.slider {
                box-shadow: 0 0 1px #000;
            }
            input:checked+.slider:before {
                transform: translateX(22px);
            }
            .switch-label {
                font-size: 0.85rem;
                color: #333;
                cursor: pointer;
                user-select: none;
            }
            .footer-buttons {
                display: flex;
                justify-content: flex-end;
                gap: 10px;
                padding: 15px 20px;
                background-color: #fff;
                border-top: 1px solid #ddd;
                position: sticky;
                bottom: 0;
                width: 100%;
                z-index: 1000;
            }
            .btn-d365 {
                font-size: 14px;
                min-width: 90px;
                font-weight: 600;
                border-radius: 2px;
                padding: 6px 12px;
            }
            .btn-primary {
                background-color: #0078d4;
                border-color: #0078d4;
                color: white;
            }
            .btn-primary:hover {
                background-color: #005a9e;
            }
            .btn-default {
                background-color: #f3f2f1;
                border-color: #8a8886;
                color: #323130;
            }
            .btn-default:hover {
                background-color: #e1dfdd;
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
      
        <asp:UpdatePanel ID="upCreateNote" runat="server" UpdateMode="Always">
            <ContentTemplate>

        <div class="create-note-container pb-5">
            <div class="accordion" id="createNoteAccordion">
                <div class="accordion-item border-0 border-bottom bg-white mb-2 shadow-sm">
                    <h2 class="accordion-header" id="headingParameters">
                        <button class="accordion-button d365-accordion-button" type="button" data-bs-toggle="collapse"
                            data-bs-target="#collapseParameters" aria-expanded="true"
                            aria-controls="collapseParameters">
                            Parameters
                        </button>
                    </h2>
                    <div id="collapseParameters" class="accordion-collapse collapse show"
                        aria-labelledby="headingParameters">
                        <div class="accordion-body px-4 py-3">
                            <div class="row g-4">
                                <div class="col-md-3">
                                    <div class="d365-section-title">Quantity</div>
                                    <div class="mb-3">
                                        <div class="d365-label">Quantity factor</div>
                                        <asp:TextBox ID="txtQuantityFactor" runat="server"
                                            CssClass="form-control form-control-sm" Text="1.00" Width="100px">
                                        </asp:TextBox>
                                    </div>
                                    <div class="mb-3">
                                        <div class="d365-label">Invert sign</div>
                                        <div class="d365-switch-container">
                                            <label class="d365-switch">
                                                <input type="checkbox" id="tglInvertSign" checked
                                                    onchange="toggleLabel(this, 'lblInvertSign')">
                                                <span class="slider"></span>
                                            </label>
                                            <span class="switch-label" id="lblInvertSign">Yes</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="d365-section-title">General</div>
                                    <div class="mb-3">
                                        <div class="d365-label">Copy charges</div>
                                        <div class="d365-switch-container">
                                            <label class="d365-switch">
                                                <input type="checkbox" id="tglCopyCharges" checked
                                                    onchange="toggleLabel(this, 'lblCopyCharges')">
                                                <span class="slider"></span>
                                            </label>
                                            <span class="switch-label" id="lblCopyCharges">Yes</span>
                                        </div>
                                    </div>
                                    <div class="mb-3">
                                        <div class="d365-label">Recalculate price</div>
                                        <div class="d365-switch-container">
                                            <label class="d365-switch">
                                                <input type="checkbox" id="tglRecalculatePrice"
                                                    onchange="toggleLabel(this, 'lblRecalculatePrice')">
                                                <span class="slider"></span>
                                            </label>
                                            <span class="switch-label" id="lblRecalculatePrice">No</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="d365-section-title">&nbsp;</div>
                                    <div class="mb-3 mt-4">
                                        <div class="d365-label">Copy precisely</div>
                                        <div class="d365-switch-container">
                                            <label class="d365-switch">
                                                <input type="checkbox" id="tglCopyPrecisely" checked
                                                    onchange="toggleLabel(this, 'lblCopyPrecisely')">
                                                <span class="slider"></span>
                                            </label>
                                            <span class="switch-label" id="lblCopyPrecisely">Yes</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="d365-section-title">&nbsp;</div>
                                    <div class="mb-3 mt-4">
                                        <div class="d365-label">Delete purchase lines</div>
                                        <div class="d365-switch-container">
                                            <label class="d365-switch">
                                                <input type="checkbox" id="tglDeletePurchaseLines"
                                                    onchange="toggleLabel(this, 'lblDeletePurchaseLines')">
                                                <span class="slider"></span>
                                            </label>
                                            <span class="switch-label" id="lblDeletePurchaseLines">No</span>
                                        </div>
                                    </div>
                                    <div class="mb-3">
                                        <div class="d365-label">Copy order header</div>
                                        <div class="d365-switch-container">
                                            <label class="d365-switch">
                                                <input type="checkbox" id="tglCopyOrderHeader" checked
                                                    onchange="toggleLabel(this, 'lblCopyOrderHeader')">
                                                <span class="slider"></span>
                                            </label>
                                            <span class="switch-label" id="lblCopyOrderHeader">Yes</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="accordion-item border-0 border-bottom bg-white mb-2 shadow-sm">
                    <h2 class="accordion-header" id="headingInvoices">
                        <button class="accordion-button d365-accordion-button collapsed" type="button"
                            data-bs-toggle="collapse" data-bs-target="#collapseInvoices" aria-expanded="false"
                            aria-controls="collapseInvoices">
                            Invoices
                        </button>
                    </h2>
                    <div id="collapseInvoices" class="accordion-collapse collapse" aria-labelledby="headingInvoices">
                        <div class="accordion-body p-0">
                            <div class="p-3">
                                <div class="d365-section-title">Headers</div>
                                <div class="border bg-white" style="max-height: 200px; overflow-y: auto;">
                                    <asp:GridView ID="gvInvoiceHeaders" runat="server"
                                        CssClass="table d365-table mb-0 w-100"
                                        AutoGenerateColumns="False"
                                        ShowHeaderWhenEmpty="true"
                                        EmptyDataText="No Record Found."
                                       >

                                        <Columns>

                                         <%--   <asp:TemplateField HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <i class="mdi mdi-refresh"
                                                        style="cursor:pointer; font-size:1.1rem; color:#666;"></i>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderStyle-Width="40px">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAllHeaders" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server"
                                                        AutoPostBack="true"
                                                        OnCheckedChanged="chkSelect_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Purchase order">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPurchId" runat="server"
                                                        Text='<%# Eval("PurchId") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Vendor account">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVendorAccount" runat="server"
                                                        Text='<%# Eval("OrderAccount") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVendorName" runat="server"
                                                        Text='<%# Eval("displayOrderAccountName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Invoice">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceId" runat="server"
                                                        Text='<%# Eval("InvoiceId") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceDate" runat="server"
                                                        Text='<%# Eval("InvoiceDate", "{0:dd/MM/yyyy}") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Voucher">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVoucher" runat="server"
                                                        Text='<%# Eval("LedgerVoucher") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Invoice amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtInvoiceAmount" runat="server"
                                                        CssClass="form-control text-end"
                                                        Text='<%# Eval("InvoiceAmount", "{0:N2}") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Currency">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrency" runat="server"
                                                        Text='<%# Eval("CurrencyCode") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>

                                    </asp:GridView>
                                </div>
                            </div>

                            <hr class="m-0" style="border-color: #dee2e6;">

                            <div class="p-3 bg-light">
                                <div class="d365-section-title">Lines</div>
                                <div class="border bg-white" style="max-height: 200px; overflow-y: auto;">
                                    <asp:GridView ID="gvInvoiceLines" runat="server"
                                        CssClass="table d365-table mb-0 w-100"
                                        AutoGenerateColumns="False"
                                        ShowHeaderWhenEmpty="true"
                                        EmptyDataText="Select an invoice header to view its lines.">

                                        <Columns>

                                            <asp:TemplateField HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <i class="mdi mdi-refresh"
                                                        style="cursor:pointer; font-size:1.1rem; color:#666;"></i>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="40px">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAllLines" runat="server"
                                                        AutoPostBack="true"
                                                        OnCheckedChanged="chkSelectAllLines_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelectLine" runat="server"
                                                        AutoPostBack="true"
                                                        OnCheckedChanged="chkSelectLine_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Line number">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLineNumber" runat="server"
                                                        Text='<%# Eval("PurchaseLineLineNumber") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Item">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemId" runat="server"
                                                        Text='<%# Eval("itemId") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Procurement category">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryName" runat="server"
                                                        Text='<%# Eval("ProcurementCategory") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" runat="server"
                                                        Text='<%# Eval("Name") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Site">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSiteId" runat="server"
                                                        Text='<%# Eval("siteId") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Warehouse">
                                             <ItemTemplate>
                                   <asp:Label ID="lblWarehouseId" runat="server"
                                       Text='<%# Eval("locationId") %>'>
                                              </asp:Label>
                                              </ItemTemplate>
                                                 </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Quantiy">
                                                 <ItemTemplate>
                                                      <asp:Label ID="lblQuantity" runat="server"
                                                                  Text='<%# Eval("qty","{0:N2}") %>'>
                                                              </asp:Label>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Unit">
                                           <ItemTemplate>
                                                <asp:Label ID="lblUnit" runat="server"
                                                            Text='<%# Eval("unit") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            
                                                            <asp:TemplateField HeaderText="Unit price">
                                                   <ItemTemplate>
                                                        <asp:Label ID="lblUnitPrice" runat="server"
                                                                    Text='<%# Eval("PurchPrice") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                 <asp:Label ID="lblAmount" runat="server"
                                                             Text='<%# Eval("lineamount") %>'>
                                                         </asp:Label>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Discount">
                                                    <ItemTemplate>
                                                         <asp:Label ID="lblDiscount" runat="server"
                                                                     Text='<%# Eval("discount") %>'>
                                                                 </asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Discount percentage">
                                                                <ItemTemplate>
                                                                     <asp:Label ID="lblDiscountpercentage" runat="server"
                                                                                 Text='<%# Eval("discPercent") %>'>
                                                                             </asp:Label>
                                                                         </ItemTemplate>
                                                                     </asp:TemplateField>
                                        
                                        </Columns>

                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

             <%--   <!-- Section 3: Selected Lines -->
                <div class="accordion-item border-0 shadow-sm bg-white">
                    <h2 class="accordion-header" id="headingSelectedLines">
                        <button class="accordion-button d365-accordion-button collapsed" type="button"
                            data-bs-toggle="collapse" data-bs-target="#collapseSelectedLines" aria-expanded="false"
                            aria-controls="collapseSelectedLines">
                            Selected Lines
                        </button>
                    </h2>
                    <div id="collapseSelectedLines" class="accordion-collapse collapse"
                        aria-labelledby="headingSelectedLines">
                        <div class="accordion-body p-3">
                            <div class="border bg-white" style="max-height: 250px; overflow-y: auto;">
                                <asp:GridView ID="gvSelectedLines" runat="server"
                                    CssClass="table d365-table mb-0 w-100"
                                    AutoGenerateColumns="False"
                                    ShowHeaderWhenEmpty="true"
                                    EmptyDataText="We didn't find anything to show here.">

                                    <Columns>

                                        <asp:TemplateField HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <i class="mdi mdi-refresh"
                                                    style="cursor:pointer; font-size:1.1rem; color:#666;"></i>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Width="40px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAllSelectedLines" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectedLine" runat="server"
                                                    AutoPostBack="true"
                                                    OnCheckedChanged="chkSelectedLine_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Invoice">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSelInvoiceId" runat="server"
                                                    Text='<%# Eval("InvoiceId") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Line number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSelLineNumber" runat="server"
                                                    Text='<%# Eval("LineNumber") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Item">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSelItemId" runat="server"
                                                    Text='<%# Eval("ItemId") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Procurement category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSelCategoryName" runat="server"
                                                    Text='<%# Eval("CategoryName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSelItemName" runat="server"
                                                    Text='<%# Eval("ItemName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSelQuantity" runat="server"
                                                    Text='<%# Eval("Quantity", "{0:N2}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Site">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSelSiteId" runat="server"
                                                    Text='<%# Eval("SiteId") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Warehouse">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSelWarehouseId" runat="server"
                                                    Text='<%# Eval("WarehouseId") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>--%>

            </div>
        </div>

        <div class="footer-buttons">
            <asp:HiddenField ID="hfCreditNoteData" runat="server" />
            <asp:Button ID="btnOK" runat="server" Text="OK" CssClass="btn btn-d365 btn-primary" OnClientClick="return prepareCreditNoteData();" OnClick="btnOK_Click" />
            <button type="button" class="btn btn-d365 btn-default" onclick="closeDialog();">Cancel</button>
        </div>

            </ContentTemplate>
        </asp:UpdatePanel>

        <script type="text/javascript">
            function prepareCreditNoteData() {
                var contract = {
                    QuantityFactor: parseFloat(document.getElementById('<%= txtQuantityFactor.ClientID %>').value) || 1.0,
                    InvertSign: document.getElementById('tglInvertSign').checked,
                    CopyCharges: document.getElementById('tglCopyCharges').checked,
                    RecalculatePrice: document.getElementById('tglRecalculatePrice').checked,
                    CopyPrecisely: document.getElementById('tglCopyPrecisely').checked,
                    DeletePurchaseLines: document.getElementById('tglDeletePurchaseLines').checked,
                    CopyOrderHeader: document.getElementById('tglCopyOrderHeader').checked
                };

                document.getElementById('<%= hfCreditNoteData.ClientID %>').value = JSON.stringify(contract);
                return true;
            }

            function toggleLabel(checkbox, labelId) {
                document.getElementById(labelId).innerText = checkbox.checked ? 'Yes' : 'No';
            }
        </script>
    </asp:Content>