<%@ Page Title="On-hand Inventory" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="PO_OnHand.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PO_OnHand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Base styles matching the enterprise application look */
        .tab-links a {
            margin-right: 15px;
            text-decoration: none;
            color: #0078D7;
            font-weight: 500;
            font-size: 0.9em;
        }
        .tab-links a:hover {
            text-decoration: underline;
        }
        .section-title {
            font-weight: bold;
            font-size: 1em;
            margin-top: 15px;
            margin-bottom: 5px;
            padding-left: 5px;
            color: #333;
        }

        /* Generic form field container */
        .form-group-custom {
            margin-bottom: 10px;
        }
        
        /* Styles for the left column TextBoxes */
        .left-panel-label {
            font-size: 0.85em;
            margin-bottom: 2px;
            display: block;
            color: #555;
            padding-left:5px;
        }
        .left-panel-textbox input[type="text"], .left-panel-textbox select {
            /* Input fields like Site, Unit, CW Unit use full border */
            border: 1px solid #ddd;
            padding: 2px 5px;
            margin-left: 5px;
            padding-left: 5px;
            height: 24px;
            /* Resized width */
            width: 100%;
            max-width: 200px; 
            box-sizing: border-box;
            font-size: 0.9em;
            background-color: white; 
        }
        /* Style for read-only fields on the left (make background white/transparent as per image) */
        .left-panel-textbox input[readonly] {
            background-color: white;
            border: 1px solid #ddd;
        }
        /* Overrides for specific fields that look like underlines in the image */
        #txtProductName, #txtFinancialCostAmount, #txtSite {
            border-top: none;
            border-left: none;
            border-right: none;
            padding-left: 0;
            background-color: transparent;
            border-bottom: 1px solid #000;
        }
        
        /* Directly target the txtCostPrice ID to set its short width */
        #txtCostPrice {
            width: 50px !important;
            border-top: none;
            border-left: none;
            border-right: none;
            padding-left: 0;
            background-color: transparent;
            border-bottom: 1px solid #000;
        }

        /* Styles for the right column quantity fields (mimicking the horizontal dividers) */
        .qty-row {
            display: flex;
            align-items: flex-end;
            padding: 3px 0;
        }
        .qty-label-col {
            flex-basis: 30%;
            font-size: 0.9em;
            padding-right: 10px;
            color: #333;
        }
        
        /* Ensures the column width is the same for headers and data, allowing alignment */
        .qty-value-col {
            flex-basis: 35%;
            padding: 0 5px;
        }
        
        /* Style for the header rows */
        .qty-header-row {
            display: flex;
            font-weight: bold;
          /*  border-bottom: 2px solid #ccc; */
            padding: 5px 0;
            margin-bottom: 5px; 
        }

        /* CRITICAL FIX: Centers the header text over the input area */
        .qty-header-row .qty-value-col {
            text-align: center; 
        }

        /* Styles to achieve the single underline effect for Quantity TextBoxes */
        .qty-value-col input[type="text"] {
            border: none; 
            border-bottom: 1px solid #000; /* Only bottom border (underline style) */
            padding: 0 2px;
            height: 20px;
            /* Resized width to match header expectations */
            width: 100%;
            max-width: 100px; 
            font-size: 0.9em;
            /* Aligns numeric data to the right for proper visual stacking */
            text-align: right; 
            background-color: transparent; 
            box-sizing: content-box;
        }
        
        /* Style for the 'ORDERED IN TOTAL' and 'VARIOUS' separators */
        .section-separator {
            font-weight: bold;
            font-size: 1em;
            padding: 10px 0 5px 0;
        }
        .container-fluid {
            padding-bottom: 20px; 
        }
        .btn-primary {
            /* Close button style matching the image */
            background-color: #0078D7;
            border: none;
            padding: 5px 15px;
            font-size: 0.9em;
            border-radius: 2px;
            color: white;
        }
        .text-end {
            text-align: right !important; 
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">

    <!-- Top Hyperlink Tabs -->
    <div class="tab-links mb-3">
        <a href="#">Overview</a>
        <a href="#">Ordered items</a>
        <a href="#">Items on order</a>
        <a href="#">Net requirements</a>
        <a href="#">Intercompany on-hand</a>
        <a href="#">Quantity adjustment</a>
        <a href="#">Counting history</a>
        <a href="#">Display dimensions</a>
    </div>

    <div class="container-fluid">
        <div class="row">
            <!-- Left Side Panel (Product Info, Dimensions, Value) -->
            <div class="col-md-4">
                
                <div class="section-title">Product name</div>
                <div class="form-group-custom left-panel-textbox">
                    <asp:TextBox ID="txtProductName" runat="server" ReadOnly="true" Text=""></asp:TextBox>
                </div>

                <div class="section-title">INVENTORY DIMENSIONS</div>
                <div class="left-panel-label">Site</div>
                <div class="form-group-custom left-panel-textbox">
                    <asp:TextBox ID="txtSite" runat="server" ReadOnly="true" Text=""></asp:TextBox>
                </div>

                <div class="section-title">UNIT</div>
                <div class="left-panel-label">Show</div>
                <div class="form-group-custom left-panel-textbox">
                    <!-- DropDownList remains as per the image -->
                    <asp:DropDownList ID="ddlShow" runat="server" CssClass="form-control" Style="border: 1px solid #ddd; background-color: white; padding: 0 5px; height: 24px; max-width: 200px;">
                       <%-- <asp:ListItem Text="Inventory unit" Value="InventoryUnit" Selected="True" />
                        <asp:ListItem Text="CW unit" Value="CWUnit" />--%>
                    </asp:DropDownList>
                </div>
                <div class="left-panel-label">Unit</div>
                <div class="form-group-custom left-panel-textbox">
                    <asp:TextBox ID="txtUnit" runat="server" Text=""></asp:TextBox>
                </div>
                <div class="left-panel-label">CW unit</div>
                <div class="form-group-custom left-panel-textbox">
                    <asp:TextBox ID="txtCWUnit" runat="server" Text=""></asp:TextBox>
                </div>

                <div class="section-title">INVENTORY VALUE</div>
                <div class="left-panel-label">Physical cost amount</div>
                <div class="form-group-custom left-panel-textbox">
                    <asp:TextBox ID="txtPhysicalCostAmount" runat="server" ReadOnly="true" Text=""></asp:TextBox>
                </div>
                <div class="left-panel-label">Financial cost amount</div>
                <div class="form-group-custom left-panel-textbox">
                    <asp:TextBox ID="txtFinancialCostAmount" runat="server" ReadOnly="true" Text=""></asp:TextBox>
                </div>
                <div class="left-panel-label">Cost price</div>
                <div class="form-group-custom left-panel-textbox">
                    <!-- The short width for txtCostPrice is handled in the CSS block -->
                    <asp:TextBox ID="txtCostPrice" runat="server" ReadOnly="true" Text=""></asp:TextBox>
                </div>
            </div>

            <!-- Right Side Panel (Quantity Data with Underlines) -->
            <div class="col-md-8">
                <div class="section-title">ON-HAND</div>
                
                <!-- Header Row (ON-HAND) -->
                <div class="qty-header-row">
                    <div class="qty-label-col"></div>
                    <div class="qty-value-col"style="text-align:left">QUANTITY</div>
                    <div class="qty-value-col" style="text-align:left">CW QUANTITY</div>
                </div>

                <!-- ON-HAND Rows -->
                <div class="qty-row">
                    <div class="qty-label-col">PHYSICAL INVENTORY</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtPhyInv" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtPhyInvCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">PHYSICAL RESERVED</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtPhyRes" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtPhyResCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">AVAILABLE PHYSICAL</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtAvailPhy" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtAvailPhyCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">AVAILABLE FOR RESERVATION</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtAvailRes" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtAvailResCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">ORDERED IN TOTAL</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtOrdTotal" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtOrdTotalCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">ORDERED RESERVED</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtOrdRes" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtOrdResCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">ON ORDER IN TOTAL</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtOnOrdTotal" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtOnOrdTotalCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">TOTAL AVAILABLE</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtTotalAvail" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtTotalAvailCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>


                <div class="section-title" style="margin-top: 25px;">PHYSICAL INVENTORY</div>
                
                <!-- Header Row (Physical Inventory) -->
                <div class="qty-header-row">
                    <div class="qty-label-col"></div>
                    <div class="qty-value-col" style="text-align:left">QUANTITY</div>
                    <div class="qty-value-col" style="text-align:left">CW QUANTITY</div>
                </div>

                <!-- PHYSICAL INVENTORY Rows -->
                <div class="qty-row">
                    <div class="qty-label-col">POSTED QUANTITY</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtPostQty" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtPostQtyCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">DEDUCTED</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtDeducted" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtDeductedCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">PICKED</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtPicked" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtPickedCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">RECEIVED</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtReceived" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtReceivedCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">REGISTERED</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtRegistered" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtRegisteredCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                
                <!-- ORDERED IN TOTAL Separator -->
                <div class="section-separator">ORDERED IN TOTAL</div>
                
                <!-- Header Row (Ordered In Total) -->
                <div class="qty-header-row">
                    <div class="qty-label-col"></div>
                    <div class="qty-value-col" style="text-align:left">QUANTITY</div>
                    <div class="qty-value-col" style="text-align:left">CW QUANTITY</div>
                </div>

                <!-- ORDERED IN TOTAL Rows -->
                <div class="qty-row">
                    <div class="qty-label-col">ARRIVED</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtArrived" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtArrivedCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">ORDERED</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtOrdered" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtOrderedCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                
                <!-- VARIOUS Separator -->
                <div class="section-separator">VARIOUS</div>

                <!-- Header Row (Various) -->
                <div class="qty-header-row">
                    <div class="qty-label-col"></div>
                    <div class="qty-value-col" style="text-align:left">QUANTITY</div>
                    <div class="qty-value-col" style="text-align:left">CW QUANTITY</div>
                </div>

                <!-- VARIOUS Rows -->
                <div class="qty-row">
                    <div class="qty-label-col">ON ORDER</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtVariousOnOrder" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtVariousOnOrderCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row">
                    <div class="qty-label-col">QUOTATION RECEIPT</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtQuotationReceipt" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtQuotationReceiptCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
                <div class="qty-row" style="padding-bottom: 20px;">
                    <div class="qty-label-col">QUOTATION ISSUE</div>
                    <div class="qty-value-col"><asp:TextBox ID="txtQuotationIssue" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                    <div class="qty-value-col"><asp:TextBox ID="txtQuotationIssueCW" runat="server" ReadOnly="true" Text=""></asp:TextBox></div>
                </div>
            </div>
        </div>
        
        <!-- Close Button (Right Aligned, just like in the image) -->
        <div class="text-end mt-4">
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnClose_Click" />
        </div>
    </div>

</asp:Content>
