<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSBusinessTrip_ListPage.aspx.cs" Inherits="DynamicsPortal.ESSBusinessTrip_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/HR/ESSHRBusinessTrip_Create.aspx', '980');"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
    <%--    <div class="action-items">
        <asp:LinkButton ID="btnEdit" runat="server"><i class="mdi mdi-border-color"></i>Edit</asp:LinkButton>
    </div>--%>
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
    </div>
    <%--    <div class="action-items">
        <asp:LinkButton ID="btnView" runat="server"><i class="mdi mdi-eye"></i>View</asp:LinkButton>
    </div>--%>
    <div class="action-items">
        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
    </div>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
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
                <asp:TemplateField HeaderText="Trip Id">
                    <ItemTemplate>
                        <asp:Label ID="lblBusinessTripId" runat="server" Text='<%# Bind("BusinessTripId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedDate" runat="server" Text='<%# Bind("RequestedDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- <asp:TemplateField HeaderText="Type">
                    <ItemTemplate>
                        <asp:Label ID="lblBusinessTripType" runat="server" Text='<%# Bind("BusinessTripType") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlBusinessTripType" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Departure Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartureDate" runat="server" Text='<%# Bind("DepartureDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDepartureDate" runat="server" Text='<%# Bind("DepartureDate") %>' masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Return Date">
                    <ItemTemplate>
                        <asp:Label ID="lblReturnDate" runat="server" Text='<%# Bind("ReturnDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtReturnDate" runat="server" Text='<%# Bind("ReturnDate") %>' masktype="date" />
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="DepartureCountry">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartureCountry" runat="server" Text='<%# Bind("DepartureCountry") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDepartureCountry" runat="server" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartureCountry_SelectedIndexChanged"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="DepartureCity">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartureCity" runat="server" Text='<%# Bind("DepartureCity") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDepartureCity" runat="server" Width="120px"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="DestinationCountry">
                    <ItemTemplate>
                        <asp:Label ID="lblDestinationCountry" runat="server" Text='<%# Bind("DestinationCountry") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDestinationCountry" runat="server" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlDestinationCountry_SelectedIndexChanged"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="DestinationCity">
                    <ItemTemplate>
                        <asp:Label ID="lblDestinationCity" runat="server" Text='<%# Bind("DestinationCity") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDestinationCity" runat="server" Width="120px"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <%--<asp:TemplateField HeaderText="Visa Required">
                    <ItemTemplate>
                        <asp:Label ID="lblRequiredVisa" runat="server" Text='<%# Bind("RequiredVisa") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlRequiredVisa" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                <%--<asp:TemplateField HeaderText="Exit Reentry Required">
                    <ItemTemplate>
                        <asp:Label ID="lblExitReentryRequired" runat="server" Text='<%# Bind("ExitReentryRequired") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlExitReentryRequired" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Flight Booking">
                    <ItemTemplate>
                        <asp:Label ID="lblFlightBookingRequired" runat="server" Text='<%# Bind("FlightBookingRequired") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlFlightBookingRequired" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hotel Booking">
                    <ItemTemplate>
                        <asp:Label ID="lblHotelBookingRequired" runat="server" Text='<%# Bind("HotelBookingRequired") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlHotelBookingRequired" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Car Rental">
                    <ItemTemplate>
                        <asp:Label ID="lblCarRentalBooking" runat="server" Text='<%# Bind("CarRentalBooking") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlCarRentalBooking" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Ticket Routing">
                    <ItemTemplate>
                        <asp:Label ID="lblTicketRouting" runat="server" Text='<%# Bind("TicketRouting") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlTicketRouting" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Meal">
                    <ItemTemplate>
                        <asp:Label ID="lblMeal" runat="server" Text='<%# Bind("Meal") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlMeal" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="NightStay">
                    <ItemTemplate>
                        <asp:Label ID="lblNightStay" runat="server" Text='<%# Bind("NightStay") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlNightStay" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="PersonalCar">
                    <ItemTemplate>
                        <asp:Label ID="lblPersonalCar" runat="server" Text='<%# Bind("PersonalCar") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlPersonalCar" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Fuel&Toll">
                    <ItemTemplate>
                        <asp:Label ID="lblFuelAndToll" runat="server" Text='<%# Bind("FuelAndToll") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlFuelAndToll" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="BusinesClass">
                    <ItemTemplate>
                        <asp:Label ID="lblBusinesClass" runat="server" Text='<%# Bind("BusinesClass") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlBusinesClass" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Reason">
                    <ItemTemplate>
                        <asp:Label ID="lblReason" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtReason" runat="server" Text='<%# Bind("Reason") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Workflow State">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkflowState" runat="server" Text='<%# Bind("WorkflowState") %>' masktype="enum"></asp:Label>
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
                    <%--<EditItemTemplate>
                            <asp:TextBox ID="RecId" runat="server"></asp:TextBox>
                        </EditItemTemplate>--%>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>
