<%@ WebHandler Language="C#" Class="LookupHandler" %>

using System;
using System.Web;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using DynamicsPortal;

public class LookupHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "application/json";

        string entity = context.Request.QueryString["entity"];
        string search = (context.Request.QueryString["q"] ?? "").ToLower();
        int page = 1;
        int.TryParse(context.Request.QueryString["page"], out page);
        if (page < 1) page = 1;
        int pageSize = 20;

        if (string.IsNullOrEmpty(entity)) {
            context.Response.Write("{\"results\": []}");
            return;
        }

        List<object> results = new List<object>();
        bool hasMore = false;

        try {
            if (entity.Equals("employee", StringComparison.OrdinalIgnoreCase)) {
                DataTable dt = ControlsHelper.retriveEmployeeReportees();

                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["EmployeeId"] != DBNull.Value && r.Field<string>("EmployeeId").ToLower().Contains(search)) ||
                            (r["EmployeeName"] != DBNull.Value && r.Field<string>("EmployeeName").ToLower().Contains(search))
                        );
                    }

                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;

                    var pagedQuery = query.Skip((page - 1) * pageSize).Take(pageSize);

                    foreach (DataRow row in pagedQuery) {
                        results.Add(new {
                            id = row["EmployeeId"].ToString(),
                            text = row["EmployeeId"].ToString() + " - " + row["EmployeeName"].ToString()
                        });
                    }
                }
            }
            else if (entity.Equals("vendor", StringComparison.OrdinalIgnoreCase)) {
                // Fetch from existing PO helper - vendor DataTable is the single source of truth
                // for VendorName, Address, ServiceAddress, ChangeRequestEnabled, PurchPoolID,
                // SiteID, LocationID, IsInterCompanyVendor and InterCompanyPartnerCompanyName.
                PortalIntegration.PurchaseOrderHeader neworder = new PortalIntegration.PurchaseOrderHeader();
                DataTable dt = neworder.retrieveVenderAccount();

                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["VendorAccount"] != DBNull.Value && r.Field<string>("VendorAccount").ToLower().Contains(search)) ||
                            (r["WorkerName"] != DBNull.Value && r.Field<string>("WorkerName").ToLower().Contains(search))
                        );
                    }

                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;

                    var pagedQuery = query.Skip((page - 1) * pageSize).Take(pageSize);

                    foreach (DataRow row in pagedQuery) {
                        results.Add(new {
                            id = row["VendorAccount"].ToString(),
                            text = row["VendorAccount"].ToString() + " - " + row["WorkerName"].ToString(),
                            vendorName = row["WorkerName"].ToString(),
                            address = row["Address"].ToString(),
                            serviceAddress = row["ServiceAddress"].ToString(),
                            changeRequest = row["ChangeRequestEnabled"].ToString(),
                            purchPoolId = row["PurchPoolID"].ToString(),

                            // ---- fields required for Site / Warehouse / Intercompany auto-fill ----
                            siteId = SafeGet(row, "SiteID"),
                            locationId = SafeGet(row, "LocationID"),
                            isInterCompanyVendor = SafeGet(row, "IsInterCompanyVendor"),
                            interCompanyPartnerCompanyName = SafeGet(row, "InterCompanyPartnerCompanyName")
                        });
                    }
                }
            }
            else if (entity.Equals("contact", StringComparison.OrdinalIgnoreCase)) {
                PortalIntegration.PurchaseOrderHeader neworder = new PortalIntegration.PurchaseOrderHeader();
                DataTable dt = neworder.retrievecontactdetails();
                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["ContactPersonId"] != DBNull.Value && r.Field<string>("ContactPersonId").ToLower().Contains(search)) ||
                            (r["PersonName"] != DBNull.Value && r.Field<string>("PersonName").ToLower().Contains(search))
                        );
                    }
                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;
                    foreach (DataRow row in query.Skip((page - 1) * pageSize).Take(pageSize)) {
                        results.Add(new {
                            id = row["ContactPersonId"].ToString(),
                            text = row["ContactPersonId"].ToString() + " - " + row["ContactForParty"].ToString() + " - " + row["PersonName"].ToString(),
                            contactFor = row["ContactForParty"].ToString(),
                            personName = row["PersonName"].ToString()
                        });
                    }
                }
            }
            else if (entity.Equals("project", StringComparison.OrdinalIgnoreCase)) {
                PortalIntegration.PurchaseOrderHeader neworder = new PortalIntegration.PurchaseOrderHeader();
                DataTable dt = neworder.retrieveprojectID();
                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["ProjectID"] != DBNull.Value && r.Field<string>("ProjectID").ToLower().Contains(search)) ||
                            (r["ProjectName"] != DBNull.Value && r.Field<string>("ProjectName").ToLower().Contains(search))
                        );
                    }
                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;
                    foreach (DataRow row in query.Skip((page - 1) * pageSize).Take(pageSize)) {
                        results.Add(new {
                            id = row["ProjectID"].ToString(),
                            text = row["ProjectID"].ToString() + " - " + row["ProjectName"].ToString(),
                            projectName = row["ProjectName"].ToString()
                        });
                    }
                }
            }
            else if (entity.Equals("currency", StringComparison.OrdinalIgnoreCase)) {
                DataTable dt = ControlsHelper.retrieveAllCurrencyDetails();
                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["CurrencyCode"] != DBNull.Value && r.Field<string>("CurrencyCode").ToLower().Contains(search))
                        );
                    }
                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;
                    foreach (DataRow row in query.Skip((page - 1) * pageSize).Take(pageSize)) {
                        results.Add(new {
                            id = row["CurrencyCode"].ToString(),
                            text = row["CurrencyCode"].ToString()
                        });
                    }
                }
            }
            else if (entity.Equals("site", StringComparison.OrdinalIgnoreCase)) {
                PortalIntegration.PurchaseOrderHeader neworder = new PortalIntegration.PurchaseOrderHeader();
                DataTable dt = neworder.retrievesiteinfo();
                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["SiteID"] != DBNull.Value && r.Field<string>("SiteID").ToLower().Contains(search)) ||
                            (r["SiteName"] != DBNull.Value && r.Field<string>("SiteName").ToLower().Contains(search))
                        );
                    }
                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;
                    foreach (DataRow row in query.Skip((page - 1) * pageSize).Take(pageSize)) {
                        results.Add(new {
                            id = row["SiteID"].ToString(),
                            text = row["SiteID"].ToString() + " - " + row["SiteName"].ToString(),
                            siteName = row["SiteName"].ToString()
                        });
                    }
                }
            }
            else if (entity.Equals("warehouse", StringComparison.OrdinalIgnoreCase)) {
                PortalIntegration.TransferOrderHeader transferordernew = new PortalIntegration.TransferOrderHeader();
                DataTable dt = transferordernew.retrievefromwarehouse();
                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["FromWarehouse"] != DBNull.Value && r.Field<string>("FromWarehouse").ToLower().Contains(search)) ||
                            (r["WarehouseName"] != DBNull.Value && r.Field<string>("WarehouseName").ToLower().Contains(search))
                        );
                    }
                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;
                    foreach (DataRow row in query.Skip((page - 1) * pageSize).Take(pageSize)) {
                        results.Add(new {
                            id = row["FromWarehouse"].ToString(),
                            text = row["FromWarehouse"].ToString() + " - " + row["WarehouseName"].ToString(),
                            warehouseName = row["WarehouseName"].ToString()
                        });
                    }
                }
            }
            else if (entity.Equals("buyergroup", StringComparison.OrdinalIgnoreCase)) {
                PortalIntegration.PurchaseOrderHeader neworder = new PortalIntegration.PurchaseOrderHeader();
                DataTable dt = neworder.retrieveBuyerGroupInformation();
                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["BuyerGroup"] != DBNull.Value && r.Field<string>("BuyerGroup").ToLower().Contains(search)) ||
                            (r["Description"] != DBNull.Value && r.Field<string>("Description").ToLower().Contains(search))
                        );
                    }
                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;
                    foreach (DataRow row in query.Skip((page - 1) * pageSize).Take(pageSize)) {
                        results.Add(new {
                            id = row["BuyerGroup"].ToString(),
                            text = row["BuyerGroup"].ToString() + " - " + row["Description"].ToString(),
                            description = row["Description"].ToString()
                        });
                    }
                }
            }
            else if (entity.Equals("worker", StringComparison.OrdinalIgnoreCase)) {
                PortalIntegration.PurchaseOrderHeader neworder = new PortalIntegration.PurchaseOrderHeader();
                DataTable dt = neworder.retrieveworkerinfo();
                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["PersonnelNumber"] != DBNull.Value && r.Field<string>("PersonnelNumber").ToLower().Contains(search)) ||
                            (r["WorkerName"] != DBNull.Value && r.Field<string>("WorkerName").ToLower().Contains(search))
                        );
                    }
                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;
                    foreach (DataRow row in query.Skip((page - 1) * pageSize).Take(pageSize)) {
                        results.Add(new {
                            id = row["PersonnelNumber"].ToString(),
                            text = row["PersonnelNumber"].ToString() + " - " + row["WorkerName"].ToString(),
                            workerName = row["WorkerName"].ToString()
                        });
                    }
                }
            }
            else if (entity.Equals("pool", StringComparison.OrdinalIgnoreCase)) {
                PortalIntegration.PurchaseOrderHeader neworder = new PortalIntegration.PurchaseOrderHeader();
                DataTable dt = neworder.retrievePoolInfo();
                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["PoolID"] != DBNull.Value && r.Field<string>("PoolID").ToLower().Contains(search)) ||
                            (r["PoolName"] != DBNull.Value && r.Field<string>("PoolName").ToLower().Contains(search))
                        );
                    }
                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;
                    foreach (DataRow row in query.Skip((page - 1) * pageSize).Take(pageSize)) {
                        results.Add(new {
                            id = row["PoolID"].ToString(),
                            text = row["PoolID"].ToString() + " - " + row["PoolName"].ToString(),
                            poolName = row["PoolName"].ToString()
                        });
                    }
                }
            }
            else if (entity.Equals("language", StringComparison.OrdinalIgnoreCase)) {
                PortalIntegration.PurchaseOrderHeader neworder = new PortalIntegration.PurchaseOrderHeader();
                DataTable dt = neworder.retrieveLangaugeID();
                if (dt != null) {
                    var query = dt.AsEnumerable();
                    if (!string.IsNullOrEmpty(search)) {
                        query = query.Where(r =>
                            (r["LangaugeID"] != DBNull.Value && r.Field<string>("LangaugeID").ToLower().Contains(search))
                        );
                    }
                    int totalCount = query.Count();
                    hasMore = (page * pageSize) < totalCount;
                    foreach (DataRow row in query.Skip((page - 1) * pageSize).Take(pageSize)) {
                        results.Add(new {
                            id = row["LangaugeID"].ToString(),
                            text = row["LangaugeID"].ToString()
                        });
                    }
                }
            }

            var responseObj = new {
                results = results,
                pagination = new { more = hasMore }
            };

            JavaScriptSerializer js = new JavaScriptSerializer();
            context.Response.Write(js.Serialize(responseObj));
        }
        catch (Exception ex) {
            context.Response.StatusCode = 500;
            context.Response.Write("{\"error\": \"" + ex.Message + "\"}");
        }
    }

    /// <summary>
    /// Safely reads a column value from a DataRow, returning "" if the column
    /// doesn't exist on the DataTable or the value is DBNull. Prevents the
    /// handler from throwing if the vendor query's column names differ slightly
    /// from what's expected (rename to match your actual schema if needed).
    /// </summary>
    private static string SafeGet(DataRow row, string columnName) {
        if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value) {
            return row[columnName].ToString();
        }
        return "";
    }

    public bool IsReusable {
        get { return false; }
    }
}
