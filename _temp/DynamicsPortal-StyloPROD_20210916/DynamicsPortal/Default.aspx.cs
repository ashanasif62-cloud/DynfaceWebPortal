using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using System;
using System.Data;
using System.Web.UI.HtmlControls;

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
        }

        private void setUserDetails()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            string employeeName = SessionVariables.getCurrentEmployeeName();
            string userLoginTime = String.Format("{0:dd/MM/yyyy HH:mm:ss}", SessionVariables.getCurrentUserLoginTime());

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
            string userJob = currentUserDetails.Job;
            string userEmailId = currentUserDetails.workerEmail;
            string userPhoneNo = currentUserDetails.Phone;
            string userFullAddress = currentUserDetails.Address;

            lblUserDepartment.InnerText = userDepartment;
            lblUserJob.InnerText = userJob;
            lblUserEmailId.InnerText = userEmailId;
            lblUserPhoneNo.InnerText = userPhoneNo;
            lblUserFullAddress.InnerText = userFullAddress;
        }

        private void createNewsUpdates()
        {
            DataTable dt = ControlsHelper.retrieveCurrentEmployeeNewsUpdates();

            foreach (DataRow dr in dt.Rows)
            {
                HtmlGenericControl li01 = new HtmlGenericControl("li");
                li01.Attributes.Add("class", "d-flex justify-content-between");

                #region NewsBody
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
                #endregion

                #region RightCol
                HtmlGenericControl divRightCol = new HtmlGenericControl("div");
                divRightCol.Attributes.Add("class", "right-col text-right");

                HtmlGenericControl divDate = new HtmlGenericControl("div");
                divDate.Attributes.Add("class", "update-date");
                divDate.InnerText = Convert.ToDateTime(dr["Start"]).Day.ToString();//dr["Start"].ToString().toDateTime().Day.ToString();

                HtmlGenericControl month = new HtmlGenericControl("span");
                month.Attributes.Add("class", "month");
                month.InnerText = Convert.ToDateTime(dr["Start"]).ToString("MMM");//dr["Start"].ToString().toDateTime().ToString("MMM");
                divDate.Controls.Add(month);
                divRightCol.Controls.Add(divDate);
                #endregion

                li01.Controls.Add(div01);
                li01.Controls.Add(divRightCol);

                divNewsUpdates.Controls.Add(li01);
            }
            //HRNewsUpdatesSvcContract hRNewsUpdatesSvcContract = new HRNewsUpdatesSvcContract();
            //hRNewsUpdatesSvcContract.Detail;
            //hRNewsUpdatesSvcContract.Expiry;
            //hRNewsUpdatesSvcContract.Start;
            //hRNewsUpdatesSvcContract.RecId;
            //hRNewsUpdatesSvcContract.Subject;

            //<!-- Item-->
            //<li class="d-flex justify-content-between">
            //    <div class="left-col d-flex">
            //        <div class="icon"><i class="icon-rss-feed"></i></div>
            //        <div class="title">
            //            <strong>Open courses for training.</strong>
            //            <p>
            //                Business Standards is now available for training.<br />
            //                Sales Techniques is now available for training.<br />
            //                Standards of Business Conduct is now available for training.
            //            </p>
            //        </div>
            //    </div>
            //    <div class="right-col text-right">
            //        <div class="update-date">21<span class="month">Jan</span></div>
            //    </div>
            //</li>
        }


        private void createRecentActivities()
        {
            SysUserAuditTrials_BLL sysUserAuditTrials = new SysUserAuditTrials_BLL();
            SysUserAuditTrial_BOL objBOL = new SysUserAuditTrial_BOL();
            try
            {
                objBOL.UserId = SessionVariables.getCurrentUserId();
                objBOL.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
                objBOL.Partition = SessionVariables.getCurrentUserPartition();

                DataTable dt = sysUserAuditTrials.SysUserRecentActivities_Retrieve(objBOL);

                foreach (DataRow dr in dt.Rows)
                {
                    DateTime dateTime = Convert.ToDateTime(dr["time"].ToString());
                    string time = dateTime.ToShortTimeString();
                    string date = dateTime.ToShortDateString();
                    string id = dr["message"].ToString().Replace(" is created.", "");
                    int days = Convert.ToInt32((DateTime.Today - dateTime).TotalDays) + 1;
                    //Int32.TryParse(((DateTime.Today - dateTime).TotalDays), out days);

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
                    /*<li><div class="row">
                            <div class="col-4 date-holder text-right">
                                <div class="icon"><i class="icon-clock"></i></div>
                                <div class="date"><span>03:00 pm</span><span class="text-info">5 Days ago</span></div>
                            </div>
                            <div class="col-8 content">
                                <strong>Leave Request</strong>
                                <p>Request Id: LEV-000022<br/>Request Date: 24 Jan 2019<br/>Volumn: 2 Day(s)</p>
                            </div>
                        </div></li>*/
                    HtmlGenericControl div001 = new HtmlGenericControl("div");
                    div001.Attributes.Add("class", "col-8 content");
                    HtmlGenericControl strong01 = new HtmlGenericControl("strong");
                    strong01.InnerText = dr["title"].ToString();
                    HtmlGenericControl p = new HtmlGenericControl("p");
                    p.InnerHtml = "Request Id: " + id + "" + "<br />" + "Request Date: " + date + "";//<br/>Volumn: 2 Day(s)" + Environment.NewLine + "
                    div001.Controls.Add(strong01);
                    div001.Controls.Add(p);

                    div.Controls.Add(div01);
                    div.Controls.Add(div001);
                    li01.Controls.Add(div);
                    #endregion
                    divRecentActivities.Controls.Add(li01);
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




    }
}