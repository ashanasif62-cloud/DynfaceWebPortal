using GeneralAuxiliary;
using PortalIntegration;
using System;

namespace DynamicsPortal
{
    public partial class SysUserInfo_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "SysUserInfoRequest";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string personalNumber = txtEmployeeId.Text;    //(cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            string userId = txtUserId.Text;
            bool isActive = cbUserEnabled.Checked;
            bool result = false;
            string password = string.Empty;
            string message = string.Empty;

            SysUserInfo sysUserInfo = new SysUserInfo();
            result = sysUserInfo.createUser(personalNumber, userId, isActive);
            password = sysUserInfo.password;
            message = sysUserInfo.message;

            if (result)
            {
                lblShowPassword.Text = "Please Save Password: " + password;
                lblShowPassword.Visible = true;
                btnSave.Visible = false;
                btnCancel.Text = "Close";
            }
            else
            {
                NotificationMessage.showMessage(AlertType.Error, message);
            }


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }



    }
}