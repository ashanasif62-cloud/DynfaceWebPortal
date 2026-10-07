using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.CurrencySvcReference;
using PortalIntegration.RetrieveReportsSvcReference;
using System;
using System.IO;
using System.Net.NetworkInformation;

namespace DynamicsPortal.ESS.PR
{
    public partial class TransferOrderOverViewReport : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "TransferOrderOverViewReport";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;
                if (!isPageAuthorizated)
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

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                // First three: bool to int (NoYesId)
                Boolean showLines = chkTransferLines.Checked;
                Boolean showReservation = chkReservations.Checked;
                Boolean showTaxInformation = false;
                string transferId = string.Empty;

                if (Session["TransferId"] != null)
                {
                    transferId = Session["TransferId"].ToString();
                }



                // Assign NoYes enum based on whether each checkbox is checked
                // Assign string values based on whether each checkbox is checked
                string viewConfigId = chkConfiguration.Checked ? "Yes" : "No";
                string viewSizeId = chkSize.Checked ? "Yes" : "No";
                string viewColorId = chkColor.Checked ? "Yes" : "No";
                string viewStyleId = chkStyle.Checked ? "Yes" : "No";
                string viewSiteId = chkSite.Checked ? "Yes" : "No";
                string viewWarehouseId = chkWarehouse.Checked ? "Yes" : "No";
                string viewLocationId = chkLocation.Checked ? "Yes" : "No";
                string viewInventoryStatus = chkInventoryStatus.Checked ? "Yes" : "No";
                string viewLicensePlate = chkLicensePlate.Checked ? "Yes" : "No";

                // Call your service method (adjust the parameter order exactly as in X++)
                RetrieveReports retrieveReportsData = new RetrieveReports();

                MemoryStream memoryStream = retrieveReportsData.viewTransferOrderOverview(showLines, showReservation, showTaxInformation, viewConfigId, viewLocationId, viewSizeId, viewStyleId, viewColorId, viewInventoryStatus, viewLicensePlate, viewWarehouseId, transferId);

                string inputAsString = Convert.ToBase64String(memoryStream.ToArray());
                string src = "data:application/pdf;base64," + inputAsString;

                embed01.Src = src;
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
        //mod haris 
        protected void btnEmailReport_Click(object sender, EventArgs e)
        {
            try
            {
                //string fromDate = txtFromDate.Text;
                //string toDate = txtToDate.Text;
                //string employeeId = SessionVariables.getCurrentEmployeeId();

                //if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                //{
                //    RetrieveReports retrieveReportsData = new RetrieveReports();
                //    DateTime fromDateTime = Convert.ToDateTime(fromDate);
                //    DateTime toDateTime = Convert.ToDateTime(toDate);

                //    // Get the response message from the emailTaxCertificateReport method
                //    string responseMessage = retrieveReportsData.emailTaxCertificateReport(employeeId, fromDateTime, toDateTime);

                //    // Show the message based on the response from the API call
                //    NotificationMessage.showMessage(AlertType.Information, responseMessage);
                //}
                //else
                //{
                //    NotificationMessage.showMessage(AlertType.Warning, "Please select From Date and To Date.");
                //}
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally { }
        }

        public enum NoYes
        {
            No = 0,
            Yes = 1
        }
    }
}