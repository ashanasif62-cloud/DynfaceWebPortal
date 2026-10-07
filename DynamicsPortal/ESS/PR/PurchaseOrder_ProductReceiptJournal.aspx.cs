using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DynamicsPortal.ESS.PR
{
    public partial class PurchaseOrder_ProductReceiptJournal : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Product receipt Journal";
            }

            if (!IsPostBack)
            {
                string purchId = Session["PurchaseOrderId"] as string;
                if (string.IsNullOrEmpty(purchId))
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "alertMessage",
                        "alert('No Purchase Order selected.');",
                        true
                    );
                    return;
                }
                BindOverviewGrid(purchId);
                BindLinesGrid(purchId);
            }

        }

        protected void gvOverview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
              
                TextBox txtDate = e.Row.FindControl("txtDate") as TextBox;

                if (txtDate != null && DateTime.TryParse(txtDate.Text, out DateTime dateValue))
                {
                    txtDate.Text = dateValue.ToString("yyyy-MM-dd");  // Remove time completely
                }
                TextBox txtDate1 = e.Row.FindControl("txtInvoiceDate") as TextBox;

                if (txtDate1 != null && DateTime.TryParse(txtDate1.Text, out DateTime dateValues))
                {
                    txtDate1.Text = dateValues.ToString("yyyy-MM-dd");  // Remove time completely
                }
            }
        }

        private void BindOverviewGrid(string purchId)
        {
            PurchaseOrder_ProductReceiptsJournal svc = new PurchaseOrder_ProductReceiptsJournal();

            // Create a DataTable to hold data for the grid
            DataTable dt = svc.retrieveAll(purchId);
            //dt.Columns.Add("PurchaseOrder");
            //dt.Columns.Add("ProductReceipt");
            //dt.Columns.Add("Date", typeof(DateTime));
            //dt.Columns.Add("InvoiceIssueDueDate", typeof(DateTime));
            //dt.Columns.Add("Terms");
            //dt.Columns.Add("ModeOfDelivery");
            //dt.Columns.Add("Company");
            //dt.Columns.Add("SalesOrder");
            //dt.Columns.Add("PostedViaIntercompany");

            // Add sample rows (replace these with your real data later)


            // Bind data to the GridView
            gvOverview.DataSource = dt;
            gvOverview.DataBind();
        }
        private void BindLinesGrid(string purchId)
        {
            PurchaseOrder_ProductReceiptsJournal lines = new PurchaseOrder_ProductReceiptsJournal();
            DataTable dt = lines.retrievelines(purchId);
            //dt.Columns.Add("PurchaseOrder");
            //dt.Columns.Add("LineNumber");
            //dt.Columns.Add("Item");
            //dt.Columns.Add("ProcurementCategory");
            //dt.Columns.Add("Description");
            //dt.Columns.Add("Site");
            //dt.Columns.Add("Warehouse");
            //dt.Columns.Add("InventoryStatus");
            //dt.Columns.Add("Ordered", typeof(decimal));
            //dt.Columns.Add("Received", typeof(decimal));
            //dt.Columns.Add("Amount", typeof(decimal));
            //dt.Columns.Add("RemainingQuantity", typeof(decimal));
            //dt.Columns.Add("CWReceived", typeof(decimal));
            //dt.Columns.Add("UnpostedInvoice");
            //dt.Columns.Add("ReasonCode");
            //dt.Columns.Add("ReasonComment");

          

            gvLines.DataSource = dt;
            gvLines.DataBind();
        }

        protected void btnVouchers_Click(object sender, EventArgs e)
        {
            // Redirect to the Vouchers page
            Response.Redirect("~/ESS/PR/PurchaseOrder_Vouchers.aspx");
        }

       
        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void btnCopyPreview_Click(object sender, EventArgs e)
        {
            // Redirect to the Vouchers page
           
        }




        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ESS/PR/AllPurchaseOrder_ListPage.aspx", false);
        }


    }
}