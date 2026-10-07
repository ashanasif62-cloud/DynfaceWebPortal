using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;

namespace PortalIntegration
{
    public class SysOperationResults
    {
        public static SysOperationResult_BOL operationResults<T>(IEnumerable<T> _generalContract)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                DataTable dataTable = RetrieveDatatable.createDataTable(_generalContract);
                if (dataTable != null)
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow dataRow = dataTable.Rows[0];
                        objBOL = operationResults(dataRow);
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

        public static SysOperationResult_BOL operationResults(DataRow _dataRow)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            string message;
            DataRow dataRow = _dataRow;
            try
            {
                bool.TryParse(dataRow["isSuccess"].ToString(), out bool isSuccess);
                message = dataRow["Message"].ToString();
                Int64.TryParse(dataRow["RecId"].ToString(), out long recId);

                objBOL.isSuccess = isSuccess;
                objBOL.Message = message;
                objBOL.RecId = recId;

                if (isSuccess)
                    objBOL.AlertType = AlertType.Success.ToString();
                else
                    objBOL.AlertType = AlertType.Error.ToString();

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

        public static SysOperationResult_BOL operationResult<T>(object _generalContract)
        {
            SysOperationResult_BOL objBOL = new SysOperationResult_BOL();
            try
            {
                T[] generalContract = new T[] { (T)_generalContract };
                //Object[] generalContract = new Object[] { _generalContract };
                objBOL = operationResults(generalContract);
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


    }
}
