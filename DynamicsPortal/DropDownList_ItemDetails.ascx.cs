using PortalIntegration;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class DropDownList_ItemDetails : System.Web.UI.UserControl
    {
        public event EventHandler ItemSelected = delegate { };
        public void Load(DataTable dt)
        {
            gvItemDetails.DataSource = dt;
            gvItemDetails.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvItemDetails.Rows)
            {
                if (gridViewRow.RowType != DataControlRowType.DataRow)
                    return;

                gridViewRow.ToolTip = "Click to select Item";
                gridViewRow.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvItemDetails, "Select$" + gridViewRow.RowIndex, true);
            }

            base.Render(writer);
        }

        protected void gvgvItemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvItemDetails, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select Item";
        }

        protected void gvItemDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtItemId.Text = gvItemDetails.SelectedRow != null ? Server.HtmlDecode(gvItemDetails.SelectedRow.Cells[0].Text) : "";
            gvItemDetails.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f3f3f3");
            ItemSelected(this, new EventArgs());
        }
    }
}