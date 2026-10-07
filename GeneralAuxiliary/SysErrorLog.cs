//using System;
//using System.IO;
//using System.Reflection;

//namespace GeneralAuxiliary
//{
//    public class SysErrorLog
//    {
//        public void write(string _methodName, string _exceptionMessage, string _exceptionInnerMessage)
//        {
//            string methodName = _methodName;
//            string exceptionMessage = _exceptionMessage;
//            string exceptionInnerMessage = _exceptionInnerMessage;
//            string userId = SessionVariables.getCurrentUserId();

//            var requiredPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)));
//            requiredPath = requiredPath.Replace("file:\\", "");

//            string filePath = requiredPath + "\\DynamicsPortal\\ErrorLog.log";

//            using (StreamWriter writer = new StreamWriter(filePath, true))
//            {
//                writer.WriteLine("UserId: " + userId +
//                                 Environment.NewLine + "Occurrence: " + methodName +
//                                 Environment.NewLine + "Message: " + exceptionMessage +
//                                 Environment.NewLine + "InnerMessage: " + exceptionInnerMessage +
//                                 Environment.NewLine + "Date: " + DateTime.Now.ToString());
//                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);

//                exceptionMessage = checkMessageContent(exceptionMessage);
//                AlertType alertType = AlertType.Error;
//                NotificationMessage.showMessage(alertType, exceptionMessage);
//            }


//        }

//        public void write(string _methodName, Exception _ex)
//        {
//            try
//            {
//                string methodName = _methodName;
//                Exception ex = _ex;
//                string exceptionMessage = string.Empty;
//                string exceptionInnerMessage = string.Empty;

//                if (ex != null)
//                {
//                    exceptionMessage = ex.Message != null ? ex.Message : string.Empty;
//                    exceptionInnerMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
//                }
//                else
//                {
//                    exceptionMessage = "Error found - no details.";
//                }

//                write(methodName, exceptionMessage, exceptionInnerMessage);
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
//                string currentMethodName = currentMethod.DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//            finally
//            { }
//        }

//        private string checkMessageContent(string _message)
//        {
//            string message = _message;
//            if (message.Contains("Failed to authenticate with AAD by the credential.") || message.Contains("There was no endpoint listening") || message.Contains("caused by an incorrect address or SOAP action") || message.Contains("dynamics.com/soap/services/"))
//            {
//                message = "An error occurred while establishing a connection to Dynamics AX services.";

//            }
//            else if (message.Contains("error occurred while establishing a connection to SQL Server") || message.Contains("The server was not found or was not accessible") || message.Contains("error: 26 - Error Locating Server/Instance Specified"))
//            {

//                message = "An error occurred while establishing a connection to SQL Server.";
//            }

//            return message;

//        }

//        private static void LogException(Exception exc, string source)
//        {
//            string logFile = "App_Data/ErrorLog.txt";
//            logFile = System.Web.HttpContext.Current.Server.MapPath(logFile);

//            // Open the log file for append and write the log
//            StreamWriter sw = new StreamWriter(logFile, true);
//            sw.WriteLine("********** {0} **********", DateTime.Now);
//            if (exc.InnerException != null)
//            {
//                sw.Write("Inner Exception Type: ");
//                sw.WriteLine(exc.InnerException.GetType().ToString());
//                sw.Write("Inner Exception: ");
//                sw.WriteLine(exc.InnerException.Message);
//                sw.Write("Inner Source: ");
//                sw.WriteLine(exc.InnerException.Source);
//                if (exc.InnerException.StackTrace != null)
//                {
//                    sw.WriteLine("Inner Stack Trace: ");
//                    sw.WriteLine(exc.InnerException.StackTrace);
//                }
//            }
//            sw.Write("Exception Type: ");
//            sw.WriteLine(exc.GetType().ToString());
//            sw.WriteLine("Exception: " + exc.Message);
//            sw.WriteLine("Source: " + source);
//            sw.WriteLine("Stack Trace: ");
//            if (exc.StackTrace != null)
//            {
//                sw.WriteLine(exc.StackTrace);
//                sw.WriteLine();
//            }
//            sw.Close();
//        }

//    }
//}
using System;
using System.IO;
using System.Reflection;

namespace GeneralAuxiliary
{
    public class SysErrorLog
    {
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string LogFilePath = Path.Combine(LogDirectory, "ErrorLog.log");

        public SysErrorLog()
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        public void write(string _methodName, string _exceptionMessage, string _exceptionInnerMessage)
        {
            try
            {
                string methodName = _methodName;
                string exceptionMessage = _exceptionMessage ?? "No message";
                string exceptionInnerMessage = _exceptionInnerMessage ?? "No inner message";
                string userId = SessionVariables.getCurrentUserId() ?? "Unknown";

                using (StreamWriter writer = File.AppendText(LogFilePath))
                {
                    writer.WriteLine($"UserId: {userId}");
                    writer.WriteLine($"Occurrence: {methodName}");
                    writer.WriteLine($"Message: {exceptionMessage}");
                    writer.WriteLine($"InnerMessage: {exceptionInnerMessage}");
                    writer.WriteLine($"Date: {DateTime.Now}");
                    writer.WriteLine(new string('-', 80));
                }

                string processedMessage = checkMessageContent(exceptionMessage);
                string processedInnerMessage = checkMessageContent(exceptionInnerMessage);

                // If checkMessageContent returns null, it means the error should be silenced (hidden from UI)
                if (processedMessage == null || processedInnerMessage == null)
                {
                    return;
                }

                AlertType alertType = AlertType.Error;
                if (string.IsNullOrEmpty(exceptionInnerMessage))
                    NotificationMessage.showMessage(alertType, processedMessage);
                else
                    NotificationMessage.showMessage(alertType, processedInnerMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while logging: {ex.Message}");
            }
        }

        public void write(string _methodName, Exception _ex)
        {
            try
            {
                string methodName = _methodName;
                string exceptionMessage = _ex?.Message ?? "Error found - no details.";
                string exceptionInnerMessage = _ex?.InnerException?.Message ?? string.Empty;

                write(methodName, exceptionMessage, exceptionInnerMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while logging: {ex.Message}");
            }
        }

        private string checkMessageContent(string _message)
        {
            if (string.IsNullOrEmpty(_message)) return "Unknown error occurred.";

            if (_message.Contains("HRWorkflowOperation") || _message.Contains("ESSWorkerBankAccountSvcReference"))
            {
                return null; // Return null to indicate that this message should be suppressed from the UI
            }

            if (_message.Contains("Failed to authenticate with AAD") ||
                _message.Contains("There was no endpoint listening") ||
                _message.Contains("caused by an incorrect address or SOAP action") ||
                _message.Contains("dynamics.com/soap/services/"))
            {
                return "An error occurred while establishing a connection to Dynamics AX services.";
            }
            else if (_message.Contains("error occurred while establishing a connection to SQL Server") ||
                     _message.Contains("The server was not found or was not accessible") ||
                     _message.Contains("error: 26 - Error Locating Server/Instance Specified"))
            {
                return "An error occurred while establishing a connection to SQL Server.";
            }

            return _message;
        }

        private static void LogException(Exception exc, string source)
        {
            try
            {
                string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "ErrorLog.txt");

                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine($"********** {DateTime.Now} **********");
                    sw.WriteLine($"Source: {source}");

                    if (exc.InnerException != null)
                    {
                        sw.WriteLine($"Inner Exception Type: {exc.InnerException.GetType()}");
                        sw.WriteLine($"Inner Exception: {exc.InnerException.Message}");
                        sw.WriteLine($"Inner Source: {exc.InnerException.Source}");
                        if (exc.InnerException.StackTrace != null)
                        {
                            sw.WriteLine("Inner Stack Trace:");
                            sw.WriteLine(exc.InnerException.StackTrace);
                        }
                    }

                    sw.WriteLine($"Exception Type: {exc.GetType()}");
                    sw.WriteLine($"Exception: {exc.Message}");
                    sw.WriteLine("Stack Trace:");
                    if (exc.StackTrace != null)
                    {
                        sw.WriteLine(exc.StackTrace);
                    }
                    sw.WriteLine(new string('-', 80));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error while logging: {ex.Message}");
            }
        }
    }
}
