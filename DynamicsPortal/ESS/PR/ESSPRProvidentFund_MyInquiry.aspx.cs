using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Data;

namespace DynamicsPortal
{
    public partial class ESSPRProvidentFund_MyInquiry : MainForm
    {
        private PRProvidentFundInquiry pRProvidentFundInquiry = new PRProvidentFundInquiry();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSPRProvidentFund_MyInquiry";
                showActionPanel = false;

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    reBindGrid();
                    bindEmployeeItemAmount();
                    bindEmployeeAmount();
                    bindEmployerAmount();
                    bindTotalAmount();
                    bindRecoveryAmount();
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

        //private void bindEmployeeItemAmount()
        //{
        //    DataTable dt = SessionVariables.getSessionDataTable();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        txtEmployeeItemAmount.Text = dt.Rows[0]["ItemAmount"].ToString();
        //    }
        //}
        //private void bindEmployeeItemAmount()
        //{
        //    DataTable dt = SessionVariables.getSessionDataTable();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        txtEmployeeItemAmount.Text = dt.Rows[0]["ItemAmount"].ToString();
        //    }
        //}

        //private void bindEmployeeAmount()
        //{
        //    DataTable dt = SessionVariables.getSessionDataTable();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        decimal employeeAmount = 0;
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            decimal.TryParse(row["EmployeeAmount"].ToString(), out decimal amount);
        //            employeeAmount += amount;
        //        }
        //        txtEmployeeAmount.Text = employeeAmount.ToString();
        //    }
        //}

        //private void bindEmployerAmount()
        //{
        //    DataTable dt = SessionVariables.getSessionDataTable();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        decimal employerAmount = 0;
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            decimal.TryParse(row["EmployerAmount"].ToString(), out decimal amount);
        //            employerAmount += amount;
        //        }
        //        txtEmployerAmount.Text = employerAmount.ToString();
        //    }
        //}

        //private void bindTotalAmount()
        //{
        //    DataTable dt = SessionVariables.getSessionDataTable();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        decimal employeeAmount = 0;
        //        decimal employerAmount = 0;
        //        decimal pfProfitAmount = 0;

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            decimal.TryParse(row["EmployeeAmount"].ToString(), out decimal empAmount);
        //            employeeAmount += empAmount;

        //            decimal.TryParse(row["EmployerAmount"].ToString(), out decimal emplAmount);
        //            employerAmount += emplAmount;

        //            decimal.TryParse(row["ItemAmount"].ToString(), out decimal pfAmount);
        //            pfProfitAmount = pfAmount;
        //        }

        //        decimal totalAmount = employeeAmount + employerAmount + pfProfitAmount;
        //        txtTotalAmount.Text = totalAmount.ToString();
        //    }
        //}
        private void bindRecoveryAmount()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (decimal.TryParse(dt.Rows[0]["RecoveryAmount"].ToString(), out decimal recoveryAmount))
                {
                    txtRecoveryAmount.Text = recoveryAmount.ToString("N2");
                }
                else
                {
                    txtRecoveryAmount.Text = "0.00";
                }
            }
        }

        private void bindEmployeeItemAmount()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (decimal.TryParse(dt.Rows[0]["ItemAmount"].ToString(), out decimal itemAmount))
                {
                    txtEmployeeItemAmount.Text = itemAmount.ToString("N2");
                }
                else
                {
                    txtEmployeeItemAmount.Text = "0.00";
                }
            }
        }

        private void bindEmployeeAmount()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                decimal employeeAmount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal.TryParse(row["EmployeeAmount"].ToString(), out decimal amount);
                    employeeAmount += amount;
                }
                txtEmployeeAmount.Text = employeeAmount.ToString("N2");
            }
        }

        private void bindEmployerAmount()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                decimal employerAmount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal.TryParse(row["EmployerAmount"].ToString(), out decimal amount);
                    employerAmount += amount;
                }
                txtEmployerAmount.Text = employerAmount.ToString("N2");
            }
        }

        private void bindTotalAmount()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                decimal employeeAmount = 0;
                decimal employerAmount = 0;
                decimal pfProfitAmount = 0;
                decimal recoveryamount = 0;

                foreach (DataRow row in dt.Rows)
                {
                    decimal.TryParse(row["EmployeeAmount"].ToString(), out decimal empAmount);
                    employeeAmount += empAmount;

                    decimal.TryParse(row["EmployerAmount"].ToString(), out decimal emplAmount);
                    employerAmount += emplAmount;

                    decimal.TryParse(row["ItemAmount"].ToString(), out decimal pfAmount);
                    pfProfitAmount = pfAmount;
                    decimal.TryParse(row["RECOVERYAMOUNT"].ToString(), out decimal RECOVERYAMOUNT);
                    recoveryamount = RECOVERYAMOUNT;
                }

                decimal totalAmount = (employeeAmount + employerAmount + pfProfitAmount ) - recoveryamount;
                txtTotalAmount.Text = totalAmount.ToString("N2");
            }
        }



        private void getGridDataTable()
        {
            DataTable dt = pRProvidentFundInquiry.retrieveAllPRProvidentFundInquiry(false);
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