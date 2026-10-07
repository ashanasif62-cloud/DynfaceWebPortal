namespace BussinessObject
{
    public class SysUserRole_BOL
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public int LicenseType { get; set; }
        public int? RecVersion { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string DataAreaId { get; set; }
        public long Partition { get; set; }
        public long RecId { get; set; }

    }
}
