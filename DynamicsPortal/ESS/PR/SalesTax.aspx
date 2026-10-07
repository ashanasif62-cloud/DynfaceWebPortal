<%@ Page Title="Sales Tax" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="SalesTax.aspx.cs" Inherits="DynamicsPortal.ESS.PR.SalesTax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
.toggle-switch {
    position: relative;
    display: inline-block;
    width: 50px;
    height: 24px;
}

.toggle-switch input[type="checkbox"] {
    opacity: 0;
    width: 0;
    height: 0;
}

.slider {
    position: absolute;
    cursor: pointer;
    top: 0; left: 0;
    right: 0; bottom: 0;
    background-color: #ccc;
    transition: 0.4s;
    border-radius: 24px;
}

.slider:before {
    position: absolute;
    content: "";
    height: 18px;
    width: 18px;
    left: 3px;
    bottom: 3px;
    background-color: white;
    transition: 0.4s;
    border-radius: 50%;
}

.toggle-switch input:checked + .slider {
    background-color: #007bff; /* Bootstrap primary color */
}

.toggle-switch input:checked + .slider:before {
    transform: translateX(26px);
}
  .form-control[readonly],
  .form-control:read-only {
      background-color: #e9ecef !important; /* light grey */
      color: #495057 !important;            /* readable dark text */
      opacity: 1 !important;                /* ensure visible */
      border-color: #ced4da;                /* subtle border */
  }
</style>
  


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">

 
    <ul class="nav nav-tabs" id="salesTaxTabs" role="tablist">
        <li class="nav-item">
          
            <a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overview" role="tab"
               aria-controls="overview" aria-selected="true">Overview</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="general-tab" data-toggle="tab" href="#general" role="tab"
               aria-controls="general" aria-selected="false">General</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="amount-tab" data-toggle="tab" href="#amount" role="tab"
               aria-controls="amount" aria-selected="false">Amount</a>
        </li>
    </ul>

    <div class="tab-content border border-top-0 p-3" id="salesTaxTabsContent">

        <div class="tab-pane fade show active" id="overview" role="tabpanel" aria-labelledby="overview-tab">
            <div class="table-responsive">
                <asp:GridView ID="gvSalesTaxOverview" runat="server" 
                    CssClass="table table-bordered table-striped table-sm align-middle"
                    AutoGenerateColumns="False" 
                    GridLines="None" 
                    AllowPaging="false" 
                    PageSize="10" 
                    ShowHeaderWhenEmpty="true">
<%--                    <Columns>  
                           <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
  
    <ItemTemplate>
        <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true" runat="server" CssClass="round-checkbox"  />
    </ItemTemplate>
</asp:TemplateField>
                        <asp:BoundField DataField="TaxCode" HeaderText="Sales tax code" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:N2}" HtmlEncode="false" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="AmountOrigin" HeaderText="Amount origin" DataFormatString="{0:N2}" HtmlEncode="false" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="Percent" HeaderText="Percent"  DataFormatString="{0:F5}"  HtmlEncode="false" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="ActualSalesTaxAmount" HeaderText="Actual sales tax amount" DataFormatString="{0:N2}" HtmlEncode="false" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="SalesTaxDirection" HeaderText="Sales tax direction" />
                    </Columns>--%>
                    <Columns>
  
    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
        <ItemTemplate>
            <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged"
                AutoPostBack="true" runat="server" CssClass="round-checkbox" />
        </ItemTemplate>
    </asp:TemplateField>

 
    <asp:BoundField DataField="TaxCode" HeaderText="Sales tax code" />

 
    <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("Quantity") != DBNull.Value ? string.Format("{0:#,##0.00}", Eval("Quantity")) : "0.00" %>
        </ItemTemplate>
    </asp:TemplateField>

 
 <asp:TemplateField HeaderText="Amount origin" ItemStyle-HorizontalAlign="Right">
    <ItemTemplate>
        <%# Eval("AmountOrigin") %>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Percent" ItemStyle-HorizontalAlign="Right">
    <ItemTemplate>
        <%# Eval("Percent") %>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Actual sales tax amount" ItemStyle-HorizontalAlign="Right">
    <ItemTemplate>
        <%# Eval("ActualSalesTaxAmount") %>
    </ItemTemplate>
</asp:TemplateField>


 
    <asp:BoundField DataField="SalesTaxDirection" HeaderText="Sales tax direction" />
</Columns>

                    <HeaderStyle BackColor="#f8f9fa" ForeColor="#495057" Font-Bold="true" />
                    <RowStyle BackColor="#ffffff" />
                    <AlternatingRowStyle BackColor="#f8f9fa" />
                   
                </asp:GridView>
            </div>
        </div>

   <div class="tab-pane fade" id="general" role="tabpanel" aria-labelledby="general-tab">
           
            <div class="row g-3 form-section">
                <div class="col-md-3">
                    <label class="form-label">Sales tax code</label>
                 
                 <asp:TextBox ID="txtGeneralSalesTaxCode" runat="server" CssClass="form-control rounded w-75" ReadOnly="true" />

                </div>
                <div class="col-md-3">
                    <label class="form-label">Description</label>
                 
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control rounded w-75" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Exempt code</label>
               
                    <asp:TextBox ID="txtExemptCode" runat="server" CssClass="form-control rounded w-75" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Legal entity for intercompany tax posting</label>
                 
                    <asp:TextBox ID="txtLegalEntity" runat="server" CssClass="form-control rounded w-75" Enabled="false" />
                </div>
            </div>
        </div>

      <div class="tab-pane fade" id="amount" role="tabpanel" aria-labelledby="amount-tab">
            <div class="row g-3">
                <!-- TRANSACTION CURRENCY SECTION (Left Side - Takes up 7/12ths of the page) -->
                <div class="col-md-7">
                    <h5 class="mb-3">TRANSACTION CURRENCY</h5>
                    <div class="row g-3 form-section">
                        
                        <!-- Column for Currency and Adjusted Amount Origin -->
                        <div class="col-md-3">
                            <label class="form-label">Currency</label>
                            <!-- Currency is usually read-only and narrow -->
                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="form-control rounded w-70" ReadOnly="true" />
                          <label class="form-label mt-3">Adjusted amount origin</label>
<asp:TextBox ID="txtAdjustedAmountOrigin" runat="server" 
    CssClass="form-control rounded w-70" 
    Style="text-align: right;" 
    ReadOnly="true" />

                        </div>
                        
                        <!-- Column for Amount Origin and Actual Nondeductible Tax -->
                        <div class="col-md-3">
                            <label class="form-label">Amount origin</label>
                            <asp:TextBox ID="txtAmountOrigin_AmountTab" runat="server" CssClass="form-control rounded w-70" Style="text-align: right;" ReadOnly="true" onblur="formatWithCommas(this)" />

                            <label class="form-label mt-3">Actual nondeductible sales tax</label>
                            <asp:TextBox ID="txtActualNonDeductible" runat="server" CssClass="form-control rounded w-70" Style="text-align: right;" ReadOnly="true" onblur="formatWithCommas(this)" />
                        </div>
                        
                        <!-- Column for Calculated fields and Override -->
                        <div class="col-md-3">
                            <label class="form-label" >Calculated nondeductible sales tax</label>
                            <asp:TextBox ID="txtCalcNonDeductible" runat="server" CssClass="form-control rounded w-70" Style="text-align: right;" ReadOnly="true" onblur="formatWithCommas(this)" />

                            <label class="form-label mt-3">Calculated sales tax amount</label>
                            <asp:TextBox ID="txtCalculatedSalesTaxAmount" runat="server" CssClass="form-control rounded w-70" Style="text-align: right;" ReadOnly="true" onblur="formatWithCommas(this)" />
                        </div>

                        <!-- Column for Actual Sales Tax and Override option -->
                         <div class="col-md-3">
                            <label class="form-label">Actual sales tax amount</label>
                            <asp:TextBox ID="txtActualSalesTaxAmount_AmountTab" runat="server" CssClass="form-control rounded w-70" Style="text-align: right;" ReadOnly="true" onblur="formatWithCommas(this)"/>
                            
                            <!-- Override control -->
                            <div class="mt-4">
    <div class="info-row">
        <div class="info-block d-flex align-items-center">
            <strong class="me-3">Override calculated sales tax</strong>
            <label class="toggle-switch mb-0">
                <asp:CheckBox ID="chkOverrideSalesTax" runat="server" ReadOnly="true" />
                <span class="slider"></span>
            </label>
        </div>
    </div>
</div>

                        </div>

                    </div>
                </div>

                <!-- ACCOUNTING CURRENCY SECTION (Right Side - Takes up 5/12ths of the page) -->
                <div class="col-md-5">
                    <h5 class="mb-3">ACCOUNTING CURRENCY</h5>
                    <div class="row g-3 form-section">
                        <!-- Column 1: Currency -->
                        <div class="col-md-6">
                            <label class="form-label">Currency</label>
                            <asp:TextBox ID="txtAccountingCurrency" runat="server" CssClass="form-control rounded w-60" ReadOnly="true" />
                        </div>
                        
                        <!-- Column 2: Amount Origin -->
                        <div class="col-md-6">
                            <label class="form-label">Amount origin</label>
                            <asp:TextBox ID="txtAcctAmountOrigin" runat="server" CssClass="form-control rounded w-70" Style="text-align: right;" ReadOnly="true" onblur="formatWithCommas(this)" />
                        </div>
                        
                        <!-- Column 1: Blank Spacer -->
                        <div class="col-md-6">
                            <!-- Spacer for vertical alignment -->
                        </div>

                        <!-- Column 2: Actual Nondeductible Tax -->
                        <div class="col-md-6">
                            <label class="form-label">Actual nondeductible sales tax</label>
                            <asp:TextBox ID="txtAcctActualNonDeductible" runat="server" CssClass="form-control rounded w-70" Style="text-align: right;" ReadOnly="true" onblur="formatWithCommas(this)" />
                        </div>
                        
                        <!-- Column 1: Blank Spacer -->
                        <div class="col-md-6">
                            <!-- Spacer for vertical alignment -->
                        </div>

                        <!-- Column 2: Actual Sales Tax Amount -->
                        <div class="col-md-6">
                            <label class="form-label">Actual sales tax amount</label>
                            <asp:TextBox ID="txtAcctActualSalesTaxAmount" runat="server" CssClass="form-control rounded w-70" Style="text-align: right;" ReadOnly="true" onblur="formatWithCommas(this)" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>

      <!-- ✅ Totals Section - Visible across all tabs -->
    <div class="row g-3 border-top pt-3 mt-2 bg-light px-2">
        <div class="col-md-3">
            <label class="form-label fw-bold">Total calculated sales tax amount</label>
            <asp:TextBox ID="txtTotalCalculatedSalesTax" runat="server"
                CssClass="form-control rounded"
                Style="text-align: right;"
                Text="0.00"
                Enabled="false" />
        </div>
        <div class="col-md-3">
            <label class="form-label fw-bold">Total actual sales tax amount</label>
            <asp:TextBox ID="txtTotalActualSalesTax" runat="server"
                CssClass="form-control rounded"
                Style="text-align: right;"
                Text="0.00"
                Enabled="false" />
        </div>
    </div>
       <div class="action-footer">
      <asp:LinkButton ID="btnOK" runat="server" OnClientClick="javascript: return closeDialog();">OK</asp:LinkButton>
     
   </div>
<script>
    function formatWithCommas(input) {
        let value = input.value.replace(/,/g, ''); // remove old commas
        if (!isNaN(value) && value !== '') {
            input.value = parseFloat(value).toLocaleString('en-US', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
        }
    }
</script>



</asp:Content>
