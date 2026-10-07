<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSPREmployeeEOS_ListPage.aspx.cs" Inherits="DynamicsPortal.ESSPREmployeeEOS_ListPage" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
<asp:UpdatePanel ID="updButtons" runat="server">
    <ContentTemplate>
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/ESSPREmployeeEOS_Create.aspx')"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
      <%--  <div class="action-items"  >
        <asp:LinkButton ID="btnEdit" runat="server"><i class="mdi mdi-border-color"  ></i>Edit</asp:LinkButton>
    </div>--%>
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
   </div>
        <%-- <div class="action-items">
        <asp:LinkButton ID="btnView" runat="server"><i class="mdi mdi-eye"></i>View</asp:LinkButton>
    </div>--%>
    <div class="action-items">
        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
    </div>
        </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
     <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
 <ContentTemplate>
    <div style="overflow: auto;">
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
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
                <asp:TemplateField HeaderText="EOS Request Id">
                    <ItemTemplate>
                        <asp:Label ID="lblEOSRequestId" runat="server" Text='<%# Bind("EOSRequestId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="EOS Type">
                    <ItemTemplate>
                        <asp:Label ID="lblEOSType" runat="server" Text='<%# Bind("EOSType") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEOSType" runat="server" Enabled="false"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Notice Period Code">
                    <ItemTemplate>
                        <asp:Label ID="lblNoticePeriodCode" runat="server" Text='<%# Bind("NoticePeriodCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Notification Date">
                    <ItemTemplate>
                        <asp:Label ID="lblEOSNotificationDate" runat="server" Text='<%# Bind("EOSNotificationDate") %>' Textmode="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEOSNotificationDate" runat="server" Text='<%# Bind("EOSNotificationDate") %>' autocomplete="off"  Textmode="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last Working Date Requested">
                    <ItemTemplate>
                        <asp:Label ID="lblLastWorkingDate_Actual" runat="server" Text='<%# Bind("LastWorkingDate_Actual") %>' Textmode="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLastWorkingDate_Actual" runat="server" Text='<%# Bind("LastWorkingDate_Actual") %>' autocomplete="off"  Textmode="date" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last Working Date Actual">
                    <ItemTemplate>
                         <asp:Label ID="lblLastWorkingDate_Calculated" runat="server" Text='<%# Bind("LastWorkingDate_Calculated") %>' Textmode="date"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reason Code">
                    <ItemTemplate>
                        <asp:Label ID="lblEOSReasonCode" runat="server" Text='<%# Bind("EOSReasonCode") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEOSReasonCode" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
            <%--    <asp:TemplateField HeaderText="Generation Type">
                    <ItemTemplate>
                        <asp:Label ID="lblGenerationType" runat="server" Text='<%# Bind("GenerationType") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblEOSStatus" runat="server" Text='<%# Bind("EOSStatus") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remarks">
                    <ItemTemplate>
                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Workflow Status">
                    <ItemTemplate>
                        <asp:Label ID="lblWFStatus" runat="server" Text='<%# Bind("WorkflowStatus") %>' masktype="enum"></asp:Label>
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
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
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
    window.refreshParentGrid = function () {
        __doPostBack('RefreshGrid', '');
    };

    function getCheckedRows() {
        var boxes = document.querySelectorAll('input[id*="chk_SelectSingle"]');
        var checked = [];
        for (var i = 0; i < boxes.length; i++) {
            if (boxes[i].checked) checked.push(boxes[i]);
        }
        return checked;
    }

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

        GlobalDeleteConfirm.show({
            message: 'You are about to delete a record in Contact Details. This action cannot be undone.',
            onConfirm: function () {
                __doPostBack('<%= btnDelete.UniqueID %>', '');
            }
        });
    });
</script>
</asp:Content>


