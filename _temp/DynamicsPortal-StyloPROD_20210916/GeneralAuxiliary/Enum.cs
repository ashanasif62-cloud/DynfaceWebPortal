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