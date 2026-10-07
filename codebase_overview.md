# DynaFace WebApp - Codebase Overview

This document provides a high-level overview of the DynaFace WebApp codebase, designed to aid in understanding the architecture and streamlining future development.

## 🏗️ Architecture Stack
The application is built using **ASP.NET WebForms (.NET Framework 4.8.1)**. It follows a multi-tier architectural pattern for modularity and separation of concerns.

### 📁 Project Structure

| Directory | Namespace | Purpose |
|-----------|-----------|---------|
| **`DynamicsPortal`** | `DynamicsPortal` | **UI Layer** - `.aspx` pages, `.ascx` controls, `.Master` templates |
| **`BussinessLogic`** | `BussinessLogic` | **Logic Layer** - Business rules, validation, orchestration |
| **`DataAccess`** | `DataAccess` | **Data Layer** - SQL via `getConnection_DAL`, stored procedures |
| **`BussinessObject`** | `BussinessObject` | **Models** - BOL (Business Object Layer) data classes |
| **`GeneralAuxiliary`** | `GeneralAuxiliary` | **Utilities** - Session, enums, error logging, notifications |
| **`AuthenticationHelper`** | `AuthenticationHelper` | **Auth** - Login, Azure AD, Google OAuth |
| **`PortalIntegration`** | `PortalIntegration` | **Integration** - WCF/SOAP clients for Dynamics 365 FO |

## 🔑 Core Patterns

### Naming Conventions
- `_BOL` suffix → Business Object (model class)
- `_BLL` suffix → Business Logic Layer class
- `_DAL` suffix → Data Access Layer class
- Private method params prefixed with `_`: `string _userId`

### Session Management
**Always** use `SessionVariables` (GeneralAuxiliary) — never direct `HttpContext.Current.Session` access.
- `SessionVariables.getCurrentUserId()` / `setCurrentUserId()`
- `SessionVariables.getCurrentEmployeeId()` / `setCurrentEmployeeId()`
- `SessionVariables.getUserCurrentDataAreaId()` — company/legal entity context

### Data Access
Use `getConnection_DAL` for all SQL. Key methods:
- `retriveDataTable_Query(sql, companyWise)` — raw SQL → DataTable
- `executeProcedureRetriveDataTable(procName, paramList)` — SP → DataTable
- `executeProcedure(procName, paramList, scalar)` — SP, returns long

### Notifications
```csharp
NotificationMessage.showMessage(AlertType.Success, "Saved.");
NotificationMessage.showMessage(AlertType.Error, "Failed.");
```

### Results
```csharp
GetResults.createOperationResults(resultId)
GetResults.updateOperationResults(resultId)
GetResults.incompleteDataMessage()
```

## 🚀 Key Workflows

### 1. Adding a New Screen
1. **UI**: Create `.aspx` using `Site.Master` in `DynamicsPortal/ESS/<Module>/`
2. **Integration**: Add retrieval/create/update methods in the relevant `PortalIntegration` class
3. **Session caching**: Cache lookup DataTables via `SessionVariables.setSessionDataTable_*()`
4. **Navigation**: Link via menu system (`GenerateMenu.cs`)

### 2. Module Map
- `ESS/PR/` → Payroll (Leave, Loans, Advances, EOS, PF, Earnings)
- `ESS/HR/` → HR (Personal Details, Policies, Letters, Hiring, Business Trips)
- `ESS/TA/` → Time & Attendance (Roster, Attendance, Timecard, Adjustments)
- `ESS/EM/` → Expense Management (Expense Reports and Lines)
- `Administration/` → User Login and User Management

### 3. Authentication
Forms Auth → Login URL: `/Administration/UserLogin.aspx`
Supports: Standard login, Azure AD OAuth (`AuthCallback.aspx`), Google OAuth (`GoogleLoginCallback.aspx`)

## 🛠️ Tech Stack
- **Backend**: C# .NET Framework 4.8.1, ASP.NET WebForms
- **Database**: Azure SQL (`dynafacedb` on `dynafacesql.database.windows.net`)
- **ERP**: Dynamics 365 Finance & Operations (WCF/SOAP via PortalIntegration)
- **Frontend**: Bootstrap 4, jQuery 3.3.1, Font Awesome, Material Design Icons
- **CSS**: `distribution/css/custom.css` is the primary override file

---
*Last indexed by Antigravity AI — 2026-04-13*
