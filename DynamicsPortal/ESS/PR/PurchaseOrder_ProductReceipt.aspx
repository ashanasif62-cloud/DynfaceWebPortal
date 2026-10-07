<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true"
    CodeBehind="PurchaseOrder_ProductReceipt.aspx.cs"
    Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_ProductReceipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Toggle Switch */
        .toggle-switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 24px;
        }
        .toggle-switch input {
            opacity: 0;
            width: 0;
            height: 0;
        }
        .slider {
            position: absolute;
            cursor: pointer;
            inset: 0;
            background-color: #ccc;
            transition: .4s;
            border-radius: 24px;
        }
        .slider:before {
            position: absolute;
            content: "";
            height: 18px;
            width: 18px;
            left: 3px;
            bottom: 3px;
            background-color: #fff;
            transition: .4s;
            border-radius: 50%;
        }
        input:checked + .slider {
            background-color: #4CAF50;
        }
        input:checked + .slider:before {
            transform: translateX(26px);
        }

        /* Card / Grid Styling */
        .card-header a {
            text-decoration: none;
            color: #000;
        }
        .rotate-icon {
            transition: transform .3s ease;
        }
        .collapsed .rotate-icon {
            transform: rotate(180deg);
        }
        .card-body {
            padding-left: 4px !important;
            padding-top: 6px !important;
        }
        .table th {
            padding: 6px !important;
        }
        .table td {
            padding: 4px !important;
        }
        
        /* Button Group Styling */
        .button-group {
            display: flex;
            gap: 8px;
            flex-wrap: wrap;
            margin-bottom: 12px;
            padding-bottom: 8px;
            border-bottom: 1px solid #dee2e6;
        }
        
        .action-link {
            text-decoration: none;
            font-size: 0.9rem;
            display: inline-flex;
            align-items: center;
            gap: 4px;
            padding: 4px 8px;
            border-radius: 3px;
        }
        
        .action-link:hover {
            background-color: #f0f0f0;
            text-decoration: none;
        }
        
        .text-primary {
            color: #007bff;
        }
        
        .text-danger {
            color: #dc3545;
        }
        
        .separator {
            color: #ccc;
            margin: 0 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
     <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
     <ContentTemplate>

    <div class="container-fluid mt-2">
        <div id="accordion">

            <!-- ================= SETTINGS ================= -->
            <div class="card mb-2 shadow-sm">
                <div class="card-header p-2 bg-light">
                    <a class="collapsed d-flex justify-content-between align-items-center"
                       data-toggle="collapse" href="#collapseSettings">
                        <strong>Settings</strong>
                        <i class="fa fa-chevron-down rotate-icon"></i>
                    </a>
                </div>

                <div id="collapseSettings" class="collapse">
                    <div class="card-body">
                        <div class="row">

                            <div class="col-md-4">
                                <h6 class="font-weight-bold">Parameters</h6>

                                <div class="form-group" style="width:250px;">
                                    <label>Quantity</label>
                                    <asp:DropDownList ID="ddlQuantity" runat="server"
                                        CssClass="form-select" Width="100%" AutoPostBack="true" />
                                </div>

                                <div class="form-group mt-2">
                                    <label>Posting</label><br />
                                    <label class="toggle-switch">
                                        <input type="checkbox" checked />
                                        <span class="slider"></span>
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <h6 class="font-weight-bold">Print Options</h6>
                                <div class="form-group" style="width:250px;">
                                    <label>Print</label>
                                    <asp:DropDownList ID="ddlPrint" runat="server"
                                        CssClass="form-select" Width="100%" AutoPostBack="true" />
                                </div>
                            </div>

                            <div class="col-md-4">
                                <h6 class="font-weight-bold">Setup</h6>
                                <div class="form-group" style="width:250px;">
                                    <label>Check Credit Limit</label>
                                    <asp:DropDownList ID="ddlCheckCredit" runat="server"
                                        CssClass="form-select" Width="100%" AutoPostBack="true" />
                                </div>

                                <h6 class="font-weight-bold mt-3">Summary Purchase</h6>
                                <div class="form-group" style="width:250px;">
                                    <label>Summary Update For</label>
                                    <asp:DropDownList ID="ddlSummary" runat="server"
                                        CssClass="form-select" Width="100%" AutoPostBack="true" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <!-- ================= OVERVIEW ================= -->
            <div class="card mb-2 shadow-sm">
    <div class="card-header p-2 bg-light">
        <a class="d-flex justify-content-between align-items-center"
           data-toggle="collapse" href="#collapseOverview">
            <strong>Overview</strong>
            <i class="fa fa-chevron-down rotate-icon"></i>
        </a>
    </div>

    <div id="collapseOverview" class="collapse show">
        <div class="card-body">
            <!-- Overview Button Group -->
            <div class="button-group">
                <asp:LinkButton ID="btnAdd" runat="server" CssClass="action-link text-primary" >
                    <i class="fa fa-plus"></i> Add
                </asp:LinkButton>
                
                <asp:LinkButton ID="btnRemove" runat="server" CssClass="action-link text-danger" O>
                    <i class="fa fa-trash"></i> Remove
                </asp:LinkButton>
                
                <asp:LinkButton ID="btnTotals" runat="server" CssClass="action-link text-primary" OnClick="btnTotals_Click">
                    <i class="fa fa-calculator"></i> Totals
                </asp:LinkButton>
                
                <asp:LinkButton ID="btnSalesTax" runat="server" CssClass="action-link text-primary" OnClick="btnSalesTax_Click">
                    <i class="fa fa-percent"></i> Sales Tax
                </asp:LinkButton>
            </div>

            <asp:GridView ID="gvOverview" runat="server"
                AutoGenerateColumns="False" OnRowDataBound="gvOverview_RowDataBound"
                CssClass="table table-bordered table-striped table-sm"
                ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="Select">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkOverviewSelect" runat="server" CssClass="round-checkbox" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Update">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUpdate" runat="server"
                                CssClass="form-control form-control-sm"
                                Text='<%# Bind("displayOrdering") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Purchase Order">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPurchaseOrder" runat="server"
                                CssClass="form-control form-control-sm"
                                Text='<%# Bind("PurchId") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtName" runat="server"
                                CssClass="form-control form-control-sm"
                                Text='<%# Bind("PurchName") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Product Receipts">
                        <ItemTemplate>
                            <asp:TextBox ID="txtProductReceipts" runat="server"
                                CssClass="form-control form-control-sm"
                                Text='<%# Bind("Num") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Product Receipt Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtProductReceiptDate" runat="server"
                                CssClass="form-control form-control-sm text-center"
                                Text='<%# Bind("TransDate", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Document Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDocumentDate" runat="server"
                                CssClass="form-control form-control-sm text-center"
                               Textmode="Date"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Terms of Payment">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlTermsOfPayment" runat="server"
                                CssClass="form-control form-control-sm"
                                ></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

           <!-- ================= LINES ================= -->
<div class="card mb-2 shadow-sm">
    <div class="card-header p-2 bg-light">
        <a class="collapsed d-flex justify-content-between align-items-center"
           data-toggle="collapse" href="#collapseLines">
            <strong>Lines</strong>
            <i class="fa fa-chevron-down rotate-icon"></i>
        </a>
    </div>

    <div id="collapseLines" class="collapse">
        <div class="card-body">
            <!-- Lines Button Group -->
            <div class="button-group">
                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="action-link text-danger">
                    <i class="mdi mdi-delete"></i> Delete
                </asp:LinkButton>
                
                <span class="separator">|</span>
                
                <asp:LinkButton ID="btnDimensions" runat="server" CssClass="action-link text-primary">
                    <i class="fa fa-cubes"></i> Dimensions
                </asp:LinkButton>
                
              <%--  <asp:LinkButton ID="btnOnHand" runat="server" OnClick="btnOnHand_Click" CssClass="action-link text-primary">
                    <i class="fa fa-archive"></i> On-hand
                </asp:LinkButton>--%>
                
                <asp:LinkButton ID="btnTransactions" runat="server"  CssClass="action-link text-primary">
                    <i class="fa fa-exchange"></i> Transactions
                </asp:LinkButton>
            </div>

            <div class="table-responsive">
                <asp:GridView ID="gvLines" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-striped table-sm"
                    ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="40px" HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true" runat="server" CssClass="round-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="hfRecId" runat="server" Text='<%# Bind("RecId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Purchase Order">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("PurchaseOrderId") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Line Number">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLineNumber" runat="server"
                                    CssClass="form-control form-control-sm text-center"
                                    Text='<%# Bind("LineNumber") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Item Number">
                            <ItemTemplate>
                                <asp:TextBox ID="txtItemNumber" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("ItemId") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Procurement Category">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProcurementCategory" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("ProcurementCategory") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Text">
                            <ItemTemplate>
                                <asp:TextBox ID="txtText" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("ItemName") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Site">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSite" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("InventSiteId") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Warehouse">
                            <ItemTemplate>
                                <asp:TextBox ID="txtWarehouse" runat="server"
                                    CssClass="form-control form-control-sm"
                                    Text='<%# Bind("InventLocationId") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Quantity Ordered">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantityOrdered" runat="server"
                                    CssClass="form-control form-control-sm text-right"
                                    Text='<%# Bind("PurchQty", "{0:N2}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantity" runat="server"
                                    CssClass="form-control form-control-sm text-right"
                                    Text='<%# Bind("PurchQty", "{0:N2}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unit Price">
                            <ItemTemplate>
                                <asp:TextBox ID="txtUnitPrice" runat="server"
                                    CssClass="form-control form-control-sm text-right"
                                    Text='<%# Bind("PurchPrice") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Line Net Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLineNetAmount" runat="server"
                                    CssClass="form-control form-control-sm text-right"
                                    Text='<%# Bind("LineAmount", "{0:N2}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>

            <!-- ================= FOOTER BUTTONS ================= -->
      

                         <div class="action-footer">
      <asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click">OK</asp:LinkButton>
      <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    
  </div>
        </div>
    </div>
         </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>