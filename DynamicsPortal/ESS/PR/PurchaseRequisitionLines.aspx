<%@ Page Title="Purchase Requisition Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseRequisitionLines.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseRequisitionLines" %>

<%@ Register Src="~/DropDownList_ProductCombination.ascx" TagPrefix="uc" TagName="ProductLookup" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">


    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />


    <style>
        .card-header {
            padding-bottom: 0;
        }
        .custom-textbox[readonly] {
    color: gray !important;              /* ForeColor */
    background-color: #F3F2F1 !important; /* BackColor */
}

        .custom-section-title {
            text-transform: uppercase;
            font-size: 0.8rem;
            color: #666;
            font-weight: 600;
            margin-top: 1rem;
            margin-bottom: 0.5rem;
        }

        .info-row {
            display: flex;
            flex-wrap: wrap;
            margin-bottom: 1rem;
        }

        .info-block {
            flex: 0 0 22%;
            margin-right: 2%;
            margin-bottom: 1rem;
            min-width: 150px;
        }

            .info-block strong {
                display: block;
                font-weight: 400;
                font-size: 0.75rem;
                color: #555;
            }

            .info-block > span {
                display: block;
            }

                .info-block > span > span {
                    display: block;
                    font-weight: 500;
                    font-size: 0.85rem;
                    color: #0078d4;
                    border-bottom: 1px solid #ccc;
                    padding-bottom: 2px;
                    margin-top: 1px;
                }

        .d365-toggle-header {
            background-color: #f3f3f3;
            padding: 0.75rem 1rem;
            border: 1px solid #dcdcdc;
            border-radius: 4px;
            font-weight: 600;
            color: #212529;
            cursor: pointer;
            text-decoration: none;
            transition: background-color 0.2s;
            margin-top: 1.25rem;
        }

            .d365-toggle-header:hover {
                background-color: #e5e5e5;
                color: #000;
            }

        .section-title {
            font-size: 1rem;
        }

        .rotate-icon {
            transition: transform 0.3s ease;
        }

        .d365-toggle-header[aria-expanded="true"] .rotate-icon {
            transform: rotate(180deg);
        }

        .tab-content p {
            margin-bottom: 0;
        }

        .card-body {
            padding: 1.25rem;
        }

        .editable-wrapper {
            position: relative;
            display: block;
        }

            .editable-wrapper .edit-icon {
                position: absolute;
                right: 8px;
                top: 50%;
                transform: translateY(-50%);
                color: #0078d4;
                font-size: 1rem;
                pointer-events: none;
            }

        .info-block input[type="text"],
        .info-block textarea,
        .info-block .aspnet-textbox {
            display: block;
            width: 100%;
            font-weight: 500;
            font-size: 0.85rem;
            color: #0078d4;
            border: none;
            border-bottom: 1px solid #ccc;
            padding: 0 0 2px 0;
            margin-top: 1px;
            background-color: transparent;
            box-shadow: none;
            outline: none;
        }

            .info-block input[type="text"]:focus,
            .info-block textarea:focus {
                border-bottom-color: #0078d4;
            }

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

            .round-checkbox input[type="checkbox"]:checked,
            input#chk_SelectAll:checked {
                background-color: #007BFF;
                border-color: #007BFF;
            }

            .round-checkbox input[type="checkbox"]:focus,
            input#chk_SelectAll:focus {
                box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.5);
            }
    </style>
    <style>
        .disabled-button {
            color: gray !important;
            cursor: not-allowed !important;
            pointer-events: none; /* Optional: prevents all mouse interaction */
        }
    </style>
    <script>
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: "m-d-yy" // Example: 9-12-2025
            });
        });
    </script>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">

    <div class="action-items">
        <a href="/ESS/PR/PurchaseRequisitionHeader_ListPage.aspx" class="btn-link">
            <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
        </a>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="BtnSaveHeader" runat="server" OnClick="BtnSave_Header_Click">
        <i class="mdi mdi-content-save" style="margin-right: 4px;"></i>Save
        </asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="BtnDeleteHeader" runat="server" OnClick="BtnDeleteHeader_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
    </div>
    <div class="action-items dropdown">
        <asp:LinkButton ID="btnWorkflow" runat="server" OnClientClick="toggleDropdown(); return false;">
        <i class="mdi mdi-sitemap"></i> Workflow
        </asp:LinkButton>

        <div id="workflowMenu" class="custom-dropdown">
            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="dropdown-item" OnClick="btnSubmit_Click">
            <i class="mdi mdi-shape-plus"></i> Submit
            </asp:LinkButton>
        </div>
    </div>

    <style>
        .custom-dropdown {
            display: none;
            position: absolute;
            background-color: #fff;
            border: 1px solid #ccc;
            padding: 0.5rem;
            z-index: 1000;
        }
    </style>

    <script>
        function toggleDropdown() {
            var menu = document.getElementById('workflowMenu');
            if (menu.style.display === 'block') menu.style.display = 'none';
            else menu.style.display = 'block';
        }
    </script>
    <div class="action-items">
        <asp:LinkButton ID="btnGoToRFQ" runat="server" Visible="false" OnClick="BtnGotoReqQuo">
        <i class="mdi mdi-file-document-box"></i>Request for Quotation
        </asp:LinkButton>
    </div>
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <asp:UpdatePanel ID="upGrid" ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>
            <div style="display: flex; justify-content: space-between; align-items: center; width: 100%;">
                <!-- Left side labels -->
                <div>
                    <asp:Label ID="lblRequisitionNumber" runat="server"></asp:Label>
                    <span>-</span>
                    <asp:Label ID="lblRequisitionName" runat="server"></asp:Label>
                </div>

                <!-- Middle label -->
                <div>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </div>

                <!-- Right side label -->
                <div>
                    <asp:Label ID="lblRequisitionStatus" runat="server"></asp:Label>
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
                <div class="card-body tab-content">
                    <!-- Lines Tab -->
                    <div class="tab-pane fade show active" id="lines" role="tabpanel">
                        <a href="#headerPanel" class="d365-toggle-header d-flex justify-content-between align-items-center" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="headerPanel">
                            <span class="section-title">Purchase requisition header</span>
                            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                        </a>
                        <div class="collapse show mt-4" id="headerPanel">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <div class="row">

                                        <!-- ADMINISTRATION -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>ADMINISTRATION</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Purchase Requisition</strong>
                                                <asp:TextBox ID="headerRequisitionNumber" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Name</strong>
                                                <asp:TextBox ID="headerRequisitionName" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>

                                        <!-- PREPARER -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Preparer</strong>
                                                <asp:TextBox ID="headerPreparer" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- STATUS + REQUISITION PURPOSE -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Status</strong>
                                                <asp:TextBox ID="headerStatus" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Requisition Purpose</strong>
                                                <asp:TextBox ID="headerPurpose" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- DATES -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>DATES</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Requested Date</strong>
                                                <asp:TextBox ID="headerRequestedDate" runat="server" CssClass="datepicker" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Accounting Date</strong>
                                                <asp:TextBox ID="headerAccountingDate" runat="server" CssClass="datepicker" />
                                            </div>
                                        </div>

                                        <!-- BUSINESS JUSTIFICATION -->
                                        <div class="col-md-4">
                                            <div class="custom-section-title mb-2"><strong>BUSINESS JUSTIFICATION</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Reason</strong>
<%--                                                <asp:TextBox ID="headerReason" runat="server" CssClass="form-control" Width="250px" ReadOnly="true" />--%>
                                                <asp:DropdownList ID="ddlheaderReason" runat="server" CssClass="" Width="250px" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Details</strong>
                                                <asp:TextBox ID="headerDetails" runat="server" CssClass="form-control custom-textbox" Width="250px" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                                            </div>
                                        </div>



                                    </div>

                                </div>
                            </div>
                        </div>

                        <a class="d365-toggle-header d-flex justify-content-between align-items-center mt-3">
                            <span class="section-title">Purchase requisition lines</span>
                            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                        </a>
                        <div class="collapse show mt-2" id="gridPanel">
                            <div class="card shadow-sm">
                                <div class="card-body pt-2 pb-2">
                                    <div class="text-left d-flex align-items-center gap-4">
                                        <div class="action-items grid-btn">
                                            <asp:LinkButton ID="BtnAddLine" runat="server" OnClick="btnAdd_Click" CssClass="text-primary" Style="font-size: 0.9rem;">
                                                <i class="mdi mdi-plus" style="font-size: 0.9rem;"></i> Add line
                                            </asp:LinkButton>
                                        </div>
                                        <div class="action-items grid-btn" style="margin-left: 30px;">
                                            <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Style="font-size: 0.9rem;">
                                                <i class="mdi mdi-delete" style="font-size: 0.9rem;"></i> Remove
                                            </asp:LinkButton>
                                        </div>
                                        <div class="action-items grid-btn" style="margin-left: 30px;">
                                            <asp:LinkButton ID="btnCancel"
                                                runat="server" OnClick="btnCancel_Click"
                                                OnClientClick="showCancelPopup(); return false;"
                                                Style="font-size: 0.9rem;">
                                                <i class="mdi mdi-cancel" style="font-size: 0.9rem;"></i> Cancel
                                            </asp:LinkButton>
                                        </div>
                                        <div id="cancelPopup"
                                            style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.4); z-index: 9999; text-align: center;">

                                            <div style="position: relative; top: 50%; left: 50%; transform: translate(-50%, -50%); background: #fff; padding: 25px 30px; border-radius: 10px; box-shadow: 0 5px 15px rgba(0,0,0,0.3); width: 320px; font-family: Arial, sans-serif;">

                                                <p style="font-size: 1rem; font-weight: 500; margin-bottom: 20px; color: #333;">
                                                    Are you sure you want to cancel this record?
                                                </p>

                                                <div style="display: flex; justify-content: center; gap: 15px;">
                                                    <button type="button"
                                                        onclick="confirmCancel(true)"
                                                        style="background: #e74c3c; color: #fff; border: none; padding: 8px 16px; border-radius: 6px; cursor: pointer; font-size: 0.9rem;">
                                                        Yes
                                                    </button>

                                                    <button type="button"
                                                        onclick="confirmCancel(false)"
                                                        style="background: #bdc3c7; color: #2c3e50; border: none; padding: 8px 16px; border-radius: 6px; cursor: pointer; font-size: 0.9rem;">
                                                        No
                                                    </button>
                                                </div>
                                            </div>
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
                            <div class="table-responsive transfer-line-card-body" style="overflow-x: auto; white-space: nowrap;">
                                <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border sortable grid-table" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId" OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" OnRowCancelingEdit="gridView_RowCancelingEdit"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                            <HeaderTemplate>
                                                <input type="checkbox" id="chk_SelectAll" cssclass="round-checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_SelectSingle" AutoPostBack="true" OnCheckedChanged="chk_SelectSingle_CheckedChanged" runat="server" CssClass="round-checkbox" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valid">
                                            <ItemTemplate>
                                                <asp:Label ID="lblValidNumber" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSequenceNumber" runat="server" Text='<%# Bind("SequenceNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Requester">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequisitioner" runat="server" Text='<%# Bind("Requisitioner") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlRequisitioner" Enabled="false" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Buying legal entity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBuyingLegalEntity" runat="server" Text='<%# Bind("BuyingLegalEntity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlBuyingLegalEntity" Enabled="false" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Receiving operating unit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReceivingOperatingUnit" runat="server" Text='<%# Bind("DepartmentName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="ddlReceivingOperatingUnit" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlReceivingOperatingUnit_SelectedIndexChanged" runat="server"></asp:TextBox>
                                                <asp:DropDownList ID="ddlReceivingOperatingUnitRecid" Enabled="false" runat="server" Visible="false"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Receiving Operating Unit RefRecId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReceivingOperatingUnitRefRecid" runat="server" Text='<%# Bind("ReceivingOperatingUnit") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlReceivingOperatingUnitRefRecId" Enabled="false" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item number">
                                            <ItemTemplate>
                                                <asp:Label ID="ItemId" runat="server" Text='<%# Bind("ItemId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlItemId" runat="server" OnSelectedIndexChanged="ddlTemId_fillDimension" AutoPostBack="true"></asp:DropDownList>
                                                <!-- Label for Edit existing row -->
                                                <asp:Label ID="lblItemIdEdit" runat="server" Text='<%# Bind("ItemId") %>' Visible="false"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Procurement category">
                                            <ItemTemplate>
                                                <asp:Label ID="ProcurementCategory" runat="server" Text='<%# Bind("ProcurementCategory") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtProcurementCategory" Enabled="false" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product name">
                                            <ItemTemplate>
                                                <asp:Label ID="PrdouctName" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtProductName" runat="server" Enabled="false"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="PurchQty" runat="server" CssClass="format-number" Text='<%# Bind("PurchQty") %>' Style="text-align: right; display: block;"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPurchQty" TextMode="Number" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit">
                                            <ItemTemplate>
                                                <asp:Label ID="ProductUnit" runat="server" Text='<%# Bind("PurchUnitofMeasureCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtProductUnit" Enabled="false" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PurchUnitofMeasure" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchUnitofMeasure" runat="server" Text='<%# Bind("PurchUnitofMeasure") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPurchUnitofMeasure" runat="server" Enabled="false"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Unit price">
                                            <ItemTemplate>
                                                <asp:Label ID="UnitPrice" runat="server" CssClass="format-number" Text='<%# Bind("PurchPrice") %>' Style="text-align: right; display: block;"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtUnitPrice" TextMode="Number" Enabled="false" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Net amount">
                                            <ItemTemplate>
                                                <asp:Label ID="NetAmount" runat="server" CssClass="format-number" Text='<%# Bind("LineAmount") %>' Style="text-align: right; display: block;"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNetAmount" TextMode="Number" Enabled="false" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Currency">
                                            <ItemTemplate>
                                                <asp:Label ID="CurrencyCode" runat="server" Text='<%# Bind("CurrencyCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCurrencyCode" Enabled="False" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor account">
                                            <ItemTemplate>
                                                <asp:Label ID="VendAccount" runat="server" Text='<%# Bind("VendAccount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtVendAccount" Enabled="false" Visible="true" runat="server"></asp:TextBox>
                                                <asp:DropDownList ID="ddlVendAccount" Enabled="false" Visible="false" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor name">
                                            <ItemTemplate>
                                                <asp:Label ID="VendorName" runat="server" Text='<%# Bind("VendorName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtVendorName" Enabled="false" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor status" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="VendorStatus" runat="server" Text='<%# Bind("VendorStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="RequisitionStatus" runat="server" Text='<%# Bind("RequisitionStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RecId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LineType" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLineType" runat="server" Text='<%# Bind("LineType") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="InternetAddress" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInternetAddress" runat="server" Text='<%# Bind("InternetAddress") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ExternalItemId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExternalItemId" runat="server" Text='<%# Bind("ExternamItemID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PurchSupplierAuxId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchSupplierAuxId" runat="server" Text='<%# Bind("SupplierAuxId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("Category") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category Code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryCode" runat="server" Text='<%# Bind("CategoryCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PriceUnit" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPriceUnit" runat="server" Text='<%# Bind("PriceUnit") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DiscountPercent" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscountPercent" runat="server" Text='<%# Bind("DiscountPercent") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("Discount") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ChargesOnPurchases" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChargesOnPurchases" runat="server" Text='<%# Bind("ChargesonDiscount") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RFQRequirement" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRFQRequirement" runat="server" Text='<%# Bind("RFQRequirement") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PreventPartialDeliveryFlag" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPreventPartialDeliveryFlag" runat="server" Text='<%# Bind("PreventPartialDeliveryFlag") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PrePaymentRequired" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrePaymentRequired" runat="server" Text='<%# Bind("PrePaymentRequired") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RequestforQuotationCase" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestforQuotationCase" runat="server" Text='<%# Bind("RequestforQuotationCase") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RequestforQuotationCaseStatus" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestforQuotationCaseStatus" runat="server" Text='<%# Bind("RequestforQuotationCaseStatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ConsolidationOppurtunityId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblConsolidationOppurtunityId" runat="server" Text='<%# Bind("ConsolidationOppurtunityId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ConsolidationOppurtunityIdStatus" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblConsolidationOppurtunityIdStatus" runat="server" Text='<%# Bind("ConsolidationOppurtunityIdStatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PurchaseOrder" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchaseOrder" runat="server" Text='<%# Bind("PurchaseOrder") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoice" runat="server" Text='<%# Bind("Invoice") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="InvoiceStatus" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceStatus" runat="server" Text='<%# Bind("InvoiceStatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PurchaseAgreementId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchaseAgreementId" runat="server" Text='<%# Bind("PurchaseAgreementId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PurchaseAgreementStatus" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchaseAgreementStatus" runat="server" Text='<%# Bind("PurchaseAgreementStatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DeliveryAddress" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveryAddress" runat="server" Text='<%# Bind("DeliveryAddress") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DeliveryPostalAddress" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveryPostalAddress" runat="server" Text='<%# Bind("DeliveryPostalAddress") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DeliveryAttention" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveryAttention" runat="server" Text='<%# Bind("DeliveryAttention") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ProjSalesCurrencyId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProjSalesCurrencyId" runat="server" Text='<%# Bind("ProjSalesCurrencyId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ProjSalesPrice" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProjSalesPrice" runat="server" Text='<%# Bind("ProjSalesPrice") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ProjSalesUnit" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProjSalesUnit" runat="server" Text='<%# Bind("ProjSalesUnit") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ProjectTaxGroupId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProjectTaxGroupId" runat="server" Text='<%# Bind("ProjectTaxGroupId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ProjectItemTaxGroupId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProjectItemTaxGroupId" runat="server" Text='<%# Bind("ProjectItemTaxGroupId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Sales Tax Group" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemSalesTaxGroup" runat="server" Text='<%# Bind("TaxItemGroup") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Transaction ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransactionID" runat="server" Text='<%# Bind("TransactionID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Combinations">
                                            <EditItemTemplate>
                                                <div style="min-width: 400px; overflow: visible;">
                                                    <uc:ProductLookup ID="ProductLookupControl" runat="server" OnProductSelected="ProductLookupControl_ProductSelected" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Configuration" Visible="false">
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
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlInventSiteId" AutoPostBack="true" OnSelectedIndexChanged="ddlInventSiteId_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Warehouse">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInventLocationId" runat="server" Text='<%# Bind("InventLocationId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlInventLocationId" AutoPostBack="true" OnSelectedIndexChanged="ddllocation_selection" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
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
                                        <asp:TemplateField HeaderText="WMS Pallet" Visible="false">
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
                                        <asp:TemplateField HeaderText="Default Dimension" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDefaultDimension" runat="server" Text='<%# Bind("DefaultDimension") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Business Unit" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBusinessUnit" runat="server" Text='<%# Bind("BusinessUnit") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cost Center" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCostCenter" runat="server" Text='<%# Bind("CostCenter") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlCostCenter" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("Department") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Group" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemGroup" runat="server" Text='<%# Bind("ItemGroup") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlItemGroup" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Main Account" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMainAccount" runat="server" Text='<%# Bind("MainAccount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlMainAccount" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProject" runat="server" Text='<%# Bind("Project") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="InventDimId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInventDimId" runat="server" Text='<%# Bind("InventDimId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                                        <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
                                        <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                                        <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
                                        <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
                                        <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false" />
                                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                            <EditItemTemplate>
                                                <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                                                <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:HiddenField ID="EditMode" runat="server" />
                            </div>
                        </div>

                        <asp:HiddenField ID="hfRequisitioner" runat="server" />

                        <a href="#detailPanel" class="d365-toggle-header d-flex justify-content-between align-items-center" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="detailPanel">
                            <span class="section-title">Line details</span>
                            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                        </a>
                        <div class="collapse show mt-4" id="detailPanel">
                            <div class="card shadow-sm">
                                <div class="card-header bg-light">
                                    <ul class="nav nav-tabs" id="detailTabs" role="tablist">
                                        <li class="nav-item">
                                            <a class="nav-link active" id="item-tab" data-toggle="tab" href="#item" role="tab">Item</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" id="general-tab" data-toggle="tab" href="#general" role="tab">General</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" id="details-tab" data-toggle="tab" href="#details" role="tab">Details</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" id="address-tab" data-toggle="tab" href="#address" role="tab">Address</a>
                                        </li>
                                       <%-- <li class="nav-item">
                                            <a class="nav-link" id="project-tab" data-toggle="tab" href="#project" role="tab">Project</a>
                                        </li>--%>
                                       <%-- <li class="nav-item">
                                            <a class="nav-link" id="questionarie-tab" data-toggle="tab" href="#questionarie" role="tab">Questionnaire</a>
                                        </li>--%>
                                      <%--  <li class="nav-item">
                                            <a class="nav-link" id="fixedassets-tab" data-toggle="tab" href="#fixedassets" role="tab">Fixed assets</a>
                                        </li>--%>
                                        <li class="nav-item">
                                            <a class="nav-link" id="financialdimensions-tab" data-toggle="tab" href="#financialdimensions" role="tab">Financial dimensions</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" id="inventorydimensions-tab" data-toggle="tab" href="#inventorydimensions" role="tab">Inventory dimensions</a>
                                        </li>
                                    </ul>
                                </div>
                                <div class="tab-content p-3">
                                    <div class="tab-pane fade show active" id="item" role="tabpanel" aria-labelledby="item-tab">
                                        <div class="row g-4">
                                            <!-- spacing between blocks -->

                                            <!-- ITEM COLUMN -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>ITEM</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Item number</strong>
                                                    <asp:TextBox ID="txtItemNumber" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Item description</strong>
                                                    <asp:TextBox ID="txtItemDescription" runat="server" CssClass="form-control custom-textbox"
                                                        TextMode="MultiLine" Rows="4" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>

                                            <!-- ITEM DETAILS COLUMN -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong></strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Line type</strong>
                                                    <asp:TextBox ID="txtLineType" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Internet address</strong>
                                                    <asp:TextBox ID="txtInternetAddress" runat="server" CssClass="form-control autocomplete-input" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>External item number</strong>
                                                    <asp:TextBox ID="txtExternalItemNumber" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Product name</strong>
                                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Supplier part auxiliary ID</strong>
                                                    <asp:TextBox ID="txtSupplierPartAuxID" runat="server" CssClass="form-control autocomplete-input" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- PROCUREMENT CATEGORY + PRICE COLUMN -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>PROCUREMENT CATEGORY</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Category</strong>
                                                    <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Code</strong>
                                                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Friendly name</strong>
                                                    <asp:TextBox ID="txtFriendlyName" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="custom-section-title mb-2 mt-3"><strong>PRICE</strong></div>

                                                <div class="info-block">
                                                    <strong>Quantity</strong>
                                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control autocomplete-input" ReadOnly="true" Style="text-align:right;"  />
                                                </div>
                                            </div>

                                            <!-- PRICE DETAILS COLUMN -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong></strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Unit</strong>
                                                    <asp:TextBox ID="txtUnit" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Unit price</strong>
                                                    <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" Style="text-align:right;" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Net amount</strong>
                                                    <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" Style="text-align:right;" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Currency</strong>
                                                    <asp:TextBox ID="txtCurrency" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Price unit</strong>
                                                    <asp:TextBox ID="txtPriceUnit" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" Style="text-align:right;" />
                                                </div>
                                            </div>

                                            <!-- CHARGES AND DISCOUNTS & VENDOR INFORMATION COLUMN -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>CHARGES AND DISCOUNTS</strong></div>
                                                <div class="info-block mb-2">
                                                    <strong>Discount percent</strong>
                                                    <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" Style="text-align:right;" />
                                                </div>
                                                <div class="info-block mb-2">
                                                    <strong>Discount</strong>
                                                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" Style="text-align:right;" />
                                                </div>
                                                <div class="info-block mb-2">
                                                    <strong>Charges on purchases</strong>
                                                    <asp:TextBox ID="txtChargesOnPurchases" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" Style="text-align:right;" />
                                                </div>

                                                <div class="custom-section-title mb-2 mt-3"><strong>VENDOR INFORMATION</strong></div>
                                                <div class="info-block mb-2">
                                                    <strong>Vendor name</strong>
                                                    <asp:TextBox ID="txtVendorName" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- NEW VENDOR DETAILS COLUMN -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong></strong></div>
                                                <div class="info-block mb-2">
                                                    <strong>Vendor account</strong>
                                                    <asp:TextBox ID="txtVendorAccount" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                                <div class="info-block mb-2">
                                                    <strong>Vendor status</strong>
                                                    <asp:TextBox ID="txtVendorStatus" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                                <div class="info-block mb-2">
                                                    <strong>RFQ requirement</strong>
                                                    <asp:TextBox ID="txtRFQRequirement" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>


                                        </div>

                                    </div>

                                    <div class="tab-pane fade" id="general" role="tabpanel" aria-labelledby="general-tab">
                                        <div class="row g-4">
                                            <!-- spacing between blocks -->

                                            <!-- ADMINISTRATION COLUMN -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>ADMINISTRATION</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Requester</strong>
                                                    <asp:TextBox ID="txtRequester" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Buying legal entity</strong>
                                                    <asp:TextBox ID="txtBuyingLegalEntity" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- RECEIVING & STATUS COLUMN -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>
                                                <!-- empty heading to align -->

                                                <div class="info-block mb-3">
                                                    <strong>Receiving operating unit</strong>
                                                    <asp:TextBox ID="txtReceivingOperatingUnit" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Status</strong>
                                                    <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- DATES COLUMN -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>DATES</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Requested date</strong>
                                                    <asp:TextBox ID="txtRequestedDate" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Accounting date</strong>
                                                    <asp:TextBox ID="txtAccountingDate" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- BUSINESS JUSTIFICATION COLUMN -->
                                            <div class="col-md-4">
                                                <div class="custom-section-title mb-2"><strong>BUSINESS JUSTIFICATION</strong></div>

            <div class="info-block mb-3">
                <strong>Reason</strong>
             <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control" Width="250px">
</asp:DropDownList>

            </div>

                                                <div class="info-block">
                                                    <strong>Details</strong>
                                                    <asp:TextBox ID="txtDetails" runat="server" CssClass="form-control custom-textbox"
                                                        TextMode="MultiLine" Rows="5" Width="250px" ReadOnly="true" />
                                                </div>
                                            </div>



                                        </div>
                                    </div>


                                    <div class="tab-pane fade" id="details" role="tabpanel" aria-labelledby="details-tab">
                                        <div class="row g-4">
                                            <!-- spacing between blocks -->

                                            <!-- DELIVERY TERMS -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>DELIVERY TERMS</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Prevent partial delivery</strong>
                                                    <div class="toggle-group mt-2">
                                                        <label class="toggle-switch">
                                                            <asp:CheckBox ID="chkPreventPartialDelivery" runat="server" ReadOnly="true" />
                                                            <span class="slider"></span>
                                                        </label>
                                                    </div>
                                                </div>

                                                <!-- PREPAYMENT -->
                                                <div class="custom-section-title mb-2"><strong>PREPAYMENT</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Prepayment required</strong>
                                                    <div class="toggle-group mt-2">
                                                        <label class="toggle-switch">
                                                            <asp:CheckBox ID="chkPrepaymentRequired" runat="server" ReadOnly="true" />
                                                            <span class="slider"></span>
                                                        </label>
                                                    </div>
                                                </div>

                                                <div class="info-block">
                                                    <strong>Prepayment details</strong>
                                                    <asp:TextBox ID="txtPrepaymentDetails" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>


                                            <!-- PURCHASE ORDER CREATION -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>PURCHASE ORDER CREATION</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Price/discount transfer</strong>
                                                    <asp:TextBox ID="txtPriceDiscountTransfer" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <asp:TextBox ID="txtPurchaseOrderNotes" runat="server" ForeColor="gray" BackColor="#F3F2F1" TextMode="MultiLine" Rows="3" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- REFERENCES -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>REFERENCES</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Request for quotation case</strong>
                                                    <asp:TextBox ID="txtRFQCase" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Request for quotation case status</strong>
                                                    <asp:TextBox ID="txtRFQCaseStatus" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Consolidation opportunity</strong>
                                                    <asp:TextBox ID="txtConsolidationOpportunity" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- CONSOLIDATION & PURCHASE ORDER -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong></strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Consolidation opportunity status</strong>
                                                    <asp:TextBox ID="txtConsolidationOpportunityStatus" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Purchase order</strong>
                                                    <asp:TextBox ID="txtPurchaseOrder" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Purchase order status</strong>
                                                    <asp:TextBox ID="txtPurchaseOrderStatus" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- INVOICE & AGREEMENT -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong></strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Invoice</strong>
                                                    <asp:TextBox ID="txtInvoice" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Invoice status</strong>
                                                    <asp:TextBox ID="txtInvoiceStatus" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Purchase agreement</strong>
                                                    <asp:TextBox ID="txtPurchaseAgreement" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Purchase agreement status</strong>
                                                    <asp:TextBox ID="txtPurchaseAgreementStatus" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- SALES TAX -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>SALES TAX</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Item sales tax group</strong>
                                                    <asp:TextBox ID="txtItemSalesTaxGroup" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Sales tax group</strong>
                                                    <asp:TextBox ID="txtSalesTaxGroup" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="tab-pane fade" id="address" role="tabpanel" aria-labelledby="address-tab">
                                        <div class="row g-4">
                                            <!-- spacing between blocks -->

                                            <!-- DELIVERY ADDRESS -->
                                            <div class="col-md-3">
                                                <div class="custom-section-title mb-2"><strong>DELIVERY ADDRESS</strong></div>

                                                <div class="info-block">
                                                    <strong>Delivery name</strong>
                                                    <asp:TextBox ID="txtDeliveryName" runat="server" CssClass="form-control"
                                                        TextMode="MultiLine" Width="250px" Rows="4" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- DELIVERY ADDRESS DETAILS -->
                                            <div class="col-md-3">


                                                <div class="info-block mb-3">
                                                    <strong>Delivery address</strong>
                                                    <asp:TextBox ID="txtDeliveryAddress" Width="250px" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Address</strong>
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control custom-textbox"
                                                        TextMode="MultiLine" Width="250px" Rows="4" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- ATTENTION -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>ATTENTION</strong></div>

                                                <div class="info-block">
                                                    <strong>Attention information</strong>
                                                    <asp:TextBox ID="txtAttentionInfo" runat="server" CssClass="form-control"
                                                        TextMode="MultiLine" Width="250px" Rows="4" ReadOnly="true" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>


                                    <div class="tab-pane fade" id="project" role="tabpanel" aria-labelledby="project-tab">
                                        <div class="row g-4">
                                            <!-- spacing between blocks -->

                                            <!-- PROJECT IDENTIFICATION -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>PROJECT IDENTIFICATION</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Project ID</strong>
                                                    <asp:TextBox ID="txtProjectID" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Activity number</strong>
                                                    <asp:TextBox ID="txtActivityNumber" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- PROJECT DETAILS -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Project Category</strong>
                                                    <asp:TextBox ID="txtProjectCategory" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Item number</strong>
                                                    <asp:TextBox ID="txtProjectItemNumber" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Line property</strong>
                                                    <asp:TextBox ID="txtProjectLineProperty" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- TRANSACTION + COST PRICE -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>TRANSACTION</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Transaction ID</strong>
                                                    <asp:TextBox ID="txtTransactionID" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="custom-section-title mb-2"><strong>COST PRICE</strong></div>

                                                <div class="info-block">
                                                    <strong>Quantity</strong>
                                                    <asp:TextBox ID="txtCostPriceQuantity" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- COST PRICE (continued) -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Unit price</strong>
                                                    <asp:TextBox ID="txtlineUnitPrice" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Net amount</strong>
                                                    <asp:TextBox ID="TxtlinenetAmount" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- SALES PRICE -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>SALES PRICE</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Sales currency</strong>
                                                    <asp:TextBox ID="txtSalesCurrency" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Sales price</strong>
                                                    <asp:TextBox ID="txtSalesPrice" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Sales unit</strong>
                                                    <asp:TextBox ID="txtSalesUnit" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- PROJECT - SALES TAX -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>PROJECT - SALES TAX</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>Project sales tax group</strong>
                                                    <asp:TextBox ID="txtProjectSalesTaxGroup" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Project sales item tax group</strong>
                                                    <asp:TextBox ID="txtProjectSalesItemTaxGroup" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>


                                        </div>
                                    </div>


                                    <div class="tab-pane fade d-none" id="questionarie" role="tabpanel" aria-labelledby="questionarie-tab">
                                        <div class="mt-3">
                                            <div class="mb-3">
                                                <button type="button" class="btn btn-secondary me-2" disabled>Complete questionnaire</button>
                                                <button type="button" class="btn btn-secondary" disabled>View answers</button>
                                            </div>
                                            <asp:GridView ID="QuestionnaireGrid" runat="server" CssClass="table table-bordered table-sm w-100" AutoGenerateColumns="False" Visible="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found.">
                                                <Columns>
                                                    <asp:BoundField DataField="Questionnaire" HeaderText="Questionnaire" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>


                                    <div class="tab-pane fade" id="fixedassets" role="tabpanel" aria-labelledby="fixedassets-tab">
                                        <div class="row g-4">
                                            <!-- spacing between blocks -->

                                            <!-- Fading Caption -->
                                            <div class="col-12">
                                                <p class="text-muted small fst-italic">Set to default</p>
                                            </div>

                                            <!-- ASSET QUALIFICATION -->
                                            <div class="col-md-3">
                                                <div class="custom-section-title mb-2"><strong>ASSET QUALIFICATION</strong></div>

                                                <div class="info-block">
                                                    <strong>Asset group</strong>
                                                    <asp:TextBox ID="txtAssetGroup" Width="250px" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- ADDITIONAL ASSET INFORMATION -->
                                            <div class="col-md-3">
                                                <div class="custom-section-title mb-2"><strong>ADDITIONAL ASSET INFORMATION</strong></div>

                                                <div class="info-block">
                                                    <strong>Reason code</strong>
                                                    <asp:TextBox ID="txtReasonCode" Width="250px" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="tab-pane fade" id="financialdimensions" role="tabpanel" aria-labelledby="financialdimensions-tab">
                                        <div class="d-flex justify-content-between flex-wrap">
                                            <div class="flex-grow-1" style="min-width: 300px;">
                                                <div class="custom-section-title">FINANCIAL DIMENSIONS</div>
                                                <div runat="server" id="financialDimensionsContainer"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="tab-pane fade" id="inventorydimensions" role="tabpanel" aria-labelledby="inventorydimensions-tab">
                                        <div class="row g-4">
                                            <!-- spacing between blocks -->

                                            <!-- PRODUCT DIMENSIONS -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>PRODUCT DIMENSIONS</strong></div>

            <div class="info-block mb-3">
                <strong>Configuration</strong>
                <asp:DropDownList ID="ddlConfigurationLineDetail" runat="server" CssClass="form-control" />
            </div>

            <div class="info-block">
                <strong>Size</strong>
                <asp:DropDownList ID="ddlSizeLineDetail" runat="server" CssClass="form-control"/>
            </div>

            <div class="info-block">
                <strong>Style</strong>
                <asp:DropDownList ID="ddlStyleLineDetail" runat="server" CssClass="form-control" />
            </div>
        </div>

                                            <!-- PRODUCT DIMENSIONS Continued -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

            <div class="info-block mb-3">
                <strong>Color</strong>
                <asp:DropDownList ID="ddlColorLineDetail" runat="server" CssClass="form-control" />
            </div>

            <div class="info-block mb-3">
                <strong>Version</strong>
                <asp:TextBox ID="txtVersionLineDetail" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>
        </div>

                                            <!-- INVENTORY DIMENSIONS -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>INVENTORY DIMENSIONS</strong></div>

            <div class="info-block mb-3">
                <strong>Site</strong>
                <asp:DropDownList ID="ddlSiteLineDetail" OnTextChanged="ddlSiteLineDetail_SelectedIndexChanged"  AutoPostBack="true" runat="server" CssClass="form-control" />
            </div>

            <div class="info-block">
                <strong>Warehouse</strong>
                <asp:DropDownList ID="ddlWarehouseLineDetail" OnTextChanged="ddlWarehouseLineDetail_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control" />
            </div>
        </div>

                                            <!-- INVENTORY DIMENSIONS Continued -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

            <div class="info-block mb-3">
                <strong>Batch number</strong>
                <asp:DropDownList ID="ddlBatchNumberLineDetail" runat="server" CssClass="form-control"  />
            </div>

            <div class="info-block">
                <strong>Location</strong>
                <asp:DropDownList ID="ddlWmsLocationLineDetail" runat="server" CssClass="form-control" />
            </div>
        </div>

                                            <!-- INVENTORY DIMENSIONS Continued -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

            <div class="info-block mb-3">
                <strong>Serial number</strong>
                <asp:DropDownList ID="ddlSerialNumberLineDetail" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>

                                                <div class="info-block">
                                                    <strong>Inventory status</strong>
                                                    <asp:TextBox ID="txtInventoryStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- INVENTORY DIMENSIONS Continued -->
                                            <div class="col-md-2">
                                                <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                                <div class="info-block mb-3">
                                                    <strong>License plate</strong>
                                                    <asp:TextBox ID="txtLicensePlate" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block mb-3">
                                                    <strong>Owner</strong>
                                                    <asp:TextBox ID="txtOwner" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>

                                                <div class="info-block">
                                                    <strong>Inventory profile</strong>
                                                    <asp:TextBox ID="txtInventoryProfile" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Header Tab -->
                    <div class="tab-pane fade" id="header" role="tabpanel">
                        <a href="#headerPanelTab" class="d365-toggle-header d-flex justify-content-between align-items-center" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="headerPanelTab">
                            <span class="section-title">Purchase requisition header</span>
                            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                        </a>
                        <div class="collapse mt-4 show" id="headerPanelTab">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <div class="row">
                                        <!-- ADMINISTRATION -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>ADMINISTRATION</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Purchase Requisition</strong>
                                                <asp:TextBox ID="headerTabRequisitionNumber" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Name</strong>
                                                <asp:TextBox ID="headerTabRequisitionName" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- PREPARER -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Preparer</strong>
                                                <asp:TextBox ID="headerTabPreparer" runat="server" ForeColor="gray" BackColor="#F3F2F1" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- STATUS + REQUISITION PURPOSE -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>&nbsp;</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Status</strong>
                                                <asp:TextBox ID="headerTabStatus" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Requisition Purpose</strong>
                                                <asp:TextBox ID="headerTabPurpose" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- DATES -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>DATES</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Requested Date</strong>
                                                <asp:TextBox ID="headerTabRequestedDate" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Accounting Date</strong>
                                                <asp:TextBox ID="headerTabAccountingDate" runat="server" CssClass="form-control custom-textbox" ReadOnly="true" />
                                            </div>
                                        </div>

                                        <!-- BUSINESS JUSTIFICATION -->
                                        <div class="col-md-2">
                                            <div class="custom-section-title mb-2"><strong>BUSINESS JUSTIFICATION</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Reason</strong>
                                                <asp:TextBox ID="txtHeaderTabReason" runat="server" CssClass="form-control custom-textbox" TextMode="MultiLine" Rows="4" />
                                            </div>

                                            <div class="info-block">
                                                <strong>Details</strong>
                                                <asp:TextBox ID="txtHeaderTabDetails" runat="server" CssClass="form-control custom-textbox" TextMode="MultiLine" Rows="4" />
                                            </div>
                                        </div>

<%--                <!-- PROJECT -->
                <div class="col-md-2">
                    <div class="custom-section-title mb-2"><strong>PROJECT</strong></div>

                                            <div class="info-block mb-3">
                                                <strong>Buying Legal Entity</strong>
                                                <asp:TextBox ID="txtHeaderTabBuyingEntity" runat="server" CssClass="form-control" />
                                            </div>

                    <div class="info-block">
                        <strong>Project ID</strong>
                        <asp:TextBox ID="txtHeaderTabProjectID" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>--%>
                                </div>
                            </div>
                        </div>
                        <a href="#historyTabPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-4" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="historyTabPanel">
                            <span class="section-title">History</span>
                            <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                        </a>
                        <div class="collapse mt-4" id="historyTabPanel">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <div class="card-body">
                                        <div class="row g-3">

                                            <!-- Column 1: Created by -->
                                            <div class="col-md-2">
                                                <div class="info-block">
                                                    <strong>Created by</strong>
                                                    <asp:TextBox ID="historyTabCreatedBy" runat="server" Text="" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- Column 2: Created date -->
                                            <div class="col-md-2">
                                                <div class="info-block">
                                                    <strong>Created date</strong>
                                                    <asp:TextBox ID="historyTabCreatedDate" runat="server" Text="" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- Column 3: Modified by -->
                                            <div class="col-md-2">
                                                <div class="info-block">
                                                    <strong>Modified by</strong>
                                                    <asp:TextBox ID="historyTabModifiedBy" runat="server" Text="" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- Column 4: Modified date -->
                                            <div class="col-md-2">
                                                <div class="info-block">
                                                    <strong>Modified date</strong>
                                                    <asp:TextBox ID="historyTabModifiedDate" runat="server" Text="" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- Column 5: Submitted by -->
                                            <div class="col-md-2">
                                                <div class="info-block">
                                                    <strong>Submitted by</strong>
                                                    <asp:TextBox ID="historyTabSubmittedBy" runat="server" Text="" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                            </div>

                                            <!-- Column 6: Submitted date, Source requisition ID, Source system name -->
                                            <div class="col-md-2">
                                                <div class="info-block mb-2">
                                                    <strong>Submitted date</strong>
                                                    <asp:TextBox ID="historyTabSubmittedDate" runat="server" Text="" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                                <div class="info-block mb-2">
                                                    <strong>Source requisition ID</strong>
                                                    <asp:TextBox ID="historyTabSourceReqId" runat="server" Text="" CssClass="form-control custom-textbox" ReadOnly="true" />
                                                </div>
                                                <div class="info-block">
                                                    <strong>Source system name</strong>
                                                    <asp:TextBox ID="historyTabSourceSystemName" runat="server" Text="" CssClass="form-control custom-textbox" ReadOnly="true" />
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
    <script>
        function fetchVendorName(dropdown) {
            var vendAccount = dropdown.value;
            var row = dropdown.closest("tr");
            var txtBox = row.querySelector("input[id*='txtVendorName']");

            if (!vendAccount) {
                txtBox.value = "";
                return;
            }

            // Call WebMethod (AJAX)
            PageMethods.GetVendorName(vendAccount, function (result) {
                txtBox.value = result;
            });
        }
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
                                fetchVendorName($dropdown[0]);
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
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        var horizontalScrollPos = 0;

        prm.add_beginRequest(function (sender, args) {
            showAJAXOverlay();

            // Save horizontal scroll position of the grid container
            var $gridContainer = $('.transfer-line-card-body');
            if ($gridContainer.length > 0) {
                horizontalScrollPos = $gridContainer.scrollLeft();
            }
        });

        prm.add_endRequest(function (sender, args) {
            hideAJAXOverlay();

            // Restore horizontal scroll position
            var $gridContainer = $('.transfer-line-card-body');
            if ($gridContainer.length > 0) {
                $gridContainer.scrollLeft(horizontalScrollPos);
            }
        });
    </script>
    <script>
        function showCancelPopup() {
            document.getElementById('cancelPopup').style.display = 'block';
        }

        function confirmCancel(doCancel) {
            document.getElementById('cancelPopup').style.display = 'none';
            if (doCancel) {
                __doPostBack('<%= btnCancel.UniqueID %>', '');
            }
        }
    </script>
</asp:Content>
