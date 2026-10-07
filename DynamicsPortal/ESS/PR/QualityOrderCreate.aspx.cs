using GeneralAuxiliary;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class QualityOrderCreate : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindReferenceTypeDropdown();
                BindItemNumberDropdown();
                BindTestGroupDropdown();
            }
        }

     
        private void BindReferenceTypeDropdown()
        {
          
        }

        private void BindItemNumberDropdown()
        {
           
        }

        private void BindTestGroupDropdown()
        {
          
        }

        
        protected void btnOk_Click(object sender, EventArgs e)
        {
           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
           
        }
    }
}
