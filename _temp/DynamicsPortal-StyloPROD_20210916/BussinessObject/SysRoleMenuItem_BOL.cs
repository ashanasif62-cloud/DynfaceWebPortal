namespace BussinessObject
{
    public class SysRolesMenuItem_BOL
    {
        public string MenuItemId { get; set; }
        public string RoleId { get; set; }
        public int LicenseType { get; set; }
        public int AccessLevel { get; set; }
        public string UserId { get; set; }   //Not Exists in Table
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
