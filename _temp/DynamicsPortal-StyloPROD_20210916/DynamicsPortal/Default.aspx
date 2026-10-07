<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Default.aspx.cs" Inherits="DynamicsPortal.Default" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .edit-user-info {
            color: #1360b7;
            font-size: 13px;
        }

            .edit-user-info::before {
                display: inline-block;
                content: '\f3eb';
                font-family: Material Design Icons;
                font-size: 14px;
                font-weight: 100;
            }
    </style>

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
                                        <img id="imgUser" runat="server" src="/distribution/img/User.png" alt="person" class="user-img"></a>
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
                                        <div class="user-lastlogin">
                                            <a class="edit-user-info" href="/ESS/HR/ESSHRPersonalDetails.aspx">Edit personal details</a>
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

                    <!-- Updates Section -->
                    <section>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-lg-4 col-md-12">
                                    <!-- Recent Updates Widget -->
                                    <div id="new-updates" class="card updates recent-updated">
                                        <div id="updates-header" class="card-header d-flex justify-content-between align-items-center">
                                            <h2 class="h6 display"><a data-toggle="collapse" data-parent="#new-updates" href="#updates-box" aria-expanded="true" aria-controls="updates-box">News Updates</a></h2>
                                            <a data-toggle="collapse" data-parent="#new-updates" href="#updates-box" aria-expanded="true" aria-controls="updates-box"><i class="fa fa-angle-down"></i></a>
                                        </div>
                                        <div id="updates-box" role="tabpanel" class="collapse show">
                                            <ul id="divNewsUpdates" runat="server" class="news list-unstyled">
                                            </ul>
                                        </div>
                                    </div>
                                    <!-- Recent Updates Widget End-->
                                </div>
                                <div class="col-lg-4 col-md-6">
                                    <!-- Recent Activities Widget      -->
                                    <div id="recent-activities-wrapper" class="card updates activities">
                                        <div id="activites-header" class="card-header d-flex justify-content-between align-items-center">
                                            <h2 class="h6 display"><a data-toggle="collapse" data-parent="#recent-activities-wrapper" href="#activities-box" aria-expanded="true" aria-controls="activities-box">Recent Activities</a></h2>
                                            <a data-toggle="collapse" data-parent="#recent-activities-wrapper" href="#activities-box" aria-expanded="true" aria-controls="activities-box"><i class="fa fa-angle-down"></i></a>
                                        </div>
                                        <div id="activities-box" role="tabpanel" class="collapse show">
                                            <ul id="divRecentActivities" runat="server" class="activities list-unstyled">
                                            </ul>
                                        </div>
                                    </div>
                                    <!-- Recent Activities Widget End  -->
                                </div>
                                <div class="col-lg-4 col-md-6">
                                    <!-- My Requests Status Widget -->
                                    <div id="requests-status-wrapper" class="card updates activities">
                                        <div id="requests-status-header" class="card-header d-flex justify-content-between align-items-center">
                                            <h2 class="h6 display">
                                                <a data-toggle="collapse" data-parent="#requests-status-wrapper" href="#requests-status-box" aria-expanded="true" aria-controls="requests-status-box">My Requests Status</a>
                                            </h2>
                                            <a data-toggle="collapse" data-parent="#requests-status-wrapper" href="#requests-status-box" aria-expanded="true" aria-controls="requests-status-box"><i class="fa fa-angle-down"></i></a>
                                        </div>
                                        <div id="requests-status-box" role="tabpanel" class="collapse show">

                                            <div class="box-footer no-padding">
                                                <ul class="activities list-unstyled" id="missingAttendance" runat="server">
<%--                                                    <li>
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-12" style="padding-bottom: 5px;">
                                                                    <div class="" style="display: inline-flex;">
                                                                        <span style="display: inline-flex; line-height: 14px; font-size: 12px;">
                                                                            <i style="" class="fa fa-calendar"></i>
                                                                            &nbsp;24/07/2021
                                                                        </span>
                                                                    </div>
                                                                    <div class="" style="display: inline-flex; line-height: 14px; font-size: 12px; padding-left: 20%;">
                                                                        <i class="fa fa-clock-o" style=""></i><span style="line-height: 14px; padding-left: 4px; font-size: 12px;">22:45:26</span>
                                                                        <span style="line-height: 14px; padding: 0px 5px; font-size: 12px;">-</span>                                                         <i style="" class="fa fa-clock-o"></i><span style="line-height: 14px; padding-left: 4px; font-size: 12px;">00:00:00</span><br>
                                                                    </div>
                                                                    <span>
                                                                        <i style="" class="fa fa-flag"></i>
                                                                        &nbsp;Present, Time-Out is missing</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>--%>
                                                </ul>
                                            </div>

                                            <ul class="activities list-unstyled">
                                                <!-- Item-->
                                                <%--
                                                    <li>
                                                    <div class="row">
                                                        <div class="col-2 date-holder text-right">
                                                            <div style="color: forestgreen; padding: 1px 5px; font-size: 18px;"><i class="fa fa-check"></i></div>
                                                        </div>
                                                        <div class="col-10 content">
                                                            <strong>Help Desk Request</strong>
                                                            <p>
                                                                Request Id: HDSK-000002<br />
                                                                Request Date: 26 Jan 2019<br />
                                                                Status: Approved
                                                            </p>
                                                        </div>
                                                    </div>
                                                </li>
                                                <!-- Item-->
                                                <li>
                                                    <div class="row">
                                                        <div class="col-2 date-holder text-right">
                                                            <div style="color: red; padding: 1px 5px; font-size: 18px;"><i class="fa fa-ban"></i></div>
                                                        </div>
                                                        <div class="col-10 content">
                                                            <strong>Leave Request</strong>
                                                            <p>
                                                                Request Id: LEV-000022<br />
                                                                Request Date: 24 Jan 2019<br />
                                                                Status: Rejected
                                                            </p>
                                                        </div>
                                                    </div>
                                                </li>
                                                <!-- Item-->
                                                <li>
                                                    <div class="row">
                                                        <div class="col-2 date-holder text-right">
                                                            <div style="color: forestgreen; padding: 1px 5px; font-size: 18px;"><i class="fa fa-check"></i></div>
                                                        </div>
                                                        <div class="col-10 content">
                                                            <strong>Bussiness Trip Request</strong>
                                                            <p>
                                                                Request Id: BTR-000002<br />
                                                                Request Date: 18 Jan 2019<br />
                                                                Status: Approved
                                                            </p>
                                                        </div>
                                                    </div>
                                                </li>
                                                <!-- Item-->
                                                <li>
                                                    <div class="row">
                                                        <div class="col-2 date-holder text-right">
                                                            <div style="color: skyblue; padding: 1px 5px; font-size: 18px;"><i class="fa fa-clock-o"></i></div>
                                                        </div>
                                                        <div class="col-10 content">
                                                            <strong>Help Desk Request</strong>
                                                            <p>
                                                                Request Id: HDSK-000001<br />
                                                                Request Date: 15 Jan 2019<br />
                                                                Status: Pending Approval
                                                            </p>
                                                        </div>
                                                    </div>
                                                </li>
                                                <!-- Item-->
                                                <li>
                                                    <div class="row">
                                                        <div class="col-2 date-holder text-right">
                                                            <div style="color: forestgreen; padding: 1px 5px; font-size: 18px;"><i class="fa fa-check"></i></div>
                                                        </div>
                                                        <div class="col-10 content">
                                                            <strong>Advance Request</strong>
                                                            <p>
                                                                Request Id: ADV-000005<br />
                                                                Request Date: 04 Jan 2019<br />
                                                                Status: Approved
                                                            </p>
                                                        </div>
                                                    </div>
                                                </li> --%>
                                            </ul>
                                        </div>
                                    </div>
                                    <!-- My Requests Status End-->
                                </div>
                            </div>
                        </div>
                    </section>

                </div>
            </div>

            <div class="dash">
                <div>
                    <div class="dash-12">
                        <div id="caleandar">
                        </div>
                    </div>

                    <div class="dash-12">
                        <div class="card">
                            <div class="user-container">
                                <div class="dash-title">Employee Links</div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRPersonalDetails.aspx" title="Benefits">Personal Profile</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/Default.aspx" title="Benefits">Employee Self Service</a>
                                </div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRPolicies_ListPage.aspx" title="Policies & Procedures">Policies & Procedures</a>
                                </div>
                                <%--<div class="dashboard-link">
                                    <a class="dashboard-link-a" href="#" title="Benefits">Benefits</a>
                                </div>--%>
                            </div>
                        </div>
                    </div>

                    <div class="dash-12">
                    </div>

                </div>
            </div>

        </div>
    </div>

</asp:Content>


