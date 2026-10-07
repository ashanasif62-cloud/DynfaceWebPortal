<%@ Page Title="Transfer Order's" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="TransferOrder_ListPage.aspx.cs"
    Inherits="DynamicsPortal.TransferOrder_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">




    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="updButtons" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="action-items">
                <asp:LinkButton ID="btnNew" runat="server"
                    OnClientClick="javascript: return openPopupPanel('/ESS/PR/TransferOrder_Create.aspx')">
                    <i class="mdi mdi-plus"></i> New
                </asp:LinkButton>
            </div>

            <div class="action-items">
                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click">
                    <i class="mdi mdi-delete"></i> Delete
                </asp:LinkButton>
            </div>

         
            <div class="action-items dropdown">
                <!-- Toggle (native button so Bootstrap handles it reliably) -->
                <button type="button" class="btn btn-primary dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="mdi mdi-truck-fast"></i> Ship
                </button>

             
                <ul class="dropdown-menu">
                    <li>
                        <asp:LinkButton ID="btnShip_All" runat="server" CssClass="dropdown-item"
                            OnClick="btnShip_Click"
                            OnClientClick="event.stopPropagation();">
                            <i class="mdi mdi-truck-fast"></i> Ship Transfer Order
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>

            <div class="action-items dropdown">

    <button type="button" class="btn btn-primary dropdown-toggle" 
            data-bs-toggle="dropdown" aria-expanded="false">
        <i class="mdi mdi-package-down"></i> Receive
    </button>

    <!-- Dropdown menu -->
    <ul class="dropdown-menu">
        <li>
            <asp:LinkButton ID="btnReceive" runat="server" CssClass="dropdown-item"
                OnClick="btnRecieve_Click"
                OnClientClick="event.stopPropagation();">
                <i class="mdi mdi-package-down"></i> Receive Transfer Order
            </asp:LinkButton>
        </li>
    </ul>
</div>



       <%--     <div class="action-items">
                <asp:LinkButton ID="btnReceive" OnClick="btnRecieve_Click" runat="server">
                    <i class="mdi mdi-package-down"></i> Receive
                </asp:LinkButton>
            </div>--%>

            <div class="action-items">
                <asp:LinkButton ID="btnHistory" runat="server" OnClick="btnHistory_Click">
                    <i class="mdi mdi-file-chart"></i> Transfer Order History
                </asp:LinkButton>
            </div>

            <div class="action-items">
                <asp:LinkButton ID="btnReport" runat="server" OnClientClick="window.location='/ESS/PR/TransferOrderOverViewReport.aspx'; return false;">
                    <i class="mdi mdi-file-chart"></i> Transfer Order Overview Report
                </asp:LinkButton>
            </div>

            <div class="action-items">
                <asp:LinkButton ID="btnReport2" runat="server" OnClientClick="window.location='/ESS/PR/TransferOnHandItemsInventoryReport.aspx'; return false;">
                    <i class="mdi mdi-barcode-scan"></i> On-hand Inventory List
                </asp:LinkButton>
            </div>

        </ContentTemplate>
      
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upGrid" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
        <ContentTemplate>
            <div>
                <asp:GridView ID="gridView" runat="server" OnRowCommand="gridView_RowCommand"
                    CssClass="table table-condensed no-border table-hover sortable"
                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
                    OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>
                                <!-- I recommend removing AutoPostBack on checkboxes unless you need instant postback -->
                                <asp:CheckBox ID="chk_SelectSingle" runat="server" CssClass="round-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Transfer number">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTransferID" runat="server"
                                    Text='<%# Eval("TransferID") %>'
                                    CommandName="TransferClick"
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    CssClass="link-style" />
                            </ItemTemplate>
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="Transfer status" SortExpression="Transfer Status">
                  <ItemTemplate>
                 <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("InventTransferStatus") %>' />
                  </ItemTemplate>
                   <EditItemTemplate>
                    <asp:Label ID="lblStatusEdit" runat="server" Text='<%# Bind("InventTransferStatus") %>' />
                </EditItemTemplate>
                 </asp:TemplateField>

                <asp:TemplateField HeaderText="From warehouse">
                    <ItemTemplate>
                        <asp:Label ID="lblFromWarehouse" runat="server" Text='<%# Bind("FromWarehouse") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="To warehouse">
                    <ItemTemplate>
                        <asp:Label ID="lblToWarehouse" runat="server" Text='<%# Bind("ToWarehouse") %>' />
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Ship date">
                    <ItemTemplate>
                        <asp:Label ID="lblShipDate" runat="server" Text='<%# Eval("ShipDate", "{0:M-dd-yyyy}") %>' />
                    </ItemTemplate>
                     <EditItemTemplate>
                        <asp:TextBox ID="txtShipDate" runat="server" Text='<%# Bind("ShipDate", "{0:M-dd-yyyy}") %>' TextMode="Date" autocomplete="off" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Receipt date">
                    <ItemTemplate>
                        <asp:Label ID="lblReceiveDate" runat="server" Text='<%# Eval("ReceiveDate", "{0:M-dd-yyyy}") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtReceiveDate" runat="server" Text='<%# Bind("ReceiveDate", "{0:M-dd-yyyy}") %>' TextMode="Date" autocomplete="off" />
                    </EditItemTemplate>
                </asp:TemplateField>

                
                <asp:TemplateField HeaderText="Created Date and Time" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblCreatedDateandTime" runat="server" 
            Text='<%# Bind("CreatedDateandTime", "{0:M-dd-yyyy hh:mm tt}") %>' />
    </ItemTemplate>
</asp:TemplateField>

                                                    <asp:TemplateField HeaderText="From Warehouse Name" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblFromWarehouseName" runat="server" Text='<%# Bind("FromWarehouseName") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="To Warehouse Name" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblToWarehouseName" runat="server" Text='<%# Bind("ToWarehouseName") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="From Address" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblFromAddress" runat="server" Text='<%# Bind("FromAddress") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="To Address" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblToAddress" runat="server" Text='<%# Bind("ToAddress") %>' />
    </ItemTemplate>
</asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                    </EditItemTemplate>
                </asp:TemplateField>

                     
                        <asp:BoundField DataField="RecId" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                        <asp:TemplateField HeaderText="RecId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(function () {
            showAJAXOverlay();
        });

        prm.add_endRequest(function () {
            hideAJAXOverlay();

     
            var list = [].slice.call(document.querySelectorAll('.dropdown-toggle'));
            list.map(function (el) {
                try { return new bootstrap.Dropdown(el); } catch (e) { }
            });
        });
    </script>
</asp:Content>
