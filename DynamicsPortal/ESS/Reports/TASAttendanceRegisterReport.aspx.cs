using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.IO;

namespace DynamicsPortal
{
    public partial class TASAttendanceRegisterReport : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "TASAttendanceRegisterReport";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindData();

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

        //private void bindData()
        //{
        //    try
        //    {
        //        TASEmployeeRoster employeeRoster = new TASEmployeeRoster();
        //        string currentEmployeeId = SessionVariables.getCurrentEmployeeId();
        //        DataTable dt = employeeRoster.retrieveAllEmployees();

        //        // Add display column for dropdown
        //        dt.Columns.Add("DisplayText", typeof(string));
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            row["DisplayText"] = $"{row["EmployeeId"]} - {row["EmployeeName"]}";
        //        }

        //        ddlEmployeeId.DataSource = dt;
        //        ddlEmployeeId.DataValueField = "EmployeeId";
        //        ddlEmployeeId.DataTextField = "DisplayText";
        //        ddlEmployeeId.DataBind();

        //        // Pre-select current employee
        //        if (!string.IsNullOrEmpty(currentEmployeeId) && ddlEmployeeId.Items.FindByValue(currentEmployeeId) != null)
        //        {
        //            ddlEmployeeId.SelectedValue = currentEmployeeId;
        //            ddlEmployeeId.Enabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //    }
        //}
        private void bindData()
        {
            try
            {
                string currentEmployeeId = SessionVariables.getCurrentEmployeeId();
                string currentEmployeeName = SessionVariables.getCurrentEmployeeName();
                if (string.IsNullOrEmpty(currentEmployeeId))
                {
                    NotificationMessage.showMessage(AlertType.Error, "Current employee not found.");
                    return;
                }

                // Create a DataTable with only the current employee
                DataTable dt = new DataTable();
                dt.Columns.Add("EmployeeId", typeof(string));
                dt.Columns.Add("EmployeeName", typeof(string));
                dt.Columns.Add("DisplayText", typeof(string));


                // Retrieve name of current employee
                //TASEmployeeRoster employeeRoster = new TASEmployeeRoster();
                //DataRow employeeRow = employeeRoster.retrieveEmployeeRowById(currentEmployeeId); // you need to write this

                //if (employeeRow != null)
                //{
                //    DataRow newRow = dt.NewRow();
                //    newRow["EmployeeId"] = currentEmployeeId;
                //    newRow["EmployeeName"] = employeeRow["EmployeeName"];
                //    newRow["DisplayText"] = $"{currentEmployeeId} - {employeeRow["EmployeeName"]}";
                //    dt.Rows.Add(newRow);
                //}

                DataRow newRow = dt.NewRow();
                newRow["EmployeeId"] = currentEmployeeId;
                newRow["EmployeeName"] = currentEmployeeName;
                newRow["DisplayText"] = $"{currentEmployeeId} - {currentEmployeeName}";
                dt.Rows.Add(newRow);

                ddlEmployeeId.DataSource = dt;
                ddlEmployeeId.DataValueField = "EmployeeId";
                ddlEmployeeId.DataTextField = "DisplayText";
                ddlEmployeeId.DataBind();

                ddlEmployeeId.SelectedValue = currentEmployeeId;
                ddlEmployeeId.Enabled = false; // optional: disable dropdown
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
        }




        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                string employeeId = ddlEmployeeId.SelectedValue;
                //  string employeeId = SessionVariables.getCurrentEmployeeId();

                if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    RetrieveReports retrieveReportsData = new RetrieveReports();
                    DateTime fromDateTime = fromDate.toDateTime();
                    DateTime toDateTime = toDate.toDateTime();

                    MemoryStream memoryStream = retrieveReportsData.viewAttendnaceRegisterReport(employeeId, fromDateTime, toDateTime);
                    string inputAsString = Convert.ToBase64String(memoryStream.ToArray());

                    string src = "data:application/pdf;base64, " + inputAsString;
                    embed01.Src = src;
                }
                else
                {
                    NotificationMessage.showMessage(AlertType.Warning, "Please select From Date and To Date.");
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
        //mod haris 
        protected void btnEmailReport_Click(object sender, EventArgs e)
        {
            try
            {
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                string employeeId = ddlEmployeeId.SelectedValue;

                if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    RetrieveReports retrieveReportsData = new RetrieveReports();
                    DateTime fromDateTime = Convert.ToDateTime(fromDate);
                    DateTime toDateTime = Convert.ToDateTime(toDate);

                    // Get the response message from the emailTaxCertificateReport method
                    string responseMessage = retrieveReportsData.emailAttendanceRegisterReport(employeeId, fromDateTime, toDateTime);

                    // Show the message based on the response from the API call
                    NotificationMessage.showMessage(AlertType.Information, responseMessage);
                }
                else
                {
                    NotificationMessage.showMessage(AlertType.Warning, "Please select From Date and To Date.");
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally { }
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            var txtEmployeeId = ddlEmployeeId.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox;
            if (txtEmployeeId != null)
            {
                string personalNumber = txtEmployeeId.Text;

                if (!string.IsNullOrEmpty(personalNumber) && ddlEmployeeId.Items.FindByValue(personalNumber) != null)
                {
                    ddlEmployeeId.SelectedValue = personalNumber;
                    ddlEmployeeId.Enabled = false;
                }
            }
        }



    }
}