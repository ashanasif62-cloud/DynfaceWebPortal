using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public class GenerateMenu
    {
        //private static EventHandler eventArgs;
        private HtmlControl findChildControlIterative(HtmlControl parentControl, string contolId)
        {
            //try
            //{
            //    HtmlControl control = parentControl;
            //    LinkedList<HtmlControl> controlsList = new LinkedList<HtmlControl>();
            //    while (control != null)
            //    {
            //        if (control.ID == contolId)
            //            return control;
            //        if (control.HasControls())
            //            foreach (HtmlControl child in control.Controls)
            //            {
            //                if (child.ID == contolId)
            //                    return child;
            //                if (child.HasControls())
            //                    controlsList.AddLast(child);
            //            }
            //        control = controlsList.First.Value;
            //        controlsList.Remove(control);
            //    }
            //    return null;

            // }
            try
            {
                HtmlControl control = parentControl;
                LinkedList<HtmlControl> controlsList = new LinkedList<HtmlControl>();

                while (control != null)
                {
                    if (control.ID == contolId)  // Fix typo here: "contolId" should be "controlId"
                        return control;

                    if (control.HasControls())
                    {
                        foreach (HtmlControl child in control.Controls)
                        {
                            if (child.ID == contolId)
                                return child;

                            if (child.HasControls())
                                controlsList.AddLast(child);
                        }
                    }

                    // ✅ Check if controlsList is empty before accessing First.Value
                    if (controlsList.Count == 0)
                        return null;  // No more controls to process, exit safely

                    control = controlsList.First.Value;
                    controlsList.RemoveFirst();  // ✅ Use RemoveFirst() to avoid errors
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void createMenus(HtmlControl mainControl, DataTable userMenuItems)
        {
            var tempParentMenus = userMenuItems.AsEnumerable().Where(dr => dr.Field<string>("ParentId") == null || dr.Field<string>("ParentId") == "");
            if (tempParentMenus.Count() > 0)
            {
                DataTable parentMenus = tempParentMenus.CopyToDataTable();
                foreach (DataRow menuItemDataRow in parentMenus.Rows)
                {
                    Boolean isVisible = menuItemDataRow["Visible"] == DBNull.Value ? true : (Convert.ToInt32(menuItemDataRow["Visible"]) == 0 ? false : true);
                    if (isVisible)
                    {
                        #region attributes
                        string menuId = menuItemDataRow["MenuId"].ToString();
                        string menuControlId = "sideMenu_ul_" + menuId;
                        #endregion
                        HtmlControl menuControl = findChildControlIterative(mainControl, menuControlId) as HtmlControl;

                        if (menuControl == null)
                        {
                            HtmlControl li = createMenu(menuItemDataRow, userMenuItems);
                            if (li != null)
                                if (li.Controls.Count > 0)
                                    mainControl.Controls.Add(li);
                        }
                    }
                }
            }
        }
        private HtmlGenericControl AddChildItem(DataTable userMenuItems, DataTable _childItems, HtmlGenericControl parentControl)
        {
            foreach (DataRow menuItemDataRow in _childItems.Rows)
            {
                Boolean isVisible = menuItemDataRow["Visible"] == DBNull.Value ? true : (Convert.ToInt32(menuItemDataRow["Visible"]) == 0 ? false : true);
                if (isVisible)
                {
                    #region attributes
                    string menuId = menuItemDataRow["MenuId"].ToString();
                    string menuControlId = "sideMenu_ul_" + menuId;
                    #endregion
                    HtmlControl menuControl = findChildControlIterative(parentControl, menuControlId) as HtmlControl;

                    if (menuControl == null)
                    {
                        HtmlControl li = createMenu(menuItemDataRow, userMenuItems);
                        if (li != null)
                            if (li.Controls.Count > 0)
                                parentControl.Controls.Add(li);
                    }
                }
            }
            return parentControl;
        }

        public HtmlControl createMenu(DataRow menuItemDataRow, DataTable userMenuItems, bool checkParents = true)
        {
            #region attributes
            string menuId = menuItemDataRow["MenuId"].ToString();
            string label = menuItemDataRow["Label"].ToString();
            string menuIcon = string.IsNullOrEmpty(menuItemDataRow["Image"].ToString()) ? "mdi mdi-file-tree" : menuItemDataRow["Image"].ToString();
            string parentId = menuItemDataRow["ParentId"].ToString();
            string menuControlId = "sideMenu_ul_" + menuId;
            #endregion
            #region controls
            HtmlGenericControl li = new HtmlGenericControl("li");
            LinkButton menuButton = new LinkButton();
            menuButton.Attributes.Add("href", "#" + menuControlId);
            menuButton.Attributes.Add("aria-expanded", "false");
            menuButton.Attributes.Add("data-toggle", "collapse");

            HtmlGenericControl iLiteral = new HtmlGenericControl("i");
            iLiteral.Attributes.Add("class", menuIcon);
            menuButton.Controls.Add(iLiteral);

            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes.Add("class", "nav-text");
            span.InnerText = label;
            menuButton.Controls.Add(span);

            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "collapse list-unstyled");
            bool parentChecked = false;
            if (checkParents)
            {
                var childItems = userMenuItems.Select("ParentId = '" + menuId + "'");
                if (childItems.Count() > 0)
                {
                    ul.ID = menuControlId;
                    AddChildItem(userMenuItems, childItems.CopyToDataTable(), ul);
                    menuButton.Attributes.Add("onClick", "return false;");
                }
                else
                    parentChecked = true;
            }
            else
                parentChecked = true;

            if (parentChecked)
            {
                string objectURL = menuItemDataRow["Object"].ToString();
                string type = menuItemDataRow["Type"] == DBNull.Value ? string.Empty : menuItemDataRow["Type"].ToString();
                string sequence = menuItemDataRow["Sequence"].ToString();
                menuButton.CommandArgument = type;
                menuButton.CommandName = objectURL;

                if (type == "1")
                {
                    if (menuId == "ESSHRHiringRequisitionRequest" || menuId == "ESSHRBusinessTripRequest")
                    {
                        menuButton.Attributes.Add("href", "javascript:;");
                        menuButton.Attributes.Add("onclick", "javascript: return openPopupPanel('" + objectURL + "', '980');");
                    }
                    else
                    {
                        menuButton.Attributes.Add("href", "javascript:;");
                        menuButton.Attributes.Add("onclick", "javascript: return openPopupPanel('" + objectURL + "');");
                    }
                }
                else
                {
                    menuButton.Attributes.Add("href", objectURL);
                }

                menuButton.Attributes.Add("data-original-title", label);
                menuButton.Attributes.Add("data-toggle", "tooltip");
                menuButton.Attributes.Add("data-placement", "top");
            }

            #endregion
            li.Controls.Add(menuButton);
            if (ul.Controls.Count > 0)
                li.Controls.Add(ul);
            return li;
        }

        protected void MenuClicked_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            string commandName = btn.CommandName;

            //MenuDelegate.checkMenu(commandName);
        }

        public void generateMenu(HtmlControl _ConfigMenu, DataTable _userMenuItems)
        {
            try
            {
                DataTable userMenuItems = _userMenuItems;
                HtmlGenericControl main001 = new HtmlGenericControl("ul");
                //main001.Attributes.Add("class", "nav side-menu");

                createMenus(main001, userMenuItems);
                _ConfigMenu.Controls.Add(main001);
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