<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master"  CodeBehind="ESSEmployeeAppraisalPlan_Create.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSEmployeeAppraisalPlan_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <tr>
            <td>
                <span>Employee Id</span>
            </td>

            <td>
                <%--<select :DropDownList_EmployeeDetails name="EmployeeId" ID="cddlEmployeeDetails" disabled>--%>
                <uc1:DropDownList_EmployeeDetails ID="cddlEmployeeDetails" runat="server"  />
                <%--<uc1:DropDownList_EmployeeDetails ID="cddlEmployeeDetails" runat="server" Enabled="false" />--%>
   <%--<asp:DropDownList_EmployeeDetails ID="cddlEmployeeDetails" runat="server" Enabled="false" ></asp:DropDownList_EmployeeDetails>--%>

               
<%--                    </select>--%>
                <%--<asp:DropDownList ID="cddlEmployeeDetails" EnableViewState="true" runat="server" Enabled="false"></asp:DropDownList>--%>
           </td>
        </tr>
      <%--  <tr>
            <td>
                <span>Appraisal Code</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlLeaveCode" runat="server" OnSelectedIndexChanged="ddlLeaveCode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </td>
        </tr>
        <%-- <tr>
            <td>
                <span>Plan Weightage</span>
            </td>

            <td>
                <asp:DropDownList ID="txtBalance" runat="server" OnSelectedIndexChanged="ddlLeaveCode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>KPI's</span>
            </td>

            <td>
                <asp:TextBox ID="KPICode" runat="server" autocomplete="off" Enabled="true" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Description</span>
            </td>

            <td>
               <asp:TextBox ID="Description" runat="server" Enabled="true" autocomplete="off"  TextMode="MultiLine" Rows="5" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>KPI Weightage</span>
            </td>

            <td>
                <asp:TextBox ID="KPIWeightage" runat="server" autocomplete="off" Enabled="true" ></asp:TextBox>
            </td>
        </tr>
        <tr>
           </tr>
        <%-- <tr>
            <td>
                <span>Review Type</span>
            </td>

            <td>
                <asp:DropDownList ID="ddlReviewType" runat="server" OnSelectedIndexChanged="ddlLeaveCode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </td>
        </tr>--%>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
