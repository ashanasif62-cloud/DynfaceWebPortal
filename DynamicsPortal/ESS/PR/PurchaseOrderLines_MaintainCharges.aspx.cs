using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PurchaseOrderLines_MaintainCharges;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_MaintainCharges_ListPage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            string purchaseOrderId = Session["PurchaseOrderId"]?.ToString();
            if (!IsPostBack)
            {
                Page.Title = "Purchase Order Maintain Charges";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Maintain Charges"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }

                if (Session["RecId"] != null)
                {
                    long recId = Convert.ToInt64(Session["RecId"]);

                    // ✅ Step 2: Call method to bind service data
                    BindChargesGrid(recId);
                   
                }
                else
                {
                    BindEmptyGrid();
                }// Show grid headers with no data
            }
        }


        /// <summary>
        /// Binds an empty DataTable to GridView so only headers are displayed.
        /// </summary>
        /// 
        private void BindChargesGrid(long recId)
        {
            try
            {
                // ✅ Create instance of your integration/service class
                PurchaseOrderLine_MaintainCharges svc = new PurchaseOrderLine_MaintainCharges();

                // ✅ Retrieve data from service based on RecId
                DataTable dt = svc.retrieveAll(recId);

                // ✅ If data returned, bind it — otherwise show empty grid
                if (dt != null && dt.Rows.Count > 0)
                {
                    gridView.DataSource = dt;
                    gridView.DataBind();
                }
                else
                {
                    BindEmptyGrid();
                    //ScriptManager.RegisterStartupScript(this, GetType(), "NoData",
                    //    "alert('No charges found for the selected record.');", true);
                }
            }
            catch (Exception ex)
            {
                BindEmptyGrid();
                System.Diagnostics.Debug.WriteLine("Error binding charges grid: " + ex.Message);
            }
        }

        private void BindEmptyGrid()
        {
            DataTable dt = new DataTable();

            // Use same column names as in GridView (and service result)
            dt.Columns.Add("MarkupCode");
            dt.Columns.Add("Txt");
            dt.Columns.Add("MarkupCategory");
            dt.Columns.Add("SpecificUnitSymbol");
            dt.Columns.Add("Value");
            dt.Columns.Add("AllowEdit");
            dt.Columns.Add("CurrencyCode");
            dt.Columns.Add("CalculatedAmount");
            dt.Columns.Add("MCRBrokerContractFee");
            dt.Columns.Add("TaxGroup");
            dt.Columns.Add("TaxItemGroup");
            dt.Columns.Add("RecId");



            gridView.DataSource = dt;
            gridView.DataBind();
        }





        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlChargesCode = (DropDownList)e.Row.FindControl("ddlChargesCode");
                PurchaseOrderLine_MaintainCharges lines = new PurchaseOrderLine_MaintainCharges();

                if (ddlChargesCode != null)
                {
                    // Load all markup codes initially
                    ddlChargesCode.DataSource = lines.retrieveMarkupCode();
                    ddlChargesCode.DataTextField = "MarkupCode";
                    ddlChargesCode.DataValueField = "MarkupCode";
                    ddlChargesCode.DataBind();

                    ddlChargesCode.Items.Insert(0, new ListItem());

                    string currentValue = DataBinder.Eval(e.Row.DataItem, "MarkupCode")?.ToString();
                    if (!string.IsNullOrEmpty(currentValue) && ddlChargesCode.Items.FindByValue(currentValue) != null)
                    {
                        ddlChargesCode.SelectedValue = currentValue;
                    }
                }

                DropDownList ddlUnit = (DropDownList)e.Row.FindControl("ddlUnit");
                if (ddlUnit != null)
                {
                    ddlUnit.DataSource = lines.retrieveUnit();     // Call your service method
                    ddlUnit.DataTextField = "SpecificUnitSymbol";  // Text to display
                    ddlUnit.DataValueField = "SpecificUnitSymbol"; // Value field
                    ddlUnit.DataBind();
                    ddlUnit.Items.Insert(0, new ListItem());

                    string currentUnit = DataBinder.Eval(e.Row.DataItem, "SpecificUnitSymbol")?.ToString();
                    if (!string.IsNullOrEmpty(currentUnit) && ddlUnit.Items.FindByValue(currentUnit) != null)
                    {
                        ddlUnit.SelectedValue = currentUnit;
                    }
                }

                DropDownList ddlSalesTaxGroup = (DropDownList)e.Row.FindControl("ddlSalesTaxGroup");
                if (ddlSalesTaxGroup != null)
                {
                    ddlSalesTaxGroup.DataSource = lines.retrieveSalesTaxGroup();
                    ddlSalesTaxGroup.DataTextField = "TaxGroup";   // Column name from DataTable
                    ddlSalesTaxGroup.DataValueField = "TaxGroup";  // Same as text field
                    ddlSalesTaxGroup.DataBind();
                    ddlSalesTaxGroup.Items.Insert(0, new ListItem());

                    string currentTaxGroup = DataBinder.Eval(e.Row.DataItem, "TaxGroup")?.ToString();
                    if (!string.IsNullOrEmpty(currentTaxGroup) && ddlSalesTaxGroup.Items.FindByValue(currentTaxGroup) != null)
                    {
                        ddlSalesTaxGroup.SelectedValue = currentTaxGroup;
                    }
                }

                // ✅ Item Sales Tax Group dropdown
                DropDownList ddlItemSalesTaxGroup = (DropDownList)e.Row.FindControl("ddlItemSalesTaxGroup");
                if (ddlItemSalesTaxGroup != null)
                {
                    ddlItemSalesTaxGroup.DataSource = lines.retrieveItemSalesTaxGroup();
                    ddlItemSalesTaxGroup.DataTextField = "TaxItemGroup";   // Column name from DataTable
                    ddlItemSalesTaxGroup.DataValueField = "TaxItemGroup";  // Same as text field
                    ddlItemSalesTaxGroup.DataBind();
                    ddlItemSalesTaxGroup.Items.Insert(0, new ListItem());

                    string currentItemTaxGroup = DataBinder.Eval(e.Row.DataItem, "TaxItemGroup")?.ToString();
                    if (!string.IsNullOrEmpty(currentItemTaxGroup) && ddlItemSalesTaxGroup.Items.FindByValue(currentItemTaxGroup) != null)
                    {
                        ddlItemSalesTaxGroup.SelectedValue = currentItemTaxGroup;
                    }
                }

                DropDownList ddlCategory = (DropDownList)e.Row.FindControl("ddlCategory");
                if (ddlCategory != null)
                {
                    // Manually add the static list of options
                    ddlCategory.Items.Clear();
                    ddlCategory.Items.Add(new ListItem("Fixed", "Fixed"));
                    ddlCategory.Items.Add(new ListItem("Pcs.", "Pcs."));
                    ddlCategory.Items.Add(new ListItem("Percent", "Percent"));
                    ddlCategory.Items.Add(new ListItem("Intercompany percent", "Intercompany percent"));
                    ddlCategory.Items.Add(new ListItem("External", "External"));
                    ddlCategory.Items.Add(new ListItem("Proportional", "Proportional"));
                    ddlCategory.Items.Add(new ListItem("Specific unit", "Specific unit"));
                    ddlCategory.Items.Add(new ListItem("Specific unit match", "Specific unit match"));

                    // Insert empty option at top (optional)
                    ddlCategory.Items.Insert(0, new ListItem());

                    // Bind the current value from your GridView row
                    string currentCategory = DataBinder.Eval(e.Row.DataItem, "MarkupCategory")?.ToString();
                    if (!string.IsNullOrEmpty(currentCategory) && ddlCategory.Items.FindByValue(currentCategory) != null)
                    {
                        ddlCategory.SelectedValue = currentCategory;
                    }
                }


                DropDownList ddlCurrency = (DropDownList)e.Row.FindControl("ddlCurrency");
                if (ddlCurrency != null)
                {
                    DataTable dtCurrency = ControlsHelper.retrieveAllCurrencyDetails();

                    ddlCurrency.DataSource = dtCurrency;
                    ddlCurrency.DataTextField = "CurrencyCode";
                    ddlCurrency.DataValueField = "CurrencyCode";
                    ddlCurrency.DataBind();

                    // Bind the current value from your GridView row
                    string currentCurrency = DataBinder.Eval(e.Row.DataItem, "CurrencyCode")?.ToString();

                    if (!string.IsNullOrEmpty(currentCurrency) && ddlCurrency.Items.FindByValue(currentCurrency) != null)
                    {
                        ddlCurrency.SelectedValue = currentCurrency;
                    }
                    else if (ddlCurrency.Items.FindByValue("USD") != null)
                    {
                        // Set default currency to USD if no value is found
                        ddlCurrency.SelectedValue = "USD";
                    }
                }
            }
        }

        protected void ddlChargesCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl = (DropDownList)sender;
                GridViewRow row = (GridViewRow)ddl.NamingContainer;
                TextBox txtDescription = (TextBox)row.FindControl("txtDescription");

                string selectedMarkupCode = ddl.SelectedValue;

                if (!string.IsNullOrEmpty(selectedMarkupCode))
                {
                    PurchaseOrderLine_MaintainCharges service = new PurchaseOrderLine_MaintainCharges();
                    DataTable dt = service.retrieveMarkupCodeDescription(selectedMarkupCode);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        txtDescription.Text = dt.Rows[0]["Txt"].ToString(); // assuming your service returns "Txt" column
                    }
                    else
                    {
                        txtDescription.Text = string.Empty;
                    }
                }
                else
                {
                    txtDescription.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Optional: log error
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
        }

      

        protected void btnNew_Click(object sender, EventArgs e)
        {

            string purchaseOrderId = Session["PurchaseOrderId"]?.ToString();
            PurchaseOrderLine_MaintainCharges svc = new PurchaseOrderLine_MaintainCharges();
            long recId = Session["RecId"] != null ? Convert.ToInt64(Session["RecId"]) : 0;

            DataTable dt = svc.retrieveAll(recId);

            // 2. Add a new blank row at the top
            DataRow newRow = dt.NewRow();
            dt.Rows.InsertAt(newRow, 0);

            // 3. Put GridView in edit mode for the first row
            gridView.EditIndex = 0;

            // 5. Rebind the GridView
            gridView.DataSource = dt;
            gridView.DataBind();

            // Mark this as Add mode


        }

       
        private DataTable BuildChargesDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("MarkupCode");
            dt.Columns.Add("Txt");
            dt.Columns.Add("MarkupCategory");
            dt.Columns.Add("SpecificUnitSymbol");
            dt.Columns.Add("Value");
            dt.Columns.Add("AllowEdit");
            dt.Columns.Add("CurrencyCode");
            dt.Columns.Add("CalculatedAmount");
            dt.Columns.Add("MCRBrokerContractFee");
            dt.Columns.Add("TaxGroup");
            dt.Columns.Add("TaxItemGroup");
            dt.Columns.Add("RecId");

            foreach (GridViewRow row in gridView.Rows)
            {
                // Skip header/footer rows
                if (row.RowType != DataControlRowType.DataRow)
                    continue;

                DataRow dr = dt.NewRow();

                dr["MarkupCode"] =
                    ((DropDownList)row.FindControl("ddlChargesCode"))?.SelectedValue ?? "";

                dr["Txt"] =
                    ((TextBox)row.FindControl("txtDescription"))?.Text ?? "";

                dr["MarkupCategory"] =
                    ((DropDownList)row.FindControl("ddlCategory"))?.SelectedValue ?? "";

                // Fixed as per your logic
                dr["SpecificUnitSymbol"] = "day";

                dr["Value"] =
                    ((TextBox)row.FindControl("txtValue"))?.Text ?? "0";

                dr["AllowEdit"] =
                    ((TextBox)row.FindControl("txtAllowEdit"))?.Text ?? "Yes";

                dr["CurrencyCode"] =
                    ((DropDownList)row.FindControl("ddlCurrency"))?.SelectedValue ?? "";

                dr["CalculatedAmount"] =
                    ((TextBox)row.FindControl("txtCalculatedAmount"))?.Text ?? "0.00";

                dr["MCRBrokerContractFee"] =
                    ((Label)row.FindControl("lblBrokerContractFee"))?.Text ?? "0";

                dr["TaxGroup"] =
                    ((DropDownList)row.FindControl("ddlSalesTaxGroup"))?.SelectedValue ?? "";

                dr["TaxItemGroup"] =
                    ((DropDownList)row.FindControl("ddlItemSalesTaxGroup"))?.SelectedValue ?? "";

                Label lblRecId = row.FindControl("lblRecId") as Label;
                dr["RecId"] = lblRecId != null ? lblRecId.Text : "0";

                dt.Rows.Add(dr);
            }

            return dt;
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string purchaseOrderId = Session["PurchaseOrderId"]?.ToString();
                long recId = Session["RecId"] != null
                    ? Convert.ToInt64(Session["RecId"])
                    : 0;

                string source = Session["MaintainChargesSource"]?.ToString() ?? "LINE";

                DataTable dt = BuildChargesDataTable();

                SysOperationResult_BOL result;

                if (source == "HEADER")
                {
                    // ✅ HEADER SERVICE
                    PurchaseOrderHeader_MaintainCharges headerSvc =
                        new PurchaseOrderHeader_MaintainCharges();

                    result = headerSvc.create(dt, purchaseOrderId, recId);
                }
                else
                {
                    // ✅ LINE SERVICE
                    PurchaseOrderLine_MaintainCharges lineSvc =
                        new PurchaseOrderLine_MaintainCharges();

                    result = lineSvc.create(dt, purchaseOrderId, recId);
                }

                NotificationMessage.showMessage(result);

//                if (result != null && result.isSuccess)
//                {
//                    // ✅ Refresh THIS page grid after short delay
//                    string script = @"
//setTimeout(function () {
//    __doPostBack('', '');
//}, 2000);";

//                    ScriptManager.RegisterStartupScript(
//                        this,
//                        this.GetType(),
//                        "RefreshPage",
//                        script,
//                        true
//                    );
//                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
        }

        //protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox chk = (CheckBox)sender;
        //    GridViewRow row = (GridViewRow)chk.NamingContainer;

        //    // Get RecId from DataKeys
        //    long recId = Convert.ToInt64(gridView.DataKeys[row.RowIndex].Value);
        //    // Uncheck all other checkboxes
        //    foreach (GridViewRow gvRow in gridView.Rows)
        //    {
        //        if (gvRow.RowIndex != row.RowIndex)
        //        {
        //            CheckBox otherChk = gvRow.FindControl("chk_SelectSingle") as CheckBox;
        //            if (otherChk != null)
        //                otherChk.Checked = false;
        //        }
        //    }

        //    // Store selected RecId in session (or hidden field)
        //    if (chk.Checked)
        //    {
        //        Session["SelectedRecId"] = recId;
        //    }
        //    else
        //    {
        //        Session.Remove("SelectedRecId");
        //    }
        //}

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;

            // Safely get RecId from DataKeys (may be DBNull for a newly-inserted, unsaved row)
            object keyValue = gridView.DataKeys[row.RowIndex].Value;

            if (keyValue == null || keyValue == DBNull.Value || !long.TryParse(keyValue.ToString(), out long recId))
            {
                // New/unsaved row has no RecId yet — just uncheck others and skip session storage
                recId = 0;
            }

            // Uncheck all other checkboxes
            foreach (GridViewRow gvRow in gridView.Rows)
            {
                if (gvRow.RowIndex != row.RowIndex)
                {
                    CheckBox otherChk = gvRow.FindControl("chk_SelectSingle") as CheckBox;
                    if (otherChk != null)
                        otherChk.Checked = false;
                }
            }

            // Store selected RecId in session only if it's a real, saved record
            if (chk.Checked && recId > 0)
            {
                Session["SelectedRecId"] = recId;
            }
            else
            {
                Session.Remove("SelectedRecId");
            }
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Step 1: Retrieve selected RecId from Session
                if (Session["SelectedRecId"] == null)
                {
                    SysOperationResult_BOL noSelectionResult = new SysOperationResult_BOL
                    {
                        isSuccess = false
                    };
                    NotificationMessage.showMessage(noSelectionResult);
                    return;
                }

                long recId = Convert.ToInt64(Session["SelectedRecId"]);

                // Step 2: Call delete service
                PurchaseOrderLine_MaintainCharges svc = new PurchaseOrderLine_MaintainCharges();
                SysOperationResult_BOL result = svc.delete(new long[] { recId });

                // Step 3: Show notification
                NotificationMessage.showMessage(result);

                // Step 4: Refresh Grid if delete succeeded
                if (result != null && result.isSuccess)
                {
                    Session.Remove("SelectedRecId");

                    if (Session["RecId"] != null)
                    {
                        long chargesRecId = Convert.ToInt64(Session["RecId"]);
                        BindChargesGrid(chargesRecId);
                    }
                }
            }
            catch (Exception)
            {
                SysOperationResult_BOL errorResult = new SysOperationResult_BOL
                {
                    isSuccess = false
                };
                NotificationMessage.showMessage(errorResult);
            }
        }


      

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            long selectedRecId = 0;

            // Find selected row
            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    Label lblRecId = row.FindControl("lblRecId") as Label;
                    if (lblRecId != null)
                    {
                        Int64.TryParse(lblRecId.Text, out selectedRecId);
                        break; // Only first selected row
                    }
                }
            }

            if (selectedRecId > 0)
            {
                long recIdPurchLine = Session["RecId"] != null ? Convert.ToInt64(Session["RecId"]) : 0;
                PurchaseOrderLine_MaintainCharges service = new PurchaseOrderLine_MaintainCharges();

                // Get updated calculated amount from service
                DataTable dt = service.retrieveCalculatedAmount(selectedRecId, recIdPurchLine);

                // Update the CalculatedAmount textbox in the same row
                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chkSelectRow = row.FindControl("chk_SelectSingle") as CheckBox;
                    if (chkSelectRow != null && chkSelectRow.Checked)
                    {
                        TextBox txtCalculatedAmount = row.FindControl("txtCalculatedAmount") as TextBox;
                        if (txtCalculatedAmount != null && dt != null && dt.Rows.Count > 0)
                        {
                            object value = dt.Rows[0]["CalculatedAmount"];
                            decimal calcAmount = (value != DBNull.Value) ? Convert.ToDecimal(value) : 0;
                            txtCalculatedAmount.Text = calcAmount.ToString("F2");
                        }
                        break;
                    }
                }
            }
        }


        //        protected void Update_Click(object sender, EventArgs e)
        //        {
        //            LinkButton updateButton = (LinkButton)sender;
        //            GridViewRow selectedRow = (GridViewRow)updateButton.NamingContainer;
        //            try
        //            {
        //                Label lblRecId = selectedRow.FindControl("lblRecId") as Label;
        //                long selectedRecId = 0;
        //                if (lblRecId != null)
        //                {
        //                    Int64.TryParse(lblRecId.Text, out selectedRecId);
        //                }

        //                if (selectedRecId <= 0)
        //                {
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "NoRecId",
        //                        "alert('RecId not found for the selected row.');", true);
        //                    return;
        //                }

        //                // Create DataTable with only the selected row for update
        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("MarkupCode");
        //                dt.Columns.Add("Txt");
        //                dt.Columns.Add("MarkupCategory");
        //                dt.Columns.Add("SpecificUnitSymbol");
        //                dt.Columns.Add("Value");
        //                dt.Columns.Add("AllowEdit");
        //                dt.Columns.Add("CurrencyCode");
        //                dt.Columns.Add("CalculatedAmount");
        //                dt.Columns.Add("MCRBrokerContractFee");
        //                dt.Columns.Add("TaxGroup");
        //                dt.Columns.Add("TaxItemGroup");
        //                dt.Columns.Add("RecId"); // Important for update

        //                DataRow dr = dt.NewRow();
        //                dr["MarkupCode"] = ((DropDownList)selectedRow.FindControl("ddlChargesCode"))?.SelectedValue ?? "";
        //                dr["Txt"] = ((TextBox)selectedRow.FindControl("txtDescription"))?.Text ?? "";
        //                dr["MarkupCategory"] = ((DropDownList)selectedRow.FindControl("ddlCategory"))?.SelectedValue ?? "";
        //                // dr["SpecificUnitSymbol"] = ((DropDownList)selectedRow.FindControl("ddlUnit"))?.SelectedValue ?? "";
        //                dr["SpecificUnitSymbol"] = "day";
        //                dr["Value"] = ((TextBox)selectedRow.FindControl("lblValue"))?.Text ?? "0";
        //                dr["AllowEdit"] = ((TextBox)selectedRow.FindControl("txtAllowEdit"))?.Text ?? "Yes";
        //                dr["CurrencyCode"] = ((DropDownList)selectedRow.FindControl("ddlCurrency"))?.Text ?? "";
        //                dr["CalculatedAmount"] = ((TextBox)selectedRow.FindControl("txtCalculatedAmount"))?.Text ?? "0";
        //                dr["MCRBrokerContractFee"] = ((Label)selectedRow.FindControl("lblBrokerContractFee"))?.Text ?? "0";
        //                dr["TaxGroup"] = ((DropDownList)selectedRow.FindControl("ddlSalesTaxGroup"))?.SelectedValue ?? "";
        //                dr["TaxItemGroup"] = ((DropDownList)selectedRow.FindControl("ddlItemSalesTaxGroup"))?.SelectedValue ?? "";
        //                dr["RecId"] = selectedRecId; // Label lblRecId = row.FindControl("lblRecId") as Label;

        //                dt.Rows.Add(dr);

        //                // Call update service
        //                PurchaseOrderLine_MaintainCharges svc = new PurchaseOrderLine_MaintainCharges();
        //                SysOperationResult_BOL result = svc.UpdateGeneral(dt);
        //                if (result != null && result.isSuccess)
        //                {
        //                    string script = @"
        //setTimeout(function () {
        //    __doPostBack('', '');
        //}, 1500);";

        //                    ScriptManager.RegisterStartupScript(this, GetType(),
        //                        "RefreshPage", script, true);

        //                    // Rebind the grid to reflect updated data
        //                    long recIdPurch = Session["RecId"] != null ? Convert.ToInt64(Session["RecId"]) : 0;
        //                    BindChargesGrid(recIdPurch);
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "Error",
        //                    $"alert('Error updating row: {ex.Message}');", true);
        //            }
        //        }

        // previous

        //        protected void Update_Click(object sender, EventArgs e)
        //        {
        //            LinkButton updateButton = (LinkButton)sender;
        //            GridViewRow selectedRow = (GridViewRow)updateButton.NamingContainer;

        //            try
        //            {
        //                Label lblRecId = selectedRow.FindControl("lblRecId") as Label;
        //                long selectedRecId = 0;

        //                if (lblRecId != null)
        //                {
        //                    Int64.TryParse(lblRecId.Text, out selectedRecId);
        //                }

        //                // ❌ No alert — just return silently
        //                if (selectedRecId <= 0)
        //                {
        //                    SysOperationResult_BOL noRecResult = new SysOperationResult_BOL();
        //                    noRecResult.isSuccess = false;


        //                    NotificationMessage.showMessage(noRecResult);
        //                    return;
        //                }

        //                // ✅ Build DataTable for update
        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("MarkupCode");
        //                dt.Columns.Add("Txt");
        //                dt.Columns.Add("MarkupCategory");
        //                dt.Columns.Add("SpecificUnitSymbol");
        //                dt.Columns.Add("Value");
        //                dt.Columns.Add("AllowEdit");
        //                dt.Columns.Add("CurrencyCode");
        //                dt.Columns.Add("CalculatedAmount");
        //                dt.Columns.Add("MCRBrokerContractFee");
        //                dt.Columns.Add("TaxGroup");
        //                dt.Columns.Add("TaxItemGroup");
        //                dt.Columns.Add("RecId");

        //                DataRow dr = dt.NewRow();
        //                dr["MarkupCode"] = ((DropDownList)selectedRow.FindControl("ddlChargesCode"))?.SelectedValue ?? "";
        //                dr["Txt"] = ((TextBox)selectedRow.FindControl("txtDescription"))?.Text ?? "";
        //                dr["MarkupCategory"] = ((DropDownList)selectedRow.FindControl("ddlCategory"))?.SelectedValue ?? "";
        //                dr["SpecificUnitSymbol"] = "day";
        //                dr["Value"] = ((TextBox)selectedRow.FindControl("lblValue"))?.Text ?? "0";
        //                dr["AllowEdit"] = ((TextBox)selectedRow.FindControl("txtAllowEdit"))?.Text ?? "Yes";
        //                dr["CurrencyCode"] = ((DropDownList)selectedRow.FindControl("ddlCurrency"))?.SelectedValue ?? "";
        //                dr["CalculatedAmount"] = ((TextBox)selectedRow.FindControl("txtCalculatedAmount"))?.Text ?? "0";
        //                dr["MCRBrokerContractFee"] = ((Label)selectedRow.FindControl("lblBrokerContractFee"))?.Text ?? "0";
        //                dr["TaxGroup"] = ((DropDownList)selectedRow.FindControl("ddlSalesTaxGroup"))?.SelectedValue ?? "";
        //                dr["TaxItemGroup"] = ((DropDownList)selectedRow.FindControl("ddlItemSalesTaxGroup"))?.SelectedValue ?? "";
        //                dr["RecId"] = selectedRecId;

        //                dt.Rows.Add(dr);

        //                // ✅ Call update service
        //                PurchaseOrderLine_MaintainCharges svc = new PurchaseOrderLine_MaintainCharges();
        //                SysOperationResult_BOL result = svc.UpdateGeneral(dt);

        //                // ✅ Show notification (success / failure)
        //                NotificationMessage.showMessage(result);

        ////                if (result != null && result.isSuccess)
        ////                {
        ////                    // ✅ Refresh page AFTER notification
        ////                    string script = @"
        ////setTimeout(function () {
        ////    __doPostBack('', '');
        ////}, 1500);";

        ////                    ScriptManager.RegisterStartupScript(
        ////                        this,
        ////                        GetType(),
        ////                        "RefreshPage",
        ////                        script,
        ////                        true
        ////                    );
        ////                }
        //            }
        //            catch (Exception ex)
        //            {
        //                SysErrorLog objErrorLog = new SysErrorLog();
        //                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //                string currentMethodName = currentMethod.DeclaringType.FullName;
        //                objErrorLog.write(currentMethodName, ex);
        //            }
        //        }



        //updated


        protected void Update_Click(object sender, EventArgs e)
        {
            LinkButton updateButton = (LinkButton)sender;
            GridViewRow selectedRow = (GridViewRow)updateButton.NamingContainer;

            try
            {
                Label lblRecId = selectedRow.FindControl("lblRecId") as Label;
                long selectedRecId = 0;

                if (lblRecId != null)
                {
                    Int64.TryParse(lblRecId.Text, out selectedRecId);
                }

                // Validate RecId
                if (selectedRecId <= 0)
                {
                    SysOperationResult_BOL result = new SysOperationResult_BOL();
                    result.isSuccess = false;
                    result.Message = "Invalid record selected.";

                    NotificationMessage.showMessage(result);
                    return;
                }

                // Create DataTable
                DataTable dt = new DataTable();

                dt.Columns.Add("MarkupCode");
                dt.Columns.Add("Txt");
                dt.Columns.Add("MarkupCategory");
                dt.Columns.Add("SpecificUnitSymbol");
                dt.Columns.Add("Value");
                dt.Columns.Add("AllowEdit");
                dt.Columns.Add("CurrencyCode");
                dt.Columns.Add("CalculatedAmount");
                dt.Columns.Add("MCRBrokerContractFee");
                dt.Columns.Add("TaxGroup");
                dt.Columns.Add("TaxItemGroup");
                dt.Columns.Add("RecId");

                DataRow dr = dt.NewRow();

                dr["MarkupCode"] = ((DropDownList)selectedRow.FindControl("ddlChargesCode"))?.SelectedValue ?? "";
                dr["Txt"] = ((TextBox)selectedRow.FindControl("txtDescription"))?.Text ?? "";
                dr["MarkupCategory"] = ((DropDownList)selectedRow.FindControl("ddlCategory"))?.SelectedValue ?? "";
                dr["SpecificUnitSymbol"] = "day";
                dr["Value"] = ((TextBox)selectedRow.FindControl("lblValue"))?.Text ?? "0";
                dr["AllowEdit"] = ((TextBox)selectedRow.FindControl("txtAllowEdit"))?.Text ?? "Yes";
                dr["CurrencyCode"] = ((DropDownList)selectedRow.FindControl("ddlCurrency"))?.SelectedValue ?? "";
                dr["CalculatedAmount"] = ((TextBox)selectedRow.FindControl("txtCalculatedAmount"))?.Text ?? "0";
                dr["MCRBrokerContractFee"] = ((Label)selectedRow.FindControl("lblBrokerContractFee"))?.Text ?? "0";
                dr["TaxGroup"] = ((DropDownList)selectedRow.FindControl("ddlSalesTaxGroup"))?.SelectedValue ?? "";
                dr["TaxItemGroup"] = ((DropDownList)selectedRow.FindControl("ddlItemSalesTaxGroup"))?.SelectedValue ?? "";
                dr["RecId"] = selectedRecId;

                dt.Rows.Add(dr);

                // Call Update Service
                PurchaseOrderLine_MaintainCharges svc = new PurchaseOrderLine_MaintainCharges();
                SysOperationResult_BOL resultUpdate = svc.UpdateGeneral(dt);

                // Show Notification
                if (resultUpdate != null)
                {
                    if (string.IsNullOrEmpty(resultUpdate.Message))
                    {
                        resultUpdate.Message = resultUpdate.isSuccess
                            ? "Record updated successfully."
                            : "Record update failed.";
                    }

                    NotificationMessage.showMessage(resultUpdate);

                    // Refresh page after successful update
                    if (resultUpdate.isSuccess)
                    {
                        string script = @"
                setTimeout(function () {
                    location.reload();
                }, 2000);";

                        ScriptManager.RegisterStartupScript(
                            this,
                            GetType(),
                            "RefreshPage",
                            script,
                            true);
                    }
                }
                else
                {
                    SysOperationResult_BOL result = new SysOperationResult_BOL();
                    result.isSuccess = false;
                    result.Message = "No response received from update service.";

                    NotificationMessage.showMessage(result);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;

                objErrorLog.write(currentMethodName, ex);

                SysOperationResult_BOL result = new SysOperationResult_BOL();
                result.isSuccess = false;
                result.Message = ex.Message;

                NotificationMessage.showMessage(result);
            }
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            string source = Session["MaintainChargesSource"] as string;

            if (source == "HEADER")
            {
                // Came from All Purchase Orders
                Response.Redirect("~/ESS/PR/AllPurchaseOrder_ListPage.aspx", false);
            }
            else if (source == "LINE")
            {
                // Came from Purchase Order Lines
                Response.Redirect("~/ESS/PR/PurchaseOrderLines_ListPage.aspx", false);
            }
            else
            {
                // Fallback (safety)
                Response.Redirect("~/ESS/PR/PurchaseOrderLines_ListPage.aspx", false);
            }
        }








        // Optional: Filter button (not used)

    }
}
