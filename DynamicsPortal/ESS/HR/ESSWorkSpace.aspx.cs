using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSWorkSpace : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            showActionPanel = false;
            pageMenuId = "ESSWorkSpace";

            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;

            if (!IsPostBack)
            {
                setUserDetails();
            }
        }

        private void setUserDetails()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            if (string.IsNullOrEmpty(employeeId)) return;

            string employeeName = SessionVariables.getCurrentEmployeeName();
            if (string.IsNullOrEmpty(employeeName)) employeeName = "Unknown";

            string userLoginTime = SessionVariables.getCurrentUserLoginTime().ToString("dd/MM/yyyy HH:mm:ss");
            if (string.IsNullOrEmpty(userLoginTime)) userLoginTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            lblUserFullName.InnerText = employeeName;
            lblUserLoginTime.InnerText = userLoginTime;

            string userImageData = ControlsHelper.getCurrentUserImage();
            if (string.IsNullOrEmpty(userImageData))
                imgUser.Src = "/distribution/img/User.png";
            else
                imgUser.Src = "data:image/png;base64," + userImageData;

            HcmWorkerDetailsSvcContract currentUserDetails = ControlsHelper.getWorkerDetails(employeeId);
            if (currentUserDetails == null)
            {
                lblUserDepartment.InnerText = "N/A";
                lblUserJob.InnerText = "N/A";
                lblYearsOfService.InnerText = "N/A";
                lblReportsTo.InnerText = "N/A";
               lblPositionType.InnerText = "N/A";
               //lblUserFullAddress.InnerText = "N/A";
                return;
            }

            string userDepartment = currentUserDetails.DepartmentName ?? "N/A";
            string userJob = currentUserDetails.Job ?? "N/A";
            string userEmailId = currentUserDetails.workerEmail ?? "N/A";
            string userPhoneNo = currentUserDetails.Phone ?? "N/A";
            string userFullAddress = currentUserDetails.Address ?? "N/A";

            lblUserDepartment.InnerText = userDepartment;
            lblUserJob.InnerText = userJob;
           // lblUserEmailId.InnerText = userEmailId;
            //lblUserPhoneNo.InnerText = userPhoneNo;
            //lblUserFullAddress.InnerText = userFullAddress;
        }
    }
}