<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSHRRequestForLetter_ListPage.aspx.cs" Inherits="DynamicsPortal.ESSHRRequestForLetter_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
<asp:UpdatePanel ID="updButtons" runat="server">
    <ContentTemplate>
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/HR/ESSHRRequestForLetter_Create.aspx');"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
    </div>
    <%--    <div class="action-items">
        <asp:LinkButton ID="btnEdit" runat="server"><i class="mdi mdi-border-color"></i>Edit</asp:LinkButton>
    </div>--%>
      <div class="action-items">
      <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click"><i class="mdi mdi-edit"></i>Edit</asp:LinkButton>
  </div>
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
                      </ContentTemplate>
</asp:UpdatePanel> 
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
      <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
  <ContentTemplate>
    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
            OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false" UseAccessibleHeader="true">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Certificate Id">
                    <ItemTemplate>
                        <asp:Label ID="lblCertificateId" runat="server" Text='<%# Bind("CertificateId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job" visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblJobId" runat="server" Text='<%# Bind("JobId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department" visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("Department") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requested Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedDate" runat="server" Text='<%# Bind("RequestedDate") %>' TextMode="date"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Certificate Type">
                    <ItemTemplate>
                        <asp:Label ID="lblCertificateType" runat="server" Text='<%# Bind("CertificateTypeCode") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
             <%--   <asp:TemplateField HeaderText="Authentication">
                    <ItemTemplate>
                        <asp:Label ID="lblAuthentication" runat="server" Text='<%# Bind("Authentication") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAuthentication" runat="server" Text='<%# Bind("Authentication") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Requested For">
                    <ItemTemplate>
                        <asp:Label ID="lblReqestedFor" runat="server" Text='<%# Bind("ReqestedFor") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlRequestedFor" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
              <%--  <asp:TemplateField HeaderText="No. of Copies">
                    <ItemTemplate>
                        <asp:Label ID="lblCopies" runat="server" Text='<%# Bind("Copies") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
             <%--   <asp:TemplateField HeaderText="Reason">
                    <ItemTemplate>
                        <asp:Label ID="lblReason" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlReason" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Remarks">
                    <ItemTemplate>
                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Workflow State">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkFlowState" runat="server" Text='<%# Bind("WorkFlowState") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            <%--    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                        <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" Text="Update" runat="server" OnClick="Update_Click" />
                        <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" Text="Cancel" runat="server" OnClick="Cancel_Click" />
                    </EditItemTemplate>
                </asp:TemplateField>--%>
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
