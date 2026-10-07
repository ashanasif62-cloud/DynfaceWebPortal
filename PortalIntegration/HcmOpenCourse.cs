using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.HcmOpenCourseSvcReference;
using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace PortalIntegration
{
    public class HcmOpenCourse
    {
        private readonly string serviceName = "HcmOpenCourseSvcGroup";
        public string tableName = "HRMCourseTable";
        public string actionItem = "Register Course";

        public SysOperationResult_BOL registerCourses(string _employeeId, long[] _coursesRecId)
        {
            long[] coursesRecId = _coursesRecId;
            string employeeId = _employeeId;
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            //try
            //{
            //    string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

            //    var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            //    var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

            //    var endpointAddress = new EndpointAddress(serviceUriString);
            //    var binding = SoapHelper.GetBinding();

            //    var client = new HcmOpenCourseSvcClient(binding, endpointAddress);
            //    var channel = client.InnerChannel;

            //    using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            //    {
            //        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            //        requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
            //        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

            //        CallContext callContext = new CallContext();
            //        callContext.MessageId = Guid.NewGuid().ToString();
            //        callContext.Company = dataAreaId;
                    
            //        GeneralContract[] results = ((HcmOpenCourseSvc)channel).registerCoursesAsync(new registerCourses(callContext, coursesRecId, employeeId)).Result.result;
            //        objBOL = SysOperationResults.operationResults(results);

            //        SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, results);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SysErrorLog objErrorLog = new SysErrorLog();
            //    System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
            //    string currentMethodName = currentMethod.DeclaringType.FullName;
            //    objErrorLog.write(currentMethodName, ex);
            //}
            //finally
            //{ }
            return objBOL;
        }

        public DataTable retrieveOpenCoursesForEmployee(string _employeeId)
        {
            string employeeId = _employeeId;
            //try
            //{
            //    string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

            //    var authenticationHeader = OAuthHelper.getAuthenticationHeader();
            //    var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
            //    var endpointAddress = new EndpointAddress(serviceUriString);
            //    var binding = SoapHelper.GetBinding();
            //    var client = new HcmOpenCourseSvcClient(binding, endpointAddress);
            //    var channel = client.InnerChannel;

            //    using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            //    {
            //        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            //        requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
            //        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

            //        CallContext callContext = new CallContext();
            //        callContext.MessageId = Guid.NewGuid().ToString();
            //        callContext.Company = dataAreaId;

            //        var results = ((HcmOpenCourseSvc)channel).reteriveOpenCoursesForEmployeeAsync(new reteriveOpenCoursesForEmployee(callContext, employeeId)).Result.result;

            //        DataTable dataTable = RetrieveDatatable.createDataTable(results);
            //        return dataTable;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SysErrorLog objErrorLog = new SysErrorLog();
            //    System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
            //    string currentMethodName = currentMethod.DeclaringType.FullName;
            //    objErrorLog.write(currentMethodName, ex);
            //    return createDataTable();
            //}
            //finally
            //{ }
            return createDataTable();
        }

        public DataTable createDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new HcmOpenCourseSvcContract[] { });
            return dataTable;
        }
    }
}
