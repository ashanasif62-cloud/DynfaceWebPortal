<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHRHiringRequisition_Create.aspx.cs" Inherits="DynamicsPortal.ESSHRHiringRequisition_Create" %>

<%@ Register Src="~/DropDownList_EmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_EmployeeDetails" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
      <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
  <ContentTemplate>
    <table class="form-table">
        <tr>
            <td>
                <span id="lblHRSerialNumber" runat="server">Case Id</span>
            </td>
            <td>
                <span>Requested By</span>
            </td>
            <td>
                <span>Date</span>
            </td>
            <td>
                <%--                <span id="lblDepartmentName" runat="server">Department</span>--%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtHRSerialNumber" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td>
                <uc1:DropDownList_EmployeeDetails runat="server" ID="cddlEmployeeDetails" />
            </td>
            <td>
                <asp:TextBox ID="txtRequestDate" runat="server" masktype="date"></asp:TextBox>
            </td>
            <td>
                <%--                <asp:TextBox ID="txtDepartmentName" runat="server" Enabled="false"></asp:TextBox>--%>
            </td>
        </tr>
    </table>

    <div style="margin-top: 20px;">
        <div class="action-panel-grid">
            <div class="action-items-grid">
                <asp:LinkButton ID="btnNewDetails" runat="server" OnClick="btnNewDetails_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
            </div>
            <div class="action-items-grid">
                <asp:LinkButton ID="btnDeleteDetails" runat="server" OnClick="btnDeleteDetails_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
            </div>
        </div>
        <div style="padding-bottom: 10px; overflow: auto;">
            <asp:GridView ID="gridView_HiringRequisitionDetails" runat="server" CssClass="table table-condensed no-border table-hover sortable"
                ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId" AutoGenerateColumns="false"
                OnRowDataBound="gridView_HiringRequisitionDetails_RowDataBound" OnRowEditing="gridView_HiringRequisitionDetails_RowEditing">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <HeaderTemplate>
                            <input type="checkbox" id="chk_SelectAll" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Designation">
                        <ItemTemplate>
                            <asp:Label ID="lblDesignation" runat="server" Text='<%# Bind("Designation") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlDesignation" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Education">
                        <ItemTemplate>
                            <asp:Label ID="lblEducation" runat="server" Text='<%# Bind("Education") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEducation" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Experience">
                        <ItemTemplate>
                            <asp:Label ID="lblExperience" runat="server" Text='<%# Bind("Experience") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtExperience" runat="server" Text='<%# Bind("Experience") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlLocation" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vacancies">
                        <ItemTemplate>
                            <asp:Label ID="lblVacancies" runat="server" Text='<%# Bind("Vacancies") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtVacancies" runat="server" Text='<%# Bind("Vacancies") %>' masktype="number"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
<%--                    <asp:TemplateField HeaderText="Expected Hiring Date">
                        <ItemTemplate>
                            <asp:Label ID="lblExpectedHiringDate" runat="server" Text='<%# Bind("ExpectedHiringDate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtExpectedHiringDate" runat="server" Text='<%# Bind("ExpectedHiringDate") %>' masktype="date"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Remarks">
                        <ItemTemplate>
                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemarks" Width="200px" runat="server" Text='<%# Bind("Remarks") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Note">
                        <ItemTemplate>
                            <asp:Label ID="lblJobDescription" runat="server" Text='<%# Bind("JobDescription") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtJobDescription" Width="200px" runat="server" Text='<%# Bind("JobDescription") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                        <ItemTemplate>
                            <asp:LinkButton Text="Edit" runat="server" CommandName="Edit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Save" Text="Save" runat="server" OnClick="Update_Click" />
                            <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="HRSerialNumber" Visible="false" />
                    <asp:BoundField DataField="MonthsOfYear" Visible="false" />
                    <asp:BoundField DataField="DepartmentID" Visible="false" />
                    <asp:BoundField DataField="BudgetYear" Visible="false" />
                    <asp:BoundField DataField="EstimatedMonthlySalary" Visible="false" />
                    <asp:BoundField DataField="AdditionalBenifts" Visible="false" />

                    <asp:TemplateField HeaderText="RecId" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                        </ItemTemplate>
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


    <div class="action-footer">
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Close</asp:LinkButton>
    </div>
          </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
