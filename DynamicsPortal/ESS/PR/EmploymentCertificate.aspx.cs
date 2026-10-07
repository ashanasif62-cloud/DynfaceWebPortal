using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class EmploymentCertificate : MainForm
    {
        private ESSEmploymentCertificate employmentCertificate = new ESSEmploymentCertificate();
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // This logic is usually only needed on the initial page load
                pageMenuId = "EmploymentCertificate_ListPage";

                // Setting the page title dynamically
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Employment Certificate";
                    titleDiv.Style["font-weight"] = "600";    // semi-bold
                    titleDiv.Style["font-size"] = "20px";    // slightly larger
                    titleDiv.Style["color"] = "#000000";      // solid black
                    titleDiv.Style["margin"] = "10px 0";      // spacing around
                }
                bindGrid();

            }
        }

        private void getGridDataTable()
        {
            DataTable dt = employmentCertificate.retriveEmployeeReportees();
            SessionVariables.setSessionDataTable(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();
        }
        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // TODO: Implement delete logic
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // TODO: Implement submit logic
        }
    }
}