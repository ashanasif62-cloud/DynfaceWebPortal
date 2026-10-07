<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="PurchaseOrderLines_MaintainCharges.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseOrder_MaintainCharges_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .grid-size {
            max-width: max-content;
            min-width: max-content;
        }
    .label-box {
    border: 1px solid #7F9DB9;
    background-color: #EAF3FF;
    height: 25px;
    width: 25px;
    border-radius: 2px;
    display: flex;
    align-items: center;
    justify-content: center;
}
.label-box::before {
    content: "";
    width: 10px;
    height: 10px;
    border: 1px solid #7F9DB9;
    background-color: white;
    display: block;
}

.form-control {
    width: 100%;
    height: 30px;
    border-radius: 5px;
    border: 1px solid #ccc;
    padding: 3px 8px;
    background-color: #f9f9f9;
}

              /* text color */
    </style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
      <%-- <div class="action-items">
       <a href="/ESS/PR/PurchaseOrderLines_ListPage.aspx" class="btn-link">
            <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
       </a>
   </div>--%>
<div class="action-items">
    <asp:LinkButton ID="btnBack" runat="server" OnClick="btnBack_Click">
        <i class='mdi mdi-arrow-left' style='margin-right:4px;'></i>Back
    </asp:LinkButton>
</div>

 
    <div class="action-items">
    <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_Click">
        <i class="mdi mdi-plus"></i> New
    </asp:LinkButton>
</div>


    <asp:HiddenField ID="hdnSelectedRecId" runat="server" />

    <asp:HiddenField ID="HiddenField1" runat="server" />
 <div class="action-items">

    <asp:LinkButton 
        ID="btnSave" 
        runat="server" 
        OnClick="btnSave_Click">
        <i class="mdi mdi-content-save"></i> Save
    </asp:LinkButton>
</div>



   
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server"  OnClick="btnDelete_Click" >
            <i class="mdi mdi-delete"></i>Delete
        </asp:LinkButton>
    </div>

  
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">

    <div>
      <asp:GridView ID="gridView" runat="server" CssClass="table table-condensed no-border table-hover sortable grid-size"
    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
    AutoGenerateColumns="False" OnRowDataBound="gridView_RowDataBound">

    <Columns>
        <asp:TemplateField HeaderText="RecId" Visible="false">
    <ItemTemplate>
        <asp:HiddenField ID="hdnSelectedRecId" runat="server" Value='<%# Eval("RecId") %>' />
    </ItemTemplate>
</asp:TemplateField>

     
        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
            <ItemTemplate>
                <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true" runat="server" CssClass="round-checkbox" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Charges code">
            <ItemTemplate>
                <asp:DropDownList 
                    ID="ddlChargesCode" 
                    runat="server" 
                    CssClass="form-control"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlChargesCode_SelectedIndexChanged">
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Text='<%# Bind("Txt") %>'></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>

    <%--    <asp:TemplateField HeaderText="Category">
            <ItemTemplate>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Category">
    <ItemTemplate>
        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"
            AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
        </asp:DropDownList>
    </ItemTemplate>
</asp:TemplateField>

        <asp:TemplateField HeaderText="Unit" Visible="false" >
            <ItemTemplate>
                <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" ></asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>

<%--        <asp:TemplateField HeaderText="Charges Value">
            <ItemTemplate>
                <asp:Label ID="txtValue" runat="server" CssClass="form-control text-right"  Text='<%# Bind("Value") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Charges value">
    <ItemTemplate>
        <asp:TextBox ID="lblValue" runat="server" 
            Text='<%# Bind("Value", "{0:F2}") %>' 
            CssClass="form-control text-right"></asp:TextBox>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtValue" runat="server" 
            Text='<%# Bind("Value", "{0:F2}") %>' 
            CssClass="form-control text-right"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>


        <asp:TemplateField HeaderText="Allow edit">
            <ItemTemplate>
                <asp:TextBox ID="txtAllowEdit" runat="server" CssClass="form-control" Text='<%# (Eval("AllowEdit").ToString().ToLower() == "true") ? "Yes" : "No" %>' ReadOnly="true"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Currency">
            <ItemTemplate>
              <%--  <asp:TextBox ID="txtCurrencyCode" runat="server" CssClass="form-control" Text='<%# Bind("CurrencyCode") %>' ReadOnly="true"></asp:TextBox>--%>
             <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control"></asp:DropDownList>
                </ItemTemplate>
        </asp:TemplateField>
<asp:TemplateField HeaderText="Calculated amount">
    <ItemTemplate>
        <asp:TextBox ID="txtCalculatedAmount" runat="server"
            CssClass="form-control text-right"
            Text='<%# Eval("CalculatedAmount", "{0:F2}") %>'>
        </asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

  <asp:TemplateField HeaderText="Broker Contract Fee">
    <ItemTemplate>
        <asp:CheckBox 
            ID="chkBrokerContractFee" 
            runat="server" 
            Checked='<%# Eval("MCRBrokerContractFee").ToString() == "Yes" %>' 
            Enabled="false" />
    </ItemTemplate>
</asp:TemplateField>



        <asp:TemplateField HeaderText="Sales Tax Group" Visible="false">
            <ItemTemplate>
                <asp:DropDownList ID="ddlSalesTaxGroup" runat="server" CssClass="form-control"></asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Item Sales Tax Group" Visible="false">
            <ItemTemplate>
                <asp:DropDownList ID="ddlItemSalesTaxGroup" runat="server" CssClass="form-control"></asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="RecId" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
      <ItemTemplate>
                <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" Visible="false" />
            <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click"  />
            </ItemTemplate>

             <EditItemTemplate>
          <%--  <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />--%>
      </EditItemTemplate>
</asp:TemplateField>
          <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
      <ItemTemplate>
          <asp:LinkButton ID="btnEdit" CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" Visible="false" />
      </ItemTemplate>
      <EditItemTemplate>
      </EditItemTemplate>
  </asp:TemplateField>
    </Columns>
</asp:GridView>

    </div>
 
<script>
    function handleBackClick() {
        var ref = document.referrer.toLowerCase();

        // 🔥 Correct page name based on your aspx declaration
        if (ref.includes('allpurchaseorder_listpage.aspx')) {
            window.location.href = '/ESS/PR/AllPurchaseOrder_ListPage.aspx';
        }
       
        else {
            // default fallback
            window.location.href = '/ESS/PR/PurchaseOrderLines_ListPage.aspx';
        }
    }
</script>

  
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
</asp:Content>
