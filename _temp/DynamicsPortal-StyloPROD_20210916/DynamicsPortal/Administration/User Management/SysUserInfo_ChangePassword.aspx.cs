using AuthenticationHelper;
using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Drawing;

namespace DynamicsPortal
{
    public partial class SysUserInfo_ChangePassword : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "SysUserInfoUpdatePassword";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    string userId = SessionVariables.getCurrentUserId();
                    if (string.IsNullOrEmpty(userId))
                    {
                        return;
                    }
                    txtUserId.Text = userId;
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
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmNewPassword = txtConfirmNewPassword.Text;
            string userId = SessionVariables.getCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                NotificationMessage.showMessage(AlertType.Error, "Invalid User Id.");
                return;
            }
            if (string.IsNullOrEmpty(oldPassword))
            {
                NotificationMessage.showMessage(AlertType.Error, "Please enter valid Old Password.");
                txtOldPassword.BorderColor = Color.Red;
                return;
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                NotificationMessage.showMessage(AlertType.Error, "Please enter valid New Password.");
                txtNewPassword.BorderColor = Color.Red;
                return;
            }
            if (string.IsNullOrEmpty(confirmNewPassword))
            {
                NotificationMessage.showMessage(AlertType.Error, "Please enter valid Confirm New Password.");
                txtConfirmNewPassword.BorderColor = Color.Red;
                return;
            }
            if (!newPassword.Equals(confirmNewPassword))
            {
                NotificationMessage.showMessage(AlertType.Error, "New Password & Confirm New Password doesn't match.");
                txtConfirmNewPassword.BorderColor = Color.Red;
                return;
            }

            string encryptedOldPassword = SecurePassword.securePassword(userId, oldPassword);
            string encryptedNewPassword = SecurePassword.securePassword(userId, newPassword);

            SysUserInfo_BOL objBOL = new SysUserInfo_BOL();
            objBOL.UserId = userId;
            objBOL.Password = encryptedOldPassword;
            objBOL.NewPassword = encryptedNewPassword;
            objBOL.ModifiedBy = userId;
            objBOL.Partition = SessionVariables.getCurrentUserPartition();

            SysUserInfo_BLL objBLL = new SysUserInfo_BLL();
            string operationResults = objBLL.sysUserInfo_UpdatePassword(objBOL);
            bool results = NotificationMessage.showMessage(operationResults);

            if (results)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        
    }
}