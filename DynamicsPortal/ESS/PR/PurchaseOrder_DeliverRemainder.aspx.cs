using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PurchaseOrderHeaderSvcrRefernce;
using PortalIntegration.PurchaseOrderLinesSvcReference;
using PortalIntegration.TransferOrderLinesSvcReference;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PO_DeliverRemainder : ModalForm
    {
        PurchaseOrderLines _linesSvc = new PurchaseOrderLines();
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pageMenuId = "PO_DeliverRemainder";

                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Update Remaining Quantity";
                    titleDiv.Style["font-weight"] = "600";
                    titleDiv.Style["font-size"] = "20px";
                }

                // ✅ Load data from session
                LoadQuantitiesFromSession();
            }
        }

        private void LoadQuantitiesFromSession()
        {
            // Get Purchase Quantity from session
            if (Session["DeliverRemainder"] != null)
            {
                string purchaseQty = Session["DeliverRemainder"].ToString();

                // Set both Purchase Quantity and Inventory Quantity to same value
                txtPurchaseQuantity.Text = purchaseQty;
                txtInventoryQuantity.Text = purchaseQty;
            }
            else
            {
                // Optional: clear fields if no session data
                txtPurchaseQuantity.Text = string.Empty;
                txtInventoryQuantity.Text = string.Empty;
            }
        }

        //private void LoadQuantitiesFromQueryString()
        //{
        //    string purchaseQty = Request.QueryString["pqty"];
        //    string inventoryQty = Request.QueryString["iqty"];

        //    if (!string.IsNullOrEmpty(purchaseQty))
        //        txtPurchaseQuantity.Text = Convert.ToDecimal(purchaseQty).ToString("N2");

        //    if (!string.IsNullOrEmpty(inventoryQty))
        //        txtInventoryQuantity.Text = Convert.ToDecimal(inventoryQty).ToString("N2");
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
         
        }

        protected void btnCancel_Quantity_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Ensure selected line info exists
                if (Session["RecId"] == null || Session["LineNumber"] == null || Session["PurchaseOrderId"] == null)
                {
                    NotificationMessage.showMessage("Required line details not found in session.");
                    return;
                }

                long recId = Convert.ToInt64(Session["RecId"]);
                string purchaseOrderId = Session["PurchaseOrderId"].ToString();

                // Parse LineNumber safely
                decimal lineNumber;
                if (!decimal.TryParse(Session["LineNumber"].ToString(), out lineNumber))
                {
                    NotificationMessage.showMessage("Invalid line number format.");
                    return;
                }

                // Parse updated quantity (user input)
              
                // ✅ Build contract for service call
                var contract = new PurchUpdateRemainContract
                {
                    RecId = recId,
                    PurchId = purchaseOrderId,
                    LineNum = lineNumber
                                                     };

                // ✅ Call service to apply the update
                SysOperationResult_BOL result = _linesSvc.CancelUpdate(recId);

                if (result != null && result.isSuccess)
                {
                    NotificationMessage.showMessage(result);

                

                    // Refresh or close popup
                    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup",
                        "window.parent.location.reload();", true);
                }
                else
                {
                    string msg = (result != null && !string.IsNullOrWhiteSpace(result.Message))
                        ? result.Message
                        : "Failed to update quantity.";
                    NotificationMessage.showMessage(msg);
                }
            }
            catch (Exception ex)
            {
                NotificationMessage.showMessage("Error: " + ex.Message);
            }
        }




    }
}
