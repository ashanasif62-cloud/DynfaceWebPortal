<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSHRApplications_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSHRApplications_ListPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
            <div class="action-items">
    <asp:LinkButton 
        ID="btnNew" 
        runat="server" 
        OnClick="btnNew_Click">
        <i class="mdi mdi-plus"></i> New
    </asp:LinkButton>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

  <%--  <div class="card">
        <div class="card-header bg-light">
            <h5 class="mb-0">Applications</h5>
        </div>--%>
    <div class="card-body">
    <div class="table-responsive">

        <asp:GridView 
            ID="gvApplications" 
            runat="server" 
            CssClass="table table-striped table-bordered mb-0"
            EmptyDataText="No applications found"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true">

            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
  
    <ItemTemplate>
        <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true" runat="server" CssClass="round-checkbox"  />
    </ItemTemplate>
</asp:TemplateField>

                   <asp:TemplateField HeaderText="Application">
          <ItemTemplate>
                <asp:LinkButton 
ID="lnkApplication" 
runat="server" 
Text='<%# Eval("ApplicationId") %>' 
CommandArgument='<%# Eval("ApplicationId") %>' 
OnClick="lnkApplication_Click" />
             
          </ItemTemplate>
      </asp:TemplateField>

<%--                <asp:TemplateField HeaderText="Application">
                    <ItemTemplate>
                        <asp:Label ID="lblApplication" runat="server"
                            Text='<%# Eval("ApplicationId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>

                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server"
                            Text='<%# Eval("FirstName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Applicant Type">
                    <ItemTemplate>
                        <asp:Label ID="lblApplicantType" runat="server"
                            Text='<%# Eval("applicantType") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

<%--                <asp:TemplateField HeaderText="Date of Receipt">
                    <ItemTemplate>
                        <asp:Label ID="lblDateOfReceipt" runat="server"
                            Text='<%# Eval("DateOfReception", "{0:yyyy-MM-dd}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Date Of Receipt">
    <ItemTemplate>
        <asp:Label 
            ID="lblDateOfReceipt" 
            runat="server" 
            Text='<%# Eval("DateOfReception") == DBNull.Value ? "" : Convert.ToDateTime(Eval("DateOfReception")).ToString("M-dd-yyyy") %>'>
        </asp:Label>
    </ItemTemplate>
</asp:TemplateField>


                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server"
                            CssClass="badge bg-secondary"
                            Text='<%# Eval("Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Correspondence Action">
                    <ItemTemplate>
                        <asp:Label ID="lblCorrespondenceAction" runat="server"
                            Text='<%# Eval("CorrespondenceAction") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Recruitment Project">
                    <ItemTemplate>
                        <asp:Label ID="lblRecruitmentProject" runat="server"
                            Text='<%# Eval("RecruitmentProject") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

    </div>
</div>

   

</asp:Content>
