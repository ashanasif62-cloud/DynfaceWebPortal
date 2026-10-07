using System;
using System.Data;
using GeneralAuxiliary;
using PortalIntegration;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSEmployeeSurvay_ListPage : MainForm
    {
        private HRExitInteviewsSvc _svc = new HRExitInteviewsSvc();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSEmployeeSurvay_ListPage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Employee Survey and Questionaires";

                    var titleDiv =
                        Master.FindControl("pageTitle")
                        as System.Web.UI.HtmlControls.HtmlGenericControl;

                    if (titleDiv != null)
                    {
                        titleDiv.InnerText =
                            "Employee Survey and Questionaires";

                        titleDiv.Style["font-weight"] = "bold";
                    }

                    BindEmployeeSurvey();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();

                log.write(
                    $"{GetType().FullName}.{nameof(Page_Load)}",
                    ex);
            }
        }


        private void BindEmployeeSurvey()
        {
            try
            {
                DataTable dataTable =
                    _svc.retrieveEmployeeSurvay();

                if (dataTable != null &&
                    dataTable.Rows.Count > 0)
                {
                    gvEmployeeSurvey.DataSource = dataTable;
                    gvEmployeeSurvey.DataBind();
                }
                else
                {
                    gvEmployeeSurvey.DataSource = null;
                    gvEmployeeSurvey.DataBind();
                }
            }
            catch (Exception ex)
            {
                gvEmployeeSurvey.DataSource = null;
                gvEmployeeSurvey.DataBind();

                SysErrorLog log = new SysErrorLog();

                log.write(
                    $"{GetType().FullName}.{nameof(BindEmployeeSurvey)}",
                    ex);
            }
        }
    }
}

