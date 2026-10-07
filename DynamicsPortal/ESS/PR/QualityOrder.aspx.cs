using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class QualityOrder : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "PurchaseOrder_QualityOrder";

            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Quality Order";
                titleDiv.Style["font-weight"] = "600";
                titleDiv.Style["font-size"] = "20px";
                titleDiv.Style["color"] = "#000000";
                titleDiv.Style["margin"] = "10px 0";
            }

            // 🔹 Bind grid only on first load
            if (!IsPostBack)
            {
                BindOverviewGrid();
            }
        }

        private void BindOverviewGrid()
        {
            // 🔹 Create a sample DataTable
            DataTable dt = new DataTable();

            // Define columns only
            dt.Columns.Add("QualityOrder");
            dt.Columns.Add("ItemNumber");
            dt.Columns.Add("Site");
            dt.Columns.Add("Warehouse");
            dt.Columns.Add("Status");
            dt.Columns.Add("Worker");

            // Add an empty row to force header rendering
            dt.Rows.Add(dt.NewRow());

            GridViewOverview.DataSource = dt;
            GridViewOverview.DataBind();

            // Hide the dummy empty row so only headers appear
            if (GridViewOverview.Rows.Count > 0)
                GridViewOverview.Rows[0].Visible = false;
        }
    }
    
}
