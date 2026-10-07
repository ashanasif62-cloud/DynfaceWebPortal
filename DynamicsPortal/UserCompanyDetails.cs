using BussinessLogic;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public class UserCompanyDetails
    {

        public void setCompanyLabel()
        {

            if ((HttpContext.Current.CurrentHandler as Page).Master != null)
            {
                string currentCompany = SessionVariables.getUserCurrentDataAreaId();
                HtmlGenericControl currentCompanyId = (HttpContext.Current.CurrentHandler as Page).Master.FindControl("currentCompanyId") as HtmlGenericControl;
                currentCompanyId.InnerText = currentCompany;
            }

        }

        public bool generateMenu()
        {
            try
            {
                DataTable dtUserCompanies = SessionVariables.getUserCompanies();
                if (dtUserCompanies.Rows.Count > 0)
                {
                    HtmlGenericControl companyMenu = (HttpContext.Current.CurrentHandler as Page).Master.FindControl("companyDetails") as HtmlGenericControl;

                    if (dtUserCompanies != null)
                        if (dtUserCompanies.Rows.Count > 0)
                            foreach (DataRow dr in dtUserCompanies.Rows)
                            {
                                string dataAreaId = dr["DataAreaId"].ToString();
                                string companyFullName = dr["FullName"].ToString();
                                //<li>
                                //    <a rel="nofollow" href="#" class="dropdown-item">
                                //        <div class="notification d-flex justify-content-between">
                                //            <div class="notification-content">Company Name (DAT)</div>
                                //        </div>
                                //    </a>
                                //</li>

                                HtmlGenericControl li = new HtmlGenericControl("li");
                                //li.Style.Add("padding", "unset");
                                //li.Attributes.Add("data-original-title", "Change Company to " + dataAreaId);
                                //li.Attributes.Add("data-toggle", "tooltip");
                                //li.Attributes.Add("data-placement", "top");

                                LinkButton btnCompany = new LinkButton();
                                btnCompany.ID = dataAreaId;
                                btnCompany.CommandName = dataAreaId;
                                btnCompany.Attributes.Add("rel", "nofollow");
                                btnCompany.Attributes.Add("class", "dropdown-item");
                                btnCompany.Attributes.Add("data-bs-toggle", "tooltip");
                                btnCompany.Attributes.Add("data-bs-placement", "right");
                                btnCompany.Attributes.Add("title", companyFullName);
                                btnCompany.Click += ChangeCompany_Click;

                                HtmlGenericControl div01 = new HtmlGenericControl("div");
                                div01.Attributes.Add("class", "notification d-flex justify-content-between");

                                HtmlGenericControl div02 = new HtmlGenericControl("div");
                                div02.Attributes.Add("class", "notification-content");
                                div02.InnerText = companyFullName + " (" + dataAreaId + ")";

                                //HtmlGenericControl span02 = new HtmlGenericControl("span");
                                //span02.Attributes.Add("class", "dropdown-item");
                                //span02.InnerText = companyFullName;

                                div01.Controls.Add(div02);
                                btnCompany.Controls.Add(div01);
                                li.Controls.Add(btnCompany);

                                companyMenu.Controls.Add(li);
                            }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }

        private void ChangeCompany_Click(object sender, EventArgs e)
        {
            LinkButton btnCompany = sender as LinkButton;
            string dataAreaId = btnCompany.CommandName;

            DataTable userCompanies = SessionVariables.getUserCompanies();
            if (userCompanies != null && !string.IsNullOrEmpty(dataAreaId))
            {
                DataRow dr = userCompanies.Select("DataAreaId = '" + dataAreaId + "'").FirstOrDefault();
                if (dr != null)
                {
                    string companyFullName = dr["FullName"].ToString();

                    SessionVariables.clearCompanyRelatedSession();

                    bool isStored = SessionVariables.setUserCurrentDataAreaId(dataAreaId);
                    bool isStored2 = SessionVariables.setUserCurrentDataAreaName(companyFullName);

                    if (isStored && isStored2)
                    {
                        if (ClientConfiguration.Default.connectWithSQL)
                        {
                            DataTable dtUserMenuItems = SysUserMenuItems_BLL.SysUserMenuItems_Retrieve();
                            bool isUserMenuItemsStored = SessionVariables.setUserMenuItems(dtUserMenuItems);

                            //DataTable dtUserRoles = SysUserRoles_BLL.SysUserRoles_RetrieveByUserId();
                            //bool isUserRolesStored = SessionVariables.setUserRoles(dtUserRoles);
                        }
                        else
                        {
                            DataTable dtUserMenuItems = new DFUserInfo().retrieveEmployeeMenuItems();
                            bool isUserMenuItemsStored = SessionVariables.setUserMenuItems(dtUserMenuItems);

                            //DataTable dtUserRoles = SysUserRoles_BLL.SysUserRoles_RetrieveByUserId();
                            //bool isUserRolesStored = SessionVariables.setUserRoles(dtUserRoles);
                        }
                        
                        string currentURL = System.Web.HttpContext.Current.Request.Url.ToString();
                        System.Web.HttpContext.Current.Response.Redirect(currentURL, false);
                        return;
                    }

                }
                else
                {
                    string message = "User doesn't have access to selected company";
                    NotificationMessage.showMessage(AlertType.Error, message);
                }
            }
            else
            {
                string message = "Unable to retrieve user company data";
                NotificationMessage.showMessage(AlertType.Error, message);
            }

        }

    }
}