using System.ComponentModel;

namespace GeneralAuxiliary
{
    public class Enums
    {

    }
}

public enum AlertType
{
    Success,
    Information,
    Warning,
    Error
}

public enum ActionType
{
    Create,
    Update,
    Delete,
    Submit,
    Retrieve,
    Email,
    Import
}
public enum LicenseType
{
    ESS = 1,
    MSS = 2,
    Admin = 3
}
public enum AccessLevel
{
    Read,
    Create,
    Delete
}
public enum TableId
{
    HRPolicies
}
public enum JmgJourRegTypeEnum_Maison
{
    SignIn = 2,
    SignOut = 15
}

public enum HcmDurationUnit
{
    Days,
    Months,
    Years
}

public enum PRAdvanceType
{
    Loan,
    Salary,
    Expense
}
public enum JournalVoucherDraw
{
    Entry,
    Posting
}

public enum InventJournalVoucherChange
{
    [Description("Change Date")]
    Changedate,

    [Description("Change Date Or Item")]
    ChangeDateOrItem
}
public enum DetailSummary
{
    Details,
    Summary
}
public enum ItemReservation
{
    Manual,
    Automatic,
    Explosion
}

public enum PurchUpdate
{
    ReceiveNow = 0,             
    All = 1,                   
    Recorded = 2,            
    PackingSlip = 3,           
    RegisteredAndServices = 4,  
    BillOfEntryQuantity_IN = 5 ,

}