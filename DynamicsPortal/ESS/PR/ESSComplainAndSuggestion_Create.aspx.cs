using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeneralAuxiliary;
using PortalIntegration.ESSComplainAndSuggestionSvcReference;
using PortalIntegration;
using BussinessObject;

namespace DynamicsPortal.ESS.PR
{
    public partial class ESSComplainAndSuggestion_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ComplainAndSuggestion_Create";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated || !isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Complain And Suggestions";

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Complain And Suggestions";
                        titleDiv.Style["font-weight"] = "bold";
                    }

                    txtRequestDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

                    // ========== NO default selection ==========
                    // Do NOT call BindTypeCodes here
                    // Just clear the dropdown
                    ddlTypeCode.Items.Clear();
                    ddlTypeCode.Items.Add(new ListItem("-- Select Type Code --", ""));
                }
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(GetType().FullName, ex);
            }
        }

        protected void rblRequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = rblRequestType.SelectedValue; // "Complaint" or "Suggestion"

            if (!string.IsNullOrEmpty(selectedType))
            {
                BindTypeCodes(selectedType);
            }
            else
            {
                // Just in case
                ddlTypeCode.Items.Clear();
                ddlTypeCode.Items.Add(new ListItem("-- Select Type Code --", ""));
            }

            // Clear the visible search box
            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearSearch",
                "document.getElementById('typeCodeSearchInput').value = '';", true);
        }
        private void BindTypeCodes(string type)
        {
            ESSComplainAndSuggestionsSvc service = new ESSComplainAndSuggestionsSvc();
            try
            {
                DataTable dt = service.retrieveTypeCode(type, string.Empty);

                ddlTypeCode.Items.Clear();
                ddlTypeCode.Items.Add(new ListItem("-- Select Type Code --", ""));

                if (dt != null && dt.Rows.Count > 0)
                {
                    string valueField = dt.Columns.Contains("TypeCode") ? "TypeCode" :
                                        dt.Columns.Contains("Code") ? "Code" : dt.Columns[0].ColumnName;

                    string textField = dt.Columns.Contains("TypeCode") ? "TypeCode" :
                                       dt.Columns.Contains("Name") ? "Name" :
                                       dt.Columns.Contains("Description") ? "Description" : dt.Columns[0].ColumnName;

                    ddlTypeCode.DataSource = dt;
                    ddlTypeCode.DataValueField = valueField;
                    ddlTypeCode.DataTextField = textField;
                    ddlTypeCode.DataBind();
                }
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(GetType().FullName + ".BindTypeCodes", ex);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // ========== VALIDATION ==========

                if (string.IsNullOrWhiteSpace(rblRequestType.SelectedValue))
                {
                    NotificationMessage.showMessage(
                        "Please select Request Type.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlTypeCode.SelectedValue))
                {
                    NotificationMessage.showMessage(
                        "Please select Type Code.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDescription.Text))
                {
                    NotificationMessage.showMessage(
                        "Please enter Description.");
                    return;
                }

                // ========== GET EMPLOYEE ==========

                TextBox txtEmployeeId =
                    cddlEmployeeDetails.FindControl("txtEmployeeId")
                    as TextBox;

                string personnelNumber =
                    txtEmployeeId != null
                        ? txtEmployeeId.Text.Trim()
                        : string.Empty;

                if (string.IsNullOrWhiteSpace(personnelNumber))
                {
                    personnelNumber =
                        SessionVariables.getCurrentEmployeeId();
                }

                if (string.IsNullOrWhiteSpace(personnelNumber))
                {
                    NotificationMessage.showMessage(
                        "Employee is required.");
                    return;
                }

                // ========== BUILD DATATABLE ==========

                DataTable dt = new DataTable();

                dt.Columns.Add("PersonnelNumber", typeof(string));
                dt.Columns.Add("RequestDate", typeof(DateTime));
                dt.Columns.Add("TypeCode", typeof(string));
                dt.Columns.Add("RequestType", typeof(string));
                dt.Columns.Add("Description", typeof(string));
                dt.Columns.Add("Remarks", typeof(string));
                dt.Columns.Add("ApprovalStatus", typeof(string));
                dt.Columns.Add("RequestedBy", typeof(string));

                DataRow row = dt.NewRow();

                row["PersonnelNumber"] = personnelNumber;
                row["RequestDate"] = DateTime.Today;
                row["TypeCode"] = ddlTypeCode.SelectedValue;
                row["RequestType"] = rblRequestType.SelectedValue;
                row["Description"] = txtDescription.Text.Trim();
                row["Remarks"] = txtRemarks.Text.Trim();
                row["ApprovalStatus"] = "0";
                row["RequestedBy"] =
                    SessionVariables.getCurrentEmployeeId();

                dt.Rows.Add(row);

                // ========== CALL SERVICE ==========

                ESSComplainAndSuggestionsSvc service =
                    new ESSComplainAndSuggestionsSvc();

                SysOperationResult_BOL result =
                    service.create(dt);

                if (result != null && result.isSuccess)
                {
                    // Show success notification
                    NotificationMessage.showMessage(result);

                    // Close modal and refresh parent grid
                    string script = @"
                setTimeout(function() {
                    if (window.parent &&
                        typeof window.parent.refreshParentGrid === 'function') {
                        window.parent.refreshParentGrid();
                    }

                    if (typeof closeDialog === 'function') {
                        closeDialog();
                    }
                    else if (window.parent &&
                             typeof window.parent.closeDialog === 'function') {
                        window.parent.closeDialog();
                    }
                }, 1500);";

                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "CloseAndRefresh",
                        script,
                        true);

                    return;
                }

                // ========== SERVICE ERROR ==========

                SysOperationResult_BOL errorResult =
                    new SysOperationResult_BOL();

                errorResult.isSuccess = false;
                errorResult.AlertType = AlertType.Error.ToString();

                errorResult.Message =
                    result != null &&
                    !string.IsNullOrWhiteSpace(result.Message)
                        ? result.Message
                        : "Failed to create the record.";

                NotificationMessage.showMessage(errorResult);
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(
                    GetType().FullName + ".btnOK_Click", ex);

                SysOperationResult_BOL errorResult =
                    new SysOperationResult_BOL();

                errorResult.isSuccess = false;
                errorResult.AlertType = AlertType.Error.ToString();
                errorResult.Message =
                    "An error occurred while creating the record.";

                NotificationMessage.showMessage(errorResult);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Just close the modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeDialog();", true);
        }
    }
}