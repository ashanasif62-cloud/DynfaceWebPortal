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

        .news-ticker-wrapper {
            overflow: hidden;
            position: relative;
            height: 420px; /* Ensure this is less than your total content height */
        }

            .news-ticker-wrapper ul {
                display: flex;
                flex-direction: column;
                animation: ticker-scroll 20s linear infinite; /* Increased duration for readability */
                will-change: transform; /* Performance optimization */
            }

            /* Optional: Pause on hover */
            .news-ticker-wrapper:hover ul {
                animation-play-state: paused;
            }

            .news-ticker-wrapper li {
                margin-bottom: 10px;
                flex-shrink: 0; /* Prevent flex items from shrinking */
            }

        /* Keyframes for top-to-bottom scroll inside the container */
        @keyframes ticker-scroll {
            0% {
                transform: translateY(0); /* Start at normal position */
            }

            100% {
                transform: translateY(-50%); /* Move up by exactly half the total height (original content height) */
            }
        }

        #activities-box {
            max-height: 420px;
            overflow-y: auto;
            overflow-x: hidden;
            padding-right: 5px; /* avoids text touching scrollbar */
            scrollbar-width: thin; /* Firefox */
        }

            #activities-box::-webkit-scrollbar {
                width: 6px;
            }

            #activities-box::-webkit-scrollbar-thumb {
                background: #ccc;
                border-radius: 3px;
            }

        #requests-status-box {
            max-height: 420px;
            overflow-y: auto;
            overflow-x: hidden;
            padding-right: 5px; /* avoids text touching scrollbar */
            scrollbar-width: thin; /* Firefox */
        }

            #requests-status-box::-webkit-scrollbar {
                width: 6px;
            }

            #requests-status-box::-webkit-scrollbar-thumb {
                background: #ccc;
                border-radius: 3px;
            }

        #caleandar {
            position: relative;
            min-height: 80px;
        }

        #calendar-loader {
            display: none;
            position: absolute;
            inset: 0;
            background: rgba(255, 255, 255, 0.75);
            z-index: 10;
            align-items: center;
            justify-content: center;
            border-radius: 6px;
        }

            #calendar-loader.active {
                display: flex;
            }

        .cal-spinner {
            width: 36px;
            height: 36px;
            border: 3px solid #e0e0e0;
            border-top-color: #1360b7;
            border-radius: 50%;
            animation: cal-spin 0.75s linear infinite;
        }

        @keyframes cal-spin {
            to {
                transform: rotate(360deg);
            }
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
                                        <img id="imgUser" runat="server" src="/distribution/img/User.png" style="width: 80px; height: 80px;" alt="person" class="user-img"></a>
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
                                            <a class="edit-user-info" href="/ESS/HR/ESSHRPersonalDetails.aspx"
                                                style="font-weight: 600; font-size: 15px; color: #0056d2; text-decoration: underline;"
                                                onclick="showAJAXOverlay();">Edit Personal Details
                                            </a>
                                        </div>
                                    </div>
                                </div>

                                <div class="user-address">

                                    <div style="margin-bottom: 10px;">
                                        <small id="lblUserEmailId" runat="server"></small>

                                        <small id="emailPhoneSeparator" runat="server">|</small>

                                        <small id="lblUserPhoneNo" runat="server"></small>
                                    </div>

                                    <div id="addressRow" runat="server" style="margin-bottom: 10px;">
                                        <small id="lblUserFullAddress" runat="server"></small>
                                    </div>

                                    <div id="employeeInfoRow" runat="server" style="display: flex; gap: 15px; align-items: flex-start;">

                                        <div id="yearsSection" runat="server" style="text-align: left;">
                                            <div id="txtYearsValue" runat="server" style="font-weight: 500; font-size: 16px;"></div>
                                            <small style="font-weight: 300; font-size: 14px; margin-top: 4px; display: block; color: #777;">Years of Service</small>
                                        </div>

                                        <span id="yearsSeparator" runat="server" style="font-weight: 100; color: #bbb; font-size: 35px; line-height: 1;">|</span>

                                        <div id="reportsSection" runat="server" style="text-align: left;">
                                            <div id="txtReportsValue" runat="server" style="font-weight: 500; font-size: 16px;"></div>
                                            <small style="font-weight: 300; font-size: 14px; margin-top: 4px; display: block; color: #777;">Reports To</small>
                                        </div>

                                        <span id="efficiencySeparator" runat="server" style="font-weight: 100; color: #bbb; font-size: 35px; line-height: 1;">|</span>

                                        <div id="efficiencySection" runat="server" style="text-align: left;">
                                            <div id="txtEfficiencyValue" runat="server" style="font-weight: 500; font-size: 16px;"></div>
                                            <small style="font-weight: 300; font-size: 14px; margin-top: 4px; display: block; color: #777;">Performance</small>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Updates Section -->
                    <section>
                        <div class="container-fluid">
                            <div class="row mt-3">
                                <div class="col-lg-4 col-md-6">
                                    <div id="new-updates" class="card updates recent-updated">
                                        <div id="updates-header" class="card-header d-flex justify-content-between align-items-center">
                                            <h2 class="h6 display">
                                                <a data-toggle="collapse" data-parent="#new-updates" href="#updates-box" aria-expanded="true" aria-controls="updates-box">News Updates
                                                </a>
                                            </h2>
                                            <a data-toggle="collapse" data-parent="#new-updates" href="#updates-box" aria-expanded="true" aria-controls="updates-box">
                                                <i class="fa fa-angle-down"></i>
                                            </a>
                                        </div>

                                        <div id="updates-box" role="tabpanel" class="collapse show">
                                            <div class="news-ticker-wrapper">
                                                <ul id="divNewsUpdates" runat="server" class="news list-unstyled">
                                                    <!-- News items added dynamically via C# -->
                                                </ul>
                                            </div>
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
                                                <a data-toggle="collapse" data-parent="#requests-status-wrapper" href="#requests-status-box" aria-expanded="true" aria-controls="requests-status-box">My Requests Status
                                                </a>
                                            </h2>
                                            <a data-toggle="collapse" data-parent="#requests-status-wrapper" href="#requests-status-box" aria-expanded="true" aria-controls="requests-status-box">
                                                <i class="fa fa-angle-down"></i>
                                            </a>
                                        </div>

                                        <div id="requests-status-box" role="tabpanel" class="collapse show">
                                            <ul class="activities list-unstyled" id="missingAttendance" runat="server">
                                                <!-- Dynamic items added via C# -->
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
                    <!-- Calendar section in your HTML -->
                    <div class="dash-12">
                        <div style="position: relative; min-height: 80px;">
                            <!-- Loader lives OUTSIDE #caleandar so innerHTML="" doesn't kill it -->
                            <div id="calendar-loader" style="display: none; position: absolute; inset: 0; background: rgba(255,255,255,0.75); z-index: 10; align-items: center; justify-content: center; border-radius: 6px;">
                                <div class="cal-spinner"></div>
                            </div>
                            <div id="caleandar"></div>
                        </div>
                    </div>

                    <div class="dash-12">
                        <div class="card">
                            <div class="user-container">
                                <div class="dash-title">Employee Links</div>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRPersonalDetails.aspx" title="Benefits">Personal Profile</a>
                                </div>
                                <%--  <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/Default.aspx" title="Benefits">Employee Self Service</a>
                                </div>--%>
                                <div class="dashboard-link">
                                    <a class="dashboard-link-a" href="/ESS/HR/ESSHRPolicies_ListPage.aspx" title="Policies & Procedures">Policies & Procedures</a>
                                </div>
                                <div class="dashboard-link d-none">
                                    <a class="dashboard-link-a" href="/DownloadApk.aspx" title="Download APK">Download App</a>
                                </div>
                                <div class="dashboard-link d-none">
                                    <a class="dashboard-link-a" href="/ESS/LinkDevice.aspx" title="Link Device">Link Device</a>
                                </div>
                                <%--<div class="dashboard-link">
                                    <a class="dashboard-link-a" href="#" title="Benefits">Benefits</a>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                    <div class="dash-12"></div>
                </div>
            </div>
        </div>
    </div>
    <script>
        (function waitForjQuery() {
            if (window.jQuery && typeof caleandar === 'function') {
                $(document).ready(function () {

                    var loader = document.getElementById('calendar-loader');
                    var isFirstLoad = true;  // ← only show spinner on month navigation, not initial load

                    function showLoader() {
                        if (isFirstLoad) return;  // don't show on first load
                        loader.style.display = 'flex';
                    }

                    function hideLoader() {
                        loader.style.display = 'none';
                    }

                    function loadEvents(year, month) {
                        var fromDate = new Date(year, month, 1);
                        var toDate = new Date(year, month + 1, 0);

                        showLoader();

                        $.ajax({
                            type: "POST",
                            url: "/default.aspx/GetRegisterDetails",
                            data: JSON.stringify({
                                fromDate: fromDate.toISOString().split('T')[0],
                                toDate: toDate.toISOString().split('T')[0]
                            }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                var rawEvents = JSON.parse(response.d);
                                var events = rawEvents.map(function (evt) {
                                    var d = new Date(evt.Date);
                                    return {
                                        Date: new Date(d.getFullYear(), d.getMonth(), d.getDate()),
                                        Title: evt.Title,
                                        Link: evt.Link || "#",
                                        BgColor: evt.BgColor || null,
                                        Color: evt.Color || null,
                                        CssClass: evt.CssClass || null
                                    };
                                });

                                var element = document.getElementById('caleandar');
                                element.innerHTML = "";   // safe — loader is outside this div now

                                caleandar(element, events, {
                                    NavShow: true,
                                    DateTimeShow: true,
                                    onMonthChange: function (y, m) {
                                        loadEvents(y, m);
                                    }
                                }, new Date(year, month, 1));
                            },
                            error: function (xhr, status, error) {
                                console.error("Calendar load error:", error);
                            },
                            complete: function () {
                                isFirstLoad = false;   // after first load, spinner shows on navigation
                                hideLoader();
                            }
                        });
                    }

                    var today = new Date();
                    loadEvents(today.getFullYear(), today.getMonth());
                });
            } else {
                setTimeout(waitForjQuery, 50);
            }
        })();
    </script>
    <script>
        $(document).ready(function () {
            var $ul = $('#divNewsUpdates');
            var $items = $ul.children();
            var wrapperHeight = $('.news-ticker-wrapper').height();

            if ($items.length > 0) {
                // Duplicate until content is taller than wrapper
                while ($ul.height() < wrapperHeight * 2) {
                    $ul.append($items.clone());
                }

                // Adjust animation duration
                var totalHeight = $ul.height();
                var duration = Math.max(10, (totalHeight / 100) * 1.5);
                $ul.css('animation-duration', duration + 's');
            }
        });
    </script>
</asp:Content>
