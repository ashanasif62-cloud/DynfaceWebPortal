<%@ Page Title="Quality Order Create" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="QualityOrderCreate.aspx.cs" Inherits="DynamicsPortal.ESS.PR.QualityOrderCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .form-section {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 2rem;
            padding: 20px;
        }
        .section-heading {
            font-weight: bold;
            font-size: 1rem;
            margin-bottom: 10px;
            color: #333;
            text-transform: uppercase;
        }
        .form-group {
            margin-bottom: 10px;
        }
        label {
            display: block;
            font-size: 0.9rem;
            font-weight: 500;
            margin-bottom: 3px;
        }
        .form-control {
            width: 100%;
            padding: 5px 8px;
            border: 1px solid #ccc;
            border-radius: 3px;
        }
        .form-actions {
            text-align: right;
            padding: 10px 20px;
        }
        .btn {
            padding: 5px 15px;
            border: none;
            border-radius: 4px;
            font-size: 0.9rem;
        }
        .btn-primary {
            background-color: #0078d7;
            color: white;
        }
        .btn-secondary {
            background-color: #f3f3f3;
            color: #333;
            border: 1px solid #ccc;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <div class="form-section">
        <!-- LEFT COLUMN -->
        <div>
            <div class="section-heading">Identification</div>

            <div class="form-group">
                <label>Quality order</label>
                <asp:TextBox ID="txtQualityOrder" runat="server" CssClass="form-control" ReadOnly="true" Text=""></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Item number <span class="text-danger">*</span></label>
                <asp:DropDownList ID="ddlItemNumber" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label>Product name</label>
                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Test group <span class="text-danger">*</span></label>
                <asp:DropDownList ID="ddlTestGroup" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label>CW qty</label>
                <asp:TextBox ID="txtCWQty" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Quantity <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <!-- RIGHT COLUMN -->
        <div>
            <div class="section-heading">Inventory dimensions</div>

            <div class="form-group"><label>Site</label><asp:TextBox ID="txtSite" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Warehouse</label><asp:TextBox ID="txtWarehouse" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Location</label><asp:TextBox ID="txtLocation" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>License plate</label><asp:TextBox ID="txtLicensePlate" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Inventory status</label><asp:TextBox ID="txtInventoryStatus" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Batch number</label><asp:TextBox ID="txtBatchNumber" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Serial number</label><asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Configuration</label><asp:TextBox ID="txtConfiguration" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Size</label><asp:TextBox ID="txtSize" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Color</label><asp:TextBox ID="txtColor" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Style</label><asp:TextBox ID="txtStyle" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Version</label><asp:TextBox ID="txtVersion" runat="server" CssClass="form-control"></asp:TextBox></div>
            <div class="form-group"><label>Owner</label><asp:TextBox ID="txtOwner" runat="server" CssClass="form-control"></asp:TextBox></div>
        </div>
    </div>

    <!-- BUTTONS -->
    <div class="form-actions">
        <asp:Button ID="btnOk" runat="server" CssClass="btn btn-primary" Text="OK" OnClick="btnOk_Click" />
        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary" Text="Cancel" OnClick="btnCancel_Click" />
    </div>
</asp:Content>
