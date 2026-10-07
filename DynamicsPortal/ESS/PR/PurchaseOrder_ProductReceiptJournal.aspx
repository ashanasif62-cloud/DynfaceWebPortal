<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder_ProductReceiptJournal.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_ProductReceiptJournal" %>
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
            padding: 10px;
            text-align: left;
            border: 1px solid #dee2e6;
        }
        .textbox-grey {
    background-color: #f0f0f0 !important;
}
/* Align action buttons */
.action-items {
    display: flex;
    align-items: center;
}

/* Unified font size */
.action-link {
    font-size: 0.875rem; /* Bootstrap small */
    line-height: 1.2;
    cursor: pointer;
}

/* Custom dropdown */
.custom-dropdown {
    display: none;
    position: absolute;
    top: 100%;
    left: 0;
    background: #ffffff;
    border: 1px solid #ddd;
    min-width: 160px;
    z-index: 1000;
}

/* Show dropdown */
.custom-dropdown.show {
    display: block;
}

/* Dropdown items */
.custom-dropdown .dropdown-item {
    padding: 6px 12px;
    text-decoration: none;
}

/* Parent relative for dropdown */
.grid-btn {
    position: relative;
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

    <!-- Tabs Navigation -->
    <ul class="nav nav-tabs" id="purchaseTabs" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overviewTab"
                role="tab" aria-controls="overviewTab" aria-selected="true" >Overview</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="line-tab" data-toggle="tab" href="#lineTab"
                role="tab" aria-controls="lineTab" aria-selected="false">Line</a>
        </li>
    </ul>

  
    <div class="tab-content" id="purchaseTabsContent">

      
        <div class="tab-pane fade show active" id="overviewTab" role="tabpanel" aria-labelledby="overview-tab">
  <!-- Action Buttons -->
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

    <!-- Vouchers Button -->
    <asp:LinkButton 
        ID="btnVouchers"
        runat="server"
        CssClass="text-primary action-link"
        OnClick="btnVouchers_Click"
        style="border: none; background: none; padding: 0;">
        Vouchers
    </asp:LinkButton>

</div>




          <asp:GridView ID="gvOverview" runat="server" AutoGenerateColumns="False"
    CssClass="grid table table-bordered table-striped"
    ShowHeaderWhenEmpty="true" EmptyDataText="No records found."
               OnRowDataBound="gvOverview_RowDataBound">
               

                <Columns>
                                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
   
    <ItemTemplate>
        <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true" runat="server" CssClass="round-checkbox"  />
    </ItemTemplate>
</asp:TemplateField>

             <asp:TemplateField HeaderText="Purchase Order">
      <ItemTemplate>
          <asp:TextBox ID="txtPurchaseID" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
              Text='<%# Bind("PurchId") %>' ReadOnly="true"  ></asp:TextBox>
      </ItemTemplate>
  </asp:TemplateField>

              <asp:TemplateField HeaderText="Product Receipt">
    <ItemTemplate>
        <asp:TextBox ID="txtPurchaseOrder" runat="server"
          CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("PackingSlipId") %>' ReadOnly="true"  ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                 <asp:TemplateField HeaderText="Date">
    <ItemTemplate>
        <asp:TextBox ID="txtDate" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("DeliveryDate","{0:yyyy-MM-dd}") %>' ReadOnly="true"  ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                        <asp:TemplateField HeaderText="Invoice Issue Date">
    <ItemTemplate>
        <asp:TextBox ID="txtInvoiceDate" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("InvoiceIssueDueDate_W","{0:yyyy-MM-dd}") %>' ReadOnly="true" ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                  <asp:TemplateField HeaderText="Terms">
    <ItemTemplate>
        <asp:TextBox ID="txtTerms" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("DlvTerm") %>' ReadOnly="true" ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                    
                 <asp:TemplateField HeaderText="Mode Of Delivery">
    <ItemTemplate>
        <asp:TextBox ID="txtModeOfDelivery" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("DlvMode") %>' ReadOnly="true"  ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                         
                 <asp:TemplateField HeaderText="Company">
    <ItemTemplate>
        <asp:TextBox ID="txtCompany" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("InterCompanyCompanyId") %>' ReadOnly="true"  Style="background-color:#f0f0f0;"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                               
                 <asp:TemplateField HeaderText="Sales Order">
    <ItemTemplate>
        <asp:TextBox ID="txtSalesOrder" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("InterCompanySalesId") %>' ReadOnly="true" ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                    
                               
                 <asp:TemplateField HeaderText="Posted Via InterCompany">
    <ItemTemplate>
        <asp:TextBox ID="txtPostedViaCompany" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("InterCompanyPosted") %>' ReadOnly="true" ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                   
                   
               
                 
                   
                </Columns>
            </asp:GridView>
        </div>

     
      <div class="tab-pane fade" id="lineTab" role="tabpanel" aria-labelledby="line-tab">
    <asp:GridView ID="gvLines" runat="server" AutoGenerateColumns="False"
        CssClass="grid table table-bordered table-striped"
        ShowHeaderWhenEmpty="true" EmptyDataText="No records found.">
        <Columns>
                                                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
   
    <ItemTemplate>
        <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true" runat="server" CssClass="round-checkbox"  />
    </ItemTemplate>
</asp:TemplateField>

                <asp:TemplateField HeaderText="Purchase Order">
      <ItemTemplate>
          <asp:TextBox ID="txtPurchaseOrder" runat="server"
              CssClass="form-control form-control-sm textbox-grey"
              Text='<%# Bind("OrigPurchid") %>' ReadOnly="true" ></asp:TextBox>
      </ItemTemplate>
  </asp:TemplateField>

               <asp:TemplateField HeaderText="Line Number ">
    <ItemTemplate>
        <asp:TextBox ID="txtLineNumber" runat="server"
            CssClass="form-control form-control-sm"
            Text='<%# Bind("PurchaseLineLineNumber") %>' ReadOnly="true" ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

            
               <asp:TemplateField HeaderText="Item ">
    <ItemTemplate>
        <asp:TextBox ID="txtItem" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("ItemId") %>' ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

              
               <asp:TemplateField HeaderText="Procurement Category">
    <ItemTemplate>
        <asp:TextBox ID="txtPurchCategory" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("ProcurementCategory") %>' ReadOnly="true" ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                           <asp:TemplateField HeaderText="Description">
    <ItemTemplate>
        <asp:TextBox ID="txtDescription" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
             Width="300px"
            Text='<%# Bind("Name") %>' ReadOnly="true"  ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                  <asp:TemplateField HeaderText="Site">
    <ItemTemplate>
        <asp:TextBox ID="txtSite" runat="server"
          CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("InventSiteId") %>' ReadOnly="true"  ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

            
                  <asp:TemplateField HeaderText="Warehouse">
    <ItemTemplate>
        <asp:TextBox ID="txtWarehouse" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("InventLocationId") %>' ReadOnly="true" ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                    <asp:TemplateField HeaderText="Ordered">
    <ItemTemplate>
        <asp:TextBox ID="txtOrdered" runat="server"
           CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("Ordered", "{0:N2}") %>' ReadOnly="true"  ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

            
                    <asp:TemplateField HeaderText="Received">
    <ItemTemplate>
        <asp:TextBox ID="txtReceieved" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("Qty", "{0:N2}") %>' ReadOnly="true"  ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

                             <asp:TemplateField HeaderText="Amount">
    <ItemTemplate>
        <asp:TextBox ID="txtAmount" runat="server"
            CssClass="form-control form-control-sm textbox-grey"
            Text='<%# Bind("ValueMST", "{0:N2}") %>' ReadOnly="true"  ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

            
     <%--                    <asp:TemplateField HeaderText="Configuration" >
     <ItemTemplate>
         <asp:Label ID="lblConfigId" runat="server" Text='<%# Bind("ConfigId") %>'></asp:Label>
     </ItemTemplate>
     <EditItemTemplate>
         <asp:DropDownList ID="ddlConfigId" runat="server"></asp:DropDownList>
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

            <asp:TemplateField HeaderText="Color" >
    <ItemTemplate>
        <asp:Label ID="lblInventColorId" runat="server" Text='<%# Bind("InventColorId") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:DropDownList ID="ddlInventColorId" runat="server"></asp:DropDownList>
    </EditItemTemplate>
</asp:TemplateField>

            <asp:TemplateField HeaderText="Style" >
    <ItemTemplate>
        <asp:Label ID="lblInventStyleId" runat="server" Text='<%# Bind("InventStyleId") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:DropDownList ID="ddlInventStyleId" runat="server"></asp:DropDownList>
    </EditItemTemplate>
</asp:TemplateField>

            <asp:TemplateField HeaderText="Site">
<ItemTemplate>
<asp:Label ID="lblSite" runat="server" Text='<%# Bind("InventSiteId") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlSiteId" OnSelectedIndexChanged="ddlsiteId_selection" AutoPostBack="true" runat="server"></asp:DropDownList>
</EditItemTemplate>
</asp:TemplateField>

            <asp:TemplateField HeaderText="Warehouse">
<ItemTemplate>
<asp:Label ID="lblWarehouse" runat="server" Text='<%# Bind("InventLocationID") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlWarehouse" AutoPostBack="true" OnSelectedIndexChanged="ddlwarehouse_selection" runat="server"></asp:DropDownList>
</EditItemTemplate>
</asp:TemplateField>

             <asp:TemplateField HeaderText="Location" >
     <ItemTemplate>
         <asp:Label ID="lblLocationId" runat="server" Text='<%# Bind("WMSLocationId") %>'></asp:Label>
     </ItemTemplate>
     <EditItemTemplate>
         <asp:DropDownList ID="ddlLocationId" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>

            
             <asp:TemplateField HeaderText="License plate" >
     <ItemTemplate>
         <asp:Label ID="lblLicenseplate" runat="server" Text='<%# Bind("Licenseplate") %>'></asp:Label>
     </ItemTemplate>
     <EditItemTemplate>
         <asp:DropDownList ID="ddlLicenseplate" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>

            
             <asp:TemplateField HeaderText="Inventory status" >
     <ItemTemplate>
         <asp:Label ID="lblInventoryStatus" runat="server" Text='<%# Bind("InventoryStatus") %>'></asp:Label>
     </ItemTemplate>
     <EditItemTemplate>
         <asp:DropDownList ID="ddlInventory Status" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>

            
 <asp:TemplateField HeaderText="Batch Number" >
     <ItemTemplate>
         <asp:Label ID="lblInventBatchId" runat="server" Text='<%# Bind("InventBatchId") %>'></asp:Label>
     </ItemTemplate>
     <EditItemTemplate>
         <asp:DropDownList ID="ddlInventBatchId" runat="server"></asp:DropDownList>
     </EditItemTemplate>
 </asp:TemplateField>

                        <asp:TemplateField HeaderText="Serial Number" >
    <ItemTemplate>
        <asp:Label ID="lblInventSerialId" runat="server" Text='<%# Bind("InventSerialId") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:DropDownList ID="ddlInventSerialId" runat="server"></asp:DropDownList>
    </EditItemTemplate>
</asp:TemplateField>--%>




          
         
          
          
          
         
         
          
          
          
          
        </Columns>
    </asp:GridView>
</div>

    </div>

</asp:Content>