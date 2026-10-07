<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHRSubDepartment.aspx.cs" Inherits="DynamicsPortal.ESSHRSubDepartment" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Sub Department">
                    <ItemTemplate>
                        <asp:Label ID="lblLaborUnionName" runat="server" Text='<%# Bind("LaborUnionName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Start Date">
                    <ItemTemplate>
                        <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("StartDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End Date">
                    <ItemTemplate>
                        <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("EndDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" Visible="false" />
                <asp:BoundField DataField="EmployeeName" HeaderText="EmployeeName" Visible="false" />
                <asp:BoundField DataField="Person" HeaderText="Person" Visible="false" />
                <asp:BoundField DataField="LaborUnion" HeaderText="LaborUnion" Visible="false" />

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

