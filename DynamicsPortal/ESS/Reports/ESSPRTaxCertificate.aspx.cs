using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.IO;

namespace DynamicsPortal
{
    public partial class ESSPRTaxCertificate : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPRTaxCertificate";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindPayPeriodCode();
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

        //protected void btnViewReport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string fromDate = txtFromDate.Text;
        //        string toDate = txtToDate.Text;
        //        string employeeId = SessionVariables.getCurrentEmployeeId();

        //        if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
        //        {
        //            RetrieveReports retrieveReportsData = new RetrieveReports();
        //            DateTime fromDateTime = fromDate.toDateTime();
        //            DateTime toDateTime = toDate.toDateTime();

        //            MemoryStream memoryStream = retrieveReportsData.viewTaxCertificateReport(employeeId, fromDateTime, toDateTime);
        //            string inputAsString = Convert.ToBase64String(memoryStream.ToArray());

        //            string src = "data:application/pdf;base64, " + inputAsString;
        //            embed01.Src = src;
        //        }
        //        else
        //        {
        //            NotificationMessage.showMessage(AlertType.Warning, "Please select From Date and To Date.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //    }
        //    finally
        //    { }
        //}
        //mod haris 
        //protected void btnEmailReport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string fromDate = txtFromDate.Text;
        //        string toDate = txtToDate.Text;
        //        string employeeId = SessionVariables.getCurrentEmployeeId();

        //        if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
        //        {
        //            RetrieveReports retrieveReportsData = new RetrieveReports();
        //            DateTime fromDateTime = Convert.ToDateTime(fromDate);
        //            DateTime toDateTime = Convert.ToDateTime(toDate);

        //            // Get the response message from the emailTaxCertificateReport method
        //            string responseMessage = retrieveReportsData.emailTaxCertificateReport(employeeId, fromDateTime, toDateTime);

        //            // Show the message based on the response from the API call
        //            NotificationMessage.showMessage(AlertType.Success, responseMessage);
        //        }
        //        else
        //        {
        //            NotificationMessage.showMessage(AlertType.Warning, "Please select From Date and To Date.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //    }
        //    finally { }
        //}

        protected void btnEmailReport_Click(object sender, EventArgs e)
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string selectedYear = ddlPayPeriodYear.SelectedValue;

                if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(selectedYear))
                {
                    int payPeriodYear = Convert.ToInt32(selectedYear);

                    RetrieveReports retrieveReportsData = new RetrieveReports();

                    // Call API using Pay Period Year
                    string responseMessage = retrieveReportsData.emailTaxCertificateReport(
                                                employeeId,
                                                payPeriodYear
                                            );

                    NotificationMessage.showMessage(AlertType.Information, responseMessage);
                }
                else
                {
                    NotificationMessage.showMessage(AlertType.Warning, "Please select Pay Period Year.");
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
        }


        private void bindPayPeriodCode()
        {

            string employeeId = SessionVariables.getCurrentEmployeeId();

            PRPayGroupPayPeriod pRPayGroupPayPeriod = new PRPayGroupPayPeriod();
            DataTable dt = pRPayGroupPayPeriod.findByEmployee(employeeId);

            ddlPayPeriodYear.DataSource = dt;
            ddlPayPeriodYear.DataValueField = "PayPeriodYear";
            ddlPayPeriodYear.DataTextField = "PayPeriodYear";
            ddlPayPeriodYear.DataBind();
        }

    }
}