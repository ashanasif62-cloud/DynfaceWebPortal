<%@ Page Title="Total" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder_Total.aspx.cs" Inherits="DynamicsPortal.PurchaseOrder_Total" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <style>
        .textbox-style {
            color: gray !important;                /* ForeColor */
            background-color: #F3F2F1 !important;  /* BackColor */
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
        <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
<ContentTemplate>

    <%--CalculationPanel--%>
  <a href="#CalculationPanel" class="d365-toggle-header d-flex justify-content-between align-items-center" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="CalculationPanel">
     <span class="section-title">Calculation basis</span>
     <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
 </a>

    <div class="collapse show mt-3" id="CalculationPanel">

        <div class="card-body">

            <div class="PR-custom-section-title mb-3"></div>
    
         <div class="form-group" style="width: 250px;">
    <label for="ddlSelection">Selection</label>
    <asp:DropDownList 
        ID="ddlSelection" 
        runat="server" 
        CssClass="form-select" 
        Width="100%" 
        AutoPostBack="true" 
        OnSelectedIndexChanged="ddlSelection_SelectedIndexChanged">
    </asp:DropDownList>
</div>

</div>


    </div>


       <%--PurchaseOrderPanel--%>
 <a href="#PurchaseOrderPanel" class="d365-toggle-header d-flex justify-content-between align-items-center" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="PurchaseOrderPanel">
    <span class="section-title">Purchase order totals</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</a>

   <div class="collapse show mt-3" id="PurchaseOrderPanel">

       <div class="card-body">

           <div class="PR-custom-section-title mb-3"></div>

           <%--///--%>
            <!-- Two-section layout -->
 <div class="d-flex flex-wrap justify-content-between">

     <!-- LEFT SECTION -->
     <div style="flex: 0 0 48%;">

         <div class="mb-2">
             <strong class="d-block mb-2">TOTALS</strong>
         </div>
         <!-- Currency -->
         <div class="form-group mb-3" style="width: 250px;">
        <label>Currency</label>
        <asp:Label ID="Label1" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: left;" />
        </div>
         <!-- Exchange rate -->
         <div class="form-group mb-3" style="width: 250px;">
             <label>Exchange rate</label>
              <asp:Label ID="lblExchangeRate" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
         </div>
          <!-- Line discount -->
         <div class="form-group mb-3" style="width: 250px;">
             <label>Line discount</label>
             <asp:Label ID="lblLineDiscount" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
         </div>
         <!-- Subtotal amount -->
         <div class="form-group mb-3" style="width: 250px;">
             <label>Subtotal amount</label>
              <asp:Label ID="lblSubtotalAmount" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
         </div>
         <!-- Total discount -->
         <div class="form-group mb-3" style="width: 250px;">
             <label>Total discount</label>
             <asp:Label ID="lblTotalDiscount" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
         </div>
         <!-- Charges -->
         <div class="form-group mb-3" style="width: 250px;">
             <label>Charges</label>
              <asp:Label ID="lblCharges" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
         </div>
         <!-- Sales tax -->
        <div class="form-group mb-3" style="width: 250px;">
            <label>Sales tax</label>
             <asp:Label ID="lblSalesTax" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
        </div>
        <!-- Round-off -->
        <div class="form-group mb-3" style="width: 250px;">
            <label>Round-off</label>
            <asp:Label ID="lblRoundOff" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
        </div>
        <!-- Total amount -->
        <div class="form-group mb-3" style="width: 250px;">
            <label>Total amount</label>
            <asp:Label ID="lblTotalAmount" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
        </div>
        <!-- Cash discount -->
    <div class="form-group mb-3" style="width: 250px;">
        <label>Cash discount</label>
            <asp:Label ID="lblCashDiscount" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
        </div>


     </div>

     <!-- Right Section -->
        <div style="flex: 0 0 48%;">

             <div class="mb-2">
             <strong class="d-block mb-2">VENDOR</strong>
             </div>
            <!-- Credit limit -->
            <div class="form-group mb-3" style="width: 250px;">
                <label>Credit limit</label>
                <asp:Label ID="lblCreditLimit" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
            </div>
            <!-- Credit available -->
            <div class="form-group mb-3" style="width: 250px;">
                <label>Credit available</label>
                <asp:Label ID="lblCreditAvailable" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
            </div>

            <div class="mb-2">
            <strong class="d-block mb-2">PREPAYMENT</strong>
            </div>
            <!-- Limit -->
            <div class="form-group mb-3" style="width: 250px;">
                <label>Limit</label>
                <asp:Label ID="lblLimit" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
            </div>
            <!-- Remaining -->
            <div class="form-group mb-3" style="width: 250px;">
                <label>Remaining:</label>
                <asp:Label ID="lblRemaining" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
            </div>

            <div class="mb-2">
            <strong class="d-block mb-2">MEASUREMENTS</strong>
            </div>
             <!-- Quantity -->
            <div class="form-group mb-3" style="width: 250px;">
                <label>Quantity</label>
                <asp:Label ID="lblQuantity" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
            </div>
            <!-- Weight -->
            <div class="form-group mb-3" style="width: 250px;">
                <label>Weight</label>
                <asp:Label ID="lblWeight" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
            </div>
             <!-- CW quantity -->
            <div class="form-group mb-3" style="width: 250px;">
                <label>CW quantity</label>
                <asp:Label ID="lblCWQuantity" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
            </div>
            <!-- Volume -->
            <div class="form-group mb-3" style="width: 250px;">
                <label>Volume</label>
                <asp:Label ID="lblVolume" runat="server" CssClass="form-control autocomplete-input textbox-style" Style="width: 100px; text-align: right;"  />
            </div>

        </div>

     </div>


           <%--///--%>

       </div>

   </div>

    <!-- Buttons -->
<div class="action-footer">
    <asp:LinkButton ID="btnOKl" runat="server" OnClientClick="javascript: return closeDialog();">OK</asp:LinkButton>
</div>
    
            </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
