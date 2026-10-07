using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_MultipleInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMultipleInvoiceGrid();
            }

        }


        private void BindMultipleInvoiceGrid()
        {
           
            DataTable dt = new DataTable();

            dt.Columns.Add("InvoiceAccount");
            dt.Columns.Add("AccountName");
            dt.Columns.Add("PurchaseOrder");
            dt.Columns.Add("PurchaseAgreement");
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("ProductReceipt");
            dt.Columns.Add("OnHold", typeof(bool));
            dt.Columns.Add("Status");
            dt.Columns.Add("MatchStatus");


            // Bind the GridView
            gvMultipleInvoice.DataSource = dt;
            gvMultipleInvoice.DataBind();
        }
    }
}