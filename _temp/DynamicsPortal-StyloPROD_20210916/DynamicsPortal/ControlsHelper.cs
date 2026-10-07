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
        public static DataTable retrieveAllPRLeaveCodes()
        {
            DataTable dtLeaveCodes = null;
            //dtLeaveCodes = SessionVariables.getSessionDataTable_PRLeaveCodes();
            //if (dtLeaveCodes == null)
            //{
            PRLeaves pRLeaves = new PRLeaves();
            dtLeaveCodes = pRLeaves.retrieveAllPRLeaveCodes();
            //    SessionVariables.setSessionDataTable_PRLeaveCodes(dtLeaveCodes);
            //}
            return dtLeaveCodes;
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
            dt = SessionVariables.getSessionDataTable_EmployeeReportees();
            if (dt == null)
            {
                PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
                dt = pREmploymentInformation.retriveEmployeeReportees();
                SessionVariables.setSessionDataTable_EmployeeReportees(dt);
            }
            return dt;
        }

        public static DataTable retrieveAllEmployee()
        {
            DataTable dtEmployee = null;
            //dtEmployee = SessionVariables.getSessionDataTable_AllEmployee();
            //if (dtEmployee == null)
            //{
            PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
            dtEmployee = pREmploymentInformation.retrieveAllEmployee();
            //    SessionVariables.setSessionDataTable_AllEmployee(dtEmployee);
            //}
            return dtEmployee;
        }

        public static DataTable retrieveAllCurrencyDetails()
        {
            DataTable dtCurrencyDetails = null;
            dtCurrencyDetails = SessionVariables.getSessionDataTable_CurrencyDetails();
            if (dtCurrencyDetails == null)
            {
                CurrencyDetails currencyDetails = new CurrencyDetails();
                dtCurrencyDetails = currencyDetails.retrieveAllCurrency();
                SessionVariables.setSessionDataTable_CurrencyDetails(dtCurrencyDetails);
            }
            return dtCurrencyDetails;
        }

        public static DataTable retrieveAllPayGroupPayPeriods()
        {
            DataTable dtActivePayPeriod = null;
            //dtActivePayPeriod = SessionVariables.getSessionDataTable_ActivePayPeriod();
            //if (dtActivePayPeriod == null)
            //{
            PRPayGroupPayPeriod pRPayGroupPayPeriod = new PRPayGroupPayPeriod();
            dtActivePayPeriod = pRPayGroupPayPeriod.retrieveAll();
            //    SessionVariables.setSessionDataTable_ActivePayPeriod(dtActivePayPeriod);
            //}
            return dtActivePayPeriod;
        }

        public static DataTable retrieveAllPRAdvanceTypes()
        {
            DataTable dtAdvanceTypes = null;
            //dtAdvanceTypes = SessionVariables.getSessionDataTable_PRAdvanceTypes();
            //if (dtAdvanceTypes == null)
            //{
            PRAdvanceTypes pRAdvanceTypes = new PRAdvanceTypes();
            dtAdvanceTypes = pRAdvanceTypes.retrieveAllPRAdvanceTypes();
            //    SessionVariables.setSessionDataTable_PRAdvanceTypes(dtAdvanceTypes);
            //}
            return dtAdvanceTypes;
        }

        public static DataTable retrieveByAdvanceType(int _advanceType)
        {
            DataTable dtAdvanceTypes = null;
            //dtAdvanceTypes = SessionVariables.getSessionDataTable_PRAdvanceTypes();
            //if (dtAdvanceTypes == null)
            //{
            PRAdvanceTypes pRAdvanceTypes = new PRAdvanceTypes();
            dtAdvanceTypes = pRAdvanceTypes.retrieveByAdvanceType(_advanceType);
            //    SessionVariables.setSessionDataTable_PRAdvanceTypes(dtAdvanceTypes);
            //}
            return dtAdvanceTypes;
        }


        public static DataTable retrieveAllDimLocation()
        {
            DataTable dtDimLocation = null;
            //dtDimLocation = SessionVariables.getSessionDataTable_DimLocation();
            //if (dtDimLocation == null)
            //{
            ESSRetrieveDimLocation eSSRetrieveDimLocation = new ESSRetrieveDimLocation();
            dtDimLocation = eSSRetrieveDimLocation.retrieveAllDimLocation();
            //    SessionVariables.setSessionDataTable_DimLocation(dtDimLocation);
            //}
            return dtDimLocation;
        }

        public static DataTable retrieveAllPRLoanTypes()
        {
            DataTable dtLoanTypes = null;
            //dtLoanTypes = SessionVariables.getSessionDataTable_PRLoanTypes();
            //if (dtLoanTypes == null)
            //{
            PRLoanTypes pRLoanTypes = new PRLoanTypes();
            dtLoanTypes = pRLoanTypes.retrieveAllPRLoanTypes();
            //    SessionVariables.setSessionDataTable_PRLoanTypes(dtLoanTypes);
            //}
            return dtLoanTypes;
        }

        public static DataTable retrieveAllPREOSNoticePeriods()
        {
            DataTable dtPREOSNoticePeriods = null;
            //dtPREOSNoticePeriods = SessionVariables.getSessionDataTable_PREOSNoticePeriods();
            //if (dtPREOSNoticePeriods == null)
            //{
            PREOSNoticePeriods pREOSNoticePeriods = new PREOSNoticePeriods();
            dtPREOSNoticePeriods = pREOSNoticePeriods.retrieveAllPREOSNoticePeriods();
            //    SessionVariables.setSessionDataTable_PREOSNoticePeriods(dtPREOSNoticePeriods);
            //}
            return dtPREOSNoticePeriods;
        }

        public static DataTable retrieveAllHcmReasonCode()
        {
            DataTable dtHcmReasonCodes = null;
            //dtHcmReasonCodes = SessionVariables.getSessionDataTable_HcmReasonCodes();
            //if (dtHcmReasonCodes == null)
            //{
            HcmReasonCode hcmReasonCodes = new HcmReasonCode();
            dtHcmReasonCodes = hcmReasonCodes.retrieveAllHcmReasonCode();
            //    SessionVariables.setSessionDataTable_HcmReasonCodes(dtHcmReasonCodes);
            //}
            return dtHcmReasonCodes;
        }

        public static DataTable retrieveAllESSRequestedFor()
        {
            DataTable dtESSRequestedFor = null;
            //dtESSRequestedFor = SessionVariables.getSessionDataTable_ESSRequestedFor();
            //if (dtESSRequestedFor == null)
            //{
            ESSRequestedFor eSSRequestedFor = new ESSRequestedFor();
            dtESSRequestedFor = eSSRequestedFor.retrieveAll();
            //    SessionVariables.setSessionDataTable_ESSRequestedFor(dtESSRequestedFor);
            //}
            return dtESSRequestedFor;
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
            DataTable dt = null;
            //dt = SessionVariables.getSessionDataTable_HcmJob();
            //if (dt == null)
            //{
            HcmJob hcmJob = new HcmJob();
            dt = hcmJob.retrieveAllJobs();
            //    SessionVariables.setSessionDataTable_HcmJob(dt);
            //}
            return dt;
        }

        public static DataTable retrieveAllHcmEducationDiscipline()
        {
            DataTable dt = null;
            //dt = SessionVariables.getSessionDataTable_HcmEducationDiscipline();
            //if (dt == null)
            //{
            HcmEducationDiscipline hcmEducationDiscipline = new HcmEducationDiscipline();
            dt = hcmEducationDiscipline.retrieveAllEducationDiscipline();
            //    SessionVariables.setSessionDataTable_HcmEducationDiscipline(dt);
            //}
            return dt;
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

        public static string getEmployeeJoiningDate(string _employeeId)
        {
            string employeeId = _employeeId;
            string value = string.Empty;

            if (!string.IsNullOrEmpty(employeeId))
            {
                HcmWorkerDetailsSvcContract getWorkerDetails = ControlsHelper.getWorkerDetails(employeeId);
                value = getWorkerDetails.JoiningDate.ToString();
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


        public static decimal getEmployeeLeaveBalance(string _employeeId, string _leaveCode)
        {
            string employeeId = _employeeId;
            string leaveCode = _leaveCode;
            decimal leaveBalance = 0;

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveCode))
            {
                PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
                leaveBalance = pREmploymentInformation.retrieveEmployeeLeaveBalance(employeeId, leaveCode);
            }

            return leaveBalance;
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