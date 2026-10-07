<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="HREmployeesReportToMe_ListPage.aspx.cs" Inherits="DynamicsPortal.HREmployeesReportToMe_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        main {
            max-width: 900px;
            margin: 0 auto;
            text-align: center;
        }

        .menu {
            margin: 4rem auto;
            max-width: 300px;
            text-transform: uppercase;
            font-weight: 700;
            padding: 5px;
        }

        .panel,
        .dropdown .row {
            padding: 12px 15px;
            background-color: #f8f8f8;
            color: #888888;
            border-radius: 3px;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
            display: flex;
            justify-content: space-between;
            align-items: center;
            text-decoration: none;
        }

        .text {
            font-size: 16px;
        }

        .icon {
            font-size: 20px;
        }

        .panel :hover {
            color: #6c6c6c;
            cursor: pointer;
        }

        input#toggle {
            display: none;
        }

            input#toggle ~ .dropdown {
                display: block;
            }

            input#toggle:checked ~ .dropdown {
                display: none;
            }

        .dropdown {
            margin-top: 7px;
        }

            .dropdown .arrow {
                width: 0;
                height: 0;
                border-left: 7px solid transparent;
                border-right: 7px solid transparent;
                border-bottom: 9px solid #f8f8f8;
                margin-left: 20px;
            }

            .dropdown .row {
                border-radius: 0;
                box-shadow: none;
                box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
                text-align: left;
            }

                .dropdown .row:nth-child(2) {
                    border-radius: 3px 3px 0 0;
                }

                .dropdown .row:last-child {
                    border-radius: 0 0 3px 3px;
                }

            .dropdown:hover > .row {
                color: #bebebe;
            }

            .dropdown .row:hover {
                background-color: #ebebeb;
                color: #6c6c6c;
            }
    </style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <%--    <div class="action-items">
        <asp:LinkButton ID="btnTransferRequest" runat="server" Enabled="false" OnClick="btnTransferRequest_Click"><i class="mdi mdi-transit-transfer"></i>Transfer</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnSalaryProvision" runat="server" Enabled="false" OnClick="btnSalaryProvision_Click"><i class="mdi mdi-account-convert"></i>Salary Provision</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnPromotionRequest" runat="server" Enabled="false" OnClick="btnPromotionRequest_Click"><i class="mdi mdi-new-box"></i>Promotion</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnDemotionRequest" runat="server" Enabled="false" OnClick="btnDemotionRequest_Click"><i class="mdi mdi-priority-low"></i>Demotion</asp:LinkButton>
       </div>--%>
    <%--    mdi-tab-plus        mdi-arrow-expand-up      mdi-arrange-bring-forward--%>
    <%--    <div class="action-items">
        <asp:LinkButton ID="btnConfirmationRequest" runat="server" OnClick="btnConfirmationRequest_Click"><i class="mdi mdi-progress-check"></i>Confirmation</asp:LinkButton>
    </div>--%>
<%--    <div class="action-items">
        <asp:LinkButton ID="btnTimeAndAttendance" runat="server" OnClick="btnTimeAndAttendance_Click"><i class="mdi mdi-plus"></i>Time Registration</asp:LinkButton>
    </div>--%>
<%--    <div class="action-items">
        <asp:LinkButton ID="btnProfileCalendar" runat="server" OnClick="btnProfileCalendar_Click"><i class="mdi mdi-plus"></i>Profile Calendar</asp:LinkButton>
    </div>--%>
<%--<div class="action-items">
        <asp:LinkButton ID="btnRegisterCourses" runat="server" OnClick="btnRegisterCourses_Click"><i class="mdi mdi-plus"></i>Register Courses</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnSubDepartment" runat="server" OnClick="btnSubDepartment_Click"><i class="mdi mdi-open-in-new"></i>Sub Department</asp:LinkButton>
    </div>--%>
    <%--<div class="action-items">
        <asp:LinkButton ID="btnSalarySlip" runat="server" OnClick="btnSalarySlip_Click"><i class="mdi mdi-open-in-new"></i>Salary Slip</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnEntitlementBalance" runat="server" OnClick="btnEntitlementBalance_Click"><i class="mdi mdi-open-in-new"></i>Leave Balance</asp:LinkButton>
    </div>--%>
<%--
    <div class="action-items">
        <asp:LinkButton ID="btnInOutSheet" runat="server" OnClick="btnInOutSheet_Click"><i class="mdi mdi-open-in-new"></i>In-Out Sheet</asp:LinkButton>
    </div>
    --%>

<%--    <div class="action-items">
        <asp:LinkButton ID="btnTimeSheet" runat="server" OnClick="btnTimeSheet_Click"><i class="mdi mdi-open-in-new"></i>Time Sheet</asp:LinkButton>
    </div>--%>

    <div class="action-items" style="display:none !important">
        <asp:LinkButton ID="btnRoster" runat="server" OnClick="btnRoster_Click">
            <i class="mdi mdi-calendar-clock"></i> Roster
        </asp:LinkButton>
    </div>

    <div class="action-items">
        <asp:LinkButton ID="btnAttendanceRegister" runat="server" OnClick="btnAttendanceRegister_Click">
            <i class="mdi mdi-clipboard-text-clock"></i> Attendance Register
        </asp:LinkButton>
    </div>
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gridView" runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found." DataKeyNames="EmployeId"
            AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort">
                    <%--<HeaderTemplate>
                        <input type="checkbox" id="chk_SelectAll" />
                    </HeaderTemplate>--%>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_SelectSingle" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Id">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeId" runat="server" Text='<%# Bind("EmployeId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email">
                    <ItemTemplate>
                        <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department Name">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartmentName" runat="server" Text='<%# Bind("DepartmentName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Position">
                    <ItemTemplate>
                        <asp:Label ID="lblPositionId" runat="server" Text='<%# Bind("PositionDescription") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Grade">
      <ItemTemplate>
          <asp:Label ID="lblGrade" runat="server" Text='<%# Bind("EmployeeGrade") %>'></asp:Label>
      </ItemTemplate>
  </asp:TemplateField>

                <asp:TemplateField HeaderText="Performance">
    <ItemTemplate>
        <asp:Label ID="lblPerforamce" runat="server" Text='<%# Bind("EmployeeEfficency") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>

            </Columns>

        </asp:GridView>
    </div>
</asp:Content>
