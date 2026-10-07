using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.RetrieveReportsSvcReference;
using System;
using System.IO;

namespace DynamicsPortal.ESS.PR
{
    public partial class TransferOrderReceiveReport : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "TransferOrderReceiveReport";

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
                string transferId = string.Empty;
                string voucherId = string.Empty;
                DateTime postingDate = DateTime.MinValue;

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
                string viewOwnerName = chkOwnerName.Checked ? "Yes" : "No";
                string viewSerialId = chkserialid.Checked ? "Yes" : "No";
                string viewBatchid = chkbatchId.Checked ? "Yes" : "No";

                if (Session["ReceiveTransferId"] != null)
                {
                    transferId = Session["ReceiveTransferId"].ToString();
                }

                if (Session["ReceiveVoucherId"] != null)
                {
                    voucherId = Session["ReceiveVoucherId"].ToString();
                }

                if (Session["ReceivePostingDate"] != null)
                {
                    postingDate = Convert.ToDateTime(Session["ReceivePostingDate"]);
                }

                RetrieveReports retrieveReportsData = new RetrieveReports();

                MemoryStream memoryStream = retrieveReportsData.viewTransferJournalReceiveReport(
                    postingDate,
                    viewConfigId,
                    viewWarehouseId,
                    viewSizeId,
                    viewStyleId,
                    viewColorId,
                    viewInventoryStatus,
                    viewSerialId,
                    viewLicensePlate,
                    viewLocationId,
                    viewBatchid,
                    viewOwnerName,
                    viewSiteId,
                    viewVersionId,
                    transferId,
                    voucherId);

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
    }
}