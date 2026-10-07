using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HRHiringRequisitionSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHRHiringRequisition_ListPage : MainForm
    {
        private HRHiringRequisition hRHiringRequisition = new HRHiringRequisition();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hRHiringRequisition.tableName;
                pageMenuId = "ESSHRHiringRequisitionHistory";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    reBindGrid_HiringRequisition();
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

        protected void reBindGrid_HiringRequisition()
        {
            getGridDataHiringRequisition();
            bindGrid_HiringRequisition();
        }
        private void getGridDataHiringRequisition()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = hRHiringRequisition.retrieveHiringRequisitions(employeeId);
            SessionVariables.setSessionDataTable(dt);
        }
        protected void bindGrid_HiringRequisition()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView_HiringRequisition.DataSource = dt;
            gridView_HiringRequisition.DataBind();
        }

        protected void gridView_HiringRequisition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            //if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            //{
            //    DataRowView dataRowView = gridViewRow.DataItem as DataRowView;
            //}

            string wfStatus = (gridViewRow.FindControl("lblHRWFStatus") as Label).Text.Replace(" ", "");
            if (wfStatus != HRWFStatus.NotSubmitted.ToString())
            {
                CheckBox cbSelect = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                cbSelect.Checked = false;
                cbSelect.Enabled = false;

                //gridViewRow.Enabled = false;
                gridViewRow.Attributes.Add("nonactionable", "true");
            }

            //string serialNumber = getViewState_HiringRequisitionId();
            //if (!string.IsNullOrEmpty(serialNumber))
            //{
            //    string lblHRSerialNumber = (gridViewRow.FindControl("lblHRSerialNumber") as Label).Text;
            //    if (lblHRSerialNumber == serialNumber)
            //    {
            //        gridViewRow.Attributes.Add("background", "#f2f4f8");
            //    }
            //}


        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView_HiringRequisition.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                        //Array.Resize(ref recordsId, recordsId.Length + 1);
                        //recordsId[recordsId.Length - 1] = recId;
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = hRHiringRequisition.delete(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView_HiringRequisition.EditIndex = -1;
                    reBindGrid_HiringRequisition();
                }
                else
                {
                    bindGrid_HiringRequisition();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView_HiringRequisition.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string requestId = (gridViewRow.FindControl("lblHRSerialNumber") as Label).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.hRHiringRequisition_Submit(recordsId.ToArray());
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    gridView_HiringRequisition.EditIndex = -1;
                    reBindGrid_HiringRequisition();
                }
                else
                {
                    bindGrid_HiringRequisition();
                }
                if (result)
                {
                    gridView_HiringRequisition.EditIndex = -1;
                    bindGrid_HiringRequisition();

                }
            }

        }


        protected void Details_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                string serialNumber = SecureQueryString.encrypt(((Label)gridViewRow.FindControl("lblHRSerialNumber")).Text);
                //string wfStatus = SecureQueryString.encrypt(((Label)gridViewRow.FindControl("lblHRWFStatus")).Text); || string.IsNullOrEmpty(wfStatus)

                if (string.IsNullOrEmpty(serialNumber))
                {
                    NotificationMessage.showInvalidRecord();
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Hiring Requisition Create",
                    "javascript: openPopupPanel('/ESS/HR/ESSHRHiringRequisition_Create.aspx?SerialNumber=" + serialNumber + "' ,'980');", true);
            }
        }


        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

        #region ViewState
        public bool setViewState_HiringRequisitionId(string _serialNumber)
        {
            bool isStored = false;
            string serialNumber = _serialNumber;

            if (ViewState != null)
            {
                ViewState["HiringRequisition_serialNumber"] = serialNumber;
                isStored = true;
            }
            return isStored;
        }

        #endregion

    }
}