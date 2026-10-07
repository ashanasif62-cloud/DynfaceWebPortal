using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.RFQServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DynamicsPortal.ESS.PR.PurchaseRequisitionLines;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseRequestforQuotation : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "PurchaseRequestFroQuotation";

            Page.Title = "Create Purchase Requisition Request";

            base.Page_Load(sender, e);

            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Create Purchase Requisition Request"; // Set the text in the div
                titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
            }

            if (!isUserAuthenticated)
                return;

            if (!isPageAuthorizated)
                return;

            if (!IsPostBack)
            {
                BindGridFromSession(); // Or any data binding method
            }
        }

        private void BindGridFromSession()
        {
            DataTable dt = Session["PurchLineTable"] as DataTable ?? null;
            gridView.DataSource = dt;
            gridView.DataBind();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                RFQContract contract = new RFQContract();
                List<long> selectedRecIds = new List<long>();
                string purchaseRequisition = string.Empty;

                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chkSelect = row.FindControl("chk_SelectSingle") as CheckBox;
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        // Get RecId from DataKeys

                        long recId = 0;
                        Int64.TryParse(((Label)row.FindControl("lblRecId")).Text, out recId);
                        if (recId > 0)
                        {
                            selectedRecIds.Add(recId);
                        }

                        // Get PurchaseRequisition from TemplateField label
                        if (string.IsNullOrEmpty(purchaseRequisition))
                        {
                            Label lblPurchaseRequisition = (Label)row.FindControl("lblPurchaseRequisition");
                            if (lblPurchaseRequisition != null)
                            {
                                purchaseRequisition = lblPurchaseRequisition.Text.Trim();
                            }
                        }
                    }
                }

                // Populate contract
                contract.SelectedLineRecIds = selectedRecIds.Cast<object>().ToArray();
                contract.PurchReqId = purchaseRequisition;
                contract.DataAreaId = SessionVariables.getUserCurrentDataAreaId();

                // Call your business logic
                RFQ newrfq = new RFQ();
                SysOperationResult_BOL result = newrfq.createrfq(contract);
                NotificationMessage.showMessage(result);

                if (result.isSuccess)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeDialogScript", "setTimeout(function() { closeDialog(); }, 5000);", true);

                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
        }


    }
}
        