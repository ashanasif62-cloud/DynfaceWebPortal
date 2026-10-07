<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="TransferJournalLines_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.PR.TransferJournalLines_ListPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>

    <style>
        .textbox-style {
            color: gray !important; /* ForeColor */
            background-color: #F3F2F1 !important; /* BackColor */
        }
    </style>

    <style>
        .textbox-border {
            border: 1px solid black !important; /* Solid black border */
            border-radius: 4px; /* Optional: rounded edges */
            padding: 0.375rem 0.75rem; /* Keep spacing like Bootstrap */
            background-color: white !important; /* Keep default white background */
            color: black !important; /* Keep text black */
        }
    </style>

    <style>
        .section-header {
            display: block;
            color: #201f1e;
            text-transform: uppercase;
            font-weight: 800;
            line-height: 15px;
            font-size: 12px;
        }
    </style>

    <style>
        .dimension-section {
            margin-bottom: 20px;
        }

        .section-title {
            font-weight: 700;
            font-size: 14px;
            margin-bottom: 12px;
            text-transform: uppercase;
            color: #201f1e;
        }

        .dimension-row {
            display: flex;
            gap: 20px;
            margin-bottom: 15px;
        }

        .dimension-col {
            flex: 1; /* equal width for both columns */
        }

        .label {
            display: block;
            font-size: 13px;
            font-weight: 500;
            margin-bottom: 6px;
            color: #323130;
        }
    </style>

    <style>
        .placeholder-control {
            border: 1px solid black !important;
            min-height: 38px; /* matches Bootstrap form-control height */
            display: block;
            background-color: #f8f9fa; /* light gray to mimic input */
            border: 1px solid #ced4da;
            border-radius: 0.25rem;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
<asp:UpdatePanel ID="updButtons" runat="server">
    <ContentTemplate>
    <div class="action-items">
        <asp:LinkButton ID="btnTransferJournal" runat="server" OnClientClick="var ref = document.referrer; if (ref.includes('TransferJournal_ListPage.aspx')) { window.location.href='/ESS/PR/TransferJournal_ListPage.aspx'; } else { window.location.href='/ESS/PR/TransferJournal_ListPage.aspx'; } return false;">
    <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
        </asp:LinkButton>
    </div>
                      </ContentTemplate>
</asp:UpdatePanel>  
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
<ContentTemplate>
    <!-- Left side labels -->
    <div style="display: flex; align-items: center; gap: 10px;">
        <asp:Label ID="lblJournalID" runat="server" Text="JRN001"></asp:Label>
        <asp:Label ID="lblJournalDescription" runat="server" Text="Monthly Salary Journal"></asp:Label>
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
                <!-- Journal header details Panel -->
                <a href="#JournalHeaderDetailsPanel"
                    class="d365-toggle-header d-flex justify-content-between align-items-center"
                    data-toggle="collapse" role="button" aria-expanded="true" aria-controls="JournalHeaderDetailsPanel">
                    <span class="section-title">Journal header details</span>
                    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                </a>
                <div class="collapse show mt-3" id="JournalHeaderDetailsPanel">

                    <div class="card-body">
                        <div class="PR-custom-section-title mb-3"></div>

                        <div class="d-flex flex-wrap justify-content-between">

                            <!-- Column 1: Voucher -->
                            <div style="flex: 0 0 24%;">
                                <div class="mb-2">
                                    <strong class="d-block mb-2">VOUCHER</strong>
                                </div>
                                <div class="form-group" style="width: 250px;">
                                    <label>Voucher series</label>
                                    <asp:Label ID="lblVoucherSeries" runat="server" CssClass="form-control autocomplete-input textbox-style"></asp:Label>
                                </div>
                            </div>

                            <!-- Column 2: Selection by + New voucher by -->
                            <div style="flex: 0 0 24%;">
                                <%-- <div class="form-group" style="width: 250px;">
                        <label>Selection by</label>
                        <asp:DropDownList ID="ddlSelectionBy" runat="server" CssClass="form-select" Width="100%"></asp:DropDownList>
                    </div>--%>

                                <div class="form-group" style="width: 250px;">
                                    <label>Selection by</label>
                                    <asp:Label ID="ddlSelectionBy" runat="server" CssClass="form-control textbox-style" Width="100%"></asp:Label>
                                </div>

                                <%--<div class="form-group" style="width: 250px;">
                        <label>New voucher by</label>
                        <asp:DropDownList ID="ddlNewVoucherBy" runat="server" CssClass="form-select" Width="100%"></asp:DropDownList>
                    </div>--%>

                                <div class="form-group" style="width: 250px;">
                                    <label>New voucher by</label>
                                    <asp:Label ID="ddlNewVoucherBy" runat="server" CssClass="form-control textbox-style" Width="100%"></asp:Label>
                                </div>

                            </div>

                            <!-- Column 3: Posting -->
                            <div style="flex: 0 0 24%;">
                                <div class="mb-2">
                                    <strong class="d-block mb-2">POSTING</strong>
                                </div>
                                <%-- <div class="form-group" style="width: 250px;">
                        <label>Detail level</label>
                        <asp:DropDownList ID="ddlDetailLevel" runat="server" CssClass="form-select" Width="100%"></asp:DropDownList>
                    </div>--%>

                                <div class="form-group" style="width: 250px;">
                                    <label>Detail level</label>
                                    <asp:Label ID="ddlDetailLevel" runat="server" CssClass="form-control textbox-style" Width="100%"></asp:Label>
                                </div>

                            </div>

                            <!-- Column 4: Delete lines + Offset account -->
                            <div style="flex: 0 0 24%;">
                                <div class="form-group">
                                    <label class="mb-2 d-block">Delete lines after posting</label>
                                    <label class="toggle-switch mb-2 d-block">
                                        <input type="checkbox" id="DeleteLinesAfterPostingBox" runat="server" />
                                        <%--<input type="checkbox" id="DeleteLinesAfterPostingBox" runat="server" disabled="disabled" />--%>
                                        <span class="slider"></span>
                                    </label>
                                    <span id="DeleteLinesAfterPosting" runat="server"></span>
                                </div>
                                <div class="form-group mb-3" style="width: 250px;">
                                    <label>Offset account</label>
                                    <asp:Label ID="lblOffsetAccount" runat="server" CssClass="form-control textbox-style" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>


                <%-- Journal Lines --%>
                <div class="d365-toggle-header d-flex justify-content-between align-items-center mt-4">
                    <span class="section-title">Journal Lines</span>
                </div>

                <div class="card shadow-sm mt-4">
                    <div class="card-body pt-2 pb-2">
                        <div class="text-left d-flex align-items-center gap-4 mb-2">
                            <div class="action-items">
                                <asp:LinkButton ID="BtnAddLine" runat="server" OnClick="btnNew_Grid_Click"
                                    Style="font-size: 0.9rem;">
                    <i class="mdi mdi-plus" style="font-size: 0.9rem;"></i> New
                                </asp:LinkButton>
                            </div>
                            <div class="action-items" style="margin-left: 30px;">
                                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"
                                    Style="font-size: 0.9rem;">
                                    <i class="mdi mdi-delete" style="font-size: 0.9rem;"></i> Remove
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="table-responsive" style="overflow-x: auto; white-space: nowrap;">
                    <asp:GridView ID="gridView" runat="server" CssClass="table table-condensed no-border table-hover sortable"
                        ShowHeaderWhenEmpty="true" OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound"
                        EmptyDataText="No Record Found." DataKeyNames="RecId" AutoGenerateColumns="false">

                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                <HeaderTemplate>
                                    <input type="checkbox" id="chk_SelectAll" class="round-checkbox" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_SelectSingle" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chk_SelectSingle_CheckedChanged" CssClass="round-checkbox" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="txtReceiptDate" runat="server" Text='<%# Eval("TransDate", "{0:MM-dd-yyyy}") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtReceiptDateEdit"  runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Item Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemNumber" runat="server" Text='<%# Bind("ItemId") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlItemNumber" AutoPostBack="true" OnSelectedIndexChanged="onItemModified" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Product name">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtProductName" Enabled="false" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From site">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromSite" runat="server" Text='<%# Bind("FromSite") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlFromSiteId" OnSelectedIndexChanged="ddlFromSiteId_selection" AutoPostBack="true" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="To site">
                                <ItemTemplate>
                                    <asp:Label ID="lblToSite" runat="server" Text='<%# Bind("ToSite") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlToSiteId" OnSelectedIndexChanged="ddlToSiteId_selection" AutoPostBack="true" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From warehouse">
                                <ItemTemplate>
                                    <asp:Label ID="lbblFromWarehouse" runat="server" Text='<%# Bind("FromWarehouse") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="lddlFromWarehouse" OnSelectedIndexChanged="onFromOnlyWarehouseSelection" AutoPostBack="true"  runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="To warehouse">
                                <ItemTemplate>
                                    <asp:Label ID="lblToWarehouse" runat="server" Text='<%# Bind("ToWarehouse") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlToWarehouse" OnSelectedIndexChanged="onToOnlyWarehouseSelection" AutoPostBack="true"  runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From location">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromLocationId" runat="server" Text='<%# Bind("FromWMSLocationId") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlFromLocationId"  runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="To location">
                                <ItemTemplate>
                                    <asp:Label ID="lblToLocationId" runat="server" Text='<%# Bind("ToWMSLocationId") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlToLocationId"  runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="LineNum" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblLineNum" runat="server" Text='<%# Bind("LineNum") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Voucher" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblVoucher" runat="server" Text='<%# Bind("Voucher") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cost price" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCostPrice" runat="server" Text='<%# Bind("CostPrice") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Price quantity" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPriceQuantity" runat="server" Text="" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CostAmount" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCostAmount" runat="server" Text='<%# Bind("CostAmount") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="From inventory status">
                                <ItemTemplate>
                                    <asp:Label ID="lblInventoryStatusId" runat="server" Text='<%# Bind("FromInventoryStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlInventoryStatusId" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="To inventory status">
                                <ItemTemplate>
                                    <asp:Label ID="lblToInventoryStatusId" runat="server" Text='<%# Bind("ToInventoryStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlToInventoryStatusId" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Quantity">
                                <ItemTemplate>
                                    <asp:Label ID="txtQuantity" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:TextBox ID="txtQuantityEdit" OnTextChanged="OnQuantityModified" AutoPostBack="true" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Unit quantity">
                                <ItemTemplate>
                                    <asp:Label ID="txtUnitQuantity" runat="server" Text='<%# Bind("UnitQty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUnitQuantityEdit" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Unit">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("Unit") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUnit" Enabled="false" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CW quantity">
                                <ItemTemplate>
                                    <asp:Label ID="txtCWQuantity" runat="server" Text='<%# Bind("CWQty") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CW Unit">
                                <ItemTemplate>
                                    <asp:Label ID="txtCWUnit" runat="server" Text='<%# Bind("CWUnit") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                          <%--  <asp:TemplateField HeaderText="Copy batch attributes">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCopyBatchAttributes" runat="server" Text='<%# Bind("PdsCopyBatchAttrib") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                           <%-- <asp:TemplateField HeaderText="Copy batch attributes">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCopyBatchAttributes" runat="server" Checked='<%# Convert.ToBoolean(Eval("PdsCopyBatchAttrib")) %>' />
                            </ItemTemplate>
                           </asp:TemplateField>--%>


                            <asp:TemplateField HeaderText="Log">
                                <ItemTemplate>
                                    <asp:TextBox ID="tctLog" runat="server" Text="" Visible="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="RecId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            

                            <%--inventory from Dimension--%>
                            <asp:TemplateField HeaderText="From configuration" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromInventConfigId" runat="server" Text='<%# Bind("FromInventConfigId") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlFromInventConfigId" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From size" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblSize" runat="server" Text='<%# Bind("FromInventSizeId") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlFromInventSizeId" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From color" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromColor" runat="server" Text='<%# Bind("FromInventColorId") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:DropDownList ID="ddlFromInventColorId" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From style" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromStyle" runat="server" Text='<%# Bind("FromInventStyleId") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:DropDownList ID="ddlFromInventStyleId" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="FromVersion" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromVersion" runat="server" Text='<%# Bind("FromInventVersionId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="FromInventSiteId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromInventSiteId" runat="server" Text='<%# Bind("FromInventSiteId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                       <%--     <asp:TemplateField HeaderText="FromWarehouse" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromWarehouse" runat="server" Text='<%# Bind("FromWarehouse") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="From batch number" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromBatchNumber" runat="server" Text='<%# Bind("FromInventBatchId") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:DropDownList ID="ddlFromInventBatchId" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From location" Visible="false">
     <ItemTemplate>
         <asp:Label ID="lblFromLocation" runat="server" Text='<%# Bind("FromInventLocationId") %>' />
     </ItemTemplate>
 </asp:TemplateField>

                            <asp:TemplateField HeaderText="From serial number" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromSerialNumber" runat="server" Text='<%# Bind("FromInventSerialId") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:DropDownList ID="ddlFromInventSerialId" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From inventory status" Visible="false">
                             <ItemTemplate>
                                 <asp:Label ID="lblFromInventoryStatus" runat="server" Text='<%# Bind("FromInventoryStatus") %>'></asp:Label>
                             </ItemTemplate>
                             <EditItemTemplate>
                                 <asp:DropDownList ID="ddlInventoryStatusId" runat="server"></asp:DropDownList>
                             </EditItemTemplate>
                           </asp:TemplateField>

                            <asp:TemplateField HeaderText="FromLicensePlate" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromLicensePlate" runat="server" Text='<%# Bind("FromInventLicensePlateId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="FromOwner" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromOwner" runat="server" Text='<%# Bind("FromInventOwnerId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="To configuration" Visible="true">
     <ItemTemplate>
         <asp:Label ID="lblToInventConfigId" runat="server" Text='<%# Bind("ToInventConfigId") %>' />
     </ItemTemplate>
     <EditItemTemplate>
         <asp:DropDownList ID="ddlToInventConfigId" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>

 <asp:TemplateField HeaderText="To size" Visible="true">
     <ItemTemplate>
         <asp:Label ID="lblToSize" runat="server" Text='<%# Bind("ToInventSizeId") %>' />
     </ItemTemplate>
     <EditItemTemplate>
         <asp:DropDownList ID="ddlToInventSizeId" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>

 <asp:TemplateField HeaderText="To color" Visible="true">
     <ItemTemplate>
         <asp:Label ID="lblToColor" runat="server" Text='<%# Bind("ToInventColorId") %>' />
     </ItemTemplate>
     <EditItemTemplate>
          <asp:DropDownList ID="ddlToInventColorId" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>

 <asp:TemplateField HeaderText="To style" Visible="true">
     <ItemTemplate>
         <asp:Label ID="lblToStyle" runat="server" Text='<%# Bind("ToInventStyleId") %>' />
     </ItemTemplate>
     <EditItemTemplate>
          <asp:DropDownList ID="ddlToInventStyleId" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>

 <asp:TemplateField HeaderText="To batch number" Visible="true">
     <ItemTemplate>
         <asp:Label ID="lblToBatchNumber" runat="server" Text='<%# Bind("ToInventBatchId") %>' />
     </ItemTemplate>
     <EditItemTemplate>
          <asp:DropDownList ID="ddlToInventBatchId" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>

 <asp:TemplateField HeaderText="To serial number" Visible="true">
     <ItemTemplate>
         <asp:Label ID="lblToSerialNumber" runat="server" Text='<%# Bind("ToInventSerialId") %>' />
     </ItemTemplate>
     <EditItemTemplate>
          <asp:DropDownList ID="ddlToInventSerialId" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>


                             <asp:TemplateField HeaderText="InventDimId" Visible="false">
     <ItemTemplate>
         <asp:Label ID="lblInventdimId" runat="server" Text='<%# Bind("InventDimId") %>' />
     </ItemTemplate>
 </asp:TemplateField>

                                 <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
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
                    data-toggle="collapse" role="button" aria-expanded="true" aria-controls="detailPanel">
                    <span class="section-title">Lines details</span>
                    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                </a>

                <div class="collapse show mt-4" id="detailPanel">
                    <div class="card shadow-sm">
                        <div class="card-header bg-light">
                            <ul class="nav nav-tabs" id="detailTabs" role="tablist">
                                <%-- General Clickable tab --%>
                                <li class="nav-item">
                                    <a class="nav-link active" id="general-tab" data-toggle="tab" href="#general" role="tab">General</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="Financial-dimension" data-toggle="tab" href="#FinancialDimension" role="tab">Financial dimension</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="dimensions-tab" data-toggle="tab" href="#InventoryDimension" role="tab">Inventory dimensions</a>
                                </li>
                            </ul>
                        </div>

                        <%-- Start All Tabs --%>
                        <div class="card-body tab-content" id="detailTabContent">

                            <%-- Start General Tab --%>
                            <div class="tab-pane fade show active" id="general" role="tabpanel">
                                <div class="row g-4">
                                    <!-- spacing between blocks -->
                                    <!-- Wrap All columns in flex -->
                                    <div class="d-flex flex-wrap justify-content-between">

                                        <!-- 1st Column -->
                                        <div style="flex: 0 0 19%;">
                                            <div class="mb-2">
                                                <strong class="section-header">IDENTIFICATION</strong>
                                            </div>
                                            <!-- Journal -->
                                            <div class="form-group" style="width: 250px;">
                                                <label>Journal</label>
                                                <asp:Label ID="genJournal" runat="server" CssClass="form-control autocomplete-input textbox-style" />
                                            </div>
                                            <!-- Line number -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Line number</label>
                                                <asp:TextBox ID="genLineNumber" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="text-align: right;" ReadOnly="true" />
                                            </div>
                                            <!-- Date -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label class="d-block">Date</label>
                                                <asp:TextBox ID="genDate" runat="server" Text="" TextMode="Date" CssClass="form-control textbox-border" Style="width: 150px;" />
                                            </div>
                                            <!-- Voucher -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Voucher</label>
                                                <asp:TextBox ID="genVoucher" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px;" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- 2nd Column -->
                                        <div style="flex: 0 0 19%;">
                                            <div class="mb-2">
                                                <strong class="section-header">JOURNAL LINE</strong>
                                            </div>
                                            <!-- CW quantity -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>CW quantity</label>
                                                <asp:TextBox ID="genCWQty" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;" ReadOnly="true" />
                                            </div>
                                            <!-- CW unit -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>CW unit</label>
                                                <asp:TextBox ID="genCWUnit" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;" ReadOnly="true" />
                                            </div>
                                            <!-- Unit quantity -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Unit quantity</label>
                                                <asp:TextBox ID="genUnityQty" runat="server" CssClass="form-control autocomplete-input" Style="width: 100px; text-align: right;" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- 3rd Column -->
                                        <div style="flex: 0 0 19%;">
                                            <!-- Unit -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label class="d-block">Unit</label>
                                                <asp:TextBox ID="genUnit" runat="server" CssClass="form-select" Style="width: 100px; text-align: left;"></asp:TextBox>
                                            </div>
                                            <!-- Quantity -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Quantity</label>
                                                <asp:TextBox ID="genQuantity" runat="server" CssClass="form-control autocomplete-input" Style="width: 100px; text-align: right;" />
                                            </div>
                                            <!-- Cost price -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Cost price</label>
                                                <asp:TextBox ID="genCostPrice" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;" ReadOnly="true" />
                                            </div>
                                            <!-- Price quantity -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Price quantity</label>
                                                <asp:TextBox ID="genPriceQty" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- 4th Column -->
                                        <div style="flex: 0 0 19%;">
                                            <!-- Charges on cost -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Charges on cost</label>
                                                <asp:TextBox ID="genChargesOnCost" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;" ReadOnly="true" />
                                            </div>
                                            <!-- Cost amount -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Cost amount</label>
                                                <asp:TextBox ID="genCostAmount" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;" ReadOnly="true" />
                                            </div>
                                            <div class="mb-2">
                                                <strong class="section-header">INVENTORY</strong>
                                            </div>
                                            <!-- Lot ID -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Lot ID</label>
                                                <asp:TextBox ID="genLotID" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px;" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- 5th Column -->
                                        <div style="flex: 0 0 19%;">
                                            <div class="mb-2">
                                                <strong class="section-header">TRANSFER</strong>
                                            </div>
                                            <!-- Receive lot ID -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>Receive lot ID</label>
                                                <asp:TextBox ID="genReceiveLotID" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px;" ReadOnly="true" />
                                            </div>
                                            <!-- To dimension No. -->
                                            <div class="form-group mb-3" style="width: 250px;">
                                                <label>To dimension No.</label>
                                                <asp:TextBox ID="genToDimensionNo" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px;" ReadOnly="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%-- End General Tab --%>

                            <%-- Start Financial Dimension Tab --%>
                            <div class="tab-pane fade" id="FinancialDimension" role="tabpanel">
                                <div class="row g-4">
                                    <div class="dimension-section">
                                        <%--Start FROM DIMENSION--%>
                                        <div class="section-title">FROM DIMENSION</div>

                                        <!-- Bootstrap row for compact equal columns -->
                                        <div class="row justify-content-center">

                                            <!-- BusinessUnit -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">BusinessUnit</label>
                                                <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="lbl11" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                            <!-- Cargo -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">Cargo</label>
                                                <asp:DropDownList ID="ddlCargo" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="lbl12" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                            <!-- Department -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">Department</label>
                                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="lbl13" runat="server" CssClass="form-control placeholder-control placeholder-control"></asp:Label>
                                            </div>

                                            <!-- ItemGroup -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">ItemGroup</label>
                                                <asp:DropDownList ID="ddlItemGroup" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="lbl14" runat="server" CssClass="form-control placeholder-control "></asp:Label>
                                            </div>

                                            <!-- LC Number -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">LC_Number</label>
                                                <asp:DropDownList ID="ddlLCNumber" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="lbl15" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                            <!-- CostCenter -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">CostCenter</label>
                                                <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="lbl16" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                        </div>
                                        <!-- end From Dimension -->

                                        <%--TO DIMENSION Start--%>
                                        <div class="section-title">TO DIMENSION</div>

                                        <!-- Bootstrap row for compact equal columns -->
                                        <div class="row justify-content-center">

                                            <!-- BusinessUnit -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">BusinessUnit</label>
                                                <asp:DropDownList ID="DropDownList11" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="Label13" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                            <!-- Cargo -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">Cargo</label>
                                                <asp:DropDownList ID="ddlTCargo" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="Label14" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                            <!-- Department -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">Department</label>
                                                <asp:DropDownList ID="ddlTDepartment" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="Label15" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                            <!-- ItemGroup -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">ItemGroup</label>
                                                <asp:DropDownList ID="ddlTItemGroup" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="Label16" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                            <!-- LC Number -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">LC_Number</label>
                                                <asp:DropDownList ID="ddlTLCNumber" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="Label17" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                            <!-- CostCenter -->
                                            <div class="col-md-3 mb-3">
                                                <label class="label">CostCenter</label>
                                                <asp:DropDownList ID="ddlTCostCenter" runat="server" CssClass="form-control placeholder-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="label invisible">Placeholder</label>
                                                <asp:Label ID="Label18" runat="server" CssClass="form-control placeholder-control"></asp:Label>
                                            </div>

                                        </div>
                                        <!-- end To Dimension -->

                                    </div>
                                </div>
                            </div>
                            <%-- End Financial Dimension Tab --%>
                            <%-- Start Inventory Dimension Tab --%>
                            <div class="tab-pane fade" id="InventoryDimension" role="tabpanel">
                                <%--<div class="container">--%>
                                <div class="row gx-5">
                                    <!-- FROM INVENTORY DIMENSIONS -->
                                    <div class="col-md-4">
                                        <h6><strong>FROM INVENTORY DIMENSIONS</strong></h6>

                                        <div class="mb-3">
                                            <label>Configuration</label>
                                            <asp:TextBox ID="txtFromConfig" runat="server" CssClass="form-control placeholder-control textbox-style" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Size</label>
                                            <asp:TextBox ID="txtFromSize" runat="server" CssClass="form-control placeholder-control textbox-style" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Color</label>
                                            <asp:TextBox ID="txtFromColor" runat="server" CssClass="form-control placeholder-control textbox-style" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Style</label>
                                            <asp:TextBox ID="txtFromStyle" runat="server" CssClass="form-control placeholder-control textbox-style" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Version</label>
                                            <asp:TextBox ID="txtFromVersion" runat="server" CssClass="form-control placeholder-control textbox-style" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4">

                                        <div class="mb-3">
                                            <label>Site</label>
                                            <asp:TextBox ID="txtFromSite" runat="server" CssClass="form-control placeholder-control"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Warehouse</label>
                                            <asp:TextBox ID="ddlFromWarehouse" runat="server" CssClass="form-control placeholder-control"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Batch number</label>
                                            <asp:TextBox ID="txtFromBatchNumber" runat="server" CssClass="form-control placeholder-control textbox-style" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Location</label>
                                            <asp:TextBox ID="txtFromLocation" runat="server" CssClass="form-control placeholder-control"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Serial number</label>
                                            <asp:TextBox ID="txtFromSerialNumber" runat="server" CssClass="form-control placeholder-control textbox-style" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="mb-3">
                                            <label>Inventory status</label>
                                            <asp:TextBox ID="txtFromInventoryStatus" runat="server" CssClass="form-control placeholder-control"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>License plate</label>
                                            <asp:TextBox ID="txtFromLicensePlate" runat="server" CssClass="form-control placeholder-control"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Owner</label>
                                            <asp:TextBox ID="txtFromOwner" runat="server" CssClass="form-control placeholder-control textbox-style" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <hr class="my-4" />

                                <div class="row gx-5">
                                    <!-- TO INVENTORY DIMENSIONS -->
                                    <div class="col-md-4">
                                        <h6><strong>TO INVENTORY DIMENSIONS</strong></h6>

                                        <div class="mb-3">
                                            <label>Configuration</label>
                                            <asp:TextBox ID="txtToConfiguration" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToInventConfigId") %>'></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Size</label>
                                            <asp:TextBox ID="txtToSize" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToInventSizeId") %>'></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Color</label>
                                            <asp:TextBox ID="txtToColor" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToInventColorId") %>'></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Style</label>
                                            <asp:TextBox ID="txtToStyle" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToInventStyleId") %>'></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Version</label>
                                            <asp:TextBox ID="txtToVersion" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToInventVersionId") %>'></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4">

                                        <div class="mb-3">
                                            <label>Site</label>
                                            <asp:DropDownList ID="ddlToSite" runat="server" CssClass="form-control placeholder-control" Text='<%# Bind("ToSite") %>'></asp:DropDownList>
                                        </div>
                                        <div class="mb-3">
                                            <label>Warehouse</label>
                                            <asp:DropDownList ID="ddlToWarehouse" runat="server" CssClass="form-control placeholder-control" Text='<%# Bind("ToWarehouse") %>'></asp:DropDownList>
                                        </div>
                                        <div class="mb-3">
                                            <label>Batch number</label>
                                            <asp:TextBox ID="txtToBatchNumber" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToInventBatchId") %>'></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Location</label>
                                            <asp:TextBox ID="txtToLocation" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToWMSLocationId") %>'></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Serial number</label>
                                            <asp:TextBox ID="txtToSerialNumber" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToInventSerialId") %>'></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="mb-3">
                                            <label>Inventory status</label>
                                            <asp:TextBox ID="txtToInventoryStatus" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("FromInventoryStatus") %>'></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>License plate</label>
                                            <asp:TextBox ID="txtToLicensePlate" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToInventLicensePlateId") %>'></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label>Owner</label>
                                            <asp:TextBox ID="txtToOwner" runat="server" CssClass="form-control placeholder-control textbox-style" Text='<%# Bind("ToInventOwnerId") %>'></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <%--</div>--%>
                            </div>

                            <%-- End Inventory Dimension Tab --%>

                            <%-- End All Tabs --%>
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

                                <!-- Identification -->
                                <div style="flex: 0 0 24%;">
                                    <div class="mb-2">
                                        <strong class="d-block mb-2">Identification</strong>
                                    </div>
                                    <div class="form-group" style="width: 250px;">
                                        <label>Journal</label>
                                        <asp:Label ID="lblJournal" runat="server" CssClass="form-control autocomplete-input textbox-style"></asp:Label>
                                    </div>

                                    <div class="form-group" style="width: 250px;">
                                        <label>Journal type</label>
                                        <asp:Label ID="lblJournalType" runat="server" CssClass="form-control autocomplete-input textbox-style"></asp:Label>
                                    </div>
                                </div>


                                <div class="col-md-2">

                                    <div class="form-group" style="width: 250px;">
                                        <label>Name</label>
                                        <asp:Label ID="lblName" runat="server" CssClass="form-control autocomplete-input textbox-style"></asp:Label>
                                    </div>
                                    <div class="form-group" style="width: 250px;">
                                        <label>Description</label>
                                        <asp:TextBox ID="lblDescription" runat="server" Text=""></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-2">

                                    <div class="form-group" style="width: 250px;">
                                        <label>Origin</label>
                                        <asp:Label ID="lblOrigin" runat="server" Text="" CssClass="form-control autocomplete-input textbox-style"></asp:Label>
                                    </div>
                                    <strong>Voucher</strong>
                                    <div class="form-group" style="width: 250px;">
                                        <label>Voucher series</label>
                                        <asp:Label ID="txtVoucherSeries" runat="server" Text="" CssClass="form-control autocomplete-input textbox-style"></asp:Label>
                                    </div>

                                </div>

                                <div class="col-md-2">

                                    <div class="form-group" style="width: 250px;">
                                        <label>Selection by</label>
                                        <asp:Label ID="txtSelectionBy" runat="server" Text="" CssClass="form-control autocomplete-input textbox-style"></asp:Label>
                                    </div>

                                    <div class="form-group" style="width: 250px;">
                                        <label>New voucher by</label>
                                        <asp:Label ID="txtNewVoucherBy" runat="server" Text="" CssClass="form-control autocomplete-input textbox-style"></asp:Label>
                                    </div>

                                </div>

                                <!-- Posting -->
                                <div style="flex: 0 0 24%;">
                                    <div class="mb-2">
                                        <strong class="d-block mb-2">POSTING</strong>
                                    </div>
                                    <div class="form-group" style="width: 250px;">
                                        <label>Detail level</label>
                                        <asp:Label ID="txtDetailLevel" runat="server" CssClass="form-control autocomplete-input textbox-style" Width="100%"></asp:Label>
                                    </div>

                                    <div class="form-group">
                                        <label class="mb-2 d-block">Delete lines after posting</label>
                                        <label class="toggle-switch mb-2 d-block">
                                            <input type="checkbox" id="txtDeleteLinesAfterPosting" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                        <span id="Span1" runat="server"></span>
                                    </div>
                                    <div class="form-group mb-3" style="width: 250px;">
                                        <label>Offset account</label>
                                        <asp:Label ID="txtOffsetAccount" runat="server" CssClass="form-control autocomplete-input textbox-style" Width="100%"></asp:Label>
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
                                <%--<div class="col-md-2">
    <div class="custom-section-title mb-2"><strong>RESERVATION</strong></div>

    <div class="col-md-2">
    <div class="info-block">
        <strong>Reservation</strong>
        <asp:TextBox ID="txtReservation" ReadOnly="true" runat="server" CssClass="form-control mt-2" />
    </div>
</div>

</div>--%>
                                <div style="flex: 0 0 24%;">
                                    <div class="mb-2">
                                        <strong class="d-block mb-2">RESERVATION</strong>
                                    </div>
                                    <div class="form-group" style="width: 250px;">
                                        <label>Reservation</label>
                                        <asp:Label ID="txtReservation" runat="server" CssClass="form-control autocomplete-input textbox-style"></asp:Label>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>

                <!-- Blocking Panel -->
                <a href="#headerBlockingPanel"
                    class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                    data-toggle="collapse"
                    role="button"
                    aria-expanded="false"
                    aria-controls="headerBlockingPanel">
                    <span class="section-title">Blocking</span>
                    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                </a>
                <div class="collapse mt-2" id="headerBlockingPanel">
                    <div class="card shadow-sm">
                        <div class="card-body">


                            <div class="row align-items-end g-3">
                                <!-- In use -->
                                <div class="col-md-2">
                                    <div class="info-block">
                                        <strong>In use</strong>
                                        <label class="toggle-switch d-block mt-2">
                                            <asp:CheckBox ID="CheckBoxInUse" ReadOnly="true" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                </div>

                                <!-- Used by user (half width) -->
                                <div class="col-md-2">
                                    <div class="info-block">
                                        <strong>Used by user</strong>
                                        <asp:TextBox ID="txtUsedBy" ReadOnly="true" runat="server" CssClass="form-control mt-2" />
                                    </div>
                                </div>

                                <!-- Locked by system -->
                                <div class="col-md-2">
                                    <div class="info-block">
                                        <strong>Locked by system</strong>
                                        <label class="toggle-switch d-block mt-2">
                                            <asp:CheckBox ID="CheckBoxLockedBy" ReadOnly="true" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                </div>

                                <!-- Private for user group -->
                                <div class="col-md-2">
                                    <div class="info-block">
                                        <strong>Private for user group</strong>
                                        <asp:DropDownList ID="ddlPrivateForUserGroup" runat="server" CssClass="form-control mt-2">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>



                        </div>
                    </div>
                </div>


                <!-- History Panel -->
                <a href="#headerFromWarehousePanel"
                    class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                    data-toggle="collapse"
                    role="button"
                    aria-expanded="false"
                    aria-controls="headerFromWarehousePanel">
                    <span class="section-title">History</span>
                    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                </a>
                <div class="collapse mt-2" id="headerFromWarehousePanel">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <div class="row g-4">

                                <!-- Column 1 -->
                                <div style="flex: 0 0 16%;">
                                    <div class="mb-2">
                                        <strong class="d-block mb-2">HISTORY</strong>
                                    </div>
                                    <div class="info-block">
                                        <strong>Posted</strong>
                                        <label class="toggle-switch d-block mt-2">
                                            <asp:CheckBox ID="txtPosted" ReadOnly="true" runat="server" />
                                            <span class="slider"></span>
                                        </label>
                                    </div>
                                </div>

                                <!-- Column 2-->
                                <div class="col-md-2">
                                    <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                    <div class="info-block mb-3">
                                        <label>Posted on</label>
                                        <asp:Label ID="txtPostedOn" runat="server" CssClass="form-control autocomplete-input textbox-style" Width="100%"></asp:Label>
                                    </div>
                                </div>

                                <!-- Column 3 -->
                                <div class="col-md-2">
                                    <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                    <div class="info-block mb-3">
                                        <label>Posted by</label>
                                        <asp:Label ID="txtPostedBy" runat="server" CssClass="form-control autocomplete-input textbox-style" Width="100%"></asp:Label>
                                    </div>
                                    <div class="info-block">
                                        <label>Original journal No.</label>
                                        <asp:Label ID="txtOriginalJournalNo" runat="server" CssClass="form-control autocomplete-input textbox-style" Width="100%"></asp:Label>
                                    </div>
                                </div>

                                <!-- Column 4 -->
                                <div style="flex: 0 0 14%; margin-left: 20px;">
                                    <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                    <div class="mb-2">
                                        <strong class="d-block mb-2">TOTALS</strong>
                                    </div>
                                    <div class="info-block">
                                        <label>Lines</label>
                                        <asp:Label ID="lblLines" runat="server" CssClass="form-control autocomplete-input textbox-style" Width="100%"></asp:Label>
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>

                <!-- Store Inventory Panel -->
                <a href="#headerToWarehousePanel"
                    class="d365-toggle-header d-flex justify-content-between align-items-center mt-2"
                    data-toggle="collapse"
                    role="button"
                    aria-expanded="false"
                    aria-controls="headerToWarehousePanel">
                    <span class="section-title">Store inventory</span>
                    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                </a>
                <div class="collapse mt-2" id="headerToWarehousePanel">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <div class="row g-4">

                                <!-- Column 1: Site -->
                                <div style="flex: 0 0 14%; margin-left: 20px;">
                                    <div class="info-block">
                                        <label>Site</label>
                                        <asp:Label ID="txtSite" runat="server" CssClass="form-control autocomplete-input textbox-style" Width="100%"></asp:Label>
                                    </div>
                                </div>

                                <!-- Column 2: Warehouse -->
                                <div style="flex: 0 0 14%; margin-left: 20px;">
                                    <div class="info-block">
                                        <label>Warehouse</label>
                                        <asp:Label ID="txtWarehouse" runat="server" CssClass="form-control autocomplete-input textbox-style" Width="100%"></asp:Label>
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>
                <div class="d-none">
                </div>
            </div>
        </div>
    </div>
      </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
