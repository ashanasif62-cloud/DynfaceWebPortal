<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHRProfessionChange_Create.aspx.cs" Inherits="DynamicsPortal.ESSHRProfessionChange_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <table class="form-table">
        <%--        <tr>
            <td>
                <span>Certificate Id</span>
            </td>
            <td>
                <asp:TextBox ID="txtProfessionChangeId" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td>
                <span>Employee</span>
            </td>

            <td>
                <uc1:DropDownList_EmployeeDetails runat="server" id="cddlEmployeeDetails" />
                <%--<asp:DropDownList ID="ddlEmployee" runat="server"></asp:DropDownList>--%>
            </td>
        </tr>
        <tr>
            <td>
                <span>Request Date</span>
            </td>

            <td>
                <asp:TextBox ID="txtRequestDate" runat="server" masktype="date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Division Name</span>
            </td>

            <td>
                <asp:TextBox ID="txtDivisionName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Profession on Iqama Current</span>
            </td>

            <td>
                <asp:TextBox ID="txtCurrentProfession" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Job Title in Company/Division</span>
            </td>

            <td>
                <asp:TextBox ID="txtJobId" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Mobile Number</span>
            </td>

            <td>
                <asp:TextBox ID="txtMobileNumber" runat="server" masktype="number"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Remarks</span>
            </td>

            <td>
                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="action-footer">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Create</asp:LinkButton>
        <asp:LinkButton ID="btnCreate_Submit" runat="server" OnClick="btnCreate_Submit_Click">Create & Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
    </div>
</asp:Content>
