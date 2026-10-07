<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseRequisitionHeader_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.PR.PurchaseRequisitionHeader_Listpage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>
            <div class="action-items">
                <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/PurchaseRequisition_Create.aspx')"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
            </div>
            <div class="action-items">
                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
            </div>
                <div class="action-items">
<asp:LinkButton ID="btnPRTotal" runat="server" OnClick="btnPRTotal_Click"><i class=""></i>Totals</asp:LinkButton>
</div>
           <div class="action-items dropdown">
    <!-- Workflow toggle -->
    <asp:LinkButton ID="btnWorkflow" runat="server"
        CssClass="dropdown-toggle"
        data-bs-toggle="dropdown"
        aria-expanded="false">
        <i class="mdi mdi-sitemap"></i> Workflow
    </asp:LinkButton>

    <!-- Dropdown menu -->
    <ul class="dropdown-menu">
        <li>
            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="dropdown-item" OnClick="btnSubmit_Click">
                <i class="mdi mdi-shape-plus"></i> Submit
            </asp:LinkButton>
        </li>
    </ul>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>      
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div>
                <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
                    OnRowEditing="gridView_RowEditing" OnRowDataBound="gridView_RowDataBound" OnRowCommand="gridView_RowCommand" AutoGenerateColumns="false">

                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <HeaderTemplate>
                                <input type="checkbox" id="chk_SelectAll" cssclass="round-checkbox" AutoPostBack="true"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle" AutoPostBack="true" OnCheckedChanged="chk_SelectSingle_CheckedChanged" runat="server" CssClass="round-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Purchase requisition">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkPurchReqId"
                                    runat="server"
                                    Text='<%# Eval("PurchReqId") %>'
                                    CommandArgument='<%# Eval("PurchReqId") %>'
                                    OnClick="lnkPurchReqId_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchReqName" runat="server" Text='<%# Bind("PurchReqName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Preparer">
                            <ItemTemplate>
                                <asp:Label ID="lblOriginator" runat="server" Text='<%# Bind("Originator") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Project Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblProjId" runat="server" Text='<%# Bind("ProjId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblRequisitionStatus" runat="server" Text='<%# Bind("RequisitionStatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created date">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedDateTime" runat="server" Text='<%# Bind("CreatedDateTime") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCreatedDateTime" runat="server" Text='<%# Bind("CreatedDateTime") %>' autocomplete="off" masktype="date"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Submitted date">
                            <ItemTemplate>
                                <asp:Label ID="lblSubmittedDateTime" runat="server" Text='<%# Bind("SubmittedDateTime") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSubmittedDateTime" runat="server" Text='<%# Bind("SubmittedDateTime") %>' autocomplete="off" masktype="date"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Requested date" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRequestedDate" runat="server" Text='<%# Bind("RequiredDate") %>' masktype="date"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRequestedDate" runat="server" Text='<%# Bind("RequiredDate") %>' autocomplete="off" masktype="date"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Accounting date" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAccountingDate" runat="server" Text='<%# Bind("TransDate") %>' masktype="date"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAccountingDate" runat="server" Text='<%# Bind("TransDate") %>' autocomplete="off" masktype="date"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created By" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Bind("CreatedBy") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>

                        <asp:TemplateField HeaderText="Justiication Reason Code" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblReasonCode" runat="server" Text='<%# Bind("PurchReqJustificationReasonCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


<asp:TemplateField HeaderText="Submitted By" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblSubmittedBy" runat="server" Text='<%# Bind("SubmittedBy") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Requisition purpose" >
                            <ItemTemplate>
                                <asp:Label ID="lblRequisitionPurpose" runat="server" Text='<%# Bind("RequisitionPurpose") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>
                                <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" Text="Attachment" runat="server" Visible="false" OnClick="btnAttachment_Click" />
                            </ItemTemplate>
                            <EditItemTemplate>
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gridView" EventName="RowCommand" />
        </Triggers>
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
        function refreshParentGrid() {
            __doPostBack('RefreshGrid', '');
        }
    </script>
</asp:Content>

