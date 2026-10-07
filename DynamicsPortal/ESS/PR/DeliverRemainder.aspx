<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="DeliverRemainder.aspx.cs" Inherits="DynamicsPortal.ESS.PR.DeliverRemainder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
<ContentTemplate>
    <p style="font-size: 12px;">
    You can update remaining delivery quantity on the line. 
    To cancel remaining quantity set SHIP REMAIN to 0. 
    Changing remaining quantity introduces underdelivery or overdelivery situation.
</p>
    

    <table class="form-table">


     
     <tr>
     <td>
         <span><strong>TRANSFER QUANTITY</strong></span>
     </td>

     <td>
         <asp:TextBox ID="lblTransferQuantity" runat="server" Text='<%# Bind("QtyTransfer") %>' Enabled="false" Width="80" ></asp:TextBox>
     </td>
 </tr>

     <tr>
    <td>
        <span><strong>QUANTITY</strong> </span>
    </td>

    <td>
        <asp:TextBox ID="lblShipQuantity" runat="server" Text='<%# Bind("Qtyshipped") %>' Enabled="false" Width="80"></asp:TextBox>
    </td>
</tr>
        <tr>
    <td>
        <span>Current</span>
    </td>
</tr>
         <tr>
    <td>
        <span><strong>SHIP REMAIN</strong></span>
    </td>

    <td>
        <asp:TextBox ID="lblShipRemain" runat="server" Text='<%# Bind("QtyRemainShip") %>' Enabled="false" Width="80"></asp:TextBox>
    </td>
</tr>
        <%--    <tr>
    <td>
        <span><strong>OVERDELIVERY</strong></span>
    </td>

    <td>
        <asp:TextBox ID="lblOverDelivery" runat="server" Enabled="false" Width="80" Visible="false"></asp:TextBox>
    </td>
</tr>--%>
           <%-- <tr>
    <td>
        <span><strong>UNDERDELIVERY</strong></span>
    </td>

    <td>
        <asp:TextBox ID="lblUnderDelivery" runat="server" Enabled="false" Width="80" Visible="false"></asp:TextBox>
    </td>
</tr>--%>
            <tr>
    <td>
        <span>New</span>
    </td>
</tr>
       
        <tr>
    <td>
        <span><strong>SHIP REMAIN</strong></span>
    </td>

    <td>
        <asp:TextBox ID="lblShipRemain1" runat="server" Text='<%# Bind("QtyRemainShip") %>' Enabled="true" Width="80"></asp:TextBox>
    </td>
</tr>

            <tr>
    <td>
        <span><strong>TOTAL SHIPMENT</strong></span>
    </td>

    <td>
        <asp:TextBox ID="lblTotalShipment" runat="server"  Text='<%# Bind("QtyTransfer") %>'  Enabled="false" Width="80"></asp:TextBox>
    </td>
</tr>
        </table>
        <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="showOverlay();" OnClick="btnSave_Click">OK</asp:LinkButton>
        <asp:LinkButton ID="btnCancel_Quantity" runat="server" OnClick="btnCancel_Quantity_Click">Cancel quantity</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
       
    </div>
            </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
