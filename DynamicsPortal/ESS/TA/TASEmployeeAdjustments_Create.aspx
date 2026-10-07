<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="TASEmployeeAdjustments_Create.aspx.cs" Inherits="DynamicsPortal.ESS.TA.TASEmployeeAdjustments_Create" %>
<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
     <script type="text/javascript">
         function fillClockTimes() {
             var attendanceDate = document.getElementById('<%= txtAttendanceDate.ClientID %>').value;
            if (attendanceDate) {
                // Build datetime-local values
                var clockIn = attendanceDate + "T09:00";
                var clockOut = attendanceDate + "T17:00";

                document.getElementById('<%= txtClockIn.ClientID %>').value = clockIn;
                document.getElementById('<%= TxtClockOut.ClientID %>').value = clockOut;
             }
         }
     </script>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
     <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
 <ContentTemplate>
    <table class="form-table">
        <tr>
            <td><span>Employee Id</span></td>
            <td>   <uc1:DropDownList_EmployeeDetails ID="DropDownList_EmployeeDetails1" runat="server" OnEmployeeSelected="cddlEmployeeDetails_OnEmployeeSelected" />

            </td>
        </tr>
        <tr>
            <td><span>Attendance Date</span></td>
            <td>
               <%-- <asp:TextBox ID="txtAttendanceDate" runat="server" TextMode="Date" />--%>
                <asp:TextBox ID="txtAttendanceDate" runat="server" TextMode="Date" onchange="fillClockTimes()" />

            </td>
        </tr>
      
           <tr>
        <td><span>Shift Id</span></td>
            <td>
                <asp:DropDownList ID="ddlShiftId" runat="server" AutoPostBack="false" />
            </td>

   </tr>
      <%--  <tr>
            <td><span>Clock In Date</span></td>
            <td>
                <asp:TextBox ID="txtClockInDate" runat="server" TextMode="Date" />
            </td>
        </tr>
         <tr>
     <td><span>Clock Out Date</span></td>
     <td>
         <asp:TextBox ID="TxtClockOutDate" runat="server" TextMode="Date" />
     </td>
 </tr>--%>
        <tr>
    <td><span>Clock In Date</span></td>
    <td>
        <asp:TextBox ID="txtClockIn" runat="server" />
    </td>
</tr>
<tr>
    <td><span>Clock Out Date</span></td>
    <td>
        <asp:TextBox ID="TxtClockOut" runat="server" />
    </td>
</tr>

        <tr>
                    <td><span>Reason Code</span></td>
                    <td>
                        <asp:DropDownList ID="ddlReasonCode" runat="server" AutoPostBack="false">
                            <asp:ListItem Text="-- Select Reason Code --" Value="" />
                        </asp:DropDownList>
                    </td>
                </tr>
        
                    <tr>
    <td style="vertical-align: top; padding-top: 5px;">
        <label for="txtRemarks">
            Remarks<span style="color:red">*</span>
        </label>
    </td>
    <td>
        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3" Columns="40"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator 
            ID="rfvRemarks" 
            runat="server" 
            ControlToValidate="txtRemarks" 
            ErrorMessage="Remarks are required." 
            ForeColor="Red" 
            Display="Dynamic">
        </asp:RequiredFieldValidator>
    </td>
</tr>


    </table>

    <div class="action-footer">
       
        <asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
         </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
