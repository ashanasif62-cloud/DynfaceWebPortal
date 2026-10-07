using System;
using System.Collections;
using System.Xml;

namespace GeneralAuxiliary
{
    public class GetResults
    {
        //"Something went wrong.";
        //"Requested Action Successfully Performed";
        private static XmlDocument getXMLMessageFile(string _xml)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                string xml = _xml;

                if (!string.IsNullOrEmpty(xml))
                    xmlDocument.LoadXml(xml);
                else
                    xmlDocument = null;

                return xmlDocument;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return null;
            }
            finally
            { }
        }

        public static string createOperationResults(long _resultId, string _message = null)
        {
            long resultId = _resultId;
            string message = _message;
            bool result = false;
            string type;

            if (resultId > 0)
            {
                message = "Record Successfully Created.";
                type = AlertType.Success.ToString();
                result = true;
            }
            else
                type = AlertType.Error.ToString();

            string xmlMsg = xmlMessage(resultId, message, type, result);
            return xmlMsg;
        }
        public static string updateOperationResults(long _resultId, string _message = null)
        {
            long resultId = _resultId;
            string message = _message;
            bool result = false;
            string type;

            if (resultId != 0)
            {
                message = "Record Successfully Updated.";
                type = AlertType.Success.ToString();
                result = true;
            }
            else
                type = AlertType.Error.ToString();

            string xmlMsg = xmlMessage(resultId, message, type, result);
            return xmlMsg;
        }
        public static string deleteOperationResults(long _resultId, string _message = null)
        {
            long resultId = _resultId;
            string message = _message;
            bool result = false;
            string type;

            if (resultId != 0)
            {
                message = "Record(s) Successfully Deleted.";
                type = AlertType.Success.ToString();
                result = true;
            }
            else
                type = AlertType.Error.ToString();

            string xmlMsg = xmlMessage(resultId, message, type, result);
            return xmlMsg;
        }
        public static string xmlErrorMessage(string _message)
        {
            int resultId = 0;
            string message = _message;
            bool result = false;
            string type = AlertType.Error.ToString();

            string xmlMsg = xmlMessage(resultId, message, type, result);
            return xmlMsg;
        }

        public static string delegateDefaultMessage()
        {
            int resultId = 0;
            string message = "Unable to Verify Requested Action.";
            bool result = false;
            string type = AlertType.Error.ToString();

            string xmlMsg = xmlMessage(resultId, message, type, result);
            return xmlMsg;
        }
        public static string incompleteDataMessage()
        {
            int resultId = 0;
            string message = "Required Information is missing.";
            bool result = false;
            string type = AlertType.Warning.ToString();

            string xmlMsg = xmlMessage(resultId, message, type, result);
            return xmlMsg;
        }

        public static string inactivePayGroupMessage()
        {
            int resultId = 0;
            string message = "Pay Group Doesnot have an active Pay Period.";
            bool result = false;
            string type = AlertType.Error.ToString();

            string xmlMsg = xmlMessage(resultId, message, type, result);
            return xmlMsg;
        }
        public static string inactiveEmployeeMessage()
        {
            int resultId = 0;
            string message = "Employee is Not Active.";
            bool result = false;
            string type = AlertType.Error.ToString();

            string xmlMsg = xmlMessage(resultId, message, type, result);
            return xmlMsg;
        }
        public static string invalidEmployeeDataMessage()
        {
            int resultId = 0;
            string message = "Unable to Validate Employee.";
            bool result = false;
            string type = AlertType.Error.ToString();

            string xmlMsg = xmlMessage(resultId, message, type, result);
            return xmlMsg;
        }
        public static string xmlMessage(long _resultId, string _message, string _type, bool _result)
        {
            long resultId = _resultId;
            string message = _message;
            string type = _type;
            bool result = _result;

            string xmlMessage = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                                "<XML>" +
                                    "<ResultId	Value=\"" + resultId + "\" />" +
                                    "<Message	Value=\"" + message + "\" />" +
                                    "<Type		Value=\"" + type + "\" />" +
                                    "<Result	Value=\"" + result + "\" />" +
                                "</XML>";
            return xmlMessage;
        }


        public static Hashtable getResultAttributes(string _xml)
        {
            try
            {
                if (!string.IsNullOrEmpty(_xml))
                {
                    Hashtable hTable = new Hashtable();
                    XmlDocument xmlDocument = getXMLMessageFile(_xml);

                    XmlNodeList xmlNodes = xmlDocument.DocumentElement.ChildNodes;

                    foreach (XmlNode xmlNode in xmlNodes)
                    {
                        XmlAttributeCollection attributes = xmlNode.Attributes;
                        foreach (XmlAttribute fieldAttribute in attributes)
                            hTable.Add(xmlNode.Name, fieldAttribute.Value);
                    }

                    return hTable;
                }
                return null;

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return null;
            }
            finally
            { }
        }

        public static string counterUpdateMessage(int _counter)
        {
            int counter = _counter;
            string message = string.Empty;
            string type = string.Empty;
            string xmlMsg;

            if (counter > 0)
            {
                message = counter + " record(s) updated successfuly.";
                type = AlertType.Success.ToString();
                xmlMsg = xmlMessage(counter, message, type, true);
            }
            else
            {
                message = "Fail to update record(s).";
                type = AlertType.Error.ToString();
                xmlMsg = xmlMessage(counter, message, type, false);
            }
            return xmlMsg;
        }


    }
}