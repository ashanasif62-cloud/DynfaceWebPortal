<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder_Vouchers.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_Vouchers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* ===================== Tab Header Styling ===================== */
        .nav-tabs {
            margin-bottom: 0;
            border-bottom: 2px solid #ddd;
        }
        .nav-tabs li a {
            padding: 10px 15px;
            font-size: 14px;
            border: none;
            border-bottom: 2px solid transparent;
            border-radius: 0;
            color: #555;
            cursor: pointer;
        }
        .nav-tabs li a:hover {
            border-bottom: 2px solid #007bff;
            color: #007bff;
        }
        .nav-tabs li.active a {
            border-bottom: 2px solid #007bff;
            color: #007bff;
        }

        /* ===================== Tab Content Styling ===================== */
        .tab-content {
            padding-top: 10px;
        }
        .tab-pane {
            padding: 0;
            background-color: transparent;
            box-shadow: none;
            border: none;
        }

        /* ===================== Grid Styling ===================== */
        .grid-container {
            padding: 0;
            background: transparent;
            box-shadow: none;
            border-radius: 0;
        }
        .table th {
            white-space: nowrap;
            font-size: 13px;
            background-color: #f5f5f5;
        }
        .table td {
            font-size: 13px;
            vertical-align: middle;
        }
        .text-right {
            text-align: right;
        }
        .tab-pane h4,
        .tab-pane p {
            margin-bottom: 15px;
        }
        #gvVouchers {
            margin-top: 10px;
        }

        /* ===================== Form Grid Styling ===================== */
        html, body {
            margin: 0;
            padding: 0;
            width: 100%;
        }
        .form-grid {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
           gap: 10px;
            width: 100%;
            margin: 0;
            padding: 15px;
            box-sizing: border-box;
        }
        .form-section {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0;
            margin: 0;
        }
        .form-section > label:first-child {
            font-weight: bold;
            font-size: 14px;
            margin-bottom: 10px;
            text-align: left;
        }
        .form-group {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            margin-bottom: 8px;
            width: 100%;
        }
        .form-group > label:first-child {
            width: 100%;
            text-align: left;
            font-size: 12px;
            margin-bottom: 3px;
        }
        .form-control-sm {
            padding: 4px 6px;
            font-size: 13px;
            border: none;
            border-bottom: 1px solid #000;
            outline: none;
            background-color: transparent;
            height: 28px;
            box-sizing: border-box;
            text-align: left;
            width: 200px;
            min-width: 200px;
        }
        .form-control-sm:focus {
            border-bottom: 2px solid #007bff;
        }
        .toggle-group {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            margin-bottom: 8px;
            width: 100%;
        }
        .toggle-container {
            display: flex;
            align-items: center;
            gap: 8px;
            width: 250px;
            margin-top: 3px;
        }
        .switch {
            position: relative;
            display: inline-block;
            width: 35px;
            height: 18px;
            margin: 0;
        }
        .switch input {
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
            transition: 0.4s;
            border-radius: 20px;
        }
        .slider:before {
            position: absolute;
            content: "";
            height: 14px;
            width: 14px;
            left: 2px;
            bottom: 2px;
            background-color: white;
            transition: 0.4s;
            border-radius: 50%;
        }
        input:checked + .slider {
            background-color: #007bff;
        }
        input:checked + .slider:before {
            transform: translateX(17px);
        }
        .toggle-label {
            font-size: 13px;
            color: #333;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    <!-- Optional action buttons -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <!-- ===================== TABS ===================== -->
    <ul class="nav nav-tabs" role="tablist">
        <li class="active"><a href="#tabOverview" role="tab">Overview</a></li>
        <li><a href="#tabGeneral" role="tab">General</a></li>
    </ul>

    <!-- ===================== TAB CONTENT ===================== -->
    <div class="tab-content">

        <!-- ===================== OVERVIEW TAB ===================== -->
        <div class="tab-pane fade show active" id="tabOverview">
            <div class="grid-container">
                <asp:GridView ID="gvVouchers"
                    runat="server"
                    CssClass="table table-bordered table-striped table-hover"
                    AutoGenerateColumns="false"
                    AllowPaging="true"
                    ShowHeaderWhenEmpty="true"
                    PageSize="20">
                 <Columns>
                        <asp:TemplateField HeaderText="Journal number">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("JournalNumber") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Voucher">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("Voucher") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("Date", "{0:yyyy-MM-dd}") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Year closed">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("YearClosed") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ledger account">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("LedgerAccount") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account name">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("AccountName") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("Description") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("Currency") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount in transaction currency">
                            <ItemTemplate><asp:Label runat="server" CssClass="text-right" Text='<%# Eval("AmountTransCurrency") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate><asp:Label runat="server" CssClass="text-right" Text='<%# Eval("Amount") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Posting type">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("PostingType") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Posting layer">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("PostingLayer") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor account">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("VendorAccount") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor name">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("VendorName") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer account">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("CustomerAccount") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer name">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("CustomerName") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer groups">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("CustomerGroups") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment reference">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("PaymentReference") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branches">
                            <ItemTemplate><asp:Label runat="server" Text='<%# Eval("Branches") %>' /></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- ===================== GENERAL TAB ===================== -->
        <div class="tab-pane fade" id="tabGeneral">
            <div class="grid-container general-form">
                <div class="form-grid">
                    <!-- COLUMN 1: ORIGIN -->
                    <div class="form-section">
                        <label>ORIGIN</label>
                        <div class="form-group">
                            <label>Document</label>
                            <asp:TextBox ID="txtDocument" runat="server" CssClass="form-control form-control-sm fixed-width"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Document date</label>
                            <asp:TextBox ID="txtDocumentDate" runat="server" CssClass="form-control form-control-sm fixed-width" TextMode="Date" ></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Journal batch number</label>
                            <asp:TextBox ID="txtJournalBatch" runat="server" CssClass="form-control form-control-sm fixed-width"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Level</label>
                            <asp:TextBox ID="txtLevel" runat="server" CssClass="form-control form-control-sm fixed-width" ></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Created by</label>
                            <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="form-control form-control-sm fixed-width" ></asp:TextBox>
                        </div>
                    </div>
                  
                    <!-- COLUMN 2: AMOUNT + PERIOD -->
                    <div class="form-section">
                        <label>AMOUNT</label>
                        <div class="form-group">
                            <label>Exchange rate</label>
                            <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="form-control form-control-sm fixed-width text-right" ></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Quantity</label>
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control form-control-sm fixed-width text-right"></asp:TextBox>
                        </div>

                        <!-- PERIOD -->
                        <label style="margin-top:15px; font-weight: bold; " >PERIOD</label>
                        <div class="form-group">
                            <label>Type</label>
                            <asp:TextBox ID="txtType" runat="server" CssClass="form-control form-control-sm fixed-width" ></asp:TextBox>
                        </div>
                    </div>

                    <!-- COLUMN 3: TYPE -->
                    <div class="form-section">
                        <label>TYPE</label>
                        <div class="form-group">
                            <label>Transaction type</label>
                            <asp:TextBox ID="txtTransactionType" runat="server" CssClass="form-control form-control-sm fixed-width"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Posting type</label>
                            <asp:TextBox ID="txtPostingType" runat="server" CssClass="form-control form-control-sm fixed-width" ></asp:TextBox>
                        </div>

                        <div class="form-group toggle-group">
                            <label>Crediting</label>
                            <div class="toggle-container">
                                <label class="switch">
                                    <asp:CheckBox ID="chkCrediting" runat="server"  />
                                    <span class="slider"></span>
                                </label>
                              
                            </div>
                        </div>

                        <div class="form-group toggle-group">
                            <label>Correction</label>
                            <div class="toggle-container">
                                <label class="switch">
                                    <asp:CheckBox ID="chkCorrection" runat="server" />
                                    <span class="slider"></span>
                                </label>
                                
                            </div>
                        </div>
                    </div>

                    <!-- COLUMN 4: REASON -->
                    <div class="form-section">
                        <label>REASON</label>
                        <div class="form-group">
                            <label>Reason code</label>
                            <asp:TextBox ID="txtReasonCode" runat="server" CssClass="form-control form-control-sm fixed-width"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Reason comment</label>
                            <asp:TextBox ID="txtReasonComment" runat="server" CssClass="form-control form-control-sm fixed-width"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <!-- ===================== TAB SCRIPT ===================== -->
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            var tabs = document.querySelectorAll(".nav-tabs li a");
            var tabPanes = document.querySelectorAll(".tab-pane");

            // Activate Overview tab on load
            tabs.forEach(function (tabLink) {
                if (tabLink.getAttribute("href") === "#tabOverview") {
                    tabLink.parentElement.classList.add("active");
                }
            });
            tabPanes.forEach(function (pane) {
                if (pane.id === "tabOverview") {
                    pane.classList.add("show", "active");
                } else {
                    pane.classList.remove("show", "active");
                }
            });

            // Tab click functionality
            tabs.forEach(function (tabLink) {
                tabLink.addEventListener("click", function (e) {
                    e.preventDefault();
                    // Remove active from all li
                    tabs.forEach(function (t) { t.parentElement.classList.remove("active"); });
                    // Remove show active from all tab panes
                    tabPanes.forEach(function (pane) { pane.classList.remove("show", "active"); });
                    // Activate clicked tab
                    this.parentElement.classList.add("active");
                    // Show corresponding tab-pane
                    var target = this.getAttribute("href");
                    document.querySelector(target).classList.add("show", "active");
                });
            });
        });
    </script>

</asp:Content>