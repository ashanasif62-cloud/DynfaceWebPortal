using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;

namespace DynamicsPortal
{
    public partial class ESSPRProvidentFund_MyTeamInquiry : MainForm
    {
        private PRProvidentFundInquiry pRProvidentFundInquiry = new PRProvidentFundInquiry();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPRProvidentFund_MyTeamInquiry";
                showActionPanel = false;

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    reBindGrid();
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
        private void getGridDataTable()
        {
            DataTable dt = pRProvidentFundInquiry.retrieveAllPRProvidentFundInquiry(true);
            SessionVariables.setSessionDataTable(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();
        }
        
    }
}