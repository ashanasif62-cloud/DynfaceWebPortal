using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSEmploymentCertificateSvcReference;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using PortalIntegration.TransferOrderLinesSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Controls;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSRequestForLetter_Edit : ModalForm
    {
        private HRRequestForLetters letters = new HRRequestForLetters();
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // This logic is usually only needed on the initial page load
                pageMenuId = "ESSRequestForLetter_Edit";

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
                BindCertificateType(Session["CertificateTypeCode"].ToString());
                BindRequestedFor(Session["ReqestedFor"].ToString());
            }


        }

        private bool BindCertificateType(string selectedValue = "")
        {
            HRRequestForLetters letters = new HRRequestForLetters();
            DataTable dt = letters.retrieveCertificateType();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlCertificateType.DataSource = dt;
                ddlCertificateType.DataTextField = "CertificateTypeCode";   // what user sees
                ddlCertificateType.DataValueField = "CertificateTypeCode";  // underlying value
                ddlCertificateType.DataBind();

                // Insert empty option at the top
                ddlCertificateType.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlCertificateType.Items.FindByValue(selectedValue) != null)
            {
                ddlCertificateType.SelectedValue = selectedValue;
            }
            else
            {
                ddlCertificateType.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        private bool BindRequestedFor(string selectedValue = "")
        {
            HRRequestForLetters letters = new HRRequestForLetters();
            DataTable dt = letters.retrieveRequestedFor();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlRequestedFor.DataSource = dt;
                ddlRequestedFor.DataTextField = "RequestedForId";   // what user sees
                ddlRequestedFor.DataValueField = "RequestedForId";  // underlying value
                ddlRequestedFor.DataBind();

                // Insert empty option at the top
                ddlRequestedFor.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlRequestedFor.Items.FindByValue(selectedValue) != null)
            {
                ddlRequestedFor.SelectedValue = selectedValue;
            }
            else
            {
                ddlRequestedFor.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a record is selected
                if (Session["SelectedRecIds"] == null)
                {
                    NotificationMessage.showMessage("No record selected.");
                    return;
                }

                long recId = Convert.ToInt64(Session["SelectedRecIds"]);

                // Validate dropdown selections
                if (string.IsNullOrEmpty(ddlCertificateType.SelectedValue))
                {
                    NotificationMessage.showMessage("Please select a Certificate Type.");
                    return;
                }

                if (string.IsNullOrEmpty(ddlRequestedFor.SelectedValue))
                {
                    NotificationMessage.showMessage("Please select Requested For.");
                    return;
                }

                // Initialize service
                HRRequestForLetters letters = new HRRequestForLetters();

                // Create DataTable using service helper
                DataTable dataTable = letters.createDataTable();

                // ✅ Create new DataRow
                DataRow dataRow = dataTable.NewRow();

                // ✅ Fill DataRow fields
                dataRow["RecId"] = recId;
                dataRow["CertificateTypeCode"] = ddlCertificateType.SelectedValue;
                dataRow["ReqestedFor"] = ddlRequestedFor.SelectedValue;
                dataRow["Remarks"] = txtRemarks.Text;

                // Add DataRow to DataTable
                dataTable.Rows.Add(dataRow);

                // ✅ Call update method
                SysOperationResult_BOL result = letters.update(dataTable, recId);

                // Handle result
                if (result != null && result.isSuccess)
                {
                    result.Message = "Employment certificate updated successfully.";
                    NotificationMessage.showMessage(result);
                }
                else
                {
                    string msg = (result != null && !string.IsNullOrWhiteSpace(result.Message))
                        ? result.Message
                        : "Failed to update employment certificate.";
                    NotificationMessage.showMessage(msg);
                }
            }
            catch (Exception ex)
            {
                NotificationMessage.showMessage("Error: " + ex.Message);
            }
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox)?.Text;

            if (!string.IsNullOrEmpty(personalNumber))
            {
                // Retrieve employee details (still useful if you want other info)
                HcmWorkerDetailsSvcContract getWorkerDetails = ControlsHelper.getWorkerDetails(personalNumber);

                // You can now use other fields if needed, for example:
                // txtEmployeeName.Text = getWorkerDetails.WorkerName;
                // txtPosition.Text = getWorkerDetails.Position;

                // Since you don't want JobId or Department, we simply skip those
            }
        }





    }
}