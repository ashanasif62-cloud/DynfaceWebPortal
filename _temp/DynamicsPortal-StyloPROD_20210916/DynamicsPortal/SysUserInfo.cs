using AuthenticationHelper;
using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmploymentInformationSvcReference;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DynamicsPortal
{
    public class SysUserInfo
    {
        public string message;
        public string password;
        private PREmploymentInformation pREmploymentInformation = new PREmploymentInformation();

        public bool createUser(string _personalNumber, string _userId, bool _isActive = true, string _roles = "")
        {
            string personalNumber = _personalNumber;
            string userId = _userId;
            bool isActive = _isActive;
            bool results = false;

            try
            {
                if (string.IsNullOrEmpty(personalNumber) || string.IsNullOrEmpty(userId))
                {
                    message = "Invalid employee details.";
                    return results;
                }
                //DataTable dtEmployeeDetails = ControlsHelper.retrieveAllEmployee();
                //DataRow dataRow = dtEmployeeDetails.Select("EmployeeId = '" + personalNumber + "'").FirstOrDefault();

                PREmploymentInformationSvcContract employmentInformationSvcContract = pREmploymentInformation.retrieveEmployeeDetails(personalNumber);
                if (employmentInformationSvcContract.RecId == 0)
                {
                    message = "Unable to retrieve employee details.";
                    return results;
                }

                //HcmWorkerDetailsSvcContract workerDetailsContract = ControlsHelper.getWorkerDetails(personalNumber);
                //if (string.IsNullOrEmpty(employmentInformationSvcContract.workerEmail))
                //{
                //    message = "Employee doesn't have email details.";
                //    return results;
                //}

                ESSEmployeeLegalEntities eSSEmployeeLegalEntities = new ESSEmployeeLegalEntities();
                string[] employeeLegalEntities = eSSEmployeeLegalEntities.retrieveEmployeeLegalEntities(personalNumber);
                if (employeeLegalEntities.Count() == 0)
                {
                    message = "Unable to retrieve employee company details.";
                    return results;
                }

                string defaultDataAreaId = employmentInformationSvcContract.DataAreaId;              //dataRow["DataAreaId"].ToString().ToUpper();
                string userName = employmentInformationSvcContract.EmployeeName;            //dataRow["EmployeeName"].ToString();
                long partition = employmentInformationSvcContract.Partition;               //0;
                //Int64.TryParse(dataRow["Partition"].ToString(), out partition);

                string currentUserId = SessionVariables.getCurrentUserId();
                string plainPassword = GenerateRandomPassword.generate();
                string encryptedPass = SecurePassword.securePassword(userId, plainPassword);

                SysUserInfo_BOL objBOL = new SysUserInfo_BOL();
                objBOL.UserId = userId;
                objBOL.EmployeeId = personalNumber;
                objBOL.UserName = userName;
                objBOL.Password = encryptedPass;
                objBOL.Partition = partition;
                objBOL.LicenseType = 1;
                objBOL.Status = isActive;
                objBOL.CreatedBy = currentUserId;
                objBOL.ModifiedBy = currentUserId;

                if (employeeLegalEntities.Contains(defaultDataAreaId))
                {
                    objBOL.DefaultCompany = defaultDataAreaId;
                }
                else
                {
                    objBOL.DefaultCompany = employeeLegalEntities[0];
                }

                SysUserInfo_BLL objBLL = new SysUserInfo_BLL();
                string operationResults = objBLL.sysUserInfo_Create(objBOL);
                results = NotificationMessage.showMessage(operationResults);

                if (results)
                {
                    foreach (string legalEntity in employeeLegalEntities)
                    {
                        objBOL.DataAreaId = legalEntity;
                        operationResults = objBLL.sysUserDetails_Create(objBOL);

                        if (!String.IsNullOrEmpty(_roles))
                        {
                            string[] roles = _roles.Split(',');
                            foreach (string role in roles)
                            {
                                if (!String.IsNullOrEmpty(role))
                                {
                                    SysUserRole_BOL userRole_BOL = new SysUserRole_BOL();
                                    userRole_BOL.UserId = objBOL.UserId;
                                    userRole_BOL.RoleId = role;
                                    userRole_BOL.LicenseType = objBOL.LicenseType;
                                    userRole_BOL.DataAreaId = objBOL.DataAreaId;
                                    userRole_BOL.Partition = objBOL.Partition;
                                    userRole_BOL.CreatedBy = objBOL.CreatedBy;
                                    userRole_BOL.ModifiedBy = objBOL.CreatedBy;

                                    SysUserRoles_BLL userRoles_BLL = new SysUserRoles_BLL();
                                    userRoles_BLL.sysUserRoles_Create(userRole_BOL);
                                }
                            }
                        }
                    }
                    //sendEmail(personalNumber, "", userId, plainPassword);
                }
                password = plainPassword;

                return results;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                message = ex.Message;
                return results;
            }
            finally
            { }
        }

        public string createPassword()
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()<>?.";
            int length = 10;
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


        private void sendEmail(string _emailTo, string _emailCC, string _userId, string _password)
        {
            string userId = _userId;
            string password = _password;
            string emailTo = _emailTo;
            string emailCC = _emailCC;
            string subject, body;

            List<string> emailToList = new List<string>() { emailTo };
            List<string> emailCCList = new List<string>() { emailCC };

            HRSendEmail hRSendEmail = new HRSendEmail();
            subject = "Employee Self Service Portal New User Notification Email";

            body = "Your ESS User is created for Employee Self Service Portal." + Environment.NewLine;
            //body += "Access URL: " + userId + Environment.NewLine;
            body += "User Id: " + userId + Environment.NewLine;
            body += "Password: " + password + Environment.NewLine;

            hRSendEmail.sendEmail(emailToList.ToArray(), subject, body, emailToList.ToArray(), false);//false bcz empid is provided instead of email id

        }
    }
}