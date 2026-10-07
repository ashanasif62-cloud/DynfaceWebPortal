<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="PurchaseRequisition_Create.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchasedRequisition_Create" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- jQuery UI CSS -->
<link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">

<!-- jQuery + jQuery UI JS -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>


   


<!-- Initialize the DatePicker -->
<script>
    $(function () {
        $(".datepicker").datepicker({
            dateFormat: "m-d-yy" // Example: 9-12-2025
        });
    });
</script>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
     
     <tr>
     <td>
         <span>Purchase Requisition</span>
     </td>

     <td>
         <asp:TextBox ID="txtPurchReqId" runat="server" Enabled="false" Width="50"></asp:TextBox>
     </td>
 </tr>

          
     <tr>
     <td>
         <span>Name</span>
     </td>

     <td>
         <asp:TextBox ID="PurchReqName" runat="server" Enabled="true" MaxLength="60"></asp:TextBox>
     </td>
 </tr>

        
        <tr>
            <td>
                <span> Requested Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestedDate" runat="server" CssClass="datepicker"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Accounting Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtAccountingDate" runat="server" CssClass="datepicker" ></asp:TextBox>
            </td>
        </tr>
      <asp:Label ID="lblMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>  
  

    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="showOverlay();" OnClick="btnSave_Click">Create</asp:LinkButton>
<%--        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>--%>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>

