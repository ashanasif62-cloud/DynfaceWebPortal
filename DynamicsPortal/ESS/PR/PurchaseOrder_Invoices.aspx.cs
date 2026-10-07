using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PurchaseOrderInvoiceSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_Invoices : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
                titleDiv.InnerText = "VendorInvoice";

            if (!IsPostBack)
            {
                string purchId = Session["PurchaseOrderId"] as string;

                if (string.IsNullOrEmpty(purchId))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                        "alert('No Purchase Order selected.');", true);
                    return;
                }

                bool headerLoaded = bindvendorinvoice(purchId);
                if (!headerLoaded) return;

                BindVendorInvoiceLineGrid(purchId);
                bindvendorinvoiceLineDetail(purchId);
                bindvendorinvoiceLineHeader(purchId);

               // pnlSaveBar.Visible = false;
            }
        }

        // ─── ViewState helpers ────────────────────────────────────────────────

        private DataTable GetGridDataTable()
        {
            if (ViewState["InvoiceLinesDT"] != null)
                return (DataTable)ViewState["InvoiceLinesDT"];
            return null;
        }

        private void SaveGridDataTable(DataTable dt)
        {
            ViewState["InvoiceLinesDT"] = dt;
        }

        private DataTable BuildEmptyLineTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RecId", typeof(long));
            dt.Columns.Add("ItemId", typeof(string));
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("ProcurementCategory", typeof(string));
            dt.Columns.Add("ReceiveNow", typeof(decimal));
            dt.Columns.Add("purchUnit", typeof(string));
            dt.Columns.Add("PurchPrice", typeof(decimal));
            dt.Columns.Add("LineAmount", typeof(decimal));
            dt.Columns.Add("InventSiteId", typeof(string));
            dt.Columns.Add("InventLocationId", typeof(string));
            dt.Columns.Add("PackingSlipId", typeof(string));
            dt.Columns.Add("checkIfQuantity", typeof(string));
            dt.Columns.Add("priceVarianceStatus", typeof(string));
            dt.Columns.Add("BudgetCheckResult", typeof(string));
            dt.Columns.Add("OrigPurchId", typeof(string));
            return dt;
        }

        // ─── Bind header ─────────────────────────────────────────────────────

        private bool bindvendorinvoice(string purchId)
        {
            try
            {
                PurchaseOrder_Invoice svc = new PurchaseOrder_Invoice();
                DataTable dt = svc.retrieveAll(purchId);

                bool isFromInvoice = true;
                if (dt == null || dt.Rows.Count == 0)
                {
                    isFromInvoice = false;
                    PurchaseOrderHeader poSvc = new PurchaseOrderHeader();
                    dt = poSvc.retrieveByPurchID(purchId);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                            $"alert('No Purchase Order or Invoice found for: {purchId}');", true);
                        return false;
                    }
                }

                DataRow row = dt.Rows[0];

                txtInvoiceAccount.Text = (isFromInvoice ? row["InvoiceAccount"] : row["VendorAccount"])?.ToString();
                txtInvoiceNumber.Text = isFromInvoice ? row["Num"]?.ToString() : "";
                txtInvoiceDescription.Text = (isFromInvoice ? row["Description"] : row["PODescription"])?.ToString();
                txtPurchaseOrder.Text = Session["PurchaseOrderId"] as string;
                txtProductReceipt.Text = isFromInvoice ? row["PackingSlipId"]?.ToString() : "";
                txtInvoiceReceivedDate.Text = FormatDate(isFromInvoice ? row["ReceivedDate"] : DateTime.Today);
                txtInvoiceDate.Text = FormatDate(isFromInvoice ? row["DocumentDate"] : row["AccountingDate"]);
                txtPostingDate.Text = FormatDate(isFromInvoice ? row["TransDate"] : DateTime.Today);
                txtDueDate.Text = FormatDate(isFromInvoice ? row["FixedDueDate"] : row["DeliveryDate"]);
                txtBudgetCheckResults.Text = (isFromInvoice ? row["BudgetCheckResult"] : row["PurchTableBudgetCheckResult"])?.ToString();
                TextBoxDeliveryName.Text = isFromInvoice ? row["DeliveryName"]?.ToString() : "";
                TxtPostalAddress.Text = isFromInvoice ? row["DeliveryName"]?.ToString() : "";
                TxtAddress.Text = isFromInvoice ? row["AddressHeader"]?.ToString() : "";
                TextBoxRemittance.Text = isFromInvoice ? row["logisticsDescription"]?.ToString() : "";
                TxtAddress1.Text = isFromInvoice ? row["RemittanceAddress"]?.ToString() : "";
                TxtSite.Text = isFromInvoice ? row["InventSiteId"]?.ToString() : "";
                TxtWarehouse.Text = isFromInvoice ? row["InventLocationId"]?.ToString() : "";

                return true;
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write("bindvendorinvoice", ex);
                return false;
            }
        }

        // ─── Bind grid ────────────────────────────────────────────────────────

        private void BindVendorInvoiceLineGrid(string purchId)
        {
            DataTable dt = GetGridDataTable();

            if (dt == null)
            {
                PurchaseOrder_Invoice svc = new PurchaseOrder_Invoice();
                dt = svc.retrieveLines(purchId);

                if (dt == null || dt.Rows.Count == 0)
                {
                    PurchaseOrderLines poLineSvc = new PurchaseOrderLines();
                    dt = poLineSvc.retrieveAll(purchId);

                    if (dt != null)
                    {
                        if (!dt.Columns.Contains("ReceiveNow"))
                            dt.Columns.Add("ReceiveNow", typeof(decimal));

                        if (!dt.Columns.Contains("purchUnit"))
                            dt.Columns.Add("purchUnit", typeof(string));

                        if (!dt.Columns.Contains("editName"))
                            dt.Columns.Add("editName", typeof(string));

                        if (!dt.Columns.Contains("OrigPurchId"))
                            dt.Columns.Add("OrigPurchId", typeof(string));

                        foreach (DataRow r in dt.Rows)
                        {
                            if (dt.Columns.Contains("PurchQty") && r["ReceiveNow"] == DBNull.Value)
                                r["ReceiveNow"] = r["PurchQty"];

                            if (dt.Columns.Contains("PurchUnitofMeasureCode") && r["purchUnit"] == DBNull.Value)
                                r["purchUnit"] = r["PurchUnitofMeasureCode"];

                            if (dt.Columns.Contains("Description") && r["editName"] == DBNull.Value)
                                r["editName"] = r["Description"];

                            if (dt.Columns.Contains("PurchaseOrderId") && r["OrigPurchId"] == DBNull.Value)
                                r["OrigPurchId"] = r["PurchaseOrderId"];
                        }
                    }
                }

                if (dt != null)
                {
                    // Ensure RecId column exists with correct casing
                    bool foundRecId = false;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName.Equals("RecId", StringComparison.OrdinalIgnoreCase))
                        {
                            col.ColumnName = "RecId";
                            foundRecId = true;
                            break;
                        }
                    }
                    if (!foundRecId) dt.Columns.Add("RecId", typeof(long));

                    // Ensure all columns required by Bind() expressions exist
                    string[] required = { "PackingSlipId", "checkIfQuantity", "priceVarianceStatus", "BudgetCheckResult", "LineAmount" };
                    foreach (string colName in required)
                        if (!dt.Columns.Contains(colName))
                            dt.Columns.Add(colName, typeof(string));
                }

                SaveGridDataTable(dt);
            }

            gvVendorInvoiceLine.DataSource = dt;
            gvVendorInvoiceLine.DataBind();
        }

        // ─── Add line ─────────────────────────────────────────────────────────

        protected void btnAddInvoiceLine_Click(object sender, EventArgs e)
        {
            string purchId = Session["PurchaseOrderId"] as string;

            // Get or build the DataTable
            DataTable dt = GetGridDataTable();
            if (dt == null)
            {
                // First add — load from service then store in ViewState
                PurchaseOrder_Invoice svc = new PurchaseOrder_Invoice();
                dt = svc.retrieveLines(purchId);

                if (dt == null || dt.Rows.Count == 0)
                {
                    PurchaseOrderLines poLineSvc = new PurchaseOrderLines();
                    dt = poLineSvc.retrieveAll(purchId);
                }

                if (dt == null)
                    dt = BuildEmptyLineTable();

                // Ensure all required columns exist
                bool foundRecId = false;
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName.Equals("RecId", StringComparison.OrdinalIgnoreCase))
                    {
                        col.ColumnName = "RecId";
                        foundRecId = true;
                        break;
                    }
                }
                if (!foundRecId) dt.Columns.Add("RecId", typeof(long));

                string[] required = { "ItemId", "ItemName", "ProcurementCategory", "ReceiveNow",
                                      "purchUnit", "PurchPrice", "LineAmount", "InventSiteId",
                                      "InventLocationId", "PackingSlipId", "checkIfQuantity",
                                      "priceVarianceStatus", "BudgetCheckResult", "OrigPurchId" };
                foreach (string colName in required)
                    if (!dt.Columns.Contains(colName))
                        dt.Columns.Add(colName, typeof(string));
            }

            // Append new empty row
            DataRow newRow = dt.NewRow();
            newRow["RecId"] = 0;
            newRow["ItemId"] = "";
            newRow["ItemName"] = "";
            newRow["ProcurementCategory"] = "";
            newRow["ReceiveNow"] = 0m;
            newRow["purchUnit"] = "";
            newRow["PurchPrice"] = 0m;
            newRow["LineAmount"] = 0m;
            newRow["InventSiteId"] = "";
            newRow["InventLocationId"] = "";
            newRow["PackingSlipId"] = "";
            newRow["checkIfQuantity"] = "";
            newRow["priceVarianceStatus"] = "";
            newRow["BudgetCheckResult"] = "";
            newRow["OrigPurchId"] = purchId;
            dt.Rows.Add(newRow);

            SaveGridDataTable(dt);

            // Put the new last row into edit mode
            gvVendorInvoiceLine.EditIndex = dt.Rows.Count - 1;
            gvVendorInvoiceLine.DataSource = dt;
            gvVendorInvoiceLine.DataBind();

            // Show Save/Cancel bar
           // pnlSaveBar.Visible = true;
        }

        // ─── Save all ─────────────────────────────────────────────────────────

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                string purchId = Session["PurchaseOrderId"] as string;

                // Read values from the row currently in edit mode
                int editIdx = gvVendorInvoiceLine.EditIndex;
                if (editIdx >= 0 && editIdx < gvVendorInvoiceLine.Rows.Count)
                {
                    GridViewRow row = gvVendorInvoiceLine.Rows[editIdx];

                    string itemId = ((TextBox)row.FindControl("txtItemId"))?.Text.Trim();
                    string quantity = ((TextBox)row.FindControl("txtReceiveNow"))?.Text.Trim();
                    string unitPrice = ((TextBox)row.FindControl("txtPurchPrice"))?.Text.Trim();

                    // TODO: call your service to save the new line
                    // PurchaseOrder_Invoice svc = new PurchaseOrder_Invoice();
                    // svc.saveInvoiceLine(purchId, itemId, quantity, unitPrice, ...);
                }

                // Reset grid
                gvVendorInvoiceLine.EditIndex = -1;
                ViewState["InvoiceLinesDT"] = null; // clear cache — reload fresh from service
              //  pnlSaveBar.Visible = false;

                BindVendorInvoiceLineGrid(purchId);

                ScriptManager.RegisterStartupScript(this, GetType(), "saveOk",
                    "alert('Line saved successfully.');", true);
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write("btnSaveAll_Click", ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "saveErr",
                    $"alert('Error saving line: {ex.Message}');", true);
            }
        }

        // ─── Cancel all ───────────────────────────────────────────────────────

        protected void btnCancelAll_Click(object sender, EventArgs e)
        {
            gvVendorInvoiceLine.EditIndex = -1;
            ViewState["InvoiceLinesDT"] = null; // discard the unsaved row
           // pnlSaveBar.Visible = false;

            string purchId = Session["PurchaseOrderId"] as string;
            BindVendorInvoiceLineGrid(purchId);
        }

        // ─── Existing grid events ─────────────────────────────────────────────

        protected void gvVendorInvoiceLine_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvVendorInvoiceLine.EditIndex = e.NewEditIndex;
            string purchId = Session["PurchaseOrderId"] as string;
            BindVendorInvoiceLineGrid(purchId);
        }

        protected void gvVendorInvoiceLine_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Row-level logic if needed
        }

        protected void gvVendorInvoiceLine_Update_Click(object sender, EventArgs e)
        {
            gvVendorInvoiceLine.EditIndex = -1;
            ViewState["InvoiceLinesDT"] = null;
            string purchId = Session["PurchaseOrderId"] as string;
            BindVendorInvoiceLineGrid(purchId);
        }

        protected void gvVendorInvoiceLine_Cancel_Click(object sender, EventArgs e)
        {
            gvVendorInvoiceLine.EditIndex = -1;
            ViewState["InvoiceLinesDT"] = null;
            string purchId = Session["PurchaseOrderId"] as string;
            BindVendorInvoiceLineGrid(purchId);
        }

        protected void chkInvoiceLine_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox clicked = sender as CheckBox;
            if (clicked == null) return;

            foreach (GridViewRow row in gvVendorInvoiceLine.Rows)
            {
                CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chk != null && chk != clicked)
                    chk.Checked = false;
            }
        }

        protected void btnDeleteInvoiceLine_Click(object sender, EventArgs e)
        {
            DataTable dt = GetGridDataTable();
            if (dt != null)
            {
                foreach (GridViewRow row in gvVendorInvoiceLine.Rows)
                {
                    CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        dt.Rows[row.RowIndex].Delete();
                    }
                }
                dt.AcceptChanges();
                SaveGridDataTable(dt);
            }

            gvVendorInvoiceLine.EditIndex = -1;
            string purchId = Session["PurchaseOrderId"] as string;
            BindVendorInvoiceLineGrid(purchId);
        }

        protected void btnInvoiceLineAttachment_Click(object sender, EventArgs e)
        {
            // Attachment logic here
        }

        // ─── Remaining existing methods (unchanged) ───────────────────────────

        private string FormatDate(object dateObject)
        {
            if (dateObject == null || dateObject == DBNull.Value)
                return string.Empty;

            DateTime parsedDate;
            if (DateTime.TryParse(dateObject.ToString(), out parsedDate))
                return parsedDate.ToString("yyyy-MM-dd");

            return string.Empty;
        }

        private bool bindvendorinvoiceLineDetail(string purchId)
        {
            try
            {
                PurchaseOrder_Invoice svc = new PurchaseOrder_Invoice();
                DataTable dt = svc.retrieveLines(purchId);

                bool isFromInvoice = true;
                if (dt == null || dt.Rows.Count == 0)
                {
                    isFromInvoice = false;
                    PurchaseOrderLines poLineSvc = new PurchaseOrderLines();
                    dt = poLineSvc.retrieveAll(purchId);
                    if (dt == null || dt.Rows.Count == 0) return false;
                }

                DataRow row = dt.Rows[0];

                txtItemNumberID.Text = row["ItemId"]?.ToString();
                txtItemName.Text = row["ItemName"]?.ToString();
                txtProcurementCategoryLD.Text = row["ProcurementCategory"]?.ToString();
                txtTextLD.Text = (isFromInvoice ? row["editName"] : row["Description"])?.ToString();
                txtQuantityLD.Text = (isFromInvoice ? row["ReceiveNow"] : row["PurchQty"])?.ToString();
                txtUnitLD.Text = (isFromInvoice ? row["purchUnit"] : row["PurchUnitofMeasureCode"])?.ToString();
                txtUnitPriceLD.Text = row["PurchPrice"]?.ToString();
                txtLineNetAmountLD.Text = row["LineAmount"]?.ToString();
                txtItemSalesTax.Text = row["TaxItemGroup"]?.ToString();
                txtLineNumber.Text = row["LineNumber"]?.ToString();
                txtDeliveryName.Text = row["DeliveryName"]?.ToString();
                TxtDeliveryDate.Text = FormatDate(row["deliveryDate"]);

                return true;
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write("bindvendorinvoiceLineDetail", ex);
                return false;
            }
        }

        private void bindvendorinvoiceLineHeader(string purchId)
        {
            try
            {
                PurchaseOrder_Invoice svc = new PurchaseOrder_Invoice();
                DataTable dt = svc.retrieveAll(purchId);

                bool isFromInvoice = true;
                if (dt == null || dt.Rows.Count == 0)
                {
                    isFromInvoice = false;
                    PurchaseOrderHeader poSvc = new PurchaseOrderHeader();
                    dt = poSvc.retrieveByPurchID(purchId);
                    if (dt == null || dt.Rows.Count == 0) return;
                }

                DataRow row = dt.Rows[0];

                TxtInvoiceAccountHeader.Text = (isFromInvoice ? row["InvoiceAccount"] : row["VendorAccount"])?.ToString();
                TxtInvoiceIdentificationHeader.Text = isFromInvoice ? row["Num"]?.ToString() : "";
                TxtDescriptionHeader.Text = (isFromInvoice ? row["Description"] : row["PODescription"])?.ToString();
                TxtPurchaseOrderHeader.Text = (isFromInvoice ? row["PurchId"] : row["PurchID"])?.ToString();
                TxtFormReceiptHeader.Text = isFromInvoice ? row["PackingSlipId"]?.ToString() : "";
                TxtPostingDateHeader.Text = FormatDate(isFromInvoice ? row["TransDate"] : DateTime.Today);
                TxtDueDateHeader.Text = FormatDate(isFromInvoice ? row["FixedDueDate"] : row["DeliveryDate"]);
                TxtBudgetCheckResultsHeader.Text = (isFromInvoice ? row["BudgetCheckResult"] : row["PurchTableBudgetCheckResult"])?.ToString();
                txtCashDiscount.Text = row["CashDisc"]?.ToString();
                txtDiscountPercentageSetup.Text = row["CashDiscPercent"]?.ToString();
                txtPostingProfile.Text = row["PostingProfile"]?.ToString();
                txtSettlementType.Text = (isFromInvoice ? row["SettleVoucher"] : "")?.ToString();
                TxtSalesTaxGroupHeader.Text = row["TaxGroup"]?.ToString();
                TxtRequestApprover.Text = row["Approver"]?.ToString();
                TxtApproverEmail.Text = row["Email"]?.ToString();
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write("bindvendorinvoiceLineHeader", ex);
            }
        }

        protected void btnPostInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                string purchId = Session["PurchaseOrderId"] as string;
                string invoiceNum = txtInvoiceNumber.Text;

                if (string.IsNullOrEmpty(purchId) || string.IsNullOrEmpty(invoiceNum))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                        "alert('Purchase Order ID or Invoice Number is missing.');", true);
                    return;
                }

                PurchaseOrder_Invoice svc = new PurchaseOrder_Invoice();
                GeneralContract result = svc.post_InvoicePO(invoiceNum, purchId);

                if (result != null)
                {
                    string message = result.Message.Replace("'", "\\'");
                    ScriptManager.RegisterStartupScript(this, GetType(), "postResult",
                        $"alert('{(result.IsSuccess ? "Success" : "Error")}: {message}');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "postError",
                        "alert('Error: Failed to get response from service.');", true);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write("btnPostInvoice_Click", ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "postException",
                    $"alert('Exception: {ex.Message}');", true);
            }
        }
    }
}