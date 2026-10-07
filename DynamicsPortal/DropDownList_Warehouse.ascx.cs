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
    public partial class DropDownList_Warehouse : System.Web.UI.UserControl
    {
        public event EventHandler StoreSelected = delegate { };
        protected void Page_Load(object sender, EventArgs e)
        {
            TransferOrderHeader transferordernew = new TransferOrderHeader();
            DataTable dataTable = transferordernew.retrievefromwarehouse();
            gvWearhouseDetails.DataSource = dataTable;
            gvWearhouseDetails.DataBind();
        }
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvWearhouseDetails.Rows)
            {
                if (gridViewRow.RowType == DataControlRowType.DataRow)
                {
                    string instantScript = "window.suppressOverlay = true; $(this).closest('.dropdown-panel').removeClass('show'); ";
                    gridViewRow.ToolTip = "Click to select Warehouse";
                    gridViewRow.Attributes["onclick"] = instantScript + Page.ClientScript.GetPostBackClientHyperlink(gvWearhouseDetails, "Select$" + gridViewRow.RowIndex, true);
                    gridViewRow.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#f5f5f5';";
                    gridViewRow.Attributes["onmouseout"] = "this.style.backgroundColor='';";
                }
            }

            base.Render(writer);
        }
        protected void gvStoreDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtWarehouseId.Text = gvWearhouseDetails.SelectedRow != null ? Server.HtmlDecode(gvWearhouseDetails.SelectedRow.Cells[0].Text) : "";

            gvWearhouseDetails.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f3f3f3");

            StoreSelected(this, new EventArgs());
        }
        protected void gvWearhouseDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string instantScript = "window.suppressOverlay = true; $(this).closest('.dropdown-panel').removeClass('show'); ";
                // On Click send a SelectEvent so that the SelectedIndexChanged-EventHandler gets called
                e.Row.Attributes["onclick"] = instantScript + Page.ClientScript.GetPostBackClientHyperlink(gvWearhouseDetails, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select Warehouse";
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.backgroundColor='#f5f5f5';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='';";
            }
        }
    }
        
}