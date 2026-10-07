<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="PREmployeeEOSClearanceItems_ListPage.aspx.cs" 
    Inherits="DynamicsPortal.ESS.PR.PREmployeeEOSClearanceItems_ListPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />

   <style>
    /* ===== Remove extra white space ===== */
    .page-content, 
    #PageContent,
    .main-content {
        padding-top: 4px !important;
        padding-bottom: 4px !important;
    }

    /* Outer container */
    .content-wrapper > div {
        padding: 6px 12px !important;
    }

    /* Cards */
    .card.mb-3 {
        margin-bottom: 8px !important;
        border-radius: 4px;
    }

    .card {
        box-shadow: none !important;
    }

    /* Section header (End of Service Request) */
    .d365-toggle-header {
        padding: 6px 12px !important;
        min-height: 32px !important;
        background-color: white !important;
        cursor: pointer !important;
        outline: none !important;
        user-select: none !important;
        -webkit-user-select: none !important;
        -moz-user-select: none !important;
        text-decoration: none !important;
    }

    .d365-toggle-header:focus {
        outline: none !important;
        box-shadow: none !important;
    }

    .section-title {
        font-size: 13.5px !important;
        font-weight: 600;
        color: #323130;
        margin: 0 !important;
    }

    /* Card body - reduce padding */
    .transfer-line-card-body {
        padding: 4px 8px !important;
    }

    /* Grid */
    .table-responsive-custom {
        width: 100%;
        overflow-x: auto;
        -webkit-overflow-scrolling: touch;
    }

    .table-condensed.table-hover {
        font-size: 12.5px !important;
        margin-bottom: 0 !important;
        width: 100%;
    }

    .table-condensed.table-hover th,
    .table-condensed.table-hover td {
        padding: 3px 8px !important;
        vertical-align: middle !important;
        white-space: nowrap;
        line-height: 1.2 !important;
        height: 26px !important;
    }

    .table-condensed.table-hover th {
        font-size: 12px !important;
        font-weight: 600;
        background-color: #f8f9fa;
        height: 28px !important;
    }

    /* Checkbox */
    .round-checkbox input[type="checkbox"] {
        width: 14px;
        height: 14px;
        margin: 0;
        cursor: pointer;
    }

    /* Dropdown & Remarks */
    .textbox-label,
    #gridView1 select.textbox-label {
        font-size: 12.5px !important;
        padding: 1px 4px !important;
        height: 24px !important;
        line-height: 1.2 !important;
        width: 100%;
        max-width: 120px;
        border: 1px solid #ced4da;
        border-radius: 3px;
        /*background-color: #fff;*/
    }

    #gridView1 .textbox-label[type="text"] {
        max-width: 150px;
    }

    /* Page Title */
    #pageTitle, 
    .page-title,
    [id*="pageTitle"] {
        font-size: 20px !important;
        font-weight: 600 !important;
        color: #323130 !important;
        cursor: default !important;
        user-select: none !important;
        -webkit-user-select: none !important;
        outline: none !important;
        text-decoration: none !important;
        margin: 4px 0 8px 0 !important;
    }

    #pageTitle:focus,
    .page-title:focus {
        outline: none !important;
        box-shadow: none !important;
    }

    /* Hide overlay */
    .UpdateProgress, .aspNetDisabled, #UpdateProgress1 {
        display: none !important;
    }

    /* Force the grid area not to stretch */
    .collapse.show {
        height: auto !important;
    }

    .transfer-line-card-body,
    .table-responsive-custom,
    .UpdatePanel {
        height: auto !important;
        min-height: 0 !important;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    
    <asp:UpdatePanel ID="updButtons" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <%--    <div class="action-items">
                <asp:LinkButton ID="btnBack" runat="server"
                    OnClientClick="setTimeout(function() { window.location = document.referrer || '/default.aspx'; }, 10); return false;">
                    <i class="mdi mdi-arrow-left"></i> Back
                </asp:LinkButton>
            </div>--%>
            <div class="action-items">
                <asp:LinkButton ID="BtnSaveHeader" runat="server" OnClick="BtnSave_Header_Click">
                    <i class="mdi mdi-content-save" style="margin-right: 4px;"></i> Save
                </asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <div style="padding: 12px;">

        <!-- ==================== EOS REQUEST SECTION ==================== -->
        <div class="card mb-3">
            <a href="#eosClearancePanel"
               class="d365-toggle-header d-flex justify-content-between align-items-center mt-0"
               data-toggle="collapse" role="button" aria-expanded="true" aria-controls="eosClearancePanel">
                <span class="section-title">End of Service Request</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>

            <div id="eosClearancePanel" class="collapse show">
                <div class="transfer-line-card-body shadow-sm">
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive-custom">
                                <asp:GridView ID="gridView"
                                    runat="server"
                                    CssClass="table table-condensed no-bordered table-hover sortable"
                                    ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No Record Found."
                                    DataKeyNames="RecId"
                                    AutoGenerateColumns="false"
                                    OnRowDataBound="gridView_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_SelectSingle"
                                                    runat="server"
                                                    AutoPostBack="true"
                                                    OnCheckedChanged="chk_SelectSingle_CheckedChanged"
                                                    CssClass="round-checkbox" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="EOS Request Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEosReqId" runat="server" Text='<%# Bind("RequestId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Employee">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployee" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Last Working Date Requested">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastWorkingActual" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Last Working Date Planned">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastWorkingCalculated" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Reason Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReasonCode" runat="server" Text='<%# Bind("ReasonCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <!-- ==================== CLEARANCE SECTION ==================== -->
        <div class="card mb-3">
            <a href="#lowerpanel"
               class="d365-toggle-header d-flex justify-content-between align-items-center mt-0"
               data-toggle="collapse" role="button" aria-expanded="true" aria-controls="lowerpanel">
                <span class="section-title">Clearance</span>
                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
            </a>

            <div id="lowerpanel" class="collapse show">
                <div class="transfer-line-card-body shadow-sm">
                    <asp:UpdatePanel ID="upGrid1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive-custom">
                                <asp:GridView ID="gridView1"
                                    runat="server"
                                    CssClass="table table-condensed no-bordered table-hover sortable"
                                    ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No Record Found."
                                    DataKeyNames="RecId"
                                    AutoGenerateColumns="false"
                                    OnRowDataBound="gridView1_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_SelectSingle1"
                                                    runat="server"
                                                    CssClass="round-checkbox" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Item">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# Bind("ClearanceItem") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("ItemDescription") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="textbox-label">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lblRemarks" runat="server"
                                                    CssClass="textbox-label"
                                                    Text='<%# Bind("Remarks") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="RecId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            // Stop blinking cursor on page title
            var title = document.getElementById('pageTitle') ||
                document.querySelector('[id*="pageTitle"]') ||
                document.querySelector('.page-title');

            if (title) {
                title.setAttribute('tabindex', '-1');
                title.style.outline = 'none';
                title.style.userSelect = 'none';
                title.style.cursor = 'default';

                title.addEventListener('mousedown', function (e) {
                    e.preventDefault();
                });
                title.addEventListener('click', function () {
                    this.blur();
                });
            }

            // Also for section headers
            $('.d365-toggle-header').on('mousedown', function (e) {
                e.preventDefault();
            }).on('click', function () {
                $(this).blur();
            });
        });
    </script>
</asp:Content>