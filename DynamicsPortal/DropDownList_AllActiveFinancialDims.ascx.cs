using PortalIntegration;
using PortalIntegration.ESSFinancialDimensionsSvcReference;
using PortalIntegration.PREmploymentInformationSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class DropDownList_AllActiveFinancialDims : System.Web.UI.UserControl
    {
        //public event EventHandler EmployeeSelected = delegate { };
        private string employeeId;
        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }

        private long employeeDimension;

        public long EmployeeDimension
        {
            get { return employeeDimension = getEmployeeDimension(); }
            set { employeeDimension = value; }
        }

        private DataContract[] employeeDimensions;

        public DataContract[] EmployeeDimensions
        {
            get { return employeeDimensions = getEmployeeDimensions(); }
            set { employeeDimensions = value; }
        }

        private ESSFinancialDimensions financialDimensions = new ESSFinancialDimensions();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

            }
            createControls();
        }

        #region Create and raise Event From Child
        /// <summary>
        /// Child Code
        /// </summary>
        ////public event EventHandler ChildPage_Load;
        ////ChildPage_Load_Click(sender, e);
        ////protected virtual void OnChildPage_Load(EventArgs e)
        ////{
        ////    var handler = ChildPage_Load;
        ////    if (handler != null)
        ////        handler(this, e);
        ////}
        ////private void ChildPage_Load_Click(object sender, EventArgs e)
        ////{
        ////    //While you can call `this.ParentForm.Close()` it's better to raise an event
        ////    //OnChildPage_Load(e);
        ////    createControls();
        ////}
        ///

        /// <summary>
        /// Parent Code
        /// </summary>
        ////cddl_AllActiveFinancialDims.ChildPage_Load += userControl11_CloseButtonClicked;            
        ////    //Close the form when you received the notification
        ////   private void userControl11_CloseButtonClicked(object sender, EventArgs e)
        ////    { } 

        #endregion

        private void createControls()
        {

            DataContract[] activeDimensContract = financialDimensions.retrieveActiveDimensions();

            foreach (DataContract dataContract in activeDimensContract)
            {
                #region Attributes
                string dimName = dataContract.Code;
                string dimValue = dataContract.Value1;
                string dimDescription = dataContract.Value2;
                #endregion

                HtmlGenericControl divControlsGroup = new HtmlGenericControl("div");
                divControlsGroup.Attributes.Add("class", "control-group");

                HtmlGenericControl labelControl = new HtmlGenericControl("span");
                labelControl.InnerText = dimName;
                labelControl.Attributes.Add("class", "control-label");

                HtmlGenericControl divDataContol = new HtmlGenericControl("div");
                divDataContol.Style.Add("display", "inline-block");

                DataTable dataTable = financialDimensions.retrieveDimensionLookUp(dimName);

                DropDownList ddlDimControls = new DropDownList();
                ddlDimControls.ID = "ddl" + dimName;
                ddlDimControls.AutoPostBack = false;
                ddlDimControls.DataSource = dataTable;
                ddlDimControls.DataValueField = "Value1";
                ddlDimControls.DataTextField = "Value2";
                ddlDimControls.DataBind();
                ddlDimControls.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlDimControls.SelectedIndex = 0;

                divDataContol.Controls.Add(ddlDimControls);
                divControlsGroup.Controls.Add(labelControl);
                divControlsGroup.Controls.Add(divDataContol);
                divFinancialDim.Controls.Add(divControlsGroup);
            }


            if (!string.IsNullOrEmpty(EmployeeId))
            {
                bindEmployeeData();
            }
        }

        private void bindEmployeeData()
        {
            PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();
            PREmploymentInformationSvcContract pREmploymentInformationSvcContract = pREmploymentInformation.retrieveEmployeeDetails(EmployeeId);

            long empDimension = pREmploymentInformationSvcContract.Dimension;
            DataContract[] dataContracts = financialDimensions.retrieveDimensionValues(empDimension);
            foreach (DataContract dataContract in dataContracts)
            {
                string dimName = dataContract.Code;
                string dimValue = dataContract.Value1;
                string dimDescription = dataContract.Value2;

                DropDownList ddl = divFinancialDim.FindControl("ddl" + dimName) as DropDownList;
                if (ddl != null)
                {
                    ddl.SelectedValue = dimValue;
                }

            }

        }

        private long getEmployeeDimension()
        {
            DataContract[] dataContracts = getEmployeeDimensions();
            long employeeDim = financialDimensions.setDimensionValues(dataContracts);
            return employeeDim;
        }
        private DataContract[] getEmployeeDimensions()
        {
            List<DataContract> dataContracts = new List<DataContract>();

            List<DropDownList> items = ControlsHelper.FindControls<DropDownList>(divFinancialDim);
            foreach (DropDownList controls in items)
            {
                DataContract dataContract = new DataContract();
                DropDownList dropDownList = controls as DropDownList;
                dataContract.Code = dropDownList.ID;
                dataContract.Value1 = dropDownList.SelectedValue;
                dataContract.Value2 = dropDownList.SelectedItem.Text;

                dataContracts.Add(dataContract);
            }
            return dataContracts.ToArray();
        }
    }
}