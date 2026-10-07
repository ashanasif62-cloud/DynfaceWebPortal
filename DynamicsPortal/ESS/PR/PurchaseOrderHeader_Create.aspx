<%@ Page Title="Create Purchase Order" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="PurchaseOrderHeader_Create.aspx.cs" Inherits="DynamicsPortal.PurchaseOrderHeader_Create" %>

<%@ Register Src="~/DropDownList_VendorAccount.ascx" TagPrefix="uc1" TagName="DropDownList_VendorAccount" %>
<%@ Register Src="~/DropDownList_Warehouse.ascx" TagPrefix="uc1" TagName="DropDownList_Warehouse" %>
<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>
<%@ Register Src="~/DropDownList_Contact.ascx" TagPrefix="uc1" TagName="DropDownList_Contact" %>
<%@ Register Src="~/DropDownList_SiteDetails.ascx" TagPrefix="uc1" TagName="DropDownList_SiteDetails" %>
<%@ Register Src="~/DropDownList_ProjectID.ascx" TagPrefix="uc1" TagName="DropDownList_ProjectID" %>
<%@ Register Src="~/DropDownList_Currency.ascx" TagPrefix="uc1" TagName="DropDownList_Currency" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <style>
        .select2-container .select2-selection--single {
            height: 34px !important;
            border: 1px solid #ccc !important;
            border-radius: 4px !important;
        }
        .select2-container--default .select2-selection--single {
            border: 1px solid #ced4da !important;
            border-radius: 0.25rem !important;
            height: 38px !important;
            outline: none !important;
            width: 100% !important;
            display: block !important;
        }
        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 36px !important;
            color: #495057 !important;
            padding-right: 12px !important; /* Remove space reserved for arrow */
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow {
            display: none !important; /* Remove the arrow completely as requested */
        }
        
        /* Force hide the original select element to prevent the empty grey box */
        select.select2-hidden-accessible {
            display: none !important;
            visibility: hidden !important;
        }
        
        /* Grid Layout for Select2 Dropdown */
        .select2-result-grid {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .select2-result-grid__col {
            padding-left: 10px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        .select2-result-grid__col:first-child {
            padding-left: 0;
        }
        .select2-results__option {
            padding: 8px 6px !important;
        }
    </style>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />

    <style>
        
        .round-checkbox input[type="checkbox"],
        input#chk_SelectAll {
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
            width: 18px;
            height: 18px;
            border: 2px solid #333;
            border-radius: 50%;
            background-color: #fff;
            cursor: pointer;
            position: relative;
            outline: none;
        }


            .round-checkbox input[type="checkbox"]:focus,
            input#chk_SelectAll:focus {
                box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.5);
            }


     
    </style>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upgrid" ChildrenAsTriggers="true" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>

            <!-- Vendor Panel -->
            <a href="#vendorPanel" class="d365-toggle-header d-flex justify-content-between align-items-center" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="vendorPanel">
                <span class="section-title">Vendor</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>
            <div class="collapse show mt-3" id="vendorPanel">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="PR-custom-section-title mb-3"></div>

                        <!-- One-time Supplier Toggle -->
                      <%--  <div class="form-group d-flex align-items-center">
                            <label class="mb-0 mr-2">One-time Supplier</label>
                            <label class="toggle-switch mb-0 mr-2">
                                <input type="checkbox" id="chkTransferLines" runat="server" onchange="updateToggleLabel(this)" />
                                <span class="slider"></span>
                            </label>
                            <span id="toggleStatus">No</span>
                        </div>--%>

                <div class="form-group">
                   <label for="ddlVendorAccount">Vendor account <span class="text-danger">*</span></label>
                   <%--  <div><uc1:DropDownList_VendorAccount ID="DropDownList_ddlVendorAccount" OnVendorAccountSelected="ddlVendorAccount_SelectedIndexChanged" runat="server" AutoPostBack="false"/></div> --%>
                    <div>
                        <select id="ddlVendorAccount" style="width: 100%;" ></select>
                        <asp:HiddenField ID="hfVendorAccountId" runat="server" />
                    </div>
                </div>

                        <!-- Vendor Name -->
                        <div class="form-group">
                            <label for="txtVendorName">Name</label>
                            <asp:TextBox ID="txtVendorName" ClientIDMode="Static"  runat="server" CssClass="form-control autocomplete-input gray-input" />
                        </div>

                <!-- Contact -->
                <div class="form-group">
                    <label for="ddlContact">Contact</label>
                    <%-- <div><uc1:DropDownList_Contact ID="DropDownList_ddlContact" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" /></div> --%>
                    <div>
                        <select id="ddlContact" style="width: 100%;"></select>
                        <asp:HiddenField ID="hfContactId" runat="server" />
                    </div>
                </div>

                        <!-- Address Section -->
                        <div class="form-group">
                            <h5>ADDRESS</h5>
                            <div class="d-flex flex-row">
                                <!-- Delivery Name -->
                                <div class="form-group mr-2 flex-fill">
                                    <label for="txtDeliveryName">Delivery name</label>
                                    <asp:TextBox ID="txtDeliveryName" ClientIDMode="Static" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                </div>

                                <!-- Address -->
                                <div class="form-group ml-2 flex-fill">
                                    <label for="txtAddress">Address</label>
                                    <asp:TextBox ID="txtAddress" runat="server" ClientIDMode="Static" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="txtDeliveryAddress">Delivery Address</label>
                            <asp:TextBox ID="txtDeliveryAddress" runat="server" ClientIDMode="Static" CssClass="form-control autocomplete-input" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- General Panel -->
            <a href="#generalPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-4" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="generalPanel">
                <span class="section-title">General</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>
            <div class="collapse show mt-2" id="generalPanel">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <!-- Two-section layout -->
                        <div class="d-flex flex-wrap justify-content-between">
                            <!-- LEFT SECTION -->
                            <div style="flex: 0 0 48%;">
                                <div class="mb-2">
                                    <strong class="d-block mb-2">PURCHASE ORDER</strong>
                                </div>
                                <!-- Purchase Order -->
                                <div class="form-group mb-3" style="width: 250px;">
                                    <label>Purchase order</label>
                                    <asp:TextBox ID="txtPurchaseOrder" Enabled="false" runat="server" CssClass="form-control autocomplete-input" />
                                </div>

                                <div class="form-group mb-3" style="width: 250px;">
                                    <label>Purchase type</label>
                                    <asp:DropDownList ID="ddlPurchaseType" runat="server" CssClass="form-control autocomplete-input">
                                        <asp:ListItem Text="Purchase order" Value="PO" />
                                    </asp:DropDownList>
                                </div>

                        <div class="form-group mb-3" style="width: 250px;">
                            <label>Invoice account <span class="text-danger">*</span></label>
                            <div>
                                <select id="ddlInvoiceAccount" style="width: 100%;"></select>
                                <asp:HiddenField ID="hfInvoiceAccountId" runat="server" />
                            </div>
                        </div>

                                <div class="form-group mb-3" style="width: 250px;">
                                    <label>Name</label>
                                    <asp:TextBox ID="txtName" ClientIDMode="Static" runat="server" CssClass="form-control autocomplete-input" />
                                </div>

                                <div class="mb-2">
                                    <strong class="d-block mb-2">REFERENCES</strong>
                                </div>

                                <div class="form-group mb-3" style="width: 250px;">
                                    <label>Project ID</label>
                                    <div>
                                        <select id="ddlProjectID" style="width: 100%;"></select>
                                        <asp:HiddenField ID="hfProjectId" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group mb-3" style="width: 250px;">
                                    <label>Purchase agreement</label>
                                    <asp:DropDownList ID="ddlPurchaseAgreement" runat="server" CssClass="filterable-dropdown" />
                                </div>

                                <div class="mb-2">
                                    <strong class="d-block mb-2">CURRENCY</strong>
                                </div>
                                <div class="form-group mb-3" style="width: 250px;">
                                     <label>Currency <span class="text-danger">*</span></label>
                                     <div>
                                        <select id="ddlCurrency" style="width: 100%;"></select>
                                        <asp:HiddenField ID="hfCurrencyCode" runat="server" />
                                     </div>
                                 </div>
                            </div>

                    <!-- RIGHT SECTION -->
                    <div style="flex: 0 0 48%;">
                        <div class="mb-2">
                            <strong class="d-block mb-2">STORAGE DIMENSION</strong>
                        </div>
                        <!-- Storage Dimensions -->
                        <div class="form-group mb-3" style="width: 250px;">
                            <label>Site</label>
                            <div>
                                <select id="ddlSite" style="width: 100%;"></select>
                                <asp:HiddenField ID="hfSiteId" runat="server" />
                            </div>
                        </div>

                        <div class="form-group mb-3" style="width: 250px;">
                            <label>Warehouse</label>
                            <div>
                                <select id="ddlWarehouse" style="width: 100%;"></select>
                                <asp:HiddenField ID="hfWarehouseId" runat="server" />
                            </div>
                        </div>



                                <div class="mb-2">
                                    <strong class="d-block mb-2">DATES</strong>
                                </div>
                                <!-- Dates -->
                                <div class="form-group mb-3" style="width: 250px;">
                                    <label>Accounting date</label>
                                    <asp:TextBox ID="dpAccountDate" runat="server"  CssClass="form-control datepicker" />
                                </div>

                                <div class="form-group mb-3" style="width: 250px;">
                                    <label>Requested receipt date</label>
                                    <asp:TextBox ID="dpRequestedReceiptDate" runat="server"  CssClass="form-control datepicker" />
                                </div>

                              
                                <!-- intercompany -->
                                <div class="form-group d-flex align-items-center">
                                    <label class="mb-0 mr-2">Intercompany</label>
                                    <label class="toggle-switch mb-0 mr-2">
                                        <input type="checkbox" id="chkIntercompany" runat="server" onchange="updatetogglelabel(this)" />
                                        <span class="slider"></span>
                                    </label>
                                    <span id="intercompanystatus">Yes</span>
                                </div>

                        <div class="form-group mb-3" style="width: 80px;">
  
    <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control autocomplete-input" />
</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Administration Panel -->
            <a href="#adminPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-4" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="adminPanel">
                <span class="section-title">Administration</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>
            <div class="collapse show mt-2" id="adminPanel">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <!-- Two-section layout -->
                        <div class="d-flex flex-wrap justify-content-between">
                            <!-- Left Section -->
                            <div style="flex: 0 0 48%;">
                                <div class="form-group mb-3" style="width: 250px;">
                                    <label for="ddlBuyerGroup">Buyer group</label>
                                    <div>
                                        <select id="ddlBuyerGroup" style="width: 100%;"></select>
                                        <asp:HiddenField ID="hfBuyerGroupId" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group mb-3" style="width: 250px;">
                                    <label for="ddlOrderer">Orderer</label>
                                    <div>
                                        <select id="ddlOrderer" style="width: 100%;"></select>
                                        <asp:HiddenField ID="hfOrdererId" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group mb-3" style="width: 250px;">
                                    <label for="ddlRequester">Requester</label>
                                    <div>
                                        <select id="ddlRequester" style="width: 100%;"></select>
                                        <asp:HiddenField ID="hfRequesterId" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <!-- Right Section -->
                            <div style="flex: 0 0 48%;">
                                <div class="form-group mb-3" style="width: 250px;">
                                    <label for="ddlPool">Pool</label>
                                    <div>
                                        <select id="ddlPool" style="width: 100%;"></select>
                                        <asp:HiddenField ID="hfPoolId" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group mb-3" style="width: 250px;">
                                    <label for="ddlLanguage">Language <span class="text-danger">*</span></label>
                                    <div>
                                        <select id="ddlLanguage" style="width: 100%;"></select>
                                        <asp:HiddenField ID="hfLanguageId" runat="server" />
                                    </div>
                                </div>
                               <div class="form-group d-flex align-items-center">
    <label class="mb-0 mr-2">Activate change management</label>
    <label class="toggle-switch mb-0 mr-2">
        <input type="checkbox" id="ActivateChangeManagementBox" runat="server" disabled="disabled" />
        <span class="slider"></span>
    </label>
    <span id="ActivateChangeManagement" runat="server"></span>
</div>


                                                          
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Unplanned Purchases Panel -->
            <a href="#unplannedPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-4" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="unplannedPanel">
                <span class="section-title">Unplanned purchases</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>
            <div class="collapse mt-2" id="unplannedPanel">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="form-group mb-3" style="width: 250px;">
                            <label for="ddlcontinuepo">Confirming PO</label>
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control autocomplete-input" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Buttons -->
            <div class="action-footer">
                <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        var select2Loaded = false;

        function pageLoad(sender, args) {
            if (!select2Loaded) {
                var script = document.createElement("script");
                script.src = "https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js";
                script.onload = function () {
                    select2Loaded = true;
                    initSelect2Lookups();
                };
                document.head.appendChild(script);
            } else {
                initSelect2Lookups();
            }
        }

        function setupSelect2(selectId, hiddenFieldId, entityName, placeholder, onChangeCallback, gridConfig) {
            var $select = $('#' + selectId);
            var $hf = $('#' + hiddenFieldId);

            if ($select.hasClass("select2-hidden-accessible")) {
                $select.select2('destroy');
            }

            var select2Options = {
                placeholder: placeholder,
                width: '100%',
                minimumInputLength: 0,
                escapeMarkup: function (markup) { return markup; },
                ajax: {
                    url: '/Api/LookupHandler.ashx',
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            entity: entityName,
                            q: params.term || '',
                            page: params.page || 1
                        };
                    },
                    processResults: function (data, params) {
                        return {
                            results: data.results,
                            pagination: data.pagination
                        };
                    },
                    cache: true
                }
            };

            if (gridConfig && gridConfig.columns) {
                select2Options.templateResult = function (data) {
                    if (data.loading) return data.text;
                    var html = "<div class='select2-result-grid'>";
                    gridConfig.columns.forEach(function (col) {
                        var val = data[col.field] || (col.field === 'id' ? data.id : '');
                        html += "<div class='select2-result-grid__col' style='flex: " + col.flex + ";'>" + val + "</div>";
                    });
                    html += "</div>";
                    return html;
                };
                select2Options.templateSelection = function (data) {
                    return data.text;
                };
            }

            $select.select2(select2Options);

            if (gridConfig && gridConfig.columns) {
                $select.on('select2:open', function () {
                    setTimeout(function () {
                        var $dropdown = $('.select2-dropdown').last();
                        if ($dropdown.find('.select2-grid-header').length === 0) {
                            var headerHTML = "<div class='select2-grid-header select2-result-grid' style='padding: 8px 6px; font-weight: bold; border-bottom: 1px solid #eee; background: #fff; margin-bottom: 4px;'>";
                            gridConfig.columns.forEach(function (col) {
                                headerHTML += "<div class='select2-result-grid__col' style='flex: " + col.flex + "; font-size: 14px; font-weight: 700; color: #000;'>" + col.title + "</div>";
                            });
                            headerHTML += "</div>";
                            $dropdown.find('.select2-search').after(headerHTML);
                        }
                    }, 0);
                });
            }

            if ($hf.val() !== "") {
                var valId = $hf.val();
                if ($select.find("option[value='" + valId + "']").length === 0) {
                    var newOption = new Option(valId, valId, true, true);
                    $select.append(newOption).trigger('change.select2');
                }
            }

            $select.on('select2:select', function (e) {
                var selectedData = e.params.data;
                $hf.val(selectedData.id);
                if (onChangeCallback) {
                    onChangeCallback(selectedData);
                }
            });
        }

        function initSelect2Lookups() {
            var vendorGridConfig = {
                columns: [
                    { title: 'Vendor account', field: 'id', flex: '0 0 120px' },
                    { title: 'Name', field: 'vendorName', flex: '1' }
                ]
            };

            var contactGridConfig = {
                columns: [
                    { title: 'Contact for', field: 'contactFor', flex: '1' },
                    { title: 'Contact ID', field: 'id', flex: '0 0 120px' },
                    { title: 'Name', field: 'personName', flex: '1' }
                ]
            };

            var siteGridConfig = {
                columns: [
                    { title: 'Site', field: 'id', flex: '0 0 120px' },
                    { title: 'Name', field: 'siteName', flex: '1' }
                ]
            };

            var projectGridConfig = {
                columns: [
                    { title: 'Project ID', field: 'id', flex: '0 0 120px' },
                    { title: 'Project Name', field: 'projectName', flex: '1' }
                ]
            };

            var warehouseGridConfig = {
                columns: [
                    { title: 'Warehouse', field: 'id', flex: '0 0 120px' },
                    { title: 'Name', field: 'warehouseName', flex: '1' }
                ]
            };

            var buyerGroupGridConfig = {
                columns: [
                    { title: 'Buyer group', field: 'id', flex: '0 0 120px' },
                    { title: 'Description', field: 'description', flex: '1' }
                ]
            };

            var workerGridConfig = {
                columns: [
                    { title: 'Personnel number', field: 'id', flex: '0 0 120px' },
                    { title: 'Name', field: 'workerName', flex: '1' }
                ]
            };

            var poolGridConfig = {
                columns: [
                    { title: 'Pool', field: 'id', flex: '0 0 120px' },
                    { title: 'Name', field: 'poolName', flex: '1' }
                ]
            };

            var languageGridConfig = {
                columns: [
                    { title: 'Language', field: 'id', flex: '1' }
                ]
            };

            // 1. Vendor Account
            setupSelect2('ddlVendorAccount', '<%= hfVendorAccountId.ClientID %>', 'vendor', 'Search Vendor ID or Name...', function (selectedData) {
                $("[id$='txtVendorName']").val(selectedData.vendorName);
                $("[id$='txtName']").val(selectedData.vendorName);
                $("[id$='txtDeliveryName']").val(selectedData.address);
                $("[id$='txtAddress']").val(selectedData.serviceAddress);
                $("[id$='txtDeliveryAddress']").val(selectedData.address);

                if (selectedData.purchPoolId) {
                    $('#<%= hfPoolId.ClientID %>').val(selectedData.purchPoolId);
                    var $poolSelect = $('#ddlPool');
                    if ($poolSelect.find("option[value='" + selectedData.purchPoolId + "']").length === 0) {
                        $poolSelect.append(new Option(selectedData.purchPoolId, selectedData.purchPoolId, true, true)).trigger('change.select2');
                    } else {
                        $poolSelect.val(selectedData.purchPoolId).trigger('change.select2');
                    }
                }

                if (selectedData.changeRequest === "Yes") {
                    $("[id$='ActivateChangeManagementBox']").prop('checked', true);
                    $("[id$='ActivateChangeManagement']").text('Yes');
                } else {
                    $("[id$='ActivateChangeManagementBox']").prop('checked', false);
                    $("[id$='ActivateChangeManagement']").text('No');
                }

                // Force Update Invoice Account Hidden Field
                $('#<%= hfInvoiceAccountId.ClientID %>').val(selectedData.id);
                var $invSelect = $('#ddlInvoiceAccount');
                if ($invSelect.find("option[value='" + selectedData.id + "']").length === 0) {
                    $invSelect.append(new Option(selectedData.id, selectedData.id, true, true)).trigger('change.select2');
                } else {
                    $invSelect.val(selectedData.id).trigger('change.select2');
                }

                $('#<%= hfCurrencyCode.ClientID %>').val("USD");
                var $curSelect = $('#ddlCurrency');
                if ($curSelect.find("option[value='USD']").length === 0) {
                    $curSelect.append(new Option("USD", "USD", true, true)).trigger('change.select2');
                } else {
                    $curSelect.val("USD").trigger('change.select2');
                }

                $('#<%= hfLanguageId.ClientID %>').val("en-us");
                var $langSelect = $('#ddlLanguage');
                if ($langSelect.find("option[value='en-us']").length === 0) {
                    $langSelect.append(new Option("en-us", "en-us", true, true)).trigger('change.select2');
                } else {
                    $langSelect.val("en-us").trigger('change.select2');
                }
            }, vendorGridConfig);

            // 2. Invoice Account
            setupSelect2('ddlInvoiceAccount', '<%= hfInvoiceAccountId.ClientID %>', 'vendor', 'Search Invoice Account...', function (selectedData) {
                $("[id$='txtName']").val(selectedData.vendorName);
            }, vendorGridConfig);

            // 3. Contact
            setupSelect2('ddlContact', '<%= hfContactId.ClientID %>', 'contact', 'Search Contact...', null, contactGridConfig);

            // 4. Project ID
            setupSelect2('ddlProjectID', '<%= hfProjectId.ClientID %>', 'project', 'Search Project...', null, projectGridConfig);

            // 5. Currency
            setupSelect2('ddlCurrency', '<%= hfCurrencyCode.ClientID %>', 'currency', 'Search Currency...', null);

            // 6. Site
            setupSelect2('ddlSite', '<%= hfSiteId.ClientID %>', 'site', 'Search Site...', null, siteGridConfig);

            // 7. Warehouse
            setupSelect2('ddlWarehouse', '<%= hfWarehouseId.ClientID %>', 'warehouse', 'Search Warehouse...', null, warehouseGridConfig);

            // 8. Buyer Group
            setupSelect2('ddlBuyerGroup', '<%= hfBuyerGroupId.ClientID %>', 'buyergroup', 'Search Buyer Group...', null, buyerGroupGridConfig);

            // 9. Orderer
            setupSelect2('ddlOrderer', '<%= hfOrdererId.ClientID %>', 'worker', 'Search Orderer...', null, workerGridConfig);

            // 10. Requester
            setupSelect2('ddlRequester', '<%= hfRequesterId.ClientID %>', 'worker', 'Search Requester...', null, workerGridConfig);

            // 11. Pool
            setupSelect2('ddlPool', '<%= hfPoolId.ClientID %>', 'pool', 'Search Pool...', null, poolGridConfig);

            // 12. Language
            setupSelect2('ddlLanguage', '<%= hfLanguageId.ClientID %>', 'language', 'Search Language...', null, languageGridConfig);

            // 1. Vendor Account
            setupSelect2('ddlVendorAccount', '<%= hfVendorAccountId.ClientID %>', 'vendor', 'Search Vendor ID or Name...', function (selectedData) {

                // Basic vendor info
                $("[id$='txtVendorName']").val(selectedData.vendorName);
                $("[id$='txtName']").val(selectedData.vendorName);
                $("[id$='txtDeliveryName']").val(selectedData.address);
                $("[id$='txtAddress']").val(selectedData.serviceAddress);
                $("[id$='txtDeliveryAddress']").val(selectedData.address);

                // Pool
                if (selectedData.purchPoolId) {
                    $('#<%= hfPoolId.ClientID %>').val(selectedData.purchPoolId);
        var $poolSelect = $('#ddlPool');
        if ($poolSelect.find("option[value='" + selectedData.purchPoolId + "']").length === 0) {
            $poolSelect.append(new Option(selectedData.purchPoolId, selectedData.purchPoolId, true, true)).trigger('change.select2');
        } else {
            $poolSelect.val(selectedData.purchPoolId).trigger('change.select2');
        }
    }

    // Change management
    if (selectedData.changeRequest === "Yes") {
        $("[id$='ActivateChangeManagementBox']").prop('checked', true);
        $("[id$='ActivateChangeManagement']").text('Yes');
    } else {
        $("[id$='ActivateChangeManagementBox']").prop('checked', false);
        $("[id$='ActivateChangeManagement']").text('No');
    }

    // Invoice account (mirrors vendor account)
    $('#<%= hfInvoiceAccountId.ClientID %>').val(selectedData.id);
    var $invSelect = $('#ddlInvoiceAccount');
    if ($invSelect.find("option[value='" + selectedData.id + "']").length === 0) {
        $invSelect.append(new Option(selectedData.id, selectedData.id, true, true)).trigger('change.select2');
    } else {
        $invSelect.val(selectedData.id).trigger('change.select2');
    }

    // Currency default
    $('#<%= hfCurrencyCode.ClientID %>').val("USD");
    var $curSelect = $('#ddlCurrency');
    if ($curSelect.find("option[value='USD']").length === 0) {
        $curSelect.append(new Option("USD", "USD", true, true)).trigger('change.select2');
    } else {
        $curSelect.val("USD").trigger('change.select2');
    }

    // Language default
    $('#<%= hfLanguageId.ClientID %>').val("en-us");
    var $langSelect = $('#ddlLanguage');
    if ($langSelect.find("option[value='en-us']").length === 0) {
        $langSelect.append(new Option("en-us", "en-us", true, true)).trigger('change.select2');
    } else {
        $langSelect.val("en-us").trigger('change.select2');
    }

    // --- NEW: Site ---
    if (selectedData.siteId) {
        $('#<%= hfSiteId.ClientID %>').val(selectedData.siteId);
        var $siteSelect = $('#ddlSite');
        if ($siteSelect.find("option[value='" + selectedData.siteId + "']").length === 0) {
            $siteSelect.append(new Option(selectedData.siteId, selectedData.siteId, true, true)).trigger('change.select2');
        } else {
            $siteSelect.val(selectedData.siteId).trigger('change.select2');
        }
    }

    // --- NEW: Warehouse (LocationID) ---
    if (selectedData.locationId) {
        $('#<%= hfWarehouseId.ClientID %>').val(selectedData.locationId);
        var $whSelect = $('#ddlWarehouse');
        if ($whSelect.find("option[value='" + selectedData.locationId + "']").length === 0) {
            $whSelect.append(new Option(selectedData.locationId, selectedData.locationId, true, true)).trigger('change.select2');
        } else {
            $whSelect.val(selectedData.locationId).trigger('change.select2');
        }
    }

    // --- NEW: Intercompany checkbox ---
    var isIntercompany = (selectedData.isInterCompanyVendor || "").toString().toLowerCase() === "yes";
    $("[id$='chkIntercompany']").prop('checked', isIntercompany);
    $("#intercompanystatus").text(isIntercompany ? "Yes" : "No");

    // --- NEW: Intercompany partner company name ---
    $("[id$='txtCompany']").val(selectedData.interCompanyPartnerCompanyName || "");

}, vendorGridConfig);
        }
    </script>

    <script>
        function updateToggleLabel(checkbox) {
            document.getElementById("toggleStatus").textContent = checkbox.checked ? "Yes" : "No";
        }

        // Optional: Update on page load (if checkbox state is remembered from server)
        window.onload = function () {
            const chk = document.getElementById("chkTransferLines");
            if (chk) updateToggleLabel(chk);
        };
    </script>
    <style>
        .autocomplete-input {
            width: 300px;
            height: 30px;
            font-size: 11px;
            font-family: Arial, sans-serif;
            border: 1px solid #444;
            border-radius: 0px;
            padding: 1px 4px;
            box-sizing: border-box;
            outline: none;
            background-color: white;
        }

    </style>
    <script>
        function applyAutocomplete() {
            $('.filterable-dropdown').each(function () {
                var $dropdown = $(this);

                // Avoid adding input twice
                if ($dropdown.next('.autocomplete-input').length === 0) {
                    var options = [];

                    $dropdown.find('option').each(function () {
                        if ($(this).val()) {
                            options.push({
                                label: $(this).text(),
                                value: $(this).val()
                            });
                        }
                    });

                    var placeholderText = '';
                    var dropdownId = $dropdown.attr('id')?.toLowerCase();

                    switch (dropdownId) {
                        case "ddluserid":
                            placeholderText = '-- Select User --';
                            break;
                        case "ddlroleid":
                            placeholderText = '-- Select Role --';
                            break;
                    }

                    var $input = $('<input type="text" class="autocomplete-input form-control" />')
                        .attr('placeholder', placeholderText)
                        .val($dropdown.val()) // show only ItemId
                        .insertAfter($dropdown)
                        .autocomplete({
                            source: options,
                            minLength: 0,
                            select: function (event, ui) {
                                $dropdown.val(ui.item.value);
                                $dropdown.trigger('change'); // ✅ ASP.NET postback trigger
                            }
                        })
                        .on("focus", function () {
                            $(this).autocomplete("search", ""); // Show all options on focus
                        })
                        .on("input", function () {
                            var typedText = $(this).val().trim().toLowerCase();
                            var matchedOption = options.find(opt => opt.label.toLowerCase() === typedText);

                            //if (!matchedOption) {
                            //    // Clear dropdown value and fields
                            //    $dropdown.val('');
                            //    fetchVendorName($dropdown[0]);
                            //}
                        });

                    $dropdown.hide();
                }
            });
        }

        // Initialize on document ready and after UpdatePanel postbacks
        $(document).ready(function () {
            applyAutocomplete();

            if (Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(applyAutocomplete);
            }
        });
    </script>
    <%--<script type="text/javascript">
        function fetchVendorName(dropdown) {
            var vendAccount = dropdown.value;

            var txtVendorName = document.getElementById("txtVendorName");
            var txtDeliveryName = document.getElementById("txtDeliveryName");
            var txtAddress = document.getElementById("txtAddress");
            var txtdeliveryaddress = document.getElementById("txtDeliveryAddress");

            var txtName = document.getElementById("txtName"); // Vendor Name textbox

            if (!vendAccount) {
                txtVendorName.value = "";
                txtDeliveryName.value = "";
                txtAddress.value = "";
                txtdeliveryaddress.value = "";
                return;
            }


            // Vendor Name
            PageMethods.GetVendorName(vendAccount, function (result) {
                txtVendorName.value = result;
                txtName.value = result;
            });

            // Delivery Name
            PageMethods.GetPostalAddress(vendAccount, function (result) {
                txtAddress.value = result;
            });

            // Address
            PageMethods.GetLocationDescription(vendAccount, function (result) {
                txtDeliveryName.value = result;
            });

            PageMethods.GetLocationDescription(vendAccount, function (result) {
                txtdeliveryaddress.value = result;
            });

        }
    </script>
    <script type="text/javascript">
        function fetchInvoiceVendorName(dropdown) {
            var vendAccount = dropdown.value;

            var txtName = document.getElementById("txtName");

            if (!vendAccount) {
                txtName.value = "";
                return;
            }

            PageMethods.GetVendorNameforInvoice(vendAccount, function (result) {
                txtName.value = result;
            });
        }
    </script>--%>
    <script>
        $(document).ready(function() {
            // ... existing ready logic if any ...
        });

        function populateVendorFields(row) {
            var $row = $(row);
            var vendorName = $row.attr('data-vendorname');
            var address = $row.attr('data-address');
            var serviceAddress = $row.attr('data-serviceaddress');
            var changeRequest = $row.attr('data-changerequest');
            var purchPoolId = $row.attr('data-purchpoolid');
            var vendorAccount = $row.find('td:first').text().trim();

            // Update main vendor fields
            $("[id$='txtVendorName']").val(vendorName);
            $("[id$='txtName']").val(vendorName);
            $("[id$='txtDeliveryName']").val(address);
            $("[id$='txtAddress']").val(serviceAddress);
            $("[id$='txtDeliveryAddress']").val(address);

            // Update Pool dropdown
            if (purchPoolId) {
                $('#<%= hfPoolId.ClientID %>').val(purchPoolId);
                var $poolSelect = $('#ddlPool');
                if ($poolSelect.find("option[value='" + purchPoolId + "']").length === 0) {
                    $poolSelect.append(new Option(purchPoolId, purchPoolId, true, true)).trigger('change.select2');
                } else {
                    $poolSelect.val(purchPoolId).trigger('change.select2');
                }
            }

            // Update Change Management
            if (changeRequest === "Yes") {
                $("[id$='ActivateChangeManagementBox']").prop('checked', true);
                $("[id$='ActivateChangeManagement']").text('Yes');
            } else {
                $("[id$='ActivateChangeManagementBox']").prop('checked', false);
                $("[id$='ActivateChangeManagement']").text('No');
            }

            // Update Invoice Account lookup (internal textbox)
            $("[id$='DropDownList_ddlInvoiceAccount'] [id$='txtVendorAccountId']").val(vendorAccount);
            
            // Hardcoded defaults to match server-side behavior
            $("[id$='DropDownList_ddlCurrency'] [id$='txtCurrencyCode']").val("USD");
            
            $('#<%= hfLanguageId.ClientID %>').val("en-us");
            var $langSelect = $('#ddlLanguage');
            if ($langSelect.find("option[value='en-us']").length === 0) {
                $langSelect.append(new Option("en-us", "en-us", true, true)).trigger('change.select2');
            } else {
                $langSelect.val("en-us").trigger('change.select2');
            }
        }

    </script>
    <script>
        $(document).ready(function () {
            let today = new Date();

            let formatted =
                (today.getMonth() + 1) + "/" +
                today.getDate().toString().padStart(2, "0") + "/" +
                today.getFullYear();

            $("#<%= dpAccountDate.ClientID %>").val(formatted);
        $("#<%= dpRequestedReceiptDate.ClientID %>").val(formatted);
    });
    </script>
    <script>
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: "yy-mm-dd", // Format like 2025-11-12
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
    <script>
        function initDatePickers() {
            $(".datepicker").datepicker({
                dateFormat: "yy-mm-dd",
                changeMonth: true,
                changeYear: true
            });
        }

        // Initialize when page loads
        $(document).ready(function () {
            initDatePickers();
        });

        // Re-initialize after UpdatePanel postback
        if (Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                initDatePickers();
            });
        }
    </script>
</asp:Content>
