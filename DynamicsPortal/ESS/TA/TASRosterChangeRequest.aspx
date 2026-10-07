<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TASRosterChangeRequest.aspx.cs" Inherits="DynamicsPortal.ESS.TA.TASRosterChangeRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        :root {
            --field-border: #d0d0d0;
            --field-border-focus: #0078d4;
            --card-shadow: 0 1px 3px rgba(0,0,0,.10);
        }

        .form-control,
        .aspnet-textbox {
            font-size: 0.8125rem;
            height: 30px;
            padding: 3px 8px;
            border: 1px solid var(--field-border);
            border-radius: 2px;
        }

        /* ── Accordion / collapsible panel ── */
        .card { border: 1px solid #edebe9; border-radius: 3px; margin-bottom: 6px; box-shadow: var(--card-shadow); }
        .card-header {
            background: #faf9f8;
            padding: 7px 12px;
            border-bottom: 1px solid #edebe9;
        }
        .card-header a {
            font-size: 0.875rem;
            color: #323130 !important;
            text-decoration: none;
        }
        .card-body { padding: 14px 16px; }
        .small-icon { font-size: 0.75rem; vertical-align: middle; }

        /* ── Grid table styling ── */
        .table-condensed th,
        .table-condensed td {
            font-size: 0.78rem;
            padding: 6px 10px;
            white-space: nowrap;
            vertical-align: middle;
        }
      /*  .table-condensed th {
            background: #f3f2f1;
            color: #323130;
            font-weight: 600;
            border-bottom: 2px solid #d0d0d0;
        }
        .table-hover tbody tr:hover { background-color: #f0f6ff; }*/

        /* ── Scrollable grid wrapper ── */
        .table-responsive {
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
            border: 1px solid #edebe9;
            border-radius: 3px;
        }
        .table-responsive table {
            margin-bottom: 0;
            white-space: nowrap;
        }

        /* ── Delete Line Button Style (matching action pane) ── */
        .action-items-line {
            margin-bottom: 10px;
            text-align: left;
        }
        .action-items-line .btn-delete-line {
            display: inline-block;
            padding: 6px 12px;
            background-color: #f3f2f1;
            color: #323130;
            text-decoration: none;
            border: 1px solid #d0d0d0;
            border-radius: 2px;
            font-size: 0.8125rem;
            cursor: pointer;
        }
        .action-items-line .btn-delete-line:hover {
            background-color: #e1dfdd;
        }
        .action-items-line .btn-delete-line i {
            margin-right: 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/TA/RosterChangeRequest_Create.aspx')">
            <i class="mdi mdi-plus"></i>Roster Change Request
        </asp:LinkButton>
        </div>
     <div class="action-items">
        <asp:LinkButton ID="btnDeleteHeader" runat="server" OnClick="btnDeleteHeader_Click" Enabled="false"> 
            <i class="mdi mdi-delete"></i>Delete 
        </asp:LinkButton>
         </div>

        <div class="action-items">
     <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
   
    <div class="card-body">
        <div style="margin-bottom: 10px;">
            <span style="margin-right: 5px;">From Date:</span>
            <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" autocomplete="off"></asp:TextBox>

            <span style="margin-left: 5px;">To Date:</span>
            <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" autocomplete="off"></asp:TextBox>

            <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" Style="margin-left: 10px;" />
        </div>

        <asp:GridView ID="gvRosterChangeRequests"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-condensed no-bordered table-hover sortable"
            Width="100%"
            DataKeyNames="RecId"
            ShowHeaderWhenEmpty="true" 
            EmptyDataText="No Record Found."
            OnRowDataBound="gridView_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle"
                            OnCheckedChanged="chk_SelectSingle_CheckedChanged" 
                            AutoPostBack="true"
                            runat="server" 
                            CssClass="round-checkbox" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="TAS Roster Request Id">
                    <ItemTemplate>
                        <asp:Label ID="lblRosterRequestId" runat="server"
                            Text='<%# Eval("tASRosterRequestId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestDate" runat="server"
                            Text='<%# FormatDate(Eval("requestDate")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Personnel Number">
                    <ItemTemplate>
                        <asp:Label ID="lblPersonnelNumber" runat="server"
                            Text='<%# Eval("employeeId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Employee Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server"
                            Text='<%# Eval("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="From Date">
                    <ItemTemplate>
                        <asp:Label ID="lblFromDate" runat="server"
                            Text='<%# FormatDate(Eval("fromDate")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="To Date">
                    <ItemTemplate>
                        <asp:Label ID="lblToDate" runat="server"
                            Text='<%# FormatDate(Eval("toDate")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Workflow Status">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkflowStatus" runat="server"
                            Text='<%# Eval("TASWFStatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="RecId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRecId" runat="server"
                            Text='<%# Eval("RecId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <%-- ======================= COLLAPSIBLE PANEL START: "Roster Record" ======================= --%>
        <div class="accordion mt-3" id="rosterRecordAccordion">
            <div class="card">
                <div class="card-header" id="headingRosterRecord">
                    <a class="text-dark d-flex justify-content-between w-100" data-toggle="collapse" href="#collapseRosterRecord" role="button" aria-expanded="true" aria-controls="collapseRosterRecord">
                        <strong>Roster Record</strong>
                        <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                    </a>
                </div>
                <div id="collapseRosterRecord" class="collapse show" aria-labelledby="headingRosterRecord">
                    <div class="card-body">
                        <%-- Delete Line Button - Left side, matching action pane style --%>
                        <div class="action-items-line">
                            <asp:LinkButton ID="btnDeleteLine" runat="server" 
                                OnClick="btnDeleteLine_Click" 
                                CssClass="btn-delete-line"
                                 Enabled="false"
                               >
                                <i class="mdi mdi-delete"></i>Delete 
                            </asp:LinkButton>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="gvRosterRecord"
                                runat="server"
                                AutoGenerateColumns="False"
                                CssClass="table table-condensed no-bordered table-hover sortable"
                                Width="100%"
                                DataKeyNames="RecId"
                                ShowHeaderWhenEmpty="true" 
                                EmptyDataText="No Record Found.">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_SelectLine"
                                                OnCheckedChanged="chk_SelectLine_CheckedChanged" 
                                                AutoPostBack="true"
                                                runat="server" 
                                                CssClass="round-checkbox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Personnel Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRRPersonnelNumber" runat="server"
                                                Text='<%# Eval("employeeId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Employee Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRREmployeeName" runat="server"
                                                Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Shift Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShiftId" runat="server"
                                                Text='<%# Eval("ShiftId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShiftDate" runat="server"
                                                Text='<%# FormatDate(Eval("ShiftDate")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRRType" runat="server"
                                                Text='<%# Eval("Type") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Shift Hours">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShiftHours" runat="server"
                                                Text='<%# SecondsToHoursFormat(Eval("ShiftHours")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Required Working Hours">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequiredWorkingHours" runat="server"
                                                Text='<%# SecondsToHoursFormat(Eval("RequiredWorkingHours")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Min. Working Hour">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMinWorkingHour" runat="server"
                                                Text='<%# SecondsToHoursFormat(Eval("MinimumWorkingHour")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Off Day">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffDay" runat="server"
                                                Text='<%# Eval("OffDay") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Generation Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGenerationType" runat="server"
                                                Text='<%# Eval("GenerationType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="RecId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecIdLine" runat="server"
                                                Text='<%# Eval("RecId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- ======================= COLLAPSIBLE PANEL END ======================= --%>
    </div>
</asp:Content>