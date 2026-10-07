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
            openEmployeeReferredform("/ESS/TA/ESSJmgTimecardTable_ListPage.aspx?EmpId=");
        }

        protected void btnProfileCalendar_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/TA/ESSJmgProfileCalendar_ListPage.aspx?EmpId=");
        }

        protected void btnSubDepartment_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/HR/ESSHRSubDepartment_ListPage.aspx?EmpId=");
        }

        protected void btnSalarySlip_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/Reports/ESSPRSalarySlip.aspx?EmpId=");
        }

        protected void btnEntitlementBalance_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/PR/ESSPREntitlementBalance.aspx?EmpId=");
        }

        protected void btnInOutSheet_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/Reports/ESSJmgInOutSheet.aspx?EmpId=");
        }

        protected void btnTimeSheet_Click(object sender, EventArgs e)
        {
            openEmployeeReferredform("/ESS/Reports/ESSJmgAttendanceRegister_TimeSheet.aspx?EmpId=");
        }

        private void openEmployeeReferredform(string _url)
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

    }
}