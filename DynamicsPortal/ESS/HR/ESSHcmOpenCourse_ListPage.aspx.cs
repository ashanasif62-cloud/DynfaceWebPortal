using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHcmOpenCourse_ListPage : MainForm
    {
        private HcmOpenCourse hcmOpenCourse = new HcmOpenCourse();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSHcmOpenCourseHistory";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    string employeeId = getQuery_EmployeeId();
                    reBindGrid(employeeId);
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

        private string getQuery_EmployeeId()
        {
            string employeeId = string.Empty;
            if (string.IsNullOrEmpty(Request.QueryString["EmpId"]))
            {
                employeeId = SessionVariables.getCurrentEmployeeId();
            }
            else
            {
                employeeId = SecureQueryString.decrypt(Request.QueryString["EmpId"]);
            }
            return employeeId;
        }

        protected void reBindGrid(string _employeeId)
        {
            string employeeId = _employeeId;

            getGridDataTable(employeeId);
            bindGrid();
        }
        private void getGridDataTable(string _employeeId)
        {
            string employeeId = _employeeId;
            DataTable dt = hcmOpenCourse.retrieveOpenCoursesForEmployee(employeeId);
            SessionVariables.setSessionDataTable(dt);
        }
        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();
        }


        protected void btnRegister_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();

            string employeeId = getQuery_EmployeeId();

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                        //Array.Resize(ref recordsId, recordsId.Length + 1);
                        //recordsId[recordsId.Length - 1] = recId;
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = hcmOpenCourse.registerCourses(employeeId, recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView.EditIndex = -1;
                    reBindGrid(employeeId);
                }
                else
                {
                    bindGrid();
                }
            }
        }


    }
}