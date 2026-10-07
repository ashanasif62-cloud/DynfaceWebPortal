using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class DropDownList_Currency : System.Web.UI.UserControl
    {
        public event EventHandler CurrencySelected = delegate { };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dataTable = ControlsHelper.retrieveAllCurrencyDetails();
                gvCurrencyDetails.DataSource = dataTable;
                gvCurrencyDetails.DataBind();
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvCurrencyDetails.Rows)
            {
                if (gridViewRow.RowType != DataControlRowType.DataRow)
                    return;

                gridViewRow.ToolTip = "Click to select Currency";
                gridViewRow.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvCurrencyDetails, "Select$" + gridViewRow.RowIndex, true);
            }

            base.Render(writer);
        }
        protected void gvCurrencyDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCurrencyCode.Text = gvCurrencyDetails.SelectedRow != null ? Server.HtmlDecode(gvCurrencyDetails.SelectedRow.Cells[0].Text) : "";

            gvCurrencyDetails.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f3f3f3");

            CurrencySelected(this, new EventArgs());
        }
        protected void gvCurrencyDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            // On Click send a SelectEvent so that the SelectedIndexChanged-EventHandler gets called
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvCurrencyDetails, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select Currency";
        }
    }
}
