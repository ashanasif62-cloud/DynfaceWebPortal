<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSPRProvidentFund_MyInquiry.aspx.cs" Inherits="DynamicsPortal.ESSPRProvidentFund_MyInquiry" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true"
            EmptyDataText="No Record Found." AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="PayPeriodCode">
                    <ItemTemplate>
                        <asp:Label ID="lblPayPeriodCode" runat="server" Text='<%# Bind("PayPeriodCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Id">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("EmployeeId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ben Ded Code">
                    <ItemTemplate>
                        <asp:Label ID="lblBenDedCode" runat="server" Text='<%# Bind("BenDedCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Amount">
                    <ItemTemplate>
                        <asp:Label ID="lbEmployeeAmount" runat="server" Text='<%# Bind("EmployeeAmount") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employer Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployerAmount" runat="server" Text='<%# Bind("EmployerAmount") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
