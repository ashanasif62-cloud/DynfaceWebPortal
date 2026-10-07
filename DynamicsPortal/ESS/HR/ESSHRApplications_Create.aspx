<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="ESSHRApplications_Create.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSHRApplications_Create" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
     
    <div class="card">

        <!-- ===================== GENERAL PANEL ===================== -->
        <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center"
             id="headingGeneral">

            <a class="text-dark d-flex justify-content-between w-100"
               data-toggle="collapse"
               href="#collapseGeneral"
               role="button"
               aria-expanded="true"
               aria-controls="collapseGeneral">

                <strong>General</strong>

                <span class="ml-auto">
                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                </span>
            </a>
        </div>

        <div id="collapseGeneral"
             class="collapse show"
             aria-labelledby="headingGeneral">
<div class="card-body bg-white">

    <!-- ========== ROW 1 ========== -->
    <div class="row">

        <!-- IDENTIFICATION -->
        <div class="col-md-3">
            <label class="text-bold small">Application</label>
            <asp:TextBox ID="txtApplication" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

        <!-- APPLICANT -->
        <div class="col-md-3">
            <label class="text-bold small">Name</label>
            <asp:TextBox ID="txtApplicantName" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

        <!-- SOURCE -->
        <div class="col-md-3">
            <label class="text-bold small">Created source</label>
            <asp:TextBox ID="txtCreatedSource" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

        <!-- DATE -->
        <div class="col-md-3">
            <label class="text-bold small">Date of receipt</label>
            <asp:TextBox ID="txtDateOfReceipt" runat="server"
                CssClass="form-control form-control-sm"
                TextMode="Date" />
        </div>

    </div>


    <!-- ========== ROW 2 ========== -->
    <div class="row mt-3">

        <div class="col-md-3">
            <label class="text-bold small">Recruitment project</label>
            <asp:DropDownList ID="ddlRecruitmentProject" runat="server"
                CssClass="form-control form-control-sm" />
              <%-- <asp:TextBox ID="txtRecruitment" runat="server"
       CssClass="form-control form-control-sm" />--%>
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Applicant</label>
            <asp:TextBox ID="txtApplicantId" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Media</label>
            <asp:DropDownList ID="ddlMedia" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Reason code</label>
            <asp:DropDownList ID="ddlReasonCode" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

    </div>


    <!-- ========== ROW 3 ========== -->
    <div class="row mt-3">

        <div class="col-md-3">
            <label class="text-bold small">Department</label>
            <asp:DropDownList ID="ddlDepartment" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Applicant type</label>
            <asp:TextBox ID="txtApplicantType" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Expire date</label>
            <asp:TextBox ID="txtExpireDate" runat="server"
                CssClass="form-control form-control-sm"
                TextMode="Date" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Status</label>
            <asp:TextBox ID="txtStatus" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

    </div>


    <!-- ========== ROW 4 ========== -->
    <div class="row mt-3">

        <div class="col-md-3">
            <label class="text-bold small">Job</label>
            <asp:DropDownList ID="ddlJob" runat="server"
                CssClass="form-control form-control-sm" />
            <%--   <asp:TextBox ID="TxtJob" runat="server"
       CssClass="form-control form-control-sm" />--%>
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Correspondence action</label>
            <asp:DropDownList ID="ddlCorrespondenceAction" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Rating</label>
            <asp:DropDownList ID="ddlRating" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Start date and time</label>
            <asp:TextBox ID="txtStartDateTime" runat="server"
                CssClass="form-control form-control-sm"
                TextMode="DateTimeLocal" />
        </div>

    </div>


    <!-- ========== ROW 5 (COSTS) ========== -->
    <div class="row mt-3">

        <div class="col-md-3">
            <label class="text-bold small">Travel cost</label>
            <asp:TextBox ID="txtTravelCost" runat="server"
                CssClass="form-control form-control-sm text-right"
                Text="0.00" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Lodging cost</label>
            <asp:TextBox ID="txtLodgingCost" runat="server"
                CssClass="form-control form-control-sm text-right"
                Text="0.00" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Other cost</label>
            <asp:TextBox ID="txtOtherCost" runat="server"
                CssClass="form-control form-control-sm text-right"
                Text="0.00" />
        </div>

        <div class="col-md-3">
            <label class="text-bold small">Contact</label>
            <asp:DropDownList ID="ddlContact" runat="server"
                CssClass="form-control form-control-sm" />
        </div>

    </div>

</div>

        </div>


        <!-- ===================== ATTACHMENTS PANEL ===================== -->
        <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center border-top"
             id="headingAttachments">

            <a class="text-dark d-flex justify-content-between w-100"
               data-toggle="collapse"
               href="#collapseAttachment"
               role="button"
               aria-expanded="false"
               aria-controls="collapseAttachment">

                <strong>Attachments</strong>

                <span class="ml-auto">
                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                </span>
            </a>
        </div>

        <div id="collapseAttachment"
             class="collapse"
             aria-labelledby="headingAttachments">

            <div class="card-body bg-white">

                <div class="row">

                    <div class="col-md-6">
                        <label class="text-bold small">Upload Attachment</label>
                        <asp:FileUpload ID="fuAttachment" runat="server"
                            CssClass="form-control form-control-sm" />
                    </div>

                   <%-- <div class="col-md-3 align-self-end">
                        <asp:Button ID="btnUpload" runat="server"
                            CssClass="btn btn-primary btn-sm"
                            Text="Upload" />
                    </div>--%>

                </div>

                <!-- Attachment grid can be added here -->

            </div>
        </div>

    </div>
  

      <div>
    <asp:LinkButton 
        ID="btnSave" 
        runat="server" 
        OnClick="btnSave_Click"
        CssClass="btn btn-primary">
        
        Create
    </asp:LinkButton>
        </div>

  




</asp:Content>
