using PortalIntegration;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS
{
    public partial class LinkDevice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DFUserInfo().retrieveQRCode();

            foreach (DataRow dr in dt.Rows)
            {
                var image = dr["QRImage"];
                imgQr.ImageUrl = "data:image/png;base64," + image;
            }
        }
    }
}