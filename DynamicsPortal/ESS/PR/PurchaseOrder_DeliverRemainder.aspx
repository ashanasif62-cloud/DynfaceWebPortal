<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true"
    CodeBehind="PurchaseOrder_DeliverRemainder.aspx.cs"
    Inherits="DynamicsPortal.ESS.PR.PO_DeliverRemainder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .form-table {
            width: 100%;
            max-width: 500px;
            border-collapse: collapse;
            margin: 40px auto;
        }

        .form-table td {
            padding: 10px 5px;
            vertical-align: middle;
        }

        .form-table span {
            font-weight: 600;
            font-size: 13px;
            text-transform: uppercase;
            color: #000;
        }

        .form-table input[type="text"] {
            text-align: right;
            padding: 5px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .deliver-header {
            font-weight: bold;
            text-transform: uppercase;
            text-align: left;
            padding-bottom: 8px;
            font-size: 10px;
        }

        .action-footer {
            display: flex;
            justify-content: right;
            gap: 10px;
            margin-top: 40px;
        }

        .action-footer a {
            text-decoration: none;
        }

        .action-footer .aspNetLinkButton, .action-footer a {
            padding: 8px 16px;
            border-radius: 6px;
            font-weight: 600;
        }

        .action-footer #btnSave {
            background-color: #0078d7;
            color: white;
        }

        .action-footer #btnCancel_Quantity,
        .action-footer #btnCancel {
            background-color: white;
            color: #000;
            border: 1px solid #ccc;
        }
    </style>
      <script type="text/javascript">
        // ✅ Automatically sync quantities at runtime
        function syncQuantities(source) {
            var inventoryQty = document.getElementById('<%= txtInventoryQuantity.ClientID %>');
            inventoryQty.value = source.value;
          
        }
      </script>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <tr>
            <td></td>
            <td class="deliver-header">Deliver Remainder</td>
        </tr>                                                                                                                                                                                                                                                                   

        <tr>
            <td><span>Purchase Quantity</span></td>
            <td>
                  <asp:TextBox ID="txtPurchaseQuantity" runat="server" Width="100"
                    onkeyup="syncQuantities(this)" oninput="syncQuantities(this)"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td><span>Inventory Quantity</span></td>
            <td>
                <asp:TextBox ID="txtInventoryQuantity" runat="server" Enabled="true" Width="100"></asp:TextBox>
            </td>
        </tr>
    </table>

    <div class="action-footer">
   <asp:LinkButton ID="btnSave" runat="server" OnClientClick="javascript: return closeDialog();">OK</asp:LinkButton>
        <asp:LinkButton ID="btnCancel_Quantity" runat="server" OnClick="btnCancel_Quantity_Click">Cancel quantity</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
