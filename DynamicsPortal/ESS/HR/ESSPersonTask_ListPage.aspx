<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSPersonTask_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSPersonTask_ListPage" %>


<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
  
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            $('#<%= ddlFilterTasks.ClientID %>').change(function () {
                var filter = $(this).val();

                $('#<%= gvTasks.ClientID %> tr').each(function () {
                    var statusElem = $(this).find('.task-status');

                    if (statusElem.length > 0) {
                        var status = statusElem.attr('data-status').trim().toLowerCase();

                        if (filter === 'Active') {
                            if (status === 'in progress' || status === 'not started') {
                                $(this).show();
                            } else {
                                $(this).hide();
                            }
                        } else {
                            $(this).show();
                        }
                    }
                });
            });

          
            $('#<%= ddlFilterTasks.ClientID %>').trigger('change');
        });
    </script>
</asp:Content>


<asp:Content ID="ActionPanelContent" ContentPlaceHolderID="ActionPanel" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="updButtons" runat="server">
        <ContentTemplate>
            <div class="action-items">

              
                <asp:DropDownList ID="ddlStatus" runat="server"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                    <asp:ListItem Text="Select Status" Value="" />
                    <asp:ListItem Text="In Progress" Value="In Progress" />
                    <asp:ListItem Text="Completed" Value="Completed" />
                    <asp:ListItem Text="Canceled" Value="Canceled" />
                </asp:DropDownList>

              
                <asp:DropDownList ID="ddlFilterTasks" runat="server">
                    <asp:ListItem Text="Active tasks" Value="Active" />
                    <asp:ListItem Text="All tasks" Value="All" />
                </asp:DropDownList>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


<asp:Content ID="PageContentContent" ContentPlaceHolderID="PageContent" runat="server">

    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="table-responsive">
                <asp:GridView ID="gvTasks" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-striped"
                    EmptyDataText="No records found"
                    ShowHeaderWhenEmpty="true"
                    OnRowCommand="gvTasks_RowCommand">

                    <Columns>

                     <%-- 
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_SelectSingle"
                                    runat="server"
                                    AutoPostBack="true"
                                    OnCheckedChanged="chk_SelectSingle_CheckedChanged" />
                                <asp:HiddenField ID="hfRecId"
                                    runat="server"
                                    Value='<%# Eval("recId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                         <asp:TemplateField>
    <ItemTemplate>
        <asp:RadioButton ID="rbtnSelect" runat="server" GroupName="TaskSelection" />
        <asp:HiddenField ID="hfRecId" runat="server" Value='<%# Eval("recId") %>' />
    </ItemTemplate>
</asp:TemplateField>

                      
                        <asp:TemplateField HeaderText="Task">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTaskName" runat="server"
                                    Text='<%# Eval("name") %>'
                                    CommandName="ViewTask"
                                    CommandArgument='<%# Eval("recId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <%# Eval("description") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                     
                        <asp:TemplateField HeaderText="Regarding">
                            <ItemTemplate>
                                <%# Eval("displayRegarding") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                      
                        <asp:TemplateField HeaderText="Due Date">
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("dueDate")).ToString("yyyy-MM-dd") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                      
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class="task-status"
                                      data-status='<%# Eval("Status") %>'>
                                    <%# Eval("Status") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Process Type">
                            <ItemTemplate>
                                <%# Eval("displayProcessType") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>