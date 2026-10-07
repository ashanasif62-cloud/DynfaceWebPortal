<%@ Page Title="Create Transfer Order" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="TransferOrder_Create.aspx.cs" Inherits="DynamicsPortal.TransferOrder_Create" %>

<%@ Register Src="~/DropDownList_Warehouse.ascx" TagPrefix="uc1" TagName="DropDownList_Warehouse" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Transfer Order Create</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />

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
        <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
<ContentTemplate>
    <table class="form-table">
           <tr>
                <td><span>From warehouse <span style="color:red">*</span> </span></td>
                <td>
                 <%--<asp:DropDownList ID="ddlFromWarehouse" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" />--%>
                 <uc1:DropDownList_Warehouse ID="DropDownList_FromWearhouse" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" />
                </td>
        <tr>
            <td>
                <span>To warehouse <span style="color:red">*</span> </span>
            </td>
            <td>
                <%--<asp:DropDownList ID="ddlToWarehouse" runat="server" AutoPostBack="false"  CssClass="filterble-dropdown" />--%>
                <uc1:DropDownList_Warehouse ID="DropDownList_ToWearhouse" runat="server" AutoPostBack="false" CssClass="filterable-dropdown" />
            </td>
        </tr>
        <tr>
            <td>
                <span>Shipping date</span>
            </td>
            <td>
                <asp:TextBox ID="txtShippingDate" runat="server" CssClass="datepicker" ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Receipt date</span>
            </td>
            <td>
                <asp:TextBox ID="txtReceiptDate" runat="server" CssClass="datepicker"  ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>
    </table>
     <div class="action-footer">
     <asp:LinkButton ID="btnOk" runat="server" OnClientClick="showOverlay();" OnClick="btnOk_Click">Save</asp:LinkButton>
     <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
 </div>
          <script>
              function applyAutocomplete() {
                  $('.filterable-dropdown').each(function () {
                      var $dropdown = $(this);

                      // Avoid adding input twice
                      if ($dropdown.next('.modal-autocomplete-input').length === 0) {
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
                          var placeholderText = '';
                          var dropdownId = $dropdown.attr('id')?.toLowerCase();

                          switch (dropdownId) {
                              case "ddlfromwarehouse":
                                  placeholderText = '-- Select From Warehouse --';
                                  break;
                              case "ddltowarehouse":
                                  placeholderText = '-- Select To Warehouse --';
                                  break;
                          }

                          var $input = $('<input type="text" class="modal-autocomplete-input form-control" />')
                              .attr('placeholder', placeholderText)
                              .val($dropdown.val()) // show only ItemId
                              .insertAfter($dropdown)
                              .autocomplete({
                                  source: options,
                                  minLength: 0,
                                  select: function (event, ui) {
                                      $dropdown.val(ui.item.value);
                                  }
                              })
                              .on("focus", function () {
                                  $(this).autocomplete("search", ""); // Show all options on focus
                              })
                              // **ADD THIS PART (sync value on blur)**
                              .on("blur", function () {
                                  //var inputVal = $(this).val().toLowerCase();
                                  var inputVal = $(this).val().toLowerCase();
                                  var match = options.find(o => o.value.toLowerCase() === inputVal);
                                  $dropdown.val(match ? match.value : "");
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
        .autocomplete-input {
            width: 50%;
            /*padding: 2px 4px;*/
            font-size: 12px;
            font-family: Arial, sans-serif;
            box-sizing: border-box;

              /* Add or change the border to dark */
            border: 1px solid #333; /* Dark gray border */
           
        }
 
        .ui-menu-item:hover {
            background-color: #f0f0f0;
        }
 
        /*.ui-autocomplete {
            z-index: 99999 !important;
            max-height: 200px;
            overflow-y: auto;
            background-color: white;
            border: 1px solid #ccc;
        }*/
 
        /* Dropdown suggestion box */
        .ui-autocomplete {
            z-index: 99999 !important;
            max-height: 200px;
            overflow-y: auto;
            background-color: white;
            border: 1px solid #ccc;
            font-family: Arial, sans-serif;
            font-size: 10px;
        }
</style>
 <script>
     function formatDateInput(id) {
         let input = document.getElementById(id);
         input.addEventListener("change", function () {
             let val = this.value; // e.g. "2025-09-05"
             if (val) {
                 let d = new Date(val);
                 let formatted = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
                 this.type = "text";   // switch to text mode
                 this.value = formatted; // e.g. "9/5/2025"
             }
         });
     }

     // Apply to both textboxes
     formatDateInput("txtShippingDate");
     formatDateInput("txtReceiptDate");
 </script>
            </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
