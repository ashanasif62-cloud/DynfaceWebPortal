using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class HREmployeesReportToMe_ListPage : MainForm
    {
        private HREmployeesListReportToMe hREmployeesListReportToMe = new HREmployeesListReportToMe();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hREmployeesListReportToMe.tableName;
                pageMenuId = "HREmployeesReportToMe";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    reBindGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }
        }
        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = hREmployeesListReportToMe.retrieveAllHREmployeesListReportToMe(employeeId);
            SessionVariables.setSessionDataTable(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();
        }


        private void openEmployeeTransferRequest(string _employeeId, string _requestType)
        {
            string employeeId = SecureQueryString.encrypt(_employeeId);
            string requestType = SecureQueryString.encrypt(_requestType);

            if (string.IsNullOrEmpty(employeeId) || string.IsNullOrEmpty(requestType))
            {
                NotificationMessage.showInvalidRecord();
                return;
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Employee Transfer Requests",
                "javascript: openPopupPanel('/ESS/HR/HcmEmployeeTransferRequests_Create.aspx?EmpId=" + employeeId + "&ReqType=" + requestType + "' ,'980');", true);

        }

        protected void btnProbationExtension_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = getSelectedGridViewRow(gridView);
            if (gridViewRow == null)
            {
                NotificationMessage.showMessage(AlertType.Error, "Please select a record.");
                return;
            }
            string employeeId = ((Label)gridViewRow.FindControl("lblEmployeId")).Text;
            string requestType = "Probation";
            openEmployeeTransferRequest(employeeId, requestType);
        }

        protected void btnTransferRequest_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = getSelectedGridViewRow(gridView);
            if (gridViewRow == null)
            {
                NotificationMessage.showMessage(AlertType.Error, "Please select a record.");
                return;
            }
            string employeeId = ((Label)gridViewRow.FindControl("lblEmployeId")).Text;
            string requestType = "Transfer";
            openEmployeeTransferRequest(employeeId, requestType);
        }

        protected void btnSalaryProvision_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = getSelectedGridViewRow(gridView);
            if (gridViewRow == null)
            {
                NotificationMessage.showMessage(AlertType.Error, "Please select a record.");
                return;
            }
            string employeeId = ((Label)gridViewRow.FindControl("lblEmployeId")).Text;
            string requestType = "SalaryProvision";
            openEmployeeTransferRequest(employeeId, requestType);
        }

        protected void btnPromotionRequest_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = getSelectedGridViewRow(gridView);
            if (gridViewRow == null)
            {
                NotificationMessage.showMessage(AlertType.Error, "Please select a record.");
                return;
            }
            string employeeId = ((Label)gridViewRow.FindControl("lblEmployeId")).Text;
            string requestType = "Promotion";
            openEmployeeTransferRequest(employeeId, requestType);
        }

        protected void btnDemotionRequest_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = getSelectedGridViewRow(gridView);
            if (gridViewRow == null)
            {
                NotificationMessage.showMessage(AlertType.Error, "Please select a record.");
                return;
            }
            string employeeId = ((Label)gridViewRow.FindControl("lblEmployeId")).Text;
            string requestType = "Demotion";
            openEmployeeTransferRequest(employeeId, requestType);
        }

        protected void btnConfirmationRequest_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = getSelectedGridViewRow(gridView);
            if (gridViewRow == null)
            {
                NotificationMessage.showMessage(AlertType.Error, "Please select a record.");
                return;
            }
            string employeeId = ((Label)gridViewRow.FindControl("lblEmployeId")).Text;
            string requestType = "Confirmation";
            openEmployeeTransferRequest(employeeId, requestType);
        }

        protected void btnRegisterCourses_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/HR/ESSHcmOpenCourse_ListPage.aspx?EmpId=");
        }

        protected void btnTimeAndAttendance_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/TA/ESSJmgTimecardTable_ListPage.aspx?EmpId=", "ESSJmgTimecardTableHistory_MyTeam");
        }

        protected void btnProfileCalendar_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/TA/ESSJmgProfileCalendar_ListPage.aspx?EmpId=", "ESSJmgProfileCalendarHistory_MyTeam");
        }

        protected void btnSubDepartment_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/HR/ESSHRSubDepartment_ListPage.aspx?EmpId=");
        }

        protected void btnSalarySlip_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/Reports/ESSPRSalarySlip.aspx?EmpId=", "ESSPRSalarySlip_MyTeam");
        }

        protected void btnEntitlementBalance_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/PR/ESSPREntitlementBalance.aspx?EmpId=", "ESSPRLeaveBalance_MyTeam");
        }

        protected void btnInOutSheet_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/Reports/ESSJmgInOutSheet.aspx?EmpId=", "ESSJmgInOutSheet_MyTeam");
        }

        protected void btnTimeSheet_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/Reports/ESSJmgAttendanceRegister_TimeSheet.aspx?EmpId=", "ESSJmgAttendanceRegister_TimeSheet_MyTeam");
        }



        protected void btnRoster_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = getSelectedGridViewRow(gridView);
            if (gridViewRow == null)
            {
                NotificationMessage.showMessage(AlertType.Error, "Please select a record.");
                return;
            }

            string employeeId = ((Label)gridViewRow.FindControl("lblEmployeId")).Text;

            if (string.IsNullOrEmpty(employeeId))
            {
                NotificationMessage.showInvalidRecord();
                return;
            }

            // Encrypt EmployeeId
            string encryptedEmpId = SecureQueryString.encrypt(employeeId);

            // Navigate to Roster List Page
            string url = "/ESS/TA/TASEmployeeRosterTeam_Listpage.aspx?EmpId=" + encryptedEmpId;
            Server.Transfer(url);
        }

        protected void btnAttendanceRegister_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gridViewRow = getSelectedGridViewRow(gridView);

                if (gridViewRow == null)
                {
                    NotificationMessage.showMessage(AlertType.Error, "Please select a record.");
                    return;
                }

                // Safer way to get the Employee Id
                Label lblEmpId = gridViewRow.FindControl("lblEmployeId") as Label;

                if (lblEmpId == null || string.IsNullOrWhiteSpace(lblEmpId.Text))
                {
                    NotificationMessage.showMessage(AlertType.Error, "Employee ID not found in the selected row.");
                    return;
                }

                string employeeId = lblEmpId.Text.Trim();

                // Encrypt EmployeeId
                string encryptedEmpId = SecureQueryString.encrypt(employeeId);

                if (string.IsNullOrEmpty(encryptedEmpId))
                {
                    NotificationMessage.showInvalidRecord();
                    return;
                }

                // Navigate to Attendance Register List Page
                string url = "/ESS/TA/TASAttendanceRegisterTeam_ListPage.aspx?EmpId=" + encryptedEmpId;
                Server.Transfer(url);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write($"{this.GetType().FullName}.{nameof(btnAttendanceRegister_Click)}", ex);
                NotificationMessage.showMessage(AlertType.Error, "An error occurred: " + ex.Message);
            }
        }

        private void openEmployeeReferredform(string _url, string _menuItemId = "")
        {
            if (!string.IsNullOrEmpty(_menuItemId))
            {
                AuthenticationHelper.AuthenticationHelper authenticationHelper = new AuthenticationHelper.AuthenticationHelper();
                isPageAuthorizated = authenticationHelper.pageAuthentication(_menuItemId);
                if (isPageAuthorizated)
                {
                    GridViewRow gridViewRow = getSelectedGridViewRow(gridView);
                    if (gridViewRow == null)
                    {
                        NotificationMessage.showMessage(AlertType.Error, "Please select a record.");
                        return;
                    }
                    string employeeId = ((Label)gridViewRow.FindControl("lblEmployeId")).Text;

                    employeeId = SecureQueryString.encrypt(employeeId);

                    if (string.IsNullOrEmpty(employeeId))
                    {
                        NotificationMessage.showInvalidRecord();
                        return;
                    }
                    Server.Transfer(_url + employeeId);

                }
                else
                {
                    NotificationMessage.showMessage(AlertType.Error, "Access Denied.");
                }

            }

        }

    }
}