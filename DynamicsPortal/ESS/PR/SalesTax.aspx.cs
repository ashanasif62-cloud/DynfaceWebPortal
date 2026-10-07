    using BussinessObject;
    using GeneralAuxiliary;
    using PortalIntegration;
    using PortalIntegration.PurchaseOrderLinesSvcReference;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    namespace DynamicsPortal.ESS.PR
    {
        public partial class SalesTax : ModalForm
        {
            protected override void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    // Set page menu ID
                    pageMenuId = "PurchaseOrder_SalesTax";

                    // Dynamically set page title in master page
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Sales Tax";
                        titleDiv.Style["font-weight"] = "600";
                        titleDiv.Style["font-size"] = "20px";
                        titleDiv.Style["color"] = "#000000";
                        titleDiv.Style["margin"] = "10px 0";
                    }

                // Get the selected Purchase Order ID from session
                string purchId = Request.QueryString["PurchId"];
                if (string.IsNullOrEmpty(purchId))
                    purchId = Session["SelectedPurchId"] as string;

                long recId = 0;
                long.TryParse(Request.QueryString["RecId"], out recId);

                // Persist for postbacks (checkbox clicks etc.)
                Session["SalesTaxRecId"] = recId;
                Session["SelectedPurchId"] = purchId;


                if (!string.IsNullOrEmpty(purchId))
                    {
                        LoadPurchaseOrder(purchId);
                    }
                    else
                    {
                        NotificationMessage.showMessage(
                            AlertType.Error,
                            "Purchase Order not found. Please select again.");
                    }
                }
            }

        //private void LoadPurchaseOrder(string purchId)
        //{
        //    try
        //    {
        //        // Create a safe DataTable structure
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("TaxCode");
        //        dt.Columns.Add("Percent", typeof(decimal));
        //        dt.Columns.Add("Quantity", typeof(decimal));
        //        dt.Columns.Add("AmountOrigin", typeof(decimal));
        //        dt.Columns.Add("ActualSalesTaxAmount", typeof(decimal));
        //        dt.Columns.Add("SalesTaxDirection");
        //        dt.Columns.Add("Txt"); // optional columns
        //        dt.Columns.Add("ExemptCode");
        //        dt.Columns.Add("Company");
        //        dt.Columns.Add("SourceCurrencyCode");
        //        dt.Columns.Add("AdjustedAmountOrigin");
        //        dt.Columns.Add("CalculatedNonDeductibleSalesTax");
        //        dt.Columns.Add("CalculatedSalesTaxAmount");
        //        dt.Columns.Add("ActualNondeductibleSalesTax");
        //        dt.Columns.Add("AccountingCurrencyCode");
        //        dt.Columns.Add("AccountingAmountOrigin");
        //        dt.Columns.Add("AccountingActualNondeductibleSalesTax");
        //        dt.Columns.Add("TaxAmount");
        //        dt.Columns.Add("OverrideCalculatedSalesTax");

        //        if (!string.IsNullOrEmpty(purchId))
        //        {
        //            PurchaseOrderHeader svc = new PurchaseOrderHeader();
        //            DataTable result = svc.retrieveSalesTax(purchId);

        //            if (result != null && result.Rows.Count > 0)
        //            {
        //                dt = result;
        //            }
        //            else
        //            {
        //                NotificationMessage.showMessage(
        //                    AlertType.Error,
        //                    "No sales tax data found for this purchase order.");
        //            }
        //        }

        //        gvSalesTaxOverview.DataSource = dt;
        //        gvSalesTaxOverview.DataBind();

        //        // Store in ViewState for postbacks
        //        ViewState["SalesTaxData"] = dt;

        //        // Populate tabs with first row if exists
        //        if (dt.Rows.Count > 0)
        //            PopulateTabs(dt.Rows[0]);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Error loading sales tax: " + ex.Message);
        //        gvSalesTaxOverview.DataSource = new DataTable();
        //        gvSalesTaxOverview.DataBind();
        //    }
        //}

        //new
        //private void LoadPurchaseOrder(string purchId)
        //{
        //    try
        //    {
        //        // Get RecId from session
        //        long recId = Session["RecId"] != null ? Convert.ToInt64(Session["RecId"]) : 0;

        //        // Create empty DataTable with all necessary columns
        //        DataTable dt = createTaxDataTable();

        //        if (!string.IsNullOrEmpty(purchId))
        //        {
        //            if (recId == 0)
        //            {
        //                // Header-level
        //                PurchaseOrderHeader headerSvc = new PurchaseOrderHeader();
        //                DataTable result = headerSvc.retrieveSalesTax(purchId);

        //                if (result != null && result.Rows.Count > 0)
        //                    dt = result;
        //                else
        //                    NotificationMessage.showMessage(AlertType.Error, "No sales tax data found for this purchase order.");
        //            }
        //            else
        //            {
        //                // Line-level
        //                PurchaseOrderLines linesSvc = new PurchaseOrderLines();
        //                DataTable result = linesSvc.retrieveSalesTax(purchId, recId);

        //                if (result != null && result.Rows.Count > 0)
        //                    dt = result;
        //                else
        //                    NotificationMessage.showMessage(AlertType.Error, "No sales tax data found for this purchase order line.");
        //            }
        //        }

        //        // Bind GridView
        //        gvSalesTaxOverview.DataSource = dt;
        //        gvSalesTaxOverview.DataBind();

        //        // Store in ViewState
        //        ViewState["SalesTaxData"] = dt;

        //        // Populate tabs with first row
        //        if (dt.Rows.Count > 0)
        //            PopulateTabs(dt.Rows[0]);
        //    }

        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Error loading sales tax: " + ex.Message);
        //        gvSalesTaxOverview.DataSource = createTaxDataTable();
        //        gvSalesTaxOverview.DataBind();
        //    }
        //}

        //private void LoadPurchaseOrder(string purchId)
        //{
        //    try
        //    {
        //        long recId = Session["RecId"] != null ? Convert.ToInt64(Session["RecId"]) : 0;

        //        DataTable dt = createTaxDataTable();

        //        if (!string.IsNullOrEmpty(purchId))
        //        {
        //            if (recId == 0)
        //            {
        //                // Header-level
        //                PurchaseOrderHeader headerSvc = new PurchaseOrderHeader();
        //                DataTable result = headerSvc.retrieveSalesTax(purchId);
        //                if (result != null && result.Rows.Count > 0) dt = result;
        //                else NotificationMessage.showMessage(AlertType.Error, "No sales tax data found for this purchase order.");
        //            }
        //            else
        //            {
        //                // Line-level
        //                PurchaseOrderLines linesSvc = new PurchaseOrderLines();
        //                DataTable result = linesSvc.retrieveSalesTax(purchId, recId);
        //                if (result != null && result.Rows.Count > 0) dt = result;
        //                else NotificationMessage.showMessage(AlertType.Error, "No sales tax data found for this purchase order line.");
        //            }
        //        }

        //        // Format numeric columns before binding
        //        // Format numeric columns before binding GridView
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            // AmountOrigin: 2 decimal places
        //            row["AmountOrigin"] = row["AmountOrigin"] != DBNull.Value
        //                ? Convert.ToDecimal(row["AmountOrigin"]).ToString("0.00")
        //                : "0.00";

        //            // Percent: 4 decimal places
        //            row["Percent"] = row["Percent"] != DBNull.Value
        //                ? Convert.ToDecimal(row["Percent"]).ToString("0.0000")
        //                : "0.0000";

        //            // ActualSalesTaxAmount: 2 decimal places
        //            row["ActualSalesTaxAmount"] = row["ActualSalesTaxAmount"] != DBNull.Value
        //                ? Convert.ToDecimal(row["ActualSalesTaxAmount"]).ToString("0.00")
        //                : "0.00";

        //            // Quantity: keep 2 decimal places
        //            row["Quantity"] = row["Quantity"] != DBNull.Value
        //                ? Convert.ToDecimal(row["Quantity"]).ToString("0.00")
        //                : "0.00";
        //        }


        //        gvSalesTaxOverview.DataSource = dt;
        //        gvSalesTaxOverview.DataBind();

        //        ViewState["SalesTaxData"] = dt;

        //        if (dt.Rows.Count > 0) PopulateTabs(dt.Rows[0]);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Error loading sales tax: " + ex.Message);
        //        gvSalesTaxOverview.DataSource = createTaxDataTable();
        //        gvSalesTaxOverview.DataBind();
        //    }
        //}

        // Create empty DataTable structure
        private void LoadPurchaseOrder(string purchId)
        {
            try
            {
                long recId = Session["SalesTaxRecId"] != null
                    ? Convert.ToInt64(Session["SalesTaxRecId"])
                    : 0;

                DataTable dt = createTaxDataTable();

                if (!string.IsNullOrEmpty(purchId))
                {
                    if (recId > 0)
                    {
                        // 🔹 LINE LEVEL SALES TAX
                        PurchaseOrderLines linesSvc = new PurchaseOrderLines();
                        DataTable result = linesSvc.retrieveSalesTax(purchId, recId);

                        if (result != null && result.Rows.Count > 0)
                        {
                            dt = result;
                        }
                        else
                        {
                            NotificationMessage.showMessage(
                                AlertType.Error,
                                "No sales tax data found for this purchase order line.");
                        }
                    }
                    else
                    {
                        // 🔹 HEADER LEVEL SALES TAX
                        PurchaseOrderHeader headerSvc = new PurchaseOrderHeader();
                        DataTable result = headerSvc.retrieveSalesTax(purchId);

                        if (result != null && result.Rows.Count > 0)
                        {
                            dt = result;
                        }
                        else
                        {
                            NotificationMessage.showMessage(
                                AlertType.Error,
                                "No sales tax data found for this purchase order.");
                        }
                    }
                }

                // 🔹 FORMAT DATA
                FormatSalesTaxData(dt);

                gvSalesTaxOverview.DataSource = dt;
                gvSalesTaxOverview.DataBind();

                ViewState["SalesTaxData"] = dt;

                if (dt.Rows.Count > 0)
                    PopulateTabs(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading sales tax: " + ex.Message);
                gvSalesTaxOverview.DataSource = createTaxDataTable();
                gvSalesTaxOverview.DataBind();
            }
        }

        private void FormatSalesTaxData(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                row["AmountOrigin"] = row["AmountOrigin"] != DBNull.Value
                    ? Convert.ToDecimal(row["AmountOrigin"]).ToString("0.00")
                    : "0.00";

                row["Percent"] = row["Percent"] != DBNull.Value
                    ? Convert.ToDecimal(row["Percent"]).ToString("0.0000")
                    : "0.0000";

                row["ActualSalesTaxAmount"] = row["ActualSalesTaxAmount"] != DBNull.Value
                    ? Convert.ToDecimal(row["ActualSalesTaxAmount"]).ToString("0.00")
                    : "0.00";

                row["Quantity"] = row["Quantity"] != DBNull.Value
                    ? Convert.ToDecimal(row["Quantity"]).ToString("0.00")
                    : "0.00";
            }
        }


        private DataTable createTaxDataTable()
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("TaxCode");
                dt.Columns.Add("Percent", typeof(decimal));
                dt.Columns.Add("Quantity", typeof(decimal));
                dt.Columns.Add("AmountOrigin", typeof(decimal));
                dt.Columns.Add("ActualSalesTaxAmount", typeof(decimal));
                dt.Columns.Add("SalesTaxDirection");
                dt.Columns.Add("Txt");
                dt.Columns.Add("ExemptCode");
                dt.Columns.Add("Company");
                dt.Columns.Add("SourceCurrencyCode");
                dt.Columns.Add("AdjustedAmountOrigin");
                dt.Columns.Add("CalculatedNonDeductibleSalesTax");
                dt.Columns.Add("CalculatedSalesTaxAmount");
                dt.Columns.Add("ActualNondeductibleSalesTax");
                dt.Columns.Add("AccountingCurrencyCode");
                dt.Columns.Add("AccountingAmountOrigin");
                dt.Columns.Add("AccountingActualNondeductibleSalesTax");
                dt.Columns.Add("TaxAmount");
                dt.Columns.Add("OverrideCalculatedSalesTax");
                return dt;
            }

            // Populate the tab fields with a given DataRow
            private void PopulateTabs(DataRow row)
            {
                if (row == null) return;

                txtGeneralSalesTaxCode.Text = row["TaxCode"]?.ToString();
                txtDescription.Text = row.Table.Columns.Contains("Txt") ? row["Txt"]?.ToString() : "";
                txtExemptCode.Text = row.Table.Columns.Contains("ExcemptCode") ? row["ExcemptCode"]?.ToString() : "";
                txtLegalEntity.Text = row.Table.Columns.Contains("Company") ? row["Company"]?.ToString() : "";

                txtCurrency.Text = row.Table.Columns.Contains("SourceCurrencyCode") ? row["SourceCurrencyCode"]?.ToString() : "";
            txtAdjustedAmountOrigin.Text = row.Table.Columns.Contains("AdjustedAmountOrigin")
  ? (row["AdjustedAmountOrigin"] != DBNull.Value
      ? Convert.ToDecimal(row["AdjustedAmountOrigin"]).ToString("0.00")
      : "0.00")
  : "0.00";

            txtAmountOrigin_AmountTab.Text = row.Table.Columns.Contains("AmountOrigin") ? row["AmountOrigin"]?.ToString() : "";

            txtActualNonDeductible.Text = row.Table.Columns.Contains("ActualNondeductibleSalesTax")
                ? (row["ActualNondeductibleSalesTax"] != DBNull.Value ? Convert.ToDecimal(row["ActualNondeductibleSalesTax"]).ToString("0.00") : "0.00")
                : "0.00";

            txtCalcNonDeductible.Text = row.Table.Columns.Contains("CalculatedNonDeductibleSalesTax")
                ? (row["CalculatedNonDeductibleSalesTax"] != DBNull.Value ? Convert.ToDecimal(row["CalculatedNonDeductibleSalesTax"]).ToString("0.00") : "0.00")
                : "0.00";
            txtCalculatedSalesTaxAmount.Text = row.Table.Columns.Contains("CalculatedSalesTaxAmount")
        ? (row["CalculatedSalesTaxAmount"] != DBNull.Value ? Convert.ToDecimal(row["CalculatedSalesTaxAmount"]).ToString("0.00") : "0.00")
        : "0.00";

            txtActualSalesTaxAmount_AmountTab.Text = row.Table.Columns.Contains("ActualSalesTaxAmount")
       ? (row["ActualSalesTaxAmount"] != DBNull.Value ? Convert.ToDecimal(row["ActualSalesTaxAmount"]).ToString("0.00") : "0.00")
       : "0.00";

            txtAccountingCurrency.Text = row.Table.Columns.Contains("AccountingCurrencyCode") ? row["AccountingCurrencyCode"]?.ToString() : "";
            txtAcctAmountOrigin.Text = row.Table.Columns.Contains("AccountingAmountOrigin")
                 ? (row["AccountingAmountOrigin"] != DBNull.Value ? Convert.ToDecimal(row["AccountingAmountOrigin"]).ToString("0.00") : "0.00")
                 : "0.00";

            txtAcctActualNonDeductible.Text = row.Table.Columns.Contains("AccountingActualNondeductibleSalesTax")
         ? (row["AccountingActualNondeductibleSalesTax"] != DBNull.Value ? Convert.ToDecimal(row["AccountingActualNondeductibleSalesTax"]).ToString("0.00") : "0.00")
         : "0.00";
            txtAcctActualSalesTaxAmount.Text = row.Table.Columns.Contains("TaxAmount")
        ? (row["TaxAmount"] != DBNull.Value ? Convert.ToDecimal(row["TaxAmount"]).ToString("0.00") : "0.00")
        : "0.00";

            if (row.Table.Columns.Contains("OverrideCalculatedSalesTax"))
                {
                    bool.TryParse(row["OverrideCalculatedSalesTax"]?.ToString(), out bool flag);
                    chkOverrideSalesTax.Checked = flag;
                }
            }

            protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
            {
                try
                {
                    CheckBox chk = sender as CheckBox;
                    if (chk == null) return;

                    GridViewRow row = chk.NamingContainer as GridViewRow;
                    if (row == null) return;

                    // Uncheck all other checkboxes
                    foreach (GridViewRow gvRow in gvSalesTaxOverview.Rows)
                    {
                        CheckBox otherChk = gvRow.FindControl("chk_SelectSingle") as CheckBox;
                        if (otherChk != null)
                        {
                            otherChk.Checked = gvRow == row;
                            gvRow.BackColor = gvRow == row && chk.Checked ? System.Drawing.Color.LightYellow : System.Drawing.Color.Transparent;
                        }
                    }

                    // Get DataTable from ViewState
                    DataTable dt = ViewState["SalesTaxData"] as DataTable;
                    if (dt != null && row.RowIndex < dt.Rows.Count)
                    {
                        DataRow selectedRow = dt.Rows[row.RowIndex];
                        PopulateTabs(selectedRow);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error in chk_SelectSingle_CheckedChanged: " + ex.Message);
                }
            }

            protected void btnOK_Click(object sender, EventArgs e)
            {
                // Check if returnUrl exists in query string
                string returnUrl = Request.QueryString["returnUrl"];

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    // Redirect back to the page that opened Sales Tax
                    Response.Redirect(Server.UrlDecode(returnUrl));
                }
                else
                {
                    // Default fallback: All Purchase Orders
                    Response.Redirect("AllPurchaseOrders.aspx");
                }
            }

        }
    }
