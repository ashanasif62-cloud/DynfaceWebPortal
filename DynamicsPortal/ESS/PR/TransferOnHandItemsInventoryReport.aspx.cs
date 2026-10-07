using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.CurrencySvcReference;
using PortalIntegration.RetrieveReportsSvcReference;
using System;
using System.IO;

namespace DynamicsPortal.ESS.PR
{
    public partial class TransferOnHandItemsInventoryReport : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "TransferOrderOnHandInventoryReport";

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
                string ItemId = string.Empty;

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
                string viewVersionId = chkversion.Checked ? "Yes" : "No";
                string veiwOwnerName = chkOwnerName.Checked ? "Yes" : "No";
                string viewSerialId = chkserialid.Checked ? "Yes" : "No";
                string viewBatchid = chkbatchId.Checked ? "Yes" : "No";

                if (Session["ItemId"] != null)
                {
                    ItemId = Session["ItemId"].ToString();
                }



                // Call your service method (adjust the parameter order exactly as in X++)
                RetrieveReports retrieveReportsData = new RetrieveReports();

                MemoryStream memoryStream = retrieveReportsData.viewOnHandInventory(viewConfigId, viewWarehouseId, viewSizeId, viewStyleId, viewColorId, viewInventoryStatus, viewSerialId, viewLicensePlate, viewLocationId, viewBatchid, veiwOwnerName, viewSiteId, viewVersionId, ItemId);

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
        ////mod haris 


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
    }
}