<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHcmPersonEducation.aspx.cs" Inherits="DynamicsPortal.ESSHcmPersonEducation" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        table.sortable th:first-child,
table.sortable td:first-child {
    min-width: 20px !important;
    width: 20px !important;
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
    <div style ="overflow: auto !important"> 
      <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>

    <div class="action-panel-grid">
        <div class="action-items-grid">
            <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
        </div>
        <div class="action-items-grid">
           <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click">
    <i class="mdi mdi-delete"></i>Delete
</asp:LinkButton>
        </div>
        <div class="action-items-grid">
            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
        </div>
    </div>

    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" OnCheckedChanged="chk_SelectSingle_CheckedChanged" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Education">
                    <ItemTemplate>
                        <asp:Label ID="lblEducationDisciplineId" runat="server" Text='<%# Bind("EducationDisciplineId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEducationDiscipline" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Duration Unit">
                    <ItemTemplate>
                        <asp:Label ID="lblDurationUnit" runat="server" Text='<%# Bind("DurationUnit") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDurationUnit" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Duration">
                    <ItemTemplate>
                        <asp:Label ID="lblDuration" runat="server" Text='<%# Bind("Duration") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDuration" runat="server" Text='<%# Bind("Duration") %>' masktype="number"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Start Date">
                    <ItemTemplate>
                        <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("StartDate", "{0:M/d/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtStartDate" runat="server" Text='<%# Bind("StartDate", "{0:M/d/yyyy}") %>' TextMode="Date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="End Date">
                    <ItemTemplate>
                        <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("EndDate", "{0:M/d/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEndDate" runat="server" Text='<%# Bind("EndDate", "{0:M/d/yyyy}") %>' TextMode="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Notes">
                    <ItemTemplate>
                        <asp:Label ID="lblNotes" runat="server" Text='<%# Bind("Notes") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNotes" runat="server" Text='<%# Bind("Notes") %>' Width="300px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
              <%--  <asp:TemplateField HeaderText="Workflow Operation">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkflowOperation" runat="server" Text='<%# Bind("WorkflowOperation") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>

                <asp:TemplateField HeaderText="Approval Status">
                    <ItemTemplate>
                        <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
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

                <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" Visible="false" />
                <asp:BoundField DataField="Person" HeaderText="Person" Visible="false" />
                <asp:TemplateField HeaderText="Education" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblEducationDiscipline" runat="server" Text='<%# Bind("EducationDiscipline") %>'></asp:Label>
                    </ItemTemplate>
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
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
         <script type="text/javascript">
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
                 var allNotSubmitted = checked.length > 0;

                 for (var i = 0; i < checked.length; i++) {
                     var status = getStatusOfRow(checked[i]);
                     if (status !== 'NotSubmitted' && status !== 'Draft') {
                         allNotSubmitted = false;
                         break;
                     }
                 }

                 var submitBtn = document.querySelector('a[id$="btnSubmit"]');
                 var deleteBtn = document.querySelector('a[id$="btnDelete"]');

                 if (submitBtn) submitBtn.classList.toggle('aspNetDisabled', !allNotSubmitted);
                 if (deleteBtn) deleteBtn.classList.toggle('aspNetDisabled', !allNotSubmitted);
             }

             function wireDeleteButton() {
                 var deleteBtn = document.querySelector('a[id$="btnDelete"]');
                 if (!deleteBtn) return;

                 deleteBtn.addEventListener('click', function (e) {
                     e.preventDefault();
                     e.stopPropagation();

                     if (deleteBtn.classList.contains('aspNetDisabled')) return; // ← add this

                     if (getCheckedRows().length === 0) return;

                     GlobalDeleteConfirm.show({
                         message: 'You are about to delete a record in Education. This action cannot be undone.',
                         onConfirm: function () {
                             __doPostBack('<%= btnDelete.UniqueID %>', '');
            }
        });
    });
            }

             function initPage() {
                 wireDeleteButton();
                 updateActionButtons(); // ← reset button states on load/postback
             }

             // Wire change events once at document level (survives UpdatePanel)
             document.addEventListener('change', function (e) {
                 var t = e.target;
                 if (!t || !t.id) return;

                 if (t.id === 'chk_SelectAll') {
                     var boxes = document.querySelectorAll('input[id*="chk_SelectSingle"]');
                     for (var i = 0; i < boxes.length; i++) {
                         boxes[i].checked = t.checked;
                     }
                 }

                 if (t.id === 'chk_SelectAll' || t.id.indexOf('chk_SelectSingle') !== -1) {
                     updateActionButtons();
                 }
             });

             function wireSubmitButton() {
                 var submitBtn = document.querySelector('a[id$="btnSubmit"]');
                 if (!submitBtn) return;

                 submitBtn.addEventListener('click', function (e) {
                     if (submitBtn.classList.contains('aspNetDisabled')) {
                         e.preventDefault();
                         e.stopPropagation();
                         return;
                     }
                 });
             }

             wireSubmitButton();
             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(wireSubmitButton);

             initPage();
             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initPage);

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

