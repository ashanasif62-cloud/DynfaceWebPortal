using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class DropDownList_AllEmployeeDetails : System.Web.UI.UserControl
    {

        public event EventHandler EmployeeSelected = delegate { };
        protected void Page_Load(object sender, EventArgs e)
        {
            gvAllEmployeeDetails.DataSource = ControlsHelper.retrieveAllEmployee();
            gvAllEmployeeDetails.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvAllEmployeeDetails.Rows)
            {

                if (gridViewRow.RowType != DataControlRowType.DataRow)
                    return;
                //r.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                //r.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                gridViewRow.ToolTip = "Click to select Employee";
                gridViewRow.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvAllEmployeeDetails, "Select$" + gridViewRow.RowIndex, true);
            }

            base.Render(writer);
        }

        protected void gvAllEmployeeDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            // On Click send a SelectEvent so that the SelectedIndexChanged-EventHandler gets called
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvAllEmployeeDetails, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select Employee";
        }

        protected void gvAllEmployeeDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEmployeeId.Text = gvAllEmployeeDetails.SelectedRow != null ? Server.HtmlDecode(gvAllEmployeeDetails.SelectedRow.Cells[0].Text) : "";

            gvAllEmployeeDetails.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f3f3f3");

            // bubble up the event to parent. 
            EmployeeSelected(this, new EventArgs());
        }
    }
}