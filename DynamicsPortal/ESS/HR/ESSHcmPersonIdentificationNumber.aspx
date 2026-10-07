<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHcmPersonIdentificationNumber.aspx.cs" Inherits="DynamicsPortal.ESSHcmPersonIdentificationNumber" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
  table.sortable th:first-child,
table.sortable td:first-child {
    min-width: 20px !important;
    width: 20px !important;
}
  #btnDelete:disabled {
    opacity: 0.4;
    cursor: not-allowed;
    pointer-events: none;
}
  .btn-delete:disabled {
    opacity: 0.4;
    cursor: not-allowed;
    pointer-events: none;
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
    <div style="overflow-x: auto; overflow-y: visible;">
     <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
    <div class="action-panel-grid">
<%--        <div class="action-items-grid">
            <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_Click"><i class="mdi mdi-plus" visible="false"></i>New</asp:LinkButton>
        </div>
        <div class="action-items-grid">
            <asp:LinkButton ID="btnDelete" CssClass="btn-delete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete" visible="false"></i>Delete</asp:LinkButton>
        </div>
        <div class="action-items-grid">
            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus" visible="false"></i>Submit</asp:LinkButton>
        </div>--%>
    </div>

    <div style="overflow-x: visible; overflow-y: visible;">
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable gv-date-enabled" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" OnCheckedChanged="chk_SelectSingle_CheckedChanged"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Identification Type">
                    <ItemTemplate>
                        <asp:Label ID="lblIdentificationType" runat="server" Text='<%# Bind("IdentificationTypeId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIdentificationType" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Number">
                    <ItemTemplate>
                        <asp:Label ID="lblIdentificationNumber" runat="server" Text='<%# Bind("IdentificationNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtIdentificationNumber" runat="server" Text='<%# Bind("IdentificationNumber") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Entry Type" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblClassification" runat="server" Text='<%# Bind("Classification") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtClassification" runat="server" Text='<%# Bind("Classification") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Is Primary" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrimary" runat="server" Text='<%# Bind("IsPrimary") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIsPrimary" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
<%--                <asp:TemplateField HeaderText="Issuing Agency">
                    <ItemTemplate>
                        <asp:Label ID="lblIssuingAgency" runat="server" Text='<%# Bind("IssuingAgencyId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIssuingAgency" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Issued Date">
                    <ItemTemplate>
                        <asp:Label ID="lblIssuedDate" runat="server" Text='<%# Bind("IssuedDate") %>' TextMode="Date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtIssuedDate" runat="server" Text='<%# Bind("IssuedDate") %>' TextMode="Date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Expiration Date">
                    <ItemTemplate>
                        <asp:Label ID="lblExpirationDate" runat="server" Text='<%# Bind("ExpirationDate") %>' TextMode="Date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtExpirationDate" runat="server" Text='<%# Bind("ExpirationDate") %>' TextMode="Date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Workflow Operation" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkflowOperation" runat="server" Text='<%# Bind("WorkflowOperation") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Approval Status" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" Visible="false" />
                        <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" Visible="false" />
                <asp:BoundField DataField="Person" HeaderText="Person" Visible="false" />
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRecVersion" runat="server" Text='<%# Eval("PersonIdentificationNumberRecVersion") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
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
                            <asp:TextBox ID="RecId" runat="server"></asp:TextBox>
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

                function updateEditBtn() {
                    var headerEditBtn = document.getElementById('btnEditHeader');
                    var checked = getCheckedRows();
                    var multiSelect = checked.length > 1;

                    if (headerEditBtn) {
                        if (checked.length === 1) {
                            headerEditBtn.classList.remove('disabled-btn');
                        } else {
                            headerEditBtn.classList.add('disabled-btn');
                        }
                    }

                    var inlineEditBtns = document.querySelectorAll('a.btn-edit');
                    for (var j = 0; j < inlineEditBtns.length; j++) {
                        if (multiSelect) {
                            inlineEditBtns[j].classList.add('disabled-btn');
                        } else {
                            inlineEditBtns[j].classList.remove('disabled-btn');
                        }
                    }
                }

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
                        console.log("Is Not Submitted : ", isNotSubmitted);
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
                            submitBtn.classList.remove('aspNetDisabled');
                        } else {
                            submitBtn.classList.add('aspNetDisabled');
                        }
                    }

                    /* Delete — enabled only if ALL checked rows are NotSubmitted */
                    var deleteBtn = document.querySelector('a[id$="btnDelete"]');
                    if (deleteBtn) {
                        if (allNotSubmitted && checked.length > 0) {
                            deleteBtn.classList.remove('aspNetDisabled');
                        } else {
                            deleteBtn.classList.add('aspNetDisabled');
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

                /* ── Edit header button ── */
                document.addEventListener('click', function (e) {
                    var editBtn = document.getElementById('btnEditHeader');
                    if (editBtn && editBtn.contains(e.target) && !editBtn.classList.contains('disabled-btn')) {
                        e.preventDefault();
                        var checked = getCheckedRows();
                        if (checked.length !== 1) return;
                        var td = checked[0].closest('tr');
                        if (!td) return;
                        var inlineEdit = td.querySelector('a[title="Edit"], input[value="Edit"]');
                        if (inlineEdit) inlineEdit.click();
                    }
                });

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
                    if (submitBtn.classList.contains('aspNetDisabled')) return;

                    if (document.querySelector('a.btn-cancel')) {
                        alert('Please save or cancel the current edit before submitting.');
                        return;
                    }

                  <%--  __doPostBack('<%= btnSubmit.UniqueID %>', '');--%>
                });

                /* ── Delete button → delegated, survives UpdatePanel redraws ── */
                document.addEventListener('click', function (e) {
                    var el = e.target;
                    var deleteBtn = null;

                    // Walk up DOM to find the btnDelete anchor
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

                    // Block delete if grid is in edit mode
                    if (document.querySelector('a.btn-cancel')) {
                        alert('Please save or cancel the current edit before deleting.');
                        return;
                    }

                    if (deleteBtn.classList.contains('aspNetDisabled')) return;

                    if (getCheckedRows().length === 0) return;

                 <%--   GlobalDeleteConfirm.show({
                        message: 'You are about to delete a record in Identification Number. Delete record?',
                        onConfirm: function () {
                            __doPostBack('<%= btnDelete.UniqueID %>', '');
            }
        });--%>
    });

            })();
        </script>
          </ContentTemplate>
              </asp:UpdatePanel>
    </div>
</asp:Content>

