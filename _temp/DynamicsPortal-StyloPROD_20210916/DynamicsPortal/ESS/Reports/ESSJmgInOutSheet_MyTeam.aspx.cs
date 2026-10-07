using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.IO;

namespace DynamicsPortal
{
    public partial class ESSJmgInOutSheet_MyTeam : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSJmgInOutSheet_MyTeam";

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
        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {

                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                string employeeId = SessionVariables.getCurrentEmployeeId();

                if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    RetrieveReports retrieveReportsData = new RetrieveReports();
                    DateTime fromDateTime = fromDate.toDateTime();
                    DateTime toDateTime = toDate.toDateTime();

                    MemoryStream memoryStream = retrieveReportsData.viewAttendanceInOutSheet(employeeId, fromDateTime, toDateTime, true);
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