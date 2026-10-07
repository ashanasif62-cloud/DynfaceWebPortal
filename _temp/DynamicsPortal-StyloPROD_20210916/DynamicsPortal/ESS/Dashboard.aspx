<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="DynamicsPortal.Dashboard" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div class="container-fluid">
        <div class="section">
            <div class="menu-section">
                <div class="user-section">
                    <div class="card">
                        <div class="user-container">
                            <div class="d-flex justify-content-between">
                                <div class="d-flex justify-content-between">
                                    <a class="feed-profile">
                                        <img id="imgUser" runat="server" src="/distribution/img/User.png" herf="/ESS/HR/ESSHRPersonalDetails.aspx" alt="person" class="user-img"></a>
                                    <div class="content">
                                        <strong id="lblUserFullName" runat="server"></strong>
                                        <div class="user-lastlogin">
                                            <small id="lblUserDepartment" runat="server"></small>
                                            <small>|</small>
                                            <small id="lblUserJob" runat="server"></small>
                                        </div>
                                        <div class="user-lastlogin">
                                            <small>Login:</small>
                                            <small id="lblUserLoginTime" runat="server"></small>
                                        </div>
                                    </div>
                                </div>

                                <div class="user-address">
                                    <div>
                                        <small id="lblUserEmailId" runat="server"></small>
                                        <small>|</small>
                                        <small id="lblUserPhoneNo" runat="server"></small>
                                    </div>
                                    <div>
                                        <small id="lblUserFullAddress" runat="server"></small>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="user-section">
                    <div class="dash-menu-contents" style="margin-top: 15px;">
                        <!-- Pie Chart -->
                        <div class="card dash-4 charts-center-alignment">
                            <h6 class="chart-title">Payroll Totals</h6>
                            <div class="pie-chart">
                                <canvas id="chartPayrollTotals"></canvas>
                            </div>
                        </div>

                        <!-- Pie Chart -->
                        <div class="card dash-4 charts-center-alignment">
                            <h6 class="chart-title">Advances</h6>
                            <div class="pie-chart">
                                <canvas id="chartAdvances"></canvas>
                            </div>
                        </div>
                        <!-- bar Chart -->
                        <div class="card dash-4 charts-center-alignment">
                            <h6 class="chart-title">Leaves</h6>
                            <div class="line-chart">
                                <canvas id="chartLeaves"></canvas>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="dash-menu-contents" style="margin-top: 15px;">
                    <!-- bar Chart -->
                    <div class="card dash-4 charts-center-alignment">
                        <h6 class="chart-title">Employee Taxes</h6>
                        <div class="line-chart">
                            <canvas id="chartTax"></canvas>
                        </div>
                    </div>
                </div>

                <div class="dash-menu-contents">
                    <%--                    <div class="dash-4">
                        <div class="card">
                            <div class="user-container">
                                <div class="dash-title">Employee Self Service</div>
                                <!-- <a class="dashboard-container-a">Employee Details</a> -->
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Request for Leave">Request for Leave</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Request for Advance</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Request for Loan</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Request for Business Trip</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Request for Rejoining</a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="dash-4">
                        <div class="card">
                            <div class="user-container">
                                <div class="dash-title">History</div>
                                
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Request for Leave">Request for Leave</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Request for Advance</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Request for Loan</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Request for Business Tripn</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Request for Rejoining</a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="dash-4">
                        <div class="card">
                            <div class="user-container">
                                <div class="dash-title">Reports</div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Request for Leave">Salary Slip</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Tax Certificate</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Leave Balances</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Salary Certificate</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" title="Employment Information">Termination Letter</a>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>

            <div class="dash">
                <div>
                    <%--class="card"--%>
                    <div class="dash-12">
                        <div class="card">
                            <div class="user-container">
                                <div class="dash-title">Employee Self Service</div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/PR/ESSPREmployeeLeave_Create.aspx');" title="Request for Leave">Request for Leave</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/PR/ESSPREmployeeAdvance_Create.aspx');" title="Request for Advance">Request for Advance</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/PR/ESSPREmployeeLoan_Create.aspx');" title="Request for Loan">Request for Loan</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/PR/ESSPREmployeeEOS_Create.aspx');" title="Request for EOS">Request for EOS</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/HR/ESSHRRejoining_Create.aspx');" title="Request for Rejoining">Request for Rejoining</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/HR/ESSHRBusinessTrip_Create.aspx');" title="Request for Business Trip">Request for Business Trip</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/HR/ESSHRProfessionChange_Create.aspx');" title="Request for Profession Change">Request for Profession Change</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/HR/ESSHRHelpDeskRequest_Create.aspx');" title="Request for Help Desk">Request for Help Desk</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/HR/ESSHRRequestForLetter_Create.aspx');" title="Request for Letters">Request for Letters</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="javascript:;" onclick="javascript: return openPopupPanel('/ESS/HR/ESSHRHiringRequisition_Create.aspx', '980');" title="Request for Hiring Requisition">Request for Hiring Requisition</a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="dash-12">
                        <div class="card">
                            <div class="user-container">
                                <div class="dash-title">History</div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRPersonalDetails.aspx" title="Employee Personal Profile">Employee Personal Profile</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/PR/ESSPREmployeeLeave_ListPage.aspx" title="Employee Leave History">Employee Leave History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/PR/ESSPREmployeeAdvance_ListPage.aspx" title="Employee Advance History">Employee Advance History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/PR/ESSPREmployeeLoan_ListPage.aspx" title="Employee Loan History">Employee Loan History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/PR/ESSPREmployeeEOS_ListPage.aspx" title="Employee EOS History">Employee EOS History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSBusinessTrip_ListPage.aspx" title="Business Trip History">Business Trip History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRRejoining_ListPage.aspx" title="Rejoining History">Rejoining History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRProfessionChange_ListPage.aspx" title="Profession Change History">Profession Change History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRHelpDeskRequest_ListPage.aspx" title="Help Desk Request History">Help Desk Request History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRRequestForLetter_ListPage.aspx" title="Employee Letters History">Employee Letters History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRHiringRequisition_ListPage.aspx" title="Hiring Requisition History">Hiring Requisition History</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSAPEmployeeAppraisalsAssignedToMe_ListPage.aspx" title="Employee Appraisals Assigned To Me">Employee Appraisals Assigned To Me</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHcmOpenCourse_ListPage.aspx" title="Open Course History">Open Course History</a>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="dash-12">
                        <div class="card">
                            <div class="user-container">
                                <div class="dash-title">Reports</div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/Reports/ESSPRSalarySlip.aspx" title="Salary Slip">Salary Slip</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/Reports/ESSPRTaxCertificate.aspx" title="Tax Certificate">Tax Certificate</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/PR/ESSPREntitlementBalance.aspx" title="Leave Balances">Leave Balances</a>
                                </div>

                                <%--<div class="dashboard-link">
                                    <a class="dashboard-link-a" href="#/ESS/Reports/ESSPRSalaryCertificate.aspx" title="Salary Certificate">Salary Certificate</a>
                                </div>

                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="#/ESS/Reports/ESSPRTerminationLetter.aspx" title="Termination Letter">Termination Letter</a>
                                </div>--%>
                            </div>
                        </div>
                    </div>

                    <%--     <div class="user-container">
                        <div class="dash-title">Dashboard</div>

                        <!-- Pie Chart -->
                        <div class="col-lg-8 col-md-8 charts-center-alignment">
                            <h6 class="chart-title">Payroll Totals</h6>
                            <div class="pie-chart">
                                <canvas id="chartPayrollTotals" width="300" height="300"></canvas>
                            </div>
                        </div>

                        <!-- Pie Chart -->
                        <div class="col-lg-8 col-md-8 charts-center-alignment">
                            <h6 class="chart-title">Advances</h6>
                            <div class="pie-chart">
                                <canvas id="chartAdvances" width="300" height="300"></canvas>
                            </div>
                        </div>
                        <div class="separator-horizontal"></div>
                        <!-- bar Chart -->
                        <div class="col-lg-12 charts-center-alignment">
                            <h6 class="chart-title">Leaves</h6>
                            <div class="line-chart">
                                <canvas id="chartLeaves"></canvas>
                            </div>
                        </div>
                        <!-- bar Chart -->
                        <div class="col-lg-12 charts-center-alignment">
                            <h6 class="chart-title">Employee Taxes</h6>
                            <div class="line-chart">
                                <canvas id="chartTax"></canvas>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>




    <%--    <div style="display: none;" class="container-fluid">
        <div class="row d-flex">
            <div class="col-lg-8">
                <div class="card">
                    <div class="user-container">
                        <div class="d-flex justify-content-between">
                            <div class="d-flex justify-content-between">
                                <a class="feed-profile">
                                    <img src="distribution/img/avatar-2.jpg" alt="person" class="user-img"></a>
                                <div class="content">
                                    <strong>Muhammad Ammar</strong>
                                    <div class="user-lastlogin">
                                        <small>Last Login: 12/06/2014 11:30 PM</small>
                                    </div>
                                </div>
                            </div>
                            <div><small>2mins ago</small></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="card">
                    <h2 class="display h4">Dashboard</h2>
                </div>
            </div>

            <div class="col-lg-4">
                <div class="card">
                    <div class="user-container">
                        <div class="dash-title">Employee Self Service</div>

                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Request for Leave">Request for Leave</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Advance</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Loan</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Business Tripn</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Rejoining</a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="card">
                    <div class="user-container">
                        <div class="dash-title">Employee Self Service</div>

                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Request for Leave">Request for Leave</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Advance</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Loan</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Business Tripn</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Rejoining</a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="card">
                    <div class="user-container">
                        <div class="dash-title">Employee Self Service</div>

                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Request for Leave">Request for Leave</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Advance</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Loan</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Business Tripn</a>
                        </div>
                        <div class="dashboard-link">
                            <a class="dashboard-link-a" title="Employment Information">Request for Rejoining</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>
</asp:Content>

