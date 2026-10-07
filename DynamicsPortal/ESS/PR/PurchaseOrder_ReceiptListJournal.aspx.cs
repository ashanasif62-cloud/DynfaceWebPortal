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
    public partial class PurchaseOrder_ReceiptListJournal : MainForm
    {
    
        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "ReceiptList";
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
        private void BindOverviewGrid(string purchId)
        {
            PurchaseOrder_ReceiptsList_Journal svc = new PurchaseOrder_ReceiptsList_Journal();
            DataTable dt = svc.retrieveAll(purchId);
            
            //dt.Columns.Add("VendorAccount");
            //dt.Columns.Add("PurchaseOrder");
            //dt.Columns.Add("ReceiptsList");
            //dt.Columns.Add("ReceiptsListDate", typeof(DateTime));
            //dt.Columns.Add("DeliveryDate", typeof(DateTime));
            //dt.Columns.Add("DeliveryTerms");
            //dt.Columns.Add("ModeOfDelivery");

     
           

            gvOverview.DataSource = dt;
            gvOverview.DataBind();
        }

        private void BindLinesGrid(string purchId)
        {
            PurchaseOrder_ReceiptsList_Journal svc = new PurchaseOrder_ReceiptsList_Journal();
            DataTable dt = svc.retrievelines(purchId);
           

         
          
            gvLines.DataSource = dt;
            gvLines.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ESS/PR/AllPurchaseOrder_ListPage.aspx", false);
        }

        protected void btnCopyPreview_Click(object sender, EventArgs e)
        {
            // Redirect to the Vouchers page

        }
    }
}