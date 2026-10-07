using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Web.UI;

namespace DynamicsPortal.ESS.PR
{
    public partial class PostingProduct_Receipt : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            //showActionPanel = false;
            pageMenuId = "PostingProductReceipt";

            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;

            if (!IsPostBack)
            {
                try
                {
                    BindSettingsGrid();
                    BindOverviewGrid();
                    BindLinesGrid();
                    BindDetailsGrid();
                }
                catch (Exception ex)
                {
                    ShowMessage("An error occurred while loading data: " + ex.Message);
                }
            }
        }

        private void BindSettingsGrid()
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("SettingName");
            //dt.Columns.Add("SettingValue");

            //dt.Rows.Add("Receipt Type", "Standard");
            //dt.Rows.Add("Warehouse", "Main WH");
            //dt.Rows.Add("Posting Date", DateTime.Now.ToString("yyyy-MM-dd"));

            //gvSettings.DataSource = dt;
            //gvSettings.DataBind();
        }

        private void BindOverviewGrid()
        {
            DataTable dt = new DataTable();

            // Define Columns
            dt.Columns.Add("Update");
            dt.Columns.Add("PurchaseOrder");
            dt.Columns.Add("Name");
            dt.Columns.Add("ProductReceipt");
            dt.Columns.Add("ShipmentNumber");
            dt.Columns.Add("ProductReceiptDate", typeof(DateTime));
            dt.Columns.Add("DocumentDate", typeof(DateTime));
            dt.Columns.Add("TermsOfPayment");

            // Add Sample Rows (replace with DB data later)
            //dt.Rows.Add("Updated", "PO12345", "ABC Supplier", "PR56789", "SHIP001", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1), "Net 30");
            //dt.Rows.Add("Pending", "PO54321", "XYZ Supplier", "PR98765", "SHIP002", DateTime.Now, DateTime.Now, "Advance Payment");

            // Bind to GridView
            gvOverview.DataSource = dt;
            gvOverview.DataBind();
        
        }

        protected void BindLinesGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PurchaseOrder");
            dt.Columns.Add("LineNumber");
            dt.Columns.Add("ProductNumber");
            dt.Columns.Add("ItemNumber");
            dt.Columns.Add("ProcurementCategory");
            dt.Columns.Add("Text");
            dt.Columns.Add("Site");
            dt.Columns.Add("Warehouse");
            dt.Columns.Add("InventoryStatus");
            dt.Columns.Add("CWUpdate");
            dt.Columns.Add("QuantityOrdered");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("DeliveryRemaining");
            dt.Columns.Add("UnitPrice", typeof(decimal));
            dt.Columns.Add("VendorBatchDate", typeof(DateTime));
            dt.Columns.Add("LineNetAmount", typeof(decimal));
            dt.Columns.Add("VendorExpiryDate", typeof(DateTime));
            dt.Columns.Add("CloseForReceipt", typeof(bool));
            dt.Columns.Add("BackOrder");
            dt.Columns.Add("QualityOrderStatus");

            // Sample row
            //dt.Rows.Add("PO-1001", "10", "PRD-001", "ITM-123", "Raw Materials", "Steel Rods",
            //            "Site-01", "WH-01", "Available", "Yes", "500", "480", "20",
            //            150.75m, DateTime.Now.AddDays(-10), 72360.00m, DateTime.Now.AddMonths(6),
            //            true, "No", "Passed");

            gvLines.DataSource = dt;
            gvLines.DataBind();
        }


        private void BindDetailsGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PurchaseOrder");
            dt.Columns.Add("Name");
            //dt.Columns.Add("CreatedBy");
            //dt.Columns.Add("CreatedOn");

            //dt.Rows.Add("D001", "REC-001", "Admin", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            //dt.Rows.Add("D002", "REC-002", "User1", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

           gvPurchases.DataSource = dt;
           gvPurchases.DataBind();
        }

        private void ShowMessage(string message)
        {
            lblMessage.Visible = true;
            lblMessage.Text = message;
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            
        }
  
    }
}
