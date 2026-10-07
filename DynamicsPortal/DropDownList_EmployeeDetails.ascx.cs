using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class DropDownList_EmployeeDetails : System.Web.UI.UserControl
    {
        public event EventHandler EmployeeSelected = delegate { };

        public string PreSelectedEmployeeId { get; set; }



        public string SelectedEmployeeId
        {
            get { return txtEmployeeId.Text; }
            set { txtEmployeeId.Text = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int rowIndex = -1;
            DataTable dataTable = ControlsHelper.retriveEmployeeReportees();
            gvEmployeeDetails.DataSource = dataTable;
            gvEmployeeDetails.DataBind();
            if (!IsPostBack)
            {

                string employeeId = !string.IsNullOrEmpty(PreSelectedEmployeeId) ? this.findEmployeeIdfromRecId(PreSelectedEmployeeId) : SessionVariables.getCurrentEmployeeId();
    
                if (dataTable != null && !string.IsNullOrEmpty(employeeId))
                {
                    // Find the employee in the dataTable first (faster than looping through GridView rows)
                    DataRow targetRow = dataTable.AsEnumerable()
                        .FirstOrDefault(r => r.Field<string>("EmployeeId") == employeeId);

                    if (targetRow != null)
                    {
                        rowIndex = dataTable.Rows.IndexOf(targetRow);
                        gvEmployeeDetails.SelectedIndex = rowIndex;
                        // Trigger the selection logic without a full loop
                        txtEmployeeId.Text = employeeId;
                        EmployeeSelected(this, new EventArgs());
                    }
                }


            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetEmployees(string searchText, int page)
        {
            const int pageSize = 5;

            try
            {
                // Use the service method you already have
                var svc = new TASRosterChangeRequestsSvc(); // change if your class name is different
                DataTable dt = svc.retrieveEmployeeId(searchText ?? "", page);

                var items = new List<object>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string empId = row.Table.Columns.Contains("EmployeeId")
                            ? row["EmployeeId"]?.ToString()
                            : row["PersonnelNumber"]?.ToString();

                        string empName = row.Table.Columns.Contains("EmployeeName")
                            ? row["EmployeeName"]?.ToString()
                            : row["Name"]?.ToString();

                        items.Add(new
                        {
                            EmployeeId = empId,
                            EmployeeName = empName
                        });
                    }
                }

                bool more = (dt != null && dt.Rows.Count >= pageSize);

                return new
                {
                    items = items,
                    more = more
                };
            }
            catch
            {
                return new { items = new List<object>(), more = false };
            }
        }

        private string findEmployeeIdfromRecId(string _recId)
        {
            DataTable dataTable = (DataTable)gvEmployeeDetails.DataSource;

            if (dataTable == null) return "";

            DataRow row = dataTable.AsEnumerable()
                .FirstOrDefault(r => r["WorkerRecId"].ToString() == _recId);

            return row != null ? row["EmployeeId"].ToString() : "";
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

            string postbackRef = Page.ClientScript.GetPostBackClientHyperlink(
                gvEmployeeDetails, "Select$" + e.Row.RowIndex);

            // Strip the "javascript:" prefix for eval
            string postbackScript = postbackRef.Replace("javascript:", "");

            e.Row.Attributes["onclick"] =
                $"selectEmployee('{mddEmployeeDetails.ClientID}', '{postbackScript}');";
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
