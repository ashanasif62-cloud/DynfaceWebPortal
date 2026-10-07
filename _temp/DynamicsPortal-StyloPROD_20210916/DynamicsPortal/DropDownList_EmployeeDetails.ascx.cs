using GeneralAuxiliary;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class DropDownList_EmployeeDetails : System.Web.UI.UserControl
    {
        public event EventHandler EmployeeSelected = delegate { };

        protected void Page_Load(object sender, EventArgs e)
        {
            int rowIndex = -1;
            if (!IsPostBack)
            {
                DataTable dataTable = ControlsHelper.retriveEmployeeReportees();
                gvEmployeeDetails.DataSource = dataTable;
                gvEmployeeDetails.DataBind();

                string employeeId = SessionVariables.getCurrentEmployeeId();


                if (dataTable != null && !string.IsNullOrEmpty(employeeId))
                {
                    #region forSingleEmployee
                    //if (dataTable.Rows.Count == 1)
                    //{
                    //    DataRow employeeDataRow = dataTable.Rows[0];
                    //    string employeeId = employeeDataRow["EmployeeId"].ToString();
                    //    rowIndex = 0;
                    //    //gvEmployeeDetails.Rows[0].Selected = true;
                    //    //button1.Click += new EventHandler(ButtonClicked); (cddlEmployeeDetails.FindControl("gvEmployeeDetails") as System.Web.UI.WebControls.GridView)
                    //}
                    //else {} 
                    #endregion

                    foreach (GridViewRow row in gvEmployeeDetails.Rows)
                    {
                        if (row.Cells[0] != null && row.Cells[0].Text.ToString().Equals(employeeId))
                        {
                            rowIndex = row.RowIndex;
                            break;
                        }
                    }
                    //GridViewRow row01 = gvEmployeeDetails.Rows.Cast<GridViewRow>().Where(r => r.Cells[0].Text.Equals(employeeId)).FirstOrDefault();

                    if (rowIndex > -1)
                    {
                        gvEmployeeDetails.SelectedIndex = rowIndex;
                        gvEmployeeDetails_SelectedIndexChanged(gvEmployeeDetails, new EventArgs());
                    }
                }


            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow gridViewRow in gvEmployeeDetails.Rows)
            {
                if (gridViewRow.RowType != DataControlRowType.DataRow)
                    return;

                //r.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                //r.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                gridViewRow.ToolTip = "Click to select Employee";
                gridViewRow.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvEmployeeDetails, "Select$" + gridViewRow.RowIndex, true);
            }

            base.Render(writer);
        }

        protected void gvEmployeeDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            // On Click send a SelectEvent so that the SelectedIndexChanged-EventHandler gets called
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvEmployeeDetails, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select Employee";
        }

        protected void gvEmployeeDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEmployeeId.Text = gvEmployeeDetails.SelectedRow != null ? Server.HtmlDecode(gvEmployeeDetails.SelectedRow.Cells[0].Text) : "";
            //string selectedEmployeeId = txtEmployeeId.Text;
            gvEmployeeDetails.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#f3f3f3");

            //if (!string.IsNullOrEmpty(selectedEmployeeId))
            //{
            //    DataTable dtAllEmployees = ControlsHelper.retriveEmployeeReportees();
            //    drEmployeeDetails = dtAllEmployees.Select("EmployeeId = '" + selectedEmployeeId + "'").FirstOrDefault();
            //}
            //bubble the event up to the parent
            //OnEmployeeSelection?.Invoke(this, e);

            // bubble up the event to parent. 
            EmployeeSelected(this, new EventArgs());

            //getTextBoxes(Page);
        }

        //private void getTextBoxes(Control parentControl)
        //{
        //    if (parentControl.HasControls())
        //    {
        //        foreach (Control control in parentControl.Controls)
        //        {
        //            getTextBoxes(control);
        //            if (control.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
        //            {
        //                TextBox txtBox = control as TextBox;
        //                if (txtBox != null)
        //                {
        //                    fillEmployeeDetails(txtBox);
        //                }
        //            }
        //            //(c.GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
        //            //(c.GetType().ToString() == "System.Web.UI.WebControls.Button")
        //        }
        //    }
        //}
        //private void fillEmployeeDetails(TextBox _txtBox)
        //{
        //    TextBox txtBox = _txtBox;
        //    string value = string.Empty;
        //    if (drEmployeeDetails != null)
        //    {
        //        //txtBox.Enabled = true;
        //        if (txtBox.ID == "txtHiringDate")
        //        {
        //            value = drEmployeeDetails["joiningDate"].ToString();
        //            txtBox.Text = value;
        //        }
        //        if (txtBox.ID == "txtServiceDuration")
        //        {
        //            value = drEmployeeDetails["yearsOfService"].ToString();
        //            txtBox.Text = value;
        //        }
        //        if (txtBox.ID == "txtActivePayPeriod")
        //        {
        //            value = drEmployeeDetails["payPeriodCode"].ToString();
        //            txtBox.Text = value;
        //        }
        //    }

        //}

    }
}
