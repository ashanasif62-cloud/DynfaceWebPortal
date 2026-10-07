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
    public partial class DropDownList_VendorAccount : System.Web.UI.UserControl
    {
        public event EventHandler VendorAccountSelected = delegate { };
        protected void Page_Load(object sender, EventArgs e)
        {
            PurchaseOrderHeader neworder = new PurchaseOrderHeader();
            DataTable dataTable = neworder.retrieveVendor();
            gvVendorAccountDetails.DataSource = dataTable;
            gvVendorAccountDetails.DataBind();
        }
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvVendorAccountDetails.Rows)
            {
                if (gridViewRow.RowType == DataControlRowType.DataRow)
                {
                    string instantScript = "window.suppressOverlay = true; if(typeof populateVendorFields === 'function') populateVendorFields(this); $(this).closest('.dropdown-panel').removeClass('show'); ";
                    gridViewRow.ToolTip = "Click to select Vendor Account";
                    gridViewRow.Attributes["onclick"] = instantScript + Page.ClientScript.GetPostBackClientHyperlink(gvVendorAccountDetails, "Select$" + gridViewRow.RowIndex, true);
                    gridViewRow.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#f5f5f5';";
                    gridViewRow.Attributes["onmouseout"] = "this.style.backgroundColor='';";
                }
            }

            base.Render(writer);
        }
        protected void gvVendorAccountDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtVendorAccountId.Text = gvVendorAccountDetails.SelectedRow != null ? Server.HtmlDecode(gvVendorAccountDetails.SelectedRow.Cells[0].Text) : "";

            gvVendorAccountDetails.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f3f3f3");

            VendorAccountSelected(this, new EventArgs());
        }
        protected void gvVendorAccountDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (drv != null)
                {
                    e.Row.Attributes["data-vendorname"] = drv["VendorName"].ToString();
                    e.Row.Attributes["data-address"] = drv["Address"].ToString();
                    e.Row.Attributes["data-serviceaddress"] = drv["ServiceAddress"].ToString();
                    e.Row.Attributes["data-changerequest"] = drv["ChangeRequestEnabled"].ToString();
                    e.Row.Attributes["data-purchpoolid"] = drv["PurchPoolID"].ToString();
                }

                string instantScript = "window.suppressOverlay = true; if(typeof populateVendorFields === 'function') populateVendorFields(this); $(this).closest('.dropdown-panel').removeClass('show'); ";
                // On Click send a SelectEvent so that the SelectedIndexChanged-EventHandler gets called
                e.Row.Attributes["onclick"] = instantScript + Page.ClientScript.GetPostBackClientHyperlink(gvVendorAccountDetails, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select Vendor Account";
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#f5f5f5';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='';";
            }
        }
    }
}