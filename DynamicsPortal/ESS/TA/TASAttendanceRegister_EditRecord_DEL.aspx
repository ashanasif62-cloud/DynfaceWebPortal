<%@ Page Title="Edit Attendance Record" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TASAttendanceRegister_EditRecord_DEL.aspx.cs" Inherits="DynamicsPortal.ESS.TA.TASAttendanceRegister_EditRecord_Del" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
     <tr>
         <td><span>Employee Id</span></td>
         <td>
             <asp:TextBox ID="txtEmployeeId" runat="server" ReadOnly="true" />
         </td>
     </tr>

     <tr>
         <td><span>Employee Name</span></td>
         <td>
             <asp:TextBox ID="txtEmployeeName" runat="server" ReadOnly="true" />
         </td>
     </tr>
      <tr>
         <td><span>Attendance Date</span></td>
         <td>
             <asp:TextBox ID="txtAttendanceDate" runat="server" Enabled ="false"  ReadOnly="true" />
         </td>
     </tr>

     <tr>
         <td><span>Clock In</span></td>
         <td>
             <asp:TextBox ID="txtClockIn" runat="server" TextMode="Time" />
         </td>
     </tr>

     <tr>
         <td><span>Clock Out</span></td>
         <td>
             <asp:TextBox ID="txtClockOut" runat="server" TextMode="Time" />
         </td>
     </tr>
 </table>

    <div class="action-footer">
        <asp:LinkButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click">Update</asp:LinkButton>
        <%--<asp:LinkButton ID="btnCancel" runat="server" OnClientClick="btnCancel_Click">Cancel</asp:LinkButton>--%>
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" OnClick="btnCancel_Click" />

    </div>
</asp:Content>
      