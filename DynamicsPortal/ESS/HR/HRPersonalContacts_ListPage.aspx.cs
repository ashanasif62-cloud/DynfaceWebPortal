using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSPersonalContactsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.HR
{
    public partial class HRPersonalContact_ListPage : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Personal Contacts";
            }

            if (!IsPostBack)
            {
                string employeeid = SessionVariables.getCurrentEmployeeId();


                BindGrid(employeeid);
            }

        }

        private void BindGrid(string employeeId)
        {
            ESSHRPersonalContacts svc = new ESSHRPersonalContacts();

            // Get data from service
            
            DataTable dt = svc.retrieveAll(employeeId);

            // Ensure boolean columns exist (safety)
            EnsureBooleanColumn(dt, "isDependent");
            EnsureBooleanColumn(dt, "isBeneficiary");
            EnsureBooleanColumn(dt, "EmergencyContact");

            // Bind to GridView
            gvPersonalContacts.DataSource = dt;
            gvPersonalContacts.DataBind();
        }

        private void EnsureBooleanColumn(DataTable dt, string columnName)
        {
            if (dt == null || !dt.Columns.Contains(columnName))
                return;

            foreach (DataRow row in dt.Rows)
            {
                if (row[columnName] == DBNull.Value)
                {
                    row[columnName] = false;
                }
                else
                {
                    row[columnName] = Convert.ToBoolean(row[columnName]);
                }
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            DataTable dtRecords = new ESSHRPersonalContacts().createDataTable();
            foreach (GridViewRow gridViewRow in gvPersonalContacts.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    Label lblRecId = gridViewRow.FindControl("lblRecId") as Label;
                    Label lblRelationshipTypeId = gridViewRow.FindControl("lblRelationShipTypeId") as Label;
                    Label lblIsEmergencyContact = gridViewRow.FindControl("lblEmergencyContact") as Label;
                    Label lblIsDependent = gridViewRow.FindControl("lblIsDependent") as Label;
                    Label lblIsBeneficery = gridViewRow.FindControl("lblIsBeneficiary") as Label;
                    DataRow drRow = dtRecords.NewRow();
                    drRow["RecId"] = lblRecId.Text;
                    drRow["RelationShipTypeId"] = lblRelationshipTypeId.Text;
                    drRow["isDependent"] = lblIsDependent.Visible;
                    drRow["isBeneficiary"] = lblIsBeneficery.Visible;
                    drRow["EmergencyContact"] = lblIsEmergencyContact.Visible;

                    dtRecords.Rows.Add(drRow);
                }
            }

            ESSHRPersonalContacts personalContact = new ESSHRPersonalContacts();
            SysOperationResult_BOL result = personalContact.deletePersonalContact(dtRecords);
            NotificationMessage.showMessage(result);

            BindGrid(SessionVariables.getCurrentEmployeeId());
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            DataTable dtRecords = new ESSHRPersonalContacts().createDataTable();

            foreach (GridViewRow gridViewRow in gvPersonalContacts.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    DateTime defaultDate = DateTime.MinValue;
                    Label lblRecId = gridViewRow.FindControl("lblRecId") as Label;
                    Label lblName = gridViewRow.FindControl("lblName") as Label;
                    Label lblRelationshipTypeId = gridViewRow.FindControl("lblRelationShipTypeId") as Label;
                    Label lblIsEmergencyContact = gridViewRow.FindControl("lblEmergencyContact") as Label;
                    Label lblIsDependent = gridViewRow.FindControl("lblIsDependent") as Label;
                    Label lblIsBeneficery = gridViewRow.FindControl("lblIsBeneficiary") as Label;
                    Label lblBeneficiaryFromDate = gridViewRow.FindControl("lblBeneficiaryFromDate") as Label;
                    Label lblBeneficiaryToDate = gridViewRow.FindControl("lblBeneficiaryToDate") as Label;
                    Label lblBirthDate = gridViewRow.FindControl("lblBirthDate") as Label;
                    Label lblGender = gridViewRow.FindControl("lblGender") as Label;
                    Label lblIsFullTimeStudent = gridViewRow.FindControl("lblisFullTimeStudent") as Label;
                    Label lblIsPersonWithDisabilities = gridViewRow.FindControl("lblisPersonWithDisabilities") as Label;
                    Label lblVerificationDate = gridViewRow.FindControl("lblVerificationDate") as Label;
                    Label lbldependentValidFrom = gridViewRow.FindControl("lblDependentValidFrom") as Label;
                    Label lbldependentValidTo = gridViewRow.FindControl("lblDependentValidTo") as Label;
                    Label lblBeneficiaryPercentage = gridViewRow.FindControl("lblBeneficiaryPercentage") as Label;
                    DataRow drRow = dtRecords.NewRow();
                    drRow["RecId"] = lblRecId.Text;
                    drRow["Name"] = lblName.Text;
                    drRow["RelationShipTypeId"] = lblRelationshipTypeId.Text;
                    drRow["isDependent"] = lblIsDependent.Visible;
                    drRow["isBeneficiary"] = lblIsBeneficery.Visible;
                    drRow["EmergencyContact"] = lblIsEmergencyContact.Visible;
                    drRow["BeneficiaryFromDate"] = !string.IsNullOrWhiteSpace(lblBeneficiaryFromDate?.Text) ? Convert.ToDateTime(lblBeneficiaryFromDate.Text)
                        : defaultDate;
                    drRow["BeneficiaryToDate"] = !string.IsNullOrWhiteSpace(lblBeneficiaryToDate?.Text) ? Convert.ToDateTime(lblBeneficiaryToDate.Text)
                        : defaultDate;
                    drRow["BirthDate"] = !string.IsNullOrWhiteSpace(lblBirthDate?.Text) ? Convert.ToDateTime(lblBirthDate.Text) : defaultDate;
                    drRow["VerificationDate"] = !string.IsNullOrWhiteSpace(lblVerificationDate?.Text) ? Convert.ToDateTime(lblVerificationDate.Text)
                        : defaultDate;
                    drRow["Gender"] = !string.IsNullOrWhiteSpace(lblGender?.Text) ? lblGender.Text : "None";
                    drRow["isFullTimeStudent"] = !string.IsNullOrWhiteSpace(lblIsFullTimeStudent?.Text) ? lblIsFullTimeStudent.Text : "No";
                    drRow["isPersonWithDisabilities"] = !string.IsNullOrWhiteSpace(lblIsPersonWithDisabilities?.Text) ? lblIsPersonWithDisabilities.Text : "No";
                    drRow["DependentValidFrom"] = !string.IsNullOrWhiteSpace(lbldependentValidFrom?.Text) ? Convert.ToDateTime(lbldependentValidFrom.Text)
                        : defaultDate;
                    drRow["DependentValidTo"] = !string.IsNullOrWhiteSpace(lbldependentValidTo?.Text) ? Convert.ToDateTime(lbldependentValidTo.Text)
                        : defaultDate;
                    drRow["BenefecieryPercentage"] = !string.IsNullOrWhiteSpace(lblBeneficiaryPercentage.Text) ? lblBeneficiaryPercentage.Text : "";
                    dtRecords.Rows.Add(drRow);
                }
            }

            if (dtRecords.Rows.Count > 0)
            {
                if (dtRecords.Rows.Count == 1)
                {
                    SessionVariables.setSessionDataTable(dtRecords);
                    string script = @"
    var win = window;
    while (win !== win.parent) { win = win.parent; }
    if (win.GlobalPopup) {
        win.GlobalPopup.show({
            title: 'Edit Personal Contact',
            url: '/ESS/HR/HRPersonalContacts_Update.aspx'
        });
    }";
                    ScriptManager.RegisterStartupScript(
                        this, this.GetType(), "openModal", script, true);
                }
                else
                {
                    SysOperationResult_BOL result = new SysOperationResult_BOL();
                    result.isSuccess = false;
                    result.AlertType = AlertType.Error.ToString();
                    result.Message = "Please Select Only a Single Record to Edit";
                    NotificationMessage.showMessage(result);
                }
            }
            else
            {
                SysOperationResult_BOL result = new SysOperationResult_BOL();
                result.isSuccess = false;
                result.AlertType = AlertType.Error.ToString();
                result.Message = "Please Select a Record to Edit";
                NotificationMessage.showMessage(result);
            }
        }
    }
}