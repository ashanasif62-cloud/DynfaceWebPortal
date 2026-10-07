<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSPREmployeeAdvance_ListPage.aspx.cs" Inherits="DynamicsPortal.ESSPREmployeeAdvance_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
<asp:UpdatePanel ID="updButtons" runat="server">
    <ContentTemplate>
    <div class="action-items">
        <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/ESSPREmployeeAdvance_Create.aspx');"><i class="mdi mdi-plus"></i>Advance Salary</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnNewLoan" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/ESSPREmployeeLoanReq_Create.aspx');"><i class="mdi mdi-plus"></i>Loan Request</asp:LinkButton>
    </div>
    <%--    <div class="action-items">
        <asp:LinkButton ID="btnEdit" runat="server"><i class="mdi mdi-border-color"></i>Edit</asp:LinkButton>
    </div>--%>
    <div class="action-items">
        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" OnClientClick="return false;"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
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
            OnRowDataBound="gridView_RowDataBound" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Advance Request Id" HeaderStyle-Width="131px">
                    <ItemTemplate>
                        <asp:Label ID="lblAdvanceRequestId" runat="server" Text='<%# Bind("AdvanceRequestId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployee" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Guarantor1" HeaderStyle-Width="130px" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblGuarantor1" runat="server" Text='<%# Bind("Guarantor1") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGuarantor1" runat="server" masktype="number" Text='<%# Bind("Guarantor1") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Guarantor2" HeaderStyle-Width="130px" Visible ="false">
                    <ItemTemplate>
                        <asp:Label ID="lblGuarantor2" runat="server" Text='<%# Bind("Guarantor2") %>' masktype="number"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGuarantor2" runat="server" masktype="number" Text='<%# Bind("Guarantor2") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Advance Type Code" HeaderStyle-Width="125px">
                    <ItemTemplate>
                        <asp:Label ID="lblAdvanceTypeCode" runat="server" Text='<%# Bind("AdvanceTypeCode") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlAdvanceTypeCode" AutoPostBack="true" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Advance Description" HeaderStyle-Width="130px">
                      <ItemTemplate>
                          <asp:Label ID="lblAdvanceDescription" runat="server" Text='<%# Bind("AdvanceDescription") %>'></asp:Label>
                      </ItemTemplate>
                      <EditItemTemplate>
                          <asp:TextBox ID="txtAdvanceDescription" runat="server" Text='<%# Bind("AdvanceDescription") %>'></asp:TextBox>
                      </EditItemTemplate>
                  </asp:TemplateField>
                <asp:TemplateField HeaderText="Advance Amount" HeaderStyle-Width="115px">
                    <ItemTemplate>
                        <asp:Label ID="lblAdvanceAmount" runat="server" Text='<%# Bind("AdvanceAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAdvanceAmount" runat="server" Text='<%# Bind("AdvanceAmount") %>' masktype="number"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
              
                <asp:TemplateField HeaderText="Requested Date" HeaderStyle-Width="110px">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Bind("RequestDate") %>' TextMode="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRequestDate" runat="server" Enabled="false" Text='<%# Bind("RequestDate") %>' autocomplete="off" masktype="date"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Payment Date" HeaderStyle-Width="95px">
                        <ItemTemplate>
                            <asp:Label ID="lblRequestedPaymentDate" runat="server" Text='<%# Bind("RequestedPaymentDate") %>' TextMode="date"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRequestedPaymentDate" runat="server" Text='<%# Bind("RequestedPaymentDate") %>' autocomplete="off" TextMode="Date"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                <asp:TemplateField HeaderText="Recovery Start Date" HeaderStyle-Width="130px">
                    <ItemTemplate>
                        <asp:Label ID="lblRecoveryStartDate" runat="server" Text='<%# Bind("RecoveryStartDate") %>' TextMode="date"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                           <asp:TextBox ID="txtRecoveryStartDate" runat="server" Enabled="false" masktype="date" Text='<%# Bind("RecoveryStartDate") %>'></asp:TextBox>
                        </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Recovery Amount" HeaderStyle-Width="115px">
                    <ItemTemplate>
                        <asp:Label ID="lblAmountRecovered" runat="server" Text='<%# Bind("RequestedInstallmentAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAmountRecovered" runat="server" Enabled="false" Text='<%# Bind("RequestedInstallmentAmount") %>' masktype="number"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Recoveries" HeaderStyle-Width="85px">
                    <ItemTemplate>
                        <asp:Label ID="lblRecoveries" runat="server" Text='<%# Bind("RequestedInstallments") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRecoveries" runat="server" Enabled="false" Text='<%# Bind("RequestedInstallments") %>' masktype="number"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="95px">
                    <ItemTemplate>
                        <asp:Label ID="lblWFStatus" runat="server" Text='<%# Bind("WFStatus") %>' masktype="enum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" Text="Edit" runat="server" OnClick="Unnamed_Click"/>
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

    function getCheckedRows() {
        var boxes = document.querySelectorAll('input[id*="chk_SelectSingle"]');
        var checked = [];
        for (var i = 0; i < boxes.length; i++) {
            if (boxes[i].checked) checked.push(boxes[i]);
        }
        return checked;
    }

    document.addEventListener('click', function (e) {
        var el = e.target;
        var deleteBtn = null;

        // Walk up DOM to find the btnDelete anchor
        while (el && el !== document) {
            if (el.tagName === 'A' && el.id && el.id.indexOf('btnDelete') !== -1) {
                deleteBtn = el;
                break;
            }
            el = el.parentElement;
        }

        if (!deleteBtn) return;

        e.preventDefault();
        e.stopPropagation();

        // Block delete if grid is in edit mode
        if (document.querySelector('a.btn-cancel')) {
            alert('Please save or cancel the current edit before deleting.');
            return;
        }

        if (deleteBtn.classList.contains('aspNetDisabled')) return;

        if (getCheckedRows().length === 0) return;

        GlobalDeleteConfirm.show({
            message: 'You are about to delete a record in Contact Details. This action cannot be undone.',
            onConfirm: function () {
                __doPostBack('<%= btnDelete.UniqueID %>', '');
                        }
        });
    });
</script>
</asp:Content>

