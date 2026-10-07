<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="PREmployeePFWithDrawl_ListPage.aspx.cs" Inherits="DynamicsPortal.PREmployeePFWithDrawl_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/PREmployeePFWithDrawl_Create.aspx');"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
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
                <asp:TemplateField HeaderText="Request Id">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Id">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("employeeId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("employeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label ID="lblPFDescription" runat="server" Text='<%# Bind("pFDescription") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPFDescription" runat="server" Text='<%# Bind("pFDescription") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Bind("requestDate")%>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestDate" runat="server" Text='<%# Bind("requestDate") %>' masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Advance Type Code">
                    <ItemTemplate>
                        <asp:Label ID="lblAdvanceTypeCode" runat="server" Text='<%# Bind("advanceTypeCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee PF/CF Balance">
                    <ItemTemplate>
                        <asp:Label ID="lblBalance" runat="server" Text='<%# Bind("balance") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employer PF/CF Balance">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployerPFBalance" runat="server" Text='<%# Bind("employerPFBalance") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Outstanding Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblOutStandingAmount" runat="server" Text='<%# Bind("outStandingAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOutStandingAmount" runat="server" Text='<%# Bind("outStandingAmount") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Request Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestAmount" runat="server" Text='<%# Bind("requestAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestAmount" runat="server" Text='<%# Bind("requestAmount") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="WorkFlow Status">
                    <ItemTemplate>
                        <asp:Label ID="lblWFStatus" runat="server" Text='<%# Bind("wFStatus") %>' masktype="enum"></asp:Label>
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
                    <%--<EditItemTemplate>
                            <asp:TextBox ID="txtRecId" runat="server"></asp:TextBox>
                        </EditItemTemplate>--%>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>
