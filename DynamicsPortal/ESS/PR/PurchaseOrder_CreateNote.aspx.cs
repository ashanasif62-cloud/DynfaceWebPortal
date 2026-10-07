using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_CreateNote : ModalForm
    {
        private PurchaseOrder_CreditNoteSvc creditNoteSvc = new PurchaseOrder_CreditNoteSvc();

        protected override void Page_Load(object sender, EventArgs e)
        {

            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Create Note";
            }

            if (!IsPostBack)
            {
                //string purchId = Request.QueryString["PurchId"];
                //if (!string.IsNullOrEmpty(purchId))
                //{
                //    Session["SelectedPurchId"] = purchId;
                //}
                string purchIdcredit = Session["SelectedPurchIdPo"] as string;

                string vendorAccount = Session["SelectedVendorAccount"] as string;

                if (!string.IsNullOrEmpty(vendorAccount))
                {
                    reBindHeadersGrid(vendorAccount);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoVendor",
                        "alert('No vendor selected.');", true);
                }
            }
        }




        protected void reBindHeadersGrid(string vendorAccount)
        {
            DataTable dt = creditNoteSvc.retrieveByVendor(vendorAccount);

            gvInvoiceHeaders.DataSource = dt;
            gvInvoiceHeaders.DataBind();
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelect = sender as CheckBox;
            if (chkSelect == null) return;

            GridViewRow row = chkSelect.NamingContainer as GridViewRow;
            if (row == null) return;

            // Uncheck every other header row so only one is "active" at a time
            foreach (GridViewRow gvRow in gvInvoiceHeaders.Rows)
            {
                CheckBox otherChk = gvRow.FindControl("chkSelect") as CheckBox;
                if (otherChk != null && otherChk != chkSelect)
                {
                    otherChk.Checked = false;
                }
            }

            if (!chkSelect.Checked)
            {
                // Row was unchecked -> clear the lines grid
                Session.Remove("SelectedPurchId");
                gvInvoiceLines.DataSource = null;
                gvInvoiceLines.DataBind();
                return;
            }

            Label lblPurchId = row.FindControl("lblPurchId") as Label;
            string purchId = lblPurchId != null ? lblPurchId.Text.Trim() : "";

            if (string.IsNullOrEmpty(purchId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoPurchId",
                    "alert('Could not determine Purchase Order ID for the selected row.');", true);
                return;
            }

            // Store in Session
            Session["SelectedPurchId"] = purchId;

            // Load lines for this Purchase Order
            reBindLinesGrid(purchId);
        }
        protected void reBindLinesGrid(string purchId)
        {
            DataTable dt = creditNoteSvc.retrieveByPurchId(purchId);
            gvInvoiceLines.DataSource = dt;
            gvInvoiceLines.DataBind();
        }

        protected void chkSelectAllLines_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelectAll = sender as CheckBox;
            if (chkSelectAll == null) return;

            foreach (GridViewRow row in gvInvoiceLines.Rows)
            {
                CheckBox chkLine = row.FindControl("chkSelectLine") as CheckBox;
                if (chkLine != null)
                {
                    chkLine.Checked = chkSelectAll.Checked;
                }
            }
        }

        protected void chkSelectLine_CheckedChanged(object sender, EventArgs e)
        {
            // Placeholder: track selected lines here if/when you need them
            // for the credit note submission (e.g. in Session or ViewState).
        }



        protected void btnOK_Click(object sender, EventArgs e)
        {
            string invoiceId = string.Empty;
            string orderAccount = Session["SelectedVendorAccount"] as string;
            string purchId = Session["SelectedPurchIdPo"] as string;
            // Find the checked header row to get its InvoiceId
            foreach (GridViewRow row in gvInvoiceHeaders.Rows)
            {
                CheckBox chk = row.FindControl("chkSelect") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    Label lblInvoiceId = row.FindControl("lblInvoiceId") as Label;
                    invoiceId = lblInvoiceId != null ? lblInvoiceId.Text.Trim() : "";
                    break;
                }
            }

            if (string.IsNullOrEmpty(invoiceId) || string.IsNullOrEmpty(orderAccount) || string.IsNullOrEmpty(purchId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                    "alert('Please select an invoice header before creating the credit note.');", true);
                return;
            }

            // Build a single-row DataTable matching create(DataTable)'s expected columns
            DataTable dt = new DataTable();
            dt.Columns.Add("PurchID");
            dt.Columns.Add("VendorAccount");
            dt.Columns.Add("InvoiceId");

            DataRow row2 = dt.NewRow();
            row2["PurchID"] = purchId;
            row2["VendorAccount"] = orderAccount;
            row2["InvoiceId"] = invoiceId;
            dt.Rows.Add(row2);

            SysOperationResult_BOL objBOL = creditNoteSvc.create(dt);

            if (objBOL != null && objBOL.isSuccess)
            {
                NotificationMessage.showMessage(objBOL);
                ScriptManager.RegisterStartupScript(this, GetType(), "CreditNoteSuccess",
                    "closeDialog();", true);
            }
            else
            {
                NotificationMessage.showMessage(objBOL);
            }
        }
    }
}
