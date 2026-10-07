using System;
using System.Data;
using PortalIntegration;
using GeneralAuxiliary;

namespace DynamicsPortal
{
    public partial class ESSPROutstandingAdvances_ListPage : MainForm
    {
        private PROutstandingAdvances logic = new PROutstandingAdvances();

        protected override void Page_Load(object sender, EventArgs e)
        {
            tableId = "PROutstandingAdvancesSummary";
            pageMenuId = "ESSPROutstandingAdvances_ListPage";

            base.Page_Load(sender, e);
            if (!isUserAuthenticated) return;

            if (!IsPostBack)
            {
                reBindGrid();
            }
        }

        protected void reBindGrid()
        {
            DataTable dt = logic.getOutstandingAdvances();
            gvOutstanding.DataSource = dt;
            gvOutstanding.DataBind();

            if (dt != null && dt.Rows.Count > 0)
            {
                CalculateFooter(dt);
            }
        }

        private void CalculateFooter(DataTable dt)
        {
            decimal totalAdv = 0, totalRec = 0, totalRem = 0;
            foreach (DataRow dr in dt.Rows)
            {
                totalAdv += Convert.ToDecimal(dr["AdvanceAmount"]);
                totalRec += Convert.ToDecimal(dr["TotalRecoveryAmount"]);
                totalRem += Convert.ToDecimal(dr["RemainingAmount"]);
            }
            if (gvOutstanding.FooterRow != null)
            {
                gvOutstanding.FooterRow.Cells[0].Text = "Total";
                gvOutstanding.FooterRow.Cells[3].Text = totalAdv.ToString("N2");
                gvOutstanding.FooterRow.Cells[4].Text = totalRec.ToString("N2");
                gvOutstanding.FooterRow.Cells[5].Text = totalRem.ToString("N2");
            }
        }
    }
}
