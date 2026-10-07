<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder_ReceiptListJournal.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_ReceiptListJournal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .nav-tabs .nav-link {
            font-weight: 600;
            color: #333;
        }

        .nav-tabs .nav-link.active {
            background-color: #f8f9fa;
            border-color: #dee2e6 #dee2e6 #fff;
        }

        .tab-content {
            border: 1px solid #dee2e6;
            border-top: none;
            padding: 20px;
            background-color: #fff;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
        }

        .grid th, .grid td {
            padding: 8px 10px;
            text-align: left;
            border: 1px solid #dee2e6;
        }
        .textbox-grey {
    background-color: #f0f0f0 !important;
}
    </style>
      <script>
  function toggleDropdown(menuId) {
      var menu = document.getElementById(menuId);
      menu.classList.toggle("show");
  }

  document.addEventListener("click", function (event) {
      if (!event.target.closest(".dropdown")) {
          document.querySelectorAll(".custom-dropdown").forEach(function (dd) {
              dd.classList.remove("show");
          });
      }
  });
      </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
    <asp:LinkButton ID="btnBack" runat="server" OnClick="btnBack_Click">
        <i class='mdi mdi-arrow-left' style='margin-right:4px;'></i>Back
    </asp:LinkButton>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <div class="action-items d-flex align-items-center gap-3">

    <!-- Preview/Print Dropdown -->
    <div class="grid-btn dropdown">

        <asp:LinkButton 
            ID="btnPreview_Print"
            runat="server"
            CssClass="text-primary action-link dropdown-toggle"
            OnClientClick="toggleDropdown('previewMenu'); return false;"
            style="border: none; background: none; padding: 0;">
            Preview/Print
        </asp:LinkButton>

        <!-- Dropdown Menu -->
        <div id="previewMenu" class="custom-dropdown">

            <asp:LinkButton 
                ID="btnCopyPreview"
                runat="server"
                CssClass="text-primary dropdown-item action-link"
                OnClick="btnCopyPreview_Click">
                Copy Preview
            </asp:LinkButton>

        </div>

    </div>

 

</div>

    <!-- Tabs Navigation -->
    <ul class="nav nav-tabs" id="purchaseTabs" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overviewTab"
                role="tab" aria-controls="overviewTab" aria-selected="true">Overview</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="lines-tab" data-toggle="tab" href="#linesTab"
                role="tab" aria-controls="linesTab" aria-selected="false">Lines</a>
        </li>
    </ul>

  
    <div class="tab-content" id="purchaseTabsContent">

      
        <div class="tab-pane fade show active" id="overviewTab" role="tabpanel" aria-labelledby="overview-tab">
            <asp:GridView ID="gvOverview" runat="server" AutoGenerateColumns="False"
                CssClass="grid table table-bordered table-striped" ShowHeaderWhenEmpty="true"
                EmptyDataText="No records found.">
                <Columns>
                      <asp:TemplateField HeaderText="Vendor Account">
      <ItemTemplate>
          <asp:TextBox ID="txtVendAccount" runat="server"
              CssClass="form-control form-control-sm textbox-grey"
              Text='<%# Bind("VendAccount") %>' ReadOnly="true"  Style="background-color:#f0f0f0;"></asp:TextBox>
      </ItemTemplate>
  </asp:TemplateField>

               <asp:TemplateField HeaderText="Purchase Order">
    <ItemTemplate>
        <asp:TextBox ID="txtPurchaseOrder" runat="server"
        CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("PurchId") %>' ReadOnly="true"  Style="background-color:#f0f0f0;"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

  <asp:TemplateField HeaderText="Receipts List">
    <ItemTemplate>
        <asp:TextBox ID="txtReceiptsList" runat="server"
          CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("ReceiptsListId") %>' ReadOnly="true"  Style="background-color:#f0f0f0;"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

               <asp:TemplateField HeaderText="Receipts List Date">
      <ItemTemplate>
          <asp:TextBox ID="txtReceiptsListDate" runat="server" CssClass="form-control form-control-sm textbox-grey"  Text='<%# Bind("ReceiptsListDate", "{0:yyyy-MM-dd}") %>' ReadOnly="true"  Style="background-color:#f0f0f0;"></asp:TextBox>
      </ItemTemplate>
  </asp:TemplateField>

                    
               <asp:TemplateField HeaderText=" Delivery Date">
      <ItemTemplate>
          <asp:TextBox ID="txtDeliveryDate" runat="server"
              CssClass="form-control form-control-sm textbox-grey"
              Text='<%# Bind("DeliveryDate", "{0:yyyy-MM-dd}") %>' ReadOnly="true"  Style="background-color:#f0f0f0;"></asp:TextBox>
      </ItemTemplate>
  </asp:TemplateField>


                   <asp:TemplateField HeaderText="Delivery Terms">
    <ItemTemplate>
        <asp:TextBox ID="txtDeliveryTerms" runat="server"
          CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("DlvTerm") %>' ReadOnly="true"  Style="background-color:#f0f0f0;"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                   <asp:TemplateField HeaderText="Mode Of Delivery">
    <ItemTemplate>
        <asp:TextBox ID="txtModeOfDelivery" runat="server"
         CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("DlvMode") %>' ReadOnly="true"  Style="background-color:#f0f0f0;"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                  
                   
                 
                  
                 
                </Columns>
            </asp:GridView>
        </div>


        <div class="tab-pane fade" id="linesTab" role="tabpanel" aria-labelledby="lines-tab">
            <asp:GridView ID="gvLines" runat="server" AutoGenerateColumns="False"
                CssClass="grid table table-bordered table-striped" ShowHeaderWhenEmpty="true"
                EmptyDataText="No records found.">
                <Columns>

          <asp:TemplateField HeaderText="Purchase Order">
    <ItemTemplate>
        <asp:TextBox ID="txtPurchaseOrder" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("OrigPurchId") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

             <asp:TemplateField HeaderText="Line Number">
    <ItemTemplate>
        <asp:TextBox ID="txtLineNumber" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("PurchaseLineLineNumber") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

 <asp:TemplateField HeaderText="Item Number">
    <ItemTemplate>
        <asp:TextBox ID="txtItemNumber" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("ItemId") %>' ReadOnly="true" ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                <asp:TemplateField HeaderText="Procurement Category">
    <ItemTemplate>
        <asp:TextBox ID="txtProcurementCategory" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("ProcurementCategory") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                    
                <asp:TemplateField HeaderText="Description">
    <ItemTemplate>
        <asp:TextBox ID="txtDescription" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
             Width="300px"
            Text='<%# Bind("Name") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                          
                <asp:TemplateField HeaderText="Site">
    <ItemTemplate>
        <asp:TextBox ID="txtSite" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("InventSiteId") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                  <asp:TemplateField HeaderText="Warehouse">
    <ItemTemplate>
        <asp:TextBox ID="txtWarehouse" runat="server"
          CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("InventLocationId") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

 

             <asp:TemplateField HeaderText="Delivery Date">
      <ItemTemplate>
          <asp:TextBox ID="txtDeliveryDate" runat="server"
             CssClass="form-control form-control-sm textbox-grey"
              Text='<%# Bind("DeliveryDate", "{0:yyyy-MM-dd}") %>' ReadOnly="true"></asp:TextBox>
      </ItemTemplate>
  </asp:TemplateField>

                    <asp:TemplateField HeaderText="Unit">
    <ItemTemplate>
        <asp:TextBox ID="txtUnit" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("PurchUnit") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                   <asp:TemplateField HeaderText="Purchase Quantity">
    <ItemTemplate>
        <asp:TextBox ID="txtPurchaseQuantity" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("PurchQty") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                 <asp:TemplateField HeaderText="Quantity">
    <ItemTemplate>
        <asp:TextBox ID="txtInventQuantity" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("InventQty") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>
                   
                   
                   
                   
                 
                   
                   
                 
                 
                 
                   
                  
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>