using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSFinancialDimensionsSvcReference;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using PortalIntegration.PREmploymentInformationSvcReference;
using System;

namespace DynamicsPortal
{
    public partial class HcmEmployeeTransferRequests_Create : ModalForm
    {
        public string employeeId { get; set; }

        public string reqType { get; set; }

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "HcmEmployeeTransferRequests";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (string.IsNullOrEmpty(Request.QueryString["EmpId"]) || string.IsNullOrEmpty(Request.QueryString["ReqType"]))
                {
                    employeeId = SessionVariables.getCurrentEmployeeId();
                    reqType = "Transfer";
                }
                else
                {
                    employeeId = SecureQueryString.decrypt(Request.QueryString["EmpId"]);
                    reqType = SecureQueryString.decrypt(Request.QueryString["ReqType"]);
                }
                if (!IsPostBack)
                {
                    cddl_AllActiveFinancialDims.EmployeeId = employeeId;
                    bindControlsData();
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

        private void bindControlsData()
        {
            //string employeeId = _employeeId;string _employeeId, string _reqType
            //string reqType = _reqType;

            txtEmployeeId.Text = employeeId;
            HcmWorkerDetails hcmWorkerDetails = new HcmWorkerDetails();
            HcmWorkerDetailsSvcContract hcmWorkerDetailsSvcContract = hcmWorkerDetails.retrieveWorkerDetails(employeeId);

            txtDateOfJoining.Text = hcmWorkerDetailsSvcContract.JoiningDate.ToString();
            txtEmployeeId.Text = hcmWorkerDetailsSvcContract.EmployeeId;
            txtEmployeeName.Text = hcmWorkerDetailsSvcContract.EmployeeName;
            txtJobId.Text = hcmWorkerDetailsSvcContract.Job;
            txtPositionId.Text = hcmWorkerDetailsSvcContract.PositionName;

            PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
            PREmploymentInformationSvcContract pREmploymentInformationSvcContract = pREmploymentInformation.retrieveEmployeeDetails(employeeId);
            txtEmploymentType.Text = pREmploymentInformationSvcContract.EmploymentType.ToString();
            txtBasicSalary.Text = pREmploymentInformationSvcContract.BasicSalary.ToString();
            //txtProbationDate.Text = pREmploymentInformationSvcContract.;
            //txtEffectiveDate.Text = pREmploymentInformationSvcContract.;
            long empDimension = pREmploymentInformationSvcContract.Dimension;

            fillEmployeeDimensions(empDimension);

            fillDimensionsList();

        }

        private void fillEmployeeDimensions(long _empDimension)
        {
            long empDimension = _empDimension;

            ESSFinancialDimensions financialDimensions = new ESSFinancialDimensions();
            DataContract[] dimensionValues = financialDimensions.retrieveDimensionValues(empDimension);
            foreach (DataContract dataContract in dimensionValues)
            {
                string dimName = dataContract.Code;
                string dimValue = dataContract.Value1;
                string dimDescription = dataContract.Value2;

                switch (dimName)
                {
                    case "Location":
                        {
                            txtDimLocation.Text = string.IsNullOrEmpty(dimDescription) ? dimValue : dimDescription;
                            break;
                        }
                    case "BusinessUnit":
                        {
                            txtDimBusinessUnit.Text = dimDescription;
                            break;
                        }
                    case "CostCenter":
                        {
                            txtDimCostCenter.Text = dimDescription;
                            break;
                        }
                    case "Department":
                        {
                            txtDimDepartment.Text = dimDescription;
                            break;
                        }
                    case "NatureofExpense":
                        {
                            txtDimNatureofExpense.Text = dimDescription;
                            break;
                        }
                    case "Purpose":
                        {
                            txtDimPurpose.Text = dimDescription;
                            break;
                        }
                    case "Worker":
                        {
                            txtDimWorker.Text = dimDescription;
                            break;
                        }
                }



            }
        }

        private void fillDimensionsList()
        {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            long empDim = cddl_AllActiveFinancialDims.EmployeeDimension;
        }
    }
}