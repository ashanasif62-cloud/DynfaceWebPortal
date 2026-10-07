using GeneralAuxiliary;
using System;

namespace DynamicsPortal
{
    public partial class ESSHRPersonalDetails : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSHRPersonalDetailsHistory";

                showActionPanel = false;

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    BindGrid();
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
        }
        protected void BindGrid()
        {
            hcmPersonIdentificationNumber.Attributes.Add("src", "/ESS/HR/ESSHcmPersonIdentificationNumber.aspx");
            logisticsElectronicAddress.Attributes.Add("src", "/ESS/HR/ESSLogisticsElectronicAddress.aspx");
            hcmPersonImage.Attributes.Add("src", "/ESS/HR/ESSHcmPersonImage.aspx");
            hrSubDepartment.Attributes.Add("src", "/ESS/HR/ESSHRSubDepartment.aspx");
            hcmPersonEducation.Attributes.Add("src", "/ESS/HR/ESSHcmPersonEducation.aspx");
            //logisticsLocation.Attributes.Add("src", "/ESS/HR/ESSLogisticsLocation.aspx");
        }

    }
}