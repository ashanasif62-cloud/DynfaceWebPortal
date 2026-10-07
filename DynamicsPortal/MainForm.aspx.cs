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
                refreshFavourites();
                setActionPanel();
                ViewState.Clear();
                // Track the current page in recent menu
                if (!string.IsNullOrEmpty(pageMenuId))
                {
                    addPageToRecent(pageMenuId);
                }
            }
            createUserCompanyDetails();
            manageFavourite();
            manageRecent();
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
                
                //(HttpContext.Current.CurrentHandler as Page).ClientScript.RegisterStartupScript((HttpContext.Current.CurrentHandler as Page).GetType(),
                //"Attachment", "javascript: openPopupPanel('/ESS/ESSDocumentAttachment.aspx?ref=" + recId + "&table=" + table + "' ,'980');", true);
                //For AJAX
                Page currentPage = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(),
                "Attachment", $"openPopupPanel('/ESS/ESSDocumentAttachment.aspx?ref={recId}&table={table}', '980');", true);
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

        protected void manageRecent()
        {
            try
            {
                var pageHandler = System.Web.HttpContext.Current.CurrentHandler;
                if (pageHandler is System.Web.UI.Page)
                {
                    HtmlGenericControl userMenuItems_recent = ((System.Web.UI.Page)pageHandler).Master.FindControl("userMenuItems_recent") as HtmlGenericControl;
                    if (userMenuItems_recent != null)
                    {
                        HtmlGenericControl liPar = new HtmlGenericControl("li");
                        HtmlGenericControl aPar = new HtmlGenericControl("a");
                        aPar.Attributes.Add("href", "#mddRecent");
                        aPar.Attributes.Add("aria-expanded", "false");
                        aPar.Attributes.Add("data-toggle", "collapse");

                        HtmlGenericControl iPar = new HtmlGenericControl("i");
                        iPar.Attributes.Add("class", "mdi mdi-history");

                        HtmlGenericControl spanPar = new HtmlGenericControl("span");
                        spanPar.Attributes.Add("class", "nav-text");
                        spanPar.InnerText = "Recent";

                        aPar.Controls.Add(iPar);
                        aPar.Controls.Add(spanPar);
                        liPar.Controls.Add(aPar);

                        HtmlGenericControl ulPar = new HtmlGenericControl("ul");
                        ulPar.Attributes.Add("id", "mddRecent");
                        ulPar.Attributes.Add("class", "collapse list-unstyled");

                        DataTable dtRecent = SessionVariables.getCurrentUserRecentLinks();
                        if (dtRecent != null && dtRecent.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecent.Rows)
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                HtmlGenericControl a = new HtmlGenericControl("a");
                                a.Attributes.Add("href", dr["Url"].ToString());
                                HtmlGenericControl i = new HtmlGenericControl("i");
                                i.Attributes.Add("class", "mdi mdi-file-tree");
                                HtmlGenericControl span = new HtmlGenericControl("span");
                                span.Attributes.Add("class", "nav-text");
                                span.InnerText = dr["Title"].ToString();
                                a.Controls.Add(i);
                                a.Controls.Add(span);
                                li.Controls.Add(a);
                                ulPar.Controls.Add(li);
                            }
                        }
                        else
                        {
                            // Show a message when no recent items exist
                            HtmlGenericControl liEmpty = new HtmlGenericControl("li");
                            HtmlGenericControl aEmpty = new HtmlGenericControl("a");
                            aEmpty.InnerText = "No recent pages";
                            aEmpty.Attributes.Add("style", "color: #999; cursor: default;");
                            liEmpty.Controls.Add(aEmpty);
                            ulPar.Controls.Add(liEmpty);
                        }

                        liPar.Controls.Add(ulPar);
                        userMenuItems_recent.Controls.Add(liPar);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                NotificationMessage.showMessage(AlertType.Error, message);

                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
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

        public static void AddRecentPage(string url, string title)
        {
            DataTable dt = SessionVariables.getCurrentUserRecentLinks();

            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Url", typeof(string));
                dt.Columns.Add("Title", typeof(string));
            }

            // Remove existing entry if duplicate
            var existing = dt.Select($"Url = '{url.Replace("'", "''")}'");
            foreach (var row in existing)
                dt.Rows.Remove(row);

            // Insert new row at top
            DataRow dr = dt.NewRow();
            dr["Url"] = url;
            dr["Title"] = title;
            dt.Rows.InsertAt(dr, 0);

            // Limit to 10 entries
            while (dt.Rows.Count > 10)
                dt.Rows.RemoveAt(dt.Rows.Count - 1);

            SessionVariables.setUserRecentLinks(dt);
        }

        protected void addPageToRecent(string _pageMenuId)
        {
            DataTable dt = SessionVariables.getUserMenuItems();

            DataRow[] foundRows = dt.Select("MenuId = '" + _pageMenuId + "'");

            if (foundRows.Length > 0)
            {
                DataRow dr = foundRows[0];
                string url, title;

                url = dr["Object"] != null ? dr["Object"].ToString() : "";
                title = dr["Label"] != null ? dr["Label"].ToString() : "";

                if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(title)) 
                {
                    AddRecentPage(url, title);
                }
            }

        }
    }
}