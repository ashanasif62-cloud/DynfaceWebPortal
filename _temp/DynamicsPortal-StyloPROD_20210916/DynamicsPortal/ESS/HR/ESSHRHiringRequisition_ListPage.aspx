<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSHRHiringRequisition_ListPage.aspx.cs" Inherits="DynamicsPortal.ESSHRHiringRequisition_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/HR/ESSHRHiringRequisition_Create.aspx','980');"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
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
    <%--<div class="action-items"><a onclick=""><i class="mdi mdi-delete"></i></a>Delete</div>--%>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gridView_HiringRequisition" runat="server"  data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found."
            DataKeyNames="RecId"
            OnRowDataBound="gridView_HiringRequisition_RowDataBound" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Case Id">
                    <ItemTemplate>
                        <asp:Label ID="lblHRSerialNumber" runat="server" Text='<%# Bind("HRSerialNumber") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requested By">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedByName" runat="server" Text='<%# Bind("RequestedByName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Bind("RequestDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartmentName" runat="server" Text='<%# Bind("DepartmentName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblHRWFStatus" runat="server" Text='<%# Bind("HRWFStatus") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-details" ToolTip="Details" Text="Details" runat="server" OnClick="Details_Click" />
                        <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment"  Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="RequestedBy" Visible="false" />
                <asp:BoundField DataField="DepartmentID" Visible="false" />
                <asp:BoundField DataField="Remarks" Visible="false" />
                <asp:BoundField DataField="Experience" Visible="false" />
                <asp:BoundField DataField="Education" Visible="false" />
                <asp:BoundField DataField="EstimatedMonthlySalary" Visible="false" />
                <asp:BoundField DataField="Designation" Visible="false" />
                <asp:BoundField DataField="Vacancies" Visible="false" />
                <asp:BoundField DataField="Year" Visible="false" />
                <asp:BoundField DataField="MonthsOfYear" Visible="false" />
                <asp:BoundField DataField="BudgetYear" Visible="false" />
                <asp:BoundField DataField="Location" Visible="false" />

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
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
