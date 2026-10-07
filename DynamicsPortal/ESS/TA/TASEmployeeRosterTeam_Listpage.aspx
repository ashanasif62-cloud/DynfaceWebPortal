<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TASEmployeeRosterTeam_Listpage.aspx.cs"
    Inherits="DynamicsPortal.ESS.TA.TASEmployeeRosterTeam_Listpage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .grid-size {
            max-width: max-content;
            min-width: max-content;
        }
    </style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">

    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" 
            OnClientClick="javascript: return openPopupPanel('/ESS/TA/TASEmployeeRoster_Create.aspx')" visible="false">
            <i class="mdi mdi-plus"></i> New
        </asp:LinkButton>
    </div>

    <div class="action-items">
        <asp:LinkButton ID="btnEdit" runat="server" OnClientClick="return openEditPopup();" visible="false">
            <i class="mdi mdi-pencil"></i> Edit
        </asp:LinkButton>
    </div>

    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"
            OnClientClick="return confirm('Are you sure you want to delete the selected roster record(s)? This action cannot be undone.');" visible="false">
            <i class="mdi mdi-delete"></i> Delete
        </asp:LinkButton>
    </div>
    
            <div class="action-items">
                <asp:LinkButton ID="btnBack" runat="server" OnClick="btnBack_Click">
                    <i class="mdi mdi-arrow-left"></i> Back
                </asp:LinkButton>
            </div>

    <script type="text/javascript">
        function openEditPopup() {
            var selectedIds = [];
            <%= GetGridViewRowSelectionScript() %>

            if (selectedIds.length > 0) {
                openPopupPanel('/ESS/TA/TASEmployeeRoster_Edit.aspx?RecId=' + selectedIds[0]);
            } else {
                alert("Please select a record to edit.");
            }
            return false;
        }
    </script>

</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">

    <%-- Required for UpdatePanel --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>

            <div style="margin-bottom: 10px;">
                <span style="margin-right: 5px;">From Date:</span>
                <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" autocomplete="off"></asp:TextBox>

                <span style="margin-left: 5px;">To Date:</span>
                <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" autocomplete="off"></asp:TextBox>

                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" Style="margin-left: 10px;" />
            </div>

            <div>
                <asp:GridView ID="gridView" runat="server"
                    CssClass="table table-condensed no-border table-hover sortable grid-size"
                    ShowHeaderWhenEmpty="true"
                    EmptyDataText="No Record Found."
                    DataKeyNames="RecId"
                    OnRowEditing="gridView_RowEditing"
                    OnRowDataBound="gridView_RowDataBound"
                    AutoGenerateColumns="false">

                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <HeaderTemplate>
                                <input type="checkbox" id="chk_SelectAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Employee ID">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("EMPLOYEEID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EMPLOYEENAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Shift ID">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftId" runat="server" Text='<%# Bind("SHIFTID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Shift Code">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftCode" runat="server" Text='<%# Bind("SHIFTCODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftDate" runat="server"
                                    Text='<%# FormatDate(Eval("ShiftDate")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Shift Day">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftDay" runat="server" Text='<%# Bind("ShiftDay") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("TYPE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Flex Clock In Start Time">
                            <ItemTemplate>
                                <asp:Label ID="lblFlexClockInStartTime" runat="server"
                                    Text='<%# FormatTime(Eval("FLEXCLOCKINSTARTTIME")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Time In">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftStartTime" runat="server"
                                    Text='<%# FormatTime(Eval("SHIFTSTARTTIME")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Flex Clock In End Time">
                            <ItemTemplate>
                                <asp:Label ID="lblFLEXCLOCKINENDTIME" runat="server"
                                    Text='<%# FormatTime(Eval("FLEXCLOCKINENDTIME")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Flex Clock Out Start Time">
                            <ItemTemplate>
                                <asp:Label ID="lblFlexClockOutStartTime" runat="server"
                                    Text='<%# FormatTime(Eval("FLEXCLOCKOUTSTARTTIME")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Time Out">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftEndTime" runat="server"
                                    Text='<%# FormatTime(Eval("SHIFTENDTIME")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Flex Clock Out End Time">
                            <ItemTemplate>
                                <asp:Label ID="lblFLEXCLOCKOUTENDTIME" runat="server"
                                    Text='<%# FormatTime(Eval("FLEXCLOCKOUTENDTIME")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Break Start Time">
                            <ItemTemplate>
                                <asp:Label ID="lblBreakStartTime" runat="server"
                                    Text='<%# FormatTime(Eval("BreakStartTime")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Break End Time">
                            <ItemTemplate>
                                <asp:Label ID="lblBreakEndTime" runat="server"
                                    Text='<%# FormatTime(Eval("BreakEndTime")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Shift Hours">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftHours" runat="server"
                                    Text='<%# FormatTimeNew(Eval("SHIFTHOURS")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Required Working Hours">
                            <ItemTemplate>
                                <asp:Label ID="lblRequiredWorkingHours" runat="server"
                                    Text='<%# FormatTimeNew(Eval("REQUIREDWORKINGHOURS")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Min. Working Hour">
                            <ItemTemplate>
                                <asp:Label ID="lblMinimumWorkingHour" runat="server"
                                    Text='<%# FormatTimeNew(Eval("MINIMUMWORKINGHOUR")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Off Day">
                            <ItemTemplate>
                                <asp:Label ID="lblOffDay" runat="server" Text='<%# Bind("OFFDAY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gazetted Day">
                            <ItemTemplate>
                                <asp:Label ID="lblGazettedDay" runat="server" Text='<%# Bind("GAZETTEDDAY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Festival Holiday">
                            <ItemTemplate>
                                <asp:Label ID="lblFestivalHolidDay" runat="server" Text='<%# Bind("FestivalHoliday") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Generation Type">
                            <ItemTemplate>
                                <asp:Label ID="lblGenerationType" runat="server" Text='<%# Bind("GENERATIONTYPE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

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
            if (typeof showAJAXOverlay === 'function') showAJAXOverlay();
        });
        prm.add_endRequest(function () {
            if (typeof hideAJAXOverlay === 'function') hideAJAXOverlay();
        });

        window.refreshParentGrid = function () {
            __doPostBack('RefreshGrid', '');
        };
    </script>

</asp:Content>