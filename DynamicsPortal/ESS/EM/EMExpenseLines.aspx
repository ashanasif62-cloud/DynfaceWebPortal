<%@ Page Title="Expense Lines" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EMExpenseLines.aspx.cs" Inherits="DynamicsPortal.ESS.EM.EMExpenseLines" %>
<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
        <style>
        .disabled-button {
            cursor: not-allowed !important;
            pointer-events: none; /* Optional: prevents all mouse interaction */
        }
    </style>
    <style>
        .disabled-buttonLines {
            cursor: not-allowed !important;
            pointer-events: none; /* Optional: prevents all mouse interaction */
        }
    </style>
    <style>
    .nav-tabs {
        border-bottom: none !important;
    }
    .nav-tabs .nav-link {
        border: none !important;
        background: #f8f9fa;
        margin-right: 4px;
        border-radius: 4px 4px 0 0;
    }
    .nav-tabs .nav-link.active {
        background: #ffffff;
        border-bottom: 2px solid transparent !important;
        font-weight: 600;
    }
    .toggle-label-text {
    display: block; /* forces the toggle to move to next line */
    margin-bottom: 0.5rem; /* adds space between text and toggle */
}
</style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnExpenseReport" runat="server" OnClientClick="window.location.href='/ESS/EM/EMExpenseReport_Listpage.aspx'; return false;">
            <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
        </asp:LinkButton>
    </div>
   <%-- <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/EM/EMExpenseLineCreate.aspx')"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>--%>
    <%--</div>--%>
       <div class="action-items">
       <asp:LinkButton ID="BtnSaveHeader" runat="server" OnClick="BtnSave_Header_Click">
       <i class="mdi mdi-content-save" style="margin-right: 4px;"></i>Save
       </asp:LinkButton>
   </div>
    <div class="action-items">
        <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAddGrid_Click"><i class="mdi mdi-plus"></i>Add</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="showOverlay();" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
    </div>

</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"/>

    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>

          <!-- TOGGLE HEADER -->
<div class="d365-toggle-header d-flex justify-content-between align-items-center" 
     data-toggle="collapse" 
     data-target="#headerPanel" 
     role="button" 
     aria-expanded="true" 
     aria-controls="headerPanel"
     style="cursor: pointer; padding: 8px 12px; background-color: #f8f9fa; border-radius: 4px;">
    <span class="section-title mb-0">Expense report header</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</div>

<!-- COLLAPSIBLE CONTENT -->
<div class="collapse show" id="headerPanel">
    <div class="card shadow-sm mb-2" style="margin-top: 4px;">
        <div class="card-body p-3">

            <!-- NAV TABS -->
            <ul class="nav nav-tabs" id="headerTabs" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="general-tab" data-toggle="tab" href="#generalTab" role="tab" aria-controls="generalTab" aria-selected="true">
                        General
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="financial-tab" data-toggle="tab" href="#financialTab" role="tab" aria-controls="financialTab" aria-selected="false">
                        Financial Dimensions
                    </a>
                </li>
            </ul>

            <!-- TAB CONTENT -->
            <div class="tab-content mt-3" id="headerTabsContent">

                <!-- GENERAL TAB -->
                <div class="tab-pane fade show active" id="generalTab" role="tabpanel" aria-labelledby="general-tab">
                    <div class="row">
                        <div class="col-md-2">
                            <div class="info-block mb-3">
                                <strong>Purpose</strong>
                                <asp:TextBox ID="txtheaderpurpose" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="info-block mb-3">
                                <strong>Location</strong>
                                <asp:TextBox ID="txtheaderlocation" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                       <%-- <div class="col-md-2">
                            <div class="info-block mb-3">
                                <strong>Map to travel requisition</strong>
                                <asp:TextBox ID="txtMapToTravelRequisition" runat="server" CssClass="form-control" />
                            </div>
                        </div>--%>

                        <%--<div class="col-md-2">
                            <div class="info-block mb-3">
                                <strong>Travel requisition amount</strong>
                                <asp:TextBox ID="txttravelrequisitionamount" runat="server" CssClass="form-control" />
                            </div>
                        </div>--%>
                    </div>
                </div>

                <!-- FINANCIAL DIMENSIONS TAB -->
                <div class="tab-pane fade" id="financialTab"  role="tabpanel" aria-labelledby="financial-tab">
     <div class="d-flex justify-content-between flex-wrap">
         <div class="flex-grow-1" style="min-width: 300px;">
             <div class="custom-section-title">FINANCIAL DIMENSIONS</div>
             <div runat="server" id="financialDimensionsContainer"></div>
         </div>
     </div>
 </div>

            </div> <!-- /tab-content -->

        </div> <!-- /card-body -->
    </div> <!-- /card -->
</div> <!-- /collapse -->
            <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable"
                ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId" AutoGenerateColumns="false"
                OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <headertemplate>
                            <input type="checkbox" id="chk_SelectAll" />
                        </headertemplate>
                        <itemtemplate>
                            <asp:CheckBox ID="chk_SelectSingle" runat="server" AutoPostBack="true" OnCheckedChanged="chk_SelectSingle_CheckedChanged" CssClass="round-checkbox rowStatus"/>
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expense Transaction Number" Visible="false">
                        <itemtemplate>
                            <asp:Label ID="lblExpenseTransactionNumber" runat="server" Text='<%# Eval("ExpenseTransactionNumber") %>' />
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expense Report Number" Visible="false">
                        <itemtemplate>
                            <asp:Label ID="lblExpenseReportNumber" runat="server" Text='<%# Eval("ExpenseReportNumber") %>' />
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Transaction Date">
                        <itemtemplate>
                            <asp:Label ID="lblTransDate" runat="server" Text='<%# Eval("TransDate")%>' />
                        </itemtemplate>
                        <edititemtemplate>
                            <asp:TextBox ID="txtTransDate" TextMode="Date" runat="server" />
                        </edititemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Approval Status">
                        <itemtemplate>
                            <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Eval("ApprovalStatus") %>' />
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expense Category">
                        <itemtemplate>
                            <asp:Label ID="lblCostType" runat="server" Text='<%# Eval("CostType") %>' />
                        </itemtemplate>
                        <edititemtemplate>
                            <asp:DropDownList ID="ddlCostType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExpenseCategory_SelectedIndexChanged" />
                        </edititemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Merchant">
                        <itemtemplate>
                            <asp:Label ID="lblMerchant" runat="server" Text='<%# Eval("MerchantId") %>' />
                        </itemtemplate>
                        <edititemtemplate>
                            <asp:DropDownList ID="ddlMerchant" runat="server" />
                        </edititemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount">
                        <itemtemplate>
                            <asp:Label ID="lblAmountCurr" runat="server" Text='<%# Eval("AmountCurr") %>' />
                        </itemtemplate>
                        <edititemtemplate>
                            <asp:TextBox ID="txtAmountCurr" runat="server" />
                        </edititemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Currency">
                        <itemtemplate>
                            <asp:Label ID="lblExchangeCode" runat="server" Text='<%# Eval("ExchangeCode") %>' />
                        </itemtemplate>
                        <edititemtemplate>
                            <asp:DropDownList ID="ddlExchangeCode" runat="server" />
                        </edititemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Project Id">
                        <itemtemplate>
                            <asp:Label ID="lblProjectId" runat="server" Text='<%# Eval("ProjId") %>' />
                        </itemtemplate>
                        <edititemtemplate>
                            <asp:DropDownList ID="ddlProjectId" runat="server" />
                        </edititemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Billable">
                        <itemtemplate>
                            <asp:Label ID="lblProjStatusId" runat="server" Text='<%# Eval("ProjStatusId") %>' />
                        </itemtemplate>
                        <edititemtemplate>
                            <asp:DropDownList ID="ddlProjStatusId" runat="server" />
                        </edititemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Activity Number">
                        <itemtemplate>
                            <asp:Label ID="lblProjActivityNumber" runat="server" Text='<%# Eval("ProjActivityNumber") %>' />
                        </itemtemplate>
                        <edititemtemplate>
                            <asp:DropDownList ID="ddlProjActivityNumber" runat="server" />
                        </edititemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                            <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                        </ItemTemplate>
                        <EditItemTemplate>

                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RecId" Visible="false">
                        <itemtemplate>
                            <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <edititemtemplate>
                            <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                            <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                        </edititemtemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:HiddenField ID="EditMode" runat="server" />

             <!-- TOGGLE LINES -->
<div class="d365-toggle-header d-flex justify-content-between align-items-center" 
     data-toggle="collapse" 
     data-target="#linesPanel" 
     role="button" 
     aria-expanded="true" 
     aria-controls="linesPanel"
     style="cursor: pointer; padding: 8px 12px; background-color: #f8f9fa; border-radius: 4px;">
    <span class="section-title mb-0">Expenses</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</div>

<!-- COLLAPSIBLE CONTENT -->
<div class="collapse show" id="linesPanel">
    <div class="card shadow-sm mb-2" style="margin-top: 4px;">
        <div class="card-body p-3">

            <div class="row">
                <!-- ITEM COLUMN -->
                <div class="col-md-7">

                    <!-- Scrollable section only for fields -->
                    <div class="scrollable-section p-1" style="max-height: 350px; overflow-y: auto; border: 1px solid #e0e0e0; border-radius: 4px;">
                        
<div class ="row">
    <div class ="col-md-6">
                                <div class="info-block mb-3">
                            <strong>Expense category</strong>
                            <asp:Dropdownlist ID="ddlExpenseCategorylines" runat="server" CssClass="form-control"/>
                        </div>
                        <div class="info-block mb-3">
                            <strong>Transaction date</strong>
                            <asp:TextBox ID="txtTransactionDatelines" TextMode="Date" runat="server" CssClass="form-control"/>
                        </div>
                        <div class="info-block mb-3">
                            <strong>Merchant</strong>
                            <asp:Dropdownlist ID="ddlMerchantlines" runat="server" CssClass="form-control"/>
                        </div>
                        <div class="info-block mb-3">
                            <strong>Payment method</strong>
                            <asp:Dropdownlist ID="ddlPaymentMethodlines" runat="server" CssClass="form-control"/>
                        </div>
                        <div class="info-block mb-3">
                            <strong>Transaction amount</strong>
                            <asp:TextBox ID="txtTransactionAmountlines" runat="server" CssClass="form-control text-right"  />
                            <asp:Dropdownlist ID="ddlCurrenylines" runat="server" CssClass="form-control"  />
                        </div>
                        <div class="info-block mb-3">
                            <strong>Receipt number</strong>
                            <asp:TextBox ID="txtReceiptNumberlines" runat="server" CssClass="form-control"/>
                        </div>
                <div class="info-block mb-3">
    <strong>Category description</strong>
    <asp:TextBox ID="txtCategorydescriptionlines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>Invoice amount</strong>
    <asp:TextBox ID="txtInvoiceNumberlines" runat="server" CssClass="form-control"   style="background-color: #f0f0f0 !important;" ReadOnly="true"/>
</div>
<div class="info-block mb-3">
    <strong>Additional information</strong>
    <asp:TextBox ID="txtAdditionalInformationlines" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"/>
</div>
<div class="info-block mb-3">
    <strong>Project ID</strong>
    <asp:Dropdownlist ID="txtProjectIdlines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>Billable</strong>
    <asp:Dropdownlist ID="ddlBillablelines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>Activity number</strong>
    <asp:DropDownlist ID="txtActivityNumberlines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>Internal note</strong>
    <asp:TextBox ID="txtInternalNotelines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>Country/region</strong>
    <asp:Dropdownlist ID="ddlCountryRegionlines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>State/province</strong>
    <asp:DropDownlist ID="ddlStatelines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>City</strong>
    <asp:Dropdownlist ID="ddlCitylines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>ZIP/postal code</strong>
    <asp:Dropdownlist ID="ddlZipCodelines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>Sales tax group</strong>
    <asp:Dropdownlist ID="ddlSalestaxlines" runat="server" CssClass="form-control"/>
</div>
<div class="info-block mb-3">
    <strong>Item sales tax group</strong>
    <asp:DropDownlist ID="ddlItemsSalesTaxGrouplines" runat="server" CssClass="form-control" />
</div>
    </div>

    <div class="col-md-6">

         <div class="toggle-group">
    <span class="toggle-label-text">Tax included</span>
    <label class="toggle-switch">
        <input type="checkbox" id="chkSite" runat="server" />
        <span class="slider"></span>
    </label>
</div>   
        <div class="info-block mb-3">
    <strong>Calculated sales tax amount</strong>
    <asp:Textbox ID="txtCalculatedsalestaxlines" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Actual sales tax amount</strong>
    <asp:Textbox ID="txtActualsalestaxlines" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Net transaction amount</strong>
    <asp:Textbox ID="txtNetTransactionAmountlines" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Reimbursement amount</strong>
    <asp:Textbox ID="txtReimbursementamount" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Reason</strong>
    <asp:Textbox ID="txtReasonlines" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />
</div>
        <div class="info-block mb-3">
    <strong>Car rental check out date</strong>
    <asp:Textbox ID="txtCarRentalCheckOutDatelines" runat="server" CssClass="form-control" TextMode="Date" />
</div>
        <div class="info-block mb-3">
    <strong>Check out location</strong>
    <asp:Textbox ID="txtCheckOutLocationlines" runat="server" CssClass="form-control" />
</div>
        <div class="info-block mb-3">
    <strong>Car rental return date</strong>
    <asp:Textbox ID="txtCarRentalReturnDate" runat="server" CssClass="form-control" TextMode="Date" />
</div>
        <div class="info-block mb-3">
    <strong>Return location</strong>
    <asp:Textbox ID="txtReturnLocationlines" runat="server" CssClass="form-control" />
</div>
        <div class="info-block mb-3">
    <strong>Renter name</strong>
    <asp:Textbox ID="txtRenterNamelines" runat="server" CssClass="form-control" />
</div>
        <div class="info-block mb-3">
    <strong>Rental reservation number</strong>
    <asp:Textbox ID="txtRentalReservationNumberlines" runat="server" CssClass="form-control" />
</div>
        <div class="info-block mb-3">
    <strong>Number of days rented</strong>
    <asp:Textbox ID="txtNumberofDaysRentedlines" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Daily rental rate</strong>
    <asp:Textbox ID="txtDailyRentalRatelines" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Weekly rental rate</strong>
    <asp:Textbox ID="txtWeeklyRentalRate" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Monthly rental rate</strong>
    <asp:Textbox ID="txtMonthlyRentalRatelines" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Amount in transaction currency</strong>
    <asp:Textbox ID="txtAmountInTransactionCurrencylines" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Total miles</strong>
    <asp:Textbox ID="txtTotallines" runat="server" CssClass="form-control text-right" />
</div>
        <div class="info-block mb-3">
    <strong>Vehicle class</strong>
    <asp:Dropdownlist ID="ddlVehicleClasslines" runat="server" CssClass="form-control" />
</div>

    </div>
</div>


                    </div> <!-- end scrollable-section -->
                </div>
            </div>
        </div>
    </div>
</div>

        </ContentTemplate>
    </asp:UpdatePanel>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_beginRequest(function () {
        showAJAXOverlay();  // Should now fire
    });

    prm.add_endRequest(function () {
        hideAJAXOverlay();
    });
</script>
<script>
    function applyAutocomplete() {
        $('.filterable-dropdown').each(function () {
            console.log("The Function is Running");
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

                var placeholderText = '-- Select --';

                var $input = $('<input type="text" class="autocomplete-input form-control" />')
                    .attr('placeholder', placeholderText)
                    .val($dropdown.find("option:selected").text())
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
    <style>
        .autocomplete-input {
            width: 150px;
            height: 22px;
            font-size: 11px;
            font-family: Arial, sans-serif;
            border: 1px solid #444;
            border-radius: 0px;
            padding: 1px 4px;
            box-sizing: border-box;
            outline: none;
            background-color: white;
        }

        .ui-menu-item:hover {
            background-color: #f0f0f0;
        }

        .ui-autocomplete {
            z-index: 99999 !important;
            max-height: 150px;
            overflow-y: auto;
            background-color: white;
            border: 1px solid #ccc;
            font-family: Arial, sans-serif;
            font-size: 10px;
        }
    </style>
</asp:Content>
