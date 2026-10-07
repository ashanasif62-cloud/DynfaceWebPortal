using BussinessObject;
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
    public partial class PurchaseOrder_ProductReceipt : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Posting product receipt";
            }

            if (!IsPostBack)
            {
                string purchId = Session["PurchaseOrderId"] as string;
                if (string.IsNullOrEmpty(purchId))
                {

                    SysOperationResult_BOL result = new SysOperationResult_BOL();
                    result.isSuccess = false;
                    result.AlertType = AlertType.Error.ToString();
                    result.Message = "No Purchase Order selected";
                    NotificationMessage.showMessage(result);
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
        //private void BindOverviewGrid(string purchId)
        //{
        //    // Create empty DataTable with columns
        //    PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();
        //    DataTable dt = svc.retrieveAll(purchId);

        //    //DataTable dt = svc.retrieveAll(purchId);
        //    ////dt.Columns.Add("displayOrdering");
        //    //dt.Columns.Add("PurchId");
        //    //dt.Columns.Add("PurchName");
        //    //dt.Columns.Add("TransDate", typeof(DateTime));
        //    //dt.Columns.Add("DocumentDate", typeof(DateTime));
        //    //dt.Columns.Add("Payment");

        //    //// (Optional) You can load real data here later, for example:
        //    //// dt = YourServiceClass.GetReceiptListData();

        //    gvOverview.DataSource = dt;
        //    gvOverview.DataBind();
        //}

        //private void BindOverviewGrid(string purchId)
        //{
        //    PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();


        //        DataTable sourceDt = svc.retrieveAll(purchId);

        //    // Create DataTable EXACTLY as GridView expects
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("displayOrdering");     // Update
        //    dt.Columns.Add("PurchId");              // Purchase Order
        //    dt.Columns.Add("PurchName");            // Vendor Name
        //    dt.Columns.Add("Num");                  // Product Receipt
        //    dt.Columns.Add("TransDate", typeof(DateTime));     // Product Receipt Date
        //    dt.Columns.Add("DocumentDate", typeof(DateTime));  // Document Date
        //   // dt.Columns.Add("Payment");              // Terms of Payment

        //    // If service returns rows → loop them
        //    if (sourceDt != null && sourceDt.Rows.Count > 0)
        //    {
        //        foreach (DataRow srcRow in sourceDt.Rows)
        //        {
        //            DataRow row = dt.NewRow();

        //            row["displayOrdering"] = "Product receipt";                 // Update
        //            row["PurchId"] = purchId;                                   // Purchase Order
        //            row["PurchName"] = Session["VendorName"]?.ToString();       // Vendor Name
        //            row["Num"] = srcRow["Num"];                                 // Product Receipt
        //            row["TransDate"] = DateTime.Today;                          // Today
        //           // row["DocumentDate"] = DateTime.Today;                       // Today
        //            //row["Payment"] = srcRow["Payment"];                         // Terms

        //            dt.Rows.Add(row);
        //        }
        //    }
        //    else
        //    {
        //        // If service returns nothing → still show one row
        //        DataRow row = dt.NewRow();

        //        row["displayOrdering"] = "Product receipt";
        //        row["PurchId"] = purchId;
        //        row["PurchName"] = Session["VendorName"]?.ToString();
        //        row["Num"] = "";                     // will be generated later
        //        row["TransDate"] = DateTime.Today;
        //       // row["DocumentDate"] = DateTime.Today;
        //        row["Payment"] = "";

        //        dt.Rows.Add(row);
        //    }

        //    gvOverview.DataSource = dt;
        //    gvOverview.DataBind();
        //}


        private void BindOverviewGrid(string purchId)
        {
            PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();
            DataTable sourceDt = svc.retrieveAll(purchId);

            DataTable dt = new DataTable();
            dt.Columns.Add("displayOrdering");
            dt.Columns.Add("PurchId");
            dt.Columns.Add("PurchName");
            dt.Columns.Add("Num");
            dt.Columns.Add("TransDate", typeof(DateTime));
            dt.Columns.Add("DocumentDate", typeof(string));   // string so it can be empty
            dt.Columns.Add("paymTermId");

            // ✅ Always ONE row only — overview is a header, not per-line
            DataRow row = dt.NewRow();
            row["displayOrdering"] = "Product receipt";
            row["PurchId"] = purchId;
            row["PurchName"] =
            row["TransDate"] = DateTime.Today;
            row["DocumentDate"] = "";    // ✅ empty — user must select
            row["paymTermId"] = "";

            // ✅ Get Num from first source row only if available
            if (sourceDt != null && sourceDt.Rows.Count > 0)
            {
                row["Num"] = sourceDt.Rows[0].Table.Columns.Contains("Num")
                             ? sourceDt.Rows[0]["Num"]

                             : "";
                row["PurchName"] = sourceDt.Rows[0].Table.Columns.Contains("PurchName")
                            ? sourceDt.Rows[0]["PurchName"]
                          : "";

                row["paymTermId"] = sourceDt.Rows[0].Table.Columns.Contains("paymTermId")
                                    ? sourceDt.Rows[0]["paymTermId"]
                                    : "";
            }
            else
            {
                row["Num"] = "";
            }

            dt.Rows.Add(row);  // ✅ Only ONE row added

            gvOverview.DataSource = dt;
            gvOverview.DataBind();
        }


        //protected void gvOverview_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DropDownList ddlTermsOfPayment = e.Row.FindControl("ddlTermsOfPayment") as DropDownList;

        //        if (ddlTermsOfPayment != null)
        //        {
        //            PurchaseOrder_ProductsReceipt header = new PurchaseOrder_ProductsReceipt();
        //            DataTable dt = header.retrieveTermsOfPayment();

        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                ddlTermsOfPayment.DataSource = dt;
        //                ddlTermsOfPayment.DataTextField = "paymTermId";  // column from your DataTable
        //                ddlTermsOfPayment.DataValueField = "paymTermId";
        //                ddlTermsOfPayment.DataBind();
        //                ddlTermsOfPayment.Items.Insert(0, new ListItem("-- Select --", ""));
        //            }
        //            TextBox txtUpdate = e.Row.FindControl("txtUpdate") as TextBox;

        //            if (txtUpdate != null && Session["UpdateMode"] != null)
        //            {
        //                txtUpdate.Text = Session["UpdateMode"].ToString();  // "Product receipt"
        //            }

        //            TextBox txtDate = e.Row.FindControl("txtReceiptListDate") as TextBox;

        //            if (txtDate != null && DateTime.TryParse(txtDate.Text, out DateTime dateValue))
        //            {
        //                txtDate.Text = dateValue.ToString("yyyy-MM-dd");  // Remove time completely
        //            }
        //            TextBox txtDate1 = e.Row.FindControl("txtDocumentDate") as TextBox;

        //            if (txtDate1 != null && DateTime.TryParse(txtDate1.Text, out DateTime dateValues))
        //            {
        //                txtDate1.Text = dateValues.ToString("yyyy-MM-dd");  // Remove time completely
        //            }
        //        }
        //    }
        //}

        protected void gvOverview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Update textbox
                TextBox txtUpdate = e.Row.FindControl("txtUpdate") as TextBox;
                if (txtUpdate != null && Session["UpdateMode"] != null)
                    txtUpdate.Text = Session["UpdateMode"].ToString();

                // Product Receipt Date — format if has value
                TextBox txtDate = e.Row.FindControl("txtProductReceiptDate") as TextBox;
                if (txtDate != null && DateTime.TryParse(txtDate.Text, out DateTime dateValue))
                    txtDate.Text = dateValue.ToString("yyyy-MM-dd");


                // Terms of Payment DDL
                DropDownList ddlTermsOfPayment = e.Row.FindControl("ddlTermsOfPayment") as DropDownList;
                if (ddlTermsOfPayment != null)
                {
                    PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();
                    DataTable dt = svc.retrieveTermsofPrePayment(); // ← use whichever method returns the data

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ddlTermsOfPayment.DataSource = dt;
                        ddlTermsOfPayment.DataTextField = "paymTermId";
                        ddlTermsOfPayment.DataValueField = "paymTermId";
                        ddlTermsOfPayment.DataBind();
                        ddlTermsOfPayment.Items.Insert(0, new ListItem("-- Select --", ""));
                    }

                    // Pre-select existing value if available
                    string existingValue = DataBinder.Eval(e.Row.DataItem, "paymTermId")?.ToString();
                    if (!string.IsNullOrEmpty(existingValue) &&
                        ddlTermsOfPayment.Items.FindByValue(existingValue) != null)
                    {
                        ddlTermsOfPayment.SelectedValue = existingValue;
                    }
                }
            }
        }
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



        private void BindLinesGrid(string purchId)
        {
            // Create empty DataTable for Lines grid
            PurchaseOrderLines svc = new PurchaseOrderLines();
            DataTable dt = svc.retrieveAll(purchId);
            if (!dt.Columns.Contains("PurchaseOrderId"))
                dt.Columns.Add("PurchaseOrderId");


            foreach (DataRow row in dt.Rows)
            {
                row["PurchaseOrderId"] = purchId;
            }
            ViewState["LinesDT"] = dt;

            gvLines.DataSource = dt;
            gvLines.DataBind();
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

        //protected void btnOk_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (gvLines.Rows.Count == 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "NoLines",
        //                "alert('No lines available to post.');", true);
        //            return;
        //        }

        //        // Get Purchase Order ID from first row
        //        string purchId = string.Empty;
        //        TextBox txtPurchId = gvLines.Rows[0].FindControl("TextBox1") as TextBox;
        //        if (txtPurchId != null)
        //            purchId = txtPurchId.Text.Trim();

        //        if (string.IsNullOrEmpty(purchId))
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "NoPO",
        //                "alert('Purchase Order ID is required.');", true);
        //            return;
        //        }

        //        // Prepare list of PO_ProductReceiptSvcContract
        //        List<PO_ProductReceiptSvcContract> lineList = new List<PO_ProductReceiptSvcContract>();
        //        foreach (GridViewRow row in gvLines.Rows)
        //        {
        //            TextBox txtLineNumber = row.FindControl("txtLineNumber") as TextBox;
        //            TextBox txtQuantity = row.FindControl("txtProductReceipts") as TextBox;

        //            if (txtLineNumber != null && txtQuantity != null)
        //            {
        //                long lineNumber = 0;
        //                long.TryParse(txtLineNumber.Text.Trim(), out lineNumber); // Convert to long

        //                PO_ProductReceiptSvcContract contract = new PO_ProductReceiptSvcContract
        //                {
        //                    OrigPurchId = purchId,
        //                    PurchaseLineLineNumber = lineNumber, // use long
        //                    Num = txtQuantity.Text.Trim()        // string
        //                };
        //                lineList.Add(contract);
        //            }
        //        }

        //        if (lineList.Count == 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "NoValidLines",
        //                "alert('No valid lines to post.');", true);
        //            return;
        //        }

        //        // Use first row's Product Receipt string
        //        string productReceiptNum = lineList[0].Num;

        //        // Call the service method
        //        PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();
        //        GeneralContract result = svc.makPurchPackingSlipPost(purchId, productReceiptNum, lineList);

        //        if (result != null && result.IsSuccess)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "Success",
        //                "alert('Product Receipt posted successfully!');", true);
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "Fail",
        //                $"alert('Error: {result?.Message ?? "Unknown error occurred."}');", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "Error",
        //            $"alert('Exception: {ex.Message}');", true);
        //    }
        //}

        //protected void btnOk_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (gvLines.Rows.Count == 0 || gvOverview.Rows.Count == 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "NoLines",
        //                "alert('No lines available to post.');", true);
        //            return;
        //        }

        //        // Get Purchase Order ID from first row
        //        string purchId = string.Empty;
        //        TextBox txtPurchId = gvLines.Rows[0].FindControl("TextBox1") as TextBox;
        //        if (txtPurchId != null)
        //            purchId = txtPurchId.Text.Trim();

        //        if (string.IsNullOrEmpty(purchId))
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "NoPO",
        //                "alert('Purchase Order ID is required.');", true);
        //            return;
        //        }

        //        // Prepare list of PO_ProductReceiptSvcContract
        //        List<PO_ProductReceiptSvcContract> lineList = new List<PO_ProductReceiptSvcContract>();
        //        int totalRows = Math.Min(gvLines.Rows.Count, gvOverview.Rows.Count);

        //        for (int i = 0; i < totalRows; i++)
        //        {
        //            // Product Receipt from gvOverview
        //            TextBox txtProductReceipts = gvOverview.Rows[i].FindControl("txtProductReceipts") as TextBox;

        //            // Line number from gvLines
        //            TextBox txtLineNumber = gvLines.Rows[i].FindControl("txtLineNumber") as TextBox;


        //            if (txtLineNumber != null && txtProductReceipts != null)
        //            {
        //                long lineNumber = 0;
        //                long.TryParse(txtLineNumber.Text.Trim(), out lineNumber); // Convert to long

        //                PO_ProductReceiptSvcContract contract = new PO_ProductReceiptSvcContract
        //                {
        //                    OrigPurchId = purchId,
        //                    PurchaseLineLineNumber = lineNumber,      // from gvLines
        //                    Num = txtProductReceipts.Text.Trim()      // from gvOverview
        //                };
        //                lineList.Add(contract);
        //            }
        //        }

        //        if (lineList.Count == 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "NoValidLines",
        //                "alert('No valid lines to post.');", true);
        //            return;
        //        }

        //        // Use first row's Product Receipt string
        //        string productReceiptNum = lineList[0].Num;

        //        // Call the service method
        //        PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();
        //        GeneralContract result = svc.makPurchPackingSlipPost(purchId, productReceiptNum, lineList);

        //        if (result != null && result.IsSuccess)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "Success",
        //                "alert('Product Receipt posted successfully!');", true);
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "Fail",
        //                $"alert('Error: {result?.Message ?? "Unknown error occurred."}');", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "Error",
        //            $"alert('Exception: {ex.Message}');", true);
        //    }
        //}

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if grids have rows
                if (gvLines.Rows.Count == 0 || gvOverview.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoLines",
                        "alert('No lines available to post.');", true);
                    return;
                }

                // Get Purchase Order ID from first row of gvLines
                string purchId = string.Empty;
                TextBox txtPurchId = gvLines.Rows[0].FindControl("TextBox1") as TextBox;
                if (txtPurchId != null)
                    purchId = txtPurchId.Text.Trim();

                if (string.IsNullOrEmpty(purchId))
                {
                    NotificationMessage.showMessage("Purchase Order ID is required.");

                    return;
                }

                // Prepare list of service contracts
                List<PO_ProductReceiptSvcContract> lineList = new List<PO_ProductReceiptSvcContract>();
                int totalRows = Math.Min(gvLines.Rows.Count, gvOverview.Rows.Count);

                TextBox txtDocumentDate = gvOverview.FindControl("txtDocumentDate") as TextBox;
                DropDownList ddlTermsOfPayment = gvOverview.FindControl("ddlTermsOfPayment") as DropDownList;

                string paymTermId = ddlTermsOfPayment?.SelectedValue ?? "";
                DateTime.TryParse(txtDocumentDate?.Text.Trim(), out DateTime documentDate);



                for (int i = 0; i < totalRows; i++)
                {
                    GridViewRow linesRow = gvLines.Rows[i];
                    GridViewRow overviewRow = gvOverview.Rows[i];

                    // Fetch hidden RecId from gvLines
                    Label hfRecId = linesRow.FindControl("hfRecId") as Label;

                    // Fetch fields from gvOverview
                    TextBox txtProductReceipts = overviewRow.FindControl("txtProductReceipts") as TextBox;


                    // Fetch fields from gvLines
                    TextBox txtLineNumber = linesRow.FindControl("txtLineNumber") as TextBox;
                    TextBox txtQuantity = linesRow.FindControl("txtQuantity") as TextBox;









                    if (hfRecId != null && txtProductReceipts != null && txtLineNumber != null && txtQuantity != null)
                    {
                        long lineNumber = 0;
                        long recId = 0;
                        decimal quantity = 0;


                        long.TryParse(txtLineNumber.Text.Trim(), out lineNumber);
                        long.TryParse(hfRecId.Text.Trim(), out recId);
                        decimal.TryParse(txtQuantity.Text.Trim(), out quantity);



                        PO_ProductReceiptSvcContract contract = new PO_ProductReceiptSvcContract
                        {
                            OrigPurchId = purchId,
                            PurchaseLineLineNumber = lineNumber,
                            RecId = recId,
                            ReceiveNow = quantity,
                            Num = txtProductReceipts.Text.Trim(),

                        };

                        lineList.Add(contract);
                    }
                }

                // Check if any valid lines found
                if (lineList.Count == 0)
                {
                    NotificationMessage.showMessage("No valid lines to post.");

                    return;
                }

                // Use first Product Receipt number
                string productReceiptNum = lineList[0].Num;

                // Call service
                PurchaseOrder_ProductsReceipt svc = new PurchaseOrder_ProductsReceipt();
                GeneralContract result = svc.makPurchPackingSlipPost(purchId, productReceiptNum, lineList, paymTermId, documentDate);
                SysOperationResult_BOL notificationResult = new SysOperationResult_BOL
                {
                    Message = result.Message,
                    isSuccess = result.IsSuccess,
                    AlertType = result.IsSuccess ? AlertType.Success.ToString() : AlertType.Error.ToString()
                };

                NotificationMessage.showMessage(notificationResult);

            }
            catch (Exception)
            {
                SysOperationResult_BOL errorResult = new SysOperationResult_BOL
                {
                    isSuccess = false
                };
                NotificationMessage.showMessage(errorResult);
            }

            //    if (result != null && result.IsSuccess)
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "Success",
            //            "alert('Product Receipt posted successfully!');", true);
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "Fail",
            //            $"alert('Error: {result?.Message ?? "Unknown error occurred."}');", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Error",
            //        $"alert('Exception: {ex.Message}');", true);
            //}
        }

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            // Get the checkbox the user clicked
            CheckBox clickedCheckbox = sender as CheckBox;

            if (clickedCheckbox == null)
                return;

            // Loop through all rows
            foreach (GridViewRow row in gvLines.Rows)
            {
                CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;

                // Uncheck all other checkboxes
                if (chk != null && chk != clickedCheckbox)
                {
                    chk.Checked = false;
                }
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Read the current DataTable stored in ViewState
            DataTable dt = ViewState["LinesDT"] as DataTable;
            if (dt == null || dt.Rows.Count == 0)
                return;

            // Loop through grid rows
            foreach (GridViewRow row in gvLines.Rows)
            {
                CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
                Label lblRecId = row.FindControl("hfRecId") as Label;

                if (chk != null && chk.Checked && lblRecId != null)
                {
                    string recId = lblRecId.Text;

                    // Remove selected row from DataTable
                    DataRow[] rowsToDelete = dt.Select("RecId = '" + recId + "'");
                    if (rowsToDelete.Length > 0)
                    {
                        dt.Rows.Remove(rowsToDelete[0]);
                    }
                }
            }

            // Finalize changes
            dt.AcceptChanges();

            // Save updated table back to ViewState
            ViewState["LinesDT"] = dt;

            // Bind updated DataTable (local only, NOT from backend)
            gvLines.DataSource = dt;
            gvLines.DataBind();
        }










        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ESS/PR/AllPurchaseOrder_ListPage.aspx");
        }

        protected void btnTotals_Click(object sender, EventArgs e)
        {
            bool isSelected = false;

            // Loop through Overview GridView rows
            foreach (GridViewRow row in gvOverview.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chkOverviewSelect") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    // Get the Purchase Order ID from the txtPurchaseOrder textbox
                    TextBox txtPurchaseOrder = row.FindControl("txtPurchaseOrder") as TextBox;
                    if (txtPurchaseOrder != null)
                    {
                        string purchIdTotal = txtPurchaseOrder.Text.Trim();
                        Session["purchIdTotal"] = purchIdTotal;

                        isSelected = true;

                        // Redirect to the Total page
                        string returnUrl = Server.UrlEncode(Request.Url.PathAndQuery);
                        Response.Redirect($"/ESS/PR/PurchaseOrder_Total.aspx?PurchId={purchIdTotal}&returnUrl={returnUrl}");
                        return; // Exit after redirect
                    }
                }
            }

            if (!isSelected)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNoSelection",
                    "alert('No record was selected. Please select a purchase order.');", true);
            }
        }

        protected void btnSalesTax_Click(object sender, EventArgs e)
        {
            bool isSelected = false;

            // Loop through Overview GridView rows
            foreach (GridViewRow row in gvOverview.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chkOverviewSelect") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    // Get the Purchase Order ID from the txtPurchaseOrder textbox
                    TextBox txtPurchaseOrder = row.FindControl("txtPurchaseOrder") as TextBox;
                    if (txtPurchaseOrder != null)
                    {
                        string SelectedPurchId = txtPurchaseOrder.Text.Trim();
                        Session["SelectedPurchId"] = SelectedPurchId;

                        isSelected = true;

                        // Redirect to SalesTax.aspx
                        string returnUrl = Server.UrlEncode(Request.Url.PathAndQuery);
                        Response.Redirect($"/ESS/PR/SalesTax.aspx?PurchId={SelectedPurchId}&returnUrl={returnUrl}");
                        return; // Exit after redirect
                    }
                }
            }

            if (!isSelected)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNoSelection",
                    "alert('Please select a Purchase Order first.');", true);
            }
        }



    }
}