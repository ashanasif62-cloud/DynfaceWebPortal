using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PurchaseOrder_ProductReceipt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_ReceiptLists : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Posting receipts list";
            }

            if (!IsPostBack)
            {
                string purchId = Session["PurchaseOrderId"] as string;
                if (string.IsNullOrEmpty(purchId))
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "alertMessage",
                        "alert('No Purchase Order selected.');",
                        true
                    );
                    return;
                }
                BindSelectionDropdown();
                BindPrintDropdown();
                BindCheckCredit();
                BindSummaryUpdate();
                BindOverviewGrid(purchId);
                BindLinesGrid(purchId);
                //BindPurchasesGrid();
               
            }

        }
        //private void BindOverviewGrid()
        //{
        //    // Create empty DataTable with columns
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Update");
        //    dt.Columns.Add("PurchaseOrder");
        //    dt.Columns.Add("Name");
        //    dt.Columns.Add("ReceiptListDate", typeof(DateTime));
        //    dt.Columns.Add("DocumentDate", typeof(DateTime));
        //    dt.Columns.Add("TermsOfPayment");

        //    // (Optional) You can load real data here later, for example:
        //    // dt = YourServiceClass.GetReceiptListData();

        //    gvOverview.DataSource = dt;
        //    gvOverview.DataBind();
        //}



        //private void BindOverviewGrid(string purchId)
        //{
        //    // Create empty DataTable with columns
        //    PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();
        //    DataTable dt = svc.retrieveAll(purchId);
        //    gvOverview.DataSource = dt;
        //    gvOverview.DataBind();
        //}

        private void BindOverviewGrid(string purchId)
        {
            PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();

            // Get data from service (for Product Receipt number, Payment, etc.)
            DataTable sourceDt = svc.retrieveAll(purchId);

            // Create DataTable EXACTLY as GridView expects
            DataTable dt = new DataTable();
            dt.Columns.Add("displayOrdering");     // Update
            dt.Columns.Add("PurchId");              // Purchase Order
            dt.Columns.Add("PurchName");            // Vendor Name
           // dt.Columns.Add("Num");                  // Product Receipt
            dt.Columns.Add("TransDate", typeof(DateTime));     // Product Receipt Date
            dt.Columns.Add("DocumentDate", typeof(DateTime));  // Document Date
            dt.Columns.Add("Payment");              // Terms of Payment

            // If service returns rows → loop them
            if (sourceDt != null && sourceDt.Rows.Count > 0)
            {
                foreach (DataRow srcRow in sourceDt.Rows)
                {
                    DataRow row = dt.NewRow();

                    row["displayOrdering"] = "Receipts list";                 // Update
                    row["PurchId"] = purchId;                                   // Purchase Order
                    row["PurchName"] = Session["VendorName"]?.ToString();       // Vendor Name
                   // row["Num"] = srcRow["Num"];                                 // Product Receipt
                    row["TransDate"] = DateTime.Today;                          // Today
                   // row["DocumentDate"] = DateTime.Today;                       // Today
                    row["Payment"] = srcRow["Payment"];                         // Terms

                    dt.Rows.Add(row);
                }
            }
            else
            {
                // If service returns nothing → still show one row
                DataRow row = dt.NewRow();

                row["displayOrdering"] = "Receipts list";
                row["PurchId"] = purchId;
                row["PurchName"] = Session["VendorName"]?.ToString();
                //row["Num"] = "";                     // will be generated later
                row["TransDate"] = DateTime.Today;
               // row["DocumentDate"] = DateTime.Today;
                row["Payment"] = "";

                dt.Rows.Add(row);
            }

            gvOverview.DataSource = dt;
            gvOverview.DataBind();
        }
        protected void gvOverview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDate = e.Row.FindControl("txtReceiptListDate") as TextBox;

                if (txtDate != null && DateTime.TryParse(txtDate.Text, out DateTime dateValue))
                {
                    txtDate.Text = dateValue.ToString("yyyy-MM-dd");  // Remove time completely
                }
                TextBox txtDate1 = e.Row.FindControl("txtDocumentDate") as TextBox;

                if (txtDate1 != null && DateTime.TryParse(txtDate1.Text, out DateTime dateValues))
                {
                    txtDate1.Text = dateValues.ToString("yyyy-MM-dd");  // Remove time completely
                }
            }
        }



        private void BindLinesGrid(string purchId)
        {
            // Create empty DataTable for Lines grid
            PurchaseOrderLines lines = new PurchaseOrderLines();
            DataTable dt = lines.retrieveAll(purchId);
            if (!dt.Columns.Contains("PurchaseOrderId"))
                dt.Columns.Add("PurchaseOrderId");


            foreach (DataRow row in dt.Rows)
            {
                row["PurchaseOrderId"] = purchId;
            }

            gvLines.DataSource = dt;
            gvLines.DataBind();
        }
        //private void BindLinesGrid()
        //{
        //    // Create empty DataTable for Lines grid
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("PurchaseOrder");
        //    dt.Columns.Add("LineNumber");
        //    dt.Columns.Add("ProductNumber");
        //    dt.Columns.Add("ItemNumber");
        //    dt.Columns.Add("ProcurementCategory");
        //    dt.Columns.Add("Text");
        //    dt.Columns.Add("Site");
        //    dt.Columns.Add("Warehouse");
        //    dt.Columns.Add("InventoryStatus");
        //    dt.Columns.Add("CWUpdate");
        //    dt.Columns.Add("QuantityOrdered", typeof(decimal));
        //    dt.Columns.Add("Quantity", typeof(decimal));
        //    dt.Columns.Add("DeliverRemainder", typeof(decimal));
        //    dt.Columns.Add("UnitPrice", typeof(decimal));
        //    dt.Columns.Add("VendorBatchDate", typeof(DateTime));
        //    dt.Columns.Add("LineNetAmount", typeof(decimal));
        //    dt.Columns.Add("VendorExpiryDate", typeof(DateTime));
        //    dt.Columns.Add("QualityOrderStatus");

        //    gvLines.DataSource = dt;
        //    gvLines.DataBind();
        //}
        protected void gvLines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find your TextBox inside the template
                TextBox txt = e.Row.FindControl("TextBox1") as TextBox;

                if (txt != null)
                {
                    // Assign value from Session
                    if (Session["PurchaseOrderId"] != null)
                    {
                        txt.Text = Session["PurchaseOrderId"].ToString();
                    }
                }
            }
        }

        private void BindSelectionDropdown()
        {
            try
            {
                ddlQuantity.Items.Clear();

                // Add items with string values instead of integers
                ddlQuantity.Items.Add(new ListItem("Receive now quantity", "ReceiveNow"));
                ddlQuantity.Items.Add(new ListItem("Ordered quantity", "All"));
                ddlQuantity.Items.Add(new ListItem("Registered quantity", "Recorded"));
                ddlQuantity.Items.Add(new ListItem("Product receipt quantity", "PackingSlip"));
                ddlQuantity.Items.Add(new ListItem("Registered quantity and services", "RegisteredAndServices"));

                // Set default selection (optional)
                ddlQuantity.SelectedValue = "All"; // Receive now quantity
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }

        private void BindPrintDropdown()
        {
            try
            {
                ddlPrint.Items.Clear();

                // Add items with string values instead of integers
                ddlPrint.Items.Add(new ListItem("Current", "Current"));
                ddlPrint.Items.Add(new ListItem("After", "After"));



                ddlPrint.SelectedValue = "Current"; //  by default Current 
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }

        private void BindCheckCredit()
        {
            try
            {
                ddlCheckCredit.Items.Clear();

                // Add items with string values instead of integers
                ddlCheckCredit.Items.Add(new ListItem("None", "None"));
                ddlCheckCredit.Items.Add(new ListItem("Balance", "Balance"));
                ddlCheckCredit.Items.Add(new ListItem("Balance+packing slip or product receipt", "Balance+packing slip or product receipt"));
                ddlCheckCredit.Items.Add(new ListItem("Balance+All", "Balance+All"));



                ddlCheckCredit.SelectedValue = "Current"; //  by default Current 
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }

        private void BindSummaryUpdate()
        {
            try
            {
                ddlSummary.Items.Clear();

                // Add items with string values instead of integers
                ddlSummary.Items.Add(new ListItem("None", "None"));
                ddlSummary.Items.Add(new ListItem("Invoice Account", "Invoice Account"));
                ddlSummary.Items.Add(new ListItem("Order", "Order"));
                ddlSummary.Items.Add(new ListItem("Automatic Summary", "Automatic Summary"));



                ddlSummary.SelectedValue = "Current"; //  by default Current 
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(GetType().FullName, ex);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                // Grid must have rows
                if (gvLines.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoLines",
                        "alert('No lines available.');", true);
                    return;
                }

                // Get Purchase Order ID from first row
                string purchId = "";
                TextBox txtPurchId = gvLines.Rows[0].FindControl("TextBox1") as TextBox;
                if (txtPurchId != null)
                    purchId = txtPurchId.Text.Trim();

                if (string.IsNullOrEmpty(purchId))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoPO",
                        "alert('Purchase Order ID is missing.');", true);
                    return;
                }

                // Prepare list of lines (only line number needed)
                List<PO_ProductReceiptSvcContract> lineList = new List<PO_ProductReceiptSvcContract>();

                for (int i = 0; i < gvLines.Rows.Count; i++)
                {
                    GridViewRow row = gvLines.Rows[i];

                    // Get Line Number
                    TextBox txtLineNumber = row.FindControl("txtLineNumber") as TextBox;
                    long lineNumber = 0;

                    if (txtLineNumber != null)
                        long.TryParse(txtLineNumber.Text.Trim(), out lineNumber);

                    // Skip invalid rows
                    if (lineNumber == 0)
                        continue;

                    // Create contract with ONLY required fields
                    PO_ProductReceiptSvcContract contract = new PO_ProductReceiptSvcContract
                    {
                        OrigPurchId = purchId,
                        PurchaseLineLineNumber = lineNumber
                    };

                    lineList.Add(contract);
                }

                // If no valid lines found
                if (lineList.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoValid",
                        "alert('No valid line numbers found.');", true);
                    return;
                }

                // Call service with only purchId + lineList
                PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();
                GeneralContract result = svc.makPurchReceiptListPost(purchId, lineList);

                // Handle result
                if (result != null && result.IsSuccess)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Success",
                        "alert('Posted successfully!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Fail",
                        $"alert('Error: {result?.Message ?? "Unknown error"}');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Error",
                    $"alert('Exception: {ex.Message}');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ESS/PR/AllPurchaseOrder_ListPage.aspx");
        }


        //private void BindPurchasesGrid()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("PurchaseOrder");
        //    dt.Columns.Add("Name");

        //    // Later you can replace with:
        //    // dt = YourPurchaseOrderService.GetPurchaseOrderData();

        //    gvPurchases.DataSource = dt;
        //    gvPurchases.DataBind();
        //}
    }
}