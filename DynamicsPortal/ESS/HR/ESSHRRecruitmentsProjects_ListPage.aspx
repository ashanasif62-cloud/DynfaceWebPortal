<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSHRRecruitmentsProjects_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.HR.HRRecruitmentsProjects_ListPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
        <div class="action-items">
    <a href="/ESS/HR/ESSHRRecruitmentsProjects_ListPage.aspx" class="btn-link">
         <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
    </a>
</div>
      <div class="action-items">
      <asp:Button 
        ID="btnApplication" 
        runat="server" 
        Text="Application"
        CssClass="btn btn-primary" 
        OnClick="btnApplication_Click" />

          </div>


     
      
</asp:Content>

 
<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <asp:GridView 
        ID="gvRecruitmentProjects"
        runat="server"
        CssClass="table table-bordered table-striped"
        AutoGenerateColumns="false"
        EmptyDataText="No recruitment projects found"
        ShowHeaderWhenEmpty="true">

        <Columns>
           <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
  
    <ItemTemplate>
        <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true" runat="server" CssClass="round-checkbox"  />
    </ItemTemplate>
</asp:TemplateField>

    <asp:TemplateField HeaderText="Recruitment Project">
    <ItemTemplate>
        <asp:LinkButton 
            ID="lnkRecruitmentProject" 
            runat="server" 
            Text='<%# Eval("RecruitmentProject") %>' 
            CommandArgument='<%# Eval("RecruitmentProject") %>' 
            OnClick="lnkPurchReqId_Click" />
    </ItemTemplate>
</asp:TemplateField>


            
                   <asp:TemplateField HeaderText="Description">
       <ItemTemplate>
           <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
       </ItemTemplate>
   </asp:TemplateField>

              <asp:TemplateField HeaderText="Recruiter" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblRecruiter" runat="server" Text='<%# Bind("Recruiter") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>

                 <asp:TemplateField HeaderText="Project Status" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblProjectStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>

          


           <%-- <asp:TemplateField HeaderText="Open Date">
    <ItemTemplate>
        <asp:Label 
            ID="lblOpenDate" 
            runat="server" 
            Text='<%# Eval("StartDate", "{0:yyyy-MM-dd}") %>' />
    </ItemTemplate>
</asp:TemplateField>

         <asp:TemplateField HeaderText="Application Deadline">
    <ItemTemplate>
        <asp:Label 
            ID="lblApplicationDeadline" 
            runat="server" 
            Text='<%# Eval("ApplicationDeadline", "{0:yyyy-MM-dd}") %>'>
        </asp:Label>
    </ItemTemplate>
</asp:TemplateField>

           <asp:TemplateField HeaderText="Close Date">
    <ItemTemplate>
        <asp:Label 
            ID="lblCloseDate" 
            runat="server" 
            Text='<%# Eval("EndDate", "{0:yyyy-MM-dd}") %>'>
        </asp:Label>
    </ItemTemplate>
</asp:TemplateField>--%>
      <asp:TemplateField HeaderText="Open Date">
    <ItemTemplate>
        <asp:Label 
            ID="lblOpenDate" 
            runat="server" 
            Text='<%# Eval("StartDate") == DBNull.Value ? "" : Convert.ToDateTime(Eval("StartDate")).ToString("M-dd-yyyy") %>'>
        </asp:Label>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Application Deadline">
    <ItemTemplate>
        <asp:Label 
            ID="lblApplicationDeadline" 
            runat="server" 
            Text='<%# Eval("ApplicationDeadline") == DBNull.Value ? "" : Convert.ToDateTime(Eval("ApplicationDeadline")).ToString("M-dd-yyyy") %>'>
        </asp:Label>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Close Date">
    <ItemTemplate>
        <asp:Label 
            ID="lblCloseDate" 
            runat="server" 
            Text='<%# Eval("EndDate") == DBNull.Value ? "" : Convert.ToDateTime(Eval("EndDate")).ToString("M-dd-yyyy") %>'>
        </asp:Label>
    </ItemTemplate>
</asp:TemplateField>



        </Columns>

    </asp:GridView>

</asp:Content>