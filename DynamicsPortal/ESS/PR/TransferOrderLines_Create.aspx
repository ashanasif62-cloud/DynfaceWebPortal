<%@ Page Title="ADD Lines" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="TransferOrderLines_Create.aspx.cs" Inherits="DynamicsPortal.TransferOrderLines_Create" %>
<%@ Register Src="~/DropDownList_ProductCombination.ascx" TagPrefix="uc" TagName="ProductLookup" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Transfer Order Create</title>
      <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
  <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <tr>
    <td>
        <span>Transfer Number</span>
    </td>
    <td>
        <asp:TextBox ID="txtTransferId" runat="server" ReadOnly="true"></asp:TextBox>
    </td>
</tr>
        <tr>
            <tr>
       <tr>
        <td><span>Item Number <span style="color:red">*</span> </span></td>
        <td>
         <asp:DropDownList ID="ddlItemid" runat="server" AutoPostBack="true"  CssClass="filterable-dropdown" OnSelectedIndexChanged="ddlItemid_SelectedIndexChanged" />
     </td>
<tr>
    <td>
        <span>Transfer Quantity <span style="color:red">*</span> </span>
    </td>
    <td>
        <asp:TextBox ID="txtQtyTransfer" runat="server" TextMode="Number"></asp:TextBox>
    </td>
</tr>
         <tr>
        <td colspan="2" style="font-weight: bold; font-size: small;">Inventory Selection Details</td>
    </tr>
        <tr id="rowCombination" runat="server">
            <td><span>Combinations</span></td>
            <td>
                <div>
                    <uc:productlookup id="ProductLookupControl" runat="server" onproductselected="ProductLookupControl_ProductSelected" />
                </div>
            </td>
        </tr>
         <tr>
        <td><span>Batch Number</span></td>
        <td><asp:DropDownList ID="ddlInventBatchId" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" /></td>
    </tr>
    <tr id="rowWmsLocation" runat="server">
        <td><span>WMS Location</span></td>
        <td><asp:DropDownList ID="ddlWmsLocationId" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" /></td>
    </tr>
    <tr id="rowWmsPallet" runat="server">
        <td><span>WMS Pallet</span></td>
        <td><asp:DropDownList ID="ddlWmsPalletId" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" /></td>
    </tr>
    <tr id="rowInventSerialId" runat="server">
        <td><span>Serial Number</span></td>
        <td><asp:DropDownList ID="ddlInventSerialId" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" /></td>
    </tr>
   <%-- <tr>
        <td><span>Invent Location ID</span></td>
        <td><asp:DropDownList ID="ddlInventLocationId" runat="server" AutoPostBack="false" /></td>
    </tr>--%>
    <tr id="rowConfigid" runat="server">
        <td><span>Configuration</span></td>
        <td><asp:DropDownList ID="ddlConfigId" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" /></td>
    </tr>
    <tr  id="rowSizeid" runat="server">
        <td><span>Size</span></td>
        <td><asp:DropDownList ID="ddlInventSizeId" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" /></td>
    </tr>
    <tr  id="rowColorid" runat="server">
        <td><span>Color</span></td>
        <td><asp:DropDownList ID="ddlInventColorId" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" /></td>
    </tr>
    <tr  id="rowStyle" runat="server">
        <td><span>Style</span></td>
        <td><asp:DropDownList ID="ddlInventStyle" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" /></td>
    </tr>
    <%--<tr>
        <td><span>Site ID</span></td>
        <td><asp:DropDownList ID="ddlInventSiteId" runat="server" AutoPostBack="false" /></td>
    </tr>--%>
<%--<tr>
    <td>
        <span>Shipping Date</span>
    </td>
    <td>
        <asp:TextBox ID="txtLinesShipDate" runat="server" TextMode="Date"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        <span>Receiving Date</span>
    </td>
    <td>
        <asp:TextBox ID="txtLinesReceiveDate" runat="server" TextMode="Date"></asp:TextBox>
    </td>
</tr>--%>
<%--<tr>
    <td>
        <span>Is Catch Weight</span>
    </td>
    <td>
        <asp:TextBox ID="txtIsCatchWeight" runat="server" ReadOnly="true"></asp:TextBox>
    </td>
</tr>--%>
<%--<tr>
    <td>
        <span>Reserve Item</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlReserveItem" runat="server">
            <asp:ListItem Text="--Select--" Value="" />
            <asp:ListItem Text="Yes" Value="Yes" />
            <asp:ListItem Text="No" Value="No" />
        </asp:DropDownList>
    </td>
</tr>--%>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="showOverlay()" OnClick="btnSave_Click">Add</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
    <div id="messageContainer" runat="server" class="alert alert-success" visible="false">
    <asp:Label ID="lblMessage" runat="server" />
</div>
      <!-- Hidden field for InventDimId -->
    <asp:HiddenField ID="hdnInventDimId" runat="server" />
    <asp:HiddenField ID="hiddenfield" runat="server" />
        <script>
        function applyAutocomplete() {
            $('.filterable-dropdown').each(function () {
                var $dropdown = $(this);
 
                // Avoid adding input twice
                if ($dropdown.next('.linesautocomplete-input').length === 0) {
                    var options = [];
 
                    $dropdown.find('option').each(function () {
                        if ($(this).val()) { // Skip empty option in autocomplete list
                            options.push({
                                label: $(this).text(),
                                value: $(this).val()
                            });
                        }
                    });
 
                    // Determine placeholder based on dropdown ID
                    var placeholderText = '-- Select --';
                    var dropdownId = $dropdown.attr('id')?.toLowerCase();
 
                    switch (dropdownId) {
                        case "ddlUserId":
                            placeholderText = '-- Select User --';
                            break;
                        case "ddlRoleId":
                            placeholderText = '-- Select Role --';
                            break;
                    }
 
                    var $input = $('<input type="text" class="linesautocomplete-input form-control" />')
                        .attr('placeholder', placeholderText)
                        .val($dropdown.find("option:selected").text())
                        .insertAfter($dropdown)
                        .autocomplete({
                            source: options,
                            minLength: 0,
                            select: function (event, ui) {
                                // Set the value in the original dropdown
                                $dropdown.val(ui.item.value);

                                // Update input text
                                $(this).val(ui.item.label);

                                // Trigger ASP.NET postback
                                setTimeout(function () {
                                    __doPostBack($dropdown.attr('name'), '');
                                }, 100);
                            }
                        })
                        .on("focus", function () {
                            $(this).autocomplete("search", ""); // Show all options on focus
                        });
 
                    $dropdown.hide();
                }
            });
        }
 
        $(document).ready(function () {
            applyAutocomplete();
 
            if (Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(applyAutocomplete);
            }
        });
        </script>
 
    <style>
       
 
       
 
        /*.ui-autocomplete {
            z-index: 99999 !important;
            max-height: 200px;
            overflow-y: auto;
            background-color: white;
            border: 1px solid #ccc;
        }*/
 
        /* Dropdown suggestion box */
        /*.ui-autocomplete {
            z-index: 99999 !important;
            max-height: 200px;
            overflow-y: auto;
            background-color: white;
            border: 1px solid #ccc;
            font-family: Arial, sans-serif;
            font-size: 10px;
        }*/
</style>
</asp:Content>


