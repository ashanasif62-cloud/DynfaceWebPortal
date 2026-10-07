using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class InvoiceJournal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl pageTitle = (HtmlGenericControl)Master.FindControl("pageTitle");
            if (pageTitle != null)
            {
                pageTitle.InnerText = "Invoice Journal";
            }

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            // Dummy data to demonstrate the Grid structure
            DataTable dt = new DataTable();
            dt.Columns.Add("PurchId");
            dt.Columns.Add("TransDate", typeof(DateTime));
            dt.Columns.Add("VATRegisterDate_W", typeof(DateTime));
            dt.Columns.Add("InvoiceId");
            dt.Columns.Add("LedgerVoucher");
            dt.Columns.Add("CurrencyCode");
            dt.Columns.Add("SumTax", typeof(decimal));
            dt.Columns.Add("InvoiceAmount", typeof(decimal));
            dt.Columns.Add("DataAreaId");
            dt.Columns.Add("SalesId");
            dt.Columns.Add("Voucher");
            dt.Columns.Add("IntercompanyPosted");
            dt.Columns.Add("DueDate", typeof(DateTime));
            dt.Columns.Add("InvoiceRegisterDate", typeof(DateTime));
            dt.Columns.Add("InvoiceRegisterVoucher");

            // Adding a sample rows
            dt.Rows.Add("PO-0000316", DateTime.Now, DateTime.Now, "INV-1001", "VOU-5001", "USD", 15.50, 250.00, "DAT", "", "V-123", "No", DateTime.Now.AddDays(30), DateTime.Now, "RV-99");
            dt.Rows.Add("PO-0000317", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1), "INV-1002", "VOU-5002", "USD", 12.00, 180.00, "DAT", "", "V-124", "No", DateTime.Now.AddDays(30), DateTime.Now, "RV-100");
            dt.Rows.Add("PO-0000318", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-2), "INV-1003", "VOU-5003", "USD", 25.00, 450.00, "DAT", "", "V-125", "No", DateTime.Now.AddDays(30), DateTime.Now, "RV-101");
            dt.Rows.Add("PO-0000319", DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-3), "INV-1004", "VOU-5004", "USD", 10.00, 150.00, "DAT", "", "V-126", "No", DateTime.Now.AddDays(30), DateTime.Now, "RV-102");
            dt.Rows.Add("PO-0000320", DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-4), "INV-1005", "VOU-5005", "USD", 35.00, 600.00, "DAT", "SO-99", "V-127", "Yes", DateTime.Now.AddDays(30), DateTime.Now, "RV-103");

            gvInvoiceJournal.DataSource = dt;
            gvInvoiceJournal.DataBind();
        }
    }
}
