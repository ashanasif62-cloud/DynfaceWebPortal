<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ESSComplainAndSuggestion_ListPage.aspx.cs"
    Inherits="DynamicsPortal.ESS.PR.ESSComplainAndSuggestion_ListPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">

    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>

            <%-- New --%>
            <div class="action-items">
                <asp:LinkButton ID="btnNew"
                    runat="server"
                    OnClientClick="javascript: return openPopupPanel('/ESS/PR/ESSComplainAndSuggestion_Create.aspx')">

                    <i class="mdi mdi-plus"></i> New

                </asp:LinkButton>
            </div>

              <div class="action-items">
      <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
  </div>


            <%-- Edit --%>
         <%--   <div class="action-items">
                <asp:LinkButton ID="btnEdit"
                    runat="server"
                    OnClick="btnEdit_Click">

                    <i class="mdi mdi-pencil"></i> Edit

                </asp:LinkButton>
            </div>--%>


            <%-- Delete --%>
           <%-- <div class="action-items">
                <asp:LinkButton ID="btnDelete"
                    runat="server"
                    OnClick="btnDelete_Click">

                    <i class="mdi mdi-delete"></i> Delete

                </asp:LinkButton>
            </div>--%>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <asp:UpdatePanel ID="upGrid"
        runat="server"
        UpdateMode="Conditional"
        ChildrenAsTriggers="true">

        <ContentTemplate>

            <div>

               
             <%--   <asp:LinkButton ID="btnRefreshGrid"
                    runat="server"
                    OnClick="btnRefreshGrid_Click"
                    style="display:none;">
                </asp:LinkButton>--%>


                <asp:GridView ID="gridView"
                    runat="server"
                    CssClass="table table-condensed no-border table-hover sortable"
                    ShowHeaderWhenEmpty="true"
                    EmptyDataText="No Record Found."
                    AutoGenerateColumns="false"
                    DataKeyNames="RecId">


                    <Columns>

                      
                    <%--    <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>

                                <asp:CheckBox ID="chk_SelectSingle"
                                    runat="server"
                                    CssClass="round-checkbox" />

                            </ItemTemplate>
                        </asp:TemplateField>--%>


                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort" ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="round-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>

                      
                        <asp:TemplateField HeaderText="Request ID">
                            <ItemTemplate>

                                <asp:Label ID="lblComplainSuggesstionReqId"
                                    runat="server"
                                    Text='<%# Bind("ComplainSuggesstionReqId") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Employee ID">
                            <ItemTemplate>

                                <asp:Label ID="lblEmployeeId"
                                    runat="server"
                                    Text='<%# Bind("PersonnelNumber") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>


                    
                        <asp:TemplateField HeaderText="Request Date">
                            <ItemTemplate>

                                <asp:Label ID="lblRequestDate"
                                    runat="server"
                                    Text='<%# Eval("RequestDate", "{0:M/d/yyyy}") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Complain Type Code">
                            <ItemTemplate>

                                <asp:Label ID="lblComplainTypeCode"
                                    runat="server"
                                    Text='<%# Bind("TypeCode") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>


                      <%--  <asp:TemplateField HeaderText="Suggestion Type Code" Visible="false">
                            <ItemTemplate>

                                <asp:Label ID="lblSuggestionTypeCode"
                                    runat="server"
                                    Text='<%# Bind("SuggesstionTypeCode") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>--%>


                      
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>

                                <asp:Label ID="lblDescription"
                                    runat="server"
                                    Text='<%# Bind("Description") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>


                   
                        <asp:TemplateField HeaderText="Approval Status">
                            <ItemTemplate>

                                <asp:Label ID="lblApprovalStatus"
                                    runat="server"
                                    Text='<%# Bind("ApprovalStatus") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>


                      
                        <asp:TemplateField HeaderText="Requested By">
                            <ItemTemplate>

                                <asp:Label ID="lblRequestedBy"
                                    runat="server"
                                    Text='<%# Bind("RequestedBy") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>


                     
                        <asp:TemplateField HeaderText="RecId" Visible="false">
                            <ItemTemplate>

                                <asp:Label ID="lblRecId"
                                    runat="server"
                                    Text='<%# Bind("RecId") %>' />

                            </ItemTemplate>
                        </asp:TemplateField>


                    </Columns>

                </asp:GridView>

            </div>

        </ContentTemplate>

    </asp:UpdatePanel>
<%-- <script type="text/javascript">
     function refreshParentGrid() {
         // Trigger the hidden button that will rebind the grid
         var btn = document.getElementById('<%= btnRefreshGrid.ClientID %>');
       if (btn) {
           btn.click();
       } else {
           // Fallback
           __doPostBack('<%= upGrid.ClientID %>', 'RefreshGrid');
         }
     }
 </script>--%>




</asp:Content>

