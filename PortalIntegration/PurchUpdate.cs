using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PortalIntegration.PurchaseOrderHeaderSvcrRefernce
{
    [DataContract]
    public enum PurchUpdate
    {
        [EnumMember]
        ReceiveNow = 0,
        [EnumMember]
        All = 1,
        [EnumMember]
        Registered = 2,
        [EnumMember]
        PackingSlip = 3,
        [EnumMember]
        RegisteredAndServices = 4,
        [EnumMember]
        BillOfEntryQuantity_IN = 5
    }
}
