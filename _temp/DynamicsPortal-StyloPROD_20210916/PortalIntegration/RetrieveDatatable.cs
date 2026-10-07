using GeneralAuxiliary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace PortalIntegration
{
    public class RetrieveDatatable
    {
        //public static DataTable createDataTable(Object[] arr)
        //{
        //    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(arr.GetType());
        //    System.IO.StringWriter sw = new System.IO.StringWriter();
        //    serializer.Serialize(sw, arr);

        //    System.Data.DataSet ds = new System.Data.DataSet();
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    System.IO.StringReader reader = new System.IO.StringReader(sw.ToString());

        //    ds.ReadXml(reader);
        //    if (ds.Tables.Count > 0)
        //        return ds.Tables[0];
        //    else
        //        return new DataTable();
        //}

        public static DataTable createDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            string tableName = type.Name;
            DataTable dataTable = new DataTable(tableName);
            //dataTable.TableName = tableName;

            try
            {
                foreach (PropertyInfo info in properties)
                {
                    dataTable.Columns.Add(new DataColumn(info.Name));   //, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
                }

                foreach (T entity in list)
                {
                    object[] values = new object[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        values[i] = properties[i].GetValue(entity);
                        //if (properties[i].PropertyType.FullName == "PortalIntegration.PREmployeeEOSRequestsSvcReference.Date")
                        //{
                        //    values[i] = ((PortalIntegration.PREmployeeEOSRequestsSvcReference.Date)properties[i].GetValue(entity))._value;
                        //}
                        //else
                    }

                    dataTable.Rows.Add(values);
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

            return dataTable;
        }

    }
}
