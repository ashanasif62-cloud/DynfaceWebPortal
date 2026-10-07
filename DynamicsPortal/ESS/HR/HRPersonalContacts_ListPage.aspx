<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="HRPersonalContacts_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.HR.HRPersonalContact_ListPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .page-title {
            font-size: 16px;
            font-weight: 600;
            margin-bottom: 12px;
        }

        .grid th {
            font-size: 12px;
            font-weight: 600;
            background-color: #f5f5f5;
        }

        .grid td {
            font-size: 13px;
        }



        .btn-link-style {
            color: #0d6efd; /* Bootstrap primary blue */
            background-color: transparent; /* No background */
            border: none; /* No border */
            padding: 0; /* Remove padding */
            font-size: 10px; /* Optional: adjust text size */
            padding-left: 5px; /* Optional: make it look like a link */
        }

        .btn-link-with-icon i {
            margin-left: 5px;
            vertical-align: middle;
        }

        .action-items-grid asp {
            margin-bottom: 10px; /* adjust space as needed */
            display: inline-block;
        }
        table.sortable th:first-child,
table.sortable td:first-child {
    min-width: 20px !important;
    width: 20px !important;
}
        .page-title,
.page-header,
#pageTitle,
.header-title {
    display: none !important;
}
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <div style="overflow: auto !important">
     <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>

   <div class="action-panel-grid">
            <div class="action-items-grid">
                <asp:LinkButton ID="btnNew" runat="server" OnClientClick="openCreateInParent(); return false;">
                    <i class="mdi mdi-plus"></i>New
                </asp:LinkButton>
            </div>
            <div class="action-items-grid">
                <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click">
                    <i class="mdi mdi-pencil"></i>Edit
                </asp:LinkButton>
            </div>
            <div class="action-items-grid">
               <%-- <asp:LinkButton ID="btnDel" runat="server" OnClick="btnDel_Click">
                    <i class="mdi mdi-delete"></i>Remove
                </asp:LinkButton>--%>

                 
                    <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDel_Click"
                        OnClientClick="return false;"><i class="mdi mdi-delete"></i> Delete</asp:LinkButton>
              
            </div>
        </div>
        <div>
            <asp:GridView
                ID="gvPersonalContacts"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-bordered"
                ShowHeaderWhenEmpty="true"
                EmptyDataText="No records found.">

                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text=<%# Eval("Name") %>></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Relationship">
                        <ItemTemplate>
                            <asp:Label ID="lblRelationShipTypeId" runat="server" Text=<%# Eval("RelationShipTypeId") %>></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Emergency contact">
                        <ItemTemplate>
                            <asp:Label
                                ID="lblEmergencyContact"
                                runat="server"
                                Text="✔"
                                CssClass="tick"
                                Visible='<%# Eval("EmergencyContact") != DBNull.Value 
                              && Convert.ToBoolean(Eval("EmergencyContact")) %>' />
                        </ItemTemplate>

                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Dependent">
                        <ItemTemplate>
                            <asp:Label
                                ID="lblIsDependent"
                                runat="server"
                                Text="✔"
                                CssClass="tick"
                                Visible='<%# Eval("isDependent") != DBNull.Value 
                                && Convert.ToBoolean(Eval("isDependent")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Beneficiary">
                        <ItemTemplate>
                            <asp:Label
                                ID="lblIsBeneficiary"
                                runat="server"
                                Text="✔"
                                CssClass="tick"
                                Visible='<%# Eval("isBeneficiary") != DBNull.Value 
                                && Convert.ToBoolean(Eval("isBeneficiary")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Beneficiary Percentage">
                        <ItemTemplate>
                            <asp:Label ID="lblBeneficiaryPercentage" runat="server" Text=<%# Eval("BenefecieryPercentage") %>></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RecId" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblBeneficiaryFromDate" runat="server" Text='<%# Bind("BeneficiaryFromDate") %>'></asp:Label>
                            <asp:Label ID="lblBeneficiaryToDate" runat="server" Text='<%# Bind("BeneficiaryToDate") %>'></asp:Label>
                            <asp:Label ID="lblBirthDate" runat="server" Text='<%# Bind("BirthDate") %>'></asp:Label>
                            <asp:Label ID="lblGender" runat="server" Text='<%# Bind("Gender") %>'></asp:Label>
                            <asp:Label ID="lblisFullTimeStudent" runat="server" Text='<%# Bind("isFullTimeStudent") %>'></asp:Label>
                            <asp:Label ID="lblisPersonWithDisabilities" runat="server" Text='<%# Bind("isPersonWithDisabilities") %>'></asp:Label>
                            <asp:Label ID="lblVerificationDate" runat="server" Text='<%# Bind("VerificationDate") %>'></asp:Label>
                            <asp:Label ID="lblDependentValidFrom" runat="server" Text='<%# Bind("DependentValidFrom") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblDependentValidTo" runat="server" Text='<%# Bind("DependentValidTo") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </div>
    <script type="text/javascript">
    function openCreateInParent() {
        /* Walk up through iframes to reach the top window where GlobalPopup lives */
        var win = window;
        while (win !== win.parent) {
            win = win.parent;
        }
        if (win.GlobalPopup) {
            win.GlobalPopup.show({
                title: 'New Personal Contact',
                url: '/ESS/HR/HRPersonalContacts_Create.aspx'
            });
        }
    }
    </script>
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

        /* ── Delete button ── */
        function initDeleteBtn() {
            var deleteBtn = document.querySelector('a[id$="btnDelete"]');
            if (!deleteBtn) return;

            deleteBtn.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                if (getCheckedRows().length !== 1) return;
                GlobalDeleteConfirm.show({
                    message: 'You are about to delete a record in Personal Contacts. This action cannot be undone.',
                    onConfirm: function () { __doPostBack('<%= btnDelete.UniqueID %>', ''); }
                });
            });
        }

        function updateDeleteBtn() {
            var deleteBtn = document.querySelector('a[id$="btnDelete"]');
            if (!deleteBtn) return;
            deleteBtn.classList.toggle('aspNetDisabled', getCheckedRows().length !== 1);
        }

        /* ── Edit button ── */
        function initEditBtn() {
            var editBtn = document.querySelector('a[id$="btnEdit"]');
            if (!editBtn) return;

            function handleClick(e) {
                e.preventDefault();
                e.stopPropagation();
                if (getCheckedRows().length !== 1) return;

                // Let ASP.NET's own postback fire (triggers btnEdit_Click → opens modal)
                editBtn.removeEventListener('click', handleClick);
                editBtn.click();
                editBtn.addEventListener('click', handleClick);
            }

            editBtn.addEventListener('click', handleClick);
        }

        function updateEditBtn() {
            var editBtn = document.querySelector('a[id$="btnEdit"]');
            if (!editBtn) return;
            editBtn.classList.toggle('aspNetDisabled', getCheckedRows().length !== 1);
        }

        /* ── Checkbox change ── */
        document.addEventListener('change', function (e) {
            if (e.target && e.target.id && e.target.id.indexOf('chk_SelectSingle') !== -1) {
                updateDeleteBtn();
                updateEditBtn();
            }
        });

        /* ── Bootstrap ── */
        function bootstrap() {
            initDeleteBtn();
            initEditBtn();
            updateDeleteBtn();
            updateEditBtn();
        }

        /* ── Re-run after every UpdatePanel postback ── */
        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                bootstrap();
            });
        }

        /* ── Initial load ── */
        bootstrap();
    })();
</script>
        </ContentTemplate>
         </asp:UpdatePanel> 
    </div>
</asp:Content>
