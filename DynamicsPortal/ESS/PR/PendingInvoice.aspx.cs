using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PendingInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl pageTitle = (HtmlGenericControl)Master.FindControl("pageTitle");
            if (pageTitle != null)
            {
                pageTitle.InnerText = "Pending Invoice";
            }

            if (!IsPostBack)
            {
                BindGrids();
            }
        }

        private void BindGrids()
        {
            // Bind Header Grid
            DataTable dtHeader = new DataTable();
            dtHeader.Columns.Add("PurchId");
            dtHeader.Columns.Add("InvoiceDate", typeof(DateTime));
            dtHeader.Columns.Add("InvoiceId");
            dtHeader.Columns.Add("OnHold");
            dtHeader.Columns.Add("CurrencyCode");

            // Sample Header Data
            dtHeader.Rows.Add("USMF-000000001", new DateTime(2024, 2, 18), "2/21/2024", "", "USD");
            
            gvPendingInvoiceHeader.DataSource = dtHeader;
            gvPendingInvoiceHeader.DataBind();

            // Bind Lines Grid
            DataTable dtLines = new DataTable();
            dtLines.Columns.Add("PurchId");
            dtLines.Columns.Add("LineNum");
            dtLines.Columns.Add("ItemId");
            dtLines.Columns.Add("ProcurementCategory");
            dtLines.Columns.Add("Name");
            dtLines.Columns.Add("InventSiteId");
            dtLines.Columns.Add("InventLocationId");

            // Sample Lines Data
            dtLines.Rows.Add("USMF-000000001", "1", "D0003", "OFFICE MACHINES", "StandardSpeaker", "1", "11");
            
            gvPendingInvoiceLines.DataSource = dtLines;
            gvPendingInvoiceLines.DataBind();
        }
    }
}
