<%@ Page Title="On Hand" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="TransferOrder_OnHand.aspx.cs" Inherits="DynamicsPortal.ESS.PR.TransferOrder_OnHand" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div class="container-fluid">
        <h1></h1>
        <div class="row">
            <!-- Left Column -->
            <div class="col-md-2">
                <label class="onHand-label">Product name</label>
                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <h4>INVENTORY DIMENSIONS</h4>
                <label class="onHand-label">Configuration</label>
                <asp:TextBox ID="txtConfigId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Size</label>
                <asp:TextBox ID="txtSizeId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Color</label>
                <asp:TextBox ID="txtColorId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Style</label>
                <asp:TextBox ID="txtStyleId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Version</label>
                <asp:TextBox ID="txtVersionId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Site</label>
                <asp:TextBox ID="txtSiteId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Warehouse</label>
                <asp:TextBox ID="txtWareHouse" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Batch number</label>
                <asp:TextBox ID="txtBatchNum" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Location</label>
                <asp:TextBox ID="txtLocationId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Serial number</label>
                <asp:TextBox ID="txtSerialNum" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <label class="onHand-label">Inventory status</label>
                <asp:TextBox ID="txtInventStatusId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>

                <h4>Unit</h4>
                <label class="onHand-label">Unit</label>
                <asp:TextBox ID="txtUnitId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>
                <label class="onHand-label">CW Unit</label>
                <asp:TextBox ID="txtCWUnitId" runat="server" CssClass="form-control input-custom" Enabled="false"></asp:TextBox>
            </div>

            <!-- Middle Column -->
            <div class="col-md-5">
                <h4>INVENTORY VALUE</h4>
                <table class="table-custom">
                    <tr><td>Physical cost amount</td><td><asp:TextBox ID="txtPhyCostAmt" runat="server" CssClass="input-custom" Enabled="false"></asp:TextBox></td></tr>
                    <tr><td>Financial cost amount</td><td><asp:TextBox ID="txtFinCostAmt" runat="server" CssClass="input-custom" Enabled="false"></asp:TextBox></td></tr>
                    <tr><td>Cost price</td><td><asp:TextBox ID="txtCostPrice" runat="server" CssClass="input-custom" Enabled="false"></asp:TextBox></td></tr>
                </table>

                <h4>ON-HAND</h4>
                <table class="table-custom">
                    <tr><td></td><td>QUANTITY</td><td>CW QUANTITY</td></tr>
                    <tr><td>PHYSICAL INVENTORY</td><td><asp:TextBox ID="txtQtyPhyInvent" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyPhyInvent" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>PHYSICAL RESERVED</td><td><asp:TextBox ID="txtQtyPhyReserve" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyPhyReserve" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>AVAILABLE PHYSICAL</td><td><asp:TextBox ID="txtQtyPhyAvailable" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyPhyAvailable" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>AVAILABLE FOR RESERVATION</td><td><asp:TextBox ID="txtQtyAvailForReservation" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td></td></tr>
                    <tr><td>ORDERED IN TOTAL</td><td><asp:TextBox ID="txtQtyTotalOrdered" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyTotalOrdered" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>ORDERED RESERVED</td><td><asp:TextBox ID="txtORDEREDRESERVED" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyORDEREDRESERVED" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>ON ORDER IN TOTAL</td><td><asp:TextBox ID="txtOnorderinttoal" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyonorderintotal" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>TOTAL AVAILABLE</td><td><asp:TextBox ID="txtQtyTotalAvailable" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyTotalAvailable" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                </table>
            </div>

            <!-- Right Column -->
            <div class="col-md-5">
                <h4>PHYSICAL INVENTORY</h4>
                <table class="table-custom">
                    <tr><td></td><td>QUANTITY</td><td>CW QUANTITY</td></tr>
                    <tr><td>POSTED QUANTITY</td><td><asp:TextBox ID="txtQtyPosted" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyPosted" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>DEDUCTED</td><td><asp:TextBox ID="txtQtyDeducted" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyDeducted" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>PICKED</td><td><asp:TextBox ID="txtQtyPicked" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyPicked" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>RECEIVED</td><td><asp:TextBox ID="txtQtyReceived" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyReceived" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>REGISTERED</td><td><asp:TextBox ID="txtQtyRegistered" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyRegistered" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                </table>

                <h4>ORDERED IN TOTAL</h4>
                <table class="table-custom">
                    <tr><td></td><td>QUANTITY</td><td>CW QUANTITY</td></tr>
                    <tr><td>ARRIVED</td><td><asp:TextBox ID="txtQtyArrived" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyArrived" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>ORDERED</td><td><asp:TextBox ID="txtQtyOrdered" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyOrdered" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                </table>

                <h4>VARIOUS</h4>
                <table class="table-custom">
                    <tr><td></td><td>QUANTITY</td><td>CW QUANTITY</td></tr>
                    <tr><td>ON ORDER</td><td><asp:TextBox ID="txtQtyOnOrder" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyOnOrder" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>QUOTATION RECEIPT</td><td><asp:TextBox ID="txtQtyQuotationReceipt" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyQuotationReceipt" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                    <tr><td>QUOTATION ISSUE</td><td><asp:TextBox ID="txtQtyQuotationIssue" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td><td><asp:TextBox ID="txtCWQtyQuotationIssue" CssClass="input-custom" Enabled="false" runat="server"></asp:TextBox></td></tr>
                </table>
            </div>

            <div class="close-btn-container">
                <asp:Button ID="btnClose" runat="server" Text="Close"
                    CssClass="Onhandd365-close-btn"
                    OnClientClick="return closeDialog();" />
            </div>
        </div>
    </div>
</asp:Content>
