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
    public partial class MainForm : System.Web.UI.Page
    {
        public string pageMenuId;
        public string tableId;

        public bool showActionPanel = true;
        public bool isPageAuthorizated;
        //public bool isUserRoleValidated;
        public bool isUserAuthenticated;

        protected virtual void Page_Load(object sender, EventArgs e)
        {
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
                setActionPanel();
                refreshFavourites();
                ViewState.Clear();
            }
            createUserCompanyDetails();
            manageFavourite();
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
            controlsHelper.setPageTitle(pageMenuId, true);
        }

        private void setActionPanel()
        {
            if ((HttpContext.Current.CurrentHandler as Page).Master != null)
            {
                HtmlGenericControl actionPanel = (HttpContext.Current.CurrentHandler as Page).Master.FindControl("actionPanelContainer") as HtmlGenericControl;
                actionPanel.Visible = showActionPanel;
            }

        }

        private void createUserCompanyDetails()
        {
            if ((HttpContext.Current.CurrentHandler as Page).Master != null)
            {
                UserCompanyDetails companyDetails = new UserCompanyDetails();
                companyDetails.setCompanyLabel();
                companyDetails.generateMenu();
            }

        }

        public bool operationResults(SysOperationResult_BOL _objBOL)
        {
            SysOperationResult_BOL objBOL = _objBOL;
            bool result = objBOL.isSuccess;
            NotificationMessage.showMessage(objBOL);
            //reBindGrid();
            //if (objBOL.isSuccess)
            //{
            //    reBindGrid();
            //}
            //else
            //{
            //    bindGrid();
            //}
            return result;
        }

        private void refreshFavourites()
        {
            DataTable userQuickLinks = Favourites.getUserQuickLinks();
            bool isUserQuickLinkStored = SessionVariables.setUserQuickLinks(userQuickLinks);

        }

        protected void manageFavourite()
        {
            var pageHandler = HttpContext.Current.CurrentHandler;
            if ((pageHandler is Page) && ((pageHandler as Page).Master != null))
            {
                Favourites favourites = new Favourites(pageMenuId);
                #region ManageFavMenu
                DataTable userQuickLinks = SessionVariables.getCurrentUserQuickLinks();
                favourites.createFavouritesMenu(userQuickLinks);
                #endregion

                #region ManageFavButton
                LinkButton btnFavourites = ((Page)pageHandler).Master.FindControl("btnFavourite") as LinkButton;
                if (btnFavourites != null)
                {
                    favourites.addOrRemoveFav();
                    btnFavourites.Click += btnFavourite_Click;
                }
                #endregion

            }
        }

        protected void btnFavourite_Click(object sender, EventArgs e)
        {
            Favourites favourites = new Favourites(pageMenuId);
            favourites.LinkButton_Click(sender, e);
        }

        protected GridViewRow getSelectedGridViewRow(GridView gridView)
        {

            GridViewRow gridViewRow = null;
            foreach (GridViewRow gvRows in gridView.Rows)
            {
                CheckBox chkSelectRow = gvRows.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    if (gvRows.RowType == DataControlRowType.DataRow)
                    {
                        gridViewRow = gvRows;
                        break;
                    }
                }
            }
            return gridViewRow;

        }
    }
}