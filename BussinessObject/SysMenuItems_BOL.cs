using System;

namespace BussinessObject

{
    public class SysMenuItems_BOL
    {
        public string MenuItemId { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string Label { get; set; }
        public string ToolTip { get; set; }
        public short Type { get; set; }
        public string Object { get; set; }
        public string ModuleId { get; set; }
        public bool Visible { get; set; }
        public short Sequence { get; set; }
        public int RecVersion { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public long RecId { get; set; }
        public long Partition { get; set; }
        public string DataAreaId { get; set; }
    }
}