<%@ Page Title="Transfer Order Lines" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransferOrderLines_ListPage.aspx.cs" Inherits="DynamicsPortal.TransferOrderLines_ListPage" %>

<%@ Register Src="~/DropDownList_ProductCombination.ascx" TagPrefix="uc" TagName="ProductLookup" %>
<%--<%@ Register Src="~/DropDownList_ItemDetails.ascx" TagPrefix="uc" TagName="ItemLookup" %>--%>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
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

                    var $wrapper = $('<div class="autocomplete-wrapper" style="position: relative;"></div>');
                    $dropdown.wrap($wrapper);
                    var $parentWrapper = $dropdown.parent('.autocomplete-wrapper');
                    var $input = $('<input type="text" class="autocomplete-input form-control" />')
                        .attr('placeholder', placeholderText)
                        .val($dropdown.find("option:selected").text())
                        .insertAfter($dropdown)
                        .autocomplete({
                            source: options,
                            minLength: 0,
                            appendTo: $parentWrapper,
                            select: function (event, ui) {
                                $dropdown.val(ui.item.value);
                                $dropdown.trigger('change');
                            },
                            open: function () {
                                // Ensure the menu is styled to stay with the input
                                $(this).autocomplete('widget').css({
                                    'width': $(this).outerWidth() + 'px', // Match input width
                                    'z-index': 1000 // Ensure it appears above other elements
                                });
                            }
                        })
                        .on("focus", function () {
                            $(this).autocomplete("search", "");
                        })
                        .on("blur", function () {
                            var typedText = $(this).val().trim();
                            var match = options.find(o => o.label === typedText);

                            if (match) {
                                // Valid option: sync dropdown value
                                $dropdown.val(match.value);
                                $dropdown.trigger('change');
                            } else {
                                // Invalid: clear both
                                $dropdown.val('');
                                $(this).val('');
                            }
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
    <script type="text/javascript">
        // Trigger file input click when Upload Excel button is clicked
        function triggerFileUpload() {
            document.getElementById('<%= fileUploadExcel.ClientID %>').click();
            return false; // Prevent default LinkButton postback
        }

        // Trigger LinkButton postback after file selection
        function onFileSelected() {
            __doPostBack('<%= btnUploadExcel.UniqueID %>', '');
        }
    </script>
   <%-- <script type="text/javascript">
        window.onload = function () {
            const selectAll = document.getElementById('<%= gridView.ClientID %>').querySelector('#chk_SelectAll');
            selectAll.addEventListener('change', function () {
                const checkboxes = document.querySelectorAll('[id$=chk_SelectSingle]');
                checkboxes.forEach(cb => cb.checked = selectAll.checked);
            });
        };
    </script>--%>
    <script type="text/javascript">
        $(function () {
            var gridViewId = '<%= gridView.ClientID %>';

        $(document).on('change', '#' + gridViewId + ' #chk_SelectAll', function () {
            var isChecked = $(this).is(':checked');
            $('[id*="chk_SelectSingle"]').prop('checked', isChecked);
        });
    });
</script>
    <script type="text/javascript">
        function applyDatePicker() {
            $(".datepicker").datepicker({
                dateFormat: "m-d-yy" // Example: 9-12-2025
            });
        }

        $(function () {
            // Apply on initial page load
            applyDatePicker();

            // Re-apply after UpdatePanel async postback
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                applyDatePicker();
            });
        });
    </script>
    <style>
        .custom-textbox[readonly] {
            color: gray !important; /* ForeColor */
            background-color: #F3F2F1 !important; /* BackColor */
        }
       
    .readonly-dropdown {
        pointer-events: none;
        background-color: #e9ecef;
    }
</style>
  

</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>
            <div class="action-items">
                <%---  <asp:LinkButton ID="btnTransferOrder" runat="server" OnClientClick="window.location.href='/ESS/PR/TransferOrder_ListPage.aspx'; return false;">  ----%>
                <asp:LinkButton ID="btnTransferOrder" runat="server" OnClientClick="var ref = document.referrer; if (ref.includes('AllTransferOrderListPage.aspx')) { window.location.href='/ESS/PR/AllTransferOrderListPage.aspx'; } else { window.location.href='/ESS/PR/TransferOrder_ListPage.aspx'; } return false;">
    <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
                </asp:LinkButton>
                <%-- <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
                </asp:LinkButton>-----%>
            </div>
            <div class="action-items">
                <asp:LinkButton ID="BtnSaveHeader" runat="server" OnClick="BtnSave_Header_Click">
        <i class="mdi mdi-content-save" style="margin-right: 4px;"></i>Save
                </asp:LinkButton>
            </div>
            <div class="action-items">
                <asp:LinkButton ID="btnShip_All" OnClick="btnShip_Click" runat="server">
        <i class="mdi mdi-truck-fast"></i> Ship
                </asp:LinkButton>
            </div>

            <div class="action-items">
                <asp:LinkButton ID="btnRecieve" OnClick="btnRecieve_Click" runat="server">
        <i class="mdi mdi-package-down"></i> Receive
                </asp:LinkButton>
            </div>

            <div class="action-items">
                <asp:LinkButton ID="btnHistory" runat="server" OnClick="btnHistory_Click">
                    <i class="mdi mdi-file-chart"></i> Transfer Order History
                </asp:LinkButton>
            </div>


            <div class="action-items">
                <asp:LinkButton ID="btnReport" runat="server" OnClick="btnViewReport">
        <i class="mdi mdi-file-chart"></i> Transfer Order Overview Report
                </asp:LinkButton>
            </div>

            <div class="action-items">
                <asp:LinkButton ID="btnReport2" runat="server" OnClick="btnViewReport_Onhand">
        <i class="mdi mdi-barcode-scan"></i> On-hand Inventory List
                </asp:LinkButton>
            </div>

            <div class="action-items">
                <asp:FileUpload ID="fileUploadExcel" runat="server" AllowMultiple="false" accept=".xlsx,.xls" Style="display: none;" onchange="onFileSelected();" />
                <asp:LinkButton ID="btnUploadExcel" runat="server" OnClientClick="return triggerFileUpload();" OnClick="btnUploadExcel_Click">
            <i class="mdi mdi-upload"></i>Upload Lines
                </asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="detailUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HiddenField ID="hfTransferID" runat="server" />
            <asp:HiddenField ID="hdnRefreshGrid" runat="server" />
            <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="alert alert-danger" Style="font-size: 12px; padding: 6px; display: block;" />
            <div style="display: flex; justify-content: space-between; align-items: center; width: 100%;">
                <!-- Left side labels -->
                <div>
                    <asp:Label ID="lblTransferID" runat="server"></asp:Label>
                </div>

                <!-- Right side label -->
                <div>
                    <asp:Label ID="lblTransferOrderStatus" runat="server"></asp:Label>
                </div>
            </div>

            <div class="card shadow-sm">
                <div class="card-header bg-light">
                    <ul class="nav nav-tabs" id="mainTabs" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" id="lines-tab" data-toggle="tab" href="#lines" role="tab">Lines</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="header-tab" data-toggle="tab" href="#header" role="tab">Header</a>
                        </li>
                    </ul>
                </div>
                <div class="tab-content">
                    <div class="tab-pane fade show active" id="lines" role="tabpanel">
                        <a href="#transferOrderHeaderPanel"
                            class="d365-toggle-header d-flex justify-content-between align-items-center mt-0"
                            data-toggle="collapse"
                            role="button"
                            aria-expanded="true"
                            aria-controls="transferOrderHeaderPanel">
                            <span class="section-title">Transfer order header</span>
                            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                        </a>
                        <div class="table-container">
                            <div class="collapse mt-0" id="transferOrderHeaderPanel">
                                <div class="card shadow-sm">
                                    <div class="card-body">
                                        <div class="row g-3">

                                            <!-- Column 1: OVERVIEW - Transfer Number -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>OVERVIEW</strong></div>
                                                <div class="info-block">
                                                    <strong>Transfer Number</strong>
                                                    <asp:TextBox ID="txtTransferNumber" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                                </div>
                                            </div>

                                            <!-- Column 2: From Warehouse (parallel to OVERVIEW, no heading) -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                                <div class="info-block">
                                                    <strong>From warehouse</strong>
                                                    <asp:TextBox ID="txtFromWarehouse" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                                </div>
                                            </div>

                                            <!-- Column 3: To Warehouse -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                                <div class="info-block">
                                                    <strong>To warehouse</strong>
                                                    <asp:TextBox ID="txtToWarehouse" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Ship date</strong>
                                                    <asp:TextBox ID="txtShipDate" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Receipt date</strong>
                                                    <asp:TextBox ID="txtReceiptDate" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>STATUS</strong></div>

                                                <div class="info-block">
                                                    <strong>Transfer status</strong>
                                                    <asp:TextBox ID="txtTransferStatus" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <a class="d365-toggle-header d-flex justify-content-between align-items-center mt-3">
                                <span class="section-title">Transfer order lines</span>
                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>

                            </a>
                            <div class="card shadow-sm">
                                <div class="card-body pt-2 pb-2">
                                    <div class="text-left d-flex align-items-center gap-4">
                                        <div class="action-items grid-btn">
                                            <asp:LinkButton ID="BtnAddLine" runat="server" OnClick="btnNew_Grid_Click"
                                                CssClass="text-primary" Style="font-size: 0.9rem;">
                <i class="mdi mdi-plus" style="font-size: 0.9rem;"></i> Add
                                            </asp:LinkButton>
                                        </div>
                                        <div class="action-items grid-btn" style="margin-left: 30px;">
                                            <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"
                                                Style="font-size: 0.9rem;">
                <i class="mdi mdi-delete" style="font-size: 0.9rem;"></i> Remove
                                            </asp:LinkButton>
                                        </div>
                                        <div class="action-items grid-btn">
                                            <asp:LinkButton ID="Remainderbtn" runat="server" OnClick="btnRemainder_Click">Deliver Remainder
                                            </asp:LinkButton>
                                        </div>

                                        <div class="action-items grid-btn" style="margin-left: 30px;">
                                            <asp:LinkButton ID="btnOnHand" runat="server" OnClick="btnOnHand_Click"
                                                Style="font-size: 0.9rem;">
                <i class="mdi mdi-database" style="font-size: 0.9rem;"></i> On-hand
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                  
                    <div class="transfer-line-card-body shadow-sm ">
                        <asp:GridView ID="gridView" runat="server" Data="searchable" CssClass="table table-condensed no-border table-hover sortable grid-size" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
                            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
                            <Columns>
                                 <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
      <HeaderTemplate>
         <input type="checkbox" id="chk_SelectAll" CssClass="round-checkbox" />
         </HeaderTemplate>
         <ItemTemplate>
             <asp:CheckBox ID="chk_SelectSingle"
                 OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true"
                 runat="server" CssClass="round-checkbox" />
         </ItemTemplate>
 </asp:TemplateField>
                             

                                <asp:TemplateField HeaderText="Transfer Number" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferID" runat="server" Text='<%# Bind("TransferID") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTransferID" runat="server" Text='<%# Bind("TransferID") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Item number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemID" runat="server" Text='<%# Bind("ItemId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlItemId" OnSelectedIndexChanged="ddlTemId_fillDimension" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <%--<uc:ItemLookup ID="cddlItemDetails" runat="server" />--%>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transfer quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyTransfer" runat="server" CssClass="format-number" Text='<%# Bind("QtyTransfer") %>' Style="text-align: right; display: block;"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtQtyTransfer" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CW transfer Qty" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCWQtyTransfer" runat="server" Text='<%# Bind("CWQtyTransfer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ship date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinesShipDate" runat="server" Text='<%# Bind("LinesShipDate", "{0:M-d-yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%-- <EditItemTemplate>
                                            <asp:TextBox ID="txtshipLineDate" runat="server" CssClass="datepicker" ></asp:TextBox>
                                        </EditItemTemplate>--%>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Receipt date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinesReceiveDate" runat="server" Text='<%# Bind("LinesReceiveDate", "{0:M-d-yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%--  <EditItemTemplate>
                                            <asp:TextBox ID="txtreceiveLineDate" runat="server" CssClass="datepicker" ></asp:TextBox>
                                        </EditItemTemplate>--%>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Combinations">
                                    <EditItemTemplate>
                                        <div style="min-width: 400px; overflow: visible;">
                                            <uc:ProductLookup ID="ProductLookupControl" runat="server" onproductselected="ProductLookupControl_ProductSelected" />
                                    </EditItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Configuration">
                                    <ItemTemplate>
                                        <asp:Label ID="lblConfigId" runat="server" Text='<%# Bind("ConfigId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlConfigId" runat="server"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Color">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventColorId" runat="server" Text='<%# Bind("InventColorId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInventColorId" runat="server"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Size">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventSizeId" runat="server" Text='<%# Bind("InventSizeId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInventSizeId" runat="server"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Style">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventStyleId" runat="server" Text='<%# Bind("InventStyle") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInventStyleId" runat="server"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Site">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventSiteId" runat="server" Text='<%# Bind("InventSiteId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%--           <EditItemTemplate>
                                            <asp:DropDownList ID="ddlInventSiteId" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warehouse">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventLocationId" runat="server" Text='<%# Bind("InventLocationId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%-- <EditItemTemplate>
                                            <asp:DropDownList ID="ddlInventLocationId" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>--%>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Batch number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventBatchId" runat="server" Text='<%# Bind("InventBatchId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInventBatchId" runat="server"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWMSLocationId" runat="server" Text='<%# Bind("WMSLocationId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlWMSLocationId" runat="server"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="WMS pallet" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWMSPalletId" runat="server" Text='<%# Bind("WMSPalletId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlWMSPalletId" runat="server"></asp:DropDownList>
                                    </EditItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Serial number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventSerialId" runat="server" Text='<%# Bind("InventSerialId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInventSerialId" runat="server"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Transaction Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransctionCode" runat="server" Text='<%# Bind("transctioncode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unit ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitId" runat="server" Text='<%# Bind("UnitId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TransferID" HeaderText="TransferID" Visible="false" />
                                <asp:TemplateField HeaderText="RecId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ReserveItem" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReserveItem" runat="server" Text='<%# Bind("ReserveItem") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qtyshipped" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyshipped" runat="server" Text='<%# Bind("Qtyshipped") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dimensionshipfrom" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDimensionshipfrom" runat="server" Text='<%# Bind("Dimensionshipfrom") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dimensionshipto" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDimensionshipto" runat="server" Text='<%# Bind("Dimensionshipto") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IsCatchWeight" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsCatchWeight" runat="server" Text='<%# Bind("IsCatchWeight") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventDimId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventDimId" runat="server" Text='<%# Bind("InventDimId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LocationIdFrom" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationIdFrom" runat="server" Text='<%# Bind("LocationIdFrom") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LocationIdTo" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationIdTo" runat="server" Text='<%# Bind("LocationIdTo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TransferStatus" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferStatus" runat="server" Text='<%# Bind("inventtransferstatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NewTransitLocationName" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewTransitLocationName" runat="server" Text='<%# Bind("NewTransitLocationName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CreatedDateTime" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedDateTime" runat="server" Text='<%# Bind("CreatedDateTime") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LineNum" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineNum" runat="server" Text='<%# Bind("LineNum") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty Received" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyRecieved" runat="server" Text='<%# Bind("QtyRecieved") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty Receive Now" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyReceiveNow" runat="server" Text='<%# Bind("QtyRecieveNow") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty Remain Receive" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyRemainReceive" runat="server" Text='<%# Bind("QtyRemainRecieve") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty Remain Ship" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyRemainShip" runat="server" Text='<%# Bind("QtyRemainShip") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty Scrapped" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyScrapped" runat="server" Text='<%# Bind("QtyScrapped") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty Ship Now" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyShipNow" runat="server" Text='<%# Bind("QtyShipNow") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remain Status" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemainStatus" runat="server" Text='<%# Bind("RemainStatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventTransId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventTransId" runat="server" Text='<%# Bind("InventTransId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventTransId Receive" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventTransIdRecive" runat="server" Text='<%# Bind("InventTransIdRecive") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventTransId Scrap" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventTransIdScrap" runat="server" Text='<%# Bind("InventTransIdScrap") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventTransId From" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventTransIdFrom" runat="server" Text='<%# Bind("InventTransIdFrom") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventTransId To" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventTransIdTo" runat="server" Text='<%# Bind("InventTransIdTo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                    <ItemTemplate>
                                        <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <a href="#detailPanel"
                        class="d365-toggle-header d-flex justify-content-between align-items-center"
                        data-toggle="collapse"
                        role="button"
                        aria-expanded="true"
                        aria-controls="detailPanel">
                        <span class="section-title">Line details</span>
                        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                    </a>

                    <div class="collapse show mt-4" id="detailPanel">
                        <div class="card shadow-sm">
                            <div class="card-header bg-light">
                                <ul class="nav nav-tabs" id="detailTabs" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link active" id="general-tab" data-toggle="tab" href="#general" role="tab">General</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" id="ship-tab" data-toggle="tab" href="#ship" role="tab">Ship now</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" id="receive-tab" data-toggle="tab" href="#receive" role="tab">Receive now</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" id="dimensions-tab" data-toggle="tab" href="#dimensions" role="tab">Dimensions</a>
                                    </li>
                                </ul>
                            </div>

                            <div class="card-body tab-content" id="detailTabContent">
                                <div class="tab-pane fade show active" id="general" role="tabpanel">
                                    <div class="row g-4">
                                        <!-- spacing between blocks -->

                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>IDENTIFICATION</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Transfer number</strong>
                                                <asp:TextBox ID="LineDetailTransferNumber" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Line number</strong>
                                                <asp:TextBox ID="txtLineNumber" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <!-- STATUS -->
                                            <div class="custom-section-title mb-2"><strong>STATUS</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Remaining</strong>
                                                <asp:TextBox ID="txtRemaining" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>

                                            <!-- INVENTORY -->
                                            <div class="custom-section-title mb-2 mt-3"><strong>INVENTORY</strong></div>

                                            <div class="info-block">
                                                <strong>Shipment lot ID</strong>
                                                <asp:TextBox ID="txtShipmentLotID" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <!-- empty heading to align -->

                                            <div class="info-block mb-3">
                                                <strong>Transit shipment lot ID</strong>
                                                <asp:TextBox ID="txtTransitShipmentLotID" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Transit receive lot ID</strong>
                                                <asp:TextBox ID="txtTransitReceiveLotID" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <!-- empty heading to align -->
                                            <div class="info-block mb-3">
                                                <strong>Receive lot ID</strong>
                                                <asp:TextBox ID="txtReceiveLotID" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>

                                            <div class="info-block mb-3">
                                                <strong>Scrap lot ID</strong>
                                                <asp:TextBox ID="txtScrapLotID" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Dimension number</strong>
                                                <asp:TextBox ID="txtDimensionNumber" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>Voyages</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Voyage</strong>
                                                <asp:TextBox ID="txtVoyage" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Voyage status</strong>
                                                <asp:TextBox ID="txtVoyageStatus" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <!-- empty heading to align -->

                                            <div class="info-block mb-3">
                                                <strong>Arrival group</strong>
                                                <asp:TextBox ID="txtArrivalGroup" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Shipping container</strong>
                                                <asp:TextBox ID="txtShippingContainer" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>
                                        </div>


                                    </div>
                                </div>


                                <div class="tab-pane fade" id="ship" role="tabpanel">
                                    <h6 class="custom-section-title">Shipment details</h6>
                                    <div class="info-row">
                                        <div class="info-block"><strong>Item number</strong><span><asp:Label ID="shipitemnumber" runat="server" Text="N/A" /></span></div>
                                        <div class="info-block"><strong>Transfer quantity</strong><span><asp:Label ID="shiptransferquantity" runat="server" Text="N/A" /></span></div>
                                        <asp:Panel CssClass="info-block" ID="lblShipNow" runat="server">
                                            <strong>Ship Now</strong><span><asp:Label ID="shipnowLabel" runat="server" /></span>
                                        </asp:Panel>

                                        <asp:Panel ID="txtshipNow" CssClass="info-block" Visible="false" runat="server">
                                            <div>
                                                <strong>Ship Now</strong>
                                                <span class="editable-wrapper">
                                                    <asp:TextBox ID="shipnow" OnTextChanged="shipnow_updateLine" AutoPostBack="true" runat="server" CssClass="editable-input" />
                                                    <i class="mdi mdi-pencil edit-icon"></i>
                                                </span>
                                            </div>
                                        </asp:Panel>

                                        <div class="info-block"><strong>Shipped quantity</strong><span><asp:Label ID="shipshippedquantity" runat="server" Text="N/A" /></span></div>
                                        <div class="info-block"><strong>Ship remaining</strong><span><asp:Label ID="shipremaining" runat="server" Text="N/A" /></span></div>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="receive" role="tabpanel">
                                    <h6 class="custom-section-title">Receive details</h6>
                                    <div class="info-row">
                                        <div class="info-block"><strong>Item number</strong><span><asp:Label ID="receiveitemnumber" runat="server" Text="N/A" /></span></div>
                                        <div class="info-block"><strong>Shipped quantity</strong><span><asp:Label ID="receivequantity" runat="server" Text="N/A" /></span></div>
                                        <asp:Panel CssClass="info-block" ID="lblReceiveNow" runat="server">
                                            <strong>Receive Now</strong><span><asp:Label ID="receivenoelabel" runat="server" /></span>
                                        </asp:Panel>
                                        <asp:Panel CssClass="info-block" ID="txtReceiveNow" Visible="false" runat="server">
                                            <div>
                                                <strong>Receive Now</strong>
                                                <span class="editable-wrapper">
                                                    <asp:TextBox ID="receivenow" AutoPostBack="true" OnTextChanged="receivenow_updateLine" runat="server" CssClass="editable-input" />
                                                    <i class="mdi mdi-pencil edit-icon"></i>
                                                </span>
                                            </div>
                                        </asp:Panel>
                                        <div class="info-block"><strong>Received quantity</strong><span><asp:Label ID="receivereceivedquantity" runat="server" Text="N/A" /></span></div>
                                        <div class="info-block"><strong>Receive remaining</strong><span><asp:Label ID="receiveremaining" runat="server" Text="N/A" /></span></div>
                                    </div>
                                </div>

                                <div class="tab-pane fade" id="dimensions" role="tabpanel">
                                    <div class="row g-4">
                                        <!-- spacing between blocks -->

                                        <!-- INVENTORY DIMENSIONS -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>Inventory dimensions</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Configuration</strong>
                                                <asp:DropDownList ID="ddlConfigurationLineDetail" runat="server"  CssClass="form-control readonly-dropdown" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Size</strong>
                                                <asp:DropDownList ID="ddlSizeLineDetail" runat="server" CssClass="form-control readonly-dropdown" />
                                            </div>
                                        </div>

                                        <!-- INVENTORY DIMENSIONS Continued -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <!-- empty heading to align -->

                                            <div class="info-block mb-3">
                                                <strong>Color</strong>
                                                <asp:DropDownList ID="ddlColorLineDetail" runat="server" CssClass="form-control" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Style</strong>
                                                <asp:DropDownList ID="ddlStyleLineDetail" runat="server" CssClass="form-control readonly-dropdown"/>
                                            </div>
                                        </div>

                                        <!-- INVENTORY DIMENSIONS Continued -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <!-- empty heading to align -->

                                            <div class="info-block mb-3">
                                                <strong>Version</strong>
                                                <asp:TextBox ID="txtVersion" runat="server" Text="" CssClass="form-control custom-textbox" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Site</strong>
                                                <asp:TextBox ID="txtSiteLineDetail" ReadOnly="true" runat="server" CssClass="form-control custom-textbox" />
                                            </div>
                                        </div>

                                        <!-- INVENTORY DIMENSIONS Continued -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <!-- empty heading to align -->

                                            <div class="info-block mb-3">
                                                <strong>Warehouse</strong>
                                                <asp:TextBox ID="txtWarehouseLineDetail" ReadOnly="true" runat="server" CssClass="form-control custom-textbox" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Batch number</strong>
                                                <asp:DropDownList ID="ddlBatchLineDetail" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>

                                        <!-- INVENTORY DIMENSIONS Continued -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <!-- empty heading to align -->

                                            <div class="info-block mb-3">
                                                <strong>Location</strong>
                                                <asp:DropDownList ID="ddlWmsLocationLineDetail" runat="server" CssClass="form-control readonly-dropdown" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Serial number</strong>
                                                <asp:DropDownList ID="ddlInventSerialLineDetail" runat="server" CssClass="form-control readonly-dropdown" />
                                            </div>
                                        </div>

                                        <!-- INVENTORY DIMENSIONS Continued -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <!-- empty heading to align -->

                                            <div class="info-block mb-3">
                                                <strong>Inventory status</strong>
                                                <asp:TextBox ID="txtInventoryStatus" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox"  />
                                            </div>

                                            <div class="info-block mb-3">
                                                <strong>License plate</strong>
                                                <asp:TextBox ID="txtLicensePlate" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox"   />
                                            </div>

                                            <div class="info-block">
                                                <strong>Owner</strong>
                                                <asp:TextBox ID="txtOwner" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox"  />
                                            </div>
                                        </div>


                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                   
               
                <div class="tab-pane fade" id="header" role="tabpanel">
                    <!-- General Panel -->
                    <a href="#headerGeneralPanel"
                        class="d365-toggle-header d-flex justify-content-between align-items-center"
                        data-toggle="collapse"
                        role="button"
                        aria-expanded="true"
                        aria-controls="headerGeneralPanel">
                        <span class="section-title">General</span>
                        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                    </a>
                    <div class="collapse show mt-2" id="headerGeneralPanel">
                        <div class="card shadow-sm">
                            <div class="card-body">
                                <div class="row g-4">
                                    <!-- spacing between blocks -->

                                    <!-- STATUS -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>Status</strong></div>

                                        <div class="info-block">
                                            <strong>Transfer status</strong>
                                            <asp:TextBox ID="txttransferstatusforheader" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                        </div>
                                    </div>

                                    <!-- TRANSFER ORDER - Created date and time -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>Transfer Order</strong></div>

                                        <div class="info-block">
                                            <strong>Created date and time</strong>
                                            <asp:TextBox ID="txtCreatedDateTime" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                        </div>
                                    </div>

                                    <!-- From warehouse -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <!-- empty heading to align -->

                                        <div class="info-block">
                                            <strong>From warehouse</strong>
                                            <asp:TextBox ID="txtfromwarehousegeneraltab" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                        </div>
                                    </div>

                                    <!-- Transit warehouse -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <!-- empty heading to align -->

                                        <div class="info-block">
                                            <strong>Transit warehouse</strong>
                                            <%--            <asp:TextBox ID="txtTransitWarehouse" ReadOnly="true" runat="server" Text="" CssClass="form-control" />--%>
                                            <asp:DropDownList
                                                ID="ddlTransitWarehouse"
                                                runat="server"
                                                CssClass="form-control custom-textbox">
                                            </asp:DropDownList>

                                        </div>
                                    </div>

                                    <!-- To warehouse -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <!-- empty heading to align -->

                                        <div class="info-block">
                                            <strong>To warehouse</strong>
                                            <asp:TextBox ID="txttowarehousegeneraltab" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                        </div>
                                        <div class="info-block">
                                            <strong>Override FEFO date control</strong>
                                            <label class="toggle-switch d-block mt-2">
                                                <asp:CheckBox ID="chkOverrideFEFO" ReadOnly="true" runat="server" />
                                                <span class="slider"></span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                    <!-- Setup Panel -->
                    <a href="#headerSetupPanel"
                        class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                        data-toggle="collapse"
                        role="button"
                        aria-expanded="false"
                        aria-controls="headerSetupPanel">
                        <span class="section-title">Setup</span>
                        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                    </a>
                    <div class="collapse mt-2" id="headerSetupPanel">
                        <div class="card shadow-sm">
                            <div class="card-body">
                                <div class="info-row">
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>RESERVATION</strong></div>
                                        <div class="info-block">
                                            <strong>Reservation items automatically</strong>
                                            <label class="toggle-switch d-block mt-2">
                                                <asp:CheckBox ID="chkReservation" ReadOnly="true" runat="server" />
                                                <span class="slider"></span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Delivery Panel -->
                    <a href="#headerDeliveryPanel"
                        class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                        data-toggle="collapse"
                        role="button"
                        aria-expanded="false"
                        aria-controls="headerDeliveryPanel">
                        <span class="section-title">Delivery</span>
                        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                    </a>
                    <div class="collapse mt-2" id="headerDeliveryPanel">
                        <div class="card shadow-sm">
                            <div class="card-body">
                                <div class="row g-4">
                                    <!-- spacing between blocks -->

                                    <!-- SHIPMENT -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>Shipment</strong></div>

                                        <div class="info-block mb-3">
                                            <strong>Ship date</strong>
                                            <asp:TextBox ID="txtshipdateheader" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>

                                        <div class="info-block">
                                            <strong>Mode of delivery</strong>
                                            <asp:TextBox ID="txtModeOfDelivery" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <!-- empty heading to align -->

                                        <div class="info-block mb-3">
                                            <strong>Delivery terms</strong>
                                            <asp:TextBox ID="txtDeliveryTerms" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>

                                        <div class="info-block">
                                            <strong>Delivery date control</strong>
                                            <asp:TextBox ID="txtDeliveryDateControl" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                        <div class="info-block mb-3">
                                            <strong>ATP time fence</strong>
                                            <asp:TextBox ID="txtATPTimeFence" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>

                                        <div class="info-block">
                                            <strong>ATP backward demand time fence</strong>
                                            <asp:TextBox ID="txtATPBackwardDemandTimeFence" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                        <div class="info-block mb-3">
                                            <strong>ATP backward supply time fence</strong>
                                            <asp:TextBox ID="txtATPBackwardSupplyTimeFence" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>

                                        <div class="info-block">
                                            <strong>ATP delayed demand offset time</strong>
                                            <asp:TextBox ID="txtATPDelayedDemandOffsetTime" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                        <div class="info-block mb-3">
                                            <strong>ATP delayed supply offset time</strong>
                                            <asp:TextBox ID="txtATPDelayedSupplyOffsetTime" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>

                                        <div class="info-block">
                                            <strong>ATP incl. planned orders</strong>
                                            <label class="toggle-switch d-block mt-2">
                                                <asp:CheckBox ID="chkATPInclPlannedOrders" ReadOnly="true" runat="server" />
                                                <span class="slider"></span>
                                            </label>
                                        </div>
                                    </div>

                                    <!-- RECEIPT -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>Receipt</strong></div>

                                        <div class="info-block">
                                            <strong>Receipt date</strong>
                                            <asp:TextBox ID="txtreceiptdateheader" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                        <div class="custom-section-title mb-2"><strong>Transport</strong></div>

                                        <div class="info-block mb-3">
                                            <strong>UPS zone</strong>
                                            <asp:TextBox ID="TextBox6" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>

                                        <div class="info-block">
                                            <strong>Call tag type</strong>
                                            <asp:TextBox ID="TextBox7" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="d-none">
                        <!-- Foreign Trade Panel -->
                        <a href="#headerForeignTradePanel"
                            class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                            data-toggle="collapse"
                            role="button"
                            aria-expanded="false"
                            aria-controls="headerForeignTradePanel">
                            <span class="section-title">Foreign Trade</span>
                            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                        </a>
                        <div class="collapse mt-2" id="headerForeignTradePanel">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <div class="row g-4">
                                        <!-- FOREIGN TRADE and INTRASTAT -->

                                        <!-- FOREIGN TRADE: Transaction code -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>Foreign trade</strong></div>
                                            <div class="info-block">
                                                <strong>Transaction code</strong>
                                                <asp:TextBox ID="txtTransactionCode" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                            </div>
                                        </div>

                                        <!-- FOREIGN TRADE: Transport -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <div class="info-block">
                                                <strong>Transport</strong>
                                                <asp:TextBox ID="txtTransport" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                            </div>
                                        </div>

                                        <!-- FOREIGN TRADE: Port and Statistics procedure -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                            <div class="info-block mb-3">
                                                <strong>Port</strong>
                                                <asp:TextBox ID="txtPort" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                            </div>
                                            <div class="info-block">
                                                <strong>Statistics procedure</strong>
                                                <asp:TextBox ID="txtStatisticsProcedure" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                            </div>
                                        </div>

                                        <!-- INTRASTAT -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>Intrastat</strong></div>
                                            <div class="info-block">
                                                <strong>Special movement</strong>
                                                <asp:TextBox ID="txtSpecialMovement" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- From Warehouse Panel -->
                    <a href="#headerFromWarehousePanel"
                        class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                        data-toggle="collapse"
                        role="button"
                        aria-expanded="false"
                        aria-controls="headerFromWarehousePanel">
                        <span class="section-title">From warehouse</span>
                        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                    </a>
                    <div class="collapse mt-2" id="headerFromWarehousePanel">
                        <div class="card shadow-sm">
                            <div class="card-body">
                                <div class="row g-4">
                                    <!-- Warehouse and Address section -->

                                    <!-- Column 1: From warehouse -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <!-- empty heading to align -->
                                        <div class="info-block">
                                            <strong>From warehouse</strong>
                                            <asp:TextBox ID="txtfromwarehouseheader" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                        </div>
                                    </div>

                                    <!-- Column 2: Address name (multiline) -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <div class="info-block">
                                            <strong>Address name</strong>
                                            <asp:TextBox ID="txtAddressName" ReadOnly="true" runat="server" Text="" CssClass="form-control" TextMode="MultiLine" Rows="5" />
                                        </div>
                                    </div>

                                    <!-- Column 3: Transfer from contact & Warehouse address -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <div class="info-block mb-3">
                                            <strong>Transfer from contact</strong>
                                            <asp:TextBox ID="txtTransferFromContact" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                        <div class="info-block">
                                            <strong>Warehouse address</strong>
                                            <asp:TextBox ID="txtWarehouseAddress" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <!-- Column 4: Address (multiline) -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <div class="info-block">
                                            <strong>Address</strong>
                                            <asp:TextBox ID="txtfromwarehouseaddressdetail" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" TextMode="MultiLine" Rows="5" />
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>

                    <!-- To Warehouse Panel -->
                    <a href="#headerToWarehousePanel"
                        class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                        data-toggle="collapse"
                        role="button"
                        aria-expanded="false"
                        aria-controls="headerToWarehousePanel">
                        <span class="section-title">To warehouse</span>
                        <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                    </a>
                    <div class="collapse mt-2" id="headerToWarehousePanel">
                        <div class="card shadow-sm">
                            <div class="card-body">
                                <div class="row g-4">
                                    <!-- Warehouse and Address section -->

                                    <!-- Column 1: To warehouse -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <!-- empty heading to align -->
                                        <div class="info-block">
                                            <strong>To warehouse</strong>
                                            <asp:TextBox ID="txttowarehouseheader" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" />
                                        </div>
                                    </div>

                                    <!-- Column 2: Address name (multiline) -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <div class="info-block">
                                            <strong>Address name</strong>
                                            <asp:TextBox ID="txttowarehouseheadername" ReadOnly="true" runat="server" Text="" CssClass="form-control" TextMode="MultiLine" Rows="5" />
                                        </div>
                                    </div>

                                    <!-- Column 3: Transfer from contact & Warehouse address -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <div class="info-block mb-3">
                                            <strong>Transfer from contact</strong>
                                            <asp:TextBox ID="TextBox11" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                        <div class="info-block">
                                            <strong>Warehouse address</strong>
                                            <asp:TextBox ID="txttowarehouseaddress" ReadOnly="true" runat="server" Text="" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <!-- Column 4: Address (multiline) -->
                                    <div class="col-md-2">
                                        <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                        <div class="info-block">
                                            <strong>Address</strong>
                                            <asp:TextBox ID="txttowarehouseaddressdetail" ReadOnly="true" runat="server" Text="" CssClass="form-control custom-textbox" TextMode="MultiLine" Rows="5" />
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="d-none">

                        <!-- Transportation Panel -->
                        <a href="#headerTransportationPanel"
                            class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                            data-toggle="collapse"
                            role="button"
                            aria-expanded="false"
                            aria-controls="headerTransportationPanel">
                            <span class="section-title">Transportation</span>
                            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                        </a>
                        <div class="collapse mt-2" id="headerTransportationPanel">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <div class="custom-section-title mb-3"></div>
                                    <div class="info-row">
                                        <div class="info-block">
                                            <strong>Freight charges</strong>
                                            <span>
                                                <asp:Label ID="lblFreightCharges" ReadOnly="true" CssClass="custom-textbox" runat="server" Text="" /></span>
                                        </div>
                                        <div class="info-block">
                                            <strong>Shipping carrier</strong>
                                            <span>
                                                <asp:Label ID="lblShippingCarrier" ReadOnly="true" runat="server" Text="" /></span>
                                        </div>
                                        <div class="info-block">
                                            <strong>Carrier service</strong>
                                            <span>
                                                <asp:Label ID="lblCarrierService" ReadOnly="true" runat="server" Text="" /></span>
                                        </div>
                                        <div class="info-block">
                                            <strong>Carrier group</strong>
                                            <span>
                                                <asp:Label ID="lblCarrierGroup" ReadOnly="true" runat="server" Text="" /></span>
                                        </div>
                                        <div class="info-block">
                                            <strong>Mode</strong>
                                            <span>
                                                <asp:Label ID="lblMode" ReadOnly="true" runat="server" Text="" /></span>
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
    <script>
        function formatLabelNumbers() {
            const labels = document.querySelectorAll('.format-number');

            labels.forEach(label => {
                const raw = label.textContent.replace(/,/g, '').trim();
                if (!isNaN(raw) && raw !== '') {
                    const num = parseFloat(raw);
                    // Format with 2 decimal places and thousand separators
                    label.textContent = num.toLocaleString(undefined, {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2
                    });
                }
            });
        }

        // Trigger on initial page load
        window.addEventListener('load', formatLabelNumbers);

        // Re-apply after partial postbacks (if using UpdatePanel)
        if (typeof Sys !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(formatLabelNumbers);
        }
    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(function () {
            showAJAXOverlay();  // Should now fire
        });

        prm.add_endRequest(function () {
            hideAJAXOverlay();
        });
    </script>
    <script type="text/javascript">
        function refreshParentGrid() {
            __doPostBack('RefreshGrid', '');
        }
    </script>
</asp:Content>
