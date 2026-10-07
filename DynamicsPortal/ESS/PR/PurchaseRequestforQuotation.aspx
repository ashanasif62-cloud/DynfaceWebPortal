<%@ Page Title="Create Request for Quotation" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="PurchaseRequestforQuotation.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseRequestforQuotation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <div class="container-fluid mt-3">
        <div class="section-title">Select the lines to copy to the request for quotation</div>

        <!-- Grid -->
       <asp:GridView ID="gridView" runat="server"  Data="searchable" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" DataKeyNames="RecId" EmptyDataText="No Record Found." 
             AutoGenerateColumns="false">
            <Columns>
           <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
     <HeaderTemplate>
         <input type="checkbox" ID="chk_SelectAll" CssClass="round-checkbox"/>
     </HeaderTemplate>
     <ItemTemplate>
         <asp:CheckBox ID="chk_SelectSingle"  runat="server" CssClass="round-checkbox" />
     </ItemTemplate>
 </asp:TemplateField>
                <asp:TemplateField HeaderText="Purchase Requisition">
    <ItemTemplate>
        <asp:Label ID="lblPurchaseRequisition" runat="server" Text='<%# Eval("PurchaseRequisition") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Item Number">
    <ItemTemplate>
        <asp:Label ID="lblItemNumber" runat="server" Text='<%# Eval("ItemNumber") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Product Name">
    <ItemTemplate>
        <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Buying Legal Entity">
    <ItemTemplate>
        <asp:Label ID="lblBuyingLegalEntity" runat="server" Text='<%# Eval("BuyingLegalEntity") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Record ID">
    <ItemTemplate>
        <asp:Label ID="lblRecId" runat="server" Text='<%# Eval("RecId") %>' />
    </ItemTemplate>
</asp:TemplateField>
            </Columns>
        </asp:GridView>
             <div class="action-footer">
        <asp:LinkButton ID="btnOK" runat="server" OnClientClick="showOverlay();" OnClick="btnOk_Click">OK</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
    </div>
</asp:Content>
