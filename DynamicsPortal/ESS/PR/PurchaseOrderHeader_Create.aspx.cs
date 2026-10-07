using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Markup;

namespace DynamicsPortal
{
    public partial class PurchaseOrderHeader_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "PurchaseOrderHeader_Create";

            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;

            if (!isPageAuthorizated)
                return;

            // Optional: You could load data only once on first load
            if (!IsPostBack)
            {

                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Create Purchase Order";
                    titleDiv.Style["font-weight"] = "600";   // semi-bold
                    titleDiv.Style["font-size"] = "20px";    // slightly larger
                    titleDiv.Style["color"] = "#000000";     // solid black
                    titleDiv.Style["margin"] = "10px 0";     // spacing around
                }




                // BindVendorAccounts();
                //BindContactDropdown();
                BindPurchaseOrder();
                //BindInvoiceAccount();
                BindCurrency();
                BindProjectDropdown();
                //BindSiteDropdown();
                //BindWarehouseDropdown();
                //BindBuyerGroupDropdown();
                //BindWorkerDropdown();
                //BindRequesterDropdown();
                //BindPoolDropdown();
                //BindLanguageDropdown();

                DateTime parsedDate = DateTime.Now;

                dpAccountDate.Text = parsedDate.ToString("yyyy-MM-dd");
                dpRequestedReceiptDate.Text = parsedDate.ToString("yyyy-MM-dd");

                txtVendorName.Attributes["style"] = "color: gray !important; background-color: #F3F2F1 !important;";
                txtAddress.Attributes["style"] = "color: gray !important; background-color: #F3F2F1 !important;";
                txtPurchaseOrder.Attributes["style"] = "color: gray !important; background-color: #F3F2F1 !important;";
                txtName.Attributes["style"] = "color: gray !important; background-color: #F3F2F1 !important;";

            }


        }

        private void BindPurchaseOrder()
        {
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();

            // Assuming retrieveVendor returns a DataTable with a column "PurchID"
            DataTable dt = neworder.retrievenumberseq();

            if (dt != null && dt.Rows.Count > 0)
            {
                // Example: Taking the first row's PurchID
                string purchaseOrderId = dt.Rows[0]["PurchID"].ToString();

                // Set the value to the TextBox
                txtPurchaseOrder.Text = purchaseOrderId;
            }
        }


        private void BindVendorAccounts()
        {
            //PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            //DataTable dt = neworder.retrieveVendor();

            //ddlVendorAccount.Items.Clear();
            //ddlVendorAccount.Items.Insert(0, new ListItem("", ""));

            //foreach (DataRow row in dt.Rows)
            //{
            //    string vendAccount = row["VendorAccount"].ToString();
            //    string vendName = row["VendorName"].ToString(); // Adjust column name if needed

            //    string text = $"{vendAccount} - {vendName}";
            //    ddlVendorAccount.Items.Add(new ListItem(text, vendAccount));
            //}

            //ddlVendorAccount.Attributes["onchange"] = "fetchVendorName(this)";
            //ddlVendorAccount.CssClass += " filterable-dropdown";
        }

        private void BindContactDropdown()
        {
            //PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            //DataTable dt = neworder.retrievecontactdetails();

            //ddlContact.Items.Clear();
            //ddlContact.Items.Insert(0, new ListItem("", ""));

            //foreach (DataRow row in dt.Rows)
            //{
            //    string contactPersonId = row["ContactPersonId"].ToString();
            //    string contactForParty = row["ContactForParty"].ToString();
            //    string personName = row["PersonName"].ToString();

            //    string text = $"{contactPersonId} - {contactForParty} - {personName}";

            //    // Use ContactPersonId as the value (or any unique key you prefer)
            //    ddlContact.Items.Add(new ListItem(text, contactPersonId));
            //}

            //ddlContact.CssClass += " filterable-dropdown";
        }

        private void BindInvoiceAccount()
        {
            //PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            //DataTable dt = neworder.retrieveVendor();

            //ddlInvoiceAccount.Items.Clear();
            //ddlInvoiceAccount.Items.Insert(0, new ListItem("", ""));

            //foreach (DataRow row in dt.Rows)
            //{
            //    string vendAccount = row["VendorAccount"].ToString();
            //    string vendName = row["VendorName"].ToString(); // Adjust column name if needed

            //    string text = $"{vendAccount} - {vendName}";
            //    ddlInvoiceAccount.Items.Add(new ListItem(text, vendAccount));
            //}

            //ddlInvoiceAccount.Attributes["onchange"] = "fetchInvoiceVendorName(this)";
            //ddlInvoiceAccount.CssClass += " filterable-dropdown";
        }

        private void BindCurrency()
        {
            // The new User Control handles its own data binding.
            /*
            DataTable dt = ControlsHelper.retrieveAllCurrencyDetails();
            ddlCurrency.DataSource = dt;
            ddlCurrency.DataValueField = "CurrencyCode";   // value to pass
            ddlCurrency.DataTextField = "CurrencyCode";    // text to show
            ddlCurrency.DataBind();
            ddlCurrency.Items.Insert(0, new ListItem("", string.Empty));
            ddlCurrency.CssClass += " filterable-dropdown";
            */
        }

        private void BindProjectDropdown()
        {
            // The new User Control handles its own data binding.
            /*
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dt = neworder.retrieveprojectID(); 

            ddlProjectID.Items.Clear();
            ddlProjectID.Items.Insert(0, new ListItem("", "")); 

            foreach (DataRow row in dt.Rows)
            {
                string projectId = row["ProjectID"].ToString();
                string projectName = row["ProjectName"].ToString();

                string text = $"{projectId} - {projectName}";
                ddlProjectID.Items.Add(new ListItem(text, projectId));
            }

            ddlProjectID.CssClass += " filterable-dropdown"; 
            */
        }

        private void BindSiteDropdown()
        {
            //PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            //DataTable dt = neworder.retrievesiteinfo(); // You should implement this method

            //ddlSite.Items.Clear();
            //ddlSite.Items.Insert(0, new ListItem("", ""));

            //foreach (DataRow row in dt.Rows)
            //{
            //    string siteId = row["SiteID"].ToString();
            //    string siteName = row["SiteName"].ToString();

            //    string text = $"{siteId} - {siteName}";
            //    ddlSite.Items.Add(new ListItem(text, siteId));
            //}

            //ddlSite.CssClass += " filterable-dropdown";
        }

        private void BindWarehouseDropdown()
        {
            //PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            //DataTable dt = neworder.retrievelocationinformation(); // You need to implement this

            //ddlWarehouse.Items.Clear();
            //ddlWarehouse.Items.Insert(0, new ListItem("", "")); // Optional empty item

            //foreach (DataRow row in dt.Rows)
            //{
            //    string locationId = row["LocationID"].ToString();
            //    string locationName = row["LocationName"].ToString();

            //    string text = $"{locationId} - {locationName}";
            //    ddlWarehouse.Items.Add(new ListItem(text, locationId));
            //}

            //ddlWarehouse.CssClass += " filterable-dropdown"; // Optional extra class
        }

        private void BindBuyerGroupDropdown()
        {
            /*
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dt = neworder.retrieveBuyerGroupInformation(); // You need to implement this method

            ddlBuyerGroup.Items.Clear();
            ddlBuyerGroup.Items.Insert(0, new ListItem("", "")); // Optional empty first item

            foreach (DataRow row in dt.Rows)
            {
                string buyerGroup = row["BuyerGroup"].ToString();
                string description = row["Description"].ToString();

                string text = $"{buyerGroup} - {description}";
                ddlBuyerGroup.Items.Add(new ListItem(text, buyerGroup));
            }

            ddlBuyerGroup.CssClass += " filterable-dropdown"; // Optional extra CSS
            */
        }

        private void BindWorkerDropdown()
        {
            /*
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dt = neworder.retrieveworkerinfo(); // You need to implement this method

            ddlOrderer.Items.Clear();
            ddlOrderer.Items.Insert(0, new ListItem("", "")); // Optional: empty first item

            foreach (DataRow row in dt.Rows)
            {
                string workerName = row["WorkerName"].ToString();
                string personnelNumber = row["PersonnelNumber"].ToString();

                string text = $"{workerName} - {personnelNumber}";
                ddlOrderer.Items.Add(new ListItem(text, personnelNumber));
            }
            string name = SessionVariables.getCurrentEmployeeId();
            if (ddlOrderer.Items.FindByValue(name) != null)
            {
                ddlOrderer.SelectedValue = name;
            }
            ddlOrderer.CssClass += " filterable-dropdown";
            */
        }

        private void BindRequesterDropdown()
        {
            /*
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dt = neworder.retrieveworkerinfo(); // You need to implement this method

            ddlRequester.Items.Clear();
            ddlRequester.Items.Insert(0, new ListItem("", "")); // Optional: empty first item

            foreach (DataRow row in dt.Rows)
            {
                string workerName = row["WorkerName"].ToString();
                string personnelNumber = row["PersonnelNumber"].ToString();

                string text = $"{workerName} - {personnelNumber}";
                ddlRequester.Items.Add(new ListItem(text, personnelNumber));
            }

            ddlRequester.CssClass += " filterable-dropdown";
            */
        }

        private void BindPoolDropdown()
        {
            /*
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dt = neworder.retrievePoolInfo(); // You need to implement this method

            ddlPool.Items.Clear();
            ddlPool.Items.Insert(0, new ListItem("", "")); // Optional: empty first item

            foreach (DataRow row in dt.Rows)
            {
                string poolId = row["PoolID"].ToString();
                string poolName = row["PoolName"].ToString();

                string text = $"{poolId} - {poolName}";
                ddlPool.Items.Add(new ListItem(text, poolId));
            }

            ddlPool.CssClass += " filterable-dropdown";
            */
        }


        private void BindLanguageDropdown()
        {
            /*
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dt = neworder.retrieveLangaugeID(); // You need to implement this method

            ddlLanguage.Items.Clear();
            ddlLanguage.Items.Insert(0, new ListItem("", "")); // Optional: empty first item

            foreach (DataRow row in dt.Rows)
            {
                string languageId = row["LangaugeID"].ToString();
                ddlLanguage.Items.Add(new ListItem(languageId, languageId));
            }

            ddlLanguage.CssClass += " filterable-dropdown";
            */
        }

        //[System.Web.Services.WebMethod]
        //public static string GetVendorName(string vendAccount)
        //{
        //    PurchaseOrderHeader neworder = new PurchaseOrderHeader();
        //    DataTable dt = neworder.retrieveVendor(vendAccount);
        //    string text = dt.Rows[0]["VendorName"].ToString();
        //    return text;
        //}




        public void btnSave_Click(object sender, EventArgs e)
        {
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

            // Prepare data
            DataTable dt = new DataTable();
            dt.Columns.Add("PurchID", typeof(string));
            dt.Columns.Add("VendorAccount", typeof(string));
            dt.Columns.Add("ItemBuyerGroup");
            dt.Columns.Add("PurchPoolID");
            dt.Columns.Add("CurrencyCode");
            dt.Columns.Add("AccountingDate");
            dt.Columns.Add("DeliveryDate");
            dt.Columns.Add("InventSiteId");
            dt.Columns.Add("InventLocationId");
            dt.Columns.Add("IsInterCompanyVendor");

            string purchId = txtPurchaseOrder.Text;
            string vendoraccount = hfVendorAccountId.Value;
            string itemBuyerGroup = hfBuyerGroupId.Value;
            string purchPoolId = hfPoolId.Value;
            string currency = hfCurrencyCode.Value;
            string accountingDate = dpAccountDate.Text;
            string deliveryDate = dpRequestedReceiptDate.Text;
            string inventSiteId = hfSiteId.Value;
            string inventLocationId = hfWarehouseId.Value;
            string intercompany = chkIntercompany.Checked ? "Yes" : "No";

            // Validation checks
            if (string.IsNullOrWhiteSpace(purchId))
            {
                lblMessage.Text = "Purchase Requisition <b>Name</b> is required.";
            }
            else if (string.IsNullOrWhiteSpace(vendoraccount))
            {
                lblMessage.Text = "<b>VendorAccount</b> is required.";
            }
            else
            {
                // Populate data row
                DataRow row = dt.NewRow();
                row["PurchID"] = purchId;
                row["VendorAccount"] = vendoraccount;
                row["ItemBuyerGroup"] = itemBuyerGroup;
                row["PurchPoolID"] = purchPoolId;
                row["CurrencyCode"] = currency;
                row["AccountingDate"] = accountingDate;
                row["DeliveryDate"] = deliveryDate;
                row["InventSiteId"] = inventSiteId;
                row["InventLocationId"] = inventLocationId;
                row["IsInterCompanyVendor"] = intercompany;
                dt.Rows.Add(row);

                // Call business logic
                PurchaseOrderHeader purchaseorder = new PurchaseOrderHeader();
                createResult = purchaseorder.create(dt);


                if (createResult != null && createResult.isSuccess)
                {
                    NotificationMessage.showMessage(createResult);


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
        }

        //protected void ddlVendorAccount_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string vendAccount = (DropDownList_ddlVendorAccount.FindControl("txtVendorAccountId") as System.Web.UI.WebControls.TextBox).Text;

        //    if (string.IsNullOrWhiteSpace(vendAccount))
        //    {
        //        return;
        //    }

        //    PurchaseOrderHeader neworder = new PurchaseOrderHeader();
        //    DataTable dt = neworder.retrieveVendor(vendAccount);

        //    if (dt.Rows.Count > 0)
        //    {
        //        txtVendorName.Text = dt.Rows[0]["VendorName"].ToString();
        //        txtDeliveryName.Text = dt.Rows[0]["Address"].ToString();
        //        txtAddress.Text = dt.Rows[0]["ServiceAddress"].ToString();
        //        txtDeliveryAddress.Text = dt.Rows[0]["Address"].ToString();
        //        txtName.Text = dt.Rows[0]["VendorName"].ToString();

        //        // Invoice account is now a Select2 + hidden field, not a user control
        //        hfInvoiceAccountId.Value = vendAccount;

        //        // Currency is now a Select2 + hidden field
        //        hfCurrencyCode.Value = "USD";

        //        // Language is now a Select2 + hidden field
        //        hfLanguageId.Value = "en-us";

        //        string changeRequestEnabled = dt.Rows[0]["ChangeRequestEnabled"].ToString();
        //        if (changeRequestEnabled.Equals("Yes", StringComparison.OrdinalIgnoreCase))
        //        {
        //            ActivateChangeManagementBox.Checked = true;
        //            ActivateChangeManagement.InnerText = "Yes";
        //        }
        //        else
        //        {
        //            ActivateChangeManagementBox.Checked = false;
        //            ActivateChangeManagement.InnerText = "No";
        //        }

        //        // Pool is now a Select2 + hidden field
        //        hfPoolId.Value = dt.Rows[0]["PurchPoolID"].ToString();

        //        // Site is now a Select2 + hidden field
        //        hfSiteId.Value = dt.Rows[0]["SiteID"].ToString();

        //        // Warehouse is now a Select2 + hidden field
        //        hfWarehouseId.Value = dt.Rows[0]["LocationID"].ToString();

        //        // Intercompany
        //        chkIntercompany.Checked =
        //            dt.Rows[0]["IsInterCompanyVendor"].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase);

        //        // Company
        //        txtCompany.Text = dt.Rows[0]["InterCompanyPartnerCompanyName"].ToString();
        //    }
        //}

        //protected void ddlVendorAccount_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string vendAccount = (DropDownList_ddlVendorAccount.FindControl("txtVendorAccountId") as System.Web.UI.WebControls.TextBox).Text;

        //    if (string.IsNullOrWhiteSpace(vendAccount))
        //    {
        //        return;
        //    }

        //    // Store the selected vendor account so btnSave_Click can read it
        //    hfVendorAccountId.Value = vendAccount;   // <-- add this line

        //    PurchaseOrderHeader neworder = new PurchaseOrderHeader();
        //    DataTable dt = neworder.retrieveVendor(vendAccount);

        //    if (dt.Rows.Count > 0)
        //    {
        //        txtVendorName.Text = dt.Rows[0]["VendorName"].ToString();
        //        txtDeliveryName.Text = dt.Rows[0]["Address"].ToString();
        //        txtAddress.Text = dt.Rows[0]["ServiceAddress"].ToString();
        //        txtDeliveryAddress.Text = dt.Rows[0]["Address"].ToString();
        //        txtName.Text = dt.Rows[0]["VendorName"].ToString();

        //        hfInvoiceAccountId.Value = vendAccount;
        //        hfCurrencyCode.Value = "USD";
        //        hfLanguageId.Value = "en-us";

        //        string changeRequestEnabled = dt.Rows[0]["ChangeRequestEnabled"].ToString();
        //        if (changeRequestEnabled.Equals("Yes", StringComparison.OrdinalIgnoreCase))
        //        {
        //            ActivateChangeManagementBox.Checked = true;
        //            ActivateChangeManagement.InnerText = "Yes";
        //        }
        //        else
        //        {
        //            ActivateChangeManagementBox.Checked = false;
        //            ActivateChangeManagement.InnerText = "No";
        //        }

        //        hfPoolId.Value = dt.Rows[0]["PurchPoolID"].ToString();
        //        hfSiteId.Value = dt.Rows[0]["SiteID"].ToString();
        //        hfWarehouseId.Value = dt.Rows[0]["LocationID"].ToString();

        //        chkIntercompany.Checked =
        //            dt.Rows[0]["IsInterCompanyVendor"].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase);

        //        txtCompany.Text = dt.Rows[0]["InterCompanyPartnerCompanyName"].ToString();
        //    }
        //}
    }
}


//private void ClearControls()
//{
//    txtVendorName.Text = string.Empty;
//    txtDeliveryName.Text = string.Empty;
//    txtAddress.Text = string.Empty;
//    txtDeliveryAddress.Text = string.Empty;
//    txtName.Text = string.Empty;
//    ddlInvoiceAccount.Text = string.Empty; // Clear this as well if applicable
//}





