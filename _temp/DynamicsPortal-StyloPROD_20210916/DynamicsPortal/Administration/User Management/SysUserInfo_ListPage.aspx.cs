using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class SysUserInfo_ListPage : MainForm
    {
        private SysUserInfo_BLL sysUserInfo_BLL = new SysUserInfo_BLL();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "SysUserInfoHistory";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindGrid();
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

        protected void bindGrid()
        {
            DataTable dt = sysUserInfo_BLL.retrieveAllSysUserInfo();
            gridView.DataSource = dt;
            gridView.DataBind();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                long recId = 0;
                Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                bool status = (gridViewRow.FindControl("chkStatus") as CheckBox).Checked;
                string modifiedBy = SessionVariables.getCurrentUserId();

                SysUserInfo_BOL objBOL = new SysUserInfo_BOL();
                objBOL.RecId = recId;
                objBOL.Status = status;
                objBOL.ModifiedBy = modifiedBy;

                SysUserInfo_BLL objBLL = new SysUserInfo_BLL();
                string operationResults = objBLL.sysUserInfo_UpdateStatus(objBOL);
                bool results = NotificationMessage.showMessage(operationResults);

                if (results)
                {

                }
            }
        }

        protected void ResetPassword_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                string userId = (gridViewRow.FindControl("lblUserId") as Label).Text;

                userId = SecureQueryString.encrypt(userId);

                if (string.IsNullOrEmpty(userId))
                {
                    NotificationMessage.showInvalidRecord();
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(),
                "Reset Password", "javascript: openPopupPanel('/Administration/User Management/SysUserInfo_ResetPassword.aspx?UserId=" + userId + "');", true);
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ImportUsers importUsers = new ImportUsers();
            importUsers.fileUpload = fileUpload;
            importUsers.dataImport();
        }
    }
}