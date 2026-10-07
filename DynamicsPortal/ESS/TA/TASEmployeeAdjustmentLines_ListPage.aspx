<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TASEmployeeAdjustmentLines_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.TA.TASEmployeeAdjustmentLines_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
    .grid-size {
        width: 100%;
        min-width: max-content;
    }

    .grid-size th,
    .grid-size td {
        white-space: nowrap;
    }

    /* Make the grid container scrollable horizontally instead of overflowing */
    .table-responsive-wrapper {
        width: 100%;
        overflow-x: auto;
    }
</style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
<asp:UpdatePanel ID="updButtons" runat="server">
    <ContentTemplate>
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/TA/TASEmployeeAdjustments_Create.aspx')">
         <i class="mdi mdi-plus"></i>Create Employee Adjustment
        </asp:LinkButton>
    </div>

    <div class="action-items">
        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
    </div>
                      </ContentTemplate>
</asp:UpdatePanel>  
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
<ContentTemplate>
    <div style="margin-bottom: 10px;">
        <!-- Filter Date Range -->
        <span style="margin-right: 5px;">From Date:</span>
        <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" autocomplete="off"></asp:TextBox>

        <span style="margin-left: 10px; margin-right: 5px;">To Date:</span>
        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" autocomplete="off"></asp:TextBox>

        <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" Style="margin-left: 10px;" />
    </div>


    <%--   <div style="margin-top: 5px;">
        <embed id="embed01" runat="server" type="application/pdf" height="700" width="850" />
    </div>--%>
        <div class="table-responsive-wrapper">
        <asp:GridView ID="gridView" runat="server" CssClass="table table-condensed no-border table-hover sortable grid-size" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames=""
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">

            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                        <asp:Label ID="lblRecId" runat="server" Text='<%# Eval("RecId") %>' Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>

                <%--<asp:TemplateField HeaderText="Employee ID">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("EmployeeId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>

                <asp:TemplateField HeaderText="Employee ID">
    <ItemTemplate>
        <asp:LinkButton 
            ID="lnkEmployeeId" 
            runat="server" 
            Text='<%# Eval("EmployeeId") %>' 
            CommandArgument='<%# Eval("EmployeeId") %>' 
            OnClick="lnkEmployeeId_Click" />
    </ItemTemplate>
</asp:TemplateField>


                <asp:TemplateField HeaderText="Employee Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Shift ID">
                    <ItemTemplate>
                        <asp:Label ID="lblShiftId" runat="server" Text='<%# Bind("ShiftId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                
                <asp:TemplateField HeaderText="Date">
    <ItemTemplate>
        <asp:Label ID="lblDate" runat="server"
            Text='<%# FormatDates(Eval("AttendanceDate")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtDate" runat="server"
            Text='<%# Bind("AttendanceDate", "{0:M/dd/yyyy}") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>


                 <asp:TemplateField HeaderText="Attendance Day">
     <ItemTemplate>
         <asp:Label ID="lblAttendanceDay" runat="server" Text='<%# Bind("day") %>'></asp:Label>
     </ItemTemplate>
 </asp:TemplateField>

                <asp:TemplateField HeaderText="Clock In Date Time" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblDateTimeIn" runat="server"
            Text='<%# FormatDateTime(Eval("DateTimeIn")) %>' />
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtDateTimeIn" runat="server"
            Text='<%# FormatDateTime(Eval("DateTimeIn")) %>'
            autocomplete="off" />
    </EditItemTemplate>
</asp:TemplateField>


             

                <asp:TemplateField HeaderText="Clock In Date" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblClockInDate" runat="server"
            Text='<%# FormatDate(Eval("ClockInDate")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtClockInDate" runat="server"
            Text='<%# Bind("ClockInDate", "{0:MM/dd/yyyy}") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>




                <asp:TemplateField HeaderText="Clock In Time" Visible="false">
                    <ItemTemplate>
                        <asp:Label
                            ID="lblClockIn"
                            runat="server"
                            Text='<%# FormatTime(Eval("TimeIn")) %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <!-- keep Bind() here so the raw value goes back on update -->
                        <asp:TextBox
                            ID="txtTimeIn"
                            runat="server"
                            Text='<%# Bind("TimeIn") %>'
                            autocomplete="off" />
                    </EditItemTemplate>
                </asp:TemplateField>

          

                <asp:TemplateField HeaderText="Clock Out Date Time" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblDateTimeOut" runat="server"
            Text='<%# FormatDateTime(Eval("DateTimeOut")) %>' />
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtDateTimeOut" runat="server"
            Text='<%# FormatDateTime(Eval("DateTimeOut")) %>'
            autocomplete="off" />
    </EditItemTemplate>
</asp:TemplateField>



                <asp:TemplateField HeaderText="Clock Out Date" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblClockOutDate" runat="server"
            Text='<%# FormatDate(Eval("ClockOutDate")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtClockOutDate" runat="server"
            Text='<%# Bind("ClockOutDate", "{0:MM/dd/yyyy}") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>




                <asp:TemplateField HeaderText="Clock Out Time" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblClockOut" runat="server"
                            Text='<%# FormatTime(Eval("TimeOut")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtClockOut" runat="server"
                            Text='<%# Bind("TimeOut") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actual Working Hours" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lbl" runat="server"
                            Text='<%# FormatTimeNew(Eval("TotalWorkingHours")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTotalWorkingHours" runat="server"
                            Text='<%# Bind("TotalWorkingHours") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Calculated Working Hours" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCalculatedWorkingHours" runat="server"
                            Text='<%# FormatTimeNew(Eval("CalculatedWorkingHours")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCalculatedWorkingHours" runat="server"
                            Text='<%# Bind("CalculatedWorkingHours") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>



                <%--         <asp:TemplateField HeaderText="Shift Day">
      <ItemTemplate>
          <asp:Label ID="lblShiftDate" runat="server" Text='<%# Bind("SHIFTDATE") %>' masktype="date"></asp:Label>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtShiftDate" runat="server" Text='<%# Bind("SHIFTDATE") %>' autocomplete="off" masktype="date"></asp:TextBox>
      </EditItemTemplate>
  </asp:TemplateField>--%>

                <%--   <asp:TemplateField HeaderText="Type">
                    <ItemTemplate>
                        <asp:Label ID="lblType" runat="server" Text='<%# Bind("TYPE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtType" runat="server" Text='<%# Bind("TYPE") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                
                <asp:TemplateField HeaderText="Attendance Status">
                    <ItemTemplate>
                        <asp:Label ID="lblAttendanceStatus" runat="server" Text='<%# Bind("AttendanceRegisterStatus") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAttendanceStatus" runat="server" Text='<%# Bind("AttendanceRegisterStatus") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Generation Type">
                    <ItemTemplate>
                        <asp:Label ID="lblGenerationType" runat="server" Text='<%# Bind("GENERATIONTYPE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGenerationType" runat="server" Text='<%# Bind("GENERATIONTYPE") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>





                <%--                <asp:TemplateField HeaderText="Location">
    <ItemTemplate>
        <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("LOCATION") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtLocation" runat="server" Text='<%# Bind("LOCATION") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>--%>



                <%--<asp:TemplateField HeaderText="Gazetted Day">
    <ItemTemplate>
        <asp:Label ID="lblGazettedDay" runat="server" Text='<%# Bind("GAZETTEDDAY") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtGazettedDay" runat="server" Text='<%# Bind("GAZETTEDDAY") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>--%>



                <asp:TemplateField HeaderText="WF Status">
                    <ItemTemplate>
                        <asp:Label ID="lblWFSTATUS" runat="server" Text='<%# Bind("WFSTATUS") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtWFSTATUS" runat="server" Text='<%# Bind("WFSTATUS") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Remarks" Visible="false">
      <ItemTemplate>
          <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:TextBox>
      </EditItemTemplate>
  </asp:TemplateField>






                <asp:BoundField DataField="DATAAREAID" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
                <asp:BoundField DataField="PARTITION" HeaderText="Partition" SortExpression="Partition" Visible="false" />
                <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
                <asp:BoundField DataField="RECID" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />

            </Columns>
        </asp:GridView>
    </div>
     </ContentTemplate>
          </asp:UpdatePanel>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            // Set the indexes of the columns you want to format (0-based)
            var columnIndexes = [5, 8];

            $('#<%= gridView.ClientID %> tr').each(function () {
                columnIndexes.forEach(function (colIndex) {
                    var td = $(this).find('td').eq(colIndex);

                    // If the cell has a span or label inside, update that
                    var inner = td.find('span, label');
                    if (inner.length > 0) {
                        var text = inner.text();
                        if (text.includes(" ")) {
                            inner.text(text.split(" ")[0]); // Keep only date part
                        }
                    } else {
                        // Fallback: update plain cell text
                        var rawText = td.text();
                        if (rawText.includes(" ")) {
                            td.text(rawText.split(" ")[0]);
                        }
                    }
                }, this); // Bind "this" to the current row
            });
        });
    </script>
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
</script>

</asp:Content>