<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="TASEmployeeConsentRequest_ListPage.aspx.cs" 
    Inherits="DynamicsPortal.ESS.TA.TASEmployeeConsentRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>
            <div class="action-items dropdown">
                <asp:LinkButton ID="btnConsent" runat="server" CssClass="dropdown-toggle"
                    data-bs-toggle="dropdown" aria-expanded="false"
                    OnClientClick="return validateSelection();">
                    <i class="mdi mdi-sitemap"></i> Consent
                </asp:LinkButton>

                <ul class="dropdown-menu">
                    <li>
                        <asp:LinkButton ID="btnYes" runat="server" CssClass="dropdown-item"
                            OnClick="btnYes_Click">
                            <i class="mdi mdi-check"></i> Yes
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnNo" runat="server" CssClass="dropdown-item"
                            OnClick="btnNo_Click">
                            <i class="mdi mdi-close"></i> No
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div>
                <asp:GridView ID="gridView" runat="server" data="searchable" 
                    CssClass="table table-condensed no-border table-hover sortable" 
                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." 
                    DataKeyNames="RecId"
                    OnRowEditing="gridView_RowEditing" 
                    OnRowDataBound="gridView_RowDataBound" 
                    AutoGenerateColumns="false" 
                    EnableViewState="true" 
                    UseAccessibleHeader="true">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <HeaderTemplate>
                                <input type="checkbox" id="chk_SelectAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle" runat="server" 
                                    AutoPostBack="true" 
                                    OnCheckedChanged="chk_SelectSingle_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Employee">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployee" runat="server" Text='<%# Bind("EmployeeId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Request Type">
                            <ItemTemplate>
                                <asp:Label ID="lblRequestType" runat="server" Text='<%# Bind("consentType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("consentDescription") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Start Date">
                            <ItemTemplate>
                                <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("StartDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="End Date">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("ExpireDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Consent Status">
                            <ItemTemplate>
                                <asp:Label ID="lblConsentStatus" runat="server" Text='<%# Bind("ConsentRequestStatus") %>' masktype="enum"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>
                                <asp:LinkButton CssClass="grid-img-btn btn-edit" ToolTip="Edit" 
                                    CausesValidation="false" Text="Edit" runat="server" CommandName="Edit" />
                                <asp:LinkButton CssClass="grid-img-btn btn-attachment" ToolTip="Attachment" 
                                    Text="Attachment" runat="server" OnClick="btnAttachment_Click" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton CssClass="grid-img-btn btn-update" ToolTip="Update" 
                                    Text="Update" runat="server" OnClick="Update_Click" />
                                <asp:LinkButton CssClass="grid-img-btn btn-cancel" ToolTip="Cancel" 
                                    Text="Cancel" runat="server" OnClick="Cancel_Click" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" Visible="false" />
                        <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" Visible="false" />
                        <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" Visible="false" />
                        <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" Visible="false" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" Visible="false" />
                        <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" Visible="false" />
                        <asp:BoundField DataField="Partition" HeaderText="Partition" Visible="false" />

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
            showAJAXOverlay();
        });
        prm.add_endRequest(function () {
            hideAJAXOverlay();
        });

        function getCheckedRows() {
            var boxes = document.querySelectorAll('input[id*="chk_SelectSingle"]');
            var checked = [];
            for (var i = 0; i < boxes.length; i++) {
                if (boxes[i].checked) checked.push(boxes[i]);
            }
            return checked;
        }

        // Validate selection before opening dropdown
        function validateSelection() {
          
            return true;
        }

        window.refreshParentGrid = function () {
            __doPostBack('RefreshGrid', '');
        };
    </script>
</asp:Content>