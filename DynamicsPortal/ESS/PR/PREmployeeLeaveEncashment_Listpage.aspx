<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PREmployeeLeaveEncashment_Listpage.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PREmployeeLeaveEncashment_Listpage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
 <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
 <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
<asp:UpdatePanel ID="updButtons" runat="server">
      <ContentTemplate>
                                <div class="action-items">
        <asp:LinkButton ID="btnBack" runat="server" OnClientClick="setTimeout(function() { window.location = document.referrer || '/default.aspx'; }, 10); return false;">
<i class="mdi mdi-arrow-left"></i>Back
</asp:LinkButton>
    </div>
           <%--   <div class="action-items">
    <asp:LinkButton ID="BtnSaveHeader" runat="server">
    <i class="mdi mdi-content-save" style="margin-right: 4px;"></i>Save
    </asp:LinkButton>
</div>--%>
            <div class="action-items">
      <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/PREmployeeLeaveEncashment_Create.aspx');">
          <i class="mdi mdi-plus"></i> New
      </asp:LinkButton>
  </div>
  <div class="action-items">
      <asp:LinkButton ID="btnDelete" OnClick="btnDelete_Click" runat="server" >
          <i class="mdi mdi-delete"></i> Delete
      </asp:LinkButton>
  </div>
           <div class="action-items">
     <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
 </div>
                  </ContentTemplate>
</asp:UpdatePanel>  
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
<ContentTemplate>
        <div style="overflow: auto;">
        <asp:GridView ID="gridView" runat="server" Data="searchable" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
<%--                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" CssClass="round-checkbox" />
                    </HeaderTemplate>--%>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" AutoPostBack="true"  runat="server" CssClass="round-checkbox" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Request Id">
    <ItemTemplate>
        <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("RequestId") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Employee Id">
    <ItemTemplate>
        <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("EmployeeId") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Employee Name">
    <ItemTemplate>
        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Requested Date">
    <ItemTemplate>
        <asp:Label ID="lblRequestedDate" runat="server" Text='<%# Bind("RequestDate") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Leaves to be Encashed">
    <ItemTemplate>
        <asp:Label ID="lblLeavesToBeEncashed" runat="server" Text='<%# Bind("LeavesToEncashed") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Balance Before Application">
    <ItemTemplate>
        <asp:Label ID="lblBalanceBeforeApplication" runat="server" Text='<%# Bind("BalanceBeforeApplication") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Remaining Balance">
    <ItemTemplate>
        <asp:Label ID="lblRemainingBalance" runat="server" Text='<%# Bind("RemainingBalance") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="WF Status">
    <ItemTemplate>
        <asp:Label ID="lblWFStatus" runat="server" Text='<%# Bind("WFStatus") %>' />
    </ItemTemplate>
</asp:TemplateField>

<%--<asp:TemplateField HeaderText="Designation">
    <ItemTemplate>
        <asp:Label ID="lblDesignation" runat="server" Text='<%# Bind("Designation") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Department">
    <ItemTemplate>
        <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("Department") %>' />
    </ItemTemplate>
</asp:TemplateField>--%>

<%--<asp:TemplateField HeaderText="Earning Amount">
    <ItemTemplate>
        <asp:Label ID="lblEarningAmount" runat="server" Text='<%# Bind("EarningAmount") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Last Encashment Date">
    <ItemTemplate>
        <asp:Label ID="lblLastEncashmentDate" runat="server" Text='<%# Bind("LastEncashmentDate") %>' />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Last Encashment Leaves">
    <ItemTemplate>
        <asp:Label ID="lblLastEncashmentLeaves" runat="server" Text='<%# Bind("LastEncashmentLeaves") %>' />
    </ItemTemplate>
</asp:TemplateField>--%>


                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
            <ItemTemplate>
                <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server"  />
                <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="RecId" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />

        <asp:TemplateField HeaderText="RecId" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
            </ItemTemplate>
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
