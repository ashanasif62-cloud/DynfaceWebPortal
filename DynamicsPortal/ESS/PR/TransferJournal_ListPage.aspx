<%@ Page Title="Transfer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransferJournal_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.PR.TransferJournal_ListPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">

     <div class="action-items">
     <asp:LinkButton ID="btnNew" runat="server" OnClientClick="javascript: return openPopupPanel('/ESS/PR/TransferJournal_Create.aspx', 900);"><i class="mdi mdi-plus"></i>New</asp:LinkButton>
 </div>

     <div class="action-items">
     <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Delete</asp:LinkButton>
 </div>

    <div class="action-items">
    <asp:LinkButton 
        ID="btnPost" 
        runat="server" 
        OnClick="btnPost_Click">
        <i class="mdi mdi-send"></i> Post
    </asp:LinkButton>
</div>

        <div class="action-items">
    <asp:LinkButton ID="btnReport" runat="server" OnClientClick="window.location='/ESS/PR/TransferJournalReport.aspx'; return false;">
        <i class="mdi mdi-file-chart"></i> Journal
    </asp:LinkButton>
</div>

</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="PageContent" runat="server">

    <div>
       <%-- <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." 
         DataKeyNames="RecId" AutoGenerateColumns="false">--%>
         <asp:GridView ID="gridView" runat="server" OnRowCommand="gridView_RowCommand" Data="searchable" CssClass="table table-condensed no-border table-hover sortable"
             ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="RecId"
             AutoGenerateColumns="false">

            <Columns>
                      <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" AutoPostBack="true" OnCheckedChanged="chk_SelectSingle_CheckedChanged" runat="server" CssClass="round-checkbox" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- Journal --%>
                <asp:TemplateField HeaderText="Journal">
                   <%-- <ItemTemplate>
                        <asp:Label ID="lblJournal" runat="server" Text='<%# Bind("JournalId") %>'></asp:Label>
                    </ItemTemplate>--%>
                       <ItemTemplate>
                         <asp:LinkButton
                             ID="lblJournal" runat="server" Text='<%# Bind("JournalId") %>'
                             CommandName="JournalClick"
                             CommandArgument='<%# Container.DataItemIndex %>'
                             CssClass="link-style" />
                     </ItemTemplate>
                </asp:TemplateField>

                <%-- Name --%>
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("JournalName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- Description --%>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- Lines --%>
                <asp:TemplateField HeaderText="Lines">
                    <ItemTemplate>
                        <asp:Label ID="lblLines" runat="server" Text='<%# Bind("NumOfLines") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- Posted --%>
                <asp:TemplateField HeaderText="Posted">
                    <ItemTemplate>
                        <asp:Label ID="lblPosted" runat="server" Text='<%# Bind("Posted") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- In Use --%>
                <asp:TemplateField HeaderText="In Use">
                    <ItemTemplate>
                        <asp:Label ID="lblInUse" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <%-- Workflow approval status --%>
                <asp:TemplateField HeaderText="Workflow approval status">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkflowApprovalStatus" runat="server" Text='<%# Bind("WorkflowStatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                     <asp:TemplateField HeaderText="RecId" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>' />
    </ItemTemplate>
</asp:TemplateField>

                         <asp:TemplateField HeaderText="VoucherSeries" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblVoucherSeries" runat="server" Text='<%# Bind("NumberSequence") %>' />
    </ItemTemplate>
</asp:TemplateField>

     <asp:TemplateField HeaderText="Selection by" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblSelectionBy" runat="server" Text='<%# Bind("VoucherDraw") %>' />
    </ItemTemplate>
</asp:TemplateField>

         <asp:TemplateField HeaderText="New voucher by" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblNewVoucherBy" runat="server" Text='<%# Bind("VoucherChange") %>' />
    </ItemTemplate>
</asp:TemplateField>

         <asp:TemplateField HeaderText="Detail level" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblDetaillevel" runat="server" Text='<%# Bind("DetailSummary") %>' />
    </ItemTemplate>
</asp:TemplateField>

             <asp:TemplateField HeaderText="Delete Posted Lines" Visible="false">
        <ItemTemplate>
            <asp:Label ID="lblDeletePostedLines" runat="server" Text='<%# Bind("DeletePostedLines") %>' />
        </ItemTemplate>
    </asp:TemplateField>

                <asp:TemplateField HeaderText="Invent Site" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblInventSiteId" runat="server" Text='<%# Bind("InventSiteId") %>' />
    </ItemTemplate>
</asp:TemplateField>

                <asp:TemplateField HeaderText="Invent Location" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblInventLocationId" runat="server" Text='<%# Bind("InventLocationId") %>' />
    </ItemTemplate>
</asp:TemplateField>


            </Columns>

        </asp:GridView>
    </div>

</asp:Content>

