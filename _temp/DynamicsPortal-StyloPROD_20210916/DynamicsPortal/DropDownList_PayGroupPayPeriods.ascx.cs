using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class DropDownList_PayGroupPayPeriods : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            gvPayGroupPayPeriods.DataSource = ControlsHelper.retrieveAllPayGroupPayPeriods();
            gvPayGroupPayPeriods.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvPayGroupPayPeriods.Rows)
            {
                if (gridViewRow.RowType != DataControlRowType.DataRow)
                    return;
                //r.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                //r.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                gridViewRow.ToolTip = "Click to select Pay Period Code";
                gridViewRow.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvPayGroupPayPeriods, "Select$" + gridViewRow.RowIndex, true);
            }

            base.Render(writer);
        }
        protected void gvPayGroupPayPeriods_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvPayGroupPayPeriods, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select Pay Period Code";
        }

        protected void gvPayGroupPayPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = gvPayGroupPayPeriods.SelectedRow != null ? Server.HtmlDecode(gvPayGroupPayPeriods.SelectedRow.Cells[1].Text) : "";
            txtPayPeriodCode.Text = selectedValue;
        }
    }
}