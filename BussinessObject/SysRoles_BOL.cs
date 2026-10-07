using System;

namespace BussinessObject
{
    public class SysRoles_BOL
    {

        public string RoleId { get; set; }
        public string UserId { get; set; }   //Not Exists in Table
        public string Description { get; set; }
        public string DataAreaId { get; set; }
        public int LicenseType { get; set; }
        public int RecVersion { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public long Partition { get; set; }
        public long RecId { get; set; }
    }
}