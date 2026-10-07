using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_Invoice_Journal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindOverviewGrid();
            BindLinesGrid();


        }

        private void BindOverviewGrid()
        {
            // Create empty DataTable with columns
            DataTable dt = new DataTable();

            dt.Columns.Add("PurchaseOrder");
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("DateOfVATRegister", typeof(DateTime));
            dt.Columns.Add("Invoice");
            dt.Columns.Add("Voucher");
            dt.Columns.Add("Currency");
            dt.Columns.Add("SalesTax");
            dt.Columns.Add("InvoiceAmount", typeof(decimal));
            dt.Columns.Add("Company");
            dt.Columns.Add("SalesOrder");
            dt.Columns.Add("Voucher2");
            dt.Columns.Add("PostedViaIntercompany");
            dt.Columns.Add("DueDate", typeof(DateTime));
            dt.Columns.Add("InvoiceRegisterDate", typeof(DateTime));

         
            gvOverview.DataSource = dt;
            gvOverview.DataBind();
        }

        private void BindLinesGrid()
        {
            // Create empty DataTable with columns
            DataTable dt = new DataTable();

            dt.Columns.Add("PurchaseOrder");
            dt.Columns.Add("LineNumber", typeof(int));
            dt.Columns.Add("Item");
            dt.Columns.Add("ProcurementCategory");
            dt.Columns.Add("Description");
            dt.Columns.Add("Site");
            dt.Columns.Add("Warehouse");
            dt.Columns.Add("InventoryStatus");
            dt.Columns.Add("CWQuantity", typeof(decimal));
            dt.Columns.Add("Quantity", typeof(decimal));
            dt.Columns.Add("UnitPrice", typeof(decimal));
            dt.Columns.Add("Discount", typeof(decimal));
            dt.Columns.Add("DiscountPercent", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("SalesTaxIncludedInAmount", typeof(bool));
            dt.Columns.Add("Box1099");
            dt.Columns.Add("Amount1099", typeof(decimal));
            dt.Columns.Add("StateProvince");
            dt.Columns.Add("Amount1099State", typeof(decimal));
            dt.Columns.Add("ReasonCode");
            dt.Columns.Add("ReasonComment");
;

            gvLines.DataSource = dt;
            gvLines.DataBind();
        }
    }
}