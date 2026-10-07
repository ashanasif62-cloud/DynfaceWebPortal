using System;

namespace BussinessObject
{
    public class SysUserInfo_BOL
    {

        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string DataAreaId { get; set; }
        public string DataAreaName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DefaultCompany { get; set; }
        public string NewPassword { get; set; }
        public int LicenseType { get; set; }
        public Byte[] Image { get; set; }
        public bool Status { get; set; }
        public int RecVersion { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public long Partition { get; set; }
        public long RecId { get; set; }
    }
}