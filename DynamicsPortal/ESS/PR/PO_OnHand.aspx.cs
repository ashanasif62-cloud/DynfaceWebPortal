using System;
using System.Web.UI;

namespace DynamicsPortal.ESS.PR
{
    public partial class PO_OnHand : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "PurchaseOrder_OnHand";

            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "On Hand";
                titleDiv.Style["font-weight"] = "600";   // semi-bold
                titleDiv.Style["font-size"] = "20px";    // slightly larger
                titleDiv.Style["color"] = "#000000";     // solid black
                titleDiv.Style["margin"] = "10px 0";     // spacing around
            }
            if (!IsPostBack)
            {
                // Initialize dropdowns or data here if needed
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            // Close or redirect modal/page
            Response.Redirect("/ESS/PR/PurchaseOrderLines_ListPage.aspx"); // Example redirect
        }
    }
}
