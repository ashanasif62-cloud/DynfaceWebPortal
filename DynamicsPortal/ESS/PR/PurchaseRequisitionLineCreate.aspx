<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="PurchaseRequisitionLineCreate.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseRequisitionLineCreate" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <tr>
            <td>
                <span>Requester</span>
            </td>

           <td>
    <asp:TextBox ID="txtRequisitioner" runat="server" ReadOnly="true"></asp:TextBox>
</td>
        </tr>
       <%-- <tr>
            <td>
                <span>Buying Legal Entity</span>
            </td>

            <td>
                <asp:TextBox ID="txtBuyingLegalEntity" runat="server" autocomplete="off" masktype="date"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
    <td>
        <span>Item Number</span>
    </td>

    <td>
        <asp:DropDownList ID="ddlItemId" runat="server" AutoPostBack="false" />
    </td>
</tr>
        <tr>
            <td>
                <span>Receiving Business Unit</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>Vendor Account Number</span>
            </td>

            <td>
                <%--<asp:DropDownList ID="ddlvendoraccnum" runat="server" OnSelectedIndexChanged="ddlvendoraccnum_Changed"></asp:DropDownList>--%>
                <asp:DropDownList ID="ddlvendoraccnum" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlvendoraccnum_Changed"></asp:DropDownList>
            </td>
        </tr>
        
        <tr>
            <td>
                <span>Vendor Name</span>
            </td>

            <td>
                <asp:textbox ID="txtvendorname" runat="server"></asp:textbox>
            </td>
        </tr>
        
        <tr>
            <td>
                <span>Unit of Measure</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlUnitofMeasure" runat="server"></asp:DropDownList>
            </td>
        </tr>
               
        <tr>
    <td>
        <span>Quantity</span>
    </td>

    <td>
          <asp:TextBox ID="txtPurchQty" runat="server" Enabled="true"></asp:TextBox>
        
    </td>
</tr>
                  
        <tr>
    <td>
        <span>DataArea Id</span>
    </td>

    <td>
          <asp:TextBox ID="txtdataAreaId" runat="server" Enabled="true"></asp:TextBox>
        
    </td>
</tr>
      
      <%--  <tr>
            <td>
                <span>Currency</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlCurrencyCode" runat="server" Enabled="false"></asp:DropDownList>
            </td>
        </tr>--%>
   <%--     
        <tr>
    <td>
        <span>Status</span>
    </td>

    <td>
        <asp:TextBox ID="RequisitionStatus" runat="server" autocomplete="off" masktype="date"></asp:TextBox>
    </td>
</tr>--%>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="showOverlay();" OnClick="btnSave_Click">Create</asp:LinkButton>
       <%--<asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>--%>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>


