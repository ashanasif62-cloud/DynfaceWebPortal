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
    public partial class DropDownList_ProjectID : System.Web.UI.UserControl
    {
        public event EventHandler ProjectSelected = delegate { };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PurchaseOrderHeader neworder = new PurchaseOrderHeader();
                DataTable dataTable = neworder.retrieveprojectID();
                gvProjectDetails.DataSource = dataTable;
                gvProjectDetails.DataBind();
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvProjectDetails.Rows)
            {
                if (gridViewRow.RowType != DataControlRowType.DataRow)
                    return;

                gridViewRow.ToolTip = "Click to select Project";
                gridViewRow.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvProjectDetails, "Select$" + gridViewRow.RowIndex, true);
            }

            base.Render(writer);
        }
        protected void gvProjectDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProjectId.Text = gvProjectDetails.SelectedRow != null ? Server.HtmlDecode(gvProjectDetails.SelectedRow.Cells[0].Text) : "";

            gvProjectDetails.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f3f3f3");

            ProjectSelected(this, new EventArgs());
        }
        protected void gvProjectDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            // On Click send a SelectEvent so that the SelectedIndexChanged-EventHandler gets called
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvProjectDetails, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select Project";
        }
    }
}
