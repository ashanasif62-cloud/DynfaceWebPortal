<%@ Page Title="" Language="C#" MasterPageFile="~/Modal.Master" AutoEventWireup="true" CodeBehind="ESSHRApplicant_Create.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSHRApplicant_Create" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .textbox-style {
            color: black !important;
            background-color: #f2f2f2 !important;
            width: 150px !important;   /* readonly fields */
        }

        .textbox-fixed {
            width: 150px !important;   /* editable fields */
        }

        .text-dark-label {
            color: black;
            font-weight: 600;
        }
         .form-container {
            max-width: 1000px;  /* wider form */
            margin: auto;
            padding: 20px;
        }

        .section-title {
            font-weight: bold;
            font-size: 16px;
            margin-bottom: 10px;
            display: block;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">

    <div class="card-body bg-white form-container"">
        <span class="section-title">General Information</span>

        <div class="d-flex flex-wrap justify-content-between">

            <!-- ================= COLUMN 1 ================= -->
            <div style="flex: 0 0 48%;">
                <div class="form-group mb-3">
                    <label class="text-dark-label">Applicant</label>
                    <asp:TextBox ID="txtApplicant" runat="server"
                        CssClass="form-control textbox-style" ReadOnly="true" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Applicant type</label>
                    <asp:TextBox ID="txtApplicantType" runat="server"
                        CssClass="form-control textbox-style" ReadOnly="true" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Personal title</label>
                    <asp:TextBox ID="txtPersonalTitle" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">First name</label>
                    <asp:TextBox ID="txtFirstName" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Middle name</label>
                    <asp:TextBox ID="txtMiddleName" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Last name prefix</label>
                    <asp:TextBox ID="txtLastNamePrefix" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Last name</label>
                    <asp:TextBox ID="txtLastName" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Personal suffix</label>
                    <asp:TextBox ID="txtPersonalSuffix" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>
            </div>

          
            <div style="flex: 0 0 48%;">
                <div class="form-group mb-3">
                    <label class="text-dark-label">Search name</label>
                    <asp:TextBox ID="txtSearchName" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Initials</label>
                    <asp:TextBox ID="txtInitials" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Known as</label>
                    <asp:TextBox ID="txtKnownAs" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Professional title</label>
                    <asp:TextBox ID="txtProfessionalTitle" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Professional suffix</label>
                    <asp:TextBox ID="txtProfessionalSuffix" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Phonetic first</label>
                    <asp:TextBox ID="txtPhoneticFirst" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Phonetic middle</label>
                    <asp:TextBox ID="txtPhoneticMiddle" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Phonetic last</label>
                    <asp:TextBox ID="txtPhoneticLast" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Display as</label>
                    <asp:TextBox ID="txtDisplayAs" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>
            </div>

            <div style="flex: 0 0 48%;">
                <span class="section-title">Applicant Information</span>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Current job title</label>
                    <asp:TextBox ID="txtCurrentJobTitle" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Highest degree</label>
                    <asp:TextBox ID="txtHighestDegree" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Number of applications</label>
                    <asp:TextBox ID="txtApplications" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Total applications</label>
                    <asp:TextBox ID="txtTotalApplications" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>
            </div>

            <!-- ================= COLUMN 4 ================= -->
            <div style="flex: 0 0 48%;">
                <span class="section-title">Future Consideration</span>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Previous employee</label>
                    <asp:TextBox ID="txtPreviousEmployee" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Skill mapping</label>
                    <asp:TextBox ID="txtSkillMapping" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Include in skill mapping</label>
                    <asp:TextBox ID="txtIncludeInSkillMapping" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <span class="section-title">Other Information</span>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Reason code</label>
                    <asp:TextBox ID="txtReasonCode" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Other information</label>
                    <asp:TextBox ID="txtOtherInformation" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>

                <div class="form-group mb-3">
                    <label class="text-dark-label">Address books</label>
                    <asp:TextBox ID="txtAddressBooks" runat="server"
                        CssClass="form-control textbox-fixed" />
                </div>
            </div>

        </div>
        </div>
  <div class="action-footer">
    <asp:LinkButton 
        ID="btnSave" 
        runat="server" 
        OnClick="btnSave_Click">
        Create
    </asp:LinkButton>

<%--    <asp:LinkButton 
        ID="btnCancel" 
        runat="server" 
        OnClick="btnCancel_Click">
        Cancel
    </asp:LinkButton>--%>
        <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="javascript: return closeDialog();">Cancel</asp:LinkButton>
</div>



</asp:Content>