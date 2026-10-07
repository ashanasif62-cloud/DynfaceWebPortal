<%@ Page Title="Create Sales Order" Language="C#" MasterPageFile="~/Modal.Master"
    AutoEventWireup="true"
    CodeBehind="SalesOrder_Create.aspx.cs"
    Inherits="DynamicsPortal.SalesOrder_Create" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
<style>
    .autocomplete-input {
        width: 250px;
        height: 30px;
        font-size: 11px;
        font-family: Arial, sans-serif;
        border: 1px solid #444;
        border-radius: 0px;
        padding: 1px 4px;
        box-sizing: border-box;
        background-color: white;
    }
</style>
    <style>
/* Toggle Switch */
.switch {
    position: relative;
    display: inline-block;
    width: 42px;
    height: 22px;
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
    transition: .3s;
    border-radius: 34px;
}

.slider:before {
    position: absolute;
    content: "";
    height: 16px;
    width: 16px;
    left: 3px;
    bottom: 3px;
    background-color: white;
    transition: .3s;
    border-radius: 50%;
}

input:checked + .slider {
    background-color: #0078D4; /* D365 blue */
}

input:checked + .slider:before {
    transform: translateX(20px);
}
</style>
</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="PageContent" runat="server">

<asp:UpdatePanel ID="upSalesOrder" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<!-- ================= CUSTOMER PANEL ================= -->
<a href="#customerPanel" class="d365-toggle-header d-flex justify-content-between align-items-center"
   data-toggle="collapse" role="button" aria-expanded="true">
    <span class="section-title">Customer</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</a>

<div class="collapse show mt-3" id="customerPanel">
<div class="card shadow-sm">
<div class="card-body">

    <label>Customer Account</label>
<asp:DropDownList 
    ID="ddlCustomerAccount" 
    runat="server"
    CssClass="form-control autocomplete-input"
    AutoPostBack="true"
    OnSelectedIndexChanged="ddlCustomerAccount_SelectedIndexChanged">
</asp:DropDownList>

<div class="form-group">
    <label>One Time Customer</label><br />

    <label class="switch">
        <asp:CheckBox ID="chkOneTimeCustomer" runat="server" />
        <span class="slider"></span>
    </label>
</div>


<div class="form-group">
<label>Contact</label>
<asp:DropDownList ID="ddlContact" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<h5>ADDRESS</h5>

<div class="form-group">
<label>Delivery Name</label>
<asp:TextBox ID="txtDeliveryName" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<div class="form-group">
<label>Address</label>
<asp:TextBox ID="txtAddress" runat="server"
CssClass="form-control autocomplete-input" TextMode="MultiLine" Rows="3" />
</div>

<div class="form-group">
<label>Delivery Address</label>
    <asp:TextBox ID="txtDeliveryAddress" runat="server"
CssClass="form-control autocomplete-input" />
</div>


</div>
</div>
</div>


<!-- ================= GENERAL PANEL ================= -->
<a href="#generalPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
   data-toggle="collapse" role="button" aria-expanded="false">
    <span class="section-title">General</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</a>

<div class="collapse mt-2" id="generalPanel">
<div class="card shadow-sm">
<div class="card-body">

<div class="d-flex flex-wrap justify-content-between">

<!-- LEFT SECTION -->
<div style="flex: 0 0 48%;">

<strong>SALES ORDER</strong>

<div class="form-group" style="width:250px;">
<label>Sales Order</label>
<asp:TextBox ID="txtSalesOrder" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<div class="form-group" style="width:250px;">
<label>Invoice Account</label>
<asp:DropDownList ID="ddlInvoiceAccount" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<div class="form-group" style="width:250px;">
<label>Order Type</label>
<asp:DropDownList ID="ddlOrderType" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<div class="form-group" style="width:250px;">
<label>Name</label>
<asp:TextBox ID="txtName" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<strong>REFERENCES</strong>

<div class="form-group" style="width:250px;">
<label>Customer Requisition</label>
<asp:TextBox ID="txtCustomerRequisition" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<div class="form-group" style="width:250px;">
<label>Customer Reference</label>
<asp:TextBox ID="txtCustomerReference" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<div class="form-group" style="width:250px;">
<label>Sales Agreement Id</label>
<asp:TextBox ID="txtSalesAgreementId" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<div class="form-group" style="width:250px;">
<label>Project Id</label>
<asp:DropDownList ID="ddlProjectId" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<div class="form-group" style="width:250px;">
<label>Bank Document Type</label>
<asp:DropDownList ID="ddlBankDocumentType" runat="server"
CssClass="form-control autocomplete-input" />
</div>

</div>

<!-- RIGHT SECTION -->
<div style="flex: 0 0 48%;">

<strong>CURRENCY</strong>

<div class="form-group" style="width:250px;">
<label>Currency</label>
<asp:DropDownList ID="ddlCurrency" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<strong>STORAGE DIMENSION</strong>

<div class="form-group" style="width:250px;">
<label>Site</label>
<asp:DropDownList ID="ddlSite" runat="server"
CssClass="form-control autocomplete-input" />
</div>

<div class="form-group" style="width:250px;">
<label>Warehouse</label>
<asp:DropDownList ID="ddlWarehouse" runat="server"
CssClass="form-control autocomplete-input" />
</div>

</div>
</div>

</div>
</div>
</div>


<!-- ================= DELIVERY PANEL ================= -->
<a href="#deliveryPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
   data-toggle="collapse" role="button">
    <span class="section-title">Delivery</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</a>

<div class="collapse mt-2" id="deliveryPanel">
<div class="card shadow-sm">
<div class="card-body">

<!-- Requested Receipt Date -->
<div class="form-group" style="width:250px;">
    <label>Requested Receipt Date</label>
    <asp:TextBox ID="txtRequestedReceiptDate" runat="server"
        CssClass="form-control autocomplete-input" />
</div>

<!-- Requested Ship Date -->
<div class="form-group" style="width:250px;">
    <label>Requested Ship Date</label>
    <asp:TextBox ID="txtRequestedShipDate" runat="server"
        CssClass="form-control autocomplete-input" />
</div>

<!-- Confirm Date Toggle -->
<div class="form-group">
    <label>Confirm Date</label><br />
    <label class="switch">
        <asp:CheckBox ID="chkConfirmDate" runat="server" />
        <span class="slider"></span>
    </label>
</div>

<!-- ================= NEW SECTION ================= -->
<div class="mt-3">
    <strong class="d-block mb-2">MISC. DELIVERY INFO</strong>
</div>

<!-- Mode of Delivery -->
<div class="form-group" style="width:250px;">
    <label>Mode of delivery</label>
    <asp:DropDownList ID="ddlModeOfDelivery" runat="server"
        CssClass="form-control autocomplete-input" />
</div>

<!-- Delivery Term -->
<div class="form-group" style="width:250px;">
    <label>Delivery term</label>
    <asp:DropDownList ID="ddlDeliveryTerm" runat="server"
        CssClass="form-control autocomplete-input" />
</div>

</div>
</div>
</div>


<!-- ================= ADMINISTRATION PANEL ================= -->
<a href="#adminPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
   data-toggle="collapse" role="button">
    <span class="section-title">Administration</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</a>

<div class="collapse mt-2" id="adminPanel">
<div class="card shadow-sm">
<div class="card-body">

<div class="d-flex flex-wrap justify-content-between">

    <!-- LEFT SIDE -->
    <div style="flex: 0 0 48%;">

        <!-- Pool -->
        <div class="form-group mb-3" style="width:250px;">
            <label>Pool</label>
            <asp:DropDownList ID="ddlPool" runat="server"
                CssClass="form-control autocomplete-input" />
        </div>

        <!-- Language -->
        <div class="form-group mb-3" style="width:250px;">
            <label>Language</label>
            <asp:DropDownList ID="ddlLanguage" runat="server"
                CssClass="form-control autocomplete-input" />
        </div>

        <!-- Sales Origin -->
        <div class="form-group mb-3" style="width:250px;">
            <label>Sales Origin</label>
            <asp:TextBox ID="TxtSalesOrigin" runat="server"
CssClass="form-control autocomplete-input" />
          
        </div>

    </div>


    <!-- RIGHT SIDE -->
    <div style="flex: 0 0 48%;">

        <!-- Sales Unit -->
        <div class="form-group mb-3" style="width:250px;">
            <label>Sales Unit</label>
            <asp:DropDownList ID="ddlSalesUnit" runat="server"
                CssClass="form-control autocomplete-input" />
        </div>

        <!-- Sales Taker -->
        <div class="form-group mb-3" style="width:250px;">
            <label>Sales Taker</label>
             <asp:TextBox ID="TxtSalesTaker" runat="server"
     CssClass="form-control autocomplete-input" />
          
        </div>

        <!-- Sales Responsible -->
        <div class="form-group mb-3" style="width:250px;">
            <label>Sales Responsible</label>
                    <asp:TextBox ID="TxtSalesResponsible" runat="server"
CssClass="form-control autocomplete-input" />
          
        </div>

    </div>

</div>

</div>
</div>
</div>


<%--<!-- ================= PRICING PANEL ================= -->
<a href="#pricingPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
   data-toggle="collapse" role="button">
    <span class="section-title">Pricing</span>
    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
</a>

<div class="collapse mt-2" id="pricingPanel">
<div class="card shadow-sm">
<div class="card-body">

<div class="form-group" style="width:250px;">
<label>Currency</label>
<asp:TextBox ID="txtCurrency" runat="server"
CssClass="form-control autocomplete-input" />
</div>

</div>
</div>
</div>--%>


<!-- ACTION BUTTONS -->
<div class="action-footer">
<asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
<asp:LinkButton ID="btnCancel" runat="server"
OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
</div>

</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
