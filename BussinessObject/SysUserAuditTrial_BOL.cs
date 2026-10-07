using System;

namespace BussinessObject
{
    public class SysUserAuditTrial_BOL
    {
        public string UserId { get; set; }
        public DateTime ActionDateTime { get; set; }
        public string ActionItem { get; set; }
        public string TableName { get; set; }
        public long RecordRecId { get; set; }
        public string ActionType { get; set; }
        public string ActionMessage { get; set; }
        public bool ActionResult { get; set; }

        public int? RecVersion { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string DataAreaId { get; set; }
        public long Partition { get; set; }
        public long RecId { get; set; }

    }
}
