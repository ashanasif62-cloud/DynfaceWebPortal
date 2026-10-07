using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSBusinessTripSvcReference;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHRBusinessTrip_Create : ModalForm
    {
        private HRBusinessTrip hRBusinessTrip = new HRBusinessTrip();
        private LogisticsAddressCity logisticsAddressCity = new LogisticsAddressCity();
        private LogisticsAddressCountryRegion logisticsAddressCountryRegion = new LogisticsAddressCountryRegion();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSHRBusinessTripRequest";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindControlsData();
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
        private void bindControlsData()
        {
            //ddlBusinessTripType.DataSource = Enum.GetNames(typeof(ESSBusinessTripType));
            //ddlBusinessTripType.DataBind();

            ddlTicketRouting.DataSource = Enum.GetNames(typeof(ESSNoYes));
            ddlTicketRouting.DataBind();

            ddlMeal.DataSource = Enum.GetNames(typeof(ESSNoYes));
            ddlMeal.DataBind();

            ddlNightStay.DataSource = Enum.GetNames(typeof(ESSNoYes));
            ddlNightStay.DataBind();

            ddlPersonalCar.DataSource = Enum.GetNames(typeof(ESSNoYes));
            ddlPersonalCar.DataBind();

            ddlFuelAndToll.DataSource = Enum.GetNames(typeof(ESSNoYes));
            ddlFuelAndToll.DataBind();

            ddlBusinesClass.DataSource = Enum.GetNames(typeof(ESSBusinesClass));
            ddlBusinesClass.DataBind();

            ddlHotelBookingRequired.DataSource = Enum.GetNames(typeof(ESSNoYes));
            ddlHotelBookingRequired.DataBind();

            ddlFlightBookingRequired.DataSource = Enum.GetNames(typeof(ESSNoYes));
            ddlFlightBookingRequired.DataBind();

            ddlCarRentalBooking.DataSource = Enum.GetNames(typeof(ESSNoYes));
            ddlCarRentalBooking.DataBind();

            DataTable dtLogisticsAddressCountryRegion = logisticsAddressCountryRegion.retrieveAll();
            ddlDepartureCountry.DataSource = dtLogisticsAddressCountryRegion;
            ddlDepartureCountry.DataTextField = "Name";
            ddlDepartureCountry.DataValueField = "CountryRegionId";
            ddlDepartureCountry.DataBind();
            ddlDepartureCountry.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            ddlDepartureCountry.SelectedIndex = 0;

            ddlDestinationCountry.DataSource = dtLogisticsAddressCountryRegion;
            ddlDestinationCountry.DataTextField = "Name";
            ddlDestinationCountry.DataValueField = "CountryRegionId";
            ddlDestinationCountry.DataBind();
            ddlDestinationCountry.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            ddlDestinationCountry.SelectedIndex = 0;

            txtRequestedDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtRequestedDate.Enabled = false;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            createRequest();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Submit_Click(object sender, EventArgs e)
        {
            create_SubmitRequest();
        }

        private void createRequest()
        {
            create_SubmitRequest(false);
        }

        private void create_SubmitRequest(bool _submitRequest = true)
        {
            long requestRecId = 0;
            bool submitRequest = _submitRequest;
            SysOperationResult_BOL createResult = new SysOperationResult_BOL();
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

            #region CreateRequest

            DataTable dataTable = hRBusinessTrip.createDataTable();
            DataRow dr = dataTable.NewRow();

            string personalNumber = (cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox).Text;
            long employeeId = ControlsHelper.getWorkerId(personalNumber);

            dr["EmpId"] = employeeId;
            dr["RequestedDate"] = txtRequestedDate.Text;
            dr["ReturnDate"] = txtReturnDate.Text;
            dr["Reason"] = txtReasonCode.Text;
            dr["DepartureDate"] = txtDepartureDate.Text;

            dr["FlightBookingRequired"] = ddlFlightBookingRequired.SelectedValue;
            dr["HotelBookingRequired"] = ddlHotelBookingRequired.SelectedValue;
            dr["CarRentalBooking"] = ddlCarRentalBooking.SelectedValue;
            dr["TicketRouting"] = ddlTicketRouting.SelectedValue;
            dr["Meal"] = ddlMeal.SelectedValue;
            dr["NightStay"] = ddlNightStay.SelectedValue;
            dr["PersonalCar"] = ddlPersonalCar.SelectedValue;
            dr["FuelAndToll"] = ddlFuelAndToll.SelectedValue;
            dr["BusinesClass"] = ddlBusinesClass.SelectedValue;
            dr["DepartureCountry"] = ddlDepartureCountry.SelectedValue;
            dr["DepartureCity"] = ddlDepartureCity.SelectedValue;
            dr["DestinationCountry"] = ddlDestinationCountry.SelectedValue;
            dr["DestinationCity"] = ddlDestinationCity.SelectedValue;

            dataTable.Rows.Add(dr);

            #endregion

            createResult = hRBusinessTrip.create(dataTable);
            requestRecId = createResult.RecId;

            if (createResult.isSuccess && submitRequest)
            {
                if (requestRecId > 0)
                {
                    submitResult = submitWFRequest(requestRecId);
                    if (submitResult.isSuccess)
                    {
                        submitResult.Message = " Request successfully submitted.";
                    }
                    else
                    {
                        submitResult.Message = " Failed to submit the request.";
                    }
                }
                else
                {
                    submitResult.AlertType = AlertType.Error.ToString();
                    submitResult.isSuccess = false;
                    submitResult.Message = " Failed to submit the created request.";
                }
                createResult.Message += " " + submitResult.Message;
                createResult.AlertType = submitResult.AlertType;
                createResult.isSuccess = submitResult.isSuccess;
            }

            bool result = operationResults(createResult, true);
        }

        private SysOperationResult_BOL submitWFRequest(long _requestRecId)
        {
            long requestRecId = _requestRecId;
            ESSWorkflow eSSWorkflow = new ESSWorkflow();
            SysOperationResult_BOL operationResult_BOL = new SysOperationResult_BOL();

            if (requestRecId > 0)
            {
                operationResult_BOL = eSSWorkflow.hRBusinessTripRequestByRecId_Submit(requestRecId);
            }
            return operationResult_BOL;
        }

        protected void ddlDepartureCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlDepartureCountry.SelectedValue))
            {
                DataTable dtlogisticsAddressCity = logisticsAddressCity.retrieveByCountry(ddlDepartureCountry.SelectedValue);
                ddlDepartureCity.DataSource = dtlogisticsAddressCity;
                ddlDepartureCity.DataTextField = "Name";
                ddlDepartureCity.DataValueField = "Name";
                ddlDepartureCity.DataBind();
                ddlDepartureCity.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlDepartureCity.SelectedIndex = 0;
            }
        }

        protected void ddlDestinationCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlDestinationCountry.SelectedValue))
            {
                DataTable dtlogisticsAddressCity = logisticsAddressCity.retrieveByCountry(ddlDestinationCountry.SelectedValue);
                ddlDestinationCity.DataSource = dtlogisticsAddressCity;
                ddlDestinationCity.DataTextField = "Name";
                ddlDestinationCity.DataValueField = "Name";
                ddlDestinationCity.DataBind();
                ddlDestinationCity.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlDestinationCity.SelectedIndex = 0;
            }
        }
    }
}
