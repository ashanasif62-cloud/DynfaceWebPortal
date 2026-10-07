using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class EmploymentCertificate_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // This logic is usually only needed on the initial page load
                pageMenuId = "EmploymentCertificate_Create";

                // Setting the page title dynamically
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Request For Certificate";
                    titleDiv.Style["font-weight"] = "600";    // semi-bold
                    titleDiv.Style["font-size"] = "20px";    // slightly larger
                    titleDiv.Style["color"] = "#000000";      // solid black
                    titleDiv.Style["margin"] = "10px 0";      // spacing around
                }

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Collect form data
            //string employee = txtEmployee.Text;
            //string job = txtJob.Text;
            //string department = txtDepartment.Text;
            //string requestedDate = txtRequestedDate.Text;
            //string certificateType = ddlCertificateType.SelectedValue;
            //string requestedFor = ddlRequestedFor.SelectedValue;
            //string remarks = txtRemarks.Text;

            //// TODO: Call your service class to create a certificate request here

            //// Example:
            //// EmploymentCertificateSvc.create(employee, job, department, requestedDate, certificateType, requestedFor, remarks);

            ScriptManager.RegisterStartupScript(this, GetType(), "CloseDialog", "closeDialog();", true);
        }
    }
}
