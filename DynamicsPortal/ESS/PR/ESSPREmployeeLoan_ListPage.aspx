<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSPREmployeeLoan_ListPage.aspx.cs" Inherits="DynamicsPortal.ESSPREmployeeLoan_ListPage" %>

<%@ Register Src="~/DropDownList_AllEmployeeDetails.ascx" TagPrefix="uc1" TagName="DropDownList_AllEmployeeDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/ESSPREmployeeLoan_Create.aspx');"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
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
    <%-- <div class="action-items"><a onclick=""><i class="mdi mdi-delete"></i></a>Delete</div> --%>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div style="overflow: auto;">
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loan Request Id">
                    <ItemTemplate>
                        <asp:Label ID="lblLoanRequestId" runat="server" Text='<%# Bind("LoanRequestId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployee" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loan Type Code">
                    <ItemTemplate>
                        <asp:Label ID="lblLoanTypeCode" runat="server" Text='<%# Bind("LoanTypeCode") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlLoanTypeCode" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loan Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblLoanAmount" runat="server" Text='<%# Bind("LoanAmount") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLoanAmount" runat="server" Text='<%# Bind("LoanAmount") %>' masktype="number"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Bind("RequestDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestDate" runat="server" Text='<%# Bind("RequestDate") %>' masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Payment Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedPaymentDate" runat="server" Text='<%# Bind("RequestedPaymentDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestedPaymentDate" runat="server" Text='<%# Bind("RequestedPaymentDate") %>' masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
<%--                <asp:TemplateField HeaderText="Guarantor 1">
                    <ItemTemplate>
                        <asp:Label ID="lblGuarantor1" runat="server" Text='<%# Bind("Guarantor1") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlGuarantor1" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Guarantor 2">
                    <ItemTemplate>
                        <asp:Label ID="lblGuarantor2" runat="server" Text='<%# Bind("Guarantor2") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlGuarantor2" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Recovery Start Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRecoveryStartDate" runat="server" Text='<%# Bind("RecoveryStartDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRecoveryStartDate" runat="server" Text='<%# Bind("RecoveryStartDate") %>' masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Monthy Installments">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedInstallmentAmount" runat="server" Text='<%# Bind("RequestedInstallmentAmount") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestedInstallmentAmount" runat="server" Text='<%# Bind("RequestedInstallmentAmount") %>' masktype="number"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Recoveries">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedInstallments" runat="server" Text='<%# Bind("RequestedInstallments") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestedInstallments" runat="server" Text='<%# Bind("RequestedInstallments") %>' masktype="number"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loan Description">
                    <ItemTemplate>
                        <asp:Label ID="lblLoanDescription" runat="server" Text='<%# Bind("LoanDescription") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLoanDescription" runat="server" Text='<%# Bind("LoanDescription") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblWFStatus" runat="server" Text='<%# Bind("WFStatus") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                        <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                    </EditItemTemplate>
                </asp:TemplateField>
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
