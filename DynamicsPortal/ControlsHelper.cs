using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HcmPersonImageSvcReference;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public class ControlsHelper
    {
        public static DataTable getProject()

        {

            DataTable dtProject = null;

            ExpenseLines lines = new ExpenseLines();

            dtProject = lines.getProjectLookup();

            return dtProject;

        }

        public static DataTable getProjectActivity()

        {

            DataTable dtProjectActivity = null;

            ExpenseLines lines = new ExpenseLines();

            dtProjectActivity = lines.getProjectActivityLookup();

            return dtProjectActivity;

        }

        public static DataTable getProjectLineProperty()

        {

            DataTable dtProjectLineProperty = null;

            ExpenseLines lines = new ExpenseLines();

            dtProjectLineProperty = lines.getProjectLinePropertyLookup();

            return dtProjectLineProperty;

        }

        public static DataTable getAllExpenseCategory()

        {

            DataTable dtExpenseCategory = null;

            ExpenseLines lines = new ExpenseLines();

            dtExpenseCategory = lines.getExpenseCategoryLookup();

            return dtExpenseCategory;

        }

        public static DataTable getMerchant(string _costType)

        {

            DataTable dtExpenseCategory = null;

            ExpenseLines lines = new ExpenseLines();

            dtExpenseCategory = lines.getMerchantLookup(_costType);

            return dtExpenseCategory;

        }

        public static DataTable getPurpose()

        {

            DataTable dtPurpose = null;

            ExpenseReport report = new ExpenseReport();

            dtPurpose = report.getPurposeLookup();

            return dtPurpose;

        }

        public static DataTable getLocation()

        {

            DataTable dtPurpose = null;

            ExpenseReport report = new ExpenseReport();

            dtPurpose = report.getLocationLookup();

            return dtPurpose;

        }


        public static DataTable retrieveAllPRLeaveCodes()
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            string cacheKey = $"Lookup_PRLeaveCodes_{dataAreaId}";
            return GlobalCacheService.GetOrAdd(cacheKey, () =>
            {
                PRLeaves pRLeaves = new PRLeaves();
                return pRLeaves.retrieveAllPRLeaveCodes();
            }, TimeSpan.FromHours(4));
        }

        public static DataTable retriveEmployeeRequestioner()
        {
            DataTable dtEmployeeRequestioner = null;
            dtEmployeeRequestioner = SessionVariables.getSessionDataTable_EmployeeRequestioner();
            if (dtEmployeeRequestioner == null)
            {
                PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
                dtEmployeeRequestioner = pREmploymentInformation.retriveEmployeeRequestioner();
                SessionVariables.setSessionDataTable_EmployeeRequestioner(dtEmployeeRequestioner);
            }
            return dtEmployeeRequestioner;
        }

        public static DataTable retriveEmployeeReportees()
        {
            DataTable dt = null;
            PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
            dt = pREmploymentInformation.retriveEmployeeReportees();
            SessionVariables.setSessionDataTable_EmployeeReportees(dt);

            return dt;
        }

        public static DataTable retrieveAllEmployee()
        {
            DataTable dtEmployee = null;
            dtEmployee = SessionVariables.getSessionDataTable_AllEmployee();
            if (dtEmployee == null)
            {
                PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
                dtEmployee = pREmploymentInformation.retrieveAllEmployee();
                SessionVariables.setSessionDataTable_AllEmployee(dtEmployee);
            }
            return dtEmployee;
        }

        public static DataTable retrieveAllCurrencyDetails()
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            string cacheKey = $"Lookup_CurrencyDetails_{dataAreaId}";
            return GlobalCacheService.GetOrAdd(cacheKey, () =>
            {
                CurrencyDetails currencyDetails = new CurrencyDetails();
                return currencyDetails.retrieveAllCurrency();
            }, TimeSpan.FromHours(4));
        }

        public static DataTable retrieveAllPayGroupPayPeriods()
        {
            DataTable dtActivePayPeriod = null;
            dtActivePayPeriod = SessionVariables.getSessionDataTable_ActivePayPeriod();
            if (dtActivePayPeriod == null)
            {
                PRPayGroupPayPeriod pRPayGroupPayPeriod = new PRPayGroupPayPeriod();
                dtActivePayPeriod = pRPayGroupPayPeriod.retrieveAll();
                SessionVariables.setSessionDataTable_ActivePayPeriod(dtActivePayPeriod);
            }
            return dtActivePayPeriod;
        }

        public static DataTable retrieveAllPRAdvanceTypes()
        {
            DataTable dtAdvanceTypes = null;
            dtAdvanceTypes = SessionVariables.getSessionDataTable_PRAdvanceTypes();
            string employeeId = SessionVariables.getCurrentEmployeeId();
            if (dtAdvanceTypes == null)
            {
                PRAdvanceTypes pRAdvanceTypes = new PRAdvanceTypes();
                dtAdvanceTypes = pRAdvanceTypes.retrieveAllPRAdvanceTypes(employeeId);
                SessionVariables.setSessionDataTable_PRAdvanceTypes(dtAdvanceTypes);
            }
            return dtAdvanceTypes;
        }

        public static DataTable retrieveByAdvanceType(string _advanceType)
        {
            DataTable dtAdvanceTypes = null;
            dtAdvanceTypes = SessionVariables.getSessionDataTable_PRAdvanceTypes();
            if (dtAdvanceTypes == null)
            {
                PRAdvanceTypes pRAdvanceTypes = new PRAdvanceTypes();
                dtAdvanceTypes = pRAdvanceTypes.retrieveByAdvanceType(_advanceType);
                //dtAdvanceTypes = pRAdvanceTypes.retrieveAllPRAdvanceTypes();
                SessionVariables.setSessionDataTable_PRAdvanceTypes(dtAdvanceTypes);
            }
            return dtAdvanceTypes;
        }


        public static DataTable retrieveAllDimLocation()
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            string cacheKey = $"Lookup_DimLocation_{dataAreaId}";
            return GlobalCacheService.GetOrAdd(cacheKey, () =>
            {
                ESSRetrieveDimLocation eSSRetrieveDimLocation = new ESSRetrieveDimLocation();
                return eSSRetrieveDimLocation.retrieveAllDimLocation();
            }, TimeSpan.FromHours(4));
        }

        public static DataTable retrieveAllPRLoanTypes()
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            string cacheKey = $"Lookup_PRLoanTypes_{dataAreaId}";
            return GlobalCacheService.GetOrAdd(cacheKey, () =>
            {
                PRLoanTypes pRLoanTypes = new PRLoanTypes();
                return pRLoanTypes.retrieveAllPRLoanTypes();
            }, TimeSpan.FromHours(4));
        }

        public static DataTable retrieveAllPREOSNoticePeriods()
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            string cacheKey = $"Lookup_PREOSNoticePeriods_{dataAreaId}";
            return GlobalCacheService.GetOrAdd(cacheKey, () =>
            {
                PREOSNoticePeriods pREOSNoticePeriods = new PREOSNoticePeriods();
                return pREOSNoticePeriods.retrieveAllPREOSNoticePeriods();
            }, TimeSpan.FromHours(4));
        }

        public static DataTable retrieveAllHcmReasonCode()
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            string cacheKey = $"Lookup_HcmReasonCodes_{dataAreaId}";
            return GlobalCacheService.GetOrAdd(cacheKey, () =>
            {
                HcmReasonCode hcmReasonCodes = new HcmReasonCode();
                return hcmReasonCodes.retrieveAllHcmReasonCode();
            }, TimeSpan.FromHours(4));
        }

        public static DataTable retrieveAllESSRequestedFor()
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            string cacheKey = $"Lookup_ESSRequestedFor_{dataAreaId}";
            return GlobalCacheService.GetOrAdd(cacheKey, () =>
            {
                ESSRequestedFor eSSRequestedFor = new ESSRequestedFor();
                return eSSRequestedFor.retrieveAll();
            }, TimeSpan.FromHours(4));
        }

        public static long getWorkerId(string _employeeId)
        {
            string employeeId = _employeeId;
            long workerRecId = 0;

            if (!string.IsNullOrEmpty(employeeId))
            {
                DataTable dataTable = retriveEmployeeReportees();
                DataRow dataRow = dataTable.Select("EmployeeId = '" + employeeId + "'").FirstOrDefault();
                if (dataRow != null)
                {
                    Int64.TryParse(dataRow["WorkerRecId"].ToString(), out workerRecId);
                }
            }
            return workerRecId;
        }

        public static HcmWorkerDetailsSvcContract getWorkerDetails(string _employeeId)
        {
            string employeeId = _employeeId;    //SessionVariables.getCurrentEmployeeId();
            if (string.IsNullOrEmpty(employeeId))
                return null;

            HcmWorkerDetails hcmWorkerDetails = new HcmWorkerDetails();
            HcmWorkerDetailsSvcContract workerDetailsContract = hcmWorkerDetails.retrieveWorkerDetails(employeeId);

            return workerDetailsContract;
        }


        public static DataTable retrieveAllHcmIssuingAgency()
        {
            HcmIssuingAgency hcmIssuingAgency = new HcmIssuingAgency();
            DataTable dataTable = hcmIssuingAgency.retrieveAllHcmIssuingAgency();
            return dataTable;
        }

        public static DataTable retrieveAllHcmIdentificationType()
        {
            HcmIdentificationType hcmIdentificationType = new HcmIdentificationType();
            DataTable dataTable = hcmIdentificationType.retrieveAll();
            return dataTable;
        }

        public static DataTable retrieveAllHcmJob()
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            string cacheKey = $"Lookup_HcmJob_{dataAreaId}";
            return GlobalCacheService.GetOrAdd(cacheKey, () =>
            {
                HcmJob hcmJob = new HcmJob();
                return hcmJob.retrieveAllJobs();
            }, TimeSpan.FromHours(4));
        }

        public static DataTable retrieveAllHcmEducationDiscipline()
        {
            string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
            string cacheKey = $"Lookup_HcmEducationDiscipline_{dataAreaId}";
            return GlobalCacheService.GetOrAdd(cacheKey, () =>
            {
                HcmEducationDiscipline hcmEducationDiscipline = new HcmEducationDiscipline();
                return hcmEducationDiscipline.retrieveAllEducationDiscipline();
            }, TimeSpan.FromHours(4));
        }

        public static DataTable retrieveCurrentEmployeeNewsUpdates()
        {
            HRNewsUpdates hRNewsUpdates = new HRNewsUpdates();
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DateTime dateTime = DateTime.Now;

            DataTable dt = hRNewsUpdates.retrieveHRNewsUpdates(dateTime, employeeId);
            return dt;
        }





        public static string getCurrentUserImage(bool _refresh = false)
        {
            string userImageData = SessionVariables.getCurrentUserImage();
            if (string.IsNullOrEmpty(userImageData) || _refresh)
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                if (!string.IsNullOrEmpty(employeeId))
                {
                    HcmPersonImage hcmPersonImage = new HcmPersonImage();
                    HcmPersonImageSvcContract hcmPersonImageSvcContract = hcmPersonImage.retrieveByEmployee(employeeId);

                    userImageData = hcmPersonImageSvcContract.StringImage;

                    SessionVariables.setCurrentUserImage(userImageData);
                }
            }
            return userImageData;
        }


        public static string getEmployeeActivePayPeriod(string _employeeId)
        {
            string employeeId = _employeeId;
            string value = string.Empty;

            if (!string.IsNullOrEmpty(employeeId))
            {
                DataTable dataTable = retriveEmployeeReportees();
                DataRow dataRow = dataTable.Select("EmployeeId = '" + employeeId + "'").FirstOrDefault();
                if (dataRow != null)
                {
                    value = dataRow["ActivePayPeriodCode"].ToString();
                }
            }
            return value;
        }

        public static string getEmployeeServiceDuration(string _employeeId)
        {
            string employeeId = _employeeId;
            string value = string.Empty;

            if (!string.IsNullOrEmpty(employeeId))
            {
                DataTable dataTable = retriveEmployeeReportees();
                DataRow dataRow = dataTable.Select("EmployeeId = '" + employeeId + "'").FirstOrDefault();
                if (dataRow != null)
                {
                    value = dataRow["ServiceDuration"].ToString();
                }
            }
            return value;
        }

        public static DateTime getEmployeeJoiningDate(string _employeeId)
        {
            string employeeId = _employeeId;
            DateTime value = DateTime.MinValue;

            if (!string.IsNullOrEmpty(employeeId))
            {
                HcmWorkerDetailsSvcContract getWorkerDetails = ControlsHelper.getWorkerDetails(employeeId);
                value = getWorkerDetails.JoiningDate;
                //DataTable dataTable = retriveEmployeeReportees();
                //DataRow dataRow = dataTable.Select("EmployeeId = '" + employeeId + "'").FirstOrDefault();
                //if (dataRow != null)
                //{
                //}
            }
            return value;
        }

        public static string getEmployeeCurrencyCode(string _employeeId)
        {
            string employeeId = _employeeId;
            string value = string.Empty;

            if (!string.IsNullOrEmpty(employeeId))
            {
                DataTable dataTable = retriveEmployeeReportees();
                DataRow dataRow = dataTable.Select("EmployeeId = '" + employeeId + "'").FirstOrDefault();
                if (dataRow != null)
                {
                    value = dataRow["Currency"].ToString();
                }
            }
            if (string.IsNullOrEmpty(value))
                value = "PKR";
            return value;
        }


        public static decimal getEmployeeLeaveBalance(string _employeeId, string _leaveCode,string _leaveStartDate, string _leaveReqId = " ")
        {
            string employeeId = _employeeId;
            string leaveCode = _leaveCode;
            string leaveStartDate = _leaveStartDate;
            decimal leaveBalance = 0;
            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveCode))
            {
                PREmployeeLeaveRequest pREmployeeLeaves = new PREmployeeLeaveRequest();
                leaveBalance = pREmployeeLeaves.retrieveEmployeeLeaveBalance(employeeId, leaveCode, leaveStartDate, _leaveReqId);
            }

            return leaveBalance;
        }

        public static decimal getEmployeeLeaveDays(long _employeeId, string _leaveCode, DateTime _leaveStartDate, DateTime _leaveEndDate)
        {
            long employeeId = _employeeId;
            string leaveCode = _leaveCode;
            decimal leaveDays = 0;

            
            PREmployeeLeaveRequest pREmployeeLeaves = new PREmployeeLeaveRequest();
            leaveDays = pREmployeeLeaves.retrieveEmployeeLeaveDays(employeeId, _leaveCode, _leaveStartDate, _leaveEndDate);
            

            return leaveDays;
        }

        public static decimal getEmployeeQuota(string _employeeId, string _leaveCode, DateTime _leaveStartDate, long recId = 0)
        {
            string employeeId = _employeeId;
            string leaveCode = _leaveCode;
            decimal Quota = 0;

            
            PREmployeeLeaveRequest pREmployeeLeaves = new PREmployeeLeaveRequest();
            Quota = pREmployeeLeaves.retrieveEmployeeQuota(employeeId, _leaveCode, _leaveStartDate, recId);
            

            return Quota;
        }

        public static decimal getEmployeePFBalance(string _employeeId, string _advanceTypeCode)
        {
            string employeeId = _employeeId;
            string advanceTypeCode = _advanceTypeCode;
            decimal EmployeePFBalance = 0;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceTypeCode))
            {
                //Umair change here
                //PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
                //EmployeePFBalance = pREmploymentInformation.retrieveEmployeeLeaveBalance(employeeId, advanceTypeCode);
            }

            return EmployeePFBalance;
        }

        public static decimal getEmployerPFBalance(string _employeeId, string _advanceTypeCode)
        {
            string employeeId = _employeeId;
            string advanceTypeCode = _advanceTypeCode;
            decimal EmployerPFBalance = 0;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(advanceTypeCode))
            {
                //Umair change here
                //PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
                //EmployerPFBalance = pREmploymentInformation.retrieveEmployeeLeaveBalance(employeeId, advanceTypeCode);
            }

            return EmployerPFBalance;
        }




        public void checkWFStatus(GridViewRow _gridViewRow, string _controlName, string[] _validStatus)
        {
            try
            {
                GridViewRow gridViewRow = _gridViewRow;
                string[] validStatus = _validStatus;//;
                string controlName = _controlName;
                bool isValid = false;
                //if (gridViewRow.RowType != DataControlRowType.DataRow)
                //    return;

                string wfStatus = (gridViewRow.FindControl(controlName) as Label).Text.Replace(" ", "").ToLower();

                foreach (string status in validStatus)
                    if (wfStatus == status.ToLower())
                    {
                        isValid = true;
                        break;
                    }

                if (!isValid)
                {
                    gridViewRow.Enabled = false;
                    gridViewRow.Attributes.Add("NonActionable", "true");
                }

                #region NOT GOOD
                //ControlCollection controls = null;
                //int gridControls = -1;
                //int gridSubControls = -1;
                //gridViewRow.Attributes["NonActionable"]
                //gridControls = gridViewRow.Controls.Count - 1;
                //if (gridControls > 0)
                //{
                //    gridSubControls = gridViewRow.Controls[gridControls].Controls.Count;
                //    foreach (DataControlFieldCell control in gridViewRow.Controls)
                //    {
                //        var HeaderText = control.ContainingField.HeaderText;
                //        if (HeaderText == columnName)
                //        {
                //            if (control.Controls.Count > 0)
                //            {
                //                string value = (control.Controls[1] as Label).Text;
                //                value = value.Replace(" ", "");
                //                if (value != validStatus)
                //                {
                //                    //GridViewRow parent = control.Parent as System.Web.UI.WebControls.GridViewRow;
                //                    gridViewRow.Enabled = false;
                //                    gridViewRow.Attributes.Add("NonActionable", "true");
                //                }

                //            }
                //((System.Web.UI.WebControls.DataControlFieldCell)(gridViewRow.Controls[7])).ContainingField.HeaderText
                //string lid = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblLoginId")).Text;
                //} 
                #endregion
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


        public static List<T> FindControls<T>(Control parent) where T : Control
        {
            List<T> foundControls = new List<T>();
            FindControls<T>(parent, foundControls);
            return foundControls;
        }

        private static void FindControls<T>(Control parent, List<T> foundControls) where T : Control
        {
            foreach (Control c in parent.Controls)
            {
                if (c is T)
                    foundControls.Add((T)c);
                else if (c.Controls.Count > 0)
                    FindControls<T>(c, foundControls);
            }
        }



        public void setPageTitle(string pageMenuId, bool showPageTitle)
        {
            string pageTitle = string.Empty;

            if (showPageTitle)
            {
                DataTable dtUserMenuItems = SessionVariables.getUserMenuItems();
                if (dtUserMenuItems.Columns.Contains("MenuId"))
                {
                    DataRow drMenu = dtUserMenuItems.Select("MenuId = '" + pageMenuId + "'").FirstOrDefault();
                    if (drMenu != null)
                    {
                        pageTitle = drMenu["Label"].ToString();
                    }
                }
            }

            if ((HttpContext.Current.CurrentHandler as Page).Master != null)
            {
                HtmlGenericControl lblPageTitle = (HttpContext.Current.CurrentHandler as Page).Master.FindControl("pageTitle") as HtmlGenericControl;
                lblPageTitle.InnerHtml = pageTitle;
                if (!showPageTitle)
                    lblPageTitle.Visible = false;
            }
        }


        public string getApplicationDirectoryPath()
        {

            string requiredPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)));
            requiredPath = requiredPath.Replace("file:\\", "");

            string directoryPath = requiredPath + "\\DynamicsPortal\\";

            return directoryPath;
        }


    }
}