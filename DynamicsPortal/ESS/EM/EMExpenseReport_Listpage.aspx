<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EMExpenseReport_Listpage.aspx.cs" Inherits="DynamicsPortal.ESS.EM.EMExpenseReport_Listpage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <style>
        .disabled-button {
            cursor: not-allowed !important;
            pointer-events: none; /* Optional: prevents all mouse interaction */
        }
    </style>
    <style>
        .disabled-buttonLines {
            color: gray !important;
            cursor: not-allowed !important;
            pointer-events: none; /* Optional: prevents all mouse interaction */
        }
    </style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>
            <div class="action-items">
                <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/EM/EMExpenseReportCreate.aspx')"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
            </div>
            <div class="action-items">
                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
            </div>
            <div class="action-items">
                <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable"
                ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId" AutoGenerateColumns="false" OnRowEditing="gridView_RowEditing"
                OnRowDataBound="gridView_RowDataBound" OnRowCommand="gridView_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <HeaderTemplate>
                            <input type="checkbox" id="chk_SelectAll" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_SelectSingle" data-status='<%# Eval("DocumentStatus") %>' runat="server" AutoPostBack="true" OnCheckedChanged="chk_SelectSingle_CheckedChanged" CssClass="round-checkbox rowStatus" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:TemplateField HeaderText="Expense Report Number">
                <ItemTemplate>
                    <asp:HyperLink
                        ID="lblExpenseReportNumber"
                        runat="server"
                        Text='<%# Eval("ExpenseReportNumber") %>'
                        NavigateUrl='<%# Eval("ExpenseReportNumber", "~/ESS/EM/EMExpenseLines.aspx?ExpenseReportNumber={0}") %>'
                        CssClass="link-style" />
                </ItemTemplate>
            </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Expense Report Number">
                        <ItemTemplate>
                            <asp:LinkButton
                                ID="lnkExpenseReportNumber"
                                runat="server"
                                Text='<%# Eval("ExpenseReportNumber") %>'
                                CommandArgument='<%# Eval("ExpenseReportNumber") %>'
                                OnClick="lnkExpenseReportNumber_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Approval Status">
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptsAttached" runat="server" Text='<%# Eval("DocumentStatus") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Purpose">
                        <ItemTemplate>
                            <asp:Label ID="lblPurpose" runat="server" Text='<%# Eval("Purpose") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlPurpose" runat="server" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlLocation" runat="server" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblAmountCurr" runat="server" Text='<%# Eval("AmountTotal") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Created Date">
                        <ItemTemplate>
                            <asp:Label ID="lblCraetedDateTime" runat="server" Text='<%# Eval("createdDateTime") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Payment Date">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentDate" runat="server" Text='<%# Eval("PaymentDate") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Payment Voucher">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentVoucher" runat="server" Text='<%# Eval("PaymentVoucher") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Invoice">
                        <ItemTemplate>
                            <asp:Label ID="lblInvoice" runat="server" Text='<%# Eval("Invoice") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="DefaultDimension" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDefaultDimension" runat="server" Text='<%# Bind("Dimension") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                            <asp:LinkButton ID="btnAttach" CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="RecId" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnEdit" CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                            <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(function () {
            showAJAXOverlay();  // Should now fire
        });

        prm.add_endRequest(function () {
            hideAJAXOverlay();
        });
    </script>
  <script type="text/javascript">
      function refreshParentGrid() {
          __doPostBack('RefreshGrid', '');
      }
  </script>


</asp:Content>
