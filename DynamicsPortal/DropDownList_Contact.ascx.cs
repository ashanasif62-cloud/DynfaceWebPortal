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
    public partial class DropDownList_Contact : System.Web.UI.UserControl
    {
        public event EventHandler ContactSelected = delegate { };
        protected void Page_Load(object sender, EventArgs e)
        {
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dataTable = neworder.retrievecontactdetails();
            gvContactDetails.DataSource = dataTable;
            gvContactDetails.DataBind();

        }
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvContactDetails.Rows)
            {
                if (gridViewRow.RowType == DataControlRowType.DataRow)
                {
                    string instantScript = "window.suppressOverlay = true; $(this).closest('.dropdown-panel').removeClass('show'); ";
                    gridViewRow.ToolTip = "Click to select Contact";
                    gridViewRow.Attributes["onclick"] = instantScript + Page.ClientScript.GetPostBackClientHyperlink(gvContactDetails, "Select$" + gridViewRow.RowIndex, true);
                    gridViewRow.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#f5f5f5';";
                    gridViewRow.Attributes["onmouseout"] = "this.style.backgroundColor='';";
                }
            }

            base.Render(writer);
        }
        protected void gvContactDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtContactId.Text = gvContactDetails.SelectedRow != null ? Server.HtmlDecode(gvContactDetails.SelectedRow.Cells[0].Text) : "";

            gvContactDetails.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f3f3f3");

            ContactSelected(this, new EventArgs());
        }
        protected void gvContactDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string instantScript = "window.suppressOverlay = true; $(this).closest('.dropdown-panel').removeClass('show'); ";
                // On Click send a SelectEvent so that the SelectedIndexChanged-EventHandler gets called
                e.Row.Attributes["onclick"] = instantScript + Page.ClientScript.GetPostBackClientHyperlink(gvContactDetails, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select Contact";
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#f5f5f5';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='';";
            }
        }
    }
}