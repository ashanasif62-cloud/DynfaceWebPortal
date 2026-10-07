<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master"
    CodeBehind="ESSLogisticsElectronicAddress.aspx.cs" Inherits="DynamicsPortal.ESSLogisticsElectronicAddress" %>

    <asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
        <style>
          

            /* ── Disabled state for Edit buttons (header + inline row icons) ── */
            .disabled-btn {
                opacity: 0.45;
                pointer-events: none;
                cursor: not-allowed;
            }
       table.sortable th:first-child,
table.sortable td:first-child {
    min-width: 20px !important;
    width: 20px !important;
    width: 20px !important;
}

       /* Sticky header row */
#PageContent_gridView tbody tr:first-child th {
    position: sticky;
    top: 2px; /* height of action-panel-grid — adjust if needed */
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
          <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>


            <div class="action-panel-grid">
                <div class="action-items-grid">
                    <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_Click"><i class="mdi mdi-plus"></i>New
                    </asp:LinkButton>
                </div>
<%--                <div class="action-items-grid">
                    <a id="btnEditHeader" href="#" class="disabled-btn"><i class="mdi mdi-border-color"></i>Edit</a>
                </div>
                <div class="action-items-grid">
                    <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"
                        OnClientClick="return false;"><i class="mdi mdi-delete"></i>Remove</asp:LinkButton>
                </div>--%>
            </div>
            <div style="overflow: auto;">
                <asp:GridView ID="gridView" runat="server" data="searchable"
                    CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true"
                    EmptyDataText="No Record Found." DataKeyNames="RecId" OnRowEditing="gridView_RowEditing"
                    OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <HeaderTemplate>
                                <input type="checkbox" id="chk_SelectAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlType" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact Number/Address">
                            <ItemTemplate>
                                <asp:Label ID="lblLocator" runat="server" Text='<%# Bind("Locator") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLocator" runat="server" Text='<%# Bind("Locator") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Extension">
                            <ItemTemplate>
                                <asp:Label ID="lblLocatorExtension" runat="server"
                                    Text='<%# Bind("LocatorExtension") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLocatorExtension" runat="server"
                                    Text='<%# Bind("LocatorExtension") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Is Primary">
                            <ItemTemplate>
                              <asp:Label ID="lblIsPrimary" runat="server"
                                  Text='<%# Convert.ToBoolean(Eval("IsPrimary")) ? "Yes" : "No" %>'>
                              </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlIsPrimary" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
<%--                            <ItemTemplate>
                                <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit"
                                    runat="server" CommandName="Edit" />
                                <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment"
                                    Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                            </ItemTemplate>--%>
                            <EditItemTemplate>
                                <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update"
                                    runat="server" OnClick="Update_Click" />
                                <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel"
                                    runat="server" OnClick="Cancel_Click" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion"
                            Visible="false" />
                        <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime"
                            SortExpression="ModifiedDateTime" Visible="false" />
                        <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy"
                            Visible="false" />
                        <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime"
                            SortExpression="CreatedDateTime" Visible="false" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"
                            Visible="false" />
                        <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId"
                            Visible="false" />
                        <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition"
                            Visible="false" />
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
        <%--<script type="text/javascript">
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

                /* ── Select-All checkbox ── */
                document.addEventListener('change', function (e) {
                    var t = e.target;
                    if (t && t.id === 'chk_SelectAll') {
                        var boxes = document.querySelectorAll('input[id*="chk_SelectSingle"]');
                        for (var i = 0; i < boxes.length; i++) {
                            boxes[i].checked = t.checked;
                        }
                        updateEditBtn();
                    } else if (t && t.id && t.id.indexOf('chk_SelectSingle') !== -1) {
                        updateEditBtn();
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

                    if (getCheckedRows().length === 0) return;

                    GlobalDeleteConfirm.show({
                        message: 'You are about to delete a record in Contact Details. This action cannot be undone.',
                        onConfirm: function () {
                            __doPostBack('<%= btnDelete.UniqueID %>', '');
            }
        });
    });

            })();
        </script>--%>
        </ContentTemplate>
              </asp:UpdatePanel>
    </asp:Content>