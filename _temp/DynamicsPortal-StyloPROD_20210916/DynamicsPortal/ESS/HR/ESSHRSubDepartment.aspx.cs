using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;

namespace DynamicsPortal
{
    public partial class ESSHRSubDepartment : ModalForm
    {
        private HcmPersonLaborUnion hcmPersonLaborUnion = new HcmPersonLaborUnion();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hcmPersonLaborUnion.tableName;
                pageMenuId = "ESSHRPersonalDetailsHistory";
                showPageTitle = false;

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

        public bool setViewState_HcmPersonLaborUnion(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_HcmPersonLaborUnion"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public DataTable getViewState_HcmPersonLaborUnion()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_HcmPersonLaborUnion"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_HcmPersonLaborUnion"] as DataTable).Copy();
            }
            else
            {
                getGridDataTable();
                dataTable = getViewState_HcmPersonLaborUnion();
            }
            return dataTable;
        }

        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = hcmPersonLaborUnion.findByEmployee(employeeId);
            setViewState_HcmPersonLaborUnion(dt);
        }

        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }

        protected void bindGrid()
        {
            DataTable dt = getViewState_HcmPersonLaborUnion();
            gridView.DataSource = dt;
            gridView.DataBind();
        }


    }
}