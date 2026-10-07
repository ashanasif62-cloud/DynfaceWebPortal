<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSHRApplicant_DetailForm.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSHRApplicant_DetailForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
       <style>
    .rotate-icon {
    transition: transform 0.2s ease;
}

.collapsed .rotate-icon {
    transform: rotate(-180deg);
}

.small-icon {
    font-size: 12px;
}
       </style>

<%--    <div class="action-items">
    <asp:LinkButton 
        ID="btnNew" 
        runat="server" 
        OnClick="btnNew_Click">
        <i class="mdi mdi-plus"></i> New
    </asp:LinkButton>
</div>--%>
           <div class="action-items">
    <asp:LinkButton ID="BtnSave" runat="server" OnClick="BtnSave_Click" >
        <i class="mdi mdi-content-save" style="margin-right: 4px;"></i>Save
    </asp:LinkButton>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

      <asp:HiddenField ID="hdnFirstName" runat="server"  Visible="false"/>

    <div class="card">

        <!-- ================= GENERAL ================= -->
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
    <div class="row">

        <!-- ========== COLUMN 1 ========== -->
        <div class="col-md-3">
            <label class="text-bold small">Applicant</label>
            <asp:TextBox 
                ID="txtApplicant" 
                runat="server" 
                CssClass="form-control border-1 border-bottom rounded-1 text-dark" 
                ReadOnly="true" 
                style="background-color: lightgray;" />

            <label class="text-bold small">Applicant type</label>
            <asp:TextBox 
                ID="txtApplicantType" 
                runat="server" 
                CssClass="form-control border-1 border-bottom rounded-1 text-dark" 
                ReadOnly="true" 
                style="background-color: lightgray;" />

            <label class="text-bold small">Personal title</label>
            <asp:TextBox ID="txtPersonalTitle" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">First name</label>
            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Middle name</label>
            <asp:TextBox ID="txtMiddleName" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />
        </div>

        <!-- ========== COLUMN 2 ========== -->
        <div class="col-md-3">
            <label class="text-dark small">Last name prefix</label>
            <asp:TextBox ID="txtLastNamePrefix" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Last name</label>
            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Personal suffix</label>
            <asp:TextBox ID="txtPersonalSuffix" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Search name</label>
            <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <div class="mt-3">
                <span class="text-dark small font-weight-bold">Name details</span>
            </div>

            <div class="mt-1">
                <label class="text-dark small mb-0">Initials</label>
                <asp:TextBox ID="txtInitials" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />
            </div>
        </div>

        <!-- ========== COLUMN 3 ========== -->
        <div class="col-md-3">
            <label class="text-dark small">Known as</label>
            <asp:TextBox ID="txtKnownAs" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Professional title</label>
            <asp:TextBox ID="txtProfessionalTitle" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Professional suffix</label>
            <asp:TextBox ID="txtProfessionalSuffix" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Phonetic first</label>
            <asp:TextBox ID="txtPhoneticFirst" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Phonetic middle</label>
            <asp:TextBox ID="txtPhoneticMiddle" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />
        </div>

        <!-- ========== COLUMN 4 ========== -->
        <div class="col-md-3">
            <label class="text-dark small">Phonetic last</label>
            <asp:TextBox ID="txtPhoneticLast" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Display as</label>
            <asp:TextBox ID="txtDisplayAs" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <div class="mt-4">
                <span class="text-dark small font-weight-bold">Applicant information</span>
            </div>

            <div class="mt-1">
                <label class="text-dark small mb-2">Current job title</label>
                <asp:TextBox ID="txtCurrentJobTitle" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />
            </div>

            <label class="text-bold small">Highest degree</label>
            <asp:TextBox ID="txtHighestDegree" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />

            <label class="text-bold small">Number of applications</label>
            <asp:TextBox ID="txtApplications" runat="server" CssClass="form-control border-1 border-bottom rounded-1 text-dark" />
        </div>

    </div>
</div>


        </div>

        <!-- ================= ADDRESSES ================= -->
        <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center"
             id="headingAddresses">

            <a class="text-dark d-flex justify-content-between w-100"
               data-toggle="collapse"
               href="#collapseAddresses"
               role="button"
               aria-expanded="false"
               aria-controls="collapseAddresses">

                <strong>Addresses</strong>

                <span class="ml-auto">
                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                </span>
            </a>
        </div>

        <div id="collapseAddresses"
             class="collapse"
             aria-labelledby="headingAddresses">

        <div class="card-body bg-white">

    <asp:GridView
        ID="gvAddresses"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-borderless table-hover"
        EmptyDataText="No addresses found"
        ShowHeaderWhenEmpty="true">

        <Columns>

           
            <asp:TemplateField HeaderText="Name or description">
                <ItemTemplate>
                    <asp:Label 
                        ID="lblAddressName"
                        runat="server"
                        CssClass="text-primary"
                        Text='<%# Bind("Name") %>' />
                </ItemTemplate>
            </asp:TemplateField>

          
            <asp:TemplateField HeaderText="Address">
                <ItemTemplate>
                    <asp:Label 
                        ID="lblAddress"
                        runat="server"
                        Text='<%# Bind("Address") %>' />
                </ItemTemplate>
            </asp:TemplateField>

         
            <asp:TemplateField HeaderText="Purpose">
                <ItemTemplate>
                    <asp:Label 
                        ID="lblPurpose"
                        runat="server"
                        Text='<%# Bind("Purpose") %>' />
                </ItemTemplate>
            </asp:TemplateField>

          
            <asp:TemplateField HeaderText="Primary">
                <ItemTemplate>
                    <asp:CheckBox
                        ID="chkPrimary"
                        runat="server"
                        Enabled="false"
                        Checked='<%# Bind("Primary") %>' />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>

</div>

        </div>

        <!-- ================= CONTACT INFORMATION ================= -->
        <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center"
             id="headingContact">

            <a class="text-dark d-flex justify-content-between w-100"
               data-toggle="collapse"
               href="#collapseContact"
               role="button"
               aria-expanded="false"
               aria-controls="collapseContact">

                <strong>Contact Information</strong>

                <span class="ml-auto">
                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                </span>
            </a>
        </div>

        <div id="collapseContact"
             class="collapse"
             aria-labelledby="headingContact">

        <div class="card-body bg-white">

    <asp:GridView
        ID="gvContacts"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-borderless table-hover"
        EmptyDataText="No contact information found"
         ShowHeaderWhenEmpty="true">

        <Columns>

          
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <asp:Label
                        ID="lblDescription"
                        runat="server"
                        CssClass="text-primary"
                        Text='<%# Bind("Description") %>' />
                </ItemTemplate>
            </asp:TemplateField>

          
            <asp:TemplateField HeaderText="Type">
                <ItemTemplate>
                    <asp:Label
                        ID="lblType"
                        runat="server"
                        Text='<%# Bind("Type") %>' />
                </ItemTemplate>
            </asp:TemplateField>

           
            <asp:TemplateField HeaderText="Contact number / address">
                <ItemTemplate>
                    <asp:Label
                        ID="lblContactValue"
                        runat="server"
                        Text='<%# Bind("ContactValue") %>' />
                </ItemTemplate>
            </asp:TemplateField>

          
            <asp:TemplateField HeaderText="Extension">
                <ItemTemplate>
                    <asp:Label
                        ID="lblExtension"
                        runat="server"
                        Text='<%# Bind("Extension") %>' />
                </ItemTemplate>
            </asp:TemplateField>

          
            <asp:TemplateField HeaderText="Primary">
                <ItemTemplate>
                    <asp:CheckBox
                        ID="chkPrimary"
                        runat="server"
                        Enabled="false"
                        Checked='<%# Bind("Primary") %>' />
                </ItemTemplate>
            </asp:TemplateField>

         
            <asp:TemplateField HeaderText="Private">
                <ItemTemplate>
                    <asp:CheckBox
                        ID="chkPrivate"
                        runat="server"
                        Enabled="false"
                        Checked='<%# Bind("Private") %>' />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>

</div>

        </div>

        <!-- ================= PERSONAL INFORMATION ================= -->
        <div class="card-header p-2 bg-light d-flex justify-content-between align-items-center"
             id="headingPersonal">

            <a class="text-dark d-flex justify-content-between w-100"
               data-toggle="collapse"
               href="#collapsePersonal"
               role="button"
               aria-expanded="false"
               aria-controls="collapsePersonal">

                <strong>Personal Information</strong>

                <span class="ml-auto">
                    <i class="fa fa-chevron-down rotate-icon small-icon"></i>
                </span>
            </a>
        </div>

        <div id="collapsePersonal"
             class="collapse"
             aria-labelledby="headingPersonal">

       <div class="card-body bg-white">
    <div class="row">

        <!-- ========== COLUMN 1 : PERSONAL DETAILS ========== -->
        <div class="col-md-3">
            <div>
                <span class="text-muted small font-weight-bold">Personal details</span>
            </div>

            <label class="text-muted small mt-2">Birth date</label>
            <asp:TextBox 
                ID="txtBirthDate" 
                runat="server" 
                TextMode="Date"
                CssClass="form-control border-1 border-bottom rounded-1" />

            <label class="text-muted small mt-3">Ethnic origin</label>
            <asp:TextBox 
                ID="txtEthnicOrigin" 
                runat="server" 
                CssClass="form-control border-1 border-bottom rounded-1" />
        </div>

        <!-- ========== COLUMN 2 : GENDER ========== -->
        <div class="col-md-3">
            <div>
                <span class="text-muted small font-weight-bold">Gender</span>
            </div>

            <label class="text-muted small mt-2">Gender</label>
            <asp:DropDownList 
                ID="ddlGender" 
                runat="server" 
                CssClass="form-control border-1 border-bottom rounded-1">
                <asp:ListItem Text="None" Value="" />
                <asp:ListItem Text="Male" Value="Male" />
                <asp:ListItem Text="Female" Value="Female" />
            </asp:DropDownList>

            <label class="text-muted small mt-3">Applicant's status</label>
            <asp:TextBox 
                ID="txtApplicantStatus" 
                runat="server" 
                CssClass="form-control border-1 border-bottom rounded-1" />
        </div>

        <!-- ========== COLUMN 3 : COUNTRY / REGION ========== -->
        <div class="col-md-3">
            <div>
                <span class="text-muted small font-weight-bold">Country/region</span>
            </div>

            <label class="text-muted small mt-2">Citizenship country/region</label>
            <asp:TextBox 
                ID="txtCitizenshipCountry" 
                runat="server" 
                CssClass="form-control border-1 border-bottom rounded-1" />

            <label class="text-muted small mt-3">Native language</label>
            <asp:TextBox 
                ID="txtNativeLanguage" 
                runat="server" 
                CssClass="form-control border-1 border-bottom rounded-1" />
        </div>

        <!-- ========== COLUMN 4 : MILITARY SERVICE ========== -->
        <div class="col-md-3">
            <div>
                <span class="text-muted small font-weight-bold">Military service</span>
            </div>

            <label class="text-muted small mt-2">Veteran status</label>
            <asp:TextBox 
                ID="txtVeteranStatus" 
                runat="server" 
                CssClass="form-control border-1 border-bottom rounded-1" />
        </div>

    </div>
</div>

        </div>

    </div>

</asp:Content>

