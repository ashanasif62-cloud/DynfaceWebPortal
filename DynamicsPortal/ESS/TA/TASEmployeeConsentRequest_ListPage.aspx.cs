using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.EmployeeConsentRequestSvcRefrence;

namespace DynamicsPortal.ESS.TA
{
    public partial class TASEmployeeConsentRequest : MainForm
    {
        private EmployeeConsentSvc service = new EmployeeConsentSvc();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "PREmployeeConsentRequest_ListPage";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Consent Request";
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Consent Request";
                        titleDiv.Style["font-weight"] = "bold";
                    }

                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
            }
        }

        private void BindGrid()
        {
            try
            {
                DataTable dt = service.retrieveAll();
                gridView.DataSource = dt;
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName + ".BindGrid", ex);
            }
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Optional
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            // Optional
        }

        // ========== YES BUTTON ==========
        protected void btnYes_Click(object sender, EventArgs e)
        {
            ProcessConsent("Yes");
        }

        // ========== NO BUTTON ==========
        protected void btnNo_Click(object sender, EventArgs e)
        {
            NotificationMessage.showMessage("No action taken.");
        }

        // Common method that updates the selected records
        private void ProcessConsent(string consentValue)
        {
            try
            {
                bool allSuccess = true;
                string lastMessage = "";
                int processedCount = 0;
                GeneralContract lastResult = null;

                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
                    if (chk == null || !chk.Checked)
                        continue;

                    Label lblRecId = row.FindControl("lblRecId") as Label;
                    if (lblRecId == null || !long.TryParse(lblRecId.Text, out long recId))
                        continue;

                    // Build full contract
                    PREmployeeConsentRequestContract contract = new PREmployeeConsentRequestContract();

                    Label lblEmployee = row.FindControl("lblEmployee") as Label;
                    Label lblRequestType = row.FindControl("lblRequestType") as Label;
                    Label lblDescription = row.FindControl("lblDescription") as Label;
                    Label lblStartDate = row.FindControl("lblStartDate") as Label;
                    Label lblEndDate = row.FindControl("lblEndDate") as Label;
                    Label lblRemarks = row.FindControl("lblRemarks") as Label;

                    if (lblEmployee != null) contract.EmployeeId = lblEmployee.Text;
                    if (lblRequestType != null) contract.ConsentType = lblRequestType.Text;
                    if (lblDescription != null) contract.Description = lblDescription.Text;
                    if (lblRemarks != null) contract.Remarks = lblRemarks.Text;

                    // Dates
                    if (lblStartDate != null && DateTime.TryParse(lblStartDate.Text, out DateTime startDate))
                        contract.StartDate = startDate;

                    if (lblEndDate != null && DateTime.TryParse(lblEndDate.Text, out DateTime endDate))
                        contract.ExpireDate = endDate;

                    // Set Consent Status
                    contract.ConsentRequestStatus = consentValue;   // "Yes"

                    // Call service
                    GeneralContract result = service.RespondToConsentRequest(recId, contract);
                    lastResult = result;
                    processedCount++;

                    if (result == null || !result.IsSuccess)
                    {
                        allSuccess = false;
                        lastMessage = result != null ? result.Message : "Update failed.";
                        break;
                    }
                }

                if (processedCount == 0)
                {
                    NotificationMessage.showMessage("Please select at least one record.");
                    return;
                }

                if (allSuccess)
                {
                    BindGrid();

                    if (lastResult != null)
                    {
                        // Extract the message from GeneralContract
                        string message = !string.IsNullOrEmpty(lastResult.Message)
                            ? lastResult.Message
                            : "Consent updated successfully.";

                        NotificationMessage.showMessage(message);
                    }
                    else
                    {
                        NotificationMessage.showMessage("Consent updated successfully.");
                    }
                }
                else
                {
                    NotificationMessage.showMessage(lastMessage);
                }
            
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName + ".ProcessConsent", ex);
                NotificationMessage.showMessage("An error occurred while updating consent.");
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            BindGrid();
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            BindGrid();
        }

      
    }
}