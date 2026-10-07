using System;
using System.Data;
using System.Web.UI;
using GeneralAuxiliary;
using PortalIntegration;

namespace DynamicsPortal.ESS.HR   // ✅ FIXED
{
    public partial class ESSEmployeeCourse_ListPage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Courses";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Courses"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }
                LoadAssigned();
                LoadOpen();
                LoadCompleted();
            }
        }

        private long GetPersonId()
        {
            try
            {
                return Convert.ToInt64(SessionVariables.getCurrentEmployeePersonId()); // ✅ FIXED
            }
            catch
            {
                return 0;
            }
        }

        private void LoadAssigned()
        {
            try
            {
                long personId = GetPersonId();

                ESSEmployeeCourse obj = new ESSEmployeeCourse();
                DataTable dt = obj.retrieveAssigned(personId);

                gvAssigned.DataSource = dt;
                gvAssigned.DataBind();
            }
            catch (Exception ex)
            {
                LogError("LoadAssigned", ex);
            }
        }

        private void LoadOpen()
        {
            try
            {
                long personId = GetPersonId();

                ESSEmployeeCourse obj = new ESSEmployeeCourse();
                DataTable dt = obj.retrieveOpenCourses(personId); // ✅ FIXED

                gvOpen.DataSource = dt;
                gvOpen.DataBind();
            }
            catch (Exception ex)
            {
                LogError("LoadOpen", ex);
            }
        }

        private void LoadCompleted()
        {
            try
            {
                long personId = GetPersonId();

                ESSEmployeeCourse obj = new ESSEmployeeCourse();
                DataTable dt = obj.retrieveCompletedCourses(personId); 

                gvCompleted.DataSource = dt;
                gvCompleted.DataBind();
            }
            catch (Exception ex)
            {
                LogError("LoadCompleted", ex);
            }
        }

        private void LogError(string method, Exception ex)
        {
            try
            {
                SysErrorLog log = new SysErrorLog();
                log.write($"{GetType().FullName}.{method}", ex);
            }
            catch { }
        }
    }
}