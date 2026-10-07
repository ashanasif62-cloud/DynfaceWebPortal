<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSOvertimePlanner_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.TA.ESSOvertimePlanner_ListPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
        .pagination-container {
            display: flex;
            align-items: center;
            justify-content: flex-end;
            padding: 1rem;
            gap: 1rem;
            background: #fff;
            border-top: 1px solid #eee;
        }
        .page-info {
            font-size: 0.875rem;
            color: #666;
            font-weight: 500;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">

    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>

            <div class="action-items">
                <asp:LinkButton ID="btnNew" runat="server"
                    OnClientClick="javascript: return openPopupPanel('/ESS/TA/ESSOvertimePlanner_Create.aspx')">
                    <i class="mdi mdi-plus"></i> New
                </asp:LinkButton>
            </div>
            <div class="action-items">
    <asp:LinkButton ID="btnEdit" runat="server"
        OnClick="btnEdit_Click">
        <i class="mdi mdi-pencil"></i> Edit
    </asp:LinkButton>
</div>

            <div class="action-items">
                <asp:LinkButton ID="btnDelete" runat="server"
                    OnClick="btnDelete_Click">
                    <i class="mdi mdi-delete"></i> Delete
                </asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <asp:UpdatePanel ID="upGrid" runat="server"
        UpdateMode="Conditional"
        ChildrenAsTriggers="true">

        <ContentTemplate>

            <div>
                <%-- Hidden button used only for refreshing the grid from popup --%>
<asp:LinkButton ID="btnRefreshGrid" runat="server" 
    OnClick="btnRefreshGrid_Click" 
    style="display:none;">
</asp:LinkButton>

                <asp:GridView ID="gridView"
                    runat="server"
                    CssClass="table table-condensed no-border table-hover sortable"
                    ShowHeaderWhenEmpty="true"
                    EmptyDataText="No Record Found."
                    AutoGenerateColumns="false"
                  DataKeyNames="recId,RecId"
                    OnRowEditing="gridView_RowEditing"
                    OnRowCancelingEdit="gridView_RowCancelingEdit"
                    OnRowUpdating="gridView_RowUpdating"
                     OnRowDataBound="gridView_RowDataBound">

                    <Columns>

                        <%-- Checkbox --%>
                        <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle" runat="server" CssClass="round-checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Plan ID --%>
                        <asp:TemplateField HeaderText="Plan ID">
                            <ItemTemplate>
                                <asp:Label ID="lblPlanId" runat="server" Text='<%# Bind("planId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- FIX: EditItemTemplate added so FindControl("lblEmployeeId") works in edit mode --%>
                        <asp:TemplateField HeaderText="Employee Id">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("employeeId") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Bind("employeeId") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- FIX: EditItemTemplate added so FindControl("lblEmployeeName") works in edit mode --%>
                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("employeeName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("employeeName") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- FIX: EditItemTemplate added — THIS was the ROOT CAUSE of Plan Date disappearing
                             Without this, FindControl("lblRequestDate") returns null in edit mode,
                             the code-behind crashes silently and the row resets --%>
                        <asp:TemplateField HeaderText="Request Date">
                            <ItemTemplate>
                                <asp:Label ID="lblRequestDate" runat="server"
                                    Text='<%# Eval("requestDate", "{0:M/d/yyyy}") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblRequestDate" runat="server"
                                    Text='<%# Eval("requestDate", "{0:M/d/yyyy}") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Plan Date with date picker --%>
                        <asp:TemplateField HeaderText="Plan Date">
                            <ItemTemplate>
                                <asp:Label ID="lblPlanDate" runat="server"
                                    Text='<%# Eval("planDate", "{0:M/d/yyyy}") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPlanDate" runat="server"
                                    Text='<%# Bind("planDate", "{0:yyyy-MM-dd}") %>'
                                    TextMode="Date">
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Start Time: view=12hr label, edit=12hr textbox "08:47 AM" --%>
                   <%--     <asp:TemplateField HeaderText="Start Time">
                            <ItemTemplate>
                                <asp:Label ID="lblStartTime" runat="server"
                                    Text='<%# FormatTime(Eval("startTime")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStartTime" runat="server"
                                    Text='<%# ConvertSecondsToTime(Eval("startTime")) %>'
                                    placeholder="e.g. 08:30 AM">
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>--%>

                            <%-- Start Time: display in HH:mm:ss format --%>
                        <asp:TemplateField HeaderText="Start Time">
                            <ItemTemplate>
                                <asp:Label ID="lblStartTime" runat="server"
                                    Text='<%# FormatTime(Eval("startTime")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStartTime" runat="server"
                                    Text='<%# ConvertSecondsToTime(Eval("startTime")) %>'
                                    TextMode="Time"
                                    placeholder="HH:mm:ss">
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- End Time: view=12hr label, edit=12hr textbox "03:46 PM" --%>
                       <%-- <asp:TemplateField HeaderText="End Time">
                            <ItemTemplate>
                                <asp:Label ID="lblEndTime" runat="server"
                                    Text='<%# FormatTime(Eval("endTime")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndTime" runat="server"
                                    Text='<%# ConvertSecondsToTime(Eval("endTime")) %>'
                                    placeholder="e.g. 05:00 PM">
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>--%>

                             <asp:TemplateField HeaderText="End Time">
                            <ItemTemplate>
                                <asp:Label ID="lblEndTime" runat="server"
                                    Text='<%# FormatTime(Eval("endTime")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndTime" runat="server"
                                    Text='<%# ConvertSecondsToTime(Eval("endTime")) %>'
                                    TextMode="Time"
                                    placeholder="HH:mm:ss">
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- WF Status (read-only) --%>
                        <asp:TemplateField HeaderText="WF Status">
                            <ItemTemplate>
                                <asp:Label ID="lblWFStatus" runat="server" Text='<%# Bind("wfStatus") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- RecId hidden --%>
                        <asp:TemplateField HeaderText="RecId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("recId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                     
                       

                    </Columns>

                </asp:GridView>

            </div>

        </ContentTemplate>

    </asp:UpdatePanel>

  <script type="text/javascript">
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
  </script>

</asp:Content>
