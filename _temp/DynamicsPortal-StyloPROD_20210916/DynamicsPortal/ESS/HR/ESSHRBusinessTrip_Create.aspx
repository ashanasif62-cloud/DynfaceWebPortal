<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHRBusinessTrip_Create.aspx.cs" Inherits="DynamicsPortal.ESSHRBusinessTrip_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">

        <tr>
            <td>
                <span>Employee</span>
            </td>
            <td>
                <span>Request Date</span>
            </td>
<%--            <td>
                <span>Type</span>
            </td>--%>
        </tr>
        <tr>
            <td>
                <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails" />
            </td>
            <td>
                <asp:TextBox ID="txtRequestedDate" runat="server" masktype="date"></asp:TextBox>
            </td>
<%--            <td>
                <asp:DropDownList ID="ddlBusinessTripType" runat="server" masktype="enum"></asp:DropDownList>
            </td>--%>
        </tr>

        <tr>
            <td>
                <span>Departure Country</span>
            </td>
            <td>
                <span>Departure City</span>
            </td>
            <td>
                <span>Departure Date</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlDepartureCountry" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartureCountry_SelectedIndexChanged" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlDepartureCity" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtDepartureDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>


        <tr>
            <td>
                <span>Destination Country</span>
            </td>
            <td>
                <span>Destination City</span>
            </td>
            <td>
                <span>Return Date</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlDestinationCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDestinationCountry_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlDestinationCity" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtReturnDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td colspan="3">
                <span>Travel & Accommodation Information</span>
            </td>
        </tr>

        <tr>
            <td>
                <span>Hotel Booking</span>
            </td>
            <td>
                <span>Flight Booking</span>
            </td>
            <td>
                <span>Car Rental</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlHotelBookingRequired" runat="server" masktype="enum"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlFlightBookingRequired" runat="server" masktype="enum"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlCarRentalBooking" runat="server" masktype="enum"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td>
                <span>Personal Car</span>
            </td>
            <td>
                <span>Fuel And Toll</span>
            </td>
            <td>
                <span>Ticket Routing</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlPersonalCar" runat="server" masktype="enum"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlFuelAndToll" runat="server" masktype="enum"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlTicketRouting" runat="server" masktype="enum"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td>
                <span>Business Class</span>
            </td>
            <td>
                <span>Night Stay</span>
            </td>
            <td>
                <span>Meal</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlBusinesClass" runat="server" masktype="enum"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlNightStay" runat="server" masktype="enum"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlMeal" runat="server" masktype="enum"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td colspan="3">
                <span>Reason</span>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:TextBox ID="txtReasonCode" Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>




    </table>


    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
