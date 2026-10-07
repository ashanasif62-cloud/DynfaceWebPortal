using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;

namespace DynamicsPortal
{
    public partial class ESSHRRejoining_Create : ModalForm
    {
        private HRRejoining eSSRejoining = new HRRejoining();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSHRRejoiningRequest";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    DataTable reasonCode = ControlsHelper.retrieveAllHcmReasonCode();
                    ddlReasonCode.DataSource = reasonCode;
                    ddlReasonCode.DataTextField = "Description";
                    ddlReasonCode.DataValueField = "ReasonCodeId";
                    ddlReasonCode.DataBind();

                    txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtRequestDate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            createRequest();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Submit_Click(object sender, EventArgs e)
        {
            create_SubmitRequest();
        }

        private void createRequest()
        {
            create_SubmitRequest(false);
        }

        private void create_SubmitRequest(bool _submitRequest = true)
        {
            long requestRecId = 0;
            bool submitRequest = _submitRequest;
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

            #region CreateRequest
            DataTable dataTable = eSSRejoining.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            long employeeId = ControlsHelper.getWorkerId(personalNumber);

            dr["EmpId"] = employeeId;
            dr["RequestDate"] = txtRequestDate.Text;
            //dr["Delay"] = txtDelay.Text;
            dr["Grace"] = txtGrace.Text;
            dr["JoiningDate"] = txtJoiningDate.Text;
            dr["ReasonCode"] = ddlReasonCode.SelectedValue;
            dr["Remarks"] = txtRemarks.Text;

            dataTable.Rows.Add(dr);

            #endregion

            createResult = eSSRejoining.create(dataTable);
            requestRecId = createResult.RecId;

            if (createResult.isSuccess && submitRequest)
            {
                if (requestRecId > 0)
                {
                    submitResult = submitWFRequest(requestRecId);
                    if (submitResult.isSuccess)
                    {
                        submitResult.Message = " Request successfully submitted.";
                    }
                    else
                    {
                        submitResult.Message = " Failed to submit the request.";
                    }
                }
                else
                {
                    submitResult.AlertType = AlertType.Error.ToString();
                    submitResult.isSuccess = false;
                    submitResult.Message = " Failed to submit the created request.";
                }
                createResult.Message += " " + submitResult.Message;
                createResult.AlertType = submitResult.AlertType;
                createResult.isSuccess = submitResult.isSuccess;
            }

            bool result = operationResults(createResult, true);
        }

        private SysOperationResult_BOL submitWFRequest(long _requestRecId)
        {
            long requestRecId = _requestRecId;
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            SysOperationResult_BOL operationResult_BOL = new SysOperationResult_BOL();

            if (requestRecId > 0)
            {
                operationResult_BOL = eSSWorkflow.hRRejoiningRequestByRecId_Submit(requestRecId);
            }
            return operationResult_BOL;
        }

    }
}
