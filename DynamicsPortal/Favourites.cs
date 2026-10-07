using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.DFEmployeeQuickLinksSvcReference;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public class Favourites
    {
        public string menuItemId;
        public Favourites(string _menuItemId)
        {
            menuItemId = _menuItemId;
        }

        public void createFavouritesMenu(DataTable _userFavData)
        {
            try
            {
                DataTable userFavData = _userFavData;
                var pageHandler = System.Web.HttpContext.Current.CurrentHandler;
                if (pageHandler is System.Web.UI.Page)
                {
                    HtmlGenericControl userMenuItems_fav = ((System.Web.UI.Page)pageHandler).Master.FindControl("userMenuItems_fav") as HtmlGenericControl;
                    if (userMenuItems_fav != null)
                    {
                        DataTable userMenuItems = SessionVariables.getUserMenuItems();
                        if (userMenuItems != null)
                        {
                            #region html
                            /*
                               <li>
                                   <a href="#mddFav" aria-expanded="false" data-toggle="collapse">
                                   <i class="mdi mdi-star-outline"></i><span class="nav-text">Favourite</span>
                                   </a>
                                   <ul id="mddFav" class="collapse list-unstyled">
                                       //<li>
                                       //<a href="/ESS/HR/ESSHRRejoining_ListPage.aspx">
                                       //      <i class="mdi mdi-file-tree"></i>
                                       //      <span class="nav-text">Rejoining History</span>
                                       //</a></li>
                                   </ul>
                               </li>
                               */
                            #endregion
                            HtmlGenericControl liPar = new HtmlGenericControl("li");
                            HtmlGenericControl aPar = new HtmlGenericControl("a");
                            aPar.Attributes.Add("href", "#mddFav");
                            aPar.Attributes.Add("aria-expanded", "false");
                            aPar.Attributes.Add("data-toggle", "collapse");
                            HtmlGenericControl iPar = new HtmlGenericControl("i");
                            iPar.Attributes.Add("class", "mdi mdi-star-outline");
                            HtmlGenericControl spanPar = new HtmlGenericControl("span");
                            spanPar.Attributes.Add("class", "nav-text");
                            spanPar.InnerText = "Favourite";
                            aPar.Controls.Add(iPar);
                            aPar.Controls.Add(spanPar);
                            liPar.Controls.Add(aPar);

                            HtmlGenericControl ulPar = new HtmlGenericControl("ul");
                            ulPar.Attributes.Add("id", "mddFav");
                            ulPar.Attributes.Add("class", "collapse list-unstyled");

                            foreach (DataRow drmenu in userFavData.Rows)
                            {
                                string menuItemId = drmenu["MenuItemId"].ToString();
                                DataRow dr = userMenuItems.Select("MenuId = '" + menuItemId + "'").FirstOrDefault();
                                if (dr != null)
                                {
                                    HtmlGenericControl li = new HtmlGenericControl("li");
                                    HtmlGenericControl a = new HtmlGenericControl("a");
                                    a.Attributes.Add("href", dr["Object"].ToString());
                                    HtmlGenericControl i = new HtmlGenericControl("i");
                                    i.Attributes.Add("class", "mdi mdi-file-tree");
                                    HtmlGenericControl span = new HtmlGenericControl("span");
                                    span.Attributes.Add("class", "nav-text");
                                    span.InnerText = dr["Label"].ToString();
                                    a.Controls.Add(i);
                                    a.Controls.Add(span);
                                    li.Controls.Add(a);
                                    ulPar.Controls.Add(li);
                                }
                            }
                            liPar.Controls.Add(ulPar);
                            userMenuItems_fav.Controls.Add(liPar);
                        }


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
            finally
            { }
        }

        public static DataTable getUserQuickLinks()
        {
            try
            {
                if (ClientConfiguration.Default.connectWithSQL)
                {
                    DataTable userQuickLinks = new DataTable();
                    string userId = SessionVariables.getCurrentUserId();
                    string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                    long partition = SessionVariables.getCurrentUserPartition();
                    if (!string.IsNullOrEmpty(userId) && partition > 0 && !string.IsNullOrEmpty(dataAreaId))
                    {
                        SysUserQuickLink_BOL objBOL = new SysUserQuickLink_BOL();
                        objBOL.UserId = userId;
                        objBOL.DataAreaId = dataAreaId;
                        objBOL.Partition = partition;
                        SysUserQuickLinks_BLL objBLL = new SysUserQuickLinks_BLL();
                        userQuickLinks = objBLL.SysUserQuickLinks_Retrieve(objBOL);
                    }
                    else
                    {
                        userQuickLinks = null;
                    }
                    return userQuickLinks;
                }
                else 
                {
                    DataTable userQuickLinks = new DFEmployeeQuickLinks().retrieveEmployeeQuickLinks();

                    return userQuickLinks;
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
                return null;
            }
            finally
            { }
        }

        public static bool checkAlreadyAddedInFav(string _menuItemId)
        {
            try
            {
                if (ClientConfiguration.Default.connectWithSQL)
                {
                    bool result = false;
                    DataTable dt_FavExists = new DataTable();
                    string userId = SessionVariables.getCurrentUserId();
                    string dataAreaId = SessionVariables.getUserCurrentDataAreaId();
                    long partition = SessionVariables.getCurrentUserPartition();
                    string menuItemId = _menuItemId;

                    if (!string.IsNullOrEmpty(userId) && partition > 0 && !string.IsNullOrEmpty(dataAreaId) && !string.IsNullOrEmpty(menuItemId))
                    {
                        SysRolesMenuItem_BOL objBOL = new SysRolesMenuItem_BOL();
                        objBOL.UserId = userId;
                        objBOL.DataAreaId = dataAreaId;
                        objBOL.Partition = partition;
                        objBOL.MenuItemId = _menuItemId;
                        SysUserQuickLinks_BLL objBLL = new SysUserQuickLinks_BLL();
                        dt_FavExists = objBLL.checkAlreadyAdded(objBOL);

                        if (dt_FavExists != null && (dt_FavExists.Rows.Count > 0))
                            result = true;

                    }
                    return result;
                }
                else
                {
                    DataTable userQuickLinks = new DFEmployeeQuickLinks().retrieveEmployeeQuickLinks();
                    bool exists = userQuickLinks.Select("MenuItemId = '" + _menuItemId + "'").Length > 0;
                    return exists;
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
                return false;
            }
            finally
            { }
        }
        public void addOrRemoveFav()     ////only for change the text of label
        {
            try
            {
                var pageHandler = System.Web.HttpContext.Current.CurrentHandler;
                if (pageHandler is System.Web.UI.Page)
                {
                    LinkButton btnFavourite = ((System.Web.UI.Page)pageHandler).Master.FindControl("btnFavourite") as LinkButton;
                    if (btnFavourite != null)
                    {
                        bool checkAlreadyExists = checkAlreadyAddedInFav(menuItemId);
                        if (checkAlreadyExists)
                        {
                            btnFavourite.Text = "Remove from favourite";
                            btnFavourite.CommandArgument = "Remove from favourite";
                            btnFavourite.ToolTip = "Remove from favourite";
                            //btnQuickLink.Click += LinkButton_Click;
                            btnFavourite.CssClass = "btn-favourite remove";
                        }
                        else
                        {
                            btnFavourite.Text = "Add to favourite";
                            btnFavourite.CommandArgument = "Add to favourite";
                            btnFavourite.ToolTip = "Add to favourite";

                            btnFavourite.CssClass = "btn-favourite add";
                        }
                        btnFavourite.Visible = true;
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
            finally
            { }
        }

        public void LinkButton_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnFavourite = (LinkButton)sender;
                SysUserQuickLink_BOL objBOL = new SysUserQuickLink_BOL();
                SysUserQuickLinks_BLL objBLL = new SysUserQuickLinks_BLL();
                long results = 0;

                DataTable userMenuItems = SessionVariables.getUserMenuItems();

                if (userMenuItems != null)
                {
                    DataRow dr = userMenuItems.Select("MenuId = '" + menuItemId + "'").FirstOrDefault();
                    if (dr != null)
                    {
                        //addOrRemoveFav();
                        if (btnFavourite.CommandArgument == "Add to favourite")
                        {
                            if (ClientConfiguration.Default.connectWithSQL)
                            {
                                objBOL.MenuItemId = menuItemId;
                                objBOL.UserId = SessionVariables.getCurrentUserId();
                                objBOL.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
                                objBOL.Partition = SessionVariables.getCurrentUserPartition();
                                objBOL.CreatedBy = objBOL.UserId;
                                objBOL.ModifiedBy = objBOL.UserId;
                                results = objBLL.UserQuickLinks_Create(objBOL);
                                if (results > 0)
                                {
                                    btnFavourite.Text = "Remove from favourite";
                                    btnFavourite.CommandArgument = "Remove from favourite";
                                    btnFavourite.ToolTip = "Remove from favourite";
                                    btnFavourite.CssClass = "btn-favourite remove";
                                }
                                else
                                {
                                    btnFavourite.Text = "Add to favourite";
                                    btnFavourite.CommandArgument = "Add to favourite";
                                    btnFavourite.ToolTip = "Add to favourite";
                                    btnFavourite.CssClass = "btn-favourite add";
                                }
                            }
                            else
                            {
                                DFEmployeeQuickLinks quickLinks = new DFEmployeeQuickLinks();
                                GeneralContract result = quickLinks.createEmployeeQuickLinks(menuItemId);
                                if (result.IsSuccess)
                                {
                                    btnFavourite.Text = "Remove from favourite";
                                    btnFavourite.CommandArgument = "Remove from favourite";
                                    btnFavourite.ToolTip = "Remove from favourite";
                                    btnFavourite.CssClass = "btn-favourite remove";
                                }
                                else
                                {
                                    btnFavourite.Text = "Add to favourite";
                                    btnFavourite.CommandArgument = "Add to favourite";
                                    btnFavourite.ToolTip = "Add to favourite";
                                    btnFavourite.CssClass = "btn-favourite add";
                                }
                            }
                        }
                        else if (btnFavourite.CommandArgument == "Remove from favourite")
                        {
                            if (ClientConfiguration.Default.connectWithSQL)
                            {
                                //Code for Remove Quick Link
                                objBOL.MenuItemId = menuItemId;
                                objBOL.UserId = SessionVariables.getCurrentUserId();
                                objBOL.DataAreaId = SessionVariables.getUserCurrentDataAreaId();
                                objBOL.Partition = SessionVariables.getCurrentUserPartition();
                                results = objBLL.UserQuickLinks_Delete(objBOL);
                                if (results == 1)
                                {
                                    btnFavourite.Text = "Add to favourite";
                                    btnFavourite.CommandArgument = "Add to favourite";
                                    btnFavourite.CssClass = "btn-favourite add";
                                }
                                else
                                {
                                    btnFavourite.Text = "Remove from favourite";
                                    btnFavourite.CommandArgument = "Remove from favourite";
                                    btnFavourite.CssClass = "btn-favourite remove";
                                }
                            }
                            else
                            {
                                DFEmployeeQuickLinks quickLinks = new DFEmployeeQuickLinks();
                                GeneralContract result = quickLinks.delete(menuItemId);
                                if (result.IsSuccess)
                                {
                                    btnFavourite.Text = "Add to favourite";
                                    btnFavourite.CommandArgument = "Add to favourite";
                                    btnFavourite.CssClass = "btn-favourite add";
                                }
                                else
                                {
                                    btnFavourite.Text = "Remove from favourite";
                                    btnFavourite.CommandArgument = "Remove from favourite";
                                    btnFavourite.CssClass = "btn-favourite remove";
                                }
                            }
                        }
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
            finally
            { }
        }


    }
}