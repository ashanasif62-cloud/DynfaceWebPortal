<%@ Page Title="Quality Order" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QualityOrder.aspx.cs" Inherits="DynamicsPortal.ESS.PR.QualityOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap CSS (if not already included in your master page) -->
  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
        <div class="action-items">
    <asp:LinkButton ID="btnNew"  runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/QualityOrderCreate.aspx', 700)"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
    <div class="container-fluid mt-3">
        <h4 class="mb-3">Quality Order</h4>

        <!-- Tabs Navigation -->
        <ul class="nav nav-tabs" id="qualityOrderTabs" role="tablist">
            <li class="nav-item">
                <a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overview" role="tab">Overview</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="general-tab" data-toggle="tab" href="#general" role="tab">General</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="financial-tab" data-toggle="tab" href="#financial" role="tab">Financial Dimension</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="inventory-tab" data-toggle="tab" href="#inventory" role="tab">Inventory Dimension</a>
            </li>
        </ul>

        <!-- Tabs Content -->
        <div class="tab-content p-3 border border-top-0" id="qualityOrderTabsContent">

            <!-- Overview Tab -->
            <div class="tab-pane fade show active" id="overview" role="tabpanel">
                <asp:GridView ID="GridViewOverview" runat="server" CssClass="table table-bordered table-hover"
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="QualityOrder" HeaderText="Quality Order" />
                        <asp:BoundField DataField="ItemNumber" HeaderText="Item Number" />
                        <asp:BoundField DataField="Site" HeaderText="Site" />
                        <asp:BoundField DataField="Warehouse" HeaderText="Warehouse" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="Worker" HeaderText="Worker" />
                    </Columns>
                </asp:GridView>
            </div>

            <!-- General Tab -->
            <div class="tab-pane fade" id="general" role="tabpanel">
                <p>No fields added yet.</p>
            </div>

            <!-- Financial Dimension Tab -->
            <div class="tab-pane fade" id="financial" role="tabpanel">
                <p>No fields added yet.</p>
            </div>

            <!-- Inventory Dimension Tab -->
            <div class="tab-pane fade" id="inventory" role="tabpanel">
                <p>No fields added yet.</p>
            </div>
        </div>
    </div>

    <!-- Bootstrap JS (if not already in master page) -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
</asp:Content>
