<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SysUserInfo_ListPage.aspx.cs" Inherits="DynamicsPortal.SysUserInfo_ListPage" %>

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
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
<asp:UpdatePanel ID="updButtons" runat="server">
    <ContentTemplate>
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server"
            OnClientClick="javascript: return openPopupPanel('/Administration/User Management/SysUserInfo_Create.aspx');"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:Button ID="btnUpload" runat="server" Style="display: none" OnClientClick="return confirm('Are you sure to import this file?')" OnClick="btnImport_Click" />
        <asp:LinkButton ID="btnImport" runat="server" OnClientClick="return showBrowseDialog();"><i class="mdi mdi-shape-plus"></i>Import</asp:LinkButton>
    </div>
                      </ContentTemplate>
</asp:UpdatePanel>  
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
<ContentTemplate>
    <asp:FileUpload ID="fileUpload" runat="server" Style="display: none;" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" onchange="upload();" />
    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found."
            DataKeyNames="RecId" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="User Id">
                    <ItemTemplate>
                        <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Id">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("EmployeeId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="User Name">
                    <ItemTemplate>
                        <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Default Company">
                    <ItemTemplate>
                        <asp:Label ID="lblDefaultCompany" runat="server" Text='<%# Bind("DefaultCompany") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#Convert.ToBoolean(Eval("Status")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" OnClick="Update_Click" />
                        <asp:LinkButton runat="server" CssClass="grid-img-btn btn-changepassword" ToolTip="Reset Password"
                            Text="Reset Password" OnClick="ResetPassword_Click" />
                    </ItemTemplate>
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
          </ContentTemplate>
     </asp:UpdatePanel>
            <script type="text/javascript">
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                prm.add_beginRequest(function () {
                    showAJAXOverlay();  // Should now fire
                });

                prm.add_endRequest(function () {
                    hideAJAXOverlay();
                });
            </script>
<script type="text/javascript">
    window.refreshParentGrid = function () {
        __doPostBack('RefreshGrid', '');
    };
</script>
</asp:Content>

