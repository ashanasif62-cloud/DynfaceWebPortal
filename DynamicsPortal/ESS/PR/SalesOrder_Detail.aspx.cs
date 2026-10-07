using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSFinancialDimensionsSvcReference;
using PortalIntegration.SalesOrderSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class SalesOrder_Detail : MainForm
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            // Always rebuild dynamic controls early
            showFinancialDimension(0);
            showFinancialDimensionline(0);


        }
        //protected override void Page_Load(object sender, EventArgs e)
        //{
        //    string selectedItemId = "";
        //    if (Session["SelectedItemId"] != null)
        //        selectedItemId = Session["SelectedItemId"] as string;

        //    if (!IsPostBack)
        //    {
        //        // Set the page title
        //        Page.Title = "Sales Order Details";

        //        // Update the master page title div
        //        var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
        //        if (titleDiv != null)
        //        {
        //            titleDiv.InnerText = "Sales Order Details"; // consistent capitalization
        //            titleDiv.Style["font-weight"] = "bold";
        //        }

        //        // Bind dropdowns and other controls
        //        BindCustomerAccount();
        //        BindInvoiceAccount();
        //        BindCurrency();
        //        BindSite();
        //        BindWarehouse();
        //        Bindsalestax();
        //        BindSalesGroup();
        //        LoadReservationDropdownHeader();
        //        BindPool(); 
        //        Bindlanguage(); 
        //        BindSalesOrigin (); 
        //        BindPaymentMethod();
        //        BindCashDiscount();
        //        BindPriceGroup();
        //        BindLineDisc();
        //        BindTotalDisc();   
        //        BindPaymentSchedule();
        //        BindPayment();
        //        BindDeliveryModeheader();
        //        BindDeliverytermheader();


        //        // Load the sales order header and list
        //        BindSalesOrderHeader();
        //        LoadSalesOrders();


        //        // If a selected item exists in the session, bind its details

        //    }

        //    // only called when a valid itemId exists

        //}


        protected override void Page_Load(object sender, EventArgs e)
        {
            string salesIdFromQuery = Request.QueryString["SalesId"];
            if (!string.IsNullOrEmpty(salesIdFromQuery))
                Session["SalesId"] = salesIdFromQuery;

            if (!IsPostBack)
            {
                Page.Title = "Sales Order Details";
                var titleDiv = Master.FindControl("pageTitle") as HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Sales Order Details";
                    titleDiv.Style["font-weight"] = "bold";
                }

                if (Session["SalesId"] == null) return;

                BindCustomerAccount();
                BindInvoiceAccount();
                BindCurrency();
                BindSite();
                BindWarehouse();
                Bindsalestax();
                BindSalesGroup();
                LoadReservationDropdownHeader();
                BindPool();
                Bindlanguage();
                BindSalesOrigin();
                BindPaymentMethod();
                BindCashDiscount();
                BindPriceGroup();
                BindLineDisc();
                BindTotalDisc();
                BindPaymentSchedule();
                BindPayment();
                BindDeliveryModeheader();
                BindDeliverytermheader();
                BindSalesOrderHeader();
                LoadSalesOrders();
            }
            else
            {
                string eventTarget = Request["__EVENTTARGET"] ?? "";
                string eventArgument = Request["__EVENTARGUMENT"] ?? "";

                // ✅ Identify which button triggered the postback
                bool isSaveLineClick = IsControlPostBack("btnSaveLine");
                bool isDeleteLineClick = IsControlPostBack("btnDeleteLine");
                bool isAddLineClick = IsControlPostBack("BtnAddLine");
                bool isSaveHeaderClick = IsControlPostBack("BtnSaveHeader");

                // ✅ Do NOT reload grid from DB when these buttons are clicked
                // (they manage the grid themselves)
                bool skipGridReload = isSaveLineClick || isDeleteLineClick ||
                                      isAddLineClick || isSaveHeaderClick ||
                                      eventTarget.Contains("ddlItemNumber") ||
                                      eventTarget.Contains("chk_SelectSingle");

                if (!skipGridReload)
                {
                    LoadSalesOrders();
                }
            }
        }

        // ✅ Helper: checks if a specific button caused the postback
        private bool IsControlPostBack(string controlId)
        {
            // LinkButtons post back via __EVENTTARGET containing their UniqueID
            string eventTarget = Request["__EVENTTARGET"] ?? "";
            return eventTarget.Contains(controlId);
        }


        private void BindSalesOrderHeader()
        {
            if (Session["SalesId"] == null)
                return;

            string salesId = Session["SalesId"].ToString();

            SalesOrder svc = new SalesOrder();
          
            DataTable dt = svc.retrieveheader(salesId);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                // COLUMN 1: DELIVERY ADDRESS
                txtName.Text = dr["deliveryName"]?.ToString();
                string customername = dr["customerName"]?.ToString();
                txtDeliveryAddress.Text = customername;
                Session["DeliveryAddress"] = customername;

                string address = dr["address"]?.ToString();
                txtAddress.Text = address;

                Session["address"] = address;

                // COLUMN 2: DELIVERY DATE
                txtRequestedShipDate.Text =
                    dr["ShippingDateRequested"] != DBNull.Value
                    ? Convert.ToDateTime(dr["ShippingDateRequested"]).ToString("yyyy-MM-dd")
                    : "";

                txtRequestedReceiptDate.Text =
                    dr["ReceiptDateRequested"] != DBNull.Value
                    ? Convert.ToDateTime(dr["ReceiptDateRequested"]).ToString("yyyy-MM-dd")
                    : "";

                //txtConfirmedShipDate.Text =
                //    dr["ConfirmedShipDate"] != DBNull.Value
                //    ? Convert.ToDateTime(dr["ConfirmedShipDate"]).ToString("yyyy-MM-dd")
                //    : "";

                //txtConfirmedReceiptDate.Text =
                //    dr["ConfirmedReceiptDate"] != DBNull.Value
                //    ? Convert.ToDateTime(dr["ConfirmedReceiptDate"]).ToString("yyyy-MM-dd")
                //    : "";

                //// COLUMN 3: REFERENCES / DISCOUNTS / WAREHOUSE
                //txtCustomerReference.Text = dr["CustomerReference"]?.ToString();
                //txtCustomerRequisition.Text = dr["CustomerRequisition"]?.ToString();
                //txtTotalDiscount.Text = dr["TotalDiscount"]?.ToString();
                //txtReleaseStatus.Text = dr["ReleaseStatus"]?.ToString();

                txtSalesOrder.Text = Session["SalesId"].ToString();
                txtCustomerName.Text = dr["salesName"].ToString();
                txtOrderType.Text = dr["SalesType"].ToString();
                ddlcustomerAccount.SelectedValue = dr["custAccount"].ToString();
                ddlInvoiceAccount.SelectedValue = dr["InvoiceAccount"].ToString();
                txtStatus.Text = dr["SalesStatus"].ToString();
                ddlSalesTaxGroup.SelectedValue = dr["TaxGroup"].ToString();
                ddlReservation.SelectedValue = dr["Reservation"].ToString();
                ddlSalesTaker.SelectedValue = dr["WorkerSalesTakerName"].ToString();
                ddlPool.SelectedValue = dr["SalesPoolId"].ToString();
                ddlLanguage.SelectedValue = dr["LanguageId"].ToString();
                ddlSalesOrigin.SelectedValue = dr["SalesOriginId"].ToString();
                ddlCurrency.SelectedValue = dr["CurrencyCode"].ToString();
                ddlPayment.SelectedValue = dr["Payment"].ToString();
                ddlMethodOfPayment.SelectedValue = dr["PaymMode"].ToString();
                ddlCashDisc.SelectedValue = dr["CashDiscCode"].ToString();
                txtDiscountPercHeader.Text = dr["CashDiscPercent"].ToString();
                ddlPriceGroup.SelectedValue = dr["PriceGroupId"].ToString();
                ddlLineDiscountGroup.SelectedValue = dr["linedisc"].ToString();
                ddlMultilineDiscGroup.SelectedValue = dr["MultiLineDiscCode"].ToString();
                ddlTotalDiscountGroup.SelectedValue = dr["EndDiscCode"].ToString();
                txtReleaseStatus.Text = dr["ReleaseStatus"].ToString();
                ddlSite.SelectedValue = dr["InventSiteId"].ToString();
                ddlWarehouse.SelectedValue = dr["InventLocationId"].ToString();
                ddlSalesGroup.SelectedValue = dr["GroupId"].ToString();
                ddlPaymentSchedule.SelectedValue = dr["PaymSchedId"].ToString();
                ddldeliverytermheader.SelectedValue = dr["DlvMode"].ToString();
                ddlmodeofdeliveryheader.SelectedValue = dr["DlvTerm"].ToString();
                Txtnameheader.Text = dr["deliveryName"].ToString();
                Txtdeliveryaddressheader.Text = Session["DeliveryAddress"].ToString();
                TxtAddressHeader.Text = Session["address"].ToString();
            }
        }

        private void showFinancialDimensionline(long _defaultDimensionRecId)
        {
            ESSFinancialDimensions finDim = new ESSFinancialDimensions();
            DataContract[] dimensions = finDim.retrieveActiveDimensions();

            if (dimensions == null || dimensions.Length == 0)
                return;

            if (financialdimesnsionlines.Controls.Count > 0)
                return; // Prevent duplicate creation

            int colCount = 0;
            HtmlGenericControl rowDiv = null;

            foreach (var dim in dimensions)
            {
                if (colCount % 2 == 0)
                {
                    rowDiv = new HtmlGenericControl("div");
                    rowDiv.Attributes["class"] = "info-row";
                    financialdimesnsionlines.Controls.Add(rowDiv);
                }

                HtmlGenericControl blockDiv = new HtmlGenericControl("div");
                blockDiv.Attributes["class"] = "info-block";

                HtmlGenericControl strong = new HtmlGenericControl("strong");
                strong.InnerText = dim.Code;
                blockDiv.Controls.Add(strong);

                HtmlGenericControl span = new HtmlGenericControl("span");

                string safeCode = dim.Code.Replace(" ", "").Replace("-", "");

                DropDownList ddl = new DropDownList();
                ddl.ID = $"ddlLineFinancialDimension_{safeCode}";
                ddl.CssClass = "filterable-dropdown";

                DataTable dimeVal = finDim.retrieveDimensionLookUp(dim.Code);
                ddl.Items.Add(new ListItem("", ""));

                foreach (DataRow row in dimeVal.Rows)
                {
                    string value = row["Value1"].ToString();
                    string valueName = row["Value2"].ToString();
                    ddl.Items.Add(new ListItem($"{value} - {valueName}", value));
                }

                span.Controls.Add(ddl);
                blockDiv.Controls.Add(span);
                rowDiv.Controls.Add(blockDiv);

                colCount++;
            }

            bindEmployeeDimension(_defaultDimensionRecId);
        }
        protected void bindEmployeeDimension(long __defaultDimensionRecId)
        {
            ESSFinancialDimensions financialDimensions = new ESSFinancialDimensions();
            DataContract[] dataContracts =
                financialDimensions.retrieveDimensionValues(__defaultDimensionRecId);

            if (dataContracts == null)
                return;

            foreach (DataContract dataContract in dataContracts)
            {
                string safeCode = dataContract.Code.Replace(" ", "").Replace("-", "");
                string dimValue = dataContract.Value1;

                DropDownList ddl = financialdimesnsionlines
                    .FindControl("ddlLineFinancialDimension_" + safeCode) as DropDownList;

                if (ddl != null && !string.IsNullOrEmpty(dimValue))
                {
                    if (ddl.Items.FindByValue(dimValue) != null)
                        ddl.SelectedValue = dimValue;
                }
            }
        }

        //private void LoadSalesOrders()
        //{
        //    if (Session["SalesId"] == null)
        //        return;

        //    string salesId = Session["SalesId"].ToString();

        //    SalesOrder svc = new SalesOrder();

        //    // 🔹 Pass SalesId to service
        //    DataTable dt = svc.retrievelines(salesId);

        //    gvSalesOrderLines.DataSource = dt;
        //    gvSalesOrderLines.DataBind();
        //}

        //private void LoadSalesOrders()
        //{
        //    if (Session["SalesId"] == null)
        //        return;

        //    string salesId = Session["SalesId"].ToString();

        //    SalesOrder svc = new SalesOrder();
        //    DataTable dt = svc.retrievelines(salesId);

        //    Session["SalesLinesUI"] = dt;

        //    gvSalesOrderLines.DataSource = dt;
        //    gvSalesOrderLines.DataBind();
        //}

        private void LoadSalesOrders()
        {
            if (Session["SalesId"] == null)
                return;

            string salesId = Session["SalesId"].ToString();
            SalesOrder svc = new SalesOrder();
        
           DataTable dt = svc.retrievelines(salesId);

            Session["SalesLinesUI"] = dt;

            gvSalesOrderLines.RowDataBound += gvSalesOrderLines_RowDataBound; // attach event
            gvSalesOrderLines.DataSource = dt;
            gvSalesOrderLines.DataBind();
        }

        private void BindSalesOrderlinedetail()
        {
            if (Session["SalesId"] == null || Session["SelectedItemId"] == null)
                return;

            string itemId = Session["SelectedItemId"].ToString();
            string salesId = Session["SalesId"].ToString();

            SalesOrder svc = new SalesOrder();
           // DataTable dt = new DataTable();
            DataTable dt = svc.retrievelinesDetail(salesId, itemId);

            // If retrievelinesDetail returns no data, fallback to retrievelines
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = svc.retrievelines(salesId);
            }

            // If still no data, exit
            if (dt == null || dt.Rows.Count == 0)
                return;
            DataRow dr = dt.Rows[0];

            // COLUMN 1: DELIVERY ADDRESS
            txtProductName.Text = dr["itemName"]?.ToString();
            txtName.Text = dr["itemtxt"]?.ToString();
            txtLineStatus.Text = dr["SalesStatus"]?.ToString();
            txtFulfillmentStatus.Text = dr["FulfillmentStatus"]?.ToString();
            txtLotID.Text = dr["InventTransId"]?.ToString();
            ddlreservation1.SelectedValue = dr["Reservation"]?.ToString();
            if (dr["CostPrice"] != DBNull.Value)
            {
                decimal costPrice = Convert.ToDecimal(dr["CostPrice"]);
                txtReturnCostPrice.Text = costPrice.ToString("0.00");
            }
            else
            {
                txtReturnCostPrice.Text = "0.00";
            }
            ddlitemsalestaxgroup.SelectedValue = dr["TaxItemGroup"].ToString();
            ddlsalestaxgroup1.SelectedValue = dr["TaxGroup"].ToString();
            ddlsalesgroup1.SelectedValue = dr["GroupId"].ToString();
            txtDeliveryName.Text = dr["deliveryName"].ToString();
            DeliveryAddressLine.Text = Session["DeliveryAddress"].ToString();
            txtAddressLine.Text = Session["address"].ToString();
            ddlsiteline.Text = dr["InventSiteId"].ToString();
            ddlwarehouseline.Text = dr["InventLocationId"].ToString();
            TxtRequestedShipDateline.Text =
               dr["ShippingDateRequested"] != DBNull.Value
               ? Convert.ToDateTime(dr["ShippingDateRequested"]).ToString("yyyy-MM-dd")
               : "";

            TxtRequestReceiptDateline.Text =
              dr["ReceiptDateRequested"] != DBNull.Value
              ? Convert.ToDateTime(dr["ReceiptDateRequested"]).ToString("yyyy-MM-dd")
              : "";

            txtBatchCTPStatus.Text = dr["MPSFullRunCTPStatus"].ToString();
            ddlterms.SelectedValue= dr["DlvMode"].ToString();
            ddlmode.SelectedValue = dr["DlvTerm"].ToString();
            txtDeliveryType.Text = dr["DeliveryType"].ToString();
            ddlSize.SelectedValue = dr["InventSizeId"].ToString();            
        }
        private void LoadReservationDropdown()
        {
            ddlreservation1.Items.Clear();

            ddlreservation1.Items.Add(new ListItem("", ""));
            ddlreservation1.Items.Add(new ListItem("Manual", "Manual"));
            ddlreservation1.Items.Add(new ListItem("Automatic", "Automatic"));
            ddlreservation1.Items.Add(new ListItem("Explosion", "Explosion"));
        }

        private void LoadReservationDropdownHeader()
        {
            ddlReservation.Items.Clear();

            ddlReservation.Items.Add(new ListItem("", ""));
            ddlReservation.Items.Add(new ListItem("Manual", "Manual"));
            ddlReservation.Items.Add(new ListItem("Automatic", "Automatic"));
            ddlReservation.Items.Add(new ListItem("Explosion", "Explosion"));
        }


        //private DataTable GetGridTable()
        //{
        //    if (Session["SalesLinesUI"] == null)
        //    {
        //        DataTable dt = new DataTable();

        //        dt.Columns.Add("RetailVariantId");
        //        dt.Columns.Add("ItemId");
        //        dt.Columns.Add("itemName");
        //        dt.Columns.Add("category");
        //        dt.Columns.Add("SalesQty");
        //        dt.Columns.Add("SalesUnitId");
        //        dt.Columns.Add("DeliveryType");
        //        dt.Columns.Add("InventSiteId");
        //        dt.Columns.Add("InventLocationId");

        //        Session["SalesLinesUI"] = dt;
        //    }

        //    return (DataTable)Session["SalesLinesUI"];
        //}

        private DataTable GetGridTable()
        {
            if (Session["SalesLinesUI"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("recId", typeof(long));        // ← was missing, caused bind crash
                dt.Columns.Add("RetailVariantId");
                dt.Columns.Add("ItemId");
                dt.Columns.Add("itemName");
                dt.Columns.Add("category");
                dt.Columns.Add("SalesQty");
                dt.Columns.Add("SalesUnitId");
                dt.Columns.Add("DeliveryType");
                dt.Columns.Add("InventSiteId");
                dt.Columns.Add("InventLocationId");
                dt.Columns.Add("SalesPrice");                 // ← required by Bind("SalesPrice")
                dt.Columns.Add("SalesLineDisc");              // ← required by Bind("SalesLineDisc")
                dt.Columns.Add("linepercent");                // ← required by Bind("linepercent")
                dt.Columns.Add("LineAmount");                 // ← required by Bind("LineAmount")
                Session["SalesLinesUI"] = dt;
            }
            return (DataTable)Session["SalesLinesUI"];
        }

        private bool BindCurrency(string selectedValue = "")
        {
          
            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveCurrency();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlCurrency.DataSource = dt;
                ddlCurrency.DataTextField = "CurrencyCode";   // what user sees
                ddlCurrency.DataValueField = "CurrencyCode";  // underlying value
                ddlCurrency.DataBind();

                // Insert empty option at the top
                ddlCurrency.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlCurrency.Items.FindByValue(selectedValue) != null)
            {
                ddlCurrency.SelectedValue = selectedValue;
            }
            else
            {
                ddlCurrency.SelectedIndex = 0; // show empty
            }

            return true;
        }
        

       

        private bool BindCustomerAccount(string selectedValue = "")
        {
            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveCustomerAccount();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlcustomerAccount.DataSource = dt;
                ddlcustomerAccount.DataTextField = "custAccount";   // what user sees
                ddlcustomerAccount.DataValueField = "custAccount";  // underlying value
                ddlcustomerAccount.DataBind();

                // Insert empty option at the top
                ddlcustomerAccount.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlcustomerAccount.Items.FindByValue(selectedValue) != null)
            {
                ddlcustomerAccount.SelectedValue = selectedValue;
            }
            else
            {
                ddlcustomerAccount.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindInvoiceAccount(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
          
             DataTable dt = header.retrieveInvoiceAccount();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlInvoiceAccount.DataSource = dt;
                ddlInvoiceAccount.DataTextField = "InvoiceAccount";   // what user sees
                ddlInvoiceAccount.DataValueField = "InvoiceAccount";  // underlying value
                ddlInvoiceAccount.DataBind();

                // Insert empty option at the top
                ddlInvoiceAccount.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlInvoiceAccount.Items.FindByValue(selectedValue) != null)
            {
                ddlInvoiceAccount.SelectedValue = selectedValue;
            }
            else
            {
                ddlInvoiceAccount.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindPool(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrievePool();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlPool.DataSource = dt;
                ddlPool.DataTextField = "SalesPoolId";   // what user sees
                ddlPool.DataValueField = "SalesPoolId";  // underlying value
                ddlPool.DataBind();

                // Insert empty option at the top
                ddlPool.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlPool.Items.FindByValue(selectedValue) != null)
            {
                ddlPool.SelectedValue = selectedValue;

            }
            else
            {
                ddlPool.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindSalesOrigin(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveSalesOrigin();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlSalesOrigin.DataSource = dt;
                ddlSalesOrigin.DataTextField = "SalesOriginId";   // what user sees
                ddlSalesOrigin.DataValueField = "SalesOriginId";  // underlying value
                ddlSalesOrigin.DataBind();

                // Insert empty option at the top
                ddlSalesOrigin.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlSalesOrigin.Items.FindByValue(selectedValue) != null)
            {
                ddlSalesOrigin.SelectedValue = selectedValue;

            }
            else
            {
                ddlSalesOrigin.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool Bindlanguage(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveLanguageId();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlLanguage.DataSource = dt;
                ddlLanguage.DataTextField = "LanguageId";   // what user sees
                ddlLanguage.DataValueField = "LanguageId";  // underlying value
                ddlLanguage.DataBind();

                // Insert empty option at the top
                ddlLanguage.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlLanguage.Items.FindByValue(selectedValue) != null)
            {
                ddlLanguage.SelectedValue = selectedValue;

            }
            else
            {
                ddlLanguage.SelectedIndex = 0; // show empty
            }

            return true;
        }


        private bool BindSite(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveSite();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlSite.DataSource = dt;
                ddlSite.DataTextField = "InventSiteId";   // what user sees
                ddlSite.DataValueField = "InventSiteId";  // underlying value
                ddlSite.DataBind();

                // Insert empty option at the top
                ddlSite.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlSite.Items.FindByValue(selectedValue) != null)
            {
                ddlSite.SelectedValue = selectedValue;
                
            }
            else
            {
                ddlSite.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindPaymentMethod(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = new DataTable();
          //  DataTable dt = header.retrievePaymentMethod();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlMethodOfPayment.DataSource = dt;
                ddlMethodOfPayment.DataTextField = "PaymMode";   // what user sees
                ddlMethodOfPayment.DataValueField = "PaymMode";  // underlying value
                ddlMethodOfPayment.DataBind();

                // Insert empty option at the top
                ddlMethodOfPayment.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlMethodOfPayment.Items.FindByValue(selectedValue) != null)
            {
                ddlMethodOfPayment.SelectedValue = selectedValue;

            }
            else
            {
                ddlMethodOfPayment.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindPaymentSchedule(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
           
           DataTable dt = header.retrievePaymentMethod();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlPaymentSchedule.DataSource = dt;
                ddlPaymentSchedule.DataTextField = "PaymSchedId";   // what user sees
                ddlPaymentSchedule.DataValueField = "PaymSchedId";  // underlying value
                ddlPaymentSchedule.DataBind();

                // Insert empty option at the top
                ddlPaymentSchedule.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlPaymentSchedule.Items.FindByValue(selectedValue) != null)
            {
                ddlPaymentSchedule.SelectedValue = selectedValue;

            }
            else
            {
                ddlPaymentSchedule.SelectedIndex = 0; // show empty
            }

            return true;
        }


        private bool BindPayment(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
           

            DataTable dt = header.retrievePayment();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlPayment.DataSource = dt;
                ddlPayment.DataTextField = "paymTermId";   // what user sees
                ddlPayment.DataValueField = "paymTermId";  // underlying value
                ddlPayment.DataBind();

                // Insert empty option at the top
                ddlPayment.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlPayment.Items.FindByValue(selectedValue) != null)
            {
                ddlPayment.SelectedValue = selectedValue;

            }
            else
            {
                ddlPayment.SelectedIndex = 0; // show empty
            }

            return true;
        }
        private bool BindCashDiscount(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
         
            DataTable dt = header.retrieveCashDiscount();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlCashDisc.DataSource = dt;
                ddlCashDisc.DataTextField = "CashDiscCode";   // what user sees
                ddlCashDisc.DataValueField = "CashDiscCode";  // underlying value
                ddlCashDisc.DataBind();

                // Insert empty option at the top
                ddlCashDisc.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlCashDisc.Items.FindByValue(selectedValue) != null)
            {
                ddlCashDisc.SelectedValue = selectedValue;

            }
            else
            {
                ddlCashDisc.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindPriceGroup(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
          
            DataTable dt = header.retrieveLineDiscGroup();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlPriceGroup.DataSource = dt;
                ddlPriceGroup.DataTextField = "PriceGroupId";   // what user sees
                ddlPriceGroup.DataValueField = "PriceGroupId";  // underlying value
                ddlPriceGroup.DataBind();

                // Insert empty option at the top
                ddlPriceGroup.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlPriceGroup.Items.FindByValue(selectedValue) != null)
            {
                ddlPriceGroup.SelectedValue = selectedValue;

            }
            else
            {
                ddlPriceGroup.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindLineDisc(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
           
            DataTable dt = header.retrieveLineDiscGroup();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlLineDiscountGroup.DataSource = dt;
                ddlLineDiscountGroup.DataTextField = "PriceGroupId";   // what user sees
                ddlLineDiscountGroup.DataValueField = "PriceGroupId";  // underlying value
                ddlLineDiscountGroup.DataBind();

                // Insert empty option at the top
                ddlLineDiscountGroup.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlLineDiscountGroup.Items.FindByValue(selectedValue) != null)
            {
                ddlLineDiscountGroup.SelectedValue = selectedValue;

            }
            else
            {
                ddlPriceGroup.SelectedIndex = 0; // show empty
            }

            return true;
        }


        private bool BindTotalDisc(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            

            DataTable dt = header.retrieveTotalDiscGroup();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlTotalDiscountGroup.DataSource = dt;
                ddlTotalDiscountGroup.DataTextField = "PriceGroupId";   // what user sees
                ddlTotalDiscountGroup.DataValueField = "PriceGroupId";  // underlying value
                ddlTotalDiscountGroup.DataBind();

                // Insert empty option at the top
                ddlTotalDiscountGroup.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlTotalDiscountGroup.Items.FindByValue(selectedValue) != null)
            {
                ddlTotalDiscountGroup.SelectedValue = selectedValue;

            }
            else
            {
                ddlTotalDiscountGroup.SelectedIndex = 0; // show empty
            }

            return true;
        }
        private bool BindSiteline(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveSite();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlsiteline.DataSource = dt;
                ddlsiteline.DataTextField = "InventSiteId";   // what user sees
                ddlsiteline.DataValueField = "InventSiteId";  // underlying value
                ddlsiteline.DataBind();

                // Insert empty option at the top
                ddlsiteline.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlsiteline.Items.FindByValue(selectedValue) != null)
            {
                ddlsiteline.SelectedValue = selectedValue;

            }
            else
            {
                ddlsiteline.SelectedIndex = 0; // show empty
            }

            return true;
        }



        private bool BindWarehouse(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveWarehouse();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlWarehouse.DataSource = dt;
                ddlWarehouse.DataTextField = "InventLocationId";   // what user sees
                ddlWarehouse.DataValueField = "InventLocationId";  // underlying value
                ddlWarehouse.DataBind();

                // Insert empty option at the top
                ddlWarehouse.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlWarehouse.Items.FindByValue(selectedValue) != null)
            {
                ddlWarehouse.SelectedValue = selectedValue;
            }
            else
            {
                ddlWarehouse.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindWarehouseline(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveWarehouse();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlwarehouseline.DataSource = dt;
                ddlwarehouseline.DataTextField = "InventLocationId";   // what user sees
                ddlwarehouseline.DataValueField = "InventLocationId";  // underlying value
                ddlwarehouseline.DataBind();

                // Insert empty option at the top
                ddlwarehouseline.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlwarehouseline.Items.FindByValue(selectedValue) != null)
            {
                ddlwarehouseline.SelectedValue = selectedValue;
            }
            else
            {
                ddlwarehouseline.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindDeliveryterm(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveDeliveryTerm();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlterms.DataSource = dt;
                ddlterms.DataTextField = "DlvMode";   // what user sees
                ddlterms.DataValueField = "DlvMode";  // underlying value
                ddlterms.DataBind();

                // Insert empty option at the top
                ddlterms.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlterms.Items.FindByValue(selectedValue) != null)
            {
                ddlterms.SelectedValue = selectedValue;

            }
            else
            {
                ddlterms.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindDeliverytermheader(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveDeliveryTerm();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddldeliverytermheader.DataSource = dt;
                ddldeliverytermheader.DataTextField = "DlvMode";   // what user sees
                ddldeliverytermheader.DataValueField = "DlvMode";  // underlying value
                ddldeliverytermheader.DataBind();

                // Insert empty option at the top
                ddldeliverytermheader.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddldeliverytermheader.Items.FindByValue(selectedValue) != null)
            {
                ddldeliverytermheader.SelectedValue = selectedValue;

            }
            else
            {
                ddldeliverytermheader.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindDeliveryMode(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveDeliveryMode();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlmode.DataSource = dt;
                ddlmode.DataTextField = "DlvTerm";   // what user sees
                ddlmode.DataValueField = "DlvTerm";  // underlying value
                ddlmode.DataBind();

                // Insert empty option at the top
                ddlmode.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlmode.Items.FindByValue(selectedValue) != null)
            {
                ddlmode.SelectedValue = selectedValue;

            }
            else
            {
                ddlmode.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindDeliveryModeheader(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = header.retrieveDeliveryMode();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlmodeofdeliveryheader.DataSource = dt;
                ddlmodeofdeliveryheader.DataTextField = "DlvTerm";   // what user sees
                ddlmodeofdeliveryheader.DataValueField = "DlvTerm";  // underlying value
                ddlmodeofdeliveryheader.DataBind();

                // Insert empty option at the top
                ddlmodeofdeliveryheader.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlmodeofdeliveryheader.Items.FindByValue(selectedValue) != null)
            {
                ddlmodeofdeliveryheader.SelectedValue = selectedValue;

            }
            else
            {
                ddlmodeofdeliveryheader.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindSalesGroup(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
           
           DataTable dt = header.retrieveSalesGroup();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlsalesgroup1.DataSource = dt;
                ddlsalesgroup1.DataTextField = "GroupId";   // what user sees
                ddlsalesgroup1.DataValueField = "GroupId";  // underlying value
                ddlsalesgroup1.DataBind();

                // Insert empty option at the top
                ddlSalesGroup.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlsalesgroup1.Items.FindByValue(selectedValue) != null)
            {
                ddlsalesgroup1.SelectedValue = selectedValue;

            }
            else
            {
                ddlsalesgroup1.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool BindSalesGroupheader(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
            DataTable dt = new DataTable();
            //DataTable dt = header.retrieveSalesGroup();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlSalesGroup.DataSource = dt;
                ddlSalesGroup.DataTextField = "GroupId";   // what user sees
                ddlSalesGroup.DataValueField = "GroupId";  // underlying value
                ddlSalesGroup.DataBind();

                // Insert empty option at the top
                ddlSalesGroup.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlSalesGroup.Items.FindByValue(selectedValue) != null)
            {
                ddlSalesGroup.SelectedValue = selectedValue;

            }
            else
            {
                ddlSalesGroup.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool Bindtaxitemline(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
         
            DataTable dt = header.retrieveItemTax();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlitemsalestaxgroup.DataSource = dt;
                ddlitemsalestaxgroup.DataTextField = "TaxItemGroup";   // what user sees
                ddlitemsalestaxgroup.DataValueField = "TaxItemGroup";  // underlying value
                ddlitemsalestaxgroup.DataBind();

                // Insert empty option at the top
                ddlitemsalestaxgroup.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlitemsalestaxgroup.Items.FindByValue(selectedValue) != null)
            {
                ddlitemsalestaxgroup.SelectedValue = selectedValue;

            }
            else
            {
                ddlitemsalestaxgroup.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool Bindsalestaxline(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
          
            DataTable dt = header.retrieveSalesTax();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlsalestaxgroup1.DataSource = dt;
                ddlsalestaxgroup1.DataTextField = "TaxGroup";   // what user sees
                ddlsalestaxgroup1.DataValueField = "TaxGroup";  // underlying value
                ddlsalestaxgroup1.DataBind();

                // Insert empty option at the top
                ddlsalestaxgroup1.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlsalestaxgroup1.Items.FindByValue(selectedValue) != null)
            {
                ddlsalestaxgroup1.SelectedValue = selectedValue;

            }
            else
            {
                ddlsalestaxgroup1.SelectedIndex = 0; // show empty
            }

            return true;
        }

        private bool Bindsalestax(string selectedValue = "")
        {

            SalesOrder header = new SalesOrder();
           
            DataTable dt = header.retrieveSalesTax();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlSalesTaxGroup.DataSource = dt;
                ddlSalesTaxGroup.DataTextField = "TaxGroup";   // what user sees
                ddlSalesTaxGroup.DataValueField = "TaxGroup";  // underlying value
                ddlSalesTaxGroup.DataBind();

                // Insert empty option at the top
                ddlSalesTaxGroup.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlSalesTaxGroup.Items.FindByValue(selectedValue) != null)
            {
                ddlSalesTaxGroup.SelectedValue = selectedValue;

            }
            else
            {
                ddlSalesTaxGroup.SelectedIndex = 0; // show empty
            }

            return true;
        }
        //protected void gvSalesOrderLines_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType != DataControlRowType.DataRow)
        //        return;

        //    DataRowView drv = e.Row.DataItem as DataRowView;
        //    SalesOrder svc = new SalesOrder();

        //    // -------------------------
        //    // Helper method to bind dropdown safely
        //    // -------------------------
        //    void BindDropdown(DropDownList ddl, DataTable dt, string textField, string valueField, string selectedValue)
        //    {
        //        ddl.DataSource = dt;
        //        ddl.DataTextField = textField;
        //        ddl.DataValueField = valueField;
        //        ddl.DataBind();

        //        // Insert empty option
        //        ddl.Items.Insert(0, new ListItem("--Select--", ""));

        //        // Set selected value safely
        //        if (!string.IsNullOrEmpty(selectedValue))
        //        {
        //            if (ddl.Items.FindByValue(selectedValue) != null)
        //            {
        //                ddl.SelectedValue = selectedValue;
        //            }
        //            else
        //            {
        //                // Add missing value to dropdown
        //                ddl.Items.Insert(1, new ListItem(selectedValue, selectedValue));
        //                ddl.SelectedValue = selectedValue;
        //            }
        //        }
        //        else
        //        {
        //            ddl.SelectedIndex = 0;
        //        }
        //    }

        //    // -------------------------
        //    // Item Number
        //    // -------------------------
        //    DropDownList ddlItemNumber = e.Row.FindControl("ddlItemNumber") as DropDownList;
        //    if (ddlItemNumber != null)
        //    {
        //        DataTable dtItems = svc.retrieveitemnumber(); // must return ItemId, ItemName
        //        string selectedItem = drv["ItemId"] != DBNull.Value ? drv["ItemId"].ToString() : "";
        //        BindDropdown(ddlItemNumber, dtItems, "ItemId", "ItemId", selectedItem);
        //    }

        //    // -------------------------
        //    // Site
        //    // -------------------------
        //    DropDownList ddlSite = e.Row.FindControl("ddlSite") as DropDownList;
        //    if (ddlSite != null)
        //    {
        //        DataTable dtSites = svc.retrieveSite(); // must return InventSiteId
        //        string selectedSite = drv["InventSiteId"] != DBNull.Value ? drv["InventSiteId"].ToString() : "";
        //        BindDropdown(ddlSite, dtSites, "InventSiteId", "InventSiteId", selectedSite);
        //    }

        //    // -------------------------
        //    // Warehouse
        //    // -------------------------
        //    DropDownList ddlWarehouse = e.Row.FindControl("ddlWarehouse") as DropDownList;
        //    if (ddlWarehouse != null)
        //    {
        //        DataTable dtWarehouse = svc.retrieveWarehouse(); // must return InventLocationId
        //        string selectedWarehouse = drv["InventLocationId"] != DBNull.Value ? drv["InventLocationId"].ToString() : "";
        //        BindDropdown(ddlWarehouse, dtWarehouse, "InventLocationId", "InventLocationId", selectedWarehouse);
        //    }
        //}


        //protected void gvSalesOrderLines_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType != DataControlRowType.DataRow) return;

        //    DataRowView drv = e.Row.DataItem as DataRowView;
        //    SalesOrder svc = new SalesOrder();

        //    //void BindDropdown(DropDownList ddl, DataTable dt, string textField, string valueField, string selectedValue)
        //    //{
        //    //    ddl.DataSource = dt;
        //    //    ddl.DataTextField = textField;
        //    //    ddl.DataValueField = valueField;
        //    //    ddl.DataBind();
        //    //    ddl.Items.Insert(0, new ListItem("--Select--", ""));
        //    //    if (!string.IsNullOrEmpty(selectedValue))
        //    //    {
        //    //        if (ddl.Items.FindByValue(selectedValue) != null)
        //    //            ddl.SelectedValue = selectedValue;
        //    //        else
        //    //        {
        //    //            ddl.Items.Insert(1, new ListItem(selectedValue, selectedValue));
        //    //            ddl.SelectedValue = selectedValue;
        //    //        }
        //    //    }
        //    //}
        //    void BindDropdown(DropDownList ddl, DataTable dt, string textField, string valueField, string selectedValue)
        //    {
        //        ddl.Items.Clear(); // VERY IMPORTANT

        //        ddl.DataSource = dt;
        //        ddl.DataTextField = textField;
        //        ddl.DataValueField = valueField;
        //        ddl.DataBind();

        //        ddl.Items.Insert(0, new ListItem("--Select--", ""));

        //        if (!string.IsNullOrEmpty(selectedValue))
        //        {
        //            ListItem item = ddl.Items.FindByValue(selectedValue);

        //            if (item != null)
        //            {
        //                ddl.ClearSelection();
        //                item.Selected = true;
        //            }
        //        }
        //    }

        //    DropDownList ddlItemNumber = e.Row.FindControl("ddlItemNumber") as DropDownList;
        //    if (ddlItemNumber != null)
        //        BindDropdown(ddlItemNumber, svc.retrieveItemNumber(), "ItemId", "ItemId", drv["ItemId"].ToString());

        //    DropDownList ddlSite = e.Row.FindControl("ddlSite") as DropDownList;
        //    if (ddlSite != null)
        //        BindDropdown(ddlSite, svc.retrieveSite(), "InventSiteId", "InventSiteId", drv["InventSiteId"].ToString());

        //    DropDownList ddlUnit = e.Row.FindControl("ddlUnit") as DropDownList;
        //    if (ddlUnit != null)
        //        BindDropdown(ddlUnit, svc.retrieveUnit(), "unit", "unit", drv["SalesUnitId"]?.ToString());

        //    DropDownList ddlWarehouse = e.Row.FindControl("ddlWarehouse") as DropDownList;
        //    if (ddlWarehouse != null)
        //        BindDropdown(ddlWarehouse, svc.retrieveWarehouse(), "InventLocationId", "InventLocationId", drv["InventLocationId"].ToString());
        //    LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
        //    LinkButton btnCancel = e.Row.FindControl("btnCancel") as LinkButton;
        //    Label lblRecId = e.Row.FindControl("lblRecId") as Label;

        //    if (btnEdit != null)
        //        btnEdit.Enabled = false;

        //    if (btnCancel != null)
        //        btnCancel.Enabled = false;

        //    // Enable Edit button only for selected RecId
        //    if (lblRecId != null && Session["SelectedRecId"] != null)
        //    {
        //        if (lblRecId.Text == Session["SelectedRecId"].ToString())
        //        {
        //            if (btnEdit != null)
        //                btnEdit.Enabled = true;
        //        }
        //    }
        //}


        //updated

        protected void gvSalesOrderLines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            DataRowView drv = e.Row.DataItem as DataRowView;
            SalesOrder svc = new SalesOrder();

            void BindDropdown(DropDownList ddl, DataTable dt, string textField,
                              string valueField, string selectedValue)
            {
                ddl.Items.Clear();
                ddl.DataSource = dt;
                ddl.DataTextField = textField;
                ddl.DataValueField = valueField;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--Select--", ""));
                if (!string.IsNullOrEmpty(selectedValue))
                {
                    ListItem item = ddl.Items.FindByValue(selectedValue);
                    if (item != null)
                    {
                        ddl.ClearSelection();
                        item.Selected = true;
                    }
                }
            }

            // Item Number
            DropDownList ddlItemNumber = e.Row.FindControl("ddlItemNumber") as DropDownList;
            if (ddlItemNumber != null)
                BindDropdown(ddlItemNumber, svc.retrieveItemNumber(), "ItemId", "ItemId",
                    drv["ItemId"]?.ToString());

            // ✅ Updated to ddlgridSite
            DropDownList ddlgridSite = e.Row.FindControl("ddlgridSite") as DropDownList;
            if (ddlgridSite != null)
                BindDropdown(ddlgridSite, svc.retrieveSite(), "InventSiteId", "InventSiteId",
                    drv["InventSiteId"]?.ToString());

            // Unit
            DropDownList ddlUnit = e.Row.FindControl("ddlUnit") as DropDownList;
            if (ddlUnit != null)
                BindDropdown(ddlUnit, svc.retrieveUnit(), "unit", "unit",
                    drv["SalesUnitId"]?.ToString());

            // ✅ Updated to ddlgridWarehouse
            DropDownList ddlgridWarehouse = e.Row.FindControl("ddlgridWarehouse") as DropDownList;
            if (ddlgridWarehouse != null)
                BindDropdown(ddlgridWarehouse, svc.retrieveWarehouse(), "InventLocationId",
                    "InventLocationId", drv["InventLocationId"]?.ToString());

            //// Edit/Cancel buttons
            //LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
            //LinkButton btnCancel = e.Row.FindControl("btnCancel") as LinkButton;
            //Label lblRecId = e.Row.FindControl("lblRecId") as Label;

            //if (btnEdit != null) btnEdit.Enabled = false;
            //if (btnCancel != null) btnCancel.Enabled = false;

            //if (lblRecId != null && Session["SelectedRecId"] != null)
            //{
            //    if (lblRecId.Text == Session["SelectedRecId"].ToString())
            //    {
            //        if (btnEdit != null) btnEdit.Enabled = true;
            //    }
            //}

            // At the bottom of gvSalesOrderLines_RowDataBound, replace the existing
            // Edit/Cancel enable logic with this:

            LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
            LinkButton btnCancel = e.Row.FindControl("btnCancel") as LinkButton;
            Label lblRecId = e.Row.FindControl("lblRecId") as Label;

            if (btnEdit != null) btnEdit.Enabled = false;
            if (btnCancel != null) btnCancel.Enabled = false;

            if (lblRecId != null)
            {
                long.TryParse(lblRecId.Text, out long rowRecId);

                // Enable for selected existing row OR for the new unsaved row (recId=0)
                bool isSelected = Session["SelectedRecId"] != null &&
                                  lblRecId.Text == Session["SelectedRecId"].ToString();
                bool isNewRow = rowRecId == 0 && Session["SelectedRecId"] != null &&
                                  Session["SelectedRecId"].ToString() == "0";

                if (isSelected || isNewRow)
                {
                    if (btnEdit != null) btnEdit.Enabled = true;
                    if (btnCancel != null) btnCancel.Enabled = true;
                }
            }
        }







        //protected void btnNew_Grid_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = GetGridTable();

        //    DataRow dr = dt.NewRow();

        //    dr["RetailVariantId"] = "";
        //    dr["ItemId"] = "";
        //    dr["itemName"] = "";
        //    dr["category"] = "";
        //    dr["SalesQty"] = 0;
        //    dr["SalesUnitId"] = "";
        //    dr["DeliveryType"] = "";
        //    dr["InventSiteId"] = "";
        //    dr["InventLocationId"] = "";

        //    dt.Rows.InsertAt(dr, 0);

        //    Session["SalesLinesUI"] = dt;

        //    gvSalesOrderLines.EditIndex = 0;
        //    gvSalesOrderLines.DataSource = dt;
        //    gvSalesOrderLines.DataBind();
        //}


        //  previous code
        //protected void btnNew_Grid_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = GetGridTable();

        //    DataRow dr = dt.NewRow();
        //    dr["RetailVariantId"] = "";
        //    dr["ItemId"] = "";
        //    dr["itemName"] = "";
        //    dr["category"] = "";
        //    dr["SalesQty"] = 0;
        //    dr["SalesUnitId"] = "";
        //    dr["DeliveryType"] = "";
        //    dr["InventSiteId"] = "";
        //    dr["InventLocationId"] = "";

        //    dt.Rows.InsertAt(dr, 0);
        //    Session["SalesLinesUI"] = dt;

        //    gvSalesOrderLines.EditIndex = 0;
        //    gvSalesOrderLines.DataSource = dt;
        //    gvSalesOrderLines.DataBind();

        //    // Enable Edit/Cancel buttons for the new row automatically
        //    GridViewRow row = gvSalesOrderLines.Rows[0];
        //    LinkButton btnEdit = row.FindControl("btnEdit") as LinkButton;
        //    LinkButton btnCancel = row.FindControl("btnCancel") as LinkButton;

        //    if (btnEdit != null) btnEdit.Enabled = true;
        //    if (btnCancel != null) btnCancel.Enabled = true;
        //}

        // updated code 
        protected void btnNew_Grid_Click(object sender, EventArgs e)
        {
            DataTable dt = GetGridTable();

            DataRow dr = dt.NewRow();
            dr["recId"] = 0L;           // ← 0 = new row sentinel
            dr["RetailVariantId"] = "";
            dr["ItemId"] = "";
            dr["itemName"] = "";
            dr["category"] = "";
            dr["SalesQty"] = 0;
            dr["SalesUnitId"] = "";
            dr["DeliveryType"] = "";
            dr["InventSiteId"] = "";
            dr["InventLocationId"] = "";
            dr["SalesPrice"] = 0;
            dr["SalesLineDisc"] = 0;
            dr["linepercent"] = 0;
            dr["LineAmount"] = 0;

            dt.Rows.InsertAt(dr, 0);
            Session["SalesLinesUI"] = dt;
            Session["SelectedRecId"] = 0L;   // ← flag so RowDataBound enables Edit/Save

            // NO EditIndex — keep grid in normal mode so ItemTemplate renders everywhere
            gvSalesOrderLines.EditIndex = -1;
            gvSalesOrderLines.RowDataBound += gvSalesOrderLines_RowDataBound;
            gvSalesOrderLines.DataSource = dt;
            gvSalesOrderLines.DataBind();
        }
        //protected void Update_Click(object sender, EventArgs e)
        //{
        //    SysOperationResult_BOL result = new SysOperationResult_BOL();

        //    if (gvSalesOrderLines.EditIndex < 0)
        //        return;

        //    GridViewRow gridRow = gvSalesOrderLines.Rows[gvSalesOrderLines.EditIndex];

        //    try
        //    {
        //        Label lblRecId = (Label)gridRow.FindControl("lblRecId");

        //        long recId = 0;
        //        if (lblRecId != null && !string.IsNullOrWhiteSpace(lblRecId.Text))
        //            long.TryParse(lblRecId.Text, out recId);

        //        TextBox txtItemId = (TextBox)gridRow.FindControl("txtItemNumber");
        //        TextBox txtQuantity = (TextBox)gridRow.FindControl("txtQuantity");
        //        DropDownList ddlUnit = (DropDownList)gridRow.FindControl("ddlUnit");
        //        DropDownList ddlSite = (DropDownList)gridRow.FindControl("ddlSite");
        //        DropDownList ddlWarehouse = (DropDownList)gridRow.FindControl("ddlWarehouse");

        //        if (txtItemId == null || txtQuantity == null)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(),
        //                "alert",
        //                "alert('Invalid row controls');",
        //                true);
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(txtItemId.Text) ||
        //            string.IsNullOrWhiteSpace(txtQuantity.Text))
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(),
        //                "alert",
        //                "alert('Please Fill Required Fields');",
        //                true);
        //            return;
        //        }

        //        SalesOrder salesLine = new SalesOrder();

        //        // =============================
        //        // 🔹 CREATE MODE
        //        // =============================
        //        if (recId == 0)
        //        {
        //            SalesOrderContract contract = new SalesOrderContract();

        //            contract.SalesId = Session["SalesId"] as string;
        //            contract.ItemId = txtItemId.Text.Trim();
        //            contract.SalesQty = Convert.ToDecimal(txtQuantity.Text);
        //            //contract.SalesUnitId = ddlUnit?.SelectedValue;
        //            contract.InventSiteId = ddlSite.SelectedValue;
        //            contract.InventLocationId = ddlWarehouse.SelectedValue;

        //            salesLine.createlines(contract);

        //            result.isSuccess = true;
        //            result.Message = "Sales line created successfully.";
        //            result.AlertType = AlertType.Success.ToString();
        //        }
        //        // =============================
        //        // 🔹 UPDATE MODE
        //        // =============================
        //        else
        //        {
        //            DataTable dtUpdate = new DataTable();

        //            dtUpdate.Columns.Add("RecId", typeof(long));
        //            dtUpdate.Columns.Add("SalesQty", typeof(decimal));
        //            dtUpdate.Columns.Add("SalesUnitId");
        //            dtUpdate.Columns.Add("InventSiteId");
        //            dtUpdate.Columns.Add("InventLocationId");

        //            DataRow row = dtUpdate.NewRow();

        //            row["RecId"] = recId;
        //            row["SalesQty"] = Convert.ToDecimal(txtQuantity.Text);
        //            row["SalesUnitId"] = ddlUnit?.SelectedValue;
        //            row["InventSiteId"] = ddlSite?.SelectedValue;
        //            row["InventLocationId"] = ddlWarehouse?.SelectedValue;

        //            dtUpdate.Rows.Add(row);

        //           // salesLine.update(dtUpdate);   // <-- make sure update method exists

        //            result.isSuccess = true;
        //            result.Message = "Sales line updated successfully.";
        //            result.AlertType = AlertType.Success.ToString();
        //        }

        //        NotificationMessage.showMessage(result);

        //        gvSalesOrderLines.EditIndex = -1;
        //        LoadSalesOrders();
        //    }
        //    catch (Exception ex)
        //    {
        //        result.isSuccess = false;
        //        result.Message = "An error occurred: " + ex.Message;
        //        result.AlertType = AlertType.Error.ToString();
        //        NotificationMessage.showMessage(result);
        //    }
        //}


        //protected void Update_Click(object sender, EventArgs e)
        //{
        //    if (gvSalesOrderLines.EditIndex < 0) return;
        //    GridViewRow row = gvSalesOrderLines.Rows[gvSalesOrderLines.EditIndex];
        //    try
        //    {
        //        Label lblRecId = row.FindControl("lblRecId") as Label;
        //        long recId = lblRecId != null && long.TryParse(lblRecId.Text, out long tmp) ? tmp : 0;
        //        DropDownList ddlItemNumber = row.FindControl("ddlItemNumber") as DropDownList;
        //        TextBox txtQuantity = row.FindControl("txtGridQty") as TextBox;
        //        DropDownList ddlSitelines = row.FindControl("ddlgridSite") as DropDownList;
        //        DropDownList ddlWarehouselines = row.FindControl("ddlgridWarehouse") as DropDownList;

        //        // 🔹 VALIDATION: ItemNumber AND Site REQUIRED
        //        if (ddlItemNumber == null || ddlSite == null ||
        //            string.IsNullOrWhiteSpace(ddlItemNumber.SelectedValue) ||
        //            string.IsNullOrWhiteSpace(ddlSite.SelectedValue))
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(),
        //                "alert",
        //                "alert('Please fill required fields: Item Number and Site');",
        //                true);
        //            return;
        //        }


        //        SalesOrder salesLine = new SalesOrder();

        //        if (recId == 0)
        //        {
        //            SalesOrderContract contract = new SalesOrderContract();


        //            contract.SalesId = Session["SalesId"] as string;
        //            contract.ItemId = ddlItemNumber.SelectedValue;
        //            contract.SalesQty = Convert.ToDecimal(txtQuantity.Text);
        //            contract.InventSiteId = ddlSitelines.SelectedValue;
        //            contract.InventLocationId = ddlWarehouselines.SelectedValue;


        //            salesLine.createlines(contract);
        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //        "success",
        //        "alert('Line Created Successfully');",
        //        true);
        //        }
        //        else
        //        {

        //            SalesOrderContract contract = new SalesOrderContract();
        //            contract.recId = recId;

        //          //  salesLine.update(contract);

        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //                "success",
        //                "alert('Line Updated Successfully');",
        //                true);
        //        }

        //        gvSalesOrderLines.EditIndex = -1;
        //        LoadSalesOrders();
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Error: {ex.Message}');", true);
        //    }
        //}




        protected void Update_Click(object sender, EventArgs e)
        {
            if (gvSalesOrderLines.EditIndex < 0) return;
            GridViewRow row = gvSalesOrderLines.Rows[gvSalesOrderLines.EditIndex];
            try
            {
                Label lblRecId = row.FindControl("lblRecId") as Label;
                long recId = lblRecId != null && long.TryParse(lblRecId.Text, out long tmp) ? tmp : 0;

                DropDownList ddlItemNumber = row.FindControl("ddlItemNumber") as DropDownList;
                TextBox txtQuantity = row.FindControl("txtGridQty") as TextBox;
                DropDownList ddlSitelines = row.FindControl("ddlgridSite") as DropDownList;
                DropDownList ddlWarehouselines = row.FindControl("ddlgridWarehouse") as DropDownList;

                // ✅ FIXED: was referencing ddlSite (header control) — now uses ddlSitelines
                if (ddlItemNumber == null || ddlSitelines == null ||
                    string.IsNullOrWhiteSpace(ddlItemNumber.SelectedValue) ||
                    string.IsNullOrWhiteSpace(ddlSitelines.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "alert",
                        "alert('Please fill required fields: Item Number and Site');",
                        true);
                    return;
                }

                SalesOrder salesLine = new SalesOrder();

                if (recId == 0)
                {
                    // ✅ CREATE MODE
                    SalesOrderContract contract = new SalesOrderContract();
                    contract.SalesId = Session["SalesId"] as string;
                    contract.ItemId = ddlItemNumber.SelectedValue;
                    contract.SalesQty = string.IsNullOrWhiteSpace(txtQuantity?.Text)
                                                ? 1
                                                : Convert.ToDecimal(txtQuantity.Text);
                    contract.InventSiteId = ddlSitelines?.SelectedValue;
                    contract.InventLocationId = ddlWarehouselines?.SelectedValue;

                    DataTable result = salesLine.createlines(contract);

                    // ✅ Check if creation succeeded
                    if (result != null && result.Rows.Count > 0)
                    {
                        string isSuccess = result.Rows[0]["IsSuccess"]?.ToString();
                        string message = result.Rows[0]["Message"]?.ToString();

                        if (isSuccess == "True" || isSuccess == "1")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                "success",
                                $"alert('Line Created Successfully');",
                                true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                "error",
                                $"alert('Create failed: {message?.Replace("'", "")}');",
                                true);
                            return; // ✅ Don't reload grid if failed
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "error",
                            "alert('Create failed: No response from service.');",
                            true);
                        return;
                    }
                }
                else
                {
                    // ✅ UPDATE MODE
                    SalesOrderContract contract = new SalesOrderContract();
                    contract.recId = recId;
                    contract.SalesId = Session["SalesId"] as string;
                    contract.ItemId = ddlItemNumber?.SelectedValue;
                    contract.SalesQty = string.IsNullOrWhiteSpace(txtQuantity?.Text)
                                                ? 0
                                                : Convert.ToDecimal(txtQuantity.Text);
                    contract.InventSiteId = ddlSitelines?.SelectedValue;
                    contract.InventLocationId = ddlWarehouselines?.SelectedValue;

                    salesLine.update(contract);

                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "success",
                        "alert('Line Updated Successfully');",
                        true);
                }

                gvSalesOrderLines.EditIndex = -1;
                LoadSalesOrders();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "error",
                    $"alert('Error: {ex.Message.Replace("'", "")}');",
                    true);
            }
        }
        //protected void ddlItemNumber_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)sender;

        //    GridViewRow row = (GridViewRow)ddl.NamingContainer;

        //    Label lblProductName = row.FindControl("lblProductName") as Label;

        //    string selectedItemId = ddl.SelectedValue;

        //    if (!string.IsNullOrEmpty(selectedItemId))
        //    {
        //        SalesOrder svc = new SalesOrder();

        //        SalesOrderContract result = svc.retrieveItemName(selectedItemId);

        //        if (result != null)
        //        {
        //            lblProductName.Text = result.itemName;
        //        }
        //    }
        //}

        private void showFinancialDimension(long _defaultDimensionRecId)
        {
            Int64 workerDimension = 0;
            ESSFinancialDimensions finDim = new ESSFinancialDimensions();

            if (workerDimension == 0)
            {
                try
                {
                    DataContract[] dimensions = finDim.retrieveActiveDimensions();

                    if (dimensions != null && dimensions.Length > 0)
                    {
                        // Container where you want to add rows
                        // (put a <div runat="server" id="financialDimensionsContainer"></div> in your .aspx)
                        financialDimensionsContainer.Controls.Clear();

                        int colCount = 0;
                        HtmlGenericControl rowDiv = null;

                        foreach (var dim in dimensions)
                        {
                            if (colCount % 2 == 0)
                            {
                                // Start a new row after every two blocks
                                rowDiv = new HtmlGenericControl("div");
                                rowDiv.Attributes["class"] = "info-row";
                                financialDimensionsContainer.Controls.Add(rowDiv);
                            }

                            // Create info-block
                            HtmlGenericControl blockDiv = new HtmlGenericControl("div");
                            blockDiv.Attributes["class"] = "info-block";

                            // Strong title
                            HtmlGenericControl strong = new HtmlGenericControl("strong");
                            strong.InnerText = dim.Code;   // e.g. Business Unit / Cost Center
                            blockDiv.Controls.Add(strong);

                            // Span wrapper
                            HtmlGenericControl span = new HtmlGenericControl("span");

                            //// Label
                            //Label lbl = new Label();
                            //lbl.ID = $"FinancialDimension{dim.Code}";
                            //lbl.Text = "N/A";
                            //span.Controls.Add(lbl);

                            // Dropdown
                            DropDownList ddl = new DropDownList();
                            ddl.ID = $"ddlFinancialDimension{dim.Code}";
                            ddl.CssClass = "form-control";
                            ddl.Visible = true;

                            // Populate dropdown from SOAP lookup
                            DataTable dimeVal = finDim.retrieveDimensionLookUp(dim.Code);
                            ddl.Items.Add(new ListItem("", ""));
                            foreach (DataRow row in dimeVal.Rows)
                            {
                                string value = row["Value1"].ToString();
                                string valueName = row["Value2"].ToString();
                                ddl.Items.Add(new ListItem($"{value} - {valueName}", value));
                            }

                            span.Controls.Add(ddl);
                            blockDiv.Controls.Add(span);

                            // Add block to row
                            rowDiv.Controls.Add(blockDiv);

                            colCount++;
                        }
                       //bindItemDimension(_defaultDimensionRecId);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to build dynamic dimensions", ex);
                }
            }

        }






        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;

            // Only store recId if checkbox is checked
            if (chk.Checked)
            {
                Label lblRecId = row.FindControl("lblRecId") as Label;
                DropDownList ddlItemNumber = row.FindControl("ddlItemNumber") as DropDownList;

                if (lblRecId != null && long.TryParse(lblRecId.Text, out long recId))
                {
                    // Save selected recId to session
                    Session["SelectedRecId"] = recId;
                    if (ddlItemNumber != null)
                    {
                        string selectedItemId = ddlItemNumber.SelectedValue;

                        // 🔥 Store ItemId in Session
                        Session["SelectedItemId"] = selectedItemId;

                        BindSize(selectedItemId);
                        BindSiteline();
                        BindWarehouseline();
                        BindDeliveryMode();
                        BindDeliveryterm();
                        LoadReservationDropdown();  
                        BindSalesGroup();  
                        Bindtaxitemline();
                        Bindsalestaxline(); 
                       
                        BindSalesOrderlinedetail();

                    }
                }
            }
            else
            {
                // Optionally, remove from session if unchecked
                Session["SelectedRecId"] = null;
                Session["SelectedItemId"] = null;
            }
        }




        //protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox chk = (CheckBox)sender;
        //    GridViewRow row = (GridViewRow)chk.NamingContainer;

        //    // Disable all rows first
        //    foreach (GridViewRow r in gvSalesOrderLines.Rows)
        //    {
        //        LinkButton btnEdit = r.FindControl("btnEdit") as LinkButton;
        //        if (btnEdit != null)
        //            btnEdit.Enabled = false;
        //    }

        //    if (chk.Checked)
        //    {
        //        LinkButton btnEdit = row.FindControl("btnEdit") as LinkButton;
        //        if (btnEdit != null)
        //            btnEdit.Enabled = true;

        //        Label lblRecId = row.FindControl("lblRecId") as Label;
        //        if (lblRecId != null)
        //            Session["SelectedRecId"] = lblRecId.Text;
        //    }
        //    else
        //    {
        //        Session["SelectedRecId"] = null;
        //    }
        //}
        protected void gvSalesOrderLines_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSalesOrderLines.EditIndex = e.NewEditIndex;

            GridViewRow row = gvSalesOrderLines.Rows[e.NewEditIndex];
            Label lblRecId = row.FindControl("lblRecId") as Label;

            if (lblRecId != null && long.TryParse(lblRecId.Text, out long recId))
            {
                Session["SelectedRecId"] = recId;
            }

            LoadSalesOrders();
        }
        protected void gvSalesOrderLines_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSalesOrderLines.EditIndex = -1;
            Session["SelectedRecId"] = null;
            LoadSalesOrders();
        }

        //previous code 
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // 1️⃣ Check if any row is selected
        //        if (Session["SelectedRecId"] == null)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //                "alert",
        //                "alert('Please select a row to update.');",
        //                true);
        //            return;
        //        }

        //        long selectedRecId;
        //        if (!long.TryParse(Session["SelectedRecId"].ToString(), out selectedRecId))
        //            return;

        //        GridViewRow selectedRow = null;

        //        // 2️⃣ Find the selected row in GridView
        //        foreach (GridViewRow row in gvSalesOrderLines.Rows)
        //        {
        //            Label lblRecId = row.FindControl("lblRecId") as Label;

        //            if (lblRecId != null && long.TryParse(lblRecId.Text, out long rowRecId))
        //            {
        //                if (rowRecId == selectedRecId)
        //                {
        //                    selectedRow = row;
        //                    break;
        //                }
        //            }
        //        }

        //        if (selectedRow == null)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //                "alert",
        //                "alert('Selected row not found.');",
        //                true);
        //            return;
        //        }

        //        // 3️⃣ Get updated values from selected row controls
        //        DropDownList ddlItemNumber = selectedRow.FindControl("ddlItemNumber") as DropDownList;
        //        TextBox txtQuantity = selectedRow.FindControl("txtQuantity") as TextBox;
        //        DropDownList ddlSite = selectedRow.FindControl("ddlSite") as DropDownList;
        //        DropDownList ddlWarehouse = selectedRow.FindControl("ddlWarehouse") as DropDownList;
        //        if (string.IsNullOrWhiteSpace(ddlSize.SelectedValue))
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //                "alert",
        //                "alert('Please select Size before updating.');",
        //                true);
        //            return;
        //        }

        //        // 4️⃣ Create contract
        //        SalesOrderContract contract = new SalesOrderContract();

        //        contract.recId = selectedRecId; // 🔥 VERY IMPORTANT

        //        contract.SalesId = Session["SalesId"] as string;
        //        contract.ItemId = ddlItemNumber?.SelectedValue;
        //        contract.SalesQty = string.IsNullOrWhiteSpace(txtQuantity?.Text)
        //                            ? 0
        //                            : Convert.ToDecimal(txtQuantity.Text);

        //        contract.InventSiteId = ddlSite?.SelectedValue;
        //        contract.InventLocationId = ddlWarehouse?.SelectedValue;
        //        contract.InventSizeId = ddlSize.SelectedValue; 

        //        // 5️⃣ Call service
        //        SalesOrder svc = new SalesOrder();
        //        svc.update(contract);

        //        ScriptManager.RegisterStartupScript(this, GetType(),
        //            "success",
        //            "alert('Line updated successfully.');",
        //            true);

        //        // Clear selection
        //        Session["SelectedRecId"] = null;

        //        // Reload grid
        //        LoadSalesOrders();
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(),
        //            "error",
        //            $"alert('Error: {ex.Message.Replace("'", "")}');",
        //            true);
        //    }
        //}

        //updated code 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["SelectedRecId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        "alert('Please select or add a row first.');", true);
                    return;
                }

                long.TryParse(Session["SelectedRecId"].ToString(), out long selectedRecId);
                string selectedRecIdStr = Session["SelectedRecId"].ToString(); // ✅ keep as string for comparison

                // ✅ Find the selected row by matching label TEXT (handles recId=0 safely)
                GridViewRow selectedRow = null;
                foreach (GridViewRow row in gvSalesOrderLines.Rows)
                {
                    Label lblRecId = row.FindControl("lblRecId") as Label;
                    if (lblRecId != null && lblRecId.Text == selectedRecIdStr)
                    {
                        selectedRow = row;
                        break;
                    }
                }

                if (selectedRow == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        "alert('Row not found in grid. Please click Add Line again.');", true);
                    return;
                }

                DropDownList ddlItemNumber = selectedRow.FindControl("ddlItemNumber") as DropDownList;
                DropDownList ddlSitelines = selectedRow.FindControl("ddlgridSite") as DropDownList;
                DropDownList ddlWarehouselines = selectedRow.FindControl("ddlgridWarehouse") as DropDownList;
                TextBox txtQuantity = selectedRow.FindControl("txtGridQty") as TextBox;

                if (ddlItemNumber == null || string.IsNullOrWhiteSpace(ddlItemNumber.SelectedValue) ||
                    ddlSitelines == null || string.IsNullOrWhiteSpace(ddlSitelines.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        "alert('Item Number and Site are required.');", true);
                    return;
                }

                SalesOrder svc = new SalesOrder();

                if (selectedRecId == 0)
                {
                    // ── CREATE ──────────────────────────────────────────────────
                    SalesOrderContract contract = new SalesOrderContract();
                    contract.SalesId = Session["SalesId"] as string;
                    contract.ItemId = ddlItemNumber.SelectedValue;
                    contract.SalesQty = string.IsNullOrWhiteSpace(txtQuantity?.Text)
                                                ? 1m : Convert.ToDecimal(txtQuantity.Text);
                    contract.InventSiteId = ddlSitelines.SelectedValue;
                    contract.InventLocationId = ddlWarehouselines?.SelectedValue;

                    DataTable result = svc.createlines(contract);

                    if (result != null && result.Rows.Count > 0 &&
                        (result.Rows[0]["IsSuccess"]?.ToString() == "True" ||
                         result.Rows[0]["IsSuccess"]?.ToString() == "1"))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "success",
                            "alert('Line created successfully.');", true);

                        // ✅ Clear the in-memory table so LoadSalesOrders fetches fresh from DB
                        Session["SalesLinesUI"] = null;
                    }
                    else
                    {
                        string msg = result?.Rows.Count > 0
                            ? result.Rows[0]["Message"]?.ToString() : "No response from service.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "error",
                            $"alert('Create failed: {msg?.Replace("'", "")}');", true);
                        return;
                    }
                }
                else
                {
                    // ── UPDATE ──────────────────────────────────────────────────
                    SalesOrderContract contract = new SalesOrderContract();
                    contract.recId = selectedRecId;
                    contract.SalesId = Session["SalesId"] as string;
                    contract.ItemId = ddlItemNumber.SelectedValue;
                    contract.SalesQty = string.IsNullOrWhiteSpace(txtQuantity?.Text)
                                                ? 0m : Convert.ToDecimal(txtQuantity.Text);
                    contract.InventSiteId = ddlSitelines.SelectedValue;
                    contract.InventLocationId = ddlWarehouselines?.SelectedValue;
                    contract.InventSizeId = ddlSize.SelectedValue;

                    svc.update(contract);
                    ScriptManager.RegisterStartupScript(this, GetType(), "success",
                        "alert('Line updated successfully.');", true);

                    Session["SalesLinesUI"] = null; // ✅ Force fresh reload
                }

                Session["SelectedRecId"] = null;
                LoadSalesOrders(); // ✅ Now loads fresh from DB with correct RecIds
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    $"alert('Error: {ex.Message.Replace("'", "")}');", true);
            }
        }
        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = GetGridTable();

        //    // Collect RecIds of selected rows
        //    List<long> selectedRecIds = new List<long>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        if (dt.Columns.Contains("Selected") && row["Selected"] != DBNull.Value && (bool)row["Selected"])
        //        {
        //            if (row.Table.Columns.Contains("recId") && long.TryParse(row["recId"].ToString(), out long recId))
        //            {
        //                selectedRecIds.Add(recId);
        //            }
        //        }
        //    }

        //    if (selectedRecIds.Count == 0)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Please select at least one row to delete.');", true);
        //        return;
        //    }

        //    try
        //    {
        //        // Call your service method
        //        SalesOrder salesOrder = new SalesOrder();
        //        SysOperationResult_BOL result = salesOrder.deletelines(selectedRecIds.ToArray());

        //        if (result.isSuccess)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //                $"alert('{result.Message}');", true);

        //            // Remove deleted rows from DataTable
        //            for (int i = dt.Rows.Count - 1; i >= 0; i--)
        //            {
        //                if (selectedRecIds.Contains(Convert.ToInt64(dt.Rows[i]["RecId"])))
        //                    dt.Rows.RemoveAt(i);
        //            }

        //            Session["SalesLinesUI"] = dt;

        //            gvSalesOrderLines.EditIndex = -1;
        //            gvSalesOrderLines.DataSource = dt;
        //            gvSalesOrderLines.DataBind();

        //            // Disable Delete button after deletion

        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //                $"alert('Failed to delete: {result.Message}');", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //            $"alert('Error: {ex.Message}');", true);
        //    }
        //}

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (Session["SelectedRecId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Please select a row to delete.');", true);
                return;
            }

            long recId = Convert.ToInt64(Session["SelectedRecId"]);

            // ✅ Block deleting an unsaved new row (recId=0 doesn't exist in DB)
            if (recId == 0)
            {
                // Just remove it from the in-memory table and rebind
                DataTable dt = GetGridTable();
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToInt64(dt.Rows[i]["recId"]) == 0) // ✅ lowercase 'recId'
                        dt.Rows.RemoveAt(i);
                }
                Session["SalesLinesUI"] = dt;
                Session["SelectedRecId"] = null;
                Session["SelectedItemId"] = null;

                gvSalesOrderLines.RowDataBound += gvSalesOrderLines_RowDataBound;
                gvSalesOrderLines.DataSource = dt;
                gvSalesOrderLines.DataBind();
                return;
            }

            try
            {
                SalesOrder salesOrder = new SalesOrder();
                SysOperationResult_BOL result = salesOrder.deletelines(new long[] { recId });

                if (result.isSuccess)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        $"alert('{result.Message}');", true);

                    Session["SalesLinesUI"] = null; // ✅ Force fresh reload from DB
                    Session["SelectedRecId"] = null;
                    Session["SelectedItemId"] = null;

                    LoadSalesOrders();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        $"alert('Failed to delete: {result.Message}');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    $"alert('Error: {ex.Message}');", true);
            }
        }



        //protected void BtnSave_Header_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SalesOrder svc = new SalesOrder();

        //        // 🔥 1️⃣ IF LINE IS SELECTED → UPDATE LINE
        //        if (Session["SelectedRecId"] != null)
        //        {
        //            if (!long.TryParse(Session["SelectedRecId"].ToString(), out long lineRecId))
        //                return;

        //            SalesOrderContract contract = new SalesOrderContract();

        //            contract.recId = lineRecId;
        //            contract.InventSiteId = txtSite.Text;
        //            //contract.External = txtExternal.Text;
        //            //contract.LineNumber = txtLineNumber.Text;
        //            //contract.Stopped = chkStopped.Checked;
        //            //contract.PreventPartialDelivery = chkPreventPartialDelivery.Checked;
        //            //contract.ExcludeFromDOM = chkExcludeFromDOM.Checked;

        //            svc.updateLineDetail(contract);

        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //                "success",
        //                "alert('Line Details Updated Successfully');",
        //                true);

        //            Session["SelectedRecId"] = null;
        //            return;
        //        }

        //        // 🔥 2️⃣ OTHERWISE CHECK HEADER SELECTION
        //        if (Session["SalesOrderRecId"] == null)
        //        {
        //            //ScriptManager.RegisterStartupScript(this, GetType(),
        //            //    "alert",
        //            //    "alert('Please select a Sales Order to update.');",
        //            //    true);
        //            return;
        //        }

        //        List<string> selectedSales = (List<string>)Session["SalesOrderRecId"];

        //        if (selectedSales.Count == 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(),
        //                "alert",
        //                "alert('Please select a Sales Order to update.');",
        //                true);
        //            return;
        //        }

        //        // 🔥 Loop through selected sales orders
        //        foreach (string recIdStr in selectedSales)
        //        {
        //            if (!long.TryParse(recIdStr, out long headerRecId))
        //                continue;

        //            SalesOrderContract headerContract = new SalesOrderContract();

        //            //headerContract.RecId = headerRecId;
        //            //headerContract.SalesId = txtSalesOrder.Text;
        //            headerContract.salesName = txtCustomerName.Text;
        //            //headerContract.OrderType = txtOrderType.Text;
        //            //headerContract.RetailSale = chkRetailSale.Checked;
        //            //headerContract.ContinuityOrder = chkContinuityOrder.Checked;
        //            //headerContract.OneTimeCustomer = chkOneTimeCustomer.Checked;
        //            //headerContract.Contact = txtContact.Text;
        //            //headerContract.InternetAddress = txtInternetAddress.Text;
        //            //headerContract.Email = txtEmail.Text;
        //            //headerContract.Telephone = txtTelephone.Text;

        //            //svc.updateHeader(headerContract);
        //        }

        //        ScriptManager.RegisterStartupScript(this, GetType(),
        //            "success",
        //            "alert('Header Updated Successfully');",
        //            true);

        //        Session["SalesOrderRecId"] = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(),
        //            "error",
        //            $"alert('Error: {ex.Message}');",
        //            true);
        //    }
        //}


        protected void BtnSave_Header_Click(object sender, EventArgs e)
        {
            try
            {
                SalesOrder svc = new SalesOrder();

                // =====================================================
                // 🔥 1️⃣ IF LINE IS SELECTED → UPDATE LINE
                // =====================================================
                if (Session["SelectedRecId"] != null)
                {
                    if (!long.TryParse(Session["SelectedRecId"].ToString(), out long lineRecId))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "error",
                            "alert('Invalid Line RecId.');",
                            true);
                        return;
                    }

                    SalesOrderContract lineContract = new SalesOrderContract();
                    lineContract.recId = lineRecId;
                    lineContract.InventSiteId = ddlsiteline.SelectedValue;
                    lineContract.InventSizeId = ddlSize.SelectedValue;

                   // svc.update(lineContract);

                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "success",
                        "alert('Line Details Updated Successfully');",
                        true);

                    Session["SelectedRecId"] = null;
                    return;
                }

                // =====================================================
                // 🔥 2️⃣ OTHERWISE UPDATE HEADER (SINGLE RECORD)
                // =====================================================
                if (Session["SalesOrderRecId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "alert",
                        "alert('No Sales Order selected.');",
                        true);
                    return;
                }

                if (!long.TryParse(Session["SalesOrderRecId"].ToString(), out long headerRecId))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "error",
                        "alert('Invalid Sales Order RecId.');",
                        true);
                    return;
                }

                SalesOrderContract headerContract = new SalesOrderContract();

                // 🔥 VERY IMPORTANT
                headerContract.recId = headerRecId;

                // Header fields
                headerContract.salesName = txtCustomerName.Text;
                headerContract.SalesId = Session["SalesId"].ToString();
                headerContract.ShippingDateRequested = Convert.ToDateTime(txtRequestedShipDate.Text);
                headerContract.InvoiceAccount = ddlInvoiceAccount.SelectedValue;
                headerContract.custAccount = ddlcustomerAccount.SelectedValue;
                headerContract.InventSiteId = ddlSite.SelectedValue;
                headerContract.InventLocationId = ddlWarehouse.SelectedValue;
                headerContract.TaxGroup = ddlSalesTaxGroup.SelectedValue;
                headerContract.Reservation = ddlReservation.SelectedValue;
                headerContract.LanguageId = ddlLanguage.SelectedValue;
                headerContract.SalesPoolId = ddlPool.SelectedValue;
                headerContract.SalesOriginId = ddlSalesOrigin.SelectedValue;
                headerContract.CurrencyCode = ddlCurrency.SelectedValue;
                headerContract.PaymMode = ddlMethodOfPayment.SelectedValue;
                headerContract.Payment = ddlPayment.SelectedValue;
                headerContract.CashDiscCode = ddlCashDisc.SelectedValue;
                headerContract.PriceGroupId = ddlPriceGroup.SelectedValue;
                headerContract.linedisc = ddlLineDiscountGroup.SelectedValue;
                headerContract.EndDiscCode = ddlTotalDiscountGroup.SelectedValue;
                // headerContract.SalesId = txtSalesOrder.Text;
                // headerContract.OrderType = txtOrderType.Text;
                // headerContract.RetailSale = chkRetailSale.Checked;
                // headerContract.ContinuityOrder = chkContinuityOrder.Checked;
                // headerContract.OneTimeCustomer = chkOneTimeCustomer.Checked;
                // headerContract.Contact = txtContact.Text;
                // headerContract.Email = txtEmail.Text;
                // headerContract.Telephone = txtTelephone.Text;

               svc.updateHeaderDetail(headerContract);

                ScriptManager.RegisterStartupScript(this, GetType(),
                    "success",
                    "alert('Header Updated Successfully');",
                    true);

                Session["SalesOrderRecId"] = null;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "error",
                    $"alert('Error: {ex.Message.Replace("'", "")}');",
                    true);
            }
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
           
        }
        //protected void ddlItemNumber_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddlItem = sender as DropDownList;

        //    if (ddlItem == null)
        //        return;

        //    string selectedItemId = ddlItem.SelectedValue;

        //    if (!string.IsNullOrWhiteSpace(selectedItemId))
        //    {
        //        BindSize(selectedItemId);   // 🔥 This binds outside ddlSize
        //    }
        //    else
        //    {
        //        ddlSize.Items.Clear();
        //        ddlSize.Items.Insert(0, new ListItem("-- Select Size --", ""));
        //    }
        //}


        private bool BindSize(string itemId, string selectedValue = "")
        {

            SalesOrder line = new SalesOrder();
              DataTable dt = line.retrieveSizeId(itemId);
          

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlSize.DataSource = dt;
                ddlSize.DataTextField = "InventSizeId";   // what user sees
                ddlSize.DataValueField = "InventSizeId";  // underlying value
                ddlSize.DataBind();

                // Insert empty option at the top
                ddlSize.Items.Insert(0, new ListItem(""));
            }

            // Now set selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlSize.Items.FindByValue(selectedValue) != null)
            {
                ddlSize.SelectedValue = selectedValue;

            }
            else
            {
                ddlSize.SelectedIndex = 0; // show empty
            }

            return true;
        }

        public DataTable salesLineItemModified(string salesId, string itemId)
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.SaleslineItemModified(salesId, itemId);
            return dt;
        }

        protected void ddlItemNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlItem = sender as DropDownList;
            if (ddlItem == null) return;

            string selectedItemId = ddlItem.SelectedValue;
            if (string.IsNullOrWhiteSpace(selectedItemId)) return;

            string salesId = Session["SalesId"]?.ToString();
            if (string.IsNullOrWhiteSpace(salesId)) return;

            GridViewRow row = ddlItem.NamingContainer as GridViewRow;
            if (row == null) return;

            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.SaleslineItemModified(salesId, selectedItemId);

            if (dt == null || dt.Rows.Count == 0) return;

            DataRow dr = dt.Rows[0];

            // ✅ Use the RENAMED control IDs
            Label lblGridProductName = row.FindControl("lblGridProductName") as Label;
            TextBox txtGridQty = row.FindControl("txtGridQty") as TextBox;
            DropDownList ddlUnit = row.FindControl("ddlUnit") as DropDownList;
            DropDownList ddlSiteGrid = row.FindControl("ddlgridSite") as DropDownList;
            DropDownList ddlWHGrid = row.FindControl("ddlgridWarehouse") as DropDownList;
            TextBox txtGridUnitPrice = row.FindControl("txtGridUnitPrice") as TextBox;
            TextBox txtGridDiscount = row.FindControl("txtGridDiscount") as TextBox;
            TextBox txtGridDiscPct = row.FindControl("txtGridDiscPct") as TextBox;
            TextBox txtGridNetAmount = row.FindControl("txtGridNetAmount") as TextBox;

            // Product Name
            if (lblGridProductName != null)
                lblGridProductName.Text = dr["itemName"]?.ToString();

            // Quantity
            if (txtGridQty != null)
                txtGridQty.Text = dr["SalesQty"]?.ToString();

            // Unit — must rebind before setting value
            if (ddlUnit != null)
            {
                string unitVal = dr["SalesUnitId"]?.ToString();
                DataTable dtUnit = svc.retrieveUnit();
                ddlUnit.Items.Clear();
                ddlUnit.DataSource = dtUnit;
                ddlUnit.DataTextField = "unit";
                ddlUnit.DataValueField = "unit";
                ddlUnit.DataBind();
                ddlUnit.Items.Insert(0, new ListItem("--Select--", ""));
                if (!string.IsNullOrEmpty(unitVal) && ddlUnit.Items.FindByValue(unitVal) != null)
                    ddlUnit.SelectedValue = unitVal;
            }

            // Site — must rebind before setting value
            if (ddlSiteGrid != null)
            {
                string siteVal = dr["InventSiteId"]?.ToString();
                DataTable dtSite = svc.retrieveSite();
                ddlSiteGrid.Items.Clear();
                ddlSiteGrid.DataSource = dtSite;
                ddlSiteGrid.DataTextField = "InventSiteId";
                ddlSiteGrid.DataValueField = "InventSiteId";
                ddlSiteGrid.DataBind();
                ddlSiteGrid.Items.Insert(0, new ListItem("--Select--", ""));
                if (!string.IsNullOrEmpty(siteVal) && ddlSiteGrid.Items.FindByValue(siteVal) != null)
                    ddlSiteGrid.SelectedValue = siteVal;
            }

            // Warehouse — must rebind before setting value
            if (ddlWHGrid != null)
            {
                string whVal = dr["InventLocationId"]?.ToString();
                DataTable dtWH = svc.retrieveWarehouse();
                ddlWHGrid.Items.Clear();
                ddlWHGrid.DataSource = dtWH;
                ddlWHGrid.DataTextField = "InventLocationId";
                ddlWHGrid.DataValueField = "InventLocationId";
                ddlWHGrid.DataBind();
                ddlWHGrid.Items.Insert(0, new ListItem("--Select--", ""));
                if (!string.IsNullOrEmpty(whVal) && ddlWHGrid.Items.FindByValue(whVal) != null)
                    ddlWHGrid.SelectedValue = whVal;
            }

            // Unit Price
            if (txtGridUnitPrice != null)
                txtGridUnitPrice.Text = dr["SalesPrice"] != DBNull.Value
                    ? Convert.ToDecimal(dr["SalesPrice"]).ToString("0.00") : "0.00";

            // Discount
            if (txtGridDiscount != null)
                txtGridDiscount.Text = dr["SalesLineDisc"] != DBNull.Value
                    ? Convert.ToDecimal(dr["SalesLineDisc"]).ToString("0.00") : "0.00";

            // Discount Percentage
            if (txtGridDiscPct != null)
                txtGridDiscPct.Text = dr["linepercent"] != DBNull.Value
                    ? Convert.ToDecimal(dr["linepercent"]).ToString("0.00") : "0.00";

            // Net Amount
            if (txtGridNetAmount != null)
                txtGridNetAmount.Text = dr["LineAmount"] != DBNull.Value
                    ? Convert.ToDecimal(dr["LineAmount"]).ToString("0.00") : "0.00";
        }







    }


}