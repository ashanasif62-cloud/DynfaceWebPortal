using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration.DocumentAttachmentSvcReference;
using System;
using System.Data;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class DocumentAttachment
    {
        private readonly string serviceName = "DocumentAttachmentSvcGroup";
        public string tableName = "DocumentAttachment";
        public string actionItem = "Document Attachment Request";

        #region comment
        //public string addAttachment(string _base64Str, string _employeeId, string _fileExt, string _fileName, long _recordRefRecId, string _tableName)
        //{
        //    string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

        //    string tableName = _tableName;
        //    string base64Str = _base64Str;
        //    string employeeId = _employeeId;
        //    string fileExt = _fileExt;
        //    string fileName = _fileName;
        //    long recordRefRecId = _recordRefRecId;

        //    var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //    var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
        //    var endpointAddress = new EndpointAddress(serviceUriString);
        //    var binding = SoapHelper.GetBinding();
        //    var client = new DocumentAttachmentSvcClient(binding, endpointAddress);
        //    var channel = client.InnerChannel;

        //    using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //    {
        //        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //        requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //        CallContext callContext = new CallContext();
        //        callContext.MessageId = Guid.NewGuid().ToString();
        //        callContext.Company = dataAreaId;

        //        string results = ((DocumentAttachmentSvc)channel).AddAttachmentAsync(
        //            new AddAttachment(callContext, base64Str, employeeId, fileExt, fileName, recordRefRecId, tableName)).Result.result;
        //        return results;
        //    }
        //} 
        #endregion

        public SysOperationResult_BOL addAttachment(byte[] _fileByteArray, string _employeeId, string _fileExt, string _fileName, long _recordRefRecId, string _tableName)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                DocumentAttachmentSvcContract documentAttachmentSvcContract = new DocumentAttachmentSvcContract();
                documentAttachmentSvcContract.EmployeeId = _employeeId;
                documentAttachmentSvcContract.FileByteArray = _fileByteArray;
                documentAttachmentSvcContract.FileExt = _fileExt;
                documentAttachmentSvcContract.FileName = _fileName;
                documentAttachmentSvcContract.RecordRefRecId = _recordRefRecId;
                documentAttachmentSvcContract.TableName = _tableName;

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new DocumentAttachmentSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract results = ((DocumentAttachmentSvc)channel).saveAttachmentByteArrayAsync(
                        new saveAttachmentByteArray(callContext, documentAttachmentSvcContract)).Result.result;

                    objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Create, objBOL);
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

        public DataTable retrieveAttachments(long _recordRefRecId, string _tableName)
        {
            long recordRefRecId = _recordRefRecId;
            string tableName = _tableName;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new DocumentAttachmentSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    DocuRefContract[] results = ((DocumentAttachmentSvc)channel).retrieveAsync(
                                                new retrieve(callContext, recordRefRecId, tableName)).Result.result;
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
                return createDataTable_DocuRef();
            }
            finally
            { }
        }

        public MemoryStream retrieveAttachment(long _recId)
        {
            MemoryStream results = new MemoryStream();
            long recId = _recId;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new DocumentAttachmentSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    results = ((DocumentAttachmentSvc)channel).retrieveAttachmentAsync(
                        new retrieveAttachment(callContext, recId)).Result.result;
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
            return results;
        }

        public SysOperationResult_BOL deleteAttachment(long _recId)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            long recId = _recId;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new DocumentAttachmentSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    GeneralContract results = ((DocumentAttachmentSvc)channel).deleteRecordAsync(
                                                new deleteRecord(callContext, recId)).Result.result;
                    objBOL = SysOperationResults.operationResult<GeneralContract>(results);

                    SysLogUserActivity.logUserActivity(tableName, actionItem, ActionType.Delete, objBOL);
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

        public string getAttachmentURL(long _recId)
        {
            long recId = _recId;
            try
            {
                string dataAreaId = SessionVariables.getUserCurrentDataAreaId();

                var authenticationHeader = OAuthHelper.getAuthenticationHeader();
                var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);
                var endpointAddress = new EndpointAddress(serviceUriString);
                var binding = SoapHelper.GetBinding();
                var client = new DocumentAttachmentSvcClient(binding, endpointAddress);
                var channel = client.InnerChannel;

                using (OperationContextScope operationContextScope = new OperationContextScope(channel))
                {
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    CallContext callContext = new CallContext();
                    callContext.MessageId = Guid.NewGuid().ToString();
                    callContext.Company = dataAreaId;

                    string results = ((DocumentAttachmentSvc)channel).getAttachmentURLAsync(
                        new getAttachmentURL(callContext, recId)).Result.result;
                    return results;
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return string.Empty;
            }
            finally
            { }
        }

        public DataTable createDataTable_DocuRef()
        {
            DataTable dataTable = new DataTable();
            dataTable = RetrieveDatatable.createDataTable(new DocuRefContract[] { });
            return dataTable;
        }


    }
}
