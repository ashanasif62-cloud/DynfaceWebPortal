<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSHRApplicants_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSHRApplicants_ListPage" %>
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

    <asp:GridView 
        ID="gvApplicants" 
        runat="server" 
        AutoGenerateColumns="False"
        CssClass="table table-striped table-bordered"
        EmptyDataText="No applicants found"
        ShowHeaderWhenEmpty="true">

        <Columns>
                       <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
  
    <ItemTemplate>
        <asp:CheckBox ID="chk_SelectSingle" OnCheckedChanged="chk_SelectSingle_CheckedChanged" AutoPostBack="true" runat="server" CssClass="round-checkbox"  />
    </ItemTemplate>
</asp:TemplateField>


            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                      <asp:LinkButton 
      ID="lnkName" 
      runat="server" 
      Text='<%# Eval("FirstName") %>' 
      CommandArgument='<%# Eval("FirstName") %>' 
      OnClick="lnkFirstName_Click" />
                   
                </ItemTemplate>
            </asp:TemplateField>

          
           <asp:TemplateField HeaderText="Applicant">
    <ItemTemplate>
         <asp:Label 
     ID="lblApplicant" 
     runat="server" 
     Text='<%# Bind("Applicant") %>'>
 </asp:Label>
      
    </ItemTemplate>
</asp:TemplateField>

            <asp:TemplateField HeaderText="Applicant Type">
                <ItemTemplate>
                    <asp:Label 
                        ID="lblApplicantType" 
                        runat="server" 
                        Text='<%# Bind("applicantType") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

          
            <asp:TemplateField HeaderText="Highest Degree">
                <ItemTemplate>
                    <asp:Label 
                        ID="lblHighestDegree" 
                        runat="server" 
                        Text='<%# Bind("EducationLevelId") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

              
            <asp:TemplateField HeaderText="Display As" Visible="false">
                <ItemTemplate>
                    <asp:Label 
                        ID="lbldisplayas" 
                        runat="server" 
                        Text='<%# Bind("NameSequenceDisplayAs") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

               <asp:TemplateField HeaderText="RecId" Visible="false">
       <ItemTemplate>
           <asp:Label 
               ID="lblRecId" 
               runat="server" 
               Text='<%# Bind("RecId") %>'>
           </asp:Label>
       </ItemTemplate>
   </asp:TemplateField>


        </Columns>

    </asp:GridView>

</asp:Content>
