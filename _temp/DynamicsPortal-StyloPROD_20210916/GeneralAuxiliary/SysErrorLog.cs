using System;
using System.IO;
using System.Reflection;

namespace GeneralAuxiliary
{
    public class SysErrorLog
    {
        public void write(string _methodName, string _exceptionMessage, string _exceptionInnerMessage)
        {
            string methodName = _methodName;
            string exceptionMessage = _exceptionMessage;
            string exceptionInnerMessage = _exceptionInnerMessage;
            string userId = SessionVariables.getCurrentUserId();

            var requiredPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)));
            requiredPath = requiredPath.Replace("file:\\", "");

            string filePath = requiredPath + "\\DynamicsPortal\\ErrorLog.log";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("UserId: " + userId +
                                 Environment.NewLine + "Occurrence: " + methodName +
                                 Environment.NewLine + "Message: " + exceptionMessage +
                                 Environment.NewLine + "InnerMessage: " + exceptionInnerMessage +
                                 Environment.NewLine + "Date: " + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);

                exceptionMessage = checkMessageContent(exceptionMessage);
                AlertType alertType = AlertType.Error;
                NotificationMessage.showMessage(alertType, exceptionMessage);
            }
            #region commented
            //User
            //string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;

            //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            //System.Diagnostics.StackFrame sf = st.GetFrame(0);
            //System.Reflection.MethodBase currentMethodName = sf.GetMethod();
            //OR
            //string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            //string tempfile = Path.GetTempFileName();
            //using (var writer = new StreamWriter(tempfile))
            //using (var reader = new StreamReader(filename))
            //{
            //    writer.WriteLine("A,B,C");
            //    while (!reader.EndOfStream)
            //        writer.WriteLine(reader.ReadLine());
            //}
            //File.Copy(tempfile, filename, true); 
            #endregion

        }

        public void write(string _methodName, Exception _ex)
        {
            try
            {
                string methodName = _methodName;
                Exception ex = _ex;
                string exceptionMessage = string.Empty;
                string exceptionInnerMessage = string.Empty;

                if (ex != null)
                {
                    exceptionMessage = ex.Message != null ? ex.Message : string.Empty;
                    exceptionInnerMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                }
                else
                {
                    exceptionMessage = "Error found - no details.";
                }

                write(methodName, exceptionMessage, exceptionInnerMessage);
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

        private string checkMessageContent(string _message)
        {
            string message = _message;
            if (message.Contains("Failed to authenticate with AAD by the credential.") || message.Contains("There was no endpoint listening") || message.Contains("caused by an incorrect address or SOAP action") || message.Contains("dynamics.com/soap/services/"))
            {
                //There was no endpoint listening at  that could accept the message. 
                //This is often caused by an incorrect address or SOAP action. See InnerException, if present, for more details.
                message = "An error occurred while establishing a connection to Dynamics AX services.";

            }
            else if (message.Contains("error occurred while establishing a connection to SQL Server") || message.Contains("The server was not found or was not accessible") || message.Contains("error: 26 - Error Locating Server/Instance Specified"))
            {
                //Message: A network-related or instance-specific error occurred while establishing a connection to SQL Server.
                //The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections.
                //(provider: SQL Network Interfaces, error: 26 - Error Locating Server/Instance Specified)
                message = "An error occurred while establishing a connection to SQL Server.";
            }

            return message;

        }

        private static void LogException(Exception exc, string source)
        {
            // Include enterprise logic for logging exceptions 
            // Get the absolute path to the log file 
            string logFile = "App_Data/ErrorLog.txt";
            logFile = System.Web.HttpContext.Current.Server.MapPath(logFile);

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }
            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Source: " + source);
            sw.WriteLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }

    }
}