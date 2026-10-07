<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="TASMarkAttendance.aspx.cs" Inherits="DynamicsPortal.ESS.TA.TASMarkAttendance" %>

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
                <!-- ✅ Date only will be shown from code-behind -->
                <asp:TextBox ID="txtAttendanceDate" runat="server" Enabled="false" ReadOnly="true" />
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
          <tr>
    <td>
        <span>Remarks<span style="color:red">*</span></span>
    </td>
    <td>
        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
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
       <%-- <asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click">Ok</asp:LinkButton>--%>
        <asp:LinkButton ID="btnOk_Submit" runat="server" OnClick="btnOk_SubmitClick">Updated & Submit</asp:LinkButton>
        <%--<asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>--%>
      <%--  <asp:LinkButton ID="btnUpdate&Submit" runat="server" OnClick="btnUpdate&Submit_Click">Update & Submit</asp:LinkButton>--%>
        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" OnClick="btnCancel_Click" />

    </div>
</asp:Content>
