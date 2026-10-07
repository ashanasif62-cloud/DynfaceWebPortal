using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.IO;

namespace DynamicsPortal
{
    public partial class ESSPRSalarySlip : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPRSalarySlip";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindData();
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

        private void bindData()
        {
            string employeeId = getQuery_EmployeeId();

            PRPayGroupPayPeriod pRPayGroupPayPeriod = new PRPayGroupPayPeriod();
            DataTable dt = pRPayGroupPayPeriod.findByEmployee(employeeId);

            ddlPayPeriodCode.DataSource = dt;
            ddlPayPeriodCode.DataValueField = "PayPeriodCode";
            ddlPayPeriodCode.DataTextField = "PayPeriodCode";
            ddlPayPeriodCode.DataBind();
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
                string employeeId = getQuery_EmployeeId();
                string payPeriodCode = ddlPayPeriodCode.SelectedValue;
                //(cddlPayGroupPayPeriods.FindControl("txtPayPeriodCode") as System.Web.UI.WebControls.TextBox).Text;
                
                if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(payPeriodCode))
                {
                    RetrieveReports retrieveReportsData = new RetrieveReports();

                    string inputAsString = string.Empty;
                    MemoryStream memoryStream = retrieveReportsData.viewSalarySlipReport(employeeId, payPeriodCode);
                    if (memoryStream != null)
                        inputAsString = Convert.ToBase64String(memoryStream.ToArray());

                    string src = "data:application/pdf;base64, " + inputAsString;
                    embed01.Src = src;
                }
                else
                {
                    NotificationMessage.showMessage(AlertType.Warning, "Please select Pay Period Code.");
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