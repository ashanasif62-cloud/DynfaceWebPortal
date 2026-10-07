using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Data;
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
            //isPageAuthorizated = true;
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


        //protected virtual void btnAttachment_Click(object sender, EventArgs e)
        //{
        //    GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
        //    if (gridViewRow.RowType == DataControlRowType.DataRow)
        //    {
        //        string recId = SecureQueryString.encrypt(((Label)gridViewRow.FindControl("lblRecId")).Text);
        //        string table = SecureQueryString.encrypt(tableId);

        //        if (string.IsNullOrEmpty(recId) || string.IsNullOrEmpty(table))
        //        {
        //            NotificationMessage.showInvalidRecord();
        //            return;
        //        }

        //        (HttpContext.Current.CurrentHandler as Page).ClientScript.RegisterStartupScript((HttpContext.Current.CurrentHandler as Page).GetType(),
        //        "Attachment", "javascript: openPopupPanel('/ESS/ESSDocumentAttachment.aspx?ref=" + recId + "&table=" + table + "' ,'980');", true);

        //    }
        //}

        protected virtual void btnAttachment_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                string recId = SecureQueryString.encrypt(((Label)gridViewRow.FindControl("lblRecId")).Text);
                bool isHRRecord = string.IsNullOrEmpty((gridViewRow.FindControl("lblApprovalStatus") as Label)?.Text);
                string table = SecureQueryString.encrypt(tableId);

                if (string.IsNullOrEmpty(recId) || string.IsNullOrEmpty(table))
                {
                    NotificationMessage.showInvalidRecord();
                    return;
                }

                string url = "/ESS/ESSDocumentAttachment.aspx?ref=" + recId + "&table=" + table + "&isHRRecord=" + isHRRecord;

                string script = @"
            (function() {
                // Remove existing modal if any
                var existing = document.getElementById('attachmentModalOverlay');
                if (existing) existing.parentNode.removeChild(existing);

                var overlay = document.createElement('div');
                overlay.id = 'attachmentModalOverlay';
                overlay.style.cssText = 'position:fixed;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,0.5);z-index:9999;display:flex;align-items:center;justify-content:center;';

                var card = document.createElement('div');
                card.style.cssText = 'background:#fff;border-radius:8px;box-shadow:0 8px 32px rgba(0,0,0,0.25);width:90%;max-width:980px;height:80vh;display:flex;flex-direction:column;overflow:hidden;';

                var header = document.createElement('div');
                header.style.cssText = 'display:flex;align-items:center;justify-content:space-between;padding:12px 16px;border-bottom:1px solid #e0e0e0;background:#f8f8f8;border-radius:8px 8px 0 0;';
                header.innerHTML = '<span style=""font-weight:600;font-size:14px;color:#333;"">Attachments</span>';

                var closeBtn = document.createElement('button');
                closeBtn.innerHTML = '&times;';
                closeBtn.style.cssText = 'background:none;border:none;font-size:22px;cursor:pointer;color:#666;line-height:1;padding:0 4px;';
                closeBtn.onclick = function() { document.body.removeChild(overlay); };
                header.appendChild(closeBtn);

                var iframe = document.createElement('iframe');
                iframe.src = '" + url + @"';
                iframe.style.cssText = 'flex:1;border:none;width:100%;';

                card.appendChild(header);
                card.appendChild(iframe);
                overlay.appendChild(card);

                // Close on backdrop click
                overlay.addEventListener('click', function(e) {
                    if (e.target === overlay) document.body.removeChild(overlay);
                });

                document.body.appendChild(overlay);
            })();
        ";

                // REPLACE with this:
                ScriptManager.RegisterStartupScript(
                    (HttpContext.Current.CurrentHandler as Page),
                    (HttpContext.Current.CurrentHandler as Page).GetType(),
                    "Attachment", script, true);
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

        protected void registerAttachmentCounts(DataTable dt, string recIdColumnName = "RecId")
        {
            if (dt == null) return;

            var page = HttpContext.Current.CurrentHandler as Page;
            if (page == null) return;

            foreach (DataRow dr in dt.Rows)
            {
                string recId = dr[recIdColumnName].ToString();
                if (string.IsNullOrEmpty(recId) || recId == "0") continue;

                string encryptedRef = SecureQueryString.encrypt(recId);

                // Get attachment count from DB
                int count = 0;// SysDocuRef.getAttachmentCount(recId, tableId); // ← your existing helper

                if (count <= 0) continue; // skip zero — no badge needed

                ScriptManager.RegisterStartupScript(page, page.GetType(),
                    "attach_badge_" + recId,
                    string.Format("if(window.updateAttachmentBadge)updateAttachmentBadge('{0}',{1});",
                        encryptedRef, count),
                    true);
            }
        }

    }
}