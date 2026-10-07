using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Windows.Media.Animation;

namespace DynamicsPortal
{
    public partial class Default : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            //System.Diagnostics.Stopwatch sw1 = System.Diagnostics.Stopwatch.StartNew();
            //sw1.Stop();
            showActionPanel = false;
            pageMenuId = "ESSPRHRDefault";

            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;

            if (!IsPostBack)
            {
                setUserDetails();
            }
            createNewsUpdates();
            createRecentActivities();
            createMyRequestStatus();
        }

        private void setUserDetails()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            string employeeName = SessionVariables.getCurrentEmployeeName();
            //string userLoginTime = String.Format("{0:dd/MM/yyyy HH:mm:ss}", SessionVariables.getCurrentUserLoginTime());
            string userLoginTime = String.Format("{0:MM/dd/yyyy hh:mm:ss tt}", SessionVariables.getCurrentUserLoginTime());

            lblUserFullName.InnerText = employeeName;
            lblUserLoginTime.InnerText = userLoginTime;//"24/09/2018 16:38:48";

            string userImageData = ControlsHelper.getCurrentUserImage();

            if (string.IsNullOrEmpty(userImageData))
                imgUser.Src = "/distribution/img/User.png";
            else
                imgUser.Src = "data:image/png;base64," + userImageData;

            HcmWorkerDetailsSvcContract currentUserDetails = ControlsHelper.getWorkerDetails(employeeId);
            if (currentUserDetails == null)
                return;

            string userDepartment = currentUserDetails.DepartmentName;
          //  string userJob = currentUserDetails.job;
            string userJob = currentUserDetails.JobDescription;
            string userEmailId = currentUserDetails.workerEmail;
            string userPhoneNo = currentUserDetails.Phone;
            string userFullAddress = currentUserDetails.Address;
            decimal yearsofService = currentUserDetails.ServiceDuration;
            string reportsTo = currentUserDetails.SupervisorName;
            decimal efficency = currentUserDetails.Efficiency;

            lblUserDepartment.InnerText = userDepartment;
            lblUserJob.InnerText = userJob;
            lblUserEmailId.InnerText = userEmailId;
            lblUserPhoneNo.InnerText = userPhoneNo;
            lblUserFullAddress.InnerText = userFullAddress;
            txtYearsValue.InnerText = yearsofService.ToString("0.0");
            txtReportsValue.InnerText = reportsTo;
            txtEfficiencyValue.InnerText = efficency.ToString("0.0");

            yearsSection.Visible = yearsofService > 0;

            reportsSection.Visible = !string.IsNullOrWhiteSpace(reportsTo);

            efficiencySection.Visible = efficency > 0;

            yearsSeparator.Visible = yearsSection.Visible && reportsSection.Visible;

            efficiencySeparator.Visible = reportsSection.Visible && efficiencySection.Visible;

            employeeInfoRow.Visible = yearsSection.Visible || reportsSection.Visible || efficiencySection.Visible;

        }

        private void createNewsUpdates()
        {
            DataTable dt = ControlsHelper.retrieveCurrentEmployeeNewsUpdates();

            foreach (DataRow dr in dt.Rows)
            {
                // ── Date conversion ──────────────────────────────────────────
                DateTime newsDate = Convert.ToDateTime(dr["Start"]);
                newsDate = newsDate.Kind == DateTimeKind.Utc
                    ? newsDate.ToLocalTime()
                    : DateTime.SpecifyKind(newsDate, DateTimeKind.Utc).ToLocalTime();

                // ── <li> wrapper — same as original ──────────────────────────
                HtmlGenericControl li01 = new HtmlGenericControl("li");
                li01.Attributes.Add("class", "d-flex justify-content-between");

                // ── LEFT: same as original ────────────────────────────────────
                HtmlGenericControl div01 = new HtmlGenericControl("div");
                div01.Attributes.Add("class", "left-col d-flex");

                HtmlGenericControl div02 = new HtmlGenericControl("div");
                div02.Attributes.Add("class", "icon");
                HtmlGenericControl i01 = new HtmlGenericControl("i");
                i01.Attributes.Add("class", "icon-rss-feed");
                div02.Controls.Add(i01);

                HtmlGenericControl divTitle = new HtmlGenericControl("div");
                divTitle.Attributes.Add("class", "title");
                HtmlGenericControl title = new HtmlGenericControl("strong");
                title.InnerText = dr["Subject"].ToString();
                HtmlGenericControl details = new HtmlGenericControl("p");
                details.InnerText = dr["Detail"].ToString();
                divTitle.Controls.Add(title);
                divTitle.Controls.Add(details);

                div01.Controls.Add(div02);
                div01.Controls.Add(divTitle);

                // ── RIGHT: improved date only ─────────────────────────────────
                // ── RIGHT: completely redesigned date ────────────────────────
                HtmlGenericControl divRightCol = new HtmlGenericControl("div");
                divRightCol.Attributes.Add("class", "right-col text-right");

                HtmlGenericControl divDateBox = new HtmlGenericControl("div");
                divDateBox.InnerHtml = string.Format(
                    "<div style='display:inline-block; text-align:center; border-radius:5px; overflow:hidden; min-width:30px; box-shadow:0 1px 3px rgba(0,0,0,0.12);'>" +
                        "<div style='background:#1a73e8; color:#fff; font-size:9px; font-weight:700; letter-spacing:1px; padding:2px 5px;'>{0}</div>" +
                        "<div style='background:#fff; border:1px solid #e0e0e0; border-top:none; padding:3px 5px;'>" +
                            "<div style='font-size:12px; font-weight:800; color:#202124; line-height:1;'>{1}</div>" +
                            "<div style='font-size:8px; color:#888; margin-top:1px;'>{2}</div>" +
                        "</div>" +
                    "</div>",
                    newsDate.ToString("MMM").ToUpper(),
                    newsDate.Day.ToString("00"),
                    newsDate.Year.ToString()
                );

                divRightCol.Controls.Add(divDateBox);

                // ── Assemble ──────────────────────────────────────────────────
                li01.Controls.Add(div01);
                li01.Controls.Add(divRightCol);

                divNewsUpdates.Controls.Add(li01);
            }
        }


        private void createRecentActivities()
        {
            SysUserAuditTrials_BLL sysUserAuditTrials = new SysUserAuditTrials_BLL();
            SysUserAuditTrial_BOL objBOL = new SysUserAuditTrial_BOL();
            try
            {
                DataTable dt = null;
                if (ClientConfiguration.Default.connectWithSQL)
                {
                    objBOL.UserId = SessionVariables.getCurrentUserId();
                    objBOL.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
                    objBOL.Partition = SessionVariables.getCurrentUserPartition();

                    dt = sysUserAuditTrials.SysUserRecentActivities_Retrieve(objBOL);
                    foreach (DataRow dr in dt.Rows)
                    {
                        DateTime dateTime = Convert.ToDateTime(dr["time"].ToString());
                        // Convert UTC time to local system time
                        if (dateTime.Kind == DateTimeKind.Utc)
                        {
                            dateTime = dateTime.ToLocalTime();
                        }
                        else
                        {
                            // If kind is unspecified, assume it's UTC and convert
                            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToLocalTime();
                        }
                        
                        string time = dateTime.ToShortTimeString();
                        string date = dateTime.ToString("dd/MM/yyyy");
                        string id = dr["message"].ToString().Replace(" is created.", "");
                        int days = Convert.ToInt32((DateTime.Today - dateTime).TotalDays) + 1;

                        HtmlGenericControl li01 = new HtmlGenericControl("li");
                        #region Body
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Attributes.Add("class", "row");

                        HtmlGenericControl div01 = new HtmlGenericControl("div");
                        div01.Attributes.Add("class", "col-4 date-holder text-right");
                        HtmlGenericControl div02 = new HtmlGenericControl("div");
                        div02.Attributes.Add("class", "icon");
                        HtmlGenericControl i01 = new HtmlGenericControl("i");
                        i01.Attributes.Add("class", "icon-clock");
                        div02.Controls.Add(i01);
                        HtmlGenericControl div03 = new HtmlGenericControl("div");
                        div03.Attributes.Add("class", "date");
                        HtmlGenericControl span01 = new HtmlGenericControl("span");
                        span01.InnerText = time;
                        HtmlGenericControl span02 = new HtmlGenericControl("span");
                        span02.Attributes.Add("class", "text-info");
                        span02.InnerText = days + " Day(s) ago";
                        div03.Controls.Add(span01);
                        div03.Controls.Add(span02);
                        div01.Controls.Add(div02);
                        div01.Controls.Add(div03);
                        
                        HtmlGenericControl div001 = new HtmlGenericControl("div");
                        div001.Attributes.Add("class", "col-8 content");
                        HtmlGenericControl strong01 = new HtmlGenericControl("strong");
                        strong01.InnerText = dr["title"].ToString();
                        HtmlGenericControl p = new HtmlGenericControl("p");
                        p.InnerHtml = "Request Id: " + id + "" + "<br />" + "Request Date: " + date + "";
                        div001.Controls.Add(strong01);
                        div001.Controls.Add(p);

                        div.Controls.Add(div01);
                        div.Controls.Add(div001);
                        li01.Controls.Add(div);
                        #endregion
                        divRecentActivities.Controls.Add(li01);
                    }
                }
                else
                {
                    dt = new DFEmployeeRecentActivities().retrieveRecentActiities();
                    foreach (DataRow dr in dt.Rows)
                    {
                        DateTime dateTime = Convert.ToDateTime(dr["time"].ToString());
                        // Convert UTC time to local system time
                        if (dateTime.Kind == DateTimeKind.Utc)
                        {
                            dateTime = dateTime.ToLocalTime();
                        }
                        else
                        {
                            // If kind is unspecified, assume it's UTC and convert
                            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToLocalTime();
                        }
                        
                        string time = dateTime.ToShortTimeString();
                        string date = dateTime.ToShortDateString();
                        string id = dr["message"].ToString().Replace(" is created.", "");
                        int days = Convert.ToInt32((DateTime.Today - dateTime).TotalDays);
                        string DaysInnerText = days < 1 ? "Today" : days == 1 ? "Yesterday" : days + 1 + " Day(s) ago";
                        HtmlGenericControl li01 = new HtmlGenericControl("li");
                        #region Body
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Attributes.Add("class", "row");

                        HtmlGenericControl div01 = new HtmlGenericControl("div");
                        div01.Attributes.Add("class", "col-4 date-holder text-right");
                        HtmlGenericControl div02 = new HtmlGenericControl("div");
                        div02.Attributes.Add("class", "icon");
                        HtmlGenericControl i01 = new HtmlGenericControl("i");
                        i01.Attributes.Add("class", "icon-clock");
                        div02.Controls.Add(i01);
                        HtmlGenericControl div03 = new HtmlGenericControl("div");
                        div03.Attributes.Add("class", "date");
                        HtmlGenericControl span01 = new HtmlGenericControl("span");
                        span01.InnerText = time;
                        HtmlGenericControl span02 = new HtmlGenericControl("span");
                        span02.Attributes.Add("class", "text-info");
                        span02.InnerText = DaysInnerText;
                        div03.Controls.Add(span01);
                        div03.Controls.Add(span02);
                        div01.Controls.Add(div02);
                        div01.Controls.Add(div03);
                        
                        HtmlGenericControl div001 = new HtmlGenericControl("div");
                        div001.Attributes.Add("class", "col-8 content");
                        HtmlGenericControl strong01 = new HtmlGenericControl("strong");
                        strong01.InnerText = dr["ActionItem"].ToString();
                        HtmlGenericControl p = new HtmlGenericControl("p");
                        p.InnerHtml = "Request Id: " + id + "" + "<br />" + "Request Date: " + date + "";
                        div001.Controls.Add(strong01);
                        div001.Controls.Add(p);

                        div.Controls.Add(div01);
                        div.Controls.Add(div001);
                        li01.Controls.Add(div);
                        #endregion
                        divRecentActivities.Controls.Add(li01);
                    }
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

        private void createMyRequestStatus()
        {
            try
            {
                WorkflowTrackingStatusTable newstatus = new WorkflowTrackingStatusTable();
                DataTable dt = newstatus.retrieveRecentActivities(SessionVariables.getCurrentEmailId());

                if (dt == null || dt.Rows.Count == 0)
                    return;

                // Sort by CreatedDateTime DESC (latest first)
                var sortedRows = dt.AsEnumerable()
                    .OrderByDescending(row =>
                    {
                        string dateStr = row["createdDateTime"].ToString();
                        if (DateTime.TryParse(dateStr, out DateTime dtDate))
                        {
                            return dtDate.Kind == DateTimeKind.Utc
                                ? dtDate.ToLocalTime()
                                : DateTime.SpecifyKind(dtDate, DateTimeKind.Utc).ToLocalTime();
                        }
                        return DateTime.MinValue;
                    });

                foreach (DataRow dr in sortedRows)
                {
                    // Parse and convert CreatedDateTime
                    DateTime dateTime = Convert.ToDateTime(dr["createdDateTime"]);
                    if (dateTime.Kind == DateTimeKind.Utc)
                        dateTime = dateTime.ToLocalTime();
                    else
                        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToLocalTime();

                    string time = dateTime.ToString("hh:mm tt");
                    string formattedDate = dateTime.ToString("dd/MM/yyyy");
                    int days = Convert.ToInt32((DateTime.Today - dateTime.Date).TotalDays);
                    string DaysInnerText = days < 1 ? "Today" : days == 1 ? "Yesterday" : days + 1 + " Day(s) ago";
                    // Fields from contract
                    string document = dr["Document"].ToString();      // e.g. "EMP-000123"
                    string documentType = dr["DocumentType"].ToString();  // e.g. "Leave Request"
                    string subject = dr["Subject"].ToString();
                    string originator = dr["From"].ToString();
                    string wfItemType = dr["workflowWorkItemType"].ToString();
                    string correlationId = dr["CorrelationId"].ToString();

                    //These Commented lines are code of filtering requests which are not related to the current user, the user should only see requests that are related to him //

                   //  string originator = dr["From"] != DBNull.Value ? dr["From"].ToString().Trim() : string.Empty;
                   // string currentEmployeeName = SessionVariables.getCurrentEmployeeName();
                   // if (currentEmployeeName != null) currentEmployeeName = currentEmployeeName.Trim();
                   // else currentEmployeeName = "";

                   // string currentUserEmail = SessionVariables.getCurrentEmailId();
                   // if (currentUserEmail != null) currentUserEmail = currentUserEmail.Trim();
                   // else currentUserEmail = "";

                    //try {
                     //   string debugStr = "Row: ";
                    //    foreach (System.Data.DataColumn col in dr.Table.Columns) {
                      //      debugStr += col.ColumnName + "=" + dr[col].ToString() + " | ";
                      //  }
                      //  debugStr += "\n";
                     //   System.IO.File.AppendAllText(@"c:\DynaFace WebApp\debug.txt", debugStr);
                  //  } catch {}

                    // Strict filter: The request must contain the user's name in the document or subject
                   // bool isMyRequest = false;
                    
                   // if (!string.IsNullOrEmpty(currentEmployeeName))
                   // {
                    //    if (document.IndexOf(currentEmployeeName, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       //     subject.IndexOf(currentEmployeeName, StringComparison.OrdinalIgnoreCase) >= 0)
                      //  {
                       //     isMyRequest = true;
                    //    }
                    //}

                    //if (!isMyRequest)
                    //{
                    //    continue;
                    // }

                    HtmlGenericControl li01 = new HtmlGenericControl("li");

                    #region Body

                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Attributes.Add("class", "row");

                    /* LEFT SIDE — time + days ago */
                    HtmlGenericControl div01 = new HtmlGenericControl("div");
                    div01.Attributes.Add("class", "col-4 date-holder text-right");

                    HtmlGenericControl div02 = new HtmlGenericControl("div");
                    div02.Attributes.Add("class", "icon");
                    HtmlGenericControl i01 = new HtmlGenericControl("i");
                    i01.Attributes.Add("class", "icon-clock");
                    div02.Controls.Add(i01);

                    HtmlGenericControl div03 = new HtmlGenericControl("div");
                    div03.Attributes.Add("class", "date");

                    HtmlGenericControl span01 = new HtmlGenericControl("span");
                    span01.InnerText = time;

                    HtmlGenericControl span02 = new HtmlGenericControl("span");
                    span02.Attributes.Add("class", "text-info");
                    span02.InnerText = DaysInnerText;

                    div03.Controls.Add(span01);
                    div03.Controls.Add(span02);
                    div01.Controls.Add(div02);
                    div01.Controls.Add(div03);

                    /* RIGHT SIDE — document info */
                    HtmlGenericControl div001 = new HtmlGenericControl("div");
                    div001.Attributes.Add("class", "col-8 content");

                    HtmlGenericControl strong01 = new HtmlGenericControl("strong");
                    strong01.InnerText = documentType; // e.g. "Leave Request"

                    HtmlGenericControl p = new HtmlGenericControl("p");
                    p.InnerHtml =
                        "Request: " + document + "<br />" +
                        "Subject: " + subject + "<br />" +
                        "Type: " + wfItemType + "<br />" +
                        "Date: " + formattedDate;

                    div001.Controls.Add(strong01);
                    div001.Controls.Add(p);

                    div.Controls.Add(div01);
                    div.Controls.Add(div001);
                    li01.Controls.Add(div);

                    #endregion

                    missingAttendance.Controls.Add(li01);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                string method = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                objErrorLog.write(method, ex);
            }
        }

        [System.Web.Services.WebMethod]
        public static string GetRegisterDetails(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var obj = new TASAttendanceRegister();
                var objPunch = new TASEmployeeTimeLog();
                //var dt = obj.getRegisterDetailByDate(fromDate, toDate);
                var dt = obj.createDataTable();
                //var dtPunches = objPunch.getTimeLogHistoryByDate(fromDate, toDate);
                var dtPunches = objPunch.createDataTable();
                var events = new List<object>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!DateTime.TryParse(dr["AttendanceDate"]?.ToString(), out DateTime date))
                            continue;

                        string clockIn = dr["ClockIn"] != DBNull.Value
                            ? TimeSpan.FromSeconds(Convert.ToInt32(dr["ClockIn"])).ToString(@"hh\:mm")
                            : "Blank";

                        string clockOut = dr["ClockOut"] != DBNull.Value
                            ? TimeSpan.FromSeconds(Convert.ToInt32(dr["ClockOut"])).ToString(@"hh\:mm")
                            : "Blank";

                        string bgColor = dr["Color"]?.ToString() ?? "";

                        var punchRows = new StringBuilder();

                        if (dtPunches != null && dtPunches.Rows.Count > 0)
                        {
                            var punchesForDay = dtPunches.AsEnumerable()
                                .Where(p => DateTime.TryParse(p["PunchDate"]?.ToString(), out DateTime punchDate)
                                            && punchDate.Date == date.Date)
                                .OrderBy(p => DateTime.Parse(p["PunchDate"].ToString()));

                            foreach (var punch in punchesForDay)
                            {
                                DateTime punchDate = DateTime.Parse(punch["PunchDate"].ToString());
                                string punchTime = punchDate.ToString("HH:mm");
                                string location = punch["Location"]?.ToString() ?? "N/A";

                                punchRows.Append($@"<tr><td>{punchTime}</td><td>{location}</td></tr>");
                            }

                            if (!punchesForDay.Any())
                            {
                                punchRows.Append("<tr><td colspan='2'>No punches found</td></tr>");
                            }
                        }
                        else
                        {
                            punchRows.Append("<tr><td colspan='2'>No punches available</td></tr>");
                        }

                        // --- Build HTML Tooltip ---
                        string titleHtml = $@"
                            <div style='font-family:Arial; font-size:12px; width:250px;'>
                                <div style='display:flex; justify-content:space-between; align-items:center;'>
                                    <div style='font-size:26px; font-weight:bold; color:#555;'>{date.Day}</div>
                                    <div style='background:{bgColor}; color:black; padding:2px 6px; border-radius:4px; margin-top:4px; font-size:12px; font-weight:bold;'>Late</div>
                                </div>
                              <div style='margin-top:6px;'>
                                <b>Deduction:</b> 213.15<br/>
                                <b>Allowance Ded:</b> 118.91<br/>
                              </div>
                                <hr/>
                                <div>
                                     <b>Punch Location:</b><br/>
                                    <div class='punch-table-container'>
                                      <table style='width:100%; font-size:11px; border-collapse:collapse;'>
                                        <tr><th align='left'>Punch Time</th><th align='left'>Location</th></tr>
                                        {punchRows}
                                      </table>
                                    </div>
                                  </div>
                                </div>";

                        events.Add(new
                        {
                            Date = date,
                            Title = titleHtml,
                            Link = "#",
                            BgColor = bgColor,
                            Color = "#000"
                        });
                    }
                }

                return Newtonsoft.Json.JsonConvert.SerializeObject(events);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write("GetRegisterDetails", ex);

                return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>());
            }
        }

    }
}