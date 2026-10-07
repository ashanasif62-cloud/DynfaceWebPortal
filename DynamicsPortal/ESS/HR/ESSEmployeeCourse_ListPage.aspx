<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSEmployeeCourse_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSEmployeeCourse_ListPage" %>


<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">

    <!-- TABS (moved above grids) -->
    <div class="tabs-container">
        <div class="tabs" role="tablist">
            <button type="button" class="tab active" data-target="assignedTab" aria-selected="true">Assigned</button>
            <button type="button" class="tab" data-target="openTab" aria-selected="false">Open</button>
            <button type="button" class="tab" data-target="completedTab" aria-selected="false">Completed</button>
        </div>
    </div>

    <!-- ASSIGNED -->
    <div id="assignedTab" class="tab-content active">
        <h3>Assigned Courses</h3>

        <asp:GridView ID="gvAssigned" runat="server"
            CssClass="table"
            AutoGenerateColumns="false"
            DataKeyNames="hrmCourseId">
            <Columns>
                <asp:BoundField DataField="hrmCourseId" HeaderText="Course ID" />
                <asp:BoundField DataField="courseName" HeaderText="Course Title" />
                <asp:BoundField DataField="dueDate" HeaderText="Due Date"
                    DataFormatString="{0:yyyy-MM-dd}" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- OPEN -->
    <div id="openTab" class="tab-content">
        <h3>Open Courses</h3>

        <asp:GridView ID="gvOpen" runat="server"
            CssClass="table"
            AutoGenerateColumns="false"
            DataKeyNames="hrmCourseId">
            <Columns>
                <asp:BoundField DataField="hrmCourseId" HeaderText="Course ID" />
                <asp:BoundField DataField="description" HeaderText="Course Title" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- COMPLETED -->
    <div id="completedTab" class="tab-content">
        <h3>Completed Courses</h3>

        <asp:GridView ID="gvCompleted" runat="server"
            CssClass="table"
            AutoGenerateColumns="false"
            DataKeyNames="hrmCourseId">
            <Columns>
                <asp:BoundField DataField="hrmCourseId" HeaderText="Course ID" />
                <asp:BoundField DataField="description" HeaderText="Course Title" />
                <asp:BoundField DataField="Location" HeaderText="Location" />
                <asp:BoundField DataField="startDate" HeaderText="Start Date"
                    DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="endDate" HeaderText="End Date"
                    DataFormatString="{0:yyyy-MM-dd}" />
            </Columns>
        </asp:GridView>
    </div>

    <script>
        (function () {
            function activateTab(targetId) {
                document.querySelectorAll('.tab-content').forEach(t => t.classList.remove('active'));
                document.getElementById(targetId).classList.add('active');

                document.querySelectorAll('.tabs .tab').forEach(b => {
                    var selected = b.getAttribute('data-target') === targetId;
                    b.classList.toggle('active', selected);
                    b.setAttribute('aria-selected', selected ? 'true' : 'false');
                });
            }

            document.addEventListener('DOMContentLoaded', function () {
                document.querySelectorAll('.tabs .tab').forEach(function (btn) {
                    btn.addEventListener('click', function () {
                        activateTab(this.getAttribute('data-target'));
                    });
                });
            });
        })();
    </script>

    <style>
        .tabs-container { margin-bottom: 12px; }
        .tabs { display:flex; gap:12px; border-bottom: 1px solid transparent; }
        .tabs .tab {
            background: transparent;
            border: none;
            padding: 8px 12px;
            font-size: 14px;
            cursor: pointer;
            color: #333;
            position: relative;
            outline: none;
        }
        .tabs .tab:hover { color: #0b5ed7; }
        /* active tab shows blue underline */
        .tabs .tab.active::after {
            content: '';
            position: absolute;
            left: 0;
            right: 0;
            bottom: -1px;
            height: 3px;
            background: #0b5ed7; /* bootstrap primary blue */
        }

        .tab-content { display:none; margin-top:10px; }
        .tab-content.active { display:block; }
    </style>

</asp:Content>