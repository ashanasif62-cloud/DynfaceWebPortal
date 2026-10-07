using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization; // requires System.Web.Extensions reference

namespace DynamicsPortal
{
    public class LookupHandler : IHttpHandler
    {
        private const int PageSize = 20;


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string entity = context.Request.QueryString["entity"] ?? "";
            string term = context.Request.QueryString["q"] ?? "";
            int page = 1;
            int.TryParse(context.Request.QueryString["page"], out page);
            if (page < 1) page = 1;

            List<Dictionary<string, object>> results;
            bool more;

            switch (entity.ToLowerInvariant())
            {
                case "vendor":
                    results = SearchVendors(term, page, out more);
                    break;

                // TODO: add cases for contact, project, currency, site,
                // warehouse, buyergroup, worker, pool, language
                // following the same pattern as SearchVendors below.

                default:
                    results = new List<Dictionary<string, object>>();
                    more = false;
                    break;
            }

            var payload = new
            {
                results = results,
                pagination = new { more = more }
            };

            var serializer = new JavaScriptSerializer();
            context.Response.Write(serializer.Serialize(payload));
        }

        private List<Dictionary<string, object>> SearchVendors(string term, int page, out bool more)
        {
            var neworder = new PurchaseOrderHeader();

            // Assumes retrieveVendor() returns ALL vendors as a DataTable
            // with columns: VendorAccount, VendorName, Address, ServiceAddress,
            // ChangeRequestEnabled, PurchPoolID, SiteID, LocationID,
            // IsInterCompanyVendor, InterCompanyPartnerCompanyName
            DataTable dt = neworder.retrieveVendor();

            IEnumerable<DataRow> rows = dt.Rows.Cast<DataRow>();

            if (!string.IsNullOrWhiteSpace(term))
            {
                rows = rows.Where(r =>
                    r["VendorAccount"].ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    r["VendorName"].ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            var allMatches = rows.ToList();

            int totalCount = allMatches.Count;
            var pageRows = allMatches
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            more = (page * PageSize) < totalCount;

            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in pageRows)
            {
                list.Add(new Dictionary<string, object>
                {
                    { "id", row["VendorAccount"].ToString() },
                    { "text", row["VendorAccount"].ToString() + " - " + row["VendorName"].ToString() },
                    { "vendorName", row["VendorName"].ToString() },
                    { "address", SafeGet(row, "Address") },
                    { "serviceAddress", SafeGet(row, "ServiceAddress") },
                    { "changeRequest", SafeGet(row, "ChangeRequestEnabled") },
                    { "purchPoolId", SafeGet(row, "PurchPoolID") },
                    { "siteId", SafeGet(row, "SiteID") },
                    { "locationId", SafeGet(row, "LocationID") },
                    { "isInterCompanyVendor", SafeGet(row, "IsInterCompanyVendor") },
                    { "interCompanyPartnerCompanyName", SafeGet(row, "InterCompanyPartnerCompanyName") }
                });
            }

            return list;
        }

        private string SafeGet(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value
                ? row[column].ToString()
                : "";
        }

        public bool IsReusable => false;
    }
}