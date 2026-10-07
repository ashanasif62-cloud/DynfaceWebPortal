using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.TA
{
    public partial class TASEmployeeAdjustments_ListPage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Employee Adjustments";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Employee Adjustments"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }
                // Load header panel and lines grid if session exists
                LoadHeaderPanel();
                LoadLinesGrid();
            }
        }

        private void LoadHeaderPanel()
        {
            TASEmployeeAdjustmentLines svc = new TASEmployeeAdjustmentLines();
            try
            {
                // Get session values
                string employeeId = Session["SelectedEmployeeId"]?.ToString();
                string fromDate = Session["SelectedFromDate"]?.ToString();
                string toDate = Session["SelectedToDate"]?.ToString();
                string attendanceDateStr = Session["SelectedAttendanceDate"]?.ToString() ?? "";


                if (!string.IsNullOrEmpty(employeeId) &&
                    !string.IsNullOrEmpty(fromDate) &&
                    !string.IsNullOrEmpty(toDate))
                {
                    // Retrieve employee adjustment data from service
                    //DataTable dt = svc.retrieveEmployeeAdjustment(attendanceDateStr);
                    DataTable dt = null;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        // Assuming first row has header info
                        DataRow row = dt.Rows[0];

                        txtEmployee.Text = Session["SelectedEmployeeName"]?.ToString() ?? "";
                        txtEmployeeId.Text = Session["SelectedEmployeeId"]?.ToString();
                       // txtDate.Text = Session["SelectedAttendanceDate"]?.ToString() ?? "";
                        txtShiftId.Text = Session["SelectedShiftId"]?.ToString();
                        txtGenerationType.Text = Session["SelectedGenerationType"]?.ToString() ?? "";
                        txtAttendanceStatus.Text = Session["SelectedAttendanceStatus"]?.ToString() ?? "";
                        txtWorkflowStatus.Text = Session["SelectedWFStatus"]?.ToString() ?? "";
                    }
                    // Parse and format the attendance date safely
                    if (DateTime.TryParse(attendanceDateStr, out DateTime dtAttendance))
                    {
                        txtDate.Text = dtAttendance.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
                    }
                   



                    else
                    {
                        ClearHeaderPanel();
                    }
                }
                else
                {
                    ClearHeaderPanel();
                }
            }
            catch (Exception ex)
            {
                // Log or show error
                Console.WriteLine("Error loading header panel: " + ex.Message);
                ClearHeaderPanel();
            }
        }

        private void LoadLinesGrid()
        {
            TASEmployeeAdjustmentLines svc = new TASEmployeeAdjustmentLines();
            try
            {
                string fromDate = Session["SelectedFromDate"]?.ToString();
                string toDate = Session["SelectedToDate"]?.ToString();
                string attendanceDate = Session["SelectedAttendanceDate"]?.ToString() ?? "";

                //DataTable dt = svc.retrieveEmployeeAdjustment(attendanceDate);
                DataTable dt = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    // Format ClockInDate
                    if (dt.Columns.Contains("ClockInDate"))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (DateTime.TryParse(row["ClockInDate"]?.ToString(), out DateTime dtIn))
                            {
                                row["ClockInDate"] = dtIn.ToString("M/d/yyyy");
                            }
                            else
                            {
                                row["ClockInDate"] = "";
                            }
                        }
                    }

                    // Format ClockOutDate
                    if (dt.Columns.Contains("ClockOutDate"))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (DateTime.TryParse(row["ClockOutDate"]?.ToString(), out DateTime dtOut))
                            {
                                row["ClockOutDate"] = dtOut.ToString("M/d/yyyy");
                            }
                            else
                            {
                                row["ClockOutDate"] = "";
                            }
                        }
                    }
                }

                gvLines.DataSource = dt;
                gvLines.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error binding grid: " + ex.Message);
                gvLines.DataSource = null;
                gvLines.DataBind();
            }
        }

        private void ClearHeaderPanel()
        {
            txtEmployee.Text = "";
            txtEmployeeId.Text = "";
            txtDate.Text = "";
            txtShiftId.Text = "";
            txtGenerationType.Text = "";
            txtAttendanceStatus.Text = "";
            txtWorkflowStatus.Text = "";
        }

        // Optional: format date to string
        protected string FormatDate(object dateValue)
        {
            if (dateValue == null || dateValue == DBNull.Value)
                return string.Empty;

            if (DateTime.TryParse(dateValue.ToString(), out DateTime dt))
                return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            return dateValue.ToString();
        }
        protected string FormatSecondsToTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            // Case 1: value is seconds (INT)
            if (int.TryParse(value.ToString(), out int totalSeconds))
            {
                DateTime time = DateTime.Today.AddSeconds(totalSeconds);
                return time.ToString("h:mm tt", CultureInfo.InvariantCulture);
            }

            // Case 2: value is DateTime
            if (value is DateTime dt)
                return dt.ToString("h:mm tt", CultureInfo.InvariantCulture);

            // Case 3: string time "09:00:00"
            if (DateTime.TryParse(value.ToString(), out DateTime parsed))
                return parsed.ToString("h:mm tt", CultureInfo.InvariantCulture);

            return value.ToString();
        }


    }
}