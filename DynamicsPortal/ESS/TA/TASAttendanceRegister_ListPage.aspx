<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TASAttendanceRegister_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.TA.TASAttendanceRegister_ListPage" %>
<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .wide-textbox {
            width: 120px;
        }
        .filter-container {
            margin-bottom: 15px;
            padding: 10px;
            background-color: #f5f5f5;
            border-radius: 4px;
        }
        .filter-item {
            display: inline-block;
            margin-right: 15px;
        }
    </style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>
            <div class="action-items">
                <asp:LinkButton ID="btnEditAttendance" runat="server" OnClick="btnEditAttendance_Click" Style="margin-left: 10px;" visible="false">
                    <i class="mdi mdi-pencil"></i>Edit Record
                </asp:LinkButton>
            </div>
            <div class="action-items">
                <asp:LinkButton ID="btnMarkAttendance" runat="server" OnClick="btnMarkAttendance_Click" Style="margin-left: 10px;" Visible="false">
                    <i class="mdi mdi-plus"></i>Mark Attendance
                </asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="filter-container">
                <div class="filter-item">
                    <span>From Date:</span>
                    <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" autocomplete="off"></asp:TextBox>
                </div>
                <div class="filter-item">
                    <span>To Date:</span>
                    <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" autocomplete="off"></asp:TextBox>
                </div>
                <div class="filter-item">
                    <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click"  />
                   
                </div>
            </div>

            <div style="overflow-x: auto;">
                <asp:GridView ID="gridView" runat="server" 
                    CssClass="table table-condensed no-border table-hover sortable" 
                    ShowHeaderWhenEmpty="true" 
                    EmptyDataText="No Record Found." 
                    DataKeyNames="RosterRefRecId"
                    OnRowEditing="gridView_RowEditing" 
                    OnRowDataBound="gridView_RowDataBound" 
                    AutoGenerateColumns="false"
                    AllowPaging="false">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <HeaderTemplate>
                                <input type="checkbox" id="chk_SelectAll" onclick="SelectAllCheckboxes(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle" runat="server" CssClass="row-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Employee ID">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Eval("EMPLOYEEID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Eval("EMPLOYEENAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Attendance Date">
                            <ItemTemplate>
                                <asp:Label ID="lblAttendanceDate" runat="server" Text='<%# FormatDate(Eval("AttendanceDate")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Attendance Day">
                            <ItemTemplate>
                                <asp:Label ID="lblAttendanceDay" runat="server" Text='<%# Eval("AttendanceDay") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Shift ID">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftId" runat="server" Text='<%# Eval("ShiftId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Shift Start Time">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftStartTime" runat="server" Text='<%# FormatTime(Eval("ShiftStartTime")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Shift End Time">
                            <ItemTemplate>
                                <asp:Label ID="lblShiftEndTime" runat="server" Text='<%# FormatTime(Eval("ShiftEndTime")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Clock In Date">
                            <ItemTemplate>
                                <asp:Label ID="lblClockInDate" runat="server" Text='<%# FormatDate(Eval("ClockInDate")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Clock In">
                            <ItemTemplate>
                                <asp:Label ID="lblClockIn" runat="server" Text='<%# FormatTime(Eval("ClockIn")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Clock Out Date">
                            <ItemTemplate>
                                <asp:Label ID="lblClockOutDate" runat="server" Text='<%# FormatDate(Eval("ClockOutDate")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Clock Out">
                            <ItemTemplate>
                                <asp:Label ID="lblClockOut" runat="server" Text='<%# FormatTime(Eval("ClockOut")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Late Arrival">
                            <ItemTemplate>
                                <asp:Label ID="lblLateArrival" runat="server" Text='<%# Eval("LateArrival") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Late Arrival Time">
                            <ItemTemplate>
                                <asp:Label ID="lblLateArrivalTime" runat="server" Text='<%# FormatTime(Eval("LateArrivalTime")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Early Out">
                            <ItemTemplate>
                                <asp:Label ID="lblEarlyOut" runat="server" Text='<%# Eval("EarlyOut") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Early Out Time">
                            <ItemTemplate>
                                <asp:Label ID="lblEarlyOutTime" runat="server" Text='<%# FormatTime(Eval("EarlyOutTime")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Absent">
                            <ItemTemplate>
                                <asp:Label ID="lblAbsent" runat="server" Text='<%# Eval("Absent") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Half Absent">
                            <ItemTemplate>
                                <asp:Label ID="lblAbsentHalf" runat="server" Text='<%# Eval("AbsentHalf") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Calculated Working Hours">
                            <ItemTemplate>
                                <asp:Label ID="lblCalculatedWorkingHours" runat="server" Text='<%# FormatTime(Eval("ShiftFlexibleWorkingTime")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Off Day">
                            <ItemTemplate>
                                <asp:Label ID="lblOffDay" runat="server" Text='<%# Eval("OffDay") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gazetted Day">
                            <ItemTemplate>
                                <asp:Label ID="lblGazettedDay" runat="server" Text='<%# Eval("GazettedDay") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Festival Day">
                            <ItemTemplate>
                                <asp:Label ID="lblFestivalDay" runat="server" Text='<%# Eval("FestivalHoliday") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="RosterRefRecId" HeaderText="RosterRefRecId" Visible="false" />
                        <asp:BoundField DataField="DATAAREAID" HeaderText="DataAreaId" Visible="false" />
                        <asp:BoundField DataField="PARTITION" HeaderText="Partition" Visible="false" />
                        <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" Visible="false" />
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function SelectAllCheckboxes(headerCheckbox) {
            var gridView = document.getElementById('<%= gridView.ClientID %>');
            var checkboxes = gridView.getElementsByTagName('input');

            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type === 'checkbox' && checkboxes[i].id.indexOf('chk_SelectSingle') > -1) {
                    checkboxes[i].checked = headerCheckbox.checked;
                }
            }
        }
    </script>
</asp:Content>