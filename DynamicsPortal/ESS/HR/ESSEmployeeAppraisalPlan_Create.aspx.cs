using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.APEmployeeAppraisalPlanSvcReference;
using System.Data;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSEmployeeAppraisalPlan_Create : ModalForm
    { 
        private APEmployeeAppraisalPlan APEmployeeAppraisalPlan = new APEmployeeAppraisalPlan();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSAPEmployeeAppraisalsPlan";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                //if (!IsPostBack)
                //{
                //    bindControlsData();
                //}
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
            long planRecId = 0;
            long requestRecId = 0;
            bool submitRequest = _submitRequest;
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();
            DataTable dtEmployeeAppraisalPlan = SessionVariables.getSessionDataTable();
            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;

            string filter = personalNumber + "-" + DateTime.Now.Year.ToString();
            //cddlEmployeeDetails.EnableViewState = false;
            //cddlEmployeeDetails.Attributes.Add("disabled", "disabled"); /* Modification*/
            string re= string.Empty;
            foreach (DataRow dataRow  in dtEmployeeAppraisalPlan.Rows)
            {
                if (dataRow[0].ToString() == filter)
                     re = dataRow["RecId"].ToString();

                break;
            }


           // IEnumerable<DataRow> resultt = dtEmployeeAppraisalPlan.AsEnumerable().GroupBy(r => r.Field<string>("RecId")).Select(g => g.First())
                ;
            
            //if (dataRow != null)
            //{
                
            //    Int64.TryParse(dataRow["recId"].ToString(), out planRecId);
            //}
            //Comment
                #region CreateRequest
            DataTable dataTable = APEmployeeAppraisalPlan.createDataTable();
            DataRow dr = dataTable.NewRow();

            long employeeId = ControlsHelper.getWorkerId(personalNumber);

            dr["EmployeeId"] = personalNumber;
            dr["KPICode"] = KPICode.Text;
            dr["Description"] = Description.Text;
            dr["KpiWeightage"] = KPIWeightage.Text;
            dr["RefRecid"] = re;

            dataTable.Rows.Add(dr);
            #endregion

            createResult = APEmployeeAppraisalPlan.create(dataTable);
            requestRecId = createResult.RecId;

            if (createResult.isSuccess)
            {
                if (requestRecId > 0)
                {
                        //submitResult.Message = " KPI Created successfully.";
                    submitResult.isSuccess = true;
                    submitResult.AlertType = AlertType.Success.ToString();
                    submitResult.RecId = requestRecId;


                }
                else
                {
                    submitResult.AlertType = AlertType.Error.ToString();
                    submitResult.isSuccess = false;
                    submitResult.Message = " Failed to create KPI.";
                }
                createResult.Message += " " + submitResult.Message;
                createResult.AlertType = submitResult.AlertType;
                createResult.isSuccess = submitResult.isSuccess;
                
            }

            bool result = operationResults(createResult, true);
        }

 
      

     
    }
}
