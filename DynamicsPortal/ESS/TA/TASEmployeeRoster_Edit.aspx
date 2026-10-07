<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="TASEmployeeRoster_Edit.aspx.cs" Inherits="DynamicsPortal.ESS.TA.TASEmployeeRoster_Edit" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <tr>
            <td><span>Employee ID</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblEmployeeID" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Employee Name</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblEmployeeName" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Shift ID</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblShiftID" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Shift Code</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblShiftCode" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Date</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblDate" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Type</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblType" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
          <%--  <td><span>Flex Clock In Start Time</span></td>
            <td>
                <asp:TextBox ID="txtFlexClockInStartTime" runat="server" ReadOnly="true" />
            </td>
        </tr>
        --%>
            
        <tr>
            <td><span>Flex Clock In Start Time</span></td>
    <td>
        <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
            <asp:Label ID="lblFlexClockInStartTime" runat="server" />
        </div>
    </td>
</tr>

      

        <tr>
            <td><span>Time In</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblTimeIn" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Flex Clock In End Time</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblFlexClockInEndTime" runat="server" />
                </div>
            </td>
        </tr>

    <%-- <tr>
    <td><span>Flex Clock Out Start Time</span></td>
    <td>
        <asp:TextBox ID="txtFlexClockOutStartTime" runat="server" ReadOnly="true" />
    </td>
</tr>--%>

        <tr>
            <td><span>Flex Clock Out Start Time</span></td>
    <td>
        <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
            <asp:Label ID="lblFlexClockOutStartTime" runat="server" />
        </div>
    </td>
</tr>


 
     


        <tr>
            <td><span>Time Out</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblTimeOut" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Flex Clock Out End Time</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblFlexClockOutEndTime" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Shift Hours</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblShiftHours" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Required Working Hours</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblRequiredWorkingHours" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Min. Working Hour</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblMinWorkingHour" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Off Day</span></td>
            <td>
                <asp:DropDownList ID="ddlOffDay" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Yes" Value="Yes" />
                    <asp:ListItem Text="No" Value="No" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><span>Generation Type</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblGenerationType" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td><span>Gazetted Day</span></td>
            <td>
                <div style="border: 1px solid #ccc; padding: 5px; background-color: #f9f9f9; border-radius: 4px; pointer-events: none; cursor: not-allowed;">
                    <asp:Label ID="lblGazettedDay" runat="server" />
                </div>
            </td>
        </tr>
    </table>

    <%--<div style="margin-top: 20px;" class="action-footer">--%>
    <div style="margin-top: 20px; display: flex; justify-content: flex-end;">
        <asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click" 
            style="background-color: #4CAF50; color: white; padding: 5px 15px; border: none; border-radius: 5px; cursor: pointer; font-size: 14px; font-weight: 700; margin-right: 10px">
            Update
        </asp:LinkButton>

        <button type="button" onclick="closeDialog()" 
            style="background-color: #f44336; color: white; padding: 5px 15px; border: none; border-radius: 5px; cursor: pointer; font-size: 14px; font-weight: 700">
            Cancel
        </button>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css" />

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            flatpickr(".time-picker", {
                enableTime: true,
                noCalendar: true,
                enableSeconds: true,
                dateFormat: "h:i:S K",
                time_24hr: false,
                minuteIncrement: 1,
                secondIncrement: 1
            });
        });
    </script>
</asp:Content>
