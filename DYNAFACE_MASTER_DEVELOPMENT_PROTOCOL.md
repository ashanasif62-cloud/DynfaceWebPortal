# 🌌 DYNAFACE MASTER DEVELOPMENT & AI INTERACTION PROTOCOL

This document is the **absolute, definitive source of truth** created by Abdul Rehman ,for both software developers and AI Coding Assistants (like Gemini/Antigravity) working on the DynaFace WebApp codebase. 

---

## 📖 1. Universal Execution Mandate (Strictly Enforced)
*   **Zero-Skip Policy:** The AI **MUST NOT** skip or bypass the interaction protocol in any environment (Visual Studio, GitHub, or Gemini), regardless of the detected form type.
*   **Manual Override:** Automatic detection of form types is secondary to the **Mandatory Question Sequence**. The AI must walk the user through every single step individually, collecting answers.
*   **No Premature Generation:** **Strictly no code is to be generated** until the question sequence for the chosen form type is 100% complete.
*   **Architecture Lockdown (Hard-Stop Logic):** If a user's request violates established architecture, naming conventions, or inheritance patterns (e.g., requesting a custom button where not authorized, adding buttons manually, or violating namespaces), the AI **must immediately stop** and notify the user that the request is unauthorized.

---

## 🏗️ 2. Architectural Stack & Directory Map
The DynaFace WebApp is built using **ASP.NET WebForms (.NET Framework 4.8.1)**, backed by **Azure SQL Database** and integrated with **Dynamics 365 Finance & Operations** (via WCF/SOAP in the Integration layer).

### 📁 Workspace Layout
| Directory | Namespace | Layer | Primary Purpose |
| :--- | :--- | :--- | :--- |
| **`DynamicsPortal`** | `DynamicsPortal` | **UI / Presentation** | `.aspx` pages, `.ascx` custom controls, `.Master` templates. |
| **`BussinessLogic`** | `BussinessLogic` | **Business Logic (BLL)**| Business rules, data validation, workflow orchestration. |
| **`DataAccess`** | `DataAccess` | **Data Access (DAL)** | SQL query operations via `getConnection_DAL` patterns. |
| **`BussinessObject`** | `BussinessObject` | **Models / Entities** | BOL (Business Object Layer) lightweight data classes. |
| **`GeneralAuxiliary`** | `GeneralAuxiliary` | **Utilities & Helpers** | Session variables, enums, error logs, and notifications. |
| **`AuthenticationHelper`**| `AuthenticationHelper` | **Identity / Auth** | Forms Auth, Azure AD OAuth, and Google OAuth flow. |
| **`PortalIntegration`** | `PortalIntegration` | **ERP Integration** | WCF/SOAP proxy clients for Microsoft Dynamics 365 FO. |

### 🗺️ System Module Mapping
All UI files in `DynamicsPortal/ESS/` are routed by functional module:
*   `ESS/PR/` ➔ **Payroll** (Leaves, Loans, Advances, EOS, Provident Fund, Earnings)
*   `ESS/HR/` ➔ **HR** (Personal Details, Policies, Letters, Hiring, Business Trips)
*   `ESS/TA/` ➔ **Time & Attendance** (Rosters, Attendance, Timecards, Adjustments)
*   `ESS/EM/` ➔ **Expense Management** (Expense Reports, Expense Lines)
*   `Administration/` ➔ **Admin & User Access** (Login, Azure AD callback, User Management)

---

## 🔑 3. Coding Standards & Naming Conventions

### 🏷️ Suffixes and Namespace Alignment
*   **Business Objects:** Suffix with `_BOL` (e.g., `EmployeeLeave_BOL.cs`).
*   **Business Logic:** Suffix with `_BLL` (e.g., `EmployeeLeave_BLL.cs`).
*   **Data Access Layer (DAL Exception):** Classes in the `DataAccess` directory/namespace **must not** use a `_DAL` suffix (e.g., use `EmployeeLeaveDAL` or clean context-specific naming, but strictly avoid the plain `_DAL` class-name suffix).
*   **Private Method Parameters:** Prefix with an underscore `_` (e.g., `string _employeeId`).

### 💾 Session Management (Strict Policy)
*   **NEVER** access `HttpContext.Current.Session` directly.
*   **Always** utilize `SessionVariables` from `GeneralAuxiliary` (e.g., `SessionVariables.getCurrentUserId()`, `SessionVariables.getUserCurrentDataAreaId()`).

### 📦 Database Data Access Pattern
All direct SQL connections use the static database utilities. Standard methods:
*   `retriveDataTable_Query(sql, companyWise)`: Raw SQL to `DataTable`.
*   `executeProcedureRetriveDataTable(procName, paramList)`: Runs Stored Procedure, returns `DataTable`.
*   `executeProcedure(procName, paramList, scalar)`: Runs Stored Procedure, returns `long` scalar value.

### 🔔 User Feedback and Operation Messages
*   **No Standard JavaScript Alerts:** Always use the server-side alert wrapper:
    ```csharp
    NotificationMessage.showMessage(AlertType.Success, "Operation completed successfully.");
    NotificationMessage.showMessage(AlertType.Error, "An error occurred.");
    ```
*   Use `GetResults` helpers to format standard operational status:
    ```csharp
    GetResults.createOperationResults(resultId);
    GetResults.updateOperationResults(resultId);
    GetResults.incompleteDataMessage();
    ```

---

## 📝 4. Mandatory Form Interaction Protocol

Every form must inherit from either `Site.Master` or `Modal.Master` and inherit the layout, styling, and logic flow of existing forms in the workspace. To enforce this, the AI must process the following step-by-step query sequence.

### 🏁 Step 1: Master Template Choice
Ask the user:
> 💬 **"Which master template are we using: `Site.Master` or `Modal.Master`?"**

---

### 💻 Step 2: Site.Master Form Flow
*(If the user selects `Site.Master`, the Grid and standard Action Buttons [New, Edit, Delete] are auto-initialized. Ask these questions in sequence:)*

1.  > 💬 **"What are the names and data types of the Grid Fields to display?"**
2.  > 💬 **"How many of these fields are Read-Only?"**
3.  > 💬 **"How many fields are Dropdowns versus Textboxes?"**
4.  > 💬 **"What is the C# Class Name for the Business Logic/Model?"**
5.  > 💬 **"Which method is designated for Binding, Saving, and Updating?"**
6.  > 💬 **"Shall I implement a Tabbed Interface for data organization?"**
7.  > 💬 **"Shall I implement a Collapsible Panel for UI efficiency?"**

---

### 🪟 Step 3: Modal.Master Form Flow
*(If the user selects `Modal.Master`, ask these questions in sequence:)*

1.  > 💬 **"How many fields do you want to add to the modal form?"**
2.  > 💬 **"What are the names and input types (Dropdown, Textbox, Date, etc.) for each field?"**
3.  > 💬 **"Which of these fields require mandatory validation before creation?"**
4.  > 💬 **"Shall I implement any additional custom buttons beyond the automated 'Create/Submit' action?"**

---

## 🎨 5. UI & Technical Implementation Requirements

To maintain professional, premium aesthetic standards (no basic-looking UI):
*   **Grid Layouts:** Grids must be fully responsive, styled matching the pre-existing grid look and feel, and equipped with **pagination, search/filter fields, and auto-refresh capability**.
*   **Custom Styling:** Never write inline styling or ad-hoc classes. Utilize existing CSS classes. The main stylesheet for brand overrides is `distribution/css/custom.css`.
*   **Date Fields:** All calendar/date inputs must include a calendar icon trigger and bind seamlessly to the client-side date picker.
*   **Micro-Animations & Visual Cues:** Leverage smooth state transitions and hover actions defined in `custom.css` to keep the user interface feeling modern and responsive.
