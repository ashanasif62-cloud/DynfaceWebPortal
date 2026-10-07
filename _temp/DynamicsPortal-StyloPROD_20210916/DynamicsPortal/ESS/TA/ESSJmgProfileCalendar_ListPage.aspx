<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSJmgProfileCalendar_ListPage.aspx.cs"
    Inherits="DynamicsPortal.ESSJmgProfileCalendar_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" lang="javascript">
        function showBrowseDialog() {
            var userImgUpload = document.getElementById("<%=fileUpload.ClientID %>");
            userImgUpload.click();
            return false;
        }
        function upload() {
            var btnUpload = document.getElementById("<%=btnUpload.ClientID %>");
            btnUpload.click();
        }
    </script>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_Click"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>

    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
    </div>

    <div class="action-items" ><%-- style="display:none;"--%>
            <asp:Button ID="btnUpload" runat="server" style="display:none;" OnClientClick="return confirm('Are you sure to import this file?')" OnClick="btnImport_Click" />
        <asp:LinkButton ID="btnImport" runat="server" OnClientClick="return showBrowseDialog();"><i class="mdi mdi-shape-plus"></i>Import</asp:LinkButton>
    </div>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:FileUpload ID="fileUpload" runat="server" Style="display: none;"  onchange="upload();" accept="text/csv" /><%--accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"--%>
<%--    <asp:RegularExpressionValidator  ID="RegularExpressionValidator1" runat="server" 
    ErrorMessage="CSV files only"  ControlToValidate="fileUpload" 
    ValidationExpression="^(([a-zA-]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.csv)$"> </asp:RegularExpressionValidator>--%>

    <div style="display: inline-block; font-size: 12px;">
        <span style="margin-right: 5px;">From Date</span>
        <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" OnTextChanged="Date_TextChanged" masktype="date"></asp:TextBox>

        <span style="margin-right: 5px;">To Date</span>
        <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="true" OnTextChanged="Date_TextChanged" masktype="date"></asp:TextBox>
    </div>
    <div style="padding-top:4px;">
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
            AutoGenerateColumns="false" OnRowDataBound="gridView_RowDataBound" OnRowEditing="gridView_RowEditing">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Employee Id">
                    <ItemTemplate>
                        <asp:Label ID="lblRelationNumber" runat="server" Text='<%# Bind("RelationNumber") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Employee Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Profile Date">
                    <ItemTemplate>
                        <asp:Label ID="lblJmgDate" runat="server" Text='<%# Bind("JmgDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtJmgDate" Width="300px" runat="server" Text='<%# Bind("JmgDate") %>' masktype="date" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Profile Id">
                    <ItemTemplate>
                        <asp:Label ID="lblProfileId" runat="server" Text='<%# Bind("ProfileId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlProfileId" Width="300px" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Special Day">
                    <ItemTemplate>
                        <asp:Label ID="lblSpecialDayId" runat="server" Text='<%# Bind("SpecialDayId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlSpecialDayId" Width="300px" runat="server" />
                    </EditItemTemplate>
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
