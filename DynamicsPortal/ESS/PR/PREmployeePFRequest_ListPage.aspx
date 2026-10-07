<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="PREmployeePFRequest_ListPage.aspx.cs" Inherits="DynamicsPortal.PREmployeePFRequest_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
<asp:UpdatePanel ID="updButtons" runat="server">
    <ContentTemplate>
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/PREmployeePFRequest_Create.aspx');"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
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
                      </ContentTemplate>
</asp:UpdatePanel> 
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
      <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
  <ContentTemplate>
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
                <asp:TemplateField HeaderText="Advance Type Code">
                    <ItemTemplate>
                        <asp:Label ID="lblAdvanceTypeCode" runat="server" Text='<%# Bind("advanceTypeCode") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlAdvanceTypeCode" runat="server" Text='<%# Bind("advanceTypeCode") %>'></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PF Balance">
                    <ItemTemplate>
<%--                        <asp:Label ID="lblPFBalance" runat="server" Text='<%# Bind("pFBalance") %>'></asp:Label>--%>
                        <asp:Label ID="lblPFBalance" runat="server" 
    Text='<%# Eval("pFBalance") != null && Eval("pFBalance").ToString() != "" 
           ? string.Format("{0:N0}", Convert.ToDecimal(Eval("pFBalance"))) 
           : "0" %>'></asp:Label>

                    </ItemTemplate>
                </asp:TemplateField>
               <%-- <asp:TemplateField HeaderText="Employer PF Balance">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployerPFBalance" runat="server" Text='<%# Bind("employerPFBalance") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Request Amount">
                    <ItemTemplate>
<%--                        <asp:Label ID="lblRequestAmount" runat="server" Text='<%# Bind("requestAmount") %>'></asp:Label>--%>
                        <asp:Label ID="lblRequestAmount" runat="server" 
    Text='<%# Eval("requestAmount") != null && Eval("requestAmount").ToString() != "" 
           ? string.Format("{0:N0}", Convert.ToDecimal(Eval("requestAmount"))) 
           : "0" %>'></asp:Label>

                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestAmount" runat="server" Text='<%# Bind("requestAmount") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Recovery Start Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRecoveryStartDate" runat="server" Text='<%# Bind("recoveryStartDate")%>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRecoveryStartDate" runat="server" Text='<%# Bind("recoveryStartDate") %>' autocomplete="off"  masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requested">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedInstallments" runat="server" Text='<%# Bind("requestedInstallments") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestedInstallments" runat="server" Text='<%# Bind("requestedInstallments") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requested Payment Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedPaymentDate" runat="server" Text='<%# Bind("requestedPaymentDate")%>' masktype="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestedPaymentDate" runat="server" Text='<%# Bind("requestedPaymentDate") %>' autocomplete="off"  masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblWFStatus" runat="server" Text='<%# Bind("wFStatus") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                   <%-- <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" CommandName="Edit" />
                        <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                    </ItemTemplate>--%>
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

