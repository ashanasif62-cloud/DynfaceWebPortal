<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" 
    CodeBehind="ESSOvertimePlanner_Edit.aspx.cs" 
    Inherits="DynamicsPortal.ESS.TA.ESSOvertimePlanner_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">

    <asp:UpdatePanel ID="updOvertimeForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>

            <table class="form-table">

                <!-- Employee Id -->
                <tr>
                    <td>
                        <span>Employee Id</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmployeeId" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>

                <!-- Employee Name -->
                <tr>
                    <td>
                        <span>Employee Name</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmployeeName" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>

                <!-- Request Date -->
                <tr>
                    <td>
                        <span>Request Date</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRequestDate" runat="server" Enabled="false" 
                                     autocomplete="off" masktype="date"></asp:TextBox>
                    </td>
                </tr>

                <!-- WF Status -->
                <tr>
                    <td>
                        <span>WF Status</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtWFStatus" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>

                <!-- Plan Date -->
                <tr>
                    <td>
                        <span>Plan Date</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlanDate" runat="server" 
                                     TextMode="Date" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>

                <!-- Start Time -->
               <%-- <tr>
                    <td>
                        <span>Start Time</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStartTime" runat="server" 
                                     TextMode="Time" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>--%>

                 <tr>
                    <td>
                        <span>Start Time</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStartTime" runat="server" 
                                     CssClass="time-input"
                                     autocomplete="off"
                                     placeholder="HH:mm:ss AM/PM"></asp:TextBox>
                      
                    </td>
                </tr>

                <!-- End Time -->
              <%--  <tr>
                    <td>
                        <span>End Time</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEndTime" runat="server" 
                                     TextMode="Time" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>--%>

                 <tr>
                    <td>
                        <span>End Time</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEndTime" runat="server" 
                                     CssClass="time-input"
                                     autocomplete="off"
                                     placeholder="HH:mm:ss AM/PM"></asp:TextBox>
                       
                    </td>
                </tr>

            </table>

            <!-- Action Buttons -->
            <div class="action-footer">
                <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Save</asp:LinkButton>
                
                <asp:LinkButton ID="btnCancel" runat="server" 
                    OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>