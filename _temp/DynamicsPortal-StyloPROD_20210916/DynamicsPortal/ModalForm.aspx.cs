using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ModalForm : System.Web.UI.Page
    {
        public string pageMenuId;
        public string tableId;

        public bool showPageTitle = true;
        public bool isUserAuthenticated;
        public bool isPageAuthorizated;
        //public bool isUserRoleValidated;

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            //userAuthentication();
            //if (!isUserAuthenticated)
            //    return;
            //if (!IsPostBack)
            //{
            //    setPageTitle();
            //}

            AuthenticationHelper.AuthenticationHelper authenticationHelper = new AuthenticationHelper.AuthenticationHelper();
            isUserAuthenticated = authenticationHelper.userAuthentication();
            if (!isUserAuthenticated)
                return;

            isPageAuthorizated = authenticationHelper.pageAuthentication(pageMenuId);
            if (!isPageAuthorizated)
                return;

            if (!IsPostBack)
            {
                //isUserRoleValidated = authenticationHelper.validateUserRoles();
                //if (!isUserRoleValidated)
                //{
                //    //FormsAuthentication.RedirectToLoginPage();
                //    return;
                //}
                setPageTitle();
            }
        }


        protected virtual void btnAttachment_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                string recId = SecureQueryString.encrypt(((Label)gridViewRow.FindControl("lblRecId")).Text);
                string table = SecureQueryString.encrypt(tableId);

                if (string.IsNullOrEmpty(recId) || string.IsNullOrEmpty(table))
                {
                    NotificationMessage.showInvalidRecord();
                    return;
                }

                (HttpContext.Current.CurrentHandler as Page).ClientScript.RegisterStartupScript((HttpContext.Current.CurrentHandler as Page).GetType(),
                "Attachment", "javascript: openPopupPanel('/ESS/ESSDocumentAttachment.aspx?ref=" + recId + "&table=" + table + "' ,'980');", true);

            }
        }


        private void setPageTitle()
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            controlsHelper.setPageTitle(pageMenuId, showPageTitle);
        }

        public bool operationResults(SysOperationResult_BOL _createResult, bool _redirectToParentForm = false)
        {
            bool redirectToParentForm = _redirectToParentForm;
            SysOperationResult_BOL createResult = _createResult;
            bool result = createResult.isSuccess;

            if (result && redirectToParentForm)
                redirectToParentForm = true;
            else
                redirectToParentForm = false;
            //this.redirectToParentForm(result);

            NotificationMessage.showMessage(createResult, redirectToParentForm);

            return result;
        }
        public bool operationResults(SysOperationResult_BOL _createResult, SysOperationResult_BOL _submitResult, bool _redirectToParentForm = false)
        {
            bool redirectToParentForm = _redirectToParentForm;
            SysOperationResult_BOL createResult = _createResult;
            SysOperationResult_BOL submitResult = _submitResult;

            if (submitResult.isSuccess)
            {
                submitResult.Message = " Request successfully submitted.";
            }
            else
            {
                submitResult.AlertType = AlertType.Error.ToString();
                submitResult.isSuccess = false;
                submitResult.Message = " Failed to submit the request.";
            }

            createResult.Message += " " + submitResult.Message;
            createResult.AlertType = submitResult.AlertType;
            createResult.isSuccess = submitResult.isSuccess;

            bool result = operationResults(createResult, redirectToParentForm);

            return result;
        }


        public void redirectToParentForm(bool _result)
        {
            bool result = _result;
            if (result)
            {
                Page.Response.Redirect(Page.Request.Url.ToString(), true);       //Page.Request.RawUrl    //Page.Request.Url.ToString()
            }
        }


    }
}