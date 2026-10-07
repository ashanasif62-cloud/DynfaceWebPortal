using DataAccess;
using GeneralAuxiliary;
using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public class ImportUsers
    {
        public FileUpload fileUpload;
        private DataTable sourceData = new DataTable();
        private OleDbConnection conn = new OleDbConnection();
        private ControlsHelper controlsHelper = new ControlsHelper();
        private FlushOperationResults operationResults = new FlushOperationResults();



        //public string dataImport()
        //{
        //    string results = string.Empty, message = string.Empty, messageSuccess = string.Empty, messageFailure = string.Empty;
        //    int totalRecords = 0, countSuccess = 0, countFailure = 0;
        //    getConnection_DAL getConnection = new getConnection_DAL();
        //    string strConnection = getConnection.getConnectionString();

        //    try
        //    {
        //        results = fetchFileData();

        //        Hashtable hTable = GetResults.getResultAttributes(results);
        //        bool isDataRetrieved = Convert.ToBoolean(hTable["Result"]);
        //        totalRecords = Convert.ToInt32(hTable["ResultId"]);
        //        if (isDataRetrieved)
        //        {
        //            if (sourceData.Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in sourceData.Rows)
        //                {
        //                    string employeeId = dr[0].ToString();
        //                    string userId = dr[1].ToString();
        //                    string rolesId = dr[2].ToString();

        //                    bool iResult = false;
        //                    string iPassword = string.Empty;
        //                    string iMessage = string.Empty;

        //                    if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(userId))
        //                    {
        //                        SysUserInfo sysUserInfo = new SysUserInfo();
        //                        iResult = sysUserInfo.createUser(employeeId, userId, true, rolesId);
        //                        iPassword = sysUserInfo.password;
        //                        iMessage = sysUserInfo.message;

        //                        if (iResult)
        //                        {
        //                            messageSuccess += "User: " + userId + " is created with password: " + iPassword + "\n";
        //                            countSuccess++;
        //                        }
        //                        else
        //                        {
        //                            messageFailure += "User: " + userId + " creation failed: " + iMessage + "\n";
        //                            countFailure++;
        //                        }
        //                    }
        //                }

        //                message = countSuccess + " record(s) successfully inserted. " + countFailure + " record(s) insertion failed.\n";
        //                message += messageSuccess;
        //                message += messageFailure;

        //                operationResults.flushResults(message);

        //                NotificationMessage.showMessage(AlertType.Information, message);

        //                if (countSuccess > 0)
        //                    results = GetResults.xmlMessage(countSuccess, message, AlertType.Success.ToString(), true);
        //                else
        //                    results = GetResults.xmlMessage(countSuccess, message, AlertType.Error.ToString(), false);
        //            }
        //        }
        //        else
        //        {
        //            NotificationMessage.showMessage(results);
        //        }

        //        return results;

        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //        return results;
        //    }
        //    finally
        //    {

        //    }
        //}


        //private string fetchFileData()
        //{
        //    string results = string.Empty;
        //    try
        //    {
        //        if (fileUpload != null && fileUpload.HasFile)
        //        {
        //            string inputFileName = fileUpload.FileName;
        //            string fileName = inputFileName.Substring(0, inputFileName.LastIndexOf('.')) + DateTime.Now.ToString("yyyyMMddHHmm");
        //            string fileExt = inputFileName.Substring(inputFileName.LastIndexOf('.') + 0).ToLower();
        //            string fileFullName = fileName + fileExt;

        //            try
        //            {
        //                if (fileExt == ".xls" || fileExt == ".xlsx")
        //                {
        //                    fileName = Path.GetFileName(fileUpload.PostedFile.FileName);
        //                    string directoryPath = controlsHelper.getApplicationDirectoryPath() + "UploadExcelFile\\";
        //                    string directoryPath = HttpContext.Current.Server.MapPath("~/UploadExcelFile//");

        //                    string fullFilePath = directoryPath + fileFullName;

        //                    Directory.CreateDirectory(directoryPath);
        //                    fileUpload.SaveAs(fullFilePath);

        //                    string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullFilePath + ";" +
        //                                      "Extended Properties=\"Excel 12.0 Xml; HDR = YES; IMEX = 1\"";
        //                    "HDR=Yes;" indicates that the first row contains columnnames, not data.
        //                    "IMEX=1;" tells the driver to always read "intermixed" data columns as text

        //                    conn = new OleDbConnection(strConn);
        //                    conn.Open();

        //                    DataTable sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        //                    if (sheets.Rows.Count > 0)
        //                    {
        //                        string sheetName = sheets.Rows[0]["TABLE_NAME"].ToString();

        //                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheetName + "]", conn);
        //                        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        //                        da.Fill(sourceData);

        //                        conn.Close();
        //                        conn.Dispose();

        //                        deleteExcelFile(fileFullName); // Delete File Log

        //                        sourceData = ds.Tables[0];
        //                        int rowCount = sourceData.Rows.Count;

        //                        results = GetResults.xmlMessage(rowCount, "", AlertType.Success.ToString(), true);
        //                        return results;

        //                    }
        //                    else
        //                    {
        //                        string msg = "Excel File doesn't contain table.";
        //                        results = GetResults.xmlErrorMessage(msg);
        //                        return results;
        //                    }
        //                }
        //                else
        //                {
        //                    string msg = "Please upload excel file.";
        //                    results = GetResults.xmlErrorMessage(msg);
        //                    return results;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                conn.Close();
        //                conn.Dispose();
        //                deleteExcelFile(fileFullName); // Delete File Log

        //                results = GetResults.xmlErrorMessage(ex.Message);
        //                return results;
        //            }
        //        }
        //        else
        //        {
        //            string msg = "Please upload excel file.";
        //            results = GetResults.xmlErrorMessage(msg);
        //            return results;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        results = GetResults.xmlErrorMessage(ex.Message);
        //        return results;
        //    }
        //}

        //protected bool deleteExcelFile(string fileName)
        //{
        //    try
        //    {
        //        string directoryPath = controlsHelper.getApplicationDirectoryPath() + "UploadExcelFile\\";
        //        if (Directory.Exists(directoryPath))
        //        {
        //            string[] logList = Directory.GetFiles(directoryPath, fileName);

        //            foreach (string log in logList)
        //            {
        //                FileInfo logInfo = new FileInfo(log);
        //                string logInfoFullName = logInfo.Name;
        //                if (logInfoFullName == fileName)
        //                {
        //                    logInfo.Delete();
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        NotificationMessage.showMessage(AlertType.Error, ex.Message);

        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);

        //        return false;
        //    }
        //    finally
        //    {
        //    }
        //}


    }
}