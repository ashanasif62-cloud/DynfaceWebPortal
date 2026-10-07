using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class TestingForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTestingForm();
            }
        }

        protected void BindTestingForm()
        {
            // Minimal in-memory sample data for the grid. Replace with DAL call using getConnection_DAL pattern.
            var data = new List<dynamic>
            {
                new { EmployeeID = "E001", EmployeeName = "John Doe", Department = "HR", StartDate = "2023-01-01", EndDate = "2023-12-31" },
                new { EmployeeID = "E002", EmployeeName = "Jane Smith", Department = "IT", StartDate = "2024-02-01", EndDate = "2024-11-30" }
            };

            gvTesting.DataSource = data;
            gvTesting.DataBind();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            // Clear form for new entry
            txtEmployeeID.Text = string.Empty;
            txtEmployeeName.Text = string.Empty;
            txtDepartment.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            // Show notification - replace with NotificationMessage.showMessage if available
            // For now use a client script registration to show a non-blocking message area if implemented in master
            ClientScript.RegisterStartupScript(this.GetType(), "msg", "console.log('Ready for new entry');", true);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // Put form into editable mode — fields are read-only by protocol; to edit, manual override required.
            // We'll just enable the textboxes temporarily here.
            txtEmployeeID.ReadOnly = false;
            txtEmployeeName.ReadOnly = false;
            txtDepartment.ReadOnly = false;
            txtStartDate.ReadOnly = false;
            txtEndDate.ReadOnly = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete selected record from datasource - placeholder
            ClientScript.RegisterStartupScript(this.GetType(), "msg", "console.log('Delete clicked');", true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Simple client-side filtering is not set up; rebind for now.
            BindTestingForm();
        }

        protected void gvTesting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTesting.PageIndex = e.NewPageIndex;
            BindTestingForm();
        }

        protected void gvTesting_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvTesting.SelectedRow;
            if (row != null)
            {
                txtEmployeeID.Text = row.Cells[1].Text;
                txtEmployeeName.Text = row.Cells[2].Text;
                txtDepartment.Text = row.Cells[3].Text;
                txtStartDate.Text = row.Cells[4].Text;
                txtEndDate.Text = row.Cells[5].Text;
            }
        }
    }
}