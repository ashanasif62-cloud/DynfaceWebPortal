using BussinessObject;
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
    public partial class TASEmployeeRoster_Edit : ModalForm
    {
        private TASEmployeeRoster tASEmployeeRoster = new TASEmployeeRoster();

        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Edit Roster";
                // titleDiv.Style["font-weight"] = "bold";
            }

            if (!IsPostBack)
            {
                LoadRecord();
            }

        }

        private void LoadRecord()
        {
            long recId = 0;
            if (long.TryParse(Request.QueryString["RecId"], out recId))
            {
                // Fetch the record from the database using recId
                DataTable record = tASEmployeeRoster.findByRecordId(recId);

                if (record != null && record.Rows.Count > 0)
                {
                    DataRow row = record.Rows[0]; // Assuming the first row contains the data
                    lblEmployeeID.Text = row["EmployeeID"].ToString();
                    lblEmployeeName.Text = row["EmployeeName"].ToString();
                    lblShiftID.Text = row["ShiftID"].ToString();
                    lblShiftCode.Text = row["ShiftCode"].ToString();
                    //lblDate.Text = FormatTime(row["ShiftDate"]?.ToString());
                    lblDate.Text = FormatDate(row["ShiftDate"]);
                    lblType.Text = lblType.Text = FormatType(row["Type"]);
                    lblFlexClockInStartTime.Text = FormatTime(row["FlexClockInStartTime"]);
                    lblTimeIn.Text = FormatTime(row["ShiftStartTime"].ToString());
                    lblFlexClockInEndTime.Text = FormatTime(row["FlexClockInEndTime"].ToString());
                    lblFlexClockOutStartTime.Text = FormatTime(row["FlexClockOutStartTime"]);
                    lblTimeOut.Text = FormatTime(row["ShiftEndTime"]?.ToString());
                    lblFlexClockOutEndTime.Text = FormatTime(row["FlexClockOutEndTime"]?.ToString());
                    lblShiftHours.Text = FormatTimeNew(row["ShiftHours"].ToString());
                    lblRequiredWorkingHours.Text = FormatTimeNew(row["RequiredWorkingHours"].ToString());
                    lblMinWorkingHour.Text = FormatTimeNew(row["MinimumWorkingHour"].ToString());
                    ddlOffDay.SelectedValue = row["OffDay"].ToString();
                    lblGenerationType.Text = row["GenerationType"].ToString();
                    lblGazettedDay.Text = row["GazettedDay"].ToString();
                }
                else
                {
                    lblEmployeeID.Text = "Record not found.";
                }
            }
            else
            {
                lblEmployeeID.Text = "Invalid record ID.";
            }
        }


        //protected string FormatTime(object timeValue)
        //{
        //    if (timeValue == null || timeValue == DBNull.Value)
        //        return String.Empty;

        //    DateTime dt;

        //    // 1) Already a DateTime?
        //    if (timeValue is DateTime t1)
        //        dt = t1;
        //    else
        //    {
        //        var s = timeValue.ToString().Trim();
        //        // 2) Seconds-since-midnight?
        //        if (int.TryParse(s, out int totalSeconds))
        //            dt = DateTime.Today.AddSeconds(totalSeconds);
        //        else
        //        {
        //            // 3) Known string formats?
        //            string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm" };
        //            if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
        //            {
        //                // parsed
        //            }
        //            else if (!DateTime.TryParse(s, out dt))
        //            {
        //                // 4) Give up
        //                return s;
        //            }
        //        }
        //    }

        //    // Final: include seconds & lowercase am/pm
        //    return dt.ToString("hh:mm:sstt", CultureInfo.InvariantCulture)
        //             .ToUpperInvariant();
        //}
        protected string FormatTime(object timeValue)
        {
            if (timeValue == null || timeValue == DBNull.Value)
                return String.Empty;

            DateTime dt;

            // 1) Already a DateTime?
            if (timeValue is DateTime t1)
                dt = t1;
            else
            {
                var s = timeValue.ToString().Trim();
                // 2) Seconds-since-midnight?
                if (int.TryParse(s, out int totalSeconds))
                    dt = DateTime.Today.AddSeconds(totalSeconds);
                else
                {
                    // 3) Known string formats?
                    string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm" };
                    if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        // parsed
                    }
                    else if (!DateTime.TryParse(s, out dt))
                    {
                        // 4) Give up
                        return s;
                    }
                }
            }

            // ✅ Final: include seconds & AM/PM with a space before
            return dt.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
        }


        protected string FormatTimeNew(object timeValue)
        {
            if (timeValue == null || timeValue == DBNull.Value)
                return String.Empty;

            DateTime dt;

            // 1) Already a DateTime?
            if (timeValue is DateTime t1)
                dt = t1;
            else
            {
                var s = timeValue.ToString().Trim();
                // 2) Seconds-since-midnight?
                if (int.TryParse(s, out int totalSeconds))
                    dt = DateTime.Today.AddSeconds(totalSeconds);
                else
                {
                    // 3) Known string formats?
                    string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm" };
                    if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        // parsed
                    }
                    else if (!DateTime.TryParse(s, out dt))
                    {
                        // 4) Give up
                        return s;
                    }
                }
            }

            // Final: 24-hour format with hours and minutes only
            return dt.ToString("HH:mm", CultureInfo.InvariantCulture);
        }

        protected string FormatTimeForInput(object timeValue)
        {
            if (timeValue == null || timeValue == DBNull.Value)
                return string.Empty;

            DateTime dt;

            // 1) Already a DateTime?
            if (timeValue is DateTime t1)
                dt = t1;
            else
            {
                var s = timeValue.ToString().Trim();
                // 2) Seconds-since-midnight?
                if (int.TryParse(s, out int totalSeconds))
                    dt = DateTime.Today.AddSeconds(totalSeconds);
                else
                {
                    // 3) Known string formats?
                    string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm" };
                    if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        // parsed
                    }
                    else if (!DateTime.TryParse(s, out dt))
                    {
                        // 4) Give up
                        return string.Empty;
                    }
                }
            }

            // Final output: strict 24-hour format with seconds
            return dt.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
        }
        protected string FormatDate(object dateValue)
        {
            if (dateValue == null || dateValue == DBNull.Value)
                return string.Empty;

            if (DateTime.TryParse(dateValue.ToString(), out DateTime dt))
                return dt.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            return dateValue.ToString();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            long recId = 0;
            if (long.TryParse(Request.QueryString["RecId"], out recId))
            {
                string employeeId = lblEmployeeID.Text;
                string employeeName = lblEmployeeName.Text;
                string flexClockInStartTime = lblFlexClockInStartTime.Text;
                string flexClockOutStartTime = lblFlexClockOutStartTime.Text;
                string offDay = ddlOffDay.SelectedValue;
                string shiftId = lblShiftID.Text;

                SysOperationResult_BOL result = tASEmployeeRoster.UpdateRecord(recId, employeeId, employeeName, flexClockInStartTime, flexClockOutStartTime, offDay, shiftId);

                if (result.isSuccess)
                {
                    // Update successful
                    // Close the dialog
                    //ScriptManager.RegisterStartupScript(this, GetType(), "CloseDialog", "closeDialog(true);", true);
                    ScriptManager.RegisterStartupScript(
                          this,
                      GetType(),
                     "UpdateSuccess",
                  "alert('Roster Record updated successfully.'); closeDialog(true);",
                      true
                 );
                }
                else
                {
                    // Handle update failure
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", $"alert('Error: {result?.Message ?? "Unknown error occurred."}');", true);
                }
            }
            else
            {
                lblEmployeeID.Text = "Invalid record ID.";
            }
        }
        protected string FormatType(object typeValue)
        {
            if (typeValue == null || typeValue == DBNull.Value)
                return string.Empty;

            string typeStr = typeValue.ToString();
            switch (typeStr)
            {
                case "StandardTime":
                    return "Standard Time";
              
                default:
                    // If the DB already has spaces, just return as-is
                    return typeStr;
            }
        }

    }
}
