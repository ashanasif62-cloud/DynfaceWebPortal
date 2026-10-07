<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSHRRecruitmentDetailForm.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSHRRecruitmentDetailForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
   <style>
    .rotate-icon {
    transition: transform 0.2s ease;
}

.collapsed .rotate-icon {
    transform: rotate(-90deg);
}

.small-icon {
    font-size: 12px;
}
       </style>
  

        <!-- SAVE -->
       
    
         <div class="action-items">
     <asp:LinkButton ID="btnSave" runat="server"  OnClick="btnDelete_Click" >
         <i class="mdi mdi-content-save"></i> Save
     </asp:LinkButton>
 </div>

         <div class="action-items">
     <asp:LinkButton ID="btnDelete" runat="server"  OnClick="btnDelete_Click" >
         <i class="mdi mdi-delete"></i>Delete
     </asp:LinkButton>
 </div>

    

    

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <div class="card mb-3">

        <!-- Card Header -->
        <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center"
             id="headingDetail">

            <a class="text-dark d-flex justify-content-between w-100"
               data-toggle="collapse"
               href="#collapseDetail"
               role="button"
               aria-expanded="true"
               aria-controls="collapseDetail">

                <strong>Detail</strong>

                <span class="ml-auto">
                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                </span>
            </a>
        </div>

        <!-- Collapsible Body -->
        <div id="collapseDetail"
             class="collapse show"
             aria-labelledby="headingDetail">

            <div class="card-body bg-white">

      <div class="row">

    <!-- ================= OVERVIEW ================= -->
    <div class="col-md-4">
        <h6 class="font-weight-bold mb-2">OVERVIEW</h6>

        <asp:HiddenField ID="hdnRecruitmentProject" runat="server"  Visible="false"/>

        <div class="form-group">
            <label>Recruitment project</label>
            <asp:TextBox  ID="txtRecruitmentProject" runat="server"
                CssClass="form-control "  Style="width:200px;" />
        </div>

        <div class="form-group">
            <label>Description</label>
            <asp:TextBox  ID="txtDescription" runat="server"
                CssClass="form-control" Style="width:200px;" />
        </div>

        <h6 class="font-weight-bold mt-3 mb-2">ORGANIZATION</h6>

        <div class="form-group">
            <label>Department</label>
            <asp:TextBox  ID="txtDepartment" runat="server"
                CssClass="form-control"  Style="width:200px;" />
        </div>
    </div>

    <!-- ================= JOB + CONTACTS ================= -->
    <div class="col-md-4">
       

        <div class="form-group">
            <label>Recruiter</label>
            <asp:TextBox  ID="txtRecruiter" runat="server"
                CssClass="form-control form-control-sm"  Style="width:200px;" />
        </div>

        <div class="form-group">
            <label>Project status</label>
            <asp:TextBox  ID="txtProjectStatus" runat="server"
                CssClass="form-control form-control-sm"  Style="width:200px;" />
        </div>

           <div class="form-group">
       <label>Job</label>
       <asp:TextBox  ID="txtJob" runat="server"
           CssClass="form-control form-control-sm"  Style="width:200px;" />
   </div>

        <div class="form-group">
            <label>Number of openings</label>
            <asp:TextBox  ID="txtNumberOfOpenings" runat="server"
                CssClass="form-control form-control-sm"  Style="width:200px;" />
        </div>

        <h6 class="font-weight-bold mt-3 mb-2">CONTACTS</h6>

        <div class="form-group">
            <label>Hiring manager</label>
            <asp:TextBox  ID="txtHiringManager" runat="server"
                CssClass="form-control form-control-sm"  Style="width:200px;" />
        </div>

        <div class="form-group">
            <label>Alternative contact</label>
            <asp:TextBox  ID="txtAlternativeContact" runat="server"
                CssClass="form-control form-control-sm"  Style="width:200px;" />
        </div>
    </div>

    <!-- ================= DATES ================= -->
    <div class="col-md-4">
        <h6 class="font-weight-bold mb-2">DATES</h6>

        <div class="row">
            <!-- LEFT -->
            <div class="col-6">
                <div class="form-group">
                    <label>Requisition approved on</label>
                    <asp:TextBox  ID="txtReqApprovedOn" runat="server"  TextMode="Date"
                        CssClass="form-control form-control-sm"  Style="width:200px;" />
                </div>

                <div class="form-group">
                    <label>Open date</label>
                    <asp:TextBox  ID="txtOpenDate" runat="server"  TextMode="Date"
                        CssClass="form-control form-control-sm"  Style="width:200px;" />
                </div>

                <div class="form-group">
                    <label>Application deadline</label>
                    <asp:TextBox  ID="txtApplicationDeadline" runat="server"  TextMode="Date"
                        CssClass="form-control form-control-sm"  Style="width:200px;" />
                </div>
            </div>

            <!-- RIGHT -->
            <div class="col-6">
                <div class="form-group">
                    <label>Close date</label>
                    <asp:TextBox  ID="txtCloseDate" runat="server"  TextMode="Date"
                        CssClass="form-control form-control-sm"  Style="width:200px;"/>
                </div>

                <div class="form-group">
                    <label>Estimated start date</label>
                    <asp:TextBox  ID="txtEstimatedStartDate" runat="server"  TextMode="Date"
                        CssClass="form-control form-control-sm" Style="width:200px;" />
                </div>

                <div class="form-group">
                    <label>Display on employee self service</label>
                    <asp:TextBox  ID="txtDisplayOnESS" runat="server"
                        CssClass="form-control form-control-sm"  Style="width:200px;" />
                </div>
            </div>
        </div>
    </div>

</div>


            </div>
        </div>

    </div>

</asp:Content>
