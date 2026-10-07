namespace BussinessObject
{
    public class SysUserRecentMenuItems_BOL
    {
        public string UserId { get; set; }
        public string MenuItemId { get; set; }
        public System.DateTime VisitDateTime { get; set; }
        public int RecVersion { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string DataAreaId { get; set; }
        public long Partition { get; set; }
        public long RecId { get; set; }

    }
}