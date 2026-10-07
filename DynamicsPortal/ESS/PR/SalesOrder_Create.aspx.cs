using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.SalesOrderSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class SalesOrder_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Create sales order";
                }
                BindValues();
                BindSalesOrder();
                BindCustomerAccount();
                BindCurrency();
                BindSite();
                BindWarehouse();
                BindDeliveryTerm();
                BindDeliveryMode();
                BindPool();
                BindLanguageId();
                BindOrderType();
                BindInvoiceAccount();
                
            }
        }

        private void BindValues()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveAll(); // Your method that gets data

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0]; // Take the first row

              
               
                txtRequestedReceiptDate.Text = Convert.ToDateTime(row["ReceiptDateRequested"]).ToString("yyyy-MM-dd");
                txtRequestedShipDate.Text = Convert.ToDateTime(row["ShippingDateRequested"]).ToString("yyyy-MM-dd");






            }
        }


        private void BindCustomerAccount()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveCustomerAccount(); // You need to implement this method

            ddlCustomerAccount.Items.Clear();
            ddlCustomerAccount.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string custAccount = row["custAccount"].ToString();
                string description = row["Name"].ToString();

                string text = $"{custAccount} - {description}";
                ddlCustomerAccount.Items.Add(new ListItem(text, custAccount));
            }

            ddlCustomerAccount.CssClass += " filterable-dropdown"; // Optional extra CSS
        }

        private void BindCurrency()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveCurrency(); // You need to implement this method

            ddlCurrency.Items.Clear();
            ddlCurrency.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string currecny = row["CurrencyCode"].ToString();
                string description = row["txt"].ToString();

                string text = $"{currecny} - {description}";
                ddlCurrency.Items.Add(new ListItem(text, currecny));
            }

            ddlCurrency.CssClass += " filterable-dropdown"; // Optional extra CSS
        }

        private void BindSite()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveSite(); // You need to implement this method

            ddlSite.Items.Clear();
            ddlSite.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string siteId = row["InventSiteId"].ToString();
                string description = row["SiteName"].ToString();

                string text = $"{siteId} - {description}";
                ddlSite.Items.Add(new ListItem(text, siteId));
            }

            ddlSite.CssClass += " filterable-dropdown"; // Optional extra CSS
        }

        private void BindSalesOrder()
        {
            SalesOrder svc = new SalesOrder();

            // Assuming retrieveVendor returns a DataTable with a column "PurchID"
            DataTable dt = svc.retrievenumberseq();

            if (dt != null && dt.Rows.Count > 0)
            {
                // Example: Taking the first row's PurchID
                string salesorder = dt.Rows[0]["SalesId"].ToString();

                // Set the value to the TextBox
                txtSalesOrder.Text = salesorder;
            }
        }


        private void BindWarehouse()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveWarehouse(); // You need to implement this method

            ddlWarehouse.Items.Clear();
            ddlWarehouse.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string warehouse = row["InventLocationId"].ToString();
                string description = row["locationName"].ToString();

                string text = $"{warehouse} - {description}";
                ddlWarehouse.Items.Add(new ListItem(text, warehouse));
            }

            ddlWarehouse.CssClass += " filterable-dropdown"; // Optional extra CSS
        }

        private void BindDeliveryTerm()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveDeliveryTerm(); // You need to implement this method

            ddlDeliveryTerm.Items.Clear();
            ddlDeliveryTerm.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string deliverymode = row["DlvMode"].ToString();
                string description = row["dlvTermTxt"].ToString();

                string text = $"{deliverymode} - {description}";
                ddlDeliveryTerm.Items.Add(new ListItem(text, deliverymode));
            }

            ddlDeliveryTerm.CssClass += " filterable-dropdown"; // Optional extra CSS
        }

        private void BindDeliveryMode()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveDeliveryMode(); // You need to implement this method

            ddlModeOfDelivery.Items.Clear();
            ddlModeOfDelivery.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string deliveryTerm = row["DlvTerm"].ToString();
                string description = row["dlvTxt"].ToString();

                string text = $"{deliveryTerm} - {description}";
                ddlModeOfDelivery.Items.Add(new ListItem(text, deliveryTerm));
            }

            ddlModeOfDelivery.CssClass += " filterable-dropdown"; // Optional extra CSS
        }

        private void BindPool()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrievePool(); // You need to implement this method

            ddlPool.Items.Clear();
           // ddlPool.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string poolId = row["SalesPoolId"].ToString();
                string description = row["salesName"].ToString();

                string text = $"{poolId} - {description}";
                ddlPool.Items.Add(new ListItem(text, poolId));
            }

            ddlPool.CssClass += " filterable-dropdown"; // Optional extra CSS
        }

        private void BindLanguageId()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveLanguageId(); // You need to implement this method

            ddlLanguage.Items.Clear();
            ddlLanguage.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string language = row["LanguageId"].ToString();
               

                string text = $"{language}";
                ddlLanguage.Items.Add(new ListItem(text, language));
            }

            ddlLanguage.CssClass += " filterable-dropdown"; // Optional extra CSS
        }

        protected void ddlCustomerAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            SalesOrder svc = new SalesOrder();


            string custAccount = ddlCustomerAccount.SelectedValue;

            if (!string.IsNullOrEmpty(custAccount))
            {
                DataTable dt = svc.retrieveCustAccountLookup(custAccount);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    ddlInvoiceAccount.SelectedValue = row["custAccount"].ToString();
                    // ADDRESS PANEL
                    txtDeliveryAddress.Text = row["Description"].ToString();
                    txtDeliveryName.Text = row["deliveryName"].ToString();
                    txtAddress.Text = row["address"].ToString();

                    // DELIVERY PANEL
                    ddlModeOfDelivery.SelectedValue = row["dlvMode"].ToString();
                    ddlDeliveryTerm.SelectedValue = row["dlvTerm"].ToString();
                    ddlCurrency.SelectedValue = row["CurrencyCode"].ToString();
                    txtName.Text = row["invoiceName"].ToString();
                    ddlLanguage.SelectedValue = row["LanguageId"].ToString() ;
                    ddlSite.SelectedValue = row["InventSiteId"].ToString();
                    ddlWarehouse.SelectedValue = row["InventLocationId"].ToString();
                    TxtSalesTaker.Text = row["WorkerSalesTakerName"].ToString();
                    TxtSalesOrigin.Text = row["SalesOriginId"].ToString();
                    TxtSalesResponsible.Text = row["WorkerSalesResponsibleName"].ToString();
                   
                    // ADMIN PANEL
                    //  ddlSalesResponsible.Text = row["salesResponsible"].ToString();

                    // If you want invoice data later
                    // txtInvoiceAccount.Text = row["invoiceAccount"].ToString();
                }
            }
        }
        private void BindInvoiceAccount()
        {
            SalesOrder svc = new SalesOrder();
            DataTable dt = svc.retrieveCustomerAccount(); // You need to implement this method

            ddlInvoiceAccount.Items.Clear();
            ddlInvoiceAccount.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string custAccount = row["custAccount"].ToString();
                string description = row["Name"].ToString();

                string text = $"{custAccount} - {description}";
                ddlInvoiceAccount.Items.Add(new ListItem(text, custAccount));
            }

            ddlInvoiceAccount.CssClass += " filterable-dropdown"; // Optional extra CSS
        }

        private void BindOrderType()
        {
            ddlOrderType.Items.Clear();

            // Add fixed options
            ddlOrderType.Items.Add(new ListItem("Journal", "Journal"));
            ddlOrderType.Items.Add(new ListItem("Subscription", "Subscription"));
            ddlOrderType.Items.Add(new ListItem("Sales Order", "Sales order"));
            ddlOrderType.Items.Add(new ListItem("Returned Order", "Returned order"));
            ddlOrderType.Items.Add(new ListItem("Item Requirements", "Item requirements"));

            // Set default value to Sales Order
            ddlOrderType.SelectedValue = "SalesOrder";
        }




        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Create SalesOrder service instance
                SalesOrder salesOrderService = new SalesOrder();

                // 2️⃣ Create and populate SalesOrderContract
                SalesOrderContract contract = new SalesOrderContract();
                contract.custAccount = ddlCustomerAccount.SelectedValue;
                contract.SalesId = txtSalesOrder.Text.Trim();
                contract.salesName = txtName.Text.Trim();
                contract.InvoiceAccount = ddlInvoiceAccount.SelectedValue;
                contract.CurrencyCode = ddlCurrency.SelectedValue;
                contract.LanguageId = ddlLanguage.SelectedValue;
                contract.SalesType = ddlOrderType.SelectedValue;

                // 3️⃣ Call create method
                DataTable dt = salesOrderService.create(contract);

                // 4️⃣ Prepare result object similar to your purchase order pattern
                var createResult = new
                {
                    isSuccess = dt != null && dt.Rows.Count > 0 && Convert.ToBoolean(dt.Rows[0]["IsSuccess"]),
                    message = dt != null && dt.Rows.Count > 0 ? dt.Rows[0]["Message"].ToString() : "No response from service."
                };

                // 5️⃣ Show notification and refresh parent grid if successful
                if (createResult.isSuccess)
                {
                    string script = @"
        setTimeout(function() { 
            if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                window.parent.refreshParentGrid();
            }
            closeDialog(); 
        }, 3000);";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    ex
                );

                NotificationMessage.showMessage("Error: " + ex.Message);
            }
        }


    }
}