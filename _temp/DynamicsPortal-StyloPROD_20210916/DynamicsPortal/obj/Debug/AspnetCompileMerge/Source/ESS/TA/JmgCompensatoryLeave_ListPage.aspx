<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="JmgCompensatoryLeave_ListPage.aspx.cs" Inherits="DynamicsPortal.JmgCompensatoryLeave_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/TA/JmgCompensatoryLeave_Create.aspx');"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
    <%--    <div class="action-items">
        <asp:LinkButton ID="btnEdit" runat="server"><i class="mdi mdi-border-color"></i>Edit</asp:LinkButton>
    </div>--%>
    <div class="action-items">
        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
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
                 
                <asp:TemplateField HeaderText="Worker" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblWorker" runat="server" Text='<%# Bind("worker") %>'></asp:Label>
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

                <asp:TemplateField HeaderText="Profile Date">
                    <ItemTemplate>
                        <asp:Label ID="lblProfileDate" runat="server" Text='<%# Bind("profileDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtProfileDate" runat="server" Text='<%# Bind("profileDate") %>' masktype="date" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Profile">
                    <ItemTemplate>
                        <asp:Label ID="lblProfile" runat="server" Text='<%# Bind("profileId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtProfile" runat="server" Text='<%# Bind("profileId") %>'/>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Bind("requestDate") %>' masktype="date" ></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestDate" runat="server" Text='<%# Bind("requestDate") %>' masktype="date" Enabled="false" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("description") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("description") %>'/>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="WFStatus">
                    <ItemTemplate>
                        <asp:Label ID="lblWFStatus" runat="server" Text='<%# Bind("wFStatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                        <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Save" Text="Save" runat="server" OnClick="Update_Click" />
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
