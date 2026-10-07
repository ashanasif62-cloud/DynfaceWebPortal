<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHcmPersonIdentificationNumber.aspx.cs" Inherits="DynamicsPortal.ESSHcmPersonIdentificationNumber" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div class="action-panel-grid">
        <div class="action-items-grid">
            <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
        </div>
        <div class="action-items-grid">
            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
        </div>
    <%--<div class="action-items-grid">
            <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Remove</asp:LinkButton>
        </div>--%>
    </div>

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
                <asp:TemplateField HeaderText="Identification Type">
                    <ItemTemplate>
                        <asp:Label ID="lblIdentificationType" runat="server" Text='<%# Bind("IdentificationTypeId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIdentificationType" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Number">
                    <ItemTemplate>
                        <asp:Label ID="lblIdentificationNumber" runat="server" Text='<%# Bind("IdentificationNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtIdentificationNumber" runat="server" Text='<%# Bind("IdentificationNumber") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Entry Type">
                    <ItemTemplate>
                        <asp:Label ID="lblClassification" runat="server" Text='<%# Bind("Classification") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtClassification" runat="server" Text='<%# Bind("Classification") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Is Primary">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrimary" runat="server" Text='<%# Bind("IsPrimary") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIsPrimary" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Issuing Agency">
                    <ItemTemplate>
                        <asp:Label ID="lblIssuingAgency" runat="server" Text='<%# Bind("IssuingAgencyId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIssuingAgency" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Issued Date">
                    <ItemTemplate>
                        <asp:Label ID="lblIssuedDate" runat="server" Text='<%# Bind("IssuedDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtIssuedDate" runat="server" Text='<%# Bind("IssuedDate") %>' masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Expiration Date">
                    <ItemTemplate>
                        <asp:Label ID="lblExpirationDate" runat="server" Text='<%# Bind("ExpirationDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtExpirationDate" runat="server" Text='<%# Bind("ExpirationDate") %>' masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Workflow Operation">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkflowOperation" runat="server" Text='<%# Bind("WorkflowOperation") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Approval Status">
                    <ItemTemplate>
                        <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
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

                <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" Visible="false" />
                <asp:BoundField DataField="Person" HeaderText="Person" Visible="false" />
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
                            <asp:TextBox ID="RecId" runat="server"></asp:TextBox>
                        </EditItemTemplate>--%>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>

