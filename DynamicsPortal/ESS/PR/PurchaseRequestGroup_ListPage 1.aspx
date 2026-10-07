<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseRequestGroup_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseRequestGroup_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/PurchasedRequisition_Create.aspx')"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
    </div>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gridView" runat="server" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound"
            OnRowCommand="gridView_RowCommand" AutoGenerateColumns="false">

            <Columns>
                <asp:TemplateField HeaderText="">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Purchase rder">
                    <ItemTemplate><asp:Label ID="lblPurchaseOrder" runat="server" Text='<%# Bind("PurchaseOrder") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vender account">
                    <ItemTemplate><asp:Label ID="lblVenderAccount" runat="server" Text='<%# Bind("VenderAcount") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice account">
                    <ItemTemplate><asp:Label ID="lblInvoiceAccount" runat="server" Text='<%# Bind("InvoiceAccount") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vender name">
                    <ItemTemplate><asp:Label ID="lblVenderName" runat="server" Text='<%# Bind("VenderName") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Purchase type">
                    <ItemTemplate><asp:Label ID="lblPurchaseType" runat="server" Text='<%# Bind("PurchaseType") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Approval status">
                    <ItemTemplate><asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Purchase order status">
                    <ItemTemplate><asp:Label ID="lblPurchaseOrderStatus" runat="server" Text='<%# Bind("PurchaseOrderStatus") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Currency">
                    <ItemTemplate><asp:Label ID="lblCurrency" runat="server" Text='<%# Bind("Currency") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Request receipt date">
                    <ItemTemplate><asp:Label ID="lblRequestReceiptDate" runat="server" Text='<%# Bind("RequestReceiptDate") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mode of delivery">
                    <ItemTemplate><asp:Label ID="lblModeOfDelivery" runat="server" Text='<%# Bind("ModeOfDelivery") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delivery terms">
                    <ItemTemplate><asp:Label ID="lblDeliveryTerms" runat="server" Text='<%# Bind("DeliveryTerms") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Purchase agreement">
                    <ItemTemplate><asp:Label ID="lblPurchaseAgreement" runat="server" Text='<%# Bind("PurchaseAgreement") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Direct delivery">
                    <ItemTemplate><asp:Label ID="lblDirectDelivery" runat="server" Text='<%# Bind("DirectDelivery") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Project subcontract number">
                    <ItemTemplate><asp:Label ID="lblProjectSubcontractNumber" runat="server" Text='<%# Bind("ProjectSubContractNumber") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RecId" Visible="false">
                    <ItemTemplate><asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' /></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
