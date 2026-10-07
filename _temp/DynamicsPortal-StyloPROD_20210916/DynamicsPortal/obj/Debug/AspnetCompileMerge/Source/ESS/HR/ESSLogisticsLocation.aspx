<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSLogisticsLocation.aspx.cs" Inherits="DynamicsPortal.ESSLogisticsLocation" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">

    <div class="action-panel-grid">
        <div class="action-items-grid">
            <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
        </div>
        <div class="action-items-grid">
            <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Remove</asp:LinkButton>
        </div>
        <%--    <div class="action-items">
        <asp:LinkButton ID="btnEdit" runat="server"><i class="mdi mdi-border-color"></i>Edit</asp:LinkButton>
    </div>--%>
        <%--    <div class="action-items">
        <asp:LinkButton ID="btnView" runat="server"><i class="mdi mdi-eye"></i>View</asp:LinkButton>
    </div>
        <div class="action-items">
            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
        </div>--%>
    </div>

    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId" AutoGenerateColumns="false"
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Label ID="lbStreet" runat="server" Text='<%# Bind("Street") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtStreet" runat="server" Text='<%# Bind("Street") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="City">
                    <ItemTemplate>
                        <asp:Label ID="lbCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCity" runat="server" Text='<%# Bind("City") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Country">
                    <ItemTemplate>
                        <asp:Label ID="lbCountryRegionId" runat="server" Text='<%# Bind("CountryRegionId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCountryRegionId" runat="server" Text='<%# Bind("CountryRegionId") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label ID="lbDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Purpose">
                    <ItemTemplate>
                        <asp:Label ID="lbPurpose" runat="server" Text='<%# Bind("Purpose") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPurpose" runat="server" Text='<%# Bind("Purpose") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="State">
                    <ItemTemplate>
                        <asp:Label ID="lbState" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtState" runat="server" Text='<%# Bind("State") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Zip Code">
                    <ItemTemplate>
                        <asp:Label ID="lbZipCode" runat="server" Text='<%# Bind("ZipCode") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtZipCode" runat="server" Text='<%# Bind("ZipCode") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Is Primary">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrimary" runat="server" Text='<%# Bind("IsPrimary") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIsPrimary" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Primary for country">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrimaryTaxRegistration" runat="server" Text='<%# Bind("IsPrimaryTaxRegistration") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIsPrimaryTaxRegistration" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Is Private">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrivate" runat="server" Text='<%# Bind("IsPrivate") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIsPrivate" runat="server" />
                    </EditItemTemplate>
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

                <asp:BoundField DataField="Party" HeaderText="Party" Visible="false" />
                <asp:BoundField DataField="Person" HeaderText="Person" Visible="false" />
                <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" Visible="false" />
                <asp:BoundField DataField="LocationId" HeaderText="LocationId" Visible="false" />

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
