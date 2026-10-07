<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TASEmployeeRoster_ListPage.aspx.cs" Inherits="DynamicsPortal.TASEmployeeRoster_ListPage" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
<style>
    .grid-size{
        max-width:max-content;
        min-width: max-content;
    }
</style>
</asp:Content>
<%--<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/TA/TASEmployeeRoster_Create.aspx')">
            <i class="mdi mdi-plus"></i>New
        </asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click">
            <i class="mdi mdi-delete"></i>Delete
        </asp:LinkButton>
    </div>
</asp:Content>--%>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
<asp:UpdatePanel ID="updButtons" runat="server">
    <ContentTemplate>
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/TA/TASEmployeeRoster_Create.aspx')">
            <i class="mdi mdi-plus"></i>New
        </asp:LinkButton>
    </div>

    <div class="action-items">
        <asp:HiddenField ID="hdnSelectedRecId" runat="server" Visible="false" />

        <asp:LinkButton ID="btnEdit" runat="server" OnClientClick="return openEditPopup();">
            <i class="mdi mdi-pencil"></i> Edit
        </asp:LinkButton>
    </div>

    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected roster record(s)? This action cannot be undone.');">
            <i class="mdi mdi-delete"></i>Delete
        </asp:LinkButton>
    </div>

    <script>
        function openEditPopup() {
            var selectedIds = [];
            // Loop through GridView rows server-side (if possible)
          <%= GetGridViewRowSelectionScript() %>
   <%-- <%= GetGridViewRowSelectionScript() %>--%>

            if (selectedIds.length > 0) {
                openPopupPanel('/ESS/TA/TASEmployeeRoster_Edit.aspx?RecId=' + selectedIds[0]);
            } else {
                alert("Please select a record to edit.");
            }
            return false;
        }
    </script>
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

    <span style="margin-left:  5px;">To Date:</span>
    <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" autocomplete="off"></asp:TextBox>
   
    <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" Style="margin-left: 10px;" />
</div>


 <%--   <div style="margin-top: 5px;">
        <embed id="embed01" runat="server" type="application/pdf" height="700" width="850" />
    </div>--%>

    <div>
        <asp:GridView ID="gridView" runat="server" CssClass="table table-condensed no-border table-hover sortable grid-size" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
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

                <asp:TemplateField HeaderText="    Shift Code    ">
                    <ItemTemplate>
                        <asp:Label ID="lblShiftCode" runat="server" Text='<%# Bind("SHIFTCODE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtShiftCode" runat="server" Text='<%# Bind("SHIFTCODE") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                           <%--    <asp:TemplateField HeaderText="Date ">
    <ItemTemplate>
        <asp:Label ID="lblShiftDate" runat="server" 
                   Text='<%# Eval("ShiftDate", "{0:MM/dd/yyyy}") %>' />
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtShiftDate" runat="server" 
                     Text='<%# Bind("ShiftDate", "{0:MM/dd/yyyy}") %>' 
                     autocomplete="off" />
    </EditItemTemplate>
</asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Date">
    <ItemTemplate>
        <asp:Label ID="lblShiftDate" runat="server"
            Text='<%# FormatDate(Eval("ShiftDate")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtShiftDate" runat="server"
            Text='<%# Bind("ShiftDate", "{0:MM/dd/yyyy}") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>


<%--          <asp:BoundField DataField="ShiftDate" 
                DataFormatString="{0:MM/dd/yyyy}" 
                HtmlEncode="false" 
                HeaderText="Date" />--%>

 
             <%--   <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblShiftDate" runat="server" Text='<%# Bind("SHIFTDATE") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtShiftDate" runat="server" Text='<%# Bind("SHIFTDATE") %>' autocomplete="off" masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>--%>
           
         

             <asp:TemplateField HeaderText="Shift Day">
       <ItemTemplate>
           <asp:Label ID="lblShiftDay" runat="server" Text='<%# Bind("ShiftDay") %>'></asp:Label>
       </ItemTemplate>
   </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Type">
                    <ItemTemplate>
                        <asp:Label ID="lblType" runat="server" Text='<%# Bind("TYPE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtType" runat="server" Text='<%# Bind("TYPE") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
<asp:TemplateField HeaderText="Flex Clock In Start Time">
  <ItemTemplate>
    <asp:Label
      ID="lblFlexClockInStartTime"
      runat="server"
      Text='<%# FormatTime(Eval("FLEXCLOCKINSTARTTIME")) %>' />
  </ItemTemplate>
  <EditItemTemplate>
    <!-- keep Bind() here so the raw value goes back on update -->
    <asp:TextBox
      ID="txtFlexClockInStartTime"
      runat="server"
      Text='<%# Bind("FLEXCLOCKINSTARTTIME") %>'
      autocomplete="off" />
  </EditItemTemplate>
</asp:TemplateField>


<asp:TemplateField HeaderText="Time In">
    <ItemTemplate>
        <asp:Label ID="lblShiftStartTime" runat="server" 
            Text='<%# FormatTime(Eval("SHIFTSTARTTIME")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtShiftStartTime" runat="server" 
            Text='<%# Bind("SHIFTSTARTTIME") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Flex Clock In End Time">
    <ItemTemplate>
        <asp:Label ID="lblFLEXCLOCKINENDTIME" runat="server" 
            Text='<%# FormatTime(Eval("FLEXCLOCKINENDTIME")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtFLEXCLOCKINENDTIME" runat="server" 
            Text='<%# Bind("FLEXCLOCKINENDTIME") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Flex Clock Out Start Time">
    <ItemTemplate>
        <asp:Label ID="lblFlexClockOutStartTime" runat="server" 
            Text='<%# FormatTime(Eval("FLEXCLOCKOUTSTARTTIME")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtFlexClockOutStartTime" runat="server" 
            Text='<%# Bind("FLEXCLOCKOUTSTARTTIME") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Time Out">
    <ItemTemplate>
        <asp:Label ID="lblShiftEndTime" runat="server" 
            Text='<%# FormatTime(Eval("SHIFTENDTIME")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtShiftEndTime" runat="server" 
            Text='<%# Bind("SHIFTENDTIME") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Flex Clock Out End Time">
    <ItemTemplate>
        <asp:Label ID="lblFLEXCLOCKOUTENDTIME" runat="server" 
            Text='<%# FormatTime(Eval("FLEXCLOCKOUTENDTIME")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtFLEXCLOCKOUTENDTIME" runat="server" 
            Text='<%# Bind("FLEXCLOCKOUTENDTIME") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

          <asp:TemplateField HeaderText="Break Start Time">
    <ItemTemplate>
        <asp:Label ID="lblBreakStartTime" runat="server" 
            Text='<%# FormatTime(Eval("BreakStartTime")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtBreakStartTime" runat="server" 
            Text='<%# Bind("BreakStartTime") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>


            <asp:TemplateField HeaderText="Break End Time">
    <ItemTemplate>
        <asp:Label ID="lblBreakEndTime" runat="server" 
            Text='<%# FormatTime(Eval("BreakEndTime")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtBreakStartTime" runat="server" 
            Text='<%# Bind("BreakEndTime") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Shift Hours">
    <ItemTemplate>
        <asp:Label ID="lblShiftHours" runat="server" 
            Text='<%# FormatTimeNew(Eval("SHIFTHOURS")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtShiftHours" runat="server" 
            Text='<%# Bind("SHIFTHOURS") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Required Working Hours">
    <ItemTemplate>
        <asp:Label ID="lblRequiredWorkingHours" runat="server" 
            Text='<%# FormatTimeNew(Eval("REQUIREDWORKINGHOURS")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtRequiredWorkingHours" runat="server" 
            Text='<%# Bind("REQUIREDWORKINGHOURS") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Min. Working Hour">
    <ItemTemplate>
        <asp:Label ID="lblMinimumWorkingHour" runat="server" 
            Text='<%# FormatTimeNew(Eval("MINIMUMWORKINGHOUR")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtMinimumWorkingHour" runat="server" 
            Text='<%# Bind("MINIMUMWORKINGHOUR") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

                
                <asp:TemplateField HeaderText="Off Day">
                    <ItemTemplate>
                        <asp:Label ID="lblOffDay" runat="server" Text='<%# Bind("OFFDAY") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOffDay" runat="server" Text='<%# Bind("OFFDAY") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
             


<%--<asp:TemplateField HeaderText="Shift Break Time">
    <ItemTemplate>
        <asp:Label ID="lblShiftBreakTime" runat="server" 
            Text='<%# FormatTime(Eval("SHIFTBREAKTIME")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtShiftBreakTime" runat="server" 
            Text='<%# FormatTime(Eval("SHIFTBREAKTIME")) %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Shift Grace Time">
    <ItemTemplate>
        <asp:Label ID="lblShiftGraceTime" runat="server" 
            Text='<%# FormatTime(Eval("SHIFTGRACETIME")) %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtShiftGraceTime" runat="server" 
            Text='<%# FormatTime(Eval("SHIFTGRACETIME")) %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>


                <asp:TemplateField HeaderText="Location">
    <ItemTemplate>
        <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("LOCATION") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtLocation" runat="server" Text='<%# Bind("LOCATION") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>--%>



<asp:TemplateField HeaderText="Gazetted Day">
    <ItemTemplate>
        <asp:Label ID="lblGazettedDay" runat="server" Text='<%# Bind("GAZETTEDDAY") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtGazettedDay" runat="server" Text='<%# Bind("GAZETTEDDAY") %>' autocomplete="off"></asp:TextBox>
    </EditItemTemplate>
</asp:TemplateField>


                <asp:TemplateField HeaderText="Festival Holiday">
    <ItemTemplate>
        <asp:Label ID="lblFestivalHolidDay" runat="server" Text='<%# Bind("FestivalHoliday") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtFestivalHoliDay" runat="server" Text='<%# Bind("FestivalHoliday") %>' autocomplete="off"></asp:TextBox>
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


            
              

<%--                <asp:TemplateField HeaderText="Worker">
                    <ItemTemplate>
                        <asp:Label ID="lblWorker" runat="server" Text='<%# Bind("WORKER") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtWorker" runat="server" Text='<%# Bind("WORKER") %>' autocomplete="off"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                --%>



                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <%--<asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />--%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="DATAAREAID" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                <asp:BoundField DataField="PARTITION" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                <asp:BoundField DataField="RECID" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
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
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
  <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
 <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script>
    $(document).ready(function () {
        // Adjust this to your actual GridView's column index (starts at 0)
        var columnIndex = 5;

        $('#<%= gridView.ClientID %> tr').each(function () {
            var td = $(this).find('td').eq(columnIndex);
            // If the cell has a span or label inside, update that
            var inner = td.find('span, label');
            if (inner.length > 0) {
                var text = inner.text();
                if (text.includes(" ")) {
                    inner.text(text.split(" ")[0]); // Keep only date
                }
            } else {
                // Fallback: update plain cell text
                var rawText = td.text();
                if (rawText.includes(" ")) {
                    td.text(rawText.split(" ")[0]);
                }
            }
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

