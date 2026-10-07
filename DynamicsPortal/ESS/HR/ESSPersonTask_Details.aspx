<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSPersonTask_Details.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSPersonTask_Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">
      <div class="action-items">
      <a href="/ESS/HR/ESSPersonTask_ListPage.aspx" class="btn-link">
          <i class="mdi mdi-arrow-left" style="margin-right: 4px;"></i>Back
      </a>
  </div>
       <div class="action-items">
       <asp:LinkButton ID="BtnSaveDate" runat="server" OnClick="BtnSave_Date_Click">
           <i class="mdi mdi-content-save" style="margin-right: 4px;"></i>Save
       </asp:LinkButton>
   </div>
    <style>
        .d365-toggle-header {
            cursor: pointer;
            font-weight: 600;
            border-bottom: 1px solid #ddd;
            padding-bottom: 5px;
        }

        .section-title {
            font-size: 16px;
        }

        .section-content {
            padding: 15px 10px;
        }

        .rotate-icon {
            transition: transform 0.3s ease;
        }

        /* Fixed Width */
        .fixed-width {
            width: 250px !important;
        }

        /* Toggle Switch */
        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 24px;
        }

        .switch input {
            display: none;
        }

        .slider {
            position: absolute;
            cursor: pointer;
            background-color: #ccc;
            transition: .4s;
            top: 0; left: 0; right: 0; bottom: 0;
            border-radius: 24px;
        }

        .slider:before {
            position: absolute;
            content: "";
            height: 18px;
            width: 18px;
            left: 3px;
            bottom: 3px;
            background-color: white;
            transition: .4s;
            border-radius: 50%;
        }

        input:checked + .slider {
            background-color: #0d6efd;
        }

        input:checked + .slider:before {
            transform: translateX(26px);
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

<div class="container-fluid">

    <!-- ================= GENERAL ================= -->
    <a href="#collapseGeneral"
        class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
        data-toggle="collapse"
        aria-expanded="true">
        <span class="section-title">General</span>
        <i class="mdi mdi-chevron-down rotate-icon"></i>
    </a>

    <div id="collapseGeneral" class="collapse show">
        <div class="section-content">
            <div class="row">

                <!-- Column 1 -->
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Description</label>
                        <asp:TextBox ID="txtDescription" runat="server"
                            CssClass="form-control fixed-width" ReadOnly="true" />
                    </div>

                    <div class="form-group">
                        <label>Process Type</label>
                        <asp:TextBox ID="txtProcessType" runat="server"
                            CssClass="form-control fixed-width" ReadOnly="true" />
                    </div>

                    <div class="form-group">
                        <label>Regarding</label>
                        <asp:TextBox ID="txtRegarding" runat="server"
                            CssClass="form-control fixed-width" ReadOnly="true" />
                    </div>
                </div>

                <!-- Column 2 -->
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Contact Person</label>
                        <asp:TextBox ID="txtContactPerson" runat="server"
                            CssClass="form-control fixed-width" ReadOnly="true" />
                    </div>

                    <div class="form-group">
                        <label>Due Date</label>
                        <asp:TextBox ID="txtDueDate" runat="server"
                            CssClass="form-control fixed-width"
                            TextMode="Date"
                            onkeydown="return false;" />
                    </div>

                    <div class="form-group">
                        <label>Status</label>
                        <asp:TextBox ID="txtStatus" runat="server"
                            CssClass="form-control fixed-width" ReadOnly="true" />
                    </div>
                </div>

                <!-- Column 3 -->
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Task Link</label><br />
                       <asp:TextBox ID="TxtTaskLink" runat="server"
     CssClass="form-control fixed-width" ReadOnly="true" />
                    </div>

                    <div class="form-group">
                        <label>Optional</label><br />
                        <label class="switch">
                            <asp:CheckBox ID="chkOptional" runat="server" Enabled="false" />
                            <span class="slider"></span>
                        </label>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <!-- ================= INSTRUCTION ================= -->
    <a href="#collapseInstruction"
        class="d365-toggle-header d-flex justify-content-between align-items-center mt-4"
        data-toggle="collapse"
        aria-expanded="false">
        <span class="section-title">Instruction</span>
        <i class="mdi mdi-chevron-down rotate-icon"></i>
    </a>

    <div id="collapseInstruction" class="collapse">
        <div class="section-content">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label>Instruction</label>
                        <asp:TextBox ID="txtInstruction" runat="server"
                            CssClass="form-control fixed-width"
                            TextMode="MultiLine"
                            Rows="5"
                            ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>

<!-- ICON ROTATION -->
<script>
    document.addEventListener("DOMContentLoaded", function () {
        var toggles = document.querySelectorAll(".d365-toggle-header");

        toggles.forEach(function (toggle) {
            toggle.addEventListener("click", function () {
                var icon = this.querySelector(".rotate-icon");
                if (icon) {
                    icon.classList.toggle("rotate");
                }
            });
        });
    });
</script>

</asp:Content>