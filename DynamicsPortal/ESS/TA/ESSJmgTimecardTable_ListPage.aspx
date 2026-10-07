<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSJmgTimecardTable_ListPage.aspx.cs" Inherits="DynamicsPortal.ESSJmgTimecardTable_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
    </div>
<%--    <div class="action-items" >
        <asp:LinkButton ID="btnUpdateAttendance" runat="server" ToolTip="Update Location Attendance" OnClick="btnUpdateAttendance_Click"><i class="mdi mdi-shape-plus"></i>Update Attendance</asp:LinkButton>
    </div>--%>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">

    <div style="display: inline-block; font-size: 10px;">
        <span style="margin-right: 5px;">From Date</span>
        <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" OnTextChanged="Date_TextChanged" autocomplete="off"  masktype="date"></asp:TextBox>

        <span style="margin-right: 5px;">To Date</span>
        <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="true" OnTextChanged="Date_TextChanged" autocomplete="off"  masktype="date"></asp:TextBox>
    </div>

    <div style="padding-top: 6px;">
        <%--        <div class="mCustomScrollbar" data-mcs-axis="yx" data-mcs-theme="dark" style="position: absolute; left: 0%; width: 55%; height: 100%;">--%>
        <asp:GridView ID="gridView_TimecardTable" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" Style="font-size: 9px;"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId" AutoGenerateColumns="false"
            OnRowDataBound="gridView_TimecardTable_RowDataBound" OnRowEditing="gridView_TimecardTable_RowEditing">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Emp Id">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("EmployeeId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Emp Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblProfileDate" runat="server" Text='<%# Bind("ProfileDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Profile">
                    <ItemTemplate>
                        <asp:Label ID="lblProfileId" runat="server" Text='<%# Bind("ProfileId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Profile Details">
                    <ItemTemplate>
                        <asp:Label ID="lblProfileDetails" runat="server" Text='<%# Bind("ProfileDetails") %>'></asp:Label>
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


                <asp:TemplateField HeaderText="Actul In-Time" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblClockInActualDateTime" runat="server" Text='<%# setDateTimeFormat(Eval("ClockInActualDateTime").ToString()) %>' masktype="datetime"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Suggested In-Time">
                    <ItemTemplate>
                        <asp:Label ID="lblClockInDateTime" runat="server" Text='<%# setDateTimeFormat(Eval("ClockInDateTime").ToString()) %>' masktype="datetime"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtClockInDateTime" runat="server" Text='<%# setDateTimeFormat(Eval("ClockInDateTime").ToString()) %>'  autocomplete="off" masktype="datetime"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Generation Type" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblClockInGenerationType" runat="server" Text='<%# Bind("ClockInGenerationType") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reason Code">
                    <ItemTemplate>
                        <asp:Label ID="lblClockInReasonCodeId" runat="server" Text='<%# Bind("ClockInReasonCodeId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlClockInReasonCodeId" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ClockInRecId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblClockInRecId" runat="server" Text='<%# Bind("ClockInRecId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Acutal Out-Time" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblClockOutActualDateTime" runat="server" Text='<%# setDateTimeFormat(Eval("ClockOutActualDateTime").ToString()) %>' masktype="datetime"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Suggested Out-Time">
                    <ItemTemplate>
                        <asp:Label ID="lblClockOutDateTime" runat="server" Text='<%# setDateTimeFormat(Eval("ClockOutDateTime").ToString()) %>' masktype="datetime"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtClockOutDateTime" runat="server" Text='<%# setDateTimeFormat(Eval("ClockOutDateTime").ToString()) %>' autocomplete="off" masktype="datetime"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Generation Type" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblClockOutGenerationType" runat="server" Text='<%# Bind("ClockOutGenerationType") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reason Code">
                    <ItemTemplate>
                        <asp:Label ID="lblClockOutReasonCodeId" runat="server" Text='<%# Bind("ClockOutReasonCodeId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlClockOutReasonCodeId" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ClockOutRecId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblClockOutRecId" runat="server" Text='<%# Bind("ClockOutRecId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>



                <asp:TemplateField HeaderText="Locked" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRegistrationsLocked" Enabled="false" runat="server" Text='<%# Bind("RegistrationsLocked") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Transferred" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRegistrationsTransferred" Enabled="false" runat="server" Text='<%# Bind("RegistrationsTransferred") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Calculated" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRegistrationsCalculated" Enabled="false" runat="server" Text='<%# Bind("RegistrationsCalculated") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblWFStatus" runat="server" Text='<%# Bind("WFStatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Worker" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblWorker" runat="server" Text='<%# Bind("Worker") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RecId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Save" Text="Save" runat="server" OnClick="Update_TimecardTable_Click" />
                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_TimecardTable_Click" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
                <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
                <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false" />
            </Columns>
        </asp:GridView>
        <%--        </div>--%>
        <%--        <div style="position: absolute; left: 55%; width: 45%; height: 100%;">
            <div class="divTable">
                <div class="divTableBody">
                    <div class="divTableRow">
                        <div class="divTableCell">
                            <div class="action-panel-grid">
                                <div class="action-items-grid">
                                    <asp:LinkButton ID="btnNew" runat="server" Enabled="false" OnClick="btnNew_TimecardTrans_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
                                </div>
                                <div class="action-items-grid">
                                    <asp:LinkButton ID="btnDelete" runat="server" Enabled="false" OnClick="btnDelete_TimecardTrans_Click"><i class="mdi mdi-delete"></i>delete</asp:LinkButton>
                                </div>
                            </div>
                            <asp:GridView ID="gridView_TimecardTrans" runat="server" CssClass="table table-condensed no-border table-hover sortable" style="font-size: 9px;"
                                ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId" AutoGenerateColumns="false"
                                OnRowDataBound="gridView_TimecardTrans_RowDataBound" OnRowEditing="gridView_TimecardTrans_RowEditing">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                        <HeaderTemplate>
                                            <input type="checkbox" id="chk_SelectAll" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Profile Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJourRegType" runat="server" Text='<%# Bind("JourRegType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlJourRegType" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Profile Date" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProfileDate" runat="server" Text='<%# Bind("ProfileDate") %>' masktype="date"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Actual Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActualDateTime" runat="server" Text='<%# setDateTimeFormat(Eval("ActualDateTime").ToString()) %>' masktype="datetime"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Suggested Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStartDateTime" runat="server" Text='<%# setDateTimeFormat(Eval("StartDateTime").ToString()) %>' masktype="datetime"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtStartDateTime" runat="server" Text='<%# setDateTimeFormat(Eval("StartDateTime").ToString()) %>' masktype="datetime"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Generation Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGenerationType" runat="server" Text='<%# Bind("GenerationType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="StopDateTime" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStopDateTime" runat="server" Text='<%# Bind("StopDateTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtStopDateTime" runat="server" Text='<%# Bind("StopDateTime") %>' masktype="datetime"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Reason Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReasonCodeId" runat="server" Text='<%# Bind("ReasonCodeId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlReasonCodeId" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="RecId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                                        <ItemTemplate>
                                            <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Save" Text="Save" runat="server" OnClick="Update_TimecardTrans_Click" />
                                            <asp:LinkButton CssClass="grid-img-btn btn-submit" ToolTip="Save & Submit" Text="Submit" runat="server" OnClick="UpdateAndSubmit_TimecardTrans_Click" />
                                            <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_TimecardTrans_Click" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                                    <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
                                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                                    <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
                                    <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
                                    <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
    </div>

</asp:Content>
