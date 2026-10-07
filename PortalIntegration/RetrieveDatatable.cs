using GeneralAuxiliary;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace PortalIntegration
{
    public class RetrieveDatatable
    {
        // Phase 5 Optimization: Cache PropertyInfo arrays per type — GetProperties() is expensive via Reflection
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache
            = new ConcurrentDictionary<Type, PropertyInfo[]>();

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
        public static DataTable CreateDataTableFromMap<T>(IEnumerable<T> items, string mapAccessorName = "parmFields")
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            try
            {
                if (items == null || !items.Any())
                {
                    // Add columns required by GridView so binding doesn't fail
                    string[] requiredColumns = new[]
                    {
                "RecId", "ExpenseTransactionNumber", "ExpenseReportNumber", "TransDate",
                "ApprovalStatus", "CostType", "MerchantId", "AmountCurr",
                "ExchangeCode", "ProjId", "ProjStatusId", "ProjActivityNumber"
            };

                    foreach (var col in requiredColumns)
                        dataTable.Columns.Add(col);

                    return dataTable;
                }

                var type = typeof(T);
                MethodInfo mapMethod = type.GetMethod(mapAccessorName);
                PropertyInfo mapProperty = type.GetProperty(mapAccessorName);

                if (mapMethod == null && mapProperty == null)
                    throw new MissingMemberException($"{type.Name} does not have a method or property named '{mapAccessorName}'.");

                // Use first item to determine column structure
                object firstMapObj = mapMethod != null
                    ? mapMethod.Invoke(items.First(), null)
                    : mapProperty.GetValue(items.First());

                IDictionary firstMap = firstMapObj as IDictionary;
                if (firstMap == null)
                    throw new InvalidOperationException("Returned value is not a valid IDictionary.");

                // Create columns from keys
                foreach (var key in firstMap.Keys)
                {
                    dataTable.Columns.Add(key.ToString());
                }

                // Fill rows
                foreach (var item in items)
                {
                    object mapObj = mapMethod != null
                        ? mapMethod.Invoke(item, null)
                        : mapProperty.GetValue(item);

                    IDictionary map = mapObj as IDictionary;
                    if (map != null)
                    {
                        DataRow row = dataTable.NewRow();
                        foreach (var key in map.Keys)
                        {
                            row[key.ToString()] = map[key] ?? DBNull.Value;
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write("CreateDataTableFromMap", ex);
            }

            return dataTable;
        }
        public static DataTable createDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            // Phase 5: Retrieve from cache; only calls GetProperties() once per type
            var properties = _propertyCache.GetOrAdd(type, t => t.GetProperties());
            string tableName = type.Name;
            DataTable dataTable = new DataTable(tableName);


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
