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
    public partial class DropDownList_SiteDetails : System.Web.UI.UserControl
    {
          public event EventHandler SiteSelected = delegate { };
        protected void Page_Load(object sender, EventArgs e)
        {
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dataTable = neworder.retrievesiteinfo();
            gvSiteDetails.DataSource = dataTable;
            gvSiteDetails.DataBind();

        }
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvSiteDetails.Rows)
            {
                if (gridViewRow.RowType == DataControlRowType.DataRow)
                {
                    string instantScript = "window.suppressOverlay = true; $(this).closest('.dropdown-panel').removeClass('show'); ";
                    gridViewRow.ToolTip = "Click to select Site";
                    gridViewRow.Attributes["onclick"] = instantScript + Page.ClientScript.GetPostBackClientHyperlink(gvSiteDetails, "Select$" + gridViewRow.RowIndex, true);
                    gridViewRow.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#f5f5f5';";
                    gridViewRow.Attributes["onmouseout"] = "this.style.backgroundColor='';";
                }
            }

            base.Render(writer);
        }
        protected void gvSiteDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSiteId.Text = gvSiteDetails.SelectedRow != null ? Server.HtmlDecode(gvSiteDetails.SelectedRow.Cells[0].Text) : "";

            gvSiteDetails.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f3f3f3");

            SiteSelected(this, new EventArgs());
        }
        protected void gvSiteDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string instantScript = "window.suppressOverlay = true; $(this).closest('.dropdown-panel').removeClass('show'); ";
                // On Click send a SelectEvent so that the SelectedIndexChanged-EventHandler gets called
                e.Row.Attributes["onclick"] = instantScript + Page.ClientScript.GetPostBackClientHyperlink(gvSiteDetails, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select Site";
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#f5f5f5';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='';";
            }
        }
    }
}