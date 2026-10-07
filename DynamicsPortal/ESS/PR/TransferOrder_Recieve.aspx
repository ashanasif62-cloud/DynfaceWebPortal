<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="TransferOrder_Recieve.aspx.cs" Inherits="DynamicsPortal.TransferOrder_Recieve" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
   
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="detailUpdatePanel" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
        <ContentTemplate>
            <div class="m-5">
                <h1></h1>
            </div>
            <div class="shipmentpanel mb-4">
                <ul class="nav nav-tabs" id="shipmentTabs" role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overview-tab-content" role="tab">Overview</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="general-tab" data-toggle="tab" href="#general-tab-content" role="tab">General</a>
                    </li>
                </ul>

                <div class="tab-content">
                    <div class="tab-pane fade show active table-responsive" id="overview-tab-content" role="tabpanel">
                        <asp:GridView ID="gridView" runat="server" CssClass="table table-condensed no-border table-hover sortable"
                            AutoGenerateColumns="false" DataKeyNames="RecId" ShowHeaderWhenEmpty="true" EmptyDataText="<div class='empty-state'>No Records to show here.</div>">
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SelectSingle" AutoPostBack="true" OnCheckedChanged="chk_UpdateLineGrid" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transfer number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgridTransferNumber" runat="server" Text='<%# Bind("TransferId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Update">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlgridUpdateType" OnSelectedIndexChanged="ddlgridUpdateType_bindLinesGrid" AutoPostBack="true" runat="server" CssClass="">
                                            <asp:ListItem Text="Recieve now" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="All" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Shipment" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shipment Voucher">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlShipmentVoucher" CssClass="" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Posting date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblgridPostingDate" TextMode="Date" CssClass=""
                                            Text='<%# DateTime.Today.ToString("yyyy-MM-dd") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tracking ID">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblgridTrackingId" CssClass="" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Explode lines">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkgridExplodeLines" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print transfer shipment">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="lblgridprintTO" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print product labels">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="lblgridprintPL" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print shelf labels">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="lblgridprintSL" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <div class="tab-pane fade" id="general-tab-content" role="tabpanel">
                        <div class="row">
                            <div class="col-md-6">
                                <h5 class="mt-2">Receive</h5>
                                <table class="form-table">
                                    <tr>
                                        <td>Update</td>
                                        <td>
                                            <asp:DropDownList ID="ddlTblUpdateType" CssClass="form-control form-control-sm" runat="server">
                                                <asp:ListItem Text="Recieve now" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="All" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Shipment Coucher</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList1" CssClass="form-control form-control-sm" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Posting Date</td>
                                        <td>
                                            <asp:TextBox ID="txtTblPostingDate" TextMode="Date" CssClass="form-control form-control-sm" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td>Tracking ID</td>
                                        <td>
                                            <asp:TextBox ID="txtTblTrackingID" CssClass="form-control form-control-sm" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-6">
                                <h5 class="mt-2">Options</h5>
                                <table class="form-table">
                                    <tr>
                                        <td>Explode lines</td>
                                        <td>
                                            <asp:CheckBox ID="chlTblExplodelines" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td>Print Transfer Shipment</td>
                                        <td>
                                            <asp:CheckBox ID="chkTblprintTS" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td>Print Product Labels</td>
                                        <td>
                                            <asp:CheckBox ID="chkTblprintPL" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td>Print Shelf Labels</td>
                                        <td>
                                            <asp:CheckBox ID="chkTblprintSL" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="shipmentpanel">
                <ul class="nav nav-tabs" id="linesTabs" role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" id="lines-tab" data-toggle="tab" href="#lines-tab-content" role="tab">Lines</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="lines-general-tab" data-toggle="tab" href="#lines-general-tab-content" role="tab">General</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="lines-dimension-tab" data-toggle="tab" href="#lines-dimension-tab-content" role="tab">Dimension</a>
                    </li>
                </ul>

                <div class="tab-content">
                    <div class="tab-pane fade show active table-responsive" id="lines-tab-content" role="tabpanel">
                        <asp:GridView ID="gridView1" runat="server" CssClass="table table-condensed no-border table-hover sortable"
                            AutoGenerateColumns="false" DataKeyNames="RecId" ShowHeaderWhenEmpty="true"
                            EmptyDataText="<div class='empty-state'>No Records to show here.</div>">
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SelectSingle" AutoPostBack="true" OnCheckedChanged="chk_updateDimensions" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Item number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemNumber" runat="server" Text='<%# Bind("Itemid") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ItemName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Receive quantity">
                                    <ItemTemplate>
                                        <%--<asp:TextBox ID="lblRecieveQty" runat="server" Text='<%# Convert.ToDecimal(Eval("Qtyshipped")) - Convert.ToDecimal(Eval("QtyRecieved")) %>' />--%>
                                        <asp:TextBox ID="lblRecieveQty" runat="server" Text='<%# Convert.ToDecimal(Eval("QtyTransfer")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Scrap quantity">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblScrapQty" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CW recieve now">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCWRecieveNow" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CW scrap now">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCWScrapNow" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Close">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="lblgridClose" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TransferId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferId" runat="server" Text='<%# Bind("TransferId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DataAreaId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDataAreaId" runat="server" Text='<%# Bind("DataAreaId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ItemName" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LinesShipDate" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinesShipDate" runat="server" Text='<%# Bind("LinesShipDate") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LinesReceiveDate" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinesReceiveDate" runat="server" Text='<%# Bind("LinesReceiveDate") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ReserveItem" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReserveItem" runat="server" Text='<%# Bind("ReserveItem") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qtyshipped" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyshipped" runat="server" Text='<%# Bind("Qtyshipped") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dimensionshipfrom" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDimensionshipfrom" runat="server" Text='<%# Bind("Dimensionshipfrom") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dimensionshipto" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDimensionshipto" runat="server" Text='<%# Bind("Dimensionshipto") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TransctionCode" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransctionCode" runat="server" Text='<%# Bind("TransctionCode") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IsCatchWeight" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsCatchWeight" runat="server" Text='<%# Bind("IsCatchWeight") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventDimId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventDimId" runat="server" Text='<%# Bind("InventDimId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LocationIdFrom" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationIdFrom" runat="server" Text='<%# Bind("LocationIdFrom") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LocationIdTo" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationIdTo" runat="server" Text='<%# Bind("LocationIdTo") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CWQtyTransfer" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCWQtyTransfer" runat="server" Text='<%# Bind("CWQtyTransfer") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TransferStatus" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferStatus" runat="server" Text='<%# Bind("inventtransferstatus") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventBatchId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventBatchId" runat="server" Text='<%# Bind("InventBatchId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RecId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="WMSLocationId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWMSLocationId" runat="server" Text='<%# Bind("WMSLocationId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="WMSPalletId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWMSPalletId" runat="server" Text='<%# Bind("WMSPalletId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventSerialId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventSerialId" runat="server" Text='<%# Bind("InventSerialId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventLocationId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventLocationId" runat="server" Text='<%# Bind("InventLocationId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ConfigId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblConfigId" runat="server" Text='<%# Bind("ConfigId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventSizeId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventSizeId" runat="server" Text='<%# Bind("InventSizeId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventColorId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventColorId" runat="server" Text='<%# Bind("InventColorId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventStyle" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventStyle" runat="server" Text='<%# Bind("InventStyle") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InventSiteId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventSiteId" runat="server" Text='<%# Bind("InventSiteId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NewTransitLocationName" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewTransitLocationName" runat="server" Text='<%# Bind("NewTransitLocationName") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CreatedDateTime" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedDateTime" runat="server" Text='<%# Bind("CreatedDateTime") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UnitId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitId" runat="server" Text='<%# Bind("UnitId") %>' /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LineNum" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineNum" runat="server" Text='<%# Bind("LineNum") %>' /></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>


                    </div>

                    <div class="tab-pane fade" id="lines-general-tab-content" role="tabpanel">
                        <div class="row">
                            <div class="col-md-6">
                                <h5 class="mt-2">Transfer Order Line</h5>
                                <table class="form-table">
                                    <tr>
                                        <td><span>Line Number</span></td>
                                        <td>
                                            <asp:TextBox ID="txtLineNum" runat="server" Enabled="false" CssClass="form-control form-control-sm" /></td>
                                    </tr>
                                    <tr>
                                        <td><span>Transfer Number</span></td>
                                        <td>
                                            <asp:TextBox ID="txtTransferNum" runat="server" Enabled="false" CssClass="form-control form-control-sm" /></td>
                                    </tr>
                                    <tr>
                                        <td><span>Item Number</span></td>
                                        <td>
                                            <asp:TextBox ID="txtItemNumber" runat="server" Enabled="false" CssClass="form-control form-control-sm" /></td>
                                    </tr>
                                </table>
                            </div>

                            <div class="col-md-6">
                                <h5 class="mt-2">Receipt Line</h5>
                                <table class="form-table">
                                    <tr>
                                        <td><span>Receive Quantity</span></td>
                                        <td>
                                            <asp:TextBox ID="txtRecieveNow" runat="server" CssClass="form-control form-control-sm" /></td>
                                    </tr>
                                    <tr>
                                        <td><span>CW Recieve Now</span></td>
                                        <td>
                                            <asp:TextBox ID="txtCWShipNow" runat="server" Enabled="false" CssClass="form-control form-control-sm" /></td>
                                    </tr>
                                    <tr>
                                        <td><span>CW Scrap Now</span></td>
                                        <td>
                                            <asp:TextBox ID="txtCWScrapNow" runat="server" Enabled="false" CssClass="form-control form-control-sm" /></td>
                                    </tr>
                                    <tr>
                                        <td><span>Close</span></td>
                                        <td>
                                            <asp:CheckBox ID="chkClose" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane fade" id="lines-dimension-tab-content" role="tabpanel">
                        <h5 class="mb-3">Inventory Dimensions</h5>
                        <table class="form-table" style="width: 100%;">
                            <tr>
                                <td>Configuration</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtInvConfig" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>Size</td>
                                <td>
                                    <asp:TextBox ID="txtInvSize" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                                <td>Style</td>
                                <td>
                                    <asp:TextBox ID="txtInvStyle" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                                <td>Warehouse</td>
                                <td>
                                    <asp:TextBox ID="txtInvWarehouse" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>Location</td>
                                <td>
                                    <asp:TextBox ID="txtInvLocation" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                                <td>Inventory status</td>
                                <td>
                                    <asp:TextBox ID="txtInvStatus" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                                <td>License plate</td>
                                <td>
                                    <asp:TextBox ID="txtInvLicensePlate" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>Color</td>
                                <td>
                                    <asp:TextBox ID="txtInvColor" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                                <td>Site</td>
                                <td>
                                    <asp:TextBox ID="txtInvSite" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                                <td>Batch number</td>
                                <td>
                                    <asp:TextBox ID="txtInvBatch" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>Serial number</td>
                                <td>
                                    <asp:TextBox ID="txtInvSerial" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                                <td>Owner</td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtInvOwner" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>


            <div class="action-footer">
                <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">OK</asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="return closeDialog();">Cancel</asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
