<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSPRProvidentFund_MyInquiry.aspx.cs" Inherits="DynamicsPortal.ESSPRProvidentFund_MyInquiry" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 10px;">
        <%--<div>
            <!-- Add any other content here if needed -->
        </div>--%>

        <div>
            <asp:Label ID="lblEmployeeAmount" runat="server" Text="Employee Total Amount" AssociatedControlID="txtEmployeeAmount"
                Style="font-size: 10px;"></asp:Label>
            <asp:TextBox ID="txtEmployeeAmount" runat="server" Enabled="false"
                Style="font-size: 10px;"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblEmployerAmount" runat="server" Text="Employer Total Amount" AssociatedControlID="txtEmployerAmount"
                Style="font-size: 10px;"></asp:Label>
            <asp:TextBox ID="txtEmployerAmount" runat="server" Enabled="false"
                Style="font-size: 10px;"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblEmployeeItemAmount" runat="server" Text="PF Profit Amount" AssociatedControlID="txtEmployeeItemAmount"
                Style="font-size: 10px;" Visible="false"></asp:Label>
            <asp:TextBox ID="txtEmployeeItemAmount" runat="server" Enabled="false" Visible="false"
                Style="font-size: 10px;"></asp:TextBox>
        </div>
                 <div>
    <asp:Label ID="lblRecoveryAmount" runat="server" Text="PF Outstanding Amount" AssociatedControlID="txtRecoveryAmount"
        Style="font-size: 10px;" Visible="false"></asp:Label>
    <asp:TextBox ID="txtRecoveryAmount" runat="server" Enabled="false" Visible="false"
        Style="font-size: 10px;"></asp:TextBox>
</div>
        <div>
            <asp:Label ID="lblTotalAmount" runat="server" Text="Total Amount" AssociatedControlID="txtTotalAmount"
                Style="font-size: 10px;"></asp:Label>
            <asp:TextBox ID="txtTotalAmount" runat="server" Enabled="false"
                Style="font-size: 10px;"></asp:TextBox>
        </div>
       

    </div>

    <div>
        <asp:GridView ID="gridView1" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true"
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
<%--                        <asp:Label ID="lbEmployeeAmount" runat="server" Text='<%# Bind("EmployeeAmount") %>' masktype="number"></asp:Label>--%>
                        <asp:Label ID="lbEmployeeAmount" runat="server"
    Text='<%# Eval("EmployeeAmount") != null && Eval("EmployeeAmount").ToString() != "" 
           ? string.Format("{0:N2}", Convert.ToDecimal(Eval("EmployeeAmount"))) 
           : "0.00" %>'></asp:Label>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employer Amount">
                    <ItemTemplate>
<%--                        <asp:Label ID="lblEmployerAmount" runat="server" Text='<%# Bind("EmployerAmount") %>' masktype="number"></asp:Label>--%>
                        <asp:Label ID="lblEmployerAmount" runat="server"
    Text='<%# Eval("EmployerAmount") != null && Eval("EmployerAmount").ToString() != "" 
           ? string.Format("{0:N2}", Convert.ToDecimal(Eval("EmployerAmount"))) 
           : "0.00" %>'></asp:Label>

                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
