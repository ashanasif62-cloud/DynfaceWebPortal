using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.APEmployeeAppraisalsKPIAppraisersSvcReference;
using PortalIntegration.APEmployeeAppraisalsSvcReference;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSAPDailyGroomingAppraisalsAssignedToMe_ListPage : MainForm
    {
        private APEmployeeAppraisals employeeAppraisals = new APEmployeeAppraisals();
        private APEmployeeAppraisalsKPIAppraisers employeeAppraisalsKPIAppraisers = new APEmployeeAppraisalsKPIAppraisers();
        private APAppraisalsRating appraisalsRating = new APAppraisalsRating();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSAPDailyGroomingAppraisalsAssignedToMeHistory";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    reBindGridAppraisals();
                    bindEmptyData();

                    btnReviewed.Enabled = false;
                    btnSave.Enabled = false;
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

        protected void reBindGridAppraisals()
        {
            getGridDataAppraisals();
            bindGridAppraisals();
        }
        private void getGridDataAppraisals()
        {
            DataTable dt = employeeAppraisals.retriveDailyGroomingAppraisalsAssignedToMe();
            SessionVariables.setSessionDataTable(dt);
        }
        protected void bindGridAppraisals()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView_Appraisals.DataSource = dt;
            gridView_Appraisals.DataBind();
        }

        protected void Lines_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                long recId = 0;
                Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                string appraisalCode = ((Label)gridViewRow.FindControl("lblAppraisalCode")).Text;
                string appraisalStatus = ((Label)gridViewRow.FindControl("lblAppraisalStatus")).Text;

                setViewState_AppraisalId(recId);
                getGridData_KPIAppraisals();
                bindGrid_KPIAppraisals();

                getGridData_AppraisalsRating(appraisalCode);
                bindGrid_AppraisalsRating();

                if (appraisalStatus != APAppraisalStatus.Assigned.ToString())
                {
                    gridView_KPIAppraisals.Enabled = false;
                    btnReviewed.Enabled = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    gridView_KPIAppraisals.Enabled = true;
                    btnReviewed.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
        }

        private void getGridData_KPIAppraisals()
        {
            //GridViewRow row = gridView_KPIAppraisals.SelectedRow;
            string employeeId = SessionVariables.getCurrentEmployeeId();
            long employeeAppraisalsId = getViewState_AppraisalId();

            DataTable dt = employeeAppraisalsKPIAppraisers.reteriveEmployeeKPIAppraisers(employeeAppraisalsId, employeeId);
            setViewState_KPIAppraisals(dt);
        }
        protected void reBindGrid_KPIAppraisals()
        {
            getGridData_KPIAppraisals();
            bindGrid_KPIAppraisals();
        }
        protected void bindGrid_KPIAppraisals()
        {
            DataTable dt = getViewState_KPIAppraisals();
            gridView_KPIAppraisals.DataSource = dt;
            gridView_KPIAppraisals.DataBind();
        }

        private void getGridData_AppraisalsRating(string _appraisalCode)
        {
            string appraisalCode = _appraisalCode;

            DataTable dt = appraisalsRating.retriveByAppraisalCode(appraisalCode);
            setViewState_AppraisalsRating(dt);
        }
        protected void bindGrid_AppraisalsRating()
        {
            DataTable dt = getViewState_AppraisalsRating();
            gridView_Rating.DataSource = dt;
            gridView_Rating.DataBind();
        }

        #region ViewState
        public void bindEmptyData()
        {
            DataTable dtKPIAppraisers = employeeAppraisalsKPIAppraisers.createDataTable();
            gridView_KPIAppraisals.DataSource = dtKPIAppraisers;
            gridView_KPIAppraisals.DataBind();

            DataTable dtKPIAppraisalsRating = appraisalsRating.createDataTable();
            gridView_Rating.DataSource = dtKPIAppraisalsRating;
            gridView_Rating.DataBind();
        }

        public bool setViewState_KPIAppraisals(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_KPIAppraisals"] = dataTable;
                isStored = true;
            }
            return isStored;
        }
        public DataTable getViewState_KPIAppraisals()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_KPIAppraisals"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_KPIAppraisals"] as DataTable).Copy();
            }
            //else
            //{
            //    getGridData_KPIAppraisals();
            //    dataTable = getViewState_KPIAppraisals();
            //}
            return dataTable;
        }


        public bool setViewState_AppraisalId(long _employeeAppraisalId)
        {
            bool isStored = false;
            long employeeAppraisalsId = _employeeAppraisalId;

            if (ViewState != null)
            {
                ViewState["AppraisalId"] = employeeAppraisalsId;
                isStored = true;
            }
            return isStored;
        }
        public long getViewState_AppraisalId()
        {
            long appraisalId = 0;

            if (ViewState["AppraisalId"] != null)
            {
                Int64.TryParse(ViewState["AppraisalId"].ToString(), out appraisalId);
            }
            return appraisalId;
        }


        public bool setViewState_AppraisalsRating(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_AppraisalsRating"] = dataTable;
                isStored = true;
            }
            return isStored;
        }
        public DataTable getViewState_AppraisalsRating()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_AppraisalsRating"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_AppraisalsRating"] as DataTable).Copy();
            }
            return dataTable;
        }


        #endregion

        protected void gridView_KPIAppraisals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

            //DropDownList ddlIsPrimaryTaxRegistration = gridViewRow.FindControl("ddlIsPrimaryTaxRegistration") as DropDownList;

            string appraiserStatus = dataRowView["AppraiserStatus"].ToString().Replace(" ", "");

            if (appraiserStatus != APAppraiserStatus.InProcess.ToString())
            {
                gridViewRow.Enabled = false;
                gridViewRow.Attributes.Add("NonActionable", "true");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isValidRequest = false;

            long employeeAppraisalsId = getViewState_AppraisalId();
            DataTable dataTableAppraisals = SessionVariables.getSessionDataTable();
            if (dataTableAppraisals != null)
            {
                DataRow dataRow = dataTableAppraisals.Select("RecId = '" + employeeAppraisalsId + "'").FirstOrDefault();
                if (dataRow != null)
                {
                    string appraisalStatus = dataRow["AppraisalStatus"].ToString();

                    if (appraisalStatus == APAppraisalStatus.Assigned.ToString())
                    {
                        isValidRequest = true;
                    }
                }
            }


            if (isValidRequest)
            {
                DataTable dataTable = employeeAppraisalsKPIAppraisers.createDataTable();
                foreach (GridViewRow gridViewRow in gridView_KPIAppraisals.Rows)
                {
                    DataRow dr = dataTable.NewRow();
                    dr["RecId"] = (gridViewRow.FindControl("lblRecId") as Label).Text;
                    dr["Score"] = (gridViewRow.FindControl("txtScore") as TextBox).Text;
                    dataTable.Rows.Add(dr);
                }
                SysOperationResult_BOL operationResult_BOL = employeeAppraisalsKPIAppraisers.update(dataTable, employeeAppraisalsId);
                bool result = operationResults(operationResult_BOL);

            }
            else
            {
                NotificationMessage.showMessage(AlertType.Error, "Please select a valid employee appraisal.");
            }

        }

        protected void btnReviewed_Click(object sender, EventArgs e)
        {
            bool isValidRequest = false;

            long employeeAppraisalsId = getViewState_AppraisalId();
            DataTable dataTableAppraisals = SessionVariables.getSessionDataTable();
            if (dataTableAppraisals != null)
            {
                DataRow dataRow = dataTableAppraisals.Select("RecId = '" + employeeAppraisalsId + "'").FirstOrDefault();
                if (dataRow != null)
                {
                    string appraisalStatus = dataRow["AppraisalStatus"].ToString();

                    if (appraisalStatus == APAppraisalStatus.Assigned.ToString())
                    {
                        isValidRequest = true;
                    }
                }
            }


            if (isValidRequest)
            {
                SysOperationResult_BOL operationResult_BOL = employeeAppraisalsKPIAppraisers.review(employeeAppraisalsId);
                bool result = operationResults(operationResult_BOL);
            }
            else
            {
                NotificationMessage.showMessage(AlertType.Error, "Please select a valid employee appraisal.");
            }

        }
        


    }
}