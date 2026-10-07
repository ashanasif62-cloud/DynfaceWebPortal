<%@ Page Title="Cancel Transfer Order" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransferOrder_History.aspx.cs" Inherits="DynamicsPortal.ESS.PR.TransferOrder_History" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <style>
        
           
           
    </style>
</asp:Content>

<asp:Content ID="ActionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>
                <div class="action-items">
        <a onclick="history.back(); return false;" class="btn-link">
             <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
        </a>
    </div>
            <div class="action-items">
                <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click">
                    Cancel
                </asp:LinkButton>
            </div>
             <div class="action-items">
                <asp:LinkButton ID="btnShipment" runat="server" OnClick="btnShipment_Click">
                    <i class="mdi mdi-truck-fast" style="margin-right: 4px;"></i>Shipment
                </asp:LinkButton>
            </div>
            <div class="action-items">
                <asp:LinkButton ID="btnReceive" runat="server" OnClick="btnReceive_Click">
                    <i class="mdi mdi-package-down" style="margin-right: 4px;"></i>Receive
                </asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="detailUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="card shadow-sm">
                <div class="card-header bg-light">
                    <ul class="nav nav-tabs" id="mainTabs" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overview" role="tab">Overview</a>
                        </li>
<%--                        <li class="nav-item">
                            <a class="nav-link" id="general-tab" data-toggle="tab" href="#general" role="tab">General</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="delivery-tab" data-toggle="tab" href="#delivery" role="tab">Delivery</a>
                        </li>--%>
                    </ul>
                </div>

                <div class="card-body tab-content">
                    <!-- Overview Tab -->
                    <div class="tab-pane fade show grid-size active" id="overview" role="tabpanel">
                            <asp:GridView ID="gridView" runat="server" CssClass="table table-condensed no-border table-hover sortable grid-header"
                                AutoGenerateColumns="false" DataKeyNames="RecId" ShowHeaderWhenEmpty="true" Data="searchable"
                                EmptyDataText="<div class='empty-state'>No Records to show here.</div>">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_SelectSingle" CssClass="round-checkbox" runat="server" AutoPostBack="true" OnCheckedChanged="chk_SelectSingle_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transfer number">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltransferNumber" runat="server" Text='<%# Bind("TransferId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Voucher">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVoucher" runat="server" Text='<%# Bind("VoucherId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <%--    <asp:TemplateField HeaderText="Posting Date">
                                        <ItemTemplate>
                                            <%# (Convert.ToDateTime(Eval("TransDate")) == new DateTime(1900,1,1)) 
                                                 ? "" 
                                                 : Convert.ToDateTime(Eval("TransDate")).ToString("M/d/yyyy") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Posting Date">
    <ItemTemplate>
        <asp:Label ID="lblPostingDate" runat="server" Text='<%# Bind("TransDate") %>' Visible="false" />
        <%# (Convert.ToDateTime(Eval("TransDate")) == new DateTime(1900,1,1)) 
             ? "" 
             : Convert.ToDateTime(Eval("TransDate")).ToString("M/d/yyyy") %>
    </ItemTemplate>
</asp:TemplateField>
                                    <asp:TemplateField HeaderText="Update Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUpdateType" runat="server" Text='<%# Bind("UpdateType") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Autoreceive">
                                        <ItemTemplate>
                                            <%# (Eval("AutoreceiveQty") != null && Eval("AutoreceiveQty").ToString() == "Yes") ? "✓" : "" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Canceled Shipment">
                                        <ItemTemplate>
                                            <%# (Eval("CanceledShipment") != null && Eval("CanceledShipment").ToString() == "Yes") ? "✓" : "" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Worker">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUpdatedByWorker" runat="server" Text='<%# Bind("UpdatedByWorker") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tracking ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTrackingId" runat="server" Text='<%# Bind("TrackingId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                    </div>

                    <!-- General Tab -->
                    <div class="tab-pane fade" id="general" role="tabpanel">
                        <div class="custom-section-title mb-3"><strong>GENERAL</strong></div>
                        <div class="info-row">
                            <div class="info-block"><strong>From warehouse</strong><span><asp:Label ID="lblHeaderFromWarehouse" runat="server" /></span></div>
                            <div class="info-block"><strong>To warehouse</strong><span><asp:Label ID="lblHeaderToWarehouse" runat="server" /></span></div>
                            <div class="info-block"><strong>Ship date</strong><span><asp:Label ID="lblHeaderShipDate" runat="server" /></span></div>
                            <div class="info-block"><strong>Receipt date</strong><span><asp:Label ID="lblHeaderReceiptDate" runat="server" /></span></div>
                        </div>
                    </div>

                    <!-- Delivery Tab -->
                    <div class="tab-pane fade" id="delivery" role="tabpanel">
                        <div class="custom-section-title mb-3"><strong>DELIVERY</strong></div>
                        <div class="info-row">
                            <div class="info-block"><strong>Delivery address</strong><span><asp:Label ID="lblDeliveryAddress" runat="server" /></span></div>
                            <div class="info-block"><strong>Delivery contact</strong><span><asp:Label ID="lblDeliveryContact" runat="server" /></span></div>
                            <div class="info-block"><strong>Delivery date</strong><span><asp:Label ID="lblDeliveryDate" runat="server" /></span></div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card shadow-sm">
                <div class="card-header bg-light">
                    <ul class="nav nav-tabs" id="lineTabs" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" id="line-overview-tab" data-toggle="tab" href="#line-overview" role="tab">Overview</a>
                        </li>
<%--                        <li class="nav-item">
                            <a class="nav-link" id="line-general-tab" data-toggle="tab" href="#line-general" role="tab">General</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="line-delivery-tab" data-toggle="tab" href="#line-delivery" role="tab">Delivery</a>
                        </li>--%>
                    </ul>
                </div>

                <div class="history-card-body tab-content">
                    <!-- Overview Tab -->
                    <div class="tab-pane fade show grid-size" id="line-overview" role="tabpanel">
                            <asp:GridView ID="gridView1" runat="server" CssClass="table table-condensed no-border table-hover sortable grid-header"
                                AutoGenerateColumns="false" DataKeyNames="RecId" ShowHeaderWhenEmpty="true" Data="searchable"
                                EmptyDataText="<div class='empty-state'>No Records to show here.</div>">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_SelectSingle" CssClass="round-checkbox" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("ItemId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
<%--                                            <asp:Label ID="lblVoucher" runat="server" Text='<%# Bind("VoucherId") %>' />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shipped quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQtyShipped" runat="server" Text='<%# Bind("QtyShipped") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Received quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQtyReceived" runat="server" Text='<%# Bind("QtyReceived") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Scrapped quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQtyScrapped" runat="server" Text='<%# Bind("QtyScrapped") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemUnit" runat="server" Text='<%# Bind("ItemUnit") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemName" runat="server" Text='<%# Bind("itemName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                    </div>

                    <!-- General Tab -->
                    <div class="tab-pane fade" id="line-general" role="tabpanel">
                        <div class="custom-section-title mb-3"><strong>GENERAL</strong></div>
                        <div class="info-row">
                            <div class="info-block"><strong>From warehouse</strong><span><asp:Label ID="Label1" runat="server" /></span></div>
                            <div class="info-block"><strong>To warehouse</strong><span><asp:Label ID="Label2" runat="server" /></span></div>
                            <div class="info-block"><strong>Ship date</strong><span><asp:Label ID="Label3" runat="server" /></span></div>
                            <div class="info-block"><strong>Receipt date</strong><span><asp:Label ID="Label4" runat="server" /></span></div>
                        </div>
                    </div>

                    <!-- Delivery Tab -->
                    <div class="tab-pane fade" id="line-delivery" role="tabpanel">
                        <div class="custom-section-title mb-3"><strong>DELIVERY</strong></div>
                        <div class="info-row">
                            <div class="info-block"><strong>Delivery address</strong><span><asp:Label ID="Label5" runat="server" /></span></div>
                            <div class="info-block"><strong>Delivery contact</strong><span><asp:Label ID="Label6" runat="server" /></span></div>
                            <div class="info-block"><strong>Delivery date</strong><span><asp:Label ID="Label7" runat="server" /></span></div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
            <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(function () {
            showAJAXOverlay();  // Should now fire
        });

        prm.add_endRequest(function () {
            hideAJAXOverlay();
        });
            </script>
</asp:Content>
