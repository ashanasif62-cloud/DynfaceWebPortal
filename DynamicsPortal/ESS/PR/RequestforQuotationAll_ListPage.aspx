<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RequestforQuotationAll_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.PR.RequestforQuotationAll_ListPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <style>
/* Round checkbox styling */
.round-checkbox input[type="checkbox"],
input#chk_SelectAll {
  appearance: none;
  -webkit-appearance: none;
  -moz-appearance: none;
  width: 18px;
  height: 18px;
  border: 2px solid #333;
  border-radius: 50%; /* Creates round shape */
  background-color: #fff;
  cursor: pointer;
  position: relative;
  outline: none; /* Removes default focus outline */
}

/* Checked state with blue background */
.round-checkbox input[type="checkbox"]:checked,
input#chk_SelectAll:checked {
  background-color: #007BFF; /* Blue background */
  border-color: #007BFF;
}

/* Focus state for accessibility */
.round-checkbox input[type="checkbox"]:focus,
input#chk_SelectAll:focus {
  box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.5); /* Blue focus ring */
}
</style>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." 
             OnRowDataBound="gridView_RowDataBound"  AutoGenerateColumns="false">

            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" CssClass="round-checkbox"  />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" CssClass="round-checkbox"  />
                    </ItemTemplate>
                </asp:TemplateField>

<%--                <asp:TemplateField HeaderText="Purchase Requisition">
    <ItemTemplate>
        <asp:LinkButton 
            ID="lnkPurchReqId" 
            runat="server" 
            Text='<%# Eval("PurchReqId") %>' 
            CommandArgument='<%# Eval("PurchReqId") %>' 
            OnClick="lnkPurchReqId_Click" />
    </ItemTemplate>
</asp:TemplateField>--%>

              
        <asp:TemplateField HeaderText="RFQ Case ID">
            <ItemTemplate>
                <asp:Label ID="lblRFQCaseId" runat="server" Text='<%# Bind("RFQCaseId") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

       
        <asp:TemplateField HeaderText="RFQ Type">
            <ItemTemplate>
                <asp:Label ID="lblRFQType" runat="server" Text='<%# Bind("RFQType") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

       
        <asp:TemplateField HeaderText="Document Title">
            <ItemTemplate>
                <asp:Label ID="lblDocumentTitle" runat="server" Text='<%# Bind("DocumentTitle") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

   
        <asp:TemplateField HeaderText="Solicitation Type">
            <ItemTemplate>
                <asp:Label ID="lblSolicitationType" runat="server" Text='<%# Bind("RFQSolicitationType") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Bid Type">
            <ItemTemplate>
                <asp:Label ID="lblBidType" runat="server" Text='<%# Bind("BidType") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

       
        <asp:TemplateField HeaderText="Requester Name">
            <ItemTemplate>
                <asp:Label ID="lblRequesterName" runat="server" Text='<%# Bind("RequesterName") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        
        <asp:TemplateField HeaderText="Status Low">
            <ItemTemplate>
                <asp:Label ID="lblStatusLow" runat="server" Text='<%# Bind("StatusLow") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Status High">
            <ItemTemplate>
                <asp:Label ID="lblStatusHigh" runat="server" Text='<%# Bind("StatusHigh") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

       
        <asp:TemplateField HeaderText="Vendor Status">
            <ItemTemplate>
                <asp:Label ID="lblVendorStatus" runat="server" Text='<%# Bind("VendorStatus") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

      
        <asp:TemplateField HeaderText="Expiry Date Time">
            <ItemTemplate>
                <asp:Label ID="lblExpiryDateTime" runat="server" Text='<%# Bind("ExpiryDateTime") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

     
<%--        <asp:TemplateField HeaderText="RecId" Visible="false">
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>--%>

               <%-- <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
                <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
                <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false" />--%>
        <%--        <asp:TemplateField HeaderText="RecId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

