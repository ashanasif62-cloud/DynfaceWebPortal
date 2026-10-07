<%@ Page Title="Employee Self Service Workspace" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSWorkSpace.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSWorkSpace" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- MDI Icons -->
    <link href="https://cdn.materialdesignicons.com/7.2.96/css/materialdesignicons.min.css" rel="stylesheet" />
    <!-- Bootstrap 4.1.3 CSS -->


    <style>
        .workspace-summary {
            display: flex;
            gap: 15px;
            margin-top: 20px;
            flex-wrap: wrap;
        }

        .workspace-tile {
            background-color: #1976d2;
            color: white;
            width: 160px;
            height: 120px;
            border-radius: 10px;
            padding: 10px;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            cursor: pointer;
            transition: all 0.3s ease;
            text-align: center;
        }

        .workspace-tile:hover {
            background-color: #135ba1;
            transform: translateY(-3px);
        }

        .workspace-icon {
            font-size: 28px;
            margin-bottom: 8px;
        }

        .workspace-value {
            font-size: 22px;
            font-weight: bold;
        }

        .workspace-label {
            font-size: 13px;
        }

        .workspace-icon.small {
            font-size: 20px;
            margin-bottom: 4px;
        }

        /* Nav tabs underline style */
        .nav-tabs {
            border-bottom: 1px solid #dee2e6;
        }

        .nav-tabs .nav-link {
            border: none;
            border-bottom: 2px solid transparent;
            color: #333;
            font-weight: 500;
            padding: 10px 20px;
            background: none;
            transition: all 0.3s ease;
        }

        .nav-tabs .nav-link.active {
            border: none;
            border-bottom: 3px solid #1976d2; /* Blue underline for active tab */
            color: #1976d2;
            font-weight: bold;
        }

        .nav-tabs .nav-link:hover {
            border: none;
            border-bottom: 3px solid #135ba1; /* Darker blue hover underline */
            color: #135ba1;
        }

        .user-img {
            width: 50px;
            height: 50px;
            border-radius: 50%;
        }

        .user-section .card {
            border: none;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        .user-container {
            padding: 15px;
        }

        /* Custom styles for collapsible panel */
        .d365-toggle-header {
            color: #333;
            text-decoration: none;
            font-size: 1.1rem;
            font-weight: 500;
            margin-top: 15px;
        }

        .d365-toggle-header:hover {
            color: #1976d2;
            text-decoration: none;
        }

        .rotate-icon {
            transition: transform 0.3s ease;
        }

        a[aria-expanded="true"] .rotate-icon {
            transform: rotate(180deg);
        }

        .card-header {
            background-color: #f8f9fa;
            border-bottom: 1px solid #dee2e6;
            padding: 10px 15px;
        }

        .card-header h5 {
            margin: 0;
            font-size: 1.1rem;
        }
    </style>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div class="container-fluid">
        <div class="section">
            <div class="menu-section-workspace">
                <!-- User Profile Card -->
                <div class="user-section">
                    <div class="card">
                        <div class="user-container">
                            <div class="d-flex justify-content-between">
                                <div class="d-flex">
                                    <a class="feed-profile">
                                        <img id="imgUser" runat="server" src="/distribution/img/User.png" alt="person" class="user-img" />
                                    </a>
                                    <div class="content ml-3">
                                        <strong id="lblUserFullName" runat="server">Unknown</strong>
                                        <div class="user-lastlogin">
                                            <small id="lblUserDepartment" runat="server">N/A</small>
                                            <small>|</small>
                                            <small id="lblUserJob" runat="server">N/A</small>
                                        </div>
                                        <div class="user-lastlogin">
                                            <small>Login:</small>
                                            <small id="lblUserLoginTime" runat="server"></small>
                                        </div>
                                        <div class="user-lastlogin">
                                            <a class="edit-user-info" href="/ESS/HR/ESSHRPersonalDetails.aspx">Edit personal details</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="user-address">
                                    <div>
                                        <small id="lblYearsOfService" runat="server">N/A</small>
                                        <small>|</small>
                                        <small id="lblReportsTo" runat="server">N/A</small>
                                        <small id="lblPositionType" runat="server">N/A</small>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Tabs Section -->
                <div class="card mt-3">
                    <div class="card-body">
                        <!-- Tab headers -->
                        <ul class="nav nav-tabs" id="workspaceTabs" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="myinfo-tab" data-toggle="tab" href="#myinfo" role="tab" aria-controls="myinfo" aria-selected="true">
                                    My Information
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="myteam-tab" data-toggle="tab" href="#myteam" role="tab" aria-controls="myteam" aria-selected="false">
                                    My Team
                                </a>
                            </li>
                        </ul>

                        <!-- Tab contents -->
                        <div class="tab-content mt-3" id="workspaceTabsContent">
                            <!-- My Information Tab -->
                            <div class="tab-pane fade show active" id="myinfo" role="tabpanel" aria-labelledby="myinfo-tab">
                                <!-- Collapsible Summary Panel -->
                                <a href="#summaryPanelMyInfo" class="d365-toggle-header d-flex justify-content-between align-items-center mt-0" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="summaryPanelMyInfo">
                                    <span class="section-title">Summary</span>
                                    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                </a>
                                <div class="collapse show mt-0" id="summaryPanelMyInfo">
                                    <div class="card shadow-sm">
                                        <div class="card-body">
                                            <div class="workspace-summary">
                                                <div class="workspace-tile">
                                                    <div class="workspace-value">
                                                        <asp:Literal ID="lblWorkItems" runat="server">0</asp:Literal>
                                                    </div>
                                                    <div class="workspace-label">Work items assigned to me</div>
                                                </div>
                                                <div class="workspace-tile">
                                                    <div class="workspace-value">
                                                        <asp:Literal ID="lblQuestionnaires" runat="server">0</asp:Literal>
                                                    </div>
                                                    <div class="workspace-label">Questionnaires assigned to me</div>
                                                </div>
                                                <div class="workspace-tile">
                                                    <i class="mdi mdi-format-list-bulleted workspace-icon small"></i>
                                                    <div class="workspace-label">Company directory</div>
                                                </div>
                                                <div class="workspace-tile">
                                                    <i class="mdi mdi-briefcase-search workspace-icon"></i>
                                                    <div class="workspace-label">Open jobs</div>
                                                </div>
                                                <div class="workspace-tile">
                                                    <i class="mdi mdi-calendar-multiple-check workspace-icon"></i>
                                                    <div class="workspace-label">Team absence calendar</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- End Collapsible Summary Panel -->

                                <!-- Collapsible My Career Information Panel -->
                                <a href="#careerPanelMyInfo" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="careerPanelMyInfo">
                                    <span class="section-title">My Career Information</span>
                                    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                </a>
                                <div class="collapse show mt-0" id="careerPanelMyInfo">
                                    <div class="card shadow-sm">
                                        <div class="card-body">
                                            <div class="workspace-summary">
                                                <div class="workspace-tile">
                                                    <div class="workspace-value">
                                                        <asp:Literal ID="Literal1" runat="server">0</asp:Literal>
                                                    </div>
                                                    <div class="workspace-label">Years of Service</div>
                                                </div>
                                                <div class="workspace-tile">
                                                    <div class="workspace-value">
                                                        <asp:Literal ID="Literal2" runat="server">N/A</asp:Literal>
                                                    </div>
                                                    <div class="workspace-label">Position Type</div>
                                                </div>
                                                <div class="workspace-tile">
                                                    <div class="workspace-value">
                                                        <asp:Literal ID="Literal3" runat="server">N/A</asp:Literal>
                                                    </div>
                                                    <div class="workspace-label">Reports To</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- End Collapsible My Career Information Panel -->

                                <!-- Collapsible Additional Information Panel -->
                                <a href="#additionalInfoPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="additionalInfoPanel">
                                    <span class="section-title">Additional Information</span>
                                    <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                </a>
                                <div class="collapse show mt-0" id="additionalInfoPanel">
                                    <div class="card shadow-sm">
                                        <div class="card-body">
                                            <!-- Performance Appraisal -->
                                            <a href="#performanceAppraisalPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-0" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="performanceAppraisalPanel">
                                                <span class="section-title">Performance Appraisal</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="performanceAppraisalPanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblPerformanceScore" runat="server">0</asp:Literal></div>
                                                                <div class="workspace-label">Performance Score</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Employee Self Services -->
                                            <a href="#employeeSelfServicesPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="employeeSelfServicesPanel">
                                                <span class="section-title">Employee Self Services</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="employeeSelfServicesPanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblSelfServiceRequests" runat="server">0</asp:Literal></div>
                                                                <div class="workspace-label">Requests</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Employee Self Service List -->
                                            <a href="#employeeSelfServiceListPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="employeeSelfServiceListPanel">
                                                <span class="section-title">Employee Self Service List</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="employeeSelfServiceListPanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblServiceListItems" runat="server">0</asp:Literal></div>
                                                                <div class="workspace-label">Items</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Clearance Group -->
                                            <a href="#clearanceGroupPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="clearanceGroupPanel">
                                                <span class="section-title">Clearance Group</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="clearanceGroupPanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblClearanceStatus" runat="server">N/A</asp:Literal></div>
                                                                <div class="workspace-label">Status</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Employee Times -->
                                            <a href="#employeeTimesPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="employeeTimesPanel">
                                                <span class="section-title">Employee Times</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="employeeTimesPanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblTotalHours" runat="server">0</asp:Literal></div>
                                                                <div class="workspace-label">Total Hours</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Benefits -->
                                            <a href="#benefitsPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="benefitsPanel">
                                                <span class="section-title">Benefits</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="benefitsPanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblBenefitClaims" runat="server">0</asp:Literal></div>
                                                                <div class="workspace-label">Claims</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Performance -->
                                            <a href="#performancePanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="performancePanel">
                                                <span class="section-title">Performance</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="performancePanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblPerformanceRating" runat="server">0</asp:Literal></div>
                                                                <div class="workspace-label">Rating</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Competencies -->
                                            <a href="#competenciesPanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="competenciesPanel">
                                                <span class="section-title">Competencies</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="competenciesPanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblCompetencyLevel" runat="server">N/A</asp:Literal></div>
                                                                <div class="workspace-label">Level</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Questionnaire -->
                                            <a href="#questionnairePanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="questionnairePanel">
                                                <span class="section-title">Questionnaire</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="questionnairePanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblQuestionnaireCount" runat="server">0</asp:Literal></div>
                                                                <div class="workspace-label">Count</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Maison Time Attendance -->
                                            <a href="#maisonTimeAttendancePanel" class="d365-toggle-header d-flex justify-content-between align-items-center mt-3" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="maisonTimeAttendancePanel">
                                                <span class="section-title">Maison Time Attendance</span>
                                                <i class="mdi mdi-chevron-down rotate-icon ml-2"></i>
                                            </a>
                                            <div class="collapse show mt-0" id="maisonTimeAttendancePanel">
                                                <div class="card shadow-sm">
                                                    <div class="card-body">
                                                        <div class="workspace-summary">
                                                            <div class="workspace-tile">
                                                                <div class="workspace-value"><asp:Literal ID="lblAttendanceDays" runat="server">0</asp:Literal></div>
                                                                <div class="workspace-label">Attendance Days</div>
                                                            </div>
                                                            <!-- Add more tiles as needed -->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- End Collapsible Additional Information Panel -->
                            </div>

                            <!-- My Team Tab -->
                            <div class="tab-pane fade" id="myteam" role="tabpanel" aria-labelledby="myteam-tab">
                                <h5 class="mt-4">My Team</h5>
                                <asp:Repeater ID="rptTeamMembers" runat="server">
                                    <ItemTemplate>
                                        <div class="card mb-2">
                                            <div class="card-body">
                                                <h6><%# Eval("Name") %></h6>
                                                <p>Job: <%# Eval("Job") %></p>
                                                <p>Department: <%# Eval("DepartmentName") %></p>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Literal ID="lblTeamError" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap 4.1.3 JS (jQuery + Popper + Bootstrap) -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>

</asp:Content>