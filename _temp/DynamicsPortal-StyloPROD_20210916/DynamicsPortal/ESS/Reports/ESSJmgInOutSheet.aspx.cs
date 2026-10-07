using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.IO;

namespace DynamicsPortal
{
    public partial class ESSJmgInOutSheet : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSJmgInOutSheet";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
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

        private string getQuery_EmployeeId()
        {
            string employeeId = string.Empty;
            if (string.IsNullOrEmpty(Request.QueryString["EmpId"]))
            {
                employeeId = SessionVariables.getCurrentEmployeeId();
            }
            else
            {
                employeeId = SecureQueryString.decrypt(Request.QueryString["EmpId"]);
            }
            return employeeId;
        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {

                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                string employeeId = getQuery_EmployeeId();

                if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    RetrieveReports retrieveReportsData = new RetrieveReports();
                    DateTime fromDateTime = fromDate.toDateTime();
                    DateTime toDateTime = toDate.toDateTime();

                    MemoryStream memoryStream = retrieveReportsData.viewAttendanceInOutSheet(employeeId, fromDateTime, toDateTime, false);
                    string inputAsString = Convert.ToBase64String(memoryStream.ToArray());

                    string src = "data:application/pdf;base64, " + inputAsString;
                    embed01.Src = src;
                }
                else
                {
                    NotificationMessage.showMessage(AlertType.Warning, "Please select From Date and To Date.");
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

    }
}