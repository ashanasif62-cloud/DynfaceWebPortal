using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.ESSBusinessTripSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class HRBusinessTrip
    {
        private readonly string serviceName = "ESSBusinessTripSvcGroup";
        public string tableName = "ESSBusinessTrip";
        public string actionItem = "Business Trip Request";

        public SysOperationResult_BOL create(DataTable _objDT)
        {
            DataTable dataTable = _objDT;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new ESSBusinessTripSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        long employeeId = 0;
                        Int64.TryParse(dataRow["EmpId"].ToString(), out employeeId);

                        ESSBusinessTripSvcContract eSSBusinessTripContract = new ESSBusinessTripSvcContract();

                        eSSBusinessTripContract.EmpId = employeeId;//- C
                        eSSBusinessTripContract.RequestedDate = dataRow["RequestedDate"].ToString().toDateTime();//- C

                        eSSBusinessTripContract.ReturnDate = dataRow["ReturnDate"].ToString().toDateTime();//- CU
                        eSSBusinessTripContract.DepartureDate = dataRow["DepartureDate"].ToString().toDateTime();//- CU

                        eSSBusinessTripContract.Reason = dataRow["Reason"].ToString();//- CU

                        //eSSBusinessTripContract.ExitReentryRequired = dataRow["ExitReentryRequired"] is DBNull ? ESSNoYes.No :
                        //                                                (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["ExitReentryRequired"].ToString());//- CU
                        eSSBusinessTripContract.FlightBookingRequired = dataRow["FlightBookingRequired"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["FlightBookingRequired"].ToString());//- CU
                        eSSBusinessTripContract.HotelBookingRequired = dataRow["HotelBookingRequired"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["HotelBookingRequired"].ToString());//- CU
                        //eSSBusinessTripContract.RequiredVisa = dataRow["RequiredVisa"] is DBNull ? ESSNoYes.No :
                        //                                                (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["RequiredVisa"].ToString());//- CU
                        //eSSBusinessTripContract.BusinessTripType = dataRow["BusinessTripType"] is DBNull ? ESSBusinessTripType.Domestic :
                        //                                                (ESSBusinessTripType)Enum.Parse(typeof(ESSBusinessTripType), dataRow["BusinessTripType"].ToString());//- CU
                        eSSBusinessTripContract.CarRentalBooking = dataRow["CarRentalBooking"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["CarRentalBooking"].ToString());//- CU

                        eSSBusinessTripContract.RequestedBy = SessionVariables.getCurrentEmployeeId();
                        
                        eSSBusinessTripContract.TicketRouting = dataRow["TicketRouting"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["TicketRouting"].ToString());
                        eSSBusinessTripContract.Meal = dataRow["Meal"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["Meal"].ToString());
                        eSSBusinessTripContract.NightStay = dataRow["NightStay"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["NightStay"].ToString());
                        eSSBusinessTripContract.PersonalCar = dataRow["PersonalCar"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["PersonalCar"].ToString());
                        eSSBusinessTripContract.FuelAndToll = dataRow["FuelAndToll"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["FuelAndToll"].ToString());
                        
                        eSSBusinessTripContract.BusinesClass = dataRow["BusinesClass"] is DBNull ? ESSBusinesClass.FirstClass :
                                                                        (ESSBusinesClass)Enum.Parse(typeof(ESSBusinesClass), dataRow["BusinesClass"].ToString());

                        eSSBusinessTripContract.DepartureCountry = dataRow["DepartureCountry"].ToString();
                        eSSBusinessTripContract.DepartureCity = dataRow["DepartureCity"].ToString();
                        eSSBusinessTripContract.DestinationCountry = dataRow["DestinationCountry"].ToString();
                        eSSBusinessTripContract.DestinationCity = dataRow["DestinationCity"].ToString();
                        //eSSBusinessTripContract.Department;
                        //eSSBusinessTripContract.Destination;
                        //eSSBusinessTripContract.EstimatedDistance;
                        //eSSBusinessTripContract.EventPeriod;
                        //eSSBusinessTripContract.Grade;
                        //eSSBusinessTripContract.HotelName;
                        ////eSSBusinessTripContract.HRGetVisa;
                        ////eSSBusinessTripContract.HRIssueBooking;
                        ////eSSBusinessTripContract.HRIssueExiteReentry;
                        //eSSBusinessTripContract.LineManager;
                        //eSSBusinessTripContract.TravelDays;
                        //eSSBusinessTripContract.VisaRequired;
                        //eSSBusinessTripContract.WorkFlowState;
                        
                        GeneralContract results = ((ESSBusinessTripSvc)channel).createAsync(new create(callContext, eSSBusinessTripContract)).Result.result;

                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);
                    }
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
            return objBOL;
        }

        public SysOperationResult_BOL update(DataTable _objDT, long _recId)
        {
            DataTable dataTable = _objDT;
            long recId = _recId;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();

            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();

                var client = new ESSBusinessTripSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        ESSBusinessTripSvcContract eSSBusinessTripContract = new ESSBusinessTripSvcContract();

                        eSSBusinessTripContract.ReturnDate = dataRow["ReturnDate"].ToString().toDateTime();//- CU
                        eSSBusinessTripContract.DepartureDate = dataRow["DepartureDate"].ToString().toDateTime();//- CU

                        eSSBusinessTripContract.Reason = dataRow["Reason"].ToString();//- CU

                        //eSSBusinessTripContract.ExitReentryRequired = dataRow["ExitReentryRequired"] is DBNull ? ESSNoYes.No :
                        //                                                (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["ExitReentryRequired"].ToString());//- CU
                        eSSBusinessTripContract.FlightBookingRequired = dataRow["FlightBookingRequired"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["FlightBookingRequired"].ToString());//- CU
                        eSSBusinessTripContract.HotelBookingRequired = dataRow["HotelBookingRequired"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["HotelBookingRequired"].ToString());//- CU
                        //eSSBusinessTripContract.RequiredVisa = dataRow["RequiredVisa"] is DBNull ? ESSNoYes.No :
                        //                                                (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["RequiredVisa"].ToString());//- CU
                        //eSSBusinessTripContract.BusinessTripType = dataRow["BusinessTripType"] is DBNull ? ESSBusinessTripType.Domestic :
                        //                                                (ESSBusinessTripType)Enum.Parse(typeof(ESSBusinessTripType), dataRow["BusinessTripType"].ToString());//- CU
                        eSSBusinessTripContract.CarRentalBooking = dataRow["CarRentalBooking"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["CarRentalBooking"].ToString());//- CU

                        eSSBusinessTripContract.RequestedBy = SessionVariables.getCurrentEmployeeId();
                        
                        eSSBusinessTripContract.TicketRouting = dataRow["TicketRouting"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["TicketRouting"].ToString());
                        eSSBusinessTripContract.Meal = dataRow["Meal"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["Meal"].ToString());
                        eSSBusinessTripContract.NightStay = dataRow["NightStay"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["NightStay"].ToString());
                        eSSBusinessTripContract.PersonalCar = dataRow["PersonalCar"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["PersonalCar"].ToString());
                        eSSBusinessTripContract.FuelAndToll = dataRow["FuelAndToll"] is DBNull ? ESSNoYes.No :
                                                                        (ESSNoYes)Enum.Parse(typeof(ESSNoYes), dataRow["FuelAndToll"].ToString());

                        eSSBusinessTripContract.BusinesClass = dataRow["BusinesClass"] is DBNull ? ESSBusinesClass.FirstClass :
                                                                        (ESSBusinesClass)Enum.Parse(typeof(ESSBusinesClass), dataRow["BusinesClass"].ToString());

                        eSSBusinessTripContract.DepartureCountry = dataRow["DepartureCountry"].ToString();
                        eSSBusinessTripContract.DepartureCity = dataRow["DepartureCity"].ToString();
                        eSSBusinessTripContract.DestinationCountry = dataRow["DestinationCountry"].ToString();
                        eSSBusinessTripContract.DestinationCity = dataRow["DestinationCity"].ToString();

                        eSSBusinessTripContract.RecId = recId;

                        GeneralContract results = ((ESSBusinessTripSvc)channel).updateAsync(new update(callContext, eSSBusinessTripContract)).Result.result;
                        objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Update, objBOL);
                    }
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
            return objBOL;
        }

        public SysOperationResult_BOL delete(long[] _recordsRecId)
        {
            long[] recordsRecId = _recordsRecId;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSBusinessTripSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract[] results = ((ESSBusinessTripSvc)channel).deleteAsync(new delete(callContext, recordsRecId)).Result.result;
                    objBOL = SysOperationResults.operationResults(results);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Delete, results);
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
            return objBOL;
        }

        public DataTable retrieveAll()
        {
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSBusinessTripSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSBusinessTripSvc)channel).retrieveAllAsync(new retrieveAll(callContext)).Result.result;

                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return createDataTable();
            }
            finally
            { }
        }

        public DataTable retriveEmployeeRequestioner()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSBusinessTripSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSBusinessTripSvc)channel).retriveEmployeeRequestionerAsync(new retriveEmployeeRequestioner(callContext, employeeId)).Result.result;

                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return createDataTable();
            }
            finally
            { }
        }

        public DataTable retriveEmployeeReportees()
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new ESSBusinessTripSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    var results = ((ESSBusinessTripSvc)channel).retriveEmployeeReporteesAsync(new retriveEmployeeReportees(callContext, employeeId)).Result.result;

                    DataTable dataTable = RetrieveDatatable.createDataTable(results);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return createDataTable();
            }
            finally
            { }
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new ESSBusinessTripSvcContract[] { });
            return dataTable;
        }

    }
}
