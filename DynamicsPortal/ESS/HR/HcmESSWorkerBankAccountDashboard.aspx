<%@  Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="HcmESSWorkerBankAccountDashboard.aspx.cs" Inherits="DynamicsPortal.HcmESSWorkerBankAccountDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
 table.sortable th:first-child,
table.sortable td:first-child {
    min-width: 20px !important;
    width: 20px !important;
}
  .disabled-btn {
            opacity: 0.45;
            pointer-events: none;
            cursor: not-allowed;
        }
  /* Action bar sticky */
.action-panel-grid {
    position: sticky;
    top: 0;
    z-index: 10;
    background-color: #fff;
}

/* Sticky header row */
#PageContent_gridView tbody tr:first-child th {
    position: sticky;
    top: 28px; /* height of action-panel-grid — adjust if needed */
    z-index: 5;
    background-color: #fff;
    box-shadow: 0 1px 0 #ddd; /* subtle line under header */
}
/* ── Master page fixes ── */
.side-navbar {
    overflow-x: hidden !important;
    overflow-y: auto !important;
}

.page-placeholder {
    overflow-x: hidden !important;
    overflow-y: auto !important;  /* this is your MAIN vertical scroll - keep it */
}

.tab-bar {
    overflow-x: auto !important;
    overflow-y: hidden !important;
}
</style>

</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div style = "overflow: auto !important">
      <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
     <div class="action-panel-grid">
     <div class="action-items-grid">
         <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
     </div>
 <div class="action-items-grid">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" 
    OnClientClick="return false;">
    <i class="mdi mdi-delete"></i> Delete
</asp:LinkButton>
     </div>
     <div class="action-items-grid">
         <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
     </div>
 </div>

 <div>
     <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable gv-date-enabled" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
         OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
                 <Columns>
            <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                <HeaderTemplate>
                    <input type="checkbox" id="chk_SelectAll" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
          <asp:TemplateField HeaderText="Bank Name">
    <ItemTemplate>
        <asp:Label ID="lblAccountId" runat="server" Text='<%# Bind("accountId") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtAccountId" runat="server" Text='<%# Bind("accountId") %>'></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Name">
    <ItemTemplate>
        <asp:Label ID="lblName" runat="server" Text='<%# Bind("name") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Bank account number">
    <ItemTemplate>
        <asp:Label ID="lblAccountNum" runat="server" Text='<%# Bind("accountNum") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtAccountNum" runat="server" Text='<%# Bind("accountNum") %>'></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="IBAN">
    <ItemTemplate>
        <asp:Label ID="lblBankIBAN" runat="server" Text='<%# Bind("bankIBAN") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtBankIBAN" runat="server" Text='<%# Bind("bankIBAN") %>'></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Branch Name">
    <ItemTemplate>
        <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("branchName") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtBranchName" runat="server" Text='<%# Bind("branchName") %>'></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Branch Number">
    <ItemTemplate>
        <asp:Label ID="lblBranchNumber" runat="server" Text='<%# Bind("branchNumber") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtBranchNumber" runat="server" Text='<%# Bind("branchNumber") %>'></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Account Holder">
    <ItemTemplate>
        <asp:Label ID="lblAccountHolder" runat="server" Text='<%# Bind("accountHolder") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtAccountHolder" runat="server" Text='<%# Bind("accountHolder") %>'></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>    

    <asp:TemplateField HeaderText="Approval Status">
        <ItemTemplate>
            <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Bind("WFStatus") %>'></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
            
            <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                <ItemTemplate>
                    <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                    <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                    <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
            <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
            <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
            <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
            <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
            <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
            <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false" />
            <asp:TemplateField HeaderText="RecId" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                </ItemTemplate>
                <%--<EditItemTemplate>
                        <asp:TextBox ID="txtRecId" runat="server"></asp:TextBox>
                    </EditItemTemplate>--%>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>
</div>
         <script type="text/javascript">
             (function () {

                 /* ── helpers ── */
                 function getCheckedRows() {
                     var boxes = document.querySelectorAll('input[id*="chk_SelectSingle"]');
                     var checked = [];
                     for (var i = 0; i < boxes.length; i++) {
                         if (boxes[i].checked) checked.push(boxes[i]);
                     }
                     return checked;
                 }

                 function getStatusOfRow(box) {
                     var row = box.closest('tr');
                     if (!row) return '';
                     var statusLabel = row.querySelector('span[id*="lblApprovalStatus"]');
                     if (!statusLabel) return '';
                     return statusLabel.innerText.trim().replace(/\s/g, '');
                 }

                 function updateActionButtons() {
                     var checked = getCheckedRows();

                     var hasNotSubmitted = false;
                     var allNotSubmitted = true; // for delete — ALL selected must be NotSubmitted

                     for (var i = 0; i < checked.length; i++) {
                         var status = getStatusOfRow(checked[i]);
                         var isNotSubmitted = (status === 'NotSubmitted' || status === 'Draft');

                         if (isNotSubmitted) {
                             hasNotSubmitted = true;
                         } else {
                             allNotSubmitted = false;
                         }
                     }

                     if (checked.length === 0) {
                         allNotSubmitted = false;
                         hasNotSubmitted = false;
                     }

                     /* Submit — enabled if at least one checked row is NotSubmitted */
                     var submitBtn = document.querySelector('a[id$="btnSubmit"]');
                     if (submitBtn) {
                         if (allNotSubmitted && checked.length > 0) {
                             submitBtn.classList.remove('disabled-btn');
                         } else {
                             submitBtn.classList.add('disabled-btn');
                         }
                     }

                     /* Delete — enabled only if ALL checked rows are NotSubmitted */
                     var deleteBtn = document.querySelector('a[id$="btnDelete"]');
                     if (deleteBtn) {
                         if (allNotSubmitted && checked.length > 0) {
                             deleteBtn.classList.remove('disabled-btn');
                         } else {
                             deleteBtn.classList.add('disabled-btn');
                         }
                     }
                 }

                 /* ── Select-All ── */
                 document.addEventListener('change', function (e) {
                     var t = e.target;
                     if (t && t.id === 'chk_SelectAll') {
                         var boxes = document.querySelectorAll('input[id*="chk_SelectSingle"]');
                         for (var i = 0; i < boxes.length; i++) {
                             boxes[i].checked = t.checked;
                         }
                     }
                     if (t && t.id && (t.id === 'chk_SelectAll' || t.id.indexOf('chk_SelectSingle') !== -1)) {
                         updateActionButtons();
                     }
                 });

                 /* ── Submit button — delegated ── */
                 document.addEventListener('click', function (e) {
                     var el = e.target;
                     var submitBtn = null;
                     while (el && el !== document) {
                         if (el.tagName === 'A' && el.id && el.id.indexOf('btnSubmit') !== -1) {
                             submitBtn = el;
                             break;
                         }
                         el = el.parentElement;
                     }
                     if (!submitBtn) return;
                     e.preventDefault();
                     e.stopPropagation();
                     if (submitBtn.classList.contains('disabled-btn')) return;

                     if (document.querySelector('a.btn-cancel')) {
                         alert('Please save or cancel the current edit before submitting.');
                         return;
                     }

                     __doPostBack('<%= btnSubmit.UniqueID %>', '');
                 });

                 /* ── Delete button — delegated, survives UpdatePanel redraws ── */
                 document.addEventListener('click', function (e) {
                     var el = e.target;
                     var deleteBtn = null;
                     while (el && el !== document) {
                         if (el.tagName === 'A' && el.id && el.id.indexOf('btnDelete') !== -1) {
                             deleteBtn = el;
                             break;
                         }
                         el = el.parentElement;
                     }
                     if (!deleteBtn) return;

                     e.preventDefault();
                     e.stopPropagation();

                     if (deleteBtn.classList.contains('disabled-btn')) return;

                     if (document.querySelector('a.btn-cancel')) {
                         alert('Please save or cancel the current edit before deleting.');
                         return;
                     }

                     if (getCheckedRows().length === 0) return;

                     GlobalDeleteConfirm.show({
                         message: 'You are about to delete a bank account record. This action cannot be undone.',
                         onConfirm: function () {
                             __doPostBack('<%= btnDelete.UniqueID %>', '');
            }
        });
    });

                 /* ── Init on load and after every UpdatePanel postback ── */
                 updateActionButtons();

                 if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                         updateActionButtons();
                     });
                 }

             })();

             function syncActionPanelWidth() {
                 var table = document.querySelector('#PageContent_gridView');
                 var actionPanel = document.querySelector('.action-panel-grid');
                 if (table && actionPanel) {
                     actionPanel.style.width = table.offsetWidth + 'px';
                 }
             }

             // Run on load
             syncActionPanelWidth();

             // Run on window resize
             window.addEventListener('resize', syncActionPanelWidth);

             // Run after every UpdatePanel refresh
             if (typeof Sys !== 'undefined') {
                 Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                     syncActionPanelWidth();
                 });
             }
         </script>
         </ContentTemplate>
          </asp:UpdatePanel>
    </div>

</asp:Content>
